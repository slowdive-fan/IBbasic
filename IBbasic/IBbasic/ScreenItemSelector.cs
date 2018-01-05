﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ScreenItemSelector
    {
        //public Module gv.mod;
	    public GameView gv;
	    private int inventoryPageIndex = 0;
	    private int inventorySlotIndex = 0;
	    private int slotsPerPage = 20;
	    private List<IbbButton> btnInventorySlot = new List<IbbButton>();
	    private IbbButton btnInventoryLeft = null;
	    private IbbButton btnInventoryRight = null;
	    private IbbButton btnPageIndex = null;
	    private IbbButton btnInfo = null;
	    private IbbButton btnAction = null;
        private IbbButton btnAction2 = null;
        private IbbButton btnExit = null;
        public string itemSelectorType = "container"; //container, equip, use
        public string callingScreen = "main"; //main, party, inventory
        public List<ItemRefs> thisItemRefs;
	    private IbbHtmlTextBox description;

        public ScreenItemSelector(Module m, GameView g)
	    {
		    //gv.mod = m;
		    gv = g;
	    }

        public void resetItemSelector(List<ItemRefs> itemRefsList, string selectorType, string callingScreenToReturnTo)
        {
            thisItemRefs = itemRefsList;
            itemSelectorType = selectorType;
            callingScreen = callingScreenToReturnTo;
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
                btnInventoryLeft.X = 3 * gv.uiSquareSize + (pW * 3);
                btnInventoryLeft.Y = (0 * gv.uiSquareSize) + (pH * 2);
                btnInventoryLeft.Height = (int)(gv.ibbheight * gv.scaler);
                btnInventoryLeft.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnPageIndex == null)
            {
                btnPageIndex = new IbbButton(gv, 1.0f);
            }
			    btnPageIndex.Img = "btn_small_off"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_off);
			    btnPageIndex.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnPageIndex.Text = "1/10";
                btnPageIndex.X = 4 * gv.uiSquareSize + (pW * 4);
                btnPageIndex.Y = (0 * gv.uiSquareSize) + (pH * 2);
                btnPageIndex.Height = (int)(gv.ibbheight * gv.scaler);
                btnPageIndex.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnInventoryRight == null)
            {
                btnInventoryRight = new IbbButton(gv, 1.0f);
            }
			    btnInventoryRight.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnInventoryRight.Img2 = "ctrl_right_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_right_arrow);
			    btnInventoryRight.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
                btnInventoryRight.X = 5 * gv.uiSquareSize + (pW * 5);
                btnInventoryRight.Y = (0 * gv.uiSquareSize) + (pH * 2);
                btnInventoryRight.Height = (int)(gv.ibbheight * gv.scaler);
                btnInventoryRight.Width = (int)(gv.ibbwidthR * gv.scaler);


            if (btnAction == null)
            {
                btnAction = new IbbButton(gv, 1.0f);
            }
                if (itemSelectorType.Equals("container"))
                {
                    btnAction.Text = "TAKE ALL";
                }
                else if (itemSelectorType.Equals("equip"))
                {
                    btnAction.Text = "EQUIP SELECTED";
                }
			    btnAction.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
			    btnAction.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
                btnAction.X = (gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (int)(gv.ibbwidthL * gv.scaler / 2.0f) - (gv.uiSquareSize * 3);
			    btnAction.Y = 6 * gv.uiSquareSize - pH * 2;
                btnAction.Height = (int)(gv.ibbheight * gv.scaler);
                btnAction.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnAction2 == null)
            {
                btnAction2 = new IbbButton(gv, 1.0f);
            }
                if (itemSelectorType.Equals("container"))
                {
                    btnAction2.Text = "TAKE SELECTED";
                }
                else if (itemSelectorType.Equals("equip"))
                {
                    btnAction2.Text = "UNEQUIP";
                }
                btnAction2.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
                btnAction2.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
                btnAction2.X = (gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (int)(gv.ibbwidthL * gv.scaler / 2.0f) + (gv.uiSquareSize * 3);
                btnAction2.Y = 6 * gv.uiSquareSize - pH * 2;
                btnAction2.Height = (int)(gv.ibbheight * gv.scaler);
                btnAction2.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnExit == null)
            {
                btnExit = new IbbButton(gv, 1.0f);
            }
                btnExit.Text = "EXIT";
                btnExit.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
                btnExit.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
                btnExit.X = (gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (int)(gv.ibbwidthL * gv.scaler / 2.0f);
                btnExit.Y = 6 * gv.uiSquareSize - pH * 2;
                btnExit.Height = (int)(gv.ibbheight * gv.scaler);
                btnExit.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnInfo == null)
            {
                btnInfo = new IbbButton(gv, 0.8f);
            }
			    btnInfo.Text = "INFO";
			    btnInfo.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnInfo.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnInfo.X = (9 * gv.uiSquareSize) - padW * 1;
                btnInfo.Y = 5 * gv.uiSquareSize - pH * 6;
                btnInfo.Height = (int)(gv.ibbheight * gv.scaler);
                btnInfo.Width = (int)(gv.ibbwidthR * gv.scaler);
            
		    for (int y = 0; y < slotsPerPage; y++)
		    {
			    IbbButton btnNew = new IbbButton(gv, 1.0f);	
			    btnNew.Img = "item_slot"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.item_slot);
			    btnNew.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			
			    if (y < 5)
			    {
				    btnNew.X = ((y + 2) * gv.uiSquareSize) + (padW * (y+1));
				    btnNew.Y = 1 * gv.uiSquareSize + padW;
			    }
			    else if ((y >=5 ) && (y < 10))
			    {
				    btnNew.X = ((y-5 + 2) * gv.uiSquareSize) + (padW * ((y-5)+1));
				    btnNew.Y = 2 * gv.uiSquareSize + (padW * 2);
			    }
			    else if ((y >=10 ) && (y < 15))
			    {
				    btnNew.X = ((y-10 + 2) * gv.uiSquareSize) + (padW * ((y-10)+1));
				    btnNew.Y = 3 * gv.uiSquareSize + (padW * 3);
			    }
			    else
			    {
				    btnNew.X = ((y-15 + 2) * gv.uiSquareSize) + (padW * ((y-15)+1));
				    btnNew.Y = 4 * gv.uiSquareSize + (padW * 4);
			    }

                btnNew.Height = (int)(gv.ibbheight * gv.scaler);
                btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);	
			
			    btnInventorySlot.Add(btnNew);
		    }			
	    }
	
	    //INVENTORY SCREEN (COMBAT and MAIN)
        public void redrawItemSelector()
        {            
    	    //IF CONTROLS ARE NULL, CREATE THEM
    	    if (btnAction == null)
    	    {
    		    setControlsStart();
    	    }

            if (itemSelectorType.Equals("container"))
            {
                btnAction.Text = "TAKE ALL";
                btnAction2.Text = "TAKE SELECTED";
            }
            else if (itemSelectorType.Equals("equip"))
            {
                btnAction.Text = "EQUIP SELECTED";
                btnAction2.Text = "UNEQUIP";
            }

            int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		
    	    int locY = 0;
    	    int locX = pW * 4;
    	    int textH = (int)gv.fontHeight;
            int spacing = textH;
            int tabX = pW * 4;
    	    int tabX2 = 5 * gv.uiSquareSize + pW * 2;
    	    int leftStartY = pH * 4;
    	    int tabStartY = 5 * gv.uiSquareSize + pW * 10;
    	
            //DRAW TEXT		
		    locY = (pH * 2);
		    gv.DrawText("Item Selection", locX + (gv.uiSquareSize * 7), locY, "wh");
		    
		    //DRAW LEFT/RIGHT ARROWS and PAGE INDEX
		    btnPageIndex.Draw();
		    btnInventoryLeft.Draw();
		    btnInventoryRight.Draw();		
		
		    //DRAW ALL INVENTORY SLOTS		
		    int cntSlot = 0;
		    foreach (IbbButton btn in btnInventorySlot)
		    {
			    if (cntSlot == inventorySlotIndex) {btn.glowOn = true;}
			    else {btn.glowOn = false;}
			    if ((cntSlot + (inventoryPageIndex * slotsPerPage)) < thisItemRefs.Count)
			    {
                    Item it = gv.cc.getItemByResRefForInfo(thisItemRefs[cntSlot + (inventoryPageIndex * slotsPerPage)].resref);
				    btn.Img2 = it.itemImage;
                    ItemRefs itr = thisItemRefs[cntSlot + (inventoryPageIndex * slotsPerPage)];
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
				    btn.Quantity = "";
			    }
			    btn.Draw();
			    cntSlot++;
		    }
		
		    //DRAW DESCRIPTION BOX
		    locY = tabStartY;		
		    if (isSelectedItemSlotInPartyInventoryRange())
		    {
			    ItemRefs itRef = GetCurrentlySelectedItemRefs();
        	    Item it = gv.cc.getItemByResRefForInfo(itRef.resref);

                //Description
		        string textToSpan = "";
                textToSpan = "<gy>Description</gy>" + "<BR>";
                textToSpan += "<gn>" + it.name + "</gn><BR>";
                if ((it.category.Equals("Melee")) || (it.category.Equals("Ranged")))
                {
                    textToSpan += "Damage: " + it.damageNumDice + "d" + it.damageDie + "+" + it.damageAdder + "<BR>";
                    textToSpan += "Attack Bonus: " + it.attackBonus + "<BR>";
                    textToSpan += "Attack Range: " + it.attackRange + "<BR>";
                    textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                    textToSpan += "Tap 'INFO' for Full Description<BR>";
                }
                else if (!it.category.Equals("General"))
                {
                    textToSpan += "AC Bonus: " + it.armorBonus + "<BR>";
                    textToSpan += "Max Dex Bonus: " + it.maxDexBonus + "<BR>";
                    textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                    textToSpan += "Tap 'INFO' for Full Description<BR>";
                }
                else if (it.category.Equals("General"))
                {
                    textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                    textToSpan += "Tap 'INFO' for Full Description<BR>";
                }
                description.tbXloc = (8 * gv.uiSquareSize);
                description.tbYloc = 1 * gv.uiSquareSize;
                description.tbWidth = 3 * gv.uiSquareSize;
                description.tbHeight = 8 * gv.uiSquareSize;
                description.logLinesList.Clear();
                description.AddHtmlTextToLog(textToSpan);
                description.onDrawLogBox();
		    }
		    btnInfo.Draw();	
		    btnAction.Draw();
            btnExit.Draw();
            if ((itemSelectorType.Equals("container")) || (itemSelectorType.Equals("equip")))
            {
                btnAction2.Draw();
            }
            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
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
    		    foreach (ItemRefs stg in cls.itemsAllowed)
    		    {
    			    if (stg.resref.Equals(it.resref))
    			    {
    				    strg += firstLetter + ", ";
    			    }
    		    }
    	    }*/
    	    return strg;
        }
	    public void onTouchItemSelector(int eX, int eY, MouseEventType.EventType eventType)
	    {
		    btnInventoryLeft.glowOn = false;
		    btnInventoryRight.glowOn = false;
		    btnInfo.glowOn = false;
		    btnAction.glowOn = false;
            btnExit.glowOn = false;
            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }
            if ((itemSelectorType.Equals("container")) || (itemSelectorType.Equals("equip")))
            {
                btnAction2.glowOn = false;
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
			    else if (btnInfo.getImpact(x, y))
			    {
				    btnInfo.glowOn = true;
			    }
			    else if (btnAction.getImpact(x, y))
			    {
				    btnAction.glowOn = true;
			    }
                else if (btnExit.getImpact(x, y))
                {
                    btnExit.glowOn = true;
                }
                else if (((itemSelectorType.Equals("container")) || (itemSelectorType.Equals("equip"))) && (btnAction2.getImpact(x,y)))
                {
                    btnAction2.glowOn = true;
                }
			    break;
			
		    case MouseEventType.EventType.MouseUp:
			    x = (int) eX;
			    y = (int) eY;
			
			    btnInventoryLeft.glowOn = false;
			    btnInventoryRight.glowOn = false;
			    btnInfo.glowOn = false;
			    btnAction.glowOn = false;
                btnExit.glowOn = false;

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

                if ((itemSelectorType.Equals("container")) || (itemSelectorType.Equals("equip")))
                {
                    btnAction2.glowOn = false;
                }
			
			    for (int j = 0; j < slotsPerPage; j++)
			    {
				    if (btnInventorySlot[j].getImpact(x, y))
				    {
					    if (inventorySlotIndex == j)
					    {
						    if (itemSelectorType.Equals("container"))
                            {
                                ItemRefs itRef = GetCurrentlySelectedItemRefs();
                                gv.mod.partyInventoryRefsList.Add(itRef.DeepCopy());
                                thisItemRefs.Remove(itRef);
                            }
                            else if (itemSelectorType.Equals("equip"))
                            {
                                switchEquipment();
                                if (callingScreen.Equals("party"))
                                {
                                    gv.screenType = "party";
                                }
                                else if (callingScreen.Equals("combatParty"))
                                {
                                    gv.screenType = "combatParty";
                                }
                            }
                        }
					    inventorySlotIndex = j;
				    }
			    }
			    if (btnInventoryLeft.getImpact(x, y))
			    {
				    if (inventoryPageIndex > 0)
				    {
					    inventoryPageIndex--;
					    btnPageIndex.Text = (inventoryPageIndex + 1) + "/10";
				    }
			    }
			    else if (btnInventoryRight.getImpact(x, y))
			    {
				    if (inventoryPageIndex < 9)
				    {
					    inventoryPageIndex++;
					    btnPageIndex.Text = (inventoryPageIndex + 1) + "/10";
				    }
			    }
			    else if (btnInfo.getImpact(x, y))
			    {
				    if (isSelectedItemSlotInPartyInventoryRange())
				    {				
					    ItemRefs itRef = GetCurrentlySelectedItemRefs();
					    if (itRef == null) { return;}
	            	    Item it = gv.cc.getItemByResRef(itRef.resref);
	            	    if (it == null) {return;}
					    gv.sf.ShowFullDescription(it);
				    }				
			    }
			    else if (btnAction.getImpact(x, y))
			    {
				    if (itemSelectorType.Equals("container"))
                    {
                        //TAKE ALL                        
                        foreach (ItemRefs s in thisItemRefs)
                        {
                    	    gv.mod.partyInventoryRefsList.Add(s.DeepCopy());                        
                        }
                	    thisItemRefs.Clear();
                        gv.screenType = "main";	
                    }
                    else if (itemSelectorType.Equals("equip"))
                    {
                        switchEquipment();
                        if (callingScreen.Equals("party"))
                        {
                            gv.screenType = "party";
                        }
                        else if (callingScreen.Equals("combatParty"))
                        {
                            gv.screenType = "combatParty";
                        }
                    }
					doCleanUp();						
			    }
                else if (btnExit.getImpact(x, y))
                {
                    if (itemSelectorType.Equals("container"))
                    {
                        gv.screenType = "main";
                    }
                    else if (itemSelectorType.Equals("equip"))
                    {
                        if (callingScreen.Equals("party"))
                        {
                            gv.screenType = "party";
                        }
                        else if (callingScreen.Equals("combatParty"))
                        {
                            gv.screenType = "combatParty";
                        }
                    } 
                    doCleanUp();
                }
                else if (((itemSelectorType.Equals("container")) || (itemSelectorType.Equals("equip"))) && (btnAction2.getImpact(x, y)))
                {
                    if (itemSelectorType.Equals("container"))
                    {
                        ItemRefs itRef = GetCurrentlySelectedItemRefs();
                        gv.mod.partyInventoryRefsList.Add(itRef.DeepCopy());
                        thisItemRefs.Remove(itRef);
                    }
                    else if (itemSelectorType.Equals("equip"))
                    {
                        unequipItem();
                        if (callingScreen.Equals("party"))
                        {
                            gv.screenType = "party";
                        }
                        else if (callingScreen.Equals("combatParty"))
                        {
                            gv.screenType = "combatParty";
                        }
                    }
                    doCleanUp();
                    
                }
			    break;		
		    }
	    }
        public void switchEquipment()
        {
            Player pc = gv.mod.playerList[gv.cc.partyScreenPcIndex];
            if (GetCurrentlySelectedItemRefs().resref.Equals("none"))
            {
                return;
            }
            if (gv.cc.partyItemSlotIndex == 0) //Main Hand
            {
                if (!pc.MainHandRefs.resref.Equals("none"))
                {
                    //move currently equipped item to the party inventory (list and taglist)
                    gv.mod.partyInventoryRefsList.Add(pc.MainHandRefs.DeepCopy());
                    //place the item into the main hand
                    pc.MainHandRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    //remove the item from the party inventory
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
                else //there was no item equipped so add item to main-hand but no need to move anything to party inventory
                {
                    pc.MainHandRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
                //if the item being equipped is a two-handed weapon, remove the item in off-hand if exists and place in inventory
                if (gv.cc.getItemByResRef(pc.MainHandRefs.resref).twoHanded)
                {
                    if (!pc.OffHandRefs.resref.Equals("none"))
                    {
                        gv.mod.partyInventoryRefsList.Add(pc.OffHandRefs.DeepCopy());
                        pc.OffHandRefs = new ItemRefs();

                        gv.sf.MessageBoxHtml("Equipping a two-handed weapon, removing item from off-hand and placing it in the party's inventory.");
                    }
                }
                //if the item is a ranged weapon that uses ammo, check ammo slot to see if need to remove ammo not this type
                Item itMH = gv.cc.getItemByResRef(pc.MainHandRefs.resref);
                Item itA = gv.cc.getItemByResRef(pc.AmmoRefs.resref);
                if ((itA != null) && (itMH != null))
                {
                    if ((itMH.category.Equals("Ranged")) && (!itMH.ammoType.Equals("none")) && (itMH.ammoType.Equals(itA.ammoType)))
                    {
                        //compatible ammo so leave as is
                    }
                    else //ammo not compatible so remove ItemRefs
                    {
                        pc.AmmoRefs = new ItemRefs();
                        gv.sf.MessageBoxHtml("Currently assigned ammo is not compatible with this weapon, unassigning ammo.");
                    }
                }
            }
            else if (gv.cc.partyItemSlotIndex == 1) //Head
            {
                // if equip slot has an item, move it to inventory first
                if (!pc.HeadRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.HeadRefs.DeepCopy());
                    pc.HeadRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
                else //equip slot was empty
                {
                    pc.HeadRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
            }
            else if (gv.cc.partyItemSlotIndex == 2) //Neck
            {
                // if equip slot has an item, move it to inventory first
                if (!pc.NeckRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.NeckRefs.DeepCopy());
                    pc.NeckRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
                else //equip slot was empty
                {
                    pc.NeckRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
            }
            else if (gv.cc.partyItemSlotIndex == 3) //Off Hand
            {
                if (!pc.OffHandRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.OffHandRefs.DeepCopy());
                    pc.OffHandRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
                else
                {
                    pc.OffHandRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
            }
            else if (gv.cc.partyItemSlotIndex == 4)//Ring
            {
                if (!pc.RingRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.RingRefs.DeepCopy());
                    pc.RingRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
                else
                {
                    pc.RingRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
            }
            else if (gv.cc.partyItemSlotIndex == 5) //Body
            {
                // if equip slot has an item, move it to inventory first
                if (!pc.BodyRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.BodyRefs.DeepCopy());
                    pc.BodyRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
                else //equip slot was empty
                {
                    pc.BodyRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
            }
            else if (gv.cc.partyItemSlotIndex == 6) //Feet
            {
                // if equip slot has an item, move it to inventory first
                if (!pc.FeetRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.FeetRefs.DeepCopy());
                    pc.FeetRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
                else //equip slot was empty
                {
                    pc.FeetRefs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
            }
            else if (gv.cc.partyItemSlotIndex == 7) //Ring2
            {
                if (!pc.Ring2Refs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.Ring2Refs.DeepCopy());
                    pc.Ring2Refs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
                else
                {
                    pc.Ring2Refs = GetCurrentlySelectedItemRefs().DeepCopy();
                    gv.mod.partyInventoryRefsList.Remove(GetCurrentlySelectedItemRefs());
                }
            }
            else if (gv.cc.partyItemSlotIndex == 8) //Ammo
            {
                // if equip slot has an ammo, no need to move it to inventory since it is only a ref            			
                pc.AmmoRefs = GetCurrentlySelectedItemRefs().DeepCopy();
            }
        }
        public void unequipItem()
        {
            Player pc = gv.mod.playerList[gv.cc.partyScreenPcIndex];
            if (gv.cc.partyItemSlotIndex == 0) //Main Hand
            {
                // if equip slot has an item, move it to inventory            		
                if (!pc.MainHandRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.MainHandRefs.DeepCopy());
                    pc.MainHandRefs = new ItemRefs();
                }
            }
            else if (gv.cc.partyItemSlotIndex == 1) //Head
            {
                // if equip slot has an item, move it to inventory first
                if (!pc.HeadRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.HeadRefs.DeepCopy());
                    pc.HeadRefs = new ItemRefs();
                }
            }
            else if (gv.cc.partyItemSlotIndex == 2) //Neck
            {
                // if equip slot has an item, move it to inventory first
                if (!pc.NeckRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.NeckRefs.DeepCopy());
                    pc.NeckRefs = new ItemRefs();
                }
            }
            else if (gv.cc.partyItemSlotIndex == 3) //Off Hand
            {
                if (!pc.OffHandRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.OffHandRefs.DeepCopy());
                    pc.OffHandRefs = new ItemRefs();
                }
            }
            else if (gv.cc.partyItemSlotIndex == 4) //Ring
            {
                if (!pc.RingRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.RingRefs.DeepCopy());
                    pc.RingRefs = new ItemRefs();
                }
            }
            else if (gv.cc.partyItemSlotIndex == 5) //Body
            {
                // if equip slot has an item, move it to inventory first
                if (!pc.BodyRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.BodyRefs.DeepCopy());
                    pc.BodyRefs = new ItemRefs();
                }
            }
            else if (gv.cc.partyItemSlotIndex == 6) //Feet
            {
                // if equip slot has an item, move it to inventory first
                if (!pc.FeetRefs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.FeetRefs.DeepCopy());
                    pc.FeetRefs = new ItemRefs();
                }
            }
            else if (gv.cc.partyItemSlotIndex == 7) //Ring2
            {
                if (!pc.Ring2Refs.resref.Equals("none"))
                {
                    gv.mod.partyInventoryRefsList.Add(pc.Ring2Refs.DeepCopy());
                    pc.Ring2Refs = new ItemRefs();
                }
            }
            else if (gv.cc.partyItemSlotIndex == 8) //Ammo
            {
                // if equip slot has an item, move it to inventory first
                pc.AmmoRefs = new ItemRefs();
            }
        }

	    public void doCleanUp()
	    {
		    btnInventorySlot.Clear();
		    btnInventoryLeft = null;
		    btnInventoryRight = null;
		    btnPageIndex = null;
		    btnInfo = null;
		    btnAction = null;
            btnExit = null;
            btnAction2 = null;
	    }
	
	    public int GetIndex()
	    {
		    return inventorySlotIndex + (inventoryPageIndex * slotsPerPage);
	    }	
	    public ItemRefs GetCurrentlySelectedItemRefs()
	    {
            return thisItemRefs[GetIndex()];
	    }
	    public bool isSelectedItemSlotInPartyInventoryRange()
	    {
            return GetIndex() < thisItemRefs.Count;
	    }
    }
}
