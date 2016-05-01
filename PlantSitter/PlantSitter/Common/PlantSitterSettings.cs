using PlantSitterShared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace PlantSitter.Common
{
    public class PlantSitterSettings : AppSettings
    {
        public bool EnableLiveTile
        {
           get
            {
                return ReadSettings(nameof(EnableLiveTile), true);
            }
            set
            {
                SaveSettings(nameof(EnableLiveTile), value);
                RaisePropertyChanged(() => EnableLiveTile);
                if(!value)
                {
                    TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                }
            }
        }

        public bool EnableNotification
        {
            get
            {
                return ReadSettings(nameof(EnableNotification), true);
            }
            set
            {
                SaveSettings(nameof(EnableNotification), value);
                RaisePropertyChanged(() => EnableLiveTile);
            }
        }

        public bool EnableNightMode
        {
            get
            {
                return ReadSettings(nameof(EnableNightMode), false);
            }
            set
            {
                SaveSettings(nameof(EnableNightMode), value);
                RaisePropertyChanged(() => EnableLiveTile);
            }
        }
    }
}
