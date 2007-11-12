// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// TestStroke.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;

using NUnit.Framework;

namespace Gimp
{
  [TestFixture]
  public class TestStroke
  {
    int _width = 64;
    int _height = 128;
    Image _image;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb);

      Layer layer = new Layer(_image, "test", _width, _height,
			      ImageType.Rgb, 100, LayerModeEffects.Normal);
      _image.AddLayer(layer, 0);
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void NewFromPoints()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      CoordinateList<double> controlpoints = new CoordinateList<double>();
      controlpoints.Add(new Coordinate<double>(50, 50));
      controlpoints.Add(new Coordinate<double>(100, 100));
      controlpoints.Add(new Coordinate<double>(150, 150));
      Stroke stroke = vectors.NewFromPoints(VectorsStrokeType.Bezier,
					    controlpoints, false);
      Assert.AreEqual(1, vectors.Strokes.Count);
    }

    [Test]
    public void GetPoints()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      CoordinateList<double> controlpoints = new CoordinateList<double>();
      controlpoints.Add(new Coordinate<double>(50, 50));
      controlpoints.Add(new Coordinate<double>(100, 100));
      controlpoints.Add(new Coordinate<double>(150, 150));
      Stroke stroke = vectors.NewFromPoints(VectorsStrokeType.Bezier,
					    controlpoints, false);
      
      bool closed;
      CoordinateList<double> points = stroke.GetPoints(out closed);
      Assert.AreEqual(controlpoints, points);
      Assert.IsFalse(closed);
    }

    [Test]
    public void Close()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      CoordinateList<double> controlpoints = new CoordinateList<double>();
      controlpoints.Add(new Coordinate<double>(50, 50));
      controlpoints.Add(new Coordinate<double>(100, 100));
      controlpoints.Add(new Coordinate<double>(150, 150));
      Stroke stroke = vectors.NewFromPoints(VectorsStrokeType.Bezier,
					    controlpoints, false);
      stroke.Close();
      bool closed;
      CoordinateList<double> points = stroke.GetPoints(out closed);
      Assert.IsTrue(closed);
    }

    [Test]
    public void Scale()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      CoordinateList<double> controlpoints = new CoordinateList<double>();
      controlpoints.Add(new Coordinate<double>(50, 50));
      controlpoints.Add(new Coordinate<double>(100, 100));
      controlpoints.Add(new Coordinate<double>(150, 150));
      Stroke stroke = vectors.NewFromPoints(VectorsStrokeType.Bezier,
					    controlpoints, false);
      double precision = 0.001;
      stroke.Close();
      double oldLength = stroke.GetLength(precision);
      stroke.Scale(2, 2);
      double newLength = stroke.GetLength(precision);
      Assert.IsTrue(Math.Abs(2 * oldLength - newLength) < precision);
    }
  }
}
