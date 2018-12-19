using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ScreenPartyRoster
    {
	    //public Module gv.mod;
	    public GameView gv;

	    public List<IbbButton> btnPartyIndex = new List<IbbButton>();
        public List<IbbButton> btnPartyRosterIndex = new List<IbbButton>();
	    private IbbButton btnDown = null;
	    private IbbButton btnUp = null;
	    private IbbButton btnHelp = null;
	    private IbbButton btnReturn = null;
	    private bool dialogOpen = false;
	    private int partyScreenPcIndex = 0;
	    private int partyRosterPcIndex = 0;
	    private bool lastClickedPlayerList = true;
	    private string stringMessagePartyRoster = "";

	    public ScreenPartyRoster(Module m, GameView g)
	    {
		    //gv.mod = m;
		    gv = g;
		    setControlsStart();
		    stringMessagePartyRoster = gv.cc.loadTextToString("MessagePartyRoster.txt");
	    }    
	    public void refreshPlayerTokens()
	    {
		    int cntPCs = 0;
		    foreach (IbbButton btn in btnPartyIndex)
		    {
			    if (cntPCs < gv.mod.playerList.Count)
			    {
                    //gv.cc.DisposeOfBitmap(ref btn.Img2);
                    btn.Img2 = gv.mod.playerList[cntPCs].tokenFilename;						
			    }
			    else
			    {
				    btn.Img2 = null;
			    }
			    cntPCs++;
		    }
	    }
        public void refreshRosterPlayerTokens()
        {
            int cntPCs = 0;
            foreach (IbbButton btn in btnPartyRosterIndex)
            {
                if (cntPCs < gv.mod.partyRosterList.Count)
                {
                    //gv.cc.DisposeOfBitmap(ref btn.Img2);
                    btn.Img2 = gv.mod.partyRosterList[cntPCs].tokenFilename;
                }
                else
                {
                    btn.Img2 = null;
                }
                cntPCs++;
            }
        }

	    public void setControlsStart()
	    {		
    	    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		    int padW = gv.uiSquareSize/6;
            
		    for (int x = 0; x < gv.mod.MaxPartySize; x++)
		    {
			    IbbButton btnNew = new IbbButton(gv, 1.0f);
                //gv.cc.DisposeOfBitmap(ref btnNew.Img);
                btnNew.Img = "item_slot"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.item_slot);
                //gv.cc.DisposeOfBitmap(ref btnNew.Glow);
                btnNew.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnNew.X = ((x+2) * gv.uiSquareSize) + (padW * (x+1));
			    btnNew.Y = (gv.uiSquareSize / 2) + (pH * 1);
                btnNew.Height = (int)(gv.ibbheight * gv.scaler);
                btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);	
			
			    btnPartyIndex.Add(btnNew);
		    }
            for (int x = 0; x < 6; x++)
            {
                IbbButton btnNew = new IbbButton(gv, 1.0f);
                //gv.cc.DisposeOfBitmap(ref btnNew.Img);
                btnNew.Img = "item_slot"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.item_slot);
                //gv.cc.DisposeOfBitmap(ref btnNew.Glow);
                btnNew.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
                btnNew.X = ((x+2) * gv.uiSquareSize) + (padW * (x+1));
                btnNew.Y = (3 * gv.uiSquareSize) + (pH * 2);
                btnNew.Height = (int)(gv.ibbheight * gv.scaler);
                btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);
                btnPartyRosterIndex.Add(btnNew);
            }
            for (int x = 0; x < 6; x++)
            {
                IbbButton btnNew = new IbbButton(gv, 1.0f);
                //gv.cc.DisposeOfBitmap(ref btnNew.Img);
                btnNew.Img = "item_slot"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.item_slot);
                //gv.cc.DisposeOfBitmap(ref btnNew.Glow);
                btnNew.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
                btnNew.X = ((x+2) * gv.uiSquareSize) + (padW * (x + 1));
                btnNew.Y = (4 * gv.uiSquareSize) + (pH * 3);
                btnNew.Height = (int)(gv.ibbheight * gv.scaler);
                btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);
                btnPartyRosterIndex.Add(btnNew);
            }
            if (btnDown == null)
            {
                btnDown = new IbbButton(gv, 1.0f);
            }
                btnDown.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
                btnDown.Img2 = "ctrl_down_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_left_arrow);
                btnDown.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnDown.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2) - (pW * 1);
			    btnDown.Y = 2 * gv.uiSquareSize - (pH * 2);
                btnDown.Height = (int)(gv.ibbheight * gv.scaler);
                btnDown.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnUp == null)
            {
                btnUp = new IbbButton(gv, 1.0f);
            }
                btnUp.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnUp.Img2 = "ctrl_up_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_right_arrow);
			    btnUp.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnUp.X = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2) + (pW * 1);
			    btnUp.Y = 2 * gv.uiSquareSize - (pH * 2);
                btnUp.Height = (int)(gv.ibbheight * gv.scaler);
                btnUp.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
            }
			    btnHelp.Text = "HELP";
			    btnHelp.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnHelp.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnHelp.X = 8 * gv.uiSquareSize + padW * 1;
			    btnHelp.Y = 6 * gv.uiSquareSize - (pH * 2);
                btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
                btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnReturn == null)
            {
                btnReturn = new IbbButton(gv, 1.2f);
            }
			    btnReturn.Text = "RETURN";
			    btnReturn.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
			    btnReturn.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
                btnReturn.X = ((gv.uiSquaresInWidth * gv.uiSquareSize / 2)) - (int)(gv.ibbwidthL * gv.scaler / 2.0f);
			    btnReturn.Y = 6 * gv.uiSquareSize - (pH * 2);
                btnReturn.Height = (int)(gv.ibbheight * gv.scaler);
                btnReturn.Width = (int)(gv.ibbwidthL * gv.scaler);			
		    
	    }
	
	    //PARTY SCREEN
        public void redrawPartyRoster(SKCanvas c)
        {            
    	    if (partyScreenPcIndex >= gv.mod.playerList.Count)
    	    {
    		    partyScreenPcIndex = 0;
    	    }
    	    if (partyRosterPcIndex >= gv.mod.partyRosterList.Count)
    	    {
    		    partyRosterPcIndex = 0;
    	    }
    	    Player pc = null;
    	    if ((gv.mod.playerList.Count > 0) && (lastClickedPlayerList))
    	    {
    		    pc = gv.mod.playerList[partyScreenPcIndex];
    	    }
    	    else if ((gv.mod.partyRosterList.Count > 0) && (!lastClickedPlayerList))
    	    {
    		    pc = gv.mod.partyRosterList[partyRosterPcIndex];
    	    }
    	    if (pc != null)
    	    {
    		    gv.sf.UpdateStats(pc);
    	    }
            
    	    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		    int padH = gv.uiSquareSize / 6;
    	    int locY = 0;
    	    int locX = gv.uiSquareSize;
    	    int textH = (int)gv.fontHeight;
            int spacing = textH;
    	    int tabX = gv.uiSquareSize * 5;
    	    int tabX2 = gv.uiSquareSize * 7;
    	    int leftStartY = 4 * gv.uiSquareSize + (pH * 6);

            //Draw screen title name
            string strg = "Current Party Members [" + gv.mod.MaxPartySize + " Maximum]";
            int textWidth = strg.Length * (gv.fontWidth + gv.fontCharSpacing);
            //int textWidth = (int)gv.cc.MeasureString("Current Party Members [" + gv.mod.MaxPartySize + " Maximum]", SharpDX.DirectWrite.FontWeight.Normal, SharpDX.DirectWrite.FontStyle.Normal, gv.drawFontRegHeight);
            int ulX = (gv.screenWidth / 2) - (textWidth / 2);
		    gv.DrawText(c, "Current Party Members [" + gv.mod.MaxPartySize + " Maximum]", ulX, pH * 3, "gy");
		    		    
		    //DRAW EACH PC BUTTON
		    this.refreshPlayerTokens();

		    int cntPCs = 0;
		    foreach (IbbButton btn in btnPartyIndex)
		    {
			    if (cntPCs < gv.mod.playerList.Count)
			    {
				    if (cntPCs == partyScreenPcIndex) {btn.glowOn = true;}
				    else {btn.glowOn = false;}					
				    btn.Draw(c);
			    }
			    cntPCs++;
		    }

            //Draw screen title name
            strg = "Party Roster [Players in Reserve]";
            textWidth = strg.Length * (gv.fontWidth + gv.fontCharSpacing);
            //textWidth = (int)gv.cc.MeasureString("Party Roster [Players in Reserve]", SharpDX.DirectWrite.FontWeight.Normal, SharpDX.DirectWrite.FontStyle.Normal, gv.drawFontRegHeight);
            ulX = (gv.screenWidth / 2) - (textWidth / 2);
		    gv.DrawText(c, "Party Roster [Players in Reserve]", ulX, 3 * gv.uiSquareSize + (pH * 0), "gy");

            //DRAW EACH ROSTER PC BUTTON
            this.refreshRosterPlayerTokens();

            cntPCs = 0;
            foreach (IbbButton btn in btnPartyRosterIndex)
            {
                if (cntPCs < gv.mod.partyRosterList.Count)
                {
                    if (cntPCs == partyRosterPcIndex) {btn.glowOn = true;}
                    else {btn.glowOn = false;}
                    btn.Draw(c);
                }
                cntPCs++;
            }
		
		    btnDown.Draw(c);
		    btnUp.Draw(c);
		    btnHelp.Draw(c);
		    btnReturn.Draw(c);

		    if (pc != null)
		    {
			    //DRAW LEFT STATS
			    gv.DrawText(c, "Name: " + pc.name, locX, locY += leftStartY, "wh");
			    gv.DrawText(c, "Race: " + gv.cc.getRace(pc.raceTag).name, locX, locY += spacing, "wh");
			    if (pc.isMale)
			    {
				    gv.DrawText(c, "Gender: Male", locX, locY += spacing, "wh");
			    }
			    else
			    {
				    gv.DrawText(c, "Gender: Female", locX, locY += spacing, "wh");
			    }
			    gv.DrawText(c, "Class: " + gv.cc.getPlayerClass(pc.classTag).name, locX, locY += spacing, "wh");			
			    gv.DrawText(c, "Level: " + pc.classLevel, locX, locY += spacing, "wh");
			    gv.DrawText(c, "XP: " + pc.XP + "/" + pc.XPNeeded, locX, locY += spacing, "wh");
			    gv.DrawText(c, "---------------", locX, locY += spacing, "wh");
			
			    //draw spells known list
			    string allSpells = "";
			    foreach (string s in pc.knownSpellsTags)
			    {
				    Spell sp = gv.cc.getSpellByTag(s);
				    allSpells += sp.name + ", ";
			    }
			    gv.DrawText(c, gv.cc.getPlayerClass(pc.classTag).spellLabelPlural + ": " + allSpells, locX, locY += spacing, "wh");
			
			    //draw traits known list
			    string allTraits = "";
			    foreach (string s in pc.knownTraitsTags)
			    {
				    Trait tr = gv.cc.getTraitByTag(s);
				    allTraits += tr.name + ", ";
			    }
			    gv.DrawText(c, "Traits: " + allTraits, locX, locY += spacing, "wh");
			
			    //DRAW RIGHT STATS
                int actext = 0;
                if (gv.mod.ArmorClassAscending) { actext = pc.AC; }
                else { actext = 20 - pc.AC; }
			    locY = 0;
			    gv.DrawText(c, "STR: " + pc.strength, tabX, locY += leftStartY, "wh");
			    gv.DrawText(c, "AC: " + actext, tabX2, locY, "wh");
			    gv.DrawText(c, "DEX: " + pc.dexterity, tabX, locY += spacing, "wh");
			    gv.DrawText(c, "HP: " + pc.hp + "/" + pc.hpMax, tabX2, locY, "wh");
			    gv.DrawText(c, "CON: " + pc.constitution, tabX, locY += spacing, "wh");
			    gv.DrawText(c, "SP: " + pc.sp + "/" + pc.spMax, tabX2, locY, "wh");
			    gv.DrawText(c, "INT: " + pc.intelligence, tabX, locY += spacing, "wh");
			    gv.DrawText(c, "BAB: " + pc.baseAttBonus, tabX2, locY, "wh");
                gv.DrawText(c, "WIS: " + pc.wisdom, tabX, locY += spacing, "wh");
                gv.DrawText(c, "CHA: " + pc.charisma, tabX, locY += spacing, "wh");
		    }
            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox(c);
            }
        }
        public void onTouchPartyRoster(int eX, int eY, MouseEventType.EventType eventType)
	    {
		    btnDown.glowOn = false;
		    btnUp.glowOn = false;
		    btnHelp.glowOn = false;
		    btnReturn.glowOn = false;
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

                if (btnDown.getImpact(x, y))
			    {
				    btnDown.glowOn = true;
			    }
			    else if (btnUp.getImpact(x, y))
			    {
				    btnUp.glowOn = true;
			    }
			    else if (btnHelp.getImpact(x, y))
			    {
				    btnHelp.glowOn = true;
			    }			
			    else if (btnReturn.getImpact(x, y))
			    {
				    btnReturn.glowOn = true;
			    }
			    break;

            case MouseEventType.EventType.MouseUp:
                x = (int)eX;
                y = (int)eY;
			
			    btnDown.glowOn = false;
			    btnUp.glowOn = false;
			    btnHelp.glowOn = false;
			    btnReturn.glowOn = false;

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

                if (btnUp.getImpact(x, y))
			    {
				    //add selected PC to partyList and remove from pcList
				    if ((gv.mod.partyRosterList.Count > 0) && (gv.mod.playerList.Count < gv.mod.MaxPartySize))
				    {
					    Player copyPC = gv.mod.partyRosterList[partyRosterPcIndex].DeepCopy();
                        //gv.cc.DisposeOfBitmap(ref copyPC.token);
                        //copyPC.token = gv.cc.LoadBitmap(copyPC.tokenFilename);
                        //gv.cc.DisposeOfBitmap(ref copyPC.portrait);
                        //copyPC.portrait = gv.cc.LoadBitmap(copyPC.portraitFilename);
					    copyPC.playerClass = gv.cc.getPlayerClass(copyPC.classTag);
					    copyPC.race = gv.cc.getRace(copyPC.raceTag);
                        //Player copyPC = gv.mod.partyRosterList[partyRosterPcIndex];
					    gv.mod.playerList.Add(copyPC);
                        gv.mod.partyRosterList.RemoveAt(partyRosterPcIndex);
				    }
			    }
			    else if (btnDown.getImpact(x, y))
			    {
				    //remove selected from partyList and add to pcList
				    if (gv.mod.playerList.Count > 0)
				    {
					    Player copyPC = gv.mod.playerList[partyScreenPcIndex].DeepCopy();
                        //gv.cc.DisposeOfBitmap(ref copyPC.token);
                        //copyPC.token = gv.cc.LoadBitmap(copyPC.tokenFilename);
                        //gv.cc.DisposeOfBitmap(ref copyPC.portrait);
                        //copyPC.portrait = gv.cc.LoadBitmap(copyPC.portraitFilename);
					    copyPC.playerClass = gv.cc.getPlayerClass(copyPC.classTag);
					    copyPC.race = gv.cc.getRace(copyPC.raceTag);
                        gv.mod.partyRosterList.Add(copyPC);
					    gv.mod.playerList.RemoveAt(partyScreenPcIndex);
				    }
			    }
			    else if (btnHelp.getImpact(x, y))
			    {
				    tutorialPartyRoster();
			    }
			    else if (btnReturn.getImpact(x, y))
			    {
				    if (gv.mod.playerList.Count > 0)
				    {
                        //check to see if any non-removeable PCs are in roster
                        if (checkForNoneRemovablePcInRoster())
                        {
                            return;
                        }
                        //check to see if mainPc is in party
                        if (checkForMainPc())
                        {
                            gv.screenType = "main";
                        }
                        else
                        {
                            gv.sf.MessageBoxHtml("You must have the Main PC (the first PC you created) in your party before exiting this screen");
                        }
				    }
			    }
			    for (int j = 0; j < gv.mod.playerList.Count; j++)
			    {
				    if (btnPartyIndex[j].getImpact(x, y))
				    {
					    partyScreenPcIndex = j;
					    lastClickedPlayerList = true;
				    }
			    }
                for (int j = 0; j < gv.mod.partyRosterList.Count; j++)
                {
                    if (btnPartyRosterIndex[j].getImpact(x, y))
                    {
                        partyRosterPcIndex = j;
                        lastClickedPlayerList = false;
                    }
                }
			    break;	
		    }
	    }

        public bool checkForMainPc()
        {
            foreach (Player pc in gv.mod.playerList)
            {
                if (pc.mainPc)
                {
                    return true;
                }
            }
            return false;
        }

        public bool checkForNoneRemovablePcInRoster()
        {
            foreach (Player pc in gv.mod.partyRosterList)
            {
                if (pc.nonRemoveablePc)
                {
                    gv.sf.MessageBoxHtml(pc.name + " must be in the active party before exiting this screen");
                    return true;
                }
            }
            return false;
        }

        public void tutorialPartyRoster()
        {
    	    gv.sf.MessageBoxHtml(this.stringMessagePartyRoster);
        }
    }
}
