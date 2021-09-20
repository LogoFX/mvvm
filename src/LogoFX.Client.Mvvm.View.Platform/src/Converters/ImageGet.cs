using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LogoFX.Client.Mvvm.View.Converters
{
    internal static class ImageGet
    {
        public delegate object MakeRet(string src);
        public static MakeRet GetDelegateOnType(Type targetType)
        {
            MakeRet mk;
            if (targetType == typeof(ImageSource))
            {
                mk = s => new BitmapImage(new Uri(s,UriKind.RelativeOrAbsolute));
            }
            else if (targetType == typeof(Uri))
            {
                mk = s => new Uri(s, UriKind.RelativeOrAbsolute);
            }
            else /*if (targetType == null || targetType == typeof(string) || targetType == typeof(object))*/
            {
                mk = s => s;
            }
            return mk;
        }
    }
}
