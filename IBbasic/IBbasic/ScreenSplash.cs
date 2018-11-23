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
        private IbbButton btnNews = null;
        private IbbButton btnTwitter = null;
        private IbbButton btnWebsite = null;
        private IbbButton btnUserName = null;
        private IbbButton btnComment = null;

        //public IbbToggle tglGoogleAnalytics = null;
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
                btnPlay.Y = 0 * gv.uiSquareSize + (1 * gv.uiSquareSize / 2) + pH * 2;
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
                btnCreate.Y = 1 * gv.uiSquareSize + (1 * gv.uiSquareSize / 2) + pH * 4;
                btnCreate.Height = (int)(gv.ibbheight * gv.scaler);
                btnCreate.Width = (int)(gv.ibbwidthL * gv.scaler);
            }
            if (btnZip == null)
            {
                btnZip = new IbbButton(gv, 1.0f);
                btnZip.Text = "PACK A MODULE";
                btnZip.Img = "btn_large";
                btnZip.Glow = "btn_large_glow";
                btnZip.X = wideX;
                btnZip.Y = 2 * gv.uiSquareSize + (1 * gv.uiSquareSize / 2) + pH * 6;
                btnZip.Height = (int)(gv.ibbheight * gv.scaler);
                btnZip.Width = (int)(gv.ibbwidthL * gv.scaler);
            }
            if (btnUnZip == null)
            {
                btnUnZip = new IbbButton(gv, 1.0f);
                btnUnZip.Text = "UNPACK A MODULE";
                btnUnZip.Img = "btn_large";
                btnUnZip.Glow = "btn_large_glow";
                btnUnZip.X = wideX;
                btnUnZip.Y = 3 * gv.uiSquareSize + (1 * gv.uiSquareSize / 2) + pH * 8;
                btnUnZip.Height = (int)(gv.ibbheight * gv.scaler);
                btnUnZip.Width = (int)(gv.ibbwidthL * gv.scaler);
            }
            if (btnNews == null)
            {
                btnNews = new IbbButton(gv, 1.0f);
                btnNews.Text = "IBbasic News";
                btnNews.Img = "btn_large";
                btnNews.Glow = "btn_large_glow";
                btnNews.X = wideX;
                btnNews.Y = 4 * gv.uiSquareSize + (1 * gv.uiSquareSize / 2) + pH * 10;
                btnNews.Height = (int)(gv.ibbheight * gv.scaler);
                btnNews.Width = (int)(gv.ibbwidthL * gv.scaler);
            }
            if (btnTwitter == null)
            {
                btnTwitter = new IbbButton(gv, 0.8f);
            }
            btnTwitter.Img = "btn_small";
            btnTwitter.Img2 = "btntwitter";
            btnTwitter.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnTwitter.X = 10 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnTwitter.Y = 6 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnTwitter.Height = (int)(gv.ibbheight * gv.scaler);
            btnTwitter.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnWebsite == null)
            {
                btnWebsite = new IbbButton(gv, 0.8f);
            }
            btnWebsite.Img2 = "btnweb";
            btnWebsite.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
            btnWebsite.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnWebsite.X = 9 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnWebsite.Y = 6 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnWebsite.Height = (int)(gv.ibbheight * gv.scaler);
            btnWebsite.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnUserName == null)
            {
                btnUserName = new IbbButton(gv, 0.8f);
            }
            btnUserName.Img = "btn_small";
            btnUserName.Img2 = "btnusername";
            btnUserName.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnUserName.X = 8 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnUserName.Y = 6 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnUserName.Height = (int)(gv.ibbheight * gv.scaler);
            btnUserName.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnComment == null)
            {
                btnComment = new IbbButton(gv, 0.8f);
            }
            btnComment.Img = "btn_small";
            btnComment.Img2 = "btnconvo";
            btnComment.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnComment.X = 7 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnComment.Y = 6 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnComment.Height = (int)(gv.ibbheight * gv.scaler);
            btnComment.Width = (int)(gv.ibbwidthR * gv.scaler);

        }

        public async void downloadFile(string filename)
        {
            //downloadText = "Downloading...may take a few seconds...";            
            bool result = await gv.DownloadResult("https://www.iceblinkengine.com/ibbasic_modules/" + filename, filename);
            
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
            btnNews.Draw();
            btnTwitter.Draw();
            btnWebsite.Draw();
            btnUserName.Draw();
            btnComment.Draw();

            //Draw IceBlink2RPG Engine Version Number
            int xLoc = (gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (4 * gv.fontWidth);
            int yLoc = 6 * gv.uiSquareSize + gv.uiSquareSize / 2;
            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    gv.DrawText("v" + gv.versionNum, xLoc + x, yLoc + y, "bk");
                }
            }
            gv.DrawText("v" + gv.versionNum, xLoc, yLoc, "wh");

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }

        public void onTouchSplash(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnPlay.glowOn = false;
            btnCreate.glowOn = false;
            btnZip.glowOn = false;
            btnUnZip.glowOn = false;
            btnNews.glowOn = false;
            btnTwitter.glowOn = false;
            btnWebsite.glowOn = false;
            btnUserName.glowOn = false;
            btnComment.glowOn = false;

            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }

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
                    btnNews.glowOn = false;
                    btnTwitter.glowOn = false;
                    btnWebsite.glowOn = false;
                    btnUserName.glowOn = false;
                    btnComment.glowOn = false;

                    if (gv.showMessageBox)
                    {
                        gv.messageBox.btnReturn.glowOn = false;
                    }

                    if (gv.showMessageBox)
                    {
                        if (gv.messageBox.btnReturn.getImpact(x, y))
                        {
                            gv.PlaySound("btn_click");
                            gv.showMessageBox = false;
                        }
                        return;
                    }
                    else
                    {
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
                            if (!gv.AllowReadWriteExternal())
                            {
                                string noreadwrite = "ANDROID: You must grant External Storage Read and Write Permissions in order to use this feature." + Environment.NewLine + Environment.NewLine +
                                                        "iOS: This feature is only available on devices running iOS 10.3 and greater.";
                                noreadwrite = "You must grant External Storage Read and Write Permissions in order to use this feature. Do this through your devices Settings/Apps/IceBlink Basic. Toggle 'Storage' to on and then restart this app.";
                                gv.IBMessageBox("Feature Not Available", noreadwrite);
                            }
                            else
                            {
                                SelectModuleToEdit();
                            }
                        }
                        else if (btnZip.getImpact(x, y))
                        {
                            if (!gv.AllowReadWriteExternal())
                            {
                                string noreadwrite = "ANDROID: You must grant External Storage Read and Write Permissions in order to use this feature." + Environment.NewLine + Environment.NewLine +
                                                        "iOS: This feature is only available on devices running iOS 10.3 and greater.";
                                noreadwrite = "You must grant External Storage Read and Write Permissions in order to use this feature. Do this through your devices Settings/Apps/IceBlink Basic. Toggle 'Storage' to on and then restart this app.";
                                gv.IBMessageBox("Feature Not Available", noreadwrite);
                            }
                            else
                            {
                                SelectModuleToZip();
                            }
                        }
                        else if (btnUnZip.getImpact(x, y))
                        {
                            if (!gv.AllowReadWriteExternal())
                            {
                                string noreadwrite = "ANDROID: You must grant External Storage Read and Write Permissions in order to use this feature." + Environment.NewLine + Environment.NewLine +
                                                        "iOS: This feature is only available on devices running iOS 10.3 and greater.";
                                noreadwrite = "You must grant External Storage Read and Write Permissions in order to use this feature. Do this through your devices Settings/Apps/IceBlink Basic. Toggle 'Storage' to on and then restart this app.";
                                gv.IBMessageBox("Feature Not Available", noreadwrite);
                            }
                            else
                            {
                                SelectModuleToUnZip();
                            }
                        }
                        else if (btnNews.getImpact(x, y))
                        {
                            if (!gv.AllowReadWriteExternal())
                            {
                                string noreadwrite = "ANDROID: You must grant External Storage Read and Write Permissions in order to use this feature." + Environment.NewLine + Environment.NewLine +
                                                        "iOS: This feature is only available on devices running iOS 10.3 and greater.";
                                noreadwrite = "You must grant External Storage Read and Write Permissions in order to use this feature. Do this through your devices Settings/Apps/IceBlink Basic. Toggle 'Storage' to on and then restart this app.";
                                gv.IBMessageBox("Feature Not Available", noreadwrite);
                            }
                            else
                            {
                                gv.TrackerSendEvent(":READ_IB_NEWS:", "none", true);
                                downloadFile("ibnews.txt");
                                string news = loadNews();
                                gv.showMessageBox = true;
                                gv.sf.MessageBoxHtml(news);
                            }                            
                        }
                        else if (btnTwitter.getImpact(x, y))
                        {
                            gv.TrackerSendEvent(":VISIT_TWITTER:", "none", true);
                            Device.OpenUri(new Uri("twitter://user?user_id=1042598307187503104"));
                        }
                        else if (btnWebsite.getImpact(x, y))
                        {
                            gv.TrackerSendEvent(":VISIT_WEBSITE:", "none", true);
                            Device.OpenUri(new Uri("https://iceblinkengine.com"));
                        }
                        else if (btnUserName.getImpact(x, y))
                        {
                            gv.TrackerSendEvent(":CHANGE_USERNAME:", "none", true);
                            changeUserName();                           
                        }
                        else if (btnComment.getImpact(x, y))
                        {
                            gv.TrackerSendEvent(":COMMENT_PAGE:", "none", true);
                            Device.OpenUri(new Uri("http://www.iceblinkengine.com/comment_system/index.php"));
                        }
                    }
                    break;
                    
                case MouseEventType.EventType.MouseMove:
                case MouseEventType.EventType.MouseDown:
                    x = (int) eX;
                    y = (int) eY;

                    if (gv.showMessageBox)
                    {
                        if (gv.messageBox.btnReturn.getImpact(x, y))
                        {
                            gv.messageBox.btnReturn.glowOn = true;
                        }
                        return;
                    }
                    else
                    {
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
                        else if (btnNews.getImpact(x, y))
                        {
                            btnNews.glowOn = true;
                        }
                        else if (btnTwitter.getImpact(x, y))
                        {
                            btnTwitter.glowOn = true;
                        }
                        else if (btnWebsite.getImpact(x, y))
                        {
                            btnWebsite.glowOn = true;
                        }
                        else if (btnUserName.getImpact(x, y))
                        {
                            btnUserName.glowOn = true;
                        }
                        else if (btnComment.getImpact(x, y))
                        {
                            btnComment.glowOn = true;
                        }
                    }
                    break;
            }
        }

        public string loadNews()
        {
            string txt = "";
            try
            {
                txt = gv.LoadStringFromUserFolder("\\modules\\ibnews.txt");                
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
                return "error loading...";
            }
            return txt;
        }

        public async void changeUserName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Your User Name (leave blank to remain anonymous), will be used for the integrated commenting system once ready:", gv.IBprefs.UserName);
            gv.IBprefs.UserName = myinput;
            gv.savePreferences();
            gv.touchEnabled = true;
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
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName, "none", false);
            }
            else
            {
                gv.mod = gv.cc.LoadModule(selectedModule + ".mod");
                gv.resetGame();
                gv.screenType = "tsModule";
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName, "none", false);
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
            List<string> modList = gv.GetAllModuleFiles(true);
            foreach (string file in modList)
            {
                if (Path.GetFileName(file) == "NewModule.mod")
                {
                    continue;
                }
                // Process each file
                ModuleInfo modinfo = gv.cc.LoadModuleFileInfo(file);
                if (modinfo == null)
                {
                    gv.sf.MessageBox("returned a null module");
                }
                retList.Add(modinfo.moduleName);                    
            }

            return retList;
        }

        public List<string> loadZipFilesToList()
        {
            List<string> retList = new List<string>();
            retList = gv.GetAllFilesWithExtensionFromBothFolders("\\modules", "\\modules", ".ibb");            
            return retList;
        }
    }
}
