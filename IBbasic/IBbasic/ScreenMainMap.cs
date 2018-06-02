using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SkiaSharp;

namespace IBbasic
{
    public class ScreenMainMap
    {
        //public Module gv.mod;
        public GameView gv;

        //public IB2UILayout mainUiLayout = null;
        public bool showMiniMap = false;
        public bool showClock = false;
        public bool showFullParty = false;
        public bool showArrows = true;
        public bool showTogglePanel = true;
        public bool showPortraitPanel = true;
        public bool hideClock = false;
        public List<FloatyText> floatyTextPool = new List<FloatyText>();
        public IBminiTextBox floatyTextBox;
        public int mapStartLocXinPixels;
        public int map3DViewStartLocXinPixels;
        public int movementDelayInMiliseconds = 100;
        private long timeStamp = 0;
        private bool finishedMove = true;
        public SKBitmap minimap = null;
        public List<Sprite> spriteList = new List<Sprite>();

        //UI PANELS
        public int buttonPanelLocX = 0;
        public int buttonPanelLocY = 0;
        public int togglePanelLocX = 0;
        public int togglePanelLocY = 0;
        public int portraitPanelLocX = 0;
        public int portraitPanelLocY = 0;
        public int logPanelLocX = 0;
        public int logPanelLocY = 0;
        public int arrowPanelLocX = 0;
        public int arrowPanelLocY = 0;
        //ARROWS PANEL
        public IbbButton btnArrowUp = null;
        public IbbButton btnArrowDown = null;
        public IbbButton btnArrowLeft = null;
        public IbbButton btnArrowRight = null;
        public IbbButton btnArrowWait = null;
        //BUTTONS PANEL        
        public IbbToggle tglPortraits = null;
        public IbbButton btnParty = null;
        public IbbButton btnInventory = null;
        public IbbButton btnJournal = null;
        //public IbbButton btnUseTraitOnMainMap = null;
        public IbbButton btnCastOnMainMap = null;
        public IbbButton btnSave = null;
        public IbbToggle tglSettings = null;
        //TOGGLES PANEL
        public IbbToggle tglMiniMap = null;
        public IbbToggle tglFullParty = null;
        public IbbToggle tglGrid = null;
        public IbbToggle tglClock = null;
        public IbbToggle tglDebugMode = null;
        //PORTRAITS PANEL
        public IbbPortrait btnPort0 = null;
        public IbbPortrait btnPort1 = null;
        public IbbPortrait btnPort2 = null;
        public IbbPortrait btnPort3 = null;
        public IbbPortrait btnPort4 = null;
        public IbbPortrait btnPort5 = null;


        public ScreenMainMap(Module m, GameView g)
        {
            //gv.mod = m;
            gv = g;
            mapStartLocXinPixels = 1 * gv.uiSquareSize;
            loadMainUILayout();
            floatyTextBox = new IBminiTextBox(gv);
            floatyTextBox.showShadow = true;
        }
        public void loadMainUILayout()
        {
            try
            {
                //mainUiLayout = new IB2UILayout(gv);
                createLogPanel();
                createButtonsPanel();
                createTogglesPanel();
                createPortraitsPanel();
                createArrowsPanel();
                //mainUiLayout.setupIB2UILayout(gv);
                                
                showMiniMap = tglMiniMap.toggleOn;                
                showClock = tglClock.toggleOn;
                showFullParty = tglFullParty.toggleOn;
                
                float sqrW = (float)gv.screenWidth / (gv.squaresInWidth);
                float sqrH = (float)gv.screenHeight / (gv.squaresInHeight);
                gv.cc.addLogText("red", "scaler: " + gv.scaler);
                gv.cc.addLogText("fuchsia", "screenWidth: " + gv.screenWidth);
                gv.cc.addLogText("lime", "screenHeight: " + gv.screenHeight);
                gv.cc.addLogText("yellow", "squareSize: " + gv.squareSize);
                gv.cc.addLogText("yellow", "sqrW: " + sqrW);
                gv.cc.addLogText("yellow", "sqrH: " + sqrH);
                gv.cc.addLogText("yellow", "fontWidth: " + gv.fontWidth);
                gv.cc.addLogText("yellow", "");
                gv.cc.addLogText("red", "Welcome to " + gv.mod.moduleLabelName);
                gv.cc.addLogText("fuchsia", "Swipe up/down to scroll this message log box");
                
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error Loading MainUILayout.json: " + ex.ToString());
                //gv.errorLog(ex.ToString());
            }
        }        
        public void createLogPanel()
        {            
            /*
            //create log panel
            IB2Panel newPanel = new IB2Panel(gv);
            newPanel.tag = "logPanel";
            newPanel.backgroundImageFilename = "ui_bg_log";
            newPanel.shownLocX = 272 + gv.oXshift / gv.scaler;
            newPanel.shownLocY = 0;
            newPanel.Width = 102;
            newPanel.Height = 136;
            
            IB2HtmlLogBox newLog = gv.log;
            newLog.tbXloc = 2;
            newLog.tbYloc = 2;
            newLog.pnlLocX = 272;
            newLog.pnlLocY = 0;
            newLog.tbWidth = 98 + gv.fontWidth; //add one char because the word wrap calculates word length plus one space at end
            newLog.tbHeight = 132;
            newLog.numberOfLinesToShow = 14;
            newPanel.logList.Add(newLog);
            mainUiLayout.panelList.Add(newPanel);
            */
        }
        public void createButtonsPanel()
        {
            buttonPanelLocX = -gv.oXshift / 2 + (gv.pS * gv.scaler);
            buttonPanelLocY = 0;

            if (tglPortraits == null)
            {
                tglPortraits = new IbbToggle(gv);
            }
            tglPortraits.ImgOn = "tgl_portrait_on";
            tglPortraits.ImgOff = "tgl_portrait_off";
            tglPortraits.X = buttonPanelLocX + 0 * gv.uiSquareSize;
            tglPortraits.Y = buttonPanelLocY + 0 * gv.uiSquareSize;
            tglPortraits.Height = (int)(gv.ibbheight * gv.scaler);
            tglPortraits.Width = (int)(gv.ibbwidthR * gv.scaler);
            tglPortraits.toggleOn = gv.toggleSettings.showPortraitPanel;
            showPortraitPanel = gv.toggleSettings.showPortraitPanel;

            if (btnParty == null)
            {
                btnParty = new IbbButton(gv, 0.8f);
            }
            btnParty.Img = "btn_small";
            btnParty.Img2 = "btnparty";
            btnParty.Glow = "btn_small_glow";
            btnParty.HotKey = "P";
            btnParty.X = buttonPanelLocX + 0 * gv.uiSquareSize;
            btnParty.Y = buttonPanelLocY + 1 * gv.uiSquareSize;
            btnParty.Height = (int)(gv.ibbheight * gv.scaler);
            btnParty.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnInventory == null)
            {
                btnInventory = new IbbButton(gv, 0.8f);
            }
            btnInventory.Img = "btn_small";
            btnInventory.Img2 = "btninventory";
            btnInventory.Glow = "btn_small_glow";
            btnInventory.HotKey = "I";
            btnInventory.X = buttonPanelLocX + 0 * gv.uiSquareSize;
            btnInventory.Y = buttonPanelLocY + 2 * gv.uiSquareSize;
            btnInventory.Height = (int)(gv.ibbheight * gv.scaler);
            btnInventory.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnJournal == null)
            {
                btnJournal = new IbbButton(gv, 0.8f);
            }
            btnJournal.Img = "btn_small";
            btnJournal.Img2 = "btnjournal";
            btnJournal.Glow = "btn_small_glow";
            btnJournal.HotKey = "J";
            btnJournal.X = buttonPanelLocX + 0 * gv.uiSquareSize;
            btnJournal.Y = buttonPanelLocY + 3 * gv.uiSquareSize;
            btnJournal.Height = (int)(gv.ibbheight * gv.scaler);
            btnJournal.Width = (int)(gv.ibbwidthR * gv.scaler);

            /*if (btnUseTraitOnMainMap == null)
            {
                btnUseTraitOnMainMap = new IbbButton(gv, 0.8f);
            }
            btnUseTraitOnMainMap.Img = "btn_small";
            btnUseTraitOnMainMap.Img2 = "btntrait";
            btnUseTraitOnMainMap.Glow = "btn_small_glow";
            btnUseTraitOnMainMap.HotKey = "";
            btnUseTraitOnMainMap.X = buttonPanelLocX + 0 * gv.uiSquareSize;
            btnUseTraitOnMainMap.Y = buttonPanelLocY + 3 * gv.uiSquareSize;
            btnUseTraitOnMainMap.Height = (int)(gv.ibbheight * gv.scaler);
            btnUseTraitOnMainMap.Width = (int)(gv.ibbwidthR * gv.scaler);*/

