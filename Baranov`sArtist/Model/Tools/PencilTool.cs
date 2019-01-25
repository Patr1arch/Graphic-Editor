using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baranov_sArtist.Model;
using Baranov_sArtist.Model.DifferentFigures;
using System.Windows;
using Baranov_sArtist.Model.Tools;

namespace Baranov_sArtist.Model.Tools
{
    class PencilTool : Tool
    {

        public override void MouseDown(Point aPoint) => NotArtist.Figures.Add(new Pencil(aPoint));

        public override void MouseMove(Point aPoint) => NotArtist.Figures[NotArtist.Figures.Count - 1].ChangeCoord(aPoint);
    } 
}
