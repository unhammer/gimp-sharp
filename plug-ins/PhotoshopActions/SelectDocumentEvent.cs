// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// SelectDocumentEvent.cs
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
using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class SelectDocumentEvent : SelectEvent
  {
    int _offset;

    public SelectDocumentEvent(SelectEvent srcEvent, int offset) : 
      base(srcEvent)
    {
      _offset = offset;
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " document";}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "Offset: " + _offset;
    }

    override public bool Execute()
    {
      ImageList images = new ImageList();
      int index = images.GetIndex(ActiveImage);

      int newIndex = index + _offset;

      // Fix me: should we wrap around?

      if (newIndex > 0 && newIndex < images.Count)
	{
	  ActiveImage = images[newIndex];
	}

      return true;
    }
  }
}
