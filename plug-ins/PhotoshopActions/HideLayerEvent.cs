// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// HideLayerEvent.cs
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
  public class HideLayerEvent : ActionEvent
  {
    string _name;
    Layer _layer;

    public HideLayerEvent(HideEvent srcEvent) : base(srcEvent)
    {
      _layer = SelectedLayer;
    }

    public HideLayerEvent(HideEvent srcEvent, string name) : base(srcEvent)
    {
      _name = name;
    }

    public HideLayerEvent(HideEvent srcEvent, PropertyType property) : 
      base(srcEvent)
    {
      if (property.Key == "Bckg")
	{
	  _name = "Background";
	}
      else
	{
	  Console.WriteLine("HideLayerEvent: " + property.Key);
	  _name = "fixme!";
	}
    }

    public override string EventForDisplay
    {
      get 
	{
	  return base.EventForDisplay + 
	    ((_name == null) ? " current layer" : " layer \"" + _name + "\"");
	}
    }

    protected override IEnumerable ListParameters()
    {
      yield break;
    }

    override public bool Execute()
    {
      if (_layer == null)
	{
	  _layer = ActiveImage.Layers[_name];
	}
      _layer.Visible = false;

      return true;
    }
  }
}
