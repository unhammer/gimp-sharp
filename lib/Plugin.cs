// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
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
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

using Gdk;
using Gtk;

namespace Gimp
{
  abstract public class Plugin
  {
    protected string _name;
    bool _usesDrawable = false;
    bool _usesImage = false;
    
    protected Image _image;
    protected Drawable _drawable;

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
    static string[] myArgs = new String[6];

    public Plugin(string[] args)
    {
      _info.Init = new InitProc(Init);
      _info.Quit = new QuitProc(Quit);
      _info.Query = new QueryProc(Query);
      _info.Run = new RunProc(Run);

      string[] progargs = new string[args.Length + 1];
      progargs[0] = "gimp-sharp";
      args.CopyTo (progargs, 1);

      gimp_main(ref _info, progargs.Length, progargs);
    }

    public string Name
      {
      get {return _name;}
    }

    protected virtual void Init() 
    {
    }

    protected virtual void Quit() 
    {
    }

    protected void InstallProcedure(string name, string blurb, string help, 
				    string author, string copyright, 
				    string date, string menu_path, 
				    string image_types,
				    GimpParamDef[] _params, 
				    GimpParamDef[] return_vals)
    {
      _name = name;
      gimp_install_procedure(name, blurb, help, author, copyright, date, 
			     menu_path, image_types, PDBProcType.PLUGIN, 
			     _params.Length, return_vals.Length, _params, 
			     return_vals);
    }

    protected void InstallProcedure(string name, string blurb, string help, 
				    string author, string copyright, 
				    string date, string menu_path, 
				    string image_types,
				    ParamDefList in_params)
    {
      _name = name;
      GetRequiredParameters();
      
      GimpParamDef[] args = in_params.GetGimpParamDef(_usesImage, 
						      _usesDrawable);
      
      gimp_install_procedure(name, blurb, help, author, copyright, date, 
			     menu_path, image_types, PDBProcType.PLUGIN, 
			     args.Length, 0, args, null);
    }

    protected void InstallProcedure(string name, string blurb, string help, 
				    string author, string copyright, 
				    string date, string menu_path, 
				    string image_types)
    {
      InstallProcedure(name, blurb, help, author, copyright, date,
		       menu_path, image_types, new ParamDefList());
    }
    
