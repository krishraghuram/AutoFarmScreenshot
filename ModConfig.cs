using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFarmScreenshot
{
    class ModConfig
    {
        public float ScaleNumber { get; set; }
        public string ScreenshotFormat { get; set; }
        public ModConfig()
        {
            this.ScaleNumber = 0.25f;
            this.ScreenshotFormat = "{PlayerName}_{Season}_{Day}_{Year}";
        }
    }
}
