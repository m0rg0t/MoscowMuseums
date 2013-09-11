using M0rg0tRss.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace M0rg0tRss.Common
{
    public class MyPictureProvider : IImageSourceProvider
    {
        public ImageSource GetImageSource(object parameter)
        {
            return (parameter as MapItem).Image; //new BitmapImage(new Uri((parameter as MapItem).ImagePath, UriKind.Absolute));
        }
    }
}