    void GetRequiredParameters()
    {
      foreach (MethodInfo method in 
	       GetType().GetMethods(BindingFlags.DeclaredOnly |
				    BindingFlags.NonPublic | 
				    BindingFlags.Instance))
	{
	  if (method.Name.Equals("DoSomething"))
	    {
	      foreach (ParameterInfo parameter in method.GetParameters())
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

    protected bool MenuRegister(string procedure_name, string menu_path)
    {
      return gimp_plugin_menu_register(procedure_name, menu_path);
    }

    protected bool MenuRegister(string menu_path)
    {
      return MenuRegister(_name, menu_path);
    }

    protected Pixbuf LoadImage(string filename)
    {
      return new Pixbuf(Assembly.GetCallingAssembly(), filename);
    }

    protected void IconRegister(string filename)
    {
      Pixbuf pixbuf = new Pixbuf(Assembly.GetCallingAssembly(), filename);
      
      Pixdata data = new Pixdata();
      data.FromPixbuf(pixbuf, false);
      gimp_plugin_icon_register(_name, IconType.INLINE_PIXBUF, 
				data.Serialize());
    }

    abstract protected void Query();

    virtual protected void Run(string name, GimpParam[] inParam,
			       out GimpParam[] outParam)
    {
      RunMode run_mode = (RunMode) inParam[0].data.d_int32;
      if (_usesImage)
	{
	  _image = new Image(inParam[1].data.d_image);
	}
      if (_usesDrawable)
	{
	  _drawable = new Drawable(inParam[2].data.d_drawable);
	}
      
      if (run_mode == RunMode.INTERACTIVE)
	{
	  GetData();
	  if (CreateDialog())
	    {
	  SetData();
	    }
	}
      else if (run_mode == RunMode.NONINTERACTIVE)
	{
	  Console.WriteLine("RunMode.NONINTERACTIVE not implemented yet!");
	}
      else if (run_mode == RunMode.WITH_LAST_VALS)
	{
	  GetData();
	  CallDoSomething();
	}
      
      if (_usesDrawable)
	{
	  _drawable.Detach();
	}
      
      outParam = new GimpParam[1];
      
      outParam[0].type = PDBArgType.STATUS;
      outParam[0].data.d_status = PDBStatusType.SUCCESS;
    }
    
    virtual protected bool CreateDialog() {return true;}

    GimpParam[] _origParam;

    public void Run(string name, int n_params, IntPtr paramPtr,
		    ref int n_return_vals, out IntPtr return_vals)
    {
      _name = name;
      
      GetRequiredParameters();
      
      // Get parameters
      _origParam = new GimpParam[n_params];
      for (int i = 0; i < n_params; i++)
	{
	_origParam[i] = (GimpParam) Marshal.PtrToStructure(paramPtr,
							   typeof(GimpParam));
	// Console.WriteLine(_origParam[i].type);
	paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(_origParam[i]));
	}

      GimpParam[] my_return_vals;
      
      Run(name, _origParam, out my_return_vals);
      
      n_return_vals = my_return_vals.Length;
      return_vals = Marshal.AllocCoTaskMem(n_return_vals * 
					   Marshal.SizeOf(_origParam[0]));

      paramPtr = return_vals;
      for (int i = 0; i < n_return_vals; i++)
	{
	Marshal.StructureToPtr(my_return_vals[i], paramPtr, false);
	paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(my_return_vals[i]));
	}
    }

    protected void SetData()
    {
      PersistentStorage storage = new PersistentStorage(this);
      storage.SetData();
    }

    protected void GetData()
    {
      PersistentStorage storage = new PersistentStorage(this);
      storage.GetData();
    }

    GimpDialog _dialog;

    protected Dialog DialogNew( string title,
				string role,
				IntPtr parent,
				Gtk.DialogFlags flags,
				GimpHelpFunc help_func,
				string help_id,
				string button1, Gtk.ResponseType action1,
				string button2, Gtk.ResponseType action2,
				string button3, Gtk.ResponseType action3)
    {
      _dialog = new GimpDialog(title, role, parent, flags, 
			       help_func, help_id, 
			       button1, action1,
			       button2, action2, 
			       button3, action3);
      return _dialog;
    }

    protected Dialog DialogNew( string title,
				string role,
				IntPtr parent,
				Gtk.DialogFlags flags,
				GimpHelpFunc help_func,
				string help_id)
    {
#if false
      return DialogNew (title, role, parent, flags, help_func, help_id,
			Stock.Help, ResponseType.Help,
			Stock.Cancel, ResponseType.Cancel,
			Stock.Ok, ResponseType.Ok);
#else
      _dialog = new GimpDialog(title, role, parent, flags, 
			       help_func, help_id, 
                               GimpStock.Reset, (ResponseType) 1,
			       Stock.Cancel, ResponseType.Cancel,
			       Stock.Ok, ResponseType.Ok);
      return _dialog;
#endif
    }

    protected GimpDialog Dialog
    {
      get {return _dialog;}
    }

    void CallDoSomething()
    {
      int m_start = Environment.TickCount;

      GetRequiredParameters();
      
      if (_usesDrawable && _usesImage)
	{
	DoSomething(_image, _drawable);
	}
      else if (_usesDrawable)
	{
	DoSomething(_drawable);
	}
      else if (_usesImage)
	{
	DoSomething(_image);
	}
      else
	{
	DoSomething();
	}

      int m_time= Environment.TickCount - m_start;
      if (m_time<0)
	m_time &=0x7FFFFFFF;
      Console.WriteLine("Processing time {0:0.0} seconds.", 
			(double)m_time / 1000 );
    }
    
    virtual protected void Reset() {}
    virtual protected void DoSomething() {}
    virtual protected void DoSomething(Drawable drawable) {}
    virtual protected void DoSomething(Image image) {}
    virtual protected void DoSomething(Image image, Drawable drawable) {}

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
	ResponseType type = _dialog.Run();
	if (type == ResponseType.Ok)
	  {
	  GetParameters();
	  CallDoSomething();
	  return true;
	  } 
	else if (type == ResponseType.Cancel || type == ResponseType.Close)
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
	}
    }

