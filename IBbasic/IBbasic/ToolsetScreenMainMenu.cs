﻿using System;
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
        public IbbButton btnModuleEditor = null;
        public IbbButton btnCreatureEditor = null;
        public IbbButton btnItem = null;
        public IbbButton btnPlayer = null;
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
            leftPanelLocY = 0 + 1 * gv.scaler;

            if (btnAreaEditor == null)
            {
                btnAreaEditor = new IbbButton(gv, 0.8f);
            }
            btnAreaEditor.Img = "btn_small";
            btnAreaEditor.Img2 = "none";
            btnAreaEditor.Text = "AREA";
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
            btnEncounterEditor.Img2 = "none";
            btnEncounterEditor.Text = "ENC";
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
            btnConvoEditor.Img2 = "none";
            btnConvoEditor.Text = "CONVO";
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
            btnContainerEditor.Img2 = "none";
            btnContainerEditor.Text = "CONT";
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
            btnShopEditor.Img2 = "none";
            btnShopEditor.Text = "SHOP";
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
            btnJournalEditor.Img2 = "none";
            btnJournalEditor.Text = "JRNL";
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
            tglMainMenu.ImgOn = "tgl_toggles_on";
            tglMainMenu.ImgOff = "tgl_toggles_off";
            tglMainMenu.X = leftPanelLocX + 0 * gv.uiSquareSize;
            tglMainMenu.Y = leftPanelLocY + 6 * gv.uiSquareSize;
            tglMainMenu.Height = (int)(gv.ibbheight * gv.scaler);
            tglMainMenu.Width = (int)(gv.ibbwidthR * gv.scaler);
            tglMainMenu.toggleOn = showMainMenuPanels;
        }
        public void createBottomPanel()
        {
            bottomPanelLocX = (1 * gv.uiSquareSize);
            bottomPanelLocY = (6 * gv.uiSquareSize) + 1 * gv.scaler;
            
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

            if (btnModuleEditor == null)
            {
                btnModuleEditor = new IbbButton(gv, 0.8f);
            }
            btnModuleEditor.Img = "btn_small";
            btnModuleEditor.Img2 = "none";
            btnModuleEditor.Text = "MOD";
            btnModuleEditor.Glow = "btn_small_glow";
            btnModuleEditor.HotKey = "";
            btnModuleEditor.X = bottomPanelLocX + 1 * gv.uiSquareSize;
            btnModuleEditor.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnModuleEditor.Height = (int)(gv.ibbheight * gv.scaler);
            btnModuleEditor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCreatureEditor == null)
            {
                btnCreatureEditor = new IbbButton(gv, 0.8f);
            }
            btnCreatureEditor.Img = "btn_small";
            btnCreatureEditor.Img2 = "none";
            btnCreatureEditor.Text = "CRT";
            btnCreatureEditor.Glow = "btn_small_glow";
            btnCreatureEditor.HotKey = "";
            btnCreatureEditor.X = bottomPanelLocX + 2 * gv.uiSquareSize;
            btnCreatureEditor.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnCreatureEditor.Height = (int)(gv.ibbheight * gv.scaler);
            btnCreatureEditor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnItem == null)
            {
                btnItem = new IbbButton(gv, 0.8f);
            }
            btnItem.Img = "btn_small";
            btnItem.Img2 = "none";
            btnItem.Text = "ITEM";
            btnItem.Glow = "btn_small_glow";
            btnItem.HotKey = "";
            btnItem.X = bottomPanelLocX + 3 * gv.uiSquareSize;
            btnItem.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnItem.Height = (int)(gv.ibbheight * gv.scaler);
            btnItem.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnPlayer == null)
            {
                btnPlayer = new IbbButton(gv, 0.8f);
            }
            btnPlayer.Img = "btn_small";
            btnPlayer.Img2 = "none";
            btnPlayer.Text = "PLYR";
            btnPlayer.Glow = "btn_small_glow";
            btnPlayer.HotKey = "";
            btnPlayer.X = bottomPanelLocX + 4 * gv.uiSquareSize;
            btnPlayer.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnPlayer.Height = (int)(gv.ibbheight * gv.scaler);
            btnPlayer.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnExit == null)
            {
                btnExit = new IbbButton(gv, 0.8f);
            }
            btnExit.Img = "btn_small";
            btnExit.Img2 = "none";
            btnExit.Text = "EXIT";
            btnExit.Glow = "btn_small_glow";
            btnExit.HotKey = "";
            btnExit.X = bottomPanelLocX + 6 * gv.uiSquareSize;
            btnExit.Y = bottomPanelLocY + 0 * gv.uiSquareSize;
            btnExit.Height = (int)(gv.ibbheight * gv.scaler);
            btnExit.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsMainMenu()
        {
            createLeftPanel();
            createBottomPanel();

            tglMainMenu.Draw();
            if (showMainMenuPanels)
            {
                btnAreaEditor.Draw();
                btnEncounterEditor.Draw();
                btnConvoEditor.Draw();
                btnContainerEditor.Draw();
                btnShopEditor.Draw();
                btnJournalEditor.Draw();
                btnSave.Draw();
                btnModuleEditor.Draw();
                btnCreatureEditor.Draw();
                btnItem.Draw();
                btnPlayer.Draw();
                btnExit.Draw();
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
            btnModuleEditor.glowOn = false;
            btnCreatureEditor.glowOn = false;
            btnItem.glowOn = false;
            btnPlayer.glowOn = false;
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
                        btnModuleEditor.glowOn = false;
                        btnCreatureEditor.glowOn = false;
                        btnItem.glowOn = false;
                        btnPlayer.glowOn = false;
                        btnExit.glowOn = false;

                        if (btnAreaEditor.getImpact(x, y))
                        {
                            //tutorialQuickStartGuide();
                            List<string> itlist = new List<string>();
                            itlist.Add("New 2D 10x10 Area");
                            itlist.Add("New 2D 20x20 Area");
                            itlist.Add("New 3D 10x10 Area");
                            itlist.Add("New 3D 20x20 Area");
                            foreach (Area a in gv.mod.moduleAreasObjects)
                            {
                                itlist.Add(a.Filename);
                            }

                            /*using (ListItemSelector itSel = new ListItemSelector(gv, itlist, "Area to Edit"))
                            {
                                var ret = itSel.ShowDialog();

                                if (ret == DialogResult.OK)
                                {
                                    if (itSel.selectedIndex == 0)
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
                                        gv.tsAreaEditor.mapSquareSizeScaler = 1;
                                        return true;
                                    }
                                    else if (itSel.selectedIndex == 1)
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
                                        gv.tsAreaEditor.mapSquareSizeScaler = 2;
                                        return true;
                                    }
                                    else if (itSel.selectedIndex == 2)
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
                                        gv.tsAreaEditor.mapSquareSizeScaler = 1;
                                        return true;
                                    }
                                    else if (itSel.selectedIndex == 3)
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
                                        gv.tsAreaEditor.mapSquareSizeScaler = 2;
                                        return true;
                                    }
                                    else if (itSel.selectedIndex > 3)
                                    {
                                        bool foundArea = gv.mod.setCurrentArea(itlist[itSel.selectedIndex], gv);
                                        if (!foundArea)
                                        {
                                            MessageBox.Show("Area: " + itlist[itSel.selectedIndex] + " does not exist in the module...check the spelling of the 'area.Filename'");
                                            return true;
                                        }
                                        gv.screenType = "tsAreaEditor";
                                        showMainMenuPanels = false;
                                        tglMainMenu.toggleOn = false;
                                        if (gv.mod.currentArea.MapSizeX > 10)
                                        {
                                            gv.tsAreaEditor.mapSquareSizeScaler = 2;
                                        }
                                        else
                                        {
                                            gv.tsAreaEditor.mapSquareSizeScaler = 1;
                                        }
                                        return true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("didn't find a selection");
                                        return true;
                                    }
                                }
                            }*/
                            return true;
                        }
                        else if (btnEncounterEditor.getImpact(x, y))
                        {
                            //changeModuleDescription();
                            return true;
                        }
                        else if (btnConvoEditor.getImpact(x, y))
                        {
                            List<string> itlist = new List<string>();
                            itlist.Add("New Conversation");
                            foreach (Convo cnv in gv.mod.moduleConvoList)
                            {
                                itlist.Add(cnv.ConvoFileName);
                            }

                            /*using (ListItemSelector itSel = new ListItemSelector(gv, itlist, "Convo to Edit"))
                            {
                                var ret = itSel.ShowDialog();

                                if (ret == DialogResult.OK)
                                {
                                    if (itSel.selectedIndex == 0)
                                    {
                                        Convo newConvo = new Convo();
                                        newConvo.ConvoFileName = "newConversation_" + gv.mod.getNextIdNumber();
                                        //TODO setup first node as root
                                        ContentNode contentNode = new ContentNode();
                                        contentNode.idNum = newConvo.NextIdNum;
                                        contentNode.conversationText = "root";
                                        newConvo.subNodes.Add(contentNode);
                                        gv.mod.moduleConvoList.Add(newConvo);
                                        return true;
                                    }                                    
                                    else if (itSel.selectedIndex > 0)
                                    {
                                        string tag = itlist[itSel.selectedIndex];
                                        try
                                        {
                                            gv.tsConvoEditor.currentConvo = gv.mod.getConvoByName(tag);
                                            gv.tsConvoEditor.currentNode = gv.tsConvoEditor.currentConvo.GetContentNodeById(0);                                            
                                            gv.tsConvoEditor.resetAllParentIds();
                                            gv.tsConvoEditor.ResetTreeView();
                                            gv.tsConvoEditor.parentNode = gv.tsConvoEditor.currentConvo.GetContentNodeById(gv.tsConvoEditor.currentNode.parentIdNum);
                                            gv.cc.ResetAllVariablesUsedList();
                                            if (gv.screenConvo.currentConvo != null)
                                            {
                                                gv.screenType = "tsConvoEditor";
                                                showMainMenuPanels = false;
                                                tglMainMenu.toggleOn = false;
                                            }
                                            else
                                            {
                                                gv.sf.MessageBox("failed to find conversation in list with tag: " + tag);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            gv.sf.MessageBox("failed to open conversation with tag: " + tag);
                                            gv.errorLog(ex.ToString());
                                        }
                                        return true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("didn't find a selection");
                                        return true;
                                    }
                                }
                            }
                            */
                            return true;
                        }
                        else if (btnContainerEditor.getImpact(x, y))
                        {
                            //changeModuleVersion();
                            return true;
                        }
                        else if (btnShopEditor.getImpact(x, y))
                        {
                            //changeModuleVersion();
                            return true;
                        }
                        else if (btnJournalEditor.getImpact(x, y))
                        {
                            //changeModuleVersion();
                            return true;
                        }
                        else if (btnSave.getImpact(x, y))
                        {
                            incrementalSaveModule();
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
                            //changeModuleVersion();
                            return true;
                        }
                        else if (btnItem.getImpact(x, y))
                        {
                            //changeModuleVersion();
                            return true;
                        }
                        else if (btnPlayer.getImpact(x, y))
                        {
                            //changeModuleVersion();
                            return true;
                        }
                        else if (btnExit.getImpact(x, y))
                        {
                            //changeModuleVersion();
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }        
        public void incrementalSaveModule()
        {
            gv.cc.incrementalSave();
        }
    }
}