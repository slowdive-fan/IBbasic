﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ScreenJournal 
    {

	    //public Module gv.mod;
	    public GameView gv;
	    private int journalScreenQuestIndex = 0;
	    private int journalScreenEntryIndex = 0;	
	    //private Bitmap journalBack;
	    private IbbButton btnReturnJournal = null;
	    public IbbButton ctrlUpArrow = null;
	    public IbbButton ctrlDownArrow = null;
	    public IbbButton ctrlLeftArrow = null;
	    public IbbButton ctrlRightArrow = null;
        private IbbHtmlTextBox description;
	
	    public ScreenJournal(Module m, GameView g)
	    {
		    //gv.mod = m;
		    gv = g;
	    }
	    public void setControlsStart()
	    {		
		    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		    int padW = gv.uiSquareSize/6;		
		    int xShift = gv.uiSquareSize / 2;
		    int yShift = gv.uiSquareSize / 2;

            description = new IbbHtmlTextBox(gv, 320, 100, 500, 300);
            description.showBoxBorder = false;

            if (ctrlUpArrow == null)
            {
                ctrlUpArrow = new IbbButton(gv, 1.0f);
            }
			    ctrlUpArrow.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    ctrlUpArrow.Img2 = "ctrl_up_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_up_arrow);
			    ctrlUpArrow.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.arrow_glow);
			    ctrlUpArrow.X = 9 * gv.uiSquareSize;
			    ctrlUpArrow.Y = (int)(0.5 * gv.uiSquareSize);
                ctrlUpArrow.Height = (int)(gv.ibbheight * gv.scaler);
                ctrlUpArrow.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (ctrlDownArrow == null)
            {
                ctrlDownArrow = new IbbButton(gv, 1.0f);
            }
			    ctrlDownArrow.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    ctrlDownArrow.Img2 = "ctrl_down_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_down_arrow);
			    ctrlDownArrow.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.arrow_glow);
			    ctrlDownArrow.X = 9 * gv.uiSquareSize;
			    ctrlDownArrow.Y = (int)(1.5 * gv.uiSquareSize) + pH * 1;
                ctrlDownArrow.Height = (int)(gv.ibbheight * gv.scaler);
                ctrlDownArrow.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (ctrlLeftArrow == null)
            {
                ctrlLeftArrow = new IbbButton(gv, 1.0f);
            }
			    ctrlLeftArrow.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    ctrlLeftArrow.Img2 = "ctrl_left_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_left_arrow);
			    ctrlLeftArrow.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.arrow_glow);
			    ctrlLeftArrow.X = 6 * gv.uiSquareSize + xShift;
			    ctrlLeftArrow.Y = (int)(1.75 * gv.uiSquareSize) + pH * 1;
                ctrlLeftArrow.Height = (int)(gv.ibbheight * gv.scaler);
                ctrlLeftArrow.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (ctrlRightArrow == null)
            {
                ctrlRightArrow = new IbbButton(gv, 1.0f);
            }
			    ctrlRightArrow.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    ctrlRightArrow.Img2 = "ctrl_right_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_right_arrow);
			    ctrlRightArrow.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.arrow_glow);
			    ctrlRightArrow.X = 7 * gv.uiSquareSize + pW * 2 + xShift;
			    ctrlRightArrow.Y = (int)(1.75 * gv.uiSquareSize) + pH * 1;
                ctrlRightArrow.Height = (int)(gv.ibbheight * gv.scaler);
                ctrlRightArrow.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnReturnJournal == null)
            {
                btnReturnJournal = new IbbButton(gv, 1.2f);
            }
			    btnReturnJournal.Text = "RETURN";
			    btnReturnJournal.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
			    btnReturnJournal.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
                btnReturnJournal.X = ((gv.uiSquaresInWidth * gv.uiSquareSize / 2)) - (int)(gv.ibbwidthL * gv.scaler / 2.0f);
			    btnReturnJournal.Y = 6 * gv.uiSquareSize - pH * 2;
                btnReturnJournal.Height = (int)(gv.ibbheight * gv.scaler);
                btnReturnJournal.Width = (int)(gv.ibbwidthL * gv.scaler);			
		    
		
	    }

        public void redrawJournal()
        {            
    	    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		
    	    int locY = pH * 5;
            int locX = 1 * gv.uiSquareSize;
            int spacing = (int)gv.fontHeight + pH;
            int leftStartY = pH * 4;
    	    int tabStartY = pH * 40;
    	
    	    if (btnReturnJournal == null)
    	    {
    		    setControlsStart();
    	    }
    	
    	    //DRAW BACKGROUND IMAGE
            IbRect src = new IbRect(0, 0, gv.cc.GetFromBitmapList("journalback").Width, gv.cc.GetFromBitmapList("journalback").Height);
            IbRect dst = new IbRect(0, 0, (gv.uiSquaresInWidth) * gv.uiSquareSize, (gv.uiSquaresInHeight) * gv.uiSquareSize);
            gv.DrawBitmap(gv.cc.GetFromBitmapList("journalback"), src, dst);
        
            //MAKE SURE NO OUT OF INDEX ERRORS
    	    if (gv.mod.partyJournalQuests.Count > 0)
    	    {
	    	    if (journalScreenQuestIndex >= gv.mod.partyJournalQuests.Count)
	    	    {
	    		    journalScreenQuestIndex = 0;
	    	    }    	
	    	    if (journalScreenEntryIndex >= gv.mod.partyJournalQuests[journalScreenQuestIndex].Entries.Count)
	    	    {
	    		    journalScreenEntryIndex = 0;
	    	    }
    	    }
			
    	    //DRAW QUESTS
            string color = "bk";
		    gv.DrawText("Active Quests:", locX, locY += leftStartY, "bk");
		    gv.DrawText("--------------", locX, locY += spacing, "bk");
		    if (gv.mod.partyJournalQuests.Count > 0)
    	    {
			    int cnt = 0;
			    foreach (JournalQuest jq in gv.mod.partyJournalQuests)
			    {
                    if (journalScreenQuestIndex == cnt) { color = "gn"; }
				    else { color = "bk"; }	
                    gv.DrawText(jq.Name, locX, locY += spacing, color);
				    cnt++;
			    }
    	    }
		
		    //DRAW QUEST ENTRIES
		    locY = tabStartY;
		    gv.DrawText("Quest Entry:", locX, locY, "bk");
		    gv.DrawText("--------------", locX, locY += spacing, "bk");	
		    if (gv.mod.partyJournalQuests.Count > 0)
    	    {
                //Description
                string textToSpan = "<gy>" + gv.mod.partyJournalQuests[journalScreenQuestIndex].Entries[journalScreenEntryIndex].EntryTitle + "</gy><br>";
                textToSpan += "<bk>" + gv.mod.partyJournalQuests[journalScreenQuestIndex].Entries[journalScreenEntryIndex].EntryText + "</bk>";
	                            
                int yLoc = pH * 18;

                description.tbXloc = locX;
                description.tbYloc = locY + spacing;
                description.tbWidth = 9 * gv.uiSquareSize;
                description.tbHeight = pH * 80;
                description.logLinesList.Clear();
                description.AddHtmlTextToLog(textToSpan);
                description.onDrawLogBox();
    	    }
		
		    //DRAW ALL CONTROLS
		    ctrlUpArrow.Draw();
		    ctrlDownArrow.Draw();
		    ctrlLeftArrow.Draw();
		    ctrlRightArrow.Draw();
		    btnReturnJournal.Draw();
        }
    
	    public void onTouchJournal(int eX, int eY, MouseEventType.EventType eventType)
	    {
		    ctrlUpArrow.glowOn = false;
		    ctrlDownArrow.glowOn = false;
		    ctrlLeftArrow.glowOn = false;
		    ctrlRightArrow.glowOn = false;
		    btnReturnJournal.glowOn = false;
		
		    switch (eventType)
		    {
		    case MouseEventType.EventType.MouseDown:
		    case MouseEventType.EventType.MouseMove:
			    int x = (int) eX;
			    int y = (int) eY;
			    if (ctrlUpArrow.getImpact(x, y))
			    {
				    ctrlUpArrow.glowOn = true;
			    }
			    else if (ctrlDownArrow.getImpact(x, y))
			    {
				    ctrlDownArrow.glowOn = true;
			    }
			    else if (ctrlLeftArrow.getImpact(x, y))
			    {
				    ctrlLeftArrow.glowOn = true;
			    }
			    else if (ctrlRightArrow.getImpact(x, y))
			    {
				    ctrlRightArrow.glowOn = true;
			    }	
			    else if (btnReturnJournal.getImpact(x, y))
			    {
				    btnReturnJournal.glowOn = true;
			    }
			
			    break;
			
		    case MouseEventType.EventType.MouseUp:
			    x = (int) eX;
			    y = (int) eY;
			
			    ctrlUpArrow.glowOn = false;
			    ctrlDownArrow.glowOn = false;
			    ctrlLeftArrow.glowOn = false;
			    ctrlRightArrow.glowOn = false;
			    btnReturnJournal.glowOn = false;
			
			    if (ctrlUpArrow.getImpact(x, y))
			    {
				    if (journalScreenQuestIndex > 0)
				    {
					    journalScreenQuestIndex--;
					    journalScreenEntryIndex = gv.mod.partyJournalQuests[journalScreenQuestIndex].Entries.Count - 1;
				    }
			    }
			    else if (ctrlDownArrow.getImpact(x, y))
			    {
				    if (journalScreenQuestIndex < gv.mod.partyJournalQuests.Count - 1)
				    {
					    journalScreenQuestIndex++;
					    journalScreenEntryIndex = gv.mod.partyJournalQuests[journalScreenQuestIndex].Entries.Count - 1;
				    }
			    }
			    else if (ctrlLeftArrow.getImpact(x, y))
			    {
				    if (journalScreenEntryIndex > 0)
				    {
					    journalScreenEntryIndex--;
				    }
			    }
			    else if (ctrlRightArrow.getImpact(x, y))
			    {
				    if (journalScreenEntryIndex < gv.mod.partyJournalQuests[journalScreenQuestIndex].Entries.Count - 1)
				    {
					    journalScreenEntryIndex++;
				    }
			    }	
			    else if (btnReturnJournal.getImpact(x, y))
			    {
				    gv.screenType = "main";
				    //journalBack = null;
				    btnReturnJournal = null;
				    ctrlUpArrow = null;
				    ctrlDownArrow = null;
				    ctrlLeftArrow = null;
				    ctrlRightArrow = null;
			    }			
			    break;		
		    }
	    }	    
    }
}
