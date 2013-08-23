// The PhotoshopActions plug-in
// Copyright (C) 2006-2013 Maurits Rijk
//
// Abbreviations.cs
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

using System.Collections;
using System.Collections.Generic;

namespace Gimp.PhotoshopActions
{
  static class Abbreviations
  {
    static readonly Dictionary<string, string> _map = 
      new Dictionary<string, string>() {
	{"AbTl", "airbrush"},
	{"ActP", "Actual Pixels"},
	{"AdBt", "bottom edges"},
	{"AdCH", "horizontal centers"},
	{"AdCV", "vertical centers"},
	{"Adjs", "Adjustment"},
	{"AdLf", "left edges"},
	{"AdNs", "Add Noise"},
	{"addNoise", "Add Noise"},
	{"addToSelectionContinuous", "Add Continuous"},
	{"AdRg", "right edges"},
	{"AdTp", "top edges"},
	{"Al", "all"},
	{"AmbB", "Ambience"},
	{"AmbC", "Ambient Color"},
	{"AmMn", "Amplitude min"},
	{"AmMx", "Amplitude max"},
	{"Ang1", "Channel 1"},
	{"Ang2", "Channel 2"},
	{"Ang3", "Channel 3"},
	{"Ang4", "Channel 4"},
	{"annotText", "text"},
	{"annotType", "Type"},
	{"AntA", "Anti-alias"},
	{"antialiasGloss", "Anti-alias Gloss"},
	{"Amnt", "Amount"},
	{"Angl", "angle"},
	{"Anno", "none"},
	{"Aply", "Apply"},
	{"autoBlackWhite", "Auto Black & White"},
	{"autoNeutrals", "Auto Neutrals"},
	{"Bcbc", "bicubic"},
	{"BckC", "background color"},
	{"Bckg", "background"},
	{"BlcB", "black body"},
	{"Blck", "black"},
	{"BlcL", "Foreground Level"},
	{"Bl", "blue"},
	{"Blks", "Blocks"},
	{"BlrM", "Method"},
	{"BlrQ", "Quality"},
	{"Blst", "Blast"},
	{"BmpA", "Height"},
	{"bottomRightPixelColor", "bottom right pixel color"},
	{"BrbW", "brush wide blurry"},
	{"Brgh", "Brightness"},
	{"BrsD", "Brush Detail"},
	{"BrSm", "brush simple"},
	{"BrsS", "Brush Size"},
	{"BtmM", "bitmap mode"},
	{"Btom", "Bottom"},
	{"bvlS", "Style"},
	{"bvlT", "Technique"},
	{"canvasSize", "Canvas Size"},
	{"CBrn", "color burn"},
	{"CDdg", "color dodge"},
	{"ChAm", "Charcoal Thickness"},
	{"ChlA", "Chalk Area"},
	{"Chnl", "Channel"},
	{"ChrA", "Charcoal Area"},
	{"Ckmt", "Spread"},
	{"Clcl", "calculation"},
	{"Clds", "Clouds"},
	{"Cler", "Clear"},
	{"Clr", "color"},
	{"ClrB", "Color Balance"},
	{"Clrs", "Colors"},
	{"Clrz", "Colorize"},
	{"ClSz", "Cell Size"},
	{"Cmps", "composite channel"},
	{"CMYM", "CMYK color mode"},
	{"CnsP", "Constrain Proportions"},
	{"Cntc", "Contract"},
	{"Cntg", "Contiguous"},
	{"Cntn", "Continue"},
	{"Cntr", "Contrast"},
	{"Cnvr", "Convert"},
	{"CnvM", "Convert Mode"},
	{"convertMode", "Convert Mode"},
	{"copy", "Copy"},
	{"copyToLayer", "Layer via Copy"},
	{"copyMerged", "Copy Merged"},
	{"CrnH", "Current History State"},
	{"CrcB", "Crack brightness"},
	{"CrcD", "Crack depth"},
	{"CrcS", "Crack spacing"},
	{"CrnL", "Current Light"},
	{"CrrL", "current layer"},
	{"CrsD", "Coarse Dots"},
	{"Crtl", "Interpolation"},
	{"Crvs", "Curves"},
	{"CstS", "custom stops"},
	{"Cyn", "cyan"},
	{"Dcmn", "document"},
	{"DfnP", "Define Pattern"},
	{"Dfnt", "Definition"},
	{"Dfrn", "difference"},
	{"Dlt", "Delete"},
	{"Dmtr", "diameter"},
	{"Dnst", "Density"},
	{"Dplc", "Duplicate"},
	{"Dpth", "Depth"},
	{"DrcB", "Direction Balance"},
	{"Drct", "Direction"},
	{"Drft", "draft"},
	{"DrkI", "Dark Intensity"},
	{"Drkn", "darken"},
	{"DrSh", "Drop Shadow"},
	{"Dslc", "Deselect"},
	{"DspF", "Displace File"},
	{"DspM", "Displacement Map"},
	{"Dstn", "Distance"},
	{"Dstr", "Distribution"},
	{"Dstt", "Desaturate"},
	{"Dthr", "Dither"},
	{"Dtl", "Detail"},
	{"Edg", "Edge"},
	{"EdgB", "Edge Brightness"},
	{"EdgI", "Edge Intensity"},
	{"EdgT", "Edge Thickness"},
	{"EdgW", "Edge Width"},
	{"ElmO", "Odd Fields"},
	{"Elps", "Ellipse"},
	{"enab", "Enabled"},
	{"Expn", "Expand"},
	{"Exps", "Exposure"},
	{"ExtD", "Depth"},
	{"ExtF", "Solid Front Faces"},
	{"ExtM", "Mask Incomplete Blocks"},
	{"ExtR", "Random"},
	{"ExtS", "Size"},
	{"ExtT", "Type"},
	{"fill", "Fill"},
	{"Fl", "fill"},
	{"FllD", "full document"},
	{"FltI", "Flatten Image"},
	{"flattenImage", "Flatten Image"},
	{"Fltt", "Flatten"},
	{"FncK", "Function Key"},
	{"FndE", "Find Edges"},
	{"FnDt", "Fine Dots"},
	{"FrgC", "foreground color"},
	{"FrmW", "Frame Width"},
	{"Frnt", "front"},
	{"Frst", "first"},
	{"FTcs", "Center"},
	{"FtOn", "Fit On Screen"},
	{"Fzns", "Fuzziness"},
	{"GblR", "Gaussian Blur"},
	{"Gd", "good"},
	{"Glos", "Gloss"},
	{"GlwE", "glowing edges"},
	{"Gmm", "Gamma"},
	{"GsnB", "Gaussian Blur"},
	{"gaussianBlur", "Gaussian Blur"},
	{"GrdF", "Form"},
	{"Grdn", "Gradient"},
	{"gradientClassEvent", "Gradient"},
	{"Grn", "green"},
	{"GrnE", "Enlarged"},
	{"GrnH", "Horizontal"},
	{"GrnR", "Regular"},
	{"Grns", "Graininess"},
	{"Grnt", "Grain Type"},
	{"GrnV", "Vertical"},
	{"groupEvent", "Group"},
	{"GrpL", "Create Clipping Mask"},
	{"GrSf", "Soft"},
	{"GrtW", "Grout Width"},
	{"Gry", "gray"},
	{"Grys", "grayscale mode"},
	{"Gsn", "gaussian"},
	{"GudG", "Guides & Grid & Slices Preferences"},
	{"H", "Hue"},
	{"Hd", "Hide"},
	{"HghS", "Highlight strength"},
	{"Hght", "Height"},
	{"hglO", "Highlight Opacity"},
	{"HlSz", "Size"},
	{"HrdL", "hard light"},
	{"Hrdn", "hardness"},
	{"Hrzn", "Horizontal"},
	{"HrzO", "Horizontal Only"},
	{"HrzS", "Horizontal Scale"},
	{"HSBC", "HSB color"},
	{"HStr", "Hue/Saturation"},
	{"hueSaturation", "Hue/Saturation"},
	{"imageSize", "Image Size"},
	{"ImgB", "Image Balance"},
	{"IndC", "indexed color mode"},
	{"InrB", "inner bevel"},
	{"Insd", "inside"},
	{"IntC", "Create"},
	{"interfaceIconFrameDimmed", "Intersect"},
	{"IntE", "Eliminate"},
	{"Intn", "Intensity"},
	{"Intr", "Interpolation"},
	{"Invr", "Invert Source"},
	{"Invs", "Inverse"},
	{"InvT", "Invert Texture"},
	{"IrGl", "Inner Glow"},
	{"IrSh", "Inner Shadow"},
	{"lagl", "Local Angle"},
	{"Lald", "Local Altitude"},
	{"layerConceals", "layer knocks out"},
	{"LbCM", "Lab color mode"},
	{"Lctn", "Location"},
	{"LDBL", "Bottom Left"},
	{"LDBt", "Bottom"},
	{"LDTp", "Top"},
	{"LDTL", "Top Left"},
	{"LDTR", "Top Right"},
	{"Left", "Left"},
	{"Lefx", "layer styles"},
	{"Lft", "Left"},
	{"LgDr", "Light/Dark Balance"},
	{"LghD", "Light Direction"},
	{"LghE", "Lighting Effects"},
	{"lightingEffects", "Lighting Effects"},
	{"LghG", "Lighten Grout"},
	{"LghI", "Light Intensity"},
	{"Lghn", "lighten"},
	{"LghS", "Light Source"},
	{"Lght", "lightness"},
	{"LghT", "Light Type"},
	{"linearLight", "linear light"},
	{"Lmns", "luminosity"},
	{"Ln", "line"}, 
	{"LngL", "long lines"},
	{"LngS", "long strokes"},
	{"Lnr", "linear"},
	{"Lns", "Lens"},
	{"LnsF", "Lens Flare"},
	{"LPBt", "light position bottom"},
	{"LPLf", "light position left"},
	{"LPRg", "light position right"},
	{"LPTL", "Top Left"},
	{"LPTp", "light position top"},
	{"Lrg", "Large"},
	{"Lst", "last"},
	{"Lvl", "Level"},
	{"Lvls", "Levels"},
	{"LvlB", "Level-based"},
	{"Lwr", "lower"},
	{"Lyr", "layer"},
	{"Md", "Mode"},
	{"Mdm", "medium"},
	{"MdmS", "Medium Strokes"},
	{"Mdpn", "Midpoint"},
	{"mergeVisible", "Merge Visible"},
	{"Mgnt", "magenta"},
	{"Mk", "Make"},
	{"Mltp", "multiply"},
	{"Mnch", "Monochromatic"},
	{"mosaicPlug", "Mosaic Tiles"},
	{"MtnB", "Motion Blur"},
	{"motionBlur", "Motion Blur"},
	{"MrgL", "Merge Layers"},
	{"mergeLayers", "Merge Layers"},
	{"MrgV", "Merge Visible"},
	{"Msc", "Mosaic"},
	{"Msge", "Message"},
	{"Msk", "mask"},
	{"Mthd", "Method"},
	{"Mtrl", "Material"},
	{"Mztn", "Mezzotint"},
	{"MztT", "Type"},
	{"N", "no"},
	{"Nkn", "35mm Prime"},
	{"Nkn1", "105mm Prime"},
	{"Nm", "Name"},
	{"NmbG", "Number of Generators"},
	{"NmbR", "Ridges"},
	{"Nose", "Noise"},
	{"Nrml", "normal"},
	{"Nw", "New"},
	{"Nxt", "next"},
	{"Opn", "Open"},
	{"Ofst", "Offset"},
	{"Opct", "Opacity"},
	{"Orng", "orange"},
	{"Ornt", "Orientation"},
	{"Otsd", "outside"},
	{"Ovrl", "overlay"},
	{"past", "Paste"},
	{"Pht3", "Photoshop"},
	{"Phtc", "Photocopy"},
	{"PhtP", "Photoshop PDF"},
	{"Plgn", "polygon"},
	{"Plr", "Polar Coordinates"},
	{"PlrR", "Polar to Rectangular"},
	{"Pncl", "Pencil Width"},
	{"PndR", "Pond Ripples"},
	{"PntD", "paint daubs"},
	{"PprB", "Paper Brightness"},
	{"PrnS", "Print Size"},
	{"PrsL", "Preserve Luminosity"},
	{"Prvs", "previous"},
	{"PrsT", "Preserve Transparency"},
	{"PstI", "Paste Into"},
	{"Pstn", "Position"},
	{"Pstr", "Posterization"},
	{"Ptrn", "pattern"},
	{"Pyrm", "pyramids"},
	{"Qcsa", "center"},
	{"Rctn", "rectangle"},
	{"RctP", "Rectangular to Polar"},
	{"Rd", "red"},
	{"Rdl", "radial"},
	{"Rds", "Radius"},
	{"Rght", "Right"},
	{"Rlf", "Relief"},
	{"Rlg", "Relief"},
	{"Rndm", "random"},
	{"Rndn", "roundness"},
	{"RGBC", "RGB color"},
	{"RGBM", "RGB color mode"},
	{"Rltv", "relative"},
	{"RndS", "Random Seed"},
	{"RplM", "Ripple Magnitude"},
	{"RplS", "Ripple Size"},
	{"Rpt", "repeat"},
	{"RptE", "repeat edge pixels"},
	{"Rset", "Reset"},
	{"Rslt", "Resolution"},
	{"Rtcl", "reticulation"},
	{"Rtte", "Rotate"},
	{"RvlS", "reveal selection"},
	{"Rvrs", "Reverse"},
	{"SBME", "Edge Only"},
	{"SBMN", "smart blur mode normal"},
	{"SBQH", "smart blur quality high"},
	{"SBQL", "smart blur quality low"},
	{"SBQM", "smart blur quality medium"},
	{"Sbtr", "Subtract Selection"},
	{"scaleStyles", "Scale Styles"},
	{"Scl", "Scale"},
	{"SclH", "Scale horizontal"},
	{"Scln", "Scaling"},
	{"SclV", "Scale vertical"},
	{"ScrL", "Line"},
	{"Scrn", "screen"},
	{"ScrT", "Pattern Type"},
	{"SDir", "Stroke Direction"},
	{"SDLD", "Left Diagonal"},
	{"SDRD", "Right Diagonal"},
	{"SdwC", "Shadow Color"},
	{"SwdM", "Shadow Mode"},
	{"sdwO", "Shadow Opacity"},
	{"setd", "Set"},
	{"SfBL", "smooth"},
	{"SftL", "soft light"},
	{"Sftn", "Softness"},
	{"ShdI", "Shadow Intensity"},
	{"Shdw", "shadow"},
	{"showNone", "Show None"},
	{"Shrp", "Sharpness"},
	{"Shw", "Show"},
	{"slct", "Select"},
	{"Slrz", "Solarize"},
	{"Sml", "small"},
	{"Smth", "Smoothness"},
	{"SnpS", "snapshot"},
	{"Spcn", "spacing"},
	{"SphM", "Mode"},
	{"Spng", "Sponge"},
	{"SprR", "Spray Radius"},
	{"SqrS", "Square Size"},
	{"StDt", "Stroke Detail"},
	{"Stgr", "Stagger"},
	{"StrD", "Stroke Detail"},
	{"Strg", "Strength"},
	{"Strk", "Stroke"},
	{"StrL", "Stroke Length"},
	{"StrP", "Stroke Pressure"},
	{"StrS", "Stroke Size"},
	{"Strt", "saturation"},
	{"StrW", "Stroke Width"},
	{"Svng", "Saving"},
	{"Sz", "Size"},
	{"trimBasedOn", "Based On"},
	{"T", "To"},
	{"TgGr", "Toggle Grid"},
	{"TglO", "Toggle Others"},
	{"TglR", "Toggle Rulers"},
	{"TgSn", "Toggle Snap To Grid"},
	{"Thrs", "Threshold"},
	{"Tlrn", "Tolerance"},
	{"TlSz", "Tile Size"},
	{"Top", "Top"},
	{"topLeftPixelColor", "top left pixel color"},
	{"Trgt", "current"},
	{"Trnf", "Transform"},
	{"Trns", "transparent"},
	{"Trsp", "transparency"},
	{"TxBl", "Blocks"},
	{"TxBr", "Brick"},
	{"TxCa", "Canvas"},
	{"TxFr", "Frosted"},
	{"TxSt", "Sandstone"},
	{"TxtC", "Texture Coverage"},
	{"Txtr", "Texture"},
	{"TxtT", "Texture Type"},
	{"TxTL", "Tiny Lens"},
	{"uglg", "Use Global Light"},
	{"UndA", "Undefined Area"},
	{"Unfr", "uniform"},
	{"useShape", "Apply Shape"},
	{"useTexture", "Apply Texture"},
	{"Usng", "Using"},
	{"Vct0", "Vector 0"},
	{"Vct1", "Vector 1"},
	{"Vrtc", "Vertical"},
	{"VrtS", "Vertical Scale"},
	{"Wdth", "Width"},
	{"WhHi", "White is High"},
	{"Wht", "white"},
	{"WhtL", "Background Level"},
	{"WLMn", "Wave length min"},
	{"WLMx", "Wave length max"},
	{"Wnd", "Wind"},
	{"WndM", "Method"},
	{"Wrp", "wrap"},
	{"WrpA", "wrap around"},
	{"WvSn", "wave sine"},
	{"WvSq", "wave square"},
	{"Wvtp", "Wave Type"},
	{"Xclu", "exclusion"},
	{"Yllw", "yellow"},
	{"Zm", "zoom"},
	{"ZZTy", "Style"},
      };

    public static string Get(string key)
    {
      if (key == null) return "key == null!!";
      return GetFullString(key) ?? key;
    }

    static string GetFullString(string key)
    {
      string fullString;
      _map.TryGetValue(key, out fullString);
      return fullString;
    }

    public static string GetUppercased(string key)
    {
      string s = Get(key);
      return char.ToUpper(s[0]) + s.Substring(1);
    }
  }
}

