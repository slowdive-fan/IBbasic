using System;
using System.Collections.Generic;
using System.Text;

namespace IBbasic
{
    public class ToolsetScreenContainerEditor
    {
        public GameView gv;
        public IbbToggle btnConatinerTag = null;
        public IbbButton btnAddContainer = null;
        public IbbButton btnRemoveContainer = null;
        public IbbButton btnAddItem = null;
        public IbbButton btnRemoveItem = null;
        public IbbButton btnHelp = null;
        public int containerListIndex = 0;
        public int itemListIndex = 0;

        public ToolsetScreenContainerEditor(GameView g)
        {
            gv = g;
            setControlsStart();
        }

        public void setControlsStart()
        {
            if (btnConatinerTag == null)
            {
                btnConatinerTag = new IbbToggle(gv);
            }
            btnConatinerTag.ImgOn = "mtgl_edit_btn";
            btnConatinerTag.ImgOff = "mtgl_edit_btn";
            btnConatinerTag.X = 4 * gv.uiSquareSize;
            btnConatinerTag.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + (int)(gv.scaler);
            btnConatinerTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnConatinerTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnAddContainer == null)
            {
                btnAddContainer = new IbbButton(gv, 0.8f);
            }
            //btnAddContainer.Text = "ADD";
            btnAddContainer.Img = "btn_small";
            btnAddContainer.Img2 = "btnadd";
            btnAddContainer.Glow = "btn_small_glow";
            btnAddContainer.X = 0 * gv.uiSquareSize + gv.fontWidth;
            btnAddContainer.Y = 3 * gv.fontHeight;
            btnAddContainer.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddContainer.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveContainer == null)
            {
                btnRemoveContainer = new IbbButton(gv, 0.8f);
            }
            //btnRemoveContainer.Text = "REMOVE";
            btnRemoveContainer.Img = "btn_small";
            btnRemoveContainer.Img2 = "btnremove";
            btnRemoveContainer.Glow = "btn_small_glow";
            btnRemoveContainer.X = 1 * gv.uiSquareSize + gv.fontWidth;
            btnRemoveContainer.Y = 3 * gv.fontHeight;
            btnRemoveContainer.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveContainer.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnAddItem == null)
            {
                btnAddItem = new IbbButton(gv, 0.8f);
            }
            //btnAddItem.Text = "ADD";
            btnAddItem.Img = "btn_small";
            btnAddItem.Img2 = "btnadd";
            btnAddItem.Glow = "btn_small_glow";
            btnAddItem.X = 4 * gv.uiSquareSize;
            btnAddItem.Y = 1 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnAddItem.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddItem.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveItem == null)
            {
                btnRemoveItem = new IbbButton(gv, 0.8f);
            }
            //btnRemoveItem.Text = "REMOVE";
            btnRemoveItem.Img = "btn_small";
            btnRemoveItem.Img2 = "btnremove";
            btnRemoveItem.Glow = "btn_small_glow";
            btnRemoveItem.X = 5 * gv.uiSquareSize;
            btnRemoveItem.Y = 1 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnRemoveItem.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveItem.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
            }
            //btnHelp.Text = "HELP";
            btnHelp.Img = "btn_small"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
            btnHelp.Img2 = "btnhelp";
            btnHelp.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
            btnHelp.X = 10 * gv.uiSquareSize;
            btnHelp.Y = 6 * gv.uiSquareSize + (int)(gv.scaler);
            btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
            btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsContainerEditor()
        {
            setControlsStart();
            int shiftForFont = (btnConatinerTag.Height / 2) - (gv.fontHeight / 2);
            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            //Page Title
            gv.DrawText("CONTAINERS EDITOR", center - (9 * gv.fontWidth), 2 * gv.scaler, "yl");

            //label for Containers in module      
            gv.DrawText("CONTAINERS", btnAddContainer.X, btnAddContainer.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            //on the left draw the add and remove buttons
            btnAddContainer.Draw();
            btnRemoveContainer.Draw();
            //list all containers (tap on a container in the list to show elements for editing)
            int startX = 1 * gv.fontWidth;
            int startY = btnAddContainer.Y + btnAddContainer.Height;
            int incY = gv.fontHeight + gv.fontLineSpacing;
            int cnt = 0;
            foreach (Container c in gv.mod.moduleContainersList)
            {
                if (cnt == containerListIndex)
                {
                    gv.DrawText(c.containerTag, startX, startY += incY, "gn");
                }
                else
                {
                    gv.DrawText(c.containerTag, startX, startY += incY, "wh");
                }
                cnt++;
            }

            if (gv.mod.moduleContainersList.Count > 0)
            {
                btnConatinerTag.Draw();
                gv.DrawText("CONTAINER TAG: " + gv.mod.moduleContainersList[containerListIndex].containerTag, btnConatinerTag.X + btnConatinerTag.Width + gv.scaler, btnConatinerTag.Y + shiftForFont, "wh");
                //label for item list
                gv.DrawText("CONTAINER ITEMS", btnAddItem.X, btnAddItem.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
                //on the left draw the add and remove buttons
                btnAddItem.Draw();
                btnRemoveItem.Draw();
                //list all containers (tap on a container in the list to show elements for editing)
                startX = btnAddItem.X;
                startY = btnAddItem.Y + btnAddItem.Height;
                incY = gv.fontHeight + gv.fontLineSpacing;
                cnt = 0;
                foreach (ItemRefs itr in gv.mod.moduleContainersList[containerListIndex].containerItemRefs)
                {
                    //need to check if should start new column after list hits end of screen
                    if (cnt == itemListIndex)
                    {
                        gv.DrawText(itr.name, startX, startY += incY, "gn");
                    }
                    else
                    {
                        gv.DrawText(itr.name, startX, startY += incY, "wh");
                    }
                    cnt++;
                }
            }

            btnHelp.Draw();

            gv.tsMainMenu.redrawTsMainMenu();

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }
        public void onTouchTsContainerEditor(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnHelp.glowOn = false;

            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }

            bool ret = gv.tsMainMenu.onTouchTsMainMenu(eX, eY, eventType);
            if (ret) { return; } //did some action on the main menu so do nothing here

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

                    if (btnHelp.getImpact(x, y))
                    {
                        btnHelp.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

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
                                        
                    //figure out if tapped on a container list
                    if (x < btnAddItem.X)
                    {
                        int containerPanelTopLocation = btnAddContainer.Y + btnAddContainer.Height;
                        int lineIndex = (y - containerPanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing) - 1;
                        if ((lineIndex < gv.mod.moduleContainersList.Count) && (lineIndex >= 0))
                        {
                            containerListIndex = lineIndex;
                            itemListIndex = 0;
                        }
                    }
                    else //tapped in item list
                    {
                        if (gv.mod.moduleContainersList.Count > 0)
                        {
                            int itemPanelTopLocation = btnAddItem.Y + btnAddItem.Height;
                            int lineIndex = (y - itemPanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing) - 1;
                            if ((lineIndex < gv.mod.moduleContainersList[containerListIndex].containerItemRefs.Count) && (lineIndex >= 0))
                            {
                                itemListIndex = lineIndex;
                            }
                        }
                    }
                    
                    if (btnConatinerTag.getImpact(x, y))
                    {
                        changeContainerTag();
                    }
                    else if (btnAddContainer.getImpact(x, y))
                    {
                        Container newContainer = new Container();
                        newContainer.containerTag = "newContainerTag";
                        gv.mod.moduleContainersList.Add(newContainer);
                    }
                    else if (btnRemoveContainer.getImpact(x, y))
                    {
                        if (gv.mod.moduleContainersList.Count > 0)
                        {
                            try
                            {
                                gv.mod.moduleContainersList.RemoveAt(containerListIndex);
                                containerListIndex = 0;
                            }
                            catch { }
                        }
                    }
                    else if (btnAddItem.getImpact(x, y))
                    {
                        if (gv.mod.moduleContainersList.Count > 0)
                        {
                            //bring up selection list and choose an item
                            addItemToContainer();
                        }
                    }
                    else if (btnRemoveItem.getImpact(x, y))
                    {
                        if (gv.mod.moduleContainersList.Count > 0)
                        {
                            try
                            {
                                if (gv.mod.moduleContainersList[containerListIndex].containerItemRefs.Count > 0)
                                {
                                    gv.mod.moduleContainersList[containerListIndex].containerItemRefs.RemoveAt(itemListIndex);
                                    itemListIndex = 0;
                                }
                            }
                            catch { }
                        }
                    }
                    else if (btnHelp.getImpact(x, y))
                    {
                        //incrementalSaveModule();
                    }
                    break;
            }
        }
        public async void changeContainerTag()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose a Tag for this Container.", gv.mod.moduleContainersList[containerListIndex].containerTag);
            gv.mod.moduleContainersList[containerListIndex].containerTag = myinput;
            gv.touchEnabled = true;
        }
        public async void addItemToContainer()
        {
            List<string> items = new List<string>();
            items.Add("none");
            //gv.mod.currentEncounter.encounterInventoryRefsList.Clear();
            foreach (Item it in gv.cc.allItemsList)
            {
                items.Add(it.name);
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an item from the list to add to container items:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                ItemRefs newIR = gv.mod.createItemRefsFromItem(it);
                gv.mod.moduleContainersList[containerListIndex].containerItemRefs.Add(newIR);
            }
            gv.touchEnabled = true;
        }
    }
}
