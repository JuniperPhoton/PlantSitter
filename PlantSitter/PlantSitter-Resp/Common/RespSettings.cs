using PlantSitterShared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitter_Resp.Common
{
    public class RespSettings : AppSettings
    {
        /// <summary>
        /// 上传的频率，分几个档：
        /// 每15，30，60分钟，1.5小时，2小时
        /// </summary>
        public int UploadFequency
        {
            get
            {
                return ReadSettings(nameof(UploadFequency), 0);
            }
            set
            {
                SaveSettings(nameof(UploadFequency), value);
                RaisePropertyChanged(() => UploadFequency);
            }
        }
    }
}
