// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// Plugin.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

using Mono.Unix;

using Gdk;
using Gtk;

namespace Gimp
{
  abstract public class Plugin
  {
    public string Name {get; set;}

    bool _usesDrawable = false;
    bool _usesImage = false;
    
    protected Image _image;
    protected Drawable _drawable;

    VariableSet _variables;

    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void InitProc();
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void QuitProc();
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void QueryProc();
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void RunProc(string name, int n_params, 
				 IntPtr param,
				 ref int n_return_vals, 
				 out IntPtr return_vals);
    
    [StructLayout(LayoutKind.Sequential)]
    public struct GimpPlugInInfo
    {
      public InitProc Init;
      public QuitProc Quit;
      public QueryProc Query;
      public RunProc Run;
    }
    
    static GimpPlugInInfo _info = new GimpPlugInInfo();

    public string[] Args {get; set;}

    protected static void GimpMain<T>(string[] args) where T : Plugin, new()
    {
      var plugin = new T();
      Catalog.Init(typeof(T).Name, Gimp.LocaleDirectory);

      _info.Init = plugin.HasMethod("Init") ? new InitProc(plugin.Init) : null;
      _info.Quit = plugin.HasMethod("Quit") ? new QuitProc(plugin.Quit) : null;
      _info.Query = new QueryProc(plugin.Query);
      _info.Run = new RunProc(plugin.Run);

      var progargs = new string[args.Length + 1];
      progargs[0] = "gimp-sharp";
      args.CopyTo(progargs, 1);

      gimp_main(ref _info, progargs.Length, progargs);
    }

    static protected string _(string s)
    {
      return Catalog.GetString(s);
    }

    protected virtual void Init() 
    {
    }

    protected virtual void Quit() 
    {
    }

    protected virtual IEnumerable<Procedure> ListProcedures() {yield break;}

    protected virtual Procedure GetProcedure() {return null;}

    bool HasMethod(string methodName)
    {
      return Array.Exists(GetMethods(), m => m.Name == methodName);
    }

    MethodInfo[] GetMethods()
    {
      return GetType().GetMethods(BindingFlags.DeclaredOnly |
				  BindingFlags.Public | 
				  BindingFlags.NonPublic | 
				  BindingFlags.Instance);
    }

    void GetRequiredParameters()
    {
      foreach (MethodInfo method in GetMethods())
	{
	  if (method.Name == "Render")
	    {
	      foreach (var parameter in method.GetParameters())
		{
		  if (parameter.ParameterType == typeof(Drawable))
		    {
		      _usesDrawable = true;
		    }
		  if (parameter.ParameterType == typeof(Image))
		    {
		      _usesImage = true;
		    }
		}
	    }
	}
    }

    protected Pixbuf LoadImage(string filename)
    {
      return new Pixbuf(Assembly.GetCallingAssembly(), filename);
    }

    virtual protected void Query()
    {
      GetRequiredParameters();

      var procedures = GetSupportedProcedures();
      procedures.Install(_usesImage, _usesDrawable);     
    }

    ProcedureSet GetSupportedProcedures()
    {
      var procedures = new ProcedureSet(ListProcedures());
      if (procedures.Count == 0)
	{
	  var procedure = GetProcedure();
	  if (procedure != null)
	    {
	      procedures.Add(procedure);
	    }
	}
      return procedures;
    }

    virtual protected void Run(string name, ParamDefList inParam,
			       out ParamDefList outParam)
    {
      RunMode run_mode = (RunMode) inParam[0].Value;
      if (_usesImage)
	{
	  _image = (Image) inParam[1].Value;
	}
      if (_usesDrawable)
	{
	  _drawable = (Drawable) inParam[2].Value;
	}
      
      if (run_mode == RunMode.Interactive)
	{
	  GetData();
	  _dialog = CreateDialog();
	  if (_dialog == null)
	    {
	      SetData();
	    }
	  else
	    {
	      _dialog.ShowAll();
	      if (DialogRun())
		{
		  SetData();
		}
	    }
	}
      else if (run_mode == RunMode.Noninteractive)
	{
	  if (ValidateParameters(inParam))
	    {
	      CallRender();
	    }
	}
      else if (run_mode == RunMode.WithLastVals)
	{
	  GetData();
	  CallRender();
	}
      
      if (_usesDrawable && _drawable != null)
	{
	  _drawable.Detach();
	}

      outParam = new ParamDefList(true);
      outParam.Add(new ParamDef(PDBStatusType.Success, typeof(PDBStatusType)));
    }

    virtual protected bool ValidateParameters(ParamDefList inParam)
    {
      foreach (var attribute in new SaveAttributeSet(GetType()))
	{
	  string name = attribute.Name;
	  if (name != null)
	    {
	      object value = inParam.GetValue(name);
	      if (value != null)
		{
		  attribute.Field.SetValue(this, value);
		}
	    }
	}
      return true;
    }