    protected void RunProcedure(string name, params object[] list)
    {
      RunProcedure(name, _image, _drawable, list);
    }

    protected void RunProcedure(string name, Image image, Drawable drawable,
				params object[] list)
    {
      string blurb;
      string help;
      string author;
      string copyright;
      string date;
      PDBProcType proc_type;
      int num_args;
      int num_values;
      IntPtr argsPtr;
      GimpParamDef[] return_vals;
    
      if (gimp_procedural_db_proc_info(name, 
				       out blurb, 
				       out help,
				       out author,
				       out copyright,
				       out date,
				       out proc_type,
				       out num_args,
				       out num_values,
				       out argsPtr,
				       out return_vals))
	{	
	// Get parameter types
	GimpParamDef[] paramDef = new GimpParamDef[num_args];
	GimpParam[] _params = new GimpParam[num_args];

	// First 3 parameters are default

	_params[0].type = PDBArgType.INT32;
	_params[0].data.d_int32 = (Int32) RunMode.NONINTERACTIVE;	
	_params[1].type = PDBArgType.IMAGE;
	_params[1].data.d_image = image.ID;
	_params[2].type = PDBArgType.DRAWABLE;
	_params[2].data.d_drawable = drawable.ID;

	int i;

	for (i = 0; i < num_args; i++)
	  {
	  paramDef[i] = (GimpParamDef) Marshal.PtrToStructure(
	    argsPtr, typeof(GimpParamDef));
	  argsPtr = (IntPtr)((int)argsPtr + Marshal.SizeOf(paramDef[i]));
	  }

	i = 3;
	foreach (object obj in list)
	  {
	  switch (paramDef[i].type)
	    {
	    case PDBArgType.INT32:
	      _params[i].type = PDBArgType.INT32;
	      _params[i].data.d_int32 = (Int32) obj;
	      break;
	    default:
	      Console.WriteLine("Implement this!");
	      break;
	    }
	  i++;
	  }

	int n_return_vals;
	gimp_run_procedure2(name, out n_return_vals, num_args, _params);
	}
      else
	{
	Console.WriteLine(name + " not found!");
	}
    }

    [DllImport("libgimpui-2.0-0.dll")]
    public static extern void gimp_ui_init(string prog_name, bool preview);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_plugin_menu_register(string procedure_name,
							string menu_path);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_plugin_icon_register(string procedure_name,
							IconType icon_type, 
							byte[] icon_data);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern void gimp_install_procedure(
      string name,
      string blurb,
      string help,
      string author,
      string copyright,
      string date,
      string menu_path,
      string image_types,
      PDBProcType	   type,
      int    n_params,
      int    n_return_vals,
      GimpParamDef[] _params,
      GimpParamDef[] return_vals);

    [DllImport("libgimp-2.0-0.dll")]
    public static extern IntPtr gimp_run_procedure2(string name,
						    out int n_return_vals,
						    int n_params,
						    GimpParam[] _params);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_procedural_db_proc_info (
      string procedure,
      out string blurb,
      out string help,
      out string author,
      out string copyright,
      out string date,
      out PDBProcType proc_type,
      out int num_args,
      out int num_values,
      out IntPtr args,
      out GimpParamDef[] return_vals);

    [DllImport("libgimp-2.0-0.dll")]
    public static extern int gimp_main(ref GimpPlugInInfo info, 
				       int argc, string[] args);
  }
  }
