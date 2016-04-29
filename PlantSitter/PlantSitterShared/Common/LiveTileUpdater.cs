using NotificationsExtensions.Tiles;
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
        /// 更新动态磁贴
        /// </summary>
        /// <param name="imgUrl"></param>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="line3"></param>
        /// <param name="line4"></param>
        public static void UpdateTile(string imgUrl, string line1, string line2, string line3, string line4)
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

