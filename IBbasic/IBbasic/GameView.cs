using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using System.Threading.Tasks;
//using Plugin.SimpleAudioPlayer;
using System.Reflection;

namespace IBbasic
{
    public class GameView
    {
        public string versionNum = "1.0.21";
        public int numOfTrackerEventHitsInThisSession = 0;
        public ContentPage cp;
        //public SKCanvas canvas;
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
        public int uiSquareSizeDefault = 34;
        public float scaler;
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
        public int oXshift = 0;
        public int oYshift = 0;
        public string mainDirectory;
        public bool showHotKeys = false;
        public int fontHeight = 9;
        public int fontWidth = 6;
        public int fontCharSpacing = 1;
        public int fontLineSpacing = 2;
        public SKRect srcRect = new SKRect();
        public SKRect dstRect = new SKRect();
                
        public string fixedModule = "";
        public Dictionary<char, SKRect> charList = new Dictionary<char, SKRect>();
        public string screenType = "splash"; //launcher, title, main, party, inventory, combatInventory, shop, journal, combat, combatCast, convo
        public AnimationState animationState = AnimationState.None;
        public int triggerIndex = 0;
        public int triggerPropIndex = 0;
        public BitmapStringConversion bsc;

        public IBbasicToolTip tooltip;
        public bool showTooltip = false;
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
        public ScreenPartyCampaign screenPartyCampaign;
        public ScreenPartyRoster screenPartyRoster;
        public bool touchEnabled = true;
        public Settings toggleSettings;
        public IBbasicPrefernces IBprefs;
        public IBbasicReview userReviews;
        public IBbasicReview communityReviews;
        //TOOLSET SCREENS
        public ToolsetScreenModule tsModule;
        public ToolsetScreenAreaEditor tsAreaEditor;
        public ToolsetScreenEncounterEditor tsEncEditor;
        public ToolsetScreenMainMenu tsMainMenu;
        public ToolsetScreenConvoEditor tsConvoEditor;
        public ToolsetScreenContainerEditor tsContainerEditor;
        public ToolsetScreenShopEditor tsShopEditor;
        public ToolsetScreenJournalEditor tsJournalEditor;
        public ToolsetScreenCreatureEditor tsCreatureEditor;
        public ToolsetScreenItemEditor tsItemEditor;
        public ToolsetScreenPlayerEditor tsPlayerEditor;
        public ToolsetScreenArtEditor tsArtEditor;
        public ToolsetScreenDataCheck tsDataCheck;
                
        public Stopwatch gameTimerStopwatch = new Stopwatch();
        public long previousTime = 0;
        public bool stillProcessingGameLoop = false;
        public float fps = 0;
        public int reportFPScount = 0;
        public int checkAudioPlayBack = 0;
        public bool animationTimerOn = false;
        public long animationStartTime = 0;
        public int animationDelayTime = 0;

        public GameView(ContentPage conPage)
        {
            //InitializeComponent();
            cp = conPage;
            cc = new CommonCode(this);
            mod = new Module();
            bsc = new BitmapStringConversion();
            toggleSettings = new Settings();

            //this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.GameView_MouseWheel);
            mainDirectory = Directory.GetCurrentDirectory();
            CreateUserFolders();

            loadPreferences();

            versionNum = GetVersion();
            //this.MinimumSize = new Size(100, 100);
            
            screenWidth = App.ScreenWidth;
            screenHeight = App.ScreenHeight;

            squareSize = 24;

            resetScaler(false, true);

            screenDensity = (float)squareSize / (float)squareSizeInPixels;
                        
            pS = squareSize / 10; //used for small UI and text location adjustments based on squaresize for consistent look on all devices/screen resolutions

            resetFonts();
                        
            log = new IB2HtmlLogBox(this);
            log.tbXloc = (8 * uiSquareSize) + (int)(8 * scaler);
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

            //setup tooltip
            tooltip = new IBbasicToolTip(this);

            //setup itemListSelector defaults   
            itemListSelector = new IBminiItemListSelector();
            itemListSelector.currentLocX = 20;
            itemListSelector.currentLocY = 10;
            itemListSelector.Width = 344;
            itemListSelector.Height = 220;

            fixedModule = App.fixedModule;

            if (fixedModule.Equals("")) //this is the IceBlink Engine app
            {
                //TODO make sure this works
                sf = new ScriptFunctions(mod, this);
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

            gameTimerStopwatch.Start();
            previousTime = gameTimerStopwatch.ElapsedMilliseconds;
        }
        public void resetScaler(bool useHalfAndFullValues, bool firstTimeThrough)
        {
            if (screenWidth > screenHeight)
            {
                scaler = (float)(screenWidth) / (float)(defaultScreenDesignWidth);
            }
            else
            {
                scaler = (float)(screenHeight) / (float)(defaultScreenDesignHeight);
            }

            float sqrW = (float)screenWidth / (float)(uiSquaresInWidth);
            float sqrH = (float)screenHeight / (float)(uiSquaresInHeight);
            if (sqrW > sqrH)
            {
                scaler = (float)(screenHeight) / (float)(defaultScreenDesignHeight);
            }
            else
            {
                scaler = (float)(screenWidth) / (float)(defaultScreenDesignWidth);
            }

            if (useHalfAndFullValues)
            {
                if ((scaler > 1.4f) && (scaler <= 1.9f)) //336-456
                {
                    scaler = 1.5f;
                }
                else if ((scaler > 1.9f) && (scaler <= 2.6f)) //457-624
                {
                    scaler = 2f;
                }
                else if ((scaler > 2.6f) && (scaler <= 2.9f)) //625-696  iphone SE, 5S 640
                {
                    scaler = 2.5f;
                }
                else if ((scaler > 2.9f) && (scaler <= 3.6f)) //697-864 720 phones, iPhones 750, 768 iPad mini, tabs 800
                {
                    scaler = 3f;
                }
                else if ((scaler > 3.6f) && (scaler <= 3.9f)) //865-936 
                {
                    scaler = 3.5f;
                }
                else if ((scaler > 3.9f) && (scaler <= 4.7f)) //937-1128 iPhones+ 1080, iPhoneX 1125
                {
                    scaler = 4f;
                }
                else if ((scaler > 4.7f) && (scaler <= 4.9f)) //1129-1176
                {
                    scaler = 4.5f;
                }
                else if ((scaler > 4.9f) && (scaler <= 5.6f)) //1177-1344 nexus 7 tab 1200
                {
                    scaler = 5f;
                }
                else if ((scaler > 5.6f) && (scaler <= 5.9f)) //1344-1416
                {
                    scaler = 5.5f;
                }
                else if ((scaler > 5.9f) && (scaler <= 6.6f)) //1417-1584 iPad 1536, lots of androids 1440
                {
                    scaler = 6f;
                }
                else if ((scaler > 6.6f) && (scaler <= 6.9f)) //1585-1656
                {
                    scaler = 6.5f;
                }
                else if ((scaler > 6.9f) && (scaler <= 7.6f)) //1657-1824
                {
                    scaler = 7f;
                }
                else if ((scaler > 7.6f) && (scaler <= 7.9f)) //1825-1896
                {
                    scaler = 7.5f;
                }
                else if ((scaler > 7.9f) && (scaler <= 8.6f)) //1897-2064  2048 iPad Pro
                {
                    scaler = 8f;
                }
                else if ((scaler > 8.6f) && (scaler <= 8.9f)) //2065-2136
                {
                    scaler = 8.5f;
                }
                else if ((scaler > 8.9f) && (scaler <= 9.6f)) //2137-2304
                {
                    scaler = 9f;
                }
                else
                {
                    scaler = (int)scaler;
                }
            }

            uiSquareSize = (int)(uiSquareSizeDefault * scaler);

            oXshift = (int)((screenWidth - (scaler * defaultScreenDesignWidth)) / 2);
            oYshift = (int)((screenHeight - (scaler * defaultScreenDesignHeight)) / 2);

            if (!firstTimeThrough)
            {
                resetFonts();

                log.tbXloc = (8 * uiSquareSize) + (int)(8 * scaler);
                log.tbYloc = 2;
                log.tbWidth = 3 * uiSquareSize; //add one char because the word wrap calculates word length plus one space at end
                log.tbHeight = 4 * uiSquareSize;
                log.numberOfLinesToShow = 14;
            }
        }
        public void resetFonts()
        {
            //reload standard font images
            cc.fontBk = cc.LoadBitmap("font");
            changeFontColor(cc.fontBk, 0, 0, 0);
            cc.fontBu = cc.LoadBitmap("font");
            changeFontColor(cc.fontBu, 0, 127, 255);
            cc.fontGn = cc.LoadBitmap("font");
            changeFontColor(cc.fontGn, 0, 255, 0);
            cc.fontGy = cc.LoadBitmap("font");
            changeFontColor(cc.fontGy, 155, 155, 155);
            cc.fontMa = cc.LoadBitmap("font");
            changeFontColor(cc.fontMa, 255, 0, 255);
            cc.fontRd = cc.LoadBitmap("font");
            changeFontColor(cc.fontRd, 255, 0, 0);
            cc.fontWh = cc.LoadBitmap("font");
            changeFontColor(cc.fontWh, 255, 255, 255);
            cc.fontYl = cc.LoadBitmap("font");
            changeFontColor(cc.fontYl, 255, 255, 0);

            //CREATES A FONTFAMILY
            charList.Clear();
            fontWidth = cc.fontBk.Width / 13;
            fontHeight = cc.fontBk.Height / 7;
            //fontWidth = 6;
            //fontHeight = 9;
            fillCharList();
            fontWidth = (int)(fontWidth * scaler);
            fontHeight = (int)(fontHeight * scaler);
            fontCharSpacing = (int)(0 * scaler);
            fontLineSpacing = (int)(0 * scaler);
        }
        public void changeFontColor(SKBitmap b, byte R, byte G, byte B)
        {            
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    if (b.GetPixel(x, y).Alpha > 30)
                    {
                        b.SetPixel(x, y, new SKColor(R, G, B, 255));
                    }
                }
            }            
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
            screenPartyCampaign = new ScreenPartyCampaign(mod, this);
            screenPartyRoster = new ScreenPartyRoster(mod,this);

