﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ScriptFunctions
    {
        public Module mod;
        public string ActionToTake = "Attack";  //Attack, Cast, Move 
        public Spell SpellToCast = null;        //Spell that the creature is casting, make sure to null out after use
        public Prop ThisProp = null;            //Prop that is calling the current script, convo, or encounter when using 'thisProp' in script or convo, make sure to null out after use
        public Creature ThisCreature = null;    //Creature that is calling the current script, when using 'thisCreature' in script, make sure to null out after use
        public object CombatTarget = null;
        public GameView gv;
        public int spCnt = 0;
        public Random rand;
        public List<object> AoeTargetsList = new List<object>();
        public List<Coordinate> AoeSquaresList = new List<Coordinate>();

        public ScriptFunctions(Module m, GameView g)
        {
            mod = m;
            gv = g;
            rand = new Random();
        }

        public void MessageBox(string message)
        {
            MessageBoxHtml(message);
        }

        public void ShowFullDescription(Item it)
        {
            string textToSpan = "<gy>Description</gy>" + "<BR>";
            textToSpan += "<gn>" + it.name + "</gn><BR>";
            if ((it.category.Equals("Melee")) || (it.category.Equals("Ranged")))
            {
                textToSpan += "Damage: " + it.damageNumDice + "d" + it.damageDie + "+" + it.damageAdder + "<BR>";
                textToSpan += "Attack Bonus: " + it.attackBonus + "<BR>";
                textToSpan += "Attack Range: " + it.attackRange + "<BR>";
                textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                textToSpan += "Two-Handed Weapon: ";
                if (it.twoHanded) { textToSpan += "Yes<BR>"; }
                else { textToSpan += "No<BR>"; }
                textToSpan += "<BR>";
                if (!it.descFull.Equals(""))
                {
                    textToSpan += it.descFull;
                }
                else
                {
                    textToSpan += it.desc;
                }
            }
            else if (!it.category.Equals("General"))
            {
                textToSpan += "AC Bonus: " + it.armorBonus + "<BR>";
                textToSpan += "Max Dex Bonus: " + it.maxDexBonus + "<BR>";
                textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                textToSpan += "<BR>";
                if (!it.descFull.Equals(""))
                {
                    textToSpan += it.descFull;
                }
                else
                {
                    textToSpan += it.desc;
                }
            }
            else if (it.category.Equals("General"))
            {
                textToSpan += "Useable By: " + isUseableBy(it) + "<BR>";
                textToSpan += "<BR>";
                if (!it.descFull.Equals(""))
                {
                    textToSpan += it.descFull;
                }
                else
                {
                    textToSpan += it.desc;
                }
            }
            MessageBoxHtml(textToSpan);
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
                string firstLetter = cls.name.Substring(0, 1);
                foreach (ItemRefs ia in cls.itemsAllowed)
                {
                    string stg = ia.resref;
                    if (stg.Equals(it.resref))
                    {
                        strg += firstLetter + ", ";
                    }
                }
            }*/
            return strg;
        }

        public void MessageBoxHtml(string message)
        {
            gv.messageBox.logLinesList.Clear();
            gv.messageBox.AddHtmlTextToLog(message);
            gv.messageBox.currentTopLineIndex = 0;
            gv.showMessageBox = true;

            //<b> Bold
            //<i> Italics
            //<u> Underline
            //<big> Big
            //<small> Small
            //<font> Font face and color
            //<br> Linefeed
            /*try
            {
                using (IBHtmlMessageBox hmb = new IBHtmlMessageBox(gv, message))
                {
                    var result = hmb.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        //MessageBox.Show("selected OK");
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        //MessageBox.Show("selected Cancel");
                    }
                }
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
            }*/
        }

        /// <summary>
        /// Used for generating a random number between some number and some other number (max must be >= min).
        /// </summary>
        /// <param name="min"> The minimum value that can be found (ex. Random(5, 9); will return a number between 5-9 so 5, 6, 7, 8 or 9 are possible results).</param>
        /// <param name="max"> The maximum value that can be found (ex. Random(5, 9); will return a number between 5-9 so 5, 6, 7, 8 or 9 are possible results).</param>
        public int RandInt(int min, int max)
        {
            //A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue.
            return rand.Next(min, max + 1);
        }
        /// <summary>
        /// Used for generating a random number between 1 and some number.
        /// </summary>
        /// <param name="max">The maximum value that can be used (ex. Random(5); will return a number between 1-5 so 1, 2, 3, 4 or 5 are possible results).</param>
        /// <returns>"returns"</returns>
        public int RandInt(int max)
        {
            return RandInt(1, max);
        }
        public int RandDiceRoll(int numberOfDice, int numberOfSidesOnDie)
        {
            int roll = 0;
            for (int x = 0; x < numberOfDice; x++)
            {
                roll += RandInt(numberOfSidesOnDie);
            }
            return roll;
        }

        public void gaController(string filename, string prm1, string prm2, string prm3, string prm4)
        {
            if (!filename.Equals("none"))
            {
                try
                {
                    if (!filename.EndsWith(".cs")) { filename += ".cs"; }
                    //go through each parm1-4 and replace if GlobalInt variable, GlobalString variable or rand(3-16)
                    string p1 = replaceParameter(prm1);
                    string p2 = replaceParameter(prm2);
                    string p3 = replaceParameter(prm3);
                    string p4 = replaceParameter(prm4);

                    if (filename.Equals("gaSetGlobalInt.cs"))
                    {
                        SetGlobalInt(prm1, p2);
                    }
                    else if (filename.Equals("gaSetLocalInt.cs"))
                    {
                        //check to see if prm1 is thisprop or thisarea
                        if (prm1.Equals("thisprop"))
                        {
                            //find the prop at this location
                            //prm1 = mod.currentArea.getPropByLocation(mod.PlayerLocationX, mod.PlayerLocationY).PropTag;
                        }
                        else if (prm1.Equals("thisarea"))
                        {
                            //use the currentArea
                            prm1 = mod.currentArea.Filename;
                        }
                        SetLocalInt(prm1, prm2, p3);
                    }
                    else if (filename.Equals("gaSetGlobalString.cs"))
                    {
                        SetGlobalString(prm1, p2);
                    }
                    else if (filename.Equals("gaSetLocalString.cs"))
                    {
                        //check to see if prm1 is thisprop or thisarea
                        if (prm1.Equals("thisprop"))
                        {
                            //find the prop at this location
                            //prm1 = mod.currentArea.getPropByLocation(mod.PlayerLocationX, mod.PlayerLocationY).PropTag;
                        }
                        else if (prm1.Equals("thisarea"))
                        {
                            //use the currentArea
                            prm1 = mod.currentArea.Filename;
                        }
                        SetLocalString(prm1, prm2, p3);
                    }
                    else if (filename.Equals("gaGetLocalInt.cs"))
                    {
                        //check to see if prm1 is thisprop or thisarea
                        if (prm1.Equals("thisprop"))
                        {
                            //find the prop at this location
                            //prm1 = mod.currentArea.getPropByLocation(mod.PlayerLocationX, mod.PlayerLocationY).PropTag;
                        }
                        else if (prm1.Equals("thisarea"))
                        {
                            //use the currentArea
                            prm1 = mod.currentArea.Filename;
                        }
                        int i = GetLocalInt(prm1, prm2);
                        SetGlobalInt(prm3, i + "");
                    }
                    else if (filename.Equals("gaGetLocalString.cs"))
                    {
                        //check to see if prm1 is thisprop or thisarea
                        if (prm1.Equals("thisprop"))
                        {
                            //find the prop at this location
                            //prm1 = mod.currentArea.getPropByLocation(mod.PlayerLocationX, mod.PlayerLocationY).PropTag;
                        }
                        else if (prm1.Equals("thisarea"))
                        {
                            //use the currentArea
                            prm1 = mod.currentArea.Filename;
                        }
                        String s = GetLocalString(prm1, prm2);
                        SetGlobalString(prm3, s);
                    }
                    else if (filename.Equals("gaTransformGlobalInt.cs"))
                    {
                        TransformGlobalInt(p1, prm2, p3, prm4);
                    }
                    else if (filename.Equals("gaTransformGlobalString.cs"))
                    {
                        TransformGlobalString(p1, p2, prm3);
                    }
                    else if (filename.Equals("gaGiveItem.cs"))
                    {
                        int parm2 = Convert.ToInt32(p2);
                        GiveItem(p1, parm2);
                    }
                    else if (filename.Equals("gaGiveXP.cs"))
                    {
                        int parm1 = Convert.ToInt32(p1);
                        GiveXP(parm1);
                    }
                    else if (filename.Equals("gaGiveGold.cs"))
                    {
                        int parm1 = Convert.ToInt32(p1);
                        GiveFunds(parm1);
                    }
                    else if (filename.Equals("gaTakeGold.cs"))
                    {
                        int parm1 = Convert.ToInt32(p1);
                        TakeFunds(parm1);
                    }
                    else if (filename.Equals("gaForceRest.cs"))
                    {
                        itForceRest();
                    }
                    else if (filename.Equals("gaForceRestNoRations.cs"))
                    {
                        itForceRestNoRations();
                    }
                    else if (filename.Equals("gaForceRestAndRaiseDead.cs"))
                    {
                        itForceRestAndRaiseDead();
                    }
                    else if (filename.Equals("gaMovePartyToLastLocation.cs"))
                    {
                        gv.mod.PlayerLocationX = gv.mod.PlayerLastLocationX;
                        gv.mod.PlayerLocationY = gv.mod.PlayerLastLocationY;
                    }
                    else if (filename.Equals("gaTakeItem.cs"))
                    {
                        int parm2 = Convert.ToInt32(p2);
                        TakeItem(p1, parm2);
                    }
                    else if (filename.Equals("gaPartyDamage.cs"))
                    {
                        int parm1 = Convert.ToInt32(p1);
                        ApplyPartyDamage(parm1);
                    }
                    else if (filename.Equals("gaRiddle.cs"))
                    {
                        riddle();
                    }
                    else if (filename.Equals("gaDamageWithoutItem.cs"))
                    {
                        int parm1 = Convert.ToInt32(p1);
                        DamageWithoutItem(parm1, p2);
                    }
                    else if (filename.Equals("gaRemovePropByTag.cs"))
                    {
                        //RemovePropByTag(p1);
                    }
                    else if (filename.Equals("gaRemovePropByIndex.cs"))
                    {
                        int parm1 = Convert.ToInt32(p1);
                        //RemovePropByIndex(parm1, prm2);
                    }
                    else if (filename.Equals("gaTransitionPartyToMapLocation.cs"))
                    {                        
                        int parm2 = Convert.ToInt32(p2);
                        int parm3 = Convert.ToInt32(p3);
                        if (gv.mod.currentArea.Filename.Equals(p1))
                        {
                            gv.mod.PlayerLocationX = parm2;
                            gv.mod.PlayerLocationY = parm3;                                
                        }
                        else
                        {
                            gv.cc.doTransitionBasedOnAreaLocation(p1, parm2, parm3);
                        }                        
                    }
                    else if (filename.Equals("gaAddPartyMember.cs"))
                    {
                        AddCharacterToParty(p1);
                    }
                    else if (filename.Equals("gaRemovePartyMember.cs"))
                    {
                        RemoveCharacterFromParty(prm1, p2);
                    }
                    else if (filename.Equals("gaMovePartyMemberToRoster.cs"))
                    {
                        MoveCharacterToRoster(prm1, p2);
                    }
                    else if (filename.Equals("gaMoveRosterMemberToParty.cs"))
                    {
                        MoveCharacterToPartyFromRoster(prm1, p2);
                    }
                    else if (filename.Equals("gaEnableDisableTriggerEvent.cs"))
                    {
                        EnableDisableTriggerEvent(p1, p2, p3, p4);
                    }
                    else if (filename.Equals("gaEnableDisableTrigger.cs"))
                    {
                        EnableDisableTrigger(p1, p2, p3);
                    }
                    else if (filename.Equals("gaDisableTriggerHideImage.cs"))
                    {
                        DisableTriggerHideImage(p1, p2);
                    }
                    else if (filename.Equals("gaShowTriggerImage.cs"))
                    {
                        ShowTriggerImage(p1, p2, p3);
                    }
                    else if (filename.Equals("gaTogglePartyToken.cs"))
                    {
                        TogglePartyToken(p1, p2);
                    }
                    else if (filename.Equals("gaEnableDisableTriggerAtCurrentLocation.cs"))
                    {
                        EnableDisableTriggerAtCurrentLocation(p1);
                    }
                    else if (filename.Equals("gaEnableDisableTriggerEventAtCurrentLocation.cs"))
                    {
                        EnableDisableTriggerEventAtCurrentLocation(prm1, prm2);
                    }
                    else if (filename.Equals("gaAddJournalEntryByTag.cs"))
                    {
                        AddJournalEntry(prm1, prm2);
                    }
                    else if (filename.Equals("gaEndGame.cs"))
                    {
                        gv.resetGame();
                        gv.screenType = "title";
                    }
                    else if (filename.Equals("gaSendTrackingPartyInfo.cs"))
                    {
                        gv.TrackerSendMilestoneEvent(prm1);
                    }
                    else if (filename.Equals("gaSendTrackingCompletedQuest.cs"))
                    {
                        gv.TrackerSendMilestoneEvent(prm1);
                    }
                    else if (filename.Equals("gaPlaySound.cs"))
                    {
                        gv.PlaySound(p1);
                    }
                    else if (filename.Equals("gaKillAllCreatures.cs"))
                    {
                        gv.mod.currentEncounter.encounterCreatureList.Clear();
                        gv.mod.currentEncounter.encounterCreatureRefsList.Clear();
                        gv.screenCombat.checkEndEncounter();
                    }
                    else if (filename.Equals("gaOpenShopByTag.cs"))
                    {
                        gv.screenShop.currentShopTag = p1;
                        gv.screenShop.currentShop = gv.mod.getShopByTag(p1);
                        gv.screenType = "shop";
                    }
                    else if (filename.Equals("gaModifiyShopBuyBackPercentage.cs"))
                    {
                        ModifyBuyBack(p1, prm2, p3);
                    }
                    else if (filename.Equals("gaModifiyShopSellPercentage.cs"))
                    {
                        ModifySellPrice(p1, prm2, p3);
                    }
                    else if (filename.Equals("gaGetPlayerIndexThatIsUsingItem.cs"))
                    {
                        //String val = gv.cc.currentPlayerIndexUsingItem + "";
                        string val = gv.mod.indexOfPCtoLastUseItem + "";
                        SetGlobalInt(prm1, val);
                    }
                    else if (filename.Equals("gaWriteTextToLog.cs"))
                    {
                        gv.cc.addLogText(prm2, p1);
                    }
                    else if (filename.Equals("gaWriteHtmlTextToLog.cs"))
                    {
                        gv.cc.addLogText(p1);
                    }
                    else if (filename.Equals("gaShowMessageBox.cs"))
                    {
                        this.MessageBoxHtml(p1);
                    }
                    else if (filename.Equals("gaShowFloatyTextOnMainMap.cs"))
                    {
                        int parm3 = Convert.ToInt32(p3);
                        int parm4 = Convert.ToInt32(p4);
                        gv.screenMainMap.addFloatyText(parm3, parm4, p1, p2, 4000);
                    }
                    else if (filename.Equals("gaDoConversationByName.cs"))
                    {
                        gv.cc.doConversationBasedOnTag(p1);
                    }
                    else if (filename.Equals("gaDoEncounterByTag.cs"))
                    {
                        gv.cc.doEncounterBasedOnTag(p1);
                    }
                    else if (filename.Equals("gaCheckForItemToggleLights.cs"))
                    {
                        int parm2 = Convert.ToInt32(p2);
                        if (CheckForItem(p1, parm2))
                        {
                            gv.mod.currentArea.areaDark = false;
                        }
                        else
                        {
                            gv.mod.currentArea.areaDark = true;
                            gv.screenMainMap.addFloatyText(mod.PlayerLocationX, mod.PlayerLastLocationY, "need light!", "white", 4000);
                        }
                    }
                    else if (filename.Equals("gaToggleAreaSquareLoSBlocking.cs"))
                    {
                        int x = Convert.ToInt32(p1);
                        int y = Convert.ToInt32(p2);
                        bool enable = Boolean.Parse(p3);
                        if (enable)
                        {
                            gv.mod.currentArea.LoSBlocked[y * gv.mod.currentArea.MapSizeX + x] = 1;
                        }
                        else
                        {
                            gv.mod.currentArea.LoSBlocked[y * gv.mod.currentArea.MapSizeX + x] = 0;
                        }
                        
                    }
                    else if (filename.Equals("gaToggleAreaSquareWalkable.cs"))
                    {
                        int x = Convert.ToInt32(p1);
                        int y = Convert.ToInt32(p2);
                        bool enable = Boolean.Parse(p3);
                        if (enable)
                        {
                            gv.mod.currentArea.Walkable[y * gv.mod.currentArea.MapSizeX + x] = 1;
                        }
                        else
                        {
                            gv.mod.currentArea.Walkable[y * gv.mod.currentArea.MapSizeX + x] = 0;
                        }
                    }
                    else if (filename.Equals("gaPropOrTriggerCastSpellOnThisSquare.cs"))
                    {
                        gv.screenCombat.doPropOrTriggerCastSpell(p1);
                    }
                    else if (filename.Equals("gaAddCreatureToCurrentEncounter.cs"))
                    {
                        AddCreatureToCurrentEncounter(p1, p2, p3);
                    }
                }
                catch (Exception ex)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        this.MessageBoxHtml("Failed to run script (" + filename + "): " + ex.ToString());
                    }
                    gv.errorLog(ex.ToString());
                }
            }
        }
        public void gcController(string filename, string prm1, string prm2, string prm3, string prm4)
        {
            if (!filename.Equals("none"))
            {
                try
                {
                    if (!filename.EndsWith(".cs")) { filename += ".cs"; }
                    //go through each parm1-4 and replace if GlobalInt variable, GlobalString variable or rand(3-16)
                    string p1 = replaceParameter(prm1);
                    string p2 = replaceParameter(prm2);
                    string p3 = replaceParameter(prm3);
                    string p4 = replaceParameter(prm4);

                    if (filename.Equals("gcCheckGlobalInt.cs"))
                    {
                        int parm3 = Convert.ToInt32(p3);
                        gv.mod.returnCheck = CheckGlobalInt(prm1, prm2, parm3);
                    }
                    else if (filename.Equals("gcCheckLocalInt.cs"))
                    {
                        //check to see if prm1 is thisprop or thisarea
                        if (prm1.Equals("thisprop"))
                        {
                            //find the prop at this location
                            //prm1 = mod.currentArea.getPropByLocation(mod.PlayerLocationX, mod.PlayerLocationY).PropTag;
                        }
                        else if (prm1.Equals("thisarea"))
                        {
                            //use the currentArea
                            prm1 = mod.currentArea.Filename;
                        }
                        int parm4 = Convert.ToInt32(p4);
                        gv.mod.returnCheck = CheckLocalInt(prm1, prm2, prm3, parm4);
                    }
                    else if (filename.Equals("gcCheckGlobalString.cs"))
                    {
                        gv.mod.returnCheck = CheckGlobalString(prm1, p2);
                    }
                    else if (filename.Equals("gcCheckLocalString.cs"))
                    {
                        //check to see if prm1 is thisprop or thisarea
                        if (prm1.Equals("thisprop"))
                        {
                            //find the prop at this location
                            //prm1 = mod.currentArea.getPropByLocation(mod.PlayerLocationX, mod.PlayerLocationY).PropTag;
                        }
                        else if (prm1.Equals("thisarea"))
                        {
                            //use the currentArea
                            prm1 = mod.currentArea.Filename;
                        }
                        gv.mod.returnCheck = CheckLocalString(prm1, prm2, p3);
                    }
                    else if (filename.Equals("gcCheckJournalEntryByTag.cs"))
                    {
                        int parm3 = Convert.ToInt32(p3);
                        gv.mod.returnCheck = CheckJournalEntry(prm1, prm2, parm3);
                    }
                    else if (filename.Equals("gcCheckForGold.cs"))
                    {
                        int parm1 = Convert.ToInt32(p1);
                        if (parm1 <= gv.mod.partyGold)
                        {
                            gv.mod.returnCheck = true;
                        }
                        else
                        {
                            gv.mod.returnCheck = false;
                        }
                    }
                    else if (filename.Equals("gcCheckAttribute.cs"))
                    {
                        int parm4 = Convert.ToInt32(p4);
                        int parm1 = 0;
                        if ((p1.Equals("")) || (p1.Equals("-1")))
                        {
                            parm1 = gv.mod.selectedPartyLeader;
                        }
                        else
                        {
                            parm1 = Convert.ToInt32(p1);
                        }
                        gv.mod.returnCheck = CheckAttribute(parm1, p2, p3, parm4);
                    }
                    else if (filename.Equals("gcCheckIsRace.cs"))
                    {
                        int parm1 = 0;
                        if ((p1.Equals("")) || (p1.Equals("-1")))
                        {
                            parm1 = gv.mod.selectedPartyLeader;
                        }
                        else
                        {
                            parm1 = Convert.ToInt32(p1);
                        }
                        gv.mod.returnCheck = CheckIsRace(parm1, p2);
                    }
                    else if (filename.Equals("gcCheckHasTrait.cs"))
                    {
                        int parm1 = 0;
                        if ((p1.Equals("")) || (p1.Equals("-1")))
                        {
                            parm1 = gv.mod.selectedPartyLeader;
                        }
                        else
                        {
                            parm1 = Convert.ToInt32(p1);
                        }
                        gv.mod.returnCheck = CheckHasTrait(parm1, p2);
                    }
                    else if (filename.Equals("gcPassSkillCheck.cs"))
                    {
                        int parm1 = 0;
                        if ((p1.Equals("")) || (p1.Equals("-1")))
                        {
                            parm1 = gv.mod.selectedPartyLeader;
                        }
                        else
                        {
                            parm1 = Convert.ToInt32(p1);
                        }
                        int parm3 = Convert.ToInt32(p3);
                        gv.mod.returnCheck = CheckPassSkill(parm1, p2, parm3);
                    }
                    else if (filename.Equals("gcCheckIsClassLevel.cs"))
                    {
                        int parm1 = 0;
                        if ((p1.Equals("")) || (p1.Equals("-1")))
                        {
                            parm1 = gv.mod.selectedPartyLeader;
                        }
                        else
                        {
                            parm1 = Convert.ToInt32(p1);
                        }
                        int parm3 = Convert.ToInt32(p3);
                        gv.mod.returnCheck = this.CheckIsClassLevel(parm1, p2, parm3);
                    }
                    else if (filename.Equals("gcCheckIsLevel.cs"))
                    {
                        int parm1 = 0;
                        if ((p1.Equals("")) || (p1.Equals("-1")))
                        {
                            parm1 = gv.mod.selectedPartyLeader;
                        }
                        else
                        {
                            parm1 = Convert.ToInt32(p1);
                        }
                        int parm2 = Convert.ToInt32(p2);
                        gv.mod.returnCheck = this.CheckIsLevel(parm1, parm2);
                    }
                    else if (filename.Equals("gcCheckIsMale.cs"))
                    {
                        int parm1 = 0;
                        if ((p1.Equals("")) || (p1.Equals("-1")))
                        {
                            parm1 = gv.mod.selectedPartyLeader;
                        }
                        else
                        {
                            parm1 = Convert.ToInt32(p1);
                        }
                        gv.mod.returnCheck = CheckIsMale(parm1);
                    }
                    else if (filename.Equals("gcCheckPcInPartyByName.cs"))
                    {
                        gv.mod.returnCheck = false;
                        foreach (Player pc in gv.mod.playerList)
                        {
                            if ((pc.name.Equals(p1)) || (pc.name.ToLower().Equals(p1)) || (pc.name.Equals(p1.ToLower())))
                            {
                                gv.mod.returnCheck = true;
                            }
                        }
                    }
                    else if (filename.Equals("gcCheckSelectedPcName.cs"))
                    {
                        gv.mod.returnCheck = false;
                        if ((gv.mod.playerList[gv.mod.selectedPartyLeader].name.Equals(p1)) || (gv.mod.playerList[gv.mod.selectedPartyLeader].name.ToLower().Equals(p1)) || (gv.mod.playerList[gv.mod.selectedPartyLeader].name.Equals(p1.ToLower())))
                        {
                            gv.mod.returnCheck = true;
                        }
                    }
                    else if (filename.Equals("gcCheckForItem.cs"))
                    {
                        int parm2 = Convert.ToInt32(p2);
                        gv.mod.returnCheck = CheckForItem(p1, parm2);
                    }
                    else if (filename.Equals("gcCheckPropIsShownByTag.cs"))
                    {
                        /*Prop prp = gv.mod.currentArea.getPropByTag(prm1);
                        if (prp != null)
                        {
                            gv.mod.returnCheck = prp.isShown;
                        }
                        else
                        {
                            gv.mod.returnCheck = false;
                            if (mod.debugMode) //SD_20131102
                            {
                                gv.cc.addLogText("<font color='yellow'>didn't find prop in this area, returning 'false' for isShown</font><BR>");
                            }
                        }*/
                    }
                    else if (filename.Equals("gcCheckProp.cs"))
                    {
                        //gv.mod.returnCheck = CheckProp(p1, p2, prm3);
                    }
                    else if (filename.Equals("gcRand1of.cs"))
                    {
                        mod.returnCheck = false;
                        int parm1 = Convert.ToInt32(p1);
                        int rnd = gv.sf.RandInt(parm1);
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>Rand = " + rnd + "</font><BR>");
                        }
                        if (rnd == 1)
                        {
                            mod.returnCheck = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        this.MessageBoxHtml("Failed to run script (" + filename + "): " + ex.ToString());
                    }
                    gv.errorLog(ex.ToString());
                }
            }
        }
        public void ogController(string filename, string prm1, string prm2, string prm3, string prm4)
        {
            if (!filename.Equals("none"))
            {
                try
                {
                    if (!filename.EndsWith(".cs")) { filename += ".cs"; }
                    //go through each parm1-4 and replace if GlobalInt variable, GlobalString variable or rand(3-16)
                    string p1 = replaceParameter(prm1);
                    string p2 = replaceParameter(prm2);
                    string p3 = replaceParameter(prm3);
                    string p4 = replaceParameter(prm4);

                    if (filename.Equals("ogGetPartySize.cs"))
                    {
                        String val = gv.mod.playerList.Count + "";
                        SetGlobalInt(prm1, val);
                    }
                    else if (filename.Equals("ogGetPartyRosterSize.cs"))
                    {
                        String val = gv.mod.partyRosterList.Count + "";
                        SetGlobalInt(prm1, val);
                    }
                    else if (filename.Equals("ogGetNumberOfCreaturesInEncounter.cs"))
                    {
                        String val = gv.mod.currentEncounter.encounterCreatureList.Count + "";
                        SetGlobalInt(prm1, val);
                    }
                    else if (filename.Equals("ogGetCurrentPlayerIndexUsingItem.cs"))
                    {
                        //String val = gv.cc.currentPlayerIndexUsingItem + "";
                        string val = gv.mod.indexOfPCtoLastUseItem + "";
                        SetGlobalInt(prm1, val);
                    }
                    else if (filename.Equals("ogGetCreatureCombatLocation.cs"))
                    {
                        Creature crt = GetCreature(prm1, p2);
                        if (crt == null)
                        {
                            return;
                        }
                        if (crt != null)
                        {
                            String valX = crt.combatLocX + "";
                            String valY = crt.combatLocY + "";
                            SetGlobalInt(prm3, valX);
                            SetGlobalInt(prm4, valY);
                        }
                    }
                    /*else if (filename.Equals("ogGetPropLocation.cs"))
                    {
                        Prop prp = GetProp(prm1, p2);
                        if (prp == null)
                        {
                            return;
                        }
                        if (prp != null)
                        {
                            String valX = prp.LocationX + "";
                            String valY = prp.LocationY + "";
                            SetGlobalInt(prm3, valX);
                            SetGlobalInt(prm4, valY);
                        }
                    }*/
                    else if (filename.Equals("ogGetPcCombatLocation.cs"))
                    {
                        Player pc = gv.mod.playerList[0];
                        if ((prm1 != null) && (!prm1.Equals("")))
                        {
                            pc = gv.mod.getPlayerByNameOrTag(prm1);
                        }
                        else if ((p2 != null) && (!p2.Equals("")))
                        {
                            int parm2 = Convert.ToInt32(p2);
                            pc = gv.mod.playerList[parm2];
                        }
                        if (pc != null)
                        {
                            String valX = pc.combatLocX + "";
                            String valY = pc.combatLocY + "";
                            SetGlobalInt(prm3, valX);
                            SetGlobalInt(prm4, valY);
                        }
                    }
                    else if (filename.Equals("ogGetCreatureHp.cs"))
                    {
                        Creature crt = GetCreature(prm1, p2);
                        if (crt == null)
                        {
                            return;
                        }
                        if (crt != null)
                        {
                            String val = crt.hp + "";
                            SetGlobalInt(prm3, val);
                        }
                    }
                    else if (filename.Equals("ogGetCreatureSp.cs"))
                    {
                        Creature crt = GetCreature(prm1, p2);
                        if (crt == null)
                        {
                            return;
                        }
                        if (crt != null)
                        {
                            String val = crt.sp + "";
                            SetGlobalInt(prm3, val);
                        }
                    }
                    else if (filename.Equals("ogGetPlayerHp.cs"))
                    {
                        this.GetPlayerHp(prm1, p2, prm3);
                    }
                    else if (filename.Equals("ogGetPlayerSp.cs"))
                    {
                        this.GetPlayerSp(prm1, p2, prm3);
                    }
                    else if (filename.Equals("ogGetPartyLocation.cs"))
                    {
                        String valX = gv.mod.PlayerLocationX + "";
                        String valY = gv.mod.PlayerLocationY + "";
                        String valName = gv.mod.currentArea.Filename;
                        SetGlobalInt(prm1, valX);
                        SetGlobalInt(prm2, valY);
                        SetGlobalString(prm3, valName);
                    }
                    else if (filename.Equals("ogGetWorldTime.cs"))
                    {
                        String val = mod.WorldTime + "";
                        SetGlobalInt(prm2, val);
                    }
                }
                catch (Exception ex)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        this.MessageBoxHtml("Failed to run script (" + filename + "): " + ex.ToString());
                    }
                    gv.errorLog(ex.ToString());
                }
            }
        }
        public void osController(string filename, string prm1, string prm2, string prm3, string prm4)
        {
            if (!filename.Equals("none"))
            {
                try
                {
                    if (!filename.EndsWith(".cs")) { filename += ".cs"; }
                    //go through each parm1-4 and replace if GlobalInt variable, GlobalString variable or rand(3-16)
                    string p1 = replaceParameter(prm1);
                    string p2 = replaceParameter(prm2);
                    string p3 = replaceParameter(prm3);
                    string p4 = replaceParameter(prm4);

                    if (filename.Equals("osSetPlayerHp.cs"))
                    {
                        SetPlayerHp(prm1, p2, prm3, p4);
                    }
                    else if (filename.Equals("osSetPlayerSp.cs"))
                    {
                        SetPlayerSp(prm1, p2, prm3, p4);
                    }
                    else if (filename.Equals("osAddSpellToPlayer.cs"))
                    {
                        AddSpellToPlayer(prm1, p2, prm3);
                    }
                    else if (filename.Equals("osAddTraitToPlayer.cs"))
                    {
                        AddTraitToPlayer(prm1, p2, prm3);
                    }
                    else if (filename.Equals("osAddAllowedItemToPlayerClass.cs"))
                    {
                        AddAllowedItemToPlayerClass(prm1, p2);
                    }
                    else if (filename.Equals("osRemoveAllowedItemFromPlayerClass.cs"))
                    {
                        RemoveAllowedItemFromPlayerClass(prm1, p2);
                    }
                    else if (filename.Equals("osSetPlayerBaseStr.cs"))
                    {
                        SetPlayerBaseAtt(prm1, p2, prm3, p4, "str");
                    }
                    else if (filename.Equals("osSetPlayerBaseDex.cs"))
                    {
                        SetPlayerBaseAtt(prm1, p2, prm3, p4, "dex");
                    }
                    else if (filename.Equals("osSetPlayerBaseInt.cs"))
                    {
                        SetPlayerBaseAtt(prm1, p2, prm3, p4, "int");
                    }
                    else if (filename.Equals("osSetPlayerBaseCha.cs"))
                    {
                        SetPlayerBaseAtt(prm1, p2, prm3, p4, "cha");
                    }
                    else if (filename.Equals("osSetPlayerBaseWis.cs"))
                    {
                        SetPlayerBaseAtt(prm1, p2, prm3, p4, "wis");
                    }
                    else if (filename.Equals("osSetPlayerBaseCon.cs"))
                    {
                        SetPlayerBaseAtt(prm1, p2, prm3, p4, "con");
                    }
                    else if (filename.Equals("osSetCreatureSp.cs"))
                    {
                        SetCreatureSp(prm1, p2, prm3, p4);
                    }
                    else if (filename.Equals("osSetCreatureHp.cs"))
                    {
                        SetCreatureHp(prm1, p2, prm3, p4);
                    }
                    else if (filename.Equals("osSetCreatureCombatLocation.cs"))
                    {
                        Creature crt = GetCreature(prm1, p2);
                        if (crt == null)
                        {
                            return;
                        }
                        if (crt != null)
                        {
                            crt.combatLocX = Convert.ToInt32(p3);
                            crt.combatLocY = Convert.ToInt32(p4);
                        }
                    }
                    
                    else if (filename.Equals("osAddCreatureToCurrentEncounter.cs"))
                    {
                        AddCreatureToCurrentEncounter(p1, p2, p3);
                    }

                    else if (filename.Equals("osSetTriggerSingleLocation.cs"))
                    {
                        Trigger t = gv.mod.currentArea.getTriggerByTag(prm1);
                        if (t != null)
                        {
                            t.TriggerSquaresList.Clear();
                            Coordinate newCoor = new Coordinate(Convert.ToInt32(p2), Convert.ToInt32(p3));
                            t.TriggerSquaresList.Add(newCoor);
                        }
                    }
                    else if (filename.Equals("osSetWorldTime.cs"))
                    {
                        SetWorldTime(prm1, p2, prm3);
                    }
                }
                catch (Exception ex)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        this.MessageBoxHtml("Failed to run script (" + filename + "): " + ex.ToString());
                    }
                    gv.errorLog(ex.ToString());
                }
            }
        }
        public string replaceParameter(string parm)
        {
            if (parm == null) { return parm; }
            //check to see if it is a GlobalInt
            foreach (GlobalInt g in gv.mod.moduleGlobalInts)
            {
                if (g.Key.Equals(parm))
                {
                    if (gv.mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>Replaced " + parm + " with " + g.Value + "</font><BR>");
                    }
                    return g.Value + "";
                }
            }
            //check to see if it is a GlobatString
            foreach (GlobalString g in gv.mod.moduleGlobalStrings)
            {
                if (g.Key.Equals(parm))
                {
                    if (gv.mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>Replaced " + parm + " with " + g.Value + "</font><BR>");
                    }
                    return g.Value;
                }
            }
            //check to see if it is a rand(3-16)
            if (parm.StartsWith("rand("))
            {
                string firstNum = parm.Split('(', '-')[1]; //.Substring(parm.IndexOf("(") + 1, parm.IndexOf("-"));
                string lastNum = parm.Split('-', ')')[1]; //.Substring(parm.IndexOf("-") + 1, parm.IndexOf(")"));
                int fNum = Convert.ToInt32(firstNum);
                int lNum = Convert.ToInt32(lastNum);
                int returnRand = gv.sf.RandInt(fNum, lNum);
                if (gv.mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>Replaced " + parm + " with " + returnRand + "</font><BR>");
                }
                return returnRand + "";
            }

            return parm; //did not find a replacement so send back original
        }

        //GLOBAL AND LOCAL INTS
        public void SetupLocalIntIfDoesntExist(string objectTag, string variableName)
        {
            //check creatures, PCs, Props, areas, items
            foreach (Area a in mod.moduleAreasObjects)
            {                
                /*foreach (Prop p in a.Props)
                {
                    if (p.PropTag.Equals(objectTag))
                    {
                        bool foundone = false;
                        foreach (LocalInt variable in p.PropLocalInts)
                        {
                            if (variable.Key.Equals(variableName))
                            {
                                foundone = true;
                            }
                        }
                        if (!foundone)
                        {
                            SetLocalInt(objectTag, variableName, "0");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<yl>" + "SetUpLocal:: tag:" + objectTag + " name:" + variableName + " value:0</yl><BR>");
                            }
                        }
                    }
                }*/                
            }
        }
        public void SetGlobalInt(string variableName, string val)
        {
            //TODO add option for '+4' or '-3' to add 4 or subtract 3
            int value = 0;
            if (val.Equals("++"))
            {
                int currentValue = GetGlobalInt(variableName);
                if (currentValue == -1) //this is our first time using this variable so set to 1
                {
                    //SetGlobalInt(p1, 1);
                    value = 1;
                }
                else //we have the variable so increment by one
                {
                    currentValue++;
                    //sf.SetGlobalInt(p1, currentValue);
                    value = currentValue;
                }
            }
            else if (val.Equals("--"))
            {
                int currentValue = GetGlobalInt(variableName);
                if (currentValue == -1) //this is our first time using this variable so set to 0
                {
                    //sf.SetGlobalInt(p1, 0);
                    value = 0;
                }
                else //we have the variable so decrement by one
                {
                    currentValue--;
                    if (currentValue < 0) { currentValue = 0; }
                    //sf.SetGlobalInt(p1, currentValue);
                    value = currentValue;
                }
            }
            else
            {
                value = Convert.ToInt32(val);
                //int parm2 = Convert.ToInt32(p2);
                //sf.SetGlobalInt(p1, parm2);
            }
            int exists = 0;
            foreach (GlobalInt variable in mod.moduleGlobalInts)
            {
                if (variable.Key.Equals(variableName))
                {
                    variable.Value = value;
                    exists = 1;
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "setGlobal: " + variableName + " = " + value + "</font>" +
                                "<BR>");
                    }
                }
            }
            if (exists == 0)
            {
                GlobalInt newGlobal = new GlobalInt();
                newGlobal.Key = variableName;
                newGlobal.Value = value;
                mod.moduleGlobalInts.Add(newGlobal);
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "setGlobal: " + variableName + " = " + value + "</font>" +
                            "<BR>");
                }
            }
        }
        public void SetLocalInt(string objectTag, string variableName, string val)
        {
            //TODO add option for '+4' or '-3' to add 4 or subtract 3
            int value = 0;
            if (val.Equals("++"))
            {
                int currentValue = GetLocalInt(objectTag, variableName);
                if (currentValue == -1) //this is our first time using this variable so set to 1
                {
                    value = 1;
                }
                else //we have the variable so increment by one
                {
                    currentValue++;
                    value = currentValue;
                }
            }
            else if (val.Equals("--"))
            {
                int currentValue = GetLocalInt(objectTag, variableName);
                if (currentValue == -1) //this is our first time using this variable so set to 0
                {
                    value = 0;
                }
                else //we have the variable so decrement by one
                {
                    currentValue--;
                    if (currentValue < 0) { currentValue = 0; }
                    value = currentValue;
                }
            }
            else
            {
                value = Convert.ToInt32(val);
            }

            //check creatures and areas and props
            foreach (Area a in mod.moduleAreasObjects)
            {
                if (a.Filename.Equals(objectTag))
                {
                    return;
                }
                else
                {
                    /*foreach (Prop p in a.Props)
                    {
                        if (p.PropTag.Equals(objectTag))
                        {
                            int exists = 0;
                            foreach (LocalInt variable in p.PropLocalInts)
                            {
                                if (variable.Key.Equals(variableName))
                                {
                                    variable.Value = value;
                                    exists = 1;
                                }
                            }
                            if (exists == 0)
                            {
                                LocalInt newLocalInt = new LocalInt();
                                newLocalInt.Key = variableName;
                                newLocalInt.Value = value;
                                p.PropLocalInts.Add(newLocalInt);
                            }
                            return;
                        }
                    }*/
                }
            }
            foreach (Creature cr in mod.currentEncounter.encounterCreatureList)
            {
                if (cr.cr_tag.Equals(objectTag))
                {
                    /*int exists = 0;
                    foreach (LocalInt variable in cr.CreatureLocalInts)
                    {
                        if (variable.Key.Equals(variableName))
                        {
                            variable.Value = value;
                            exists = 1;
                        }
                    }
                    if (exists == 0)
                    {
                        LocalInt newLocalInt = new LocalInt();
                        newLocalInt.Key = variableName;
                        newLocalInt.Value = value;
                        cr.CreatureLocalInts.Add(newLocalInt);
                    }*/
                    return;
                }
            }
            if (mod.debugMode)
            {
                gv.cc.addLogText("<font color='yellow'>" + objectTag + " setLocal: " + variableName + " = " + value + "</font><BR>");
            }
        }
        public int GetGlobalInt(string variableName)
        {
            foreach (GlobalInt variable in mod.moduleGlobalInts)
            {
                if (variable.Key.Equals(variableName))
                {
                    return variable.Value;
                }
            }
            if (mod.debugMode) //SD_20131102
            {
                gv.cc.addLogText("<font color='yellow'>" + "Couldn't find the tag specified...returning a value of -1" + "</font>" +
                        "<BR>");
            }
            return -1;
        }
        public int GetLocalInt(string objectTag, string variableName)
        {
            //check creatures, areas        
            foreach (Area a in mod.moduleAreasObjects)
            {
                if (a.Filename.Equals(objectTag))
                {
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>Found the object, but couldn't find the tag specified...returning a value of -1</font><BR>");
                    }
                    return -1;
                }
                else
                {
                    /*foreach (Prop p in a.Props)
                    {
                        if (p.PropTag.Equals(objectTag))
                        {
                            foreach (LocalInt variable in p.PropLocalInts)
                            {
                                if (variable.Key.Equals(variableName))
                                {
                                    return variable.Value;
                                }
                            }
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>Found the object, but couldn't find the tag specified...returning a value of -1</font><BR>");
                            }
                            return -1;
                        }
                    }*/
                }
            }
            foreach (Creature cr in mod.currentEncounter.encounterCreatureList)
            {
                if (cr.cr_tag.Equals(objectTag))
                {
                    /*foreach (LocalInt variable in cr.CreatureLocalInts)
                    {
                        if (variable.Key.Equals(variableName))
                        {
                            return variable.Value;
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>Found the object, but couldn't find the tag specified...returning a value of -1</font><BR>");
                    }*/
                    return -1;
                }
            }
            if (mod.debugMode)
            {
                gv.cc.addLogText("<font color='yellow'>couldn't find the object with the tag specified (only Creatures and Areas), returning a value of -1</font><BR>");
            }
            return -1;
        }
        public bool CheckGlobalInt(string variableName, string compare, int value)
        {
            if (mod.debugMode) //SD_20131102
            {
                gv.cc.addLogText("<font color='yellow'>" + "checkGlobal: " + variableName + " " + compare + " " + value + "</font>" +
                        "<BR>");
            }

            foreach (GlobalInt variable in mod.moduleGlobalInts)
            {
                if (variable.Key.Equals(variableName))
                {
                    if (compare.Equals("="))
                    {
                        if (variable.Value == value)
                        {
                            if (mod.debugMode) //SD_20131102
                            {
                                gv.cc.addLogText("<font color='yellow'>" + "foundGlobal: " + variable.Key + " == " + variable.Value + "</font>" +
                                        "<BR>");
                            }
                            return true;
                        }
                    }
                    else if (compare.Equals(">"))
                    {
                        if (variable.Value > value)
                        {
                            if (mod.debugMode) //SD_20131102
                            {
                                gv.cc.addLogText("<font color='yellow'>" + "foundGlobal: " + variable.Key + " > " + variable.Value + "</font>" +
                                        "<BR>");
                            }
                            return true;
                        }
                    }
                    else if (compare.Equals("<"))
                    {
                        if (variable.Value < value)
                        {
                            if (mod.debugMode) //SD_20131102
                            {
                                gv.cc.addLogText("<font color='yellow'>" + "foundGlobal: " + variable.Key + " < " + variable.Value + "</font>" +
                                        "<BR>");
                            }
                            return true;
                        }
                    }
                    else if (compare.Equals("!"))
                    {
                        if (variable.Value != value)
                        {
                            if (mod.debugMode) //SD_20131102
                            {
                                gv.cc.addLogText("<font color='yellow'>" + "foundGlobal: " + variable.Key + " != " + variable.Value + "</font>" +
                                        "<BR>");
                            }
                            return true;
                        }
                    }
                }
            }
            if (mod.debugMode) //SD_20131102
            {
                gv.cc.addLogText("<font color='yellow'>" + "returning false" + "</font>" +
                        "<BR>");
            }
            return false;
        }
        public bool CheckLocalInt(string objectTag, string variableName, string compare, int value)
        {
            if (mod.debugMode) //SD_20131102
            {
                gv.cc.addLogText("<yl>" + "checkLocal: " + objectTag + " " + variableName + " " + compare + " " + value + "</yl><BR>");
            }
            //check Props
            foreach (Area a in mod.moduleAreasObjects)
            {                
                /*foreach (Prop p in a.Props)
                {
                    if (p.PropTag.Equals(objectTag))
                    {
                        SetupLocalIntIfDoesntExist(objectTag, variableName);
                        foreach (LocalInt variable in p.PropLocalInts)
                        {
                            if (variable.Key.Equals(variableName))
                            {
                                if (compare.Equals("="))
                                {
                                    if (variable.Value == value)
                                    {
                                        if (mod.debugMode) //SD_20131102
                                        {
                                            gv.cc.addLogText("<yl>" + "foundLocal: " + variable.Key + " == " + variable.Value + "</yl><BR>");
                                        }
                                        return true;
                                    }
                                }
                                else if (compare.Equals(">"))
                                {
                                    if (variable.Value > value)
                                    {
                                        if (mod.debugMode) //SD_20131102
                                        {
                                            gv.cc.addLogText("<yl>" + "foundLocal: " + variable.Key + " > " + variable.Value + "</yl><BR>");
                                        }
                                        return true;
                                    }
                                }
                                else if (compare.Equals("<"))
                                {
                                    if (variable.Value < value)
                                    {
                                        if (mod.debugMode) //SD_20131102
                                        {
                                            gv.cc.addLogText("<yl>" + "foundLocal: " + variable.Key + " < " + variable.Value + "</yl><BR>");
                                        }
                                        return true;
                                    }
                                }
                                else if (compare.Equals("!"))
                                {
                                    if (variable.Value != value)
                                    {
                                        if (mod.debugMode) //SD_20131102
                                        {
                                            gv.cc.addLogText("<yl>" + "foundLocal: " + variable.Key + " != " + variable.Value + "</yl><BR>");
                                        }
                                        return true;
                                    }
                                }
                            }
                        }
                        return false;
                    }
                }*/
            }
            if (mod.debugMode)
            {
                gv.cc.addLogText("<yl>couldn't find the object with the tag (tag: " + objectTag + ") specified (only Props have LocalInts)</yl><BR>");
            }
            return false;
        }
        public void TransformGlobalInt(string firstInt, string transformType, string secondInt, string variableName)
        {
            string val = "";
            int value = 0;
            int fInt = Convert.ToInt32(firstInt);
            int sInt = Convert.ToInt32(secondInt);

            if (transformType.Equals("+"))
            {
                value = fInt + sInt;
            }
            else if (transformType.Equals("-"))
            {
                value = fInt - sInt;
            }
            else if (transformType.Equals("/"))
            {
                value = fInt / sInt;
            }
            else if (transformType.Equals("%"))
            {
                value = fInt % sInt;
            }
            else if (transformType.Equals("*"))
            {
                value = fInt * sInt;
            }
            else
            {
                value = 0;
            }

            val = value + "";
            SetGlobalInt(variableName, val);
        }
        

        //GLOBAL AND LOCAL STRINGS
        public void SetGlobalString(string variableName, string val)
        {
            int exists = 0;
            foreach (GlobalString variable in mod.moduleGlobalStrings)
            {
                if (variable.Key.Equals(variableName))
                {
                    variable.Value = val;
                    exists = 1;
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "setGlobal: " + variableName + " = " + val + "</font>" +
                                "<BR>");
                    }
                }
            }
            if (exists == 0)
            {
                GlobalString newGlobal = new GlobalString();
                newGlobal.Key = variableName;
                newGlobal.Value = val;
                mod.moduleGlobalStrings.Add(newGlobal);
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "setGlobal: " + variableName + " = " + val + "</font>" +
                            "<BR>");
                }
            }
        }
        public void SetLocalString(string objectTag, string variableName, string value)
        {
            //check creatures and areas
            foreach (Area a in mod.moduleAreasObjects)
            {
                if (a.Filename.Equals(objectTag))
                {
                    /*int exists = 0;
                    foreach (LocalString variable in a.AreaLocalStrings)
                    {
                        if (variable.Key.Equals(variableName))
                        {
                            variable.Value = value;
                            exists = 1;
                        }
                    }
                    if (exists == 0)
                    {
                        LocalString newLocalInt = new LocalString();
                        newLocalInt.Key = variableName;
                        newLocalInt.Value = value;
                        a.AreaLocalStrings.Add(newLocalInt);
                    }*/
                    return;
                }
                else
                {
                    /*foreach (Prop p in a.Props)
                    {
                        if (p.PropTag.Equals(objectTag))
                        {
                            int exists = 0;
                            foreach (LocalString variable in p.PropLocalStrings)
                            {
                                if (variable.Key.Equals(variableName))
                                {
                                    variable.Value = value;
                                    exists = 1;
                                }
                            }
                            if (exists == 0)
                            {
                                LocalString newLocalInt = new LocalString();
                                newLocalInt.Key = variableName;
                                newLocalInt.Value = value;
                                p.PropLocalStrings.Add(newLocalInt);
                            }
                            return;
                        }
                    }*/
                }
            }
            foreach (Creature cr in mod.currentEncounter.encounterCreatureList)
            {
                if (cr.cr_tag.Equals(objectTag))
                {
                    /*int exists = 0;
                    foreach (LocalString variable in cr.CreatureLocalStrings)
                    {
                        if (variable.Key.Equals(variableName))
                        {
                            variable.Value = value;
                            exists = 1;
                        }
                    }
                    if (exists == 0)
                    {
                        LocalString newLocalInt = new LocalString();
                        newLocalInt.Key = variableName;
                        newLocalInt.Value = value;
                        cr.CreatureLocalStrings.Add(newLocalInt);
                    }*/
                    return;
                }
            }
            if (mod.debugMode)
            {
                gv.cc.addLogText("<font color='yellow'>" + objectTag + " setLocal: " + variableName + " = " + value + "</font><BR>");
            }
        }
        public string GetGlobalString(string variableName)
        {
            foreach (GlobalString variable in mod.moduleGlobalStrings)
            {
                if (variable.Key.Equals(variableName))
                {
                    return variable.Value;
                }
            }
            if (mod.debugMode) //SD_20131102
            {
                gv.cc.addLogText("<font color='yellow'>" + "Couldn't find the tag specified...returning a value of \"none\"" + "</font>" +
                        "<BR>");
            }
            return "none";
        }
        public string GetLocalString(string objectTag, string variableName)
        {
            //check creatures, areas        
            foreach (Area a in mod.moduleAreasObjects)
            {
                if (a.Filename.Equals(objectTag))
                {
                    /*foreach (LocalString variable in a.AreaLocalStrings)
                    {
                        if (variable.Key.Equals(variableName))
                        {
                            return variable.Value;
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>Found the object, but couldn't find the tag specified...returning a value of \"none\"</font><BR>");
                    }*/
                    return "none";
                }
                else
                {
                    /*foreach (Prop p in a.Props)
                    {
                        if (p.PropTag.Equals(objectTag))
                        {
                            foreach (LocalString variable in p.PropLocalStrings)
                            {
                                if (variable.Key.Equals(variableName))
                                {
                                    return variable.Value;
                                }
                            }
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>Found the object, but couldn't find the tag specified...returning a value of \"none\"</font><BR>");
                            }
                            return "none";
                        }
                    }*/
                }
            }
            foreach (Creature cr in mod.currentEncounter.encounterCreatureList)
            {
                if (cr.cr_tag.Equals(objectTag))
                {
                    /*foreach (LocalString variable in cr.CreatureLocalStrings)
                    {
                        if (variable.Key.Equals(variableName))
                        {
                            return variable.Value;
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>Found the object, but couldn't find the tag specified...returning a value of \"none\"</font><BR>");
                    }*/
                    return "none";
                }
            }
            if (mod.debugMode)
            {
                gv.cc.addLogText("<font color='yellow'>couldn't find the object with the tag specified (only Creatures and Areas), returning a value of \"none\"</font><BR>");
            }
            return "none";
        }
        public bool CheckGlobalString(string variableName, string value)
        {
            if (mod.debugMode)
            {
                gv.cc.addLogText("<font color='yellow'>" + "checkGlobal: " + variableName + " == " + value + "</font>" +
                        "<BR>");
            }
            foreach (GlobalString variable in mod.moduleGlobalStrings)
            {
                if (variable.Key.Equals(variableName))
                {
                    if (variable.Value.Equals(value))
                    {
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>foundGlobal: " + variable.Key + " == " + variable.Value + "</font><BR>");
                        }
                        return true;
                    }
                }
            }
            if (mod.debugMode)
            {
                gv.cc.addLogText("<font color='yellow'>returning false</font><BR>");
            }
            return false;
        }
        public bool CheckLocalString(string objectTag, string variableName, string value)
        {
            if (mod.debugMode) //SD_20131102
            {
                gv.cc.addLogText("<font color='yellow'>" + "checkLocal: " + objectTag + " " + variableName + " == " + value + "</font><BR>");
            }
            //check creatures, areas
            foreach (Area a in mod.moduleAreasObjects)
            {
                if (a.Filename.Equals(objectTag))
                {
                    /*foreach (LocalString variable in a.AreaLocalStrings)
                    {
                        if (variable.Key.Equals(variableName))
                        {
                            if (variable.Value.Equals(value))
                            {
                                if (mod.debugMode) //SD_20131102
                                {
                                    gv.cc.addLogText("<font color='yellow'>foundLocal: " + variable.Key + " == " + variable.Value + "</font><BR>");
                                }
                                return true;
                            }
                        }
                    }*/
                    return false;
                }
                else
                {
                    /*foreach (Prop p in a.Props)
                    {
                        if (p.PropTag.Equals(objectTag))
                        {
                            foreach (LocalString variable in p.PropLocalStrings)
                            {
                                if (variable.Key.Equals(variableName))
                                {
                                    if (variable.Value.Equals(value))
                                    {
                                        if (mod.debugMode) //SD_20131102
                                        {
                                            gv.cc.addLogText("<font color='yellow'>foundLocal: " + variable.Key + " == " + variable.Value + "</font><BR>");
                                        }
                                        return true;
                                    }
                                }
                            }
                            return false;
                        }
                    }*/
                }
            }
            foreach (Creature cr in mod.currentEncounter.encounterCreatureList)
            {
                if (cr.cr_tag.Equals(objectTag))
                {
                    /*foreach (LocalString variable in cr.CreatureLocalStrings)
                    {
                        if (variable.Key.Equals(variableName))
                        {
                            if (variable.Value.Equals(value))
                            {
                                if (mod.debugMode) //SD_20131102
                                {
                                    gv.cc.addLogText("<font color='yellow'>foundLocal: " + variable.Key + " == " + variable.Value + "</font><BR>");
                                }
                                return true;
                            }
                        }
                    }*/
                    return false;
                }
            }
            if (mod.debugMode)
            {
                gv.cc.addLogText("<font color='yellow'>couldn't find the object with the tag (tag: " + objectTag + ") specified (only Creatures or Areas)</font><BR>");
            }
            return false;
        }
        public void TransformGlobalString(string firstString, string secondString, string variableName)
        {
            firstString = firstString.Replace("\"", "");
            secondString = secondString.Replace("\"", "");
            string val = firstString + secondString;
            SetGlobalString(variableName, val);
        }

        public void GiveFunds(int amount)
        {
            mod.partyGold += amount;
            gv.cc.addLogText("<font color='yellow'>" + "The party receives " + amount + " Gold" + "</font>" + "<BR>");
        }
        public void TakeFunds(int amount)
        {
            mod.partyGold -= amount;
            if (mod.partyGold < 0)
            {
                mod.partyGold = 0;
            }
            gv.cc.addLogText("<font color='yellow'>" + "The party loses " + amount + " Gold" + "</font>" + "<BR>");
        }
        public void GiveItem(string resref, int quantity)
        {
            Item newItem = gv.cc.getItemByResRef(resref);
            for (int i = 0; i < quantity; i++)
            {
                ItemRefs ir = mod.createItemRefsFromItem(newItem);
                mod.partyInventoryRefsList.Add(ir);
            }
            gv.cc.addLogText("<font color='yellow'>" + "The party gains " + quantity + " " + newItem.name + "</font><BR>");
        }
        public void RemoveItemFromInventory(ItemRefs itRef, int quantity)
        {
            //decrement item quantity
            itRef.quantity -= quantity;
            //if item quantity <= 0, remove item from inventory
            if (itRef.quantity < 1)
            {
                gv.mod.partyInventoryRefsList.Remove(itRef);
            }
        }
        public void GiveXP(int amount)
        {
            if (mod.playerList.Count > 0)
            {
                int xpToGive = amount / mod.playerList.Count;
                //give xp to each PC member...split the value given
                foreach (Player givePcXp in mod.playerList)
                {
                    givePcXp.XP += xpToGive;
                }
                gv.cc.addLogText("<font color='yellow'>" + "Each player has gained " + xpToGive + " XP" + "</font>" +
                        "<BR>");
            }
        }
        public void TakeItem(string resref, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                bool FoundOne = false;
                int cnt = 0;
                foreach (ItemRefs itr in mod.partyInventoryRefsList)
                {
                    if (!FoundOne)
                    {
                        if (itr.resref.Equals(resref))
                        {
                            gv.sf.RemoveItemFromInventory(itr, 1);
                            FoundOne = true;
                            break;
                        }
                    }
                    cnt++;
                }
                cnt = 0;
                foreach (Player pc in mod.playerList)
                {
                    if (!FoundOne)
                    {
                        if (pc.BodyRefs.resref.Equals(resref))
                        {
                            mod.playerList[cnt].BodyRefs = new ItemRefs();
                            FoundOne = true;
                        }
                        if (pc.OffHandRefs.resref.Equals(resref))
                        {
                            mod.playerList[cnt].OffHandRefs = new ItemRefs();
                            FoundOne = true;
                        }
                        if (pc.MainHandRefs.resref.Equals(resref))
                        {
                            mod.playerList[cnt].MainHandRefs = new ItemRefs();
                            FoundOne = true;
                        }
                        if (pc.RingRefs.resref.Equals(resref))
                        {
                            mod.playerList[cnt].RingRefs = new ItemRefs();
                            FoundOne = true;
                        }
                        if (pc.Ring2Refs.resref.Equals(resref))
                        {
                            mod.playerList[cnt].Ring2Refs = new ItemRefs();
                            FoundOne = true;
                        }
                        if (pc.HeadRefs.resref.Equals(resref))
                        {
                            mod.playerList[cnt].HeadRefs = new ItemRefs();
                            FoundOne = true;
                        }
                        if (pc.NeckRefs.resref.Equals(resref))
                        {
                            mod.playerList[cnt].NeckRefs = new ItemRefs();
                            FoundOne = true;
                        }
                        if (pc.FeetRefs.resref.Equals(resref))
                        {
                            mod.playerList[cnt].FeetRefs = new ItemRefs();
                            FoundOne = true;
                        }
                    }
                    cnt++;
                }
            }
        }
        public void AddCharacterToParty(string filename)
        {
            try
            {
                //if the filename doesn't have a .json extension, add it
                if (!filename.EndsWith(".json"))
                {
                    filename += ".json";
                }
                Player newPc = gv.cc.LoadPlayer(filename); //ex: filename = "ezzbel.json"
                //newPc.token = gv.cc.LoadBitmap(newPc.tokenFilename);
                //newPc.portrait = gv.cc.LoadBitmap(newPc.portraitFilename);
                newPc.playerClass = gv.cc.getPlayerClass(newPc.classTag);
                newPc.race = gv.cc.getRace(newPc.raceTag);
                //check to see if already in party before adding
                bool foundOne = false;
                foreach (Player pc in mod.playerList)
                {
                    if (newPc.tag.Equals(pc.tag))
                    {
                        foundOne = true;
                    }
                }
                if (!foundOne)
                {
                    mod.playerList.Add(newPc);
                    if (!newPc.AmmoRefs.resref.Equals("none"))
                    {
                        GiveItem(newPc.AmmoRefs.resref, 1);
                    }
                    //gv.TrackerSendEventOnePlayerInfo(newPc,"PartyAddCompanion:" + newPc.name);
                    gv.cc.addLogText("<font color='lime'>" + newPc.name + " joins the party</font><BR>");
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "This PC is already in the party" + "</font><BR>");
                    }
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to load character from character folder" + "</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void RemoveCharacterFromParty(string PCtag, string index)
        {
            try
            {
                Player pc = gv.mod.playerList[0];
                if ((PCtag != null) && (!PCtag.Equals("use_index")))
                {
                    pc = gv.mod.getPlayerByNameOrTag(PCtag);
                    if (pc == null)
                    {
                        if (mod.debugMode) //SD_20131102
                        {
                            gv.cc.addLogText("<font color='yellow'>Could not find PC: " + PCtag + ", aborting</font><BR>");
                        }
                        return;
                    }
                }
                else if ((index != null) && (!index.Equals("")))
                {
                    int parm2 = Convert.ToInt32(index);
                    if ((parm2 >= 0) && (parm2 < gv.mod.playerList.Count))
                    {
                        pc = gv.mod.playerList[parm2];
                    }
                    else
                    {
                        if (mod.debugMode) //SD_20131102
                        {
                            gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                        }
                        return;
                    }
                }
                mod.playerList.Remove(pc);
                mod.selectedPartyLeader = 0;
                gv.cc.partyScreenPcIndex = 0;
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to remove character from party" + "</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void MoveCharacterToRoster(string PCtag, string index)
        {
            try
            {
                Player pc = gv.mod.playerList[0];
                if ((PCtag != null) && (!PCtag.Equals("")))
                {
                    pc = gv.mod.getPlayerByNameOrTag(PCtag);
                    if (pc == null)
                    {
                        if (mod.debugMode) //SD_20131102
                        {
                            gv.cc.addLogText("<font color='yellow'>Could not find PC: " + PCtag + ", aborting</font><BR>");
                        }
                        return;
                    }
                }
                else if ((index != null) && (!index.Equals("")))
                {
                    int parm2 = Convert.ToInt32(index);
                    if ((parm2 >= 0) && (parm2 < gv.mod.playerList.Count))
                    {
                        pc = gv.mod.playerList[parm2];
                    }
                    else
                    {
                        if (mod.debugMode) //SD_20131102
                        {
                            gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                        }
                        return;
                    }
                }
                //remove selected from partyList and add to pcList
                if (mod.playerList.Count > 0)
                {
                    Player copyPC = pc.DeepCopy();
                    //copyPC.token = gv.cc.LoadBitmap(copyPC.tokenFilename);
                    copyPC.playerClass = gv.cc.getPlayerClass(copyPC.classTag);
                    copyPC.race = gv.cc.getRace(copyPC.raceTag);
                    mod.partyRosterList.Add(copyPC);
                    mod.playerList.Remove(pc);
                }
                mod.selectedPartyLeader = 0;
                gv.cc.partyScreenPcIndex = 0;
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to remove character from party" + "</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void MoveCharacterToPartyFromRoster(string PCtag, string index)
        {
            try
            {
                if (gv.mod.partyRosterList.Count < 1)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Party Roster is empty, aborting</font><BR>");
                    }
                    return;
                }

                Player pc = null;
                if ((PCtag != null) && (!PCtag.Equals("")))
                {
                    foreach (Player plr in gv.mod.partyRosterList)
                    {
                        if (plr.name.Equals(PCtag))
                        {
                            pc = plr;
                        }
                    }
                    if (pc == null)
                    {
                        if (mod.debugMode) //SD_20131102
                        {
                            gv.cc.addLogText("<font color='yellow'>Could not find PC: " + PCtag + ", aborting</font><BR>");
                        }
                        return;
                    }
                }
                else if ((index != null) && (!index.Equals("")))
                {
                    int parm2 = Convert.ToInt32(index);
                    if ((parm2 >= 0) && (parm2 < gv.mod.partyRosterList.Count))
                    {
                        pc = gv.mod.partyRosterList[parm2];
                    }
                    else
                    {
                        if (mod.debugMode) //SD_20131102
                        {
                            gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                        }
                        return;
                    }
                }
                if ((mod.partyRosterList.Count > 0) && (mod.playerList.Count < mod.MaxPartySize))
                {
                    Player copyPC = pc.DeepCopy();
                    //copyPC.token = gv.cc.LoadBitmap(copyPC.tokenFilename);
                    copyPC.playerClass = gv.cc.getPlayerClass(copyPC.classTag);
                    copyPC.race = gv.cc.getRace(copyPC.raceTag);
                    mod.playerList.Add(copyPC);
                    mod.partyRosterList.Remove(pc);
                }
                mod.selectedPartyLeader = 0;
                gv.cc.partyScreenPcIndex = 0;
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to remove character from party" + "</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void EnableDisableTriggerEvent(string tag, string evNum, string enabl)
        {
            int eventNumber = Convert.ToInt32(evNum);
            bool enable = Boolean.Parse(enabl);

            try
            {
                foreach (Area ar in mod.moduleAreasObjects)
                {
                    Trigger trig = ar.getTriggerByTag(tag);
                    if (trig != null)
                    {
                        if (eventNumber == 1)
                        {
                            trig.EnabledEvent1 = enable;
                        }
                        else if (eventNumber == 2)
                        {
                            trig.EnabledEvent2 = enable;
                        }
                        else if (eventNumber == 3)
                        {
                            trig.EnabledEvent3 = enable;
                        }
                        return;
                    }
                }
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to find trigger in this area" + "</font><BR>");
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to find trigger due to exception error" + "</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void EnableDisableTriggerEvent(string tag, string evNum, string enabl, string areaName)
        {
            int eventNumber = Convert.ToInt32(evNum);
            bool enable = Boolean.Parse(enabl);

            try
            {
                foreach (Area ar in mod.moduleAreasObjects)
                {
                    Trigger trig = ar.getTriggerByTag(tag);
                    if (trig != null)
                    {
                        if (eventNumber == 1)
                        {
                            trig.EnabledEvent1 = enable;
                        }
                        else if (eventNumber == 2)
                        {
                            trig.EnabledEvent2 = enable;
                        }
                        else if (eventNumber == 3)
                        {
                            trig.EnabledEvent3 = enable;
                        }
                        return;
                    }
                }
                string s = gv.LoadStringFromEitherFolder("\\modules\\" + gv.mod.moduleName + "\\" + areaName + ".are", "\\modules\\" + gv.mod.moduleName + "\\" + areaName + ".are");
                if (!s.Equals(""))
                {
                    using (StringReader sr = new StringReader(s))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        Area are = (Area)serializer.Deserialize(sr, typeof(Area));
                        if (are != null)
                        {
                            if (gv.cc.saveMod != null)
                            {
                                foreach (AreaSave sar in gv.cc.saveMod.moduleAreasObjects)
                                {
                                    if (sar.Filename.Equals(are.Filename)) //sar is saved game, ar is new game from toolset version
                                    {
                                        //tiles
                                        for (int index = 0; index < are.Visible.Count; index++)
                                        {
                                            are.Visible[index] = sar.Visible[index];
                                        }
                                        //triggers
                                        foreach (Trigger tr in are.Triggers)
                                        {
                                            foreach (TriggerSave str in sar.Triggers)
                                            {
                                                if (tr.TriggerTag.Equals(str.TriggerTag))
                                                {
                                                    tr.Enabled = str.Enabled;
                                                    tr.EnabledEvent1 = str.EnabledEvent1;
                                                    tr.EnabledEvent2 = str.EnabledEvent2;
                                                    tr.EnabledEvent3 = str.EnabledEvent3;
                                                    tr.isShown = str.isShown;
                                                    //may want to copy the trigger's squares list from the save game if builders can modify the list with scripts
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            gv.mod.moduleAreasObjects.Add(are);
                            foreach (Area ar in mod.moduleAreasObjects)
                            {
                                Trigger trig = ar.getTriggerByTag(tag);
                                if (trig != null)
                                {
                                    if (eventNumber == 1)
                                    {
                                        trig.EnabledEvent1 = enable;
                                    }
                                    else if (eventNumber == 2)
                                    {
                                        trig.EnabledEvent2 = enable;
                                    }
                                    else if (eventNumber == 3)
                                    {
                                        trig.EnabledEvent3 = enable;
                                    }
                                    return;
                                }
                            }
                        }
                    }
                }
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to find trigger in this area" + "</font><BR>");
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to find trigger due to exception error" + "</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void EnableDisableTrigger(string tag, string enabl)
        {
            bool enable = Boolean.Parse(enabl);
            try
            {
                foreach (Area ar in mod.moduleAreasObjects)
                {
                    Trigger trig = ar.getTriggerByTag(tag);
                    if (trig != null)
                    {
                        trig.Enabled = enable;
                        return;
                    }
                }
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "can't find designated trigger tag in any area" + "</font><BR>");
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to find trigger due to exception error" + "</font>" +
                            "<BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void EnableDisableTrigger(string tag, string enabl, string areaName)
        {
            bool enable = Boolean.Parse(enabl);
            try
            {
                foreach (Area ar in mod.moduleAreasObjects)
                {
                    Trigger trig = ar.getTriggerByTag(tag);
                    if (trig != null)
                    {
                        trig.Enabled = enable;
                        return;
                    }
                }
                string s = gv.LoadStringFromEitherFolder("\\modules\\" + gv.mod.moduleName + "\\" + areaName + ".are", "\\modules\\" + gv.mod.moduleName + "\\" + areaName + ".are");
                if (!s.Equals(""))
                {
                    using (StringReader sr = new StringReader(s))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        Area are = (Area)serializer.Deserialize(sr, typeof(Area));
                        if (are != null)
                        {
                            if (gv.cc.saveMod != null)
                            {
                                foreach (AreaSave sar in gv.cc.saveMod.moduleAreasObjects)
                                {
                                    if (sar.Filename.Equals(are.Filename)) //sar is saved game, ar is new game from toolset version
                                    {
                                        //tiles
                                        for (int index = 0; index < are.Visible.Count; index++)
                                        {
                                            are.Visible[index] = sar.Visible[index];
                                        }
                                        //triggers
                                        foreach (Trigger tr in are.Triggers)
                                        {
                                            foreach (TriggerSave str in sar.Triggers)
                                            {
                                                if (tr.TriggerTag.Equals(str.TriggerTag))
                                                {
                                                    tr.Enabled = str.Enabled;
                                                    tr.EnabledEvent1 = str.EnabledEvent1;
                                                    tr.EnabledEvent2 = str.EnabledEvent2;
                                                    tr.EnabledEvent3 = str.EnabledEvent3;
                                                    tr.isShown = str.isShown;
                                                    //may want to copy the trigger's squares list from the save game if builders can modify the list with scripts
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            gv.mod.moduleAreasObjects.Add(are);
                            foreach (Area ar in mod.moduleAreasObjects)
                            {
                                Trigger trig = ar.getTriggerByTag(tag);
                                if (trig != null)
                                {
                                    trig.Enabled = enable;
                                    return;
                                }
                            }
                        }
                    }
                }
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "can't find designated trigger tag in area: " + areaName + ".are</font><BR>");
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to find trigger due to exception error" + "</font>" +
                            "<BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void DisableTriggerHideImage(string tag, string areaName)
        {
            try
            {
                foreach (Area ar in mod.moduleAreasObjects)
                {
                    Trigger trig = ar.getTriggerByTag(tag);
                    if (trig != null)
                    {
                        trig.Enabled = false;
                        trig.isShown = false;
                        return;
                    }
                }
                string s = gv.LoadStringFromEitherFolder("\\modules\\" + gv.mod.moduleName + "\\" + areaName + ".are", "\\modules\\" + gv.mod.moduleName + "\\" + areaName + ".are");
                if (!s.Equals(""))
                {
                    using (StringReader sr = new StringReader(s))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        Area are = (Area)serializer.Deserialize(sr, typeof(Area));
                        if (are != null)
                        {
                            if (gv.cc.saveMod != null)
                            {
                                foreach (AreaSave sar in gv.cc.saveMod.moduleAreasObjects)
                                {
                                    if (sar.Filename.Equals(are.Filename)) //sar is saved game, ar is new game from toolset version
                                    {
                                        //tiles
                                        for (int index = 0; index < are.Visible.Count; index++)
                                        {
                                            are.Visible[index] = sar.Visible[index];
                                        }
                                        //triggers
                                        foreach (Trigger tr in are.Triggers)
                                        {
                                            foreach (TriggerSave str in sar.Triggers)
                                            {
                                                if (tr.TriggerTag.Equals(str.TriggerTag))
                                                {
                                                    tr.Enabled = str.Enabled;
                                                    tr.EnabledEvent1 = str.EnabledEvent1;
                                                    tr.EnabledEvent2 = str.EnabledEvent2;
                                                    tr.EnabledEvent3 = str.EnabledEvent3;
                                                    tr.isShown = str.isShown;
                                                    //may want to copy the trigger's squares list from the save game if builders can modify the list with scripts
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            gv.mod.moduleAreasObjects.Add(are);
                            foreach (Area ar in mod.moduleAreasObjects)
                            {
                                Trigger trig = ar.getTriggerByTag(tag);
                                if (trig != null)
                                {
                                    trig.Enabled = false;
                                    trig.isShown = false;
                                    return;
                                }
                            }
                        }
                    }
                }
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "can't find designated trigger tag in area: " + areaName + ".are</font><BR>");
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to find trigger due to exception error" + "</font>" +
                            "<BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void ShowTriggerImage(string tag, string enabl, string areaName)
        {
            bool enable = Boolean.Parse(enabl);
            try
            {
                foreach (Area ar in mod.moduleAreasObjects)
                {
                    Trigger trig = ar.getTriggerByTag(tag);
                    if (trig != null)
                    {
                        trig.isShown = enable;
                        return;
                    }
                }
                string s = gv.LoadStringFromEitherFolder("\\modules\\" + gv.mod.moduleName + "\\" + areaName + ".are", "\\modules\\" + gv.mod.moduleName + "\\" + areaName + ".are");
                if (!s.Equals(""))
                {
                    using (StringReader sr = new StringReader(s))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        Area are = (Area)serializer.Deserialize(sr, typeof(Area));
                        if (are != null)
                        {
                            if (gv.cc.saveMod != null)
                            {
                                foreach (AreaSave sar in gv.cc.saveMod.moduleAreasObjects)
                                {
                                    if (sar.Filename.Equals(are.Filename)) //sar is saved game, ar is new game from toolset version
                                    {
                                        //tiles
                                        for (int index = 0; index < are.Visible.Count; index++)
                                        {
                                            are.Visible[index] = sar.Visible[index];
                                        }
                                        //triggers
                                        foreach (Trigger tr in are.Triggers)
                                        {
                                            foreach (TriggerSave str in sar.Triggers)
                                            {
                                                if (tr.TriggerTag.Equals(str.TriggerTag))
                                                {
                                                    tr.Enabled = str.Enabled;
                                                    tr.EnabledEvent1 = str.EnabledEvent1;
                                                    tr.EnabledEvent2 = str.EnabledEvent2;
                                                    tr.EnabledEvent3 = str.EnabledEvent3;
                                                    tr.isShown = str.isShown;
                                                    //may want to copy the trigger's squares list from the save game if builders can modify the list with scripts
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            gv.mod.moduleAreasObjects.Add(are);
                            foreach (Area ar in mod.moduleAreasObjects)
                            {
                                Trigger trig = ar.getTriggerByTag(tag);
                                if (trig != null)
                                {
                                    trig.isShown = enable;
                                    return;
                                }
                            }
                        }
                    }
                }
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "can't find designated trigger tag in any area" + "</font><BR>");
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to find trigger due to exception error" + "</font>" +
                            "<BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void EnableDisableTriggerAtCurrentLocation(string enabl)
        {
            bool enable = Boolean.Parse(enabl);
            try
            {
                Trigger trig = mod.currentArea.getTriggerByLocation(mod.PlayerLocationX, mod.PlayerLocationY);
                if (trig != null)
                {
                    trig.Enabled = enable;
                    return;
                }
                if (mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>can't find designated trigger at this location</font><BR>");
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>failed to find trigger due to exception error</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void EnableDisableTriggerEventAtCurrentLocation(string evNum, string enabl)
        {
            bool enable = Boolean.Parse(enabl);
            int eventNumber = Convert.ToInt32(evNum);

            try
            {
                Trigger trig = mod.currentArea.getTriggerByLocation(mod.PlayerLocationX, mod.PlayerLocationY);
                if (trig != null)
                {
                    if (eventNumber == 1)
                    {
                        trig.EnabledEvent1 = enable;
                    }
                    else if (eventNumber == 2)
                    {
                        trig.EnabledEvent2 = enable;
                    }
                    else if (eventNumber == 3)
                    {
                        trig.EnabledEvent3 = enable;
                    }
                    return;
                }
                if (mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>can't find designated trigger at this location</font><BR>");
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>failed to find trigger due to exception error</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void TogglePartyToken(string filename, string enabl)
        {
            bool enable = Boolean.Parse(enabl);
            try
            {
                if ((filename.Equals("none")) || (filename.Equals("")))
                {
                    //leave the token filename alone, use current filename
                }
                else
                {
                    gv.mod.partyTokenFilename = filename;
                }
                //gv.cc.DisposeOfBitmap(ref gv.mod.partyTokenBitmap);
                //gv.mod.partyTokenBitmap = gv.cc.LoadBitmap(gv.mod.partyTokenFilename);
                if (!mod.playerList[0].combatFacingLeft)
                {
//TODO                    mod.partyTokenBitmap = gv.cc.flip(mod.partyTokenBitmap);
                }
                gv.mod.showPartyToken = enable;
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to switch party token" + "</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }
        public void SetPropIsShown(string tag, string show)
        {
            /*bool shown = Boolean.Parse(show);
            Prop prp = gv.mod.currentArea.getPropByTag(tag);
            if (prp != null)
            {
                prp.isShown = shown;
            }
            else
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>didn't find prop in this area.</font><BR>");
                }
            }*/
        }
        
        /*public void RemovePropByTag(string tag)
        {
            try
            {
                foreach (Area ar in mod.moduleAreasObjects)
                {
                    Prop prp = ar.getPropByTag(tag);
                    if (prp != null)
                    {
                        ar.Props.Remove(prp);
                        if (mod.debugMode) //SD_20131102
                        {
                            gv.cc.addLogText("<font color='yellow'>" + "removed prop: " + tag + "</font><BR>");
                        }
                        return;
                    }
                }
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "can't find prop " + tag + " in any area" + "</font><BR>");
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to find prop due to exception error" + "</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }*/
        /*public void RemovePropByIndex(int index, string areafilename)
        {
            try
            {
                foreach (Area ar in mod.moduleAreasObjects)
                {
                    if (ar.Filename.Equals(areafilename))
                    {
                        if (index < ar.Props.Count)
                        {
                            if (mod.debugMode) //SD_20131102
                            {
                                gv.cc.addLogText("<font color='yellow'>" + "removed prop: " + ar.Props[index].PropTag + "</font><BR>");
                            }
                            ar.Props.RemoveAt(index);
                            return;
                        }
                    }
                }
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "can't find prop at index " + index + " in area " + areafilename + "</font><BR>");
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "failed to find prop due to exception error</font><BR>");
                }
                gv.errorLog(ex.ToString());
            }
        }*/
        public void GetPlayerHp(string tag, string index, string key)
        {
            Player pc = gv.mod.playerList[0];
            if ((tag != null) && (!tag.Equals("")))
            {
                pc = gv.mod.getPlayerByNameOrTag(tag);
                if (pc == null)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Could not find PC: " + tag + ", aborting</font><BR>");
                    }
                    return;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int parm2 = Convert.ToInt32(index);
                if ((parm2 >= 0) && (parm2 < gv.mod.playerList.Count))
                {
                    pc = gv.mod.playerList[parm2];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                    }
                    return;
                }
            }
            this.SetGlobalInt(key, pc.hp + "");
        }
        public void GetPlayerSp(string tag, string index, string key)
        {
            Player pc = gv.mod.playerList[0];
            if ((tag != null) && (!tag.Equals("")))
            {
                pc = gv.mod.getPlayerByNameOrTag(tag);
                if (pc == null)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Could not find PC: " + tag + ", aborting</font><BR>");
                    }
                    return;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int parm2 = Convert.ToInt32(index);
                if ((parm2 >= 0) && (parm2 < gv.mod.playerList.Count))
                {
                    pc = gv.mod.playerList[parm2];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                    }
                    return;
                }
            }
            this.SetGlobalInt(key, pc.sp + "");
        }
        public void SetPlayerHp(string tag, string index, string opertr, string value)
        {
            Player pc = gv.mod.playerList[0];
            if ((tag != null) && (!tag.Equals("")))
            {
                pc = gv.mod.getPlayerByNameOrTag(tag);
                if (pc == null)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Could not find PC: " + tag + ", aborting</font><BR>");
                    }
                    return;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int parm2 = Convert.ToInt32(index);
                if (parm2 == -1)
                {
                    parm2 = gv.mod.selectedPartyLeader;
                }
                if ((parm2 >= 0) && (parm2 < gv.mod.playerList.Count))
                {
                    pc = gv.mod.playerList[parm2];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                    }
                    return;
                }
            }
            if (opertr.Equals("+"))
            {
                pc.hp += Convert.ToInt32(value);
            }
            else if (opertr.Equals("-"))
            {
                pc.hp -= Convert.ToInt32(value);
            }
            else if (opertr.Equals("/"))
            {
                pc.hp /= Convert.ToInt32(value);
            }
            else if (opertr.Equals("*"))
            {
                pc.hp *= Convert.ToInt32(value);
            }
            else
            {
                pc.hp = Convert.ToInt32(value);
            }
        }
        public void SetPlayerBaseAtt(string tag, string index, string opertr, string value, string att)
        {
            Player pc = gv.mod.playerList[0];
            if ((tag != null) && (!tag.Equals("")))
            {
                pc = gv.mod.getPlayerByNameOrTag(tag);
                if (pc == null)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Could not find PC: " + tag + ", aborting</font><BR>");
                    }
                    return;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int parm2 = Convert.ToInt32(index);
                if (parm2 == -1)
                {
                    parm2 = gv.mod.selectedPartyLeader;
                }
                if ((parm2 >= 0) && (parm2 < gv.mod.playerList.Count))
                {
                    pc = gv.mod.playerList[parm2];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                    }
                    return;
                }
            }
            if (opertr.Equals("+"))
            {
                if (att.Equals("str"))
                {
                    pc.baseStr += Convert.ToInt32(value);
                }
                else if (att.Equals("dex"))
                {
                    pc.baseDex += Convert.ToInt32(value);
                }
                else if (att.Equals("int"))
                {
                    pc.baseInt += Convert.ToInt32(value);
                }
                else if (att.Equals("cha"))
                {
                    pc.baseCha += Convert.ToInt32(value);
                }
                else if (att.Equals("wis"))
                {
                    pc.baseCha += Convert.ToInt32(value);
                }
                else if (att.Equals("con"))
                {
                    pc.baseCha += Convert.ToInt32(value);
                }
            }
            else if (opertr.Equals("-"))
            {
                if (att.Equals("str"))
                {
                    pc.baseStr -= Convert.ToInt32(value);
                }
                else if (att.Equals("dex"))
                {
                    pc.baseDex -= Convert.ToInt32(value);
                }
                else if (att.Equals("int"))
                {
                    pc.baseInt -= Convert.ToInt32(value);
                }
                else if (att.Equals("cha"))
                {
                    pc.baseCha -= Convert.ToInt32(value);
                }
                else if (att.Equals("wis"))
                {
                    pc.baseCha -= Convert.ToInt32(value);
                }
                else if (att.Equals("con"))
                {
                    pc.baseCha -= Convert.ToInt32(value);
                }
            }
            else if (opertr.Equals("/"))
            {
                if (att.Equals("str"))
                {
                    pc.baseStr /= Convert.ToInt32(value);
                }
                else if (att.Equals("dex"))
                {
                    pc.baseDex /= Convert.ToInt32(value);
                }
                else if (att.Equals("int"))
                {
                    pc.baseInt /= Convert.ToInt32(value);
                }
                else if (att.Equals("cha"))
                {
                    pc.baseCha /= Convert.ToInt32(value);
                }
                else if (att.Equals("wis"))
                {
                    pc.baseCha /= Convert.ToInt32(value);
                }
                else if (att.Equals("con"))
                {
                    pc.baseCha /= Convert.ToInt32(value);
                }
            }
            else if (opertr.Equals("*"))
            {
                if (att.Equals("str"))
                {
                    pc.baseStr *= Convert.ToInt32(value);
                }
                else if (att.Equals("dex"))
                {
                    pc.baseDex *= Convert.ToInt32(value);
                }
                else if (att.Equals("int"))
                {
                    pc.baseInt *= Convert.ToInt32(value);
                }
                else if (att.Equals("cha"))
                {
                    pc.baseCha *= Convert.ToInt32(value);
                }
                else if (att.Equals("wis"))
                {
                    pc.baseCha *= Convert.ToInt32(value);
                }
                else if (att.Equals("con"))
                {
                    pc.baseCha *= Convert.ToInt32(value);
                }
            }
            else
            {
                if (att.Equals("str"))
                {
                    pc.baseStr = Convert.ToInt32(value);
                }
                else if (att.Equals("dex"))
                {
                    pc.baseDex = Convert.ToInt32(value);
                }
                else if (att.Equals("int"))
                {
                    pc.baseInt = Convert.ToInt32(value);
                }
                else if (att.Equals("cha"))
                {
                    pc.baseCha = Convert.ToInt32(value);
                }
                else if (att.Equals("wis"))
                {
                    pc.baseCha = Convert.ToInt32(value);
                }
                else if (att.Equals("con"))
                {
                    pc.baseCha = Convert.ToInt32(value);
                }
            }
        }
        public void AddSpellToPlayer(string tag, string index, string SpellTag)
        {
            Player pc = gv.mod.playerList[0];
            if ((tag != null) && (!tag.Equals("")))
            {
                pc = gv.mod.getPlayerByNameOrTag(tag);
                if (pc == null)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Could not find PC: " + tag + ", aborting</font><BR>");
                    }
                    return;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int parm2 = Convert.ToInt32(index);
                if (parm2 == -1)
                {
                    parm2 = gv.mod.selectedPartyLeader;
                }
                if ((parm2 >= 0) && (parm2 < gv.mod.playerList.Count))
                {
                    pc = gv.mod.playerList[parm2];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                    }
                    return;
                }
            }
            //get spell to add
            Spell sp = gv.cc.getSpellByTag(SpellTag);
            if (sp != null)
            {
                pc.knownSpellsTags.Add(sp.tag);
            }
            else if (mod.debugMode) //SD_20131102
            {
                gv.cc.addLogText("<font color='yellow'>Could not find Spell with tag: " + SpellTag + ", aborting</font><BR>");
            }
        }
        public void AddTraitToPlayer(string tag, string index, string TraitTag)
        {
            Player pc = gv.mod.playerList[0];
            if ((tag != null) && (!tag.Equals("")))
            {
                pc = gv.mod.getPlayerByNameOrTag(tag);
                if (pc == null)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Could not find PC: " + tag + ", aborting</font><BR>");
                    }
                    return;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int parm2 = Convert.ToInt32(index);
                if (parm2 == -1)
                {
                    parm2 = gv.mod.selectedPartyLeader;
                }
                if ((parm2 >= 0) && (parm2 < gv.mod.playerList.Count))
                {
                    pc = gv.mod.playerList[parm2];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                    }
                    return;
                }
            }
            //get trait to add
            Trait tr = gv.cc.getTraitByTag(TraitTag);
            if (tr != null)
            {
                pc.knownTraitsTags.Add(tr.tag);
            }
            else if (mod.debugMode) //SD_20131102
            {
                gv.cc.addLogText("<font color='yellow'>Could not find Trait with tag: " + TraitTag + ", aborting</font><BR>");
            }
        }
        public void AddAllowedItemToPlayerClass(string tag, string resref)
        {
            PlayerClass pcl = gv.cc.getPlayerClass(tag);
            if (pcl != null)
            {
                Item it = gv.cc.getItemByResRef(resref);
                if (it != null)
                {
                    if (it.containsAllowedClassByTag(pcl.tag))
                    {
                        if (mod.debugMode) //SD_20131102
                        {
                            gv.cc.addLogText("<font color='yellow'>Item is already allowed, aborting</font><BR>");
                        }
                        return;
                    }
                    else
                    {
                        it.classesAllowed.Add(pcl.tag);
                    }
                }
            }
        }
        public void RemoveAllowedItemFromPlayerClass(string tag, string resref)
        {
            PlayerClass pcl = gv.cc.getPlayerClass(tag);
            if (pcl != null)
            {
                Item it = gv.cc.getItemByResRef(resref);
                if (it != null)
                {
                    foreach (string s in it.classesAllowed)
                    {
                        if (s.Equals(tag))
                        {
                            it.classesAllowed.Remove(s);
                            return;
                        }
                    }
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Did't find Item to remove from known list, aborting</font><BR>");
                    }
                }
            }
        }
        public void SetPlayerSp(string tag, string index, string opertr, string value)
        {
            Player pc = gv.mod.playerList[0];
            if ((tag != null) && (!tag.Equals("")))
            {
                pc = gv.mod.getPlayerByNameOrTag(tag);
                if (pc == null)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Could not find PC: " + tag + ", aborting</font><BR>");
                    }
                    return;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int parm2 = Convert.ToInt32(index);
                if (parm2 == -1)
                {
                    parm2 = gv.mod.selectedPartyLeader;
                }
                if ((parm2 >= 0) && (parm2 < gv.mod.playerList.Count))
                {
                    pc = gv.mod.playerList[parm2];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                    }
                    return;
                }
            }
            if (opertr.Equals("+"))
            {
                pc.sp += Convert.ToInt32(value);
            }
            else if (opertr.Equals("-"))
            {
                pc.sp -= Convert.ToInt32(value);
            }
            else if (opertr.Equals("/"))
            {
                pc.sp /= Convert.ToInt32(value);
            }
            else if (opertr.Equals("*"))
            {
                pc.sp *= Convert.ToInt32(value);
            }
            else
            {
                pc.sp = Convert.ToInt32(value);
            }
        }
        public void SetCreatureSp(string tag, string index, string opertr, string value)
        {
            Creature crt = GetCreature(tag, index);
            if (crt == null)
            {
                return;
            }
            if (opertr.Equals("+"))
            {
                crt.sp += Convert.ToInt32(value);
            }
            else if (opertr.Equals("-"))
            {
                crt.sp -= Convert.ToInt32(value);
            }
            else if (opertr.Equals("/"))
            {
                crt.sp /= Convert.ToInt32(value);
            }
            else if (opertr.Equals("*"))
            {
                crt.sp *= Convert.ToInt32(value);
            }
            else
            {
                crt.sp = Convert.ToInt32(value);
            }
        }
        public void SetCreatureHp(string tag, string index, string opertr, string value)
        {
            Creature crt = GetCreature(tag, index);
            if (crt == null)
            {
                return;
            }
            if (opertr.Equals("+"))
            {
                crt.hp += Convert.ToInt32(value);
            }
            else if (opertr.Equals("-"))
            {
                crt.hp -= Convert.ToInt32(value);
            }
            else if (opertr.Equals("/"))
            {
                crt.hp /= Convert.ToInt32(value);
            }
            else if (opertr.Equals("*"))
            {
                crt.hp *= Convert.ToInt32(value);
            }
            else
            {
                crt.hp = Convert.ToInt32(value);
            }
        }
        public void SetWorldTime(string opertr, string value, string multsix)
        {
            if (opertr.Equals("+"))
            {
                mod.WorldTime += Convert.ToInt32(value);
            }
            else if (opertr.Equals("-"))
            {
                mod.WorldTime -= Convert.ToInt32(value);
            }
            else
            {
                mod.WorldTime = Convert.ToInt32(value);
            }
            //round to nearest multiple of 6
            if (multsix.Equals("true"))
            {
                mod.WorldTime = (mod.WorldTime / 6) * 6;
            }
            if (mod.WorldTime < 0)
            {
                mod.WorldTime = 0;
            }
        }
        public void ApplyPartyDamage(int dam)
        {
            foreach (Player pc in mod.playerList)
            {
                gv.cc.addLogText("<font color='yellow'>" + pc.name + " takes " + dam + " damage" + "</font><BR>");
                pc.hp -= dam;
                if (pc.hp <= 0)
                {
                    gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                    pc.charStatus = "Dead";
                }
            }
            gv.screenMainMap.addFloatyText(gv.mod.PlayerLocationX + 2, gv.mod.PlayerLocationY - 2, dam.ToString(), "red", 4000);
        }
        public void riddle()
        {
            /*TODO
            AlertDialog.Builder builder = new AlertDialog.Builder(gv.gameContext);
            builder.setTitle("Engraved you find the following:");
            builder.setMessage(Html.fromHtml("<i>\"First of the First King of Men only afew;<br>Second of the First Kingdom on lands anew;<br>Third of the First City in icy blue;<br>Fourth of the foe a friend he grew;<br>Right in the center of me and you.</i><br><br>Type in your answer below:"));

            // Set up the input
            final EditText input = new EditText(gv.gameContext);
            // Specify the type of input expected
            input.setInputType(InputType.TYPE_CLASS_TEXT);
            builder.setView(input);

            // Set up the buttons
            builder.setPositiveButton("Speak Answer", new DialogInterface.OnClickListener()
            {
                @Override
                public void onClick(DialogInterface dialog, int which)
                {
                    //if (input.getText().toString().length() > 0)
                    if ((input.getText().toString().equals("core") || input.getText().toString().equals("Core")))
                    {
                        EnableDisableTriggerEvent("riddleTrig", "1", "false");
                        EnableDisableTriggerEvent("riddleTrig", "2", "true");
                        MessageBox("That is correct...the chest opens");
                    }
                    else
                    {
                        MessageBox("Incorrect, try again");
                    }

                }
            });

            builder.setNegativeButton("Leave Chest Alone", new DialogInterface.OnClickListener()
            {
                @Override
                public void onClick(DialogInterface dialog, int which)
                {
                    dialog.cancel();
                }
            });

            builder.show();
            */
        }
        public void DamageWithoutItem(int damage, string itemTag)
        {
            bool itemfound = CheckForItem(itemTag, 1);
            if (itemfound)
            {
                //have item so 10% chance to damage
                if (RandInt(100) > 90)
                {
                    //do damage to all
                    gv.cc.addLogText("<font color='aqua'>drowning (-1hp ea)</font><br>");
                    foreach (Player pc in mod.playerList)
                    {
                        pc.hp -= damage;
                        if (pc.hp <= 0)
                        {
                            gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                            pc.charStatus = "Dead";
                        }
                    }
                }
            }
            else
            {
                //do not have item so 50% chance to damage
                if (RandInt(100) > 50)
                {
                    //do damage to all
                    gv.cc.addLogText("<font color='aqua'>drowning (-1hp ea)</font><br>");
                    foreach (Player pc in mod.playerList)
                    {
                        pc.hp -= damage;
                        if (pc.hp <= 0)
                        {
                            gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                            pc.charStatus = "Dead";
                        }
                    }
                }
            }
        }
        public void AddCreatureToCurrentEncounter(string p1, string p2, string p3)
        {
            //p1 is the resref of the added creature (use one from a blueprint in the toolset's creature blueprints section)
            //p2 x location of the creature in current encounter (will be automatically adjusted to nearest location if the spot is already occupied or non-walkable)
            //p3 y location of the creature in current encounter (will be automatically adjusted to nearest location if the spot is already occupied or non-walkable)

            foreach (Creature c in gv.mod.moduleCreaturesList)
            {
                if (c.cr_resref.Equals(p1))
                {
                    //fetch the data for our creature by making a blueprint(object) copy
                    Creature copy = c.DeepCopy();
                    //Automaically create a unique tag                    
                    copy.cr_tag = "SummonTag" + mod.getNextIdNumber();
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<bu>Added Creature tag is: " + copy.cr_tag + "</bu>");
                    }

                    //find correct summon spot, replace with nearest location if neccessary
                    copy.combatLocX = Convert.ToInt32(p2);// x destination intended for the summon
                    copy.combatLocY = Convert.ToInt32(p3);// y destination intended for the summon
                    bool changeSummonLocation = false;// used as switch for cycling through all tiles in case the originally intended spot was occupied/not-walkable
                    int targetTile = copy.combatLocY * gv.mod.currentEncounter.MapSizeX + copy.combatLocX;//the index of the original target spot in the encounter's tiles list
                    List<int> freeTilesByIndex = new List<int>();// a new list used to store the indices of all free tiles in the enocunter
                    int tileLocX = 0;//just temporary storage in for locations of tiles
                    int tileLocY = 0;//just temporary storage in for locations of tiles
                    double floatTileLocY = 0;//was uncertain about rounding and conversion details, therefore need this one (see below)
                    bool tileIsFree = true;//identify a tile suited as new summon loaction
                    int nearestTileByIndex = -1;//store the nearest tile by index; as the relevant loop runs this will be replaced several times likely with ever nearer tiles
                    int dist = 0;//distance between the orignally intended summon location and a free tile
                    int lowestDist = 10000;//this storest the lowest ditance found while the loop runs
                    int deltaX = 0;//temporary value used for distance calculation 
                    int deltaY = 0;//temporary value used for distance calculation 

                    //Check whether the target tile is free (then it's not neccessary to loop through any other tiles)
                    //three checks are done in the following: walkable, occupied by creature, occupied by pc

                    //first check: check walkable
                    if (gv.mod.currentEncounter.Walkable[targetTile] == 0)
                    {
                        changeSummonLocation = true;
                    }

                    //second check: check occupied by creature (only necceessary if walkable)
                    if (changeSummonLocation == false)
                    {
                        foreach (Creature cr in gv.mod.currentEncounter.encounterCreatureList)
                        {
                            if ((cr.combatLocX == copy.combatLocX) && (cr.combatLocY == copy.combatLocY))
                            {
                                changeSummonLocation = true;
                                break;
                            }
                        }
                    }

                    //third check: check occupied by pc (only necceessary if walkable and not occupied by creature)
                    if (changeSummonLocation == false)
                    {
                        foreach (Player pc in gv.mod.playerList)
                        {
                            if ((pc.combatLocX == copy.combatLocX) && (pc.combatLocY == copy.combatLocY))
                            {
                                changeSummonLocation = true;
                                break;
                            }
                        }
                    }

                    //target square was already occupied/non-walkable, so all other tiles are searched for the NEAREST FREE tile to switch the summon location to
                    if (changeSummonLocation == true)
                    {
                        //FIRST PART: get all FREE tiles in the current encounter
                        for (int i = 0; i < gv.mod.currentEncounter.Layer1Filename.Count; i++)
                        {
                            //get the x and y location of current tile by calculation derived from index number, assuming that counting starts at top left corner of a map (0x, 0y)
                            //and that each horizintal x-line is counted first, then counting next horizonal x-line starting from the left again
                            tileIsFree = true;
                            //Note: When e.g. MapsizeY is 7, the y values range from 0 to 6
                            tileLocX = i % gv.mod.currentEncounter.MapSizeY;
                            //Note: ensure rounding down here 
                            floatTileLocY = i / gv.mod.currentEncounter.MapSizeX;
                            tileLocY = (int)Math.Floor(floatTileLocY);

                            //look at content of currently checked tile, again with three checks for walkable, occupied by creature, occupied by pc
                            //walkbale check
                            if (gv.mod.currentEncounter.Walkable[i] == 0)
                            {
                                tileIsFree = false;
                            }

                            //creature occupied check
                            if (tileIsFree == true)
                            {
                                foreach (Creature cr in gv.mod.currentEncounter.encounterCreatureList)
                                {
                                    if ((cr.combatLocX == tileLocX) && (cr.combatLocY == tileLocY))
                                    {
                                        tileIsFree = false;
                                        break;
                                    }
                                }
                            }

                            //pc occupied check
                            if (tileIsFree == true)
                            {
                                foreach (Player pc in gv.mod.playerList)
                                {
                                    if ((pc.combatLocX == tileLocX) && (pc.combatLocY == tileLocY))
                                    {
                                        tileIsFree = false;
                                        break;
                                    }
                                }
                            }

                            //this writes all free tiles into a fresh list; please note that the values of the elements of this new list are our relevant index values
                            //therefore it's not the index (which doesnt correalte to locations) in this list that's relevant, but the value of the element at that index
                            if (tileIsFree == true)
                            {
                                freeTilesByIndex.Add(i);
                            }
                        }

                        //SECOND PART: find the free tile NEAREST to originally intended summon location
                        for (int i = 0; i < freeTilesByIndex.Count; i++)
                        {
                            dist = 0;

                            //get location x and y of the tile stored at the index number i, i.e. get the value of elment indexed with i and transform to x and y location
                            tileLocX = freeTilesByIndex[i] % gv.mod.currentEncounter.MapSizeY;
                            floatTileLocY = freeTilesByIndex[i] / gv.mod.currentEncounter.MapSizeX;
                            tileLocY = (int)Math.Floor(floatTileLocY);

                            //get distance between the current free tile and the originally intended summon location
                            deltaX = (int)Math.Abs((tileLocX - copy.combatLocX));
                            deltaY = (int)Math.Abs((tileLocY - copy.combatLocY));
                            if (deltaX > deltaY)
                            {
                                dist = deltaX;
                            }
                            else
                            {
                                dist = deltaY;
                            }

                            //filter out the nearest tile by remembering it and its distance for further comparison while the loop runs through all free tiles
                            if (dist < lowestDist)
                            {
                                lowestDist = dist;
                                nearestTileByIndex = freeTilesByIndex[i];
                            }
                        }

                        if (nearestTileByIndex != -1)
                        {
                            //get the nearest tile's x and y location and use it as creature summon coordinates
                            tileLocX = nearestTileByIndex % gv.mod.currentEncounter.MapSizeY;
                            floatTileLocY = nearestTileByIndex / gv.mod.currentEncounter.MapSizeX;
                            tileLocY = (int)Math.Floor(floatTileLocY);

                            copy.combatLocX = tileLocX;
                            copy.combatLocY = tileLocY;
                        }

                    }

                    //just check whether a free squre does exist at all; if not, do not complete the summon
                    if ((nearestTileByIndex != -1) || (changeSummonLocation == false))
                    {
                        copy.moveOrder = gv.screenCombat.moveOrderList.Count;
                        //finally add creature
                        mod.currentEncounter.encounterCreatureList.Add(copy);
                        //add to end of move order
                        MoveOrder newMO = new MoveOrder();
                        newMO.PcOrCreature = copy;
                        newMO.rank = 100;
                        gv.screenCombat.moveOrderList.Add(newMO);
                        //increment the number of initial move order objects
                        gv.screenCombat.initialMoveOrderListSize++;
                        //add to encounter xp
                        gv.screenCombat.encounterXP += copy.cr_XP;
                    }
                }
            }
        }
        public void ModifyBuyBack(string shoptag, string opertr, string value)
        {
            Shop shp = gv.mod.getShopByTag(shoptag);
            if (shp == null)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<yl>Could not find Shop: " + shoptag + ", aborting</yl><BR>");
                }
                return;                
            }
            try
            {
                if (opertr.Equals("+"))
                {
                    shp.buybackModifier += Convert.ToInt32(value);
                }
                else if (opertr.Equals("-"))
                {
                    shp.buybackModifier -= Convert.ToInt32(value);
                }
                else
                {
                    shp.buybackModifier = Convert.ToInt32(value);
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<yl>Error modifying shop buyback: " + ex.ToString() + ", aborting</yl><BR>");
                }
                return;
            }
        }
        public void ModifySellPrice(string shoptag, string opertr, string value)
        {
            Shop shp = gv.mod.getShopByTag(shoptag);
            if (shp == null)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<yl>Could not find Shop: " + shoptag + ", aborting</yl><BR>");
                }
                return;
            }
            try
            {
                if (opertr.Equals("+"))
                {
                    shp.sellModifier += Convert.ToInt32(value);
                }
                else if (opertr.Equals("-"))
                {
                    shp.sellModifier -= Convert.ToInt32(value);
                }
                else
                {
                    shp.sellModifier = Convert.ToInt32(value);
                }
            }
            catch (Exception ex)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<yl>Error modifying shop buyback: " + ex.ToString() + ", aborting</yl><BR>");
                }
                return;
            }
        }

        public bool CheckForItem(string resref, int quantity)
        {
            //check if item is on any of the party members
            if (mod.debugMode) //SD_20131102
            {
                gv.cc.addLogText("<font color='yellow'>" + "checkForItemResRef: " + resref + " quantity: " + quantity + "</font><BR>");
            }
            int numFound = 0;
            foreach (Player pc in mod.playerList)
            {
                if (pc.BodyRefs.resref.Equals(resref)) { numFound++; }
                if (pc.MainHandRefs.resref.Equals(resref)) { numFound++; }
                if (pc.RingRefs.resref.Equals(resref)) { numFound++; }
                if (pc.OffHandRefs.resref.Equals(resref)) { numFound++; }
                if (pc.HeadRefs.resref.Equals(resref)) { numFound++; }
                if (pc.NeckRefs.resref.Equals(resref)) { numFound++; }
                if (pc.Ring2Refs.resref.Equals(resref)) { numFound++; }
                if (pc.FeetRefs.resref.Equals(resref)) { numFound++; }
            }
            foreach (ItemRefs item in mod.partyInventoryRefsList)
            {
                if (item.resref.Equals(resref)) { numFound += item.quantity; }
            }
            if (numFound >= quantity)
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "found enough: " + resref + " numFound: " + numFound + "</font><BR>");
                }
                return true;
            }
            else
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>" + "didn't find enough: " + resref + " numFound: " + numFound + "</font><BR>");
                }
                return false;
            }
        }
        public bool CheckIsRace(int PCIndex, string tag)
        {
            if (mod.playerList[PCIndex].race.tag.Equals(tag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*public bool CheckPartyDistance(string tag, int distance)
        {
            Prop prp = GetProp(tag, "");
            if (prp == null)
            {
                if (mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>error finding prop in this area, returning 'false'</font><BR>");
                }
                return false;
            }
            
            int dist = 0;
            int deltaX = (int)Math.Abs((prp.LocationX - mod.PlayerLocationX));
            int deltaY = (int)Math.Abs((prp.LocationY - mod.PlayerLocationY));
            if (deltaX > deltaY)
            {
                dist = deltaX;
            }
            else
            {
                dist = deltaY;
            }
            if (mod.debugMode)
            {
                gv.cc.addLogText("<font color='yellow'>party distance from " + prp.PropTag + " is " + dist + "</font><BR>");
            }
            if (dist <= distance)
            {
                return true;
            }
            return false;
        }*/
        public bool CheckHasTrait(int PCIndex, string tag)
        {
            foreach (string s in mod.playerList[PCIndex].knownTraitsTags)
            {
                if (tag.Equals(s))
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckPassSkill(int PCIndex, string tag, int dc)
        {
            string foundLargest = "none";
            int largest = 0;
            foreach (string s in mod.playerList[PCIndex].knownTraitsTags)
            {
                if (s.StartsWith(tag))
                {
                    if (s.Equals(tag))
                    {
                        if (foundLargest.Equals("none"))
                        {
                            foundLargest = s;
                        }
                    }
                    else //get the number at the end 
                    {
                        string c = s.Substring(s.Length - 1, 1);
                        int i = Convert.ToInt32(c);
                        if (i > largest)
                        {
                            largest = i;
                            foundLargest = s;
                        }
                    }
                }
            }
            if (!foundLargest.Equals("none"))
            {
                //PC has trait skill so do calculation check
                Trait tr = gv.cc.getTraitByTag(foundLargest);
                int skillMod = tr.skillModifier;
                int attMod = 0;
                if (tr.skillModifierAttribute.Equals("str"))
                {
                    attMod = (mod.playerList[PCIndex].strength - 10) / 2;
                }
                else if (tr.skillModifierAttribute.Equals("dex"))
                {
                    attMod = (mod.playerList[PCIndex].dexterity - 10) / 2;
                }
                else if (tr.skillModifierAttribute.Equals("int"))
                {
                    attMod = (mod.playerList[PCIndex].intelligence - 10) / 2;
                }
                else if (tr.skillModifierAttribute.Equals("cha"))
                {
                    attMod = (mod.playerList[PCIndex].charisma - 10) / 2;
                }
                else if (tr.skillModifierAttribute.Equals("con"))
                {
                    attMod = (mod.playerList[PCIndex].constitution - 10) / 2;
                }
                else if (tr.skillModifierAttribute.Equals("wis"))
                {
                    attMod = (mod.playerList[PCIndex].wisdom - 10) / 2;
                }
                int roll = gv.sf.RandInt(20);
                if (roll + attMod + skillMod >= dc)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'> skill check(" + tag + "): " + roll + "+" + attMod + "+" + skillMod + ">=" + dc + "</font><BR>");
                    }
                    return true;
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'> skill check: " + roll + "+" + attMod + "+" + skillMod + "<" + dc + "</font><BR>");
                    }
                    return false;
                }
            }
            return false;
        }
        public bool CheckIsMale(int PCIndex)
        {
            if (mod.playerList[PCIndex].isMale)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckIsClassLevel(int PCIndex, string tag, int level)
        {
            if (mod.playerList[PCIndex].playerClass.tag.Equals(tag))
            {
                if (mod.playerList[PCIndex].classLevel >= level)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool CheckIsLevel(int PCIndex, int level)
        {
            if (mod.playerList[PCIndex].classLevel >= level)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckFunds(int amount)
        {
            if (mod.partyGold >= amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckAttribute(int PCIndex, string attribute, string compare, int value)
        {
            int pcAttValue = 0;
            if (attribute.Equals("str"))
            {
                pcAttValue = mod.playerList[PCIndex].strength;
            }
            else if (attribute.Equals("dex"))
            {
                pcAttValue = mod.playerList[PCIndex].dexterity;
            }
            else if (attribute.Equals("int"))
            {
                pcAttValue = mod.playerList[PCIndex].intelligence;
            }
            else if (attribute.Equals("cha"))
            {
                pcAttValue = mod.playerList[PCIndex].charisma;
            }
            else if (attribute.Equals("con"))
            {
                pcAttValue = mod.playerList[PCIndex].constitution;
            }
            else if (attribute.Equals("wis"))
            {
                pcAttValue = mod.playerList[PCIndex].wisdom;
            }
            else
            {
                return false;
            }

            if (compare.Equals("="))
            {
                if (pcAttValue == value)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "pcAttValue: " + pcAttValue + " == " + value + "</font><BR>");
                    }
                    return true;
                }
            }
            else if (compare.Equals(">"))
            {
                if (pcAttValue > value)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "pcAttValue: " + pcAttValue + " > " + value + "</font><BR>");
                    }
                    return true;
                }
            }
            else if (compare.Equals("<"))
            {
                if (pcAttValue < value)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "pcAttValue: " + pcAttValue + " < " + value + "</font><BR>");
                    }
                    return true;
                }
            }
            else if (compare.Equals("!"))
            {
                if (pcAttValue != value)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "pcAttValue: " + pcAttValue + " != " + value + "</font><BR>");
                    }
                    return true;
                }
            }
            return false;
        }
        /*public bool CheckProp(string tag, string index, string property)
        {
            if (gv.mod.currentArea.Props.Count < 1)
            {
                if (mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>didn't find prop in this area, returning 'false'</font><BR>");
                }
                return false;
            }

            Prop prp = gv.mod.currentArea.Props[0];
            if ((tag != null) && (!tag.Equals("")))
            {
                if (tag.Equals("thisProp"))
                {
                    prp = ThisProp;
                }
                else
                {
                    prp = gv.mod.currentArea.getPropByTag(tag);
                }
                if (prp == null)
                {
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>didn't find prop in this area (prop=null), returning 'false'</font><BR>");
                    }
                    return false;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int indx = Convert.ToInt32(index);
                if ((indx >= 0) && (indx < gv.mod.currentArea.Props.Count))
                {
                    prp = gv.mod.currentArea.Props[indx];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Prop index outside range of PropList size, returning 'false'</font><BR>");
                    }
                    return false;
                }
            }
            else
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>Do not recognize the Prop's tag or index, returning 'false'</font><BR>");
                }
            }

            if ((property.Equals("s")) || (property.Equals("S")) || (property.Equals("isShown")))
            {
                return prp.isShown;
            }
            else if ((property.Equals("a")) || (property.Equals("A")) || (property.Equals("isActive")))
            {
                return prp.isActive;
            }
            else if ((property.Equals("h")) || (property.Equals("H")) || (property.Equals("HasCollisions")))
            {
                return prp.HasCollision;
            }
            else
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>Do not recognize the property: " + property + ", returning 'false'</font><BR>");
                }
            }
            return false;
        }*/
        /*public void SetProp(string tag, string index, string property, string bln)
        {
            if (gv.mod.currentArea.Props.Count < 1)
            {
                if (mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>didn't find prop in this area, aborting SetProp</font><BR>");
                }
                return;
            }

            Prop prp = gv.mod.currentArea.Props[0];
            if ((tag != null) && (!tag.Equals("")))
            {
                if (tag.Equals("thisProp"))
                {
                    prp = ThisProp;
                }
                else
                {
                    prp = gv.mod.currentArea.getPropByTag(tag);
                }
                if (prp == null)
                {
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>didn't find prop in this area (prop=null), aborting SetProp</font><BR>");
                    }
                    return;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int indx = Convert.ToInt32(index);
                if ((indx >= 0) && (indx < gv.mod.currentArea.Props.Count))
                {
                    prp = gv.mod.currentArea.Props[indx];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Prop index outside range of PropList size, aborting SetProp</font><BR>");
                    }
                    return;
                }
            }
            else
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>Do not recognize the Prop's tag or index, aborting SetProp</font><BR>");
                }
            }

            bool setBool = Boolean.Parse(bln);

            if ((property.Equals("s")) || (property.Equals("S")) || (property.Equals("isShown")))
            {
                prp.isShown = setBool;
                if (mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>prop isShown set to " + bln + "</font><BR>");
                }
                return;
            }
            else if ((property.Equals("a")) || (property.Equals("A")) || (property.Equals("isActive")))
            {
                prp.isActive = setBool;
                if (mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>prop isActive set to " + bln + "</font><BR>");
                }
                return;
            }
            else if ((property.Equals("h")) || (property.Equals("H")) || (property.Equals("HasCollisions")))
            {
                prp.HasCollision = setBool;
                if (mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>prop HasCollisions set to " + bln + "</font><BR>");
                }
                return;
            }
            else
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>Do not recognize the property: " + property + ", aborting SetProp</font><BR>");
                }
            }
        }*/
        /*public Prop GetProp(string tag, string index)
        {
            Prop prp = null;
            if ((tag != null) && (!tag.Equals("")))
            {
                if (tag.Equals("thisProp"))
                {
                    prp = ThisProp;
                }
                else
                {
                    prp = gv.mod.currentArea.getPropByTag(tag);
                }
                if (prp == null)
                {
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>didn't find prop in this area (prop=null)</font><BR>");
                    }
                    return null;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int indx = Convert.ToInt32(index);
                if ((indx >= 0) && (indx < gv.mod.currentArea.Props.Count))
                {
                    prp = gv.mod.currentArea.Props[indx];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Prop index outside range of PropList size</font><BR>");
                    }
                    return null;
                }
            }
            else
            {
                if (mod.debugMode) //SD_20131102
                {
                    gv.cc.addLogText("<font color='yellow'>Do not recognize the Prop's tag or index</font><BR>");
                }
            }
            return prp;
        }*/

        /*public Prop GetPropByUniqueTag(string tag)
        {
            Prop prp = null;

            if ((tag != null) && (!tag.Equals("")))
            {
                if (tag.Equals("thisProp"))
                {
                    prp = ThisProp;
                }
                else
                {
                    for (int i = 0; i < gv.mod.moduleAreasObjects.Count; i++)
                    {
                        for (int j = 0; j < gv.mod.moduleAreasObjects[i].Props.Count; j++)
                        {
                            if (gv.mod.moduleAreasObjects[i].Props[j].PropTag.Equals(tag))
                            {
                                prp = gv.mod.moduleAreasObjects[i].Props[j];
                                return prp;
                            }
                        }
                    }
                }
            }
            else return prp;
            return prp;
        }*/


        public Creature GetCreature(string tag, string index)
        {
            Creature crt = null;
            if ((tag != null) && (!tag.Equals("")))
            {
                if (tag.Equals("thisCreature"))
                {
                    crt = ThisCreature;
                }
                else
                {
                    crt = gv.screenCombat.GetCreatureByTag(tag);
                }
                if (crt == null)
                {
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>Could not find creature with tag: " + tag + ", aborting</font><BR>");
                    }
                    return null;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int parm2 = Convert.ToInt32(index);
                if ((parm2 >= 0) && (parm2 < gv.mod.currentEncounter.encounterCreatureList.Count))
                {
                    crt = gv.mod.currentEncounter.encounterCreatureList[parm2];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>index outside range of creatureList size, aborting</font><BR>");
                    }
                    return null;
                }
            }
            return crt;
        }
        public Player GetPlayer(string tag, string index)
        {
            Player pc = null;
            if ((tag != null) && (!tag.Equals("")))
            {
                pc = gv.mod.getPlayerByNameOrTag(tag);
                if (pc == null)
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>Could not find PC: " + tag + ", aborting</font><BR>");
                    }
                    return null;
                }
            }
            else if ((index != null) && (!index.Equals("")))
            {
                int parm2 = Convert.ToInt32(index);
                if (parm2 == -1)
                {
                    parm2 = gv.mod.selectedPartyLeader;
                }
                if ((parm2 >= 0) && (parm2 < gv.mod.playerList.Count))
                {
                    pc = gv.mod.playerList[parm2];
                }
                else
                {
                    if (mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<font color='yellow'>index outside range of playerList size, aborting</font><BR>");
                    }
                    return null;
                }
            }
            return pc;
        }

        public bool CheckJournalEntry(string categoryTag, string compareOperator, int entryId)
        {
            foreach (JournalQuest quest in mod.partyJournalQuests)
            {
                if (quest.Tag.Equals(categoryTag))
                {
                    if (compareOperator.Equals("="))
                    {
                        foreach (JournalEntry entry in quest.Entries)
                        {
                            if (entry.EntryId == entryId)
                            {
                                return true;
                            }
                        }
                    }
                    else if (compareOperator.Equals(">"))
                    {
                        foreach (JournalEntry entry in quest.Entries)
                        {
                            if (entry.EntryId > entryId)
                            {
                                return true;
                            }
                        }
                    }
                    else if (compareOperator.Equals("<"))
                    {
                        foreach (JournalEntry entry in quest.Entries)
                        {
                            if (entry.EntryId >= entryId)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    else if (compareOperator.Equals("!"))
                    {
                        foreach (JournalEntry entry in quest.Entries)
                        {
                            if (entry.EntryId == entryId)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }                
            }
            foreach (JournalQuest quest in mod.partyJournalCompleted)
            {
                if (quest.Tag.Equals(categoryTag))
                {
                    if (compareOperator.Equals("="))
                    {
                        foreach (JournalEntry entry in quest.Entries)
                        {
                            if (entry.EntryId == entryId)
                            {
                                return true;
                            }
                        }
                    }
                    else if (compareOperator.Equals(">"))
                    {
                        foreach (JournalEntry entry in quest.Entries)
                        {
                            if (entry.EntryId > entryId)
                            {
                                return true;
                            }
                        }
                    }
                    else if (compareOperator.Equals("<"))
                    {
                        foreach (JournalEntry entry in quest.Entries)
                        {
                            if (entry.EntryId >= entryId)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    else if (compareOperator.Equals("!"))
                    {
                        foreach (JournalEntry entry in quest.Entries)
                        {
                            if (entry.EntryId == entryId)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        public void AddJournalEntry(string categoryTag, string entryTag)
        {
            JournalQuest jcm = mod.getJournalCategoryByTag(categoryTag);
            if (jcm != null)
            {
                JournalQuest jcp = mod.getPartyJournalActiveCategoryByTag(categoryTag);
                if (jcp != null) //an existing category, just add entry
                {
                    JournalEntry jem = jcm.getJournalEntryByTag(entryTag);
                    if (jem != null)
                    {
                        if (!entryAlreadyExists(jem.Tag))
                        {
                            jcp.Entries.Add(jem);
                            //if (jem.EndPoint)
                            //{
                            //    mod.partyJournalCompleted.Add(jcp);
                            //    mod.partyJournalQuests.Remove(jcp);
                            //}
                            //Toast.makeText(gv.gameContext, "Your journal has been updated with: " + jem.EntryTitle, Toast.LENGTH_LONG).show();
                            //gv.TrackerSendEvent("Journal", jcp.Name, jem.EntryTitle, 0l);
                            gv.TrackerSendEventJournal(jcp.Name + " -- " + jem.EntryTitle);
                        }
                    }
                    else
                    {
                        //Toast.makeText(gv.gameContext, "module's journal entry wasn't found based on tag given", Toast.LENGTH_LONG).show();
                    }
                }
                else //a new category, add category and entry
                {
                    JournalQuest jcp2 = mod.getJournalCategoryByTag(categoryTag).DeepCopy();
                    //Toast.makeText(gv.gameContext, "player's journal category wasn't found based on tag given, creating category...", Toast.LENGTH_SHORT).show();
                    //MessageBox.Show("player's journal category wasn't found based on tag given, creating category...");
                    JournalEntry jem = jcm.getJournalEntryByTag(entryTag);
                    if (jem != null)
                    {
                        jcp2.Entries.Clear();
                        jcp2.Entries.Add(jem);
                        mod.partyJournalQuests.Add(jcp2);
                        //Toast.makeText(gv.gameContext, "Your journal has been updated with: " + jem.EntryTitle, Toast.LENGTH_LONG).show();
                        //gv.TrackerSendEvent("Journal", jcp2.Name, jem.EntryTitle, 0l);
                        gv.TrackerSendEventJournal(jcp2.Name + " -- " + jem.EntryTitle);
                    }
                    else
                    {
                        //Toast.makeText(gv.gameContext, "module's journal entry wasn't found based on tag given", Toast.LENGTH_LONG).show();
                    }
                }
            }
            else
            {
                //Toast.makeText(gv.gameContext, "module's journal category wasn't found based on tag given", Toast.LENGTH_LONG).show();
            }
        }
        public void AddJournalEntryNoMessages(string categoryTag, string entryTag)
        {
            JournalQuest jcm = mod.getJournalCategoryByTag(categoryTag);
            if (jcm != null)
            {
                JournalQuest jcp = mod.getPartyJournalActiveCategoryByTag(categoryTag);
                if (jcp != null) //an existing category, just add entry
                {
                    JournalEntry jem = jcm.getJournalEntryByTag(entryTag);
                    if (jem != null)
                    {
                        if (!entryAlreadyExists(jem.Tag))
                        {
                            jcp.Entries.Add(jem);
                            //if (jem.EndPoint)
                            //{
                            //    mod.partyJournalCompleted.Add(jcp);
                            //    mod.partyJournalQuests.Remove(jcp);
                            //}
                            //Toast.makeText(gv.gameContext, "Your journal has been updated with: " + jem.EntryTitle, Toast.LENGTH_LONG).show();
                        }
                    }
                    else
                    {
                        //Toast.makeText(gv.gameContext, "module's journal entry wasn't found based on tag given", Toast.LENGTH_LONG).show();
                    }
                }
                else //a new category, add category and entry
                {
                    JournalQuest jcp2 = mod.getJournalCategoryByTag(categoryTag).DeepCopy();
                    //Toast.makeText(gv.gameContext, "player's journal category wasn't found based on tag given, creating category...", Toast.LENGTH_SHORT).show();
                    JournalEntry jem = jcm.getJournalEntryByTag(entryTag);
                    if (jem != null)
                    {
                        jcp2.Entries.Clear();
                        jcp2.Entries.Add(jem);
                        mod.partyJournalQuests.Add(jcp2);
                        //Toast.makeText(gv.gameContext, "Your journal has been updated with: " + jem.EntryTitle, Toast.LENGTH_LONG).show();
                        //if (jem.EndPoint)
                        //{
                        //    mod.partyJournalCompleted.Add(jcp2);
                        //    mod.partyJournalQuests.Remove(jcp2);
                        //}
                    }
                    else
                    {
                        //Toast.makeText(gv.gameContext, "module's journal entry wasn't found based on tag given", Toast.LENGTH_LONG).show();
                    }
                }
            }
            else
            {
                //Toast.makeText(gv.gameContext, "module's journal category wasn't found based on tag given", Toast.LENGTH_LONG).show();
            }
        }
        public bool entryAlreadyExists(string entryTag)
        {
            foreach (JournalQuest quest in mod.partyJournalQuests)
            {
                foreach (JournalEntry entry in quest.Entries)
                {
                    if (entry.Tag.Equals(entryTag))
                    {
                        return true;
                    }
                }
            }
            foreach (JournalQuest quest in mod.partyJournalCompleted)
            {
                foreach (JournalEntry entry in quest.Entries)
                {
                    if (entry.Tag.Equals(entryTag))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //UPDATE STATS
        public void UpdateStats(Player pc)
        {
            //used at level up, doPcTurn, open inventory, etc.
            ReCalcSavingThrowBases(pc); //SD_20131029
            pc.fortitude = pc.baseFortitude + CalcSavingThrowModifiersFortitude(pc) + (pc.constitution - 10) / 2; //SD_20131127
            pc.will = pc.baseWill + CalcSavingThrowModifiersWill(pc) + (pc.intelligence - 10) / 2; //SD_20131127
            pc.reflex = pc.baseReflex + CalcSavingThrowModifiersReflex(pc) + (pc.dexterity - 10) / 2; //SD_20131127
            pc.strength = pc.baseStr + pc.race.strMod + CalcAttributeModifierStr(pc); //SD_20131127
            pc.dexterity = pc.baseDex + pc.race.dexMod + CalcAttributeModifierDex(pc); //SD_20131127
            pc.intelligence = pc.baseInt + pc.race.intMod + CalcAttributeModifierInt(pc); //SD_20131127
            pc.charisma = pc.baseCha + pc.race.chaMod + CalcAttributeModifierCha(pc); //SD_20131127
            pc.wisdom = pc.baseWis + pc.race.wisMod + CalcAttributeModifierWis(pc); //SD_20131127
            pc.constitution = pc.baseCon + pc.race.conMod + CalcAttributeModifierCon(pc); //SD_20131127
            pc.luck = pc.baseLuck + pc.race.luckMod + CalcAttributeModifierLuk(pc);
            pc.damageTypeResistanceTotalAcid = pc.race.damageTypeResistanceValueAcid + CalcAcidModifiers(pc);
            if (pc.damageTypeResistanceTotalAcid > 100) { pc.damageTypeResistanceTotalAcid = 100; }
            pc.damageTypeResistanceTotalNormal = pc.race.damageTypeResistanceValueNormal + CalcNormalModifiers(pc);
            if (pc.damageTypeResistanceTotalNormal > 100) { pc.damageTypeResistanceTotalNormal = 100; }
            pc.damageTypeResistanceTotalCold = pc.race.damageTypeResistanceValueCold + CalcColdModifiers(pc);
            if (pc.damageTypeResistanceTotalCold > 100) { pc.damageTypeResistanceTotalCold = 100; }
            pc.damageTypeResistanceTotalElectricity = pc.race.damageTypeResistanceValueElectricity + CalcElectricityModifiers(pc);
            if (pc.damageTypeResistanceTotalElectricity > 100) { pc.damageTypeResistanceTotalElectricity = 100; }
            pc.damageTypeResistanceTotalFire = pc.race.damageTypeResistanceValueFire + CalcFireModifiers(pc);
            if (pc.damageTypeResistanceTotalFire > 100) { pc.damageTypeResistanceTotalFire = 100; }
            pc.damageTypeResistanceTotalMagic = pc.race.damageTypeResistanceValueMagic + CalcMagicModifiers(pc);
            if (pc.damageTypeResistanceTotalMagic > 100) { pc.damageTypeResistanceTotalMagic = 100; }
            pc.damageTypeResistanceTotalPoison = pc.race.damageTypeResistanceValuePoison + CalcPoisonModifiers(pc);
            if (pc.damageTypeResistanceTotalPoison > 100) { pc.damageTypeResistanceTotalPoison = 100; }
            
            if (pc.playerClass.babTable.Length > 0)//SD_20131115
            {
                pc.baseAttBonus = pc.playerClass.babTable[pc.classLevel] + CalcBABAdders(pc); //SD_20131115
            }

            int modifierFromSPRelevantAttribute = 0;
            foreach (PlayerClass pClass in gv.cc.datafile.dataPlayerClassList)
            {
                if (pc.classTag.Equals(pClass.tag))
                {
                    if (pClass.modifierFromSPRelevantAttribute.Contains("intelligence"))
                    {
                        modifierFromSPRelevantAttribute = (pc.intelligence - 10) / 2;
                    }
                    if (pClass.modifierFromSPRelevantAttribute.Contains("wisdom"))
                    {
                        modifierFromSPRelevantAttribute = (pc.wisdom - 10) / 2;
                    }
                    if (pClass.modifierFromSPRelevantAttribute.Contains("charisma"))
                    {
                        modifierFromSPRelevantAttribute = (pc.charisma - 10) / 2;
                    }
                    if (pClass.modifierFromSPRelevantAttribute.Contains("constitution"))
                    {
                        modifierFromSPRelevantAttribute = (pc.constitution - 10) / 2;
                    }
                    if (pClass.modifierFromSPRelevantAttribute.Contains("strength"))
                    {
                        modifierFromSPRelevantAttribute = (pc.strength - 10) / 2;
                    }
                    if (pClass.modifierFromSPRelevantAttribute.Contains("dexterity"))
                    {
                        modifierFromSPRelevantAttribute = (pc.dexterity - 10) / 2;
                    }
                    break;
                }
            }

            int cMod = (pc.constitution - 10) / 2;
            int iMod = modifierFromSPRelevantAttribute;
            pc.spMax = pc.playerClass.startingSP + iMod + ((pc.classLevel - 1) * (pc.playerClass.spPerLevelUp + iMod));
            pc.hpMax = pc.playerClass.startingHP + cMod + ((pc.classLevel - 1) * (pc.playerClass.hpPerLevelUp + cMod));

            pc.XPNeeded = pc.playerClass.xpTable[pc.classLevel];

            int dMod = (pc.dexterity - 10) / 2;
            int maxDex = CalcMaxDexBonus(pc);
            if (dMod > maxDex) { dMod = maxDex; }
            int armBonus = 0;
            int acMods = 0;
            armBonus = CalcArmorBonuses(pc);
            acMods = CalcACModifiers(pc);
            pc.AC = pc.ACBase + dMod + armBonus + acMods;
            if (gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).ArmorWeightType.Equals("Light"))
            {
                pc.moveDistance = pc.race.MoveDistanceLightArmor + CalcMovementBonuses(pc);
            }
            else //medium or heavy SD_20131116
            {
                pc.moveDistance = pc.race.MoveDistanceMediumHeavyArmor + CalcMovementBonuses(pc);
            }
            RunAllItemWhileEquippedScripts(pc);
            if (pc.hp > pc.hpMax) { pc.hp = pc.hpMax; } //SD_20131201
            if (pc.sp > pc.spMax) { pc.sp = pc.spMax; } //SD_20131201
            if (pc.hp > 0)
            {
                pc.charStatus = "Alive";
            }
        }
        public void ReCalcSavingThrowBases(Player pc)
        {
            try
            {
                if (!pc.playerClass.name.Equals("newClass"))
                {
                    pc.baseFortitude = pc.playerClass.baseFortitudeAtLevel[pc.classLevel];
                    pc.baseReflex = pc.playerClass.baseReflexAtLevel[pc.classLevel];
                    pc.baseWill = pc.playerClass.baseWillAtLevel[pc.classLevel];
                }
            }
            catch { }
        }
        public int CalcSavingThrowModifiersReflex(Player pc)
        {
            int savBonuses = 0;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).savingThrowModifierReflex;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).savingThrowModifierReflex;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).savingThrowModifierReflex;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).savingThrowModifierReflex;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).savingThrowModifierReflex;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).savingThrowModifierReflex;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).savingThrowModifierReflex;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).savingThrowModifierReflex;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    savBonuses += ef.modifyReflex;
                }
                else
                {
                    if ((ef.modifyReflex != 0) && (ef.modifyReflex > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyReflex;
                    }
                }
            }
            if (highestNonStackable > -99) { savBonuses = highestNonStackable; }
            return savBonuses;
        }
        public int CalcSavingThrowModifiersFortitude(Player pc)
        {
            int savBonuses = 0;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).savingThrowModifierFortitude;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).savingThrowModifierFortitude;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).savingThrowModifierFortitude;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).savingThrowModifierFortitude;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).savingThrowModifierFortitude;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).savingThrowModifierFortitude;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).savingThrowModifierFortitude;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).savingThrowModifierFortitude;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    savBonuses += ef.modifyFortitude;
                }
                else
                {
                    if ((ef.modifyFortitude != 0) && (ef.modifyFortitude > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyFortitude;
                    }
                }
            }
            if (highestNonStackable > -99) { savBonuses = highestNonStackable; }
            return savBonuses;
        }
        public int CalcSavingThrowModifiersWill(Player pc)
        {
            int savBonuses = 0;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).savingThrowModifierWill;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).savingThrowModifierWill;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).savingThrowModifierWill;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).savingThrowModifierWill;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).savingThrowModifierWill;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).savingThrowModifierWill;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).savingThrowModifierWill;
            savBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).savingThrowModifierWill;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    savBonuses += ef.modifyWill;
                }
                else
                {
                    if ((ef.modifyWill != 0) && (ef.modifyWill > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyWill;
                    }
                }
            }
            if (highestNonStackable > -99) { savBonuses = highestNonStackable; }
            return savBonuses;
        }
        public int CalcAttributeModifierStr(Player pc)
        {
            int attBonuses = 0;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).attributeBonusModifierStr;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).attributeBonusModifierStr;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).attributeBonusModifierStr;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).attributeBonusModifierStr;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).attributeBonusModifierStr;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).attributeBonusModifierStr;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).attributeBonusModifierStr;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).attributeBonusModifierStr;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    attBonuses += ef.modifyStr;
                }
                else
                {
                    if ((ef.modifyStr != 0) && (ef.modifyStr > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyStr;
                    }
                }
            }
            if (highestNonStackable > -99) { attBonuses = highestNonStackable; }
            return attBonuses;
        }
        public int CalcAttributeModifierDex(Player pc)
        {
            int attBonuses = 0;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).attributeBonusModifierDex;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).attributeBonusModifierDex;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).attributeBonusModifierDex;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).attributeBonusModifierDex;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).attributeBonusModifierDex;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).attributeBonusModifierDex;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).attributeBonusModifierDex;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).attributeBonusModifierDex;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    attBonuses += ef.modifyDex;
                }
                else
                {
                    if ((ef.modifyDex != 0) && (ef.modifyDex > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyDex;
                    }
                }
            }
            if (highestNonStackable > -99) { attBonuses = highestNonStackable; }
            return attBonuses;
        }
        public int CalcAttributeModifierInt(Player pc)
        {
            int attBonuses = 0;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).attributeBonusModifierInt;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).attributeBonusModifierInt;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).attributeBonusModifierInt;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).attributeBonusModifierInt;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).attributeBonusModifierInt;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).attributeBonusModifierInt;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).attributeBonusModifierInt;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).attributeBonusModifierInt;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    attBonuses += ef.modifyInt;
                }
                else
                {
                    if ((ef.modifyInt != 0) && (ef.modifyInt > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyInt;
                    }
                }
            }
            if (highestNonStackable > -99) { attBonuses = highestNonStackable; }
            return attBonuses;
        }
        public int CalcAttributeModifierCha(Player pc)
        {
            int attBonuses = 0;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).attributeBonusModifierCha;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).attributeBonusModifierCha;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).attributeBonusModifierCha;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).attributeBonusModifierCha;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).attributeBonusModifierCha;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).attributeBonusModifierCha;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).attributeBonusModifierCha;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).attributeBonusModifierCha;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    attBonuses += ef.modifyCha;
                }
                else
                {
                    if ((ef.modifyCha != 0) && (ef.modifyCha > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyCha;
                    }
                }
            }
            if (highestNonStackable > -99) { attBonuses = highestNonStackable; }
            return attBonuses;
        }
        public int CalcAttributeModifierCon(Player pc)
        {
            int attBonuses = 0;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).attributeBonusModifierCon;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).attributeBonusModifierCon;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).attributeBonusModifierCon;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).attributeBonusModifierCon;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).attributeBonusModifierCon;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).attributeBonusModifierCon;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).attributeBonusModifierCon;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).attributeBonusModifierCon;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    attBonuses += ef.modifyCon;
                }
                else
                {
                    if ((ef.modifyCon != 0) && (ef.modifyCon > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyCon;
                    }
                }
            }
            if (highestNonStackable > -99) { attBonuses = highestNonStackable; }
            return attBonuses;
        }
        public int CalcAttributeModifierWis(Player pc)
        {
            int attBonuses = 0;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).attributeBonusModifierWis;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).attributeBonusModifierWis;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).attributeBonusModifierWis;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).attributeBonusModifierWis;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).attributeBonusModifierWis;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).attributeBonusModifierWis;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).attributeBonusModifierWis;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).attributeBonusModifierWis;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    attBonuses += ef.modifyWis;
                }
                else
                {
                    if ((ef.modifyWis != 0) && (ef.modifyWis > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyWis;
                    }
                }
            }
            if (highestNonStackable > -99) { attBonuses = highestNonStackable; }
            return attBonuses;
        }
        public int CalcAttributeModifierLuk(Player pc)
        {
            int attBonuses = 0;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).attributeBonusModifierLuk;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).attributeBonusModifierLuk;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).attributeBonusModifierLuk;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).attributeBonusModifierLuk;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).attributeBonusModifierLuk;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).attributeBonusModifierLuk;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).attributeBonusModifierLuk;
            attBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).attributeBonusModifierLuk;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    attBonuses += ef.modifyLuk;
                }
                else
                {
                    if ((ef.modifyLuk != 0) && (ef.modifyLuk > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyLuk;
                    }
                }
            }
            if (highestNonStackable > -99) { attBonuses = highestNonStackable; }
            return attBonuses;
        }
        public int CalcAcidModifiers(Player pc)
        {
            int md = 0;
            md += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).damageTypeResistanceValueAcid;
            md += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).damageTypeResistanceValueAcid;
            md += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).damageTypeResistanceValueAcid;
            md += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).damageTypeResistanceValueAcid;
            md += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).damageTypeResistanceValueAcid;
            md += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).damageTypeResistanceValueAcid;
            md += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).damageTypeResistanceValueAcid;
            md += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).damageTypeResistanceValueAcid;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    md += ef.modifyDamageTypeResistanceAcid;
                }
                else
                {
                    if ((ef.modifyDamageTypeResistanceAcid != 0) && (ef.modifyDamageTypeResistanceAcid > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyDamageTypeResistanceAcid;
                    }
                }
            }
            if (highestNonStackable > -99) { md = highestNonStackable; }
            return md;
        }
        public int CalcNormalModifiers(Player pc)
        {
            int md = 0;
            md += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).damageTypeResistanceValueNormal;
            md += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).damageTypeResistanceValueNormal;
            md += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).damageTypeResistanceValueNormal;
            md += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).damageTypeResistanceValueNormal;
            md += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).damageTypeResistanceValueNormal;
            md += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).damageTypeResistanceValueNormal;
            md += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).damageTypeResistanceValueNormal;
            md += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).damageTypeResistanceValueNormal;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    md += ef.modifyDamageTypeResistanceNormal;
                }
                else
                {
                    if ((ef.modifyDamageTypeResistanceNormal != 0) && (ef.modifyDamageTypeResistanceNormal > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyDamageTypeResistanceNormal;
                    }
                }
            }
            if (highestNonStackable > -99) { md = highestNonStackable; }
            return md;
        }
        public int CalcColdModifiers(Player pc)
        {
            int md = 0;
            md += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).damageTypeResistanceValueCold;
            md += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).damageTypeResistanceValueCold;
            md += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).damageTypeResistanceValueCold;
            md += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).damageTypeResistanceValueCold;
            md += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).damageTypeResistanceValueCold;
            md += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).damageTypeResistanceValueCold;
            md += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).damageTypeResistanceValueCold;
            md += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).damageTypeResistanceValueCold;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    md += ef.modifyDamageTypeResistanceCold;
                }
                else
                {
                    if ((ef.modifyDamageTypeResistanceCold != 0) && (ef.modifyDamageTypeResistanceCold > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyDamageTypeResistanceCold;
                    }
                }
            }
            if (highestNonStackable > -99) { md = highestNonStackable; }
            return md;
        }
        public int CalcElectricityModifiers(Player pc)
        {
            int md = 0;
            md += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).damageTypeResistanceValueElectricity;
            md += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).damageTypeResistanceValueElectricity;
            md += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).damageTypeResistanceValueElectricity;
            md += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).damageTypeResistanceValueElectricity;
            md += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).damageTypeResistanceValueElectricity;
            md += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).damageTypeResistanceValueElectricity;
            md += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).damageTypeResistanceValueElectricity;
            md += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).damageTypeResistanceValueElectricity;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    md += ef.modifyDamageTypeResistanceElectricity;
                }
                else
                {
                    if ((ef.modifyDamageTypeResistanceElectricity != 0) && (ef.modifyDamageTypeResistanceElectricity > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyDamageTypeResistanceElectricity;
                    }
                }
            }
            if (highestNonStackable > -99) { md = highestNonStackable; }
            return md;
        }
        public int CalcFireModifiers(Player pc)
        {
            int md = 0;
            md += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).damageTypeResistanceValueFire;
            md += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).damageTypeResistanceValueFire;
            md += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).damageTypeResistanceValueFire;
            md += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).damageTypeResistanceValueFire;
            md += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).damageTypeResistanceValueFire;
            md += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).damageTypeResistanceValueFire;
            md += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).damageTypeResistanceValueFire;
            md += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).damageTypeResistanceValueFire;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    md += ef.modifyDamageTypeResistanceFire;
                }
                else
                {
                    if ((ef.modifyDamageTypeResistanceFire != 0) && (ef.modifyDamageTypeResistanceFire > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyDamageTypeResistanceFire;
                    }
                }
            }
            if (highestNonStackable > -99) { md = highestNonStackable; }
            return md;
        }
        public int CalcMagicModifiers(Player pc)
        {
            int md = 0;
            md += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).damageTypeResistanceValueMagic;
            md += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).damageTypeResistanceValueMagic;
            md += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).damageTypeResistanceValueMagic;
            md += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).damageTypeResistanceValueMagic;
            md += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).damageTypeResistanceValueMagic;
            md += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).damageTypeResistanceValueMagic;
            md += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).damageTypeResistanceValueMagic;
            md += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).damageTypeResistanceValueMagic;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    md += ef.modifyDamageTypeResistanceMagic;
                }
                else
                {
                    if ((ef.modifyDamageTypeResistanceMagic != 0) && (ef.modifyDamageTypeResistanceMagic > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyDamageTypeResistanceMagic;
                    }
                }
            }
            if (highestNonStackable > -99) { md = highestNonStackable; }
            return md;
        }
        public int CalcPoisonModifiers(Player pc)
        {
            int md = 0;
            md += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).damageTypeResistanceValuePoison;
            md += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).damageTypeResistanceValuePoison;
            md += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).damageTypeResistanceValuePoison;
            md += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).damageTypeResistanceValuePoison;
            md += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).damageTypeResistanceValuePoison;
            md += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).damageTypeResistanceValuePoison;
            md += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).damageTypeResistanceValuePoison;
            md += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).damageTypeResistanceValuePoison;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    md += ef.modifyDamageTypeResistancePoison;
                }
                else
                {
                    if ((ef.modifyDamageTypeResistancePoison != 0) && (ef.modifyDamageTypeResistancePoison > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyDamageTypeResistancePoison;
                    }
                }
            }
            if (highestNonStackable > -99) { md = highestNonStackable; }
            return md;
        }
        public int CalcBABAdders(Player pc)
        {            
            int adder = 0;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    adder += ef.babModifier;
                }
                else
                {
                    if ((ef.babModifier != 0) && (ef.babModifier > highestNonStackable))
                    {
                        highestNonStackable = ef.babModifier;
                    }
                }
            }
            if (highestNonStackable > -99) { adder = highestNonStackable; }
            return adder;
        }
        public int CalcACModifiers(Player pc)
        {
            int adder = 0;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    adder += ef.acModifier;
                }
                else
                {
                    if ((ef.acModifier != 0) && (ef.acModifier > highestNonStackable))
                    {
                        highestNonStackable = ef.acModifier;
                    }
                }
            }
            if (highestNonStackable > -99) { adder = highestNonStackable; }
            return adder;
        }
        public int CalcArmorBonuses(Player pc)
        {
            int armBonuses = 0;
            armBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).armorBonus;
            armBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).armorBonus;
            armBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).armorBonus;
            armBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).armorBonus;
            armBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).armorBonus;
            armBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).armorBonus;
            armBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).armorBonus;
            armBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).armorBonus;
            return armBonuses;
        }
        public int CalcMaxDexBonus(Player pc)
        {
            int armMaxDexBonuses = 99;
            int mdb = gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).maxDexBonus;
            if (mdb < armMaxDexBonuses) { armMaxDexBonuses = mdb; }
            mdb = gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).maxDexBonus;
            if (mdb < armMaxDexBonuses) { armMaxDexBonuses = mdb; }
            mdb = gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).maxDexBonus;
            if (mdb < armMaxDexBonuses) { armMaxDexBonuses = mdb; }
            mdb = gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).maxDexBonus;
            if (mdb < armMaxDexBonuses) { armMaxDexBonuses = mdb; }
            mdb = gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).maxDexBonus;
            if (mdb < armMaxDexBonuses) { armMaxDexBonuses = mdb; }
            mdb = gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).maxDexBonus;
            if (mdb < armMaxDexBonuses) { armMaxDexBonuses = mdb; }
            mdb = gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).maxDexBonus;
            if (mdb < armMaxDexBonuses) { armMaxDexBonuses = mdb; }
            mdb = gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).maxDexBonus;
            if (mdb < armMaxDexBonuses) { armMaxDexBonuses = mdb; }
            return armMaxDexBonuses;
        }
        public int CalcMovementBonuses(Player pc)
        {
            int moveBonuses = 0;
            moveBonuses += gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).MovementPointModifier;
            moveBonuses += gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).MovementPointModifier;
            moveBonuses += gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).MovementPointModifier;
            moveBonuses += gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).MovementPointModifier;
            moveBonuses += gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).MovementPointModifier;
            moveBonuses += gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).MovementPointModifier;
            moveBonuses += gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).MovementPointModifier;
            moveBonuses += gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).MovementPointModifier;
            int highestNonStackable = -99;
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.isStackableEffect)
                {
                    moveBonuses += ef.modifyMoveDistance;
                }
                else
                {
                    if ((ef.modifyMoveDistance != 0) && (ef.modifyMoveDistance > highestNonStackable))
                    {
                        highestNonStackable = ef.modifyMoveDistance;
                    }
                }
            }
            if (highestNonStackable > -99) { moveBonuses = highestNonStackable; }
            return moveBonuses;
        }
        public void RunAllItemWhileEquippedScripts(Player pc)
        {
            try
            {
                /*if (!gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).onWhileEquipped.Equals("none"))
                {
                    gv.cc.doScriptBasedOnFilename(gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).onWhileEquipped, "", "", "", "");
                }
                if (!gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).onWhileEquipped.Equals("none"))
                {
                    gv.cc.doScriptBasedOnFilename(gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).onWhileEquipped, "", "", "", "");
                }
                if (!gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).onWhileEquipped.Equals("none"))
                {
                    gv.cc.doScriptBasedOnFilename(gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).onWhileEquipped, "", "", "", "");
                }
                if (!gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).onWhileEquipped.Equals("none"))
                {
                    gv.cc.doScriptBasedOnFilename(gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).onWhileEquipped, "", "", "", "");
                }
                if (!gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).onWhileEquipped.Equals("none"))
                {
                    gv.cc.doScriptBasedOnFilename(gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).onWhileEquipped, "", "", "", "");
                }
                if (!gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).onWhileEquipped.Equals("none"))
                {
                    gv.cc.doScriptBasedOnFilename(gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).onWhileEquipped, "", "", "", "");
                }
                if (!gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).onWhileEquipped.Equals("none"))
                {
                    gv.cc.doScriptBasedOnFilename(gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).onWhileEquipped, "", "", "", "");
                }
                if (!gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).onWhileEquipped.Equals("none"))
                {
                    gv.cc.doScriptBasedOnFilename(gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).onWhileEquipped, "", "", "", "");
                }
                if (!gv.cc.getItemByResRefForInfo(pc.AmmoRefs.resref).onWhileEquipped.Equals("none"))
                {
                    gv.cc.doScriptBasedOnFilename(gv.cc.getItemByResRefForInfo(pc.AmmoRefs.resref).onWhileEquipped, "", "", "", "");
                }*/
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
            }
        }
        public bool hasTrait(Player pc, string tag)
        {
            return pc.knownTraitsTags.Contains(tag);
        }
        public int CalcNumberOfAttacks(Player pc)
        {
            if (isMeleeAttack(pc))
            {
                return CalcNumberOfMeleeAttacks(pc);
            }
            else
            {
                return CalcNumberOfRangedAttacks(pc);
            }
        }
        public int CalcNumberOfMeleeAttacks(Player pc)
        {
            int numOfAdditionalPositiveMeleeAttacks = 0;
            int numOfAdditionalPositiveStackableMeleeAttacks = 0;
            int numOfAdditionalNegativeMeleeAttacks = 0;
            int numOfAdditionalNegativeStackableMeleeAttacks = 0;
            //go through all traits and see if has passive rapidshot type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    //replace non-stackable positive with highest value
                    if ((ef.modifyNumberOfMeleeAttacks > numOfAdditionalPositiveMeleeAttacks) && (ta.isPassive) && (!ef.isStackableEffect))
                    {
                        numOfAdditionalPositiveMeleeAttacks = ef.modifyNumberOfMeleeAttacks;
                    }
                    //replace non-stackable negative with lowest value
                    if ((ef.modifyNumberOfMeleeAttacks < numOfAdditionalNegativeMeleeAttacks) && (ta.isPassive) && (!ef.isStackableEffect))
                    {
                        numOfAdditionalNegativeMeleeAttacks = ef.modifyNumberOfMeleeAttacks;
                    }
                    //if isStackable positive then pile on
                    if ((ef.modifyNumberOfMeleeAttacks > 0) && (ta.isPassive) && (ef.isStackableEffect))
                    {
                        numOfAdditionalPositiveStackableMeleeAttacks += ef.modifyNumberOfMeleeAttacks;
                    }
                    //if isStackable negative then pile on
                    if ((ef.modifyNumberOfMeleeAttacks < 0) && (ta.isPassive) && (ef.isStackableEffect))
                    {
                        numOfAdditionalNegativeStackableMeleeAttacks += ef.modifyNumberOfMeleeAttacks;
                    }
                }
            }
            //go through each effect and see if has a buff type like rapidshot, use largest, not cumulative
            foreach (Effect ef in pc.effectsList)
            {
                //replace non-stackable positive with highest value
                if ((ef.modifyNumberOfMeleeAttacks > numOfAdditionalPositiveMeleeAttacks) && (!ef.isStackableEffect))
                {
                    numOfAdditionalPositiveMeleeAttacks = ef.modifyNumberOfMeleeAttacks;
                }
                //replace non-stackable negative with lowest value
                if ((ef.modifyNumberOfMeleeAttacks < numOfAdditionalNegativeMeleeAttacks) && (!ef.isStackableEffect))
                {
                    numOfAdditionalNegativeMeleeAttacks = ef.modifyNumberOfMeleeAttacks;
                }
                //if isStackable positive then pile on
                if ((ef.modifyNumberOfMeleeAttacks > 0) && (ef.isStackableEffect))
                {
                    numOfAdditionalPositiveStackableMeleeAttacks += ef.modifyNumberOfMeleeAttacks;
                }
                //if isStackable negative then pile on
                if ((ef.modifyNumberOfMeleeAttacks < 0) && (ef.isStackableEffect))
                {
                    numOfAdditionalNegativeStackableMeleeAttacks += ef.modifyNumberOfMeleeAttacks;
                }
            }

            int numOfPos = 0;
            int numOfNeg = 0;
            //check to see if stackable is greater than non-stackable and combine the highest positive and negative effect
            if (numOfAdditionalPositiveMeleeAttacks > numOfAdditionalPositiveStackableMeleeAttacks)
            {
                numOfPos = numOfAdditionalPositiveMeleeAttacks;
            }
            else
            {
                numOfPos = numOfAdditionalPositiveStackableMeleeAttacks;
            }
            if (numOfAdditionalNegativeMeleeAttacks < numOfAdditionalNegativeStackableMeleeAttacks)
            {
                numOfNeg = numOfAdditionalNegativeMeleeAttacks;
            }
            else
            {
                numOfNeg = numOfAdditionalNegativeStackableMeleeAttacks;
            }

            int numOfAdditionalAttacks = numOfPos + numOfNeg;
            if (numOfAdditionalAttacks != 0)
            {
                return numOfAdditionalAttacks + 1;
            }
            else if (gv.sf.hasTrait(pc, "twoAttack"))
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        public int CalcNumberOfRangedAttacks(Player pc)
        {
            int numOfAdditionalPositiveRangedAttacks = 0;
            int numOfAdditionalPositiveStackableRangedAttacks = 0;
            int numOfAdditionalNegativeRangedAttacks = 0;
            int numOfAdditionalNegativeStackableRangedAttacks = 0;
            //go through all traits and see if has passive rapidshot type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    //replace non-stackable positive with highest value
                    if ((ef.modifyNumberOfRangedAttacks > numOfAdditionalPositiveRangedAttacks) && (ta.isPassive) && (!ef.isStackableEffect))
                    {
                        numOfAdditionalPositiveRangedAttacks = ef.modifyNumberOfRangedAttacks;
                    }
                    //replace non-stackable negative with lowest value
                    if ((ef.modifyNumberOfRangedAttacks < numOfAdditionalNegativeRangedAttacks) && (ta.isPassive) && (!ef.isStackableEffect))
                    {
                        numOfAdditionalNegativeRangedAttacks = ef.modifyNumberOfRangedAttacks;
                    }
                    //if isStackable positive then pile on
                    if ((ef.modifyNumberOfRangedAttacks > 0) && (ta.isPassive) && (ef.isStackableEffect))
                    {
                        numOfAdditionalPositiveStackableRangedAttacks += ef.modifyNumberOfRangedAttacks;
                    }
                    //if isStackable negative then pile on
                    if ((ef.modifyNumberOfRangedAttacks < 0) && (ta.isPassive) && (ef.isStackableEffect))
                    {
                        numOfAdditionalNegativeStackableRangedAttacks += ef.modifyNumberOfRangedAttacks;
                    }
                }
            }
            //go through each effect and see if has a buff type like rapidshot, use largest, not cumulative
            foreach (Effect ef in pc.effectsList)
            {
                //replace non-stackable positive with highest value
                if ((ef.modifyNumberOfRangedAttacks > numOfAdditionalPositiveRangedAttacks) && (!ef.isStackableEffect))
                {
                    numOfAdditionalPositiveRangedAttacks = ef.modifyNumberOfRangedAttacks;
                }
                //replace non-stackable negative with lowest value
                if ((ef.modifyNumberOfRangedAttacks < numOfAdditionalNegativeRangedAttacks) && (!ef.isStackableEffect))
                {
                    numOfAdditionalNegativeRangedAttacks = ef.modifyNumberOfRangedAttacks;
                }
                //if isStackable positive then pile on
                if ((ef.modifyNumberOfRangedAttacks > 0) && (ef.isStackableEffect))
                {
                    numOfAdditionalPositiveStackableRangedAttacks += ef.modifyNumberOfRangedAttacks;
                }
                //if isStackable negative then pile on
                if ((ef.modifyNumberOfRangedAttacks < 0) && (ef.isStackableEffect))
                {
                    numOfAdditionalNegativeStackableRangedAttacks += ef.modifyNumberOfRangedAttacks;
                }
            }

            int numOfPos = 0;
            int numOfNeg = 0;
            //check to see if stackable is greater than non-stackable and combine the highest positive and negative effect
            if (numOfAdditionalPositiveRangedAttacks > numOfAdditionalPositiveStackableRangedAttacks)
            {
                numOfPos = numOfAdditionalPositiveRangedAttacks;
            }
            else
            {
                numOfPos = numOfAdditionalPositiveStackableRangedAttacks;
            }
            if (numOfAdditionalNegativeRangedAttacks < numOfAdditionalNegativeStackableRangedAttacks)
            {
                numOfNeg = numOfAdditionalNegativeRangedAttacks;
            }
            else
            {
                numOfNeg = numOfAdditionalNegativeStackableRangedAttacks;
            }

            int numOfAdditionalAttacks = numOfPos + numOfNeg;
            if (numOfAdditionalAttacks != 0)
            {
                return numOfAdditionalAttacks + 1;
            }
            else if (gv.sf.hasTrait(pc, "rapidshot2"))
            {
                return 3;
            }
            else if (gv.sf.hasTrait(pc, "rapidshot"))
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        public bool isMeleeAttack(Player pc)
        {
            if ((gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Melee"))
                    || (gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).name.Equals("none"))
                    || (gv.cc.getItemByResRefForInfo(pc.AmmoRefs.resref).name.Equals("none")))
            {
                return true;
            }
            return false;
        }
        public int CalcNumberOfCleaveAttackTargets(Player pc)
        {
            int cleaveAttTargets = 0;
            //go through all traits and see if has passive cleave type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    if ((ef.modifyNumberOfEnemiesAttackedOnCleave > cleaveAttTargets) && (ta.isPassive))
                    {
                        cleaveAttTargets = ef.modifyNumberOfEnemiesAttackedOnCleave;
                    }
                }
            }
            //go through each effect and see if has a buff type like cleave, use largest, not cumulative
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.modifyNumberOfEnemiesAttackedOnCleave > cleaveAttTargets)
                {
                    cleaveAttTargets = ef.modifyNumberOfEnemiesAttackedOnCleave;
                }
            }
            if (cleaveAttTargets > 0)
            {
                return cleaveAttTargets;
            }
            else if (gv.sf.hasTrait(pc, "cleave"))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public int CalcNumberOfSweepAttackTargets(Player pc)
        {
            int sweepAttTargets = 0;
            //go through all traits and see if has passive sweep type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    if ((ef.modifyNumberOfEnemiesAttackedOnSweepAttack > sweepAttTargets) && (ta.isPassive))
                    {
                        sweepAttTargets = ef.modifyNumberOfEnemiesAttackedOnSweepAttack;
                    }
                }
            }
            //go through each effect and see if has a buff/active type like sweep, use largest, not cumulative
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.modifyNumberOfEnemiesAttackedOnSweepAttack > sweepAttTargets)
                {
                    sweepAttTargets = ef.modifyNumberOfEnemiesAttackedOnSweepAttack;
                }
            }
            if (sweepAttTargets > 0)
            {
                return sweepAttTargets;
            }            
            else
            {
                return 0;
            }
        }
        public int CalcPcMeleeAttackAttributeModifier(Player pc)
        {
            int modifier = (pc.strength - 10) / 2;
            bool useDexModifier = false;
            //go through all traits and see if has passive criticalstrike type trait
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    if ((ef.useDexterityForMeleeAttackModifierIfGreaterThanStrength) && (ta.isPassive))
                    {
                        useDexModifier = true;
                    }
                }
            }
            //go through each effect and see if has a buff type like criticalstrike
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.useDexterityForMeleeAttackModifierIfGreaterThanStrength)
                {
                    useDexModifier = true;
                }
            }
            //if has critical strike trait use dexterity for attack modifier in melee if greater than strength modifier
            if ((pc.knownTraitsTags.Contains("criticalstrike")) || (useDexModifier))
            {
                int modifierDex = (pc.dexterity - 10) / 2;
                if (modifierDex > modifier)
                {
                    modifier = (pc.dexterity - 10) / 2;
                }
            }
            return modifier;
        }
        public int CalcPcMeleeDamageAttributeModifier(Player pc)
        {
            int damModifier = (pc.strength - 10) / 2;
            bool useDexModifier = false;
            //go through all traits and see if has passive criticalstrike type trait
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    if ((ef.useDexterityForMeleeDamageModifierIfGreaterThanStrength) && (ta.isPassive))
                    {
                        useDexModifier = true;
                    }
                }
            }
            //go through each effect and see if has a buff type like criticalstrike
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.useDexterityForMeleeDamageModifierIfGreaterThanStrength)
                {
                    useDexModifier = true;
                }
            }
            //if has critical strike trait use dexterity for damage modifier in melee if greater than strength modifier
            if ((pc.knownTraitsTags.Contains("criticalstrike")) || (useDexModifier))
            {
                int damModifierDex = (pc.dexterity - 10) / 4;
                if (damModifierDex > damModifier)
                {
                    damModifier = damModifierDex;
                }
            }
            return damModifier;
        }
        public bool canNegateAdjacentAttackPenalty(Player pc)
        {
            bool cancelAttackPenalty = false;
            //go through all traits and see if has passive pointblankshot type trait
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    if ((ef.negateAttackPenaltyForAdjacentEnemyWithRangedAttack) && (ta.isPassive))
                    {
                        cancelAttackPenalty = true;
                    }
                }
            }
            //go through each effect and see if has a buff type like pointblankshot
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.negateAttackPenaltyForAdjacentEnemyWithRangedAttack)
                {
                    cancelAttackPenalty = true;
                }
            }
            if ((gv.sf.hasTrait(pc, "pointblankshot")) || (cancelAttackPenalty))
            {
                return true;
            }
            return false;
        }
        public int CalcPcRangedAttackModifier(Player pc)
        {
            int preciseShotAdder = 0;
            string label = "";
            //go through all traits and see if has passive preciseshot type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    if ((ef.babModifierForRangedAttack > preciseShotAdder) && (ta.isPassive))
                    {
                        preciseShotAdder = ef.babModifierForRangedAttack;
                        label = ta.name;
                    }
                }
            }
            //go through each effect and see if has a buff type like preciseshot, use largest, not cumulative
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.babModifierForRangedAttack > preciseShotAdder)
                {
                    preciseShotAdder = ef.babModifierForRangedAttack;
                    label = ef.name;
                }
            }
            return preciseShotAdder;
        }
        public int CalcPcMeleeDamageModifier(Player pc)
        {
            int adder = 0;
            //go through all traits and see if has passive preciseshot type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    if ((ef.damageModifierForMeleeAttack > adder) && (ta.isPassive))
                    {
                        adder = ef.damageModifierForMeleeAttack;
                    }
                }
            }
            //go through each effect and see if has a buff type like preciseshot, use largest, not cumulative
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.damageModifierForMeleeAttack > adder)
                {
                    adder = ef.damageModifierForMeleeAttack;
                }
            }
            return adder;
        }
        public int CalcPcRangedDamageModifier(Player pc)
        {
            int preciseShotAdder = 0;
            //go through all traits and see if has passive preciseshot type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    if ((ef.damageModifierForRangedAttack > preciseShotAdder) && (ta.isPassive))
                    {
                        preciseShotAdder = ef.damageModifierForRangedAttack;
                    }
                }
            }
            //go through each effect and see if has a buff type like preciseshot, use largest, not cumulative
            foreach (Effect ef in pc.effectsList)
            {
                if (ef.damageModifierForRangedAttack > preciseShotAdder)
                {
                    preciseShotAdder = ef.damageModifierForRangedAttack;
                }
            }
            return preciseShotAdder;
        }
        public int CalcPcHpRegenInCombat(Player pc)
        {
            int adder = 0;
            //go through all traits and see if has passive HP regen type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    if ((ef.modifyHpInCombat > adder) && (ta.isPassive))
                    {
                        adder = ef.modifyHpInCombat;
                    }
                }
            }
            return adder;
        }
        public int CalcPcSpRegenInCombat(Player pc)
        {
            int adder = 0;
            //go through all traits and see if has passive HP regen type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    if ((ef.modifySpInCombat > adder) && (ta.isPassive))
                    {
                        adder = ef.modifySpInCombat;
                    }
                }
            }
            return adder;
        }
        public int CalcShopBuyBackModifier(Player pc)
        {
            int positiveMod = 0;
            int positiveStackableMod = 0;
            int negativeMod = 0;
            int negativeStackableMod = 0;
            //go through all traits and see if has passive rapidshot type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    //replace non-stackable positive with highest value
                    if ((ef.modifyShopBuyBackPrice > positiveMod) && (ta.isPassive) && (!ef.isStackableEffect))
                    {
                        positiveMod = ef.modifyShopBuyBackPrice;
                    }
                    //replace non-stackable negative with lowest value
                    if ((ef.modifyShopBuyBackPrice < negativeMod) && (ta.isPassive) && (!ef.isStackableEffect))
                    {
                        negativeMod = ef.modifyShopBuyBackPrice;
                    }
                    //if isStackable positive then pile on
                    if ((ef.modifyShopBuyBackPrice > 0) && (ta.isPassive) && (ef.isStackableEffect))
                    {
                        positiveStackableMod += ef.modifyShopBuyBackPrice;
                    }
                    //if isStackable negative then pile on
                    if ((ef.modifyShopBuyBackPrice < 0) && (ta.isPassive) && (ef.isStackableEffect))
                    {
                        negativeStackableMod += ef.modifyShopBuyBackPrice;
                    }
                }
            }
            //go through each effect and see if has a buff type like rapidshot, use largest, not cumulative
            foreach (Effect ef in pc.effectsList)
            {
                //replace non-stackable positive with highest value
                if ((ef.modifyShopBuyBackPrice > positiveMod) && (!ef.isStackableEffect))
                {
                    positiveMod = ef.modifyShopBuyBackPrice;
                }
                //replace non-stackable negative with lowest value
                if ((ef.modifyShopBuyBackPrice < negativeMod) && (!ef.isStackableEffect))
                {
                    negativeMod = ef.modifyShopBuyBackPrice;
                }
                //if isStackable positive then pile on
                if ((ef.modifyShopBuyBackPrice > 0) && (ef.isStackableEffect))
                {
                    positiveStackableMod += ef.modifyShopBuyBackPrice;
                }
                //if isStackable negative then pile on
                if ((ef.modifyShopBuyBackPrice < 0) && (ef.isStackableEffect))
                {
                    negativeStackableMod += ef.modifyShopBuyBackPrice;
                }
            }

            int numOfPos = 0;
            int numOfNeg = 0;
            //check to see if stackable is greater than non-stackable and combine the highest positive and negative effect
            if (positiveMod > positiveStackableMod)
            {
                numOfPos = positiveMod;
            }
            else
            {
                numOfPos = positiveStackableMod;
            }
            if (negativeMod < negativeStackableMod)
            {
                numOfNeg = negativeMod;
            }
            else
            {
                numOfNeg = negativeStackableMod;
            }

            return numOfPos + numOfNeg;
        }
        public int CalcShopSellModifier(Player pc)
        {
            int positiveMod = 0;
            int positiveStackableMod = 0;
            int negativeMod = 0;
            int negativeStackableMod = 0;
            //go through all traits and see if has passive rapidshot type trait, use largest, not cumulative
            foreach (string taTag in pc.knownTraitsTags)
            {
                Trait ta = gv.cc.getTraitByTag(taTag);
                foreach (EffectTagForDropDownList efTag in ta.traitEffectTagList)
                {
                    Effect ef = gv.cc.getEffectByTag(efTag.tag);
                    //replace non-stackable positive with highest value
                    if ((ef.modifyShopSellPrice > positiveMod) && (ta.isPassive) && (!ef.isStackableEffect))
                    {
                        positiveMod = ef.modifyShopSellPrice;
                    }
                    //replace non-stackable negative with lowest value
                    if ((ef.modifyShopSellPrice < negativeMod) && (ta.isPassive) && (!ef.isStackableEffect))
                    {
                        negativeMod = ef.modifyShopSellPrice;
                    }
                    //if isStackable positive then pile on
                    if ((ef.modifyShopSellPrice > 0) && (ta.isPassive) && (ef.isStackableEffect))
                    {
                        positiveStackableMod += ef.modifyShopSellPrice;
                    }
                    //if isStackable negative then pile on
                    if ((ef.modifyShopSellPrice < 0) && (ta.isPassive) && (ef.isStackableEffect))
                    {
                        negativeStackableMod += ef.modifyShopSellPrice;
                    }
                }
            }
            //go through each effect and see if has a buff type like rapidshot, use largest, not cumulative
            foreach (Effect ef in pc.effectsList)
            {
                //replace non-stackable positive with highest value
                if ((ef.modifyShopSellPrice > positiveMod) && (!ef.isStackableEffect))
                {
                    positiveMod = ef.modifyShopSellPrice;
                }
                //replace non-stackable negative with lowest value
                if ((ef.modifyShopSellPrice < negativeMod) && (!ef.isStackableEffect))
                {
                    negativeMod = ef.modifyShopSellPrice;
                }
                //if isStackable positive then pile on
                if ((ef.modifyShopSellPrice > 0) && (ef.isStackableEffect))
                {
                    positiveStackableMod += ef.modifyShopSellPrice;
                }
                //if isStackable negative then pile on
                if ((ef.modifyShopSellPrice < 0) && (ef.isStackableEffect))
                {
                    negativeStackableMod += ef.modifyShopSellPrice;
                }
            }

            int numOfPos = 0;
            int numOfNeg = 0;
            //check to see if stackable is greater than non-stackable and combine the highest positive and negative effect
            if (positiveMod > positiveStackableMod)
            {
                numOfPos = positiveMod;
            }
            else
            {
                numOfPos = positiveStackableMod;
            }
            if (negativeMod < negativeStackableMod)
            {
                numOfNeg = negativeMod;
            }
            else
            {
                numOfNeg = negativeStackableMod;
            }

            return numOfPos + numOfNeg;
        }

        //DEFAULT SCRIPTS
        public void dsWorldTime()
        {
            mod.WorldTime += mod.currentArea.TimePerSquare;
            //Code: Bleed to death at -20 hp
            spCnt++;
            foreach (Player pc in mod.playerList)
            {
                //check to see if allow HP to regen
                if (gv.cc.getPlayerClass(pc.classTag).hpRegenTimeNeeded > 0)
                {
                    if (pc.hp > 0) //do not regen if dead
                    {
                        pc.hpRegenTimePassedCounter += mod.currentArea.TimePerSquare;
                        if (pc.hpRegenTimePassedCounter >= gv.cc.getPlayerClass(pc.classTag).hpRegenTimeNeeded)
                        {
                            pc.hp++;
                            if (pc.hp > pc.hpMax)
                            {
                                pc.hp = pc.hpMax;
                            }
                            pc.hpRegenTimePassedCounter = 0;
                            gv.cc.addLogText("<font color='lime'>" + pc.name + " regen 1hp</font><br>");
                        }
                    }
                }
                //check to see if allow SP to regen
                if (gv.cc.getPlayerClass(pc.classTag).spRegenTimeNeeded > 0)
                {
                    pc.spRegenTimePassedCounter += mod.currentArea.TimePerSquare;
                    if (pc.spRegenTimePassedCounter >= gv.cc.getPlayerClass(pc.classTag).spRegenTimeNeeded)
                    {
                        pc.sp++;
                        if (pc.sp > pc.spMax) { pc.sp = pc.spMax; }
                        pc.spRegenTimePassedCounter = 0;
                        gv.cc.addLogText("<font color='lime'>" + pc.name + " regen 1sp</font><br>");
                    }
                }
                //check all items to see if any are regeneration SP or HP type scripts that happen over intervals of time
                RunAllItemRegenerations(pc);

                if ((pc.hp <= 0) && (pc.hp > -20))
                {
                    pc.hp -= 1;
                    gv.cc.addLogText("<font color='red'>" + pc.name + " bleeds 1 HP, dead at -20 HP!" + "</font><BR>");
                    pc.charStatus = "Dead";
                    if (pc.hp <= -20)
                    {
                        gv.cc.addLogText("<font color='red'>" + pc.name + " has DIED!" + "</font><BR>");
                    }
                }
            }
        }
        public void RunAllItemRegenerations(Player pc)
        {
            try
            {
                if (gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).roundsPerSpRegenOutsideCombat > 0)
                {
                    doRegenSp(pc, gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).roundsPerSpRegenOutsideCombat);
                }
                if (gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).roundsPerHpRegenOutsideCombat > 0)
                {
                    doRegenHp(pc, gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref).roundsPerHpRegenOutsideCombat);
                }

                if (gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).roundsPerSpRegenOutsideCombat > 0)
                {
                    doRegenSp(pc, gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).roundsPerSpRegenOutsideCombat);
                }
                if (gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).roundsPerHpRegenOutsideCombat > 0)
                {
                    doRegenHp(pc, gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref).roundsPerHpRegenOutsideCombat);
                }

                if (gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).roundsPerSpRegenOutsideCombat > 0)
                {
                    doRegenSp(pc, gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).roundsPerSpRegenOutsideCombat);
                }
                if (gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).roundsPerHpRegenOutsideCombat > 0)
                {
                    doRegenHp(pc, gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref).roundsPerHpRegenOutsideCombat);
                }

                if (gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).roundsPerSpRegenOutsideCombat > 0)
                {
                    doRegenSp(pc, gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).roundsPerSpRegenOutsideCombat);
                }
                if (gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).roundsPerHpRegenOutsideCombat > 0)
                {
                    doRegenHp(pc, gv.cc.getItemByResRefForInfo(pc.RingRefs.resref).roundsPerHpRegenOutsideCombat);
                }

                if (gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).roundsPerSpRegenOutsideCombat > 0)
                {
                    doRegenSp(pc, gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).roundsPerSpRegenOutsideCombat);
                }
                if (gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).roundsPerHpRegenOutsideCombat > 0)
                {
                    doRegenHp(pc, gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref).roundsPerHpRegenOutsideCombat);
                }

                if (gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).roundsPerSpRegenOutsideCombat > 0)
                {
                    doRegenSp(pc, gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).roundsPerSpRegenOutsideCombat);
                }
                if (gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).roundsPerHpRegenOutsideCombat > 0)
                {
                    doRegenHp(pc, gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref).roundsPerHpRegenOutsideCombat);
                }

                if (gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).roundsPerSpRegenOutsideCombat > 0)
                {
                    doRegenSp(pc, gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).roundsPerSpRegenOutsideCombat);
                }
                if (gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).roundsPerHpRegenOutsideCombat > 0)
                {
                    doRegenHp(pc, gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref).roundsPerHpRegenOutsideCombat);
                }

                if (gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).roundsPerSpRegenOutsideCombat > 0)
                {
                    doRegenSp(pc, gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).roundsPerSpRegenOutsideCombat);
                }
                if (gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).roundsPerHpRegenOutsideCombat > 0)
                {
                    doRegenHp(pc, gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref).roundsPerHpRegenOutsideCombat);
                }

                if (gv.cc.getItemByResRefForInfo(pc.AmmoRefs.resref).roundsPerSpRegenOutsideCombat > 0)
                {
                    doRegenSp(pc, gv.cc.getItemByResRefForInfo(pc.AmmoRefs.resref).roundsPerSpRegenOutsideCombat);
                }
                if (gv.cc.getItemByResRefForInfo(pc.AmmoRefs.resref).roundsPerHpRegenOutsideCombat > 0)
                {
                    doRegenHp(pc, gv.cc.getItemByResRefForInfo(pc.AmmoRefs.resref).roundsPerHpRegenOutsideCombat);
                }
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
            }
        }
        public void doRegenSp(Player pc, int rounds)
        {
            if (mod.WorldTime % (rounds * 6) == 0)
            {
                pc.sp++;
                if (pc.sp > pc.spMax) { pc.sp = pc.spMax; }
                //gv.cc.addLogText("<font color='lime'>" + pc.name + " regen 1sp</font><br>");
            }
        }
        public void doRegenHp(Player pc, int rounds)
        {
            if (mod.WorldTime % (rounds * 6) == 0)
            {
                pc.hp++;
                if (pc.hp > pc.hpMax) { pc.hp = pc.hpMax; }
                //gv.cc.addLogText("<font color='lime'>" + pc.name + " regen 1hp</font><br>");
            }
        }

        //ITEM ON USE
        public void itForceRest()
        {
            if (gv.mod.useRationSystem)
            {
                if (gv.mod.numberOfRationsRemaining > 0)
                {
                    foreach (ItemRefs ir in gv.mod.partyInventoryRefsList)
                    {
                        if (gv.cc.getItemByResRef(ir.resref).isRation)
                        {
                            ir.quantity--;
                            if (ir.quantity < 1)
                            {
                                gv.mod.partyInventoryRefsList.Remove(ir);
                            }
                            break;
                        }
                    }

                    foreach (Player pc in mod.playerList)
                    {
                        if (pc.hp > -20)
                        {
                            pc.hp = pc.hpMax;
                            pc.sp = pc.spMax;
                        }
                    }
                    MessageBox("Party safely rests until completely healed.");
                    gv.cc.addLogText("<gn>" + "Party safely rests until completely healed." + "</gn><BR>");
                }
                else
                {
                    MessageBox("Party cannot rest without rations.");
                    gv.cc.addLogText("<rd>" + "Party cannot rest without rations." + "</rd><BR>");
                }
            }
            else
            {
                foreach (Player pc in mod.playerList)
                {
                    if (pc.hp > -20)
                    {
                        pc.hp = pc.hpMax;
                        pc.sp = pc.spMax;
                    }
                }
                MessageBox("Party safely rests until completely healed.");
                gv.cc.addLogText("<gn>" + "Party safely rests until completely healed." + "</gn><BR>");
            }
        }
        public void itForceRestNoRations()
        {
            foreach (Player pc in mod.playerList)
            {
                if (pc.hp > -20)
                {
                    pc.hp = pc.hpMax;
                    pc.sp = pc.spMax;
                }
            }
            MessageBox("Party safely rests until completely healed.");
            gv.cc.addLogText("<gn>" + "Party safely rests until completely healed." + "</gn><BR>");
        }
        public void itForceRestAndRaiseDead()
        {
            //MessageBox.Show("Heal Light Wounds");
            foreach (Player pc in mod.playerList)
            {
                pc.hp = pc.hpMax;
                pc.sp = pc.spMax;
                pc.charStatus = "Alive";
            }
            MessageBox("Party safely rests until completely healed and the dead are raised.");
            gv.cc.addLogText("<gn>" + "Party safely rests until completely healed and the dead are raised." + "</gn><BR>");
        }
        
        //Effects
        public void efGeneric(object src, Effect ef)
        {
            #region apply the modifiers for damage, heal, buffs and debuffs
            if (src is Creature)
            {
                Creature crt = (Creature)src;
                if (ef.doDamage)
                {
                    #region Do Damage
                    #region Get Resistances
                    float resist = 0;
                    if (ef.damType.Equals("Normal")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueNormal() / 100f)); }
                    else if (ef.damType.Equals("Acid")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueAcid() / 100f)); }
                    else if (ef.damType.Equals("Cold")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueCold() / 100f)); }
                    else if (ef.damType.Equals("Electricity")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueElectricity() / 100f)); }
                    else if (ef.damType.Equals("Fire")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueFire() / 100f)); }
                    else if (ef.damType.Equals("Magic")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueMagic() / 100f)); }
                    else if (ef.damType.Equals("Poison")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValuePoison() / 100f)); }
                    #endregion
                    int damageTotal = 0;
                    #region Calculate Number of Attacks
                    //(for reference) NumOfAttacks: A of these attacks for every B levels after level C up to D attacks total                    
                    int numberOfAttacks = 0;
                    if (ef.damNumberOfAttacksForEveryNLevels == 0) //this effect is using a fixed amount of attacks
                    {
                        numberOfAttacks = ef.damNumberOfAttacks;
                    }
                    else //this effect is using a variable amount of attacks
                    {
                        //numberOfAttacks = (((classLevel - C) / B) + 1) * A;
                        numberOfAttacks = (((ef.classLevelOfSender - ef.damNumberOfAttacksAfterLevelN) / ef.damNumberOfAttacksForEveryNLevels) + 1) * ef.damNumberOfAttacks; //ex: 1 bolt for every 2 levels after level 1
                        if (numberOfAttacks > ef.damNumberOfAttacksUpToNAttacksTotal) { numberOfAttacks = ef.damNumberOfAttacksUpToNAttacksTotal; } //can't have more than a max amount of attacks
                    }                    
                    #endregion
                    //loop over number of attacks
                    for (int i = 0; i < numberOfAttacks; i++)
                    {
                        #region Calculate Damage
                        //(for reference) Attack: AdB+C for every D levels after level E up to F levels total
                        // damage += RandDieRoll(A,B) + C
                        //int damage = (int)((1 * RandInt(4) + 1) * resist);
                        int damage = 0;
                        if (ef.damAttacksEveryNLevels == 0) //this damage is not level based
                        {
                            damage = RandDiceRoll(ef.damNumOfDice, ef.damDie) + ef.damAdder;
                        }
                        else //this damage is level based
                        {
                            int numberOfDamAttacks = ((ef.classLevelOfSender - ef.damAttacksAfterLevelN) / ef.damAttacksEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                            if (numberOfDamAttacks > ef.damAttacksUpToNLevelsTotal) { numberOfDamAttacks = ef.damAttacksUpToNLevelsTotal; } //can't have more than a max amount of attacks
                            for (int j = 0; j < numberOfDamAttacks; j++)
                            {
                                damage += RandDiceRoll(ef.damNumOfDice, ef.damDie) + ef.damAdder;
                            }
                        }
                        #endregion
                        #region Do Calc Save and DC
                        int saveChkRoll = RandInt(20);
                        int saveChk = 0;
                        int DC = 0;
                        int saveChkAdder = 0;
                        if (ef.saveCheckType.Equals("will"))
                        {
                            saveChkAdder = crt.getterWill();
                        }
                        else if (ef.saveCheckType.Equals("reflex"))
                        {
                            saveChkAdder = crt.getterReflex();
                        }
                        else if (ef.saveCheckType.Equals("fortitude"))
                        {
                            saveChkAdder = crt.getterFortitude();
                        }
                        else
                        {
                            saveChkAdder = -99;
                        }
                        saveChk = saveChkRoll + saveChkAdder;
                        DC = ef.saveCheckDC;
                        #endregion
                        if (saveChk >= DC) //passed save check (do half or avoid all?)
                        {
                            damage = damage / 2;
                            gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " evades most of the " + ef.name + "</font><BR>");
                            if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + saveChkAdder + " >= " + DC + "</font><BR>"); }
                        }
                        if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + "</font><BR>"); }
                        int damageAndResist = (int)((float)damage * resist);
                        damageTotal += damageAndResist;
                        gv.cc.addLogText("<font color='silver'>" + crt.cr_name + "</font>" + "<font color='white'>" + " is damaged with " + ef.name 
                                        + " (" + "</font>" + "<font color='lime'>" + damageAndResist + "</font>" + "<font color='white'>" + " damage)</font><BR>");
                    }
                    crt.hp -= damageTotal;
                    if (crt.hp <= 0)
                    {
                        //gv.screenCombat.deathAnimationLocations.Add(new Coordinate(crt.combatLocX, crt.combatLocY));
                        foreach (Coordinate coor in crt.tokenCoveredSquares)
                        {
                            gv.screenCombat.deathAnimationLocations.Add(new Coordinate(coor.X, coor.Y));
                        }
                        gv.cc.addLogText("<font color='lime'>" + "You killed the " + crt.cr_name + "</font><BR>");
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), damageTotal + "");
                    #endregion
                }
                if (ef.doHeal)
                {
                    #region Do Heal
                    #region Calculate Heal
                    //(for reference) Heal: AdB+C for every D levels after level E up to F levels total
                    // heal += RandDieRoll(A,B) + C
                    int heal = 0;
                    if (ef.healActionsEveryNLevels == 0) //this heal is not level based
                    {
                        heal = RandDiceRoll(ef.healNumOfDice, ef.healDie) + ef.healAdder;
                    }
                    else //this heal is level based
                    {
                        int numberOfHealActions = ((ef.classLevelOfSender - ef.healActionsAfterLevelN) / ef.healActionsEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                        if (numberOfHealActions > ef.healActionsUpToNLevelsTotal) { numberOfHealActions = ef.healActionsUpToNLevelsTotal; } //can't have more than a max amount of actions
                        for (int j = 0; j < numberOfHealActions; j++)
                        {
                            heal += RandDiceRoll(ef.healNumOfDice, ef.healDie) + ef.healAdder;
                        }
                    }
                    #endregion
                    crt.hp += heal;
                    if (crt.hp > crt.hpMax)
                    {
                        crt.hp = crt.hpMax;
                    }
                    gv.cc.addLogText("<font color='lime'>" + crt.cr_name + " gains " + heal + " HPs" + "</font><BR>");
                    //Do floaty text heal
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), heal + "", "green");
                    #endregion
                }
                if (ef.doBuff)
                {
                    gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " has effect: " + ef.name + ", (" + ef.durationInUnits + " seconds remain)</font><BR>");
                    //no need to do anything here as buffs are used in updateStats or during
                    //checks such as ef.addStatusType.Equals("Held") on Player or Creature class
                }
                if (ef.doDeBuff)
                {
                    gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " has effect: " + ef.name + ", (" + ef.durationInUnits + " seconds remain)</font><BR>");
                    //no need to do anything here as buffs are used in updateStats or during
                    //checks such as ef.addStatusType.Equals("Held") on Player or Creature class
                }
            }
            else //target is Player
            {
                Player pc = (Player)src;
                if (ef.doDamage)
                {
                    #region Do Damage
                    #region Get Resistances
                    float resistPc = 0;
                    if (ef.damType.Equals("Normal")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalNormal / 100f)); }
                    else if (ef.damType.Equals("Acid")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalAcid / 100f)); }
                    else if (ef.damType.Equals("Cold")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalCold / 100f)); }
                    else if (ef.damType.Equals("Electricity")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalElectricity / 100f)); }
                    else if (ef.damType.Equals("Fire")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalFire / 100f)); }
                    else if (ef.damType.Equals("Magic")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalMagic / 100f)); }
                    else if (ef.damType.Equals("Poison")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalPoison / 100f)); }
                    #endregion
                    int damageTotal = 0;
                    #region Calculate Number of Attacks
                    //(for reference) NumOfAttacks: A of these attacks for every B levels after level C up to D attacks total                    
                    int numberOfAttacks = 0;
                    if (ef.damNumberOfAttacksForEveryNLevels == 0) //this effect is using a fixed amount of attacks
                    {
                        numberOfAttacks = ef.damNumberOfAttacks;
                    }
                    else //this effect is using a variable amount of attacks
                    {
                        //numberOfAttacks = (((classLevel - C) / B) + 1) * A;
                        numberOfAttacks = (((ef.classLevelOfSender - ef.damNumberOfAttacksAfterLevelN) / ef.damNumberOfAttacksForEveryNLevels) + 1) * ef.damNumberOfAttacks; //ex: 1 bolt for every 2 levels after level 1
                    }
                    if (numberOfAttacks > ef.damNumberOfAttacksUpToNAttacksTotal) { numberOfAttacks = ef.damNumberOfAttacksUpToNAttacksTotal; } //can't have more than a max amount of attacks
                    #endregion
                    //loop over number of attacks
                    for (int i = 0; i < numberOfAttacks; i++)
                    {
                        #region Calculate Damage
                        //(for reference) Attack: AdB+C for every D levels after level E up to F levels total
                        // damage += RandDieRoll(A,B) + C
                        //int damage = (int)((1 * RandInt(4) + 1) * resist);
                        int damagePc = 0;
                        if (ef.damAttacksEveryNLevels == 0) //this damage is not level based
                        {
                            damagePc = RandDiceRoll(ef.damNumOfDice, ef.damDie) + ef.damAdder;
                        }
                        else //this damage is level based
                        {
                            int numberOfDamAttacks = ((ef.classLevelOfSender - ef.damAttacksAfterLevelN) / ef.damAttacksEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                            if (numberOfDamAttacks > ef.damAttacksUpToNLevelsTotal) { numberOfDamAttacks = ef.damAttacksUpToNLevelsTotal; } //can't have more than a max amount of attacks
                            for (int j = 0; j < numberOfDamAttacks; j++)
                            {
                                damagePc += RandDiceRoll(ef.damNumOfDice, ef.damDie) + ef.damAdder;
                            }
                        }
                        #endregion
                        #region Do Calc Save and DC
                        int saveChkRollPc = RandInt(20);
                        int saveChkPc = 0;
                        int DCPc = 0;
                        int saveChkAdder = 0;
                        if (ef.saveCheckType.Equals("will"))
                        {
                            saveChkAdder = pc.will;
                        }
                        else if (ef.saveCheckType.Equals("reflex"))
                        {
                            saveChkAdder = pc.reflex;
                        }
                        else if (ef.saveCheckType.Equals("fortitude"))
                        {
                            saveChkAdder = pc.fortitude;
                        }
                        else
                        {
                            saveChkAdder = -99;
                        }
                        saveChkPc = saveChkRollPc + saveChkAdder;
                        DCPc = ef.saveCheckDC;
                        #endregion
                        if (saveChkPc >= DCPc) //passed save check (do half or avoid all?)
                        {
                            damagePc = damagePc / 2;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades most of the " + ef.name + "</font><BR>");
                            if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + saveChkRollPc + " + " + saveChkAdder + " >= " + DCPc + "</font><BR>"); }
                        }
                        if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + "resist = " + resistPc + " damage = " + damagePc + "</font><BR>"); }
                        int damageAndResist = (int)((float)damagePc * resistPc);
                        damageTotal += damageAndResist;
                        gv.cc.addLogText("<font color='silver'>"
                                        + pc.name + "</font>" + "<font color='white'>" + " is damaged with " + ef.name + " (" + "</font>" + "<font color='lime'>"
                                        + damageAndResist + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    }
                    pc.hp -= damageTotal;
                    if (pc.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(pc.combatLocX, pc.combatLocY));
                        gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                        pc.charStatus = "Dead";
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), damageTotal + "");
                    #endregion
                }
                if (ef.doHeal)
                {
                    #region Do Heal
                    if (pc.hp <= -20)
                    {
                        //MessageBox("Can't heal a dead character!");
                        gv.cc.addLogText("<font color='red'>" + "Can't heal a dead character!" + "</font><BR>");
                    }
                    else
                    {
                        #region Calculate Heal
                        //(for reference) Heal: AdB+C for every D levels after level E up to F levels total
                        // heal += RandDieRoll(A,B) + C
                        int heal = 0;
                        if (ef.healActionsEveryNLevels == 0) //this heal is not level based
                        {
                            heal = RandDiceRoll(ef.healNumOfDice, ef.healDie) + ef.healAdder;
                        }
                        else //this heal is level based
                        {
                            int numberOfHealActions = ((ef.classLevelOfSender - ef.healActionsAfterLevelN) / ef.healActionsEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                            if (numberOfHealActions > ef.healActionsUpToNLevelsTotal) { numberOfHealActions = ef.healActionsUpToNLevelsTotal; } //can't have more than a max amount of actions
                            for (int j = 0; j < numberOfHealActions; j++)
                            {
                                heal += RandDiceRoll(ef.healNumOfDice, ef.healDie) + ef.healAdder;
                            }
                        }
                        #endregion
                        pc.hp += heal;
                        if (pc.hp > pc.hpMax)
                        {
                            pc.hp = pc.hpMax;
                        }
                        if (pc.hp > 0)
                        {
                            pc.charStatus = "Alive";
                        }
                        gv.cc.addLogText("<font color='lime'>" + pc.name + " gains " + heal + " HPs" + "</font><BR>");
                        //Do floaty text heal
                        //gv.screenCombat.floatyTextOn = true;
                        gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), heal + "", "green");
                    }
                    #endregion
                }
                if (ef.doBuff)
                {
                    gv.cc.addLogText("<font color='yellow'>" + pc.name + " has effect: " + ef.name + ", (" + ef.durationInUnits + " seconds remain)</font><BR>");
                    //no need to do anything here as buffs are used in updateStats or during
                    //checks such as ef.addStatusType.Equals("Held") on Player or Creature class
                }
                if (ef.doDeBuff)
                {
                    gv.cc.addLogText("<font color='yellow'>" + pc.name + " has effect: " + ef.name + ", (" + ef.durationInUnits + " seconds remain)</font><BR>");
                    //no need to do anything here as buffs are used in updateStats or during
                    //checks such as ef.addStatusType.Equals("Held") on Player or Creature class
                }
            }
            #endregion

            #region remove dead creatures **not sure if we should do this here or not**           
            /*for (int x = mod.currentEncounter.encounterCreatureList.Count - 1; x >= 0; x--)
            {
                if (mod.currentEncounter.encounterCreatureList[x].hp <= 0)
                {
                    try
                    {
                        //do OnDeath IBScript
                        gv.cc.doIBScriptBasedOnFilename(mod.currentEncounter.encounterCreatureList[x].onDeathIBScript, mod.currentEncounter.encounterCreatureList[x].onDeathIBScriptParms);
                        mod.currentEncounter.encounterCreatureList.RemoveAt(x);
                        mod.currentEncounter.encounterCreatureRefsList.RemoveAt(x);
                    }
                    catch (Exception ex)
                    {
                        gv.errorLog(ex.ToString());
                    }
                }
            }*/
            #endregion

