// The Slice Tool plug-in
// Copyright (C) 2004-2006 Maurits Rijk  m.rijk@chello.nl
//
// RolloverEntry.cs
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

using Gtk;

namespace Gimp.SliceTool
{
  public class RolloverEntry : FileEntry
  {
    public RolloverEntry(GimpTable table, string label, uint row) : 
      base("Select Image", "", false, true)
    {
      CheckButton button = new CheckButton(label);
      button.Clicked += new EventHandler(OnClick);
      table.Attach(button, 0, 1, row, row + 1);

      Sensitive = false;
      table.Attach(this, 1, 2, row, row + 1);
    }

    void OnClick(object o, EventArgs args)
    {
      bool active = (o as CheckButton).Active;
      Sensitive = active;

      if (!active)
	{
	  FileName = "";
	}
    }
  }
}
