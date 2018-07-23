using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IBbasic
{
    public class ScreenSplash
    {
        //private Module gv.mod;
        private GameView gv;        
        bool dialogOpen = false;        
        //private IbbButton btnUnzip = null;
        //private IbbButton btnZip = null;
        private IbbButton btnPlay = null;
        private IbbButton btnCreate = null;
        private IbbButton btnZip = null;
        private IbbButton btnUnZip = null;
        public IbbToggle tglGoogleAnalytics = null;
        private SKBitmap titleBitmap;
        //private List<string> moduleFolderList = new List<string>();
        //private List<string> moduleZipList = new List<string>();
        //private string quickStartGuideString = "";

        public ScreenSplash(Module m, GameView g)
        {
            gv = g;
            setControlsStart();
            titleBitmap = gv.cc.GetFromBitmapList("iblogo.png");            
        }

        public void setControlsStart()
        {
            int pH = (int)((float)gv.screenHeight / 100.0f);
            int wideX = (gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (int)(gv.ibbwidthL * gv.scaler / 2);

            if (btnPlay == null)
            {
                btnPlay = new IbbButton(gv, 1.2f);
                btnPlay.Text = "PLAY";
                btnPlay.Img = "btn_large";
                btnPlay.Glow = "btn_large_glow";
                btnPlay.X = wideX;
                btnPlay.Y = 1 * gv.uiSquareSize + pH * 2;
                btnPlay.Height = (int)(gv.ibbheight * gv.scaler);
                btnPlay.Width = (int)(gv.ibbwidthL * gv.scaler);
            }
            if (btnCreate == null)
            {
                btnCreate = new IbbButton(gv, 1.0f);
                btnCreate.Text = "CREATE";
                btnCreate.Img = "btn_large";
                btnCreate.Glow = "btn_large_glow";
                btnCreate.X = wideX;
                btnCreate.Y = 2 * gv.uiSquareSize + pH * 4;
                btnCreate.Height = (int)(gv.ibbheight * gv.scaler);
                btnCreate.Width = (int)(gv.ibbwidthL * gv.scaler);
            }
            if (btnZip == null)
            {
                btnZip = new IbbButton(gv, 1.0f);
                btnZip.Text = "ZIP A MODULE";
                btnZip.Img = "btn_large";
                btnZip.Glow = "btn_large_glow";
                btnZip.X = wideX;
                btnZip.Y = 3 * gv.uiSquareSize + pH * 6;
                btnZip.Height = (int)(gv.ibbheight * gv.scaler);
                btnZip.Width = (int)(gv.ibbwidthL * gv.scaler);
            }
            if (btnUnZip == null)
            {
                btnUnZip = new IbbButton(gv, 1.0f);
                btnUnZip.Text = "UNZIP A MODULE";
                btnUnZip.Img = "btn_large";
                btnUnZip.Glow = "btn_large_glow";
                btnUnZip.X = wideX;
                btnUnZip.Y = 4 * gv.uiSquareSize + pH * 8;
                btnUnZip.Height = (int)(gv.ibbheight * gv.scaler);
                btnUnZip.Width = (int)(gv.ibbwidthL * gv.scaler);
            }
            if (tglGoogleAnalytics == null)
            {
                tglGoogleAnalytics = new IbbToggle(gv);
            }
                tglGoogleAnalytics.ImgOn = "mtgl_rbtn_on";
                tglGoogleAnalytics.ImgOff = "mtgl_rbtn_off";
                tglGoogleAnalytics.X = wideX - (gv.uiSquareSize / 2);
                tglGoogleAnalytics.Y = 6 * gv.uiSquareSize - (gv.uiSquareSize / 6);
                tglGoogleAnalytics.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
                tglGoogleAnalytics.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        
        

        //TITLE SCREEN
        public void redrawSplash()
        {
            //DRAW TITLE SCREEN
            float dstHeight = ((float)gv.screenWidth / (float)titleBitmap.Width) * (float)titleBitmap.Height;
            //do narration with image setup
            IbRect src = new IbRect(0, 0, titleBitmap.Width, titleBitmap.Height);
            IbRect dst = new IbRect(0 - gv.oXshift, 0 - gv.oYshift, gv.screenWidth, (int)dstHeight);
            gv.DrawBitmap(titleBitmap, src, dst);

            btnPlay.Draw();
            btnCreate.Draw();
            btnZip.Draw();
            btnUnZip.Draw();

            //Google Analytics
            if (gv.IBprefs.GoogleAnalyticsOn) { tglGoogleAnalytics.toggleOn = true; }
            else { tglGoogleAnalytics.toggleOn = false; }
            tglGoogleAnalytics.Draw();
            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    gv.DrawText("Share Game Play", tglGoogleAnalytics.X + tglGoogleAnalytics.Width + gv.scaler + x, tglGoogleAnalytics.Y + y, "bk");
                    gv.DrawText("Analytics with IB Team:", tglGoogleAnalytics.X + tglGoogleAnalytics.Width + gv.scaler + x, tglGoogleAnalytics.Y + gv.fontHeight + gv.fontLineSpacing + y, "bk");

                }
            }
            gv.DrawText("Share Game Play", tglGoogleAnalytics.X + tglGoogleAnalytics.Width + gv.scaler, tglGoogleAnalytics.Y, "wh");
            gv.DrawText("Analytics with IB Team", tglGoogleAnalytics.X + tglGoogleAnalytics.Width + gv.scaler, tglGoogleAnalytics.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            
            //Draw IceBlink2RPG Engine Version Number
            int xLoc = (gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (4 * gv.fontWidth);
            int pH = (int)((float)gv.screenHeight / 100.0f);
            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    gv.DrawText("v" + gv.versionNum, xLoc + x, (6 * gv.uiSquareSize) + (gv.uiSquareSize / 2) + y, "bk");
                }
            }
            gv.DrawText("v" + gv.versionNum, xLoc, (6 * gv.uiSquareSize) + (gv.uiSquareSize / 2), "wh");
        }

        public void onTouchSplash(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnPlay.glowOn = false;
            btnCreate.glowOn = false;
            btnZip.glowOn = false;
            btnUnZip.glowOn = false;

            //int eventAction = event.getAction();
            switch (eventType)
            {
                case MouseEventType.EventType.MouseUp:
                    int x = (int)eX;
                    int y = (int)eY;

                    btnPlay.glowOn = false;
                    btnCreate.glowOn = false;
                    btnZip.glowOn = false;
                    btnUnZip.glowOn = false;

                    if (btnPlay.getImpact(x, y))
                    {
                        btnPlay.Text = "Loading...";
                        if (gv.fixedModule.Equals(""))
                        {
                            gv.screenLauncher = new ScreenLauncher(gv.mod, gv);
                            gv.screenLauncher.loadModuleInfoFiles();
                            gv.screenType = "launcher";
                        }
                        else
                        {
                            gv.mod.moduleName = gv.fixedModule + "/" + gv.fixedModule;
                            gv.resetGame();
                        }
                    }
                    else if (btnCreate.getImpact(x, y))
                    {
                        SelectModuleToEdit();
                    }
                    else if (btnZip.getImpact(x, y))
                    {
                        SelectModuleToZip();
                    }
                    else if (btnUnZip.getImpact(x, y))
                    {
                        SelectModuleToUnZip();
                    }
                    else if (tglGoogleAnalytics.getImpact(x, y))
                    {
                        tglGoogleAnalytics.toggleOn = !tglGoogleAnalytics.toggleOn;
                        gv.IBprefs.GoogleAnalyticsOn = tglGoogleAnalytics.toggleOn;
                        gv.savePreferences();
                        string policy = "When this box is checked, data about your game play will be sent " +
                        "to the Iceblink Engine Team's Google Analytics Dashboard. There is no personally " +
                        "identifiable information contained in the data sent. The data is used to see " +
                        "where we can improve on the app and to let builders know when and how their module " +
                        "is being played. Since we all do this for free, this little bit of insight into how " +
                        "our project is going and seeing that others are enjoying it is part of our fuel to " +
                        "keep us motivated. The type of data sent looks like this:" + Environment.NewLine + Environment.NewLine +
                        "7/22/2018 7:41:21 AM: TheElderinStone(v7):(IBv1.0.03):none_754200 *** TheElderinStone(v7):(IBv1.0.03):none_754200:20180722074120:00000420:HP5005:SP5045:XP0:LVL6:PS6::NEWGAME:TheElderinStone *** none" + Environment.NewLine + Environment.NewLine +
                        "7/22/2018 7:41:22 AM: TheElderinStone(v7):(IBv1.0.03):Odren_754200 *** TheElderinStone(v7):(IBv1.0.03):Odren_754200: 20180722074122:00000424:HP4018: SP4051: XP0: LVL6: PS6::CONVO:0 Intro *** none" + Environment.NewLine + Environment.NewLine +
                        "7/22/2018 7:41:25 AM: TheElderinStone(v7):(IBv1.0.03):Odren_754200 *** TheElderinStone(v7):(IBv1.0.03):Odren_754200: 20180722074125:00000424:HP4018: SP4051: XP0: LVL6: PS6::JOURNAL:The Elderin Stone-- Retrieving the Stone *** none";
                        gv.IBMessageBox("IBbasic Privacy Policy", policy);
                    }
                    break;
                    
                case MouseEventType.EventType.MouseMove:
                case MouseEventType.EventType.MouseDown:
                    x = (int) eX;
                    y = (int) eY;

                    if (btnPlay.getImpact(x, y))
                    {
                        btnPlay.glowOn = true;                                              
                    }
                    else if (btnCreate.getImpact(x, y))
                    {
                        btnCreate.glowOn = true;
                    }
                    else if (btnZip.getImpact(x, y))
                    {
                        btnZip.glowOn = true;
                    }
                    else if (btnUnZip.getImpact(x, y))
                    {
                        btnUnZip.glowOn = true;
                    }
                    break;
            }
        }

        public async void GetStringInput()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("enter a string:","main directory: " + gv.mainDirectory);
            btnCreate.Text = myinput;
            gv.touchEnabled = true;
        }
        public async void GetNumInput()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("test number", 999);
            btnCreate.Text = myinput.ToString();
            gv.touchEnabled = true;
        }
        public async void SelectModuleToEdit()
        {
            gv.touchEnabled = false;

            List<string> itlist = new List<string>();
            itlist.Add("New Module");

            var list2 = loadModuleNamesToList();
            foreach (string s in list2)
            {
                itlist.Add(s);
            }
                        
            string selectedModule = await gv.ListViewPage(itlist, "Module to Edit");
            
            //btnCreate.Text = selectedModule;

            if (selectedModule.Equals("New Module"))
            {
                gv.mod = gv.cc.LoadModule("NewModule.mod");
                gv.resetGame();
                gv.screenType = "tsModule";
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName, "none");
            }
            else
            {
                gv.mod = gv.cc.LoadModule(selectedModule + ".mod");
                gv.resetGame();
                gv.screenType = "tsModule";
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName, "none");
            }
            
            gv.touchEnabled = true;
        }
        public async void SelectModuleToZip()
        {
            gv.touchEnabled = false;

            List<string> itlist = new List<string>();
            itlist.Add("None");

            var list2 = loadModuleNamesToList();
            foreach (string s in list2)
            {
                itlist.Add(s);
            }

            string selectedModule = await gv.ListViewPage(itlist, "Module to Zip");

            if (selectedModule.Equals("None"))
            {
                //do nothing
            }
            else
            {
                gv.ZipModule(selectedModule);
            }

            gv.touchEnabled = true;           
        }
        public async void SelectModuleToUnZip()
        {
            gv.touchEnabled = false;

            List<string> itlist = new List<string>();
            itlist.Add("None");

            var list2 = loadZipFilesToList();
            foreach (string s in list2)
            {
                itlist.Add(s);
            }

            string selectedModule = await gv.ListViewPage(itlist, "Module to UnZip");

            if (selectedModule.Equals("None"))
            {
                //do nothing
            }
            else
            {
                gv.UnZipModule(selectedModule);
            }

            gv.touchEnabled = true;
        }

        public List<string> loadModuleNamesToList()
        {
            List<string> retList = new List<string>();
            List<string> modList = gv.GetAllModuleFiles();
            foreach (string file in modList)
            {
                if (Path.GetFileName(file) != "NewModule.mod")
                {
                    // Process each file
                    Module modinfo = gv.cc.LoadModuleFileInfo(file);
                    if (modinfo == null)
                    {
                        gv.sf.MessageBox("returned a null module");
                    }
                    retList.Add(modinfo.moduleName);                    
                }
            }

            return retList;
        }

        public List<string> loadZipFilesToList()
        {
            List<string> retList = new List<string>();
            retList = gv.GetAllFilesWithExtensionFromBothFolders("\\modules", "\\modules", ".zip");            
            return retList;
        }
    }
}