            if (btnCastOnMainMap == null)
            {
                btnCastOnMainMap = new IbbButton(gv, 0.8f);
            }
            btnCastOnMainMap.Img = "btn_small";
            btnCastOnMainMap.Img2 = "btnspell";
            btnCastOnMainMap.Glow = "btn_small_glow";
            btnCastOnMainMap.HotKey = "C";
            btnCastOnMainMap.X = buttonPanelLocX + 0 * gv.uiSquareSize;
            btnCastOnMainMap.Y = buttonPanelLocY + 4 * gv.uiSquareSize;
            btnCastOnMainMap.Height = (int)(gv.ibbheight * gv.scaler);
            btnCastOnMainMap.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnSave == null)
            {
                btnSave = new IbbButton(gv, 0.8f);
            }
            btnSave.Img = "btn_small";
            btnSave.Img2 = "btndisk";
            btnSave.Glow = "btn_small_glow";
            btnSave.HotKey = "";
            btnSave.X = buttonPanelLocX + 0 * gv.uiSquareSize;
            btnSave.Y = buttonPanelLocY + 5 * gv.uiSquareSize;
            btnSave.Height = (int)(gv.ibbheight * gv.scaler);
            btnSave.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (tglSettings == null)
            {
                tglSettings = new IbbToggle(gv);
            }
            tglSettings.ImgOn = "tgl_toggles_on";
            tglSettings.ImgOff = "tgl_toggles_off";
            tglSettings.X = buttonPanelLocX + 0 * gv.uiSquareSize;
            tglSettings.Y = buttonPanelLocY + 6 * gv.uiSquareSize;
            tglSettings.Height = (int)(gv.ibbheight * gv.scaler);
            tglSettings.Width = (int)(gv.ibbwidthR * gv.scaler);
            tglSettings.toggleOn = gv.toggleSettings.showTogglePanel;
            showTogglePanel = gv.toggleSettings.showTogglePanel;



            /*
            //create buttons panel
            IB2Panel newPanel = new IB2Panel(gv);
            newPanel.tag = "BottomPanel";
            newPanel.backgroundImageFilename = "none";
            newPanel.shownLocX = 0 - gv.oXshift / gv.scaler + (gv.pS * gv.scaler);
            newPanel.shownLocY = 0;
            newPanel.Width = 34;
            newPanel.Height = 238;

            //toggle
            IB2ToggleButton newToggle = new IB2ToggleButton(gv);
            newToggle.tag = "tglMiniMap";
            newToggle.ImgOnFilename = "tgl_minimap_on";
            newToggle.ImgOffFilename = "tgl_minimap_off";
            newToggle.toggleOn = gv.toggleSettings.showMiniMap;
            showMiniMap = gv.toggleSettings.showMiniMap;
            newToggle.X = 0;
            newToggle.Y = 0;
            newToggle.Width = 34;
            newToggle.Height = 34;
            newToggle.show = true;
            newPanel.toggleList.Add(newToggle);

            //button
            IB2Button newButton = new IB2Button(gv);
            newButton.tag = "btnParty";
            newButton.ImgFilename = "btn_small";
            newButton.ImgOffFilename = "btn_small_off";
            newButton.ImgOnFilename = "btn_small_on";
            newButton.Img2Filename = "btnparty";
            newButton.Img2OffFilename = "";
            newButton.Img3Filename = "";
            newButton.GlowFilename = "btn_small_glow";
            newButton.btnState = buttonState.Normal;
            newButton.btnNotificationOn = false;
            newButton.glowOn = false;
            newButton.Text = "";
            newButton.Quantity = "";
            newButton.HotKey = "P";
            newButton.X = 0;
            newButton.Y = 34;
            newButton.IBScript = "none";
            newButton.Width = 34;
            newButton.Height = 34;
            newButton.scaler = 1.0f;
            newButton.show = true;
            newPanel.buttonList.Add(newButton);

            //button
            newButton = new IB2Button(gv);
            newButton.tag = "btnInventory";
            newButton.ImgFilename = "btn_small";
            newButton.ImgOffFilename = "btn_small_off";
            newButton.ImgOnFilename = "btn_small_on";
            newButton.Img2Filename = "btninventory";
            newButton.Img2OffFilename = "";
            newButton.Img3Filename = "";
            newButton.GlowFilename = "btn_small_glow";
            newButton.btnState = buttonState.Normal;
            newButton.btnNotificationOn = false;
            newButton.glowOn = false;
            newButton.Text = "";
            newButton.Quantity = "";
            newButton.HotKey = "I";
            newButton.X = 0;
            newButton.Y = 68;
            newButton.IBScript = "none";
            newButton.Width = 34;
            newButton.Height = 34;
            newButton.scaler = 1.0f;
            newButton.show = true;
            newPanel.buttonList.Add(newButton);

            //button
            newButton = new IB2Button(gv);
            newButton.tag = "btnJournal";
            newButton.ImgFilename = "btn_small";
            newButton.ImgOffFilename = "btn_small_off";
            newButton.ImgOnFilename = "btn_small_on";
            newButton.Img2Filename = "btnjournal";
            newButton.Img2OffFilename = "";
            newButton.Img3Filename = "";
            newButton.GlowFilename = "btn_small_glow";
            newButton.btnState = buttonState.Normal;
            newButton.btnNotificationOn = false;
            newButton.glowOn = false;
            newButton.Text = "";
            newButton.Quantity = "";
            newButton.HotKey = "J";
            newButton.X = 0;
            newButton.Y = 102;
            newButton.IBScript = "none";
            newButton.Width = 34;
            newButton.Height = 34;
            newButton.scaler = 1.0f;
            newButton.show = true;
            newPanel.buttonList.Add(newButton);

            //button
            newButton = new IB2Button(gv);
            newButton.tag = "btnCastOnMainMap";
            newButton.ImgFilename = "btn_small";
            newButton.ImgOffFilename = "btn_small_off";
            newButton.ImgOnFilename = "btn_small_on";
            newButton.Img2Filename = "btnspell";
            newButton.Img2OffFilename = "";
            newButton.Img3Filename = "";
            newButton.GlowFilename = "btn_small_glow";
            newButton.btnState = buttonState.Normal;
            newButton.btnNotificationOn = false;
            newButton.glowOn = false;
            newButton.Text = "";
            newButton.Quantity = "";
            newButton.HotKey = "C";
            newButton.X = 0;
            newButton.Y = 136;
            newButton.IBScript = "none";
            newButton.Width = 34;
            newButton.Height = 34;
            newButton.scaler = 1.0f;
            newButton.show = true;
            newPanel.buttonList.Add(newButton);

            //button
            newButton = new IB2Button(gv);
            newButton.tag = "btnSave";
            newButton.ImgFilename = "btn_small";
            newButton.ImgOffFilename = "btn_small_off";
            newButton.ImgOnFilename = "btn_small_on";
            newButton.Img2Filename = "btndisk";
            newButton.Img2OffFilename = "";
            newButton.Img3Filename = "";
            newButton.GlowFilename = "btn_small_glow";
            newButton.btnState = buttonState.Normal;
            newButton.btnNotificationOn = false;
            newButton.glowOn = false;
            newButton.Text = "";
            newButton.Quantity = "";
            newButton.HotKey = "";
            newButton.X = 0;
            newButton.Y = 170;
            newButton.IBScript = "none";
            newButton.Width = 34;
            newButton.Height = 34;
            newButton.scaler = 1.0f;
            newButton.show = true;
            newPanel.buttonList.Add(newButton);
                        
            //button
            newButton = new IB2Button(gv);
            newButton.tag = "btnSettings";
            newButton.ImgFilename = "btn_small";
            newButton.ImgOffFilename = "btn_small_off";
            newButton.ImgOnFilename = "btn_small_on";
            newButton.Img2Filename = "btnsettings";
            newButton.Img2OffFilename = "";
            newButton.Img3Filename = "";
            newButton.GlowFilename = "btn_small_glow";
            newButton.btnState = buttonState.Normal;
            newButton.btnNotificationOn = false;
            newButton.glowOn = false;
            newButton.Text = "";
            newButton.Quantity = "";
            newButton.HotKey = "";
            newButton.X = 0;
            newButton.Y = 204;
            newButton.IBScript = "none";
            newButton.Width = 34;
            newButton.Height = 34;
            newButton.scaler = 1.0f;
            newButton.show = true;
            newPanel.buttonList.Add(newButton);

            mainUiLayout.panelList.Add(newPanel);
            */
        }
        public void createTogglesPanel()
        {
            togglePanelLocX = (1 * gv.uiSquareSize) + (gv.uiSquareSize / 2);
            if (showTogglePanel)
            {
                togglePanelLocY = (6 * gv.uiSquareSize) + gv.oYshift;
            }
            else
            {
                togglePanelLocY = (8 * gv.uiSquareSize) + gv.oYshift;
            }

            if (tglMiniMap == null)
            {
                tglMiniMap = new IbbToggle(gv);
            }
            tglMiniMap.ImgOff = "tgl_minimap_off";
            tglMiniMap.ImgOn = "tgl_minimap_on";
            tglMiniMap.X = togglePanelLocX + 0 * gv.uiSquareSize;
            tglMiniMap.Y = togglePanelLocY + 0 * gv.uiSquareSize;
            tglMiniMap.Height = (int)(gv.ibbheight * gv.scaler);
            tglMiniMap.Width = (int)(gv.ibbwidthR * gv.scaler);
            tglMiniMap.toggleOn = gv.toggleSettings.showMiniMap;
            showMiniMap = gv.toggleSettings.showMiniMap;
                        

            if (tglFullParty == null)
            {
                tglFullParty = new IbbToggle(gv);
            }
            tglFullParty.ImgOn = "tgl_fullparty_on";
            tglFullParty.ImgOff = "tgl_fullparty_off";
            tglFullParty.X = togglePanelLocX + 1 * gv.uiSquareSize;
            tglFullParty.Y = togglePanelLocY + 0 * gv.uiSquareSize;
            tglFullParty.Height = (int)(gv.ibbheight * gv.scaler);
            tglFullParty.Width = (int)(gv.ibbwidthR * gv.scaler);
            tglFullParty.toggleOn = gv.toggleSettings.showFullParty;
            showFullParty = gv.toggleSettings.showFullParty;

            if (tglGrid == null)
            {
                tglGrid = new IbbToggle(gv);
            }
            tglGrid.ImgOn = "tgl_grid_on";
            tglGrid.ImgOff = "tgl_grid_off";
            tglGrid.X = togglePanelLocX + 2 * gv.uiSquareSize;
            tglGrid.Y = togglePanelLocY + 0 * gv.uiSquareSize;
            tglGrid.Height = (int)(gv.ibbheight * gv.scaler);
            tglGrid.Width = (int)(gv.ibbwidthR * gv.scaler);
            tglGrid.toggleOn = gv.toggleSettings.map_showGrid;
            gv.mod.map_showGrid = gv.toggleSettings.map_showGrid;

            if (tglClock == null)
            {
                tglClock = new IbbToggle(gv);
            }
            tglClock.ImgOn = "tgl_clock_on";
            tglClock.ImgOff = "tgl_clock_off";
            tglClock.X = togglePanelLocX + 3 * gv.uiSquareSize;
            tglClock.Y = togglePanelLocY + 0 * gv.uiSquareSize;
            tglClock.Height = (int)(gv.ibbheight * gv.scaler);
            tglClock.Width = (int)(gv.ibbwidthR * gv.scaler);
            tglClock.toggleOn = gv.toggleSettings.showClock;
            showClock = gv.toggleSettings.showClock;

            if (tglDebugMode == null)
            {
                tglDebugMode = new IbbToggle(gv);
            }
            tglDebugMode.ImgOn = "tgl_debugmode_on";
            tglDebugMode.ImgOff = "tgl_debugmode_off";
            tglDebugMode.X = togglePanelLocX + 4 * gv.uiSquareSize;
            tglDebugMode.Y = togglePanelLocY + 0 * gv.uiSquareSize;
            tglDebugMode.Height = (int)(gv.ibbheight * gv.scaler);
            tglDebugMode.Width = (int)(gv.ibbwidthR * gv.scaler);
            tglDebugMode.toggleOn = gv.toggleSettings.debugMode;
            gv.mod.debugMode = gv.toggleSettings.debugMode;


            /*
            //create buttons panel
            IB2Panel newPanel = new IB2Panel(gv);
            newPanel.tag = "TogglePanel";
            newPanel.backgroundImageFilename = "none";
            newPanel.shownLocX = 51;
            newPanel.shownLocY = 204 + gv.oYshift / gv.scaler;
            newPanel.hiddenLocX = 51;
            newPanel.hiddenLocY = 272 + gv.oYshift / gv.scaler;
            newPanel.hidingXIncrement = 0;
            newPanel.hidingYIncrement = 3;
            newPanel.Width = 238;
            newPanel.Height = 34;
            showTogglePanel = gv.toggleSettings.showTogglePanel;
            if (gv.toggleSettings.showTogglePanel)
            {
                newPanel.currentLocX = 51;
                newPanel.currentLocY = 204 + gv.oYshift / gv.scaler;
                newPanel.showing = true;
            }
            else
            {
                newPanel.currentLocX = 51;
                newPanel.currentLocY = 272 + gv.oYshift / gv.scaler;
                newPanel.hiding = true;
            }

            //toggle   
            IB2ToggleButton newToggle = new IB2ToggleButton(gv);
            newToggle.tag = "tglPortraits";
            newToggle.ImgOnFilename = "tgl_fullparty_on";
            newToggle.ImgOffFilename = "tgl_fullparty_off";
            newToggle.toggleOn = gv.toggleSettings.showPortraitPanel;
            showPortraitPanel = gv.toggleSettings.showPortraitPanel;
            newToggle.X = 0;
            newToggle.Y = 0;
            newToggle.Width = 34;
            newToggle.Height = 34;
            newToggle.show = true;
            newPanel.toggleList.Add(newToggle);

            //toggle   
            newToggle = new IB2ToggleButton(gv);
            newToggle.tag = "tglFullParty";
            newToggle.ImgOnFilename = "tgl_fullparty_on";
            newToggle.ImgOffFilename = "tgl_fullparty_off";
            newToggle.toggleOn = gv.toggleSettings.showFullParty;
            showFullParty = gv.toggleSettings.showFullParty;
            newToggle.X = 34;
            newToggle.Y = 0;
            newToggle.Width = 34;
            newToggle.Height = 34;
            newToggle.show = true;
            newPanel.toggleList.Add(newToggle);

            //toggle
            newToggle = new IB2ToggleButton(gv);
            newToggle.tag = "tglGrid";
            newToggle.ImgOnFilename = "tgl_grid_on";
            newToggle.ImgOffFilename = "tgl_grid_off";
            newToggle.toggleOn = gv.toggleSettings.map_showGrid;
            gv.mod.map_showGrid = gv.toggleSettings.map_showGrid;
            newToggle.X = 68;
            newToggle.Y = 0;
            newToggle.Width = 34;
            newToggle.Height = 34;
            newToggle.show = true;
            newPanel.toggleList.Add(newToggle);

            //toggle
            newToggle = new IB2ToggleButton(gv);
            newToggle.tag = "tglClock";
            newToggle.ImgOnFilename = "tgl_clock_on";
            newToggle.ImgOffFilename = "tgl_clock_off";
            newToggle.toggleOn = gv.toggleSettings.showClock;
            showClock = gv.toggleSettings.showClock;
            newToggle.X = 102;
            newToggle.Y = 0;
            newToggle.Width = 34;
            newToggle.Height = 34;
            newToggle.show = true;
            newPanel.toggleList.Add(newToggle);
                        
            //toggle
            newToggle = new IB2ToggleButton(gv);
            newToggle.tag = "tglDebugMode";
            newToggle.ImgOnFilename = "tgl_debugmode_on";
            newToggle.ImgOffFilename = "tgl_debugmode_off";
            newToggle.toggleOn = gv.toggleSettings.debugMode;
            gv.mod.debugMode = gv.toggleSettings.debugMode;
            newToggle.X = 136;
            newToggle.Y = 0;
            newToggle.Width = 34;
            newToggle.Height = 34;
            newToggle.show = true;
            newPanel.toggleList.Add(newToggle);

            mainUiLayout.panelList.Add(newPanel);
            */
        }
        public void createPortraitsPanel()
        {
            portraitPanelLocX = (1 * gv.uiSquareSize) + (gv.uiSquareSize / 2);
            if (showPortraitPanel)
            {
                portraitPanelLocY = (0 * gv.uiSquareSize) - gv.oYshift + (2 * gv.scaler);
            }
            else
            {
                portraitPanelLocY = (-2 * gv.uiSquareSize) - gv.oYshift + (2 * gv.scaler);
            }

            if (btnPort0 == null)
            {
                btnPort0 = new IbbPortrait(gv, 0.8f);
            }
            btnPort0.ImgBG = "btn_small";
            //btnPort0.Img = "ptr_adela";
            btnPort0.Glow = "btn_small_glow";
            btnPort0.X = portraitPanelLocX + 0 * gv.uiSquareSize;
            btnPort0.Y = portraitPanelLocY + 0 * gv.uiSquareSize;
            btnPort0.Height = (int)(gv.ibpheight * gv.scaler);
            btnPort0.Width = (int)(gv.ibpwidth * gv.scaler);

            if (btnPort1 == null)
            {
                btnPort1 = new IbbPortrait(gv, 0.8f);
            }
            btnPort1.ImgBG = "btn_small";
            //btnPort1.Img = "ptr_adela";
            btnPort1.Glow = "btn_small_glow";
            btnPort1.X = portraitPanelLocX + 1 * gv.uiSquareSize;
            btnPort1.Y = portraitPanelLocY + 0 * gv.uiSquareSize;
            btnPort1.Height = (int)(gv.ibpheight * gv.scaler);
            btnPort1.Width = (int)(gv.ibpwidth * gv.scaler);

            if (btnPort2 == null)
            {
                btnPort2 = new IbbPortrait(gv, 0.8f);
            }
            btnPort2.ImgBG = "btn_small";
            //btnPort2.Img = "ptr_adela";
            btnPort2.Glow = "btn_small_glow";
            btnPort2.X = portraitPanelLocX + 2 * gv.uiSquareSize;
            btnPort2.Y = portraitPanelLocY + 0 * gv.uiSquareSize;
            btnPort2.Height = (int)(gv.ibpheight * gv.scaler);
            btnPort2.Width = (int)(gv.ibpwidth * gv.scaler);

            if (btnPort3 == null)
            {
                btnPort3 = new IbbPortrait(gv, 0.8f);
            }
            btnPort3.ImgBG = "btn_small";
            //btnPort3.Img = "ptr_adela";
            btnPort3.Glow = "btn_small_glow";
            btnPort3.X = portraitPanelLocX + 3 * gv.uiSquareSize;
            btnPort3.Y = portraitPanelLocY + 0 * gv.uiSquareSize;
            btnPort3.Height = (int)(gv.ibpheight * gv.scaler);
            btnPort3.Width = (int)(gv.ibpwidth * gv.scaler);

            if (btnPort4 == null)
            {
                btnPort4 = new IbbPortrait(gv, 0.8f);
            }
            btnPort4.ImgBG = "btn_small";
            //btnPort4.Img = "ptr_adela";
            btnPort4.Glow = "btn_small_glow";
            btnPort4.X = portraitPanelLocX + 4 * gv.uiSquareSize;
            btnPort4.Y = portraitPanelLocY + 0 * gv.uiSquareSize;
            btnPort4.Height = (int)(gv.ibpheight * gv.scaler);
            btnPort4.Width = (int)(gv.ibpwidth * gv.scaler);

            if (btnPort5 == null)
            {
                btnPort5 = new IbbPortrait(gv, 0.8f);
            }
            btnPort5.ImgBG = "btn_small";
            //btnPort5.Img = "ptr_adela";
            btnPort5.Glow = "btn_small_glow";
            btnPort5.X = portraitPanelLocX + 5 * gv.uiSquareSize;
            btnPort5.Y = portraitPanelLocY + 0 * gv.uiSquareSize;
            btnPort5.Height = (int)(gv.ibpheight * gv.scaler);
            btnPort5.Width = (int)(gv.ibpwidth * gv.scaler);

            
        }
        public void createArrowsPanel()
        {
            arrowPanelLocX = (8 * gv.uiSquareSize) + gv.oXshift / 2 + (8 * gv.scaler);
            //arrowPanelLocX = (8 * gv.uiSquareSize);
            arrowPanelLocY = (4 * gv.uiSquareSize);
                        
            if (btnArrowUp == null)
            {
                btnArrowUp = new IbbButton(gv, 0.8f);
            }
            btnArrowUp.Img = "btn_small";
            btnArrowUp.Img2 = "ctrl_up_arrow";
            btnArrowUp.Glow = "btn_small_glow";
            btnArrowUp.X = arrowPanelLocX + 1 * gv.uiSquareSize;
            btnArrowUp.Y = arrowPanelLocY + 0 * gv.uiSquareSize;
            btnArrowUp.Height = (int)(gv.ibbheight * gv.scaler);
            btnArrowUp.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnArrowDown == null)
            {
                btnArrowDown = new IbbButton(gv, 0.8f);
            }
            btnArrowDown.Img = "btn_small";
            btnArrowDown.Img2 = "ctrl_down_arrow";
            btnArrowDown.Glow = "btn_small_glow";
            btnArrowDown.X = arrowPanelLocX + 1 * gv.uiSquareSize;
            btnArrowDown.Y = arrowPanelLocY + 2 * gv.uiSquareSize;
            btnArrowDown.Height = (int)(gv.ibbheight * gv.scaler);
            btnArrowDown.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnArrowLeft == null)
            {
                btnArrowLeft = new IbbButton(gv, 0.8f);
            }
            btnArrowLeft.Img = "btn_small";
            btnArrowLeft.Img2 = "ctrl_left_arrow";
            btnArrowLeft.Glow = "btn_small_glow";
            btnArrowLeft.X = arrowPanelLocX + 0 * gv.uiSquareSize;
            btnArrowLeft.Y = arrowPanelLocY + 1 * gv.uiSquareSize;
            btnArrowLeft.Height = (int)(gv.ibbheight * gv.scaler);
            btnArrowLeft.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnArrowRight == null)
            {
                btnArrowRight = new IbbButton(gv, 0.8f);
            }
            btnArrowRight.Img = "btn_small";
            btnArrowRight.Img2 = "ctrl_right_arrow";
            btnArrowRight.Glow = "btn_small_glow";
            btnArrowRight.X = arrowPanelLocX + 2 * gv.uiSquareSize;
            btnArrowRight.Y = arrowPanelLocY + 1 * gv.uiSquareSize;
            btnArrowRight.Height = (int)(gv.ibbheight * gv.scaler);
            btnArrowRight.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnArrowWait == null)
            {
                btnArrowWait = new IbbButton(gv, 0.8f);
            }
            btnArrowWait.Img = "btn_small";
            btnArrowWait.Img2 = "btnwait";
            btnArrowWait.Glow = "btn_small_glow";
            btnArrowWait.X = arrowPanelLocX + 1 * gv.uiSquareSize;
            btnArrowWait.Y = arrowPanelLocY + 1 * gv.uiSquareSize;
            btnArrowWait.Height = (int)(gv.ibbheight * gv.scaler);
            btnArrowWait.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        
        //MAIN SCREEN UPDATE
        public void Update(int elapsed)
        {
            //mainUiLayout.Update(elapsed);

            #region PROP AMBIENT SPRITES
            foreach (Sprite spr in spriteList)
            {
                spr.Update(elapsed, gv);
            }
            #endregion

            #region FLOATY TEXT            
            if (floatyTextPool.Count > 0)
            {
                int shiftUp = (int)(0.05f * elapsed);
                foreach (FloatyText ft in floatyTextPool)
                {
                    ft.z += shiftUp;
                    ft.timeToLive -= (int)(elapsed);
                }

                //remove expired floaty text
                for (int i = floatyTextPool.Count - 1; i >= 0; i--)
                {
                    if (floatyTextPool[i].timeToLive <= 0)
                    {
                        floatyTextPool.RemoveAt(i);
                    }
                }

                //remove if too many floats are in pool
                for (int i = floatyTextPool.Count - 1; i >= 0; i--)
                {
                    if (((floatyTextPool.Count - 1 - i) > 15))
                    {
                        floatyTextPool.RemoveAt(i);
                    }
                }
            }            
            #endregion
        }
        
        //MAIN SCREEN DRAW
        public void resetMiniMapBitmap()
        {
            int minimapSquareSizeInPixels = 4 * gv.squareSize / gv.mod.currentArea.MapSizeX;
            int drawW = minimapSquareSizeInPixels * gv.mod.currentArea.MapSizeX;
            int drawH = minimapSquareSizeInPixels * gv.mod.currentArea.MapSizeY;
            using (SKBitmap surface = new SKBitmap(drawW, drawH))
            {
                using (SKCanvas device = new SKCanvas(surface))
                {
                    #region Draw Layer 1
                    for (int x = 0; x < gv.mod.currentArea.MapSizeX; x++)
                    {
                        for (int y = 0; y < gv.mod.currentArea.MapSizeY; y++)
                        {
                            string tile = gv.mod.currentArea.Layer1Filename[y * gv.mod.currentArea.MapSizeX + x];
                            SKRect src = new SKRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            float scalerX = gv.cc.GetFromTileBitmapList(tile).Width / gv.tileSizeInPixels;
                            float scalerY = gv.cc.GetFromTileBitmapList(tile).Height / gv.tileSizeInPixels;
                            int brX = (int)(minimapSquareSizeInPixels * scalerX);
                            int brY = (int)(minimapSquareSizeInPixels * scalerY);
                            SKRect dst = new SKRect(x * minimapSquareSizeInPixels, y * minimapSquareSizeInPixels, x * minimapSquareSizeInPixels + brX, y * minimapSquareSizeInPixels + brY);
                            device.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), dst, src);
                        }
                    }
                    #endregion
                    #region Draw Layer 2
                    for (int x = 0; x < gv.mod.currentArea.MapSizeX; x++)
                    {
                        for (int y = 0; y < gv.mod.currentArea.MapSizeY; y++)
                        {
                            string tile = gv.mod.currentArea.Layer2Filename[y * gv.mod.currentArea.MapSizeX + x];
                            SKRect src = new SKRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            float scalerX = gv.cc.GetFromTileBitmapList(tile).Width / gv.tileSizeInPixels;
                            float scalerY = gv.cc.GetFromTileBitmapList(tile).Height / gv.tileSizeInPixels;
                            int brX = (int)(minimapSquareSizeInPixels * scalerX);
                            int brY = (int)(minimapSquareSizeInPixels * scalerY);
                            SKRect dst = new SKRect(x * minimapSquareSizeInPixels, y * minimapSquareSizeInPixels, x * minimapSquareSizeInPixels + brX, y * minimapSquareSizeInPixels + brY);
                            device.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), dst, src);
                        }
                    }
                    #endregion
                    #region Draw Layer 3
                    for (int x = 0; x < gv.mod.currentArea.MapSizeX; x++)
                    {
                        for (int y = 0; y < gv.mod.currentArea.MapSizeY; y++)
                        {
                            string tile = gv.mod.currentArea.Layer3Filename[y * gv.mod.currentArea.MapSizeX + x];
                            SKRect src = new SKRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            float scalerX = gv.cc.GetFromTileBitmapList(tile).Width / gv.tileSizeInPixels;
                            float scalerY = gv.cc.GetFromTileBitmapList(tile).Height / gv.tileSizeInPixels;
                            int brX = (int)(minimapSquareSizeInPixels * scalerX);
                            int brY = (int)(minimapSquareSizeInPixels * scalerY);
                            SKRect dst = new SKRect(x * minimapSquareSizeInPixels, y * minimapSquareSizeInPixels, x * minimapSquareSizeInPixels + brX, y * minimapSquareSizeInPixels + brY);
                            device.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), dst, src);
                        }
                    }
                    #endregion      
                    minimap = surface.Copy();
                }
            }
        }        
        public void redrawMain()
        {
            hideTriggerImageIfNotEnabled();

            if (gv.mod.currentArea.Layer1Filename.Count == 0)
            {
                return;
            }

            if (gv.mod.currentArea.Is3dArea)
            {
                draw3dView();
            }
            else
            {
                setExplored();
                if (!gv.mod.currentArea.areaDark)
                {
                    drawWorldMap();
                    //drawProps();
                    if (gv.mod.map_showGrid)
                    {
                        drawGrid();
                    }
                }
                drawPlayer();

                if (!gv.mod.currentArea.areaDark)
                {
                    if (gv.mod.currentArea.UseDayNightCycle)
                    {
                        drawOverlayTints();
                    }
                    drawFogOfWar();
                }
            }

            if ((showClock) && (!hideClock))
            {
                drawMainMapClockText();
            }
            drawUiLayout();
            drawMiniMap();

            drawFloatyTextPool();
            drawMainMapFloatyText();

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }
        public void draw3dView()
        {
            map3DViewStartLocXinPixels = 2 * gv.squareSize - 10;
            int scaler = gv.scaler * 2;

            //draw backdrop
            int frameX = GetFrameX();
            int frameY = GetFrameY();
            //draw near band or current square (0,0) to (88,88)
            string backdrop = gv.mod.currentArea.Layer1Filename[gv.mod.PlayerLocationY * gv.mod.currentArea.MapSizeX + gv.mod.PlayerLocationX];
            IbRect src = new IbRect(0, 0, 88, 88);
            if (gv.cc.GetFromTileBitmapList(backdrop).Width == 352)
            {
                if (gv.cc.GetFromTileBitmapList(backdrop).Height == 352)
                {
                    //has direction only
                    src = new IbRect(0 + frameX * 88, 0 + frameY * 88, 88, 88);
                }
                else
                {
                    //has direction and day/night
                    src = new IbRect(0 + frameX * 88, 0, 88, 88);
                }
            }
            IbRect dst = new IbRect(map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), (1 * gv.squareSize * gv.scaler), 88 * scaler, 88 * scaler);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(backdrop), src, dst);

            if (IsSquareOnMap(0, 1))
            {
                //draw middle band or one square in front (0,16) to (88,72)
                int indexBD = GetSquareIndex(0, 1);
                backdrop = gv.mod.currentArea.Layer1Filename[indexBD];
                src = new IbRect(0, 16, 88, 56);
                if (gv.cc.GetFromTileBitmapList(backdrop).Width == 352)
                {
                    if (gv.cc.GetFromTileBitmapList(backdrop).Height == 352)
                    {
                        //has direction only
                        src = new IbRect(0 + frameX * 88, 16 + frameY * 88, 88, 56);
                    }
                    else
                    {
                        //has direction and day/night
                        src = new IbRect(0 + frameX * 88, 16, 88, 56);
                    }
                }
                dst = new IbRect(map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), (1 * gv.squareSize * gv.scaler) + 16 * scaler, 88 * scaler, 56 * scaler);
                gv.DrawBitmap(gv.cc.GetFromTileBitmapList(backdrop), src, dst);
            }

            if (IsSquareOnMap(0, 2))
            {
                //draw far band or two squares in front (0,32) to (88,56)
                int indexBD = GetSquareIndex(0, 2);
                backdrop = gv.mod.currentArea.Layer1Filename[indexBD];
                src = new IbRect(0, 32, 88, 24);
                if (gv.cc.GetFromTileBitmapList(backdrop).Width == 352)
                {
                    if (gv.cc.GetFromTileBitmapList(backdrop).Height == 352)
                    {
                        //has direction only
                        src = new IbRect(0 + frameX * 88, 32 + frameY * 88, 88, 24);
                    }
                    else
                    {
                        //has direction and day/night
                        src = new IbRect(0 + frameX * 88, 32, 88, 24);
                    }
                }
                dst = new IbRect(map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), (1 * gv.squareSize * gv.scaler) + 32 * scaler, 88 * scaler, 24 * scaler);
                gv.DrawBitmap(gv.cc.GetFromTileBitmapList(backdrop), src, dst);
            }
                        
            //draw far row
            for (int col = -2; col <= 2; col++)
            {
                int row = 3;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw front
                int index = GetSquareIndex(col, row);
                string wallset = gv.mod.currentArea.Layer2Filename[index];
                string overlay = gv.mod.currentArea.Layer3Filename[index];

                try
                {
                    if (col == -2)
                    {
                        int tlX = -4 * scaler;
                        int tlY = 0;
                        int brX = 16 * scaler;
                        int brY = 88 * scaler;
                        src = new IbRect(188, 0, 16, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                    }
                    else if (col == 2)
                    {
                        int tlX = 76 * scaler;
                        int tlY = 0;
                        int brX = 16 * scaler;
                        int brY = 88 * scaler;
                        src = new IbRect(188, 0, 16, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                    }
                    else
                    {
                        int step = 1;
                        if (col == 0) { step = 4; }
                        else if (col == 1) { step = 7; }
                        int tlY = 0;
                        int brX = 16 * scaler;
                        int brY = 88 * scaler;
                        for (int ii = 0; ii < 3; ii++)
                        {
                            if (ii == 1) { src = new IbRect(172, 0, 16, 88); }
                            else { src = new IbRect(188, 0, 16, 88); }
                            int tlX = ((((step + ii) * 8) - 4) * scaler);
                            dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                            if (!wallset.Equals("none"))
                            {
                                gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                            }
                            if (!overlay.Equals("none"))
                            {
                                gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                            }
                        }
                    }
                }
                catch { }
            }

            //draw middle row
            for (int col = -2; col < 0; col++)
            {
                int row = 2;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw front
                int index = GetSquareIndex(col, row);
                string wallset = gv.mod.currentArea.Layer2Filename[index];
                string overlay = gv.mod.currentArea.Layer3Filename[index];
                string trigImage = GetTriggerImageAtSquare(col, row);
                int tlY = 0;
                int tlX = 0;
                int brX = 0;
                int brY = 0;

                try
                {
                    if (col == -2)
                    {
                        //draw front (8)
                        tlX = 0 * scaler;
                        brX = 16 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(144, 0, 16, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                        //draw right side
                        tlX = 8 * scaler;
                        brX = 12 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(160, 0, 12, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                    }
                    else if (col == -1)
                    {
                        //draw front (24)
                        tlX = 0 * scaler;
                        brX = 40 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(120, 0, 40, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                        if (!trigImage.Equals("none"))
                        {
                            src = new IbRect(0, 0, 48, 48);
                            int tlYY = tlY + 40 * scaler;
                            int tlXX = tlX + 14 * scaler;
                            brX = 12 * scaler;
                            brY = 12 * scaler;
                            dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlYY + (1 * gv.squareSize * gv.scaler), brX, brY);
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(trigImage), src, dst);
                        }
                        //draw right side
                        tlX = 32 * scaler;
                        brX = 12 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(160, 0, 12, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                    }
                }
                catch { }
            }
            for (int col = 2; col >= 0; col--)
            {
                int row = 2;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw front
                int index = GetSquareIndex(col, row);
                string wallset = gv.mod.currentArea.Layer2Filename[index];
                string overlay = gv.mod.currentArea.Layer3Filename[index];
                string trigImage = GetTriggerImageAtSquare(col, row);
                int tlY = 0;
                int tlX = 0;
                int brX = 0;
                int brY = 0;

                try
                {
                    if (col == 2)
                    {
                        //draw left side
                        tlX = 68 * scaler;
                        brX = 12 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(160, 0, 12, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst, true);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst, true);
                        }
                        //draw front (8)
                        tlX = 72 * scaler;
                        brX = 16 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(120, 0, 16, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                        if (!trigImage.Equals("none"))
                        {
                            src = new IbRect(0, 0, 8, 48);
                            int tlYY = tlY + 40 * scaler;
                            int tlXX = tlX + 14 * scaler;
                            brX = 2 * scaler;
                            brY = 12 * scaler;
                            dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlYY + (1 * gv.squareSize * gv.scaler), brX, brY);
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(trigImage), src, dst);
                        }
                    }
                    else if (col == 1)
                    {
                        //draw left side
                        tlX = 44 * scaler;
                        brX = 12 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(160, 0, 12, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst, true);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst, true);
                        }
                        //draw front (24)
                        tlX = 48 * scaler;
                        brX = 40 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(120, 0, 40, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                        if (!trigImage.Equals("none"))
                        {
                            src = new IbRect(0, 0, 48, 48);
                            int tlYY = tlY + 40 * scaler;
                            int tlXX = tlX + 14 * scaler;
                            brX = 12 * scaler;
                            brY = 12 * scaler;
                            dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlYY + (1 * gv.squareSize * gv.scaler), brX, brY);
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(trigImage), src, dst);
                        }
                    }
                    else if (col == 0)
                    {
                        //draw front (24)
                        tlX = 24 * scaler;
                        brX = 40 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(120, 0, 40, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                        if (!trigImage.Equals("none"))
                        {
                            src = new IbRect(0, 0, 48, 48);
                            int tlYY = tlY + 40 * scaler;
                            int tlXX = tlX + 14 * scaler;
                            brX = 12 * scaler;
                            brY = 12 * scaler;
                            dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlYY + (1 * gv.squareSize * gv.scaler), brX, brY);
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(trigImage), src, dst);
                        }
                    }
                }
                catch { }
            }
            for (int col = -2; col <= 2; col++)
            {
                int row = 1;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw left
                if (col < 0)
                {
                    //string tile = GetLeftWallTile(col, row);
                    int index = GetSquareIndex(col, row);
                    string wallset = gv.mod.currentArea.Layer2Filename[index];
                    string overlay = gv.mod.currentArea.Layer3Filename[index];
                    string trigImage = GetTriggerImageAtSquare(col, row);
                    int tlX = ((((col + 2) * 24) - 8) * scaler);
                    int tlY = 0;
                    int brX = 24 * scaler;
                    int brY = 88 * scaler;

                    try
                    {
                        src = new IbRect(96, 0, 24, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (col == -2)
                        {
                            src = new IbRect(104, 0, 8, 88);
                            dst = new IbRect(((((col + 2) * 24) - 0) * scaler) + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), (1 * gv.squareSize * gv.scaler), 8 * scaler, 88 * scaler);
                        }
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                    }
                    catch { }
                }
                //draw right
                if (col > 0)
                {
                    int index = GetSquareIndex(col, row);
                    string wallset = gv.mod.currentArea.Layer2Filename[index];
                    string overlay = gv.mod.currentArea.Layer3Filename[index];
                    int tlX = ((((col + 2) * 24) - 24) * scaler);
                    int tlY = 0;
                    int brX = 24 * scaler;
                    int brY = 88 * scaler;

                    try
                    {
                        src = new IbRect(96, 0, 24, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (col == 2)
                        {
                            src = new IbRect(104, 0, 8, 88);
                            dst = new IbRect(((((col + 2) * 24) - 16) * scaler) + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), (1 * gv.squareSize * gv.scaler), 8 * scaler, 88 * scaler);
                        }
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst, true);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst, true);
                        }
                    }
                    catch { }
                }
            }

            //draw near row
            for (int col = -1; col <= 1; col++)
            {
                int row = 1;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw front
                //string tile = GetFrontWallTile(col, row);
                int index = GetSquareIndex(col, row);
                string wallset = gv.mod.currentArea.Layer2Filename[index];
                string overlay = gv.mod.currentArea.Layer3Filename[index];
                string trigImage = GetTriggerImageAtSquare(col, row);
                int tlX = ((((col + 1) * 56) - 48) * scaler);
                int tlY = 0;
                int brX = 72 * scaler;
                int brY = 88 * scaler;

                try
                {
                    src = new IbRect(24, 0, 72, 88);
                    dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                    if (col == -1)
                    {
                        src = new IbRect(72, 0, 24, 88);
                        dst = new IbRect(((((col + 1) * 56) - 0) * scaler) + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), (1 * gv.squareSize * gv.scaler), 24 * scaler, 88 * scaler);
                    }
                    else if (col == 1)
                    {
                        src = new IbRect(24, 0, 24, 88);
                        dst = new IbRect(((((col + 1) * 56) - 48) * scaler) + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), (1 * gv.squareSize * gv.scaler), 24 * scaler, 88 * scaler);
                    }
                    if (!wallset.Equals("none"))
                    {
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                    }
                    if (!overlay.Equals("none"))
                    {
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                    }
                    if (!trigImage.Equals("none"))
                    {
                        int tlYY = tlY + 40 * scaler;
                        int tlXX = tlX + 28 * scaler;
                        brY = 24 * scaler;
                        if (col == -1)
                        {
                            src = new IbRect(0, 0, 48, 48);
                            tlXX = tlX + 56 * scaler;
                            brX = 24 * scaler;
                        }
                        else if (col == 0)
                        {
                            src = new IbRect(0, 0, 48, 48);
                            tlXX = tlX + 28 * scaler;
                            brX = 24 * scaler;
                        }
                        else if (col == 1)
                        {
                            src = new IbRect(0, 0, 48, 48);
                            tlXX = tlX + 0 * scaler;
                            brX = 24 * scaler;
                        }
                        dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlYY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList(trigImage), src, dst);
                    }
                }
                catch { }
            }
            for (int col = -1; col <= 1; col++)
            {
                int row = 0;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw left
                if (col == -1)
                {
                    //string tile = GetLeftWallTile(col, row);
                    int index = GetSquareIndex(col, row);
                    string wallset = gv.mod.currentArea.Layer2Filename[index];
                    string overlay = gv.mod.currentArea.Layer3Filename[index];
                    int tlX = 0;
                    int tlY = 0;
                    int brX = 16 * scaler;
                    int brY = 88 * scaler;

                    try
                    {
                        src = new IbRect(0, 0, 16, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                    }
                    catch { }
                }
                //draw right
                if (col == 1)
                {
                    int index = GetSquareIndex(col, row);
                    string wallset = gv.mod.currentArea.Layer2Filename[index];
                    string overlay = gv.mod.currentArea.Layer3Filename[index];
                    int tlX = 72 * scaler;
                    int tlY = 0;
                    int brX = 16 * scaler;
                    int brY = 88 * scaler;

                    try
                    {
                        src = new IbRect(0, 0, 16, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(wallset), src, dst, true);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(overlay), src, dst, true);
                        }
                    }
                    catch { }
                }
            }
            for (int col = 0; col <= 0; col++)
            {
                int row = 0;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw currently on square Trigger Image
                if (col == 0)
                {
                    //string tile = GetLeftWallTile(col, row);
                    int index = GetSquareIndex(col, row);
                    string trigImage = GetTriggerImageAtSquare(col, row);

                    try
                    {
                        if (!trigImage.Equals("none"))
                        {
                            src = new IbRect(0, 0, 48, 48);
                            int tlXX = 20 * scaler;
                            int tlYY = 40 * scaler;
                            int brX = 48 * scaler;
                            int brY = 48 * scaler;
                            dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlYY + (1 * gv.squareSize * gv.scaler), brX, brY);
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(trigImage), src, dst);
                        }
                    }
                    catch { }
                }
            }
        }
        public int GetFrameX()
        {
            if (gv.mod.PlayerFacingDirection == 0) //NORTH
            {
                return 0;
            }
            else if (gv.mod.PlayerFacingDirection == 1) //EAST
            {
                return 1;
            }
            else if (gv.mod.PlayerFacingDirection == 2) //SOUTH
            {
                return 2;
            }
            else if (gv.mod.PlayerFacingDirection == 3) //WEST
            {
                return 3;
            }
            return 0;
        }
        public int GetFrameY()
        {
            if (!gv.mod.currentArea.UseDayNightCycle)
            {
                return 0;
            }
            int dawn = 5 * 60;
            int sunrise = 6 * 60;
            int day = 7 * 60;
            int sunset = 17 * 60;
            int dusk = 18 * 60;
            int night = 20 * 60;
            int time = gv.mod.WorldTime % 1440;
            if ((time >= dawn) && (time < sunrise))
            {
                //dawn
                return 3;
            }
            else if ((time >= sunrise) && (time < day))
            {
                //sunrise
                return 3;
            }
            else if ((time >= day) && (time < sunset))
            {
                //no tint for day
                return 0;
            }
            else if ((time >= sunset) && (time < dusk))
            {
                //sunset
                return 1;
            }
            else if ((time >= dusk) && (time < night))
            {
                //dusk
                return 1;
            }
            else if ((time >= night) || (time < dawn))
            {
                //night
                return 2;
            }
            return 0;
        }
        public string GetTriggerImageAtSquare(int col, int row)
        {
            string filename = "none";
            foreach (Trigger t in gv.mod.currentArea.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    int x = 0;
                    int y = 0;
                    if (gv.mod.PlayerFacingDirection == 0) //NORTH
                    {
                        //change x and y to this direction
                        x = gv.mod.PlayerLocationX + col;
                        y = gv.mod.PlayerLocationY - row;
                    }
                    else if (gv.mod.PlayerFacingDirection == 1) //EAST
                    {
                        //change x and y to this direction
                        x = gv.mod.PlayerLocationX + row;
                        y = gv.mod.PlayerLocationY + col;
                    }
                    else if (gv.mod.PlayerFacingDirection == 2) //SOUTH
                    {
                        //change x and y to this direction
                        x = gv.mod.PlayerLocationX - col;
                        y = gv.mod.PlayerLocationY + row;
                    }
                    else //WEST
                    {
                        //change x and y to this direction
                        x = gv.mod.PlayerLocationX - row;
                        y = gv.mod.PlayerLocationY - col;
                    }
                    if ((p.X == x) && (p.Y == y))
                    {
                        if (t.isShown)
                        {
                            return t.ImageFileName;
                        }
                    }
                }
            }
            return filename;
        }
        public void hideTriggerImageIfNotEnabled()
        {
            foreach (Trigger t in gv.mod.currentArea.Triggers)
            {
                if ((t.DoOnceOnly) && (t.Enabled))
                {
                    continue;
                }
                else if ((t.DoOnceOnlyEvent1) && (t.EnabledEvent1))
                {
                    continue;
                }
                else if ((t.DoOnceOnlyEvent2) && (t.EnabledEvent2))
                {
                    continue;
                }
                else if ((t.DoOnceOnlyEvent3) && (t.EnabledEvent3))
                {
                    continue;
                }
                else
                {
                    t.isShown = false;
                }
            }
        }
        public int GetSquareIndex(int col, int row)
        {
            if (gv.mod.PlayerFacingDirection == 0) //NORTH
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX + col;
                int y = gv.mod.PlayerLocationY - row;
                return y * gv.mod.currentArea.MapSizeX + x;
            }
            else if (gv.mod.PlayerFacingDirection == 1) //EAST
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX + row;
                int y = gv.mod.PlayerLocationY + col;
                return y * gv.mod.currentArea.MapSizeX + x;
            }
            else if (gv.mod.PlayerFacingDirection == 2) //SOUTH
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX - col;
                int y = gv.mod.PlayerLocationY + row;
                return y * gv.mod.currentArea.MapSizeX + x;
            }
            else //WEST
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX - row;
                int y = gv.mod.PlayerLocationY - col;
                return y * gv.mod.currentArea.MapSizeX + x;
            }
        }
        public void draw3dViewOld()
        {
            mapStartLocXinPixels = 2 * gv.squareSize - 10;
            int scaler = gv.scaler * 2;

            //draw backdrop
            string backdrop = GetWallBackDrop();
            IbRect src = new IbRect(0, 0, 87, 87);
            IbRect dst = new IbRect(mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), (1 * gv.squareSize * gv.scaler), 88 * scaler, 88 * scaler);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(backdrop), src, dst);
            //draw far row
            for (int col = -5; col <= 5; col++)
            {
                int row = 2;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw front
                string tile = GetFrontWallTile(col, row);
                int tlX = ((col + 5) * 8 * scaler);
                int tlY = (5 * 8 * scaler);
                int brX = 8 * scaler;
                int brY = 8 * scaler;

                try
                {
                    src = new IbRect(1, 1, 8, 8);
                    dst = new IbRect(tlX + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                    if (!tile.Equals("none"))
                    {
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                    }
                }
                catch { }                
            }
            for (int col = -4; col <= 4; col++)
            {
                int row = 2;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }                
                //draw left
                if (col <= 0)
                {
                    string tile = GetLeftWallTile(col, row);
                    int tlX = ((((col + 5) * 8) - 8) * scaler);
                    int tlY = (4 * 8 * scaler);
                    int brX = 8 * scaler;
                    int brY = 24 * scaler;

                    try
                    {
                        src = new IbRect(131, 1, 8, 24);
                        dst = new IbRect(tlX + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!tile.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                    }
                    catch { }
                }
                //draw right
                if (col >= 0)
                {
                    string tile = GetRightWallTile(col, row);
                    int tlX = ((((col + 5) * 8) + 8) * scaler);
                    int tlY = (4 * 8 * scaler);
                    int brX = 8 * scaler;
                    int brY = 24 * scaler;

                    try
                    {
                        src = new IbRect(196, 1, 8, 24);
                        dst = new IbRect(tlX + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!tile.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                    }
                    catch { }
                }
            }
            //draw middle row
            for (int col = -2; col <= 2; col++)
            {
                int row = 1;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw front
                string tile = GetFrontWallTile(col, row);
                int tlX = ((((col + 2) * 24) - 16) * scaler);
                int tlY = 32 * scaler;
                int brX = 24 * scaler;
                int brY = 24 * scaler;

                try
                {
                    src = new IbRect(261, 1, 24, 24);
                    dst = new IbRect(tlX + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                    if (col == -2)
                    {
                        src = new IbRect(276, 1, 8, 24);
                        dst = new IbRect(((((col + 2) * 24)) * scaler) + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), 32 * scaler + (1 * gv.squareSize * gv.scaler), 8 * scaler, 24 * scaler);
                    }
                    else if (col == 2)
                    {
                        src = new IbRect(261, 1, 8, 24);
                        dst = new IbRect(((((col + 2) * 24) - 16) * scaler) + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), 32 * scaler + (1 * gv.squareSize * gv.scaler), 8 * scaler, 24 * scaler);
                    }
                    if (!tile.Equals("none"))
                    {
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                    }
                }
                catch { }                
            }
            for (int col = -1; col <= 1; col++)
            {
                int row = 1;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }                
                //draw left
                if (col <= 0)
                {
                    string tile = GetLeftWallTile(col, row);
                    int tlX = ((((col + 2) * 24) - 32) * scaler);
                    int tlY = 16 * scaler;
                    int brX = 16 * scaler;
                    int brY = 56 * scaler;

                    try
                    {
                        src = new IbRect(1, 61, 16, 56);
                        dst = new IbRect(tlX + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (col == -1)
                        {
                            src = new IbRect(9, 61, 8, 56);
                            dst = new IbRect(((((col + 2) * 24) - 24) * scaler) + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), 16 * scaler + (1 * gv.squareSize * gv.scaler), 8 * scaler, 56 * scaler);
                        }
                        if (!tile.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                    }
                    catch { }
                }
                //draw right
                if (col >= 0)
                {
                    string tile = GetRightWallTile(col, row);
                    int tlX = ((((col + 2) * 24) + 8) * scaler);
                    int tlY = 16 * scaler;
                    int brX = 16 * scaler;
                    int brY = 56 * scaler;

                    try
                    {
                        src = new IbRect(66, 61, 16, 56);
                        dst = new IbRect(tlX + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (col == 1)
                        {
                            src = new IbRect(66, 61, 8, 56);
                            dst = new IbRect(((((col + 2) * 24) + 8) * scaler) + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), 16 * scaler + (1 * gv.squareSize * gv.scaler), 8 * scaler, 56 * scaler);
                        }
                        if (!tile.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                    }
                    catch { }
                }
            }
            //draw near row
            for (int col = -1; col <= 1; col++)
            {
                int row = 0;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw front
                string tile = GetFrontWallTile(col, row);
                int tlX = ((((col + 1) * 56) - 40) * scaler);
                int tlY = 16 * scaler;
                int brX = 56 * scaler;
                int brY = 56 * scaler;

                try
                {
                    src = new IbRect(131, 61, 56, 56);
                    dst = new IbRect(tlX + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                    if (col == -1)
                    {
                        src = new IbRect(171, 61, 16, 56);
                        dst = new IbRect(((((col + 1) * 56)) * scaler) + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), 16 * scaler + (1 * gv.squareSize * gv.scaler), 16 * scaler, 56 * scaler);
                    }
                    else if (col == 1)
                    {
                        src = new IbRect(131, 61, 16, 56);
                        dst = new IbRect(((((col + 1) * 56) - 40) * scaler) + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), 16 * scaler + (1 * gv.squareSize * gv.scaler), 16 * scaler, 56 * scaler);
                    }
                    if (!tile.Equals("none"))
                    {
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                    }
                }
                catch { }
            }
            for (int col = -1; col <= 1; col++)
            {
                int row = 0;
                //check if square is on map first
                if (!IsSquareOnMap(col, row))
                {
                    continue;
                }
                //draw left
                if (col == 0)
                {
                    string tile = GetLeftWallTile(col, row);
                    int tlX = 0;
                    int tlY = 0;
                    int brX = 16 * scaler;
                    int brY = 88 * scaler;

                    try
                    {
                        src = new IbRect(196, 61, 16, 88);
                        dst = new IbRect(tlX + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!tile.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                    }
                    catch { }
                }
                //draw right
                if (col == 0)
                {
                    string tile = GetRightWallTile(col, row);
                    int tlX = 72 * scaler;
                    int tlY = 0;
                    int brX = 16 * scaler;
                    int brY = 88 * scaler;

                    try
                    {
                        src = new IbRect(261, 61, 16, 88);
                        dst = new IbRect(tlX + mapStartLocXinPixels + (2 * gv.squareSize * gv.scaler), tlY + (1 * gv.squareSize * gv.scaler), brX, brY);
                        if (!tile.Equals("none"))
                        {
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                    }
                    catch { }
                }
            }
        }
        public string GetFrontWallTile(int col, int row)
        {
            if (gv.mod.PlayerFacingDirection == 0) //NORTH
            {
                if (!IsSquareOnMap(col, row))
                {
                    return "none";
                }
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX + col;
                int y = gv.mod.PlayerLocationY - row;
                //return gv.mod.currentArea.NorthWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }
            else if (gv.mod.PlayerFacingDirection == 1) //EAST
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX + row;
                int y = gv.mod.PlayerLocationY + col;
                //return gv.mod.currentArea.EastWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }
            else if (gv.mod.PlayerFacingDirection == 2) //SOUTH
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX - col;
                int y = gv.mod.PlayerLocationY + row;
                //return gv.mod.currentArea.SouthWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }
            else if (gv.mod.PlayerFacingDirection == 3) //WEST
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX - row;
                int y = gv.mod.PlayerLocationY - col;
                //return gv.mod.currentArea.WestWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }

            return "none";
        }
        public string GetLeftWallTile(int col, int row)
        {
            if (gv.mod.PlayerFacingDirection == 0) //NORTH
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX + col;
                int y = gv.mod.PlayerLocationY - row;
                //return gv.mod.currentArea.WestWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }
            else if (gv.mod.PlayerFacingDirection == 1) //EAST
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX + row;
                int y = gv.mod.PlayerLocationY + col;
                //return gv.mod.currentArea.NorthWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }
            else if (gv.mod.PlayerFacingDirection == 2) //SOUTH
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX - col;
                int y = gv.mod.PlayerLocationY + row;
                //return gv.mod.currentArea.EastWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }
            else if (gv.mod.PlayerFacingDirection == 3) //WEST
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX - row;
                int y = gv.mod.PlayerLocationY - col;
                //return gv.mod.currentArea.SouthWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }

            return "none";
        }
        public string GetRightWallTile(int col, int row)
        {
            if (gv.mod.PlayerFacingDirection == 0) //NORTH
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX + col;
                int y = gv.mod.PlayerLocationY - row;
                //return gv.mod.currentArea.EastWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }
            else if (gv.mod.PlayerFacingDirection == 1) //EAST
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX + row;
                int y = gv.mod.PlayerLocationY + col;
                //return gv.mod.currentArea.SouthWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }
            else if (gv.mod.PlayerFacingDirection == 2) //SOUTH
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX - col;
                int y = gv.mod.PlayerLocationY + row;
                //return gv.mod.currentArea.WestWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }
            else if (gv.mod.PlayerFacingDirection == 3) //WEST
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX - row;
                int y = gv.mod.PlayerLocationY - col;
                //return gv.mod.currentArea.NorthWallTile[y * gv.mod.currentArea.MapSizeX + x];
            }

            return "none";
        }
        public string GetWallBackDrop()
        {
            if (gv.mod.PlayerFacingDirection == 0) //NORTH
            {
                //return gv.mod.currentArea.NorthWallBackDrop[gv.mod.PlayerLocationY * gv.mod.currentArea.MapSizeX + gv.mod.PlayerLocationX];
            }
            else if (gv.mod.PlayerFacingDirection == 1) //EAST
            {
                //return gv.mod.currentArea.EastWallBackDrop[gv.mod.PlayerLocationY * gv.mod.currentArea.MapSizeX + gv.mod.PlayerLocationX];
            }
            else if (gv.mod.PlayerFacingDirection == 2) //SOUTH
            {
                //return gv.mod.currentArea.SouthWallBackDrop[gv.mod.PlayerLocationY * gv.mod.currentArea.MapSizeX + gv.mod.PlayerLocationX];
            }
            else if (gv.mod.PlayerFacingDirection == 3) //WEST
            {
                //return gv.mod.currentArea.WestWallBackDrop[gv.mod.PlayerLocationY * gv.mod.currentArea.MapSizeX + gv.mod.PlayerLocationX];
            }
            return "none";
        }
        public void drawWorldMap()
        {
            /*
            int minX = gv.mod.PlayerLocationX - gv.playerOffset - 2; //using -2 in case a large tile (3x3) needs to start off the visible map space to be seen
            if (minX < 0) { minX = 0; }
            int minY = gv.mod.PlayerLocationY - gv.playerOffset - 2; //usin g -2 in case a large tile (3x3) needs to start off the visible map space to be seen
            if (minY < 0) { minY = 0; }

            int maxX = gv.mod.PlayerLocationX + gv.playerOffset + 1;
            if (maxX > this.gv.mod.currentArea.MapSizeX) { maxX = this.gv.mod.currentArea.MapSizeX; }
            int maxY = gv.mod.PlayerLocationY + gv.playerOffset + 1; // use 2 so that extends down to bottom of screen
            if (maxY > this.gv.mod.currentArea.MapSizeY) { maxY = this.gv.mod.currentArea.MapSizeY; }
            */
            int offset = gv.playerOffset;
            /*if (use11x11)
            {
                offset = gv.playerOffsetZoom;
            }*/
            #region Draw Layer 1
            for (int x = gv.mod.PlayerLocationX - offset; x <= gv.mod.PlayerLocationX + offset - 1; x++)
            {
                for (int y = gv.mod.PlayerLocationY - offset; y <= gv.mod.PlayerLocationY + offset - 1; y++)
                {
                    //check if valid map location
                    if (x < 0) { continue; }
                    if (y < 0) { continue; }
                    if (x > this.gv.mod.currentArea.MapSizeX - 1) { continue; }
                    if (y > this.gv.mod.currentArea.MapSizeY - 1) { continue; }

                    string tile = gv.mod.currentArea.Layer1Filename[y * gv.mod.currentArea.MapSizeX + x];
                    int tlX = (x - gv.mod.PlayerLocationX + offset) * (int)(gv.squareSize * gv.scaler);
                    int tlY = (y - gv.mod.PlayerLocationY + offset) * (int)(gv.squareSize * gv.scaler);
                    //float scalerX = gv.cc.GetFromTileBitmapList(tile).PixelSize.Width / gv.tileSizeInPixels;
                    //if (scalerX == 0) { scalerX = 1.0f; }
                    //float scalerY = gv.cc.GetFromTileBitmapList(tile).PixelSize.Height / gv.tileSizeInPixels;
                    //if (scalerY == 0) { scalerY = 1.0f; }
                    int brX = (int)((int)(gv.squareSize * gv.scaler));
                    int brY = (int)((int)(gv.squareSize * gv.scaler));

                    try
                    {
                        IbRect src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                        IbRect dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                        //bool mirror = false;
                        //if (gv.mod.currentArea.Layer1Mirror[y * gv.mod.currentArea.MapSizeX + x] == 1) { mirror = true; }
                        //gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst, gv.mod.currentArea.Layer1Rotate[y * gv.mod.currentArea.MapSizeX + x], mirror);
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                    }
                    catch { }
                }
            }
            #endregion
            #region Draw Layer 2
            for (int x = gv.mod.PlayerLocationX - offset; x <= gv.mod.PlayerLocationX + offset - 1; x++)
            {
                for (int y = gv.mod.PlayerLocationY - offset; y <= gv.mod.PlayerLocationY + offset - 1; y++)
                {
                    //check if valid map location
                    if (x < 0) { continue; }
                    if (y < 0) { continue; }
                    if (x > this.gv.mod.currentArea.MapSizeX - 1) { continue; }
                    if (y > this.gv.mod.currentArea.MapSizeY - 1) { continue; }

                    string tile = gv.mod.currentArea.Layer2Filename[y * gv.mod.currentArea.MapSizeX + x];
                    int tlX = (x - gv.mod.PlayerLocationX + offset) * (int)(gv.squareSize * gv.scaler);
                    int tlY = (y - gv.mod.PlayerLocationY + offset) * (int)(gv.squareSize * gv.scaler);
                    int brX = (int)((int)(gv.squareSize * gv.scaler));
                    int brY = (int)((int)(gv.squareSize * gv.scaler));

                    try
                    {
                        IbRect src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                        IbRect dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                        //bool mirror = false;
                        //if (gv.mod.currentArea.Layer2Mirror[y * gv.mod.currentArea.MapSizeX + x] == 1) { mirror = true; }
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                    }
                    catch { }
                }
            }
            #endregion
            #region Draw Layer 3
            if (gv.mod.currentArea.Layer3Filename.Count > 0)
            {
                for (int x = gv.mod.PlayerLocationX - offset; x <= gv.mod.PlayerLocationX + offset - 1; x++)
                {
                    for (int y = gv.mod.PlayerLocationY - offset; y <= gv.mod.PlayerLocationY + offset - 1; y++)
                    {
                        //check if valid map location
                        if (x < 0) { continue; }
                        if (y < 0) { continue; }
                        if (x > this.gv.mod.currentArea.MapSizeX - 1) { continue; }
                        if (y > this.gv.mod.currentArea.MapSizeY - 1) { continue; }

                        string tile = gv.mod.currentArea.Layer3Filename[y * gv.mod.currentArea.MapSizeX + x];
                        int tlX = (x - gv.mod.PlayerLocationX + offset) * (int)(gv.squareSize * gv.scaler);
                        int tlY = (y - gv.mod.PlayerLocationY + offset) * (int)(gv.squareSize * gv.scaler);
                        int brX = (int)((int)(gv.squareSize * gv.scaler));
                        int brY = (int)((int)(gv.squareSize * gv.scaler));

                        try
                        {
                            IbRect src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            IbRect dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                            //bool mirror = false;
                            //if (gv.mod.currentArea.Layer3Mirror[y * gv.mod.currentArea.MapSizeX + x] == 1) { mirror = true; }
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                    }
                }
            }
            #endregion
        }
        /*public void drawProps()
        {
            int offset = gv.playerOffset;

            foreach (Prop p in gv.mod.currentArea.Props)
            {
                if (p.isShown)
                {
                    if ((p.LocationX >= gv.mod.PlayerLocationX - offset) && (p.LocationX <= gv.mod.PlayerLocationX + offset)
                        && (p.LocationY >= gv.mod.PlayerLocationY - offset) && (p.LocationY <= gv.mod.PlayerLocationY + offset))
                    {
                        //prop X - playerX
                        int x = ((p.LocationX - gv.mod.PlayerLocationX) * (int)(gv.squareSize * gv.scaler)) + (offset * (int)(gv.squareSize * gv.scaler));
                        int y = ((p.LocationY - gv.mod.PlayerLocationY) * (int)(gv.squareSize * gv.scaler)) + (offset * (int)(gv.squareSize * gv.scaler));
                        int dstW = (int)(((float)gv.cc.GetFromBitmapList(p.ImageFileName).PixelSize.Width / (float)(gv.squareSizeInPixels) * (float)(gv.squareSize * gv.scaler)));
                        int dstH = (int)(((float)gv.cc.GetFromBitmapList(p.ImageFileName).PixelSize.Height / (float)(gv.squareSizeInPixels) * (float)(gv.squareSize * gv.scaler)));
                        if (p.ImageFileName.StartsWith("tkn_"))
                        {
                            dstH = (int)(((float)(gv.cc.GetFromBitmapList(p.ImageFileName).PixelSize.Height / 2) / (float)(gv.squareSizeInPixels) * (float)(gv.squareSize * gv.scaler)));
                        }
                        int dstXshift = (dstW - (int)(gv.squareSize * gv.scaler)) / 2;
                        int dstYshift = (dstH - (int)(gv.squareSize * gv.scaler)) / 2;
                        IbRect src = new IbRect(0, 0, gv.cc.GetFromBitmapList(p.ImageFileName).PixelSize.Width, gv.cc.GetFromBitmapList(p.ImageFileName).PixelSize.Width);
                        IbRect dst = new IbRect(x + mapStartLocXinPixels - dstXshift, y - dstYshift, dstW, dstH);
                                                
                        gv.DrawBitmap(gv.cc.GetFromBitmapList(p.ImageFileName), src, dst, !p.PropFacingLeft);

                        
                    }
                }
            }
        }*/        
        public void drawMiniMap()
        {
            if (showMiniMap)
            {
                int pW = (int)((float)gv.screenWidth / 100.0f);
                int pH = (int)((float)gv.screenHeight / 100.0f);
                int shift = pW;
                
                //minimap should be 4 squares wide
                int minimapSquareSizeInPixels = 4 * gv.squareSize * gv.scaler / gv.mod.currentArea.MapSizeX;
                int drawW = minimapSquareSizeInPixels * gv.mod.currentArea.MapSizeX;
                int drawH = minimapSquareSizeInPixels * gv.mod.currentArea.MapSizeY;

                //draw background border
                IbRect src = new IbRect(0, 0, gv.cc.GetFromBitmapList("ui_portrait_frame").Width, gv.cc.GetFromBitmapList("ui_portrait_frame").Height);
                //IbRect dst = new IbRect(gv.squareSize, pH, drawW, drawH);
                IbRect dst = new IbRect(gv.squareSize * gv.scaler - (int)(10 * gv.scaler),
                                            pH - (int)(10 * gv.scaler),
                                            drawW + (int)(20 * gv.scaler),
                                            drawH + (int)(20 * gv.scaler));
                gv.DrawBitmap(gv.cc.GetFromBitmapList("ui_portrait_frame"), src, dst);

                //draw minimap
                if (minimap == null) { resetMiniMapBitmap(); }
                src = new IbRect(0, 0, minimap.Width, minimap.Height);
                dst = new IbRect(gv.squareSize * gv.scaler, pH, drawW, drawH);
                gv.DrawBitmap(minimap, src, dst);

                //draw Fog of War
                if (gv.mod.currentArea.UseMiniMapFogOfWar)
                {
                    for (int x = 0; x < this.gv.mod.currentArea.MapSizeX; x++)
                    {
                        for (int y = 0; y < this.gv.mod.currentArea.MapSizeY; y++)
                        {
                            int xx = x * minimapSquareSizeInPixels;
                            int yy = y * minimapSquareSizeInPixels;
                            src = new IbRect(0, 0, gv.cc.black_tile.Width, gv.cc.black_tile.Height);
                            dst = new IbRect(gv.squareSize * gv.scaler + xx, pH + yy, minimapSquareSizeInPixels, minimapSquareSizeInPixels);
                            if (gv.mod.currentArea.Visible[y * gv.mod.currentArea.MapSizeX + x] == 0)
                            {
                                gv.DrawBitmap(gv.cc.black_tile, src, dst);
                            }
                        }
                    }
                }
                                
	            //draw a location marker square RED
                int x2 = gv.mod.PlayerLocationX * minimapSquareSizeInPixels + gv.squareSize * gv.scaler;
                int y2 = gv.mod.PlayerLocationY * minimapSquareSizeInPixels;
                src = new IbRect(0, 0, gv.cc.map_marker.Width, gv.cc.map_marker.Height);
                dst = new IbRect(x2, y2 + pH, minimapSquareSizeInPixels, minimapSquareSizeInPixels);
                gv.DrawBitmap(gv.cc.map_marker, src, dst);	            
            }
        }
        public void drawPlayer()
        {
            int offset = gv.playerOffset;
            /*if (use11x11)
            {
                offset = gv.playerOffsetZoom;
            }*/
            if (gv.mod.selectedPartyLeader >= gv.mod.playerList.Count)
            {
                gv.mod.selectedPartyLeader = 0;
            }
            int x = offset * (int)(gv.squareSize * gv.scaler);
            int y = offset * (int)(gv.squareSize * gv.scaler);
            int shift = (int)(gv.squareSize * gv.scaler) / 3;
            IbRect src = new IbRect(0, 0, gv.cc.GetFromBitmapList(gv.mod.playerList[gv.mod.selectedPartyLeader].tokenFilename).Width, gv.cc.GetFromBitmapList(gv.mod.playerList[gv.mod.selectedPartyLeader].tokenFilename).Width);
            IbRect dst = new IbRect(x + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
            if (gv.mod.showPartyToken)
            {
                gv.DrawBitmap(gv.cc.GetFromBitmapList(gv.mod.partyTokenFilename), src, dst, !gv.mod.playerList[0].combatFacingLeft);
            }
            else
            {
                if ((showFullParty) && (gv.mod.playerList.Count > 1))
                {
                    if (gv.mod.playerList[0].combatFacingLeft == true)
                    {
                        //gv.oXshift = gv.oXshift + shift / 2;
                    }
                    else
                    {
                        //gv.oXshift = gv.oXshift - shift / 2;
                    }
                    int reducedSquareSize = (int)(gv.squareSize * gv.scaler) * 2 / 3;
                    for (int i = gv.mod.playerList.Count - 1; i >= 0; i--)
                    {
                        if ((i == 0) && (i != gv.mod.selectedPartyLeader))
                        {
                            dst = new IbRect(x + shift + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            gv.DrawBitmap(gv.cc.GetFromBitmapList(gv.mod.playerList[i].tokenFilename), src, dst, !gv.mod.playerList[i].combatFacingLeft);
                        }
                        if ((i == 1) && (i != gv.mod.selectedPartyLeader))
                        {
                            dst = new IbRect(x - shift + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            gv.DrawBitmap(gv.cc.GetFromBitmapList(gv.mod.playerList[i].tokenFilename), src, dst, !gv.mod.playerList[i].combatFacingLeft);
                        }
                        if ((i == 2) && (i != gv.mod.selectedPartyLeader))
                        {
                            if (gv.mod.selectedPartyLeader == 0)
                            {
                                dst = new IbRect(x + (shift) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else if (gv.mod.selectedPartyLeader == 1)
                            {
                                dst = new IbRect(x - (shift) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else
                            {
                                dst = new IbRect(x + (shift * 175 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            gv.DrawBitmap(gv.cc.GetFromBitmapList(gv.mod.playerList[i].tokenFilename), src, dst, !gv.mod.playerList[i].combatFacingLeft);
                        }
                        if ((i == 3) && (i != gv.mod.selectedPartyLeader))
                        {
                            if (gv.mod.selectedPartyLeader == 0)
                            {
                                dst = new IbRect(x + (shift * 175 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else if (gv.mod.selectedPartyLeader == 1)
                            {
                                dst = new IbRect(x + (shift * 175 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else if (gv.mod.selectedPartyLeader == 2)
                            {
                                dst = new IbRect(x + (shift * 175 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else
                            {
                                dst = new IbRect(x - (shift * 175 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            gv.DrawBitmap(gv.cc.GetFromBitmapList(gv.mod.playerList[i].tokenFilename), src, dst, !gv.mod.playerList[i].combatFacingLeft);
                        }
                        if ((i == 4) && (i != gv.mod.selectedPartyLeader))
                        {
                            if (gv.mod.selectedPartyLeader == 0)
                            {
                                dst = new IbRect(x - (shift * 175 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else if (gv.mod.selectedPartyLeader == 1)
                            {
                                dst = new IbRect(x - (shift * 175 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else if (gv.mod.selectedPartyLeader == 2)
                            {
                                dst = new IbRect(x - (shift * 175 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else if (gv.mod.selectedPartyLeader == 3)
                            {
                                dst = new IbRect(x - (shift * 175 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else
                            {
                                dst = new IbRect(x + (shift * 250 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            gv.DrawBitmap(gv.cc.GetFromBitmapList(gv.mod.playerList[i].tokenFilename), src, dst, !gv.mod.playerList[i].combatFacingLeft);
                        }

                        if ((i == 5) && (i != gv.mod.selectedPartyLeader))
                        {
                            if (gv.mod.selectedPartyLeader == 0)
                            {
                                dst = new IbRect(x + (shift * 250 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else if (gv.mod.selectedPartyLeader == 1)
                            {
                                dst = new IbRect(x + (shift * 250 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else if (gv.mod.selectedPartyLeader == 2)
                            {
                                dst = new IbRect(x + (shift * 250 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else if (gv.mod.selectedPartyLeader == 3)
                            {
                                dst = new IbRect(x + (shift * 250 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else if (gv.mod.selectedPartyLeader == 4)
                            {
                                dst = new IbRect(x + (shift * 250 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            else
                            {
                                dst = new IbRect(x - (shift * 250 / 100) + mapStartLocXinPixels, y + reducedSquareSize * 47 / 100, reducedSquareSize, reducedSquareSize);
                            }
                            gv.DrawBitmap(gv.cc.GetFromBitmapList(gv.mod.playerList[i].tokenFilename), src, dst, !gv.mod.playerList[i].combatFacingLeft);
                        }
                    }
                    
                    if (gv.mod.playerList[0].combatFacingLeft == true)
                    {
                        //gv.oXshift = gv.oXshift - shift / 2;
                    }
                    else
                    {
                        //gv.oXshift = gv.oXshift + shift / 2;
                    }
                }
                //always draw party leader on top
                int storeShift = shift;
                shift = 0;
                if (gv.mod.selectedPartyLeader == 0)
                {
                    if (showFullParty)
                    {
                        dst = new IbRect(x + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                    else
                    {
                        dst = new IbRect(x + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                }
                else if (gv.mod.selectedPartyLeader == 1)
                {
                    if (showFullParty)
                    {
                        dst = new IbRect(x + shift + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                    else
                    {
                        dst = new IbRect(x + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                }
                else if (gv.mod.selectedPartyLeader == 2)
                {
                    if (showFullParty)
                    {
                        dst = new IbRect(x - shift + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                    else
                    {
                        dst = new IbRect(x + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                }
                else if (gv.mod.selectedPartyLeader == 3)
                {
                    if (showFullParty)
                    {
                        dst = new IbRect(x + (shift * 2) + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                    else
                    {
                        dst = new IbRect(x + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                }
                else if (gv.mod.selectedPartyLeader == 4)
                {
                    if (showFullParty)
                    {
                        dst = new IbRect(x - (shift * 2) + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                    else
                    {
                        dst = new IbRect(x + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                }
                else if (gv.mod.selectedPartyLeader == 5)
                {
                    if (showFullParty)
                    {
                        dst = new IbRect(x - (shift * 3) + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                    else
                    {
                        dst = new IbRect(x + mapStartLocXinPixels, y, (int)(gv.squareSize * gv.scaler), (int)(gv.squareSize * gv.scaler));
                    }
                }                
                gv.DrawBitmap(gv.cc.GetFromBitmapList(gv.mod.playerList[gv.mod.selectedPartyLeader].tokenFilename), src, dst, !gv.mod.playerList[gv.mod.selectedPartyLeader].combatFacingLeft);
                shift = storeShift;
            }
        }
        public void drawGrid()
        {
            int offset = gv.playerOffset;
            /*if (use11x11)
            {
                offset = gv.playerOffsetZoom;
            }*/

            int minX = gv.mod.PlayerLocationX - offset;
            if (minX < 0) { minX = 0; }
            int minY = gv.mod.PlayerLocationY - offset;
            if (minY < 0) { minY = 0; }

            int maxX = gv.mod.PlayerLocationX + offset;
            if (maxX > this.gv.mod.currentArea.MapSizeX) { maxX = this.gv.mod.currentArea.MapSizeX; }
            int maxY = gv.mod.PlayerLocationY + offset;
            if (maxY > this.gv.mod.currentArea.MapSizeY) { maxY = this.gv.mod.currentArea.MapSizeY; }

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    int tlX = (x - gv.mod.PlayerLocationX + offset) * (int)(gv.squareSize * gv.scaler);
                    int tlY = (y - gv.mod.PlayerLocationY + offset) * (int)(gv.squareSize * gv.scaler);
                    int brX = (int)(gv.squareSize * gv.scaler);
                    int brY = (int)(gv.squareSize * gv.scaler);
                    IbRect src = new IbRect(0, 0, gv.cc.walkBlocked.Width, gv.cc.walkBlocked.Height);
                    IbRect dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                    if (gv.mod.currentArea.LoSBlocked[y * gv.mod.currentArea.MapSizeX + x] == 1)
                    {
                        gv.DrawBitmap(gv.cc.losBlocked, src, dst);
                    }
                    if (gv.mod.currentArea.Walkable[y * gv.mod.currentArea.MapSizeX + x] == 0)
                    {
                        gv.DrawBitmap(gv.cc.walkBlocked, src, dst);
                    }
                    else
                    {
                        gv.DrawBitmap(gv.cc.walkPass, src, dst);
                    }
                }
            }
        }
        public void drawMainMapFloatyText()
        {
            floatyTextBox.onDrawTextBox();
            /*
            int txtH = (int)gv.fontHeight;

            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    gv.DrawText(gv.cc.floatyText, gv.cc.floatyTextLoc.X + x + mapStartLocXinPixels, gv.cc.floatyTextLoc.Y + y + txtH, "bk");
                }
            }
            
            gv.DrawText(gv.cc.floatyText, gv.cc.floatyTextLoc.X + mapStartLocXinPixels, gv.cc.floatyTextLoc.Y + txtH, "wh");
            */
        }
        public void drawOverlayTints()
        {
            int offset = gv.playerOffset;
            /*if (use11x11)
            {
                offset = gv.playerOffsetZoom;
            }*/

            IbRect src = new IbRect(0, 0, gv.cc.tint_sunset.Width, gv.cc.tint_sunset.Height);
            //IbRect dst = new IbRect(gv.oXshift + mapStartLocXinPixels, 0, ((int)(gv.squareSize * sqrScale) * (gv.playerOffsetX * 2 + 1)), ((int)(gv.squareSize * sqrScale) * (gv.playerOffsetY * 2 + 2)));
            IbRect dst = new IbRect(mapStartLocXinPixels, 0, ((int)(gv.squareSize * gv.scaler) * (offset * 2 + 1)) + gv.pS, ((int)(gv.squareSize * gv.scaler) * (offset * 2 + 2)) + gv.pS);

            int dawn = 5 * 60;
            int sunrise = 6 * 60;
            int day = 7 * 60;
            int sunset = 17 * 60;
            int dusk = 18 * 60;
            int night = 20 * 60;
            int time = gv.mod.WorldTime % 1440;
            if ((time >= dawn) && (time < sunrise))
            {
                gv.DrawBitmap(gv.cc.tint_dawn, src, dst);
            }
            else if ((time >= sunrise) && (time < day))
            {
                gv.DrawBitmap(gv.cc.tint_sunrise, src, dst);
            }
            else if ((time >= day) && (time < sunset))
            {
                //no tint for day
            }
            else if ((time >= sunset) && (time < dusk))
            {
                gv.DrawBitmap(gv.cc.tint_sunset, src, dst);
            }
            else if ((time >= dusk) && (time < night))
            {
                gv.DrawBitmap(gv.cc.tint_dusk, src, dst);
            }
            else if ((time >= night) || (time < dawn))
            {
                gv.DrawBitmap(gv.cc.tint_night, src, dst);
            }

        }
        public void drawMainMapClockText()
        {
            int timeofday = gv.mod.WorldTime % (24 * 60);
            int hour = timeofday / 60;
            int minute = timeofday % 60;
            string sMinute = minute + "";
            if (minute < 10)
            {
                sMinute = "0" + minute;
            }
            string direction = "N";
            if (gv.mod.PlayerFacingDirection == 0) { direction = "N"; }
            if (gv.mod.PlayerFacingDirection == 1) { direction = "E"; }
            if (gv.mod.PlayerFacingDirection == 2) { direction = "S"; }
            if (gv.mod.PlayerFacingDirection == 3) { direction = "W"; }

            int txtH = (int)gv.fontHeight;
            int xLoc = 1 * gv.uiSquareSize + (2 * gv.scaler);
            if (gv.mod.currentArea.Is3dArea)
            {
                xLoc = map3DViewStartLocXinPixels + (2 * gv.squareSize * gv.scaler);
            }
            int yLoc = (7 * gv.uiSquareSize) - gv.fontHeight - gv.fontHeight;
            if (gv.mod.currentArea.Is3dArea)
            {
                yLoc = (6 * gv.uiSquareSize) - (2 * gv.fontHeight);
            }
            if (showTogglePanel)
            {
                yLoc = (6 * gv.uiSquareSize) + gv.oYshift - gv.fontHeight - gv.fontHeight;
                if (gv.mod.currentArea.Is3dArea)
                {
                    yLoc = (6 * gv.uiSquareSize) - (2 * gv.fontHeight);
                }
            }

            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    if (gv.mod.useRationSystem)
                    {
                        gv.DrawText(hour + ":" + sMinute + " Rations(" + gv.mod.numberOfRationsRemaining.ToString() + ")" + " (" + gv.mod.PlayerLocationX + "," + gv.mod.PlayerLocationY + ") " + direction, x + xLoc, y + yLoc, "bk");
                    }
                    else
                    {
                        gv.DrawText(hour + ":" + sMinute + " (" + gv.mod.PlayerLocationX + "," + gv.mod.PlayerLocationY + ") " + direction, x + xLoc, y + yLoc, "bk");
                    }
                }
            }
            if (gv.mod.useRationSystem)
            {
                gv.DrawText(hour + ":" + sMinute + " Rations(" + gv.mod.numberOfRationsRemaining.ToString() + ")" + " (" + gv.mod.PlayerLocationX + "," + gv.mod.PlayerLocationY + ") " + direction, xLoc, yLoc, "wh");
            }
            else
            {
                gv.DrawText(hour + ":" + sMinute + " (" + gv.mod.PlayerLocationX + "," + gv.mod.PlayerLocationY + ") " + direction, xLoc, yLoc, "wh");
            }
        }
        public void drawFogOfWar()
        {
            /*int minX = gv.mod.PlayerLocationX - gv.playerOffsetX-1;
            if (minX < 0) { minX = 0; }
            int minY = gv.mod.PlayerLocationY - gv.playerOffsetY-1;
            if (minY < 0) { minY = 0; }

            int maxX = gv.mod.PlayerLocationX + gv.playerOffsetX + 2;
            if (maxX > this.gv.mod.currentArea.MapSizeX) { maxX = this.gv.mod.currentArea.MapSizeX; }
            int maxY = gv.mod.PlayerLocationY + gv.playerOffsetY + 3;
            if (maxY > this.gv.mod.currentArea.MapSizeY) { maxY = this.gv.mod.currentArea.MapSizeY; }

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    int tlX = (x - gv.mod.PlayerLocationX + gv.playerOffsetX) * gv.squareSize;
                    int tlY = (y - gv.mod.PlayerLocationY + gv.playerOffsetY) * gv.squareSize;
                    int brX = gv.squareSize;
                    int brY = gv.squareSize;
                    IbRect src = new IbRect(0, 0, gv.cc.black_tile.PixelSize.Width, gv.cc.black_tile.PixelSize.Height);
                    IbRect dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                    if (gv.mod.currentArea.Visible[y * gv.mod.currentArea.MapSizeX + x] == 0)
                    {
                        gv.DrawBitmap(gv.cc.black_tile, src, dst);
                    }
                }
            }*/
            int offset = gv.playerOffset;
            /*if (use11x11)
            {
                offset = gv.playerOffsetZoom;
            }*/
            for (int x = gv.mod.PlayerLocationX - offset; x <= gv.mod.PlayerLocationX + offset - 1; x++)
            {
                for (int y = gv.mod.PlayerLocationY - offset; y <= gv.mod.PlayerLocationY + offset - 1; y++)
                {
                    //check if valid map location
                    if (x < 0) { continue; }
                    if (y < 0) { continue; }
                    if (x > this.gv.mod.currentArea.MapSizeX - 1) { continue; }
                    if (y > this.gv.mod.currentArea.MapSizeY - 1) { continue; }

                    int tlX = (x - gv.mod.PlayerLocationX + offset) * (int)(gv.squareSize * gv.scaler);
                    int tlY = (y - gv.mod.PlayerLocationY + offset) * (int)(gv.squareSize * gv.scaler);
                    int brX = (int)(gv.squareSize * gv.scaler);
                    int brY = (int)(gv.squareSize * gv.scaler);

                    try
                    {
                        IbRect src = new IbRect(0, 0, gv.cc.black_tile.Width, gv.cc.black_tile.Height);
                        IbRect dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                        if (gv.mod.currentArea.Visible[y * gv.mod.currentArea.MapSizeX + x] == 0)
                        {
                            gv.DrawBitmap(gv.cc.black_tile, src, dst);
                        }                        
                    }
                    catch { }
                }
            }
        }
        public void drawFloatyTextPool()
        {
            if (floatyTextPool.Count > 0)
            {
                int txtH = (int)gv.fontHeight;
                //int pH = (int)((float)gv.screenHeight / 200.0f);

                foreach (FloatyText ft in floatyTextPool)
                {
                    if (gv.cc.getDistance(ft.location, new Coordinate(gv.mod.PlayerLastLocationX, gv.mod.PlayerLocationY)) > 3)
                    {
                        continue; //out of range from view so skip drawing floaty message
                    }

                    ft.onDrawTextBox();

                    /*
                    //location.X should be the the props actual map location in squares (not screen location)
                    int xLoc = (ft.location.X + gv.playerOffsetX - gv.mod.PlayerLocationX) * (int)(gv.squareSize * sqrScale);
                    int yLoc = ((ft.location.Y + gv.playerOffsetY - gv.mod.PlayerLocationY) * (int)(gv.squareSize * sqrScale)) - (ft.z);

                    for (int x = 0; x <= 2; x++)
                    {
                        for (int y = 0; y <= 2; y++)
                        {
                            gv.DrawText(ft.value, xLoc + x + mapStartLocXinPixels, yLoc + y + txtH, "bk");
                        }
                    }
                    string colr = "yl";
                    if (ft.color.Equals("yellow"))
                    {
                        colr = "yl";
                    }
                    else if (ft.color.Equals("blue"))
                    {
                        colr = "bu";
                    }
                    else if (ft.color.Equals("green"))
                    {
                        colr = "gn";
                    }
                    else if (ft.color.Equals("red"))
                    {
                        colr = "rd";
                    }
                    else
                    {
                        colr = "wh";
                    }
                    gv.DrawText(ft.value, xLoc + mapStartLocXinPixels, yLoc + txtH, colr);
                    */
                }
            }
        }
        public void drawUiLayout()
        {
            if (gv.mod.currentArea.Is3dArea)
            {
                int width3 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_3d.png").Width;
                int height3 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_3d.png").Height;
                IbRect src3 = new IbRect(0, 0, width3, height3);
                IbRect dst3 = new IbRect(0 - (170 * gv.scaler), 0 - (102 * gv.scaler), width3 * gv.scaler, height3 * gv.scaler);
                gv.DrawBitmap(gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_3d.png"), src3, dst3);
            }
            else
            {
                int width2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Width;
                int height2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Height;
                IbRect src2 = new IbRect(0, 0, width2, height2);
                IbRect dst2 = new IbRect(0 - (170 * gv.scaler), 0 - (102 * gv.scaler), width2 * gv.scaler, height2 * gv.scaler);
                gv.DrawBitmap(gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png"), src2, dst2);
            }
            
            int width = gv.cc.GetFromTileBitmapList("ui_bg_log_2d.png").Width;
            int height = gv.cc.GetFromTileBitmapList("ui_bg_log_2d.png").Height;
            IbRect src = new IbRect(0, 0, width, height);
            IbRect dst = new IbRect(gv.log.tbXloc - (2 * gv.scaler), gv.log.tbYloc, width * gv.scaler, height * gv.scaler);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList("ui_bg_log_2d.png"), src, dst);
                        
            createArrowsPanel();
            btnArrowUp.Draw();
            btnArrowDown.Draw();
            btnArrowLeft.Draw();
            btnArrowRight.Draw();
            btnArrowWait.Draw();

            createButtonsPanel();            
            btnParty.Draw();
            btnInventory.Draw();
            btnJournal.Draw();
            //btnUseTraitOnMainMap.Draw();
            btnCastOnMainMap.Draw();
            btnSave.Draw();
            tglSettings.Draw();

            createTogglesPanel();
            tglMiniMap.Draw();
            tglPortraits.Draw();
            tglFullParty.Draw();
            tglGrid.Draw();
            tglClock.Draw();
            tglDebugMode.Draw();

            createPortraitsPanel();
            //SET PORTRAITS
            btnPort0.show = false;
            btnPort1.show = false;
            btnPort2.show = false;
            btnPort3.show = false;
            btnPort4.show = false;
            btnPort5.show = false;
            if (gv.mod.playerList.Count > 0)
            {
                Player pc = gv.mod.playerList[0];
                btnPort0.show = true;
                if (pc.IsReadyToAdvanceLevel())
                {
                    btnPort0.levelUpOn = true;
                }
                else
                {
                    btnPort0.levelUpOn = false;
                }
                btnPort0.Img = pc.tokenFilename;
                btnPort0.TextHP = pc.hp + "/" + pc.hpMax;
                btnPort0.TextSP = pc.sp + "/" + pc.spMax;
                if (gv.mod.selectedPartyLeader == 0)
                {
                    btnPort0.glowOn = true;
                }
                else
                {
                    btnPort0.glowOn = false;
                }
            }
            if (gv.mod.playerList.Count > 1)
            {
                Player pc = gv.mod.playerList[1];
                btnPort1.show = true;
                if (pc.IsReadyToAdvanceLevel())
                {
                    btnPort1.levelUpOn = true;
                }
                else
                {
                    btnPort1.levelUpOn = false;
                }
                btnPort1.Img = pc.tokenFilename;
                btnPort1.TextHP = pc.hp + "/" + pc.hpMax;
                btnPort1.TextSP = pc.sp + "/" + pc.spMax;
                if (gv.mod.selectedPartyLeader == 1)
                {
                    btnPort1.glowOn = true;
                }
                else
                {
                    btnPort1.glowOn = false;
                }
            }
            if (gv.mod.playerList.Count > 2)
            {
                Player pc = gv.mod.playerList[2];
                btnPort2.show = true;
                if (pc.IsReadyToAdvanceLevel())
                {
                    btnPort2.levelUpOn = true;
                }
                else
                {
                    btnPort2.levelUpOn = false;
                }
                btnPort2.Img = pc.tokenFilename;
                btnPort2.TextHP = pc.hp + "/" + pc.hpMax;
                btnPort2.TextSP = pc.sp + "/" + pc.spMax;
                if (gv.mod.selectedPartyLeader == 2)
                {
                    btnPort2.glowOn = true;
                }
                else
                {
                    btnPort2.glowOn = false;
                }
            }
            if (gv.mod.playerList.Count > 3)
            {
                Player pc = gv.mod.playerList[3];
                btnPort3.show = true;
                if (pc.IsReadyToAdvanceLevel())
                {
                    btnPort3.levelUpOn = true;
                }
                else
                {
                    btnPort3.levelUpOn = false;
                }
                btnPort3.Img = pc.tokenFilename;
                btnPort3.TextHP = pc.hp + "/" + pc.hpMax;
                btnPort3.TextSP = pc.sp + "/" + pc.spMax;
                if (gv.mod.selectedPartyLeader == 3)
                {
                    btnPort3.glowOn = true;
                }
                else
                {
                    btnPort3.glowOn = false;
                }
            }
            if (gv.mod.playerList.Count > 4)
            {
                Player pc = gv.mod.playerList[4];
                btnPort4.show = true;
                if (pc.IsReadyToAdvanceLevel())
                {
                    btnPort4.levelUpOn = true;
                }
                else
                {
                    btnPort4.levelUpOn = false;
                }
                btnPort4.Img = pc.tokenFilename;
                btnPort4.TextHP = pc.hp + "/" + pc.hpMax;
                btnPort4.TextSP = pc.sp + "/" + pc.spMax;
                if (gv.mod.selectedPartyLeader == 4)
                {
                    btnPort4.glowOn = true;
                }
                else
                {
                    btnPort4.glowOn = false;
                }
            }
            if (gv.mod.playerList.Count > 5)
            {
                Player pc = gv.mod.playerList[5];
                btnPort5.show = true;
                if (pc.IsReadyToAdvanceLevel())
                {
                    btnPort5.levelUpOn = true;
                }
                else
                {
                    btnPort5.levelUpOn = false;
                }
                btnPort5.Img = pc.tokenFilename;
                btnPort5.TextHP = pc.hp + "/" + pc.hpMax;
                btnPort5.TextSP = pc.sp + "/" + pc.spMax;
                if (gv.mod.selectedPartyLeader == 5)
                {
                    btnPort5.glowOn = true;
                }
                else
                {
                    btnPort5.glowOn = false;
                }
            }

            btnPort0.Draw();
            btnPort1.Draw();
            btnPort2.Draw();
            btnPort3.Draw();
            btnPort4.Draw();
            btnPort5.Draw();

            gv.log.onDrawLogBox();
        }

        public void addFloatyText(int sqrX, int sqrY, string value, string color, int length)
        {
            FloatyText floatyBox = new FloatyText(sqrX, sqrY, value, color, length);
            floatyBox.showShadow = true;
            floatyBox.gv = gv;
            floatyBox.linesList.Clear();
            floatyBox.tbWidth = 5 * gv.squareSize;
            floatyBox.AddFormattedTextToTextBox(value);
            //based on number of lines, pick YLoc
            //floatyBox.location.Y = (gridy * (int)(gv.squareSize * sqrScale)) - ((floatyTextBox.linesList.Count / 2) * (gv.fontHeight + gv.fontLineSpacing)) + ((int)(gv.squareSize * sqrScale) / 2);
            floatyBox.tbHeight = (floatyBox.linesList.Count + 1) * (gv.fontHeight + gv.fontLineSpacing);

            floatyTextPool.Add(floatyBox);
        }
                
        public void onTouchMain(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnArrowUp.glowOn = false;
            btnArrowDown.glowOn = false;
            btnArrowLeft.glowOn = false;
            btnArrowRight.glowOn = false;
            btnArrowWait.glowOn = false;

            btnParty.glowOn = false;
            btnInventory.glowOn = false;
            btnJournal.glowOn = false;
            //btnUseTraitOnMainMap.glowOn = false;
            btnCastOnMainMap.glowOn = false;
            btnSave.glowOn = false;

            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (gv.showMessageBox)
                    {
                        if (gv.messageBox.btnReturn.getImpact(x, y))
                        {
                            gv.messageBox.btnReturn.glowOn = true;
                        }
                        return;
                    }

                    if (btnArrowUp.getImpact(x, y))
                    {
                        btnArrowUp.glowOn = true;
                    }
                    else if (btnArrowDown.getImpact(x, y))
                    {
                        btnArrowDown.glowOn = true;
                    }
                    else if (btnArrowLeft.getImpact(x, y))
                    {
                        btnArrowLeft.glowOn = true;
                    }
                    else if (btnArrowRight.getImpact(x, y))
                    {
                        btnArrowRight.glowOn = true;
                    }
                    else if (btnArrowWait.getImpact(x, y))
                    {
                        btnArrowWait.glowOn = true;
                    }
                    else if (btnParty.getImpact(x, y))
                    {
                        btnParty.glowOn = true;
                    }
                    else if (btnInventory.getImpact(x, y))
                    {
                        btnInventory.glowOn = true;
                    }
                    else if (btnJournal.getImpact(x, y))
                    {
                        btnJournal.glowOn = true;
                    }
                    /*else if (btnUseTraitOnMainMap.getImpact(x, y))
                    {
                        btnUseTraitOnMainMap.glowOn = true;
                    }*/
                    else if (btnCastOnMainMap.getImpact(x, y))
                    {
                        btnCastOnMainMap.glowOn = true;
                    }
                    else if (btnSave.getImpact(x, y))
                    {
                        btnSave.glowOn = true;
                    }

                    //NEW SYSTEM
                    //mainUiLayout.setHover(x, y);

                    //Draw Floaty Text On Mouse Over Prop
                    int offset = gv.playerOffset;
                    
                    int gridx = ((eX - gv.squareSize) / (int)(gv.squareSize * gv.scaler)) + 1;
                    int gridy = (eY) / (int)(gv.squareSize * gv.scaler);
                    int actualX = gv.mod.PlayerLocationX + (gridx - offset) - (mapStartLocXinPixels / (int)(gv.squareSize));
                    int actualY = gv.mod.PlayerLocationY + (gridy - offset);
                    //gv.cc.floatyText = "";
                    floatyTextBox.linesList.Clear();
                    if (IsTouchInMapWindow(gridx, gridy))
                    {
                        /*foreach (Prop p in gv.mod.currentArea.Props)
                        {
                            if ((p.LocationX == actualX) && (p.LocationY == actualY))
                            {
                                if ((!p.MouseOverText.Equals("none")) && ((gv.mod.currentArea.Visible[actualY * gv.mod.currentArea.MapSizeX + actualX] == 1)))
                                {
                                    string text = p.MouseOverText;
                                    floatyTextBox.tbWidth = 5 * gv.squareSize;
                                    floatyTextBox.tbXloc = ((gridx) * (int)(gv.squareSize * gv.scaler));
                                    floatyTextBox.AddFormattedTextToTextBox(text);
                                    //based on number of lines, pick YLoc
                                    floatyTextBox.tbYloc = (gridy * (int)(gv.squareSize * gv.scaler)) - ((floatyTextBox.linesList.Count / 2) * (gv.fontHeight + gv.fontLineSpacing)) + (gv.squareSize / 2);                                    
                                    floatyTextBox.tbHeight = (floatyTextBox.linesList.Count + 1) * (gv.fontHeight + gv.fontLineSpacing);
                                    //floatyTextBox.linesList.Clear();
                                    

                                    //gv.cc.floatyText = p.MouseOverText;
                                    //int halfWidth = (p.MouseOverText.Length * (gv.fontWidth + gv.fontCharSpacing)) / 2;
                                    //gv.cc.floatyTextLoc = new Coordinate((gridx * gv.squareSize) - mapStartLocXinPixels - halfWidth, gridy * gv.squareSize);
                                }
                            }
                        }*/
                    }
                    break;

                case MouseEventType.EventType.MouseUp:

                    btnArrowUp.glowOn = false;
                    btnArrowDown.glowOn = false;
                    btnArrowLeft.glowOn = false;
                    btnArrowRight.glowOn = false;
                    btnArrowWait.glowOn = false;

                    btnParty.glowOn = false;
                    btnInventory.glowOn = false;
                    btnJournal.glowOn = false;
                    //btnUseTraitOnMainMap.glowOn = false;
                    btnCastOnMainMap.glowOn = false;
                    btnSave.glowOn = false;

                    x = (int)eX;
                    y = (int)eY;
                    offset = gv.playerOffset;

                    int gridX = ((eX - gv.squareSize) / (int)(gv.squareSize * gv.scaler)) + 1;
                    int gridY = (int)eY / (int)(gv.squareSize * gv.scaler);
                    int actualx = gv.mod.PlayerLocationX + (gridX - offset - (mapStartLocXinPixels / (int)(gv.squareSize)));
                    int actualy = gv.mod.PlayerLocationY + (gridY - offset);

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

                    //NEW SYSTEM FOR GLOW
                    //mainUiLayout.setHover(-1, -1);

                    //NEW SYSTEM
                    //string rtn = mainUiLayout.getImpact(x, y);
                    
                    if (tglGrid.getImpact(x, y))
                    {
                        if (tglGrid.toggleOn)
                        {
                            tglGrid.toggleOn = false;
                            gv.mod.map_showGrid = false;
                            gv.toggleSettings.map_showGrid = gv.mod.map_showGrid;
                        }
                        else
                        {
                            tglGrid.toggleOn = true;
                            gv.mod.map_showGrid = true;
                            gv.toggleSettings.map_showGrid = gv.mod.map_showGrid;
                        }
                    }                    
                    if (tglClock.getImpact(x, y))
                    {
                        tglClock.toggleOn = !tglClock.toggleOn;
                        showClock = !showClock;
                        gv.toggleSettings.showClock = showClock;
                    }                    
                    if (tglDebugMode.getImpact(x, y))
                    {
                        if (tglDebugMode.toggleOn)
                        {
                            tglDebugMode.toggleOn = false;
                            gv.mod.debugMode = false;
                            gv.toggleSettings.debugMode = gv.mod.debugMode;
                            gv.cc.addLogText("lime", "DebugMode Off");
                        }
                        else
                        {
                            tglDebugMode.toggleOn = true;
                            gv.mod.debugMode = true;
                            gv.toggleSettings.debugMode = gv.mod.debugMode;
                            gv.cc.addLogText("lime", "DebugMode On");
                        }
                    }
                    if (tglFullParty.getImpact(x, y))
                    {
                        if (tglFullParty.toggleOn)
                        {
                            tglFullParty.toggleOn = false;
                            showFullParty = false;
                            gv.toggleSettings.showFullParty = showFullParty;
                            gv.cc.addLogText("lime", "Show Party Leader");
                        }
                        else
                        {
                            tglFullParty.toggleOn = true;
                            showFullParty = true;
                            gv.toggleSettings.showFullParty = showFullParty;
                            gv.cc.addLogText("lime", "Show Full Party");
                        }
                    }
                    if (tglMiniMap.getImpact(x, y))
                    {
                        if (tglMiniMap.toggleOn)
                        {
                            tglMiniMap.toggleOn = false;
                            showMiniMap = false;
                            gv.toggleSettings.showMiniMap = showMiniMap;
                            gv.cc.addLogText("lime", "Hide Mini Map");
                        }
                        else
                        {
                            tglMiniMap.toggleOn = true;
                            showMiniMap = true;
                            gv.toggleSettings.showMiniMap = showMiniMap;
                            gv.cc.addLogText("lime", "Show Mini Map");
                        }
                    }

                    if (gv.mod.currentArea.Is3dArea)
                    {
                        if (btnArrowUp.getImpact(x, y))
                        {
                            if (gv.mod.PlayerFacingDirection == 0) //NORTH
                            {
                                if (gv.mod.PlayerLocationY > 0)
                                {
                                    if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY - 1) == false)
                                    {
                                        gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                                        gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                                        gv.mod.PlayerLocationY--;
                                        gv.cc.doUpdate();
                                    }
                                }
                            }
                            else if (gv.mod.PlayerFacingDirection == 1) //EAST
                            {
                                int mapwidth = gv.mod.currentArea.MapSizeX;
                                if (gv.mod.PlayerLocationX < (mapwidth - 1))
                                {
                                    if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX + 1, gv.mod.PlayerLocationY) == false)
                                    {
                                        gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                                        gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                                        gv.mod.PlayerLocationX++;
                                        foreach (Player pc in gv.mod.playerList)
                                        {
                                            if (pc.combatFacingLeft)
                                            {
                                                pc.combatFacingLeft = false;
                                            }
                                        }
                                        gv.cc.doUpdate();
                                    }
                                }
                            }
                            else if (gv.mod.PlayerFacingDirection == 2) //SOUTH
                            {
                                int mapheight = gv.mod.currentArea.MapSizeY;
                                if (gv.mod.PlayerLocationY < (mapheight - 1))
                                {
                                    if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY + 1) == false)
                                    {
                                        gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                                        gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                                        gv.mod.PlayerLocationY++;
                                        gv.cc.doUpdate();
                                    }
                                }
                            }
                            else if (gv.mod.PlayerFacingDirection == 3) //WEST
                            {
                                if (gv.mod.PlayerLocationX > 0)
                                {
                                    if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX - 1, gv.mod.PlayerLocationY) == false)
                                    {
                                        gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                                        gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                                        gv.mod.PlayerLocationX--;
                                        foreach (Player pc in gv.mod.playerList)
                                        {
                                            if (!pc.combatFacingLeft)
                                            {
                                                pc.combatFacingLeft = true;
                                            }
                                        }
                                        gv.cc.doUpdate();
                                    }
                                }
                            }                            
                        }
                        else if (btnArrowLeft.getImpact(x, y))
                        {
                            gv.mod.PlayerFacingDirection--;
                            if (gv.mod.PlayerFacingDirection < 0)
                            {
                                gv.mod.PlayerFacingDirection = 3;
                            }
                        }
                        else if (btnArrowRight.getImpact(x, y))
                        {
                            gv.mod.PlayerFacingDirection++;
                            if (gv.mod.PlayerFacingDirection > 3)
                            {
                                gv.mod.PlayerFacingDirection = 0;
                            }
                        }
                    }
                    else
                    {
                        if ((btnArrowUp.getImpact(x,y)) || ((gv.mod.PlayerLocationX == actualx) && ((gv.mod.PlayerLocationY - 1) == actualy)))
                        {

                            if (gv.mod.PlayerLocationY > 0)
                            {
                                if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY - 1) == false)
                                {
                                    gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                                    gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                                    gv.mod.PlayerLocationY--;
                                    gv.cc.doUpdate();
                                }
                            }

                        }
                        else if ((btnArrowDown.getImpact(x, y)) || ((gv.mod.PlayerLocationX == actualx) && ((gv.mod.PlayerLocationY + 1) == actualy)))
                        {


                            int mapheight = gv.mod.currentArea.MapSizeY;
                            if (gv.mod.PlayerLocationY < (mapheight - 1))
                            {
                                if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY + 1) == false)
                                {
                                    gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                                    gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                                    gv.mod.PlayerLocationY++;
                                    gv.cc.doUpdate();
                                }
                            }

                        }
                        else if ((btnArrowLeft.getImpact(x, y)) || (((gv.mod.PlayerLocationX - 1) == actualx) && (gv.mod.PlayerLocationY == actualy)))
                        {

                            if (gv.mod.PlayerLocationX > 0)
                            {
                                if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX - 1, gv.mod.PlayerLocationY) == false)
                                {
                                    gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                                    gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                                    gv.mod.PlayerLocationX--;
                                    foreach (Player pc in gv.mod.playerList)
                                    {
                                        if (!pc.combatFacingLeft)
                                        {
                                            pc.combatFacingLeft = true;
                                        }
                                    }
                                    gv.cc.doUpdate();
                                }
                            }

                        }
                        else if ((btnArrowRight.getImpact(x, y)) || (((gv.mod.PlayerLocationX + 1) == actualx) && (gv.mod.PlayerLocationY == actualy)))
                        {

                            int mapwidth = gv.mod.currentArea.MapSizeX;
                            if (gv.mod.PlayerLocationX < (mapwidth - 1))
                            {
                                if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX + 1, gv.mod.PlayerLocationY) == false)
                                {
                                    gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                                    gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                                    gv.mod.PlayerLocationX++;
                                    foreach (Player pc in gv.mod.playerList)
                                    {
                                        if (pc.combatFacingLeft)
                                        {
                                            pc.combatFacingLeft = false;
                                        }
                                    }
                                    gv.cc.doUpdate();
                                }
                            }

                        }
                    }

                    if (btnParty.getImpact(x, y))
                    {
                        gv.screenParty.resetPartyScreen();
                        gv.screenType = "party";
                        gv.cc.tutorialMessageParty(false);
                    }
                    else if ((btnPort0.getImpact(x, y)) && (gv.mod.playerList.Count > 0))
                    {
                        /*if (e.Button == MouseButtons.Left)
                        {
                            gv.mod.selectedPartyLeader = 0;
                            gv.cc.partyScreenPcIndex = 0;
                            gv.screenParty.resetPartyScreen();
                            gv.screenType = "party";
                            gv.cc.tutorialMessageParty(false);
                        }*/
                        //else if (e.Button == MouseButtons.Right)
                        //{
                            gv.mod.selectedPartyLeader = 0;
                            gv.cc.partyScreenPcIndex = 0;
                        //}
                    }
                    else if ((btnPort1.getImpact(x, y)) && (gv.mod.playerList.Count > 1))
                    {
                        
                            gv.mod.selectedPartyLeader = 1;
                            gv.cc.partyScreenPcIndex = 1;
                        
                    }
                    else if ((btnPort2.getImpact(x, y)) && (gv.mod.playerList.Count > 2))
                    {
                        
                            gv.mod.selectedPartyLeader = 2;
                            gv.cc.partyScreenPcIndex = 2;
                        
                    }
                    else if ((btnPort3.getImpact(x, y)) && (gv.mod.playerList.Count > 3))
                    {
                        
                            gv.mod.selectedPartyLeader = 3;
                            gv.cc.partyScreenPcIndex = 3;
                        
                    }
                    else if ((btnPort4.getImpact(x, y)) && (gv.mod.playerList.Count > 4))
                    {
                        
                            gv.mod.selectedPartyLeader = 4;
                            gv.cc.partyScreenPcIndex = 4;
                        
                    }
                    else if ((btnPort5.getImpact(x, y)) && (gv.mod.playerList.Count > 5))
                    {
                        
                            gv.mod.selectedPartyLeader = 5;
                            gv.cc.partyScreenPcIndex = 5;
                        
                    }
                    else if (btnInventory.getImpact(x, y))
                    {
                        gv.screenType = "inventory";
                        gv.screenInventory.resetInventory();
                        gv.cc.tutorialMessageInventory(false);
                    }
                    else if (btnJournal.getImpact(x, y))
                    {
                        gv.screenType = "journal";
                    }
                    else if (tglSettings.getImpact(x, y))
                    {
                        tglSettings.toggleOn = !tglSettings.toggleOn;
                        showTogglePanel = !showTogglePanel;
                        gv.toggleSettings.showTogglePanel = showTogglePanel;                        
                    }
                    else if (tglPortraits.getImpact(x, y))
                    {
                        tglPortraits.toggleOn = !tglPortraits.toggleOn;
                        showPortraitPanel = !showPortraitPanel;
                        gv.toggleSettings.showPortraitPanel = showPortraitPanel;
                    }
                    else if (btnSave.getImpact(x, y))
                    {
                        if (gv.mod.allowSave)
                        {
                            //gv.cc.doSavesDialog();
                            gv.cc.doSavesSetupDialog();
                        }
                    }
                    else if (btnArrowWait.getImpact(x, y))
                    {
                        gv.cc.doUpdate();
                    }
                    else if (btnCastOnMainMap.getImpact(x, y))
                    {
                        doCastSelectorSetup();                        
                    }
                    /*else if (btnUseTraitOnMainMap.getImpact(x, y))
                    {
                        doTraitUserSelectorSetup();
                    }   */      
                    break;
            }
        }
        public void doCastSelectorSetup()
        {
            List<int> pcIndex = new List<int>();
            //If only one PC, do not show select PC dialog...just go to cast selector
            if (pcIndex.Count == 1)
            {
                try
                {
                    gv.screenCastSelector.castingPlayerIndex = pcIndex[0];
                    gv.screenCombat.spellSelectorIndex = 0;
                    gv.screenType = "mainMapCast";
                    return;
                }
                catch (Exception ex)
                {
                    //print error
                    gv.sf.MessageBoxHtml("error with Pc Selector screen: " + ex.ToString());
                    gv.errorLog(ex.ToString());
                    return;
                }
            }

            List<string> pcNames = new List<string>();            
            pcNames.Add("cancel");

            int cnt = 0;
            foreach (Player p in gv.mod.playerList)
            {
                if (p.isAlive())
                {
                    if (hasMainMapTypeSpell(p))
                    {
                        pcNames.Add(p.name);
                        pcIndex.Add(cnt);
                    }
                }
                cnt++;
            }
            
            gv.itemListSelector.setupIBminiItemListSelector(gv, pcNames, "Select Caster", "mainmapselectcaster");
            gv.itemListSelector.showIBminiItemListSelector = true;
        }
        public void doCastSelector(int selectedIndex)
        {
            if (selectedIndex > 0)
            {
                List<int> pcIndex = new List<int>();
                int cnt = 0;
                foreach (Player p in gv.mod.playerList)
                {
                    if (hasMainMapTypeSpell(p))
                    {
                        pcIndex.Add(cnt);
                    }
                    cnt++;
                }
                try
                {
                    gv.screenCastSelector.castingPlayerIndex = pcIndex[selectedIndex - 1]; // pcIndex.get(item - 1);
                    gv.screenCombat.spellSelectorIndex = 0;
                    gv.screenType = "mainMapCast";
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBoxHtml("error with Pc Selector screen: " + ex.ToString());
                    gv.errorLog(ex.ToString());
                    //print error
                }
            }
            else if (selectedIndex == 0) // selected "cancel"
            {
                //do nothing
            }
        }
        public void doTraitUserSelectorSetup()
        {
            List<int> pcIndex = new List<int>();
            //If only one PC, do not show select PC dialog...just go to cast selector
            if (pcIndex.Count == 1)
            {
                try
                {
                    gv.screenTraitUseSelector.traitUsingPlayerIndex = pcIndex[0];
                    gv.screenCombat.traitUseSelectorIndex = 0;
                    gv.screenType = "mainMapTraitUse";
                    return;
                }
                catch (Exception ex)
                {
                    //print error
                    gv.sf.MessageBoxHtml("error with Pc Selector screen: " + ex.ToString());
                    gv.errorLog(ex.ToString());
                    return;
                }
            }

            List<string> pcNames = new List<string>();
            pcNames.Add("cancel");

            int cnt = 0;
            foreach (Player p in gv.mod.playerList)
            {
                if (p.isAlive())
                {
                    if (hasMainMapTypeTrait(p))
                    {
                        pcNames.Add(p.name);
                        pcIndex.Add(cnt);
                    }
                }
                cnt++;
            }

            gv.itemListSelector.setupIBminiItemListSelector(gv, pcNames, "Select Trait User", "mainmapselecttraituser");
            gv.itemListSelector.showIBminiItemListSelector = true;
        }
        public void doTraitUserSelector(int selectedIndex)
        {
            if (selectedIndex > 0)
            {
                List<int> pcIndex = new List<int>();
                int cnt = 0;
                foreach (Player p in gv.mod.playerList)
                {
                    if (hasMainMapTypeTrait(p))
                    {
                        pcIndex.Add(cnt);
                    }
                    cnt++;
                }
                try
                {
                    gv.screenTraitUseSelector.traitUsingPlayerIndex = pcIndex[selectedIndex - 1]; // pcIndex.get(item - 1);
                    gv.screenCombat.traitUseSelectorIndex = 0;
                    gv.screenType = "mainMapTraitUse";
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBoxHtml("error with Pc Selector screen: " + ex.ToString());
                    gv.errorLog(ex.ToString());
                    //print error
                }
            }
            else if (selectedIndex == 0) // selected "cancel"
            {
                //do nothing
            }
        }
        /*
        public void onKeyUp(Keys keyData)
        {
            if ((moveDelay()) && (finishedMove))
            {
                if (keyData == Keys.Left | keyData == Keys.D4 | keyData == Keys.NumPad4)
                {
                    
                        moveLeft();
                    
                }
                else if (keyData == Keys.Right | keyData == Keys.D6 | keyData == Keys.NumPad6)
                {
                    
                        moveRight();
                    
                }
                else if (keyData == Keys.Up | keyData == Keys.D8 | keyData == Keys.NumPad8)
                {
                    
                        moveUp();
                    
                }
                else if (keyData == Keys.Down | keyData == Keys.D2 | keyData == Keys.NumPad2)
                {
                    
                        moveDown();
                    
                }
                else { }
            }
            if (keyData == Keys.Q)
            {
                if (gv.mod.allowSave)
                {
                    gv.cc.QuickSave();
                    gv.cc.addLogText("lime", "Quicksave Completed");
                }
                else
                {
                    gv.cc.addLogText("red", "No save allowed at this time.");
                }
            }
            else if (keyData == Keys.D)
            {
                //IB2ToggleButton tgl = mainUiLayout.GetToggleByTag("tglDebugMode");
                //if (tgl == null) { return; }
                if (gv.mod.debugMode)
                {
                    tglDebugMode.toggleOn = false;
                    gv.mod.debugMode = false;
                    gv.toggleSettings.debugMode = gv.mod.debugMode;
                    gv.cc.addLogText("lime", "DebugMode Turned Off");
                }
                else
                {
                    tglDebugMode.toggleOn = true;
                    gv.mod.debugMode = true;
                    gv.toggleSettings.debugMode = gv.mod.debugMode;
                    gv.cc.addLogText("lime", "DebugMode Turned On");
                }
            }
            else if (keyData == Keys.I)
            {
                gv.screenType = "inventory";
                gv.screenInventory.resetInventory();
                gv.cc.tutorialMessageInventory(false);
            }
            else if (keyData == Keys.J)
            {
                gv.screenType = "journal";
            }
            else if (keyData == Keys.P)
            {
                gv.screenParty.resetPartyScreen();
                gv.screenType = "party";
                gv.cc.tutorialMessageParty(false);
            }
            else if (keyData == Keys.C)
            {
                doCastSelectorSetup();
                List<string> pcNames = new List<string>();
                List<int> pcIndex = new List<int>();
                pcNames.Add("cancel");

                int cnt = 0;
                foreach (Player p in gv.mod.playerList)
                {
                    if (hasMainMapTypeSpell(p))
                    {
                        pcNames.Add(p.name);
                        pcIndex.Add(cnt);
                    }
                    cnt++;
                }

                //If only one PC, do not show select PC dialog...just go to cast selector
                if (pcIndex.Count == 1)
                {
                    try
                    {
                        gv.screenCastSelector.castingPlayerIndex = pcIndex[0];
                        gv.screenCombat.spellSelectorIndex = 0;
                        gv.screenType = "mainMapCast";
                        return;
                    }
                    catch (Exception ex)
                    {
                        //print error
                        IBMessageBox.Show(gv, "error with Pc Selector screen: " + ex.ToString());
                        gv.errorLog(ex.ToString());
                        return;
                    }
                }

                using (ItemListSelector pcSel = new ItemListSelector(gv, pcNames, "Select Caster"))
                {
                    pcSel.ShowDialog();

                    if (pcSel.selectedIndex > 0)
                    {
                        try
                        {
                            gv.screenCastSelector.castingPlayerIndex = pcIndex[pcSel.selectedIndex - 1]; // pcIndex.get(item - 1);
                            gv.screenCombat.spellSelectorIndex = 0;
                            gv.screenType = "mainMapCast";
                        }
                        catch (Exception ex)
                        {
                            IBMessageBox.Show(gv, "error with Pc Selector screen: " + ex.ToString());
                            gv.errorLog(ex.ToString());
                            //print error
                        }
                    }
                    else if (pcSel.selectedIndex == 0) // selected "cancel"
                    {
                        //do nothing
                    }
                }
            }
        }
        */
        private bool moveDelay()
        {
            long elapsed = DateTime.Now.Ticks - timeStamp;
            if (elapsed > 10000 * movementDelayInMiliseconds) //10,000 ticks in 1 ms
            {
                timeStamp = DateTime.Now.Ticks;
                return true;
            }
            return false;
        }
        private void moveLeft()
        {
            if (gv.mod.currentArea.Is3dArea)
            {
                gv.mod.PlayerFacingDirection--;
                if (gv.mod.PlayerFacingDirection < 0)
                {
                    gv.mod.PlayerFacingDirection = 3;
                }
            }
            else
            {
                if (gv.mod.PlayerLocationX > 0)
                {
                    if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX - 1, gv.mod.PlayerLocationY) == false)
                    {
                        gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                        gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                        gv.mod.PlayerLocationX--;
                        foreach (Player pc in gv.mod.playerList)
                        {
                            if (!pc.combatFacingLeft)
                            {
                                pc.combatFacingLeft = true;
                            }
                        }
                        gv.cc.doUpdate();
                    }
                }
            }
        }
        private void moveRight()
        {
            if (gv.mod.currentArea.Is3dArea)
            {
                gv.mod.PlayerFacingDirection++;
                if (gv.mod.PlayerFacingDirection > 3)
                {
                    gv.mod.PlayerFacingDirection = 0;
                }
            }
            else
            {
                int mapwidth = gv.mod.currentArea.MapSizeX;
                if (gv.mod.PlayerLocationX < (mapwidth - 1))
                {
                    if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX + 1, gv.mod.PlayerLocationY) == false)
                    {
                        gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                        gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                        gv.mod.PlayerLocationX++;
                        foreach (Player pc in gv.mod.playerList)
                        {
                            if (pc.combatFacingLeft)
                            {
                                pc.combatFacingLeft = false;
                            }
                        }
                        gv.cc.doUpdate();
                    }
                }
            }
        }
        private void moveUp()
        {
            if (gv.mod.currentArea.Is3dArea)
            {
                if (gv.mod.PlayerFacingDirection == 0) //NORTH
                {
                    if (gv.mod.PlayerLocationY > 0)
                    {
                        if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY - 1) == false)
                        {
                            gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                            gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                            gv.mod.PlayerLocationY--;
                            gv.cc.doUpdate();
                        }
                    }
                }
                else if (gv.mod.PlayerFacingDirection == 1) //EAST
                {
                    int mapwidth = gv.mod.currentArea.MapSizeX;
                    if (gv.mod.PlayerLocationX < (mapwidth - 1))
                    {
                        if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX + 1, gv.mod.PlayerLocationY) == false)
                        {
                            gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                            gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                            gv.mod.PlayerLocationX++;
                            foreach (Player pc in gv.mod.playerList)
                            {
                                if (pc.combatFacingLeft)
                                {
                                    pc.combatFacingLeft = false;
                                }
                            }
                            gv.cc.doUpdate();
                        }
                    }
                }
                else if (gv.mod.PlayerFacingDirection == 2) //SOUTH
                {
                    int mapheight = gv.mod.currentArea.MapSizeY;
                    if (gv.mod.PlayerLocationY < (mapheight - 1))
                    {
                        if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY + 1) == false)
                        {
                            gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                            gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                            gv.mod.PlayerLocationY++;
                            gv.cc.doUpdate();
                        }
                    }
                }
                else if (gv.mod.PlayerFacingDirection == 3) //WEST
                {
                    if (gv.mod.PlayerLocationX > 0)
                    {
                        if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX - 1, gv.mod.PlayerLocationY) == false)
                        {
                            gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                            gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                            gv.mod.PlayerLocationX--;
                            foreach (Player pc in gv.mod.playerList)
                            {
                                if (!pc.combatFacingLeft)
                                {
                                    pc.combatFacingLeft = true;
                                }
                            }
                            gv.cc.doUpdate();
                        }
                    }
                }
            }
            else
            {
                if (gv.mod.PlayerLocationY > 0)
                {
                    if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY - 1) == false)
                    {
                        gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                        gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                        gv.mod.PlayerLocationY--;
                        gv.cc.doUpdate();
                    }
                }
            }                
        }
        private void moveDown()
        {
            int mapheight = gv.mod.currentArea.MapSizeY;
            if (gv.mod.PlayerLocationY < (mapheight - 1))
            {
                if (gv.mod.currentArea.GetBlocked(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY + 1) == false)
                {
                    gv.mod.PlayerLastLocationX = gv.mod.PlayerLocationX;
                    gv.mod.PlayerLastLocationY = gv.mod.PlayerLocationY;
                    gv.mod.PlayerLocationY++;
                    gv.cc.doUpdate();
                }
            }
        }

        public List<string> wrapList(string str, int wrapLength)
        {
            if (str == null)
            {
                return null;
            }
            if (wrapLength < 1)
            {
                wrapLength = 1;
            }
            int inputLineLength = str.Length;
            int offset = 0;
            List<string> returnList = new List<string>();

            while ((inputLineLength - offset) > wrapLength)
            {
                if (str.ElementAt(offset) == ' ')
                {
                    offset++;
                    continue;
                }

                int spaceToWrapAt = str.LastIndexOf(' ', wrapLength + offset);

                if (spaceToWrapAt >= offset)
                {
                    // normal case
                    returnList.Add(str.Substring(offset, spaceToWrapAt));
                    offset = spaceToWrapAt + 1;
                }
                else
                {
                    // do not wrap really long word, just extend beyond limit
                    spaceToWrapAt = str.IndexOf(' ', wrapLength + offset);
                    if (spaceToWrapAt >= 0)
                    {
                        returnList.Add(str.Substring(offset, spaceToWrapAt));
                        offset = spaceToWrapAt + 1;
                    }
                    else
                    {
                        returnList.Add(str.Substring(offset));
                        offset = inputLineLength;
                    }
                }
            }

            // Whatever is left in line is short enough to just pass through
            returnList.Add(str.Substring(offset));
            return returnList;
        }
        private void setExplored()
        {
            // set current position to visible
            if (gv.mod.PlayerLocationY * gv.mod.currentArea.MapSizeX + gv.mod.PlayerLocationX < gv.mod.currentArea.Visible.Count)
            {
                gv.mod.currentArea.Visible[gv.mod.PlayerLocationY * gv.mod.currentArea.MapSizeX + gv.mod.PlayerLocationX] = 1;
            }
            // set tiles to visible around the PC
            for (int x = gv.mod.PlayerLocationX - gv.mod.currentArea.AreaVisibleDistance; x <= gv.mod.PlayerLocationX + gv.mod.currentArea.AreaVisibleDistance; x++)
            {
                for (int y = gv.mod.PlayerLocationY - gv.mod.currentArea.AreaVisibleDistance; y <= gv.mod.PlayerLocationY + gv.mod.currentArea.AreaVisibleDistance; y++)
                {
                    int xx = x;
                    int yy = y;
                    if (xx < 1) { xx = 0; }
                    if (xx > (gv.mod.currentArea.MapSizeX - 1)) { xx = (gv.mod.currentArea.MapSizeX - 1); }
                    if (yy < 1) { yy = 0; }
                    if (yy > (gv.mod.currentArea.MapSizeY - 1)) { yy = (gv.mod.currentArea.MapSizeY - 1); }
                    if (IsLineOfSightForEachCorner(new Coordinate(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY), new Coordinate(xx, yy)))
                    {
                        gv.mod.currentArea.Visible[yy * gv.mod.currentArea.MapSizeX + xx] = 1;
                    }
                }
            }
            //make all adjacent squares visible
            int minX = gv.mod.PlayerLocationX - 1;
            if (minX < 0) { minX = 0; }
            int minY = gv.mod.PlayerLocationY - 1;
            if (minY < 0) { minY = 0; }

            int maxX = gv.mod.PlayerLocationX + 1;
            if (maxX > this.gv.mod.currentArea.MapSizeX - 1) { maxX = this.gv.mod.currentArea.MapSizeX - 1; }
            int maxY = gv.mod.PlayerLocationY + 1;
            if (maxY > this.gv.mod.currentArea.MapSizeY - 1) { maxY = this.gv.mod.currentArea.MapSizeY - 1; }

            for (int xx = minX; xx <= maxX; xx++)
            {
                for (int yy = minY; yy <= maxY; yy++)
                {
                    gv.mod.currentArea.Visible[yy * gv.mod.currentArea.MapSizeX + xx] = 1;
                }
            }
        }
        public bool IsSquareOnMap(int col, int row)
        {
            if (gv.mod.PlayerFacingDirection == 0) //NORTH
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX + col;
                int y = gv.mod.PlayerLocationY - row;
                if (x < 0) { return false; }
                if (x >= gv.mod.currentArea.MapSizeX) { return false; }
                if (y < 0) { return false; }
                if (y >= gv.mod.currentArea.MapSizeY) { return false; }
            }
            else if (gv.mod.PlayerFacingDirection == 1) //EAST
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX + row;
                int y = gv.mod.PlayerLocationY + col;
                if (x < 0) { return false; }
                if (x >= gv.mod.currentArea.MapSizeX) { return false; }
                if (y < 0) { return false; }
                if (y >= gv.mod.currentArea.MapSizeY) { return false; }
            }
            else if (gv.mod.PlayerFacingDirection == 2) //SOUTH
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX - col;
                int y = gv.mod.PlayerLocationY + row;
                if (x < 0) { return false; }
                if (x >= gv.mod.currentArea.MapSizeX) { return false; }
                if (y < 0) { return false; }
                if (y >= gv.mod.currentArea.MapSizeY) { return false; }
            }
            else if (gv.mod.PlayerFacingDirection == 3) //WEST
            {
                //change x and y to this direction
                int x = gv.mod.PlayerLocationX - row;
                int y = gv.mod.PlayerLocationY - col;
                if (x < 0) { return false; }
                if (x >= gv.mod.currentArea.MapSizeX) { return false; }
                if (y < 0) { return false; }
                if (y >= gv.mod.currentArea.MapSizeY) { return false; }
            }            
            return true;
        }
        public bool IsTouchInMapWindow(int sqrX, int sqrY)
        {
            //all input coordinates are in Screen Location, not Map Location
            if ((sqrX < 0) || (sqrY < 0))
            {
                return false;
            }
            if ((sqrX > 19) || (sqrY > 10))
            {
                return false;
            }
            return true;
        }
        public bool IsLineOfSightForEachCorner(Coordinate s, Coordinate e)
        {
            //start is at the center of party location square
            Coordinate start = new Coordinate((s.X * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2), (s.Y * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2));
            //check center of all four sides of the end square
            int halfSquare = ((int)(gv.squareSize * gv.scaler) / 2);
            //left side center
            if (IsVisibleLineOfSight(start, new Coordinate(e.X * (int)(gv.squareSize * gv.scaler), e.Y * (int)(gv.squareSize * gv.scaler) + halfSquare), e)) { return true; }
            //right side center
            if (IsVisibleLineOfSight(start, new Coordinate(e.X * (int)(gv.squareSize * gv.scaler) + (int)(gv.squareSize * gv.scaler), e.Y * (int)(gv.squareSize * gv.scaler) + halfSquare), e)) { return true; }
            //top side center
            if (IsVisibleLineOfSight(start, new Coordinate(e.X * (int)(gv.squareSize * gv.scaler) + halfSquare, e.Y * (int)(gv.squareSize * gv.scaler)), e)) { return true; }
            //bottom side center
            if (IsVisibleLineOfSight(start, new Coordinate(e.X * (int)(gv.squareSize * gv.scaler) + halfSquare, e.Y * (int)(gv.squareSize * gv.scaler) + (int)(gv.squareSize * gv.scaler)), e)) { return true; }

            return false;
        }
        public bool IsVisibleLineOfSight(Coordinate s, Coordinate e, Coordinate endSquare)
        {
            // Bresenham Line algorithm
            Coordinate start = s;
            Coordinate end = e;
            int deltax = Math.Abs(end.X - start.X);
            int deltay = Math.Abs(end.Y - start.Y);
            int ystep = 10;
            int xstep = 10;
            int gridx = 0;
            int gridy = 0;
            int gridXdelayed = s.X;
            int gridYdelayed = s.Y;

            //gv.DrawLine(end.X + gv.oXshift, end.Y + gv.oYshift, start.X + gv.oXshift, start.Y + gv.oYshift, Color.Lime, 1);
            
            #region low angle version
            if (deltax > deltay) //Low Angle line
            {
                Coordinate nextPoint = start;
                int error = deltax / 2;

                if (end.Y < start.Y) { ystep = -1 * ystep; } //down and right or left

                if (end.X > start.X) //down and right
                {
                    for (int x = start.X; x <= end.X; x += xstep)
                    {
                        nextPoint.X = x;
                        error -= deltay;
                        if (error < 0)
                        {
                            nextPoint.Y += ystep;
                            error += deltax;
                        }
                        //do your checks here for LoS blocking
                        gridx = nextPoint.X / (int)(gv.squareSize * gv.scaler);
                        gridy = nextPoint.Y / (int)(gv.squareSize * gv.scaler);
                        if (gridx < 1) { gridx = 0; }
                        if (gridx > (gv.mod.currentArea.MapSizeX - 1)) { gridx = (gv.mod.currentArea.MapSizeX - 1); }
                        if (gridy < 1) { gridy = 0; }
                        if (gridy > (gv.mod.currentArea.MapSizeY - 1)) { gridy = (gv.mod.currentArea.MapSizeY - 1); }
                        if (gv.mod.currentArea.LoSBlocked[gridy * gv.mod.currentArea.MapSizeX + gridx] == 1)
                        {
                            if ((gridx == endSquare.X) && (gridy == endSquare.Y))
                            {
                                //you are on the end square so return true
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                else //down and left
                {
                    for (int x = start.X; x >= end.X; x -= xstep)
                    {
                        nextPoint.X = x;
                        error -= deltay;
                        if (error < 0)
                        {
                            nextPoint.Y += ystep;
                            error += deltax;
                        }
                        //do your checks here for LoS blocking
                        gridx = nextPoint.X / (int)(gv.squareSize * gv.scaler);
                        gridy = nextPoint.Y / (int)(gv.squareSize * gv.scaler);
                        if (gridx < 1) { gridx = 0; }
                        if (gridx > (gv.mod.currentArea.MapSizeX - 1)) { gridx = (gv.mod.currentArea.MapSizeX - 1); }
                        if (gridy < 1) { gridy = 0; }
                        if (gridy > (gv.mod.currentArea.MapSizeY - 1)) { gridy = (gv.mod.currentArea.MapSizeY - 1); }
                        if (gv.mod.currentArea.LoSBlocked[gridy * gv.mod.currentArea.MapSizeX + gridx] == 1)
                        {
                            if ((gridx == endSquare.X) && (gridy == endSquare.Y))
                            {
                                //you are on the end square so return true
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            #endregion

            #region steep version
            else //Low Angle line
            {
                Coordinate nextPoint = start;
                int error = deltay / 2;

                if (end.X < start.X) { xstep = -1 * xstep; } //up and right or left

                if (end.Y > start.Y) //up and right
                {
                    for (int y = start.Y; y <= end.Y; y += ystep)
                    {
                        nextPoint.Y = y;
                        error -= deltax;
                        if (error < 0)
                        {
                            nextPoint.X += xstep;
                            error += deltay;
                        }
                        //do your checks here for LoS blocking
                        gridx = nextPoint.X / (int)(gv.squareSize * gv.scaler);
                        gridy = nextPoint.Y / (int)(gv.squareSize * gv.scaler);
                        if (gridx < 1) { gridx = 0; }
                        if (gridx > (gv.mod.currentArea.MapSizeX - 1)) { gridx = (gv.mod.currentArea.MapSizeX - 1); }
                        if (gridy < 1) { gridy = 0; }
                        if (gridy > (gv.mod.currentArea.MapSizeY - 1)) { gridy = (gv.mod.currentArea.MapSizeY - 1); }
                        if (gv.mod.currentArea.LoSBlocked[gridy * gv.mod.currentArea.MapSizeX + gridx] == 1)
                        {
                            if ((gridx == endSquare.X) && (gridy == endSquare.Y))
                            {
                                //you are on the end square so return true
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                else //up and right
                {
                    for (int y = start.Y; y >= end.Y; y -= ystep)
                    {
                        nextPoint.Y = y;
                        error -= deltax;
                        if (error < 0)
                        {
                            nextPoint.X += xstep;
                            error += deltay;
                        }
                        //do your checks here for LoS blocking
                        gridx = nextPoint.X / (int)(gv.squareSize * gv.scaler);
                        gridy = nextPoint.Y / (int)(gv.squareSize * gv.scaler);
                        if (gridx < 1) { gridx = 0; }
                        if (gridx > (gv.mod.currentArea.MapSizeX - 1)) { gridx = (gv.mod.currentArea.MapSizeX - 1); }
                        if (gridy < 1) { gridy = 0; }
                        if (gridy > (gv.mod.currentArea.MapSizeY - 1)) { gridy = (gv.mod.currentArea.MapSizeY - 1); }
                        if (gv.mod.currentArea.LoSBlocked[gridy * gv.mod.currentArea.MapSizeX + gridx] == 1)
                        {
                            if ((gridx == endSquare.X) && (gridy == endSquare.Y))
                            {
                                //you are on the end square so return true
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            #endregion

            return true;
        }
        public bool hasMainMapTypeSpell(Player pc)
        {
            foreach (string s in pc.knownSpellsTags)
            {
                Spell sp = gv.cc.getSpellByTag(s);
                if (sp == null) { continue; }
                if ((sp.useableInSituation.Equals("Always")) || (sp.useableInSituation.Equals("OutOfCombat")))
                {
                    return true;
                }
            }
            return false;
        }
        public bool hasMainMapTypeTrait(Player pc)
        {
            foreach (string s in pc.knownTraitsTags)
            {
                Trait tr = gv.cc.getTraitByTag(s);
                if (tr == null) { continue; }
                if ((tr.useableInSituation.Equals("Always")) || (tr.useableInSituation.Equals("OutOfCombat")))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
