using JP.API;
using JP.Utils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PlantSitterShared.Cache
{
    public class CacheUtil
    {
        /// <summary>
        /// 第一项是URL 地址，第二是文件的本地 Path
        /// </summary>
        private Dictionary<string, string> CachedFiles { get; set; }

        public CacheUtil()
        {

        }

        public async Task<StorageFile> DownloadImageAsync(string url)
        {
            if(CachedFiles.ContainsKey(url))
            {
                return await StorageFile.GetFileFromPathAsync(CachedFiles[url]);
            }
            var stream=await APIHelper.GetIRandomAccessStreamFromUrlAsync(url);
            var newFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("plant.jpg", CreationCollisionOption.GenerateUniqueName);
            var bytes = new byte[stream.AsStream().Length];
            await stream.AsStream().ReadAsync(bytes, 0, (int)stream.AsStream().Length);
            await FileIO.WriteBytesAsync(newFile, bytes);

            CachedFiles.Add(url, newFile.Path);
            await SaveAsync();
            return newFile;
        }

        public async Task SaveAsync()
        {
            var tempFolder = ApplicationData.Current.LocalCacheFolder;
            await SerializerHelper.SerializerToJson<Dictionary<string, string>>(CachedFiles, "CachedFiles", tempFolder, true);
        }

        public async Task LoadAsync()
        {
            var tempFolder = ApplicationData.Current.LocalCacheFolder;
            this.CachedFiles = await SerializerHelper.DeserializeFromJsonByFileName<Dictionary<string, string>>("CachedFiles", tempFolder);
            if(this.CachedFiles==null)
            {
                CachedFiles = new Dictionary<string, string>();
            }
        }
    }
}
