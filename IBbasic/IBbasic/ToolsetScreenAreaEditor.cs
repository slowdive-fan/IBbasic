﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    public class ToolsetScreenAreaEditor
    {
        public GameView gv;
        public int mapSquareSizeScaler = 1;
        public int upperSquareX = 0;
        public int upperSquareY = 0;
        public int panelTopLocation = 0;
        public int panelLeftLocation = 0;
        public IbRect src = null;
        public IbRect dst = null;
        private IbbButton btnInfo = null;
        private IbbButton btnTiles = null;
        private IbbButton btnTriggers = null;
        private IbbButton btnWalkLoS = null;
        private IbbButton btn3DPreview = null;
        private IbbButton btnSettings = null;
        private IbbButton btnHelp = null;
        public bool touchDown = false;
        
        //Place Tiles Panel
        private IbbButton btnTilesLeft = null;
        private IbbButton btnTilesRight = null;
        private IbbButton btnTilesPageIndex = null;
        public IbbToggle tglMiscTiles = null;
        public IbbToggle tglWallFloorTiles = null;
        public IbbToggle tglPropTiles = null;
        public IbbToggle tglUserTiles = null;
        public IbbToggle rbtnShowLayer1 = null;
        public IbbToggle rbtnShowLayer2 = null;
        public IbbToggle rbtnShowLayer3 = null;
        public IbbToggle rbtnEditLayer1 = null;
        public IbbToggle rbtnEditLayer2 = null;
        public IbbToggle rbtnEditLayer3 = null;
        public string currentTile = "none";
        private int tilesPageIndex = 0;
        private int tileSlotIndex = 0;
        private int tileSlotsPerPage = 20;

        //Area Settings Panel
        public IbbToggle tglSettingAreaName = null;
        public IbbToggle tglSettingInGameAreaName = null;
        public IbbToggle tglAreaMusic = null;
        public IbbToggle tglSettingAreaVisibleDistance = null;
        public IbbToggle tglSettingTimePerSquare = null;
        public IbbToggle tglSettingIs3dArea = null;
        public IbbToggle tglSettingRestingAllowed = null;
        public IbbToggle tglSettingUseMiniMapFogOfWar = null;
        public IbbToggle tglSettingUseDayNightCycle = null;

        //Info Panel
        public Coordinate selectedSquare = new Coordinate();
        public IbbToggle rbtnZoom1 = null;
        public IbbToggle rbtnZoom2 = null;

        //Walkable-LoS Panel
        public IbbToggle rbtnWalkBlocking = null;
        public IbbToggle rbtnWalkOpen = null;
        public IbbToggle rbtnSightBlocking = null;
        public IbbToggle rbtnSightOpen = null;
        public IbbToggle btnPlusLeft = null;
        public IbbToggle btnMinusLeft = null;
        public IbbToggle btnPlusRight = null;
        public IbbToggle btnMinusRight = null;
        public IbbToggle btnPlusTop = null;
        public IbbToggle btnMinusTop = null;
        public IbbToggle btnPlusBottom = null;
        public IbbToggle btnMinusBottom = null;

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
        public IbbToggle tglHideAfterEnc = null;
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

        //3D Preview Panel
        public IbbButton btnMoveForward = null;
        public IbbButton btnTurnLeft = null;
        public IbbButton btnTurnRight = null;        

        private IBminiTextBox description;

        public string currentMode = "Info"; //Info, Tiles, Triggers, WalkLoS, 3DPreview, Settings
        public int mapStartLocXinPixels;
        public int map3DViewStartLocXinPixels;

        List<string> tilesWallFloorList = new List<string>();
        List<string> tilesPropList = new List<string>();
        List<string> tilesMiscList = new List<string>();
        List<string> tilesUserList = new List<string>();
        List<string> wallsets = new List<string>();
        List<string> backdrops = new List<string>();
        List<string> overlays = new List<string>();

        //3D PREVIEW MODE
        public int PlayerFacingDirection = 0; //0 = north, 1 = east, 2 = south, 3 = west
        public int PlayerLocationX = 0;
        public int PlayerLocationY = 0;
        public int movementDelayInMiliseconds = 100;
        private long timeStamp = 0;

        public ToolsetScreenAreaEditor(GameView g)
        {
            gv = g;
            mapStartLocXinPixels = 1 * gv.uiSquareSize;
            setControlsStart();
            setupTilesPanelControls();
            setupInfoPanelControls();
            setupSettingsPanelControls();
            setupWalkLoSPanelControls();
            setupTriggerPanelControls();
            setup3DPreviewControls();
            rbtnZoom1.toggleOn = true;
            rbtnZoom2.toggleOn = false;
            tglMiscTiles.toggleOn = true;
            tglWallFloorTiles.toggleOn = false;
            tglPropTiles.toggleOn = false;
            tglUserTiles.toggleOn = false;
            rbtnEditLayer1.toggleOn = true;
            rbtnEditLayer2.toggleOn = false;
            rbtnEditLayer3.toggleOn = false;
            rbtnShowLayer1.toggleOn = true;
            rbtnShowLayer2.toggleOn = true;
            rbtnShowLayer3.toggleOn = true;
            rbtnWalkOpen.toggleOn = true;
            rbtnViewInfoTrigger.toggleOn = true;
            rbtnTriggerProperties.toggleOn = true;
            tglSettingIs3dArea.toggleOn = gv.mod.currentArea.Is3dArea;
            tglSettingRestingAllowed.toggleOn = gv.mod.currentArea.RestingAllowed;
            tglSettingUseMiniMapFogOfWar.toggleOn = gv.mod.currentArea.UseMiniMapFogOfWar;
            description = new IBminiTextBox(gv);
            description.tbXloc = 0 * gv.squareSize;
            description.tbYloc = 3 * gv.squareSize;
            description.tbWidth = 6 * gv.squareSize;
            description.tbHeight = 6 * gv.squareSize;
            description.showBoxBorder = false;

            //from user foldeer first
            List<string> files = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            foreach (string f in files)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                if (filenameNoExt.StartsWith("t_"))
                {
                    tilesUserList.Add(filenameNoExt);
                }
                else if (filenameNoExt.StartsWith("w_"))
                {
                    wallsets.Add(filenameNoExt);
                }
                else if (filenameNoExt.StartsWith("bd_"))
                {
                    backdrops.Add(filenameNoExt);
                }
                else if (filenameNoExt.StartsWith("o_"))
                {
                    overlays.Add(filenameNoExt);
                }
            }
            //from engine assets
            List<string> files2 = gv.GetAllFilesWithExtensionFromBothFolders("\\tiles", "none", ".png");
            foreach (string f in files2)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                if ((filenameNoExt.StartsWith("t_f_")) || (filenameNoExt.StartsWith("t_fc_")) || (filenameNoExt.StartsWith("t_w_")))
                {
                    tilesWallFloorList.Add(filenameNoExt);
                }
                else if ((filenameNoExt.StartsWith("t_m_")) || (filenameNoExt.StartsWith("t_n_")))
                {
                    tilesPropList.Add(filenameNoExt);
                }
                else if (filenameNoExt.StartsWith("t_"))
                {
                    tilesMiscList.Add(filenameNoExt);
                }
                else if (filenameNoExt.StartsWith("w_"))
                {
                    wallsets.Add(filenameNoExt);
                }
                else if (filenameNoExt.StartsWith("bd_"))
                {
                    backdrops.Add(filenameNoExt);
                }
                else if (filenameNoExt.StartsWith("o_"))
                {
                    overlays.Add(filenameNoExt);
                }
            }
        }

        public void resetAllPreview()
        {
            mapSquareSizeScaler = 1;
            upperSquareX = 0;
            upperSquareY = 0;
            selectedSquare.X = 0;
            selectedSquare.Y = 0;
            rbtnZoom1.toggleOn = true;
            rbtnZoom2.toggleOn = false;
        }

        public void setControlsStart()
        {
            if (btnInfo == null)
            {
                btnInfo = new IbbButton(gv, 1.0f);
            }
            btnInfo.Img = "btn_small";
            btnInfo.Img2 = "btninfo";
            btnInfo.Glow = "btn_small_glow";
            //btnInfo.Text = "INFO";
            btnInfo.X = 0 * gv.uiSquareSize;
            btnInfo.Y = (int)(0 * gv.uiSquareSize + gv.scaler);
            btnInfo.Height = (int)(gv.ibbheight * gv.scaler);
            btnInfo.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnTiles == null)
            {
                btnTiles = new IbbButton(gv, 1.0f);
            }
            btnTiles.Img = "btn_small";
            btnTiles.Img2 = "btntiles";
            btnTiles.Glow = "btn_small_glow";
            //btnTiles.Text = "TILES";
            btnTiles.X = 0 * gv.uiSquareSize;
            btnTiles.Y = (int)(1 * gv.uiSquareSize + gv.scaler);
            btnTiles.Height = (int)(gv.ibbheight * gv.scaler);
            btnTiles.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnTriggers == null)
            {
                btnTriggers = new IbbButton(gv, 0.8f);
            }
            btnTriggers.Img = "btn_small";
            btnTriggers.Img2 = "btntriggers";
            btnTriggers.Glow = "btn_small_glow";
            //btnTriggers.Text = "TRIGR";
            btnTriggers.X = 0 * gv.uiSquareSize;
            btnTriggers.Y = (int)(2 * gv.uiSquareSize + gv.scaler);
            btnTriggers.Height = (int)(gv.ibbheight * gv.scaler);
            btnTriggers.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnWalkLoS == null)
            {
                btnWalkLoS = new IbbButton(gv, 1.0f);
            }
            btnWalkLoS.Img2 = "btnwalklos";
            //btnWalkLoS.Text = "WLKLOS";
            btnWalkLoS.Img = "btn_small";
            btnWalkLoS.Glow = "btn_small_glow";
            btnWalkLoS.X = 0 * gv.uiSquareSize;
            btnWalkLoS.Y = (int)(3 * gv.uiSquareSize + gv.scaler);
            btnWalkLoS.Height = (int)(gv.ibbheight * gv.scaler);
            btnWalkLoS.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btn3DPreview == null)
            {
                btn3DPreview = new IbbButton(gv, 1.0f);
            }
            //btn3DPreview.Text = "3DTest";
            btn3DPreview.Img = "btn_small";
            btn3DPreview.Img2 = "btn3dtest";
            btn3DPreview.Glow = "btn_small_glow";
            btn3DPreview.X = 0 * gv.uiSquareSize;
            btn3DPreview.Y = (int)(4 * gv.uiSquareSize + gv.scaler);
            btn3DPreview.Height = (int)(gv.ibbheight * gv.scaler);
            btn3DPreview.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnSettings == null)
            {
                btnSettings = new IbbButton(gv, 1.0f);
            }
            //btnSettings.Text = "SETTING";
            btnSettings.Img2 = "btnsettings2";
            btnSettings.Img = "btn_small";
            btnSettings.Glow = "btn_small_glow";
            btnSettings.X = 0 * gv.uiSquareSize;
            btnSettings.Y = (int)(5 * gv.uiSquareSize + gv.scaler);
            btnSettings.Height = (int)(gv.ibbheight * gv.scaler);
            btnSettings.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
            }
            //btnHelp.Text = "HELP";
            btnHelp.Img2 = "btnhelp";
            btnHelp.Img = "btn_small";
            btnHelp.Glow = "btn_small_glow";
            btnHelp.X = 10 * gv.uiSquareSize;
            btnHelp.Y = (int)(6 * gv.uiSquareSize + gv.scaler);
            btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
            btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        public void setupTilesPanelControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (int)((2 * gv.scaler));
            panelTopLocation = 0;

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

            if (tglMiscTiles == null)
            {
                tglMiscTiles = new IbbToggle(gv);
                tglMiscTiles.toggleOn = true;
            }
            tglMiscTiles.ImgOn = "tgl_misc_on";
            tglMiscTiles.ImgOff = "tgl_misc_off";
            tglMiscTiles.X = panelLeftLocation + (int)(0 * gv.squareSize * gv.scaler);
            tglMiscTiles.Y = panelTopLocation + (int)(5 * gv.squareSize * gv.scaler);
            tglMiscTiles.Height = (int)(gv.squareSize * gv.scaler);
            tglMiscTiles.Width = (int)(gv.squareSize * gv.scaler);

            if (tglWallFloorTiles == null)
            {
                tglWallFloorTiles = new IbbToggle(gv);
                tglWallFloorTiles.toggleOn = true;
            }
            tglWallFloorTiles.ImgOn = "tgl_wallfloor_on";
            tglWallFloorTiles.ImgOff = "tgl_wallfloor_off";
            tglWallFloorTiles.X = panelLeftLocation + (int)(1 * gv.squareSize * gv.scaler);
            tglWallFloorTiles.Y = panelTopLocation + (int)(5 * gv.squareSize * gv.scaler);
            tglWallFloorTiles.Height = (int)(gv.squareSize * gv.scaler);
            tglWallFloorTiles.Width = (int)(gv.squareSize * gv.scaler);

            if (tglPropTiles == null)
            {
                tglPropTiles = new IbbToggle(gv);
                tglPropTiles.toggleOn = true;
            }
            tglPropTiles.ImgOn = "tgl_prop_on";
            tglPropTiles.ImgOff = "tgl_prop_off";
            tglPropTiles.X = panelLeftLocation + (int)(2 * gv.squareSize * gv.scaler);
            tglPropTiles.Y = panelTopLocation + (int)(5 * gv.squareSize * gv.scaler);
            tglPropTiles.Height = (int)(gv.squareSize * gv.scaler);
            tglPropTiles.Width = (int)(gv.squareSize * gv.scaler);

            if (tglUserTiles == null)
            {
                tglUserTiles = new IbbToggle(gv);
                tglUserTiles.toggleOn = true;
            }
            tglUserTiles.ImgOn = "tgl_user_on";
            tglUserTiles.ImgOff = "tgl_user_off";
            tglUserTiles.X = panelLeftLocation + (int)(3 * gv.squareSize * gv.scaler);
            tglUserTiles.Y = panelTopLocation + (int)(5 * gv.squareSize * gv.scaler);
            tglUserTiles.Height = (int)(gv.squareSize * gv.scaler);
            tglUserTiles.Width = (int)(gv.squareSize * gv.scaler);

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
        public void setupInfoPanelControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (int)((2 * gv.scaler));
            panelTopLocation = 0;

            //INFO PANEL            
            if (rbtnZoom1 == null)
            {
                rbtnZoom1 = new IbbToggle(gv);
            }
            rbtnZoom1.ImgOn = "mtgl_rbtn_on";
            rbtnZoom1.ImgOff = "mtgl_rbtn_off";
            rbtnZoom1.X = panelLeftLocation;
            rbtnZoom1.Y = panelTopLocation + (17 * (gv.fontHeight + gv.fontLineSpacing));
            rbtnZoom1.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnZoom1.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnZoom2 == null)
            {
                rbtnZoom2 = new IbbToggle(gv);
            }
            rbtnZoom2.ImgOn = "mtgl_rbtn_on";
            rbtnZoom2.ImgOff = "mtgl_rbtn_off";
            rbtnZoom2.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            rbtnZoom2.Y = panelTopLocation + (17 * (gv.fontHeight + gv.fontLineSpacing));
            rbtnZoom2.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnZoom2.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }
        public void setupSettingsPanelControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (int)((2 * gv.scaler));
            panelTopLocation = 0;

            //TILES PANEL
            if (tglSettingAreaName == null)
            {
                tglSettingAreaName = new IbbToggle(gv);
            }
            tglSettingAreaName.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            tglSettingAreaName.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            tglSettingAreaName.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglSettingAreaName.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 2);
            tglSettingAreaName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingAreaName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingInGameAreaName == null)
            {
                tglSettingInGameAreaName = new IbbToggle(gv);
            }
            tglSettingInGameAreaName.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            tglSettingInGameAreaName.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            tglSettingInGameAreaName.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglSettingInGameAreaName.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 4);
            tglSettingInGameAreaName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingInGameAreaName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglAreaMusic == null)
            {
                tglAreaMusic = new IbbToggle(gv);
            }
            tglAreaMusic.ImgOn = "mtgl_edit_btn";
            tglAreaMusic.ImgOff = "mtgl_edit_btn";
            tglAreaMusic.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglAreaMusic.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 6);
            tglAreaMusic.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglAreaMusic.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingAreaVisibleDistance == null)
            {
                tglSettingAreaVisibleDistance = new IbbToggle(gv);
            }
            tglSettingAreaVisibleDistance.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            tglSettingAreaVisibleDistance.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            tglSettingAreaVisibleDistance.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglSettingAreaVisibleDistance.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 8);
            tglSettingAreaVisibleDistance.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingAreaVisibleDistance.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingTimePerSquare == null)
            {
                tglSettingTimePerSquare = new IbbToggle(gv);
            }
            tglSettingTimePerSquare.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            tglSettingTimePerSquare.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            tglSettingTimePerSquare.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglSettingTimePerSquare.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 10);
            tglSettingTimePerSquare.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingTimePerSquare.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingIs3dArea == null)
            {
                tglSettingIs3dArea = new IbbToggle(gv);
            }
            tglSettingIs3dArea.ImgOn = "mtgl_rbtn_on";
            tglSettingIs3dArea.ImgOff = "mtgl_rbtn_off";
            tglSettingIs3dArea.X = panelLeftLocation;
            tglSettingIs3dArea.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 12);
            tglSettingIs3dArea.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingIs3dArea.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingRestingAllowed == null)
            {
                tglSettingRestingAllowed = new IbbToggle(gv);
            }
            tglSettingRestingAllowed.ImgOn = "mtgl_rbtn_on";
            tglSettingRestingAllowed.ImgOff = "mtgl_rbtn_off";
            tglSettingRestingAllowed.X = panelLeftLocation;
            tglSettingRestingAllowed.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 14);
            tglSettingRestingAllowed.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingRestingAllowed.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingUseMiniMapFogOfWar == null)
            {
                tglSettingUseMiniMapFogOfWar = new IbbToggle(gv);
            }
            tglSettingUseMiniMapFogOfWar.ImgOn = "mtgl_rbtn_on";
            tglSettingUseMiniMapFogOfWar.ImgOff = "mtgl_rbtn_off";
            tglSettingUseMiniMapFogOfWar.X = panelLeftLocation;
            tglSettingUseMiniMapFogOfWar.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 16);
            tglSettingUseMiniMapFogOfWar.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingUseMiniMapFogOfWar.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingUseDayNightCycle == null)
            {
                tglSettingUseDayNightCycle = new IbbToggle(gv);
            }
            tglSettingUseDayNightCycle.ImgOn = "mtgl_rbtn_on";
            tglSettingUseDayNightCycle.ImgOff = "mtgl_rbtn_off";
            tglSettingUseDayNightCycle.X = panelLeftLocation;
            tglSettingUseDayNightCycle.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 18);
            tglSettingUseDayNightCycle.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingUseDayNightCycle.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setupWalkLoSPanelControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (int)((2 * gv.scaler));
            panelTopLocation = 0;

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

            if (btnPlusLeft == null)
            {
                btnPlusLeft = new IbbToggle(gv);
            }
            btnPlusLeft.ImgOn = "mtgl_expand_on";
            btnPlusLeft.ImgOff = "mtgl_expand_on";
            btnPlusLeft.X = panelLeftLocation;
            btnPlusLeft.Y = panelTopLocation + (4 * gv.uiSquareSize) + (1 * gv.uiSquareSize / 2);
            btnPlusLeft.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPlusLeft.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnMinusLeft == null)
            {
                btnMinusLeft = new IbbToggle(gv);
            }
            btnMinusLeft.ImgOn = "mtgl_expand_off";
            btnMinusLeft.ImgOff = "mtgl_expand_off";
            btnMinusLeft.X = panelLeftLocation;
            btnMinusLeft.Y = panelTopLocation + (4 * gv.uiSquareSize) + (2 * gv.uiSquareSize / 2);
            btnMinusLeft.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnMinusLeft.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPlusRight == null)
            {
                btnPlusRight = new IbbToggle(gv);
            }
            btnPlusRight.ImgOn = "mtgl_expand_on";
            btnPlusRight.ImgOff = "mtgl_expand_on";
            btnPlusRight.X = panelLeftLocation + (1 * gv.uiSquareSize) + (1 * gv.uiSquareSize / 2);
            btnPlusRight.Y = panelTopLocation + (4 * gv.uiSquareSize) + (1 * gv.uiSquareSize / 2);
            btnPlusRight.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPlusRight.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnMinusRight == null)
            {
                btnMinusRight = new IbbToggle(gv);
            }
            btnMinusRight.ImgOn = "mtgl_expand_off";
            btnMinusRight.ImgOff = "mtgl_expand_off";
            btnMinusRight.X = panelLeftLocation + (1 * gv.uiSquareSize) + (1 * gv.uiSquareSize / 2);
            btnMinusRight.Y = panelTopLocation + (4 * gv.uiSquareSize) + (2 * gv.uiSquareSize / 2);
            btnMinusRight.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnMinusRight.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPlusTop == null)
            {
                btnPlusTop = new IbbToggle(gv);
            }
            btnPlusTop.ImgOn = "mtgl_expand_on";
            btnPlusTop.ImgOff = "mtgl_expand_on";
            btnPlusTop.X = panelLeftLocation + (0 * gv.uiSquareSize) + (1 * gv.uiSquareSize / 2);
            btnPlusTop.Y = panelTopLocation + (4 * gv.uiSquareSize) + (0 * gv.uiSquareSize / 2);
            btnPlusTop.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPlusTop.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnMinusTop == null)
            {
                btnMinusTop = new IbbToggle(gv);
            }
            btnMinusTop.ImgOn = "mtgl_expand_off";
            btnMinusTop.ImgOff = "mtgl_expand_off";
            btnMinusTop.X = panelLeftLocation + (1 * gv.uiSquareSize) + (0 * gv.uiSquareSize / 2);
            btnMinusTop.Y = panelTopLocation + (4 * gv.uiSquareSize) + (0 * gv.uiSquareSize / 2);
            btnMinusTop.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnMinusTop.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPlusBottom == null)
            {
                btnPlusBottom = new IbbToggle(gv);
            }
            btnPlusBottom.ImgOn = "mtgl_expand_on";
            btnPlusBottom.ImgOff = "mtgl_expand_on";
            btnPlusBottom.X = panelLeftLocation + (0 * gv.uiSquareSize) + (1 * gv.uiSquareSize / 2);
            btnPlusBottom.Y = panelTopLocation + (5 * gv.uiSquareSize) + (1 * gv.uiSquareSize / 2);
            btnPlusBottom.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPlusBottom.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnMinusBottom == null)
            {
                btnMinusBottom = new IbbToggle(gv);
            }
            btnMinusBottom.ImgOn = "mtgl_expand_off";
            btnMinusBottom.ImgOff = "mtgl_expand_off";
            btnMinusBottom.X = panelLeftLocation + (1 * gv.uiSquareSize) + (0 * gv.uiSquareSize / 2);
            btnMinusBottom.Y = panelTopLocation + (5 * gv.uiSquareSize) + (1 * gv.uiSquareSize / 2);
            btnMinusBottom.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnMinusBottom.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }
        public void setupTriggerPanelControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (int)((2 * gv.scaler));
            panelTopLocation = 0;

            //Trigger Panel           
            if (rbtnViewInfoTrigger == null)
            {
                rbtnViewInfoTrigger = new IbbToggle(gv);
            }
            rbtnViewInfoTrigger.ImgOn = "mtgl_rbtn_on";
            rbtnViewInfoTrigger.ImgOff = "mtgl_rbtn_off";
            rbtnViewInfoTrigger.X = panelLeftLocation;
            rbtnViewInfoTrigger.Y = panelTopLocation + 0 * gv.uiSquareSize + ((gv.fontHeight + gv.fontLineSpacing) * 1);
            rbtnViewInfoTrigger.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnViewInfoTrigger.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnPlaceNewTrigger == null)
            {
                rbtnPlaceNewTrigger = new IbbToggle(gv);
            }
            rbtnPlaceNewTrigger.ImgOn = "mtgl_rbtn_on";
            rbtnPlaceNewTrigger.ImgOff = "mtgl_rbtn_off";
            rbtnPlaceNewTrigger.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            rbtnPlaceNewTrigger.Y = panelTopLocation + 0 * gv.uiSquareSize + ((gv.fontHeight + gv.fontLineSpacing) * 1);
            rbtnPlaceNewTrigger.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnPlaceNewTrigger.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEditTrigger == null)
            {
                rbtnEditTrigger = new IbbToggle(gv);
            }
            rbtnEditTrigger.ImgOn = "mtgl_rbtn_on";
            rbtnEditTrigger.ImgOff = "mtgl_rbtn_off";
            rbtnEditTrigger.X = panelLeftLocation;
            rbtnEditTrigger.Y = panelTopLocation + 0 * gv.uiSquareSize + ((gv.fontHeight + gv.fontLineSpacing) * 3);
            rbtnEditTrigger.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnEditTrigger.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnTriggerProperties == null)
            {
                rbtnTriggerProperties = new IbbToggle(gv);
            }
            rbtnTriggerProperties.ImgOn = "mtgl_rbtn_on";
            rbtnTriggerProperties.ImgOff = "mtgl_rbtn_off";
            rbtnTriggerProperties.X = panelLeftLocation;
            rbtnTriggerProperties.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 6);
            rbtnTriggerProperties.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnTriggerProperties.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEvent1Properties == null)
            {
                rbtnEvent1Properties = new IbbToggle(gv);
            }
            rbtnEvent1Properties.ImgOn = "mtgl_rbtn_on";
            rbtnEvent1Properties.ImgOff = "mtgl_rbtn_off";
            rbtnEvent1Properties.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            rbtnEvent1Properties.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 6);
            rbtnEvent1Properties.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnEvent1Properties.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEvent2Properties == null)
            {
                rbtnEvent2Properties = new IbbToggle(gv);
            }
            rbtnEvent2Properties.ImgOn = "mtgl_rbtn_on";
            rbtnEvent2Properties.ImgOff = "mtgl_rbtn_off";
            rbtnEvent2Properties.X = panelLeftLocation;
            rbtnEvent2Properties.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 8);
            rbtnEvent2Properties.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            rbtnEvent2Properties.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (rbtnEvent3Properties == null)
            {
                rbtnEvent3Properties = new IbbToggle(gv);
            }
            rbtnEvent3Properties.ImgOn = "mtgl_rbtn_on";
            rbtnEvent3Properties.ImgOff = "mtgl_rbtn_off";
            rbtnEvent3Properties.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            rbtnEvent3Properties.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 8);
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
            tglTriggerTag.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 10);
            tglTriggerTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglTriggerTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglTriggerEnabled == null)
            {
                tglTriggerEnabled = new IbbToggle(gv);
            }
            tglTriggerEnabled.ImgOn = "mtgl_rbtn_on";
            tglTriggerEnabled.ImgOff = "mtgl_rbtn_off";
            tglTriggerEnabled.X = panelLeftLocation;
            tglTriggerEnabled.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 12);
            tglTriggerEnabled.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglTriggerEnabled.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglTriggerDoOnce == null)
            {
                tglTriggerDoOnce = new IbbToggle(gv);
            }
            tglTriggerDoOnce.ImgOn = "mtgl_rbtn_on";
            tglTriggerDoOnce.ImgOff = "mtgl_rbtn_off";
            tglTriggerDoOnce.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            tglTriggerDoOnce.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 12);
            tglTriggerDoOnce.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglTriggerDoOnce.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglImageFilename == null)
            {
                tglImageFilename = new IbbToggle(gv);
            }
            tglImageFilename.ImgOn = "mtgl_edit_btn";
            tglImageFilename.ImgOff = "mtgl_edit_btn";
            tglImageFilename.X = panelLeftLocation;
            tglImageFilename.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 14);
            tglImageFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglImageFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglImageFacingLeft == null)
            {
                tglImageFacingLeft = new IbbToggle(gv);
            }
            tglImageFacingLeft.ImgOn = "mtgl_rbtn_on";
            tglImageFacingLeft.ImgOff = "mtgl_rbtn_off";
            tglImageFacingLeft.X = panelLeftLocation;
            tglImageFacingLeft.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 18);
            tglImageFacingLeft.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglImageFacingLeft.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglIsShown == null)
            {
                tglIsShown = new IbbToggle(gv);
            }
            tglIsShown.ImgOn = "mtgl_rbtn_on";
            tglIsShown.ImgOff = "mtgl_rbtn_off";
            tglIsShown.X = panelLeftLocation + (1 * gv.uiSquareSize + (gv.uiSquareSize / 2));
            tglIsShown.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 18);
            tglIsShown.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglIsShown.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglHideAfterEnc == null)
            {
                tglHideAfterEnc = new IbbToggle(gv);
            }
            tglHideAfterEnc.ImgOn = "mtgl_rbtn_on";
            tglHideAfterEnc.ImgOff = "mtgl_rbtn_off";
            tglHideAfterEnc.X = panelLeftLocation;
            tglHideAfterEnc.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 16);
            tglHideAfterEnc.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglHideAfterEnc.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglNumberOfScriptCallsRemaining == null)
            {
                tglNumberOfScriptCallsRemaining = new IbbToggle(gv);
            }
            tglNumberOfScriptCallsRemaining.ImgOn = "mtgl_edit_btn";
            tglNumberOfScriptCallsRemaining.ImgOff = "mtgl_edit_btn";
            tglNumberOfScriptCallsRemaining.X = panelLeftLocation;
            tglNumberOfScriptCallsRemaining.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 21);
            tglNumberOfScriptCallsRemaining.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglNumberOfScriptCallsRemaining.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCanBeTriggeredByPc == null)
            {
                tglCanBeTriggeredByPc = new IbbToggle(gv);
            }
            tglCanBeTriggeredByPc.ImgOn = "mtgl_rbtn_on";
            tglCanBeTriggeredByPc.ImgOff = "mtgl_rbtn_off";
            tglCanBeTriggeredByPc.X = panelLeftLocation;
            tglCanBeTriggeredByPc.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 23);
            tglCanBeTriggeredByPc.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCanBeTriggeredByPc.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCanBeTriggeredByCreature == null)
            {
                tglCanBeTriggeredByCreature = new IbbToggle(gv);
            }
            tglCanBeTriggeredByCreature.ImgOn = "mtgl_rbtn_on";
            tglCanBeTriggeredByCreature.ImgOff = "mtgl_rbtn_off";
            tglCanBeTriggeredByCreature.X = panelLeftLocation;
            tglCanBeTriggeredByCreature.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 25);
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
            tglEnabledEvent.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 10);
            tglEnabledEvent.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEnabledEvent.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglDoOnceOnlyEvent == null)
            {
                tglDoOnceOnlyEvent = new IbbToggle(gv);
            }
            tglDoOnceOnlyEvent.ImgOn = "mtgl_rbtn_on";
            tglDoOnceOnlyEvent.ImgOff = "mtgl_rbtn_off";
            tglDoOnceOnlyEvent.X = panelLeftLocation;
            tglDoOnceOnlyEvent.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 12);
            tglDoOnceOnlyEvent.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglDoOnceOnlyEvent.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventType == null)
            {
                tglEventType = new IbbToggle(gv);
            }
            tglEventType.ImgOn = "mtgl_edit_btn";
            tglEventType.ImgOff = "mtgl_edit_btn";
            tglEventType.X = panelLeftLocation;
            tglEventType.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 14);
            tglEventType.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventType.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventFilenameOrTag == null)
            {
                tglEventFilenameOrTag = new IbbToggle(gv);
            }
            tglEventFilenameOrTag.ImgOn = "mtgl_edit_btn";
            tglEventFilenameOrTag.ImgOff = "mtgl_edit_btn";
            tglEventFilenameOrTag.X = panelLeftLocation;
            tglEventFilenameOrTag.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 16);
            tglEventFilenameOrTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventFilenameOrTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventTransPointX == null)
            {
                tglEventTransPointX = new IbbToggle(gv);
            }
            tglEventTransPointX.ImgOn = "mtgl_edit_btn";
            tglEventTransPointX.ImgOff = "mtgl_edit_btn";
            tglEventTransPointX.X = panelLeftLocation;
            tglEventTransPointX.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 18);
            tglEventTransPointX.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventTransPointX.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventTransPointY == null)
            {
                tglEventTransPointY = new IbbToggle(gv);
            }
            tglEventTransPointY.ImgOn = "mtgl_edit_btn";
            tglEventTransPointY.ImgOff = "mtgl_edit_btn";
            tglEventTransPointY.X = panelLeftLocation;
            tglEventTransPointY.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 20);
            tglEventTransPointY.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventTransPointY.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventParm1 == null)
            {
                tglEventParm1 = new IbbToggle(gv);
            }
            tglEventParm1.ImgOn = "mtgl_edit_btn";
            tglEventParm1.ImgOff = "mtgl_edit_btn";
            tglEventParm1.X = panelLeftLocation;
            tglEventParm1.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 18);
            tglEventParm1.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventParm1.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventParm2 == null)
            {
                tglEventParm2 = new IbbToggle(gv);
            }
            tglEventParm2.ImgOn = "mtgl_edit_btn";
            tglEventParm2.ImgOff = "mtgl_edit_btn";
            tglEventParm2.X = panelLeftLocation;
            tglEventParm2.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 20);
            tglEventParm2.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventParm2.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventParm3 == null)
            {
                tglEventParm3 = new IbbToggle(gv);
            }
            tglEventParm3.ImgOn = "mtgl_edit_btn";
            tglEventParm3.ImgOff = "mtgl_edit_btn";
            tglEventParm3.X = panelLeftLocation;
            tglEventParm3.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 22);
            tglEventParm3.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventParm3.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglEventParm4 == null)
            {
                tglEventParm4 = new IbbToggle(gv);
            }
            tglEventParm4.ImgOn = "mtgl_edit_btn";
            tglEventParm4.ImgOff = "mtgl_edit_btn";
            tglEventParm4.X = panelLeftLocation;
            tglEventParm4.Y = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * 24);
            tglEventParm4.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglEventParm4.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setup3DPreviewControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (int)((2 * gv.scaler));
            panelTopLocation = 0;

            //TILES PANEL
            if (btnTurnLeft == null)
            {
                btnTurnLeft = new IbbButton(gv, 1.0f);
            }
            btnTurnLeft.Img = "btn_small";
            btnTurnLeft.Img2 = "ctrl_left_arrow";
            btnTurnLeft.Glow = "btn_small_glow";
            btnTurnLeft.X = panelLeftLocation;
            btnTurnLeft.Y = panelTopLocation + (4 * gv.uiSquareSize) + (gv.uiSquareSize / 4);
            btnTurnLeft.Height = (int)(gv.ibbheight * gv.scaler);
            btnTurnLeft.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnMoveForward == null)
            {
                btnMoveForward = new IbbButton(gv, 1.0f);
            }
            btnMoveForward.Img = "btn_small";
            btnMoveForward.Img2 = "ctrl_up_arrow";
            btnMoveForward.Glow = "btn_small_glow";
            btnMoveForward.X = panelLeftLocation + 1 * gv.uiSquareSize;
            btnMoveForward.Y = panelTopLocation + (4 * gv.uiSquareSize) + (gv.uiSquareSize / 4);
            btnMoveForward.Height = (int)(gv.ibbheight * gv.scaler);
            btnMoveForward.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnTurnRight == null)
            {
                btnTurnRight = new IbbButton(gv, 1.0f);
            }
            btnTurnRight.Img = "btn_small";
            btnTurnRight.Img2 = "ctrl_right_arrow";
            btnTurnRight.Glow = "btn_small_glow";
            btnTurnRight.X = panelLeftLocation + 2 * gv.uiSquareSize;
            btnTurnRight.Y = panelTopLocation + (4 * gv.uiSquareSize) + (gv.uiSquareSize / 4);
            btnTurnRight.Height = (int)(gv.ibbheight * gv.scaler);
            btnTurnRight.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsAreaEditor(SKCanvas c)
        {
            setControlsStart();
            if (currentMode.Equals("3DPreview"))
            {
                draw3dView(c);
                drawLocationText(c);
            }
            else
            {
                drawAreaMap(c);
                drawTriggers(c);
                drawGrid(c);
            }

            int width2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Width;
            int height2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Height;
            IbRect src2 = new IbRect(0, 0, width2, height2);
            IbRect dst2 = new IbRect(0 - (int)((170 * gv.scaler)), 0 - (int)((102 * gv.scaler)), (int)(width2 * gv.scaler), (int)(height2 * gv.scaler));
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png"), src2, dst2);

            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            //Page Title
            string areaToEdit = "none";
            if (gv.mod.currentArea != null)
            {
                areaToEdit = gv.mod.currentArea.Filename;
            }
            gv.DrawText(c, "AREA EDITOR: " + areaToEdit, 2 * gv.uiSquareSize, 2 * gv.scaler, "yl");

            if (currentMode.Equals("Info"))
            {
                setupInfoPanelControls();
                drawInfoPanel(c);
            }
            else if (currentMode.Equals("Tiles"))
            {
                setupTilesPanelControls();
                drawTilesPanel(c);
            }
            else if (currentMode.Equals("Triggers"))
            {
                setupTriggerPanelControls();
                drawTriggersPanel(c);
            }
            else if (currentMode.Equals("WalkLoS"))
            {
                setupWalkLoSPanelControls();
                drawWalkLoSPanel(c);
            }
            else if (currentMode.Equals("3DPreview"))
            {
                setup3DPreviewControls();
                draw3DPreviewPanel(c);
                drawMiniMap(c);
            }
            else if (currentMode.Equals("Settings"))
            {
                setupSettingsPanelControls();
                drawSettingsPanel(c);
            }

            btnInfo.Draw(c);
            btnTiles.Draw(c);
            btnTriggers.Draw(c);
            btnWalkLoS.Draw(c);
            btn3DPreview.Draw(c);
            btnSettings.Draw(c);            

            gv.tsMainMenu.redrawTsMainMenu(c);

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox(c);
            }
        }
        public void draw3dView(SKCanvas c)
        {
            map3DViewStartLocXinPixels = 2 * gv.squareSize - 10;
            int scaler = (int)(gv.scaler * 2);

            //draw backdrop
            int frameX = GetFrameX();
            int frameY = GetFrameY();
            //draw near band or current square (0,0) to (88,88)
            string backdrop = gv.mod.currentArea.Layer1Filename[PlayerLocationY * gv.mod.currentArea.MapSizeX + PlayerLocationX];
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
            IbRect dst = new IbRect(map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), (int)((int)((1 * gv.squareSize * gv.scaler))), 88 * scaler, 88 * scaler);
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(backdrop), src, dst);

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
                dst = new IbRect(map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), (int)((1 * gv.squareSize * gv.scaler)) + 16 * scaler, 88 * scaler, 56 * scaler);
                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(backdrop), src, dst);
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
                dst = new IbRect(map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), (int)((1 * gv.squareSize * gv.scaler)) + 32 * scaler, 88 * scaler, 24 * scaler);
                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(backdrop), src, dst);
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
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                    }
                    else if (col == 2)
                    {
                        int tlX = 76 * scaler;
                        int tlY = 0;
                        int brX = 16 * scaler;
                        int brY = 88 * scaler;
                        src = new IbRect(188, 0, 16, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
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
                            dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                            if (!wallset.Equals("none"))
                            {
                                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                            }
                            if (!overlay.Equals("none"))
                            {
                                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
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
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                        //draw right side
                        tlX = 8 * scaler;
                        brX = 12 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(160, 0, 12, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                    }
                    else if (col == -1)
                    {
                        //draw front (24)
                        tlX = 0 * scaler;
                        brX = 40 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(120, 0, 40, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                        if (!trigImage.Equals("none"))
                        {
                            src = new IbRect(0, 0, 48, 48);
                            int tlYY = tlY + 40 * scaler;
                            int tlXX = tlX + 14 * scaler;
                            brX = 12 * scaler;
                            brY = 12 * scaler;
                            dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlYY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(trigImage), src, dst);
                        }
                        //draw right side
                        tlX = 32 * scaler;
                        brX = 12 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(160, 0, 12, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
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
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst, true);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst, true);
                        }
                        //draw front (8)
                        tlX = 72 * scaler;
                        brX = 16 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(120, 0, 16, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                        if (!trigImage.Equals("none"))
                        {
                            src = new IbRect(0, 0, 8, 48);
                            int tlYY = tlY + 40 * scaler;
                            int tlXX = tlX + 14 * scaler;
                            brX = 2 * scaler;
                            brY = 12 * scaler;
                            dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlYY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(trigImage), src, dst);
                        }
                    }
                    else if (col == 1)
                    {
                        //draw left side
                        tlX = 44 * scaler;
                        brX = 12 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(160, 0, 12, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst, true);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst, true);
                        }
                        //draw front (24)
                        tlX = 48 * scaler;
                        brX = 40 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(120, 0, 40, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                        if (!trigImage.Equals("none"))
                        {
                            src = new IbRect(0, 0, 48, 48);
                            int tlYY = tlY + 40 * scaler;
                            int tlXX = tlX + 14 * scaler;
                            brX = 12 * scaler;
                            brY = 12 * scaler;
                            dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlYY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(trigImage), src, dst);
                        }
                    }
                    else if (col == 0)
                    {
                        //draw front (24)
                        tlX = 24 * scaler;
                        brX = 40 * scaler;
                        brY = 88 * scaler;
                        src = new IbRect(120, 0, 40, 88);
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
                        }
                        if (!trigImage.Equals("none"))
                        {
                            src = new IbRect(0, 0, 48, 48);
                            int tlYY = tlY + 40 * scaler;
                            int tlXX = tlX + 14 * scaler;
                            brX = 12 * scaler;
                            brY = 12 * scaler;
                            dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlYY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(trigImage), src, dst);
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
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (col == -2)
                        {
                            src = new IbRect(104, 0, 8, 88);
                            dst = new IbRect(((((col + 2) * 24) - 0) * scaler) + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), (int)((1 * gv.squareSize * gv.scaler)), 8 * scaler, 88 * scaler);
                        }
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
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
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (col == 2)
                        {
                            src = new IbRect(104, 0, 8, 88);
                            dst = new IbRect(((((col + 2) * 24) - 16) * scaler) + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), (int)((1 * gv.squareSize * gv.scaler)), 8 * scaler, 88 * scaler);
                        }
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst, true);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst, true);
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
                    dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                    if (col == -1)
                    {
                        src = new IbRect(72, 0, 24, 88);
                        dst = new IbRect(((((col + 1) * 56) - 0) * scaler) + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), (int)((1 * gv.squareSize * gv.scaler)), 24 * scaler, 88 * scaler);
                    }
                    else if (col == 1)
                    {
                        src = new IbRect(24, 0, 24, 88);
                        dst = new IbRect(((((col + 1) * 56) - 48) * scaler) + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), (int)((1 * gv.squareSize * gv.scaler)), 24 * scaler, 88 * scaler);
                    }
                    if (!wallset.Equals("none"))
                    {
                        gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                    }
                    if (!overlay.Equals("none"))
                    {
                        gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
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
                        dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlYY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(trigImage), src, dst);
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
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst);
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
                        dst = new IbRect(tlX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                        if (!wallset.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(wallset), src, dst, true);
                        }
                        if (!overlay.Equals("none"))
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(overlay), src, dst, true);
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
                            dst = new IbRect(tlXX + map3DViewStartLocXinPixels + (int)((2 * gv.squareSize * gv.scaler)), tlYY + (int)((1 * gv.squareSize * gv.scaler)), brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(trigImage), src, dst);
                        }
                    }
                    catch { }
                }                
            }
        }
        public int GetFrameX()
        {
            if (PlayerFacingDirection == 0) //NORTH
            {
                return 0;
            }
            else if (PlayerFacingDirection == 1) //EAST
            {
                return 1;
            }
            else if (PlayerFacingDirection == 2) //SOUTH
            {
                return 2;
            }
            else if (PlayerFacingDirection == 3) //WEST
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
                    if (PlayerFacingDirection == 0) //NORTH
                    {
                        //change x and y to this direction
                        x = PlayerLocationX + col;
                        y = PlayerLocationY - row;                        
                    }
                    else if (PlayerFacingDirection == 1) //EAST
                    {
                        //change x and y to this direction
                        x = PlayerLocationX + row;
                        y = PlayerLocationY + col;
                    }
                    else if (PlayerFacingDirection == 2) //SOUTH
                    {
                        //change x and y to this direction
                        x = PlayerLocationX - col;
                        y = PlayerLocationY + row;
                    }
                    else //WEST
                    {
                        //change x and y to this direction
                        x = PlayerLocationX - row;
                        y = PlayerLocationY - col;
                    }
                    if ((p.X == x) && (p.Y == y))
                    {
                        return t.ImageFileName;
                    }
                }
            }
            return filename;
        }
        public void drawMiniMap(SKCanvas c)
        {
            int shiftY = panelTopLocation + gv.fontHeight + gv.fontLineSpacing;
            int shiftX = panelLeftLocation - (gv.squareSize / 1);

            //Draw Layers
            #region Draw Layer 1
            if (rbtnShowLayer1.toggleOn)
            {
                for (int x = 0; x <= gv.mod.currentArea.MapSizeX - 1; x++)
                {
                    for (int y = 0; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                    {
                        string tile = gv.mod.currentArea.Layer1Filename[y * gv.mod.currentArea.MapSizeX + x];
                        int tlX = (int)(x * gv.squareSize / (2 * 2) * gv.scaler);
                        int tlY = (int)(y * gv.squareSize / (2 * 2) * gv.scaler);
                        int brX = (int)(gv.squareSize / (2 * 2) * gv.scaler);
                        int brY = (int)(gv.squareSize / (2 * 2) * gv.scaler);

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            dst = new IbRect(tlX + shiftX, tlY + shiftY, brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                    }
                }
            }
            #endregion
            #region Draw Layer 2
            if (rbtnShowLayer2.toggleOn)
            {
                for (int x = 0; x <= gv.mod.currentArea.MapSizeX - 1; x++)
                {
                    for (int y = 0; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                    {
                        string tile = gv.mod.currentArea.Layer2Filename[y * gv.mod.currentArea.MapSizeX + x];
                        int tlX = (int)(x * gv.squareSize / (2 * 2) * gv.scaler);
                        int tlY = (int)(y * gv.squareSize / (2 * 2) * gv.scaler);
                        int brX = (int)(gv.squareSize / (2 * 2) * gv.scaler);
                        int brY = (int)(gv.squareSize / (2 * 2) * gv.scaler);

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            if (gv.mod.currentArea.Is3dArea)
                            {
                                src = new IbRect(32, 16, 56, 56);
                            }
                            dst = new IbRect(tlX + shiftX, tlY + shiftY, brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                    }
                }
            }
            #endregion
            #region Draw Layer 3
            if (rbtnShowLayer3.toggleOn)
            {
                if (gv.mod.currentArea.Layer3Filename.Count > 0)
                {
                    for (int x = 0; x <= gv.mod.currentArea.MapSizeX - 1; x++)
                    {
                        for (int y = 0; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                        {
                            string tile = gv.mod.currentArea.Layer3Filename[y * gv.mod.currentArea.MapSizeX + x];
                            int tlX = (int)(x * gv.squareSize / (2 * 2) * gv.scaler);
                            int tlY = (int)(y * gv.squareSize / (2 * 2) * gv.scaler);
                            int brX = (int)(gv.squareSize / (2 * 2) * gv.scaler);
                            int brY = (int)(gv.squareSize / (2 * 2) * gv.scaler);

                            try
                            {
                                src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                                if (gv.mod.currentArea.Is3dArea)
                                {
                                    src = new IbRect(32, 16, 56, 56);
                                }
                                dst = new IbRect(tlX + shiftX, tlY + shiftY, brX, brY);
                                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                            }
                            catch { }
                        }
                    }
                }
            }
            #endregion

            //Draw grid
            for (int x = 0; x <= gv.mod.currentArea.MapSizeX - 1; x++)
            {
                for (int y = 0; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                {
                    int tlX = (int)(x * gv.squareSize / (2 * 2) * gv.scaler);
                    int tlY = (int)(y * gv.squareSize / (2 * 2) * gv.scaler);
                    int brX = (int)(gv.squareSize / (2 * 2) * gv.scaler);
                    int brY = (int)(gv.squareSize / (2 * 2) * gv.scaler);
                    src = new IbRect(0, 0, gv.cc.walkBlocked.Width, gv.cc.walkBlocked.Height);
                    dst = new IbRect(tlX + shiftX, tlY + shiftY, brX, brY);
                    if (gv.mod.currentArea.LoSBlocked[y * gv.mod.currentArea.MapSizeX + x] == 1)
                    {
                        gv.DrawBitmap(c, gv.cc.losBlocked, src, dst);
                    }
                    if (gv.mod.currentArea.Walkable[y * gv.mod.currentArea.MapSizeX + x] == 0)
                    {
                        gv.DrawBitmap(c, gv.cc.walkBlocked, src, dst);
                    }
                    else
                    {
                        gv.DrawBitmap(c, gv.cc.walkPass, src, dst);
                    }
                }
            }
            
            //Draw any Trigger Images
            foreach (Trigger t in gv.mod.currentArea.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    int dx = (int)((p.X * gv.squareSize / (2 * 2) * gv.scaler)) + shiftX;
                    int dy = (int)((p.Y * gv.squareSize / (2 * 2) * gv.scaler)) + shiftY;
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
                    IbRect rect = new IbRect(dx + 3, dy + 3, gv.squareSize / (int)((2 * 2) * gv.scaler) - 6, gv.squareSize / (int)((2 * 2) * gv.scaler) - 6);
                    gv.DrawRectangle(c, rect, clr, 2);

                    if ((!t.ImageFileName.Equals("none")) && (t.isShown))
                    {
                        int tlX = (int)(p.X * gv.squareSize / (2 * 2) * gv.scaler);
                        int tlY = (int)(p.Y * gv.squareSize / (2 * 2) * gv.scaler);
                        int brX = (int)(gv.squareSize / (2 * 2) * gv.scaler);
                        int brY = (int)(gv.squareSize / (2 * 2) * gv.scaler);
                        src = new IbRect(0, 0, gv.cc.GetFromBitmapList(t.ImageFileName).Width, gv.cc.GetFromBitmapList(t.ImageFileName).Height);
                        dst = new IbRect(tlX + shiftX, tlY + shiftY, brX, brY);
                        gv.DrawBitmap(c, gv.cc.GetFromBitmapList(t.ImageFileName), src, dst, !t.ImageFacingLeft);
                    }
                }
            }

            //Draw Location Marker
            int tlX2 = (int)(PlayerLocationX * gv.squareSize / (2 * 2) * gv.scaler);
            int tlY2 = (int)(PlayerLocationY * gv.squareSize / (2 * 2) * gv.scaler);
            int brX2 = (int)(gv.squareSize / (2 * 2) * gv.scaler);
            int brY2 = (int)(gv.squareSize / (2 * 2) * gv.scaler);
            if (PlayerFacingDirection == 0) //NORTH
            {                
                try
                {
                    string tile = "minimap_location_n";
                    src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                    dst = new IbRect(tlX2 + shiftX, tlY2 + shiftY, brX2, brY2);
                    gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                }
                catch { }
            }
            else if (PlayerFacingDirection == 1) //EAST
            {
                try
                {
                    string tile = "minimap_location_e";
                    src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                    dst = new IbRect(tlX2 + shiftX, tlY2 + shiftY, brX2, brY2);
                    gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                }
                catch { }
            }
            else if (PlayerFacingDirection == 2) //SOUTH
            {
                try
                {
                    string tile = "minimap_location_s";
                    src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                    dst = new IbRect(tlX2 + shiftX, tlY2 + shiftY, brX2, brY2);
                    gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                }
                catch { }
            }
            else //WEST
            {
                try
                {
                    string tile = "minimap_location_w";
                    src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                    dst = new IbRect(tlX2 + shiftX, tlY2 + shiftY, brX2, brY2);
                    gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                }
                catch { }
            }
        }
        public void drawLocationText(SKCanvas c)
        {            
            string direction = "N";
            if (PlayerFacingDirection == 0) { direction = "N"; }
            if (PlayerFacingDirection == 1) { direction = "E"; }
            if (PlayerFacingDirection == 2) { direction = "S"; }
            if (PlayerFacingDirection == 3) { direction = "W"; }

            int txtH = (int)gv.fontHeight;
            int xLoc = 1 * gv.uiSquareSize + (int)((2 * gv.scaler));
            int yLoc = (7 * gv.uiSquareSize) - gv.fontHeight - gv.fontHeight;
            
            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    gv.DrawText(c, "(" + PlayerLocationX + "," + PlayerLocationY + ") " + direction, x + xLoc, y + yLoc, "bk");
                }
            }
            gv.DrawText(c, "(" + PlayerLocationX + "," + PlayerLocationY + ") " + direction, xLoc, yLoc, "wh");
        }
        public int GetSquareIndex(int col, int row)
        {
            if (PlayerFacingDirection == 0) //NORTH
            {
                //change x and y to this direction
                int x = PlayerLocationX + col;
                int y = PlayerLocationY - row;
                return y * gv.mod.currentArea.MapSizeX + x;
            }
            else if (PlayerFacingDirection == 1) //EAST
            {
                //change x and y to this direction
                int x = PlayerLocationX + row;
                int y = PlayerLocationY + col;
                return y * gv.mod.currentArea.MapSizeX + x;
            }
            else if (PlayerFacingDirection == 2) //SOUTH
            {
                //change x and y to this direction
                int x = PlayerLocationX - col;
                int y = PlayerLocationY + row;
                return y * gv.mod.currentArea.MapSizeX + x;
            }
            else //WEST
            {
                //change x and y to this direction
                int x = PlayerLocationX - row;
                int y = PlayerLocationY - col;
                return y * gv.mod.currentArea.MapSizeX + x;
            }
        }        
        public bool IsSquareOnMap(int col, int row)
        {
            if (PlayerFacingDirection == 0) //NORTH
            {
                //change x and y to this direction
                int x = PlayerLocationX + col;
                int y = PlayerLocationY - row;
                if (x < 0) { return false; }
                if (x >= gv.mod.currentArea.MapSizeX) { return false; }
                if (y < 0) { return false; }
                if (y >= gv.mod.currentArea.MapSizeY) { return false; }
            }
            else if (PlayerFacingDirection == 1) //EAST
            {
                //change x and y to this direction
                int x = PlayerLocationX + row;
                int y = PlayerLocationY + col;
                if (x < 0) { return false; }
                if (x >= gv.mod.currentArea.MapSizeX) { return false; }
                if (y < 0) { return false; }
                if (y >= gv.mod.currentArea.MapSizeY) { return false; }
            }
            else if (PlayerFacingDirection == 2) //SOUTH
            {
                //change x and y to this direction
                int x = PlayerLocationX - col;
                int y = PlayerLocationY + row;
                if (x < 0) { return false; }
                if (x >= gv.mod.currentArea.MapSizeX) { return false; }
                if (y < 0) { return false; }
                if (y >= gv.mod.currentArea.MapSizeY) { return false; }
            }
            else //WEST
            {
                //change x and y to this direction
                int x = PlayerLocationX - row;
                int y = PlayerLocationY - col;
                if (x < 0) { return false; }
                if (x >= gv.mod.currentArea.MapSizeX) { return false; }
                if (y < 0) { return false; }
                if (y >= gv.mod.currentArea.MapSizeY) { return false; }
            }
            return true;
        }

        public void drawAreaMap(SKCanvas c)
        {
            #region Draw Layer 1
            if (rbtnShowLayer1.toggleOn)
            {
                for (int x = upperSquareX; x <= gv.mod.currentArea.MapSizeX - 1; x++)
                {
                    for (int y = upperSquareY; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                    {
                        string tile = gv.mod.currentArea.Layer1Filename[y * gv.mod.currentArea.MapSizeX + x];
                        int tlX = (int)((x - upperSquareX) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                        int tlY = (int)((y - upperSquareY) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                        int brX = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
                        int brY = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                            //bool mirror = false;
                            //if (gv.mod.currentArea.Layer1Mirror[y * gv.mod.currentArea.MapSizeX + x] == 1) { mirror = true; }
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                    }
                }
            }
            #endregion
            #region Draw Layer 2
            if (rbtnShowLayer2.toggleOn)
            {
                for (int x = upperSquareX; x <= gv.mod.currentArea.MapSizeX - 1; x++)
                {
                    for (int y = upperSquareY; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                    {
                        string tile = gv.mod.currentArea.Layer2Filename[y * gv.mod.currentArea.MapSizeX + x];
                        int tlX = (int)((x - upperSquareX) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                        int tlY = (int)((y - upperSquareY) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                        int brX = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
                        int brY = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            if (gv.mod.currentArea.Is3dArea)
                            {
                                src = new IbRect(32, 16, 56, 56);
                            }
                            dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                            //bool mirror = false;
                            //if (gv.mod.currentArea.Layer2Mirror[y * gv.mod.currentArea.MapSizeX + x] == 1) { mirror = true; }
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                    }
                }
            }
            #endregion
            #region Draw Layer 3
            if (rbtnShowLayer3.toggleOn)
            {
                if (gv.mod.currentArea.Layer3Filename.Count > 0)
                {
                    for (int x = upperSquareX; x <= gv.mod.currentArea.MapSizeX - 1; x++)
                    {
                        for (int y = upperSquareY; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                        {
                            string tile = gv.mod.currentArea.Layer3Filename[y * gv.mod.currentArea.MapSizeX + x];
                            int tlX = (int)((x - upperSquareX) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                            int tlY = (int)((y - upperSquareY) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                            int brX = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
                            int brY = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);

                            try
                            {
                                src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                                if (gv.mod.currentArea.Is3dArea)
                                {
                                    src = new IbRect(32, 16, 56, 56);
                                }
                                dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                                //bool mirror = false;
                                //if (gv.mod.currentArea.Layer3Mirror[y * gv.mod.currentArea.MapSizeX + x] == 1) { mirror = true; }
                                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                            }
                            catch { }
                        }
                    }
                }
            }
            #endregion
        }
        public void drawTriggers(SKCanvas c)
        {
            foreach (Trigger t in gv.mod.currentArea.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    string TrigColor = "highlight_yellowTrig";
                    SKColor clr = SKColors.Orange;
                    if ((t.Event1Type.Equals("encounter")) || (t.Event2Type.Equals("encounter")) || (t.Event3Type.Equals("encounter")))
                    {
                        TrigColor = "highlight_redTrig";
                    }
                    else if (t.Event1Type.Equals("conversation"))
                    {
                        TrigColor = "highlight_yellowTrig";
                    }
                    else if (t.Event1Type.Equals("script"))
                    {
                        TrigColor = "highlight_blueTrig";
                    }
                    else if (t.Event1Type.Equals("transition"))
                    {
                        TrigColor = "highlight_greenTrig";
                    }
                    else if (t.Event1Type.Equals("container"))
                    {
                        TrigColor = "highlight_magentaTrig";
                    }

                    int tlX = (int)((p.X - upperSquareX) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                    int tlY = (int)((p.Y - upperSquareY) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                    int brX = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
                    int brY = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
                    src = new IbRect(0, 0, gv.cc.GetFromBitmapList(TrigColor).Width, gv.cc.GetFromBitmapList(TrigColor).Height);
                    dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                    gv.DrawBitmap(c, gv.cc.GetFromBitmapList(TrigColor), src, dst, !t.ImageFacingLeft);

                    if ((!t.ImageFileName.Equals("none")) && (t.isShown))
                    {
                        src = new IbRect(0, 0, gv.cc.GetFromBitmapList(t.ImageFileName).Width, gv.cc.GetFromBitmapList(t.ImageFileName).Height);
                        dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                        gv.DrawBitmap(c, gv.cc.GetFromBitmapList(t.ImageFileName), src, dst, !t.ImageFacingLeft);
                    }
                }
            }
        }
        public void drawGrid(SKCanvas c)
        {
            for (int x = upperSquareX; x <= gv.mod.currentArea.MapSizeX - 1; x++)
            {
                for (int y = upperSquareY; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                {
                    int tlX = (int)((x - upperSquareX) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                    int tlY = (int)((y - upperSquareY) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
                    int brX = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
                    int brY = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
                    src = new IbRect(0, 0, gv.cc.walkBlocked.Width, gv.cc.walkBlocked.Height);
                    dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
                    if (gv.mod.currentArea.LoSBlocked[y * gv.mod.currentArea.MapSizeX + x] == 1)
                    {
                        gv.DrawBitmap(c, gv.cc.losBlocked, src, dst);
                    }
                    if (gv.mod.currentArea.Walkable[y * gv.mod.currentArea.MapSizeX + x] == 0)
                    {
                        gv.DrawBitmap(c, gv.cc.walkBlocked, src, dst);
                    }
                    else
                    {
                        gv.DrawBitmap(c, gv.cc.walkPass, src, dst);
                    }
                }
            }
        }
        public void drawInfoPanel(SKCanvas c)
        {
            int index = selectedSquare.Y * gv.mod.currentArea.MapSizeX + selectedSquare.X;
            if (index > gv.mod.currentArea.Layer2Filename.Count) { index = 0; }

            gv.DrawText(c, "INFO OF TILE", panelLeftLocation, panelTopLocation + (0 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            gv.DrawText(c, "Tile: (" + selectedSquare.X + "," + selectedSquare.Y + ")", panelLeftLocation, panelTopLocation + (1 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            gv.DrawText(c, "Layer 1:", panelLeftLocation, panelTopLocation + (2 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            gv.DrawText(c, gv.mod.currentArea.Layer1Filename[index], panelLeftLocation, panelTopLocation + (3 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            gv.DrawText(c, "Layer 2:", panelLeftLocation, panelTopLocation + (4 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            gv.DrawText(c, gv.mod.currentArea.Layer2Filename[index], panelLeftLocation, panelTopLocation + (5 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            gv.DrawText(c, "Layer 3:", panelLeftLocation, panelTopLocation + (6 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            gv.DrawText(c, gv.mod.currentArea.Layer3Filename[index], panelLeftLocation, panelTopLocation + (7 * (gv.fontHeight + gv.fontLineSpacing)), "wh");

            //info on trigger
            string trigTag = "none";
            if (gv.mod.currentArea.getTriggerByLocation(selectedSquare.X, selectedSquare.Y) != null)
            {
                trigTag = gv.mod.currentArea.getTriggerByLocation(selectedSquare.X, selectedSquare.Y).TriggerTag;
            }
            gv.DrawText(c, "Trigger:", panelLeftLocation, panelTopLocation + (9 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            gv.DrawText(c, trigTag, panelLeftLocation, panelTopLocation + (10 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            //info on walk/LoS
            gv.DrawText(c, "Walkable:", panelLeftLocation, panelTopLocation + (11 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            if (gv.mod.currentArea.Walkable[index] == 1)
            {
                gv.DrawText(c, "OPEN", panelLeftLocation, panelTopLocation + (12 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            }
            else
            {
                gv.DrawText(c, "BLOCKED", panelLeftLocation, panelTopLocation + (12 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            }

            //info on walk/LoS
            gv.DrawText(c, "Line-Of-Sight:", panelLeftLocation, panelTopLocation + (13 * (gv.fontHeight + gv.fontLineSpacing)), "yl");
            if (gv.mod.currentArea.LoSBlocked[index] == 1)
            {
                gv.DrawText(c, "BLOCKED", panelLeftLocation, panelTopLocation + (14 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            }
            else
            {
                gv.DrawText(c, "VISIBLE", panelLeftLocation, panelTopLocation + (14 * (gv.fontHeight + gv.fontLineSpacing)), "wh");
            }

            //ZOOM
            gv.DrawText(c, "ZOOM LEVEL", panelLeftLocation, panelTopLocation + (16 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            int shiftForFont = (rbtnShowLayer1.Height / 2) - (gv.fontHeight / 2);
            rbtnZoom1.Draw(c);
            gv.DrawText(c, "1X", rbtnZoom1.X + rbtnZoom1.Width + gv.scaler, rbtnZoom1.Y + shiftForFont, "wh");
            rbtnZoom2.Draw(c);
            gv.DrawText(c, "2X", rbtnZoom2.X + rbtnZoom2.Width + gv.scaler, rbtnZoom2.Y + shiftForFont, "wh");

            
            //draw selected info tile highlight
            int tlX = (int)((selectedSquare.X - upperSquareX) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
            int tlY = (int)((selectedSquare.Y - upperSquareY) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
            int brX = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
            int brY = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
            dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("highlight_magenta"), src, dst);

            drawPreviewViewport(c);
            //btnHelp.Draw();
        }
        public void drawPreviewViewport(SKCanvas c)
        {
            int previewSquareSize = (int)(gv.squareSize * gv.scaler * 3 / gv.mod.currentArea.MapSizeX);
            if (previewSquareSize > (int)(gv.squareSize * gv.scaler * 3 / gv.mod.currentArea.MapSizeY))
            {
                previewSquareSize = (int)(gv.squareSize * gv.scaler * 3 / gv.mod.currentArea.MapSizeY);
            }
            int shiftY = panelTopLocation + (int)(7 * gv.squareSize * gv.scaler);
            int shiftX = panelLeftLocation;

            //draw preview background
            int brX1 = (int)(gv.squareSize * gv.scaler * 3);
            int brY1 = (int)(gv.squareSize * gv.scaler * 3);
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("ui_previewback").Width, gv.cc.GetFromTileBitmapList("ui_previewback").Height);
            dst = new IbRect(0 + shiftX, 0 + shiftY, brX1, brY1);
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("ui_previewback"), src, dst);


            //Draw Layers
            #region Draw Layer 1
            if (rbtnShowLayer1.toggleOn)
            {
                for (int x = 0; x <= gv.mod.currentArea.MapSizeX - 1; x++)
                {
                    for (int y = 0; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                    {
                        string tile = gv.mod.currentArea.Layer1Filename[y * gv.mod.currentArea.MapSizeX + x];
                        int tlX = (int)(x * previewSquareSize);
                        int tlY = (int)(y * previewSquareSize);
                        int brX = (int)(previewSquareSize);
                        int brY = (int)(previewSquareSize);

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            dst = new IbRect(tlX + shiftX, tlY + shiftY, brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                    }
                }
            }
            #endregion
            #region Draw Layer 2
            if (rbtnShowLayer2.toggleOn)
            {
                for (int x = 0; x <= gv.mod.currentArea.MapSizeX - 1; x++)
                {
                    for (int y = 0; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                    {
                        string tile = gv.mod.currentArea.Layer2Filename[y * gv.mod.currentArea.MapSizeX + x];
                        int tlX = (int)(x * previewSquareSize);
                        int tlY = (int)(y * previewSquareSize);
                        int brX = (int)(previewSquareSize);
                        int brY = (int)(previewSquareSize);

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            if (gv.mod.currentArea.Is3dArea)
                            {
                                src = new IbRect(32, 16, 56, 56);
                            }
                            dst = new IbRect(tlX + shiftX, tlY + shiftY, brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                    }
                }
            }
            #endregion
            #region Draw Layer 3
            if (rbtnShowLayer3.toggleOn)
            {
                for (int x = 0; x <= gv.mod.currentArea.MapSizeX - 1; x++)
                {
                    for (int y = 0; y <= gv.mod.currentArea.MapSizeY - 1; y++)
                    {
                        string tile = gv.mod.currentArea.Layer3Filename[y * gv.mod.currentArea.MapSizeX + x];
                        int tlX = (int)(x * previewSquareSize);
                        int tlY = (int)(y * previewSquareSize);
                        int brX = (int)(previewSquareSize);
                        int brY = (int)(previewSquareSize);

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            if (gv.mod.currentArea.Is3dArea)
                            {
                                src = new IbRect(32, 16, 56, 56);
                            }
                            dst = new IbRect(tlX + shiftX, tlY + shiftY, brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                    }
                }
            }
            #endregion

            //draw Pan square

            int panSqrSize = 10 * mapSquareSizeScaler;
            //draw pan selection square highlight
            int tlX2 = (int)(upperSquareX * previewSquareSize);
            int tlY2 = (int)(upperSquareY * previewSquareSize);
            int brX2 = (int)(panSqrSize * previewSquareSize);
            int brY2 = (int)(panSqrSize * previewSquareSize);
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magentaTrig").Width, gv.cc.GetFromTileBitmapList("highlight_magentaTrig").Height);
            dst = new IbRect(tlX2 + shiftX, tlY2 + shiftY, brX2, brY2);
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("highlight_magentaTrig"), src, dst);

        }
        public void drawTilesPanel(SKCanvas c)
        {
            #region 2D AREA
            if (!gv.mod.currentArea.Is3dArea)
            {
                /*if (tilesPageIndex * tileSlotsPerPage >= floorTiles.Count)
                {
                    return;
                }*/

                int cnt = 0;
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < (tileSlotsPerPage / 4); y++)
                    {
                        string tile = "none";
                        if (tglMiscTiles.toggleOn)
                        {
                            if ((cnt + (tilesPageIndex * tileSlotsPerPage)) < tilesMiscList.Count)
                            {
                                tile = tilesMiscList[cnt + (tilesPageIndex * tileSlotsPerPage)];
                            }
                        }
                        else if (tglWallFloorTiles.toggleOn)
                        {
                            if ((cnt + (tilesPageIndex * tileSlotsPerPage)) < tilesWallFloorList.Count)
                            {
                                tile = tilesWallFloorList[cnt + (tilesPageIndex * tileSlotsPerPage)];
                            }
                        }
                        else if (tglPropTiles.toggleOn)
                        {
                            if ((cnt + (tilesPageIndex * tileSlotsPerPage)) < tilesPropList.Count)
                            {
                                tile = tilesPropList[cnt + (tilesPageIndex * tileSlotsPerPage)];
                            }
                        }
                        else if (tglUserTiles.toggleOn)
                        {
                            if ((cnt + (tilesPageIndex * tileSlotsPerPage)) < tilesUserList.Count)
                            {
                                tile = tilesUserList[cnt + (tilesPageIndex * tileSlotsPerPage)];
                            }
                        }
                        int tlX = (int)(x * gv.squareSize * gv.scaler);
                        int tlY = (int)(y * gv.squareSize * gv.scaler);
                        int brX = (int)(gv.squareSize * gv.scaler);
                        int brY = (int)(gv.squareSize * gv.scaler);

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            dst = new IbRect(tlX + panelLeftLocation, tlY + panelTopLocation, brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                        cnt++;
                    }
                }

                int slotX = tileSlotIndex / (tileSlotsPerPage / 4);
                int slotY = tileSlotIndex - slotX * (tileSlotsPerPage / 4);
                int tlX2 = (int)(slotX * gv.squareSize * gv.scaler);
                int tlY2 = (int)(slotY * gv.squareSize * gv.scaler);
                int brX2 = (int)(gv.squareSize * gv.scaler);
                int brY2 = (int)(gv.squareSize * gv.scaler);
                src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
                dst = new IbRect(tlX2 + panelLeftLocation, tlY2 + panelTopLocation, brX2, brY2);
                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("highlight_magenta"), src, dst);

                //Description     
                gv.DrawText(c, "PAINT TILES", panelLeftLocation, panelTopLocation, "gn");

                tglMiscTiles.Draw(c);
                tglWallFloorTiles.Draw(c);
                tglPropTiles.Draw(c);
                tglUserTiles.Draw(c);

                btnTilesLeft.Draw(c);
                btnTilesPageIndex.Draw(c);
                tlX2 = (int)(btnTilesPageIndex.X + ((gv.uiSquareSize - (gv.squareSize * gv.scaler)) / 2));
                tlY2 = (int)(btnTilesPageIndex.Y + ((gv.uiSquareSize - (gv.squareSize * gv.scaler)) / 2));
                brX2 = (int)(gv.squareSize * gv.scaler);
                brY2 = (int)(gv.squareSize * gv.scaler);
                src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(currentTile).Width, gv.cc.GetFromTileBitmapList(currentTile).Height);
                dst = new IbRect(tlX2, tlY2, brX2, brY2);
                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(currentTile), src, dst);
                btnTilesRight.Draw(c);

                int shiftForFont = (rbtnShowLayer1.Height / 2) - (gv.fontHeight / 2);
                gv.DrawText(c, "SHOW", rbtnShowLayer1.X + gv.scaler, rbtnShowLayer1.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
                rbtnShowLayer1.Draw(c);
                gv.DrawText(c, "Lyr1", rbtnShowLayer1.X + rbtnShowLayer1.Width + gv.scaler, rbtnShowLayer1.Y + shiftForFont, "wh");
                rbtnShowLayer2.Draw(c);
                gv.DrawText(c, "Lyr2", rbtnShowLayer2.X + rbtnShowLayer2.Width + gv.scaler, rbtnShowLayer2.Y + shiftForFont, "wh");
                rbtnShowLayer3.Draw(c);
                gv.DrawText(c, "Lyr3", rbtnShowLayer3.X + rbtnShowLayer3.Width + gv.scaler, rbtnShowLayer3.Y + shiftForFont, "wh");

                gv.DrawText(c, "EDIT", rbtnEditLayer1.X + gv.scaler, rbtnEditLayer1.Y - gv.fontHeight - gv.fontLineSpacing, "gn");
                rbtnEditLayer1.Draw(c);
                gv.DrawText(c, "Lyr1", rbtnEditLayer1.X + rbtnEditLayer1.Width + gv.scaler, rbtnEditLayer1.Y + shiftForFont, "wh");
                rbtnEditLayer2.Draw(c);
                gv.DrawText(c, "Lyr2", rbtnEditLayer2.X + rbtnEditLayer2.Width + gv.scaler, rbtnEditLayer2.Y + shiftForFont, "wh");
                rbtnEditLayer3.Draw(c);
                gv.DrawText(c, "Lyr3", rbtnEditLayer3.X + rbtnEditLayer3.Width + gv.scaler, rbtnEditLayer3.Y + shiftForFont, "wh");
                //btnHelp.Draw();
                
            }
            #endregion
            #region 3D AREA
            else
            {
                
                int cnt = 0;
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < (tileSlotsPerPage / 4); y++)
                    {
                        string tile = "none";
                        //Edit Backdrop
                        if (rbtnEditLayer1.toggleOn)
                        {
                            if (cnt + (tilesPageIndex * tileSlotsPerPage) < backdrops.Count)
                            {
                                tile = backdrops[cnt + (tilesPageIndex * tileSlotsPerPage)];
                            }                            
                        }
                        //Edit wallset
                        else if (rbtnEditLayer2.toggleOn)
                        {
                            if (cnt + (tilesPageIndex * tileSlotsPerPage) < wallsets.Count)
                            {
                                tile = wallsets[cnt + (tilesPageIndex * tileSlotsPerPage)];
                            }                            
                        }
                        //Edit overlay
                        else if (rbtnEditLayer3.toggleOn)
                        {
                            if (cnt + (tilesPageIndex * tileSlotsPerPage) < overlays.Count)
                            {
                                tile = overlays[cnt + (tilesPageIndex * tileSlotsPerPage)];
                            }                            
                        }

                        int tlX = (int)(x * gv.squareSize * gv.scaler);
                        int tlY = (int)(y * gv.squareSize * gv.scaler);
                        int brX = (int)(gv.squareSize * gv.scaler);
                        int brY = (int)(gv.squareSize * gv.scaler);

                        try
                        {
                            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(tile).Width, gv.cc.GetFromTileBitmapList(tile).Height);
                            if ((rbtnEditLayer2.toggleOn) || (rbtnEditLayer3.toggleOn))
                            {
                                src = new IbRect(32,16,56,56);
                            }
                            dst = new IbRect(tlX + panelLeftLocation, tlY + panelTopLocation, brX, brY);
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(tile), src, dst);
                        }
                        catch { }
                        cnt++;
                    }
                }

                int slotX = tileSlotIndex / (tileSlotsPerPage / 4);
                int slotY = tileSlotIndex - slotX * (tileSlotsPerPage / 4);
                int tlX2 = (int)(slotX * gv.squareSize * gv.scaler);
                int tlY2 = (int)(slotY * gv.squareSize * gv.scaler);
                int brX2 = (int)(gv.squareSize * gv.scaler);
                int brY2 = (int)(gv.squareSize * gv.scaler);
                src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
                dst = new IbRect(tlX2 + panelLeftLocation, tlY2 + panelTopLocation, brX2, brY2);
                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("highlight_magenta"), src, dst);

                //Description     
                gv.DrawText(c, "PAINT TILES", panelLeftLocation, panelTopLocation, "gn");

                btnTilesLeft.Draw(c);
                btnTilesPageIndex.Draw(c);
                tlX2 = (int)(btnTilesPageIndex.X + ((gv.uiSquareSize - (gv.squareSize * gv.scaler)) / 2));
                tlY2 = (int)(btnTilesPageIndex.Y + ((gv.uiSquareSize - (gv.squareSize * gv.scaler)) / 2));
                brX2 = (int)(gv.squareSize * gv.scaler);
                brY2 = (int)(gv.squareSize * gv.scaler);
                src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(currentTile).Width, gv.cc.GetFromTileBitmapList(currentTile).Height);
                if ((rbtnEditLayer2.toggleOn) || (rbtnEditLayer3.toggleOn))
                {
                    src = new IbRect(32, 16, 56, 56);
                }
                dst = new IbRect(tlX2, tlY2, brX2, brY2);
                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(currentTile), src, dst);
                btnTilesRight.Draw(c);

                int shiftForFont = (rbtnShowLayer1.Height / 2) - (gv.fontHeight / 2);
                gv.DrawText(c, "SHOW", rbtnShowLayer1.X + gv.scaler, rbtnShowLayer1.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
                rbtnShowLayer1.Draw(c);
                gv.DrawText(c, "bkdrp", rbtnShowLayer1.X + rbtnShowLayer1.Width + gv.scaler, rbtnShowLayer1.Y + shiftForFont, "wh");
                rbtnShowLayer2.Draw(c);
                gv.DrawText(c, "walls", rbtnShowLayer2.X + rbtnShowLayer2.Width + gv.scaler, rbtnShowLayer2.Y + shiftForFont, "wh");
                rbtnShowLayer3.Draw(c);
                gv.DrawText(c, "ovrlay", rbtnShowLayer3.X + rbtnShowLayer3.Width + gv.scaler, rbtnShowLayer3.Y + shiftForFont, "wh");

                gv.DrawText(c, "EDIT", rbtnEditLayer1.X + gv.scaler, rbtnEditLayer1.Y - gv.fontHeight - gv.fontLineSpacing, "gn");
                rbtnEditLayer1.Draw(c);
                gv.DrawText(c, "bkdrp", rbtnEditLayer1.X + rbtnEditLayer1.Width + gv.scaler, rbtnEditLayer1.Y + shiftForFont, "wh");
                rbtnEditLayer2.Draw(c);
                gv.DrawText(c, "walls", rbtnEditLayer2.X + rbtnEditLayer2.Width + gv.scaler, rbtnEditLayer2.Y + shiftForFont, "wh");
                rbtnEditLayer3.Draw(c);
                gv.DrawText(c, "ovrlay", rbtnEditLayer3.X + rbtnEditLayer3.Width + gv.scaler, rbtnEditLayer3.Y + shiftForFont, "wh");
                //btnHelp.Draw();
            }
            #endregion
        }
        public void drawTriggersPanel(SKCanvas c)
        {
            //selectedTrigger = gv.mod.currentArea.Triggers[0];
            //Description     
            gv.DrawText(c, "TRIGGERS", panelLeftLocation, panelTopLocation + (0 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            //gv.DrawText("LINE-OF-SIGHT", panelLeftLocation, panelTopLocation + (2 * (gv.fontHeight + gv.fontLineSpacing)), "gn");

            rbtnViewInfoTrigger.Draw(c);
            gv.DrawText(c, "View", rbtnViewInfoTrigger.X + rbtnViewInfoTrigger.Width + gv.scaler, rbtnViewInfoTrigger.Y, "wh");
            gv.DrawText(c, "Info", rbtnViewInfoTrigger.X + rbtnViewInfoTrigger.Width + gv.scaler, rbtnViewInfoTrigger.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            rbtnPlaceNewTrigger.Draw(c);
            gv.DrawText(c, "Create", rbtnPlaceNewTrigger.X + rbtnPlaceNewTrigger.Width + gv.scaler, rbtnPlaceNewTrigger.Y, "wh");
            gv.DrawText(c, "New", rbtnPlaceNewTrigger.X + rbtnPlaceNewTrigger.Width + gv.scaler, rbtnPlaceNewTrigger.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            if (selectedTrigger != null)
            {
                rbtnEditTrigger.Draw(c);
                gv.DrawText(c, "Edit Last", rbtnEditTrigger.X + rbtnEditTrigger.Width + gv.scaler, rbtnEditTrigger.Y, "wh");
                gv.DrawText(c, "Selected Trigger", rbtnEditTrigger.X + rbtnEditTrigger.Width + gv.scaler, rbtnEditTrigger.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            }

            if (selectedTrigger != null)
            {
                rbtnTriggerProperties.Draw(c);
                gv.DrawText(c, "Main", rbtnTriggerProperties.X + rbtnTriggerProperties.Width + gv.scaler, rbtnTriggerProperties.Y, "wh");
                gv.DrawText(c, "Info", rbtnTriggerProperties.X + rbtnTriggerProperties.Width + gv.scaler, rbtnTriggerProperties.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                string color = "wh";
                if (selectedTrigger != null)
                {
                    if (selectedTrigger.Event1Type.Equals("conversation")) { color = "yl"; }
                    else if (selectedTrigger.Event1Type.Equals("encounter")) { color = "rd"; }
                    else if (selectedTrigger.Event1Type.Equals("script")) { color = "bu"; }
                    else if (selectedTrigger.Event1Type.Equals("transition")) { color = "gn"; }
                    else if (selectedTrigger.Event1Type.Equals("container")) { color = "ma"; }
                }
                rbtnEvent1Properties.Draw(c);
                gv.DrawText(c, "Event", rbtnEvent1Properties.X + rbtnEvent1Properties.Width + gv.scaler, rbtnEvent1Properties.Y, color);
                gv.DrawText(c, "one", rbtnEvent1Properties.X + rbtnEvent1Properties.Width + gv.scaler, rbtnEvent1Properties.Y + gv.fontHeight + gv.fontLineSpacing, color);

                color = "wh";
                if (selectedTrigger != null)
                {
                    if (selectedTrigger.Event2Type.Equals("conversation")) { color = "yl"; }
                    else if (selectedTrigger.Event2Type.Equals("encounter")) { color = "rd"; }
                    else if (selectedTrigger.Event2Type.Equals("script")) { color = "bu"; }
                    else if (selectedTrigger.Event2Type.Equals("transition")) { color = "gn"; }
                    else if (selectedTrigger.Event2Type.Equals("container")) { color = "ma"; }
                }
                rbtnEvent2Properties.Draw(c);
                gv.DrawText(c, "Event", rbtnEvent2Properties.X + rbtnEvent2Properties.Width + gv.scaler, rbtnEvent2Properties.Y, color);
                gv.DrawText(c, "two", rbtnEvent2Properties.X + rbtnEvent2Properties.Width + gv.scaler, rbtnEvent2Properties.Y + gv.fontHeight + gv.fontLineSpacing, color);

                color = "wh";
                if (selectedTrigger != null)
                {
                    if (selectedTrigger.Event3Type.Equals("conversation")) { color = "yl"; }
                    else if (selectedTrigger.Event3Type.Equals("encounter")) { color = "rd"; }
                    else if (selectedTrigger.Event3Type.Equals("script")) { color = "bu"; }
                    else if (selectedTrigger.Event3Type.Equals("transition")) { color = "gn"; }
                    else if (selectedTrigger.Event3Type.Equals("container")) { color = "ma"; }
                }
                rbtnEvent3Properties.Draw(c);
                gv.DrawText(c, "Event", rbtnEvent3Properties.X + rbtnEvent3Properties.Width + gv.scaler, rbtnEvent3Properties.Y, color);
                gv.DrawText(c, "three", rbtnEvent3Properties.X + rbtnEvent3Properties.Width + gv.scaler, rbtnEvent3Properties.Y + gv.fontHeight + gv.fontLineSpacing, color);
                            
                if (rbtnTriggerProperties.toggleOn)
                {
                    tglTriggerTag.Draw(c);
                    gv.DrawText(c, "Trigger Tag:", tglTriggerTag.X + tglTriggerTag.Width + gv.scaler, tglTriggerTag.Y, "gy");
                    gv.DrawText(c, selectedTrigger.TriggerTag, tglTriggerTag.X + tglTriggerTag.Width + gv.scaler, tglTriggerTag.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.Enabled) { tglTriggerEnabled.toggleOn = true; }
                    else { tglTriggerEnabled.toggleOn = false; }
                    tglTriggerEnabled.Draw(c);
                    gv.DrawText(c, "Triggr", tglTriggerEnabled.X + tglTriggerEnabled.Width + gv.scaler, tglTriggerEnabled.Y, "gy");
                    gv.DrawText(c, "Enabld", tglTriggerEnabled.X + tglTriggerEnabled.Width + gv.scaler, tglTriggerEnabled.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.DoOnceOnly) { tglTriggerDoOnce.toggleOn = true; }
                    else { tglTriggerDoOnce.toggleOn = false; }
                    tglTriggerDoOnce.Draw(c);
                    gv.DrawText(c, "Do Once", tglTriggerDoOnce.X + tglTriggerDoOnce.Width + gv.scaler, tglTriggerDoOnce.Y, "gy");
                    gv.DrawText(c, "Only", tglTriggerDoOnce.X + tglTriggerDoOnce.Width + gv.scaler, tglTriggerDoOnce.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    tglImageFilename.Draw(c);
                    gv.DrawText(c, "Image Filename:", tglImageFilename.X + tglImageFilename.Width + gv.scaler, tglImageFilename.Y, "gy");
                    gv.DrawText(c, selectedTrigger.ImageFileName, tglImageFilename.X + tglImageFilename.Width + gv.scaler, tglImageFilename.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.ImageFacingLeft) { tglImageFacingLeft.toggleOn = true; }
                    else { tglImageFacingLeft.toggleOn = false; }
                    tglImageFacingLeft.Draw(c);
                    gv.DrawText(c, "Face", tglImageFacingLeft.X + tglImageFacingLeft.Width + gv.scaler, tglImageFacingLeft.Y, "gy");
                    gv.DrawText(c, "Left", tglImageFacingLeft.X + tglImageFacingLeft.Width + gv.scaler, tglImageFacingLeft.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.isShown) { tglIsShown.toggleOn = true; }
                    else { tglIsShown.toggleOn = false; }
                    tglIsShown.Draw(c);
                    gv.DrawText(c, "Is", tglIsShown.X + tglIsShown.Width + gv.scaler, tglIsShown.Y, "gy");
                    gv.DrawText(c, "Shown", tglIsShown.X + tglIsShown.Width + gv.scaler, tglIsShown.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.HideImageAfterEnc) { tglHideAfterEnc.toggleOn = true; }
                    else { tglHideAfterEnc.toggleOn = false; }
                    tglHideAfterEnc.Draw(c);
                    gv.DrawText(c, "Hide Image", tglHideAfterEnc.X + tglHideAfterEnc.Width + gv.scaler, tglHideAfterEnc.Y, "gy");
                    gv.DrawText(c, "After Encounter", tglHideAfterEnc.X + tglHideAfterEnc.Width + gv.scaler, tglHideAfterEnc.Y + gv.fontHeight + gv.fontLineSpacing, "gy");
                    
                    gv.DrawText(c, "COMBAT ONLY:", tglNumberOfScriptCallsRemaining.X, tglNumberOfScriptCallsRemaining.Y - gv.fontHeight - gv.fontLineSpacing, "gn");

                    tglNumberOfScriptCallsRemaining.Draw(c);
                    gv.DrawText(c, "Num of Calls", tglNumberOfScriptCallsRemaining.X + tglNumberOfScriptCallsRemaining.Width + gv.scaler, tglNumberOfScriptCallsRemaining.Y, "gy");
                    gv.DrawText(c, selectedTrigger.numberOfScriptCallsRemaining.ToString(), tglNumberOfScriptCallsRemaining.X + tglNumberOfScriptCallsRemaining.Width + gv.scaler, tglNumberOfScriptCallsRemaining.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.canBeTriggeredByPc) { tglCanBeTriggeredByPc.toggleOn = true; }
                    else { tglCanBeTriggeredByPc.toggleOn = false; }
                    tglCanBeTriggeredByPc.Draw(c);
                    gv.DrawText(c, "Triggered", tglCanBeTriggeredByPc.X + tglCanBeTriggeredByPc.Width + gv.scaler, tglCanBeTriggeredByPc.Y, "gy");
                    gv.DrawText(c, "by PC", tglCanBeTriggeredByPc.X + tglCanBeTriggeredByPc.Width + gv.scaler, tglCanBeTriggeredByPc.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.canBeTriggeredByCreature) { tglCanBeTriggeredByCreature.toggleOn = true; }
                    else { tglCanBeTriggeredByCreature.toggleOn = false; }
                    tglCanBeTriggeredByCreature.Draw(c);
                    gv.DrawText(c, "Triggered", tglCanBeTriggeredByCreature.X + tglCanBeTriggeredByCreature.Width + gv.scaler, tglCanBeTriggeredByCreature.Y, "gy");
                    gv.DrawText(c, "by Creatures", tglCanBeTriggeredByCreature.X + tglCanBeTriggeredByCreature.Width + gv.scaler, tglCanBeTriggeredByCreature.Y + gv.fontHeight + gv.fontLineSpacing, "gy");
                }
                if (rbtnEvent1Properties.toggleOn)
                {
                    if (selectedTrigger.EnabledEvent1) { tglEnabledEvent.toggleOn = true; }
                    else { tglEnabledEvent.toggleOn = false; }
                    tglEnabledEvent.Draw(c);
                    gv.DrawText(c, "Event", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y, "gy");
                    gv.DrawText(c, "Enabled", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.DoOnceOnlyEvent1) { tglDoOnceOnlyEvent.toggleOn = true; }
                    else { tglDoOnceOnlyEvent.toggleOn = false; }
                    tglDoOnceOnlyEvent.Draw(c);
                    gv.DrawText(c, "Do Event", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y, "gy");
                    gv.DrawText(c, "Once Only", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    tglEventType.Draw(c);
                    gv.DrawText(c, "TYPE:", tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y, "gy");
                    gv.DrawText(c, selectedTrigger.Event1Type, tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    tglEventFilenameOrTag.Draw(c);
                    gv.DrawText(c, "Filename/tag", tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y, "gy");
                    gv.DrawText(c, selectedTrigger.Event1FilenameOrTag, tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.Event1Type.Equals("transition"))
                    {
                        tglEventTransPointX.Draw(c);
                        gv.DrawText(c, "Loc X:", tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event1TransPointX.ToString(), tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventTransPointY.Draw(c);
                        gv.DrawText(c, "Loc Y:", tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event1TransPointY.ToString(), tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }

                    if (selectedTrigger.Event1Type.Equals("script"))
                    {
                        tglEventParm1.Draw(c);
                        gv.DrawText(c, "Parm 1:", tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event1Parm1, tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm2.Draw(c);
                        gv.DrawText(c, "Parm 2:", tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event1Parm2, tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm3.Draw(c);
                        gv.DrawText(c, "Parm 3:", tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event1Parm3, tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm4.Draw(c);
                        gv.DrawText(c, "Parm 4:", tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event1Parm4, tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }
                }
                if (rbtnEvent2Properties.toggleOn)
                {
                    if (selectedTrigger.EnabledEvent2) { tglEnabledEvent.toggleOn = true; }
                    else { tglEnabledEvent.toggleOn = false; }
                    tglEnabledEvent.Draw(c);
                    gv.DrawText(c, "Event", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y, "gy");
                    gv.DrawText(c, "Enabled", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.DoOnceOnlyEvent2) { tglDoOnceOnlyEvent.toggleOn = true; }
                    else { tglDoOnceOnlyEvent.toggleOn = false; }
                    tglDoOnceOnlyEvent.Draw(c);
                    gv.DrawText(c, "Do Event", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y, "gy");
                    gv.DrawText(c, "Once Only", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    tglEventType.Draw(c);
                    gv.DrawText(c, "TYPE:", tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y, "gy");
                    gv.DrawText(c, selectedTrigger.Event2Type, tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    tglEventFilenameOrTag.Draw(c);
                    gv.DrawText(c, "Filename/tag", tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y, "gy");
                    gv.DrawText(c, selectedTrigger.Event2FilenameOrTag, tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.Event2Type.Equals("transition"))
                    {
                        tglEventTransPointX.Draw(c);
                        gv.DrawText(c, "Loc X:", tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event2TransPointX.ToString(), tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventTransPointY.Draw(c);
                        gv.DrawText(c, "Loc Y:", tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event2TransPointY.ToString(), tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }

                    if (selectedTrigger.Event2Type.Equals("script"))
                    {
                        tglEventParm1.Draw(c);
                        gv.DrawText(c, "Parm 1:", tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event2Parm1, tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm2.Draw(c);
                        gv.DrawText(c, "Parm 2:", tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event2Parm2, tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm3.Draw(c);
                        gv.DrawText(c, "Parm 3:", tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event2Parm3, tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm4.Draw(c);
                        gv.DrawText(c, "Parm 4:", tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event2Parm4, tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }
                }
                if (rbtnEvent3Properties.toggleOn)
                {
                    if (selectedTrigger.EnabledEvent3) { tglEnabledEvent.toggleOn = true; }
                    else { tglEnabledEvent.toggleOn = false; }
                    tglEnabledEvent.Draw(c);
                    gv.DrawText(c, "Event", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y, "gy");
                    gv.DrawText(c, "Enabled", tglEnabledEvent.X + tglEnabledEvent.Width + gv.scaler, tglEnabledEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    if (selectedTrigger.DoOnceOnlyEvent3) { tglDoOnceOnlyEvent.toggleOn = true; }
                    else { tglDoOnceOnlyEvent.toggleOn = false; }
                    tglDoOnceOnlyEvent.Draw(c);
                    gv.DrawText(c, "Do Event", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y, "gy");
                    gv.DrawText(c, "Once Only", tglDoOnceOnlyEvent.X + tglDoOnceOnlyEvent.Width + gv.scaler, tglDoOnceOnlyEvent.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                    tglEventType.Draw(c);
                    gv.DrawText(c, "TYPE:", tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y, "gy");
                    gv.DrawText(c, selectedTrigger.Event3Type, tglEventType.X + tglEventType.Width + gv.scaler, tglEventType.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    tglEventFilenameOrTag.Draw(c);
                    gv.DrawText(c, "Filename/tag", tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y, "gy");
                    gv.DrawText(c, selectedTrigger.Event3FilenameOrTag, tglEventFilenameOrTag.X + tglEventFilenameOrTag.Width + gv.scaler, tglEventFilenameOrTag.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                    if (selectedTrigger.Event3Type.Equals("transition"))
                    {
                        tglEventTransPointX.Draw(c);
                        gv.DrawText(c, "Loc X:", tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event3TransPointX.ToString(), tglEventTransPointX.X + tglEventTransPointX.Width + gv.scaler, tglEventTransPointX.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventTransPointY.Draw(c);
                        gv.DrawText(c, "Loc Y:", tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event3TransPointY.ToString(), tglEventTransPointY.X + tglEventTransPointY.Width + gv.scaler, tglEventTransPointY.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }

                    if (selectedTrigger.Event3Type.Equals("script"))
                    {
                        tglEventParm1.Draw(c);
                        gv.DrawText(c, "Parm 1:", tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event3Parm1, tglEventParm1.X + tglEventParm1.Width + gv.scaler, tglEventParm1.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm2.Draw(c);
                        gv.DrawText(c, "Parm 2:", tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event3Parm2, tglEventParm2.X + tglEventParm2.Width + gv.scaler, tglEventParm2.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm3.Draw(c);
                        gv.DrawText(c, "Parm 3:", tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event3Parm3, tglEventParm3.X + tglEventParm3.Width + gv.scaler, tglEventParm3.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                        tglEventParm4.Draw(c);
                        gv.DrawText(c, "Parm 4:", tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y, "gy");
                        gv.DrawText(c, selectedTrigger.Event3Parm4, tglEventParm4.X + tglEventParm4.Width + gv.scaler, tglEventParm4.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
                    }
                }
            }

            //draw selected info tile highlight
            int tlX = (int)((selectedSquare.X - upperSquareX) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
            int tlY = (int)((selectedSquare.Y - upperSquareY) * gv.squareSize / mapSquareSizeScaler * gv.scaler);
            int brX = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
            int brY = (int)(gv.squareSize / mapSquareSizeScaler * gv.scaler);
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
            dst = new IbRect(tlX + mapStartLocXinPixels, tlY, brX, brY);
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("highlight_magenta"), src, dst);

            //rbtnEditTrigger.Draw();
            //gv.DrawText("Edit Existing", rbtnEditTrigger.X + rbtnEditTrigger.Width + gv.scaler, rbtnEditTrigger.Y, "wh");
            //gv.DrawText("Trigger", rbtnEditTrigger.X + rbtnEditTrigger.Width + gv.scaler, rbtnEditTrigger.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            //gv.DrawText("select one of", panelLeftLocation, panelTopLocation + (14 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            //gv.DrawText("the above actions", panelLeftLocation, panelTopLocation + (15 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            //gv.DrawText("then tap on a map", panelLeftLocation, panelTopLocation + (16 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            //gv.DrawText("square to make", panelLeftLocation, panelTopLocation + (17 * (gv.fontHeight + gv.fontLineSpacing)), "gn");
            //gv.DrawText("the setting", panelLeftLocation, panelTopLocation + (18 * (gv.fontHeight + gv.fontLineSpacing)), "gn");

            btnHelp.Draw(c);
        }
        public void drawWalkLoSPanel(SKCanvas c)
        {
            //Description     
            gv.DrawText(c, "WALKABLE", panelLeftLocation, panelTopLocation + gv.fontHeight + gv.fontLineSpacing, "gn");
            gv.DrawText(c, "LINE-OF-SIGHT", panelLeftLocation, panelTopLocation + (2 * (gv.fontHeight + gv.fontLineSpacing)), "gn");

            rbtnWalkOpen.Draw(c);
            gv.DrawText(c, "WALKABLE", rbtnWalkOpen.X + rbtnWalkOpen.Width + gv.scaler, rbtnWalkOpen.Y, "wh");
            gv.DrawText(c, "OPEN", rbtnWalkOpen.X + rbtnWalkOpen.Width + gv.scaler, rbtnWalkOpen.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            rbtnWalkBlocking.Draw(c);
            gv.DrawText(c, "WALKABLE", rbtnWalkBlocking.X + rbtnWalkBlocking.Width + gv.scaler, rbtnWalkBlocking.Y, "wh");
            gv.DrawText(c, "BLOCK", rbtnWalkBlocking.X + rbtnWalkBlocking.Width + gv.scaler, rbtnWalkBlocking.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            rbtnSightOpen.Draw(c);
            gv.DrawText(c, "Line-of-Sight", rbtnSightOpen.X + rbtnSightOpen.Width + gv.scaler, rbtnSightOpen.Y, "wh");
            gv.DrawText(c, "Visible", rbtnSightOpen.X + rbtnSightOpen.Width + gv.scaler, rbtnSightOpen.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            rbtnSightBlocking.Draw(c);
            gv.DrawText(c, "Line-of-Sight", rbtnSightBlocking.X + rbtnSightBlocking.Width + gv.scaler, rbtnSightBlocking.Y, "wh");
            gv.DrawText(c, "Blocked", rbtnSightBlocking.X + rbtnSightBlocking.Width + gv.scaler, rbtnSightBlocking.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            gv.DrawText(c, "MAP SIZE (NO UNDO)", panelLeftLocation, panelTopLocation + (14 * (gv.fontHeight + gv.fontLineSpacing)), "gn");

            btnPlusLeft.Draw(c);
            gv.DrawText(c, "X=" + gv.mod.currentArea.MapSizeX, btnPlusLeft.X + btnPlusLeft.Width + gv.scaler, btnPlusLeft.Y + btnPlusLeft.Height / 4, "yl");
            btnMinusLeft.Draw(c);
            gv.DrawText(c, "Y=" + gv.mod.currentArea.MapSizeY, btnMinusLeft.X + btnMinusLeft.Width + gv.scaler, btnMinusLeft.Y + btnMinusLeft.Height / 4, "yl");
            btnPlusRight.Draw(c);
            btnMinusRight.Draw(c);
            btnPlusTop.Draw(c);
            btnMinusTop.Draw(c);
            btnPlusBottom.Draw(c);
            btnMinusBottom.Draw(c);

        }
        public void drawPropsPanel(SKCanvas c)
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
            description.onDrawTextBox(c);

            btnHelp.Draw(c);
        }
        public void drawSettingsPanel(SKCanvas c)
        {            
            //Description     
            gv.DrawText(c, "AREA SETTINGS", panelLeftLocation, panelTopLocation, "gn");

            int shiftForFont = (tglSettingAreaName.Height / 2) - (gv.fontHeight / 2);
            tglSettingAreaName.Draw(c);
            gv.DrawText(c, "Area Name:", tglSettingAreaName.X + tglSettingAreaName.Width + gv.scaler, tglSettingAreaName.Y, "yl");
            gv.DrawText(c, gv.mod.currentArea.Filename, tglSettingAreaName.X + tglSettingAreaName.Width + gv.scaler, tglSettingAreaName.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            tglSettingInGameAreaName.Draw(c);
            gv.DrawText(c, "In-Game Name:", tglSettingInGameAreaName.X + tglSettingInGameAreaName.Width + gv.scaler, tglSettingInGameAreaName.Y, "yl");
            gv.DrawText(c, gv.mod.currentArea.inGameAreaName, tglSettingInGameAreaName.X + tglSettingInGameAreaName.Width + gv.scaler, tglSettingInGameAreaName.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            tglAreaMusic.Draw(c);
            gv.DrawText(c, "Area Music:", tglAreaMusic.X + tglAreaMusic.Width + gv.scaler, tglAreaMusic.Y, "yl");
            gv.DrawText(c, gv.mod.currentArea.AreaMusic, tglAreaMusic.X + tglAreaMusic.Width + gv.scaler, tglAreaMusic.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            tglSettingAreaVisibleDistance.Draw(c);
            gv.DrawText(c, "Visible", tglSettingAreaVisibleDistance.X + tglSettingAreaVisibleDistance.Width + gv.scaler, tglSettingAreaVisibleDistance.Y, "wh");
            gv.DrawText(c, "Distance: " + gv.mod.currentArea.AreaVisibleDistance, tglSettingAreaVisibleDistance.X + tglSettingAreaVisibleDistance.Width + gv.scaler, tglSettingAreaVisibleDistance.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            tglSettingTimePerSquare.Draw(c);
            gv.DrawText(c, "Time Per", tglSettingTimePerSquare.X + tglSettingTimePerSquare.Width + gv.scaler, tglSettingTimePerSquare.Y, "wh");
            gv.DrawText(c, "Square: " + gv.mod.currentArea.TimePerSquare, tglSettingTimePerSquare.X + tglSettingTimePerSquare.Width + gv.scaler, tglSettingTimePerSquare.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            if (gv.mod.currentArea.Is3dArea) { tglSettingIs3dArea.toggleOn = true; }
            else { tglSettingIs3dArea.toggleOn = false; }
            tglSettingIs3dArea.Draw(c);
            gv.DrawText(c, "Is 3D", tglSettingIs3dArea.X + tglSettingIs3dArea.Width + gv.scaler, tglSettingIs3dArea.Y, "rd");
            gv.DrawText(c, "View", tglSettingIs3dArea.X + tglSettingIs3dArea.Width + gv.scaler, tglSettingIs3dArea.Y + gv.fontHeight + gv.fontLineSpacing, "rd");

            if (gv.mod.currentArea.RestingAllowed) { tglSettingRestingAllowed.toggleOn = true; }
            else { tglSettingRestingAllowed.toggleOn = false; }
            tglSettingRestingAllowed.Draw(c);
            gv.DrawText(c, "Resting", tglSettingRestingAllowed.X + tglSettingRestingAllowed.Width + gv.scaler, tglSettingRestingAllowed.Y, "wh");
            gv.DrawText(c, "Allowed", tglSettingRestingAllowed.X + tglSettingRestingAllowed.Width + gv.scaler, tglSettingRestingAllowed.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            if (gv.mod.currentArea.UseMiniMapFogOfWar) { tglSettingUseMiniMapFogOfWar.toggleOn = true; }
            else { tglSettingUseMiniMapFogOfWar.toggleOn = false; }
            tglSettingUseMiniMapFogOfWar.Draw(c);
            gv.DrawText(c, "Use Mini Map", tglSettingUseMiniMapFogOfWar.X + tglSettingUseMiniMapFogOfWar.Width + gv.scaler, tglSettingUseMiniMapFogOfWar.Y, "wh");
            gv.DrawText(c, "Fog-of-War", tglSettingUseMiniMapFogOfWar.X + tglSettingUseMiniMapFogOfWar.Width + gv.scaler, tglSettingUseMiniMapFogOfWar.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            if (gv.mod.currentArea.UseDayNightCycle) { tglSettingUseDayNightCycle.toggleOn = true; }
            else { tglSettingUseDayNightCycle.toggleOn = false; }
            tglSettingUseDayNightCycle.Draw(c);
            gv.DrawText(c, "Use Day/Night", tglSettingUseDayNightCycle.X + tglSettingUseDayNightCycle.Width + gv.scaler, tglSettingUseDayNightCycle.Y, "wh");
            gv.DrawText(c, "Cycle", tglSettingUseDayNightCycle.X + tglSettingUseDayNightCycle.Width + gv.scaler, tglSettingUseDayNightCycle.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

            btnHelp.Draw(c);
        }
        public void draw3DPreviewPanel(SKCanvas c)
        {
            //Description     
            gv.DrawText(c, "3D VIEW PREVIEW", panelLeftLocation, panelTopLocation, "gn");

            btnMoveForward.Draw(c);
            btnTurnLeft.Draw(c);
            btnTurnRight.Draw(c);
            btnHelp.Draw(c);
        }

        public void onTouchTsAreaEditor(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnInfo.glowOn = false;
            btnTiles.glowOn = false;
            btnTriggers.glowOn = false;
            btnWalkLoS.glowOn = false;
            btn3DPreview.glowOn = false;
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
            else if (currentMode.Equals("3DPreview"))
            {
                ret = onTouch3DPreviewPanel(eX, eY, eventType);
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
                    else if (btn3DPreview.getImpact(x, y))
                    {
                        btn3DPreview.glowOn = true;
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
                    btn3DPreview.glowOn = false;
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
                    else if (btn3DPreview.getImpact(x, y))
                    {
                        selectedSquare.X = 0;
                        selectedSquare.Y = 0;
                        currentMode = "3DPreview";
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
                        int gridX = ((eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler))) + upperSquareX;
                        int gridY = (eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler))) + upperSquareY;
                        if ((tapInMapArea(gridX, gridY)) && (tapInMapViewport(x, y)))
                        {
                            if (rbtnEditLayer1.toggleOn)
                            {
                                gv.mod.currentArea.Layer1Filename[gridY * gv.mod.currentArea.MapSizeX + gridX] = currentTile;
                            }
                            else if (rbtnEditLayer2.toggleOn)
                            {
                                gv.mod.currentArea.Layer2Filename[gridY * gv.mod.currentArea.MapSizeX + gridX] = currentTile;
                            }
                            else if (rbtnEditLayer3.toggleOn)
                            {
                                gv.mod.currentArea.Layer3Filename[gridY * gv.mod.currentArea.MapSizeX + gridX] = currentTile;
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
                            int xloc = (int)((x - panelLeftLocation) / (gv.squareSize * gv.scaler));
                            int yloc = (int)((y - panelTopLocation) / (gv.squareSize * gv.scaler));
                            if (gv.mod.currentArea.Is3dArea)
                            {
                                if (rbtnEditLayer1.toggleOn)
                                {
                                    if ((((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage) < backdrops.Count)
                                    {
                                        tileSlotIndex = (xloc * (tileSlotsPerPage / 4)) + yloc;
                                        currentTile = backdrops[((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage];
                                    }
                                }
                                else if (rbtnEditLayer2.toggleOn)
                                {
                                    if ((((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage) < wallsets.Count)
                                    {
                                        tileSlotIndex = (xloc * (tileSlotsPerPage / 4)) + yloc;
                                        currentTile = wallsets[((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage];
                                    }
                                }
                                else if (rbtnEditLayer3.toggleOn)
                                {
                                    if ((((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage) < overlays.Count)
                                    {
                                        tileSlotIndex = (xloc * (tileSlotsPerPage / 4)) + yloc;
                                        currentTile = overlays[((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage];
                                    }
                                }
                            }
                            else
                            {
                                if (tglMiscTiles.toggleOn)
                                {
                                    if ((((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage) < tilesMiscList.Count)
                                    {
                                        tileSlotIndex = (xloc * (tileSlotsPerPage / 4)) + yloc;
                                        currentTile = tilesMiscList[((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage];
                                    }
                                }
                                else if (tglWallFloorTiles.toggleOn)
                                {
                                    if ((((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage) < tilesWallFloorList.Count)
                                    {
                                        tileSlotIndex = (xloc * (tileSlotsPerPage / 4)) + yloc;
                                        currentTile = tilesWallFloorList[((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage];
                                    }
                                }
                                else if (tglPropTiles.toggleOn)
                                {
                                    if ((((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage) < tilesPropList.Count)
                                    {
                                        tileSlotIndex = (xloc * (tileSlotsPerPage / 4)) + yloc;
                                        currentTile = tilesPropList[((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage];
                                    }
                                }
                                else if (tglUserTiles.toggleOn)
                                {
                                    if ((((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage) < tilesUserList.Count)
                                    {
                                        tileSlotIndex = (xloc * (tileSlotsPerPage / 4)) + yloc;
                                        currentTile = tilesUserList[((xloc * (tileSlotsPerPage / 4)) + yloc) + (tilesPageIndex) * tileSlotsPerPage];
                                    }
                                }
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
                        if (gv.mod.currentArea.Is3dArea)
                        {
                            if (rbtnEditLayer1.toggleOn)
                            {
                                if (tilesPageIndex <= (backdrops.Count / tileSlotsPerPage) - 1)
                                {
                                    tilesPageIndex++;
                                    btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                                }
                            }
                            else if (rbtnEditLayer2.toggleOn)
                            {
                                if (tilesPageIndex <= (wallsets.Count / tileSlotsPerPage) - 1)
                                {
                                    tilesPageIndex++;
                                    btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                                }
                            }
                            else if (rbtnEditLayer3.toggleOn)
                            {
                                if (tilesPageIndex <= (overlays.Count / tileSlotsPerPage) - 1)
                                {
                                    tilesPageIndex++;
                                    btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                                }
                            }
                        }
                        else
                        {
                            if (tglMiscTiles.toggleOn)
                            {
                                if (tilesPageIndex <= (tilesMiscList.Count / tileSlotsPerPage) - 1)
                                {
                                    tilesPageIndex++;
                                    btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                                }
                            }
                            else if (tglWallFloorTiles.toggleOn)
                            {
                                if (tilesPageIndex <= (tilesWallFloorList.Count / tileSlotsPerPage) - 1)
                                {
                                    tilesPageIndex++;
                                    btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                                }
                            }
                            else if (tglPropTiles.toggleOn)
                            {
                                if (tilesPageIndex <= (tilesPropList.Count / tileSlotsPerPage) - 1)
                                {
                                    tilesPageIndex++;
                                    btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                                }
                            }
                            else if (tglUserTiles.toggleOn)
                            {
                                if (tilesPageIndex <= (tilesUserList.Count / tileSlotsPerPage) - 1)
                                {
                                    tilesPageIndex++;
                                    btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                                }
                            }
                        }                        
                        return true;
                    }
                    else if (tglMiscTiles.getImpact(x, y))
                    {
                        if (!gv.mod.currentArea.Is3dArea)
                        {
                            tglMiscTiles.toggleOn = true;
                            tglWallFloorTiles.toggleOn = false;
                            tglPropTiles.toggleOn = false;
                            tglUserTiles.toggleOn = false;
                            tilesPageIndex = 0;
                            tileSlotIndex = 0;
                            btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                        }
                    }
                    else if (tglWallFloorTiles.getImpact(x, y))
                    {
                        if (!gv.mod.currentArea.Is3dArea)
                        {
                            tglMiscTiles.toggleOn = false;
                            tglWallFloorTiles.toggleOn = true;
                            tglPropTiles.toggleOn = false;
                            tglUserTiles.toggleOn = false;
                            tilesPageIndex = 0;
                            tileSlotIndex = 0;
                            btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                        }
                    }
                    else if (tglPropTiles.getImpact(x, y))
                    {
                        if (!gv.mod.currentArea.Is3dArea)
                        {
                            tglMiscTiles.toggleOn = false;
                            tglWallFloorTiles.toggleOn = false;
                            tglPropTiles.toggleOn = true;
                            tglUserTiles.toggleOn = false;
                            tilesPageIndex = 0;
                            tileSlotIndex = 0;
                            btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                        }
                    }
                    else if (tglUserTiles.getImpact(x, y))
                    {
                        if (!gv.mod.currentArea.Is3dArea)
                        {
                            tglMiscTiles.toggleOn = false;
                            tglWallFloorTiles.toggleOn = false;
                            tglPropTiles.toggleOn = false;
                            tglUserTiles.toggleOn = true;
                            tilesPageIndex = 0;
                            tileSlotIndex = 0;
                            btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                        }
                    }
                    else if (rbtnShowLayer1.getImpact(x,y))
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
                        if (gv.mod.currentArea.Is3dArea)
                        {
                            tileSlotIndex = 0;
                            tilesPageIndex = 0;
                            currentTile = backdrops[0];
                            btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                        }
                    }
                    else if (rbtnEditLayer2.getImpact(x, y))
                    {
                        rbtnEditLayer1.toggleOn = false;
                        rbtnEditLayer2.toggleOn = true;
                        rbtnEditLayer3.toggleOn = false;
                        if (gv.mod.currentArea.Is3dArea)
                        {
                            tileSlotIndex = 0;
                            tilesPageIndex = 0;
                            currentTile = wallsets[0];
                            btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                        }
                    }
                    else if (rbtnEditLayer3.getImpact(x, y))
                    {
                        rbtnEditLayer1.toggleOn = false;
                        rbtnEditLayer2.toggleOn = false;
                        rbtnEditLayer3.toggleOn = true;
                        if (gv.mod.currentArea.Is3dArea)
                        {
                            tileSlotIndex = 0;
                            tilesPageIndex = 0;
                            currentTile = overlays[0];
                            btnTilesPageIndex.Text = (tilesPageIndex + 1) + "";
                        }
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

                    if (tglSettingAreaName.getImpact(x, y))
                    {
                        changeAreaName();
                    }
                    else if (tglSettingInGameAreaName.getImpact(x, y))
                    {
                        changeAreaInGameName();
                    }
                    else if (tglAreaMusic.getImpact(x, y))
                    {
                        changeAreaMusic();
                    }
                    else if (tglSettingAreaVisibleDistance.getImpact(x, y))
                    {
                        changeAreaVisibleDistance();
                    }
                    else if (tglSettingTimePerSquare.getImpact(x, y))
                    {
                        changeTimePerSquare();
                    }
                    else if (tglSettingIs3dArea.getImpact(x, y))
                    {
                        tglSettingIs3dArea.toggleOn = !tglSettingIs3dArea.toggleOn;
                        gv.mod.currentArea.Is3dArea = tglSettingIs3dArea.toggleOn;
                    }
                    else if (tglSettingRestingAllowed.getImpact(x, y))
                    {
                        tglSettingRestingAllowed.toggleOn = !tglSettingRestingAllowed.toggleOn;
                        gv.mod.currentArea.RestingAllowed = tglSettingRestingAllowed.toggleOn;
                    }
                    else if (tglSettingUseMiniMapFogOfWar.getImpact(x, y))
                    {
                        tglSettingUseMiniMapFogOfWar.toggleOn = !tglSettingUseMiniMapFogOfWar.toggleOn;
                        gv.mod.currentArea.UseMiniMapFogOfWar = tglSettingUseMiniMapFogOfWar.toggleOn;
                    }
                    else if (tglSettingUseDayNightCycle.getImpact(x, y))
                    {
                        tglSettingUseDayNightCycle.toggleOn = !tglSettingUseDayNightCycle.toggleOn;
                        gv.mod.currentArea.UseDayNightCycle = tglSettingUseDayNightCycle.toggleOn;
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
                                        
                    if (tapInPreviewViewport(x, y))
                    {
                        int previewTop = panelTopLocation + (int)(7 * gv.squareSize * gv.scaler);
                        int previewLeft = panelLeftLocation;
                        int previewSquareSize = (int)(gv.squareSize * gv.scaler * 3 / gv.mod.currentArea.MapSizeX);
                        if (previewSquareSize > (int)(gv.squareSize * gv.scaler * 3 / gv.mod.currentArea.MapSizeY))
                        {
                            previewSquareSize = (int)(gv.squareSize * gv.scaler * 3 / gv.mod.currentArea.MapSizeY);
                        }
                        int panSqrSize = 10 * mapSquareSizeScaler;

                        upperSquareX = ((x - previewLeft) / previewSquareSize) - (panSqrSize / 2);
                        upperSquareY = ((y - previewTop) / previewSquareSize) - (panSqrSize / 2);

                        if (upperSquareX > (gv.mod.currentArea.MapSizeX - panSqrSize)) { upperSquareX = gv.mod.currentArea.MapSizeX - panSqrSize; }
                        if (upperSquareX < 0) { upperSquareX = 0; }
                        if (upperSquareY > (gv.mod.currentArea.MapSizeY - panSqrSize)) { upperSquareY = gv.mod.currentArea.MapSizeY - panSqrSize; }
                        if (upperSquareY < 0) { upperSquareY = 0; }
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

                    //figure out if tapped on a map square
                    int gridX = ((eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler))) + upperSquareX;
                    int gridY = (eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler))) + upperSquareY;
                    if ((tapInMapArea(gridX, gridY)) && (tapInMapViewport(x, y)))
                    {
                        selectedSquare.X = gridX;
                        selectedSquare.Y = gridY;
                    }

                    else if (rbtnZoom1.getImpact(x, y))
                    {
                        rbtnZoom1.toggleOn = true;
                        rbtnZoom2.toggleOn = false;
                        upperSquareX = 0;
                        upperSquareY = 0;
                        selectedSquare.X = 0;
                        selectedSquare.Y = 0;
                        mapSquareSizeScaler = 1;
                        
                    }
                    else if (rbtnZoom2.getImpact(x, y))
                    {
                        rbtnZoom1.toggleOn = false;
                        rbtnZoom2.toggleOn = true;
                        upperSquareX = 0;
                        upperSquareY = 0;
                        selectedSquare.X = 0;
                        selectedSquare.Y = 0;
                        mapSquareSizeScaler = 2;
                        
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
                        int gridX = ((eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler))) + upperSquareX;
                        int gridY = (eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler))) + upperSquareY;
                        if ((tapInMapArea(gridX, gridY)) && (tapInMapViewport(x, y)))
                        {
                            if (rbtnWalkOpen.toggleOn)
                            {
                                gv.mod.currentArea.Walkable[gridY * gv.mod.currentArea.MapSizeX + gridX] = 1;
                            }
                            else if (rbtnWalkBlocking.toggleOn)
                            {
                                gv.mod.currentArea.Walkable[gridY * gv.mod.currentArea.MapSizeX + gridX] = 0;
                            }
                            else if (rbtnSightOpen.toggleOn)
                            {
                                gv.mod.currentArea.LoSBlocked[gridY * gv.mod.currentArea.MapSizeX + gridX] = 0;
                            }
                            else if (rbtnSightBlocking.toggleOn)
                            {
                                gv.mod.currentArea.LoSBlocked[gridY * gv.mod.currentArea.MapSizeX + gridX] = 1;
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
                    else if (btnPlusLeft.getImpact(x, y))
                    {
                        btnPlusLeftX_Click();
                    }
                    else if (btnMinusLeft.getImpact(x, y))
                    {
                        btnMinusLeftX_Click();
                    }
                    else if (btnPlusRight.getImpact(x, y))
                    {
                        btnPlusRightX_Click();
                    }
                    else if (btnMinusRight.getImpact(x, y))
                    {
                        btnMinusRightX_Click();
                    }
                    else if (btnPlusTop.getImpact(x, y))
                    {
                        btnPlusTopY_Click();
                    }
                    else if (btnMinusTop.getImpact(x, y))
                    {
                        btnMinusTopY_Click();
                    }
                    else if (btnPlusBottom.getImpact(x, y))
                    {
                        btnPlusBottomY_Click();
                    }
                    else if (btnMinusBottom.getImpact(x, y))
                    {
                        btnMinusBottomY_Click();
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
                    int gridX = ((eX - mapStartLocXinPixels) / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler))) + upperSquareX;
                    int gridY = (eY / (int)(gv.squareSize * ((float)gv.scaler / (float)mapSquareSizeScaler))) + upperSquareY;
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
                            selectedTrigger = gv.mod.currentArea.getTriggerByLocation(gridX, gridY);
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
                                    gv.mod.currentArea.Triggers.Add(newTrigger);
                                    selectedTrigger = newTrigger;
                                }
                            }
                            else
                            {
                                //add the selected square to the squareList if doesn't already exist                                    
                                //check: if click square already exists, then erase from list                            
                                Trigger newTrigger = gv.mod.currentArea.getTriggerByTag(selectedTrigger.TriggerTag);
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
                            Trigger newTrigger = gv.mod.currentArea.getTriggerByTag(selectedTrigger.TriggerTag);
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
                                    gv.screenTokenSelector.resetTokenSelector("tsAreaEditor", null);
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
                            else if (tglHideAfterEnc.getImpact(x, y))
                            {
                                if (selectedTrigger != null)
                                {
                                    tglHideAfterEnc.toggleOn = !tglHideAfterEnc.toggleOn;
                                    selectedTrigger.HideImageAfterEnc = tglHideAfterEnc.toggleOn;
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
        public bool onTouch3DPreviewPanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnHelp.glowOn = false;
            btnTurnLeft.glowOn = false;
            btnTurnRight.glowOn = false;
            btnMoveForward.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (btnTurnLeft.getImpact(x, y))
                    {
                        btnTurnLeft.glowOn = true;
                        return true;
                    }
                    else if (btnTurnRight.getImpact(x, y))
                    {
                        btnTurnRight.glowOn = true;
                        return true;
                    }
                    else if (btnMoveForward.getImpact(x, y))
                    {
                        btnMoveForward.glowOn = true;
                        return true;
                    }
                    else if (btnHelp.getImpact(x, y))
                    {
                        btnHelp.glowOn = true;
                        return true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    //figure out if tapped on a map square
                    int shiftY = panelTopLocation + gv.fontHeight + gv.fontLineSpacing;
                    int shiftX = panelLeftLocation - (gv.squareSize / 1);
                    int gridX = ((eX - shiftX) / (int)(gv.squareSize * ((float)gv.scaler / ((float)2 * 2))));
                    int gridY = ((eY - shiftY) / (int)(gv.squareSize * ((float)gv.scaler / ((float)2 * 2))));
                    if ((tapInMapArea(gridX, gridY)) && (tapInMiniMapViewport(x, y)))
                    {
                        PlayerLocationX = gridX;
                        PlayerLocationY = gridY;
                    }

                    btnHelp.glowOn = false;
                    btnTurnLeft.glowOn = false;
                    btnTurnRight.glowOn = false;
                    btnMoveForward.glowOn = false;

                    if (btnMoveForward.getImpact(x, y))
                    {
                        moveUp();
                    }
                    else if (btnTurnLeft.getImpact(x, y))
                    {
                        moveLeft();
                    }
                    else if (btnTurnRight.getImpact(x, y))
                    {
                        moveRight();
                    }

                    break;
            }
            return false;
        }

        /*public void onKeyUp(Keys keyData)
        {
            if (moveDelay())
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
                else { }
            }            
        }*/
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
                PlayerFacingDirection--;
                if (PlayerFacingDirection < 0)
                {
                    PlayerFacingDirection = 3;
                }
            }            
        }
        private void moveRight()
        {
            if (gv.mod.currentArea.Is3dArea)
            {
                PlayerFacingDirection++;
                if (PlayerFacingDirection > 3)
                {
                    PlayerFacingDirection = 0;
                }
            }            
        }
        private void moveUp()
        {
            if (gv.mod.currentArea.Is3dArea)
            {
                if (PlayerFacingDirection == 0) //NORTH
                {
                    if (PlayerLocationY > 0)
                    {
                        if (gv.mod.currentArea.GetBlocked(PlayerLocationX, PlayerLocationY - 1) == false)
                        {
                            PlayerLocationY--;
                        }
                    }
                }
                else if (PlayerFacingDirection == 1) //EAST
                {
                    int mapwidth = gv.mod.currentArea.MapSizeX;
                    if (PlayerLocationX < (mapwidth - 1))
                    {
                        if (gv.mod.currentArea.GetBlocked(PlayerLocationX + 1, PlayerLocationY) == false)
                        {
                            PlayerLocationX++;
                        }
                    }
                }
                else if (PlayerFacingDirection == 2) //SOUTH
                {
                    int mapheight = gv.mod.currentArea.MapSizeY;
                    if (PlayerLocationY < (mapheight - 1))
                    {
                        if (gv.mod.currentArea.GetBlocked(PlayerLocationX, PlayerLocationY + 1) == false)
                        {
                            PlayerLocationY++;
                        }
                    }
                }
                else if (PlayerFacingDirection == 3) //WEST
                {
                    if (PlayerLocationX > 0)
                    {
                        if (gv.mod.currentArea.GetBlocked(PlayerLocationX - 1, PlayerLocationY) == false)
                        {
                            PlayerLocationX--;
                        }
                    }
                }
            }            
        }
                
        public bool squareUsedByOtherTrigger(int gridX, int gridY)
        {
            if (selectedTrigger == null) { return true; }
            foreach (Trigger trig in gv.mod.currentArea.Triggers)
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
            foreach (Trigger trig in gv.mod.currentArea.Triggers)
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
            if (gridX > gv.mod.currentArea.MapSizeX - 1) { return false; }
            if (gridY > gv.mod.currentArea.MapSizeY - 1) { return false; }
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
            int width = (int)(gv.mod.currentArea.MapSizeX * gv.squareSize / (mapSquareSizeScaler * 2) * gv.scaler);
            int height = (int)(gv.mod.currentArea.MapSizeY * gv.squareSize / (mapSquareSizeScaler * 2) * gv.scaler);
            int shiftY = panelTopLocation + gv.fontHeight + gv.fontLineSpacing;
            int shiftX = panelLeftLocation - (gv.squareSize / 1);
            if (x < shiftX) { return false; }
            if (y < shiftY) { return false; }
            if (x > shiftX + width) { return false; }
            if (y > shiftY + height) { return false; }
            return true;
        }
        public bool tapInPreviewViewport(int x, int y)
        {
            int previewTop = panelTopLocation + (int)(7 * gv.squareSize * gv.scaler);
            int previewLeft = panelLeftLocation;
            if (x < previewLeft) { return false; }
            if (y < previewTop) { return false; }
            if (x > previewLeft + (int)(gv.squareSize * gv.scaler * 3)) { return false; }
            if (y > previewTop + (int)(gv.squareSize * gv.scaler * 3)) { return false; }
            return true;
        }

        public async void changeAreaName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Change the Area Name:", gv.mod.currentArea.Filename);
            gv.mod.currentArea.Filename = myinput;
            gv.touchEnabled = true;
            /*using (TextInputDialog itSel = new TextInputDialog(gv, "Change the Area Name.", gv.mod.currentArea.Filename))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (itSel.textInput.Length > 0)
                    {
                        gv.mod.currentArea.Filename = itSel.textInput;
                    }
                    else
                    {
                        MessageBox.Show("Entering a blank name is not allowed");
                    }
                }
            }*/
        }
        public async void changeAreaInGameName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose an In-Game Area Name:", gv.mod.currentArea.inGameAreaName);
            gv.mod.currentArea.inGameAreaName = myinput;
            gv.touchEnabled = true;
            /*using (TextInputDialog itSel = new TextInputDialog(gv, "Choose an In-Game Area Name.", gv.mod.currentArea.inGameAreaName))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (itSel.textInput.Length > 0)
                    {
                        gv.mod.currentArea.inGameAreaName = itSel.textInput;
                    }
                    else
                    {
                        MessageBox.Show("Entering a blank name is not allowed");
                    }
                }
            }*/
        }
        public async void changeAreaMusic()
        {
            List<string> items = GetAreaMusicList();
            items.Insert(0, "none");
            items.Insert(0, "cancel");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select music for this area:");
            if (selected != "cancel")
            {
                gv.mod.currentArea.AreaMusic = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetAreaMusicList()
        {
            List<string> musicList = new List<string>();
            //MODULE SPECIFIC
            List<string> files = gv.GetAllFilesWithExtensionFromBothFolders("\\sounds", "\\modules\\" + gv.mod.moduleName, ".mp3");
            foreach (string f in files)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                musicList.Add(filenameNoExt);
            }
            return musicList;
        }
        public async void changeAreaVisibleDistance()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter Area Visible Distance for Fog-of-War on 2D maps (must be an integer):", gv.mod.currentArea.AreaVisibleDistance);
            gv.mod.currentArea.AreaVisibleDistance = myinput;
            gv.touchEnabled = true;
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Enter Area Visible Distance for Fog-of-War", gv.mod.currentArea.AreaVisibleDistance))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.currentArea.AreaVisibleDistance = itSel.numInput;
                }
            }*/
        }
        public async void changeTimePerSquare()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Time Elapse per Square Moved (must be an integer):", gv.mod.currentArea.TimePerSquare);
            gv.mod.currentArea.TimePerSquare = myinput;
            gv.touchEnabled = true;
            //string title = "Time Elapse per Square Moved";
            //gv.mod.currentArea.TimePerSquare = gv.DialogReturnInteger(title, gv.mod.currentArea.TimePerSquare);
        }
        public async void changeTriggerTag()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose a unique (must be unique) tag for this trigger:", selectedTrigger.TriggerTag);
            selectedTrigger.TriggerTag = myinput;
            gv.touchEnabled = true;
            /*if (selectedTrigger == null) { return; }
            using (TextInputDialog itSel = new TextInputDialog(gv, "Choose a unique (must be unique) tag for this trigger.", selectedTrigger.TriggerTag))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (itSel.textInput.Length > 0)
                    {
                        selectedTrigger.TriggerTag = itSel.textInput;
                    }
                    else
                    {
                        MessageBox.Show("Entering a blank tag is not allowed");
                    }
                }
            }*/
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
                    foreach(Container c in gv.mod.moduleContainersList)
                    {
                        types.Add(c.containerTag);
                    }
                }
                else if (selectedTrigger.Event1Type.Equals("transition"))
                {
                    List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".are");
                    foreach (string s in ret)
                    {
                        types.Add(s);
                    }
                }
                else if (selectedTrigger.Event1Type.Equals("conversation"))
                {
                    List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".dlg");
                    foreach (string s in ret)
                    {
                        types.Add(s);
                    }
                }
                else if (selectedTrigger.Event1Type.Equals("encounter"))
                {
                    List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".enc");
                    foreach (string s in ret)
                    {
                        types.Add(s);
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
                    List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".are");
                    foreach (string s in ret)
                    {
                        types.Add(s);
                    }
                }
                else if (selectedTrigger.Event2Type.Equals("conversation"))
                {
                    List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".dlg");
                    foreach (string s in ret)
                    {
                        types.Add(s);
                    }
                }
                else if (selectedTrigger.Event2Type.Equals("encounter"))
                {
                    List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".enc");
                    foreach (string s in ret)
                    {
                        types.Add(s);
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
                    List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".are");
                    foreach (string s in ret)
                    {
                        types.Add(s);
                    }
                }
                else if (selectedTrigger.Event3Type.Equals("conversation"))
                {
                    List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".dlg");
                    foreach (string s in ret)
                    {
                        types.Add(s);
                    }
                }
                else if (selectedTrigger.Event3Type.Equals("encounter"))
                {
                    List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".enc");
                    foreach (string s in ret)
                    {
                        types.Add(s);
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

        private void btnPlusLeftX_Click()
        {
            //y * area.MapSizeX + x
            int oldX = gv.mod.currentArea.MapSizeX;
            for (int i = gv.mod.currentArea.Layer1Filename.Count - oldX; i >= 0; i -= oldX)
            {
                //Tile newTile = new Tile();
                //gv.mod.currentArea.Tiles.Insert(i, newTile);
                gv.mod.currentArea.Layer1Filename.Insert(i, "t_f_grass");
                gv.mod.currentArea.Layer2Filename.Insert(i, "t_a_blank");
                gv.mod.currentArea.Layer3Filename.Insert(i, "t_a_blank");
                gv.mod.currentArea.Walkable.Insert(i, 1);
                gv.mod.currentArea.LoSBlocked.Insert(i, 0);
                gv.mod.currentArea.Visible.Insert(i, 0);
            }
            foreach (Trigger t in gv.mod.currentArea.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.X++;
                }
            }
            gv.mod.currentArea.MapSizeX++;
            //mapSizeChangeStuff();
        }
        private void btnMinusLeftX_Click()
        {
            //y * gv.mod.currentArea.MapSizeX + x
            int oldX = gv.mod.currentArea.MapSizeX;
            for (int i = gv.mod.currentArea.Layer1Filename.Count - oldX; i >= 0; i -= oldX)
            {
                //gv.mod.currentArea.Tiles.RemoveAt(i);
                gv.mod.currentArea.Layer1Filename.RemoveAt(i);
                gv.mod.currentArea.Layer2Filename.RemoveAt(i);
                gv.mod.currentArea.Layer3Filename.RemoveAt(i);
                gv.mod.currentArea.Walkable.RemoveAt(i);
                gv.mod.currentArea.LoSBlocked.RemoveAt(i);
                gv.mod.currentArea.Visible.RemoveAt(i);
            }
            foreach (Trigger t in gv.mod.currentArea.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.X--;
                }
            }
            gv.mod.currentArea.MapSizeX--;
            //mapSizeChangeStuff();
        }
        private void btnPlusRightX_Click()
        {
            //y * gv.mod.currentArea.MapSizeX + x
            int oldX = gv.mod.currentArea.MapSizeX;
            for (int i = gv.mod.currentArea.Layer1Filename.Count - 1; i >= 0; i -= oldX)
            {
                //Tile newTile = new Tile();
                //gv.mod.currentArea.Tiles.Insert(i + 1, newTile);
                gv.mod.currentArea.Layer1Filename.Insert(i + 1, "t_f_grass");
                gv.mod.currentArea.Layer2Filename.Insert(i + 1, "t_a_blank");
                gv.mod.currentArea.Layer3Filename.Insert(i + 1, "t_a_blank");
                gv.mod.currentArea.Walkable.Insert(i + 1, 1);
                gv.mod.currentArea.LoSBlocked.Insert(i + 1, 0);
                gv.mod.currentArea.Visible.Insert(i + 1, 0);
            }
            gv.mod.currentArea.MapSizeX++;
            //mapSizeChangeStuff();
        }
        private void btnMinusRightX_Click()
        {
            //y * gv.mod.currentArea.MapSizeX + x
            int oldX = gv.mod.currentArea.MapSizeX;
            for (int i = gv.mod.currentArea.Layer1Filename.Count - 1; i >= 0; i -= oldX)
            {
                //gv.mod.currentArea.Tiles.RemoveAt(i);
                gv.mod.currentArea.Layer1Filename.RemoveAt(i);
                gv.mod.currentArea.Layer2Filename.RemoveAt(i);
                gv.mod.currentArea.Layer3Filename.RemoveAt(i);
                gv.mod.currentArea.Walkable.RemoveAt(i);
                gv.mod.currentArea.LoSBlocked.RemoveAt(i);
                gv.mod.currentArea.Visible.RemoveAt(i);
            }
            gv.mod.currentArea.MapSizeX--;
            //mapSizeChangeStuff();
        }
        private void btnPlusTopY_Click()
        {
            //y * gv.mod.currentArea.MapSizeX + x
            for (int i = 0; i < gv.mod.currentArea.MapSizeX; i++)
            {
                //Tile newTile = new Tile();
                //gv.mod.currentArea.Tiles.Insert(0, newTile);
                gv.mod.currentArea.Layer1Filename.Insert(0, "t_f_grass");
                gv.mod.currentArea.Layer2Filename.Insert(0, "t_a_blank");
                gv.mod.currentArea.Layer3Filename.Insert(0, "t_a_blank");
                gv.mod.currentArea.Walkable.Insert(0, 1);
                gv.mod.currentArea.LoSBlocked.Insert(0, 0);
                gv.mod.currentArea.Visible.Insert(0, 0);
            }
            foreach (Trigger t in gv.mod.currentArea.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.Y++;
                }
            }
            gv.mod.currentArea.MapSizeY++;
            //mapSizeChangeStuff();
        }
        private void btnMinusTopY_Click()
        {
            //y * gv.mod.currentArea.MapSizeX + x
            for (int i = 0; i < gv.mod.currentArea.MapSizeX; i++)
            {
                //gv.mod.currentArea.Tiles.RemoveAt(0);
                gv.mod.currentArea.Layer1Filename.RemoveAt(0);
                gv.mod.currentArea.Layer2Filename.RemoveAt(0);
                gv.mod.currentArea.Layer3Filename.RemoveAt(0);
                gv.mod.currentArea.Walkable.RemoveAt(0);
                gv.mod.currentArea.LoSBlocked.RemoveAt(0);
                gv.mod.currentArea.Visible.RemoveAt(0);
            }
            foreach (Trigger t in gv.mod.currentArea.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.Y--;
                }
            }
            gv.mod.currentArea.MapSizeY--;
            //mapSizeChangeStuff();
        }
        private void btnPlusBottomY_Click()
        {
            //y * gv.mod.currentArea.MapSizeX + x
            for (int i = 0; i < gv.mod.currentArea.MapSizeX; i++)
            {
                //Tile newTile = new Tile();
                //gv.mod.currentArea.Tiles.Add(newTile);
                gv.mod.currentArea.Layer1Filename.Add("t_f_grass");
                gv.mod.currentArea.Layer2Filename.Add("t_a_blank");
                gv.mod.currentArea.Layer3Filename.Add("t_a_blank");
                gv.mod.currentArea.Walkable.Add(1);
                gv.mod.currentArea.LoSBlocked.Add(0);
                gv.mod.currentArea.Visible.Add(0);
            }
            gv.mod.currentArea.MapSizeY++;
            //mapSizeChangeStuff();
        }
        private void btnMinusBottomY_Click()
        {
            //y * gv.mod.currentArea.MapSizeX + x
            for (int i = 0; i < gv.mod.currentArea.MapSizeX; i++)
            {
                //gv.mod.currentArea.Tiles.RemoveAt(gv.mod.currentArea.Tiles.Count - 1);
                int total = gv.mod.currentArea.Visible.Count;
                gv.mod.currentArea.Layer1Filename.RemoveAt(total - 1);
                gv.mod.currentArea.Layer2Filename.RemoveAt(total - 1);
                gv.mod.currentArea.Layer3Filename.RemoveAt(total - 1);
                gv.mod.currentArea.Walkable.RemoveAt(total - 1);
                gv.mod.currentArea.LoSBlocked.RemoveAt(total - 1);
                gv.mod.currentArea.Visible.RemoveAt(total - 1);
            }
            gv.mod.currentArea.MapSizeY--;
            //mapSizeChangeStuff();
        }
    }
}
