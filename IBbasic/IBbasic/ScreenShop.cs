﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ScreenShop 
    {
	    //public Module gv.mod;
	    public GameView gv;

	    public List<IbbButton> btnInventorySlot = new List<IbbButton>();
	    public IbbButton btnInventoryLeft = null;
	    public IbbButton btnInventoryRight = null;
	    public IbbButton btnPageIndex = null;
	    public IbbButton btnShopLeft = null;
	    public IbbButton btnShopRight = null;
	    public IbbButton btnShopPageIndex = null;
	    public List<IbbButton> btnShopSlot = new List<IbbButton>();
	    private IbbButton btnHelp = null;
	    private IbbButton btnReturn = null;
	    public int inventoryPageIndex = 0;
	    public int inventorySlotIndex = 0;
	    public int shopPageIndex = 0;
	    public int shopSlotIndex = 0;
	    public string currentShopTag = "";
	    public Shop currentShop = new Shop();
	    private string stringMessageShop = "";
        private IbbHtmlTextBox description;
	
        public ScreenShop(Module m, GameView g)
	    {
		    //gv.mod = m;
		    gv = g;
		    setControlsStart();
		    stringMessageShop = gv.cc.loadTextToString("MessageShop.txt");
	    }    
	
        public void setControlsStart()
	    {		
    	    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		    int padW = gv.uiSquareSize/6;

            description = new IbbHtmlTextBox(gv, 320, 100, 500, 300);
            description.showBoxBorder = false;

            if (btnInventoryLeft == null)
            {
                btnInventoryLeft = new IbbButton(gv, 1.0f);
            }
			    btnInventoryLeft.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnInventoryLeft.Img2 = "ctrl_left_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_left_arrow);
			    btnInventoryLeft.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnInventoryLeft.X = 1 * gv.uiSquareSize;
			    btnInventoryLeft.Y = (int)(3 * gv.uiSquareSize);
                btnInventoryLeft.Height = (int)(gv.ibbheight * gv.scaler);
                btnInventoryLeft.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnPageIndex == null)
            {
                btnPageIndex = new IbbButton(gv, 1.0f);
            }
			    btnPageIndex.Img = "btn_small_off"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_off);
			    btnPageIndex.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnPageIndex.Text = "1";
			    btnPageIndex.X = 2 * gv.uiSquareSize;
			    btnPageIndex.Y = (int)(3 * gv.uiSquareSize);
                btnPageIndex.Height = (int)(gv.ibbheight * gv.scaler);
                btnPageIndex.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnInventoryRight == null)
            {
                btnInventoryRight = new IbbButton(gv, 1.0f);
            }
			    btnInventoryRight.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnInventoryRight.Img2 = "ctrl_right_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_right_arrow);
			    btnInventoryRight.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnInventoryRight.X = 3 * gv.uiSquareSize;
			    btnInventoryRight.Y = (int)(3 * gv.uiSquareSize);
                btnInventoryRight.Height = (int)(gv.ibbheight * gv.scaler);
                btnInventoryRight.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnShopLeft == null)
            {
                btnShopLeft = new IbbButton(gv, 1.0f);
            }
			    btnShopLeft.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnShopLeft.Img2 = "ctrl_left_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_left_arrow);
			    btnShopLeft.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnShopLeft.X = 1 * gv.uiSquareSize;
			    btnShopLeft.Y = (int)(0 * gv.uiSquareSize);
                btnShopLeft.Height = (int)(gv.ibbheight * gv.scaler);
                btnShopLeft.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnShopPageIndex == null)
            {
                btnShopPageIndex = new IbbButton(gv, 1.0f);
            }
			    btnShopPageIndex.Img = "btn_small_off"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_off);
			    btnShopPageIndex.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnShopPageIndex.Text = "1";
			    btnShopPageIndex.X = 2 * gv.uiSquareSize;
			    btnShopPageIndex.Y = (int)(0 * gv.uiSquareSize);
                btnShopPageIndex.Height = (int)(gv.ibbheight * gv.scaler);
                btnShopPageIndex.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnShopRight == null)
            {
                btnShopRight = new IbbButton(gv, 1.0f);
            }
			    btnShopRight.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnShopRight.Img2 = "ctrl_right_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_right_arrow);
			    btnShopRight.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnShopRight.X = 3 * gv.uiSquareSize;
			    btnShopRight.Y = (int)(0 * gv.uiSquareSize);
                btnShopRight.Height = (int)(gv.ibbheight * gv.scaler);
                btnShopRight.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnReturn == null)
            {
                btnReturn = new IbbButton(gv, 1.2f);
            }
			    btnReturn.Text = "EXIT SHOP";
			    btnReturn.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
			    btnReturn.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
                btnReturn.X = (gv.screenWidth / 2) - (int)(gv.ibbwidthL * gv.scaler / 2.0f);
			    btnReturn.Y = 6 * gv.uiSquareSize;
                btnReturn.Height = (int)(gv.ibbheight * gv.scaler);
                btnReturn.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
            }
			    btnHelp.Text = "HELP";
			    btnHelp.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnHelp.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnHelp.X = 3 * gv.uiSquareSize + padW * 1;
			    btnHelp.Y = 6 * gv.uiSquareSize;
                btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
                btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);

            
		    for (int j = 0; j < 10; j++)
		    {
			    IbbButton btnNew = new IbbButton(gv, 1.0f);	
			    btnNew.Img = "item_slot"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.item_slot);
			    btnNew.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    if (j < 5)
			    {
				    btnNew.X = ((j+1) * gv.uiSquareSize);
				    btnNew.Y = 4 * gv.uiSquareSize;
			    }
			    else
			    {
				    btnNew.X = ((j-5+1) * gv.uiSquareSize);
				    btnNew.Y = 5 * gv.uiSquareSize;
			    }
                btnNew.Height = (int)(gv.ibbheight * gv.scaler);
                btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);	
			
			    btnInventorySlot.Add(btnNew);
		    }
           
		    for (int j = 0; j < 10; j++)
		    {
			    IbbButton btnNew = new IbbButton(gv, 1.0f);	
			    btnNew.Img = "item_slot"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.item_slot);
			    btnNew.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    if (j < 5)
			    {
				    btnNew.X = ((j+1) * gv.uiSquareSize);
				    btnNew.Y = 1 * gv.uiSquareSize;
			    }
			    else
			    {
				    btnNew.X = ((j-5+1) * gv.uiSquareSize);
				    btnNew.Y = 2 * gv.uiSquareSize;
			    }
                btnNew.Height = (int)(gv.ibbheight * gv.scaler);
                btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);	
			
			    btnShopSlot.Add(btnNew);
		    }
	    }

        public void redrawShop(SKCanvas c)
        {
            
    	    this.doItemStackingParty();
    	    
    	    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		
    	    int locY = 0;
    	    int locX = pW * 4;
    	    int textH = (int)gv.fontHeight;
            int spacing = textH;
    	    int tabX = pW * 4;
    	    int tabX2 = 5 * gv.uiSquareSize + pW * 2;
    	    int leftStartY = pH * 4;
    	    int tabStartY = 9 * gv.uiSquareSize + pH * 2;
    	    int tabShopStartY = 4 * gv.uiSquareSize + pH * 2;
    	
    	    gv.DrawText(c, currentShop.shopName, 5 * gv.uiSquareSize, locY + pH, "gy");
		
	
		    //DRAW LEFT/RIGHT ARROWS and PAGE INDEX of SHOP
		    btnShopPageIndex.Draw(c);
		    btnShopLeft.Draw(c);
		    btnShopRight.Draw(c);		
		
		    //DRAW ALL SHOP INVENTORY SLOTS of SHOP		
		    int cntSlot = 0;
		    foreach (IbbButton btn in btnShopSlot)
		    {
			    if (cntSlot == shopSlotIndex) {btn.glowOn = true;}
			    else {btn.glowOn = false;}
			    if ((cntSlot + (shopPageIndex * 10)) < currentShop.shopItemRefs.Count)
			    {
				    ItemRefs itrs = currentShop.shopItemRefs[cntSlot + (shopPageIndex * 10)];
				    Item it = gv.cc.getItemByResRefForInfo(itrs.resref);
                    //gv.cc.DisposeOfBitmap(ref btn.Img2);
                    btn.Img2 = it.itemImage;	
				    if (itrs.quantity < it.groupSizeForSellingStackableItems)
    			    {
    				    //less than the stack size for selling
    				    int cost = (itrs.quantity * storeSellValueForItem(it)) / it.groupSizeForSellingStackableItems;                        
    				    btn.Text = "" + cost;
    			    }
    			    else //have more than the stack size for selling
    			    {
    				    int full = (itrs.quantity / it.groupSizeForSellingStackableItems) * storeSellValueForItem(it);
    				    int part = ((itrs.quantity % it.groupSizeForSellingStackableItems) * storeSellValueForItem(it)) / it.groupSizeForSellingStackableItems;
    				    int total = full + part;
    				    btn.Text = "" + total;
    			    }
				
				    if (itrs.quantity > 1)
				    {
					    btn.Quantity = itrs.quantity + "";
				    }
				    else
				    {
					    btn.Quantity = "";
				    }
			    }
			    else
			    {
				    btn.Img2 = null;
				    btn.Text = "";
				    btn.Quantity = "";
			    }
			    btn.Draw(c);
			    cntSlot++;
		    }
		
		    //DRAW DESCRIPTION BOX of SHOP
		    locY = tabShopStartY;		
		    if ((shopSlotIndex + (shopPageIndex * 10)) < currentShop.shopItemRefs.Count)
		    {
                //DRAW DESCRIPTION BOX
			    Item it = gv.cc.getItemByResRefForInfo(currentShop.shopItemRefs[shopSlotIndex + (shopPageIndex * 10)].resref);

                string textToSpan = "<gn>" + it.name + "</gn><BR>";
                if ((it.category.Equals("Melee")) || (it.category.Equals("Ranged")))
                {
                    textToSpan += "Damage: " + it.damageNumDice + "d" + it.damageDie + "+" + it.damageAdder + "<BR>";
                    textToSpan += "Attack Bonus: " + it.attackBonus + "<BR>";
                    textToSpan += "Attack Range: " + it.attackRange + "<BR>";
                    textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                }
                else if (!it.category.Equals("General"))
                {
                    textToSpan += "AC Bonus: " + it.armorBonus + "<BR>";
                    textToSpan += "Max Dex Bonus: " + it.maxDexBonus + "<BR>";
                    textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                }
                else if (it.category.Equals("General"))
                {
                    textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                }
                                
                description.tbXloc = 6 * gv.uiSquareSize + pW;
                description.tbYloc = 1 * gv.uiSquareSize;
                description.tbWidth = 5 * gv.uiSquareSize;
                description.tbHeight = 4 * gv.uiSquareSize;
                description.logLinesList.Clear();
                description.AddHtmlTextToLog(textToSpan);
                description.onDrawLogBox(c);
		    }
		
		    //DRAW LEFT/RIGHT ARROWS and PAGE INDEX
		    btnPageIndex.Draw(c);
		    btnInventoryLeft.Draw(c);
		    btnInventoryRight.Draw(c);

            //DRAW TEXT		
            locY = (3 * gv.uiSquareSize);
            gv.DrawText(c, "Party", locX + gv.uiSquareSize * 4, locY, "gy");
            gv.DrawText(c, "Inventory", locX + gv.uiSquareSize * 4, locY += spacing, "gy");
            locY = (int)(3.5 * gv.uiSquareSize);
            gv.DrawText(c, "Party", locX + gv.uiSquareSize * 4, locY, "yl");
            gv.DrawText(c, gv.mod.goldLabelPlural + ": " + gv.mod.partyGold, locX + gv.uiSquareSize * 4, locY += spacing, "yl");

            //DRAW ALL PARTY INVENTORY SLOTS		
            cntSlot = 0;
		    foreach (IbbButton btn in btnInventorySlot)
		    {
			    if (cntSlot == inventorySlotIndex) {btn.glowOn = true;}
			    else {btn.glowOn = false;}
			    if ((cntSlot + (inventoryPageIndex * 10)) < gv.mod.partyInventoryRefsList.Count)
			    {
				    ItemRefs itr = gv.mod.partyInventoryRefsList[cntSlot + (inventoryPageIndex * 10)];
				    Item it = gv.cc.getItemByResRefForInfo(itr.resref);
                    //gv.cc.DisposeOfBitmap(ref btn.Img2);
                    btn.Img2 = it.itemImage;	
				    if (itr.quantity < it.groupSizeForSellingStackableItems)
    			    {
    				    //less than the stack size for selling
    				    int cost = (itr.quantity * storeBuyBackValueForItem(it)) / it.groupSizeForSellingStackableItems;
    				    btn.Text = "" + cost;
    			    }
    			    else //have more than the stack size for selling
    			    {
    				    int full = (itr.quantity / it.groupSizeForSellingStackableItems) * storeBuyBackValueForItem(it);
    				    int part = ((itr.quantity % it.groupSizeForSellingStackableItems) * storeBuyBackValueForItem(it)) / it.groupSizeForSellingStackableItems;
    				    int total = full + part;
    				    btn.Text = "" + total;
    			    }				
				    if (itr.quantity > 1)
				    {
					    btn.Quantity = itr.quantity + "";
				    }
				    else
				    {
					    btn.Quantity = "";
				    }
			    }
			    else
			    {
				    btn.Img2 = null;
				    btn.Text = "";
				    btn.Quantity = "";
			    }
			    btn.Draw(c);
			    cntSlot++;
		    }
		
		    //DRAW DESCRIPTION BOX
		    locY = tabStartY;		
		    if ((inventorySlotIndex + (inventoryPageIndex * 10)) < gv.mod.partyInventoryRefsList.Count)
		    {
			    ItemRefs itr = gv.mod.partyInventoryRefsList[inventorySlotIndex + (inventoryPageIndex * 10)];
			    Item it = gv.cc.getItemByResRefForInfo(itr.resref);
                string textToSpan = "<gn>" + it.name + "</gn><BR>";
                if ((it.category.Equals("Melee")) || (it.category.Equals("Ranged")))
                {
                    textToSpan += "Damage: " + it.damageNumDice + "d" + it.damageDie + "+" + it.damageAdder + "<BR>";
                    textToSpan += "Attack Bonus: " + it.attackBonus + "<BR>";
                    textToSpan += "Attack Range: " + it.attackRange + "<BR>";
                    textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                }
                else if (!it.category.Equals("General"))
                {
                    textToSpan += "AC Bonus: " + it.armorBonus + "<BR>";
                    textToSpan += "Max Dex Bonus: " + it.maxDexBonus + "<BR>";
                    textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                }
                else if (it.category.Equals("General"))
                {
                    textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                }

                description.tbXloc = 6 * gv.uiSquareSize + pW;
                description.tbYloc = 4 * gv.uiSquareSize;
                description.tbWidth = 5 * gv.uiSquareSize;
                description.tbHeight = 4 * gv.uiSquareSize;
                description.logLinesList.Clear();
                description.AddHtmlTextToLog(textToSpan);
                description.onDrawLogBox(c);
		    }
				
		    btnHelp.Draw(c);		
		    btnReturn.Draw(c);
            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox(c);
            }
        }
	
        public string isUseableBy(Item it)
        {
            string strg = "";
            foreach (string s in it.classesAllowed)
            {
                strg += s.Substring(0, 1) + ", ";
            }

            /*foreach (PlayerClass cls in gv.cc.datafile.dataPlayerClassList)
    	    {
    		    string firstLetter = cls.name.Substring(0,1);
    		    foreach (ItemRefs itr in cls.itemsAllowed)
    		    {
    			    if (itr.resref.Equals(it.resref))
    			    {
    				    strg += firstLetter + ", ";
    			    }
    		    }
    	    }*/
    	    return strg;
        }
        public void doItemStackingParty()
	    {
		    for (int i = 0; i < gv.mod.partyInventoryRefsList.Count; i++)
		    {
			    ItemRefs itr = gv.mod.partyInventoryRefsList[i];
			    Item itm = gv.cc.getItemByResRefForInfo(itr.resref);
			    if (itm.isStackable)
			    {
				    for (int j = gv.mod.partyInventoryRefsList.Count - 1; j >= 0; j--)
				    {
					    ItemRefs it = gv.mod.partyInventoryRefsList[j];
					    //do check to see if same resref and then stack and delete
					    if ((it.resref.Equals(itr.resref)) && (i != j))
					    {
						    itr.quantity += it.quantity;
						    gv.mod.partyInventoryRefsList.RemoveAt(j);
					    }
				    }
			    }
		    }
	    }
        public void doItemStackingShop()
	    {
    	    //Not being used but leaving here just in case for future use
		    for (int i = 0; i < currentShop.shopItemRefs.Count; i++)
		    {
			    ItemRefs itr = currentShop.shopItemRefs[i];
			    for (int j = currentShop.shopItemRefs.Count - 1; j >= 0; j--)
			    {
				    ItemRefs it = currentShop.shopItemRefs[j];
				    //do check to see if same resref and then stack and delete
				    if ((it.resref.Equals(itr.resref)) && (i != j))
				    {
					    itr.quantity += it.quantity;
					    currentShop.shopItemRefs.RemoveAt(j);
				    }
			    }
		    }
	    }
        public int storeSellValueForItem(Item it)
        {
            int adder = 0;
            int highestNonStackable = -99;
            foreach (Player pc in gv.mod.playerList)
            {
                int mod = gv.sf.CalcShopSellModifier(pc);
                if (mod > highestNonStackable)
                {
                    mod = highestNonStackable;
                }
            }

            if (highestNonStackable > -99) { adder = highestNonStackable; }

            int totalSellPerc = currentShop.sellPercent + currentShop.sellModifier + adder;

            int sellPrice = (int)(it.value * ((float)totalSellPerc / 100f));            
            if (sellPrice < 1) { sellPrice = 1; }
            return sellPrice;
        }
        public int storeBuyBackValueForItem(Item it)
        {
            int adder = 0;
            int highestNonStackable = -99;
            foreach (Player pc in gv.mod.playerList)
            {
                int mod = gv.sf.CalcShopBuyBackModifier(pc);
                if (mod > highestNonStackable)
                {
                    mod = highestNonStackable;
                }
            }

            if (highestNonStackable > -99) { adder = highestNonStackable; }

            int totalBuyPerc = currentShop.buybackPercent + currentShop.buybackModifier + adder;

            int buyPrice = (int)(it.value * ((float)totalBuyPerc / 100f));
            if (buyPrice < 1) { buyPrice = 1; }
            if (buyPrice > storeSellValueForItem(it)) { buyPrice = storeSellValueForItem(it); }
            return buyPrice;
        }
    
        public void onTouchShop(int eX, int eY, MouseEventType.EventType eventType)
	    {
		    btnInventoryLeft.glowOn = false;
		    btnInventoryRight.glowOn = false;
		    btnHelp.glowOn = false;
		    btnReturn.glowOn = false;
		    btnShopLeft.glowOn = false;
		    btnShopRight.glowOn = false;
            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }

            switch (eventType)
		    {
		    case MouseEventType.EventType.MouseDown:
		    case MouseEventType.EventType.MouseMove:
			    int x = (int) eX;
			    int y = (int) eY;

                if (gv.showMessageBox)
                {
                    if (gv.messageBox.btnReturn.getImpact(x, y))
                    {
                        gv.messageBox.btnReturn.glowOn = true;
                    }
                    return;
                }

                if (btnInventoryLeft.getImpact(x, y))
			    {
				    btnInventoryLeft.glowOn = true;
			    }
			    else if (btnInventoryRight.getImpact(x, y))
			    {
				    btnInventoryRight.glowOn = true;
			    }
			    else if (btnHelp.getImpact(x, y))
			    {
				    btnHelp.glowOn = true;
			    }
			    else if (btnReturn.getImpact(x, y))
			    {
				    btnReturn.glowOn = true;
			    }
			    else if (btnShopLeft.getImpact(x, y))
			    {
				    btnShopLeft.glowOn = true;
			    }
			    else if (btnShopRight.getImpact(x, y))
			    {
				    btnShopRight.glowOn = true;
			    }
			    break;
			
		    case MouseEventType.EventType.MouseUp:
                x = (int)eX;
                y = (int)eY;
			
			    btnInventoryLeft.glowOn = false;
			    btnInventoryRight.glowOn = false;
			    btnHelp.glowOn = false;
			    btnReturn.glowOn = false;
			    btnShopLeft.glowOn = false;
			    btnShopRight.glowOn = false;

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

                for (int j = 0; j < 10; j++)
			    {
				    if (btnInventorySlot[j].getImpact(x, y))
				    {
					    if (inventorySlotIndex == j)
					    {
						    doShopInventoryActionsSetup();
					    }
					    inventorySlotIndex = j;
				    }
			    }
			    for (int j = 0; j < 10; j++)
			    {
				    if (btnShopSlot[j].getImpact(x, y))
				    {
					    if (shopSlotIndex == j)
					    {
						    doShopShopActionsSetup();
					    }
					    shopSlotIndex = j;
				    }
			    }
			    if (btnInventoryLeft.getImpact(x, y))
			    {
				    if (inventoryPageIndex > 0)
				    {
					    inventoryPageIndex--;
					    btnPageIndex.Text = (inventoryPageIndex + 1) + "";
				    }
			    }
			    else if (btnInventoryRight.getImpact(x, y))
			    {
				    if (inventoryPageIndex < 9)
				    {
					    inventoryPageIndex++;
					    btnPageIndex.Text = (inventoryPageIndex + 1) + "";
				    }
			    }
			    else if (btnShopLeft.getImpact(x, y))
			    {
				    if (shopPageIndex > 0)
				    {
					    shopPageIndex--;
					    btnShopPageIndex.Text = (shopPageIndex + 1) + "";
				    }
			    }
			    else if (btnShopRight.getImpact(x, y))
			    {
				    if (shopPageIndex < 9)
				    {
					    shopPageIndex++;
					    btnShopPageIndex.Text = (shopPageIndex + 1) + "";
				    }
			    }
			    else if (btnHelp.getImpact(x, y))
			    {
				    tutorialMessageShop();
			    }
			    else if (btnReturn.getImpact(x, y))
			    {
				    gv.screenType = "main";	
			    }
			    break;		
		    }
	    }

        public void doShopInventoryActionsSetup()
        {
            List<string> actionList = new List<string> { "Yes, Sell Item", "No, Keep Item" };
            gv.itemListSelector.setupIBminiItemListSelector(gv, actionList, "Do you wish to sell this item?", "shopinventoryaction");
            gv.itemListSelector.showIBminiItemListSelector = true;
        }
        public void doShopInventoryActions(int selectedIndex)
	    {
		    if ((inventorySlotIndex + (inventoryPageIndex * 10)) < gv.mod.partyInventoryRefsList.Count)
		    {
                //DialogResult dlg = IBMessageBox.Show(gv, "Do you wish to sell this item?", enumMessageButton.YesNo);
                if (selectedIndex == 0)
                {
                    //sell item
                    ItemRefs itr = gv.mod.partyInventoryRefsList[inventorySlotIndex + (inventoryPageIndex * 10)];
                    Item it = gv.cc.getItemByResRef(itr.resref);
                    if (it != null)
                    {
                        if (!it.plotItem)
                        {
                            if (itr.quantity < it.groupSizeForSellingStackableItems)
                            {
                                //less than the stack size for selling
                                gv.mod.partyGold += (itr.quantity * storeBuyBackValueForItem(it)) / it.groupSizeForSellingStackableItems;
                                ItemRefs itrCopy = itr.DeepCopy();
                                itrCopy.quantity = itr.quantity;
                                currentShop.shopItemRefs.Add(itrCopy);
                                //remove item and tag from party inventory
                                gv.sf.RemoveItemFromInventory(itr, itr.quantity);
                            }
                            else //have more than the stack size for selling
                            {
                                gv.mod.partyGold += storeBuyBackValueForItem(it);
                                ItemRefs itrCopy = itr.DeepCopy();
                                itrCopy.quantity = it.groupSizeForSellingStackableItems;
                                currentShop.shopItemRefs.Add(itrCopy);
                                //remove item and tag from party inventory
                                gv.sf.RemoveItemFromInventory(itr, it.groupSizeForSellingStackableItems);
                            }
                        }
                        else
                        {
                            gv.sf.MessageBoxHtml("You can't sell this item.");
                        }
                    }
                }
                if (selectedIndex == 1)
                {
                    //do nothing
                }
		    }
	    }

        public void doShopShopActionsSetup()
        {
            List<string> actionList = new List<string> { "Yes, Buy Item", "No, Don't Buy Item" };
            gv.itemListSelector.setupIBminiItemListSelector(gv, actionList, "Do you wish to buy this item?", "shopshopaction");
            gv.itemListSelector.showIBminiItemListSelector = true;
        }
        public void doShopShopActions(int selectedIndex)
	    {
		    if ((shopSlotIndex + (shopPageIndex * 10)) < currentShop.shopItemRefs.Count)
		    {
                //check to see if have enough gold
	            Item it = gv.cc.getItemByResRef(currentShop.shopItemRefs[shopSlotIndex + (shopPageIndex * 10)].resref);
                
                //DialogResult dlg = IBMessageBox.Show(gv, "Do you wish to buy this item?", enumMessageButton.YesNo);
                if (selectedIndex == 0)
                {
                    if (it != null)
                    {
                        if (gv.mod.partyGold < storeSellValueForItem(it))
                        {
                            gv.sf.MessageBoxHtml("Your party does not have enough gold to purchase this item.");
                            return;
                        }
                    }
                    //buy item
                    ItemRefs itr = currentShop.shopItemRefs[shopSlotIndex + (shopPageIndex * 10)];
                    it = gv.cc.getItemByResRef(itr.resref);
                    if (it != null)
                    {
                        if (itr.quantity < it.groupSizeForSellingStackableItems)
                        {
                            //less than the stack size for selling
                            gv.mod.partyGold -= (itr.quantity * storeSellValueForItem(it)) / it.groupSizeForSellingStackableItems;
                            //add item and tag to party inventory
                            gv.mod.partyInventoryRefsList.Add(itr.DeepCopy());
                            //remove tag from shop list
                            currentShop.shopItemRefs.Remove(itr);
                        }
                        else //have more than the stack size for selling
                        {
                            //subtract gold from party
                            gv.mod.partyGold -= storeSellValueForItem(it);
                            //add item and tag to party inventory
                            gv.mod.partyInventoryRefsList.Add(itr.DeepCopy());
                            //remove tag from shop list
                            currentShop.shopItemRefs.Remove(itr);
                        }
                    }
                }
                if (selectedIndex == 1)
                {
                    //do nothing
                }
		    }
	    }
	
	    public void tutorialMessageShop()
        {
		    gv.sf.MessageBoxHtml(this.stringMessageShop);    	
        }
    }
}
