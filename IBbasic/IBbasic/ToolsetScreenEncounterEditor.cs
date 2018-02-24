using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IBbasic
{
    public class ToolsetScreenEncounterEditor
    {
        public GameView gv;
        public int mapSquareSizeScaler = 1;
        public int panelTopLocation = 0;
        public int panelLeftLocation = 0;
        public IbRect src = null;
        public IbRect dst = null;
        private IbbButton btnInfo = null;
        private IbbButton btnTiles = null;
        private IbbButton btnTriggers = null;
        private IbbButton btnWalkLoS = null;
        private IbbButton btnCrt = null;
        private IbbButton btnSettings = null;
        private IbbButton btnHelp = null;
        public bool touchDown = false;

        //Place Tiles Panel
        private IbbButton btnTilesLeft = null;
        private IbbButton btnTilesRight = null;
        private IbbButton btnTilesPageIndex = null;
        public IbbToggle rbtnShowLayer1 = null;
        public IbbToggle rbtnShowLayer2 = null;
        public IbbToggle rbtnShowLayer3 = null;
        public IbbToggle rbtnEditLayer1 = null;
        public IbbToggle rbtnEditLayer2 = null;
        public IbbToggle rbtnEditLayer3 = null;
        public string currentTile = "none";
        private int tilesPageIndex = 0;
        private int tileSlotIndex = 0;
        private int tileSlotsPerPage = 24;

        //Place Creatures Panel
        private IbbButton btnCrtLeft = null;
        private IbbButton btnCrtRight = null;
        private IbbButton btnCrtPageIndex = null;
        public Creature currentCrt = null;
        private int crtPageIndex = 0;
        private int crtSlotIndex = 0;
        private int crtSlotsPerPage = 24;

        //Encounter Settings Panel
        public IbbToggle tglSettingEncounterName = null;
        public IbbToggle tglSettingGoldDrop = null;
        public IbbToggle tglSettingUseDayNightCycle = null;
        public IbbToggle tglSettingPlacePCs = null;
        public IbbToggle tglSettingRemoveAllPCs = null;
        public IbbToggle tglSettingRemoveAllCrts = null;
        public IbbToggle tglSettingAddItem = null;
        public IbbToggle tglSettingRemoveItem = null;

        //Info Panel
        public Coordinate selectedSquare = new Coordinate();

        //Walkable-LoS Panel
        public IbbToggle rbtnWalkBlocking = null;
        public IbbToggle rbtnWalkOpen = null;
        public IbbToggle rbtnSightBlocking = null;
        public IbbToggle rbtnSightOpen = null;

        //Triggers Panel
        public Trigger selectedTrigger = null;
        public IbbToggle rbtnViewInfoTrigger = null;
        public IbbToggle rbtnPlaceNewTrigger = null;
        public IbbToggle rbtnEditTrigger = null;
        public IbbToggle rbtnTriggerProperties = null;
        public IbbToggle rbtnEvent1Properties = null;
        public IbbToggle rbtnEvent2Properties = null;
        public IbbToggle rbtnEvent3Properties = null;
        public IbbToggle tglTriggerTag = null;
        public IbbToggle tglTriggerEnabled = null;
        public IbbToggle tglTriggerDoOnce = null;
        public IbbToggle tglImageFilename = null;
        public IbbToggle tglImageFacingLeft = null;
        public IbbToggle tglIsShown = null;
        public IbbToggle tglNumberOfScriptCallsRemaining = null;
        public IbbToggle tglCanBeTriggeredByPc = null;
        public IbbToggle tglCanBeTriggeredByCreature = null;

        public IbbToggle tglEnabledEvent = null;
        public IbbToggle tglDoOnceOnlyEvent = null;
        public IbbToggle tglEventType = null;
        public IbbToggle tglEventFilenameOrTag = null;
        public IbbToggle tglEventTransPointX = null;
        public IbbToggle tglEventTransPointY = null;
        public IbbToggle tglEventParm1 = null;
        public IbbToggle tglEventParm2 = null;
        public IbbToggle tglEventParm3 = null;
        public IbbToggle tglEventParm4 = null;

        private IBminiTextBox description;

        public string currentMode = "Info"; //Info, Tiles, Triggers, WalkLoS, Crt, Settings
        public int mapStartLocXinPixels;

        List<string> tilesList = new List<string>();

        public ToolsetScreenEncounterEditor(GameView g)
        {
            gv = g;
            mapStartLocXinPixels = 1 * gv.uiSquareSize;
            setControlsStart();
            setupTilesPanelControls();
            setupSettingsPanelControls();
            setupWalkLoSPanelControls();
            setupTriggerPanelControls();
            setupCrtPanelControls();
            rbtnEditLayer1.toggleOn = true;
            rbtnEditLayer2.toggleOn = false;
            rbtnEditLayer3.toggleOn = false;
            rbtnShowLayer1.toggleOn = true;
            rbtnShowLayer2.toggleOn = true;
            rbtnShowLayer3.toggleOn = true;
            rbtnWalkOpen.toggleOn = true;
            rbtnViewInfoTrigger.toggleOn = true;
            rbtnTriggerProperties.toggleOn = true;
            tglSettingUseDayNightCycle.toggleOn = gv.mod.currentEncounter.UseDayNightCycle;
            description = new IBminiTextBox(gv);
            description.tbXloc = 0 * gv.squareSize;
            description.tbYloc = 3 * gv.squareSize;
            description.tbWidth = 6 * gv.squareSize;
            description.tbHeight = 6 * gv.squareSize;
            description.showBoxBorder = false;

            List<string> files = gv.GetAllFilesWithExtensionFromBothFolders("\\tiles", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            foreach (string f in files)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                if (filenameNoExt.StartsWith("t_"))
                {
                    tilesList.Add(filenameNoExt);
                }
            }
        }

        public void setControlsStart()
        {
            if (btnInfo == null)
            {
                btnInfo = new IbbButton(gv, 1.0f);
            }
            btnInfo.Img = "btn_small";
            btnInfo.Glow = "btn_small_glow";
            btnInfo.Text = "INFO";
            btnInfo.X = 0 * gv.uiSquareSize;
            btnInfo.Y = 0 * gv.uiSquareSize + gv.scaler;
            btnInfo.Height = (int)(gv.ibbheight * gv.scaler);
            btnInfo.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnTiles == null)
            {
                btnTiles = new IbbButton(gv, 1.0f);
            }
            btnTiles.Img = "btn_small";
            btnTiles.Glow = "btn_small_glow";
            btnTiles.Text = "TILES";
            btnTiles.X = 0 * gv.uiSquareSize;
            btnTiles.Y = 1 * gv.uiSquareSize + gv.scaler;
            btnTiles.Height = (int)(gv.ibbheight * gv.scaler);
            btnTiles.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnTriggers == null)
            {
                btnTriggers = new IbbButton(gv, 0.8f);
            }
            btnTriggers.Img = "btn_small";
            btnTriggers.Glow = "btn_small_glow";
            btnTriggers.Text = "TRIGR";
            btnTriggers.X = 0 * gv.uiSquareSize;
            btnTriggers.Y = 2 * gv.uiSquareSize + gv.scaler;
            btnTriggers.Height = (int)(gv.ibbheight * gv.scaler);
            btnTriggers.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnWalkLoS == null)
            {
                btnWalkLoS = new IbbButton(gv, 1.0f);
            }
            btnWalkLoS.Text = "WLKLOS";
            btnWalkLoS.Img = "btn_small";
            btnWalkLoS.Glow = "btn_small_glow";
            btnWalkLoS.X = 0 * gv.uiSquareSize;
            btnWalkLoS.Y = 3 * gv.uiSquareSize + gv.scaler;
            btnWalkLoS.Height = (int)(gv.ibbheight * gv.scaler);
            btnWalkLoS.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCrt == null)
            {
                btnCrt = new IbbButton(gv, 1.0f);
            }
            btnCrt.Text = "CRTRS";
            btnCrt.Img = "btn_small";
            btnCrt.Glow = "btn_small_glow";
            btnCrt.X = 0 * gv.uiSquareSize;
            btnCrt.Y = 4 * gv.uiSquareSize + gv.scaler;
            btnCrt.Height = (int)(gv.ibbheight * gv.scaler);
            btnCrt.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnSettings == null)
            {
                btnSettings = new IbbButton(gv, 1.0f);
            }
            btnSettings.Text = "SETTING";
            btnSettings.Img = "btn_small";
            btnSettings.Glow = "btn_small_glow";
            btnSettings.X = 0 * gv.uiSquareSize;
            btnSettings.Y = 5 * gv.uiSquareSize + gv.scaler;
            btnSettings.Height = (int)(gv.ibbheight * gv.scaler);
            btnSettings.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
            }
            btnHelp.Text = "HELP";
            btnHelp.Img = "btn_small";
            btnHelp.Glow = "btn_small_glow";
            btnHelp.X = 10 * gv.uiSquareSize;
            btnHelp.Y = 6 * gv.uiSquareSize + gv.scaler;
            btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
            btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        public void setupTilesPanelControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (gv.oXshift / 2);
            panelTopLocation = (gv.oYshift / 4);

            //TILES PANEL
            if (btnTilesLeft == null)
            {
                btnTilesLeft = new IbbButton(gv, 1.0f);
            }
            btnTilesLeft.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
            btnTilesLeft.Img2 = "ctrl_left_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_left_arrow);
            btnTilesLeft.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnTilesLeft.X = panelLeftLocation;
            btnTilesLeft.Y = panelTopLocation + (4 * gv.uiSquareSize) + (gv.uiSquareSize / 4);
            btnTilesLeft.Height = (int)(gv.ibbheight * gv.scaler);
            btnTilesLeft.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnTilesPageIndex == null)
            {
                btnTilesPageIndex = new IbbButton(gv, 1.0f);
            }
            btnTilesPageIndex.Img = "btn_small_off"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_off);
            btnTilesPageIndex.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            //btnTilesPageIndex.Text = "1";
            btnTilesPageIndex.X = panelLeftLocation + 1 * gv.uiSquareSize;
            btnTilesPageIndex.Y = panelTopLocation + (4 * gv.uiSquareSize) + (gv.uiSquareSize / 4);
            btnTilesPageIndex.Height = (int)(gv.ibbheight * gv.scaler);
            btnTilesPageIndex.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnTilesRight == null)
            {
                btnTilesRight = new IbbButton(gv, 1.0f);
            }
            btnTilesRight.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
            btnTilesRight.Img2 = "ctrl_right_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_right_arrow);
            btnTilesRight.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnTilesRight.X = panelLeftLocation + 2 * gv.uiSquareSize;
            btnTilesRight.Y = panelTopLocation + (4 * gv.uiSquareSize) + (gv.uiSquareSize / 4);
            btnTilesRight.Height = (int)(gv.ibbheight * gv.scaler);
            btnTilesRight.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (rbtnShowLayer1 == null)
            {
                rbtnShowLayer1 = new IbbToggle(gv);
            }
            rbtnShowLayer1.ImgOn = "mtgl_rbtn_on";
            rbtnShowLayer1.ImgOff = "mtgl_rbtn_off";
            rbtnShowLayer1.X = panelLeftLocation;
            rbtnShowLayer1.Y = panelTopLocation + (5 * gv.uiSquareSize) + ((gv.fontHeight + gv.fontLineSpacing) * 2);
            rbtnShowLayer1.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnShowLayer1.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnShowLayer2 == null)
            {
                rbtnShowLayer2 = new IbbToggle(gv);
            }
            rbtnShowLayer2.ImgOn = "mtgl_rbtn_on";
            rbtnShowLayer2.ImgOff = "mtgl_rbtn_off";
            rbtnShowLayer2.X = panelLeftLocation;
            rbtnShowLayer2.Y = panelTopLocation + (5 * gv.uiSquareSize) + ((gv.fontHeight + gv.fontLineSpacing) * 4);
            rbtnShowLayer2.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnShowLayer2.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnShowLayer3 == null)
            {
                rbtnShowLayer3 = new IbbToggle(gv);
            }
            rbtnShowLayer3.ImgOn = "mtgl_rbtn_on";
            rbtnShowLayer3.ImgOff = "mtgl_rbtn_off";
            rbtnShowLayer3.X = panelLeftLocation;
            rbtnShowLayer3.Y = panelTopLocation + (5 * gv.uiSquareSize) + ((gv.fontHeight + gv.fontLineSpacing) * 6);
            rbtnShowLayer3.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnShowLayer3.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEditLayer1 == null)
            {
                rbtnEditLayer1 = new IbbToggle(gv);
            }
            rbtnEditLayer1.ImgOn = "mtgl_rbtn_on";
            rbtnEditLayer1.ImgOff = "mtgl_rbtn_off";
            rbtnEditLayer1.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            rbtnEditLayer1.Y = panelTopLocation + (5 * gv.uiSquareSize) + ((gv.fontHeight + gv.fontLineSpacing) * 2);
            rbtnEditLayer1.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnEditLayer1.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEditLayer2 == null)
            {
                rbtnEditLayer2 = new IbbToggle(gv);
            }
            rbtnEditLayer2.ImgOn = "mtgl_rbtn_on";
            rbtnEditLayer2.ImgOff = "mtgl_rbtn_off";
            rbtnEditLayer2.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            rbtnEditLayer2.Y = panelTopLocation + (5 * gv.uiSquareSize) + ((gv.fontHeight + gv.fontLineSpacing) * 4);
            rbtnEditLayer2.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnEditLayer2.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEditLayer3 == null)
            {
                rbtnEditLayer3 = new IbbToggle(gv);
            }
            rbtnEditLayer3.ImgOn = "mtgl_rbtn_on";
            rbtnEditLayer3.ImgOff = "mtgl_rbtn_off";
            rbtnEditLayer3.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            rbtnEditLayer3.Y = panelTopLocation + (5 * gv.uiSquareSize) + ((gv.fontHeight + gv.fontLineSpacing) * 6);
            rbtnEditLayer3.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnEditLayer3.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setupCrtPanelControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (gv.oXshift / 2);
            panelTopLocation = (gv.oYshift / 4);

            //TILES PANEL
            if (btnCrtLeft == null)
            {
                btnCrtLeft = new IbbButton(gv, 1.0f);
            }
            btnCrtLeft.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
            btnCrtLeft.Img2 = "ctrl_left_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_left_arrow);
            btnCrtLeft.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnCrtLeft.X = panelLeftLocation;
            btnCrtLeft.Y = panelTopLocation + (4 * gv.uiSquareSize) + (gv.uiSquareSize / 4);
            btnCrtLeft.Height = (int)(gv.ibbheight * gv.scaler);
            btnCrtLeft.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCrtPageIndex == null)
            {
                btnCrtPageIndex = new IbbButton(gv, 1.0f);
            }
            btnCrtPageIndex.Img = "btn_small_off"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_off);
            btnCrtPageIndex.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnCrtPageIndex.X = panelLeftLocation + 1 * gv.uiSquareSize;
            btnCrtPageIndex.Y = panelTopLocation + (4 * gv.uiSquareSize) + (gv.uiSquareSize / 4);
            btnCrtPageIndex.Height = (int)(gv.ibbheight * gv.scaler);
            btnCrtPageIndex.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCrtRight == null)
            {
                btnCrtRight = new IbbButton(gv, 1.0f);
            }
            btnCrtRight.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
            btnCrtRight.Img2 = "ctrl_right_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_right_arrow);
            btnCrtRight.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnCrtRight.X = panelLeftLocation + 2 * gv.uiSquareSize;
            btnCrtRight.Y = panelTopLocation + (4 * gv.uiSquareSize) + (gv.uiSquareSize / 4);
            btnCrtRight.Height = (int)(gv.ibbheight * gv.scaler);
            btnCrtRight.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        public void setupSettingsPanelControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (gv.oXshift / 2);
            panelTopLocation = (gv.oYshift / 4);

            //TILES PANEL
            if (tglSettingEncounterName == null)
            {
                tglSettingEncounterName = new IbbToggle(gv);
            }
            tglSettingEncounterName.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            tglSettingEncounterName.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            tglSettingEncounterName.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglSettingEncounterName.Y = panelTopLocation + 0 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglSettingEncounterName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingEncounterName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingGoldDrop == null)
            {
                tglSettingGoldDrop = new IbbToggle(gv);
            }
            tglSettingGoldDrop.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            tglSettingGoldDrop.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            tglSettingGoldDrop.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglSettingGoldDrop.Y = panelTopLocation + 1 * gv.uiSquareSize;
            tglSettingGoldDrop.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingGoldDrop.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingUseDayNightCycle == null)
            {
                tglSettingUseDayNightCycle = new IbbToggle(gv);
            }
            tglSettingUseDayNightCycle.ImgOn = "mtgl_rbtn_on";
            tglSettingUseDayNightCycle.ImgOff = "mtgl_rbtn_off";
            tglSettingUseDayNightCycle.X = panelLeftLocation;
            tglSettingUseDayNightCycle.Y = panelTopLocation + 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglSettingUseDayNightCycle.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingUseDayNightCycle.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingPlacePCs == null)
            {
                tglSettingPlacePCs = new IbbToggle(gv);
            }
            tglSettingPlacePCs.ImgOn = "mtgl_rbtn_on";
            tglSettingPlacePCs.ImgOff = "mtgl_rbtn_off";
            tglSettingPlacePCs.X = panelLeftLocation;
            tglSettingPlacePCs.Y = panelTopLocation + 2 * gv.uiSquareSize;
            tglSettingPlacePCs.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingPlacePCs.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingRemoveAllPCs == null)
            {
                tglSettingRemoveAllPCs = new IbbToggle(gv);
            }
            tglSettingRemoveAllPCs.ImgOn = "mtgl_edit_btn";
            tglSettingRemoveAllPCs.ImgOff = "mtgl_edit_btn";
            tglSettingRemoveAllPCs.X = panelLeftLocation;
            tglSettingRemoveAllPCs.Y = panelTopLocation + 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglSettingRemoveAllPCs.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingRemoveAllPCs.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingRemoveAllCrts == null)
            {
                tglSettingRemoveAllCrts = new IbbToggle(gv);
            }
            tglSettingRemoveAllCrts.ImgOn = "mtgl_edit_btn";
            tglSettingRemoveAllCrts.ImgOff = "mtgl_edit_btn";
            tglSettingRemoveAllCrts.X = panelLeftLocation + 1 * gv.uiSquareSize + 2 * (gv.uiSquareSize / 3);
            tglSettingRemoveAllCrts.Y = panelTopLocation + 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglSettingRemoveAllCrts.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingRemoveAllCrts.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingAddItem == null)
            {
                tglSettingAddItem = new IbbToggle(gv);
            }
            tglSettingAddItem.ImgOn = "mtgl_edit_btn";
            tglSettingAddItem.ImgOff = "mtgl_edit_btn";
            tglSettingAddItem.X = panelLeftLocation;
            tglSettingAddItem.Y = panelTopLocation + 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglSettingAddItem.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingAddItem.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingRemoveItem == null)
            {
                tglSettingRemoveItem = new IbbToggle(gv);
            }
            tglSettingRemoveItem.ImgOn = "mtgl_edit_btn";
            tglSettingRemoveItem.ImgOff = "mtgl_edit_btn";
            tglSettingRemoveItem.X = panelLeftLocation + 1 * gv.uiSquareSize + 2 * (gv.uiSquareSize / 3);
            tglSettingRemoveItem.Y = panelTopLocation + 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglSettingRemoveItem.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingRemoveItem.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setupWalkLoSPanelControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (gv.oXshift / 2);
            panelTopLocation = (gv.oYshift / 4);

            //WALK-LoS PANEL           
            if (rbtnWalkBlocking == null)
            {
                rbtnWalkBlocking = new IbbToggle(gv);
            }
            rbtnWalkBlocking.ImgOn = "mtgl_rbtn_on";
            rbtnWalkBlocking.ImgOff = "mtgl_rbtn_off";
            rbtnWalkBlocking.X = panelLeftLocation;
            rbtnWalkBlocking.Y = panelTopLocation + 1 * gv.uiSquareSize;
            rbtnWalkBlocking.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnWalkBlocking.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnWalkOpen == null)
            {
                rbtnWalkOpen = new IbbToggle(gv);
            }
            rbtnWalkOpen.ImgOn = "mtgl_rbtn_on";
            rbtnWalkOpen.ImgOff = "mtgl_rbtn_off";
            rbtnWalkOpen.X = panelLeftLocation;
            rbtnWalkOpen.Y = panelTopLocation + 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            rbtnWalkOpen.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnWalkOpen.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnSightBlocking == null)
            {
                rbtnSightBlocking = new IbbToggle(gv);
            }
            rbtnSightBlocking.ImgOn = "mtgl_rbtn_on";
            rbtnSightBlocking.ImgOff = "mtgl_rbtn_off";
            rbtnSightBlocking.X = panelLeftLocation;
            rbtnSightBlocking.Y = panelTopLocation + 2 * gv.uiSquareSize;
            rbtnSightBlocking.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnSightBlocking.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnSightOpen == null)
            {
                rbtnSightOpen = new IbbToggle(gv);
            }
            rbtnSightOpen.ImgOn = "mtgl_rbtn_on";
            rbtnSightOpen.ImgOff = "mtgl_rbtn_off";
            rbtnSightOpen.X = panelLeftLocation;
            rbtnSightOpen.Y = panelTopLocation + 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            rbtnSightOpen.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnSightOpen.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setupTriggerPanelControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (gv.oXshift / 2);
            panelTopLocation = (gv.oYshift / 4);

            //Trigger Panel           
            if (rbtnViewInfoTrigger == null)
            {
                rbtnViewInfoTrigger = new IbbToggle(gv);
            }
            rbtnViewInfoTrigger.ImgOn = "mtgl_rbtn_on";
            rbtnViewInfoTrigger.ImgOff = "mtgl_rbtn_off";
            rbtnViewInfoTrigger.X = panelLeftLocation;
            rbtnViewInfoTrigger.Y = panelTopLocation + 0 * gv.uiSquareSize + ((gv.fontHeight + gv.fontLineSpacing) * 2);
            rbtnViewInfoTrigger.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnViewInfoTrigger.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnPlaceNewTrigger == null)
            {
                rbtnPlaceNewTrigger = new IbbToggle(gv);
            }
            rbtnPlaceNewTrigger.ImgOn = "mtgl_rbtn_on";
            rbtnPlaceNewTrigger.ImgOff = "mtgl_rbtn_off";
            rbtnPlaceNewTrigger.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            rbtnPlaceNewTrigger.Y = panelTopLocation + 0 * gv.uiSquareSize + ((gv.fontHeight + gv.fontLineSpacing) * 2);
            rbtnPlaceNewTrigger.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnPlaceNewTrigger.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEditTrigger == null)
            {
                rbtnEditTrigger = new IbbToggle(gv);
            }
            rbtnEditTrigger.ImgOn = "mtgl_rbtn_on";
            rbtnEditTrigger.ImgOff = "mtgl_rbtn_off";
            rbtnEditTrigger.X = panelLeftLocation;
            rbtnEditTrigger.Y = panelTopLocation + 0 * gv.uiSquareSize + ((gv.fontHeight + gv.fontLineSpacing) * 4);
            rbtnEditTrigger.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnEditTrigger.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnTriggerProperties == null)
            {
                rbtnTriggerProperties = new IbbToggle(gv);
            }
            rbtnTriggerProperties.ImgOn = "mtgl_rbtn_on";
            rbtnTriggerProperties.ImgOff = "mtgl_rbtn_off";
            rbtnTriggerProperties.X = panelLeftLocation;
            rbtnTriggerProperties.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 7);
            rbtnTriggerProperties.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnTriggerProperties.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEvent1Properties == null)
            {
                rbtnEvent1Properties = new IbbToggle(gv);
            }
            rbtnEvent1Properties.ImgOn = "mtgl_rbtn_on";
            rbtnEvent1Properties.ImgOff = "mtgl_rbtn_off";
            rbtnEvent1Properties.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            rbtnEvent1Properties.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 7);
            rbtnEvent1Properties.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnEvent1Properties.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEvent2Properties == null)
            {
                rbtnEvent2Properties = new IbbToggle(gv);
            }
            rbtnEvent2Properties.ImgOn = "mtgl_rbtn_on";
            rbtnEvent2Properties.ImgOff = "mtgl_rbtn_off";
            rbtnEvent2Properties.X = panelLeftLocation;
            rbtnEvent2Properties.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 9);
            rbtnEvent2Properties.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnEvent2Properties.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEvent3Properties == null)
            {
                rbtnEvent3Properties = new IbbToggle(gv);
            }
            rbtnEvent3Properties.ImgOn = "mtgl_rbtn_on";
            rbtnEvent3Properties.ImgOff = "mtgl_rbtn_off";
            rbtnEvent3Properties.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            rbtnEvent3Properties.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 9);
            rbtnEvent3Properties.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnEvent3Properties.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            //TRIGGER PROPERTIES
            if (tglTriggerTag == null)
            {
                tglTriggerTag = new IbbToggle(gv);
            }
            tglTriggerTag.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            tglTriggerTag.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            tglTriggerTag.X = panelLeftLocation;
            tglTriggerTag.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 12);
            tglTriggerTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglTriggerTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglTriggerEnabled == null)
            {
                tglTriggerEnabled = new IbbToggle(gv);
            }
            tglTriggerEnabled.ImgOn = "mtgl_rbtn_on";
            tglTriggerEnabled.ImgOff = "mtgl_rbtn_off";
            tglTriggerEnabled.X = panelLeftLocation;
            tglTriggerEnabled.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 14);
            tglTriggerEnabled.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglTriggerEnabled.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglTriggerDoOnce == null)
            {
                tglTriggerDoOnce = new IbbToggle(gv);
            }
            tglTriggerDoOnce.ImgOn = "mtgl_rbtn_on";
            tglTriggerDoOnce.ImgOff = "mtgl_rbtn_off";
            tglTriggerDoOnce.X = panelLeftLocation;
            tglTriggerDoOnce.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 16);
            tglTriggerDoOnce.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglTriggerDoOnce.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglImageFilename == null)
            {
                tglImageFilename = new IbbToggle(gv);
            }
            tglImageFilename.ImgOn = "mtgl_edit_btn";
            tglImageFilename.ImgOff = "mtgl_edit_btn";
            tglImageFilename.X = panelLeftLocation;
            tglImageFilename.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 18);
            tglImageFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglImageFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglImageFacingLeft == null)
            {
                tglImageFacingLeft = new IbbToggle(gv);
            }
            tglImageFacingLeft.ImgOn = "mtgl_rbtn_on";
            tglImageFacingLeft.ImgOff = "mtgl_rbtn_off";
            tglImageFacingLeft.X = panelLeftLocation;
            tglImageFacingLeft.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 20);
            tglImageFacingLeft.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglImageFacingLeft.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglIsShown == null)
            {
                tglIsShown = new IbbToggle(gv);
            }
            tglIsShown.ImgOn = "mtgl_rbtn_on";
            tglIsShown.ImgOff = "mtgl_rbtn_off";
            tglIsShown.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            tglIsShown.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 20);
            tglIsShown.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglIsShown.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglNumberOfScriptCallsRemaining == null)
            {
                tglNumberOfScriptCallsRemaining = new IbbToggle(gv);
            }
            tglNumberOfScriptCallsRemaining.ImgOn = "mtgl_edit_btn";
            tglNumberOfScriptCallsRemaining.ImgOff = "mtgl_edit_btn";
            tglNumberOfScriptCallsRemaining.X = panelLeftLocation;
            tglNumberOfScriptCallsRemaining.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 23);
            tglNumberOfScriptCallsRemaining.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglNumberOfScriptCallsRemaining.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCanBeTriggeredByPc == null)
            {
                tglCanBeTriggeredByPc = new IbbToggle(gv);
            }
            tglCanBeTriggeredByPc.ImgOn = "mtgl_rbtn_on";
            tglCanBeTriggeredByPc.ImgOff = "mtgl_rbtn_off";
            tglCanBeTriggeredByPc.X = panelLeftLocation;
            tglCanBeTriggeredByPc.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 25);
            tglCanBeTriggeredByPc.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCanBeTriggeredByPc.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCanBeTriggeredByCreature == null)
            {
                tglCanBeTriggeredByCreature = new IbbToggle(gv);
            }
            tglCanBeTriggeredByCreature.ImgOn = "mtgl_rbtn_on";
            tglCanBeTriggeredByCreature.ImgOff = "mtgl_rbtn_off";
            tglCanBeTriggeredByCreature.X = panelLeftLocation;
            tglCanBeTriggeredByCreature.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 27);
            tglCanBeTriggeredByCreature.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCanBeTriggeredByCreature.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            //EVENTS 
            if (tglEnabledEvent == null)
            {
                tglEnabledEvent = new IbbToggle(gv);
            }
            tglEnabledEvent.ImgOn = "mtgl_rbtn_on";
            tglEnabledEvent.ImgOff = "mtgl_rbtn_off";
            tglEnabledEvent.X = panelLeftLocation;
            tglEnabledEvent.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 12);
            tglEnabledEvent.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEnabledEvent.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglDoOnceOnlyEvent == null)
            {
                tglDoOnceOnlyEvent = new IbbToggle(gv);
            }
            tglDoOnceOnlyEvent.ImgOn = "mtgl_rbtn_on";
            tglDoOnceOnlyEvent.ImgOff = "mtgl_rbtn_off";
            tglDoOnceOnlyEvent.X = panelLeftLocation;
            tglDoOnceOnlyEvent.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 14);
            tglDoOnceOnlyEvent.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglDoOnceOnlyEvent.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventType == null)
            {
                tglEventType = new IbbToggle(gv);
            }
            tglEventType.ImgOn = "mtgl_edit_btn";
            tglEventType.ImgOff = "mtgl_edit_btn";
            tglEventType.X = panelLeftLocation;
            tglEventType.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 16);
            tglEventType.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventType.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventFilenameOrTag == null)
            {
                tglEventFilenameOrTag = new IbbToggle(gv);
            }
            tglEventFilenameOrTag.ImgOn = "mtgl_edit_btn";
            tglEventFilenameOrTag.ImgOff = "mtgl_edit_btn";
            tglEventFilenameOrTag.X = panelLeftLocation;
            tglEventFilenameOrTag.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 18);
            tglEventFilenameOrTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventFilenameOrTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventTransPointX == null)
            {
                tglEventTransPointX = new IbbToggle(gv);
            }
            tglEventTransPointX.ImgOn = "mtgl_edit_btn";
            tglEventTransPointX.ImgOff = "mtgl_edit_btn";
            tglEventTransPointX.X = panelLeftLocation;
            tglEventTransPointX.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 20);
            tglEventTransPointX.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventTransPointX.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventTransPointY == null)
            {
                tglEventTransPointY = new IbbToggle(gv);
            }
            tglEventTransPointY.ImgOn = "mtgl_edit_btn";
            tglEventTransPointY.ImgOff = "mtgl_edit_btn";
            tglEventTransPointY.X = panelLeftLocation;
            tglEventTransPointY.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 22);
            tglEventTransPointY.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventTransPointY.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventParm1 == null)
            {
                tglEventParm1 = new IbbToggle(gv);
            }
            tglEventParm1.ImgOn = "mtgl_edit_btn";
            tglEventParm1.ImgOff = "mtgl_edit_btn";
            tglEventParm1.X = panelLeftLocation;
            tglEventParm1.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 20);
            tglEventParm1.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventParm1.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventParm2 == null)
            {
                tglEventParm2 = new IbbToggle(gv);
            }
            tglEventParm2.ImgOn = "mtgl_edit_btn";
            tglEventParm2.ImgOff = "mtgl_edit_btn";
            tglEventParm2.X = panelLeftLocation;
            tglEventParm2.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 22);
            tglEventParm2.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventParm2.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventParm3 == null)
            {
                tglEventParm3 = new IbbToggle(gv);
            }
            tglEventParm3.ImgOn = "mtgl_edit_btn";
            tglEventParm3.ImgOff = "mtgl_edit_btn";
            tglEventParm3.X = panelLeftLocation;
            tglEventParm3.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 24);
            tglEventParm3.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventParm3.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventParm4 == null)
            {
                tglEventParm4 = new IbbToggle(gv);
            }
            tglEventParm4.ImgOn = "mtgl_edit_btn";
            tglEventParm4.ImgOff = "mtgl_edit_btn";
            tglEventParm4.X = panelLeftLocation;
            tglEventParm4.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 26);
            tglEventParm4.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventParm4.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }

        public void redrawTsEncEditor()
        {
            setControlsStart();
            drawAreaMap();
            drawTriggers();
            drawCreaturesAndPCs();
            drawGrid();

            int width2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Width;
            int height2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Height;
            IbRect src2 = new IbRect(0, 0, width2, height2);
            IbRect dst2 = new IbRect(0 - (170 * gv.scaler), 0 - (102 * gv.scaler), width2 * gv.scaler, height2 * gv.scaler);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png"), src2, dst2);

            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            //Page Title
            string encToEdit = "none";
            if (gv.mod.currentEncounter != null)
            {
                encToEdit = gv.mod.currentEncounter.encounterName;
            }
            gv.DrawText("ENCOUNTER EDITOR: " + encToEdit, 2 * gv.uiSquareSize, 2 * gv.scaler, "yl");

            if (currentMode.Equals("Info"))
            {
                drawInfoPanel();
            }
            else if (currentMode.Equals("Tiles"))
            {
                setupTilesPanelControls();
                drawTilesPanel();
            }
            else if (currentMode.Equals("Triggers"))
            {
                setupTriggerPanelControls();
                drawTriggersPanel();
            }
            else if (currentMode.Equals("WalkLoS"))
            {
                setupWalkLoSPanelControls();
                drawWalkLoSPanel();
            }
            else if (currentMode.Equals("Crt"))
            {
                setupCrtPanelControls();
                drawCrtPanel();
            }
            else if (currentMode.Equals("Settings"))
            {
                setupSettingsPanelControls();
                drawSettingsPanel();
            }

            btnInfo.Draw();
            btnTiles.Draw();
            btnTriggers.Draw();
            btnWalkLoS.Draw();
            btnCrt.Draw();
            btnSettings.Draw();

            gv.tsMainMenu.redrawTsMainMenu();

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }
        public void drawAreaMap()
        {
            #region Draw Layer 1
            if (rbtnShowLayer1.toggleOn)
            {
                for (int x = 0; x <= gv.mod.currentEncounter.MapSizeX - 1; x++)
                {
                    for (int y = 0; y <= gv.mod.currentEncounter.MapSizeY - 1; y++)
                    {
                        string tile = gv.mod.currentEncounter.Layer1Filename[y * gv.mod.currentEncounter.MapSizeX + x];
                        int tlX = x * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                        int tlY = y * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                        int brX = gv.squareSize / mapSquareSizeScaler * gv.scaler;
                        int brY = gv.squareSize / mapSquareSizeScaler * gv.scaler;

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                    }
                }
            }
            #endregion
            #region Draw Layer 2
            if (rbtnShowLayer2.toggleOn)
            {
                for (int x = 0; x <= gv.mod.currentEncounter.MapSizeX - 1; x++)
                {
                    for (int y = 0; y <= gv.mod.currentEncounter.MapSizeY - 1; y++)
                    {
                        string tile = gv.mod.currentEncounter.Layer2Filename[y * gv.mod.currentEncounter.MapSizeX + x];
                        int tlX = x * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                        int tlY = y * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                        int brX = gv.squareSize / mapSquareSizeScaler * gv.scaler;
                        int brY = gv.squareSize / mapSquareSizeScaler * gv.scaler;

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                    }
                }
            }
            #endregion
            #region Draw Layer 3
            if (rbtnShowLayer3.toggleOn)
            {
                if (gv.mod.currentEncounter.Layer3Filename.Count > 0)
                {
                    for (int x = 0; x <= gv.mod.currentEncounter.MapSizeX - 1; x++)
                    {
                        for (int y = 0; y <= gv.mod.currentEncounter.MapSizeY - 1; y++)
                        {
                            string tile = gv.mod.currentEncounter.Layer3Filename[y * gv.mod.currentEncounter.MapSizeX + x];
                            int tlX = x * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                            int tlY = y * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                            int brX = gv.squareSize / mapSquareSizeScaler * gv.scaler;
                            int brY = gv.squareSize / mapSquareSizeScaler * gv.scaler;

                            try
                            {
                                src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                                dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                                gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                            }
                            catch { }
                        }
                    }
                }
            }
            #endregion
        }
        public void drawTriggers()
        {
            foreach (Trigger t in gv.mod.currentEncounter.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    int dx = (p.X * gv.squareSize / mapSquareSizeScaler * gv.scaler) + mapStartLocXinPixels;
                    int dy = p.Y * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                    //Pen pen = new Pen(Color.Orange, 2);
                    SKColor clr = SKColors.Orange;
                    if ((t.Event1Type.Equals("encounter")) || (t.Event2Type.Equals("encounter")) || (t.Event3Type.Equals("encounter")))
                    {
                        clr = SKColors.Red;
                    }
                    else if (t.Event1Type.Equals("conversation"))
                    {
                        clr = SKColors.Yellow;
                    }
                    else if (t.Event1Type.Equals("script"))
                    {
                        clr = SKColors.Blue;
                    }
                    else if (t.Event1Type.Equals("transition"))
                    {
                        clr = SKColors.Lime;
                    }
                    else if (t.Event1Type.Equals("container"))
                    {
                        clr = SKColors.Magenta;
                    }
                    IbRect rect = new IbRect(dx + 3, dy + 3, gv.squareSize / mapSquareSizeScaler * gv.scaler - 6, gv.squareSize / mapSquareSizeScaler * gv.scaler - 6);
                    gv.DrawRectangle(rect, clr, 2);

                    if ((!t.ImageFileName.Equals("none")) && (t.isShown))
                    {
                        int tlX = p.X * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                        int tlY = p.Y * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                        int brX = gv.squareSize / mapSquareSizeScaler * gv.scaler;
                        int brY = gv.squareSize / mapSquareSizeScaler * gv.scaler;
                        src = new IbRect(0, 0, gv.cc.GetFromBitmapList(t.ImageFileName).Width, gv.cc.GetFromBitmapList(t.ImageFileName).Height);
                        dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                        gv.DrawBitmap(gv.cc.GetFromBitmapList(t.ImageFileName), src, dst, !t.ImageFacingLeft);
                    }
                }
            }
        }
        public void drawCreaturesAndPCs()
        {
            foreach (CreatureRefs crtRef in gv.mod.currentEncounter.encounterCreatureRefsList)
            {
                float cspx = (crtRef.creatureStartLocationX * gv.squareSize / mapSquareSizeScaler * gv.scaler) + mapStartLocXinPixels;
                float cspy = (crtRef.creatureStartLocationY * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                Creature crt = gv.cc.getCreatureByResRef(crtRef.creatureResRef);
                float scalerX = gv.cc.GetFromBitmapList(crt.cr_tokenFilename).Width / gv.standardTokenSize;
                if (scalerX == 0) { scalerX = 1.0f; }
                float scalerY = gv.cc.GetFromBitmapList(crt.cr_tokenFilename).Height / (gv.standardTokenSize * 2);
                if (scalerY == 0) { scalerY = 1.0f; }
                src = new IbRect(0, 0, gv.cc.GetFromBitmapList(crt.cr_tokenFilename).Width, gv.cc.GetFromBitmapList(crt.cr_tokenFilename).Height / 2);
                dst = new IbRect((int)cspx, (int)cspy, (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler * scalerX), (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler * scalerY));
                gv.DrawBitmap(gv.cc.GetFromBitmapList(crt.cr_tokenFilename), src, dst, !crt.combatFacingLeft);
                //gv.DrawText(crtRef.creatureTag, cspx, cspy + 25.0f, "yl");
            }

            int cnt = 0;
            foreach (Coordinate PCpoint in gv.mod.currentEncounter.encounterPcStartLocations)
            {
                int cspx = PCpoint.X * gv.squareSize / mapSquareSizeScaler * gv.scaler + mapStartLocXinPixels;
                int cspy = PCpoint.Y * gv.squareSize / mapSquareSizeScaler * gv.scaler + (int)(1.5 * gv.fontHeight);
                gv.DrawText("PC" + (cnt + 1).ToString(), cspx + 5, cspy, "yl");
                cnt++;
            }
        }
        public void drawGrid()
        {
            for (int x = 0; x <= gv.mod.currentEncounter.MapSizeX - 1; x++)
            {
                for (int y = 0; y <= gv.mod.currentEncounter.MapSizeY - 1; y++)
                {
                    int tlX = x * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                    int tlY = y * gv.squareSize / mapSquareSizeScaler * gv.scaler;
                    int brX = gv.squareSize / mapSquareSizeScaler * gv.scaler;
                    int brY = gv.squareSize / mapSquareSizeScaler * gv.scaler;
                    src = new IbRect(0, 0, gv.cc.walkBlocked.Width, gv.cc.walkBlocked.Height);
                    dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                    if (gv.mod.currentEncounter.LoSBlocked[y * gv.mod.currentEncounter.MapSizeX + x] == 1)
                    {
                        gv.DrawBitmap(gv.cc.losBlocked, src, dst);
                    }
                    if (gv.mod.currentEncounter.Walkable[y * gv.mod.currentEncounter.MapSizeX + x] == 0)
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
        public void drawInfoPanel()
        {
            int index = selectedSquare.Y * gv.mod.currentEncounter.MapSizeX + selectedSquare.X;

            gv.DrawText("INFO OF TILE", panelLeftLocation, panelTopLocation + (1 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            gv.DrawText("Tile: (" + selectedSquare.X + "," + selectedSquare.Y + ")", panelLeftLocation, panelTopLocation + (2 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            gv.DrawText("Layer 1:", panelLeftLocation, panelTopLocation + (4 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            gv.DrawText(gv.mod.currentEncounter.Layer1Filename[index], panelLeftLocation, panelTopLocation + (5 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            gv.DrawText("Layer 2:", panelLeftLocation, panelTopLocation + (7 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            gv.DrawText(gv.mod.currentEncounter.Layer2Filename[index], panelLeftLocation, panelTopLocation + (8 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            gv.DrawText("Layer 3:", panelLeftLocation, panelTopLocation + (10 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            gv.DrawText(gv.mod.currentEncounter.Layer3Filename[index], panelLeftLocation, panelTopLocation + (11 * (gv.fontHeight + gv.fontLineSpacing)), "wh");

            //info on creature
            string crtTag = "none";
            if (gv.mod.currentEncounter.getCreatureRefByLocation(selectedSquare.X, selectedSquare.Y) != null)
            {
                crtTag = gv.mod.currentEncounter.getCreatureRefByLocation(selectedSquare.X, selectedSquare.Y).creatureTag;
            }
            gv.DrawText("Creature Tag:", panelLeftLocation, panelTopLocation + (13 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            gv.DrawText(crtTag, panelLeftLocation, panelTopLocation + (14 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            
            //info on trigger
            string trigTag = "none";
            if (gv.mod.currentEncounter.getTriggerByLocation(selectedSquare.X, selectedSquare.Y) != null)
            {
                trigTag = gv.mod.currentEncounter.getTriggerByLocation(selectedSquare.X, selectedSquare.Y).TriggerTag;
            }
            gv.DrawText("Trigger:", panelLeftLocation, panelTopLocation + (16 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            gv.DrawText(trigTag, panelLeftLocation, panelTopLocation + (17 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            //info on walk/LoS
            gv.DrawText("Walkable:", panelLeftLocation, panelTopLocation + (19 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            if (gv.mod.currentEncounter.Walkable[index] == 1)
            {
                gv.DrawText("OPEN", panelLeftLocation, panelTopLocation + (20 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            }
            else
            {
                gv.DrawText("BLOCKED", panelLeftLocation, panelTopLocation + (20 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            }

            //info on walk/LoS
            gv.DrawText("Line-Of-Sight:", panelLeftLocation, panelTopLocation + (22 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            if (gv.mod.currentEncounter.LoSBlocked[index] == 1)
            {
                gv.DrawText("BLOCKED", panelLeftLocation, panelTopLocation + (23 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            }
            else
            {
                gv.DrawText("VISIBLE", panelLeftLocation, panelTopLocation + (23 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            }

            //draw selected info tile highlight
            int tlX = selectedSquare.X * gv.squareSize / mapSquareSizeScaler * gv.scaler;
            int tlY = selectedSquare.Y * gv.squareSize / mapSquareSizeScaler * gv.scaler;
            int brX = gv.squareSize / mapSquareSizeScaler * gv.scaler;
            int brY = gv.squareSize / mapSquareSizeScaler * gv.scaler;
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
            dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList("highlight_magenta"), src, dst);

            btnHelp.Draw();
        }
        public void drawTilesPanel()
        {
            int cnt = 0;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < (tileSlotsPerPage / 4); y++)
                {
                    string tile = "none";
                    if ((cnt + (tilesPageIndex * tileSlotsPerPage)) < tilesList.Count)
                    {
                        tile = tilesList[cnt + (tilesPageIndex * tileSlotsPerPage)];
                    }
                    int tlX = x * gv.squareSize * gv.scaler;
                    int tlY = y * gv.squareSize * gv.scaler;
                    int brX = gv.squareSize * gv.scaler;
                    int brY = gv.squareSize * gv.scaler;

                    try
                    {
                        src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                        dst = new IbRect(tlX + panelLeftLocation, tlY + panelTopLocation, brX, brY);
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile), src, dst);
                    }
                    catch { }
                    cnt++;
                }
            }

            int slotX = tileSlotIndex / (tileSlotsPerPage / 4);
            int slotY = tileSlotIndex - slotX * (tileSlotsPerPage / 4);
            int tlX2 = slotX * gv.squareSize * gv.scaler;
            int tlY2 = slotY * gv.squareSize * gv.scaler;
            int brX2 = gv.squareSize * gv.scaler;
            int brY2 = gv.squareSize * gv.scaler;
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
            dst = new IbRect(tlX2 + panelLeftLocation, tlY2 + panelTopLocation, brX2, brY2);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList("highlight_magenta"), src, dst);

            //Description     
            gv.DrawText("PAINT TILES", panelLeftLocation, panelTopLocation, "gn");

            btnTilesLeft.Draw();
            btnTilesPageIndex.Draw();
            tlX2 = btnTilesPageIndex.X + ((gv.uiSquareSize - (gv.squareSize * gv.scaler)) / 2);
            tlY2 = btnTilesPageIndex.Y + ((gv.uiSquareSize - (gv.squareSize * gv.scaler)) / 2);
            brX2 = gv.squareSize * gv.scaler;
            brY2 = gv.squareSize * gv.scaler;
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(currentTile).Width, gv.cc.GetFromTileBitmapList(currentTile).Height);
            dst = new IbRect(tlX2, tlY2, brX2, brY2);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(currentTile), src, dst);
            btnTilesRight.Draw();

            int shiftForFont = (rbtnShowLayer1.Height / 2) - (gv.fontHeight / 2);
            gv.DrawText("SHOW", rbtnShowLayer1.X + gv.scaler, rbtnShowLayer1.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            rbtnShowLayer1.Draw();
            gv.DrawText("Lyr1", rbtnShowLayer1.X + rbtnShowLayer1.Width + gv.scaler, rbtnShowLayer1.Y + shiftForFont, "wh");
            rbtnShowLayer2.Draw();
            gv.DrawText("Lyr2", rbtnShowLayer2.X + rbtnShowLayer2.Width + gv.scaler, rbtnShowLayer2.Y + shiftForFont, "wh");
            rbtnShowLayer3.Draw();
            gv.DrawText("Lyr3", rbtnShowLayer3.X + rbtnShowLayer3.Width + gv.scaler, rbtnShowLayer3.Y + shiftForFont, "wh");

            gv.DrawText("EDIT", rbtnEditLayer1.X + gv.scaler, rbtnEditLayer1.Y - gv.fontHeight - gv.fontLineSpacing, "gn");
            rbtnEditLayer1.Draw();
            gv.DrawText("Lyr1", rbtnEditLayer1.X + rbtnEditLayer1.Width + gv.scaler, rbtnEditLayer1.Y + shiftForFont, "wh");
            rbtnEditLayer2.Draw();
            gv.DrawText("Lyr2", rbtnEditLayer2.X + rbtnEditLayer2.Width + gv.scaler, rbtnEditLayer2.Y + shiftForFont, "wh");
            rbtnEditLayer3.Draw();
            gv.DrawText("Lyr3", rbtnEditLayer3.X + rbtnEditLayer3.Width + gv.scaler, rbtnEditLayer3.Y + shiftForFont, "wh");
        }
        public void drawTriggersPanel()
        {
            //Description     
            gv.DrawText("TRIGGERS", panelLeftLocation, panelTopLocation + gv.fontHeight + gv.fontLineSpacing, "gn");

            rbtnViewInfoTrigger.Draw();
            gv.DrawText("View", rbtnViewInfoTrigger.X + rbtnViewInfoTrigger.Width + gv.scaler, rbtnViewInfoTrigger.Y, "wh");
            gv.DrawText("Info", rbtnViewInfoTrigger.X + rbtnViewInfoTrigger.Width + gv.scaler, rbtnViewInfoTrigger.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            rbtnPlaceNewTrigger.Draw();
            gv.DrawText("Create", rbtnPlaceNewTrigger.X + rbtnPlaceNewTrigger.Width + gv.scaler, rbtnPlaceNewTrigger.Y, "wh");
            gv.DrawText("New", rbtnPlaceNewTrigger.X + rbtnPlaceNewTrigger.Width + gv.scaler, rbtnPlaceNewTrigger.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            if (selectedTrigger != null)
            {
                rbtnEditTrigger.Draw();
                gv.DrawText("Edit Last", rbtnEditTrigger.X + rbtnEditTrigger.Width + gv.scaler, rbtnEditTrigger.Y, "wh");
                gv.DrawText("Selected Trigger", rbtnEditTrigger.X + rbtnEditTrigger.Width + gv.scaler, rbtnEditTrigger.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            }

            if (selectedTrigger != null)
            {
                rbtnTriggerProperties.Draw();
                gv.DrawText("Main", rbtnTriggerProperties.X + rbtnTriggerProperties.Width + gv.scaler, rbtnTriggerProperties.Y, "wh");
                gv.DrawText("Info", rbtnTriggerProperties.X + rbtnTriggerProperties.Width + gv.scaler, rbtnTriggerProperties.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                string color = "wh";
                if (selectedTrigger != null)
                {
                    if (selectedTrigger.Event1Type.Equals("conversation")) { color = "yl"; }
                    else if (selectedTrigger.Event1Type.Equals("encounter")) { color = "rd"; }
                    else if (selectedTrigger.Event1Type.Equals("script")) { color = "bu"; }
                    else if (selectedTrigger.Event1Type.Equals("transition")) { color = "gn"; }
                    else if (selectedTrigger.Event1Type.Equals("container")) { color = "ma"; }
                }
                rbtnEvent1Properties.Draw();
                gv.DrawText("Event", rbtnEvent1Properties.X + rbtnEvent1Properties.Width + gv.scaler, rbtnEvent1Properties.Y, color);
                gv.DrawText("one", rbtnEvent1Properties.X + rbtnEvent1Properties.Width + gv.scaler, rbtnEvent1Properties.Y + gv.fontHeight + gv.fontLineSpacing, color);

                color = "wh";
                if (selectedTrigger != null)
                {
                    if (selectedTrigger.Event2Type.Equals("conversation")) { color = "yl"; }
                    else if (selectedTrigger.Event2Type.Equals("encounter")) { color = "rd"; }
                    else if (selectedTrigger.Event2Type.Equals("script")) { color = "bu"; }
                    else if (selectedTrigger.Event2Type.Equals("transition")) { color = "gn"; }
                    else if (selectedTrigger.Event2Type.Equals("container")) { color = "ma"; }
                }
                rbtnEvent2Properties.Draw();
                gv.DrawText("Event", rbtnEvent2Properties.X + rbtnEvent2Properties.Width + gv.scaler, rbtnEvent2Properties.Y, color);
                gv.DrawText("two", rbtnEvent2Properties.X + rbtnEvent2Properties.Width + gv.scaler, rbtnEvent2Properties.Y + gv.fontHeight + gv.fontLineSpacing, color);

                color = "wh";
                if (selectedTrigger != null)
                {
                    if (selectedTrigger.Event3Type.Equals("conversation")) { color = "yl"; }
                    else if (selectedTrigger.Event3Type.Equals("encounter")) { color = "rd"; }
                    else if (selectedTrigger.Event3Type.Equals("script")) { color = "bu"; }
                    else if (selectedTrigger.Event3Type.Equals("transition")) { color = "gn"; }
                    else if (selectedTrigger.Event3Type.Equals("container")) { color = "ma"; }
                }
                rbtnEvent3Properties.Draw();
                gv.DrawText("Event", rbtnEvent3Properties.X + rbtnEvent3Properties.Width + gv.scaler, rbtnEvent3Properties.Y, color);
                gv.DrawText("three", rbtnEvent3Properties.X + rbtnEvent3Properties.Width + gv.scaler, rbtnEvent3Properties.Y + gv.fontHeight + gv.fontLineSpacing, color);

                if (rbtnTriggerProperties.toggleOn)
                {
                    tglTriggerTag.Draw();
                    gv.DrawText("Trigger Tag:", tglTriggerTag.X + tglTriggerTag.Width + gv.scaler, tglTriggerTag.Y, "gy");
                    gv.DrawText(selectedTrigger.TriggerTag, tglTriggerTag.X + tglTriggerTag.Width + gv.scaler, tglTriggerTag.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.Enabled) { tglTriggerEnabled.toggleOn = true; }
                    else { tglTriggerEnabled.toggleOn = false; }
                    tglTriggerEnabled.Draw();
                    gv.DrawText("Trigger", tglTriggerEnabled.X + tglTriggerEnabled.Width + gv.scaler, tglTriggerEnabled.Y, "gy");
                    gv.DrawText("Enabled", tglTriggerEnabled.X + tglTriggerEnabled.Width + gv.scaler, tglTriggerEnabled.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.DoOnceOnly) { tglTriggerDoOnce.toggleOn = true; }
                    else { tglTriggerDoOnce.toggleOn = false; }
                    tglTriggerDoOnce.Draw();
                    gv.DrawText("Do Once", tglTriggerDoOnce.X + tglTriggerDoOnce.Width + gv.scaler, tglTriggerDoOnce.Y, "gy");
                    gv.DrawText("Only", tglTriggerDoOnce.X + tglTriggerDoOnce.Width + gv.scaler, tglTriggerDoOnce.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    tglImageFilename.Draw();
                    gv.DrawText("Image Filename:", tglImageFilename.X + tglImageFilename.Width + gv.scaler, tglImageFilename.Y, "gy");
                    gv.DrawText(selectedTrigger.ImageFileName, tglImageFilename.X + tglImageFilename.Width + gv.scaler, tglImageFilename.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.ImageFacingLeft) { tglImageFacingLeft.toggleOn = true; }
                    else { tglImageFacingLeft.toggleOn = false; }
                    tglImageFacingLeft.Draw();
                    gv.DrawText("Face", tglImageFacingLeft.X + tglImageFacingLeft.Width + gv.scaler, tglImageFacingLeft.Y, "gy");
                    gv.DrawText("Left", tglImageFacingLeft.X + tglImageFacingLeft.Width + gv.scaler, tglImageFacingLeft.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.isShown) { tglIsShown.toggleOn = true; }
                    else { tglIsShown.toggleOn = false; }
                    tglIsShown.Draw();
                    gv.DrawText("Is", tglIsShown.X + tglIsShown.Width + gv.scaler, tglIsShown.Y, "gy");
                    gv.DrawText("Shown", tglIsShown.X + tglIsShown.Width + gv.scaler, tglIsShown.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    gv.DrawText("COMBAT ONLY:", tglNumberOfScriptCallsRemaining.X, tglNumberOfScriptCallsRemaining.Y - gv.fontHeight - gv.fontLineSpacing, "gn");

                    tglNumberOfScriptCallsRemaining.Draw();
                    gv.DrawText("Num of Calls", tglNumberOfScriptCallsRemaining.X + tglNumberOfScriptCallsRemaining.Width + gv.scaler, tglNumberOfScriptCallsRemaining.Y, "gy");
                    gv.DrawText(selectedTrigger.numberOfScriptCallsRemaining.ToString(), tglNumberOfScriptCallsRemaining.X + tglNumberOfScriptCallsRemaining.Width + gv.scaler, tglNumberOfScriptCallsRemaining.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.canBeTriggeredByPc) { tglCanBeTriggeredByPc.toggleOn = true; }
                    else { tglCanBeTriggeredByPc.toggleOn = false; }
                    tglCanBeTriggeredByPc.Draw();
                    gv.DrawText("Triggered", tglCanBeTriggeredByPc.X + tglCanBeTriggeredByPc.Width + gv.scaler, tglCanBeTriggeredByPc.Y, "gy");
                    gv.DrawText("by PC", tglCanBeTriggeredByPc.X + tglCanBeTriggeredByPc.Width + gv.scaler, tglCanBeTriggeredByPc.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.canBeTriggeredByCreature) { tglCanBeTriggeredByCreature.toggleOn = true; }
                    else { tglCanBeTriggeredByCreature.toggleOn = false; }
                    tglCanBeTriggeredByCreature.Draw();
                    gv.DrawText("Triggered", tglCanBeTriggeredByCreature.X + tglCanBeTriggeredByCreature.Width + gv.scaler, tglCanBeTriggeredByCreature.Y, "gy");
                    gv.DrawText("by Creatures", tglCanBeTriggeredByCreature.X + tglCanBeTriggeredByCreature.Width + gv.scaler, tglCanBeTriggeredByCreature.Y + gv.fontHeight + gv.fontLineSpacing, "gy");
                }
                if (rbtnEvent1Properties.toggleOn)
                {
                    if (selectedTrigger.EnabledEvent1) { tglEnabledEvent.toggleOn = true; }
                    else { tglEnabledEvent.toggleOn = false; }
                    tglEnabledEvent.Draw();
                    gv.DrawText("Event", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y, "gy");
                    gv.DrawText("Enabled", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.DoOnceOnlyEvent1) { tglDoOnceOnlyEvent.toggleOn = true; }
                    else { tglDoOnceOnlyEvent.toggleOn = false; }
                    tglDoOnceOnlyEvent.Draw();
                    gv.DrawText("Do Event", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y, "gy");
                    gv.DrawText("Once Only", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    tglEventType.Draw();
                    gv.DrawText("TYPE:", tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y, "gy");
                    gv.DrawText(selectedTrigger.Event1Type, tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    tglEventFilenameOrTag.Draw();
                    gv.DrawText("Filename/tag", tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y, "gy");
                    gv.DrawText(selectedTrigger.Event1FilenameOrTag, tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.Event1Type.Equals("transition"))
                    {
                        tglEventTransPointX.Draw();
                        gv.DrawText("Loc X:", tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y, "gy");
                        gv.DrawText(selectedTrigger.Event1TransPointX.ToString(), tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventTransPointY.Draw();
                        gv.DrawText("Loc Y:", tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y, "gy");
                        gv.DrawText(selectedTrigger.Event1TransPointY.ToString(), tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }

                    if (selectedTrigger.Event1Type.Equals("script"))
                    {
                        tglEventParm1.Draw();
                        gv.DrawText("Parm 1:", tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y, "gy");
                        gv.DrawText(selectedTrigger.Event1Parm1, tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm2.Draw();
                        gv.DrawText("Parm 2:", tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y, "gy");
                        gv.DrawText(selectedTrigger.Event1Parm2, tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm3.Draw();
                        gv.DrawText("Parm 3:", tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y, "gy");
                        gv.DrawText(selectedTrigger.Event1Parm3, tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm4.Draw();
                        gv.DrawText("Parm 4:", tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y, "gy");
                        gv.DrawText(selectedTrigger.Event1Parm4, tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }
                }
                if (rbtnEvent2Properties.toggleOn)
                {
                    if (selectedTrigger.EnabledEvent2) { tglEnabledEvent.toggleOn = true; }
                    else { tglEnabledEvent.toggleOn = false; }
                    tglEnabledEvent.Draw();
                    gv.DrawText("Event", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y, "gy");
                    gv.DrawText("Enabled", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.DoOnceOnlyEvent2) { tglDoOnceOnlyEvent.toggleOn = true; }
                    else { tglDoOnceOnlyEvent.toggleOn = false; }
                    tglDoOnceOnlyEvent.Draw();
                    gv.DrawText("Do Event", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y, "gy");
                    gv.DrawText("Once Only", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    tglEventType.Draw();
                    gv.DrawText("TYPE:", tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y, "gy");
                    gv.DrawText(selectedTrigger.Event2Type, tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    tglEventFilenameOrTag.Draw();
                    gv.DrawText("Filename/tag", tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y, "gy");
                    gv.DrawText(selectedTrigger.Event2FilenameOrTag, tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.Event2Type.Equals("transition"))
                    {
                        tglEventTransPointX.Draw();
                        gv.DrawText("Loc X:", tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y, "gy");
                        gv.DrawText(selectedTrigger.Event2TransPointX.ToString(), tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventTransPointY.Draw();
                        gv.DrawText("Loc Y:", tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y, "gy");
                        gv.DrawText(selectedTrigger.Event2TransPointY.ToString(), tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }

                    if (selectedTrigger.Event2Type.Equals("script"))
                    {
                        tglEventParm1.Draw();
                        gv.DrawText("Parm 1:", tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y, "gy");
                        gv.DrawText(selectedTrigger.Event2Parm1, tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm2.Draw();
                        gv.DrawText("Parm 2:", tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y, "gy");
                        gv.DrawText(selectedTrigger.Event2Parm2, tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm3.Draw();
                        gv.DrawText("Parm 3:", tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y, "gy");
                        gv.DrawText(selectedTrigger.Event2Parm3, tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm4.Draw();
                        gv.DrawText("Parm 4:", tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y, "gy");
                        gv.DrawText(selectedTrigger.Event2Parm4, tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }
                }
                if (rbtnEvent3Properties.toggleOn)
                {
                    if (selectedTrigger.EnabledEvent3) { tglEnabledEvent.toggleOn = true; }
                    else { tglEnabledEvent.toggleOn = false; }
                    tglEnabledEvent.Draw();
                    gv.DrawText("Event", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y, "gy");
                    gv.DrawText("Enabled", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.DoOnceOnlyEvent3) { tglDoOnceOnlyEvent.toggleOn = true; }
                    else { tglDoOnceOnlyEvent.toggleOn = false; }
                    tglDoOnceOnlyEvent.Draw();
                    gv.DrawText("Do Event", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y, "gy");
                    gv.DrawText("Once Only", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    tglEventType.Draw();
                    gv.DrawText("TYPE:", tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y, "gy");
                    gv.DrawText(selectedTrigger.Event3Type, tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    tglEventFilenameOrTag.Draw();
                    gv.DrawText("Filename/tag", tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y, "gy");
                    gv.DrawText(selectedTrigger.Event3FilenameOrTag, tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.Event3Type.Equals("transition"))
                    {
                        tglEventTransPointX.Draw();
                        gv.DrawText("Loc X:", tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y, "gy");
                        gv.DrawText(selectedTrigger.Event3TransPointX.ToString(), tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventTransPointY.Draw();
                        gv.DrawText("Loc Y:", tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y, "gy");
                        gv.DrawText(selectedTrigger.Event3TransPointY.ToString(), tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }

                    if (selectedTrigger.Event3Type.Equals("script"))
                    {
                        tglEventParm1.Draw();
                        gv.DrawText("Parm 1:", tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y, "gy");
                        gv.DrawText(selectedTrigger.Event3Parm1, tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm2.Draw();
                        gv.DrawText("Parm 2:", tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y, "gy");
                        gv.DrawText(selectedTrigger.Event3Parm2, tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm3.Draw();
                        gv.DrawText("Parm 3:", tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y, "gy");
                        gv.DrawText(selectedTrigger.Event3Parm3, tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm4.Draw();
                        gv.DrawText("Parm 4:", tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y, "gy");
                        gv.DrawText(selectedTrigger.Event3Parm4, tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }
                }
            }

            //draw selected info tile highlight
            int tlX = selectedSquare.X * gv.squareSize / mapSquareSizeScaler * gv.scaler;
            int tlY = selectedSquare.Y * gv.squareSize / mapSquareSizeScaler * gv.scaler;
            int brX = gv.squareSize / mapSquareSizeScaler * gv.scaler;
            int brY = gv.squareSize / mapSquareSizeScaler * gv.scaler;
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
            dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList("highlight_magenta"), src, dst);

            btnHelp.Draw();
        }
        public void drawWalkLoSPanel()
        {
            //Description     
            gv.DrawText("WALKABLE", panelLeftLocation, panelTopLocation + gv.fontHeight + gv.fontLineSpacing, "gn");
            gv.DrawText("LINE-OF-SIGHT", panelLeftLocation, panelTopLocation + (2 * (gv.fontHeight + gv.fontLineSpacing)), "gn");

            rbtnWalkOpen.Draw();
            gv.DrawText("WALKABLE", rbtnWalkOpen.X + rbtnWalkOpen.Width + gv.scaler, rbtnWalkOpen.Y, "wh");
            gv.DrawText("OPEN", rbtnWalkOpen.X + rbtnWalkOpen.Width + gv.scaler, rbtnWalkOpen.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            rbtnWalkBlocking.Draw();
            gv.DrawText("WALKABLE", rbtnWalkBlocking.X + rbtnWalkBlocking.Width + gv.scaler, rbtnWalkBlocking.Y, "wh");
            gv.DrawText("BLOCK", rbtnWalkBlocking.X + rbtnWalkBlocking.Width + gv.scaler, rbtnWalkBlocking.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            rbtnSightOpen.Draw();
            gv.DrawText("Line-of-Sight", rbtnSightOpen.X + rbtnSightOpen.Width + gv.scaler, rbtnSightOpen.Y, "wh");
            gv.DrawText("Visible", rbtnSightOpen.X + rbtnSightOpen.Width + gv.scaler, rbtnSightOpen.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            rbtnSightBlocking.Draw();
            gv.DrawText("Line-of-Sight", rbtnSightBlocking.X + rbtnSightBlocking.Width + gv.scaler, rbtnSightBlocking.Y, "wh");
            gv.DrawText("Blocked", rbtnSightBlocking.X + rbtnSightBlocking.Width + gv.scaler, rbtnSightBlocking.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            gv.DrawText("select one of", panelLeftLocation, panelTopLocation + (14 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            gv.DrawText("the above actions", panelLeftLocation, panelTopLocation + (15 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            gv.DrawText("then tap on a map", panelLeftLocation, panelTopLocation + (16 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            gv.DrawText("square to make", panelLeftLocation, panelTopLocation + (17 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            gv.DrawText("the setting", panelLeftLocation, panelTopLocation + (18 * (gv.fontHeight + gv.fontLineSpacing)), "gn");

            btnHelp.Draw();
        }
        public void drawPropsPanel()
        {
            //Description            
            int yLoc = 0 * gv.uiSquareSize;
            description.tbXloc = 8 * gv.uiSquareSize;
            description.tbYloc = yLoc;
            description.tbWidth = 3 * gv.uiSquareSize;
            description.tbHeight = 4 * gv.uiSquareSize;
            string textToSpan = "";
            textToSpan = "<gn>PROPS:</gn>" + Environment.NewLine;
            textToSpan += gv.mod.moduleDescription;
            description.linesList.Clear();
            description.AddFormattedTextToTextBox(textToSpan);
            description.onDrawTextBox();

            btnHelp.Draw();
        }
        public void drawSettingsPanel()
        {
            //Description     
            gv.DrawText("ENCOUNTER SETTINGS", panelLeftLocation, panelTopLocation, "gn");

            int shiftForFont = (tglSettingEncounterName.Height / 2) - (gv.fontHeight / 2);

            tglSettingEncounterName.Draw();
            gv.DrawText("Encounter Name:", tglSettingEncounterName.X + tglSettingEncounterName.Width + gv.scaler, tglSettingEncounterName.Y, "yl");
            gv.DrawText(gv.mod.currentEncounter.encounterName, tglSettingEncounterName.X + tglSettingEncounterName.Width + gv.scaler, tglSettingEncounterName.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            tglSettingGoldDrop.Draw();
            gv.DrawText("Gold Drop:", tglSettingGoldDrop.X + tglSettingGoldDrop.Width + gv.scaler, tglSettingGoldDrop.Y, "yl");
            gv.DrawText(gv.mod.currentEncounter.goldDrop.ToString(), tglSettingGoldDrop.X + tglSettingGoldDrop.Width + gv.scaler, tglSettingGoldDrop.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            if (gv.mod.currentEncounter.UseDayNightCycle) { tglSettingUseDayNightCycle.toggleOn = true; }
            else { tglSettingUseDayNightCycle.toggleOn = false; }
            tglSettingUseDayNightCycle.Draw();
            gv.DrawText("Uses Day/", tglSettingUseDayNightCycle.X + tglSettingUseDayNightCycle.Width + gv.scaler, tglSettingUseDayNightCycle.Y, "yl");
            gv.DrawText("Night Cycle", tglSettingUseDayNightCycle.X + tglSettingUseDayNightCycle.Width + gv.scaler, tglSettingUseDayNightCycle.Y + gv.fontHeight + gv.fontLineSpacing, "yl");

            tglSettingPlacePCs.Draw();
            gv.DrawText("Place PC Start", tglSettingPlacePCs.X + tglSettingPlacePCs.Width + gv.scaler, tglSettingPlacePCs.Y, "yl");
            gv.DrawText("Locations Mode", tglSettingPlacePCs.X + tglSettingPlacePCs.Width + gv.scaler, tglSettingPlacePCs.Y + gv.fontHeight + gv.fontLineSpacing, "yl");

            tglSettingRemoveAllPCs.Draw();
            gv.DrawText("Remove", tglSettingRemoveAllPCs.X + tglSettingRemoveAllPCs.Width + gv.scaler, tglSettingRemoveAllPCs.Y, "yl");
            gv.DrawText("PCs", tglSettingRemoveAllPCs.X + tglSettingRemoveAllPCs.Width + gv.scaler, tglSettingRemoveAllPCs.Y + gv.fontHeight + gv.fontLineSpacing, "yl");

            tglSettingRemoveAllCrts.Draw();
            gv.DrawText("Remove", tglSettingRemoveAllCrts.X + tglSettingRemoveAllCrts.Width + gv.scaler, tglSettingRemoveAllCrts.Y, "yl");
            gv.DrawText("Crturs", tglSettingRemoveAllCrts.X + tglSettingRemoveAllCrts.Width + gv.scaler, tglSettingRemoveAllCrts.Y + gv.fontHeight + gv.fontLineSpacing, "yl");

            gv.DrawText("Encounter Loot", tglSettingAddItem.X, tglSettingAddItem.Y - gv.fontHeight - gv.fontLineSpacing, "gn");

            tglSettingAddItem.Draw();
            gv.DrawText("Add", tglSettingAddItem.X + tglSettingAddItem.Width + gv.scaler, tglSettingAddItem.Y, "yl");
            gv.DrawText("Item", tglSettingAddItem.X + tglSettingAddItem.Width + gv.scaler, tglSettingAddItem.Y + gv.fontHeight + gv.fontLineSpacing, "yl");

            tglSettingRemoveItem.Draw();
            gv.DrawText("Remove", tglSettingRemoveItem.X + tglSettingRemoveItem.Width + gv.scaler, tglSettingRemoveItem.Y, "yl");
            gv.DrawText("Item", tglSettingRemoveItem.X + tglSettingRemoveItem.Width + gv.scaler, tglSettingRemoveItem.Y + gv.fontHeight + gv.fontLineSpacing, "yl");

            int cnt = 0;
            //gv.mod.currentEncounter.encounterInventoryRefsList.Add(new ItemRefs());
            foreach (ItemRefs it in gv.mod.currentEncounter.encounterInventoryRefsList)
            {
                gv.DrawText(it.name, tglSettingAddItem.X, tglSettingAddItem.Y + (cnt + 2) * (gv.fontHeight + gv.fontLineSpacing), "wh");
                cnt++;
            }
            btnHelp.Draw();
        }
        public void drawCrtPanel()
        {
            int cnt = 0;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < (crtSlotsPerPage / 4); y++)
                {
                    string crt = "none";
                    if ((cnt + (crtPageIndex * crtSlotsPerPage)) < gv.cc.allCreaturesList.Count)
                    {
                        crt = gv.cc.allCreaturesList[cnt + (crtPageIndex * crtSlotsPerPage)].cr_tokenFilename;
                    }
                    int tlX = x * gv.squareSize * gv.scaler;
                    int tlY = y * gv.squareSize * gv.scaler;
                    int brX = gv.squareSize * gv.scaler;
                    int brY = gv.squareSize * gv.scaler;

                    try
                    {
                        src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(crt).Width, gv.cc.GetFromTileBitmapList(crt).Width);
                        dst = new IbRect(tlX + panelLeftLocation, tlY + panelTopLocation, brX, brY);
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList(crt), src, dst);
                    }
                    catch { }
                    cnt++;
                }
            }

            int slotX = crtSlotIndex / (crtSlotsPerPage / 4);
            int slotY = crtSlotIndex - slotX * (crtSlotsPerPage / 4);
            int tlX2 = slotX * gv.squareSize * gv.scaler;
            int tlY2 = slotY * gv.squareSize * gv.scaler;
            int brX2 = gv.squareSize * gv.scaler;
            int brY2 = gv.squareSize * gv.scaler;
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
            dst = new IbRect(tlX2 + panelLeftLocation, tlY2 + panelTopLocation, brX2, brY2);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList("highlight_magenta"), src, dst);

            //Description     
            gv.DrawText("PLACE CREATURES", panelLeftLocation, panelTopLocation, "gn");

            btnCrtLeft.Draw();
            btnCrtPageIndex.Draw();
            if (currentCrt != null)
            {
                tlX2 = btnCrtPageIndex.X + ((gv.uiSquareSize - (gv.squareSize * gv.scaler)) / 2);
                tlY2 = btnCrtPageIndex.Y + ((gv.uiSquareSize - (gv.squareSize * gv.scaler)) / 2);
                brX2 = gv.squareSize * gv.scaler;
                brY2 = gv.squareSize * gv.scaler;
                src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(currentCrt.cr_tokenFilename).Width, gv.cc.GetFromTileBitmapList(currentCrt.cr_tokenFilename).Width);
                dst = new IbRect(tlX2, tlY2, brX2, brY2);
                gv.DrawBitmap(gv.cc.GetFromTileBitmapList(currentCrt.cr_tokenFilename), src, dst);
            }
            btnCrtRight.Draw();
            if (currentCrt != null)
            {
                gv.DrawText("CREATURE INFO", panelLeftLocation, panelTopLocation + (23 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
                gv.DrawText("Name:", panelLeftLocation, panelTopLocation + (24 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
                gv.DrawText(currentCrt.cr_name, panelLeftLocation, panelTopLocation + (25 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
                gv.DrawText("AC: " + currentCrt.AC.ToString(), panelLeftLocation, panelTopLocation + (26 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
                gv.DrawText("HP: " + currentCrt.hp.ToString(), panelLeftLocation, panelTopLocation + (27 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
                gv.DrawText("Attack BAB: " + currentCrt.cr_att.ToString(), panelLeftLocation, panelTopLocation + (28 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
                gv.DrawText("Attack Range: " + currentCrt.cr_attRange.ToString(), panelLeftLocation, panelTopLocation + (29 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            }
        }

        public void onTouchTsEncEditor(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnInfo.glowOn = false;
            btnTiles.glowOn = false;
            btnTriggers.glowOn = false;
            btnWalkLoS.glowOn = false;
            btnCrt.glowOn = false;
            btnSettings.glowOn = false;
            btnHelp.glowOn = false;

            if (eventType == MouseEventType.EventType.MouseDown)
            {
                touchDown = true;
            }
            else if (eventType == MouseEventType.EventType.MouseUp)
            {
                touchDown = false;
            }

            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }

            bool ret = gv.tsMainMenu.onTouchTsMainMenu(eX, eY, eventType);
            if (ret) { return; } //did some action on the main menu so do nothing here

            if (currentMode.Equals("Tiles"))
            {
                ret = onTouchTilePanel(eX, eY, eventType);
                if (ret) { return; } //did some action on the tile panel so do nothing here
            }
            else if (currentMode.Equals("Settings"))
            {
                ret = onTouchSettingsPanel(eX, eY, eventType);
                if (ret) { return; } //did some action on the Settings panel so do nothing here
            }
            else if (currentMode.Equals("Info"))
            {
                ret = onTouchInfoPanel(eX, eY, eventType);
                if (ret) { return; } //did some action on the Info panel so do nothing here
            }
            else if (currentMode.Equals("WalkLoS"))
            {
                ret = onTouchWalkLoSPanel(eX, eY, eventType);
                if (ret) { return; } //did some action on the Walk-LoS panel so do nothing here
            }
            else if (currentMode.Equals("Triggers"))
            {
                ret = onTouchTriggerPanel(eX, eY, eventType);
                if (ret) { return; } //did some action on the Walk-LoS panel so do nothing here
            }
            else if (currentMode.Equals("Crt"))
            {
                ret = onTouchCrtPanel(eX, eY, eventType);
                if (ret) { return; } //did some action on the 3DPreview panel so do nothing here
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

                    if (btnInfo.getImpact(x, y))
                    {
                        btnInfo.glowOn = true;
                    }
                    else if (btnTiles.getImpact(x, y))
                    {
                        btnTiles.glowOn = true;
                    }
                    else if (btnTriggers.getImpact(x, y))
                    {
                        btnTriggers.glowOn = true;
                    }
                    else if (btnWalkLoS.getImpact(x, y))
                    {
                        btnWalkLoS.glowOn = true;
                    }
                    else if (btnCrt.getImpact(x, y))
                    {
                        btnCrt.glowOn = true;
                    }
                    else if (btnSettings.getImpact(x, y))
                    {
                        btnSettings.glowOn = true;
                    }
                    else if (btnHelp.getImpact(x, y))
                    {
                        btnHelp.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnInfo.glowOn = false;
                    btnTiles.glowOn = false;
                    btnTriggers.glowOn = false;
                    btnWalkLoS.glowOn = false;
                    btnCrt.glowOn = false;
                    btnSettings.glowOn = false;
                    btnHelp.glowOn = false;

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

                    if (btnInfo.getImpact(x, y))
                    {
                        selectedSquare.X = 0;
                        selectedSquare.Y = 0;
                        currentMode = "Info";
                    }
                    else if (btnTiles.getImpact(x, y))
                    {
                        selectedSquare.X = 0;
                        selectedSquare.Y = 0;
                        currentMode = "Tiles";
                    }
                    else if (btnTriggers.getImpact(x, y))
                    {
                        selectedSquare.X = 0;
                        selectedSquare.Y = 0;
                        currentMode = "Triggers";
                    }
                    else if (btnWalkLoS.getImpact(x, y))
                    {
                        selectedSquare.X = 0;
                        selectedSquare.Y = 0;
                        currentMode = "WalkLoS";
                    }
                    else if (btnCrt.getImpact(x, y))
                    {
                        selectedSquare.X = 0;
                        selectedSquare.Y = 0;
                        currentMode = "Crt";
                    }
                    else if (btnSettings.getImpact(x, y))
                    {
                        selectedSquare.X = 0;
                        selectedSquare.Y = 0;
                        currentMode = "Settings";
                    }
                    else if (btnHelp.getImpact(x, y))
                    {
                        //incrementalSaveModule();
                    }
                    break;
            }
        }
        public bool onTouchTilePanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnTilesLeft.glowOn = false;
            btnTilesRight.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (touchDown)
                    {
                        //figure out if tapped on a map square
                        int gridX = (eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                        int gridY = eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                        if ((tapInMapArea(gridX, gridY)) && (tapInMapViewport(x, y)))
                        {
                            if (rbtnEditLayer1.toggleOn)
                            {
                                gv.mod.currentEncounter.Layer1Filename[gridY * gv.mod.currentEncounter.MapSizeX + gridX] = currentTile;
                            }
                            else if (rbtnEditLayer2.toggleOn)
                            {
                                gv.mod.currentEncounter.Layer2Filename[gridY * gv.mod.currentEncounter.MapSizeX + gridX] = currentTile;
                            }
                            else if (rbtnEditLayer3.toggleOn)
                            {
                                gv.mod.currentEncounter.Layer3Filename[gridY * gv.mod.currentEncounter.MapSizeX + gridX] = currentTile;
                            }
                        }
                    }

                    if (btnTilesLeft.getImpact(x, y))
                    {
                        btnTilesLeft.glowOn = true;
                        return true;
                    }
                    else if (btnTilesRight.getImpact(x, y))
                    {
                        btnTilesRight.glowOn = true;
                        return true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnTilesLeft.glowOn = false;
                    btnTilesRight.glowOn = false;

                    //figure out if tapped on a tile in palette
                    if ((x > (panelLeftLocation)) && (x < (panelLeftLocation) + (4 * gv.squareSize * gv.scaler)))
                    {
                        if ((y > (panelTopLocation)) && (y < (panelTopLocation + ((tileSlotsPerPage / 4) * gv.squareSize * gv.scaler))))
                        {
                            //inside the palette area so determine which slot index tapped on
                            int xloc = (x - panelLeftLocation) / (gv.squareSize * gv.scaler);
                            int yloc = (y - panelTopLocation) / (gv.squareSize * gv.scaler);

                            if ((((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage) < tilesList.Count)
                            {
                                tileSlotIndex = (xloc * (tileSlotsPerPage / 4)) + yloc;
                                currentTile = tilesList[((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage];
                            }
                        }
                    }

                    if (btnTilesLeft.getImpact(x, y))
                    {
                        if (tilesPageIndex > 0)
                        {
                            tilesPageIndex--;
                            btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                        }
                        return true;
                    }
                    else if (btnTilesRight.getImpact(x, y))
                    {

                        if (tilesPageIndex <= (tilesList.Count / tileSlotsPerPage) - 1)
                        {
                            tilesPageIndex++;
                            btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                        }

                        return true;
                    }
                    else if (rbtnShowLayer1.getImpact(x, y))
                    {
                        rbtnShowLayer1.toggleOn = !rbtnShowLayer1.toggleOn;
                    }
                    else if (rbtnShowLayer2.getImpact(x, y))
                    {
                        rbtnShowLayer2.toggleOn = !rbtnShowLayer2.toggleOn;
                    }
                    else if (rbtnShowLayer3.getImpact(x, y))
                    {
                        rbtnShowLayer3.toggleOn = !rbtnShowLayer3.toggleOn;
                    }
                    else if (rbtnEditLayer1.getImpact(x, y))
                    {
                        rbtnEditLayer1.toggleOn = true;
                        rbtnEditLayer2.toggleOn = false;
                        rbtnEditLayer3.toggleOn = false;

                    }
                    else if (rbtnEditLayer2.getImpact(x, y))
                    {
                        rbtnEditLayer1.toggleOn = false;
                        rbtnEditLayer2.toggleOn = true;
                        rbtnEditLayer3.toggleOn = false;

                    }
                    else if (rbtnEditLayer3.getImpact(x, y))
                    {
                        rbtnEditLayer1.toggleOn = false;
                        rbtnEditLayer2.toggleOn = false;
                        rbtnEditLayer3.toggleOn = true;

                    }
                    break;
            }
            return false;
        }
        public bool onTouchSettingsPanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnHelp.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (btnHelp.getImpact(x, y))
                    {
                        btnHelp.glowOn = true;
                        return true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnHelp.glowOn = false;

                    if (!tapInMapViewport(x, y))
                    {
                        if (tglSettingPlacePCs.getImpact(x, y))
                        {
                            tglSettingPlacePCs.toggleOn = !tglSettingPlacePCs.toggleOn;
                        }
                        else
                        {
                            tglSettingPlacePCs.toggleOn = false;
                        }
                    }
                    else
                    {
                        if (tglSettingPlacePCs.toggleOn)
                        {
                            if (gv.mod.currentEncounter.encounterPcStartLocations.Count < 6)
                            {
                                int gridX = (eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                                int gridY = eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                                gv.mod.currentEncounter.encounterPcStartLocations.Add(new Coordinate(gridX, gridY));
                            }
                        }
                    }

                    if (tglSettingEncounterName.getImpact(x, y))
                    {
                        changeEncounterName();
                    }
                    else if (tglSettingGoldDrop.getImpact(x, y))
                    {
                        changeGoldDrop();
                    }
                    else if (tglSettingUseDayNightCycle.getImpact(x, y))
                    {
                        tglSettingUseDayNightCycle.toggleOn = !tglSettingUseDayNightCycle.toggleOn;
                        gv.mod.currentEncounter.UseDayNightCycle = tglSettingUseDayNightCycle.toggleOn;
                    }
                    else if (tglSettingPlacePCs.getImpact(x, y))
                    {
                        //tglSettingPlacePCs.toggleOn = !tglSettingPlacePCs.toggleOn;
                    }
                    else if (tglSettingRemoveAllPCs.getImpact(x, y))
                    {
                        gv.mod.currentEncounter.encounterPcStartLocations.Clear();
                    }
                    else if (tglSettingRemoveAllCrts.getImpact(x, y))
                    {
                        gv.mod.currentEncounter.encounterCreatureRefsList.Clear();
                    }
                    else if (tglSettingAddItem.getImpact(x, y))
                    {
                        addItemToLootList();
                    }
                    else if (tglSettingRemoveItem.getImpact(x, y))
                    {
                        removeItemFromLootList();
                    }
                    else if (btnHelp.getImpact(x, y))
                    {
                        //incrementalSaveModule();
                        /*if (mapSquareSizeScaler == 1)
                        {
                            mapSquareSizeScaler = 2;
                        }
                        else
                        {
                            mapSquareSizeScaler = 1;
                        }*/
                    }
                    break;
            }
            return false;
        }
        public bool onTouchInfoPanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnHelp.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (btnHelp.getImpact(x, y))
                    {
                        btnHelp.glowOn = true;
                        return true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    //figure out if tapped on a map square
                    int gridX = (eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                    int gridY = eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                    if ((tapInMapArea(gridX, gridY)) && (tapInMapViewport(x, y)))
                    {
                        selectedSquare.X = gridX;
                        selectedSquare.Y = gridY;
                    }

                    btnHelp.glowOn = false;
                    break;
            }
            return false;
        }
        public bool onTouchWalkLoSPanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnHelp.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (touchDown)
                    {
                        //figure out if tapped on a map square
                        int gridX = (eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                        int gridY = eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                        if ((tapInMapArea(gridX, gridY)) && (tapInMapViewport(x, y)))
                        {
                            if (rbtnWalkOpen.toggleOn)
                            {
                                gv.mod.currentEncounter.Walkable[gridY * gv.mod.currentEncounter.MapSizeX + gridX] = 1;
                            }
                            else if (rbtnWalkBlocking.toggleOn)
                            {
                                gv.mod.currentEncounter.Walkable[gridY * gv.mod.currentEncounter.MapSizeX + gridX] = 0;
                            }
                            else if (rbtnSightOpen.toggleOn)
                            {
                                gv.mod.currentEncounter.LoSBlocked[gridY * gv.mod.currentEncounter.MapSizeX + gridX] = 0;
                            }
                            else if (rbtnSightBlocking.toggleOn)
                            {
                                gv.mod.currentEncounter.LoSBlocked[gridY * gv.mod.currentEncounter.MapSizeX + gridX] = 1;
                            }
                        }
                    }

                    if (btnHelp.getImpact(x, y))
                    {
                        btnHelp.glowOn = true;
                        return true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnHelp.glowOn = false;

                    if (rbtnWalkOpen.getImpact(x, y))
                    {
                        rbtnWalkOpen.toggleOn = true;
                        rbtnWalkBlocking.toggleOn = false;
                        rbtnSightOpen.toggleOn = false;
                        rbtnSightBlocking.toggleOn = false;
                    }
                    else if (rbtnWalkBlocking.getImpact(x, y))
                    {
                        rbtnWalkOpen.toggleOn = false;
                        rbtnWalkBlocking.toggleOn = true;
                        rbtnSightOpen.toggleOn = false;
                        rbtnSightBlocking.toggleOn = false;
                    }
                    else if (rbtnSightOpen.getImpact(x, y))
                    {
                        rbtnWalkOpen.toggleOn = false;
                        rbtnWalkBlocking.toggleOn = false;
                        rbtnSightOpen.toggleOn = true;
                        rbtnSightBlocking.toggleOn = false;
                    }
                    else if (rbtnSightBlocking.getImpact(x, y))
                    {
                        rbtnWalkOpen.toggleOn = false;
                        rbtnWalkBlocking.toggleOn = false;
                        rbtnSightOpen.toggleOn = false;
                        rbtnSightBlocking.toggleOn = true;
                    }
                    break;
            }
            return false;
        }
        public bool onTouchTriggerPanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnHelp.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (btnHelp.getImpact(x, y))
                    {
                        btnHelp.glowOn = true;
                        return true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    //figure out if tapped on a map square
                    int gridX = (eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                    int gridY = eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                    if ((tapInMapArea(gridX, gridY)) && (tapInMapViewport(x, y)))
                    {
                        selectedSquare.X = gridX;
                        selectedSquare.Y = gridY;
                    }

                    #region Tap on Map
                    //figure out if tapped on a map square
                    //int gridX2 = (eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                    //int gridY2 = eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                    if ((tapInMapArea(gridX, gridY)) && (tapInMapViewport(x, y)))
                    {
                        if (rbtnViewInfoTrigger.toggleOn)
                        {
                            selectedTrigger = gv.mod.currentEncounter.getTriggerByLocation(gridX, gridY);
                        }
                        else if (rbtnPlaceNewTrigger.toggleOn)
                        {
                            if (selectedTrigger == null)
                            {
                                if (!squareUsedByAnyTrigger(gridX, gridY))
                                {
                                    //create a new trigger object
                                    Trigger newTrigger = new Trigger();
                                    //increment the tag to something unique
                                    newTrigger.TriggerTag = "newTrigger_" + gv.mod.getNextIdNumber();
                                    Coordinate newCoor = new Coordinate();
                                    newCoor.X = gridX;
                                    newCoor.Y = gridY;
                                    newTrigger.TriggerSquaresList.Add(newCoor);
                                    gv.mod.currentEncounter.Triggers.Add(newTrigger);
                                    selectedTrigger = newTrigger;
                                }
                            }
                            else
                            {
                                //add the selected square to the squareList if doesn't already exist                                    
                                //check: if click square already exists, then erase from list                            
                                Trigger newTrigger = gv.mod.currentEncounter.getTriggerByTag(selectedTrigger.TriggerTag);
                                if (newTrigger != null)
                                {
                                    bool exists = false;
                                    foreach (Coordinate p in newTrigger.TriggerSquaresList)
                                    {
                                        if ((p.X == gridX) && (p.Y == gridY))
                                        {
                                            //already exists, erase
                                            newTrigger.TriggerSquaresList.Remove(p);
                                            exists = true;
                                            break;
                                        }
                                    }
                                    if (!exists) //doesn't exist so is a new point, add to list
                                    {
                                        if (!squareUsedByOtherTrigger(gridX, gridY))
                                        {
                                            Coordinate newCoor = new Coordinate();
                                            newCoor.X = gridX;
                                            newCoor.Y = gridY;
                                            newTrigger.TriggerSquaresList.Add(newCoor);
                                        }
                                    }
                                }
                            }
                        }
                        else if (rbtnEditTrigger.toggleOn)
                        {
                            //add the selected square to the squareList if doesn't already exist                                    
                            //check: if click square already exists, then erase from list                            
                            Trigger newTrigger = gv.mod.currentEncounter.getTriggerByTag(selectedTrigger.TriggerTag);
                            if (newTrigger != null)
                            {
                                bool exists = false;
                                foreach (Coordinate p in newTrigger.TriggerSquaresList)
                                {
                                    if ((p.X == gridX) && (p.Y == gridY))
                                    {
                                        //already exists, erase
                                        newTrigger.TriggerSquaresList.Remove(p);
                                        exists = true;
                                        break;
                                    }
                                }
                                if (!exists) //doesn't exist so is a new point, add to list
                                {
                                    if (!squareUsedByOtherTrigger(gridX, gridY))
                                    {
                                        Coordinate newCoor = new Coordinate();
                                        newCoor.X = gridX;
                                        newCoor.Y = gridY;
                                        newTrigger.TriggerSquaresList.Add(newCoor);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    btnHelp.glowOn = false;

                    if (rbtnViewInfoTrigger.getImpact(x, y))
                    {
                        rbtnViewInfoTrigger.toggleOn = true;
                        rbtnPlaceNewTrigger.toggleOn = false;
                        rbtnEditTrigger.toggleOn = false;
                    }
                    else if (rbtnPlaceNewTrigger.getImpact(x, y))
                    {
                        rbtnViewInfoTrigger.toggleOn = false;
                        rbtnPlaceNewTrigger.toggleOn = true;
                        rbtnEditTrigger.toggleOn = false;
                        selectedTrigger = null;
                    }
                    else if ((rbtnEditTrigger.getImpact(x, y)) && (selectedTrigger != null))
                    {
                        rbtnViewInfoTrigger.toggleOn = false;
                        rbtnPlaceNewTrigger.toggleOn = false;
                        rbtnEditTrigger.toggleOn = true;
                    }
                    else if (rbtnTriggerProperties.getImpact(x, y))
                    {
                        rbtnTriggerProperties.toggleOn = true;
                        rbtnEvent1Properties.toggleOn = false;
                        rbtnEvent2Properties.toggleOn = false;
                        rbtnEvent3Properties.toggleOn = false;
                    }
                    else if (rbtnEvent1Properties.getImpact(x, y))
                    {
                        rbtnTriggerProperties.toggleOn = false;
                        rbtnEvent1Properties.toggleOn = true;
                        rbtnEvent2Properties.toggleOn = false;
                        rbtnEvent3Properties.toggleOn = false;
                    }
                    else if (rbtnEvent2Properties.getImpact(x, y))
                    {
                        rbtnTriggerProperties.toggleOn = false;
                        rbtnEvent1Properties.toggleOn = false;
                        rbtnEvent2Properties.toggleOn = true;
                        rbtnEvent3Properties.toggleOn = false;
                    }
                    else if (rbtnEvent3Properties.getImpact(x, y))
                    {
                        rbtnTriggerProperties.toggleOn = false;
                        rbtnEvent1Properties.toggleOn = false;
                        rbtnEvent2Properties.toggleOn = false;
                        rbtnEvent3Properties.toggleOn = true;
                    }
                    if (rbtnEditTrigger.toggleOn)
                    {
                        if (rbtnTriggerProperties.toggleOn)
                        {
                            if (tglTriggerTag.getImpact(x, y))
                            {
                                changeTriggerTag();
                            }
                            else if (tglNumberOfScriptCallsRemaining.getImpact(x, y))
                            {
                                changeNumberOfScriptCallsRemaining();
                            }
                            else if (tglTriggerEnabled.getImpact(x, y))
                            {
                                if (selectedTrigger != null)
                                {
                                    tglTriggerEnabled.toggleOn = !tglTriggerEnabled.toggleOn;
                                    selectedTrigger.Enabled = tglTriggerEnabled.toggleOn;
                                }
                            }
                            else if (tglTriggerDoOnce.getImpact(x, y))
                            {
                                if (selectedTrigger != null)
                                {
                                    tglTriggerDoOnce.toggleOn = !tglTriggerDoOnce.toggleOn;
                                    selectedTrigger.DoOnceOnly = tglTriggerDoOnce.toggleOn;
                                }
                            }
                            else if (tglImageFilename.getImpact(x, y))
                            {
                                if (selectedTrigger != null)
                                {
                                    gv.screenType = "tokenSelector";
                                    gv.screenTokenSelector.resetTokenSelector("tsEncEditor", null);
                                }
                            }
                            else if (tglImageFacingLeft.getImpact(x, y))
                            {
                                if (selectedTrigger != null)
                                {
                                    tglImageFacingLeft.toggleOn = !tglImageFacingLeft.toggleOn;
                                    selectedTrigger.ImageFacingLeft = tglImageFacingLeft.toggleOn;
                                }
                            }
                            else if (tglIsShown.getImpact(x, y))
                            {
                                if (selectedTrigger != null)
                                {
                                    tglIsShown.toggleOn = !tglIsShown.toggleOn;
                                    selectedTrigger.isShown = tglIsShown.toggleOn;
                                }
                            }
                            else if (tglCanBeTriggeredByPc.getImpact(x, y))
                            {
                                if (selectedTrigger != null)
                                {
                                    tglCanBeTriggeredByPc.toggleOn = !tglCanBeTriggeredByPc.toggleOn;
                                    selectedTrigger.canBeTriggeredByPc = tglCanBeTriggeredByPc.toggleOn;
                                }
                            }
                            else if (tglCanBeTriggeredByCreature.getImpact(x, y))
                            {
                                if (selectedTrigger != null)
                                {
                                    tglCanBeTriggeredByCreature.toggleOn = !tglCanBeTriggeredByCreature.toggleOn;
                                    selectedTrigger.canBeTriggeredByCreature = tglCanBeTriggeredByCreature.toggleOn;
                                }
                            }
                        }
                        if ((rbtnEvent1Properties.toggleOn) || (rbtnEvent2Properties.toggleOn) || (rbtnEvent3Properties.toggleOn))
                        {
                            int eventNumber = 3;
                            if (rbtnEvent1Properties.toggleOn) { eventNumber = 1; }
                            else if (rbtnEvent2Properties.toggleOn) { eventNumber = 2; }

                            if (tglEnabledEvent.getImpact(x, y))
                            {
                                if (selectedTrigger != null)
                                {
                                    if (eventNumber == 1)
                                    {
                                        selectedTrigger.EnabledEvent1 = !selectedTrigger.EnabledEvent1;
                                    }
                                    else if (eventNumber == 2)
                                    {
                                        selectedTrigger.EnabledEvent2 = !selectedTrigger.EnabledEvent2;
                                    }
                                    else if (eventNumber == 3)
                                    {
                                        selectedTrigger.EnabledEvent3 = !selectedTrigger.EnabledEvent3;
                                    }
                                }
                            }
                            else if (tglDoOnceOnlyEvent.getImpact(x, y))
                            {
                                if (selectedTrigger != null)
                                {
                                    if (eventNumber == 1)
                                    {
                                        selectedTrigger.DoOnceOnlyEvent1 = !selectedTrigger.DoOnceOnlyEvent1;
                                    }
                                    else if (eventNumber == 2)
                                    {
                                        selectedTrigger.DoOnceOnlyEvent2 = !selectedTrigger.DoOnceOnlyEvent2;
                                    }
                                    else if (eventNumber == 3)
                                    {
                                        selectedTrigger.DoOnceOnlyEvent3 = !selectedTrigger.DoOnceOnlyEvent3;
                                    }
                                }
                            }
                            else if (tglEventType.getImpact(x, y))
                            {
                                changeEventType(eventNumber);
                            }
                            else if (tglEventFilenameOrTag.getImpact(x, y))
                            {
                                changeEventFilenameOrTag(eventNumber);
                            }
                            if (tglEventTransPointX.getImpact(x, y))
                            {
                                changeEventTransitionLocationX(eventNumber);
                            }
                            if (tglEventTransPointY.getImpact(x, y))
                            {
                                changeEventTransitionLocationY(eventNumber);
                            }
                            if (tglEventParm1.getImpact(x, y))
                            {
                                changeEventParm1(eventNumber);
                            }
                            if (tglEventParm2.getImpact(x, y))
                            {
                                changeEventParm2(eventNumber);
                            }
                            else if (tglEventParm3.getImpact(x, y))
                            {
                                changeEventParm3(eventNumber);
                            }
                            else if (tglEventParm4.getImpact(x, y))
                            {
                                changeEventParm4(eventNumber);
                            }

                        }
                    }
                    break;
            }
            return false;
        }
        public bool onTouchCrtPanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnCrtLeft.glowOn = false;
            btnCrtRight.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (touchDown)
                    {
                        //figure out if tapped on a map square
                        int gridX = (eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                        int gridY = eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler));
                        if ((tapInMapArea(gridX, gridY)) && (tapInMapViewport(x, y)))
                        {
                            if (currentCrt != null)
                            {
                                //TODO check if square is currently occupied by creature or PC
                                //place currently selected creature
                                CreatureRefs crtRef = new CreatureRefs();
                                crtRef.creatureResRef = currentCrt.cr_resref;
                                crtRef.creatureTag = currentCrt.cr_tag + "_" + gv.mod.getNextIdNumber();
                                crtRef.creatureStartLocationX = gridX;
                                crtRef.creatureStartLocationY = gridY;
                                gv.mod.currentEncounter.encounterCreatureRefsList.Add(crtRef);
                            }
                        }
                    }

                    if (btnCrtLeft.getImpact(x, y))
                    {
                        btnCrtLeft.glowOn = true;
                        return true;
                    }
                    else if (btnCrtRight.getImpact(x, y))
                    {
                        btnCrtRight.glowOn = true;
                        return true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnCrtLeft.glowOn = false;
                    btnCrtRight.glowOn = false;

                    //figure out if tapped on a creature in palette
                    if ((x > (panelLeftLocation)) && (x < (panelLeftLocation) + (4 * gv.squareSize * gv.scaler)))
                    {
                        if ((y > (panelTopLocation)) && (y < (panelTopLocation + ((crtSlotsPerPage / 4) * gv.squareSize * gv.scaler))))
                        {
                            //inside the palette area so determine which slot index tapped on
                            int xloc = (x - panelLeftLocation) / (gv.squareSize * gv.scaler);
                            int yloc = (y - panelTopLocation) / (gv.squareSize * gv.scaler);

                            if ((((xloc * (crtSlotsPerPage / 4)) + yloc) + (crtPageIndex) * crtSlotsPerPage) < gv.cc.allCreaturesList.Count)
                            {
                                crtSlotIndex = (xloc * (crtSlotsPerPage / 4)) + yloc;
                                currentCrt = gv.cc.allCreaturesList[((xloc * (crtSlotsPerPage / 4)) + yloc) + (crtPageIndex) * crtSlotsPerPage];
                            }
                        }
                    }

                    if (btnCrtLeft.getImpact(x, y))
                    {
                        if (crtPageIndex > 0)
                        {
                            crtPageIndex--;
                            btnCrtPageIndex.Text = (crtPageIndex + 1) + "";
                        }
                        return true;
                    }
                    else if (btnCrtRight.getImpact(x, y))
                    {
                        if (crtPageIndex <= (gv.cc.allCreaturesList.Count / crtSlotsPerPage) - 1)
                        {
                            crtPageIndex++;
                            btnCrtPageIndex.Text = (crtPageIndex + 1) + "";
                        }
                        return true;
                    }

                    break;
            }
            return false;
        }

        public bool squareUsedByOtherTrigger(int gridX, int gridY)
        {
            if (selectedTrigger == null) { return true; }
            foreach (Trigger trig in gv.mod.currentEncounter.Triggers)
            {
                if (trig == selectedTrigger) { continue; }
                foreach (Coordinate coor in trig.TriggerSquaresList)
                {
                    if ((coor.X == gridX) && (coor.Y == gridY))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool squareUsedByAnyTrigger(int gridX, int gridY)
        {
            foreach (Trigger trig in gv.mod.currentEncounter.Triggers)
            {
                foreach (Coordinate coor in trig.TriggerSquaresList)
                {
                    if ((coor.X == gridX) && (coor.Y == gridY))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool tapInMapArea(int gridX, int gridY)
        {
            if (gridX < 0) { return false; }
            if (gridY < 0) { return false; }
            if (gridX > gv.mod.currentEncounter.MapSizeX - 1) { return false; }
            if (gridY > gv.mod.currentEncounter.MapSizeY - 1) { return false; }
            return true;
        }
        public bool tapInMapViewport(int x, int y)
        {
            if (x < mapStartLocXinPixels) { return false; }
            if (y < 0) { return false; }
            if (x > mapStartLocXinPixels + gv.squareSize * gv.scaler * 10) { return false; }
            if (y > gv.squareSize * gv.scaler * 10) { return false; }
            return true;
        }
        public bool tapInMiniMapViewport(int x, int y)
        {
            int width = gv.mod.currentEncounter.MapSizeX * gv.squareSize / (mapSquareSizeScaler * 2) * gv.scaler;
            int height = gv.mod.currentEncounter.MapSizeY * gv.squareSize / (mapSquareSizeScaler * 2) * gv.scaler;
            int shiftY = panelTopLocation + gv.fontHeight + gv.fontLineSpacing;
            int shiftX = panelLeftLocation - (gv.squareSize / 1);
            if (x < shiftX) { return false; }
            if (y < shiftY) { return false; }
            if (x > shiftX + width) { return false; }
            if (y > shiftY + height) { return false; }
            return true;
        }

        public async void changeEncounterName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Change the Encounter Name:", gv.mod.currentEncounter.encounterName);
            gv.mod.currentEncounter.encounterName = myinput;
            gv.touchEnabled = true;
            /*using (TextInputDialog itSel = new TextInputDialog(gv, "Change the Encounter Name.", gv.mod.currentEncounter.encounterName))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (itSel.textInput.Length > 0)
                    {
                        gv.mod.currentEncounter.encounterName = itSel.textInput;
                    }
                    else
                    {
                        MessageBox.Show("Entering a blank name is not allowed");
                    }
                }
            }*/
        }
        public async void changeGoldDrop()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the amount of gold to drop at the end of the encounter.", gv.mod.currentEncounter.goldDrop);
            gv.mod.currentEncounter.goldDrop = myinput;
            gv.touchEnabled = true;
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Enter the amount of gold to drop at the end of the encounter.", gv.mod.currentEncounter.goldDrop))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.currentEncounter.goldDrop = itSel.numInput;
                }
            }*/
        }
        public async void changeTriggerTag()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose a unique (must be unique) tag for this trigger:", selectedTrigger.TriggerTag);
            selectedTrigger.TriggerTag = myinput;
            gv.touchEnabled = true;
        }
        public async void changeNumberOfScriptCallsRemaining()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Number of times this trigger will be triggered before disabling the trigger (combat trigger feature only):", selectedTrigger.numberOfScriptCallsRemaining);
            selectedTrigger.numberOfScriptCallsRemaining = myinput;
            gv.touchEnabled = true;
            /*if (selectedTrigger == null) { return; }
            using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Number of times this trigger will be triggered before disabling the trigger (combat trigger feature only)", selectedTrigger.numberOfScriptCallsRemaining))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    selectedTrigger.numberOfScriptCallsRemaining = itSel.numInput;
                }
            }*/
        }
        public async void changeEventType(int eventNumber)
        {
            if (selectedTrigger == null) { return; }
            List<string> types = new List<string>(); //container, transition, conversation, encounter, script
            types.Add("none");
            types.Add("conversation");
            types.Add("encounter");
            types.Add("transition");
            types.Add("container");
            types.Add("script");

            if (eventNumber == 1)
            {
                gv.touchEnabled = false;
                string selected = await gv.ListViewPage(types, "Select the type of event");
                selectedTrigger.Event1Type = selected;
                gv.touchEnabled = true;
                /*using (DropDownDialog itSel = new DropDownDialog(gv, "Select the type of event", types, selectedTrigger.Event1Type))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        selectedTrigger.Event1Type = itSel.selectedAreaName;
                    }
                }*/
            }
            else if (eventNumber == 2)
            {
                gv.touchEnabled = false;
                string selected = await gv.ListViewPage(types, "Select the type of event");
                selectedTrigger.Event2Type = selected;
                gv.touchEnabled = true;
                /*using (DropDownDialog itSel = new DropDownDialog(gv, "Select the type of event", types, selectedTrigger.Event2Type))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        selectedTrigger.Event2Type = itSel.selectedAreaName;
                    }
                }*/
            }
            else if (eventNumber == 3)
            {
                gv.touchEnabled = false;
                string selected = await gv.ListViewPage(types, "Select the type of event");
                selectedTrigger.Event3Type = selected;
                gv.touchEnabled = true;
                /*using (DropDownDialog itSel = new DropDownDialog(gv, "Select the type of event", types, selectedTrigger.Event3Type))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        selectedTrigger.Event3Type = itSel.selectedAreaName;
                    }
                }*/
            }
        }
        public async void changeEventFilenameOrTag(int eventNumber)
        {

            if (selectedTrigger == null) { return; }

            List<string> types = new List<string>(); //container, transition, conversation, encounter, script
            types.Add("none");

            if (eventNumber == 1)
            {
                if (selectedTrigger.Event1Type.Equals("container"))
                {
                    foreach (Container c in gv.mod.moduleContainersList)
                    {
                        types.Add(c.containerTag);
                    }
                }
                else if (selectedTrigger.Event1Type.Equals("transition"))
                {
                    foreach (Area a in gv.mod.moduleAreasObjects)
                    {
                        types.Add(a.Filename);
                    }
                }
                else if (selectedTrigger.Event1Type.Equals("conversation"))
                {
                    foreach (Convo c in gv.mod.moduleConvoList)
                    {
                        types.Add(c.ConvoFileName);
                    }
                }
                else if (selectedTrigger.Event1Type.Equals("encounter"))
                {
                    foreach (Encounter e in gv.mod.moduleEncountersList)
                    {
                        types.Add(e.encounterName);
                    }
                }
                else if (selectedTrigger.Event1Type.Equals("script"))
                {
                    //need a list of all scripts
                    foreach (ScriptObject s in gv.cc.scriptList)
                    {
                        types.Add(s.name);
                    }
                }
                gv.touchEnabled = false;
                string selected = await gv.ListViewPage(types, "Select an item from the list");
                selectedTrigger.Event1FilenameOrTag = selected;
                gv.touchEnabled = true;
                /*using (DropDownDialog itSel = new DropDownDialog(gv, "Select an item from the list", types, selectedTrigger.Event1FilenameOrTag))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        selectedTrigger.Event1FilenameOrTag = itSel.selectedAreaName;
                    }
                }*/
            }
            else if (eventNumber == 2)
            {
                if (selectedTrigger.Event2Type.Equals("container"))
                {
                    foreach (Container c in gv.mod.moduleContainersList)
                    {
                        types.Add(c.containerTag);
                    }
                }
                else if (selectedTrigger.Event2Type.Equals("transition"))
                {
                    foreach (Area a in gv.mod.moduleAreasObjects)
                    {
                        types.Add(a.Filename);
                    }
                }
                else if (selectedTrigger.Event2Type.Equals("conversation"))
                {
                    foreach (Convo c in gv.mod.moduleConvoList)
                    {
                        types.Add(c.ConvoFileName);
                    }
                }
                else if (selectedTrigger.Event2Type.Equals("encounter"))
                {
                    foreach (Encounter e in gv.mod.moduleEncountersList)
                    {
                        types.Add(e.encounterName);
                    }
                }
                else if (selectedTrigger.Event2Type.Equals("script"))
                {
                    //need a list of all scripts
                    foreach (ScriptObject s in gv.cc.scriptList)
                    {
                        types.Add(s.name);
                    }
                }
                gv.touchEnabled = false;
                string selected = await gv.ListViewPage(types, "Select an item from the list");
                selectedTrigger.Event2FilenameOrTag = selected;
                gv.touchEnabled = true;
                /*using (DropDownDialog itSel = new DropDownDialog(gv, "Select an item from the list", types, selectedTrigger.Event2FilenameOrTag))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        selectedTrigger.Event2FilenameOrTag = itSel.selectedAreaName;
                    }
                }*/
            }
            else if (eventNumber == 3)
            {
                if (selectedTrigger.Event3Type.Equals("container"))
                {
                    foreach (Container c in gv.mod.moduleContainersList)
                    {
                        types.Add(c.containerTag);
                    }
                }
                else if (selectedTrigger.Event3Type.Equals("transition"))
                {
                    foreach (Area a in gv.mod.moduleAreasObjects)
                    {
                        types.Add(a.Filename);
                    }
                }
                else if (selectedTrigger.Event3Type.Equals("conversation"))
                {
                    foreach (Convo c in gv.mod.moduleConvoList)
                    {
                        types.Add(c.ConvoFileName);
                    }
                }
                else if (selectedTrigger.Event3Type.Equals("encounter"))
                {
                    foreach (Encounter e in gv.mod.moduleEncountersList)
                    {
                        types.Add(e.encounterName);
                    }
                }
                else if (selectedTrigger.Event3Type.Equals("script"))
                {
                    //need a list of all scripts
                    foreach (ScriptObject s in gv.cc.scriptList)
                    {
                        types.Add(s.name);
                    }
                }
                gv.touchEnabled = false;
                string selected = await gv.ListViewPage(types, "Select an item from the list");
                selectedTrigger.Event3FilenameOrTag = selected;
                gv.touchEnabled = true;
                /*using (DropDownDialog itSel = new DropDownDialog(gv, "Select an item from the list", types, selectedTrigger.Event3FilenameOrTag))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        selectedTrigger.Event3FilenameOrTag = itSel.selectedAreaName;
                    }
                }*/
            }
        }
        public async void changeEventTransitionLocationX(int eventNumber)
        {

            if (selectedTrigger == null) { return; }
            if (eventNumber == 1)
            {
                if (!selectedTrigger.Event1Type.Equals("transition")) { return; }
                gv.touchEnabled = false;
                int myinput = await gv.NumInputBox("X location on map transitioning to (must be an integer):", selectedTrigger.Event1TransPointX);
                selectedTrigger.Event1TransPointX = myinput;
                gv.touchEnabled = true;
                /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "X location on map transitioning to", selectedTrigger.Event1TransPointX))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        selectedTrigger.Event1TransPointX = itSel.numInput;
                    }
                }*/
            }
            else if (eventNumber == 2)
            {
                if (!selectedTrigger.Event2Type.Equals("transition")) { return; }
                gv.touchEnabled = false;
                int myinput = await gv.NumInputBox("X location on map transitioning to (must be an integer):", selectedTrigger.Event2TransPointX);
                selectedTrigger.Event2TransPointX = myinput;
                gv.touchEnabled = true;
                /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "X location on map transitioning to", selectedTrigger.Event2TransPointX))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        selectedTrigger.Event2TransPointX = itSel.numInput;
                    }
                }*/
            }
            else if (eventNumber == 3)
            {
                if (!selectedTrigger.Event3Type.Equals("transition")) { return; }
                gv.touchEnabled = false;
                int myinput = await gv.NumInputBox("X location on map transitioning to (must be an integer):", selectedTrigger.Event3TransPointX);
                selectedTrigger.Event3TransPointX = myinput;
                gv.touchEnabled = true;
                /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "X location on map transitioning to", selectedTrigger.Event3TransPointX))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        selectedTrigger.Event3TransPointX = itSel.numInput;
                    }
                }*/
            }

        }
        public async void changeEventTransitionLocationY(int eventNumber)
        {

            if (selectedTrigger == null) { return; }
            if (eventNumber == 1)
            {
                if (!selectedTrigger.Event1Type.Equals("transition")) { return; }
                gv.touchEnabled = false;
                int myinput = await gv.NumInputBox("Y location on map transitioning to (must be an integer):", selectedTrigger.Event1TransPointY);
                selectedTrigger.Event1TransPointY = myinput;
                gv.touchEnabled = true;
            }
            else if (eventNumber == 2)
            {
                if (!selectedTrigger.Event2Type.Equals("transition")) { return; }
                gv.touchEnabled = false;
                int myinput = await gv.NumInputBox("Y location on map transitioning to (must be an integer):", selectedTrigger.Event2TransPointY);
                selectedTrigger.Event3TransPointY = myinput;
                gv.touchEnabled = true;
            }
            else if (eventNumber == 3)
            {
                if (!selectedTrigger.Event3Type.Equals("transition")) { return; }
                gv.touchEnabled = false;
                int myinput = await gv.NumInputBox("Y location on map transitioning to (must be an integer):", selectedTrigger.Event3TransPointY);
                selectedTrigger.Event3TransPointY = myinput;
                gv.touchEnabled = true;
            }

        }
        public async void changeEventParm1(int eventNumber)
        {

            if (selectedTrigger == null) { return; }
            if (eventNumber == 1)
            {
                if (!selectedTrigger.Event1Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event1Parm1);
                selectedTrigger.Event1Parm1 = myinput;
                gv.touchEnabled = true;
                /*using (TextInputDialog itSel = new TextInputDialog(gv, "Enter the first parameter for this script", selectedTrigger.Event1Parm1))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        if (itSel.textInput.Length > 0)
                        {
                            selectedTrigger.Event1Parm1 = itSel.textInput;
                        }
                        else
                        {
                            MessageBox.Show("Entering a blank text is not allowed...will use 'none' instead");
                        }
                    }
                }*/
            }
            else if (eventNumber == 2)
            {
                if (!selectedTrigger.Event2Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event2Parm1);
                selectedTrigger.Event2Parm1 = myinput;
                gv.touchEnabled = true;
                /*using (TextInputDialog itSel = new TextInputDialog(gv, "Enter the first parameter for this script", selectedTrigger.Event2Parm1))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        if (itSel.textInput.Length > 0)
                        {
                            selectedTrigger.Event2Parm1 = itSel.textInput;
                        }
                        else
                        {
                            MessageBox.Show("Entering a blank text is not allowed...will use 'none' instead");
                        }
                    }
                }*/
            }
            else if (eventNumber == 3)
            {
                if (!selectedTrigger.Event3Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event3Parm1);
                selectedTrigger.Event3Parm1 = myinput;
                gv.touchEnabled = true;
                /*using (TextInputDialog itSel = new TextInputDialog(gv, "Enter the first parameter for this script", selectedTrigger.Event3Parm1))
                {
                    var ret = itSel.ShowDialog();

                    if (ret == DialogResult.OK)
                    {
                        if (itSel.textInput.Length > 0)
                        {
                            selectedTrigger.Event3Parm1 = itSel.textInput;
                        }
                        else
                        {
                            MessageBox.Show("Entering a blank text is not allowed...will use 'none' instead");
                        }
                    }
                }*/
            }

        }
        public async void changeEventParm2(int eventNumber)
        {

            if (selectedTrigger == null) { return; }
            if (eventNumber == 1)
            {
                if (!selectedTrigger.Event1Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event1Parm2);
                selectedTrigger.Event1Parm2 = myinput;
                gv.touchEnabled = true;
            }
            else if (eventNumber == 2)
            {
                if (!selectedTrigger.Event2Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event2Parm2);
                selectedTrigger.Event2Parm2 = myinput;
                gv.touchEnabled = true;
            }
            else if (eventNumber == 3)
            {
                if (!selectedTrigger.Event3Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event3Parm2);
                selectedTrigger.Event3Parm2 = myinput;
                gv.touchEnabled = true;
            }

        }
        public async void changeEventParm3(int eventNumber)
        {

            if (selectedTrigger == null) { return; }
            if (eventNumber == 1)
            {
                if (!selectedTrigger.Event1Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event1Parm3);
                selectedTrigger.Event1Parm3 = myinput;
                gv.touchEnabled = true;
            }
            else if (eventNumber == 2)
            {
                if (!selectedTrigger.Event2Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event2Parm3);
                selectedTrigger.Event2Parm3 = myinput;
                gv.touchEnabled = true;
            }
            else if (eventNumber == 3)
            {
                if (!selectedTrigger.Event3Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event3Parm3);
                selectedTrigger.Event3Parm3 = myinput;
                gv.touchEnabled = true;
            }

        }
        public async void changeEventParm4(int eventNumber)
        {

            if (selectedTrigger == null) { return; }
            if (eventNumber == 1)
            {
                if (!selectedTrigger.Event1Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event1Parm4);
                selectedTrigger.Event1Parm4 = myinput;
                gv.touchEnabled = true;
            }
            else if (eventNumber == 2)
            {
                if (!selectedTrigger.Event2Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event2Parm4);
                selectedTrigger.Event2Parm4 = myinput;
                gv.touchEnabled = true;
            }
            else if (eventNumber == 3)
            {
                if (!selectedTrigger.Event3Type.Equals("script")) { return; }
                gv.touchEnabled = false;
                string myinput = await gv.StringInputBox("Enter the parameter for this script:", selectedTrigger.Event3Parm4);
                selectedTrigger.Event3Parm4 = myinput;
                gv.touchEnabled = true;
            }
        }
        public async void addItemToLootList()
        {
            List<string> items = new List<string>();
            items.Add("none");
            //gv.mod.currentEncounter.encounterInventoryRefsList.Clear();
            foreach (Item it in gv.cc.allItemsList)
            {
                items.Add(it.name);
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an item from the list to add to loot items:");
            if (selected != "none")
            {
                ItemRefs ir = gv.mod.createItemRefsFromItem(gv.cc.getItemByName(selected));
                gv.mod.currentEncounter.encounterInventoryRefsList.Add(ir);
            }
            gv.touchEnabled = true;
        }
        public async void removeItemFromLootList()
        {
            List<string> items = new List<string>();
            items.Add("none");

            foreach (ItemRefs it in gv.mod.currentEncounter.encounterInventoryRefsList)
            {
                items.Add(it.name);
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an item from the loot list to remove:");
            if (selected != "none")
            {
                int indx = 0;
                foreach (string s in items)
                {
                    if (s.Equals(selected))
                    {
                        gv.mod.currentEncounter.encounterInventoryRefsList.RemoveAt(indx - 1);
                    }
                    indx++;
                }
            }
            gv.touchEnabled = true;            
        }
    }
}
