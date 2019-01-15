using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Baranov_sArtist.Model.Tools;
using Baranov_sArtist.Model;
using Baranov_sArtist.Model.DifferentFigures;

namespace Baranov_sArtist.Model.DifferentFigures
{ 
    class Line : Figure
    {
        public Line(Point aPoint)
        {
            coordinates = new List<Point> { aPoint, aPoint };
            color = NotArtist.SelectedColor;
        }

        public override void Draw(DrawingContext drawingContext) => drawingContext.DrawLine(new Pen(color, 4), coordinates[0], coordinates[1]);


        public override void ChangeCoord(Point aPoint) => coordinates[1] = aPoint;
    }
}
