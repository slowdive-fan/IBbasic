using System;
using System.Collections.Generic;
using System.Text;

namespace IBbasic
{
    public class ToolsetScreenShopEditor
    {
        public GameView gv;
        public IbbToggle btnShopTag = null;
        public IbbToggle btnShopName = null;
        public IbbToggle btnShopBuyBack = null;
        public IbbToggle btnShopSell = null;
        public IbbButton btnAddShop = null;
        public IbbButton btnRemoveShop = null;
        public IbbButton btnAddItem = null;
        public IbbButton btnRemoveItem = null;
        public IbbButton btnHelp = null;
        public int shopListIndex = 0;
        public int itemListIndex = 0;

        public ToolsetScreenShopEditor(GameView g)
        {
            gv = g;
            setControlsStart();
        }

        public void setControlsStart()
        {
            if (btnShopName == null)
            {
                btnShopName = new IbbToggle(gv);
            }
            btnShopName.ImgOn = "mtgl_edit_btn";
            btnShopName.ImgOff = "mtgl_edit_btn";
            btnShopName.X = 4 * gv.uiSquareSize;
            btnShopName.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 4);
            btnShopName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnShopName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnShopTag == null)
            {
                btnShopTag = new IbbToggle(gv);
            }
            btnShopTag.ImgOn = "mtgl_edit_btn";
            btnShopTag.ImgOff = "mtgl_edit_btn";
            btnShopTag.X = 6 * gv.uiSquareSize + (1 * gv.uiSquareSize / 4);
            btnShopTag.Y = 0 * gv.uiSquareSize + (3 * gv.uiSquareSize / 4);
            btnShopTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnShopTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnShopBuyBack == null)
            {
                btnShopBuyBack = new IbbToggle(gv);
            }
            btnShopBuyBack.ImgOn = "mtgl_edit_btn";
            btnShopBuyBack.ImgOff = "mtgl_edit_btn";
            btnShopBuyBack.X = 6 * gv.uiSquareSize + (1 * gv.uiSquareSize / 4);
            btnShopBuyBack.Y = 1 * gv.uiSquareSize + (1 * gv.uiSquareSize / 4);
            btnShopBuyBack.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnShopBuyBack.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnShopSell == null)
            {
                btnShopSell = new IbbToggle(gv);
            }
            btnShopSell.ImgOn = "mtgl_edit_btn";
            btnShopSell.ImgOff = "mtgl_edit_btn";
            btnShopSell.X = 6 * gv.uiSquareSize + (1 * gv.uiSquareSize / 4);
            btnShopSell.Y = 1 * gv.uiSquareSize + (3 * gv.uiSquareSize / 4);
            btnShopSell.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnShopSell.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnAddShop == null)
            {
                btnAddShop = new IbbButton(gv, 0.8f);
            }
            //btnAddShop.Text = "ADD";
            btnAddShop.Img = "btn_small";
            btnAddShop.Img2 = "btnadd";
            btnAddShop.Glow = "btn_small_glow";
            btnAddShop.X = 0 * gv.uiSquareSize + gv.fontWidth;
            btnAddShop.Y = 3 * gv.fontHeight;
            btnAddShop.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddShop.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveShop == null)
            {
                btnRemoveShop = new IbbButton(gv, 0.8f);
            }
            //btnRemoveShop.Text = "REMOVE";
            btnRemoveShop.Img = "btn_small";
            btnRemoveShop.Img2 = "btnremove";
            btnRemoveShop.Glow = "btn_small_glow";
            btnRemoveShop.X = 1 * gv.uiSquareSize + gv.fontWidth;
            btnRemoveShop.Y = 3 * gv.fontHeight;
            btnRemoveShop.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveShop.Width = (int)(gv.ibbwidthR * gv.scaler);

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
            btnHelp.Y = (int)(6 * gv.uiSquareSize + gv.scaler);
            btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
            btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsShopEditor()
        {
            setControlsStart();
            int shiftForFont = (btnShopTag.Height / 2) - (gv.fontHeight / 2);
            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            //Page Title
            gv.DrawText("SHOP EDITOR", center - (9 * gv.fontWidth), 2 * gv.scaler, "yl");

            //label for Containers in module      
            gv.DrawText("SHOPS", btnAddShop.X, btnAddShop.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            //on the left draw the add and remove buttons
            btnAddShop.Draw();
            btnRemoveShop.Draw();
            //list all containers (tap on a container in the list to show elements for editing)
            int startX = 1 * gv.fontWidth;
            int startY = btnAddShop.Y + btnAddShop.Height - gv.fontHeight;
            int incY = gv.fontHeight + gv.fontLineSpacing;
            int cnt = 0;
            foreach (Shop c in gv.mod.moduleShopsList)
            {
                if (cnt == shopListIndex)
                {
                    gv.DrawText(c.shopTag, startX, startY += incY, "gn");
                }
                else
                {
                    gv.DrawText(c.shopTag, startX, startY += incY, "wh");
                }
                cnt++;
            }

            if (gv.mod.moduleShopsList.Count > 0)
            {
                btnShopTag.Draw();
                gv.DrawText("SHOP TAG: " + gv.mod.moduleShopsList[shopListIndex].shopTag, btnShopTag.X + btnShopTag.Width + gv.scaler, btnShopTag.Y + shiftForFont, "wh");
                btnShopName.Draw();
                gv.DrawText("SHOP NAME: " + gv.mod.moduleShopsList[shopListIndex].shopName, btnShopName.X + btnShopName.Width + gv.scaler, btnShopName.Y + shiftForFont, "wh");
                btnShopBuyBack.Draw();
                gv.DrawText("BUY %: " + gv.mod.moduleShopsList[shopListIndex].buybackPercent, btnShopBuyBack.X + btnShopBuyBack.Width + gv.scaler, btnShopBuyBack.Y + shiftForFont, "wh");
                btnShopSell.Draw();
                gv.DrawText("SELL %: " + gv.mod.moduleShopsList[shopListIndex].sellPercent, btnShopSell.X + btnShopSell.Width + gv.scaler, btnShopSell.Y + shiftForFont, "wh");

                //label for item list
                gv.DrawText("SHOP ITEMS", btnAddItem.X, btnAddItem.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
                //on the left draw the add and remove buttons
                btnAddItem.Draw();
                btnRemoveItem.Draw();
                //list all containers (tap on a container in the list to show elements for editing)
                startX = btnAddItem.X;
                startY = btnAddItem.Y + btnAddItem.Height - gv.fontHeight;
                incY = gv.fontHeight + gv.fontLineSpacing;
                cnt = 0;
                int columncount = 0;
                int longestline = 0;
                foreach (ItemRefs itr in gv.mod.moduleShopsList[shopListIndex].shopItemRefs)
                {
                    if (itr.name.Length > longestline)
                    {
                        longestline = itr.name.Length;
                    }
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
                    columncount++;
                    if (columncount > 19)
                    {
                        columncount = 0;
                        startY = btnAddItem.Y + btnAddItem.Height - gv.fontHeight;
                        startX = btnAddItem.X + (longestline + 1) * gv.fontWidth;
                    }
                }
            }

            btnHelp.Draw();

            gv.tsMainMenu.redrawTsMainMenu();

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }
        public void onTouchTsShopEditor(int eX, int eY, MouseEventType.EventType eventType)
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
                        int containerPanelTopLocation = btnAddShop.Y + btnAddShop.Height - gv.fontHeight;
                        int lineIndex = (y - containerPanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing) - 1;
                        if ((lineIndex < gv.mod.moduleShopsList.Count) && (lineIndex >= 0))
                        {
                            shopListIndex = lineIndex;
                            itemListIndex = 0;
                        }
                    }
                    else //tapped in item list
                    {
                        if (gv.mod.moduleShopsList.Count > 0)
                        {
                            int longestline = 0;
                            foreach (ItemRefs itr in gv.mod.moduleShopsList[shopListIndex].shopItemRefs)
                            {
                                if (itr.name.Length > longestline)
                                {
                                    longestline = itr.name.Length;
                                }
                            }
                            if ((x > btnAddItem.X) && (x < btnAddItem.X + (longestline + 1) * gv.fontWidth))
                            {
                                //left column clicked 
                                int itemPanelTopLocation = btnAddItem.Y + btnAddItem.Height - gv.fontHeight;
                                int lineIndex = (y - itemPanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing) - 1;
                                if ((lineIndex < gv.mod.moduleShopsList[shopListIndex].shopItemRefs.Count) && (lineIndex >= 0))
                                {
                                    itemListIndex = lineIndex;
                                }
                            }
                            else //right column
                            {
                                if (gv.mod.moduleShopsList[shopListIndex].shopItemRefs.Count > 20)
                                {
                                    int itemPanelTopLocation = btnAddItem.Y + btnAddItem.Height - gv.fontHeight;
                                    int lineIndex = (y - itemPanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing) - 1;
                                    if ((lineIndex < gv.mod.moduleShopsList[shopListIndex].shopItemRefs.Count - 20) && (lineIndex >= 0))
                                    {
                                        itemListIndex = lineIndex + 20;
                                    }
                                }
                            }
                        }
                    }                   

                    if (btnAddShop.getImpact(x, y))
                    {
                        Shop newContainer = new Shop();
                        newContainer.shopTag = "newShopTag";
                        newContainer.shopName = "newShopName";
                        gv.mod.moduleShopsList.Add(newContainer);
                    }
                    else if (btnRemoveShop.getImpact(x, y))
                    {
                        if (gv.mod.moduleShopsList.Count > 0)
                        {
                            try
                            {
                                gv.mod.moduleShopsList.RemoveAt(shopListIndex);
                                shopListIndex = 0;
                            }
                            catch { }
                        }
                    }
                    else if (btnHelp.getImpact(x, y))
                    {
                        //incrementalSaveModule();
                    }

                    if (gv.mod.moduleShopsList.Count > 0)
                    {
                        if (btnShopTag.getImpact(x, y))
                        {
                            changeShopTag();
                        }
                        else if (btnShopName.getImpact(x, y))
                        {
                            changeShopName();
                        }
                        else if (btnShopBuyBack.getImpact(x, y))
                        {
                            changeShopBuy();
                        }
                        else if (btnShopSell.getImpact(x, y))
                        {
                            changeShopSell();
                        }
                        else if (btnAddItem.getImpact(x, y))
                        {
                            if (gv.mod.moduleShopsList.Count > 0)
                            {
                                //bring up selection list and choose an item
                                addItemToShop();
                            }
                        }
                        else if (btnRemoveItem.getImpact(x, y))
                        {
                            if (gv.mod.moduleShopsList.Count > 0)
                            {
                                try
                                {
                                    if (gv.mod.moduleShopsList[shopListIndex].shopItemRefs.Count > 0)
                                    {
                                        gv.mod.moduleShopsList[shopListIndex].shopItemRefs.RemoveAt(itemListIndex);
                                        itemListIndex = 0;
                                    }
                                }
                                catch { }
                            }
                        }
                    }

                    break;
            }
        }
        public async void changeShopTag()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose a unique tag for this Shop.", gv.mod.moduleShopsList[shopListIndex].shopTag);
            gv.mod.moduleShopsList[shopListIndex].shopTag = myinput;
            gv.touchEnabled = true;
        }
        public async void changeShopName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose a name for this Shop.", gv.mod.moduleShopsList[shopListIndex].shopName);
            gv.mod.moduleShopsList[shopListIndex].shopName = myinput;
            gv.touchEnabled = true;
        }
        public async void changeShopBuy()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Buy Back percentage for shop.", gv.mod.moduleShopsList[shopListIndex].buybackPercent);
            gv.mod.moduleShopsList[shopListIndex].buybackPercent = myinput;
            gv.touchEnabled = true;
        }
        public async void changeShopSell()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Sell percentage for shop.", gv.mod.moduleShopsList[shopListIndex].sellPercent);
            gv.mod.moduleShopsList[shopListIndex].sellPercent = myinput;
            gv.touchEnabled = true;
        }
        public async void addItemToShop()
        {
            List<string> items = new List<string>();
            items.Add("none");
            //gv.mod.currentEncounter.encounterInventoryRefsList.Clear();
            foreach (Item it in gv.cc.allItemsList)
            {
                items.Add(it.name);
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an item from the list to add to shop items:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                ItemRefs newIR = gv.mod.createItemRefsFromItem(it);
                gv.mod.moduleShopsList[shopListIndex].shopItemRefs.Add(newIR);
            }
            gv.touchEnabled = true;
        }
    }
}
