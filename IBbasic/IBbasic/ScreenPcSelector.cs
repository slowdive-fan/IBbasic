﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ScreenPcSelector
    {
        //public Module gv.mod;
        public GameView gv;

        public List<IbbButton> btnPartyIndex = new List<IbbButton>();
        private IbbButton btnReturn = null;

        public string pcSelectorType = "spellcaster"; //spellcaster, target, itemuser
        public string callingScreen = "main"; //main, party, inventory        
        public int pcSelectorPcIndex = 0;
        
        public ScreenPcSelector(Module m, GameView g)
        {
            //gv.mod = m;
            gv = g;
            setControlsStart();
        }

        public void setControlsStart()
        {
            int pW = (int)((float)gv.screenWidth / 100.0f);
            int pH = (int)((float)gv.screenHeight / 100.0f);
            int padW = gv.uiSquareSize / 6;
                        
            if (btnReturn == null)
            {
                btnReturn = new IbbButton(gv, 1.0f);
                btnReturn.Text = "RETURN SELECTED";
                btnReturn.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
                btnReturn.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
                btnReturn.X = (gv.screenWidth / 2) - (int)(170.0f * gv.scaler / 2.0f);
                btnReturn.Y = 10 * gv.uiSquareSize + pH * 2;
                btnReturn.Height = (int)(50 * gv.scaler);
                btnReturn.Width = (int)(170 * gv.scaler);
            }            

            for (int x = 0; x < 6; x++)
            {
                IbbButton btnNew = new IbbButton(gv, 1.0f);
                btnNew.Img = "item_slot"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.item_slot);
                btnNew.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
                btnNew.X = ((x) * gv.uiSquareSize) + (padW * (x + 1));
                btnNew.Y = pH * 2;
                btnNew.Height = (int)(50 * gv.scaler);
                btnNew.Width = (int)(50 * gv.scaler);

                btnPartyIndex.Add(btnNew);
            }
        }

        //PARTY SCREEN
        public void redrawPcSelector()
        {

            if (pcSelectorPcIndex >= gv.mod.playerList.Count)
            {
                pcSelectorPcIndex = 0;
            }
            Player pc = gv.mod.playerList[pcSelectorPcIndex];
            gv.sf.UpdateStats(pc);

            int pW = (int)((float)gv.screenWidth / 100.0f);
            int pH = (int)((float)gv.screenHeight / 100.0f);
            int padH = gv.uiSquareSize / 6;
            int locY = 0;
            int locX = pW * 4;
            int textH = (int)gv.fontHeight;
            int spacing = textH;
            int tabX = pW * 50;
            int tabX2 = pW * 70;
            int leftStartY = btnPartyIndex[0].Y + btnPartyIndex[0].Height + (pH * 4);

            //DRAW EACH PC BUTTON
            int cntPCs = 0;
            foreach (IbbButton btn in btnPartyIndex)
            {
                if (cntPCs < gv.mod.playerList.Count)
                {
                    if (cntPCs == pcSelectorPcIndex) { btn.glowOn = true; }
                    else { btn.glowOn = false; }
                    btn.Draw();
                }
                cntPCs++;
            }

            //DRAW LEFT STATS
            //name            
            gv.DrawText("Name: " + pc.name, locX, locY += leftStartY, "wh");

            //race
            gv.DrawText("Race: " + gv.cc.getRace(pc.raceTag).name, locX, locY += spacing, "wh");

            //gender
            if (pc.isMale)
            {
                gv.DrawText("Gender: Male", locX, locY += spacing, "wh");
            }
            else
            {
                gv.DrawText("Gender: Female", locX, locY += spacing, "wh");
            }

            //class
            gv.DrawText("Class: " + gv.cc.getPlayerClass(pc.classTag).name, locX, locY += spacing, "wh");
            gv.DrawText("Level: " + pc.classLevel, locX, locY += spacing, "wh");
            gv.DrawText("XP: " + pc.XP + "/" + pc.XPNeeded, locX, locY += spacing, "wh");
            gv.DrawText("---------------", locX, locY += spacing, "wh");
            
            //DRAW RIGHT STATS
            int actext = 0;
            if (gv.mod.ArmorClassAscending) { actext = pc.AC; }
            else { actext = 20 - pc.AC; }
            locY = 0;
            gv.DrawText("STR: " + pc.strength, tabX, locY += leftStartY, "wh");
            gv.DrawText("AC: " + actext, tabX2, locY, "wh");
            gv.DrawText("DEX: " + pc.dexterity, tabX, locY += spacing, "wh");
            gv.DrawText("HP: " + pc.hp + "/" + pc.hpMax, tabX2, locY, "wh");
            gv.DrawText("CON: " + pc.constitution, tabX, locY += spacing, "wh");
            gv.DrawText("SP: " + pc.sp + "/" + pc.spMax, tabX2, locY, "wh");
            gv.DrawText("INT: " + pc.intelligence, tabX, locY += spacing, "wh");
            gv.DrawText("BAB: " + pc.baseAttBonus, tabX2, locY, "wh");
            gv.DrawText("WIS: " + pc.wisdom, tabX, locY += spacing, "wh");
            gv.DrawText("CHA: " + pc.charisma, tabX, locY += spacing, "wh");
                        
            btnReturn.Draw();
        }
        public void onTouchPcSelector(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnReturn.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (btnReturn.getImpact(x, y))
                    {
                        btnReturn.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnReturn.glowOn = false;

                    Player pc = gv.mod.playerList[pcSelectorPcIndex];

                    if (btnReturn.getImpact(x, y))
                    {
                        if (pcSelectorType.Equals("spellcaster"))
                        {
                            gv.screenType = "main";
                        }
                    }                    
                    for (int j = 0; j < gv.mod.playerList.Count; j++)
                    {
                        if (btnPartyIndex[j].getImpact(x, y))
                        {
                            pcSelectorPcIndex = j;
                        }
                    }                    
                    break;
            }
        }        
    }
}
