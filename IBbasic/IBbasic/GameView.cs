using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace IBbasic
{
    public class GameView
    {
        SKCanvas canvas;
        //this class is handled differently than Android version
        public float screenDensity;
        public int screenWidth;
        public int screenHeight;
        public int defaultScreenDesignHeight = 240;
        public int defaultScreenDesignWidth = 384;
        public int squareSizeInPixels = 24;
        public int tileSizeInPixels = 24;
        public int standardTokenSize = 48;
        public int squareSize = 24; //in dp (squareSizeInPixels * screenDensity)
        public int uiSquareSize = 34;
        public int scaler;
        public int pS; // = squareSize / 10 ... used for small UI and text location adjustments based on squaresize
        public int squaresInWidth = 16; //19 or 11
        public int squaresInHeight = 10; //11 or 7
        public int uiSquaresInWidth = 11;
        public int uiSquaresInHeight = 7;
        public int ibbwidthL = 102; //144
        public int ibbwidthR = 34; //48
        public int ibbheight = 34;
        public int ibbMiniTglWidth = 16;
        public int ibbMiniTglHeight = 16;
        public int ibpwidth = 30;
        public int ibpheight = 46;
        public int playerOffset = 5;
        public int playerOffsetX = 5;
        public int playerOffsetY = 5;
        //public int playerOffsetZoom = 5;
        public int oXshift = 0;
        public int oYshift = 0;
        public string mainDirectory;
        public bool showHotKeys = false;
        public int fontHeight = 8;
        public int fontWidth = 8;
        public int fontCharSpacing = 1;
        public int fontLineSpacing = 2;
        public SKRect srcRect = new SKRect();
        public SKRect dstRect = new SKRect();

        //DIRECT2D STUFF
        /*
        public SharpDX.Direct3D11.Device _device;
        public SwapChain _swapChain;
        public Texture2D _backBuffer;
        public RenderTargetView _backBufferView;
        public SharpDX.Direct2D1.Factory factory2D;
        public SharpDX.DirectWrite.Factory factoryDWrite;
        public RenderTarget renderTarget2D;
        public SolidColorBrush sceneColorBrush;
        */

        public string versionNum = "1";
        public string fixedModule = "";
        public Dictionary<char, SKRect> charList = new Dictionary<char, SKRect>();
        public string screenType = "splash"; //launcher, title, main, party, inventory, combatInventory, shop, journal, combat, combatCast, convo
        public AnimationState animationState = AnimationState.None;
        public int triggerIndex = 0;
        public int triggerPropIndex = 0;
        public BitmapStringConversion bsc;

        public IB2HtmlLogBox log;
        public IBminiMessageBox messageBox;
        public bool showMessageBox = false;
        public IBminiItemListSelector itemListSelector;
        public CommonCode cc;
        public Module mod;
        public ScriptFunctions sf;
        public ScreenParty screenParty;
        public ScreenInventory screenInventory;
        public ScreenItemSelector screenItemSelector;
        public ScreenPortraitSelector screenPortraitSelector;
        public ScreenTokenSelector screenTokenSelector;
        public ScreenPcSelector screenPcSelector;
        public ScreenJournal screenJournal;
        public ScreenShop screenShop;
        public ScreenCastSelector screenCastSelector;
        public ScreenTraitUseSelector screenTraitUseSelector;
        public ScreenConvo screenConvo;
        public ScreenTitle screenTitle;
        public ScreenPcCreation screenPcCreation;
        public ScreenSpellLevelUp screenSpellLevelUp;
        public ScreenTraitLevelUp screenTraitLevelUp;
        public ScreenLauncher screenLauncher;
        public ScreenSplash screenSplash;
        public ScreenCombat screenCombat;
        public ScreenMainMap screenMainMap;
        public ScreenPartyBuild screenPartyBuild;
        public ScreenPartyRoster screenPartyRoster;
        public bool touchEnabled = true;
        public Settings toggleSettings;
        //TOOLSET SCREENS
        public ToolsetScreenModule tsModule;
        public ToolsetScreenAreaEditor tsAreaEditor;
        public ToolsetScreenMainMenu tsMainMenu;
        public ToolsetScreenConvoEditor tsConvoEditor;
        
        //public SoundPlayer soundPlayer = new SoundPlayer();
        //public Dictionary<string, Stream> oSoundStreams = new Dictionary<string, Stream>();
        //public System.Media.SoundPlayer playerButtonEnter = new System.Media.SoundPlayer();
        //public System.Media.SoundPlayer playerButtonClick = new System.Media.SoundPlayer();
       
        //TODOpublic Timer gameTimer = new Timer();
        public Stopwatch gameTimerStopwatch = new Stopwatch();
        public long previousTime = 0;
        public bool stillProcessingGameLoop = false;
        public float fps = 0;
        public int reportFPScount = 0;
        //TODOpublic Timer animationTimer = new Timer();

        public GameView()
        {
            //InitializeComponent();

            cc = new CommonCode(this);
            mod = new Module();
            bsc = new BitmapStringConversion();
            toggleSettings = new Settings();

            //this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.GameView_MouseWheel);
            mainDirectory = Directory.GetCurrentDirectory();
            
            //this.MinimumSize = new Size(100, 100);

            #region screen size selection
            /*
            using (Config itSel = new Config())
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (itSel.width == -1)
                    {
                        this.WindowState = FormWindowState.Maximized;
                        this.Width = Screen.PrimaryScreen.Bounds.Width;
                        this.Height = Screen.PrimaryScreen.Bounds.Height;
                    }
                    else
                    {
                        this.Width = itSel.width;
                        this.Height = itSel.height;
                    }
                }
            }
            */
            #endregion

            //this is the standard way, comment out the next 3 lines if manually forcing a screen resolution for testing UI layouts
            //this.WindowState = FormWindowState.Maximized;
            //this.Width = Screen.PrimaryScreen.Bounds.Width;
            //this.Height = Screen.PrimaryScreen.Bounds.Height;            
            //for testing other screen sizes, manually enter a resolution here
            //typical resolutions: 1366x768, 1920x1080, 1280x1024, 1280x800, 1024x768, 800x600, 1440x900, 1280x720, 640x360, 427x240, 1368x792, 912x528, 456x264, 960x540,
            //this.Width = 785; //785
            //this.Height = 480; //480

            screenWidth = App.ScreenWidth;
            screenHeight = App.ScreenHeight;
            
            if (screenWidth > screenHeight)
            {
                scaler = screenHeight / defaultScreenDesignHeight;
            }
            else
            {
                scaler = screenWidth / defaultScreenDesignHeight;
            }

            float sqrW = (float)screenWidth / (float)(uiSquaresInWidth);
            float sqrH = (float)screenHeight / (float)(uiSquaresInHeight);
            if (sqrW > sqrH)
            {
                squareSize = (int)(sqrH);
            }
            else
            {
                squareSize = (int)(sqrW);
            }
                        
            if ((squareSize >= 96) && (squareSize < 102))
            {
                squareSize = 96;
            }
            else if ((squareSize >= 72) && (squareSize < 76))
            {
                squareSize = 72;
            }
            else if ((squareSize >= 48) && (squareSize < 52))
            {
                squareSize = 48;
            }

            squareSize = 24;

            uiSquareSize = uiSquareSize * scaler;
            
            screenDensity = (float)squareSize / (float)squareSizeInPixels;
            //oXshift = (screenWidth - (squareSize * squaresInWidth)) / 2;
            //oYshift = (screenHeight - (squareSize * squaresInHeight)) / 2;
            oXshift = (screenWidth - (scaler * defaultScreenDesignWidth)) / 2;
            oYshift = (screenHeight - (scaler * defaultScreenDesignHeight)) / 2;

            pS = squareSize / 10; //used for small UI and text location adjustments based on squaresize for consistent look on all devices/screen resolutions

            //InitializeRenderer(); //uncomment this for DIRECT2D ADDITIONS

            //CREATES A FONTFAMILY
            fillCharList();

            setupFonts();

            //TODOanimationTimer.Tick += new System.EventHandler(this.AnimationTimer_Tick);

            log = new IB2HtmlLogBox(this);
            log.tbXloc = (8 * uiSquareSize) + oXshift / 2 + (8 * scaler);
            log.tbYloc = 2;
            log.tbWidth = 3 * uiSquareSize; //add one char because the word wrap calculates word length plus one space at end
            log.tbHeight = 4 * uiSquareSize;
            log.numberOfLinesToShow = 14;

            //setup messageBox defaults
            messageBox = new IBminiMessageBox(this);
            messageBox.currentLocX = 20;
            messageBox.currentLocY = 10;
            messageBox.numberOfLinesToShow = 17;
            messageBox.tbWidth = 344;
            messageBox.Width = 344;
            messageBox.Height = 220;
            messageBox.tbHeight = 212;
            messageBox.setupIBminiMessageBox();

            //setup itemListSelector defaults   
            itemListSelector = new IBminiItemListSelector();
            itemListSelector.currentLocX = 20;
            itemListSelector.currentLocY = 10;
            itemListSelector.Width = 344;
            itemListSelector.Height = 220;

            if (fixedModule.Equals("")) //this is the IceBlink Engine app
            {
                //TODO make sure this works
                screenSplash = new ScreenSplash(mod, this);
                screenType = "splash";
            }
            else //this is a fixed module
            {
                mod = cc.LoadModule(fixedModule + ".mod");
                resetGame();
                cc.LoadSaveListItems();
                screenType = "title";
            }

            //gameTimer.Interval = 60; //~15 fps
            //gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            gameTimerStopwatch.Start();
            previousTime = gameTimerStopwatch.ElapsedMilliseconds;
            //gameTimer.Start();
        }        
        public void setupFonts()
        {
            fontWidth = 6 * scaler;
            fontHeight = 6 * scaler;
            fontCharSpacing = 0 * scaler;
            fontLineSpacing = 2  * scaler;
        }
        
        public void createScreens()
	    {
		    sf = new ScriptFunctions(mod, this);
		    screenParty = new ScreenParty(mod, this);
		    screenInventory = new ScreenInventory(mod, this);
            screenItemSelector = new ScreenItemSelector(mod, this);
            screenPortraitSelector = new ScreenPortraitSelector(mod, this);
            screenTokenSelector = new ScreenTokenSelector(mod, this);
            screenPcSelector = new ScreenPcSelector(mod, this);
		    screenJournal = new ScreenJournal(mod, this);	
		    screenShop = new ScreenShop(mod, this);
		    screenCastSelector = new ScreenCastSelector(mod, this);
            screenTraitUseSelector = new ScreenTraitUseSelector(mod, this);
            screenConvo = new ScreenConvo(mod, this);		    
		    screenMainMap = new ScreenMainMap(mod, this);
            screenCombat = new ScreenCombat(mod, this);
            screenTitle = new ScreenTitle(mod, this);
		    screenPcCreation = new ScreenPcCreation(mod, this);
		    screenSpellLevelUp = new ScreenSpellLevelUp(mod, this);
		    screenTraitLevelUp = new ScreenTraitLevelUp(mod, this);		
		    screenLauncher = new ScreenLauncher(mod, this);
		    screenPartyBuild = new ScreenPartyBuild(mod, this);
            screenPartyRoster = new ScreenPartyRoster(mod,this);

            //TOOLSET SCREENS
            //tsModule = new ToolsetScreenModule(this);
            //tsAreaEditor = new ToolsetScreenAreaEditor(this);
            //tsMainMenu = new ToolsetScreenMainMenu(this);
            //tsConvoEditor = new ToolsetScreenConvoEditor(this);
            
	    }
        public void LoadStandardImages()
        {
            //cc.btnIni = cc.LoadBitmap("btn_ini");
            //cc.btnIniGlow = cc.LoadBitmap("btn_ini_glow");
            cc.walkPass = cc.LoadBitmap("walk_pass");
            cc.walkBlocked = cc.LoadBitmap("walk_block");
            cc.losBlocked = cc.LoadBitmap("los_block");
            cc.black_tile = cc.LoadBitmap("black_tile");
            //cc.black_tile2 = cc.LoadBitmap("black_tile2");
            cc.turn_marker = cc.LoadBitmap("turn_marker");
            cc.map_marker = cc.LoadBitmap("map_marker");
            cc.pc_dead = cc.LoadBitmap("pcdead");
            cc.pc_stealth = cc.LoadBitmap("pc_stealth");
            //cc.offScreen = cc.LoadBitmap("offScreen");
            //cc.offScreen5 = cc.LoadBitmap("offScreen5");
            //cc.offScreen6 = cc.LoadBitmap("offScreen6");
            //cc.offScreen7 = cc.LoadBitmap("offScreen7");
            //cc.offScreenTrans = cc.LoadBitmap("offScreenTrans");
            //cc.death_fx = cc.LoadBitmap("death_fx");
            cc.hitSymbol = cc.LoadBitmap("hit_symbol");
            cc.missSymbol = cc.LoadBitmap("miss_symbol");
            cc.highlight_green = cc.LoadBitmap("highlight_green");
            cc.highlight_red = cc.LoadBitmap("highlight_red");
            cc.tint_dawn = cc.LoadBitmap("tint_dawn");
            cc.tint_sunrise = cc.LoadBitmap("tint_sunrise");
            cc.tint_sunset = cc.LoadBitmap("tint_sunset");
            cc.tint_dusk = cc.LoadBitmap("tint_dusk");
            cc.tint_night = cc.LoadBitmap("tint_night");
            //off for now
            //cc.tint_rain = cc.LoadBitmap("tint_rain");
            cc.ui_portrait_frame = cc.LoadBitmap("ui_portrait_frame");
            cc.ui_bg_fullscreen = cc.LoadBitmap("ui_bg_fullscreen");
            cc.facing1 = cc.LoadBitmap("facing1");
            cc.facing2 = cc.LoadBitmap("facing2");
            cc.facing3 = cc.LoadBitmap("facing3");
            cc.facing4 = cc.LoadBitmap("facing4");
            cc.facing6 = cc.LoadBitmap("facing6");
            cc.facing7 = cc.LoadBitmap("facing7");
            cc.facing8 = cc.LoadBitmap("facing8");
            cc.facing9 = cc.LoadBitmap("facing9");
        }	
	    public void resetGame()
	    {
            mod = cc.LoadModule(mod.moduleName + ".mod");
            //reset log number of lines based on the value from the Module's mod file
            log.numberOfLinesToShow = mod.logNumberOfLines;            
                        
		    mod.debugMode = false;
		    bool foundArea = mod.setCurrentArea(mod.startingArea, this);
            if (!foundArea)
            {
                sf.MessageBox("Area: " + mod.startingArea + " does not exist in the module...check the spelling or make sure your are pointing to the correct starting area that you intended");
            }
		    mod.PlayerLocationX = mod.startingPlayerPositionX;
		    mod.PlayerLocationY = mod.startingPlayerPositionY;
		    LoadStandardImages();
		    	
		    foreach (Container c in mod.moduleContainersList)
            {
                c.initialContainerItemRefs.Clear();
                foreach (ItemRefs i in c.containerItemRefs)
                {
                    c.initialContainerItemRefs.Add(i.DeepCopy());
                }
            }
            foreach (Shop s in mod.moduleShopsList)
            {
                s.initialShopItemRefs.Clear();
                foreach (ItemRefs i in s.shopItemRefs)
                {
                    s.initialShopItemRefs.Add(i.DeepCopy());
                }
            }
            /*foreach (Area a in mod.moduleAreasObjects)
            {
                a.InitialAreaPropTagsList.Clear();
                foreach (Prop p in a.Props)
                {
                    a.InitialAreaPropTagsList.Add(p.PropTag);
                }            
            }*/
        
		    cc.nullOutControls();
            cc.setControlsStart();
            
		    createScreens();
		    //initializeSounds();
            loadSettings();
		
		    cc.LoadTestParty();
		
		    //load all the message box helps/tutorials
		    cc.stringBeginnersGuide = cc.loadTextToString("MessageBeginnersGuide.txt");
		    cc.stringPlayersGuide = cc.loadTextToString("MessagePlayersGuide.txt");
		    cc.stringPcCreation = cc.loadTextToString("MessagePcCreation.txt");
		    cc.stringMessageCombat = cc.loadTextToString("MessageCombat.txt");
		    cc.stringMessageInventory = cc.loadTextToString("MessageInventory.txt");
		    cc.stringMessageParty = cc.loadTextToString("MessageParty.txt");
		    cc.stringMessageMainMap = cc.loadTextToString("MessageMainMap.txt");
	    }
        public void loadSettings()
        {
            toggleSettings = new Settings();
            string s = this.GetSettingsString();
            if (s != "")
            {
                using (StringReader sr = new StringReader(s))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    toggleSettings = (Settings)serializer.Deserialize(sr, typeof(Settings));
                }
            }
        }
        public void saveSettings()
        {
            //SAVE THE FILE
            SaveSettings(toggleSettings);
        }
        private void fillCharList()
        {
            charList.Add('A', new SKRect(fontWidth * 0, fontHeight * 0, fontWidth * 0 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('B', new SKRect(fontWidth * 1, fontHeight * 0, fontWidth * 1 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('C', new SKRect(fontWidth * 2, fontHeight * 0, fontWidth * 2 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('D', new SKRect(fontWidth * 3, fontHeight * 0, fontWidth * 3 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('E', new SKRect(fontWidth * 4, fontHeight * 0, fontWidth * 4 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('F', new SKRect(fontWidth * 5, fontHeight * 0, fontWidth * 5 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('G', new SKRect(fontWidth * 6, fontHeight * 0, fontWidth * 6 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('H', new SKRect(fontWidth * 7, fontHeight * 0, fontWidth * 7 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('I', new SKRect(fontWidth * 8, fontHeight * 0, fontWidth * 8 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('J', new SKRect(fontWidth * 9, fontHeight * 0, fontWidth * 9 + fontWidth, fontHeight * 0 + fontHeight));

            charList.Add('K', new SKRect(fontWidth * 0, fontHeight * 1, fontWidth * 0 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('L', new SKRect(fontWidth * 1, fontHeight * 1, fontWidth * 1 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('M', new SKRect(fontWidth * 2, fontHeight * 1, fontWidth * 2 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('N', new SKRect(fontWidth * 3, fontHeight * 1, fontWidth * 3 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('O', new SKRect(fontWidth * 4, fontHeight * 1, fontWidth * 4 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('P', new SKRect(fontWidth * 5, fontHeight * 1, fontWidth * 5 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('Q', new SKRect(fontWidth * 6, fontHeight * 1, fontWidth * 6 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('R', new SKRect(fontWidth * 7, fontHeight * 1, fontWidth * 7 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('S', new SKRect(fontWidth * 8, fontHeight * 1, fontWidth * 8 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('T', new SKRect(fontWidth * 9, fontHeight * 1, fontWidth * 9 + fontWidth, fontHeight * 1 + fontHeight));

            charList.Add('U', new SKRect(fontWidth * 0, fontHeight * 2, fontWidth * 0 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('V', new SKRect(fontWidth * 1, fontHeight * 2, fontWidth * 1 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('W', new SKRect(fontWidth * 2, fontHeight * 2, fontWidth * 2 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('X', new SKRect(fontWidth * 3, fontHeight * 2, fontWidth * 3 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('Y', new SKRect(fontWidth * 4, fontHeight * 2, fontWidth * 4 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('Z', new SKRect(fontWidth * 5, fontHeight * 2, fontWidth * 5 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('a', new SKRect(fontWidth * 6, fontHeight * 2, fontWidth * 6 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('b', new SKRect(fontWidth * 7, fontHeight * 2, fontWidth * 7 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('c', new SKRect(fontWidth * 8, fontHeight * 2, fontWidth * 8 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('d', new SKRect(fontWidth * 9, fontHeight * 2, fontWidth * 9 + fontWidth, fontHeight * 2 + fontHeight));

            charList.Add('e', new SKRect(fontWidth * 0, fontHeight * 3, fontWidth * 0 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('f', new SKRect(fontWidth * 1, fontHeight * 3, fontWidth * 1 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('g', new SKRect(fontWidth * 2, fontHeight * 3, fontWidth * 2 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('h', new SKRect(fontWidth * 3, fontHeight * 3, fontWidth * 3 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('i', new SKRect(fontWidth * 4, fontHeight * 3, fontWidth * 4 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('j', new SKRect(fontWidth * 5, fontHeight * 3, fontWidth * 5 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('k', new SKRect(fontWidth * 6, fontHeight * 3, fontWidth * 6 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('l', new SKRect(fontWidth * 7, fontHeight * 3, fontWidth * 7 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('m', new SKRect(fontWidth * 8, fontHeight * 3, fontWidth * 8 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('n', new SKRect(fontWidth * 9, fontHeight * 3, fontWidth * 9 + fontWidth, fontHeight * 3 + fontHeight));

            charList.Add('o', new SKRect(fontWidth * 0, fontHeight * 4, fontWidth * 0 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('p', new SKRect(fontWidth * 1, fontHeight * 4, fontWidth * 1 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('q', new SKRect(fontWidth * 2, fontHeight * 4, fontWidth * 2 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('r', new SKRect(fontWidth * 3, fontHeight * 4, fontWidth * 3 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('s', new SKRect(fontWidth * 4, fontHeight * 4, fontWidth * 4 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('t', new SKRect(fontWidth * 5, fontHeight * 4, fontWidth * 5 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('u', new SKRect(fontWidth * 6, fontHeight * 4, fontWidth * 6 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('v', new SKRect(fontWidth * 7, fontHeight * 4, fontWidth * 7 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('w', new SKRect(fontWidth * 8, fontHeight * 4, fontWidth * 8 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('x', new SKRect(fontWidth * 9, fontHeight * 4, fontWidth * 9 + fontWidth, fontHeight * 4 + fontHeight));

            charList.Add('y', new SKRect(fontWidth * 0, fontHeight * 5, fontWidth * 0 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('z', new SKRect(fontWidth * 1, fontHeight * 5, fontWidth * 1 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('0', new SKRect(fontWidth * 2, fontHeight * 5, fontWidth * 2 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('1', new SKRect(fontWidth * 3, fontHeight * 5, fontWidth * 3 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('2', new SKRect(fontWidth * 4, fontHeight * 5, fontWidth * 4 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('3', new SKRect(fontWidth * 5, fontHeight * 5, fontWidth * 5 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('4', new SKRect(fontWidth * 6, fontHeight * 5, fontWidth * 6 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('5', new SKRect(fontWidth * 7, fontHeight * 5, fontWidth * 7 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('6', new SKRect(fontWidth * 8, fontHeight * 5, fontWidth * 8 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('7', new SKRect(fontWidth * 9, fontHeight * 5, fontWidth * 9 + fontWidth, fontHeight * 5 + fontHeight));

            charList.Add('8', new SKRect(fontWidth * 0, fontHeight * 6, fontWidth * 0 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('9', new SKRect(fontWidth * 1, fontHeight * 6, fontWidth * 1 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('.', new SKRect(fontWidth * 2, fontHeight * 6, fontWidth * 2 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add(',', new SKRect(fontWidth * 3, fontHeight * 6, fontWidth * 3 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('"', new SKRect(fontWidth * 4, fontHeight * 6, fontWidth * 4 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('\'', new SKRect(fontWidth * 5, fontHeight * 6, fontWidth * 5 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('?', new SKRect(fontWidth * 6, fontHeight * 6, fontWidth * 6 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('!', new SKRect(fontWidth * 7, fontHeight * 6, fontWidth * 7 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('~', new SKRect(fontWidth * 8, fontHeight * 6, fontWidth * 8 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('#', new SKRect(fontWidth * 9, fontHeight * 6, fontWidth * 9 + fontWidth, fontHeight * 6 + fontHeight));

            charList.Add('$', new SKRect(fontWidth * 0, fontHeight * 7, fontWidth * 0 + fontWidth, fontHeight * 7 + fontHeight));
            charList.Add('%', new SKRect(fontWidth * 1, fontHeight * 7, fontWidth * 1 + fontWidth, fontHeight * 7 + fontHeight));
            charList.Add('^', new SKRect(fontWidth * 2, fontHeight * 7, fontWidth * 2 + fontWidth, fontHeight * 7 + fontHeight));
            charList.Add('&', new SKRect(fontWidth * 3, fontHeight * 7, fontWidth * 3 + fontWidth, fontHeight * 7 + fontHeight));
            charList.Add('*', new SKRect(fontWidth * 4, fontHeight * 7, fontWidth * 4 + fontWidth, fontHeight * 7 + fontHeight));
            charList.Add('(', new SKRect(fontWidth * 5, fontHeight * 7, fontWidth * 5 + fontWidth, fontHeight * 7 + fontHeight));
            charList.Add(')', new SKRect(fontWidth * 6, fontHeight * 7, fontWidth * 6 + fontWidth, fontHeight * 7 + fontHeight));
            charList.Add('-', new SKRect(fontWidth * 7, fontHeight * 7, fontWidth * 7 + fontWidth, fontHeight * 7 + fontHeight));
            charList.Add('_', new SKRect(fontWidth * 8, fontHeight * 7, fontWidth * 8 + fontWidth, fontHeight * 7 + fontHeight));
            charList.Add('+', new SKRect(fontWidth * 9, fontHeight * 7, fontWidth * 9 + fontWidth, fontHeight * 7 + fontHeight));

            charList.Add('=', new SKRect(fontWidth * 0, fontHeight * 8, fontWidth * 0 + fontWidth, fontHeight * 8 + fontHeight));
            charList.Add('[', new SKRect(fontWidth * 1, fontHeight * 8, fontWidth * 1 + fontWidth, fontHeight * 8 + fontHeight));
            charList.Add(']', new SKRect(fontWidth * 2, fontHeight * 8, fontWidth * 2 + fontWidth, fontHeight * 8 + fontHeight));
            charList.Add('/', new SKRect(fontWidth * 3, fontHeight * 8, fontWidth * 3 + fontWidth, fontHeight * 8 + fontHeight));
            charList.Add(':', new SKRect(fontWidth * 4, fontHeight * 8, fontWidth * 4 + fontWidth, fontHeight * 8 + fontHeight));
            charList.Add('|', new SKRect(fontWidth * 4, fontHeight * 8, fontWidth * 4 + fontWidth, fontHeight * 8 + fontHeight));
            charList.Add(';', new SKRect(fontWidth * 5, fontHeight * 8, fontWidth * 5 + fontWidth, fontHeight * 8 + fontHeight));
            charList.Add('<', new SKRect(fontWidth * 6, fontHeight * 8, fontWidth * 6 + fontWidth, fontHeight * 8 + fontHeight));
            charList.Add('>', new SKRect(fontWidth * 7, fontHeight * 8, fontWidth * 7 + fontWidth, fontHeight * 8 + fontHeight));
            //charList.Add('/', new SharpDX.SKRect(64, 64, 8, 12));
            charList.Add(' ', new SKRect(fontWidth * 9, fontHeight * 8, fontWidth * 9 + fontWidth, fontHeight * 8 + fontHeight));
        }
                
        /*public void initializeSounds()
	    {
            oSoundStreams.Clear();
            string jobDir = "";
            jobDir = this.mainDirectory + "\\default\\NewModule\\sounds";
            foreach (string f in Directory.GetFiles(jobDir, "*.*", SearchOption.AllDirectories))
            {
                oSoundStreams.Add(Path.GetFileNameWithoutExtension(f), File.OpenRead(Path.GetFullPath(f)));
            }
	    }*/
	    public void PlaySound(string filenameNoExtension)
	    {            
            /*if ((filenameNoExtension.Equals("none")) || (filenameNoExtension.Equals("")) || (!mod.playSoundFx))
            {
                //play nothing
                return;
            }
            else
            {
                try
                {
                    soundPlayer.Stream = oSoundStreams[filenameNoExtension];
                    soundPlayer.Play();
                }
                catch (Exception ex)
                {
                    errorLog(ex.ToString());
                    if (mod.debugMode) //SD_20131102
                    {
                        cc.addLogText("<yl>failed to play sound" + filenameNoExtension + "</yl><BR>");
                    }
                    initializeSounds();
                }
            }*/            
	    }

        //Animation Timer Stuff
        public void postDelayed(string type, int delay)
        {
            /*if (type.Equals("doAnimation"))
            {
                animationTimer.Enabled = true;
                if (delay < 1)
                {
                    delay = 1;
                }
                animationTimer.Interval = delay;
                animationTimer.Start();
            }*/
        }
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            /*
            animationTimer.Enabled = false;
            animationTimer.Stop();
            screenCombat.doAnimationController();
            */
        }
        
        public void gameTimer_Tick(SKCanvasView sk_canvas)
        {
            if (!stillProcessingGameLoop)
            {
                stillProcessingGameLoop = true; //starting the game loop so do not allow another tick call to run until finished with this tick call.
                long current = gameTimerStopwatch.ElapsedMilliseconds; //get the current total amount of ms since the game launched
                int elapsed = (int)(current - previousTime); //calculate the total ms elapsed since the last time through the game loop
                Update(elapsed); //runs AI and physics
                //Render(); //draw the screen frame
                sk_canvas.InvalidateSurface();
                if (reportFPScount >= 10)
                {
                    reportFPScount = 0;
                    fps = 1000 / (current - previousTime);
                }
                reportFPScount++;
                previousTime = current; //remember the current time at the beginning of this tick call for the next time through the game loop to calculate elapsed time
                stillProcessingGameLoop = false; //finished game loop so okay to let the next tick call enter the game loop      
            }  
        }
        private void Update(int elapsed)
        {
            //iterate through spriteList and handle any sprite location and animation frame calculations
            if (screenType.Equals("main"))
            {
                screenMainMap.Update(elapsed);
            }
            else if (screenType.Equals("combat"))
            {
                screenCombat.Update(elapsed);
            }
        }

        //DRAW ROUTINES
        public void DrawText(string text, float xLoc, float yLoc, string color)
        {
            //default is WHITE
            SKBitmap bm = cc.GetFromBitmapList("fontWh.png");
            if (color.Equals("bk"))
            {
                bm = cc.GetFromBitmapList("fontBk.png");
            }
            else if (color.Equals("bu"))
            {
                bm = cc.GetFromBitmapList("fontBu.png");
            }
            else if (color.Equals("gn"))
            {
                bm = cc.GetFromBitmapList("fontGn.png");
            }
            else if (color.Equals("gy"))
            {
                bm = cc.GetFromBitmapList("fontGy.png");
            }
            else if (color.Equals("ma"))
            {
                bm = cc.GetFromBitmapList("fontMa.png");
            }
            else if (color.Equals("rd"))
            {
                bm = cc.GetFromBitmapList("fontRd.png");
            }
            else if (color.Equals("yl"))
            {
                bm = cc.GetFromBitmapList("fontYl.png");
            }

            float x = 0;
            foreach (char c in text)
            {
                if (c == '\r') { continue; }
                if (c == '\n') { continue; }
                char c1 = '0';
                if (!charList.ContainsKey(c)) { c1 = '#'; }
                else c1 = c;
                canvas.DrawBitmap(bm, charList[c1], new SKRect(xLoc + x + oXshift, yLoc + oYshift, xLoc + x + oXshift + fontWidth, yLoc + oYshift + fontHeight));
                x += fontWidth + fontCharSpacing;
            }
        }        
        public void DrawLine(int lastX, int lastY, int nextX, int nextY, SKColor penColor, int penWidth)
        {
            using (SKPaint skp = new SKPaint())
            {
                skp.IsAntialias = true;
                skp.Style = SKPaintStyle.Stroke;
                skp.Color = penColor;
                skp.StrokeWidth = penWidth;
                canvas.DrawLine(lastX + oXshift, lastY + oYshift, nextX + oXshift, nextY + oYshift, skp);
                //renderTarget2D.DrawLine(new Vector2(lastX + oXshift, lastY + oYshift), new Vector2(nextX + oXshift, nextY + oYshift), scb, penWidth);
            }
        }
        public void DrawRectangle(IbRect rect, SKColor penColor, int penWidth)
        {
            using (SKPaint skp = new SKPaint())
            {
                SKRect SKRectangle = new SKRect();
                rect.Left = rect.Left + oXshift;
                rect.Top = rect.Top + oYshift;

                skp.IsAntialias = true;
                skp.Style = SKPaintStyle.Stroke;
                skp.Color = penColor;
                skp.StrokeWidth = penWidth;
                canvas.DrawRect(SKRectangle, skp);                               
            }            
        }
        public void DrawBitmap(SKBitmap bitmap, IbRect source, IbRect destination)
        {
            //used for most everything
            SKRect dst = new SKRect(destination.Left + oXshift, destination.Top + oYshift, destination.Left + oXshift + destination.Width, destination.Top + oYshift + destination.Height);
            SKRect src = new SKRect(source.Left, source.Top, source.Left + source.Width, source.Top + source.Height);
            canvas.DrawBitmap(bitmap, src, dst);
        }
        public void DrawBitmap(SKBitmap bitmap, IbRect source, IbRect destination, bool mirror)
        {
            //used for drawing PCs, creatures and Props
            DrawBitmap(bitmap, source, destination, 0.0f, mirror);
        }
        public void DrawBitmap(SKBitmap bitmap, IbRect source, IbRect destination, int angleInDegrees, bool mirror)
        {
            // used for tiles
            //convert degrees to radians
            float angleInRadians = (angleInDegrees * (float)(Math.PI * 2)) / 360f;
            DrawBitmap(bitmap, source, destination, angleInRadians, mirror);
        }
        public void DrawBitmap(SKBitmap bitmap, IbRect source, IbRect destination, float angleInRadians, bool mirror)
        {
            int mir = 1;
            if (mirror) { mir = -1; }
            float xscl = 1f;
            float yscl = 1f;
            //float xscl = 1f + (((float)1 * 2 * scaler) / squareSize);
            //float yscl = 1f + (((float)1 * 2 * scaler) / squareSize);
            float angleInDegrees = (angleInRadians * 360f) / (float)(Math.PI * 2);

            canvas.Save();
            canvas.Scale(mir * xscl, yscl, (destination.Left + oXshift) + (destination.Width / 2), (destination.Top + oYshift) + (destination.Height / 2));
            canvas.RotateDegrees(angleInDegrees, (destination.Left + oXshift) + (destination.Width / 2), (destination.Top + oYshift) + (destination.Height / 2));
            dstRect.Left = destination.Left + oXshift;
            dstRect.Top = destination.Top + oYshift;
            dstRect.Right = destination.Width + destination.Left + oXshift;
            dstRect.Bottom = destination.Height + destination.Top + oYshift;
            srcRect.Left = source.Left;
            srcRect.Top = source.Top;
            srcRect.Right = source.Left + source.Width;
            srcRect.Bottom = source.Top + source.Height;

            //used for sprites
            SKRect dst = new SKRect(dstRect.Left, dstRect.Top, dstRect.Right, dstRect.Bottom);
            SKRect src = new SKRect(srcRect.Left, srcRect.Top, srcRect.Right, srcRect.Bottom);
            canvas.DrawBitmap(bitmap, src, dst);
            canvas.Restore();
        }

        //DIRECT2D STUFF
        /*public void InitializeRenderer()
        {
            string state = "";
            try
            {                
                // SwapChain description
                state += "Creating Swap Chain:";
                var desc = new SwapChainDescription()
                {
                    BufferCount = 1,
                    ModeDescription =
                        new ModeDescription(this.Width, this.Height,
                                            new Rational(60, 1), Format.R8G8B8A8_UNorm),
                    IsWindowed = true,
                    OutputHandle = this.Handle,
                    SampleDescription = new SampleDescription(1, 0),
                    SwapEffect = SwapEffect.Discard,
                    Usage = Usage.RenderTargetOutput
                };

                // Create Device and SwapChain                
                state += "Get Highest Feature Level:";
                var featureLvl = SharpDX.Direct3D11.Device.GetSupportedFeatureLevel();
                state += " Highest Feature Level is: " + featureLvl.ToString() + " :Create Device:";
                try
                {
                    SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, new[] { featureLvl }, desc, out _device, out _swapChain);
                }
                catch (Exception ex)
                {
                    this.errorLog(state + "<--->" + ex.ToString());
                    MessageBox.Show("Failed on Create Device using a feature level of " + featureLvl.ToString() + ". Will try using feature level 'Level_9_1' and DriverType.Software instead of DriverType.Hardware");
                    SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Software, DeviceCreationFlags.BgraSupport, new[] { SharpDX.Direct3D.FeatureLevel.Level_9_1 }, desc, out _device, out _swapChain);                    
                }

                if (_device == null)
                {
                    MessageBox.Show("Failed to create a device, closing IceBlink 2. Please send us your 'IB2ErrorLog.txt' file for more debugging help.");
                    Application.Exit();
                }

                // Ignore all windows events
                state += "Create Factory:";
                SharpDX.DXGI.Factory factory = _swapChain.GetParent<SharpDX.DXGI.Factory>();
                factory.MakeWindowAssociation(this.Handle, WindowAssociationFlags.IgnoreAll);

                // New RenderTargetView from the backbuffer
                state += "Creating Back Buffer:";
                _backBuffer = Texture2D.FromSwapChain<Texture2D>(_swapChain, 0);
                
                state += "Create RenderTargetView:";
                _backBufferView = new RenderTargetView(_device, _backBuffer);
                
                factory2D = new SharpDX.Direct2D1.Factory();
                using (var surface = _backBuffer.QueryInterface<Surface>())
                {
                    renderTarget2D = new RenderTarget(factory2D, surface, new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
                }
                renderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;

                //TEXT STUFF
                state += "Creating Text Factory:";
                factoryDWrite = new SharpDX.DirectWrite.Factory();
                sceneColorBrush = new SolidColorBrush(renderTarget2D, SharpDX.Color.Blue);
                renderTarget2D.TextAntialiasMode = TextAntialiasMode.Cleartype;
            }
            catch (SharpDXException ex)
            {
                MessageBox.Show("SharpDX error message appended to IB2ErrorLog.txt");
                this.errorLog(state + "<--->" + ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SharpDX error message appended to IB2ErrorLog.txt");
                this.errorLog(state + "<--->" + ex.ToString());
            }
        }*/
        /*public void BeginDraw()
        {
            _device.ImmediateContext.Rasterizer.SetViewport(new Viewport(0, 0, this.Width, this.Height));
            _device.ImmediateContext.OutputMerger.SetTargets(_backBufferView);
            renderTarget2D.BeginDraw();
        }*/        
        /*public void EndDraw()
        {
            renderTarget2D.EndDraw();
            _swapChain.Present(1, PresentFlags.None);
        }*/
        public void Render(SKCanvas c)
        {
            canvas = c;
            //BeginDraw(); //uncomment this for DIRECT2D ADDITIONS  
          
            //renderTarget2D.Clear(Color4.Black); //uncomment this for DIRECT2D ADDITIONS

            if ((mod.useUIBackground) && (!screenType.Equals("tsAreaEditor")) && (!screenType.Equals("tsConvoEditor")) && (!screenType.Equals("main")) && (!screenType.Equals("combat")) && (!screenType.Equals("launcher")) && (!screenType.Equals("title")))
            {
                drawUIBackground();
            }
            //TOOLSET SCREENS
            if (screenType.Equals("tsModule"))
            {
                tsModule.redrawTsModule();
            }
            else if (screenType.Equals("tsAreaEditor"))
            {
                tsAreaEditor.redrawTsAreaEditor();
            }
            else if (screenType.Equals("tsConvoEditor"))
            {
                tsConvoEditor.redrawTsConvoEditor();
            }
            //GAME SCREENS
            else if (screenType.Equals("title"))
            {
                screenTitle.redrawTitle();
            }
            else if (screenType.Equals("launcher"))
            {
                screenLauncher.redrawLauncher();
            }
            else if (screenType.Equals("splash"))
            {
                screenSplash.redrawSplash();
            }
            else if (screenType.Equals("pcCreation"))
            {
                screenPcCreation.redrawPcCreation();
            }
            else if (screenType.Equals("learnSpellCreation"))
            {
                screenSpellLevelUp.redrawSpellLevelUp(true);
            }
            else if (screenType.Equals("learnSpellLevelUp"))
            {
                screenSpellLevelUp.redrawSpellLevelUp(false);
            }
            else if (screenType.Equals("learnTraitCreation"))
            {
                screenTraitLevelUp.redrawTraitLevelUp(true);
            }
            else if (screenType.Equals("learnTraitLevelUp"))
            {
                screenTraitLevelUp.redrawTraitLevelUp(false);
            }
            else if (screenType.Equals("main"))
            {
                screenMainMap.redrawMain();
            }
            else if (screenType.Equals("party"))
            {
                screenParty.redrawParty();
            }
            else if (screenType.Equals("combatParty"))
            {
                screenParty.redrawParty();
            }
            else if (screenType.Equals("inventory"))
            {
                screenInventory.redrawInventory();
            }
            else if (screenType.Equals("itemSelector"))
            {
                screenItemSelector.redrawItemSelector();
            }
            else if (screenType.Equals("portraitSelector"))
            {
                screenPortraitSelector.redrawPortraitSelector();
            }
            else if (screenType.Equals("tokenSelector"))
            {
                screenTokenSelector.redrawTokenSelector();
            }
            else if (screenType.Equals("pcSelector"))
            {
                screenPcSelector.redrawPcSelector();
            }
            else if (screenType.Equals("combatInventory"))
            {
                screenInventory.redrawInventory();
            }
            else if (screenType.Equals("journal"))
            {
                screenJournal.redrawJournal();
            }
            else if (screenType.Equals("shop"))
            {
                screenShop.redrawShop();
            }
            else if (screenType.Equals("combat"))
            {
                screenCombat.redrawCombat();
            }
            else if (screenType.Equals("combatCast"))
            {
                screenCastSelector.redrawCastSelector(true);
            }
            else if (screenType.Equals("mainMapCast"))
            {
                screenCastSelector.redrawCastSelector(false);
            }
            else if (screenType.Equals("combatTraitUse"))
            {
                screenTraitUseSelector.redrawTraitUseSelector(true);
            }
            else if (screenType.Equals("mainMapTraitUse"))
            {
                screenTraitUseSelector.redrawTraitUseSelector(false);
            }
            else if (screenType.Equals("convo"))
            {
                screenConvo.redrawConvo();
            }
            else if (screenType.Equals("partyBuild"))
            {
                screenPartyBuild.redrawPartyBuild();
            }
            else if (screenType.Equals("partyRoster"))
            {
                screenPartyRoster.redrawPartyRoster();
            }
            if (mod.debugMode)
            {
                int txtH = (int)fontHeight;
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 2; y++)
                    {
                        DrawText("FPS:" + fps.ToString(), x + 5, screenHeight - oYshift - txtH - 5 + y, "bk");
                    }
                }
                DrawText("FPS:" + fps.ToString(), 5, screenHeight - oYshift - txtH - 5, "wh");
            }
            if (itemListSelector.showIBminiItemListSelector)
            {
                itemListSelector.drawItemListSelection();
            }
            //EndDraw(); //uncomment this for DIRECT2D ADDITIONS
        }
        public void drawUIBackground()
        {
            try
            {
                IbRect src = new IbRect(0, 0, cc.ui_bg_fullscreen.Width, cc.ui_bg_fullscreen.Height);
                IbRect dst = new IbRect(0 - oXshift, 0 - oYshift, screenWidth, screenHeight);
                DrawBitmap(cc.ui_bg_fullscreen, src, dst);
            }
            catch
            { }
        }
        /*public void DrawRectangle(SharpDX.RectangleF rect, SharpDX.Color penColor, int penWidth)
        {
            rect.X += oXshift;
            using (SolidColorBrush scb = new SolidColorBrush(renderTarget2D, penColor))
            {
                renderTarget2D.DrawRectangle(rect, scb, penWidth);
            }
        }
        public void DrawD2DBitmap(SharpDX.Direct2D1.Bitmap bitmap, SharpDX.RectangleF source, SharpDX.RectangleF target)
        {
            DrawD2DBitmap(bitmap, source, target, false);
        }
        public void DrawD2DBitmap(SharpDX.Direct2D1.Bitmap bitmap, SharpDX.RectangleF source, SharpDX.RectangleF target, bool mirror)
        {
            DrawD2DBitmap(bitmap, source, target, 0.0f, mirror, 1.0f , 0, 0, 0, 0, false);
        }
        public void DrawD2DBitmap(SharpDX.Direct2D1.Bitmap bitmap, SharpDX.RectangleF source, SharpDX.RectangleF target, int angleInDegrees, bool mirror)
        {
            //convert degrees to radians
            float angleInRadians = (float)(Math.PI * 2 * (float)angleInDegrees / (float)360);
            DrawD2DBitmap(bitmap, source, target, angleInRadians, mirror, 1.0f, 0, 0, 0, 0, false);
        }
        public void DrawD2DBitmap(SharpDX.Direct2D1.Bitmap bitmap, SharpDX.RectangleF source, SharpDX.RectangleF target, float angleInRadians, bool mirror, float opac, int Xshift, int Yshift, int Xscale, int Yscale, bool NearestNeighbourInterpolation)
        {
            int mir = 1;
            if (mirror) { mir = -1; }
            float xshf = (float)Xshift * 2 * scaler;
            float yshf = (float)Yshift * 2 * scaler;
            float xscl = 1f + (((float)Xscale * 2 * scaler) / squareSize);
            float yscl = 1f + (((float)Yscale * 2 * scaler) / squareSize);

            Vector2 center = new Vector2((target.Left + oXshift) + (target.Width / 2), (target.Top + oYshift) + (target.Height / 2));
            renderTarget2D.Transform = SharpDX.Matrix.Transformation2D(center, 0, new Vector2(mir * xscl, yscl), center, angleInRadians, new Vector2(xshf, yshf));
            SharpDX.RectangleF trg = new SharpDX.RectangleF(target.Left + oXshift, target.Top + oYshift, target.Width, target.Height);
            SharpDX.RectangleF src = new SharpDX.RectangleF(source.Left, source.Top, source.Width, source.Height);

            if (NearestNeighbourInterpolation)
            {
                renderTarget2D.DrawBitmap(bitmap, trg, opac, BitmapInterpolationMode.NearestNeighbor, src);
            }
            else
            {
                renderTarget2D.DrawBitmap(bitmap, trg, opac, BitmapInterpolationMode.NearestNeighbor, src);
            }            
            renderTarget2D.Transform = Matrix3x2.Identity;
        }
        */
        //INPUT STUFF
        //public bool formMoveable = false;
        //public System.Drawing.Point currentPosition;
        //public System.Drawing.Point startPosition;

        /*private void GameView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (showMessageBox)
            {
                messageBox.onMouseWheel(sender, e);
            }
            else if ((screenType.Equals("main")) || (screenType.Equals("combat")))
            {
                log.onMouseWheel(sender, e);
            }
            else if (screenType.Equals("tsConvoEditor"))
            {
                tsConvoEditor.onMouseWheel(sender, e);
            }
            onMouseEvent(sender, e, MouseEventType.EventType.MouseWheel);
        }*/
        /*private void GameView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y < 15)
            {
                Cursor.Current = Cursors.NoMove2D;
                formMoveable = true;
                startPosition.X = e.X;
                startPosition.Y = e.Y;
                return;
            }
            onMouseEvent(sender, e, MouseEventType.EventType.MouseDown);
        }*/
        /*private void GameView_MouseUp(object sender, MouseEventArgs e)
        {
            formMoveable = false;
            Cursor.Current = Cursors.Default;
            onMouseEvent(sender, e, MouseEventType.EventType.MouseUp);
        }*/
        /*private void GameView_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Y < 15) || (formMoveable))
            {
                Cursor.Current = Cursors.NoMove2D;
            }
            if (formMoveable)
            {
                System.Drawing.Point newPosition = this.Location;
                currentPosition.X = e.X;
                currentPosition.Y = e.Y;
                newPosition.X = newPosition.X - (startPosition.X - currentPosition.X); // .Offset(mouseOffset.X, mouseOffset.Y);                
                newPosition.Y = newPosition.Y - (startPosition.Y - currentPosition.Y);
                this.Location = newPosition;
                return;
            }
            onMouseEvent(sender, e, MouseEventType.EventType.MouseMove);
        }*/
        /*private void GameView_MouseClick(object sender, MouseEventArgs e)
        {
            onMouseEvent(sender, e, MouseEventType.EventType.MouseClick);
        }*/
        public void onMouseEvent(object sender, SKTouchEventArgs e)
        {
            MouseEventType.EventType eventType = MouseEventType.EventType.MouseMove;
            if (e.ActionType == SKTouchAction.Moved)
            {
                eventType = MouseEventType.EventType.MouseMove;
            }
            else if(e.ActionType == SKTouchAction.Pressed)
            {
                eventType = MouseEventType.EventType.MouseDown;
            }
            else if (e.ActionType == SKTouchAction.Released)
            {
                eventType = MouseEventType.EventType.MouseUp;
            }
            try 
            {
                int eX = (int)e.Location.X - oXshift;
                int eY = (int)e.Location.Y - oYshift;
                //do only itemListSelector if visible
                if (itemListSelector.showIBminiItemListSelector)
                {
                    itemListSelector.onTouchItemListSelection(eX, eY, eventType);
                    return;
                }
                if (touchEnabled)
                {
                    //do touch scrolling if in a scrolling text box
                    if (showMessageBox)
                    {
                        messageBox.onTouchSwipe(eX, eY, eventType);
                    }
                    else if ((screenType.Equals("main")) || (screenType.Equals("combat")))
                    {
                        log.onTouchSwipe(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsConvoEditor"))
                    {
                        //tsConvoEditor.onTouchSwipe(eX, eY, eventType);
                    }

                    //TOOLSET SCREENS
                    if (screenType.Equals("tsModule"))
                    {
                        tsModule.onTouchTsModule(eX, eY, eventType);	
                    }
                    else if (screenType.Equals("tsAreaEditor"))
                    {
                        tsAreaEditor.onTouchTsAreaEditor(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsConvoEditor"))
                    {
                        tsConvoEditor.onTouchTsConvoEditor(eX, eY, eventType);
                    }
                    //GAME SCREENS
                    else if (screenType.Equals("main"))
                    {
                        screenMainMap.onTouchMain(eX, eY, eventType);
                    }
                    else if (screenType.Equals("splash"))
                    {
                        screenSplash.onTouchSplash(eX, eY, eventType);
        }
                    else if (screenType.Equals("launcher"))
                    {
                        screenLauncher.onTouchLauncher(eX, eY, eventType);
                    }
                    else if (screenType.Equals("pcCreation"))
                    {
                        screenPcCreation.onTouchPcCreation(eX, eY, eventType);
                    }
                    else if (screenType.Equals("learnSpellCreation"))
                    {
                        screenSpellLevelUp.onTouchSpellLevelUp(eX, eY, eventType, true);   	
                    }
                    else if (screenType.Equals("learnSpellLevelUp"))
                    {
                        screenSpellLevelUp.onTouchSpellLevelUp(eX, eY, eventType, false);     	
                    }
                    else if (screenType.Equals("learnTraitCreation"))
                    {
                        screenTraitLevelUp.onTouchTraitLevelUp(eX, eY, eventType, true);   	
                    }
                    else if (screenType.Equals("learnTraitLevelUp"))
                    {
                        screenTraitLevelUp.onTouchTraitLevelUp(eX, eY, eventType, false);     	
                    }
                    else if (screenType.Equals("title"))
                    {
                        screenTitle.onTouchTitle(eX, eY, eventType);
                    }
                    else if (screenType.Equals("party"))
                    {
                        screenParty.onTouchParty(eX, eY, eventType, false);
                    }
                    else if (screenType.Equals("combatParty"))
                    {
                        screenParty.onTouchParty(eX, eY, eventType, true);
                    }
                    else if (screenType.Equals("inventory"))
                    {
                        screenInventory.onTouchInventory(eX, eY, eventType, false);
                    }
                    else if (screenType.Equals("combatInventory"))
                    {
                        screenInventory.onTouchInventory(eX, eY, eventType, true);
                    }
                    else if (screenType.Equals("itemSelector"))
                    {
                        screenItemSelector.onTouchItemSelector(eX, eY, eventType);
                    }
                    else if (screenType.Equals("portraitSelector"))
                    {
                        screenPortraitSelector.onTouchPortraitSelector(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tokenSelector"))
                    {
                        screenTokenSelector.onTouchTokenSelector(eX, eY, eventType);
                    }
                    else if (screenType.Equals("pcSelector"))
                    {
                        screenPcSelector.onTouchPcSelector(eX, eY, eventType);
                    }
                    else if (screenType.Equals("journal"))
                    {
                        screenJournal.onTouchJournal(eX, eY, eventType);
                    }
                    else if (screenType.Equals("shop"))
                    {
                        screenShop.onTouchShop(eX, eY, eventType);
                    }
                    else if (screenType.Equals("combat"))
                    {
                        screenCombat.onTouchCombat(eX, eY, eventType);
                    }
                    else if (screenType.Equals("combatCast"))
                    {
                        screenCastSelector.onTouchCastSelector(eX, eY, eventType, true);
                    }
                    else if (screenType.Equals("mainMapCast"))
                    {
                        screenCastSelector.onTouchCastSelector(eX, eY, eventType, false);
                    }
                    else if (screenType.Equals("combatTraitUse"))
                    {
                        screenTraitUseSelector.onTouchTraitUseSelector(eX, eY, eventType, true);
                    }
                    else if (screenType.Equals("mainMapTraitUse"))
                    {
                        screenTraitUseSelector.onTouchTraitUseSelector(eX, eY, eventType, false);
                    }
                    else if (screenType.Equals("convo"))
                    {
                        screenConvo.onTouchConvo(eX, eY, eventType);
                    }
                    else if (screenType.Equals("partyBuild"))
                    {
                        screenPartyBuild.onTouchPartyBuild(eX, eY, eventType);
                    }
                    else if (screenType.Equals("partyRoster"))
                    {
                        screenPartyRoster.onTouchPartyRoster(eX, eY, eventType);
                    }
                }
            }
            catch (Exception ex) 
            {
                errorLog(ex.ToString());   		
            }		
        }

        /*public void onKeyboardEvent(Keys keyData)
        {
            try
            {
                if (keyData == Keys.Escape)
                {
                    if (showMessageBox)
                    {
                        showMessageBox = false;
                        return;
                    }
                    if (itemListSelector.showIBminiItemListSelector)
                    {
                        itemListSelector.showIBminiItemListSelector = false;
                        return;
                    }
                    doVerifyClosingSetup();                    
                }
                if (touchEnabled)
                {
                    if (keyData == Keys.H)
                    {
                        if (showHotKeys) { showHotKeys = false; }
                        else { showHotKeys = true; }
                    }
                    if (screenType.Equals("main"))
                    {
                        screenMainMap.onKeyUp(keyData);
                    }
                    else if (screenType.Equals("combat"))
                    {
                        screenCombat.onKeyUp(keyData);
                    }
                    else if (screenType.Equals("convo"))
                    {
                        screenConvo.onKeyUp(keyData);
                    }
                    else if (screenType.Equals("tsAreaEditor"))
                    {
                        tsAreaEditor.onKeyUp(keyData);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog(ex.ToString());
            }
        }  */      
        /*protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            onKeyboardEvent(keyData);
                
            return base.ProcessCmdKey(ref msg, keyData);
        }*/

        //ON FORM CLOSING
        /*public void doVerifyClosingSetup()
        {
            List<string> actionList = new List<string> { "Yes, Exit", "No, Keep Playing" };
            itemListSelector.setupIBminiItemListSelector(this, actionList, "Are you sure you wish to exit?", "verifyclosing");
            itemListSelector.showIBminiItemListSelector = true;
        }*/
        /*public void doVerifyClosing(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                saveSettings();
                this.Close();
            }
            if (selectedIndex == 1)
            {
                //keep playing
            }
        }*/
        /*private void GameView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }*/

        //DIALOGS
        public string DialogReturnString(string headerText, string existingTextInputValue)
        {
            /*using (TextInputDialog itSel = new TextInputDialog(this, headerText, existingTextInputValue))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (itSel.textInput.Length > 0)
                    {
                        return itSel.textInput;
                    }
                    else
                    {
                        MessageBox.Show("Entering a blank name is not allowed");
                        return existingTextInputValue;
                    }
                }
                return existingTextInputValue;
            }*/
            return "none";
        }
        public int DialogReturnInteger(string headerText, int existingIntInputValue)
        {
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(this, headerText, existingIntInputValue))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    return itSel.numInput;
                }
                return existingIntInputValue;
            }*/
            return 0;
        }

        public void SaveSettings(Settings tglSettings)
        {
            DependencyService.Get<ISaveAndLoad>().SaveSettings(tglSettings);
        }
        public void SaveSaveGame(string modName, string filename, SaveGame save)
        {
            DependencyService.Get<ISaveAndLoad>().SaveSaveGame(modName, filename, save);
        }
        public void SaveCharacter(string pathAndFilename, Player pc)
        {
            DependencyService.Get<ISaveAndLoad>().SaveCharacter(pathAndFilename, pc);
        }
        public List<string> GetFiles(string path, string assetPath, string endsWith)
        {
            return DependencyService.Get<ISaveAndLoad>().GetFiles(path, assetPath, endsWith);
        }
        public string GetModuleFileString(string modFilename)
        {
            return DependencyService.Get<ISaveAndLoad>().GetModuleFileString(modFilename);
        }
        public string GetModuleAssetFileString(string modFolder, string assetFilename)
        {
            return DependencyService.Get<ISaveAndLoad>().GetModuleAssetFileString(modFolder, assetFilename);
        }
        public string GetDataAssetFileString(string assetFilename)
        {
            return DependencyService.Get<ISaveAndLoad>().GetDataAssetFileString(assetFilename);
        }
        public SKBitmap LoadBitmap(string filename)
        {
            return DependencyService.Get<ISaveAndLoad>().LoadBitmap(filename);
        }
        public List<string> GetAllModuleFiles()
        {
            return DependencyService.Get<ISaveAndLoad>().GetAllModuleFiles();
        }
        public string GetSettingsString()
        {
            return DependencyService.Get<ISaveAndLoad>().GetSettingsString();
        }
        public string GetSaveFileString(string modName, string filename)
        {
            return DependencyService.Get<ISaveAndLoad>().GetSaveFileString(modName, filename);
        }


        public void errorLog(string text)
        {
            /*if (mainDirectory == null) 
            { 
                mainDirectory = Directory.GetCurrentDirectory(); 
            }
            using (StreamWriter writer = new StreamWriter(mainDirectory + "//IBminiErrorLog.txt", true))
            {
                writer.Write(DateTime.Now + ": ");
                writer.WriteLine(text);
            }*/
        }
    }
}