//            gv.postDelayed("doFloatyText", 100);
        }
        public float GetDistanceF(int sX, int sY, int eX, int eY)
        {
            double y = Math.Abs(eY - sY);
            double x = Math.Abs(eX - sX);
            double dist = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return (float)dist;
        }
        public void CreateAoeSquaresList(object src, object trg, AreaOfEffectShape shape, int aoeRadius)
        {
            AoeSquaresList.Clear();

            Coordinate target = new Coordinate(0,0);

            if (trg is Player)
            {
                Player pc = (Player)trg;
                target = new Coordinate(pc.combatLocX, pc.combatLocY);
            }
            else if (trg is Creature)
            {
                Creature crt = (Creature)trg;
                target = new Coordinate(crt.combatLocX, crt.combatLocY);
            }
            else if (trg is Coordinate)
            {
                target = (Coordinate)trg;
            }            

            //define AoE Radius
            int srcX = 0;
            int srcY = 0;
            if (src is Player)
            {
                Player pcs = (Player)src;
                srcX = pcs.combatLocX;
                srcY = pcs.combatLocY;
            }
            else if (src is Creature)
            {
                Creature crts = (Creature)src;
                srcX = crts.combatLocX;
                srcY = crts.combatLocY;
            }
            else if (src is Item) //item was used
            {
                Player pcs = mod.playerList[gv.screenCombat.currentPlayerIndex];
                srcX = pcs.combatLocX;
                srcY = pcs.combatLocY;
            }
            else if (src is Coordinate) //prop or trigger was used
            {
                Coordinate coor = (Coordinate)src;
                srcX = coor.X;
                srcY = coor.Y;
            }

            //shape and radius
            #region Circle
            if (shape == AreaOfEffectShape.Circle)
            {
                for (int x = target.X - aoeRadius; x <= target.X + aoeRadius; x++)
                {
                    for (int y = target.Y - aoeRadius; y <= target.Y + aoeRadius; y++)
                    {
                        //TODO check for LoS from (target.X, target.Y) center location to (x,y)
                        AoeSquaresList.Add(new Coordinate(x, y));
                    }
                }                
            }
            #endregion
            #region Cone
            else if (shape == AreaOfEffectShape.Cone)
            {
                int signX = target.X - srcX;
                int signY = target.Y - srcY;
                int incY = 0;
                if (signY < 0) { incY = -1; }
                else { incY = 1; }
                int incX = 0;
                if (signX < 0) { incX = -1; }
                else { incX = 1; }

                //non-diagnols
                if ((signX == 0) || (signY == 0))
                {
                    if (signY == 0) //right or left
                    {
                        for (int x = 0; Math.Abs(x) <= aoeRadius; x += incX)
                        {
                            for (int y = -Math.Abs(x); y <= Math.Abs(x); y++)
                            {
                                float r = GetDistanceF(0, 0, x, y);
                                if (r <= aoeRadius)
                                {
                                    //TODO check for LoS from (target.X, target.Y) center location to (x,y)
                                    AoeSquaresList.Add(new Coordinate(x + target.X, y + target.Y));
                                }
                            }
                        }
                    }
                    else //up or down
                    {
                        for (int y = 0; Math.Abs(y) <= aoeRadius; y += incY)
                        {
                            for (int x = -Math.Abs(y); x <= Math.Abs(y); x++)
                            {
                                float r = GetDistanceF(0, 0, x, y);
                                if (r <= aoeRadius)
                                {
                                    //TODO check for LoS from (target.X, target.Y) center location to (x,y)
                                    AoeSquaresList.Add(new Coordinate(x + target.X, y + target.Y));
                                }
                            }
                        }
                    }
                }
                //diagnols
                else
                {
                    for (int x = 0; Math.Abs(x) <= aoeRadius; x += incX)
                    {
                        for (int y = 0; Math.Abs(y) <= aoeRadius; y += incY)
                        {
                            float r = GetDistanceF(0, 0, x, y);
                            if (r <= aoeRadius)
                            {
                                //TODO check for LoS from (target.X, target.Y) center location to (x,y)
                                AoeSquaresList.Add(new Coordinate(x + target.X, y + target.Y));
                            }
                        }
                    }
                }
            }
            #endregion
            #region Line
            else if (shape == AreaOfEffectShape.Line)
            {
                int rise = target.Y - srcY;
                int incY = 0;
                if (rise < 0) { incY = -1; }
                else { incY = 1; }
                if (rise == 0) { incY = 0; }
                int run = target.X - srcX;
                int incX = 0;
                if (run < 0) { incX = -1; }
                else { incX = 1; }
                if (run == 0) { incX = 0; }
                int slope = 1;
                if (Math.Abs(rise) > Math.Abs(run))
                {
                    if (run != 0)
                    {
                        slope = rise / run;
                    }
                }
                else
                {
                    if (rise != 0)
                    {
                        slope = run / rise;
                    }
                }
                int currentX = target.X;
                int currentY = target.Y;
                int riseCnt = 1;
                for (int i = 0; i < aoeRadius; i++)
                {
                    //TODO check for LoS from (target.X, target.Y) center location to (x,y)
                    AoeSquaresList.Add(new Coordinate(currentX, currentY));

                    //do the increments for the next location
                    if (Math.Abs(rise) > Math.Abs(run))
                    {
                        if (riseCnt < Math.Abs(slope)) //do rise increment only
                        {
                            currentY += incY;
                            riseCnt++;
                        }
                        else //do rise and run then reset riseCnt = 0
                        {
                            currentY += incY;
                            currentX += incX;
                            riseCnt = 1;
                        }
                    }
                    else
                    {
                        if (riseCnt < Math.Abs(slope)) //do rise increment only
                        {
                            currentX += incX;
                            riseCnt++;
                        }
                        else //do rise and run then reset riseCnt = 0
                        {
                            currentY += incY;
                            currentX += incX;
                            riseCnt = 1;
                        }
                    }
                }
            }
            #endregion
        }
        public void CreateAoeTargetsList(object src, object trg, bool usedForEffectSquares)
        {
            AoeTargetsList.Clear();

            int startX2 = 0;
            int startY2 = 0;
            if (src is Player)
            {
                startX2 = (int)(gv.screenCombat.targetHighlightCenterLocation.X * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                startY2 = (int)(gv.screenCombat.targetHighlightCenterLocation.Y * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
            }
            else if (src is Item)
            {
                startX2 = (int)(gv.screenCombat.targetHighlightCenterLocation.X * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                startY2 = (int)(gv.screenCombat.targetHighlightCenterLocation.Y * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
            }
            else if (src is Coordinate) //called from a prop or trigger
            {
                startX2 = (int)(gv.mod.currentEncounter.triggerScriptCalledFromSquareLocX * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                startY2 = (int)(gv.mod.currentEncounter.triggerScriptCalledFromSquareLocX * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
            }
            else if (src is Creature) //source is a Creature
            {
                if (trg is Player)
                {
                    Player pcs = (Player)trg;
                    startX2 = (int)(pcs.combatLocX * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                    startY2 = (int)(pcs.combatLocY * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                }
                else if (trg is Creature)
                {
                    Creature crts = (Creature)trg;
                    startX2 = (int)(crts.combatLocX * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                    startY2 = (int)(crts.combatLocY * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                }
                else if (trg is Coordinate)
                {
                    Coordinate pnt = (Coordinate)gv.sf.CombatTarget;
                    startX2 = (int)(pnt.X * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                    startY2 = (int)(pnt.Y * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                }
            }

            foreach (Coordinate coor in AoeSquaresList)
            {
                int endX2 = (int)(coor.X * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                int endY2 = (int)(coor.Y * gv.squareSize * gv.scaler + (gv.squareSize * gv.scaler / 2));
                
                if (gv.screenCombat.isVisibleLineOfSight(new Coordinate(endX2, endY2), new Coordinate(startX2, startY2)))
                {
                    if (usedForEffectSquares)
                    {
                        AoeTargetsList.Add(new Coordinate(coor.X, coor.Y));
                    }
                    else
                    {
                        foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
                        {
                            //if any part of creature is in range of radius of x and radius of y
                            foreach (Coordinate crtCoor in crt.tokenCoveredSquares)
                            {
                                if ((crtCoor.X == coor.X) && (crtCoor.Y == coor.Y))
                                {
                                    AoeTargetsList.Add(crt);
                                }
                            }
                        }
                        foreach (Player pc in mod.playerList)
                        {
                            //if in range of radius of x and radius of y
                            if ((pc.combatLocX == coor.X) && (pc.combatLocY == coor.Y))
                            {
                                AoeTargetsList.Add(pc);
                            }
                        }
                    }                    
                }
            }
        }

        public void spGeneric(Spell thisSpell, object src, object trg, bool outsideCombat)
        {
            //set squares list
            CreateAoeSquaresList(src, trg, thisSpell.aoeShape, thisSpell.aoeRadius);

            //set target list
            if (outsideCombat)
            {
                AoeTargetsList.Clear();
                AoeTargetsList.Add(trg);
            }
            else
            {
                if (thisSpell.isUsedForCombatSquareEffect)
                {
                    CreateAoeTargetsList(src, trg, true);
                }
                else
                {
                    CreateAoeTargetsList(src, trg, false);
                }
            }

            if (!thisSpell.spellEffectTag.Equals("none"))
            {
                Effect thisSpellEffect = gv.cc.getEffectByTag(thisSpell.spellEffectTag);
                spGenericLoop(thisSpellEffect, thisSpell, src, trg, outsideCombat);
            }
            else if (thisSpell.spellEffectTagList.Count > 0)
            {
                foreach (EffectTagForDropDownList eftag in thisSpell.spellEffectTagList)
                {
                    Effect thisSpellEffect = gv.cc.getEffectByTag(eftag.tag);
                    spGenericLoop(thisSpellEffect, thisSpell, src, trg, outsideCombat);
                }
            }

            #region remove dead creatures            
            /*for (int x = mod.currentEncounter.encounterCreatureList.Count - 1; x >= 0; x--)
            {
                if (mod.currentEncounter.encounterCreatureList[x].hp <= 0)
                {
                    try
                    {
                        //do OnDeath IBScript
                        gv.cc.doIBScriptBasedOnFilename(mod.currentEncounter.encounterCreatureList[x].onDeathIBScript, mod.currentEncounter.encounterCreatureList[x].onDeathIBScriptParms);
                        mod.currentEncounter.encounterCreatureList.RemoveAt(x);
                        mod.currentEncounter.encounterCreatureRefsList.RemoveAt(x);
                    }
                    catch (Exception ex)
                    {
                        gv.errorLog(ex.ToString());
                    }
                }
            }*/
            #endregion
        }
        public void spGenericLoop(Effect thisSpellEffect, Spell thisSpell, object src, object trg, bool outsideCombat)
        {
            #region Get casting source information
            int classLevel = 0;
            string sourceName = "";
            if (thisSpellEffect == null)
            {
                gv.sf.MessageBoxHtml("EffectTag: " + thisSpell.spellEffectTag + " does not exist in this module. Abort spell cast.");
                return;
            }
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                classLevel = source.classLevel;
                sourceName = source.name;
                source.sp -= thisSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Creature) //creature casting
            {
                Creature source = (Creature)src;
                classLevel = source.cr_level;
                sourceName = source.cr_name;
                source.sp -= thisSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Item) //item was used
            {
                Item source = (Item)src;
                classLevel = source.levelOfItemForCastSpell;
                sourceName = source.name;
            }
            else if (src is Coordinate) //trigger or prop was used
            {
                classLevel = 1;
                sourceName = "trigger";
            }
            #endregion

            if (thisSpell.isUsedForCombatSquareEffect)
            {
                #region Iterate over squares and apply effect to them
                int numberOfRounds = thisSpellEffect.durationInUnits / gv.mod.TimePerRound;
                gv.cc.addLogText("<gn>" + thisSpellEffect.name + " is applied for " + numberOfRounds + " round(s)</gn><BR>");
                foreach (object target in AoeTargetsList)
                {
                    if (target is Coordinate)
                    {
                        Coordinate c = (Coordinate)target;
                        Effect e = thisSpellEffect.DeepCopy();
                        e.combatLocX = c.X;
                        e.combatLocY = c.Y;
                        gv.mod.currentEncounter.AddEffectByObject(e, classLevel);
                    }
                }
                #endregion
            }
            else
            {
                #region Iterate over targets and apply the modifiers for damage, heal, buffs and debuffs
                foreach (object target in AoeTargetsList)
                {
                    if (target is Creature)
                    {
                        Creature crt = (Creature)target;
                        if (thisSpellEffect.doDamage)
                        {
                            #region Do Damage
                            #region Get Resistances
                            float resist = 0;
                            if (thisSpellEffect.damType.Equals("Normal")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueNormal() / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Acid")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueAcid() / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Cold")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueCold() / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Electricity")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueElectricity() / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Fire")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueFire() / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Magic")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueMagic() / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Poison")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValuePoison() / 100f)); }
                            #endregion
                            int damageTotal = 0;
                            #region Calculate Number of Attacks
                            //(for reference) NumOfAttacks: A of these attacks for every B levels after level C up to D attacks total                    
                            int numberOfAttacks = 0;
                            if (thisSpellEffect.damNumberOfAttacksForEveryNLevels == 0) //this effect is using a fixed amount of attacks
                            {
                                numberOfAttacks = thisSpellEffect.damNumberOfAttacks;
                            }
                            else //this effect is using a variable amount of attacks
                            {
                                //numberOfAttacks = (((classLevel - C) / B) + 1) * A;
                                numberOfAttacks = (((classLevel - thisSpellEffect.damNumberOfAttacksAfterLevelN) / thisSpellEffect.damNumberOfAttacksForEveryNLevels) + 1) * thisSpellEffect.damNumberOfAttacks; //ex: 1 bolt for every 2 levels after level 1
                                if (numberOfAttacks > thisSpellEffect.damNumberOfAttacksUpToNAttacksTotal) { numberOfAttacks = thisSpellEffect.damNumberOfAttacksUpToNAttacksTotal; } //can't have more than a max amount of attacks
                            }

                            #endregion
                            //loop over number of attacks
                            for (int i = 0; i < numberOfAttacks; i++)
                            {
                                #region Calculate Damage
                                //(for reference) Attack: AdB+C for every D levels after level E up to F levels total
                                // damage += RandDieRoll(A,B) + C
                                //int damage = (int)((1 * RandInt(4) + 1) * resist);
                                int damage = 0;
                                if (thisSpellEffect.damAttacksEveryNLevels == 0) //this damage is not level based
                                {
                                    damage = RandDiceRoll(thisSpellEffect.damNumOfDice, thisSpellEffect.damDie) + thisSpellEffect.damAdder;
                                }
                                else //this damage is level based
                                {
                                    int numberOfDamAttacks = ((classLevel - thisSpellEffect.damAttacksAfterLevelN) / thisSpellEffect.damAttacksEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                                    if (numberOfDamAttacks > thisSpellEffect.damAttacksUpToNLevelsTotal) { numberOfDamAttacks = thisSpellEffect.damAttacksUpToNLevelsTotal; } //can't have more than a max amount of attacks
                                    for (int j = 0; j < numberOfDamAttacks; j++)
                                    {
                                        damage += RandDiceRoll(thisSpellEffect.damNumOfDice, thisSpellEffect.damDie) + thisSpellEffect.damAdder;
                                    }
                                }
                                #endregion
                                #region Do Calc Save and DC
                                int saveChkRoll = RandInt(20);
                                int saveChk = 0;
                                int DC = 0;
                                int saveChkAdder = 0;
                                if (thisSpellEffect.saveCheckType.Equals("will"))
                                {
                                    saveChkAdder = crt.getterWill();
                                }
                                else if (thisSpellEffect.saveCheckType.Equals("reflex"))
                                {
                                    saveChkAdder = crt.getterReflex();
                                }
                                else if (thisSpellEffect.saveCheckType.Equals("fortitude"))
                                {
                                    saveChkAdder = crt.getterFortitude();
                                }
                                else
                                {
                                    saveChkAdder = -99;
                                }
                                saveChk = saveChkRoll + saveChkAdder;
                                DC = thisSpellEffect.saveCheckDC;
                                #endregion
                                if (saveChk >= DC) //passed save check (do half or avoid all?)
                                {
                                    damage = damage / 2;
                                    gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " evades most of the " + thisSpellEffect.name + "</font><BR>");
                                    if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + saveChkAdder + " >= " + DC + "</font><BR>"); }
                                }
                                if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + "</font><BR>"); }
                                int damageAndResist = (int)((float)damage * resist);
                                damageTotal += damageAndResist;
                                gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " damages " + "</font>" + "<font color='silver'>"
                                                + crt.cr_name + "</font>" + "<font color='white'>" + "with " + thisSpellEffect.name + " (" + "</font>" + "<font color='lime'>"
                                                + damageAndResist + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                            }
                            crt.hp -= damageTotal;
                            if (crt.hp <= 0)
                            {
                                //gv.screenCombat.deathAnimationLocations.Add(new Coordinate(crt.combatLocX, crt.combatLocY));
                                foreach (Coordinate coor in crt.tokenCoveredSquares)
                                {
                                    gv.screenCombat.deathAnimationLocations.Add(new Coordinate(coor.X, coor.Y));
                                }
                                gv.cc.addLogText("<font color='lime'>" + "You killed the " + crt.cr_name + "</font><BR>");
                            }
                            //Do floaty text damage
                            //gv.screenCombat.floatyTextOn = true;
                            gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), damageTotal + "");
                            #endregion
                        }
                        if (thisSpellEffect.doHeal)
                        {
                            if (src is Player) //PCs shouldn't heal creatures
                            {
                                continue;
                            }
                            #region Do Heal
                            #region Calculate Heal
                            //(for reference) Heal: AdB+C for every D levels after level E up to F levels total
                            // heal += RandDieRoll(A,B) + C
                            int heal = 0;
                            if (thisSpellEffect.healActionsEveryNLevels == 0) //this heal is not level based
                            {
                                heal = RandDiceRoll(thisSpellEffect.healNumOfDice, thisSpellEffect.healDie) + thisSpellEffect.healAdder;
                            }
                            else //this heal is level based
                            {
                                int numberOfHealActions = ((classLevel - thisSpellEffect.healActionsAfterLevelN) / thisSpellEffect.healActionsEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                                if (numberOfHealActions > thisSpellEffect.healActionsUpToNLevelsTotal) { numberOfHealActions = thisSpellEffect.healActionsUpToNLevelsTotal; } //can't have more than a max amount of actions
                                for (int j = 0; j < numberOfHealActions; j++)
                                {
                                    heal += RandDiceRoll(thisSpellEffect.healNumOfDice, thisSpellEffect.healDie) + thisSpellEffect.healAdder;
                                }
                            }
                            #endregion
                            if (thisSpellEffect.healHP)
                            {
                                crt.hp += heal;
                                if (crt.hp > crt.hpMax)
                                {
                                    crt.hp = crt.hpMax;
                                }
                                gv.cc.addLogText("<font color='lime'>" + crt.cr_name + " gains " + heal + " HPs" + "</font><BR>");
                                gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), heal + "", "green");
                            }
                            else
                            {
                                crt.sp += heal;
                                gv.cc.addLogText("<font color='lime'>" + crt.cr_name + " gains " + heal + " SPs" + "</font><BR>");
                                gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), heal + "", "yellow");
                            }
                            //Do floaty text heal
                            //gv.screenCombat.floatyTextOn = true;

                            #endregion
                        }
                        if (thisSpellEffect.doBuff)
                        {
                            if (src is Player) //PCs shouldn't buff creatures
                            {
                                continue;
                            }
                            #region Do Buff
                            int numberOfRounds = thisSpellEffect.durationInUnits / gv.mod.TimePerRound;
                            gv.cc.addLogText("<font color='lime'>" + thisSpellEffect.name + " is applied on " + crt.cr_name + " for " + numberOfRounds + " round(s)</font><BR>");
                            crt.AddEffectByObject(thisSpellEffect, classLevel);
                            #endregion
                        }
                        if (thisSpellEffect.doDeBuff)
                        {
                            #region Do DeBuff
                            #region Do Calc Save and DC
                            int saveChkRoll = RandInt(20);
                            int saveChk = 0;
                            int DC = 0;
                            int saveChkAdder = 0;
                            if (thisSpellEffect.saveCheckType.Equals("will"))
                            {
                                saveChkAdder = crt.getterWill();
                            }
                            else if (thisSpellEffect.saveCheckType.Equals("reflex"))
                            {
                                saveChkAdder = crt.getterReflex();
                            }
                            else if (thisSpellEffect.saveCheckType.Equals("fortitude"))
                            {
                                saveChkAdder = crt.getterFortitude();
                            }
                            else
                            {
                                saveChkAdder = -99;
                            }
                            saveChk = saveChkRoll + saveChkAdder;
                            DC = thisSpellEffect.saveCheckDC;
                            #endregion
                            if (saveChk >= DC) //passed save check
                            {
                                gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " avoids the " + thisSpellEffect.name + " effect.</font><BR>");
                            }
                            else
                            {
                                int numberOfRounds = thisSpellEffect.durationInUnits / gv.mod.TimePerRound;
                                gv.cc.addLogText("<font color='lime'>" + thisSpellEffect.name + " is applied on " + crt.cr_name + " for " + numberOfRounds + " round(s)</font><BR>");
                                crt.AddEffectByObject(thisSpellEffect, classLevel);
                            }
                            #endregion
                        }
                        if (thisSpell.removeEffectTagList.Count > 0)
                        {
                            #region remove effects  
                            foreach (EffectTagForDropDownList efTag in thisSpell.removeEffectTagList)
                            {
                                for (int x = crt.cr_effectsList.Count - 1; x >= 0; x--)
                                {
                                    if (crt.cr_effectsList[x].tag.Equals(efTag.tag))
                                    {
                                        try
                                        {
                                            crt.cr_effectsList.RemoveAt(x);
                                        }
                                        catch (Exception ex)
                                        {
                                            gv.errorLog(ex.ToString());
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    else //target is Player
                    {
                        Player pc = (Player)target;
                        if (thisSpellEffect.doDamage)
                        {
                            #region Do Damage
                            #region Get Resistances
                            float resistPc = 0;
                            if (thisSpellEffect.damType.Equals("Normal")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalNormal / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Acid")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalAcid / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Cold")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalCold / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Electricity")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalElectricity / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Fire")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalFire / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Magic")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalMagic / 100f)); }
                            else if (thisSpellEffect.damType.Equals("Poison")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalPoison / 100f)); }
                            #endregion
                            int damageTotal = 0;
                            #region Calculate Number of Attacks
                            //(for reference) NumOfAttacks: A of these attacks for every B levels after level C up to D attacks total                    
                            int numberOfAttacks = 0;
                            if (thisSpellEffect.damNumberOfAttacksForEveryNLevels == 0) //this effect is using a fixed amount of attacks
                            {
                                numberOfAttacks = thisSpellEffect.damNumberOfAttacks;
                            }
                            else //this effect is using a variable amount of attacks
                            {
                                //numberOfAttacks = (((classLevel - C) / B) + 1) * A;
                                numberOfAttacks = (((classLevel - thisSpellEffect.damNumberOfAttacksAfterLevelN) / thisSpellEffect.damNumberOfAttacksForEveryNLevels) + 1) * thisSpellEffect.damNumberOfAttacks; //ex: 1 bolt for every 2 levels after level 1
                            }
                            if (numberOfAttacks > thisSpellEffect.damNumberOfAttacksUpToNAttacksTotal) { numberOfAttacks = thisSpellEffect.damNumberOfAttacksUpToNAttacksTotal; } //can't have more than a max amount of attacks
                            #endregion
                            //loop over number of attacks
                            for (int i = 0; i < numberOfAttacks; i++)
                            {
                                #region Calculate Damage
                                //(for reference) Attack: AdB+C for every D levels after level E up to F levels total
                                // damage += RandDieRoll(A,B) + C
                                //int damage = (int)((1 * RandInt(4) + 1) * resist);
                                int damagePc = 0;
                                if (thisSpellEffect.damAttacksEveryNLevels == 0) //this damage is not level based
                                {
                                    damagePc = RandDiceRoll(thisSpellEffect.damNumOfDice, thisSpellEffect.damDie) + thisSpellEffect.damAdder;
                                }
                                else //this damage is level based
                                {
                                    int numberOfDamAttacks = ((classLevel - thisSpellEffect.damAttacksAfterLevelN) / thisSpellEffect.damAttacksEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                                    if (numberOfDamAttacks > thisSpellEffect.damAttacksUpToNLevelsTotal) { numberOfDamAttacks = thisSpellEffect.damAttacksUpToNLevelsTotal; } //can't have more than a max amount of attacks
                                    for (int j = 0; j < numberOfDamAttacks; j++)
                                    {
                                        damagePc += RandDiceRoll(thisSpellEffect.damNumOfDice, thisSpellEffect.damDie) + thisSpellEffect.damAdder;
                                    }
                                }
                                #endregion
                                #region Do Calc Save and DC
                                int saveChkRollPc = RandInt(20);
                                int saveChkPc = 0;
                                int DCPc = 0;
                                int saveChkAdder = 0;
                                if (thisSpellEffect.saveCheckType.Equals("will"))
                                {
                                    saveChkAdder = pc.will;
                                }
                                else if (thisSpellEffect.saveCheckType.Equals("reflex"))
                                {
                                    saveChkAdder = pc.reflex;
                                }
                                else if (thisSpellEffect.saveCheckType.Equals("fortitude"))
                                {
                                    saveChkAdder = pc.fortitude;
                                }
                                else
                                {
                                    saveChkAdder = -99;
                                }
                                saveChkPc = saveChkRollPc + saveChkAdder;
                                DCPc = thisSpellEffect.saveCheckDC;
                                #endregion
                                if (saveChkPc >= DCPc) //passed save check (do half or avoid all?)
                                {
                                    damagePc = damagePc / 2;
                                    gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades most of the " + thisSpellEffect.name + "</font><BR>");
                                    if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + saveChkRollPc + " + " + saveChkAdder + " >= " + DCPc + "</font><BR>"); }
                                }
                                if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + "resist = " + resistPc + " damage = " + damagePc + "</font><BR>"); }
                                int damageAndResist = (int)((float)damagePc * resistPc);
                                damageTotal += damageAndResist;
                                gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " damages " + "</font>" + "<font color='silver'>"
                                                + pc.name + "</font>" + "<font color='white'>" + "with " + thisSpellEffect.name + " (" + "</font>" + "<font color='lime'>"
                                                + damageAndResist + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                            }
                            pc.hp -= damageTotal;
                            if (pc.hp <= 0)
                            {
                                gv.screenCombat.deathAnimationLocations.Add(new Coordinate(pc.combatLocX, pc.combatLocY));
                                gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                                pc.charStatus = "Dead";
                            }
                            //Do floaty text damage
                            //gv.screenCombat.floatyTextOn = true;
                            gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), damageTotal + "");
                            #endregion
                        }
                        if (thisSpellEffect.doHeal)
                        {
                            if (src is Creature) //Creatures shouldn't heal PCs
                            {
                                continue;
                            }
                            #region Do Heal
                            if (pc.hp <= -20)
                            {
                                //MessageBox("Can't heal a dead character!");
                                gv.cc.addLogText("<font color='red'>" + "Can't heal a dead character!" + "</font><BR>");
                            }
                            else
                            {
                                #region Calculate Heal
                                //(for reference) Heal: AdB+C for every D levels after level E up to F levels total
                                // heal += RandDieRoll(A,B) + C
                                int heal = 0;
                                if (thisSpellEffect.healActionsEveryNLevels == 0) //this heal is not level based
                                {
                                    heal = RandDiceRoll(thisSpellEffect.healNumOfDice, thisSpellEffect.healDie) + thisSpellEffect.healAdder;
                                }
                                else //this heal is level based
                                {
                                    int numberOfHealActions = ((classLevel - thisSpellEffect.healActionsAfterLevelN) / thisSpellEffect.healActionsEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                                    if (numberOfHealActions > thisSpellEffect.healActionsUpToNLevelsTotal) { numberOfHealActions = thisSpellEffect.healActionsUpToNLevelsTotal; } //can't have more than a max amount of actions
                                    for (int j = 0; j < numberOfHealActions; j++)
                                    {
                                        heal += RandDiceRoll(thisSpellEffect.healNumOfDice, thisSpellEffect.healDie) + thisSpellEffect.healAdder;
                                    }
                                }
                                #endregion
                                if (thisSpellEffect.healHP)
                                {
                                    pc.hp += heal;
                                    if (pc.hp > pc.hpMax)
                                    {
                                        pc.hp = pc.hpMax;
                                    }
                                    if (pc.hp > 0)
                                    {
                                        pc.charStatus = "Alive";
                                    }
                                    gv.cc.addLogText("<font color='lime'>" + pc.name + " gains " + heal + " HPs" + "</font><BR>");
                                    gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), heal + "", "green");
                                }
                                else
                                {
                                    pc.sp += heal;
                                    if (pc.sp > pc.spMax)
                                    {
                                        pc.sp = pc.spMax;
                                    }
                                    gv.cc.addLogText("<font color='lime'>" + pc.name + " gains " + heal + " SPs" + "</font><BR>");
                                    gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), heal + "", "yellow");
                                }
                                //Do floaty text heal
                                //gv.screenCombat.floatyTextOn = true;

                            }
                            #endregion
                        }
                        if (thisSpellEffect.doBuff)
                        {
                            if (src is Creature) //Creatures shouldn't buff PCs
                            {
                                continue;
                            }
                            #region Do Buff
                            int numberOfRounds = thisSpellEffect.durationInUnits / gv.mod.TimePerRound;
                            gv.cc.addLogText("<font color='lime'>" + thisSpellEffect.name + " is applied on " + pc.name + " for " + numberOfRounds + " round(s)</font><BR>");
                            pc.AddEffectByObject(thisSpellEffect, classLevel);
                            #endregion
                        }
                        if (thisSpellEffect.doDeBuff)
                        {
                            #region Do DeBuff
                            #region Do Calc Save and DC
                            int saveChkRoll = RandInt(20);
                            int saveChk = 0;
                            int DC = 0;
                            int saveChkAdder = 0;
                            if (thisSpellEffect.saveCheckType.Equals("will"))
                            {
                                saveChkAdder = pc.will;
                            }
                            else if (thisSpellEffect.saveCheckType.Equals("reflex"))
                            {
                                saveChkAdder = pc.reflex;
                            }
                            else if (thisSpellEffect.saveCheckType.Equals("fortitude"))
                            {
                                saveChkAdder = pc.fortitude;
                            }
                            else
                            {
                                saveChkAdder = -99;
                            }
                            saveChk = saveChkRoll + saveChkAdder;
                            DC = thisSpellEffect.saveCheckDC;
                            #endregion
                            if (saveChk >= DC) //passed save check
                            {
                                gv.cc.addLogText("<font color='yellow'>" + pc.name + " avoids the " + thisSpellEffect.name + " effect.</font><BR>");
                            }
                            else
                            {
                                int numberOfRounds = thisSpellEffect.durationInUnits / gv.mod.TimePerRound;
                                gv.cc.addLogText("<font color='lime'>" + thisSpellEffect.name + " is applied on " + pc.name + " for " + numberOfRounds + " round(s)</font><BR>");
                                pc.AddEffectByObject(thisSpellEffect, classLevel);
                            }
                            #endregion
                        }
                        if (thisSpell.removeEffectTagList.Count > 0)
                        {
                            #region remove effects  
                            foreach (EffectTagForDropDownList efTag in thisSpell.removeEffectTagList)
                            {
                                for (int x = pc.effectsList.Count - 1; x >= 0; x--)
                                {
                                    if (pc.effectsList[x].tag.Equals(efTag.tag))
                                    {
                                        try
                                        {
                                            pc.effectsList.RemoveAt(x);
                                        }
                                        catch (Exception ex)
                                        {
                                            gv.errorLog(ex.ToString());
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                #endregion
            }
        }

        public void trGeneric(Trait thisTrait, object src, object trg, bool outsideCombat)
        {
            //Effect thisTraitEffect = gv.cc.getEffectByTag(thisTrait.traitEffectTagList[0].tag);

            //set squares list
            CreateAoeSquaresList(src, trg, thisTrait.aoeShape, thisTrait.aoeRadius);

            //set target list
            if (outsideCombat)
            {
                AoeTargetsList.Clear();
                AoeTargetsList.Add(trg);
            }
            else
            {
                if (thisTrait.isUsedForCombatSquareEffect)
                {
                    CreateAoeTargetsList(src, trg, true);
                }
                else
                {
                    CreateAoeTargetsList(src, trg, false);
                }
            }

            //loop through all effects of trait here
            foreach (EffectTagForDropDownList eftag in thisTrait.traitEffectTagList)
            {
                Effect thisTraitEffect = gv.cc.getEffectByTag(eftag.tag);

                #region Get trait using source information
                int classLevel = 0;
                string sourceName = "";
                if (thisTraitEffect == null)
                {
                    gv.sf.MessageBoxHtml("EffectTag: " + eftag.tag + " does not exist in this module. Abort trait use.");
                    return;
                }
                if (src is Player) //player casting
                {
                    Player source = (Player)src;
                    classLevel = source.classLevel;
                    sourceName = source.name;
                    source.sp -= thisTrait.costSP;
                    if (source.sp < 0) { source.sp = 0; }
                }
                else if (src is Creature) //creature casting
                {
                    Creature source = (Creature)src;
                    classLevel = source.cr_level;
                    sourceName = source.cr_name;
                    source.sp -= thisTrait.costSP;
                    if (source.sp < 0) { source.sp = 0; }
                }
                else if (src is Item) //item was used
                {
                    Item source = (Item)src;
                    classLevel = source.levelOfItemForCastSpell;
                    sourceName = source.name;
                }
                else if (src is Coordinate) //trigger or prop was used
                {
                    classLevel = 1;
                    sourceName = "trigger";
                }
                #endregion

                if (thisTrait.isUsedForCombatSquareEffect)
                {
                    #region Iterate over squares and apply effect to them
                    int numberOfRounds = thisTraitEffect.durationInUnits / gv.mod.TimePerRound;
                    gv.cc.addLogText("<gn>" + thisTraitEffect.name + " is applied for " + numberOfRounds + " round(s)</gn><BR>");
                    foreach (object target in AoeTargetsList)
                    {
                        if (target is Coordinate)
                        {
                            Coordinate c = (Coordinate)target;
                            Effect e = thisTraitEffect.DeepCopy();
                            e.combatLocX = c.X;
                            e.combatLocY = c.Y;
                            gv.mod.currentEncounter.AddEffectByObject(e, classLevel);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Iterate over targets and apply the modifiers for damage, heal, buffs and debuffs
                    foreach (object target in AoeTargetsList)
                    {
                        if (target is Creature)
                        {
                            Creature crt = (Creature)target;
                            if (thisTraitEffect.doDamage)
                            {
                                #region Do Damage
                                #region Get Resistances
                                float resist = 0;
                                if (thisTraitEffect.damType.Equals("Normal")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueNormal() / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Acid")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueAcid() / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Cold")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueCold() / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Electricity")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueElectricity() / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Fire")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueFire() / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Magic")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValueMagic() / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Poison")) { resist = (float)(1f - ((float)crt.getterDamageTypeResistanceValuePoison() / 100f)); }
                                #endregion
                                int damageTotal = 0;
                                #region Calculate Number of Attacks
                                //(for reference) NumOfAttacks: A of these attacks for every B levels after level C up to D attacks total                    
                                int numberOfAttacks = 0;
                                if (thisTraitEffect.damNumberOfAttacksForEveryNLevels == 0) //this effect is using a fixed amount of attacks
                                {
                                    numberOfAttacks = thisTraitEffect.damNumberOfAttacks;
                                }
                                else //this effect is using a variable amount of attacks
                                {
                                    //numberOfAttacks = (((classLevel - C) / B) + 1) * A;
                                    numberOfAttacks = (((classLevel - thisTraitEffect.damNumberOfAttacksAfterLevelN) / thisTraitEffect.damNumberOfAttacksForEveryNLevels) + 1) * thisTraitEffect.damNumberOfAttacks; //ex: 1 bolt for every 2 levels after level 1
                                    if (numberOfAttacks > thisTraitEffect.damNumberOfAttacksUpToNAttacksTotal) { numberOfAttacks = thisTraitEffect.damNumberOfAttacksUpToNAttacksTotal; } //can't have more than a max amount of attacks
                                }

                                #endregion
                                //loop over number of attacks
                                for (int i = 0; i < numberOfAttacks; i++)
                                {
                                    #region Calculate Damage
                                    //(for reference) Attack: AdB+C for every D levels after level E up to F levels total
                                    // damage += RandDieRoll(A,B) + C
                                    //int damage = (int)((1 * RandInt(4) + 1) * resist);
                                    int damage = 0;
                                    if (thisTraitEffect.damAttacksEveryNLevels == 0) //this damage is not level based
                                    {
                                        damage = RandDiceRoll(thisTraitEffect.damNumOfDice, thisTraitEffect.damDie) + thisTraitEffect.damAdder;
                                    }
                                    else //this damage is level based
                                    {
                                        int numberOfDamAttacks = ((classLevel - thisTraitEffect.damAttacksAfterLevelN) / thisTraitEffect.damAttacksEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                                        if (numberOfDamAttacks > thisTraitEffect.damAttacksUpToNLevelsTotal) { numberOfDamAttacks = thisTraitEffect.damAttacksUpToNLevelsTotal; } //can't have more than a max amount of attacks
                                        for (int j = 0; j < numberOfDamAttacks; j++)
                                        {
                                            damage += RandDiceRoll(thisTraitEffect.damNumOfDice, thisTraitEffect.damDie) + thisTraitEffect.damAdder;
                                        }
                                    }
                                    #endregion
                                    #region Do Calc Save and DC
                                    int saveChkRoll = RandInt(20);
                                    int saveChk = 0;
                                    int DC = 0;
                                    int saveChkAdder = 0;
                                    if (thisTraitEffect.saveCheckType.Equals("will"))
                                    {
                                        saveChkAdder = crt.getterWill();
                                    }
                                    else if (thisTraitEffect.saveCheckType.Equals("reflex"))
                                    {
                                        saveChkAdder = crt.getterReflex();
                                    }
                                    else if (thisTraitEffect.saveCheckType.Equals("fortitude"))
                                    {
                                        saveChkAdder = crt.getterFortitude();
                                    }
                                    else
                                    {
                                        saveChkAdder = -99;
                                    }
                                    saveChk = saveChkRoll + saveChkAdder;
                                    DC = thisTraitEffect.saveCheckDC;
                                    #endregion
                                    if (saveChk >= DC) //passed save check (do half or avoid all?)
                                    {
                                        damage = damage / 2;
                                        gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " evades most of the " + thisTraitEffect.name + "</font><BR>");
                                        if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + saveChkAdder + " >= " + DC + "</font><BR>"); }
                                    }
                                    if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + "</font><BR>"); }
                                    int damageAndResist = (int)((float)damage * resist);
                                    damageTotal += damageAndResist;
                                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " damages " + "</font>" + "<font color='silver'>"
                                                    + crt.cr_name + "</font>" + "<font color='white'>" + "with " + thisTraitEffect.name + " (" + "</font>" + "<font color='lime'>"
                                                    + damageAndResist + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                                }
                                crt.hp -= damageTotal;
                                if (crt.hp <= 0)
                                {
                                    //gv.screenCombat.deathAnimationLocations.Add(new Coordinate(crt.combatLocX, crt.combatLocY));
                                    foreach (Coordinate coor in crt.tokenCoveredSquares)
                                    {
                                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(coor.X, coor.Y));
                                    }
                                    gv.cc.addLogText("<font color='lime'>" + "You killed the " + crt.cr_name + "</font><BR>");
                                }
                                //Do floaty text damage
                                //gv.screenCombat.floatyTextOn = true;
                                gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), damageTotal + "");
                                #endregion
                            }
                            if (thisTraitEffect.doHeal)
                            {
                                if (src is Player) //PCs shouldn't heal creatures
                                {
                                    continue;
                                }
                                #region Do Heal
                                #region Calculate Heal
                                //(for reference) Heal: AdB+C for every D levels after level E up to F levels total
                                // heal += RandDieRoll(A,B) + C
                                int heal = 0;
                                if (thisTraitEffect.healActionsEveryNLevels == 0) //this heal is not level based
                                {
                                    heal = RandDiceRoll(thisTraitEffect.healNumOfDice, thisTraitEffect.healDie) + thisTraitEffect.healAdder;
                                }
                                else //this heal is level based
                                {
                                    int numberOfHealActions = ((classLevel - thisTraitEffect.healActionsAfterLevelN) / thisTraitEffect.healActionsEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                                    if (numberOfHealActions > thisTraitEffect.healActionsUpToNLevelsTotal) { numberOfHealActions = thisTraitEffect.healActionsUpToNLevelsTotal; } //can't have more than a max amount of actions
                                    for (int j = 0; j < numberOfHealActions; j++)
                                    {
                                        heal += RandDiceRoll(thisTraitEffect.healNumOfDice, thisTraitEffect.healDie) + thisTraitEffect.healAdder;
                                    }
                                }
                                #endregion
                                if (thisTraitEffect.healHP)
                                {
                                    crt.hp += heal;
                                    if (crt.hp > crt.hpMax)
                                    {
                                        crt.hp = crt.hpMax;
                                    }
                                    gv.cc.addLogText("<font color='lime'>" + crt.cr_name + " gains " + heal + " HPs" + "</font><BR>");
                                    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), heal + "", "green");
                                }
                                else
                                {
                                    crt.sp += heal;
                                    gv.cc.addLogText("<font color='lime'>" + crt.cr_name + " gains " + heal + " SPs" + "</font><BR>");
                                    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), heal + "", "yellow");
                                }
                                //Do floaty text heal
                                //gv.screenCombat.floatyTextOn = true;
                                
                                #endregion
                            }
                            if (thisTraitEffect.doBuff)
                            {
                                if (src is Player) //PCs shouldn't buff creatures
                                {
                                    continue;
                                }
                                #region Do Buff
                                int numberOfRounds = thisTraitEffect.durationInUnits / gv.mod.TimePerRound;
                                gv.cc.addLogText("<font color='lime'>" + thisTraitEffect.name + " is applied on " + crt.cr_name + " for " + numberOfRounds + " round(s)</font><BR>");
                                crt.AddEffectByObject(thisTraitEffect, classLevel);
                                #endregion
                            }
                            if (thisTraitEffect.doDeBuff)
                            {
                                #region Do DeBuff
                                #region Do Calc Save and DC
                                int saveChkRoll = RandInt(20);
                                int saveChk = 0;
                                int DC = 0;
                                int saveChkAdder = 0;
                                if (thisTraitEffect.saveCheckType.Equals("will"))
                                {
                                    saveChkAdder = crt.getterWill();
                                }
                                else if (thisTraitEffect.saveCheckType.Equals("reflex"))
                                {
                                    saveChkAdder = crt.getterReflex();
                                }
                                else if (thisTraitEffect.saveCheckType.Equals("fortitude"))
                                {
                                    saveChkAdder = crt.getterFortitude();
                                }
                                else
                                {
                                    saveChkAdder = -99;
                                }
                                saveChk = saveChkRoll + saveChkAdder;
                                DC = thisTraitEffect.saveCheckDC;
                                #endregion
                                if (saveChk >= DC) //passed save check
                                {
                                    gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " avoids the " + thisTraitEffect.name + " effect.</font><BR>");
                                }
                                else
                                {
                                    int numberOfRounds = thisTraitEffect.durationInUnits / gv.mod.TimePerRound;
                                    gv.cc.addLogText("<font color='lime'>" + thisTraitEffect.name + " is applied on " + crt.cr_name + " for " + numberOfRounds + " round(s)</font><BR>");
                                    crt.AddEffectByObject(thisTraitEffect, classLevel);
                                }
                                #endregion
                            }
                            if (thisTrait.removeEffectTagList.Count > 0)
                            {
                                #region remove effects  
                                foreach (EffectTagForDropDownList efTag in thisTrait.removeEffectTagList)
                                {
                                    for (int x = crt.cr_effectsList.Count - 1; x >= 0; x--)
                                    {
                                        if (crt.cr_effectsList[x].tag.Equals(efTag.tag))
                                        {
                                            try
                                            {
                                                crt.cr_effectsList.RemoveAt(x);
                                            }
                                            catch (Exception ex)
                                            {
                                                gv.errorLog(ex.ToString());
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        else //target is Player
                        {
                            Player pc = (Player)target;
                            if (thisTraitEffect.doDamage)
                            {
                                #region Do Damage
                                #region Get Resistances
                                float resistPc = 0;
                                if (thisTraitEffect.damType.Equals("Normal")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalNormal / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Acid")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalAcid / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Cold")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalCold / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Electricity")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalElectricity / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Fire")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalFire / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Magic")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalMagic / 100f)); }
                                else if (thisTraitEffect.damType.Equals("Poison")) { resistPc = (float)(1f - ((float)pc.damageTypeResistanceTotalPoison / 100f)); }
                                #endregion
                                int damageTotal = 0;
                                #region Calculate Number of Attacks
                                //(for reference) NumOfAttacks: A of these attacks for every B levels after level C up to D attacks total                    
                                int numberOfAttacks = 0;
                                if (thisTraitEffect.damNumberOfAttacksForEveryNLevels == 0) //this effect is using a fixed amount of attacks
                                {
                                    numberOfAttacks = thisTraitEffect.damNumberOfAttacks;
                                }
                                else //this effect is using a variable amount of attacks
                                {
                                    //numberOfAttacks = (((classLevel - C) / B) + 1) * A;
                                    numberOfAttacks = (((classLevel - thisTraitEffect.damNumberOfAttacksAfterLevelN) / thisTraitEffect.damNumberOfAttacksForEveryNLevels) + 1) * thisTraitEffect.damNumberOfAttacks; //ex: 1 bolt for every 2 levels after level 1
                                }
                                if (numberOfAttacks > thisTraitEffect.damNumberOfAttacksUpToNAttacksTotal) { numberOfAttacks = thisTraitEffect.damNumberOfAttacksUpToNAttacksTotal; } //can't have more than a max amount of attacks
                                #endregion
                                //loop over number of attacks
                                for (int i = 0; i < numberOfAttacks; i++)
                                {
                                    #region Calculate Damage
                                    //(for reference) Attack: AdB+C for every D levels after level E up to F levels total
                                    // damage += RandDieRoll(A,B) + C
                                    //int damage = (int)((1 * RandInt(4) + 1) * resist);
                                    int damagePc = 0;
                                    if (thisTraitEffect.damAttacksEveryNLevels == 0) //this damage is not level based
                                    {
                                        damagePc = RandDiceRoll(thisTraitEffect.damNumOfDice, thisTraitEffect.damDie) + thisTraitEffect.damAdder;
                                    }
                                    else //this damage is level based
                                    {
                                        int numberOfDamAttacks = ((classLevel - thisTraitEffect.damAttacksAfterLevelN) / thisTraitEffect.damAttacksEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                                        if (numberOfDamAttacks > thisTraitEffect.damAttacksUpToNLevelsTotal) { numberOfDamAttacks = thisTraitEffect.damAttacksUpToNLevelsTotal; } //can't have more than a max amount of attacks
                                        for (int j = 0; j < numberOfDamAttacks; j++)
                                        {
                                            damagePc += RandDiceRoll(thisTraitEffect.damNumOfDice, thisTraitEffect.damDie) + thisTraitEffect.damAdder;
                                        }
                                    }
                                    #endregion
                                    #region Do Calc Save and DC
                                    int saveChkRollPc = RandInt(20);
                                    int saveChkPc = 0;
                                    int DCPc = 0;
                                    int saveChkAdder = 0;
                                    if (thisTraitEffect.saveCheckType.Equals("will"))
                                    {
                                        saveChkAdder = pc.will;
                                    }
                                    else if (thisTraitEffect.saveCheckType.Equals("reflex"))
                                    {
                                        saveChkAdder = pc.reflex;
                                    }
                                    else if (thisTraitEffect.saveCheckType.Equals("fortitude"))
                                    {
                                        saveChkAdder = pc.fortitude;
                                    }
                                    else
                                    {
                                        saveChkAdder = -99;
                                    }
                                    saveChkPc = saveChkRollPc + saveChkAdder;
                                    DCPc = thisTraitEffect.saveCheckDC;
                                    #endregion
                                    if (saveChkPc >= DCPc) //passed save check (do half or avoid all?)
                                    {
                                        damagePc = damagePc / 2;
                                        gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades most of the " + thisTraitEffect.name + "</font><BR>");
                                        if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + saveChkRollPc + " + " + saveChkAdder + " >= " + DCPc + "</font><BR>"); }
                                    }
                                    if (mod.debugMode) { gv.cc.addLogText("<font color='yellow'>" + "resist = " + resistPc + " damage = " + damagePc + "</font><BR>"); }
                                    int damageAndResist = (int)((float)damagePc * resistPc);
                                    damageTotal += damageAndResist;
                                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " damages " + "</font>" + "<font color='silver'>"
                                                    + pc.name + "</font>" + "<font color='white'>" + "with " + thisTraitEffect.name + " (" + "</font>" + "<font color='lime'>"
                                                    + damageAndResist + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                                }
                                pc.hp -= damageTotal;
                                if (pc.hp <= 0)
                                {
                                    gv.screenCombat.deathAnimationLocations.Add(new Coordinate(pc.combatLocX, pc.combatLocY));
                                    gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                                    pc.charStatus = "Dead";
                                }
                                //Do floaty text damage
                                //gv.screenCombat.floatyTextOn = true;
                                gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), damageTotal + "");
                                #endregion
                            }
                            if (thisTraitEffect.doHeal)
                            {
                                if (src is Creature) //Creatures shouldn't heal PCs
                                {
                                    continue;
                                }
                                #region Do Heal
                                if (pc.hp <= -20)
                                {
                                    //MessageBox("Can't heal a dead character!");
                                    gv.cc.addLogText("<font color='red'>" + "Can't heal a dead character!" + "</font><BR>");
                                }
                                else
                                {
                                    #region Calculate Heal
                                    //(for reference) Heal: AdB+C for every D levels after level E up to F levels total
                                    // heal += RandDieRoll(A,B) + C
                                    int heal = 0;
                                    if (thisTraitEffect.healActionsEveryNLevels == 0) //this heal is not level based
                                    {
                                        heal = RandDiceRoll(thisTraitEffect.healNumOfDice, thisTraitEffect.healDie) + thisTraitEffect.healAdder;
                                    }
                                    else //this heal is level based
                                    {
                                        int numberOfHealActions = ((classLevel - thisTraitEffect.healActionsAfterLevelN) / thisTraitEffect.healActionsEveryNLevels) + 1; //ex: 1 bolt for every 2 levels after level 1
                                        if (numberOfHealActions > thisTraitEffect.healActionsUpToNLevelsTotal) { numberOfHealActions = thisTraitEffect.healActionsUpToNLevelsTotal; } //can't have more than a max amount of actions
                                        for (int j = 0; j < numberOfHealActions; j++)
                                        {
                                            heal += RandDiceRoll(thisTraitEffect.healNumOfDice, thisTraitEffect.healDie) + thisTraitEffect.healAdder;
                                        }
                                    }
                                    #endregion
                                    if (thisTraitEffect.healHP)
                                    {
                                        pc.hp += heal;
                                        if (pc.hp > pc.hpMax)
                                        {
                                            pc.hp = pc.hpMax;
                                        }
                                        if (pc.hp > 0)
                                        {
                                            pc.charStatus = "Alive";
                                        }
                                        gv.cc.addLogText("<font color='lime'>" + pc.name + " gains " + heal + " HPs" + "</font><BR>");
                                        gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), heal + "", "green");
                                    }
                                    else
                                    {
                                        pc.sp += heal;
                                        if (pc.sp > pc.spMax)
                                        {
                                            pc.sp = pc.spMax;
                                        }
                                        gv.cc.addLogText("<font color='lime'>" + pc.name + " gains " + heal + " SPs" + "</font><BR>");
                                        gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), heal + "", "yellow");
                                    }
                                    //Do floaty text heal
                                    //gv.screenCombat.floatyTextOn = true;
                                    
                                }
                                #endregion
                            }
                            if (thisTraitEffect.doBuff)
                            {
                                if (src is Creature) //Creatures shouldn't buff PCs
                                {
                                    continue;
                                }
                                #region Do Buff
                                int numberOfRounds = thisTraitEffect.durationInUnits / gv.mod.TimePerRound;
                                gv.cc.addLogText("<font color='lime'>" + thisTraitEffect.name + " is applied on " + pc.name + " for " + numberOfRounds + " round(s)</font><BR>");
                                pc.AddEffectByObject(thisTraitEffect, classLevel);
                                #endregion
                            }
                            if (thisTraitEffect.doDeBuff)
                            {
                                #region Do DeBuff
                                #region Do Calc Save and DC
                                int saveChkRoll = RandInt(20);
                                int saveChk = 0;
                                int DC = 0;
                                int saveChkAdder = 0;
                                if (thisTraitEffect.saveCheckType.Equals("will"))
                                {
                                    saveChkAdder = pc.will;
                                }
                                else if (thisTraitEffect.saveCheckType.Equals("reflex"))
                                {
                                    saveChkAdder = pc.reflex;
                                }
                                else if (thisTraitEffect.saveCheckType.Equals("fortitude"))
                                {
                                    saveChkAdder = pc.fortitude;
                                }
                                else
                                {
                                    saveChkAdder = -99;
                                }
                                saveChk = saveChkRoll + saveChkAdder;
                                DC = thisTraitEffect.saveCheckDC;
                                #endregion
                                if (saveChk >= DC) //passed save check
                                {
                                    gv.cc.addLogText("<font color='yellow'>" + pc.name + " avoids the " + thisTraitEffect.name + " effect.</font><BR>");
                                }
                                else
                                {
                                    int numberOfRounds = thisTraitEffect.durationInUnits / gv.mod.TimePerRound;
                                    gv.cc.addLogText("<font color='lime'>" + thisTraitEffect.name + " is applied on " + pc.name + " for " + numberOfRounds + " round(s)</font><BR>");
                                    pc.AddEffectByObject(thisTraitEffect, classLevel);
                                }
                                #endregion
                            }
                            if (thisTrait.removeEffectTagList.Count > 0)
                            {
                                #region remove effects  
                                foreach (EffectTagForDropDownList efTag in thisTrait.removeEffectTagList)
                                {
                                    for (int x = pc.effectsList.Count - 1; x >= 0; x--)
                                    {
                                        if (pc.effectsList[x].tag.Equals(efTag.tag))
                                        {
                                            try
                                            {
                                                pc.effectsList.RemoveAt(x);
                                            }
                                            catch (Exception ex)
                                            {
                                                gv.errorLog(ex.ToString());
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
            }
        }

        //SPELLS WIZARD
        public void spDimensionDoor(object src, object trg)
        {
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                Coordinate target = (Coordinate)trg;

                if (IsSquareOpen(target))
                {
                    gv.cc.addLogText("<gn>" + source.name + " teleports to another location</gn><BR>");
                    source.combatLocX = target.X;
                    source.combatLocY = target.Y;
                    source.sp -= gv.cc.currentSelectedSpell.costSP;
                    if (source.sp < 0) { source.sp = 0; }
                }
                else
                {
                    gv.cc.addLogText("<yl>" + source.name + " fails to teleport, square is already occupied or not valid</yl><BR>");
                }
            }
            else if (src is Creature) //creature casting
            {
                Creature source = (Creature)src;
                Coordinate target = (Coordinate)trg;

                if (IsSquareOpen(target))
                {
                    gv.cc.addLogText("<gn>" + source.cr_name + " teleports to another location</gn><BR>");
                    source.combatLocX = target.X;
                    source.combatLocY = target.Y;
                    source.sp -= SpellToCast.costSP;
                    if (source.sp < 0) { source.sp = 0; }
                }
                else
                {
                    gv.cc.addLogText("<yl>" + source.cr_name + " fails to teleport, square is already occupied or not valid</yl><BR>");
                }
            }
        }
        public void trRemoveTrap(object src, object trg)
        {
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                Coordinate target = (Coordinate)trg;

                foreach(Prop prp in gv.mod.currentEncounter.propsList)
                {
                    if ((prp.LocationX == target.X) && (prp.LocationY == target.Y))
                    {
                        if (prp.isTrap)
                        {
                            gv.mod.currentEncounter.propsList.Remove(prp);
                            gv.cc.addLogText("<gn>" + source.name + " removed trap</gn><BR>");
                            gv.cc.addFloatyText(new Coordinate(target.X, target.X), "trap removed", "green");
                            return;
                        }
                    }
                }
            }            
        }
        public bool IsSquareOpen(Coordinate target)
        {
            if (gv.mod.currentEncounter.Walkable[target.Y * mod.currentEncounter.MapSizeX + target.X] == 0)
            {
                return false;
            }
            foreach (Player pc in gv.mod.playerList)
            {
                if ((pc.combatLocX == target.X) && (pc.combatLocY == target.Y))
                {
                    if (pc.isAlive())
                    {
                        return false;
                    }
                }
            }
            foreach (Creature crt in gv.mod.currentEncounter.encounterCreatureList)
            {
                if ((crt.combatLocX == target.X) && (crt.combatLocY == target.Y))
                {
                    return false;
                }
            }
            return true;
        }

        /*        
        public void spFlameFingers(object src, object trg, Spell thisSpell)
        {
            //set squares list
            CreateAoeSquaresList(src, trg, thisSpell.aoeShape, thisSpell.aoeRadius);
            
            //set target list
            CreateAoeTargetsList(src, trg);
            
            //get casting source information
            int classLevel = 0;
            string sourceName = "";
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                classLevel = source.classLevel;
                sourceName = source.name;
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else //creature casting
            {
                Creature source = (Creature)src;
                classLevel = source.cr_level;
                sourceName = source.cr_name;
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            
            //iterate over targets and do damage
            foreach (object target in AoeTargetsList)
            {
                if (target is Creature)
                {
                    Creature crt = (Creature)target;
                    float resist = (float)(1f - ((float)crt.damageTypeResistanceValueFire / 100f));
                    float damage = classLevel * RandInt(3);
                    int fireDam = (int)(damage * resist);
                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + crt.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        fireDam = fireDam / 2;
                        gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " evades most of the Flame Fingers spell" + "</font><BR>");
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + crt.reflex + " >= " + DC + "</font><BR>");
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + " fireDam = " + fireDam + "</font><BR>");
                    }
                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " scorches " + "</font>" + "<font color='silver'>" + crt.cr_name + "</font><BR>");
                    gv.cc.addLogText("<font color='white'>" + "with Flame Fingers (" + "</font>" + "<font color='lime'>" + fireDam + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    crt.hp -= fireDam;
                    if (crt.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(crt.combatLocX, crt.combatLocY));
                        gv.cc.addLogText("<font color='lime'>" + "You killed the " + crt.cr_name + "</font><BR>");
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), fireDam + "");                    
                }
                else //target is Player
                {
                    Player pc = (Player)target;
                    float resist = (float)(1f - ((float)pc.damageTypeResistanceTotalFire / 100f));
                    float damage = classLevel * RandInt(3);
                    int fireDam = (int)(damage * resist);
                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + pc.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        if (this.hasTrait(pc, "evasion"))
                        {
                            fireDam = 0;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades all of the Flame Fingers spell" + "</font><BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                            }
                        }
                        else
                        {
                            fireDam = fireDam / 2;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades most of the Flame Fingers spell" + "</font><BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                            }
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + " fireDam = " + fireDam + "</font><BR>");
                    }
                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " scorches " + "</font>" + "<font color='silver'>" + pc.name + "</font><BR>");
                    gv.cc.addLogText("<font color='white'>" + "with Flame Fingers (" + "</font>" + "<font color='lime'>" + fireDam + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    pc.hp -= fireDam;
                    if (pc.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(pc.combatLocX, pc.combatLocY));
                        gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                        pc.charStatus = "Dead";
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), fireDam + "");
                }
            }
            
            //remove dead creatures            

//            gv.postDelayed("doFloatyText", 100);
        }
        public void spMageBolt(object src, object trg)
        {
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                Creature target = (Creature)trg;

                int damageTotal = 0;
                int numberOfBolts = ((source.classLevel - 1) / 2) + 1; //1 bolt for every 2 levels after level 1
                if (numberOfBolts > 5) { numberOfBolts = 5; } //can not have more than 5 bolts
                for (int i = 0; i < numberOfBolts; i++)
                {
                    int damage = 1 * RandInt(4) + 1;
                    target.hp = target.hp - damage;
                    damageTotal += damage;
                    gv.cc.addLogText("<font color='aqua'>" + source.name + "</font>" +
                            "<font color='white'>" + " attacks " + "</font>" +
                            "<font color='silver'>" + target.cr_name + "</font>" +
                            "<BR>");
                    gv.cc.addLogText("<font color='white'>" + "Mage Bolt HITS (" + "</font>" +
                            "<font color='lime'>" + damage + "</font>" +
                            "<font color='white'>" + " damage)" + "</font>" +
                            "<BR>");
                    if (target.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(target.combatLocX, target.combatLocY));
                        gv.cc.addLogText("<font color='lime'>" + "You killed the " + target.cr_name + "</font><BR>");
                    }
                }
                //Do floaty text damage
                //gv.screenCombat.floatyTextOn = true;
                gv.cc.addFloatyText(new Coordinate(target.combatLocX, target.combatLocY), damageTotal + "");
//                gv.postDelayed("doFloatyText", 100);

                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Creature) //creature casting
            {
                Creature source = (Creature)src;
                Player target = (Player)trg;

                int damageTotal = 0;
                int numberOfBolts = ((source.cr_level - 1) / 2) + 1; //1 bolt for every 2 levels after level 1
                if (numberOfBolts > 5) { numberOfBolts = 5; } //can not have more than 5 bolts
                for (int i = 0; i < numberOfBolts; i++)
                {
                    int damage = 1 * RandInt(4) + 1;

                    target.hp = target.hp - damage;
                    damageTotal += damage;
                    gv.cc.addLogText("<font color='silver'>" + source.cr_name + "</font>" +
                            "<font color='white'>" + " attacks " + "</font>" +
                            "<font color='aqua'>" + target.name + "</font>" +
                            "<BR>");
                    gv.cc.addLogText("<font color='white'>" + "Mage Bolt HITS (" + "</font>" +
                            "<font color='red'>" + damage + "</font>" +
                            "<font color='white'>" + " damage)" + "</font>" +
                            "<BR>");
                    if (target.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(target.combatLocX, target.combatLocY));
                        gv.cc.addLogText("<font color='red'>" + target.name + " drops unconcious!" + "</font><BR>");
                        target.charStatus = "Dead";
                    }
                }
                //Do floaty text damage
                //gv.screenCombat.floatyTextOn = true;
                gv.cc.addFloatyText(new Coordinate(target.combatLocX, target.combatLocY), damageTotal + "");
//                gv.postDelayed("doFloatyText", 100);

                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Coordinate)
            {
                //Toast.makeText(gv.gameContext, "target is not a PC or Creature", Toast.LENGTH_SHORT).show();
            }
            else
            {
                //Toast.makeText(gv.gameContext, "don't recognize target type", Toast.LENGTH_SHORT).show();			
            }
        }
        public void spSleep(object src, object trg, Spell thisSpell)
        {
            //set squares list
            CreateAoeSquaresList(src, trg, thisSpell.aoeShape, thisSpell.aoeRadius);

            //set target list
            CreateAoeTargetsList(src, trg);

            //get casting source information
            int classLevel = 0;
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                classLevel = source.classLevel;
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else //creature casting
            {
                Creature source = (Creature)src;
                classLevel = source.cr_level;
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }

            //iterate over targets and do damage
            foreach (object target in AoeTargetsList)
            {
                if (target is Creature)
                {
                    Creature crt = (Creature)target;
                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + crt.will;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " avoids the sleep spell" + "</font><BR>");
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + crt.will + " >= " + DC + "</font><BR>");
                        }
                    }
                    else //failed check
                    {
                        gv.cc.addLogText("<font color='red'>" + crt.cr_name + " is held by a sleep spell" + "</font><BR>");
                        crt.cr_status = "Held";
                        Effect ef = mod.getEffectByTag("sleep");
                        crt.AddEffectByObject(ef, classLevel);
                    }
                }
                else //target is Player
                {
                    Player pc = (Player)target;
                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + pc.will;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        gv.cc.addLogText("<font color='yellow'>" + pc.name + " avoids the sleep spell" + "</font><BR>");
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.will + " >= " + DC + "</font><BR>");
                        }
                    }
                    else //failed check
                    {
                        gv.cc.addLogText("<font color='red'>" + pc.name + " is held by a sleep spell" + "</font><BR>");
                        pc.charStatus = "Held";
                        Effect ef = mod.getEffectByTag("sleep");
                        pc.AddEffectByObject(ef, classLevel);
                    }
                }
            }
        }
        public void spMageArmor(object src, object trg)
        {
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                Player target = (Player)trg;

                if (source != target)
                {
                    MessageBox("Mage Armor can only be applied to the caster...casting aborted");
                    return;
                }

                int numberOfRounds = (source.classLevel * 20); //20 rounds per level
                Effect ef = mod.getEffectByTag("mageArmor").DeepCopy();
                ef.durationInUnits = numberOfRounds * gv.mod.TimePerRound;
                gv.cc.addLogText("<font color='lime'>" + "Mage Armor is applied on " + target.name + "<BR>");
                gv.cc.addLogText("<font color='lime'>" + " for " + numberOfRounds + " round(s)" + "</font><BR>");
                target.AddEffectByObject(ef, source.classLevel);
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Creature) //creature casting
            {
                Creature source = (Creature)src;
                Creature target = (Creature)trg;

                int numberOfRounds = (source.cr_level * 20); //20 rounds per level
                Effect ef = mod.getEffectByTag("mageArmor").DeepCopy();
                ef.durationInUnits = numberOfRounds * gv.mod.TimePerRound;
                gv.cc.addLogText("<font color='lime'>" + "Mage Armor is applied on " + target.cr_name + "<BR>");
                gv.cc.addLogText("<font color='lime'>" + " for " + numberOfRounds + " round(s)" + "</font><BR>");
                target.AddEffectByObject(ef, source.cr_level);
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Coordinate)
            {
                //Toast.makeText(gv.gameContext, "source is not a PC or Creature", Toast.LENGTH_SHORT).show();
            }
            else
            {
                //Toast.makeText(gv.gameContext, "don't recognize source type", Toast.LENGTH_SHORT).show();			
            }
        }
        public void spMinorRegen(object src, object trg)
        {
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                Player target = (Player)trg;

                Effect ef = mod.getEffectByTag("minorRegen");
                gv.cc.addLogText("<font color='lime'>" + "Minor Regeneration is applied on " + target.name + "</font><BR>");
                target.AddEffectByObject(ef, source.classLevel);
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Creature) //creature casting
            {
                Creature source = (Creature)src;
                Creature target = (Creature)trg;

                Effect ef = mod.getEffectByTag("minorRegen");
                gv.cc.addLogText("<font color='lime'>" + "Minor Regeneration is applied on " + target.cr_name + "</font><BR>");
                target.AddEffectByObject(ef, source.cr_level);
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Coordinate)
            {
                //Toast.makeText(gv.gameContext, "source is not a PC or Creature", Toast.LENGTH_SHORT).show();
            }
            else
            {
                //Toast.makeText(gv.gameContext, "don't recognize source type", Toast.LENGTH_SHORT).show();			
            }
        }
        public void spWeb(object src, object trg, Spell thisSpell)
        {
            //set squares list
            CreateAoeSquaresList(src, trg, thisSpell.aoeShape, thisSpell.aoeRadius);

            //set target list
            CreateAoeTargetsList(src, trg);

            //get casting source information
            int classLevel = 0;
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                classLevel = source.classLevel;
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else //creature casting
            {
                Creature source = (Creature)src;
                classLevel = source.cr_level;
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }

            //iterate over targets and do damage
            foreach (object target in AoeTargetsList)
            {
                if (target is Creature)
                {
                    Creature crt = (Creature)target;
                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + crt.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " avoids the web spell" + "</font><BR>");
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + crt.reflex + " >= " + DC + "</font><BR>");
                        }
                    }
                    else //failed check
                    {
                        gv.cc.addLogText("<font color='red'>" + crt.cr_name + " is held by a web spell" + "</font><BR>");
                        crt.cr_status = "Held";
                        Effect ef = mod.getEffectByTag("web");
                        crt.AddEffectByObject(ef, classLevel);
                    }
                }
                else //target is Player
                {
                    Player pc = (Player)target;
                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + pc.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        gv.cc.addLogText("<font color='yellow'>" + pc.name + " avoids the web spell" + "</font><BR>");
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                        }
                    }
                    else //failed check
                    {
                        gv.cc.addLogText("<font color='red'>" + pc.name + " is held by a web spell" + "</font><BR>");
                        pc.charStatus = "Held";
                        Effect ef = mod.getEffectByTag("web");
                        pc.AddEffectByObject(ef, classLevel);
                    }
                }
            }
        }
        public void spIceStorm(object src, object trg, Spell thisSpell)
        {
            //set squares list
            CreateAoeSquaresList(src, trg, thisSpell.aoeShape, thisSpell.aoeRadius);

            //set target list
            CreateAoeTargetsList(src, trg);

            //get casting source information
            int classLevel = 0;
            string sourceName = "";
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                classLevel = source.classLevel;
                sourceName = source.name;
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else //creature casting
            {
                Creature source = (Creature)src;
                classLevel = source.cr_level;
                sourceName = source.cr_name;
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }

            //iterate over targets and do damage
            foreach (object target in AoeTargetsList)
            {
                if (target is Creature)
                {
                    Creature crt = (Creature)target;
                    float resist = (float)(1f - ((float)crt.damageTypeResistanceValueCold / 100f));
                    float damage = classLevel * RandInt(3);
                    int iceDam = (int)(damage * resist);

                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + crt.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        iceDam = iceDam / 2;
                        gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " evades most of the Ice Storm spell" + "</font><BR>");
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + crt.reflex + " >= " + DC + "</font><BR>");
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + " iceDam = " + iceDam + "</font><BR>");
                    }
                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " attacks " + "</font>" + "<font color='silver'>" + crt.cr_name + "</font><BR>");
                    gv.cc.addLogText("<font color='white'>" + "Ice Storm (" + "</font>" + "<font color='lime'>" + iceDam + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    crt.hp -= iceDam;
                    if (crt.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(crt.combatLocX, crt.combatLocY));
                        gv.cc.addLogText("<font color='lime'>" + "You killed the " + crt.cr_name + "</font><BR>");
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), iceDam + "");
                }
                else //target is Player
                {
                    Player pc = (Player)target;
                    float resist = (float)(1f - ((float)pc.damageTypeResistanceTotalCold / 100f));
                    float damage = classLevel * RandInt(3);
                    int iceDam = (int)(damage * resist);

                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + pc.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        if (this.hasTrait(pc, "evasion"))
                        {
                            iceDam = 0;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades all of the Ice Storm spell" + "</font><BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                            }
                        }
                        else
                        {
                            iceDam = iceDam / 2;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades most of the Ice Storm spell" + "</font><BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                            }
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + " iceDam = " + iceDam + "</font><BR>");
                    }
                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " attacks " + "</font>" + "<font color='silver'>" + pc.name + "</font><BR>");
                    gv.cc.addLogText("<font color='white'>" + "Ice Storm (" + "</font>" + "<font color='lime'>" + iceDam + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    pc.hp -= iceDam;
                    if (pc.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(pc.combatLocX, pc.combatLocY));
                        gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                        pc.charStatus = "Dead";
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), iceDam + "");                    
                }
            }

            //remove dead creatures            

//            gv.postDelayed("doFloatyText", 100);
        }
        public void spFireball(object src, object trg, Spell thisSpell)
        {
            //set squares list
            CreateAoeSquaresList(src, trg, thisSpell.aoeShape, thisSpell.aoeRadius);

            //set target list
            CreateAoeTargetsList(src, trg);

            //get casting source information
            int classLevel = 0;
            string sourceName = "";
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                classLevel = source.classLevel;
                sourceName = source.name;
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else //creature casting
            {
                Creature source = (Creature)src;
                classLevel = source.cr_level;
                sourceName = source.cr_name;
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }

            //iterate over targets and do damage
            foreach (object target in AoeTargetsList)
            {
                if (target is Creature)
                {
                    Creature crt = (Creature)target;
                    float resist = (float)(1f - ((float)crt.damageTypeResistanceValueFire / 100f));
                    float damage = classLevel * RandInt(6);
                    int fireDam = (int)(damage * resist);

                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + crt.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        fireDam = fireDam / 2;
                        gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " evades most of the Fireball spell" + "</font><BR>");
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + crt.reflex + " >= " + DC + "</font><BR>");
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + " fireDam = " + fireDam + "</font><BR>");
                    }
                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " attacks " + "</font>" + "<font color='silver'>" + crt.cr_name + "</font><BR>");
                    gv.cc.addLogText("<font color='white'>" + "Fireball (" + "</font>" + "<font color='lime'>" + fireDam + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    crt.hp -= fireDam;
                    if (crt.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(crt.combatLocX, crt.combatLocY));
                        gv.cc.addLogText("<font color='lime'>" + "You killed the " + crt.cr_name + "</font><BR>");
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), fireDam + "");
                }
                else //target is Player
                {
                    Player pc = (Player)target;
                    float resist = (float)(1f - ((float)pc.damageTypeResistanceTotalFire / 100f));
                    float damage = classLevel * RandInt(6);
                    int fireDam = (int)(damage * resist);

                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + pc.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        if (this.hasTrait(pc, "evasion"))
                        {
                            fireDam = 0;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades all of the Fireball spell" + "</font><BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                            }
                        }
                        else
                        {
                            fireDam = fireDam / 2;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades most of the Fireball spell" + "</font><BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                            }
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + " fireDam = " + fireDam + "</font><BR>");
                    }
                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " attacks " + "</font>" + "<font color='silver'>" + pc.name + "</font><BR>");
                    gv.cc.addLogText("<font color='white'>" + "Fireball (" + "</font>" + "<font color='lime'>" + fireDam + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    pc.hp -= fireDam;
                    if (pc.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(pc.combatLocX, pc.combatLocY));
                        gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                        pc.charStatus = "Dead";
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), fireDam + "");
                }
            }

            //remove dead creatures            

//            gv.postDelayed("doFloatyText", 100);
        }
        public void spLightning(object src, object trg, Spell thisSpell)
        {
            //set squares list
            CreateAoeSquaresList(src, trg, thisSpell.aoeShape, thisSpell.aoeRadius);

            //set target list
            CreateAoeTargetsList(src, trg);

            //get casting source information
            int classLevel = 0;
            string sourceName = "";
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                classLevel = source.classLevel;
                sourceName = source.name;
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else //creature casting
            {
                Creature source = (Creature)src;
                classLevel = source.cr_level;
                sourceName = source.cr_name;
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }

            //iterate over targets and do damage
            foreach (object target in AoeTargetsList)
            {
                if (target is Creature)
                {
                    Creature crt = (Creature)target;
                    float resist = (float)(1f - ((float)crt.damageTypeResistanceValueElectricity / 100f));
                    float damage = classLevel * RandInt(6);
                    int elecDam = (int)(damage * resist);

                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + crt.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        elecDam = elecDam / 2;
                        gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " evades most of the Lightning spell" + "</font><BR>");
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + crt.reflex + " >= " + DC + "</font><BR>");
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + " elecDam = " + elecDam + "</font><BR>");
                    }
                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " attacks " + "</font>" + "<font color='silver'>" + crt.cr_name + "</font><BR>");
                    gv.cc.addLogText("<font color='white'>" + "Lightning (" + "</font>" + "<font color='lime'>" + elecDam + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    crt.hp -= elecDam;
                    if (crt.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(crt.combatLocX, crt.combatLocY));
                        gv.cc.addLogText("<font color='lime'>" + "You killed the " + crt.cr_name + "</font><BR>");
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), elecDam + "");
                }
                else //target is Player
                {
                    Player pc = (Player)target;
                    float resist = (float)(1f - ((float)pc.damageTypeResistanceTotalElectricity / 100f));
                    float damage = classLevel * RandInt(6);
                    int elecDam = (int)(damage * resist);

                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + pc.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        if (this.hasTrait(pc, "evasion"))
                        {
                            elecDam = 0;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades all of the Lightning spell" + "</font><BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                            }
                        }
                        else
                        {
                            elecDam = elecDam / 2;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades most of the Lightning spell" + "</font><BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                            }
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + " elecDam = " + elecDam + "</font><BR>");
                    }
                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " attacks " + "</font>" + "<font color='silver'>" + pc.name + "</font><BR>");
                    gv.cc.addLogText("<font color='white'>" + "Lightning (" + "</font>" + "<font color='lime'>" + elecDam + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    pc.hp -= elecDam;
                    if (pc.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(pc.combatLocX, pc.combatLocY));
                        gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                        pc.charStatus = "Dead";
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), elecDam + "");
                }
            }

            //remove dead creatures            

//            gv.postDelayed("doFloatyText", 100);
        }
        
        //SPELLS CLERIC
        public void spHeal(object src, object trg, int healAmount)
        {
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                Player target = (Player)trg;

                if (target.hp <= -20)
                {
                    //MessageBox("Can't heal a dead character!");
                    gv.cc.addLogText("<font color='red'>" + "Can't heal a dead character!" + "</font><BR>");
                }
                else
                {
                    target.hp += healAmount;
                    if (target.hp > target.hpMax)
                    {
                        target.hp = target.hpMax;
                    }
                    if (target.hp > 0)
                    {
                        target.charStatus = "Alive";
                    }
                    //MessageBox(pc.name + " gains " + healAmount + " HPs");
                    gv.cc.addLogText("<font color='lime'>" + target.name + " gains " + healAmount + " HPs" + "</font><BR>");
                }
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            if (src is Item) //player casting
            {
                Player target = (Player)trg;

                if (target.hp <= -20)
                {
                    //MessageBox("Can't heal a dead character!");
                    gv.cc.addLogText("<font color='red'>" + "Can't heal a dead character!" + "</font><BR>");
                }
                else
                {
                    target.hp += healAmount;
                    if (target.hp > target.hpMax)
                    {
                        target.hp = target.hpMax;
                    }
                    if (target.hp > 0)
                    {
                        target.charStatus = "Alive";
                    }
                    //MessageBox(pc.name + " gains " + healAmount + " HPs");
                    MessageBoxHtml(target.name + " gains " + healAmount + " HPs, now has " + target.hp + "/" + target.hpMax + "HPs");
                    gv.cc.addLogText("<font color='lime'>" + target.name + " gains " + healAmount + " HPs" + "</font><BR>");
                }
            }
            else if (src is Creature) //creature casting
            {
                Creature source = (Creature)src;
                Creature target = (Creature)trg;

                target.hp += healAmount;
                if (target.hp > target.hpMax)
                {
                    target.hp = target.hpMax;
                }
                gv.cc.addLogText("<font color='lime'>" + target.cr_name + " gains " + healAmount + " HPs" + "</font><BR>");
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Coordinate)
            {
                //Toast.makeText(gv.gameContext, "target is not a PC or Creature", Toast.LENGTH_SHORT).show();
            }
            else
            {
                //Toast.makeText(gv.gameContext, "don't recognize target type", Toast.LENGTH_SHORT).show();			
            }
        }
        public void spMassHeal(object src, object trg, int healAmount)
        {
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                //Player target = (Player)trg;

                foreach (Player pc in mod.playerList)
                {
                    if (pc.hp <= -20)
                    {
                        gv.cc.addLogText("<font color='red'>" + "Can't heal a dead character!" + "</font><BR>");
                    }
                    else
                    {
                        pc.hp += healAmount;
                        if (pc.hp > pc.hpMax)
                        {
                            pc.hp = pc.hpMax;
                        }
                        if (pc.hp > 0)
                        {
                            pc.charStatus = "Alive";
                        }
                        //MessageBox(pc.name + " gains " + healAmount + " HPs");
                        gv.cc.addLogText("<font color='lime'>" + pc.name + " gains " + healAmount + " HPs" + "</font><BR>");
                    }
                }
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Creature) //creature casting
            {
                Creature source = (Creature)src;
                //Creature target = (Creature)trg;

                foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
                {
                    crt.hp += healAmount;
                    if (crt.hp > crt.hpMax)
                    {
                        crt.hp = crt.hpMax;
                    }
                    gv.cc.addLogText("<font color='lime'>" + crt.cr_name + " gains " + healAmount + " HPs" + "</font><BR>");
                }
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Coordinate)
            {
                //Toast.makeText(gv.gameContext, "target is not a PC or Creature", Toast.LENGTH_SHORT).show();
            }
            else
            {
                //Toast.makeText(gv.gameContext, "don't recognize target type", Toast.LENGTH_SHORT).show();			
            }
        }
        public void spBless(object src, object trg)
        {
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                //Player target = (Player)trg;

                foreach (Player pc in mod.playerList)
                {
                    int numberOfRounds = (source.classLevel * 5); //5 rounds per level
                    Effect ef = mod.getEffectByTag("bless").DeepCopy();
                    ef.durationInUnits = numberOfRounds * gv.mod.TimePerRound;
                    gv.cc.addLogText("<font color='lime'>" + "Bless is applied on " + pc.name
                            + " for " + numberOfRounds + " round(s)" + "</font>" +
                            "<BR>");
                    pc.AddEffectByObject(ef, source.classLevel);
                }
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Creature) //creature casting
            {
                Creature source = (Creature)src;
                //Creature target = (Creature)trg;

                foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
                {
                    int numberOfRounds = (source.cr_level * 5); //5 rounds per level
                    Effect ef = mod.getEffectByTag("bless").DeepCopy();
                    ef.durationInUnits = numberOfRounds * gv.mod.TimePerRound;
                    gv.cc.addLogText("<font color='lime'>" + "Bless is applied on " + crt.cr_name
                            + " for " + numberOfRounds + " round(s)" + "</font>" +
                            "<BR>");
                    crt.AddEffectByObject(ef, source.cr_level);
                }
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Coordinate)
            {
                //Toast.makeText(gv.gameContext, "source is not a PC or Creature", Toast.LENGTH_SHORT).show();
            }
            else
            {
                //Toast.makeText(gv.gameContext, "don't recognize source type", Toast.LENGTH_SHORT).show();			
            }
        }
        public void spMagicStone(object src, object trg)
        {
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                Creature target = (Creature)trg;

                int damageTotal = 0;
                int numberOfBolts = ((source.classLevel - 1) / 2) + 1; //1 stone for every 2 levels after level 1
                if (numberOfBolts > 3) { numberOfBolts = 3; } //can not have more than 3 stones
                for (int i = 0; i < numberOfBolts; i++)
                {
                    float resist = (float)(1f - ((float)target.damageTypeResistanceValueMagic / 100f));
                    float damage = 1 * RandInt(3) + 1;
                    int stoneDam = (int)(damage * resist);
                    //int damage = 1 * RandInt(3) + 1;                
                    target.hp = target.hp - stoneDam;
                    damageTotal += stoneDam;
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage
                                    + " stoneDam = " + stoneDam + "</font>" +
                                    "<BR>");
                    }

                    gv.cc.addLogText("<font color='aqua'>" + source.name + "</font>" +
                            "<font color='white'>" + " attacks " + "</font>" +
                            "<font color='silver'>" + target.cr_name + "</font>" +
                            "<BR>");
                    gv.cc.addLogText("<font color='white'>" + "Magic Stone (" + "</font>");
                    gv.cc.addLogText("<font color='lime'>" + stoneDam + "</font>" +
                            "<font color='white'>" + " damage)" + "</font>" +
                            "<BR>");
                    if (target.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(target.combatLocX, target.combatLocY));
                        gv.cc.addLogText("<font color='lime'>" + "You killed the " + target.cr_name + "</font><BR>");

                    }
                }
                //Do floaty text damage
                //gv.screenCombat.floatyTextOn = true;
                gv.cc.addFloatyText(new Coordinate(target.combatLocX, target.combatLocY), damageTotal + "");
//                gv.postDelayed("doFloatyText", 100);

                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Creature) //creature casting
            {
                Creature source = (Creature)src;
                Player target = (Player)trg;

                int damageTotal = 0;
                int numberOfBolts = ((source.cr_level - 1) / 2) + 1; //1 bolt for every 2 levels after level 1
                if (numberOfBolts > 3) { numberOfBolts = 3; } //can not have more than 5 bolts
                for (int i = 0; i < numberOfBolts; i++)
                {
                    float resist = (float)(1f - ((float)target.damageTypeResistanceTotalMagic / 100f));
                    float damage = 1 * RandInt(3) + 1;
                    int stoneDam = (int)(damage * resist);
                    //int damage = 1 * RandInt(4) + 1;
                    damageTotal += stoneDam;
                    target.hp = target.hp - stoneDam;
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage
                                    + " stoneDam = " + stoneDam + "</font>" +
                                    "<BR>");
                    }
                    gv.cc.addLogText("<font color='silver'>" + source.cr_name + "</font>" +
                            "<font color='white'>" + " attacks " + "</font>" +
                            "<font color='aqua'>" + target.name + "</font>" +
                            "<BR>");
                    gv.cc.addLogText("<font color='white'>" + "Magic Stone (" + "</font>");
                    gv.cc.addLogText("<font color='lime'>" + stoneDam + "</font>" +
                            "<font color='white'>" + " damage)" + "</font>" +
                            "<BR>");
                    if (target.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(target.combatLocX, target.combatLocY));
                        gv.cc.addLogText("<font color='red'>" + target.name + " drops unconcious!" + "</font><BR>");
                        target.charStatus = "Dead";
                    }
                }
                //Do floaty text damage
                //gv.screenCombat.floatyTextOn = true;
                gv.cc.addFloatyText(new Coordinate(target.combatLocX, target.combatLocY), damageTotal + "");
//                gv.postDelayed("doFloatyText", 100);

                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Coordinate)
            {
                //Toast.makeText(gv.gameContext, "target is not a PC or Creature", Toast.LENGTH_SHORT).show();
            }
            else
            {
                //Toast.makeText(gv.gameContext, "don't recognize target type", Toast.LENGTH_SHORT).show();			
            }
        }
        public void spBlastOfLight(object src, object trg, Spell thisSpell)
        {
            //set squares list
            CreateAoeSquaresList(src, trg, thisSpell.aoeShape, thisSpell.aoeRadius);

            //set target list
            CreateAoeTargetsList(src, trg);

            //get casting source information
            int classLevel = 0;
            string sourceName = "";
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                classLevel = source.classLevel;
                sourceName = source.name;
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else //creature casting
            {
                Creature source = (Creature)src;
                classLevel = source.cr_level;
                sourceName = source.cr_name;
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }

            //iterate over targets and do damage
            foreach (object target in AoeTargetsList)
            {
                if (target is Creature)
                {
                    Creature crt = (Creature)target;
                    float resist = (float)(1f - ((float)crt.damageTypeResistanceValueFire / 100f));
                    float damage = 2 * RandInt(6);
                    int fireDam = (int)(damage * resist);

                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + crt.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        fireDam = fireDam / 2;
                        gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " evades most of the Blast of Light spell" + "</font><BR>");
                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + crt.reflex + " >= " + DC + "</font><BR>");
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + " fireDam = " + fireDam + "</font><BR>");
                    }
                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " attacks " + "</font>" + "<font color='silver'>" + crt.cr_name + "</font><BR>");
                    gv.cc.addLogText("<font color='white'>" + "Blast of Light (" + "</font>" + "<font color='lime'>" + fireDam + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    crt.hp -= fireDam;
                    if (crt.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(crt.combatLocX, crt.combatLocY));
                        gv.cc.addLogText("<font color='lime'>" + "You killed the " + crt.cr_name + "</font><BR>");
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), fireDam + "");
                }
                else //target is Player
                {
                    Player pc = (Player)target;
                    float resist = (float)(1f - ((float)pc.damageTypeResistanceTotalFire / 100f));
                    float damage = 2 * RandInt(6);
                    int fireDam = (int)(damage * resist);

                    int saveChkRoll = RandInt(20);
                    int saveChk = saveChkRoll + pc.reflex;
                    int DC = 13;
                    if (saveChk >= DC) //passed save check
                    {
                        if (this.hasTrait(pc, "evasion"))
                        {
                            fireDam = 0;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades all of the Blast of Light spell" + "</font><BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                            }
                        }
                        else
                        {
                            fireDam = fireDam / 2;
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " evades most of the Blast of Light spell" + "</font><BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font><BR>");
                            }
                        }
                    }
                    if (mod.debugMode)
                    {
                        gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage + " fireDam = " + fireDam + "</font><BR>");
                    }
                    gv.cc.addLogText("<font color='aqua'>" + sourceName + "</font>" + "<font color='white'>" + " attacks " + "</font>" + "<font color='silver'>" + pc.name + "</font><BR>");
                    gv.cc.addLogText("<font color='white'>" + "Blast of Light (" + "</font>" + "<font color='lime'>" + fireDam + "</font>" + "<font color='white'>" + " damage)" + "</font><BR>");
                    pc.hp -= fireDam;
                    if (pc.hp <= 0)
                    {
                        gv.screenCombat.deathAnimationLocations.Add(new Coordinate(pc.combatLocX, pc.combatLocY));
                        gv.cc.addLogText("<font color='red'>" + pc.name + " drops unconcious!" + "</font><BR>");
                        pc.charStatus = "Dead";
                    }
                    //Do floaty text damage
                    //gv.screenCombat.floatyTextOn = true;
                    gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), fireDam + "");                    
                }
            }

            //remove dead creatures            

//            gv.postDelayed("doFloatyText", 100);
        }
        public void spHold(object src, object trg)
        {
            if (src is Player) //player casting
            {
                Player source = (Player)src;
                Creature target = (Creature)trg;

                int saveChkRoll = RandInt(20);
                //int saveChk = saveChkRoll + target.Will;
                int saveChk = saveChkRoll;
                int DC = 16;
                if (saveChk >= DC) //passed save check
                {
                    gv.cc.addLogText("<font color='yellow'>" + target.cr_name + " avoids the hold spell" + "</font><BR>");
                }
                else
                {
                    gv.cc.addLogText("<font color='red'>" + target.cr_name + " is held by a hold spell" + "</font><BR>");
                    target.cr_status = "Held";
                    Effect ef = mod.getEffectByTag("hold");
                    target.AddEffectByObject(ef, source.classLevel);
                }
                source.sp -= gv.cc.currentSelectedSpell.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Creature) //creature casting
            {
                Creature source = (Creature)src;
                Player target = (Player)trg;

                int saveChkRoll = RandInt(20);
                //int saveChk = saveChkRoll + target.Will;
                int saveChk = saveChkRoll;
                int DC = 16;
                if (saveChk >= DC) //passed save check
                {
                    gv.cc.addLogText("<font color='yellow'>" + target.name + " avoids the hold spell" + "</font><BR>");
                }
                else
                {
                    gv.cc.addLogText("<font color='red'>" + target.name + " is held by a hold spell" + "</font><BR>");
                    target.charStatus = "Held";
                    Effect ef = mod.getEffectByTag("hold");
                    target.AddEffectByObject(ef, source.cr_level);
                }
                source.sp -= SpellToCast.costSP;
                if (source.sp < 0) { source.sp = 0; }
            }
            else if (src is Coordinate)
            {
                //Toast.makeText(gv.gameContext, "target is not a PC or Creature", Toast.LENGTH_SHORT).show();
            }
            else
            {
                //Toast.makeText(gv.gameContext, "don't recognize target type", Toast.LENGTH_SHORT).show();			
            }
        }
        */
    }
}