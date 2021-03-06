// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// RemoveFunc.cs
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

using Gdk;

namespace Gimp.SliceTool
{
  public class RemoveFunc : MouseFunc
  {
    static readonly Cursor _cursor;

    public RemoveFunc(SliceData sliceData, Preview preview) : 
      base(sliceData, preview)
    {
    }

    static RemoveFunc()
    {
      _cursor = LoadCursor("cursor-eraser.png");
    }

    override protected void OnPress(IntCoordinate c)
    {
      var slice = SliceData.MayRemove(c);
      if (slice != null)
	{
	  SliceData.Remove(slice);
	  Redraw();
	}
    }

    override public Cursor GetCursor(IntCoordinate c)
    {
      return (SliceData.MayRemove(c) == null) ? base.GetCursor(c) : _cursor;
    }
  }
}
