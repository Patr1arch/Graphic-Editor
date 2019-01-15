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
                Instance.MyToolBarPanel.Children.Add(button);
            }

            foreach (KeyValuePair<String, SolidColorBrush> color in NotArtist.TransformColor)
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
            Instance.MyToolBarPanel.Children.Add(buttonClean);

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
            Instance.MyToolBarPanel.Children.Add(button_MinusZoom);
        }
    }
}
