// The Splitter plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Dialog.cs
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

namespace Gimp.Splitter
{
  public class Dialog : GimpDialog
  {
    public Dialog(VariableSet variables) : 
      base("Splitter", variables)
    {
      var vbox = new VBox(false, 12) {BorderWidth = 12};
      VBox.PackStart(vbox, true, true, 0);

      var table = new GimpTable(4, 2)
	{ColumnSpacing = 6, RowSpacing = 6};
      vbox.PackStart(table, false, false, 0);

      var hbox = new HBox(false, 6);
      table.Attach(hbox, 0, 2, 0, 1);

      hbox.Add(new Label("f(x, y):"));
      hbox.Add(new GimpEntry(GetVariable<string>("formula")));
      hbox.Add(new Label("= 0"));

      table.Attach(CreateLayerFrame("Layer 1", "translate_1_x", "translate_1_y",
				    "rotate_1"), 0, 1, 1, 2);

      table.Attach(CreateLayerFrame("Layer 2", "translate_2_x", "translate_2_y", 
				    "rotate_2"), 1, 2, 1, 2);

      table.Attach(new GimpCheckButton(_("_Merge visible layers"), 
				       GetVariable<bool>("merge")), 0, 1, 3, 4);

      table.Attach(CreateAdvancedOptions(), 1, 2, 3, 4);

      var keep = new GimpComboBox(GetVariable<int>("keep_layer"),
				  new string[]{_("Both Layers"), 
					       _("Layer 1"), _("Layer 2")});
      table.AttachAligned(0, 5, _("Keep:"), 0.0, 0.5, keep, 1, true);
    }

    GimpFrame CreateLayerFrame(string frameLabel, string translateX,
			       string translateY, string rotate)
    {
      var frame = new GimpFrame(_(frameLabel));

      var table = new GimpTable(3, 3)
	{BorderWidth = 12, RowSpacing = 12, ColumnSpacing = 12};
      frame.Add(table);

      AddSpinButton(table, 0, int.MinValue, int.MaxValue, "Translate X:",
		    GetVariable<int>(translateX));
      AddSpinButton(table, 1, int.MinValue, int.MaxValue, "Translate Y:",
		    GetVariable<int>(translateY));
      AddSpinButton(table, 2, 0, 360, "Rotate:", GetVariable<int>(rotate));

      return frame;
    }

    void AddSpinButton(GimpTable table, int row, int min, int max, string label,
		       Variable<int> variable)
    {
      var spinner = new GimpSpinButton(min, max, 1, variable) {WidthChars = 4};
      table.AttachAligned(0, row, _(label), 0.0, 0.5, spinner, 1, true);      
    }

    Button CreateAdvancedOptions()
    {
      var advanced = new Button(_("Advanced Options..."));
      advanced.Clicked += delegate
	{
	  var seed = Variables.GetClone<UInt32>("seed");
	  var randomSeed = Variables.GetClone<bool>("random_seed");

	  var advancedDialog = new AdvancedDialog(seed, randomSeed);
	  advancedDialog.ShowAll();
	  if (advancedDialog.Run() == ResponseType.Ok)
	  {
	    seed.Commit();
	    randomSeed.Commit();
	  }
	  advancedDialog.Destroy();
	};
      return advanced;
    }
  }
}
