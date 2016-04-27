using PlantSitterShared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitter.Common
{
    public class PlantSitterSettings : AppSettings
    {
        /// <summary>
        /// 上传的频率，单位是毫秒
        /// </summary>
        public double UploadFequency
        {
            get
            {
                return ReadSettings(nameof(UploadFequency), TimeSpan.FromMinutes(5).TotalMilliseconds);
            }
            set
            {
                SaveSettings(nameof(UploadFequency), value);
                RaisePropertyChanged(() => UploadFequency);
            }
        }
    }
}
