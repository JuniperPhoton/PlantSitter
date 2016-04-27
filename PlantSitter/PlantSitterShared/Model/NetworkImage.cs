using GalaSoft.MvvmLight;
using JP.API;
using JP.Utils.Data;
using JP.Utils.Data.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Media.Imaging;

namespace PlantSitterShared.Model
{
    public class NetworkImage : ViewModelBase
    {
        public string ThumbnailUrl { get; set; }
        public string Url { get; set; }

        private BitmapImage _imgSource;
        public BitmapImage ImgSource
        {
            get
            {
                return _imgSource;
            }
            set
            {
                if (_imgSource != value)
                {
                    _imgSource = value;
                    RaisePropertyChanged(() => ImgSource);
                }
            }
        }

        public NetworkImage()
        {

        }

        public async Task DownloadThumbImageAsync()
        {
            if(ThumbnailUrl.IsNotNullOrEmpty())
            {
                var stream =await APIHelper.GetIRandomAccessStreamFromUrlAsync(ThumbnailUrl);
                var bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(stream);
                this.ImgSource = bitmap;
            }
        }

        public static List<NetworkImage> ParseToList(string respJson)
        {
            List<NetworkImage> list = new List<NetworkImage>();
            JsonObject jsonObj;
            if (!JsonObject.TryParse(respJson, out jsonObj))
            {
                return null;
            }
            var valueArray = JsonParser.GetJsonArrayFromJsonObj(jsonObj, "value");
            foreach(var item in valueArray)
            {
                var thumbUrl = JsonParser.GetStringFromJsonObj(item, "thumbnailUrl");
                var url = JsonParser.GetStringFromJsonObj(item, "contentUrl");
                var networkImage = new NetworkImage();
                networkImage.ThumbnailUrl = thumbUrl;
                networkImage.Url = url;
                list.Add(networkImage);
            }
            return list;
        }
    }
}
