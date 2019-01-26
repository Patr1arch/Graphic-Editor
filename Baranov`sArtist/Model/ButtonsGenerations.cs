using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baranov_sArtist.Model.Tools;
using Baranov_sArtist.Model;
using Baranov_sArtist.Model.DifferentFigures;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Baranov_sArtist.Model
{
    class ButtonsGenerations : MainWindow
    {
        public static void Generation()
        {
            foreach (KeyValuePair<string, Tool> keyValue in NotArtist.ToolsList)
            {
                string icon = "pack://application:,,,/Model/superIcons/" + keyValue.Key.ToString() + ".png";
                ImageBrush image = new ImageBrush();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(icon);
                bitmapImage.EndInit();
                image.ImageSource = bitmapImage;
                Button button = new Button();
                button.Name = keyValue.Key.ToString();
                button.Width = 24;
                button.Height = 24;
                button.Margin = new Thickness(4);
                button.HorizontalAlignment = HorizontalAlignment.Left;
                button.Click += new RoutedEventHandler(Instance.ButtonChangeTool);
                button.Background = image;
                Instance.toolbarPanel.Children.Add(button);
            }

            foreach (KeyValuePair<String, Brush> color in NotArtist.TransformColor)
            {
                Button button = new Button();
                button.Name = color.Key.ToString();
                button.Background = color.Value;
                button.Height = 24;
                button.Width = 24;
                button.Margin = new Thickness(2.5);
                button.Click += new RoutedEventHandler(Instance.ButtonChangeColor);
                Instance.colorbarPanel.Children.Add(button);
            }

            string iconClean = "pack://application:,,,/Model/superIcons/Clean.png";
            ImageBrush imageClean = new ImageBrush();
            BitmapImage bitmapImageClean = new BitmapImage();
            bitmapImageClean.BeginInit();
            bitmapImageClean.UriSource = new Uri(iconClean);
            bitmapImageClean.EndInit();
            imageClean.ImageSource = bitmapImageClean;
            Button buttonClean = new Button();
            buttonClean.Name = "Clean";
            buttonClean.Width = 24;
            buttonClean.Height = 24;
            buttonClean.Margin = new Thickness(4);
            buttonClean.HorizontalAlignment = HorizontalAlignment.Left;
            buttonClean.Click += new RoutedEventHandler(Instance.CleanMyCanvas);
            buttonClean.Background = imageClean;
            Instance.toolbarPanel.Children.Add(buttonClean);

            string iconMinusZoom = "pack://application:,,,/Model/superIcons/MinusZoom.png";
            ImageBrush imageMinusZoom = new ImageBrush();
            BitmapImage bitmapImageMinusZoom = new BitmapImage();
            bitmapImageMinusZoom.BeginInit();
            bitmapImageMinusZoom.UriSource = new Uri(iconMinusZoom);
            bitmapImageMinusZoom.EndInit();
            imageMinusZoom.ImageSource = bitmapImageMinusZoom;
            Button button_MinusZoom = new Button();
            button_MinusZoom.Name = "MinusZoom";
            button_MinusZoom.Width = 24;
            button_MinusZoom.Height = 24;
            button_MinusZoom.Margin = new Thickness(4);
            button_MinusZoom.HorizontalAlignment = HorizontalAlignment.Left;
            button_MinusZoom.Click += new RoutedEventHandler(Instance.MinusZoomMyCanvas);
            button_MinusZoom.Background = imageMinusZoom;
            Instance.toolbarPanel.Children.Add(button_MinusZoom);
        }

        public static void PropertyButtonGeneration()
        {
            Label Changelinecolor = new Label();
            Changelinecolor.Content = "Change line color";
            Changelinecolor.HorizontalAlignment = HorizontalAlignment.Center;
            Instance.PropToolBarPanel.Children.Add(Changelinecolor);
            ToolBar prop1 = new ToolBar();
            prop1.Name = "PropertiesToolBar1";
            prop1.Margin = new Thickness(2);

            foreach (KeyValuePair<String, Brush> color in NotArtist.TransformColor)
            {
                Button button = new Button();
                button.Tag = color.Key.ToString();
                button.Background = color.Value;
                button.Height = 18;
                button.Width = 18;
                button.Margin = new Thickness(2.5);
                button.Click += new RoutedEventHandler(Instance.ChangeStrokeColor);
                prop1.Items.Add(button);

            }

            Instance.PropToolBarPanel.Children.Add(prop1);

            bool HaveLineorPolyline = false;

            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select == true & (figure.Type == "Line" || figure.Type == "Pencil"))
                {
                    HaveLineorPolyline = true;
                }
            }

            if (HaveLineorPolyline == false)
            {
                Label Changefillcolor = new Label();
                Changefillcolor.Content = "Change fill color";
                Changefillcolor.HorizontalAlignment = HorizontalAlignment.Center;
                Instance.PropToolBarPanel.Children.Add(Changefillcolor);
                ToolBar prop2 = new ToolBar();
                prop2.Name = "PropertiesToolBar2";
                prop2.Margin = new Thickness(2);

                foreach (KeyValuePair<String, Brush> color in NotArtist.TransformColor)
                {
                    Button button = new Button();
                    button.Tag = color.Key.ToString();
                    button.Background = color.Value;
                    button.Height = 18;
                    button.Width = 18;
                    button.Margin = new Thickness(2.5);
                    button.Click += new RoutedEventHandler(Instance.ChangeBrushColor);
                    prop2.Items.Add(button);
                }

                Instance.PropToolBarPanel.Children.Add(prop2);
            }

            HaveLineorPolyline = false;

            Label Changedash = new Label();
            Changedash.Content = "Change dash";
            Changedash.HorizontalAlignment = HorizontalAlignment.Center;
            Instance.PropToolBarPanel.Children.Add(Changedash);

            foreach (KeyValuePair<String, DashStyle> dash in NotArtist.TransformDashProp)
            {
                Button button = new Button();
                button.Height = 23;
                button.Width = 60;
                button.Content = dash.Key.ToString();
                button.Margin = new Thickness(2);
                button.Click += new RoutedEventHandler(Instance.ChangeDash);
                Instance.PropToolBarPanel.Children.Add(button);
            }

            Label Removethefigures = new Label();
            Removethefigures.Content = "Remove the figures";
            Removethefigures.HorizontalAlignment = HorizontalAlignment.Center;
            Instance.PropToolBarPanel.Children.Add(Removethefigures);

            Button ClearSelectedFigure = new Button();
            ClearSelectedFigure.Height = 23;
            ClearSelectedFigure.Width = 60;
            ClearSelectedFigure.Content = "Delete";
            ClearSelectedFigure.Click += new RoutedEventHandler(Instance.ClearSelectedFigure);
            ClearSelectedFigure.Margin = new Thickness(2);
            Instance.PropToolBarPanel.Children.Add(ClearSelectedFigure);

            Label Movethefigures = new Label();
            Movethefigures.Content = "Move the figures";
            Movethefigures.HorizontalAlignment = HorizontalAlignment.Center;
            Instance.PropToolBarPanel.Children.Add(Movethefigures);

            Button HandForSelectedFigure = new Button();
            HandForSelectedFigure.Height = 23;
            HandForSelectedFigure.Width = 60;
            HandForSelectedFigure.Content = "Hand";
            HandForSelectedFigure.Click += new RoutedEventHandler(Instance.HandForSelectedFigure);
            HandForSelectedFigure.Margin = new Thickness(2);
            Instance.PropToolBarPanel.Children.Add(HandForSelectedFigure);

            Label ZIndex = new Label();
            ZIndex.Content = "Z-Index";
            ZIndex.HorizontalAlignment = HorizontalAlignment.Center;
            Instance.PropToolBarPanel.Children.Add(ZIndex);

            Button IncreaseZIndex = new Button();
            IncreaseZIndex.Height = 23;
            IncreaseZIndex.Width = 60;
            IncreaseZIndex.Content = "IncZindex";
            IncreaseZIndex.Click += new RoutedEventHandler(Instance.IncreaseZIndex);
            IncreaseZIndex.Margin = new Thickness(2);
            Instance.PropToolBarPanel.Children.Add(IncreaseZIndex);

            Button DecZIndex = new Button();
            DecZIndex.Height = 23;
            DecZIndex.Width = 60;
            DecZIndex.Content = "DecZIndex";
            DecZIndex.Click += new RoutedEventHandler(Instance.DecZIndex);
            DecZIndex.Margin = new Thickness(2);
            Instance.PropToolBarPanel.Children.Add(DecZIndex);


            bool HaveOnlyEllipse = true;
            double RoundX = 0;
            double RoundY = 0;
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Type == "RoundRect" & figure.Select == true)
                {
                    RoundX = figure.RoundX;
                    RoundY = figure.RoundY;
                    break;
                }
            }

            foreach (Figure figure in NotArtist.Figures)
            {
                if ((figure.Type != "RoundRect" & figure.Select) || ((figure.RoundX != RoundX || figure.RoundY != RoundY) & figure.Select == true))
                {
                    HaveOnlyEllipse = false;
                    break;
                }
            }

            if (HaveOnlyEllipse)
            {
                Label ChengeRoundX = new Label();
                ChengeRoundX.Content = "Change RoundX";
                ChengeRoundX.HorizontalAlignment = HorizontalAlignment.Center;
                Instance.PropToolBarPanel.Children.Add(ChengeRoundX);
                Slider sldRoundX = new Slider();
                sldRoundX.Maximum = 40;
                sldRoundX.Minimum = 5;
                sldRoundX.Height = 26;
                sldRoundX.Width = 79;
                sldRoundX.Value = RoundX;
                sldRoundX.ValueChanged += new RoutedPropertyChangedEventHandler<double>(Instance.changeRoundX);
                sldRoundX.PreviewMouseUp += new MouseButtonEventHandler(Instance.SldMouseUp);
                Instance.PropToolBarPanel.Children.Add(sldRoundX);
                Label ChengeRoundY = new Label();
                ChengeRoundY.Content = "Change RoundY";
                ChengeRoundY.HorizontalAlignment = HorizontalAlignment.Center;
                Instance.PropToolBarPanel.Children.Add(ChengeRoundY);
                Slider sldRoundY = new Slider();
                sldRoundY.Maximum = 40;
                sldRoundY.Minimum = 5;
                sldRoundY.Height = 26;
                sldRoundY.Width = 79;
                sldRoundY.Value = RoundY;
                sldRoundY.ValueChanged += new RoutedPropertyChangedEventHandler<double>(Instance.changeRoundY);
                sldRoundY.PreviewMouseUp += new MouseButtonEventHandler(Instance.SldMouseUp);
                Instance.PropToolBarPanel.Children.Add(sldRoundY);
            }
            HaveOnlyEllipse = true;
        }

        public static void RowThicknessButton(double i)
        {
            Label ChangeRowThikness = new Label();
            ChangeRowThikness.Content = "Change row thikness";
            ChangeRowThikness.HorizontalAlignment = HorizontalAlignment.Center;
            Instance.PropToolBarPanel.Children.Add(ChangeRowThikness);
            Slider sld = new Slider();
            sld.Height = 26;
            sld.Width = 79;
            sld.Minimum = 1;
            sld.Maximum = 20;
            sld.Value = i;
            sld.ValueChanged += new RoutedPropertyChangedEventHandler<double>(Instance.RowThicnessChange);
            sld.PreviewMouseUp += new MouseButtonEventHandler(Instance.SldMouseUp);
            Instance.PropToolBarPanel.Children.Add(sld);
        }
    }
}
