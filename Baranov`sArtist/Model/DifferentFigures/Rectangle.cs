using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Baranov_sArtist.Model.DifferentFigures
{
    class Rectangle : Figure
    {
        public Rectangle(Point aPoint)
        {
            color = NotArtist.SelectedColor;

            brushColor = NotArtist.BrushNow;

            coordinates = new List<Point> { aPoint, aPoint };
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var diagonal = Point.Subtract(coordinates[0], coordinates[1]);
            drawingContext.DrawRectangle(brushColor, new Pen(color, 4), new Rect(coordinates[1], diagonal));
        }

        public override void ChangeCoord(Point aPoint)
        {
            coordinates[1] = aPoint;
        }
    }
}
