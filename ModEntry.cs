using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace AutoFarmScreenshot
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private ModConfig Config;
        float scale = 0.25f;
        string screenshot_name = null;
        string screenshot_format = null;
        IReflectedMethod takeMapscreenshot = null;
        IReflectedMethod addMessage = null;
        int tryTimes = 0;
        int counter = 0;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // read config
            Config = Helper.ReadConfig<ModConfig>();
            scale = Config.ScaleNumber;
            screenshot_format = Config.ScreenshotFormat;
            // attach event
            Helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            Helper.Events.GameLoop.TimeChanged += GameLoop_TimeChanged;
            Helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
        }

        private void initValue()
        {
            tryTimes = 0;
            counter = 0;
            var SToday = SDate.Now();
        }

        private string Format_Screenshot_Name()
        {
            string _screenshot_name = screenshot_format;
            var screenshot_dict = new System.Collections.Generic.Dictionary<string, string> {
                { "{PlayerName}", Game1.player.name.ToString() },
                { "{Year}", SDate.Now().Year.ToString("00") },
                { "{Season}", SDate.Now().Season.ToString() },
                { "{Day}", SDate.Now().Day.ToString("00") },
                { "{Counter}", counter.ToString() },
                { "{TotalDays}", SDate.Now().DaysSinceStart.ToString("0000") },
                { "{FarmName}", Game1.player.farmName.ToString() }
            };
            foreach (System.Collections.Generic.KeyValuePair<string, string> format_item in screenshot_dict)
            {
                _screenshot_name = _screenshot_name.Replace(format_item.Key, format_item.Value);
            }
            return _screenshot_name;
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            initValue();
        }

        private void GameLoop_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            screenshot_name = Format_Screenshot_Name();
            string fullName = takeMapscreenshot.Invoke<string>(scale, screenshot_name, null);
            if (fullName != null)
            {
                // take screenshot
                counter++;
                addMessage.Invoke("Saved screenshot as '" + fullName + "'.", Color.Green);
            }
            else
            {
                if (tryTimes++ > 3)
                {
                    addMessage.Invoke("Failed taking screenshot over 3 times, won't try agian today!", Color.Red);
                }
                else
                    addMessage.Invoke("Failed taking screenshot!", Color.Red);
            }
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            takeMapscreenshot = Helper.Reflection.GetMethod(Game1.game1, "takeMapScreenshot");
            addMessage = Helper.Reflection.GetMethod(Game1.chatBox, "addMessage");
        }

    }
}
