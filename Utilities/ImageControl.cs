using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Utilities
{
    public static class ImageControl
    {
        public static string GetNextImage(ObservableCollection<string> images, string currentImage)
        {
            int index = images.IndexOf(currentImage);
            if (index == images.Count - 1)
                currentImage = images[0];
            else
                currentImage = images[index + 1];
            return currentImage;
        }

        public static string GetPreviousImage(ObservableCollection<string> images, string currentImage)
        {
            int index = images.IndexOf(currentImage);
            if (index == 0)
                currentImage = images.Last();
            else
                currentImage = images[index - 1];
            return currentImage;
        }
    }

}
