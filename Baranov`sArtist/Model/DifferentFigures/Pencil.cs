using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Baranov_sArtist.Model.DifferentFigures
{
    class Pencil : Figure
    {

        public Pencil (Point aPoint)
        {
            coordinates = new List<Point> { aPoint, aPoint };
            color = NotArtist.SelectedColor;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            for (int i = 0; i < coordinates.Count - 1; i++)
            {
                drawingContext.DrawLine(new Pen(color, 4), coordinates[i], coordinates[i + 1]);
            }
        }

        public override void ChangeCoord(Point aPoint)
        {
            coordinates.Add(aPoint);
        }
    }
}
