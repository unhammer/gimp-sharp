// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// AddGuideEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class AddGuideEvent : ActionEvent
  {
    string _orientation;
    string _units;
    double _position;

    public AddGuideEvent(ActionEvent srcEvent) : base(srcEvent) 
    {
    }
    
    override public ActionEvent Parse(ActionParser parser)
    {
      _position = parser.ReadDouble("Pstn", out _units);

      parser.ParseToken("Ornt");
      parser.ParseFourByteString("enum");
      parser.ParseToken("Ornt");

      _orientation = parser.ReadTokenOrString();

      return this;
    }

    override public bool Execute()
    {
      if (Image == null)
	{
	  Console.WriteLine("Please open image first");
	  return false;
	}

      if (_units != "#Prc")
	{
	  Console.WriteLine("Unit type {0} not supported", _units);
	  throw new GimpSharpException();
	}

      if (_orientation == "Vrtc")
	{
	  int position = (int) (_position * Image.Width / 100);
	  new VerticalGuide(Image, position);
	}
      else if (_orientation == "Hrzn")
	{
	  int position = (int) (_position * Image.Height / 100);
	  new HorizontalGuide(Image, position);
	}
      else
	{
	  throw new GimpSharpException();
	}

      return true;
    }
  }
}