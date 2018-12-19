using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    public class ToolsetScreenMainMenu
    {
        public GameView gv;

        public bool showMainMenuPanels = true;
        //UI PANELS
        public int leftPanelLocX = 0;
        public int leftPanelLocY = 0;
        public int bottomPanelLocX = 0;
        public int bottomPanelLocY = 0;
        //LEFT PANEL
        public IbbToggle tglMainMenu = null;
        public IbbButton btnAreaEditor = null;
        public IbbButton btnEncounterEditor = null;
        public IbbButton btnConvoEditor = null;
        public IbbButton btnContainerEditor = null;
        public IbbButton btnShopEditor = null;
        public IbbButton btnJournalEditor = null;
        //BOTTOM PANEL
        public IbbButton btnSave = null;
        public IbbButton btnBackUpModule = null;
        public IbbButton btnModuleEditor = null;
        public IbbButton btnCreatureEditor = null;
        public IbbButton btnItem = null;
        public IbbButton btnPlayer = null;
        public IbbButton btnArt = null;
        public IbbToggle tglZoom = null;
        public IbbButton btnExit = null;

        public ToolsetScreenMainMenu(GameView g)
        {
            gv = g;
            createLeftPanel();
            createBottomPanel();
        }
        public void createLeftPanel()
        {
            leftPanelLocX = 0;
            leftPanelLocY = (int)(0 + 1 * gv.scaler);

            if (btnAreaEditor == null)
            {
                btnAreaEditor = new IbbButton(gv, 0.8f);
            }
            btnAreaEditor.Img = "btn_small";
            btnAreaEditor.Img2 = "btnarea";
            //btnAreaEditor.Text = "AREA";
            btnAreaEditor.Glow = "btn_small_glow";
            btnAreaEditor.HotKey = "";
            btnAreaEditor.X = leftPanelLocX + 0 * gv.uiSquareSize;
            btnAreaEditor.Y = leftPanelLocY + 0 * gv.uiSquareSize;
            btnAreaEditor.Height = (int)(gv.ibbheight * gv.scaler);
            btnAreaEditor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnEncounterEditor == null)
            {
                btnEncounterEditor = new IbbButton(gv, 0.8f);
            }
            btnEncounterEditor.Img = "btn_small";
            btnEncounterEditor.Img2 = "btnencounter";
            //btnEncounterEditor.Text = "ENC";
            btnEncounterEditor.Glow = "btn_small_glow";
            btnEncounterEditor.HotKey = "";
            btnEncounterEditor.X = leftPanelLocX + 0 * gv.uiSquareSize;
            btnEncounterEditor.Y = leftPanelLocY + 1 * gv.uiSquareSize;
            btnEncounterEditor.Height = (int)(gv.ibbheight * gv.scaler);
            btnEncounterEditor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnConvoEditor == null)
            {
                btnConvoEditor = new IbbButton(gv, 0.8f);
            }
            btnConvoEditor.Img = "btn_small";
            btnConvoEditor.Img2 = "btnconvo";
            //btnConvoEditor.Text = "CONVO";
            btnConvoEditor.Glow = "btn_small_glow";
            btnConvoEditor.HotKey = "";
            btnConvoEditor.X = leftPanelLocX + 0 * gv.uiSquareSize;
            btnConvoEditor.Y = leftPanelLocY + 2 * gv.uiSquareSize;
            btnConvoEditor.Height = (int)(gv.ibbheight * gv.scaler);
            btnConvoEditor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnContainerEditor == null)
            {
                btnContainerEditor = new IbbButton(gv, 0.8f);
            }
            btnContainerEditor.Img = "btn_small";
            btnContainerEditor.Img2 = "btncontainer";
            //btnContainerEditor.Text = "CONT";
            btnContainerEditor.Glow = "btn_small_glow";
            btnContainerEditor.HotKey = "C";
            btnContainerEditor.X = leftPanelLocX + 0 * gv.uiSquareSize;
            btnContainerEditor.Y = leftPanelLocY + 3 * gv.uiSquareSize;
            btnContainerEditor.Height = (int)(gv.ibbheight * gv.scaler);
            btnContainerEditor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnShopEditor == null)
            {
                btnShopEditor = new IbbButton(gv, 0.8f);
            }
            btnShopEditor.Img = "btn_small";
            btnShopEditor.Img2 = "btnshop";
            //btnShopEditor.Text = "SHOP";
            btnShopEditor.Glow = "btn_small_glow";
            btnShopEditor.HotKey = "C";
            btnShopEditor.X = leftPanelLocX + 0 * gv.uiSquareSize;
            btnShopEditor.Y = leftPanelLocY + 4 * gv.uiSquareSize;
            btnShopEditor.Height = (int)(gv.ibbheight * gv.scaler);
            btnShopEditor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnJournalEditor == null)
            {
                btnJournalEditor = new IbbButton(gv, 0.8f);
            }
            btnJournalEditor.Img = "btn_small";
            btnJournalEditor.Img2 = "btnjournal";
            //btnJournalEditor.Text = "JRNL";
            btnJournalEditor.Glow = "btn_small_glow";
            btnJournalEditor.HotKey = "C";
            btnJournalEditor.X = leftPanelLocX + 0 * gv.uiSquareSize;
            btnJournalEditor.Y = leftPanelLocY + 5 * gv.uiSquareSize;
            btnJournalEditor.Height = (int)(gv.ibbheight * gv.scaler);
            btnJournalEditor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (tglMainMenu == null)
            {
                tglMainMenu = new IbbToggle(gv);
            }
            tglMainMenu.ImgOn = "tgl_menu_on";
            tglMainMenu.ImgOff = "tgl_menu_off";
            tglMainMenu.X = leftPanelLocX + 0 * gv.uiSquareSize;
            tglMainMenu.Y = leftPanelLocY + 6 * gv.uiSquareSize;
            tglMainMenu.Height = (int)(gv.ibbheight * gv.scaler);
            tglMainMenu.Width = (int)(gv.ibbwidthR * gv.scaler);
            tglMainMenu.toggleOn = showMainMenuPanels;
        }
        public void createBottomPanel()
        {
            bottomPanelLocX = (1 * gv.uiSquareSize);
            bottomPanelLocY = (int)((6 * gv.uiSquareSize) + 1 * gv.scaler);

            if (btnSave == null)
            {
                btnSave = new IbbButton(gv, 0.8f);
            }
            btnSave.Img = "btn_small";
            btnSave.Img2 = "btndisk";
            btnSave.Glow = "btn_small_glow";
            btnSave.HotKey = "";
            btnSave.X = bottomPanelLocX + 0 * gv.uiSquareSize;
            btnSave.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnSave.Height = (int)(gv.ibbheight * gv.scaler);
            btnSave.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnBackUpModule == null)
            {
                btnBackUpModule = new IbbButton(gv, 0.8f);
            }
            btnBackUpModule.Img = "btn_small";
            btnBackUpModule.Img2 = "btndiskinc";
            btnBackUpModule.Glow = "btn_small_glow";
            btnBackUpModule.HotKey = "";
            btnBackUpModule.X = bottomPanelLocX + 1 * gv.uiSquareSize;
            btnBackUpModule.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnBackUpModule.Height = (int)(gv.ibbheight * gv.scaler);
            btnBackUpModule.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnModuleEditor == null)
            {
                btnModuleEditor = new IbbButton(gv, 0.8f);
            }
            btnModuleEditor.Img = "btn_small";
            btnModuleEditor.Img2 = "btnsettings2";
            //btnModuleEditor.Text = "MOD";
            btnModuleEditor.Glow = "btn_small_glow";
            btnModuleEditor.HotKey = "";
            btnModuleEditor.X = bottomPanelLocX + 2 * gv.uiSquareSize;
            btnModuleEditor.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnModuleEditor.Height = (int)(gv.ibbheight * gv.scaler);
            btnModuleEditor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCreatureEditor == null)
            {
                btnCreatureEditor = new IbbButton(gv, 0.8f);
            }
            btnCreatureEditor.Img = "btn_small";
            btnCreatureEditor.Img2 = "btncreature";
            //btnCreatureEditor.Text = "CRT";
            btnCreatureEditor.Glow = "btn_small_glow";
            btnCreatureEditor.HotKey = "";
            btnCreatureEditor.X = bottomPanelLocX + 3 * gv.uiSquareSize;
            btnCreatureEditor.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnCreatureEditor.Height = (int)(gv.ibbheight * gv.scaler);
            btnCreatureEditor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnItem == null)
            {
                btnItem = new IbbButton(gv, 0.8f);
            }
            btnItem.Img = "btn_small";
            btnItem.Img2 = "btnitems";
            //btnItem.Text = "ITEM";
            btnItem.Glow = "btn_small_glow";
            btnItem.HotKey = "";
            btnItem.X = bottomPanelLocX + 4 * gv.uiSquareSize;
            btnItem.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnItem.Height = (int)(gv.ibbheight * gv.scaler);
            btnItem.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnPlayer == null)
            {
                btnPlayer = new IbbButton(gv, 0.8f);
            }
            btnPlayer.Img = "btn_small";
            btnPlayer.Img2 = "btnplayers";
            //btnPlayer.Text = "PLYR";
            btnPlayer.Glow = "btn_small_glow";
            btnPlayer.HotKey = "";
            btnPlayer.X = bottomPanelLocX + 5 * gv.uiSquareSize;
            btnPlayer.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnPlayer.Height = (int)(gv.ibbheight * gv.scaler);
            btnPlayer.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnArt == null)
            {
                btnArt = new IbbButton(gv, 0.8f);
            }
            btnArt.Img = "btn_small";
            btnArt.Img2 = "btnart";
            //btnArt.Text = "ART";
            btnArt.Glow = "btn_small_glow";
            btnArt.HotKey = "";
            btnArt.X = bottomPanelLocX + 6 * gv.uiSquareSize;
            btnArt.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnArt.Height = (int)(gv.ibbheight * gv.scaler);
            btnArt.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (tglZoom == null)
            {
                tglZoom = new IbbToggle(gv);
                tglZoom.toggleOn = true;
            }
            tglZoom.ImgOn = "tgl_zoom_on";
            tglZoom.ImgOff = "tgl_zoom_off";
            tglZoom.X = bottomPanelLocX + 7 * gv.uiSquareSize;
            tglZoom.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            tglZoom.Height = (int)(gv.ibbheight * gv.scaler);
            tglZoom.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnExit == null)
            {
                btnExit = new IbbButton(gv, 0.8f);
            }
            btnExit.Img = "btn_small";
            btnExit.Img2 = "btnexit";
            //btnExit.Text = "EXIT";
            btnExit.Glow = "btn_small_glow";
            btnExit.HotKey = "";
            btnExit.X = bottomPanelLocX + 8 * gv.uiSquareSize;
            btnExit.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnExit.Height = (int)(gv.ibbheight * gv.scaler);
            btnExit.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsMainMenu(SKCanvas c)
        {
            createLeftPanel();
            createBottomPanel();

            tglMainMenu.Draw(c);
            if (showMainMenuPanels)
            {
                btnAreaEditor.Draw(c);
                btnEncounterEditor.Draw(c);
                btnConvoEditor.Draw(c);
                btnContainerEditor.Draw(c);
                btnShopEditor.Draw(c);
                btnJournalEditor.Draw(c);
                btnSave.Draw(c);
                btnBackUpModule.Draw(c);
                btnModuleEditor.Draw(c);
                btnCreatureEditor.Draw(c);
                btnItem.Draw(c);
                btnPlayer.Draw(c);
                btnArt.Draw(c);
                tglZoom.Draw(c);
                btnExit.Draw(c);
            }    
        }
        public bool onTouchTsMainMenu(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnAreaEditor.glowOn = false;
            btnEncounterEditor.glowOn = false;
            btnConvoEditor.glowOn = false;
            btnContainerEditor.glowOn = false;
            btnShopEditor.glowOn = false;
            btnJournalEditor.glowOn = false;
            btnSave.glowOn = false;
            btnBackUpModule.glowOn = false;
            btnModuleEditor.glowOn = false;
            btnCreatureEditor.glowOn = false;
            btnItem.glowOn = false;
            btnPlayer.glowOn = false;
            btnArt.glowOn = false;
            btnExit.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (showMainMenuPanels)
                    {
                        if (btnAreaEditor.getImpact(x, y))
                        {
                            btnAreaEditor.glowOn = true;
                        }
                        else if (btnEncounterEditor.getImpact(x, y))
                        {
                            btnEncounterEditor.glowOn = true;
                        }
                        else if (btnConvoEditor.getImpact(x, y))
                        {
                            btnConvoEditor.glowOn = true;
                        }
                        else if (btnContainerEditor.getImpact(x, y))
                        {
                            btnContainerEditor.glowOn = true;
                        }
                        else if (btnShopEditor.getImpact(x, y))
                        {
                            btnShopEditor.glowOn = true;
                        }
                        else if (btnJournalEditor.getImpact(x, y))
                        {
                            btnJournalEditor.glowOn = true;
                        }
                        else if (btnSave.getImpact(x, y))
                        {
                            btnSave.glowOn = true;
                        }
                        else if (btnBackUpModule.getImpact(x, y))
                        {
                            btnBackUpModule.glowOn = true;
                        }
                        else if (btnModuleEditor.getImpact(x, y))
                        {
                            btnModuleEditor.glowOn = true;
                        }
                        else if (btnCreatureEditor.getImpact(x, y))
                        {
                            btnCreatureEditor.glowOn = true;
                        }
                        else if (btnItem.getImpact(x, y))
                        {
                            btnItem.glowOn = true;
                        }
                        else if (btnPlayer.getImpact(x, y))
                        {
                            btnPlayer.glowOn = true;
                        }
                        else if (btnArt.getImpact(x, y))
                        {
                            btnArt.glowOn = true;
                        }
                        else if (btnExit.getImpact(x, y))
                        {
                            btnExit.glowOn = true;
                        }
                        return true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    if (tglMainMenu.getImpact(x, y))
                    {
                        tglMainMenu.toggleOn = !tglMainMenu.toggleOn;
                        showMainMenuPanels = !showMainMenuPanels;
                        return true;
                    }

                    if (showMainMenuPanels)
                    {
                        btnAreaEditor.glowOn = false;
                        btnEncounterEditor.glowOn = false;
                        btnConvoEditor.glowOn = false;
                        btnContainerEditor.glowOn = false;
                        btnShopEditor.glowOn = false;
                        btnJournalEditor.glowOn = false;
                        btnSave.glowOn = false;
                        btnBackUpModule.glowOn = false;
                        btnModuleEditor.glowOn = false;
                        btnCreatureEditor.glowOn = false;
                        btnItem.glowOn = false;
                        btnPlayer.glowOn = false;
                        btnArt.glowOn = false;
                        btnExit.glowOn = false;

                        if (btnAreaEditor.getImpact(x, y))
                        {
                            SelectAreaToEdit();
                            return true;
                        }
                        else if (btnEncounterEditor.getImpact(x, y))
                        {
                            SelectEncToEdit();
                            //changeModuleDescription();
                            return true;
                        }
                        else if (btnConvoEditor.getImpact(x, y))
                        {
                            SelectConvoToEdit();
                            return true;
                        }
                        else if (btnContainerEditor.getImpact(x, y))
                        {
                            gv.screenType = "tsContainerEditor";
                            showMainMenuPanels = false;
                            tglMainMenu.toggleOn = false;
                            return true;
                        }
                        else if (btnShopEditor.getImpact(x, y))
                        {
                            gv.screenType = "tsShopEditor";
                            showMainMenuPanels = false;
                            tglMainMenu.toggleOn = false;
                            return true;
                        }
                        else if (btnJournalEditor.getImpact(x, y))
                        {
                            gv.screenType = "tsJournalEditor";
                            showMainMenuPanels = false;
                            tglMainMenu.toggleOn = false;
                            return true;
                        }
                        else if (btnSave.getImpact(x, y))
                        {
                            SaveModule();
                            return true;
                        }
                        else if (btnBackUpModule.getImpact(x, y))
                        {
                            BackupModule();
                            return true;
                        }
                        else if (btnModuleEditor.getImpact(x, y))
                        {
                            gv.screenType = "tsModule";
                            showMainMenuPanels = false;
                            tglMainMenu.toggleOn = false;
                            return true;
                        }
                        else if (btnCreatureEditor.getImpact(x, y))
                        {
                            gv.screenType = "tsCreatureEditor";
                            showMainMenuPanels = false;
                            tglMainMenu.toggleOn = false;
                            return true;
                        }
                        else if (btnItem.getImpact(x, y))
                        {
                            gv.screenType = "tsItemEditor";
                            showMainMenuPanels = false;
                            tglMainMenu.toggleOn = false;
                            return true;
                        }
                        else if (btnPlayer.getImpact(x, y))
                        {
                            gv.screenType = "tsPlayerEditor";
                            showMainMenuPanels = false;
                            tglMainMenu.toggleOn = false;
                            return true;
                        }
                        else if (btnArt.getImpact(x, y))
                        {
                            gv.screenType = "tsArtEditor";
                            showMainMenuPanels = false;
                            tglMainMenu.toggleOn = false;
                            return true;
                        }
                        else if (tglZoom.getImpact(x, y))
                        {
                            if (tglZoom.toggleOn)
                            {
                                tglZoom.toggleOn = false;
                                gv.resetScaler(true, false);
                            }
                            else
                            {
                                tglZoom.toggleOn = true;
                                gv.resetScaler(false, false);
                            }
                        }
                        else if (btnExit.getImpact(x, y))
                        {
                            gv.cc.doVerifyReturnToMainSetup();
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }
        public async void SelectAreaToEdit()
        {
            gv.touchEnabled = false;

            List<string> itlist = new List<string>();
            itlist.Add("cancel");
            itlist.Add("New 2D 10x10 Area");
            itlist.Add("New 2D 20x20 Area");
            itlist.Add("New 3D 10x10 Area");
            itlist.Add("New 3D 20x20 Area");

            List<string> areasFromModFolder = gv.GetAllFilesWithExtensionFromBothFolders("\\modules\\" + gv.mod.moduleName, "\\modules\\" + gv.mod.moduleName, ".are");
            //List<string> areasFromModFolder = gv.GetAllAreaFilenames();

            foreach (string a in areasFromModFolder)
            {
                itlist.Add(a);
            }

            string selectedArea = await gv.ListViewPage(itlist, "Area to Edit");

            gv.touchEnabled = true;

            if (selectedArea.Equals("cancel"))
            {
                //do nothing
            }
            else if (selectedArea.Equals("New 2D 10x10 Area"))
            {
                Area newArea = new Area();
                newArea.Filename = "new2DareaSmall_" + gv.mod.getNextIdNumber();
                newArea.MapSizeX = 10;
                newArea.MapSizeY = 10;
                newArea.Is3dArea = false;
                newArea.SetAllToGrass();
                gv.mod.moduleAreasObjects.Add(newArea);
                gv.mod.currentArea = newArea;
                gv.screenType = "tsAreaEditor";
                showMainMenuPanels = false;
                tglMainMenu.toggleOn = false;
                gv.tsAreaEditor.resetAllPreview();
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName + ":AREA:" + newArea.Filename + ":", "none", false);
            }
            else if (selectedArea.Equals("New 2D 20x20 Area"))
            {
                Area newArea = new Area();
                newArea.Filename = "new2DareaLarge_" + gv.mod.getNextIdNumber();
                newArea.MapSizeX = 20;
                newArea.MapSizeY = 20;
                newArea.Is3dArea = false;
                newArea.SetAllToGrass();
                gv.mod.moduleAreasObjects.Add(newArea);
                gv.mod.currentArea = newArea;
                gv.screenType = "tsAreaEditor";
                showMainMenuPanels = false;
                tglMainMenu.toggleOn = false;
                gv.tsAreaEditor.resetAllPreview();
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName + ":AREA:" + newArea.Filename + ":", "none", false);
            }
            else if (selectedArea.Equals("New 3D 10x10 Area"))
            {
                Area newArea = new Area();
                newArea.Filename = "new3DareaSmall_" + gv.mod.getNextIdNumber();
                newArea.MapSizeX = 10;
                newArea.MapSizeY = 10;
                newArea.Is3dArea = true;
                newArea.SetAllToGrass3D();
                gv.mod.moduleAreasObjects.Add(newArea);
                gv.mod.currentArea = newArea;
                gv.screenType = "tsAreaEditor";
                showMainMenuPanels = false;
                tglMainMenu.toggleOn = false;
                gv.tsAreaEditor.resetAllPreview();
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName + ":AREA:" + newArea.Filename + ":", "none", false);
            }
            else if (selectedArea.Equals("New 3D 20x20 Area"))
            {
                Area newArea = new Area();
                newArea.Filename = "new3DareaLarge_" + gv.mod.getNextIdNumber();
                newArea.MapSizeX = 20;
                newArea.MapSizeY = 20;
                newArea.Is3dArea = true;
                newArea.SetAllToGrass3D();
                gv.mod.moduleAreasObjects.Add(newArea);
                gv.mod.currentArea = newArea;
                gv.screenType = "tsAreaEditor";
                showMainMenuPanels = false;
                tglMainMenu.toggleOn = false;
                gv.tsAreaEditor.resetAllPreview();
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName + ":AREA:" + newArea.Filename + ":", "none", false);
            }
            else
            {
                bool foundArea = gv.mod.setCurrentArea(selectedArea, gv);
                if (!foundArea)
                {
                    gv.sf.MessageBox("Area: " + selectedArea + " does not exist in the module...check the spelling of the 'area.Filename'");
                    return;
                }
                gv.screenType = "tsAreaEditor";
                showMainMenuPanels = false;
                tglMainMenu.toggleOn = false;
                gv.tsAreaEditor.resetAllPreview();
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName + ":AREA:" + gv.mod.currentArea.Filename + ":", "none", false);
            }
        }
        public async void SelectEncToEdit()
        {
            gv.touchEnabled = false;

            List<string> itlist = new List<string>();
            itlist.Add("cancel");
            itlist.Add("New Encounter");

            List<string> encsFromModFolder = gv.GetAllFilesWithExtensionFromBothFolders("\\modules\\" + gv.mod.moduleName, "\\modules\\" + gv.mod.moduleName, ".enc");

            foreach (string a in encsFromModFolder)
            {
                itlist.Add(a);
            }

            string selectedEnc = await gv.ListViewPage(itlist, "Encounters to Edit");

            gv.touchEnabled = true;

            if (selectedEnc.Equals("cancel"))
            {
                //do nothing
            }
            else if (selectedEnc.Equals("New Encounter"))
            {
                Encounter newEnc = new Encounter();
                newEnc.encounterName = "newEncounter_" + gv.mod.getNextIdNumber();
                newEnc.MapSizeX = 10;
                newEnc.MapSizeY = 10;
                newEnc.UseDayNightCycle = true;
                newEnc.SetAllToGrass();
                gv.mod.moduleEncountersList.Add(newEnc);
                gv.mod.currentEncounter = newEnc;
                gv.screenType = "tsEncEditor";
                showMainMenuPanels = false;
                tglMainMenu.toggleOn = false;
                gv.tsEncEditor.mapSquareSizeScaler = 1;
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName + ":ENC:" + gv.mod.currentEncounter.encounterName + ":", "none", false);
            }
            else
            {
                bool foundEnc = gv.mod.setCurrentEncounter(selectedEnc, gv);
                if (!foundEnc)
                {
                    gv.sf.MessageBox("Encounter: " + selectedEnc + " does not exist in the module...check the spelling of the 'enc.Filename'");
                }
                gv.screenType = "tsEncEditor";
                showMainMenuPanels = false;
                tglMainMenu.toggleOn = false;
                gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName + ":ENC:" + gv.mod.currentEncounter.encounterName + ":", "none", false);
            }
        }
        public async void SelectConvoToEdit()
        {
            gv.touchEnabled = false;

            List<string> itlist = new List<string>();
            itlist.Add("cancel");
            itlist.Add("New Conversation");
            List<string> convosFromModFolder = gv.GetAllFilesWithExtensionFromBothFolders("\\modules\\" + gv.mod.moduleName, "\\modules\\" + gv.mod.moduleName, ".dlg");
            
            foreach (string cnv in convosFromModFolder)
            {
                itlist.Add(cnv);
            }

            string selectedConvo = await gv.ListViewPage(itlist, "Conversation to Edit");

            gv.touchEnabled = true;

            if (selectedConvo.Equals("cancel"))
            {
                //do nothing
            }
            else if (selectedConvo.Equals("New Conversation"))
            {
                Convo newConvo = new Convo();
                newConvo.ConvoFileName = "newConversation_" + gv.mod.getNextIdNumber();
                //TODO setup first node as root
                ContentNode contentNode = new ContentNode();
                contentNode.idNum = newConvo.NextIdNum;
                contentNode.conversationText = "root";
                newConvo.subNodes.Add(contentNode);
                gv.mod.moduleConvoList.Add(newConvo);

                string tag = newConvo.ConvoFileName;
                try
                {
                    bool foundCnv = gv.mod.setCurrentConvo(tag, gv);
                    if (!foundCnv)
                    {
                        gv.sf.MessageBox("Convo: " + tag + " does not exist in the module...check the spelling of the filename");
                    }
                    gv.tsConvoEditor.currentNode = gv.mod.currentConvo.GetContentNodeById(0);
                    gv.tsConvoEditor.resetAllParentIds();
                    gv.tsConvoEditor.ResetTreeView();
                    gv.tsConvoEditor.parentNode = gv.mod.currentConvo.GetContentNodeById(gv.tsConvoEditor.currentNode.parentIdNum);
                    //gv.cc.ResetAllVariablesUsedList();
                    if (gv.mod.currentConvo != null)
                    {
                        gv.screenType = "tsConvoEditor";
                        showMainMenuPanels = false;
                        tglMainMenu.toggleOn = false;
                        gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName + ":CONVO:" + gv.mod.currentConvo.ConvoFileName + ":", "none", false);
                    }
                    else
                    {
                        gv.sf.MessageBox("failed to find conversation in list with tag: " + tag);
                    }
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("failed to open conversation with tag: " + tag);
                    //gv.errorLog(ex.ToString());
                }
            }
            else
            {
                try
                {
                    bool foundCnv = gv.mod.setCurrentConvo(selectedConvo, gv);
                    if (!foundCnv)
                    {
                        gv.sf.MessageBox("Convo: " + selectedConvo + " does not exist in the module...check the spelling of the filename");
                    }
                    //gv.mod.currentConvo = gv.mod.getConvoByName(selectedConvo, gv);
                    gv.tsConvoEditor.currentNode = gv.mod.currentConvo.GetContentNodeById(0);
                    gv.tsConvoEditor.resetAllParentIds();
                    gv.tsConvoEditor.ResetTreeView();
                    gv.tsConvoEditor.parentNode = gv.mod.currentConvo.GetContentNodeById(gv.tsConvoEditor.currentNode.parentIdNum);
                    //gv.cc.ResetAllVariablesUsedList();
                    if (gv.mod.currentConvo != null)
                    {
                        gv.screenType = "tsConvoEditor";
                        showMainMenuPanels = false;
                        tglMainMenu.toggleOn = false;
                        gv.TrackerSendEvent(":TOOLSET:" + gv.mod.moduleName + ":CONVO:" + gv.mod.currentConvo.ConvoFileName + ":", "none", false);
                    }
                    else
                    {
                        gv.sf.MessageBox("failed to find conversation in list with tag: " + selectedConvo);
                    }
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("failed to open conversation with tag: " + selectedConvo);
                    gv.errorLog(ex.ToString());
                }
            }
        }
        public void SaveModule()
        {
            gv.cc.saveFiles();
        }
        public void BackupModule()
        {
            gv.cc.createBackupFiles();
        }
    }
}
