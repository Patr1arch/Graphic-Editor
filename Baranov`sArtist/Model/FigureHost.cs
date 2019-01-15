using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Baranov_sArtist.Model
{
    class FigureHost : FrameworkElement
    {
        public VisualCollection Children;

        public FigureHost()
        {
            Children = new VisualCollection(this);
        }

        protected override Visual GetVisualChild(int index)
        {
            return Children[index];
        }

        protected override int VisualChildrenCount => Children.Count;
    }
}
