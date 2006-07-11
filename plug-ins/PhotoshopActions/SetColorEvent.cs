// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// SetColorEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class SetColorEvent : ActionEvent
  {
    [Parameter("T")]
    ObjcParameter _objc;

    public SetColorEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
      _objc.Fill(this);
    }

    protected override void FillParameters(TreeStore store, TreeIter iter)
    {
    }
        
    protected RGB Color
    {
      get 
	{
	  return GetColor(_objc);
	}
    }
  }
}