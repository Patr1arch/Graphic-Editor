using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Baranov_sArtist.Model.Tools;
using Baranov_sArtist.Model;
using Baranov_sArtist.Model.DifferentFigures;

namespace Baranov_sArtist
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            MyCanvas.Children.Add(NotArtist.FigureHost);
            ButtonsGenerations.Generation();
        }

        private void Invalidate()
        {
            NotArtist.FigureHost.Children.Clear();
            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            foreach (var figure in NotArtist.Figures)
            {
                figure.Draw(drawingContext);
            }
            drawingContext.Close();
            NotArtist.FigureHost.Children.Add(drawingVisual);
        }

        private void CanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            NotArtist.SelectedTool.MouseDown(e.GetPosition(MyCanvas));
            Invalidate();
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                NotArtist.SelectedTool.MouseMove(e.GetPosition(MyCanvas));
                if (NotArtist.SelectedTool == NotArtist.ToolsList["Hand"])
                {
                    ScrollViewerCanvas.ScrollToVerticalOffset(NotArtist.HandScrollY);
                    ScrollViewerCanvas.ScrollToHorizontalOffset(NotArtist.HandScrollX);
                }
                Invalidate();
            }
        }

        private void CanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            NotArtist.SelectedTool.MouseUp(e.GetPosition(MyCanvas));
            if (NotArtist.SelectedTool == NotArtist.ToolsList["ZoomTool"])
            {
                MyCanvas.LayoutTransform = new ScaleTransform(NotArtist.ScaleRateX, NotArtist.ScaleRateY);
                ScrollViewerCanvas.ScrollToVerticalOffset(NotArtist.DistanceToPointY * NotArtist.ScaleRateY);
                ScrollViewerCanvas.ScrollToHorizontalOffset(NotArtist.DistanceToPointX * NotArtist.ScaleRateX);
            }
            Invalidate();
        }

        public void ButtonChangeTool(object sender, RoutedEventArgs e)
        {
            NotArtist.SelectedTool = NotArtist.ToolsList[(sender as Button).Name.ToString()];
        }

        public void ButtonChangeColor(object sender, RoutedEventArgs e)
        {
            if (NotArtist.FirstPress == true)
            {
                NotArtist.SelectedColor = NotArtist.TransformColor[(sender as Button).Name.ToString()];
                buttonFirstColor.Background = (sender as Button).Background;
            }
            else
            {
                NotArtist.BrushNow = NotArtist.TransformColor[(sender as Button).Name.ToString()];
                buttonSecondColor.Background = (sender as Button).Background;
            }
        }

        private void FirstColor(object sender, RoutedEventArgs e)
        {
            NotArtist.FirstPress = true;
            NotArtist.SecondPress = false;
            buttonFirstColor.BorderThickness = new Thickness(5);
            buttonSecondColor.BorderThickness = new Thickness(0);
        }

        private void SecondColor(object sender, RoutedEventArgs e)
        {
            NotArtist.FirstPress = false;
            NotArtist.SecondPress = true;
            buttonSecondColor.BorderThickness = new Thickness(5);
            buttonFirstColor.BorderThickness = new Thickness(0);
        }

        private void MyCanvasLoaded(object sender, RoutedEventArgs e)
        {
            NotArtist.CanvasHeigth = MyCanvas.Height;
            NotArtist.CanvasWidth = MyCanvas.Width;
        }

        private void MyCanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            NotArtist.CanvasHeigth = MyCanvas.Height;
            NotArtist.CanvasWidth = MyCanvas.Width;
        }

        public void CleanMyCanvas(object sender, RoutedEventArgs e)
        {
            NotArtist.FigureHost.Children.Clear();
            NotArtist.Figures.Clear();
        }

        public void MinusZoomMyCanvas(object sender, RoutedEventArgs e)
        {
            MyCanvas.LayoutTransform = new ScaleTransform(1, 1);
        }
    }
}