    virtual protected GimpDialog CreateDialog() 
    {
      CallRender();
      return null;
    }

    public void Run(string name, int n_params, IntPtr paramPtr,
		    ref int n_return_vals, out IntPtr return_vals)
    {
      Name = name;

      GetRequiredParameters();

      var procedures = GetSupportedProcedures();

      var procedure = procedures[name];
      var inParam = procedure.InParams;
      inParam.Marshall(paramPtr, n_params);

      ParamDefList outParam;
      Run(name, inParam, out outParam);
      outParam.Marshall(out return_vals, out n_return_vals);
    }

    protected void SetData()
    {
      var storage = new PersistentStorage(this);
      storage.SetData();
    }

    protected void GetData()
    {
      var storage = new PersistentStorage(this);
      storage.GetData();
    }

    GimpDialog _dialog;

    virtual protected GimpDialog DialogNew(string title,
				       string role,
				       IntPtr parent,
				       Gtk.DialogFlags flags,
				       GimpHelpFunc help_func,
				       string help_id,
				       string button1, ResponseType action1,
				       string button2, ResponseType action2,
				       string button3, ResponseType action3)
    {
      _dialog = new GimpDialog(title, role, parent, flags, 
			       help_func, help_id, 
			       button1, action1,
			       button2, action2, 
			       button3, action3);

      _dialog.SetTransient();
      return _dialog;
    }

    virtual protected GimpDialog DialogNew( string title,
					string role,
					IntPtr parent,
					Gtk.DialogFlags flags,
					GimpHelpFunc help_func,
					string help_id)
    {
      return DialogNew(title, role, parent, flags,
		       help_func, help_id,
		       GimpStock.Reset, (ResponseType) 1,
		       Stock.Cancel, ResponseType.Cancel,
		       Stock.Ok, ResponseType.Ok);
    }

    protected GimpDialog Dialog
    {
      get {return _dialog;}
    }

    void CallRender()
    {
      var stopWatch = new Stopwatch();
      stopWatch.Start();

      GetRequiredParameters();

      if (_usesDrawable && _usesImage)
	{
	  Render(_image, _drawable);
	}
      else if (_usesDrawable)
	{
	  Render(_drawable);
	}
      else if (_usesImage)
	{
	  Render(_image);
	}
      else
	{
	  Render();
	}

      // TODO: maybe a check could/should be added here if we need to flush
      // the displays at all.
      Display.DisplaysFlush();

      stopWatch.Stop();
      var ts = stopWatch.Elapsed;
      Console.WriteLine(String.Format("Processing time: {0:00}:{1:00}:{2:00}.{3:00}",
				      ts.Hours, ts.Minutes, ts.Seconds, 
				      ts.Milliseconds / 10));
    }
    
    virtual protected void Reset() 
    {
      // _variables.Reset();
    }

    virtual protected void Render() {}
    virtual protected void Render(Drawable drawable) {}
    virtual protected void Render(Image image) {}
    virtual protected void Render(Image image, Drawable drawable) {}

    virtual protected void GetParameters() {}

    virtual protected void DialogRun(ResponseType type) {}

    virtual protected bool OnClose()
    {
      return true;
    }

    protected bool DialogRun()
    {
      while (true)
	{
	  var type = _dialog.Run();
	  if (type == ResponseType.Ok)
	    {
	      GetParameters();
	      CallRender();
	      return true;
	    } 
	  else if (type == ResponseType.Cancel || type == ResponseType.Close
		   || type == ResponseType.DeleteEvent)
	    {
	      if (OnClose())
		{
		  return false;
		}
	    }
	  else if (type == ResponseType.Help)
	    {
	      Console.WriteLine("Show help here!");
	    }
	  else if (type == (ResponseType) 1)
	    {
	      Reset();
	    }
	  else if (type >=0)		// User defined response
	    {
	      DialogRun(type);
	      Console.WriteLine("Type: " + type);
	    }
	  else
	    {
	      Console.WriteLine("Unknown type: " + type);
	    }
	}
    }

    protected void RunProcedure(string name, params object[] list)
    {
      RunProcedure(name, _image, _drawable, list);
    }

    protected void RunProcedure(string name, Image image, Drawable drawable,
				params object[] list)
    {
      var procedure = new Procedure(name);
      procedure.Run(image, drawable, list);
    }

    [DllImport("libgimpui-2.0-0.dll")]
    public static extern void gimp_ui_init(string prog_name, bool preview);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern int gimp_main(ref GimpPlugInInfo info, 
				       int argc, string[] args);
  }
}
