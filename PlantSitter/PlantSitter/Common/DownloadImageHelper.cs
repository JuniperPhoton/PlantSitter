using PlantSitterShared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace PlantSitter.Common
{
    public static class DownloadImageHelper
    {
        public async static Task DownloadImage(Plant plant)
        {
            if (string.IsNullOrEmpty(plant.ImageUrl))
            {
                return;
            }
            var file =await App.CacheUtilInstance.DownloadImageAsync(plant.ImageUrl);
            using (var fs = await file.OpenReadAsync())
            {
                var bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(fs);
                plant.ImgBitmap = bitmap;
                plant.CacheFilePath = file.Path;
            }
        }
    }
}
