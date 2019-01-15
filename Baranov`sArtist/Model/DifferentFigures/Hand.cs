using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Baranov_sArtist.Model.DifferentFigures
{
    class Hand : Figure
    {
        public Hand(Point aPoint)
        {
            coordinates = new List<Point> { aPoint, aPoint };
        }

        public override void Draw(DrawingContext drawingContext) => drawingContext.DrawLine(null, coordinates[0], coordinates[1]);

        public override void ChangeCoord(Point aPoint) => coordinates[1] = aPoint;
    }
}
