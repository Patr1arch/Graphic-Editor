using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Runtime.Serialization;

namespace Baranov_sArtist.Model.DifferentFigures
{
    [Serializable]
    class Pencil : Figure
    {

        public Pencil() { }

        public Pencil(Point aPoint)
        {
            coordinates = new List<Point> { aPoint, aPoint };
            Color = NotArtist.SelectedColor;
            ColorString = NotArtist.ColorStringNow;
            PenThikness = NotArtist.ThicnessNow;
            Dash = NotArtist.DashNow;
            DashString = NotArtist.DashStringhNow;
            Pen = new Pen(Color, PenThikness) { DashStyle = Dash };
            Select = false;
            SelectRect = null;
            Type = "Pencil";
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var geometry = new StreamGeometry();
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(new Point(coordinates[0].X, coordinates[0].Y), true /* is filled */, false /* is closed */);
                ctx.PolyLineTo(coordinates, true /* is stroked */, false /* is smooth join */);
            }
            geometry.Freeze();
            drawingContext.DrawGeometry(Brushes.Transparent, Pen, geometry);
        }

        public override void ChangeCoord(Point aPoint)
        {
            coordinates.Add(aPoint);
        }

        public override void Selected()
        {
            if (!Select)
            {
                Point pForRect3 = coordinates[0];
                Point pForRect4 = new Point(0, 0);
                foreach (Point aPoint in coordinates)
                {
                    if (aPoint.X < pForRect3.X)
                    {
                        pForRect3.X = aPoint.X;
                    }

                    if (aPoint.Y < pForRect3.Y)
                    {
                        pForRect3.Y = aPoint.Y;
                    }

                    if (aPoint.X > pForRect4.X)
                    {
                        pForRect4.X = aPoint.X;
                    }

                    if (aPoint.Y > pForRect4.Y)
                    {
                        pForRect4.Y = aPoint.Y;
                    }
                }
                SelectRect = new ZoomRectangle(new Point(pForRect3.X - 7, pForRect3.Y - 7), new Point(pForRect4.X + 7, pForRect4.Y + 7));
                var drawingVisual = new DrawingVisual();
                var drawingContext = drawingVisual.RenderOpen();
                SelectRect.Draw(drawingContext);
                drawingContext.Close();
                NotArtist.FigureHost.Children.Add(drawingVisual);
                Select = true;
            }
        }

        public override void UnSelected()
        {
            if (Select)
            {
                Select = false;
                SelectRect = null;
            }
        }

        public override void ChangePen(Brush color, string str)
        {
            Pen = new Pen(color, PenThikness) { DashStyle = Dash };
            Color = color;
            ColorString = str;
        }

        public override void ChangePen(DashStyle dash, string str)
        {
            Pen = new Pen(Color, PenThikness) { DashStyle = dash };
            Dash = dash;
            DashString = str;
        }

        public override void ChangePen(double thikness)
        {
            Pen = new Pen(Color, thikness) { DashStyle = Dash };
            PenThikness = thikness;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Coordinates", coordinates);
            info.AddValue("PenThikness", PenThikness);
            info.AddValue("Color", ColorString);
            info.AddValue("Dash", DashString);
            info.AddValue("Type", Type);
        }

        public Pencil(SerializationInfo info, StreamingContext context)
        {
            coordinates = (List<Point>)info.GetValue("Coordinates", typeof(List<Point>));
            PenThikness = (double)info.GetValue("PenThikness", typeof(double));
            ColorString = (string)info.GetValue("Color", typeof(string));
            DashString = (string)info.GetValue("Dash", typeof(string));
            Type = (string)info.GetValue("Type", typeof(string));
            Color = NotArtist.TransformColor[ColorString];
            Dash = NotArtist.TransformDashProp[DashString];
            Pen = new Pen(Color, PenThikness) { DashStyle = Dash };
        }

        public override Figure Clone()
        {
            return new Pencil
            {
                BrushColor = this.BrushColor,
                BrushColorString = this.BrushColorString,
                Color = this.Color,
                ColorString = this.ColorString,
                coordinates = new List<Point>(coordinates),
                Dash = this.Dash,
                DashString = this.DashString,
                Pen = this.Pen,
                PenThikness = this.PenThikness,
                RoundX = this.RoundX,
                RoundY = this.RoundY,
                Select = this.Select,
                SelectRect = this.SelectRect,
                Type = this.Type
            };
        }

        public override string ConvertToSVG()
        {

            var svgCoordinates = string.Empty;

            for (var i = 0; i < coordinates.Count; i++)
            {
                svgCoordinates += Math.Round(coordinates[i].X).ToString() + "," + Math.Round(coordinates[i].Y).ToString() + " ";
            }
            return "<polyline points=\"" + svgCoordinates + "\" style=\"fill:none;stroke:" + ((SolidColorBrush)Pen.Brush).Color.ToString().Remove(1, 2) + ";stroke-width:" + Math.Round(Pen.Thickness).ToString() + "\"/>";
        }
    }
}
