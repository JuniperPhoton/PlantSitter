using NotificationsExtensions.Tiles;
using PlantSitterShared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace PlantSitterShared.Common
{
    public static class LiveTileUpdater
    {
        /// <summary>
        /// 更新动态磁铁
        /// </summary>
        public static void UpdateTile(UserPlan plan)
        {
            var lastRecord = plan.RecordData.First();
            var soilSumup = lastRecord.SoilMoisture.ToString() == "0" ? "干燥" : "湿润";
            var line1 = "土壤：" + soilSumup;
            var line2 = "环境温度：" + lastRecord.EnviTemp.ToString() + " ℃";
            var line3 = "环境湿度：" + lastRecord.EnviMoisture.ToString() + " %";
            var line4 = "光照：" + lastRecord.Light.ToString() + " Lux";

            UpdateTileiInner(plan.CurrentPlant.CacheFilePath, lastRecord.RecordTime.ToString(), line2, line3, line4);
        }

        /// <summary>
        /// 更新动态磁贴
        /// </summary>
        /// <param name="imgUrl"></param>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="line3"></param>
        /// <param name="line4"></param>
        private static void UpdateTileiInner(string imgUrl, string line1, string line2, string line3, string line4)
        {
            var tile = new TileBinding()
            {
                Branding = TileBranding.Name,

                Content = new TileBindingContentAdaptive()
                {
                    PeekImage = new TilePeekImage()
                    {
                        Source = new TileImageSource(imgUrl),
                    },
                    BackgroundImage = new TileBackgroundImage()
                    {
                        Source = new TileImageSource(imgUrl),
                        Overlay = 80,
                    },
                    Children =
                        {
                            new TileText()
                            {
                                Text = line1,
                            },

                            new TileText()
                            {
                                Text=line2,
                            },

                            new TileText
                            {
                                Text=line3,
                            },

                            new TileText
                            {
                                Text=line4,
                            },
                        }
                }
            };

            var tileContent = new TileContent();
            tileContent.Visual = new TileVisual();
            tileContent.Visual.Branding = TileBranding.Name;
            tileContent.Visual.TileMedium = tile;
            tileContent.Visual.TileWide = tile;
            tileContent.Visual.TileLarge = tile;

            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(tileContent.GetXml()));
        }
    }
}

