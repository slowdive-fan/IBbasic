﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ScreenTitle 
    {
	    //public Module gv.mod;
	    public GameView gv;
	
	    private IbbButton btnNewGame = null;
	    private IbbButton btnLoadSavedGame = null;
	    private IbbButton btnPlayerGuide = null;
	    private IbbButton btnBeginnerGuide = null;
	    private IbbButton btnAbout = null;        

        public ScreenTitle(Module m, GameView g)
	    {
		    //gv.mod = m;
		    gv = g;
		    setControlsStart();
	    }
	
	    public void setControlsStart()
	    {
            int pH = (int)((float)gv.screenHeight / 100.0f);

            if (btnNewGame == null)
            {
                btnNewGame = new IbbButton(gv, 1.0f);
            }
			    btnNewGame.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
			    btnNewGame.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
			    btnNewGame.Text = "New Game";
                btnNewGame.X = (gv.uiSquareSize * gv.uiSquaresInWidth / 2) - (int)(gv.ibbwidthL * gv.scaler / 2.0f);
                btnNewGame.Y = (1 * gv.uiSquareSize) + (2 * pH) - (gv.uiSquareSize / 2);
                btnNewGame.Height = (int)(gv.ibbheight * gv.scaler);
                btnNewGame.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnLoadSavedGame == null)
            {
                btnLoadSavedGame = new IbbButton(gv, 1.0f);
            }
			    btnLoadSavedGame.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
			    btnLoadSavedGame.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
			    btnLoadSavedGame.Text = "Load Saved Game";
                btnLoadSavedGame.X = (gv.uiSquareSize * gv.uiSquaresInWidth / 2) - (int)(gv.ibbwidthL * gv.scaler / 2.0f);
                btnLoadSavedGame.Y = (2 * gv.uiSquareSize) + (4 * pH) - (gv.uiSquareSize / 2);
                btnLoadSavedGame.Height = (int)(gv.ibbheight * gv.scaler);
                btnLoadSavedGame.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnPlayerGuide == null)
            {
                btnPlayerGuide = new IbbButton(gv, 1.0f);
            }
			    btnPlayerGuide.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
			    btnPlayerGuide.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
			    btnPlayerGuide.Text = "Player's Guide";
                btnPlayerGuide.X = (gv.uiSquareSize * gv.uiSquaresInWidth / 2) - (int)(gv.ibbwidthL * gv.scaler / 2.0f);
                btnPlayerGuide.Y = (3 * gv.uiSquareSize) + (6 * pH) - (gv.uiSquareSize / 2);
                btnPlayerGuide.Height = (int)(gv.ibbheight * gv.scaler);
                btnPlayerGuide.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnBeginnerGuide == null)
            {
                btnBeginnerGuide = new IbbButton(gv, 1.0f);
            }
			    btnBeginnerGuide.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
			    btnBeginnerGuide.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
			    btnBeginnerGuide.Text = "Beginner's Guide";
                btnBeginnerGuide.X = (gv.uiSquareSize * gv.uiSquaresInWidth / 2) - (int)(gv.ibbwidthL * gv.scaler / 2.0f);
                btnBeginnerGuide.Y = (4 * gv.uiSquareSize) + (8 * pH) - (gv.uiSquareSize / 2);
                btnBeginnerGuide.Height = (int)(gv.ibbheight * gv.scaler);
                btnBeginnerGuide.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnAbout == null)
            {
                btnAbout = new IbbButton(gv, 1.0f);
            }
			    btnAbout.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
			    btnAbout.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
			    btnAbout.Text = "Credits";
                btnAbout.X = (gv.uiSquareSize * gv.uiSquaresInWidth / 2) - (int)(gv.ibbwidthL * gv.scaler / 2.0f);
                btnAbout.Y = (5 * gv.uiSquareSize) + (10 * pH) - (gv.uiSquareSize / 2);
                btnAbout.Height = (int)(gv.ibbheight * gv.scaler);
                btnAbout.Width = (int)(gv.ibbwidthL * gv.scaler);			
		    
	    }

	    //TITLE SCREEN  
        public void redrawTitle()
        {
            setControlsStart();       
    	    //DRAW TITLE SCREEN
            float dstHeight = ((float)gv.screenWidth / (float)gv.cc.GetFromBitmapList(gv.mod.titleImageName).Width) * (float)gv.cc.GetFromBitmapList(gv.mod.titleImageName).Height;
            //do narration with image setup    	
            IbRect src = new IbRect(0, 0, gv.cc.GetFromBitmapList(gv.mod.titleImageName).Width, gv.cc.GetFromBitmapList(gv.mod.titleImageName).Height);
            IbRect dst = new IbRect(0 - gv.oXshift, 0 - gv.oYshift, gv.screenWidth, (int)dstHeight);
            gv.DrawBitmap(gv.cc.GetFromBitmapList(gv.mod.titleImageName), src, dst);

            //Draw This Module's Version Number
            int xLoc = (gv.uiSquareSize * gv.uiSquaresInWidth / 2) - (1 * gv.fontWidth);
            int pH = (int)((float)gv.screenHeight / 100.0f);
            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    gv.DrawText("v" + gv.mod.moduleVersion, xLoc + x, (6 * gv.uiSquareSize) + (pH * 4) + y, "bk");
                }
            }
            gv.DrawText("v" + gv.mod.moduleVersion, xLoc, (6 * gv.uiSquareSize) + (pH * 4), "wh");
            
            drawTitleControls();
            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }
        public void drawTitleControls()
	    {    	
		    btnNewGame.Draw();		
		    btnLoadSavedGame.Draw();		
		    btnPlayerGuide.Draw();
		    btnBeginnerGuide.Draw();           
		    btnAbout.Draw();
	    }
        public void onTouchTitle(int eX, int eY, MouseEventType.EventType eventType)
	    {
    	    btnNewGame.glowOn = false;
		    btnLoadSavedGame.glowOn = false;
		    btnPlayerGuide.glowOn = false;
		    btnBeginnerGuide.glowOn = false;				
		    btnAbout.glowOn = false;
            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }

            switch (eventType)
		    {
		    case MouseEventType.EventType.MouseUp:
                int x = (int)eX;
                int y = (int)eY;
				
			    btnNewGame.glowOn = false;
			    btnLoadSavedGame.glowOn = false;
			    btnAbout.glowOn = false;
			    btnPlayerGuide.glowOn = false;
			    btnBeginnerGuide.glowOn = false;
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
                else
                {
                    if (btnNewGame.getImpact(x, y))
                    {
                        gv.PlaySound("btn_click");
                        gv.mod.uniqueSessionIdNumberTag = gv.sf.RandInt(1000000) + "";
                        gv.TrackerSendEvent(":NEWGAME:" + gv.mod.moduleName, "none", false);
                        if (gv.mod.mustUsePreMadePC)
                        {
                            //no spell selection offered
                            gv.showMessageBox = true;
                            gv.cc.tutorialMessageMainMap();
                            gv.screenType = "main";
                            gv.cc.doUpdate();
                        }
                        else
                        {
                            gv.screenType = "partyBuild";
                            gv.screenPartyBuild.loadPlayerList();
                        }
                    }
                    else if (btnLoadSavedGame.getImpact(x, y))
                    {
                        gv.PlaySound("btn_click");
                        gv.TrackerSendEvent(":LOADSAVE:" + gv.mod.moduleName, "none", false);
                        if (gv.cc.slot5.Equals(""))
                        {
                            //Toast.makeText(gv.gameContext, "Still Loading Data... try again in a second", Toast.LENGTH_SHORT).show();
                        }
                        else
                        {
                            gv.cc.doLoadSaveGameSetupDialog();
                        }
                    }
                    else if (btnPlayerGuide.getImpact(x, y))
                    {
                        gv.PlaySound("btn_click");
                        gv.showMessageBox = true;
                        gv.cc.tutorialPlayersGuide();
                    }
                    else if (btnBeginnerGuide.getImpact(x, y))
                    {
                        gv.PlaySound("btn_click");
                        gv.showMessageBox = true;
                        gv.cc.tutorialBeginnersGuide();
                    }
                    else if (btnAbout.getImpact(x, y))
                    {
                        gv.PlaySound("btn_click");
                        gv.showMessageBox = true;
                        gv.cc.doAboutDialog();
                    }
                }					
			    break;

            case MouseEventType.EventType.MouseDown:
            case MouseEventType.EventType.MouseMove:
                x = (int)eX;
                y = (int)eY;

                if (gv.showMessageBox)
                {
                    if (gv.messageBox.btnReturn.getImpact(x, y))
                    {
                        gv.messageBox.btnReturn.glowOn = true;
                    }
                    return;
                }
                else
                {
                    if (btnNewGame.getImpact(x, y))
                    {
                        btnNewGame.glowOn = true;
                    }
                    else if (btnLoadSavedGame.getImpact(x, y))
                    {
                        btnLoadSavedGame.glowOn = true;
                    }
                    else if (btnAbout.getImpact(x, y))
                    {
                        btnAbout.glowOn = true;
                    }
                    else if (btnPlayerGuide.getImpact(x, y))
                    {
                        btnPlayerGuide.glowOn = true;
                    }
                    else if (btnBeginnerGuide.getImpact(x, y))
                    {
                        btnBeginnerGuide.glowOn = true;
                    }
                }
			    break;		
		    }
	    }
    }
}