            //TOOLSET SCREENS
            tsModule = new ToolsetScreenModule(this);
            tsAreaEditor = new ToolsetScreenAreaEditor(this);
            tsEncEditor = new ToolsetScreenEncounterEditor(this);
            tsMainMenu = new ToolsetScreenMainMenu(this);
            tsConvoEditor = new ToolsetScreenConvoEditor(this);
            tsContainerEditor = new ToolsetScreenContainerEditor(this);
            tsShopEditor = new ToolsetScreenShopEditor(this);
            tsJournalEditor = new ToolsetScreenJournalEditor(this);
            tsCreatureEditor = new ToolsetScreenCreatureEditor(this);
            tsItemEditor = new ToolsetScreenItemEditor(this);
            tsPlayerEditor = new ToolsetScreenPlayerEditor(this);
            tsArtEditor = new ToolsetScreenArtEditor(this);
            tsDataCheck = new ToolsetScreenDataCheck(this);

        }
        public void LoadStandardImages()
        {
            cc.walkPass = cc.LoadBitmap("walk_pass");
            cc.walkBlocked = cc.LoadBitmap("walk_block");
            cc.losBlocked = cc.LoadBitmap("los_block");
            cc.black_tile = cc.LoadBitmap("black_tile");
            cc.turn_marker = cc.LoadBitmap("turn_marker");
            cc.map_marker = cc.LoadBitmap("map_marker");
            cc.pc_dead = cc.LoadBitmap("pcdead");
            cc.pc_stealth = cc.LoadBitmap("pc_stealth");
            cc.hitSymbol = cc.LoadBitmap("hit_symbol");
            cc.missSymbol = cc.LoadBitmap("miss_symbol");
            cc.highlight_green = cc.LoadBitmap("highlight_green");
            cc.highlight_red = cc.LoadBitmap("highlight_red");
            cc.tint_dawn = cc.LoadBitmap("tint_dawn");
            cc.tint_sunrise = cc.LoadBitmap("tint_sunrise");
            cc.tint_sunset = cc.LoadBitmap("tint_sunset");
            cc.tint_dusk = cc.LoadBitmap("tint_dusk");
            cc.tint_night = cc.LoadBitmap("tint_night");
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
            cc.saveMod = new SaveGame();
            cc.commonBitmapList.Clear();
            resetFonts();
            //reset log number of lines based on the value from the Module's mod file
            log.numberOfLinesToShow = mod.logNumberOfLines;            
                        
		    mod.debugMode = false;
		    bool foundArea = mod.setCurrentArea(mod.startingArea, this);
            if (!foundArea)
            {
                //sf.MessageBox("Area: " + mod.startingArea + " does not exist in the module...check the spelling or make sure your are pointing to the correct starting area that you intended");
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
        public void loadPreferences()
        {
            IBprefs = new IBbasicPrefernces();
            string s = LoadStringFromEitherFolder("\\IBbasicPreferences.json", "\\IBbasicPreferences.json");
            if (s != "")
            {
                try
                {
                    using (StringReader sr = new StringReader(s))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        IBprefs = (IBbasicPrefernces)serializer.Deserialize(sr, typeof(IBbasicPrefernces));
                    }
                }
                catch { }
            }
        }
        public void savePreferences()
        {
            try
            {
                string json = JsonConvert.SerializeObject(IBprefs, Newtonsoft.Json.Formatting.Indented);
                SaveText("\\IBbasicPreferences.json", json);
            }
            catch { }
        }
        public void loadSettings()
        {
            toggleSettings = new Settings();
            string s = LoadStringFromEitherFolder("\\settings.json", "\\settings.json");
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
            string json = JsonConvert.SerializeObject(toggleSettings, Newtonsoft.Json.Formatting.Indented);
            SaveText("\\settings.json", json);            
        }
        private void fillCharListold()
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
            charList.Add('K', new SKRect(fontWidth * 10, fontHeight * 0, fontWidth * 10 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('L', new SKRect(fontWidth * 11, fontHeight * 0, fontWidth * 11 + fontWidth, fontHeight * 0 + fontHeight));
            charList.Add('M', new SKRect(fontWidth * 12, fontHeight * 0, fontWidth * 12 + fontWidth, fontHeight * 0 + fontHeight));

            charList.Add('N', new SKRect(fontWidth * 0, fontHeight * 1, fontWidth * 0 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('O', new SKRect(fontWidth * 1, fontHeight * 1, fontWidth * 1 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('P', new SKRect(fontWidth * 2, fontHeight * 1, fontWidth * 2 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('Q', new SKRect(fontWidth * 3, fontHeight * 1, fontWidth * 3 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('R', new SKRect(fontWidth * 4, fontHeight * 1, fontWidth * 4 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('S', new SKRect(fontWidth * 5, fontHeight * 1, fontWidth * 5 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('T', new SKRect(fontWidth * 6, fontHeight * 1, fontWidth * 6 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('U', new SKRect(fontWidth * 7, fontHeight * 1, fontWidth * 7 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('V', new SKRect(fontWidth * 8, fontHeight * 1, fontWidth * 8 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('W', new SKRect(fontWidth * 9, fontHeight * 1, fontWidth * 9 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('X', new SKRect(fontWidth * 10, fontHeight * 1, fontWidth * 10 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('Y', new SKRect(fontWidth * 11, fontHeight * 1, fontWidth * 11 + fontWidth, fontHeight * 1 + fontHeight));
            charList.Add('Z', new SKRect(fontWidth * 12, fontHeight * 1, fontWidth * 12 + fontWidth, fontHeight * 1 + fontHeight));

            charList.Add('a', new SKRect(fontWidth * 0, fontHeight * 2, fontWidth * 0 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('b', new SKRect(fontWidth * 1, fontHeight * 2, fontWidth * 1 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('c', new SKRect(fontWidth * 2, fontHeight * 2, fontWidth * 2 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('d', new SKRect(fontWidth * 3, fontHeight * 2, fontWidth * 3 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('e', new SKRect(fontWidth * 4, fontHeight * 2, fontWidth * 4 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('f', new SKRect(fontWidth * 5, fontHeight * 2, fontWidth * 5 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('g', new SKRect(fontWidth * 6, fontHeight * 2, fontWidth * 6 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('h', new SKRect(fontWidth * 7, fontHeight * 2, fontWidth * 7 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('i', new SKRect(fontWidth * 8, fontHeight * 2, fontWidth * 8 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('j', new SKRect(fontWidth * 9, fontHeight * 2, fontWidth * 9 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('k', new SKRect(fontWidth * 10, fontHeight * 2, fontWidth * 10 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('l', new SKRect(fontWidth * 11, fontHeight * 2, fontWidth * 11 + fontWidth, fontHeight * 2 + fontHeight));
            charList.Add('m', new SKRect(fontWidth * 12, fontHeight * 2, fontWidth * 12 + fontWidth, fontHeight * 2 + fontHeight));

            charList.Add('n', new SKRect(fontWidth * 0, fontHeight * 3, fontWidth * 0 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('o', new SKRect(fontWidth * 1, fontHeight * 3, fontWidth * 1 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('p', new SKRect(fontWidth * 2, fontHeight * 3, fontWidth * 2 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('q', new SKRect(fontWidth * 3, fontHeight * 3, fontWidth * 3 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('r', new SKRect(fontWidth * 4, fontHeight * 3, fontWidth * 4 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('s', new SKRect(fontWidth * 5, fontHeight * 3, fontWidth * 5 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('t', new SKRect(fontWidth * 6, fontHeight * 3, fontWidth * 6 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('u', new SKRect(fontWidth * 7, fontHeight * 3, fontWidth * 7 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('v', new SKRect(fontWidth * 8, fontHeight * 3, fontWidth * 8 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('w', new SKRect(fontWidth * 9, fontHeight * 3, fontWidth * 9 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('x', new SKRect(fontWidth * 10, fontHeight * 3, fontWidth * 10 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('y', new SKRect(fontWidth * 11, fontHeight * 3, fontWidth * 11 + fontWidth, fontHeight * 3 + fontHeight));
            charList.Add('z', new SKRect(fontWidth * 12, fontHeight * 3, fontWidth * 12 + fontWidth, fontHeight * 3 + fontHeight));

            charList.Add('0', new SKRect(fontWidth * 0, fontHeight * 4, fontWidth * 0 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('1', new SKRect(fontWidth * 1, fontHeight * 4, fontWidth * 1 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('2', new SKRect(fontWidth * 2, fontHeight * 4, fontWidth * 2 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('3', new SKRect(fontWidth * 3, fontHeight * 4, fontWidth * 3 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('4', new SKRect(fontWidth * 4, fontHeight * 4, fontWidth * 4 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('5', new SKRect(fontWidth * 5, fontHeight * 4, fontWidth * 5 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('6', new SKRect(fontWidth * 6, fontHeight * 4, fontWidth * 6 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('7', new SKRect(fontWidth * 7, fontHeight * 4, fontWidth * 7 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('8', new SKRect(fontWidth * 8, fontHeight * 4, fontWidth * 8 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('9', new SKRect(fontWidth * 9, fontHeight * 4, fontWidth * 9 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('.', new SKRect(fontWidth * 10, fontHeight * 4, fontWidth * 10 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add(',', new SKRect(fontWidth * 11, fontHeight * 4, fontWidth * 11 + fontWidth, fontHeight * 4 + fontHeight));
            charList.Add('"', new SKRect(fontWidth * 12, fontHeight * 4, fontWidth * 12 + fontWidth, fontHeight * 4 + fontHeight));

            charList.Add('\'', new SKRect(fontWidth * 0, fontHeight * 5, fontWidth * 0 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('?', new SKRect(fontWidth * 1, fontHeight * 5, fontWidth * 1 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('!', new SKRect(fontWidth * 2, fontHeight * 5, fontWidth * 2 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('~', new SKRect(fontWidth * 3, fontHeight * 5, fontWidth * 3 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('#', new SKRect(fontWidth * 4, fontHeight * 5, fontWidth * 4 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('$', new SKRect(fontWidth * 5, fontHeight * 5, fontWidth * 5 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('%', new SKRect(fontWidth * 6, fontHeight * 5, fontWidth * 6 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('^', new SKRect(fontWidth * 7, fontHeight * 5, fontWidth * 7 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('&', new SKRect(fontWidth * 8, fontHeight * 5, fontWidth * 8 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('*', new SKRect(fontWidth * 9, fontHeight * 5, fontWidth * 9 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('(', new SKRect(fontWidth * 10, fontHeight * 5, fontWidth * 10 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add(')', new SKRect(fontWidth * 11, fontHeight * 5, fontWidth * 11 + fontWidth, fontHeight * 5 + fontHeight));
            charList.Add('-', new SKRect(fontWidth * 12, fontHeight * 5, fontWidth * 12 + fontWidth, fontHeight * 5 + fontHeight));

            charList.Add('_', new SKRect(fontWidth * 0, fontHeight * 6, fontWidth * 0 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('+', new SKRect(fontWidth * 1, fontHeight * 6, fontWidth * 1 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('=', new SKRect(fontWidth * 2, fontHeight * 6, fontWidth * 2 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('[', new SKRect(fontWidth * 3, fontHeight * 6, fontWidth * 3 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add(']', new SKRect(fontWidth * 4, fontHeight * 6, fontWidth * 4 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('/', new SKRect(fontWidth * 5, fontHeight * 6, fontWidth * 5 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add(':', new SKRect(fontWidth * 6, fontHeight * 6, fontWidth * 6 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('|', new SKRect(fontWidth * 7, fontHeight * 6, fontWidth * 7 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add(';', new SKRect(fontWidth * 8, fontHeight * 6, fontWidth * 8 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('<', new SKRect(fontWidth * 9, fontHeight * 6, fontWidth * 9 + fontWidth, fontHeight * 6 + fontHeight));
            charList.Add('>', new SKRect(fontWidth * 10, fontHeight * 6, fontWidth * 10 + fontWidth, fontHeight * 6 + fontHeight));
            //charList.Add('/', new SharpDX.SKRect(64, 64, 8, 12));
            charList.Add(' ', new SKRect(fontWidth * 12, fontHeight * 6, fontWidth * 12 + fontWidth, fontHeight * 6 + fontHeight));
        }
                                
        //Animation Timer Stuff
        public void postDelayed(string type, int delay)
        {
            animationTimerOn = true;
            animationStartTime = gameTimerStopwatch.ElapsedMilliseconds; //get the current total amount of ms since the game launched
            animationDelayTime = delay;
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
        private void AnimationTimer_Tick()
        {
            animationTimerOn = false;
            screenCombat.doAnimationController();
        }
        
        public void gameTimer_Tick(SKCanvasView sk_canvas)
        {
            if (!stillProcessingGameLoop)
            {
                stillProcessingGameLoop = true; //starting the game loop so do not allow another tick call to run until finished with this tick call.
                long current = gameTimerStopwatch.ElapsedMilliseconds; //get the current total amount of ms since the game launched
                int elapsed = (int)(current - previousTime); //calculate the total ms elapsed since the last time through the game loop
                if (animationTimerOn) //do combat animation stuff
                {
                    long timePassed = current - animationStartTime;
                    if (timePassed > animationDelayTime)
                    {
                        AnimationTimer_Tick();
                    }
                }
                Update(elapsed); //runs AI and physics
                sk_canvas.InvalidateSurface(); //draw the screen frame
                if (reportFPScount >= 10)
                {
                    reportFPScount = 0;
                    long denom = current - previousTime;
                    if (denom != 0)
                    {
                        fps = 1000 / denom;
                    }
                }
                if (checkAudioPlayBack >= 10)
                {
                    checkAudioPlayBack = 0;
                    if (mod.playSoundFx)
                    {
                        RestartAreaMusicIfEnded();
                    }
                }
                checkAudioPlayBack++;
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
        public void DrawText(SKCanvas canvas, string text, float xLoc, float yLoc, string color)
        {
            if (canvas == null)
            {
                return;
            }
            //default is WHITE
            /*SKBitmap bm = cc.GetFromBitmapList("fontWh");
            if (color.Equals("bk"))
            {
                bm = cc.GetFromBitmapList("fontBk");
            }
            else if (color.Equals("bu"))
            {
                bm = cc.GetFromBitmapList("fontBu");
            }
            else if (color.Equals("gn"))
            {
                bm = cc.GetFromBitmapList("fontGn");
            }
            else if (color.Equals("gy"))
            {
                bm = cc.GetFromBitmapList("fontGy");
            }
            else if (color.Equals("ma"))
            {
                bm = cc.GetFromBitmapList("fontMa");
            }
            else if (color.Equals("rd"))
            {
                bm = cc.GetFromBitmapList("fontRd");
            }
            else if (color.Equals("yl"))
            {
                bm = cc.GetFromBitmapList("fontYl");
            }*/
            try
            {
                float x = 0;
                foreach (char c in text)
                {
                    if (c == '\r') { continue; }
                    if (c == '\n') { continue; }
                    char c1 = '0';
                    if (!charList.ContainsKey(c)) { c1 = '#'; }
                    else c1 = c;
                    if (color.Equals("bk"))
                    {
                        if (cc.fontBk == null) { this.resetFonts(); }
                        canvas.DrawBitmap(cc.fontBk, charList[c1], new SKRect(xLoc + x + oXshift, yLoc + oYshift, xLoc + x + oXshift + fontWidth, yLoc + oYshift + fontHeight));
                    }
                    else if (color.Equals("wh"))
                    {
                        if (cc.fontWh == null) { this.resetFonts(); }
                        canvas.DrawBitmap(cc.fontWh, charList[c1], new SKRect(xLoc + x + oXshift, yLoc + oYshift, xLoc + x + oXshift + fontWidth, yLoc + oYshift + fontHeight));
                    }
                    else if (color.Equals("bu"))
                    {
                        if (cc.fontBu == null) { this.resetFonts(); }
                        canvas.DrawBitmap(cc.fontBu, charList[c1], new SKRect(xLoc + x + oXshift, yLoc + oYshift, xLoc + x + oXshift + fontWidth, yLoc + oYshift + fontHeight));
                    }
                    else if (color.Equals("gn"))
                    {
                        if (cc.fontGn == null) { this.resetFonts(); }
                        canvas.DrawBitmap(cc.fontGn, charList[c1], new SKRect(xLoc + x + oXshift, yLoc + oYshift, xLoc + x + oXshift + fontWidth, yLoc + oYshift + fontHeight));
                    }
                    else if (color.Equals("gy"))
                    {
                        if (cc.fontGy == null) { this.resetFonts(); }
                        canvas.DrawBitmap(cc.fontGy, charList[c1], new SKRect(xLoc + x + oXshift, yLoc + oYshift, xLoc + x + oXshift + fontWidth, yLoc + oYshift + fontHeight));
                    }
                    else if (color.Equals("ma"))
                    {
                        if (cc.fontMa == null) { this.resetFonts(); }
                        canvas.DrawBitmap(cc.fontMa, charList[c1], new SKRect(xLoc + x + oXshift, yLoc + oYshift, xLoc + x + oXshift + fontWidth, yLoc + oYshift + fontHeight));
                    }
                    else if (color.Equals("rd"))
                    {
                        if (cc.fontRd == null) { this.resetFonts(); }
                        canvas.DrawBitmap(cc.fontRd, charList[c1], new SKRect(xLoc + x + oXshift, yLoc + oYshift, xLoc + x + oXshift + fontWidth, yLoc + oYshift + fontHeight));
                    }
                    else if (color.Equals("yl"))
                    {
                        if (cc.fontYl == null) { this.resetFonts(); }
                        canvas.DrawBitmap(cc.fontYl, charList[c1], new SKRect(xLoc + x + oXshift, yLoc + oYshift, xLoc + x + oXshift + fontWidth, yLoc + oYshift + fontHeight));
                    }
                    //canvas.DrawBitmap(bm, charList[c1], new SKRect(xLoc + x + oXshift, yLoc + oYshift, xLoc + x + oXshift + fontWidth, yLoc + oYshift + fontHeight));
                    x += fontWidth + fontCharSpacing;
                }
            }
            catch { }
        }        
        public void DrawLine(SKCanvas canvas, int lastX, int lastY, int nextX, int nextY, string penColor, int penWidth)
        {
            if (canvas == null)
            {
                return;
            }
            //string clr = "red";
            /*using (SKPaint skp = new SKPaint())
            {
                skp.IsAntialias = true;
                skp.Style = SKPaintStyle.Stroke;
                skp.Color = penColor;
                skp.StrokeWidth = penWidth;
                canvas.DrawLine(lastX + oXshift, lastY + oYshift, nextX + oXshift, nextY + oYshift, skp);
                //renderTarget2D.DrawLine(new Vector2(lastX + oXshift, lastY + oYshift), new Vector2(nextX + oXshift, nextY + oYshift), scb, penWidth);
            }*/
            try
            {
                if (penColor.Equals("red"))
                {
                    using (SKPaint skp = new SKPaint())
                    {
                        skp.IsAntialias = true;
                        skp.Style = SKPaintStyle.Stroke;
                        skp.Color = SkiaSharp.SKColors.Red;
                        skp.StrokeWidth = penWidth;
                        canvas.DrawLine(lastX + oXshift, lastY + oYshift, nextX + oXshift, nextY + oYshift, skp);
                    }
                }
                else if (penColor.Equals("green"))
                {
                    using (SKPaint skp = new SKPaint())
                    {
                        skp.IsAntialias = true;
                        skp.Style = SKPaintStyle.Stroke;
                        skp.Color = SkiaSharp.SKColors.Lime;
                        skp.StrokeWidth = penWidth;
                        canvas.DrawLine(lastX + oXshift, lastY + oYshift, nextX + oXshift, nextY + oYshift, skp);
                    }
                }
                else if (penColor.Equals("black"))
                {
                    using (SKPaint skp = new SKPaint())
                    {
                        skp.IsAntialias = true;
                        skp.Style = SKPaintStyle.Stroke;
                        skp.Color = SkiaSharp.SKColors.Black;
                        skp.StrokeWidth = penWidth;
                        canvas.DrawLine(lastX + oXshift, lastY + oYshift, nextX + oXshift, nextY + oYshift, skp);
                    }
                }
            }
            catch { }
        }
        public void DrawRectangle(SKCanvas canvas, IbRect rect, SKColor penColor, int penWidth)
        {
            if (canvas == null)
            {
                return;
            }
            try
            {
                using (SKPaint skp = new SKPaint())
                {
                    SKRect SKRectangle = new SKRect();
                    SKRectangle.Left = rect.Left + oXshift;
                    SKRectangle.Top = rect.Top + oYshift;
                    SKRectangle.Bottom = SKRectangle.Top + rect.Height;
                    SKRectangle.Right = SKRectangle.Left + rect.Width;

                    skp.IsAntialias = true;
                    skp.Style = SKPaintStyle.Stroke;
                    skp.Color = penColor;
                    skp.StrokeWidth = penWidth;
                    canvas.DrawRect(SKRectangle, skp);
                }
            }
            catch { }
        }
        public void DrawBitmap(SKCanvas canvas, SKBitmap bitmap, IbRect source, IbRect destination)
        {
            if (canvas == null)
            {
                return;
            }
            try
            {
                //used for most everything
                SKRect dst = new SKRect(destination.Left + oXshift, destination.Top + oYshift, destination.Left + oXshift + destination.Width, destination.Top + oYshift + destination.Height);
                SKRect src = new SKRect(source.Left, source.Top, source.Left + source.Width, source.Top + source.Height);
                canvas.DrawBitmap(bitmap, src, dst);
            }
            catch { }
        }
        public void DrawBitmap(SKCanvas canvas, SKBitmap bitmap, IbRect source, IbRect destination, bool mirror)
        {
            //used for drawing PCs, creatures and Props
            DrawBitmap(canvas, bitmap, source, destination, 0.0f, mirror);
        }
        public void DrawBitmap(SKCanvas canvas, SKBitmap bitmap, IbRect source, IbRect destination, int angleInDegrees, bool mirror)
        {
            // used for tiles
            //convert degrees to radians
            float angleInRadians = (angleInDegrees * (float)(Math.PI * 2)) / 360f;
            DrawBitmap(canvas, bitmap, source, destination, angleInRadians, mirror);
        }
        public void DrawBitmap(SKCanvas canvas, SKBitmap bitmap, IbRect source, IbRect destination, float angleInRadians, bool mirror)
        {
            if (canvas == null)
            {
                return;
            }
            try
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
                //SKRect dst = new SKRect(dstRect.Left, dstRect.Top, dstRect.Right, dstRect.Bottom);
                //SKRect src = new SKRect(srcRect.Left, srcRect.Top, srcRect.Right, srcRect.Bottom);
                canvas.DrawBitmap(bitmap, srcRect, dstRect);
                canvas.Restore();
            }
            catch { }
        }

        public void Render(SKCanvas c)
        {
            //canvas = c;
            //BeginDraw(); //uncomment this for DIRECT2D ADDITIONS  

            //renderTarget2D.Clear(Color4.Black); //uncomment this for DIRECT2D ADDITIONS

            if ((mod.useUIBackground) && (!screenType.Equals("tsAreaEditor")) && (!screenType.Equals("tsConvoEditor")) && (!screenType.Equals("tsEncEditor")) && (!screenType.Equals("main")) && (!screenType.Equals("combat")) && (!screenType.Equals("launcher")) && (!screenType.Equals("title")))
            {
                drawUIBackground(c);
            }
            //TOOLSET SCREENS
            if (screenType.Equals("tsModule"))
            {
                tsModule.redrawTsModule(c);
            }
            else if (screenType.Equals("tsDataCheck"))
            {
                tsDataCheck.redrawTsDataCheck(c);
            }
            else if (screenType.Equals("tsAreaEditor"))
            {
                tsAreaEditor.redrawTsAreaEditor(c);
            }
            else if (screenType.Equals("tsEncEditor"))
            {
                tsEncEditor.redrawTsEncEditor(c);
            }
            else if (screenType.Equals("tsConvoEditor"))
            {
                tsConvoEditor.redrawTsConvoEditor(c);
            }
            else if (screenType.Equals("tsContainerEditor"))
            {
                tsContainerEditor.redrawTsContainerEditor(c);
            }
            else if (screenType.Equals("tsShopEditor"))
            {
                tsShopEditor.redrawTsShopEditor(c);
            }
            else if (screenType.Equals("tsJournalEditor"))
            {
                tsJournalEditor.redrawTsJournalEditor(c);
            }
            else if (screenType.Equals("tsCreatureEditor"))
            {
                tsCreatureEditor.redrawTsCreatureEditor(c);
            }
            else if (screenType.Equals("tsItemEditor"))
            {
                tsItemEditor.redrawTsItemEditor(c);
            }
            else if (screenType.Equals("tsPlayerEditor"))
            {
                tsPlayerEditor.redrawTsPlayerEditor(c);
            }
            else if (screenType.Equals("tsArtEditor"))
            {
                tsArtEditor.redrawTsArtEditor(c);
            }
            //GAME SCREENS
            else if (screenType.Equals("title"))
            {
                screenTitle.redrawTitle(c);
            }
            else if (screenType.Equals("launcher"))
            {
                screenLauncher.redrawLauncher(c);
            }
            else if (screenType.Equals("splash"))
            {
                screenSplash.redrawSplash(c);
            }
            else if (screenType.Equals("pcCreation"))
            {
                screenPcCreation.redrawPcCreation(c);
            }
            else if (screenType.Equals("learnSpellCreation"))
            {
                screenSpellLevelUp.redrawSpellLevelUp(c, true);
            }
            else if (screenType.Equals("learnSpellLevelUp"))
            {
                screenSpellLevelUp.redrawSpellLevelUp(c, false);
            }
            else if (screenType.Equals("learnTraitCreation"))
            {
                screenTraitLevelUp.redrawTraitLevelUp(c, true);
            }
            else if (screenType.Equals("learnTraitLevelUp"))
            {
                screenTraitLevelUp.redrawTraitLevelUp(c, false);
            }
            else if (screenType.Equals("main"))
            {
                screenMainMap.redrawMain(c);
            }
            else if (screenType.Equals("party"))
            {
                screenParty.redrawParty(c);
            }
            else if (screenType.Equals("combatParty"))
            {
                screenParty.redrawParty(c);
            }
            else if (screenType.Equals("inventory"))
            {
                screenInventory.redrawInventory(c);
            }
            else if (screenType.Equals("itemSelector"))
            {
                screenItemSelector.redrawItemSelector(c);
            }
            else if (screenType.Equals("portraitSelector"))
            {
                screenPortraitSelector.redrawPortraitSelector(c);
            }
            else if (screenType.Equals("tokenSelector"))
            {
                screenTokenSelector.redrawTokenSelector(c);
            }
            else if (screenType.Equals("pcSelector"))
            {
                screenPcSelector.redrawPcSelector(c);
            }
            else if (screenType.Equals("combatInventory"))
            {
                screenInventory.redrawInventory(c);
            }
            else if (screenType.Equals("journal"))
            {
                screenJournal.redrawJournal(c);
            }
            else if (screenType.Equals("shop"))
            {
                screenShop.redrawShop(c);
            }
            else if (screenType.Equals("combat"))
            {
                screenCombat.redrawCombat(c);
            }
            else if (screenType.Equals("combatCast"))
            {
                screenCastSelector.redrawCastSelector(c, true);
            }
            else if (screenType.Equals("mainMapCast"))
            {
                screenCastSelector.redrawCastSelector(c, false);
            }
            else if (screenType.Equals("combatTraitUse"))
            {
                screenTraitUseSelector.redrawTraitUseSelector(c, true);
            }
            else if (screenType.Equals("mainMapTraitUse"))
            {
                screenTraitUseSelector.redrawTraitUseSelector(c, false);
            }
            else if (screenType.Equals("convo"))
            {
                screenConvo.redrawConvo(c);
            }
            else if (screenType.Equals("partyBuild"))
            {
                screenPartyBuild.redrawPartyBuild(c);
            }
            else if (screenType.Equals("partyCampaign"))
            {
                screenPartyCampaign.redrawPartyCampaign(c);
            }
            else if (screenType.Equals("partyRoster"))
            {
                screenPartyRoster.redrawPartyRoster(c);
            }
            if (mod.debugMode)
            {
                int txtH = (int)fontHeight;
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 2; y++)
                    {
                        DrawText(c, "FPS:" + fps.ToString(), x + 5, screenHeight - oYshift - txtH - 5 + y, "bk");
                    }
                }
                DrawText(c, "FPS:" + fps.ToString(), 5, screenHeight - oYshift - txtH - 5, "wh");
            }
            if (itemListSelector.showIBminiItemListSelector)
            {
                itemListSelector.drawItemListSelection(c);
            }
            if (showTooltip)
            {
                tooltip.onDrawLogBox(c);
            }
            //EndDraw(); //uncomment this for DIRECT2D ADDITIONS
        }
        public void drawUIBackground(SKCanvas canvas)
        {
            try
            {
                IbRect src = new IbRect(0, 0, cc.ui_bg_fullscreen.Width, cc.ui_bg_fullscreen.Height);
                IbRect dst = new IbRect(0 - oXshift, 0 - oYshift, screenWidth, screenHeight);
                DrawBitmap(canvas, cc.ui_bg_fullscreen, src, dst);
            }
            catch
            { }
        }

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
                        if (messageBox.btnReturn.getImpact(eX, eY))
                        {
                            if (eventType == MouseEventType.EventType.MouseUp)
                            {
                                showMessageBox = false;
                                return;
                            }
                        }
                        return;
                    }
                    else if ((screenType.Equals("main")) || (screenType.Equals("combat")))
                    {
                        log.onTouchSwipe(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsConvoEditor"))
                    {
                        tsConvoEditor.onTouchSwipe(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsItemEditor"))
                    {
                        tsItemEditor.onTouchSwipe(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsCreatureEditor"))
                    {
                        tsCreatureEditor.onTouchSwipe(eX, eY, eventType);
                    }

                    //TOOLSET SCREENS
                    if (screenType.Equals("tsModule"))
                    {
                        tsModule.onTouchTsModule(eX, eY, eventType);	
                    }
                    else if (screenType.Equals("tsDataCheck"))
                    {
                        tsDataCheck.onTouchTsDataCheck(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsAreaEditor"))
                    {
                        tsAreaEditor.onTouchTsAreaEditor(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsEncEditor"))
                    {
                        tsEncEditor.onTouchTsEncEditor(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsConvoEditor"))
                    {
                        tsConvoEditor.onTouchTsConvoEditor(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsContainerEditor"))
                    {
                        tsContainerEditor.onTouchTsContainerEditor(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsShopEditor"))
                    {
                        tsShopEditor.onTouchTsShopEditor(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsJournalEditor"))
                    {
                        tsJournalEditor.onTouchTsJournalEditor(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsCreatureEditor"))
                    {
                        tsCreatureEditor.onTouchTsCreatureEditor(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsItemEditor"))
                    {
                        tsItemEditor.onTouchTsItemEditor(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsPlayerEditor"))
                    {
                        tsPlayerEditor.onTouchTsPlayerEditor(eX, eY, eventType);
                    }
                    else if (screenType.Equals("tsArtEditor"))
                    {
                        tsArtEditor.onTouchTsArtEditor(eX, eY, eventType);
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
                    else if (screenType.Equals("partyCampaign"))
                    {
                        screenPartyCampaign.onTouchPartyCampaign(eX, eY, eventType);
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
        public Task<string> StringInputBox(string headerText, string existingTextInputValue)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Text Entry", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = headerText };
            //var txtInput = new Editor { Text = existingTextInputValue, AutoSize = EditorAutoSizeOption.TextChanges }; //multi-line
            var txtInput = new Entry { Text = existingTextInputValue }; //single-line

            //txtInput.HorizontalOptions = LayoutOptions.FillAndExpand;
            //txtInput.VerticalOptions = LayoutOptions.FillAndExpand;
            //txtInput.On<Android>().SetImeOptions(ImeFlags.None);

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Xamarin.Forms.Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                //Small delay
                await Task.Delay(100);
                // close page
                var result = txtInput.Text;
                await cp.Navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Xamarin.Forms.Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                //Small delay
                await Task.Delay(100);
                // close page
                await cp.Navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(existingTextInputValue);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            cp.Navigation.PushModalAsync(page);
            // open keyboard
            //txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result

            return tcs.Task;           
        }
        public Task<int> NumInputBox(string headerText, int existingIntValue)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<int>();

            var lblTitle = new Label { Text = "Integer Entry", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = headerText };
            var txtInput = new Entry { Text = existingIntValue.ToString(), Keyboard = Keyboard.Numeric };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Xamarin.Forms.Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                int resultInt = existingIntValue;
                try
                {
                    resultInt = Convert.ToInt32(txtInput.Text);
                }
                catch
                {
                    await cp.DisplayAlert("Error!", "That was not an integer...returning original value.", "OK");
                }
                //var result = txtInput.Text;
                await cp.Navigation.PopModalAsync();
                // pass result
                tcs.SetResult(resultInt);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Xamarin.Forms.Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                await cp.Navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(existingIntValue);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            cp.Navigation.PushModalAsync(page);
            // open keyboard
            //txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }
        public Task<string> ListViewPage(List<string> list, string headerText)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();
            
            // create and show page
            var page = new ListViewPage1(tcs, list, headerText);
            cp.Navigation.PushModalAsync(page);
            
            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }
        public Task<bool> DownloadResult(string url, string folder)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<bool>();

            // create and show page
            bool rst = DownloadFile(url, folder);
            if (rst)
            {
                tcs.SetResult(true);
            }
            else
            {
                tcs.SetResult(false);
            }
            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
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
        public void IBMessageBox(string title, string message)
        {
            cp.DisplayAlert(title, message, "OK");
        }

        //PLATFORM SPECIFIC CALLS
        public string GetVersion()
        {
            return DependencyService.Get<ISaveAndLoad>().GetVersion();
        }
        public void RateApp()
        {
            DependencyService.Get<ISaveAndLoad>().RateApp();
        }
        public bool AllowReadWriteExternal()
        {
            return DependencyService.Get<ISaveAndLoad>().AllowReadWriteExternal();
        }
        public void CreateUserFolders() //called at start-up
        {
            DependencyService.Get<ISaveAndLoad>().CreateUserFolders();
        }
        public void CreateBackUpModuleFolder(string modFilename) //called from toolset button disk+
        {
            DependencyService.Get<ISaveAndLoad>().CreateBackUpModuleFolder(modFilename);
        }
        public void SaveText(string fullPath, string text) //save games, save characters, save mod files, save preferences, save settings
        {
            DependencyService.Get<ISaveAndLoad>().SaveText(fullPath, text);
        }
        public void SaveImage(string fullPath, SKBitmap bmp) //save from art editor
        {
            DependencyService.Get<ISaveAndLoad>().SaveImage(fullPath, bmp);
        }
        public void ZipModule(string modFilename)
        {
            DependencyService.Get<ISaveAndLoad>().ZipModule(modFilename);
        }
        public void UnZipModule(string modFilename)
        {
            DependencyService.Get<ISaveAndLoad>().UnZipModule(modFilename);
        }
        public void SubmitModule(string modFilename)
        {
            DependencyService.Get<ISaveAndLoad>().SubmitModule(modFilename);
        }
        public string LoadStringFromUserFolder(string fullPath)
        {
            return DependencyService.Get<ISaveAndLoad>().LoadStringFromUserFolder(fullPath);
        }
        public string LoadStringFromAssetFolder(string fullPath)
        {
            return DependencyService.Get<ISaveAndLoad>().LoadStringFromAssetFolder(fullPath);
        }
        public string LoadStringFromEitherFolder(string assetFolderpath, string userFolderpath)
        {
            return DependencyService.Get<ISaveAndLoad>().LoadStringFromEitherFolder(assetFolderpath, userFolderpath);
        }
        public List<string> GetAllFilesWithExtensionFromUserFolder(string folderpath, string extension)
        {
            return DependencyService.Get<ISaveAndLoad>().GetAllFilesWithExtensionFromUserFolder(folderpath, extension);
        }
        public List<string> GetAllFilesWithExtensionFromBothFolders(string assetFolderpath, string userFolderpath, string extension)
        {
            return DependencyService.Get<ISaveAndLoad>().GetAllFilesWithExtensionFromBothFolders(assetFolderpath, userFolderpath, extension);
        }
        public string GetModuleFileString(string modFilename)
        {
            return DependencyService.Get<ISaveAndLoad>().GetModuleFileString(modFilename);
        }
        public SKBitmap LoadBitmap(string filename, Module mdl)
        {
            return DependencyService.Get<ISaveAndLoad>().LoadBitmap(filename, mdl);
        }
        public List<string> GetAllModuleFiles(bool userOnly)
        {
            return DependencyService.Get<ISaveAndLoad>().GetAllModuleFiles(userOnly);
        }
        public bool DownloadFile(string url, string folder)
        {
            return DependencyService.Get<ISaveAndLoad>().DownloadFile(url, folder);
        }

        public void loadReviews()
        {
            userReviews = new IBbasicReview();
            communityReviews = new IBbasicReview();

            try
            {
                string s = LoadStringFromEitherFolder("\\IBbasicUserReviews.json", "\\IBbasicUserReviews.json");
                if (s != "")
                {
                    try
                    {
                        using (StringReader sr = new StringReader(s))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            userReviews = (IBbasicReview)serializer.Deserialize(sr, typeof(IBbasicReview));
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                //int x = 0;
            }

            try
            {
                string s = LoadStringFromEitherFolder("\\IBbasicCommunityReviews.json", "\\IBbasicCommunityReviews.json");
                if (s != "")
                {
                    try
                    {
                        using (StringReader sr = new StringReader(s))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            communityReviews = (IBbasicReview)serializer.Deserialize(sr, typeof(IBbasicReview));
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                //int x = 0;
            }
        }
        public void saveUserReviews()
        {
            try
            {
                string json = JsonConvert.SerializeObject(userReviews, Newtonsoft.Json.Formatting.Indented);
                SaveText("\\IBbasicUserReviews.json", json);
            }
            catch { }
        }
        public void postReview(IBReview thisReview)
        {
            try
            {
                string json = JsonConvert.SerializeObject(userReviews, Newtonsoft.Json.Formatting.Indented);
                SaveText("\\IBbasicUserReviews.json", json);
            }
            catch { }
            string realtime = DateTime.Now.ToString("yyyyMMddHHmmss");
            TrackerSendEvent(":POST_REVIEW:", "REVIEW::" + realtime + "::" + thisReview.reviewDate + ":" + thisReview.moduleName + "(v" + thisReview.moduleVersion + "):" + thisReview.userID + ":" + thisReview.userName + ":" + thisReview.review, true);
        }
        public void postEndorse(IBEndorse thisEndorse)
        {
            try
            {
                string json = JsonConvert.SerializeObject(userReviews, Newtonsoft.Json.Formatting.Indented);
                SaveText("\\IBbasicUserReviews.json", json);
            }
            catch { }
            string realtime = DateTime.Now.ToString("yyyyMMddHHmmss");
            TrackerSendEvent(":POST_ENDORSE:", "ENDORSE::" + realtime + "::" + thisEndorse.moduleName + ":" + thisEndorse.userID + ":" + thisEndorse.userName + ":" + thisEndorse.endorsed.ToString(), true);
        }

        //ANALYTICS
        public void TrackerSendEvent(string action, string label, bool mustSend)
        {
            //if (myTracker != null)
            //{
            string realtime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //string mainPcName = GetMainPcName();
            int totalHP = 0;
            int totalSP = 0;
            int totalXP = 0;
            int totalLVL = 0;
            int partySize = 0;
            if (IBprefs.UserID.Equals("none"))
            {
                IBprefs.GenerateUniqueUserID();
                savePreferences();
            }
            if (mod.playerList.Count > 0)
            {
                foreach (Player pc in mod.playerList)
                {
                    totalHP += pc.hp;
                    totalSP += pc.sp;
                    totalXP += pc.XP;
                    totalLVL += pc.classLevel;
                    partySize++;
                }
            }
            string totals = "HP" + totalHP + ":SP" + totalSP + ":XP" + totalXP + ":LVL" + totalLVL + ":PS" + partySize;
            string totAction = mod.moduleName + "(v" + mod.moduleVersion + ")" + ":(IBv" + versionNum + ")" + ":" + IBprefs.UserID + "_" + IBprefs.UserName + ":" + realtime + ":" + mod.WorldTime.ToString("D8") + ":" + totals + ":" + action;

            try
            {
                //Hearkenwold:Drin_586842:20170101123456:00027546:HP234:SP123:XP4567:LVL18:PS6::CONVO:guard
                string category = mod.moduleName + "(v" + mod.moduleVersion + ")" + ":(IBv" + versionNum + ")" + ":" + IBprefs.UserID + "_" + IBprefs.UserName;
                TrackAppEvent(category, totAction, label, mustSend);
            }
            catch (Exception e)
            {
                //e.printStackTrace();               
            }
        }
        public void TrackerSendMilestoneEvent(string milestone)
        {
            //string mainPcName = GetMainPcName();
            TrackerSendEvent("none", mod.moduleName + "(v" + mod.moduleVersion + ")" + ":(IBv" + versionNum + ")" + ":" + IBprefs.UserID + "_" + IBprefs.UserName + "::" + milestone, false);
            TrackerSendEventFullPartyInfo("PARTYINFO");
        }
        public void TrackerSendEventEncounter(string encounterName)
        {
            TrackerSendEvent(":ENC:" + encounterName, "none", false);
            //TrackerSendEventFullPartyInfo("PARTYINFO");
        }
        public void TrackerSendEventJournal(string category_entry)
        {
            TrackerSendEvent(":JOURNAL:" + category_entry, "none", false);            
        }
        public void TrackerSendEventConvo(string convoName)
        {
            TrackerSendEvent(":CONVO:" + convoName, "none", false);            
        }
        public void TrackerSendEventArea(string areaName)
        {
            TrackerSendEvent(":AREA:" + areaName, "none", false);                        
        }
        public void TrackerSendEventContainer(string containerName)
        {
            TrackerSendEvent(":CONTAINER:" + containerName, "none", false);            
        }
        public void TrackerSendEventFullPartyInfo(string actionLabel)
        {
            //actions: PartyStart, PartyAddCompanion, PartyEndingCh1
            if (mod.playerList.Count > 0)
            {
                try
                {
                    int x = 1;
                    foreach (Player pc in mod.playerList)
                    {
                        string partyInfo = PlayerInfoFull(pc, x);
                        TrackerSendEvent(":::" + actionLabel + ":::" + partyInfo, "none", false);
                        x++;
                    }
                }
                catch (Exception e)
                {
                    //e.printStackTrace();
                }
            }
        }
        public string GetMainPcName()
        {
            string name = "none";
            foreach (Player pc in mod.playerList)
            {
                if (pc.mainPc)
                {
                    return pc.name;
                }
            }
            return name;
        }
        public string PlayerInfoFull(Player pc, int index)
        {
            string info = "";
            info = "INDEX:" + index + ",NAME:" + pc.name + ",TOKEN:" + pc.tokenFilename + ",RACE:" + pc.raceTag + ",CLASS:" + pc.classTag
                    + ",STR:" + pc.strength + ",DEX:" + pc.dexterity + ",CON:" + pc.constitution + ",INT:" + pc.intelligence + ",WIS:" + pc.wisdom + ",CHA:" + pc.charisma
                    + ",LVL:" + pc.classLevel + ",XP:" + pc.XP + ",AC:" + pc.AC + ",HP:" + pc.hp + "/" + pc.hpMax + ",SP:" + pc.sp + "/" + pc.spMax + ",WEAPON:" + pc.MainHandRefs.name
                    + ",HEAD:" + pc.HeadRefs.name + ",NECK:" + pc.NeckRefs.name + ",BODY:" + pc.BodyRefs.name
                    + ",OFFHAND:" + pc.OffHandRefs.name + ",RING1:" + pc.RingRefs.name + ",RING2:" + pc.Ring2Refs.name
                    + ",FEET:" + pc.FeetRefs.name + ",AMMO:" + pc.AmmoRefs.name;
            info += ",TRAITS:";
            foreach (string tag in pc.knownTraitsTags)
            {
                info += tag + ",";
            }
            info += ",SPELLS:";
            foreach (string tag in pc.knownSpellsTags)
            {
                info += tag + ",";
            }
            return info;
        }

        public void TrackAppEvent(string Category, string EventAction, string EventLabel, bool mustSend)
        {
            if ((IBprefs.GoogleAnalyticsOn) || (mustSend))
            {
                DependencyService.Get<ISaveAndLoad>().TrackAppEvent(Category, EventAction, EventLabel);
            }
        }

        public void RestartAreaMusicIfEnded()
        {
            DependencyService.Get<ISaveAndLoad>().RestartAreaMusicIfEnded(this);
        }
        public void PlaySound(string filenameNoExtension)
        {
            DependencyService.Get<ISaveAndLoad>().PlaySound(this, filenameNoExtension);
        }
        public void CreateAreaMusicPlayer()
        {
            //DependencyService.Get<ISaveAndLoad>().CreateAreaMusicPlayer();
        }
        public void LoadAreaMusicFile(string fileName)
        {
            //DependencyService.Get<ISaveAndLoad>().LoadAreaMusicFile(fileName);
        }
        public void PlayAreaMusic(string filenameNoExtension)
        {
            DependencyService.Get<ISaveAndLoad>().PlayAreaMusic(this, filenameNoExtension);
        }
        public void PlayAreaAmbientSounds(string filenameNoExtension)
        {
            DependencyService.Get<ISaveAndLoad>().PlayAreaAmbientSounds(this, filenameNoExtension);
        }
        public void StopAreaMusic()
        {
            DependencyService.Get<ISaveAndLoad>().StopAreaMusic();
        }
        public void PauseAreaMusic()
        {
            //DependencyService.Get<ISaveAndLoad>().PlayAreaMusic();
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