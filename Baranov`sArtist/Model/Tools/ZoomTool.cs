using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Baranov_sArtist.Model.Tools;
using Baranov_sArtist.Model;
using Baranov_sArtist.Model.DifferentFigures;

namespace Baranov_sArtist.Model.Tools
{
    class ZoomTool : Tool
    {
        public override void MouseDown(Point aPoint) => NotArtist.Figures.Add(new ZoomRectangle(aPoint));

        public override void MouseMove(Point aPoint) => NotArtist.Figures[NotArtist.Figures.Count - 1].ChangeCoord(aPoint);

        public override void MouseUp(Point aPoint)

        {
            if (NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].X > NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].X)
            {
                NotArtist.ScaleRateX = NotArtist.CanvasWidth / (NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].X - NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].X);
            }
            else
            {
                NotArtist.ScaleRateX = NotArtist.CanvasWidth / (NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].X - NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].X);
            }

            if (NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].Y > NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].Y)
            {
                NotArtist.ScaleRateY = NotArtist.CanvasHeigth / (NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].Y - NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].Y);
            }
            else
            {
                NotArtist.ScaleRateY = NotArtist.CanvasHeigth / (NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].Y - NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].Y);
            }

            if (NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].X > NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].X)
            {
                NotArtist.DistanceToPointX = NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].X;
            }
            else
            {
                NotArtist.DistanceToPointX = NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].X;
            }

            if (NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].Y > NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].Y)
            {
                NotArtist.DistanceToPointY = NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].Y;
            }
            else
            {
                NotArtist.DistanceToPointY = NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].Y;
            }
            NotArtist.Figures.Remove(NotArtist.Figures[NotArtist.Figures.Count - 1]);
        }
    }
}
