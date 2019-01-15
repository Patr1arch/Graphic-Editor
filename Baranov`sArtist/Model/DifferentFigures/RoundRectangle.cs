using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Baranov_sArtist.Model.DifferentFigures
{
    class RoundRectangle : Figure
    {
        public RoundRectangle(Point aPoint)
        {
            color = NotArtist.SelectedColor;
            brushColor = NotArtist.BrushNow;
            coordinates = new List<Point> { aPoint, aPoint };
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var diagonal = Point.Subtract(coordinates[0], coordinates[1]);
            drawingContext.DrawRoundedRectangle(brushColor, new Pen(color, 4), new Rect(coordinates[1], diagonal), (coordinates[1].X - coordinates[0].X) / 3.5, (coordinates[1].Y - coordinates[0].Y) / 3.5);
        }

        public override void ChangeCoord(Point aPoint) => coordinates[1] = aPoint;
    }
}
