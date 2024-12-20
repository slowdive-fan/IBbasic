﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    public class Settings
    {
        public bool debugMode = false;
        public bool com_showGrid = false;
        public bool map_showGrid = false;
        public bool playSoundFx = false;
        public bool com_use11x11 = true;
        public bool map_use11x11 = false;
        public bool showClock = true;
        public bool showFullParty = false;
        public bool showMiniMap = false;
        public bool showTogglePanel = true; //both
        public bool showLogPanel = false; //both 
        public bool showPortraitPanel = true;
        public bool showHP = false;
        public bool showSP = false;
        public bool showMO = false;
        public int combatAnimationSpeed = 100;
        
        public Settings()
        {

        }
    }

    public class IBbasicPrefernces
    {
        public bool GoogleAnalyticsOn = true;
        public string UserID = "none";
        public string UserName = "";
        public int CustomWindowSizeWidth = 1024;
        public int CustomWindowSizeHeight = 768;

        public IBbasicPrefernces()
        {

        }

        public void GenerateUniqueUserID()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            UserID = new String(stringChars);
        }
    }
}
