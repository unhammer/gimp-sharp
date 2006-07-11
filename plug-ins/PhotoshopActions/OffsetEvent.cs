// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// OffsetEvent.cs
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

using Gtk;

namespace Gimp.PhotoshopActions
{
  public class OffsetEvent : ActionEvent
  {
    [Parameter("Hrzn")]
    int _horizontal;
    [Parameter("Vrtc")]
    int _vertical;
    [Parameter("Fl")]
    EnumParameter _fillMode;

    protected override void FillParameters(TreeStore store, TreeIter iter)
    {
      store.AppendValues(iter, "Horizontal: " + _horizontal);
      store.AppendValues(iter, "Vertical: " + _vertical);
      string fillMode;
      switch(_fillMode.Value)
	{
	case "Wrp":
	  fillMode = "wrap";
	  break;
	default: 
	  fillMode = _fillMode.Value;
	  break;
	}
      store.AppendValues(iter, "Fill: " + fillMode);
    }

    override public bool Execute()
    {
      bool wrapAround = (_fillMode.Value == "Wrp");
      ActiveDrawable.Offset(wrapAround, OffsetType.Background, 
			    _horizontal, _vertical);
      return true;
    }
  }
}