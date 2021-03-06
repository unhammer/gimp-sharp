// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// RadialBlurEvent.cs
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
  public class RadialBlurEvent : ActionEvent
  {
    [Parameter("Amnt")]
    int _amount;
    [Parameter("BlrM")]
    EnumParameter _blurMode;
    [Parameter("BlrQ")]
    EnumParameter _blurQuality;

    protected override IEnumerable ListParameters()
    {
      yield return "Amount: " + _amount;
      yield return Format(_blurMode, "BlrM");
      yield return Format(_blurQuality, "BlrQ");
    }

    override public bool Execute()
    {
      RunProcedure("plug_in_mblur", 1, _amount, 0);

      return true;
    }
  }
}
