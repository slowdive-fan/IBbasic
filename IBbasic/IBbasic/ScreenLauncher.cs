using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace IBbasic
{
    public class ScreenLauncher 
    {
	    //private Module gv.mod;
	    private GameView gv;
	
	    private IbbButton btnLeft = null;
	    private IbbButton btnRight = null;
	    private IbbButton btnModuleName = null;
        private IbbButton btnGetUpdates = null;
        private IBminiTextBox description;
	    //private List<Module> moduleList = new List<Module>();
        private List<ModuleInfo> moduleInfoList = new List<ModuleInfo>();
        public List<ModuleInfo> modsAvailableList = new List<ModuleInfo>();
        private List<SKBitmap> titleList = new List<SKBitmap>();
        private IbbButton btnUserName = null;
        private IbbButton btnReadReviews = null;
        private IbbButton btnPostReview = null;
        private IbbToggle tglEndorse = null;
        private int moduleIndex = 0;
        public string downloadText = "";
        long previousTime = 0;
        int elapsedTime = 0;
        long currentTime = 0;

        public ScreenLauncher(Module m, GameView g) 
	    {
		    //gv.mod = m;
		    gv = g;
		    setControlsStart();
            int pH = (int)((float)gv.screenHeight / 100.0f);
            description = new IBminiTextBox(gv);
            description.tbXloc = (int)(0 * gv.squareSize * gv.scaler);
            description.tbYloc = (int)(6 * gv.squareSize * gv.scaler);
            description.tbWidth = (int)(16 * gv.squareSize * gv.scaler);
            description.tbHeight = (int)(6 * gv.squareSize * gv.scaler);
            description.showBoxBorder = false;

            downloadFile("IBbasicCommunityReviews.json", "IBbasicCommunityReviews.json");
            gv.loadReviews();
        }
	
        //when click on "Get Updates"
        //download mod_available.json from server
        public async void downloadFile(string filename, string folderpath)
        {
            //downloadText = "Downloading...may take a few seconds...";            
            bool result = await gv.DownloadResult("https://www.iceblinkengine.com/ibbasic_modules/" + filename, folderpath);
            downloadText = "";
            if (result) { downloadText = "download was successful."; }
            else { downloadText = "failed to download the file...please check your connection."; }
            /*using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFile("http://www.iceblinkengine.com/ibmini_modules/" + filename, outFolder + "\\" + filename);
                    gv.sf.MessageBox("Completed Downloading: " + filename);
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("Error Downloading: " + ex.ToString());
                }
            }*/
        }
        //convert to object
        public void loadModsAvailableList()
        {
            if (modsAvailableList == null)
            {
                modsAvailableList = new List<ModuleInfo>();
            }
            modsAvailableList.Clear();
            try
            {
                string file = gv.LoadStringFromUserFolder("\\modules\\mods_available.json");
                using (StringReader sr = new StringReader(file))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    modsAvailableList = (List<ModuleInfo>)serializer.Deserialize(sr, typeof(List<ModuleInfo>));
                }
                // deserialize JSON directly from a file                
                //JsonSerializer serializer = new JsonSerializer();
                //modsAvailableList = (List<ModuleInfo>)serializer.Deserialize(file, typeof(List<ModuleInfo>));                
            }
            catch (Exception ex)
            {

            }
        }
        //compare to moduleInfoList and add any that are not there and assign button name
        public void setupModuleInfoListAndButtonText(bool showMessageBox)
        {
            string updateList = "";
            string newModList = "";
            //go through all moduleInfoList items and set buttonText to PLAY
            /*foreach (ModuleInfo modInfo in moduleInfoList)
            {
                modInfo.buttonText = "PLAY";
            }*/
            //go through each item in modsAvailableList and see if is in moduleInfoList
            foreach (ModuleInfo modAvail in modsAvailableList)
            {
                bool foundOne = false;
                foreach (ModuleInfo modInfo in moduleInfoList)
                {
                    if (modAvail.moduleName.Equals(modInfo.moduleName))
                    {
                        foundOne = true;
                        //if is there check versions and set to UPDATE or PLAY
                        if (modAvail.moduleVersion > modInfo.moduleVersion)
                        {
                            modInfo.buttonText = "UPDATE";
                            updateList += modAvail.moduleLabelName + "<br>";
                        }
                    }
                }
                if (!foundOne)
                {
                    //if not there, add to list and set to DOWNLOAD
                    modAvail.buttonText = "DOWNLOAD";
                    moduleInfoList.Add(modAvail);
                    newModList += modAvail.moduleLabelName + "<br>";
                }
                else
                {
                    modAvail.buttonText = "PLAY";
                }
            }
            if (showMessageBox)
            {
                gv.showMessageBox = true;
                string text = "<yl>New Modules:</yl><br>"
                        + newModList
                        + "<yl>Updated Modules:</yl><br>"
                        + updateList;
                gv.sf.MessageBoxHtml(text);
            }
        }

        /*public void loadModuleFiles()
        {
            string[] files;

            files = Directory.GetFiles(gv.mainDirectory + "\\modules", "*.mod", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (Path.GetFileName(file) != "NewModule.mod")
                {
                    // Process each file
                    Module mod = gv.cc.LoadModule(file, true);
                    if (mod == null)
                    {
                        gv.sf.MessageBox("returned a null module");
                    }
                    moduleList.Add(mod);
                    //titleList.Add(gv.cc.LoadBitmap("title", mod));
                    titleList.Add(gv.cc.GetFromBitmapList(mod.titleImageName));
                }
            }
        }*/
        public void loadModuleInfoFiles()
        {
            moduleInfoList.Clear();
            titleList.Clear();

            List<string> modList = gv.GetAllModuleFiles(false);
            foreach (string file in modList)
            {
                ModuleInfo modinfo = gv.cc.LoadModuleFileInfo(file);
                Module modfile = gv.cc.LoadModule(file);
                if ((modinfo == null) || (modfile == null))
                {
                    gv.sf.MessageBox("returned a null module");
                }
                else
                {
                    moduleInfoList.Add(modinfo);
                    titleList.Add(gv.cc.GetFromBitmapList(modinfo.titleImageName, modfile));
                }

                /*string s = gv.GetModuleFileString(file);
                using (StringReader sr = new StringReader(s))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    ModuleInfo modinfo = (Module)serializer.Deserialize(sr, typeof(Module));
                    Module modfile = gv.cc.LoadModule(fname + "\\" + fname + ".mod");
                    if (modinfo != null)
                    {
                        moduleInfoList.Add(modinfo);
                        titleList.Add(gv.cc.GetFromBitmapList(modinfo.titleImageName, modinfo));
                    }                    
                }*/                                
            }
        }

        public void setControlsStart()
        {
            int pW = (int)((float)gv.screenWidth / 100.0f);
            int pH = (int)((float)gv.screenHeight / 100.0f);
            int wideX = (gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (int)(gv.ibbwidthL * gv.scaler / 2);
            int smallLeftX = wideX - (int)(gv.ibbwidthR * gv.scaler);
            int smallRightX = wideX + (int)(gv.ibbwidthL * gv.scaler);
            int largeRightX = wideX + (int)(gv.ibbwidthL * gv.scaler) + (int)(gv.ibbwidthR * gv.scaler) + (int)(gv.ibbwidthR * gv.scaler / 2);
            int padW = gv.uiSquareSize / 6;

            if (btnLeft == null)
            {
                btnLeft = new IbbButton(gv, 1.0f);
            }
                btnLeft.Img = "btn_small";
                btnLeft.Img2 = "ctrl_left_arrow";
                btnLeft.Glow = "btn_small_glow";
                btnLeft.X = smallLeftX;
                btnLeft.Y = (2 * gv.uiSquareSize) + (gv.uiSquareSize / 6);                
                btnLeft.Height = (int)(gv.ibbheight * gv.scaler);
                btnLeft.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnModuleName == null)
            {
                btnModuleName = new IbbButton(gv, 1.0f);
            }
                btnModuleName.Img = "btn_large";
                btnModuleName.Glow = "btn_large_glow";
                btnModuleName.Text = "";
                btnModuleName.X = wideX;
                btnModuleName.Y = (2 * gv.uiSquareSize) + (gv.uiSquareSize / 6);                
                btnModuleName.Height = (int)(gv.ibbheight * gv.scaler);
                btnModuleName.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnRight == null)
            {
                btnRight = new IbbButton(gv, 1.0f);
            }
                btnRight.Img = "btn_small";
                btnRight.Img2 = "ctrl_right_arrow";
                btnRight.Glow = "btn_small_glow";
                btnRight.X = smallRightX;
                btnRight.Y = (2 * gv.uiSquareSize) + (gv.uiSquareSize / 6);
                btnRight.Height = (int)(gv.ibbheight * gv.scaler);
                btnRight.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnGetUpdates == null)
            {
                btnGetUpdates = new IbbButton(gv, 1.0f);
            }
                btnGetUpdates.Img = "btn_large";
                btnGetUpdates.Glow = "btn_large_glow";
                btnGetUpdates.Text = "GET MODULES";
                btnGetUpdates.X = wideX;
                btnGetUpdates.Y = (6 * gv.uiSquareSize) - (pH * 2);
                btnGetUpdates.Height = (int)(gv.ibbheight * gv.scaler);
                btnGetUpdates.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnUserName == null)
            {
                btnUserName = new IbbButton(gv, 0.8f);
            }
            btnUserName.Img = "btn_small";
            btnUserName.Img2 = "btnusername";
            btnUserName.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnUserName.X = 10 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnUserName.Y = 6 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnUserName.Height = (int)(gv.ibbheight * gv.scaler);
            btnUserName.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnReadReviews == null)
            {
                btnReadReviews = new IbbButton(gv, 0.8f);
            }
            btnReadReviews.Img = "btn_small";
            btnReadReviews.Img2 = "btnconvo";
            btnReadReviews.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnReadReviews.X = 10 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnReadReviews.Y = 0 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnReadReviews.Height = (int)(gv.ibbheight * gv.scaler);
            btnReadReviews.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnPostReview == null)
            {
                btnPostReview = new IbbButton(gv, 0.8f);
            }
            btnPostReview.Img = "btn_small";
            btnPostReview.Img2 = "btnpencil";
            btnPostReview.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnPostReview.X = 10 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnPostReview.Y = 1 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            btnPostReview.Height = (int)(gv.ibbheight * gv.scaler);
            btnPostReview.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (tglEndorse == null)
            {
                tglEndorse = new IbbToggle(gv);
                tglEndorse.toggleOn = false;
            }
            tglEndorse.ImgOn = "tgl_endorse_on";
            tglEndorse.ImgOff = "tgl_endorse_off";
            tglEndorse.X = 10 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            tglEndorse.Y = 2 * gv.uiSquareSize + (0 * gv.uiSquareSize / 2);
            tglEndorse.Height = (int)(gv.ibbheight * gv.scaler);
            tglEndorse.Width = (int)(gv.ibbwidthR * gv.scaler);

        }
        //TITLE SCREEN  
        public void redrawLauncher()
        {
            setControlsStart();
            int titleW = gv.uiSquareSize * 4;
            int titleH = gv.uiSquareSize * 2;
            int titleX = (gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (gv.uiSquareSize * 2);
            //DRAW TITLE SCREEN
    	    if ((titleList.Count > 0) && (moduleIndex < titleList.Count))
		    {
                IbRect src = new IbRect(0, 0, titleList[moduleIndex].Width, titleList[moduleIndex].Height);
                IbRect dst = new IbRect(titleX, 0, titleW, titleH);
                gv.DrawBitmap(titleList[moduleIndex], src, dst);
		    }

            gv.DrawText(downloadText, 0 * gv.uiSquareSize, 0 * gv.uiSquareSize, "yl");

            //DRAW DESCRIPTION BOX
            if ((moduleInfoList.Count > 0) && (moduleIndex < moduleInfoList.Count))
		    {
                btnModuleName.Text = moduleInfoList[moduleIndex].buttonText + " MODULE";
                drawLauncherControls();

                string textToSpan = "<gn>" + moduleInfoList[moduleIndex].moduleLabelName + "</gn><br>";
                description.tbXloc = 0 * gv.uiSquareSize / 2;
                description.tbYloc = 3 * gv.uiSquareSize + (gv.uiSquareSize / 4);
                description.tbWidth = 11 * gv.uiSquareSize + gv.uiSquareSize / 2;
                description.tbHeight = 6 * gv.uiSquareSize;
                textToSpan += moduleInfoList[moduleIndex].moduleDescription;
                description.linesList.Clear();
                description.AddFormattedTextToTextBox(textToSpan);
                description.onDrawTextBox();

                btnPostReview.Draw();

                btnReadReviews.Draw();
                string reviewCnt = getTotalReviewCount().ToString();
                gv.DrawText(reviewCnt, btnReadReviews.X - (gv.fontWidth * reviewCnt.Length), btnReadReviews.Y + (btnReadReviews.Height / 2), "wh");

                setEndorseToggle();
                tglEndorse.Draw();
                string endorseCnt = getTotalEndorsementCount().ToString();
                gv.DrawText(endorseCnt, tglEndorse.X - (gv.fontWidth * endorseCnt.Length), tglEndorse.Y + (tglEndorse.Height / 2), "wh");

            }

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }

        public int getTotalReviewCount()
        {
            int cnt = 0;
            foreach (IBReview rev in gv.userReviews.reviews)
            {
                if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                {
                    if (rev.review.Length > 0)
                    {
                        cnt++;
                    }
                }
            }
            foreach (IBReview rev in gv.communityReviews.reviews)
            {
                if (rev.userID != gv.IBprefs.UserID)
                {
                    if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                    {
                        if (rev.review.Length > 0)
                        {
                            cnt++;
                        }
                    }
                }
            }
            return cnt;
        }
        public int getTotalEndorsementCount()
        {
            int cnt = 0;
            foreach (IBEndorse rev in gv.userReviews.endorsements)
            {
                if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                {
                    if (rev.endorsed)
                    {
                        cnt++;
                    }
                }
            }
            foreach (IBEndorse rev in gv.communityReviews.endorsements)
            {
                if (rev.userID != gv.IBprefs.UserID)
                {
                    if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                    {
                        if (rev.endorsed)
                        {
                            cnt++;
                        }
                    }
                }
            }
            return cnt;
        }
        public string getTotalEndorsementString()
        {
            string s = "Liked by: ";
            foreach (IBEndorse rev in gv.userReviews.endorsements)
            {
                if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                {
                    s += rev.userName + ", ";
                }
            }
            foreach (IBEndorse rev in gv.communityReviews.endorsements)
            {
                if (rev.userID != gv.IBprefs.UserID)
                {
                    if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                    {
                        s += rev.userName + ", ";
                    }
                }
            }
            return s;
        }
        public void setEndorseToggle()
        {
            tglEndorse.toggleOn = false;
            foreach (IBEndorse rev in gv.userReviews.endorsements)
            {
                if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                {
                    if (rev.endorsed)
                    {
                        tglEndorse.toggleOn = true;
                    }
                }
            }
        }

        public void drawLauncherControls()
	    {    	
		    btnLeft.Draw();		
		    btnRight.Draw();
		    btnModuleName.Draw();
            btnGetUpdates.Draw();
            btnUserName.Draw();
        }
        public void onTouchLauncher(int eX, int eY, MouseEventType.EventType eventType)
	    {
    	    btnLeft.glowOn = false;
    	    btnRight.glowOn = false;	
    	    btnModuleName.glowOn = false;
            btnGetUpdates.glowOn = false;
            btnUserName.glowOn = false;
            btnReadReviews.glowOn = false;
            btnPostReview.glowOn = false;
            downloadText = "";

            switch (eventType)
		    {
		        case MouseEventType.EventType.MouseUp:
			        int x = (int) eX;
			        int y = (int) eY;
				
			        btnLeft.glowOn = false;
	    	        btnRight.glowOn = false;	
	    	        btnModuleName.glowOn = false;
                    btnGetUpdates.glowOn = false;
                    btnUserName.glowOn = false;
                    btnReadReviews.glowOn = false;
                    btnPostReview.glowOn = false;

                    if (gv.showMessageBox)
                    {
                        gv.messageBox.btnReturn.glowOn = false;
                    }
                    if (gv.showMessageBox)
                    {
                        if (gv.messageBox.btnReturn.getImpact(x, y))
                        {
                            gv.showMessageBox = false;
                        }
                        return;
                    }
                    else
                    {
                        if (btnLeft.getImpact(x, y))
                        {
                            if (moduleIndex > 0)
                            {
                                moduleIndex--;
                                btnModuleName.Text = moduleInfoList[moduleIndex].moduleName;
                            }
                        }
                        else if (btnRight.getImpact(x, y))
                        {
                            if (moduleIndex < moduleInfoList.Count - 1)
                            {
                                moduleIndex++;
                                btnModuleName.Text = moduleInfoList[moduleIndex].moduleName;
                            }
                        }
                        else if (btnModuleName.getImpact(x, y))
                        {
                            if (moduleInfoList[moduleIndex].buttonText.Equals("PLAY"))
                            {
                                //load the mod since we only have the ModuleInfo                            
                                gv.mod = gv.cc.LoadModule(moduleInfoList[moduleIndex].moduleName + ".mod");
                                gv.resetGame();
                                gv.cc.LoadSaveListItems();
                                gv.screenType = "title";
                            }
                            else if (moduleInfoList[moduleIndex].buttonText.Equals("UPDATE"))
                            {
                                gv.TrackerSendEvent(":UPDATE_START_" + moduleInfoList[moduleIndex].moduleName + ":TIME(ms)-0", "none", true);
                                //download and replace existing file
                                previousTime = gv.gameTimerStopwatch.ElapsedMilliseconds;
                                downloadFile(moduleInfoList[moduleIndex].moduleName + ".ibb", "modules\\" + moduleInfoList[moduleIndex].moduleName + ".ibb");
                                currentTime = gv.gameTimerStopwatch.ElapsedMilliseconds;
                                elapsedTime = (int)(currentTime - previousTime);
                                gv.TrackerSendEvent(":UPDATE_END_" + moduleInfoList[moduleIndex].moduleName + ":TIME(ms)-" + elapsedTime.ToString(), "none", true);
                                //delete old folder
                                //DeleteFolder(moduleInfoList[moduleIndex].moduleName);
                                //unzip file
                                UnZipFile(moduleInfoList[moduleIndex].moduleName);
                                //once download is complete, do the "Get Updates" button stuff
                                loadModuleInfoFiles();
                                loadModsAvailableList();
                                setupModuleInfoListAndButtonText(false);
                            }
                            else if (moduleInfoList[moduleIndex].buttonText.Equals("DOWNLOAD"))
                            {
                                gv.TrackerSendEvent(":DOWNLOAD_START_" + moduleInfoList[moduleIndex].moduleName + ":TIME(ms)-0", "none", true);
                                //download file
                                previousTime = gv.gameTimerStopwatch.ElapsedMilliseconds;
                                downloadFile(moduleInfoList[moduleIndex].moduleName + ".ibb", "modules\\" + moduleInfoList[moduleIndex].moduleName + ".ibb");
                                currentTime = gv.gameTimerStopwatch.ElapsedMilliseconds;
                                elapsedTime = (int)(currentTime - previousTime);
                                gv.TrackerSendEvent(":DOWNLOAD_END_" + moduleInfoList[moduleIndex].moduleName + ":TIME(ms)-" + elapsedTime.ToString(), "none", true);
                                //unzip file
                                UnZipFile(moduleInfoList[moduleIndex].moduleName);
                                //once download is complete, do the "Get Updates" button stuff
                                loadModuleInfoFiles();
                                loadModsAvailableList();
                                setupModuleInfoListAndButtonText(false);
                            }
                        }
                        else if (btnGetUpdates.getImpact(x, y))
                        {
                            gv.TrackerSendEvent(":GET_UPDATES_START:TIME(ms)-0", "none", true);
                            previousTime = gv.gameTimerStopwatch.ElapsedMilliseconds;
                            downloadFile("mods_available.json", "modules\\mods_available.json");
                            currentTime = gv.gameTimerStopwatch.ElapsedMilliseconds;
                            elapsedTime = (int)(currentTime - previousTime);
                            gv.TrackerSendEvent(":GET_UPDATES_END:TIME(ms) - " + elapsedTime.ToString(), "none", true);
                            loadModsAvailableList();
                            setupModuleInfoListAndButtonText(true);
                        }
                        else if (btnUserName.getImpact(x, y))
                        {
                            gv.TrackerSendEvent(":CHANGE_USERNAME:", "none", true);
                            changeUserName();
                        }
                        else if (btnReadReviews.getImpact(x, y))
                        {
                            gv.TrackerSendEvent(":READ_REVIEWS:", "none", true);
                            downloadFile("IBbasicCommunityReviews.json", "IBbasicCommunityReviews.json");
                            gv.loadReviews();
                            string reviews = "";
                            reviews += "<bu>LIKES:</bu><br>";
                            reviews += "<gn>" + getTotalEndorsementString() + "</gn><br>";
                            reviews += "<bu>REVIEWS:</bu><br>";
                            foreach (IBReview rev in gv.userReviews.reviews)
                            {
                                if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                                {
                                    reviews += "<yl>" + rev.reviewDate + ":" + rev.moduleName + "(v" + rev.moduleVersion + "):" + rev.userName + ":</yl>" + rev.review + Environment.NewLine;
                                }
                            }
                            foreach (IBReview rev in gv.communityReviews.reviews)
                            {
                                if (rev.userID != gv.IBprefs.UserID)
                                {
                                    if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                                    {
                                        reviews += "<yl>" + rev.reviewDate + ":" + rev.moduleName + "(v" + rev.moduleVersion + "):" + rev.userName + ":</yl>" + rev.review + Environment.NewLine;
                                    }
                                }
                            }
                            gv.showMessageBox = true;
                            gv.sf.MessageBoxHtml(reviews);
                        }
                        else if (btnPostReview.getImpact(x, y))
                        {
                            gv.TrackerSendEvent(":POST_REVIEW:", "none", true);
                            changeReview();
                        }
                        else if (tglEndorse.getImpact(x, y))
                        {
                            gv.TrackerSendEvent(":TOGGLE_ENDORSE:", "none", true);
                            changeEndorse();
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
                        if (btnLeft.getImpact(x, y))
                        {
                            btnLeft.glowOn = true;
                        }
                        else if (btnRight.getImpact(x, y))
                        {
                            btnRight.glowOn = true;
                        }
                        else if (btnModuleName.getImpact(x, y))
                        {
                            btnModuleName.glowOn = true;
                            if (moduleInfoList[moduleIndex].buttonText.Equals("UPDATE"))
                            {
                                downloadText = "Downloading update...may take seconds to a few minutes...";
                            }
                            else if (moduleInfoList[moduleIndex].buttonText.Equals("DOWNLOAD"))
                            {
                                downloadText = "Downloading module...may take seconds to a few minutes...";
                            }
                        }
                        else if (btnGetUpdates.getImpact(x, y))
                        {
                            btnGetUpdates.glowOn = true;
                            downloadText = "Checking for updates...may take seconds to a few minutes...";
                        }
                        else if (btnUserName.getImpact(x, y))
                        {
                            btnUserName.glowOn = true;
                        }
                        else if (btnReadReviews.getImpact(x, y))
                        {
                            btnReadReviews.glowOn = true;
                        }
                        else if (btnPostReview.getImpact(x, y))
                        {
                            btnPostReview.glowOn = true;
                        }
                    }
                    break;		
		    }
	    }

        public void UnZipFile(string filename)
        {
            try
            {
                gv.UnZipModule(filename);
                //string modulePath = gv.mainDirectory + "\\modules\\" + filename;
                //string zipPath = gv.mainDirectory + "\\modules\\" + filename + ".zip";
                //ZipFile.ExtractToDirectory(zipPath, modulePath);
                //MessageBox.Show("Extract file completed");
            }
            catch { }
        }

        public async void changeUserName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Your User Name (leave blank to remain anonymous), will be used for the in-app module reviewing and endorsing system:", gv.IBprefs.UserName);
            gv.IBprefs.UserName = myinput;
            gv.savePreferences();
            gv.touchEnabled = true;
        }
        public async void changeReview()
        {
            bool foundOne = false;
            foreach (IBReview rev in gv.userReviews.reviews)
            {
                if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                {
                    foundOne = true;
                    //gv.touchEnabled = false;
                    string myinput = await gv.StringInputBox("Your review for this module (all reviews will be reviewed by the IB Team within 24 hours and any inappropriate or non-constructive reviews will be rejected):", rev.review);
                    rev.review = myinput;
                    rev.userID = gv.IBprefs.UserID;
                    rev.userName = gv.IBprefs.UserName;
                    rev.moduleVersion = moduleInfoList[moduleIndex].moduleVersion;
                    rev.reviewDate = DateTime.Now.ToString("yyyy-MM-dd");
                    gv.postReview(rev);
                    gv.saveUserReviews();
                    //gv.touchEnabled = true;
                    break;
                }
            }
            if (!foundOne)
            {
                IBReview newRev = new IBReview();
                newRev.moduleName = moduleInfoList[moduleIndex].moduleName;
                newRev.userID = gv.IBprefs.UserID;
                newRev.userName = gv.IBprefs.UserName;
                newRev.moduleVersion = moduleInfoList[moduleIndex].moduleVersion;
                newRev.reviewDate = DateTime.Now.ToString("yyyy-MM-dd");
                string myinput = await gv.StringInputBox("Your review for this module (all reviews will be reviewed by the IB Team within 24 hours and any inappropriate or non-constructive reviews will be rejected):", newRev.review);
                newRev.review = myinput;
                gv.postReview(newRev);
                gv.userReviews.reviews.Add(newRev);
                gv.saveUserReviews();
            }
        }
        public async void changeEndorse()
        {
            bool foundOne = false;
            foreach (IBEndorse rev in gv.userReviews.endorsements)
            {
                if (rev.moduleName.Equals(moduleInfoList[moduleIndex].moduleName))
                {
                    foundOne = true;
                    //gv.touchEnabled = false;
                    rev.endorsed = !rev.endorsed;
                    gv.postEndorse(rev);
                    gv.saveUserReviews();
                    //gv.touchEnabled = true;
                    break;
                }
            }
            if (!foundOne)
            {
                IBEndorse newRev = new IBEndorse();
                newRev.moduleName = moduleInfoList[moduleIndex].moduleName;
                newRev.userID = gv.IBprefs.UserID;
                newRev.userName = gv.IBprefs.UserName;
                newRev.endorsed = true;
                gv.postEndorse(newRev);
                gv.userReviews.endorsements.Add(newRev);
                gv.saveUserReviews();
            }
        }
    }
}
