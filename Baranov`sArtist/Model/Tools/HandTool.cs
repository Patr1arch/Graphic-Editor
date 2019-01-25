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
    class HandTool : Tool
    {
        public override void MouseDown(Point aPoint)
        {
            NotArtist.Figures.Add(new Hand(aPoint));

            NotArtist.HandScrollX = aPoint.X;
            NotArtist.HandScrollY = aPoint.Y;
        }

        public override void MouseMove(Point aPoint)
        {
            NotArtist.HandScrollX = NotArtist.HandScrollX + NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].X - NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].X;
            NotArtist.HandScrollY = NotArtist.HandScrollY + NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[0].Y - NotArtist.Figures[NotArtist.Figures.Count - 1].coordinates[1].Y;
            NotArtist.Figures[NotArtist.Figures.Count - 1].ChangeCoord(aPoint);
        }

        public override void MouseUp(Point aPoint) => NotArtist.Figures.Remove(NotArtist.Figures[NotArtist.Figures.Count - 1]);
    }
}
