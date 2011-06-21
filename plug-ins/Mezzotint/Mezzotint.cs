// The Mezzotint plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Mezzotint.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;
using System.Collections.Generic;

using Gtk;

namespace Gimp.Mezzotint
{
  class Mezzotint : Plugin
  {
    DrawablePreview _preview;

    static void Main(string[] args)
    {
      GimpMain<Mezzotint>(args);
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return new Procedure("plug_in_mezzotint",
				 _("Mezzotint"),
				 _("Mezzotint"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2007-2011",
				 _("Mezzotint..."),
				 "RGB*")
	{
	  MenuPath = "<Image>/Filters/Noise",
	  IconFile = "Mezzotint.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Mezzotint", true);

      var dialog = DialogNew("Mezzotint", "Mezzotint", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "Mezzotint");

      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      _preview = new DrawablePreview(_drawable, false);
      _preview.Invalidated += UpdatePreview;
      vbox.PackStart(_preview, true, true, 0);

      var type = ComboBox.NewText();
      type.AppendText("Fine dots");
      type.AppendText("Medium dots");
      type.AppendText("Grainy dots");
      type.AppendText("Coarse dots");
      type.AppendText("Short lines");
      type.AppendText("Medium lines");
      type.AppendText("Long lines");
      type.AppendText("Short strokes");
      type.AppendText("Medium strokes");
      type.AppendText("Long strokes");
      type.Active = 0;

      vbox.PackStart(type, false, false, 0);

      return dialog;
    }

    void UpdatePreview(object sender, EventArgs e)
    {
      var rectangle = _preview.Bounds;

      int rowStride = rectangle.Width * 3;
      byte[] buffer = new byte[rectangle.Area * 3];	// Fix me!

      var srcPR = new PixelRgn(_drawable, rectangle, false, false);

      var iterator = new RegionIterator(srcPR);
      iterator.ForEach(src =>
	{
	  int x = src.X;
	  int y = src.Y;
	  var pixel = DoMezzotint(src);
	  
	  int index = (y - rectangle.Y1) * rowStride + (x - rectangle.X1) * 3;
	  pixel.CopyTo(buffer, index);
	});
      _preview.DrawBuffer(buffer, rowStride);
    }

    override protected void Render(Drawable drawable)
    {
      var iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress(_("Mezzotint"));
      iter.IterateSrcDest(pixel => DoMezzotint(pixel));
    }

    Pixel DoMezzotint(Pixel pixel)
    {
      pixel.Fill(val => (val > 127) ? 255 : 0);
      return pixel;
    }
  }
}
