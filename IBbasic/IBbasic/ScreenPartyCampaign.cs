using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IBbasic
{
    public class ScreenPartyCampaign
    {
        //public Module gv.mod;
        public GameView gv;

        public List<IbbButton> btnPartyIndex = new List<IbbButton>();
        public List<PartyCampaign> partiesList = new List<PartyCampaign>();
        private IbbButton btnLeft = null;
        private IbbButton btnRight = null;
        private IbbButton btnExport = null;
        private IbbButton btnReturn = null;
        public IbbToggle btnPartyName = null;
        public IbbToggle btnPartyNotes = null;
        public IbbToggle btnPartyFilename = null;
        public string sendingScreen = "partybuild"; //party or partybuild
        //private int partyScreenPcIndex = 0;
        private int partyIndex = 0;
        //private bool lastClickedPlayerList = true;
        //private string stringMessagePartyBuild = "";

        public ScreenPartyCampaign(Module m, GameView g)
        {
            //gv.mod = m;
            gv = g;
            setControlsStart();
        }
        public void refreshPlayerTokens()
        {
            if (partyIndex >= partiesList.Count) { return; }
            int cntPCs = 0;
            foreach (IbbButton btn in btnPartyIndex)
            {
                if (cntPCs < partiesList[partyIndex].playerList.Count)
                {
                    //gv.cc.DisposeOfBitmap(ref btn.Img2);
                    btn.Img2 = partiesList[partyIndex].playerList[cntPCs].tokenFilename;
                }
                else
                {
                    btn.Img2 = null;
                }
                cntPCs++;
            }
        }
        public void loadPartiesList()
        {
            partiesList.Clear();
            partyIndex = 0;
            if (sendingScreen.Equals("party"))
            {
                partiesList.Add(MakePartyCampaignFromCurrentParty());
            }

            //MODULE SPECIFIC
            List<string> files = gv.GetAllFilesWithExtensionFromUserFolder("\\user", ".json");
            foreach (string f in files)
            {
                try
                {
                    PartyCampaign pcam = LoadParty(f);
                    if (pcam != null)
                    {
                        partiesList.Add(pcam);
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    gv.errorLog(ex.ToString());
                }
            }
        }
        public PartyCampaign MakePartyCampaignFromCurrentParty()
        {
            PartyCampaign toReturn = new PartyCampaign();

            toReturn.partyName = gv.mod.partyName;
            toReturn.partyNotes = gv.mod.partyNotes;
            toReturn.partyFilename = gv.mod.partyFilename;
            toReturn.playerList = gv.mod.playerList;
            toReturn.partyInventoryRefsList = gv.mod.partyInventoryRefsList;
            //go through each item equipped and see if it is not a standard item
            foreach (Player pc in gv.mod.playerList)
            {
                Item it = gv.cc.getItemByResRefForInfo(pc.BodyRefs.resref);
                if (it != null)
                {
                    if ((it.moduleItem) || (it.campaignItem))
                    {
                        it.campaignItem = true;
                        toReturn.partyNonStandardItemsList.Add(it);
                    }
                }
                it = gv.cc.getItemByResRefForInfo(pc.MainHandRefs.resref);
                if (it != null)
                {
                    if ((it.moduleItem) || (it.campaignItem))
                    {
                        it.campaignItem = true;
                        toReturn.partyNonStandardItemsList.Add(it);
                    }
                }
                it = gv.cc.getItemByResRefForInfo(pc.OffHandRefs.resref);
                if (it != null)
                {
                    if ((it.moduleItem) || (it.campaignItem))
                    {
                        it.campaignItem = true;
                        toReturn.partyNonStandardItemsList.Add(it);
                    }
                }
                it = gv.cc.getItemByResRefForInfo(pc.RingRefs.resref);
                if (it != null)
                {
                    if ((it.moduleItem) || (it.campaignItem))
                    {
                        it.campaignItem = true;
                        toReturn.partyNonStandardItemsList.Add(it);
                    }
                }
                it = gv.cc.getItemByResRefForInfo(pc.HeadRefs.resref);
                if (it != null)
                {
                    if ((it.moduleItem) || (it.campaignItem))
                    {
                        it.campaignItem = true;
                        toReturn.partyNonStandardItemsList.Add(it);
                    }
                }
                it = gv.cc.getItemByResRefForInfo(pc.NeckRefs.resref);
                if (it != null)
                {
                    if ((it.moduleItem) || (it.campaignItem))
                    {
                        it.campaignItem = true;
                        toReturn.partyNonStandardItemsList.Add(it);
                    }
                }
                it = gv.cc.getItemByResRefForInfo(pc.Ring2Refs.resref);
                if (it != null)
                {
                    if ((it.moduleItem) || (it.campaignItem))
                    {
                        it.campaignItem = true;
                        toReturn.partyNonStandardItemsList.Add(it);
                    }
                }
                it = gv.cc.getItemByResRefForInfo(pc.FeetRefs.resref);
                if (it != null)
                {
                    if ((it.moduleItem) || (it.campaignItem))
                    {
                        it.campaignItem = true;
                        toReturn.partyNonStandardItemsList.Add(it);
                    }
                }
            }
            //go through inventory and see if item is not a standard item
            foreach (ItemRefs itref in gv.mod.partyInventoryRefsList)
            {
                Item it = gv.cc.getItemByResRefForInfo(itref.resref);
                if (it != null)
                {
                    if ((it.moduleItem) || (it.campaignItem))
                    {
                        it.campaignItem = true;
                        toReturn.partyNonStandardItemsList.Add(it);
                    }
                }
            }

            return toReturn;
        }
        public PartyCampaign LoadParty(string filename)
        {
            PartyCampaign toReturn = null;
            string json = gv.LoadStringFromUserFolder("\\user\\" + filename + ".json");
            //string strg = gv.GetSaveFileString(gv.mod.moduleName, filename);
            using (StringReader sr = new StringReader(json))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (PartyCampaign)serializer.Deserialize(sr, typeof(PartyCampaign));
            }
            // deserialize JSON directly from a file
            /*using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (PartyCampaign)serializer.Deserialize(file, typeof(PartyCampaign));
            }*/
            if (toReturn != null)
            {
                foreach (Player pc in toReturn.playerList)
                {
                    try { pc.race = gv.cc.getRace(pc.raceTag).DeepCopy(); }
                    catch (Exception ex) { gv.errorLog(ex.ToString()); }
                    try { pc.playerClass = gv.cc.getPlayerClass(pc.classTag).DeepCopy(); }
                    catch (Exception ex) { gv.errorLog(ex.ToString()); }
                }
            }
            return toReturn;
        }
        public void updatePlayers()
        {
            foreach (Player pc in gv.mod.playerList)
            {
                try { pc.race = gv.cc.getRace(pc.raceTag).DeepCopy(); }
                catch (Exception ex) { gv.errorLog(ex.ToString()); }
                try { pc.playerClass = gv.cc.getPlayerClass(pc.classTag).DeepCopy(); }
                catch (Exception ex) { gv.errorLog(ex.ToString()); }
            }
        }
        public void ExportParty(string filename, PartyCampaign pcam)
        {
            string json = JsonConvert.SerializeObject(pcam, Newtonsoft.Json.Formatting.Indented);
            gv.SaveText("\\user\\" + filename, json);
        }
        public void ImportParty(string filename)
        {
            PartyCampaign toReturn = null;
            string json = gv.LoadStringFromUserFolder("\\user\\" + filename);
            //string strg = gv.GetSaveFileString(gv.mod.moduleName, filename);
            using (StringReader sr = new StringReader(json))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (PartyCampaign)serializer.Deserialize(sr, typeof(PartyCampaign));
            }
            if (toReturn != null)
            {
                foreach (Player pc in toReturn.playerList)
                {
                    try { pc.race = gv.cc.getRace(pc.raceTag).DeepCopy(); }
                    catch (Exception ex) { gv.errorLog(ex.ToString()); }
                    try { pc.playerClass = gv.cc.getPlayerClass(pc.classTag).DeepCopy(); }
                    catch (Exception ex) { gv.errorLog(ex.ToString()); }
                }
                gv.mod.partyName = toReturn.partyName;
                gv.mod.partyNotes = toReturn.partyNotes;
                gv.mod.partyFilename = toReturn.partyFilename;
                //first remove all down to reserve list
                foreach (Player ply in gv.mod.playerList)
                {
                    Player copyPC = ply.DeepCopy();
                    copyPC.playerClass = gv.cc.getPlayerClass(copyPC.classTag);
                    copyPC.race = gv.cc.getRace(copyPC.raceTag);
                    gv.screenPartyBuild.pcList.Add(copyPC);
                }
                //copy all to playerList
                gv.mod.playerList = toReturn.playerList;
                //add campaign items to gv.cc.allitemslist
                foreach (Item it in toReturn.partyNonStandardItemsList)
                {
                    //check if resref already exists and increment this one if needed
                    bool foundOne = false;
                    foreach (Item itall in gv.cc.allItemsList)
                    {
                        if (itall.resref.Equals(it.resref))
                        {
                            foundOne = true;
                        }
                    }
                    if (!foundOne)
                    {
                        it.campaignItem = true;
                        gv.cc.allItemsList.Add(it.DeepCopy());
                    }
                }
                //fill inventory
                foreach (ItemRefs itref in toReturn.partyInventoryRefsList)
                {
                    gv.mod.partyInventoryRefsList.Add(itref.DeepCopy());
                }
            }
        }

        public void setControlsStart()
        {
            int pW = (int)((float)gv.screenWidth / 100.0f);
            int pH = (int)((float)gv.screenHeight / 100.0f);
            int center = (gv.uiSquareSize * gv.uiSquaresInWidth / 2);
            int padW = gv.uiSquareSize / 6;

            btnPartyIndex.Clear();
            for (int y = 0; y < gv.mod.numberOfPlayerMadePcsAllowed; y++)
            {
                IbbButton btnNew = new IbbButton(gv, 1.0f);
                btnNew.Img = "item_slot"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.item_slot);
                btnNew.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
                btnNew.X = (gv.uiSquareSize / 4);
                btnNew.Y = ((y + 1) * gv.uiSquareSize);
                btnNew.Height = (int)(gv.ibbheight * gv.scaler);
                btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);

                btnPartyIndex.Add(btnNew);
            }

            if (btnPartyName == null)
            {
                btnPartyName = new IbbToggle(gv);
            }
            btnPartyName.ImgOn = "mtgl_edit_btn";
            btnPartyName.ImgOff = "mtgl_edit_btn";
            btnPartyName.X = 2 * gv.uiSquareSize + (gv.uiSquareSize / 4);
            btnPartyName.Y = 0 * gv.uiSquareSize + (0 * gv.uiSquareSize / 4);
            btnPartyName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPartyName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPartyFilename == null)
            {
                btnPartyFilename = new IbbToggle(gv);
            }
            btnPartyFilename.ImgOn = "mtgl_edit_btn";
            btnPartyFilename.ImgOff = "mtgl_edit_btn";
            btnPartyFilename.X = 2 * gv.uiSquareSize + (1 * gv.uiSquareSize / 4);
            btnPartyFilename.Y = 0 * gv.uiSquareSize + (2 * gv.uiSquareSize / 4);
            btnPartyFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPartyFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPartyNotes == null)
            {
                btnPartyNotes = new IbbToggle(gv);
            }
            btnPartyNotes.ImgOn = "mtgl_edit_btn";
            btnPartyNotes.ImgOff = "mtgl_edit_btn";
            btnPartyNotes.X = 6 * gv.uiSquareSize + (4 * gv.uiSquareSize / 4);
            btnPartyNotes.Y = 0 * gv.uiSquareSize + (2 * gv.uiSquareSize / 4);
            btnPartyNotes.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPartyNotes.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnLeft == null)
            {
                btnLeft = new IbbButton(gv, 1.0f);
            }
            btnLeft.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
            btnLeft.Img2 = "ctrl_left_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_left_arrow);
            btnLeft.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnLeft.X = (0 * gv.uiSquareSize) + (1 * gv.uiSquareSize / 4);
            btnLeft.Y = (0 * gv.uiSquareSize);
            btnLeft.Height = (int)(gv.ibbheight * gv.scaler);
            btnLeft.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnExport == null)
            {
                btnExport = new IbbButton(gv, 1.0f);
            }
            btnExport.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_off);
            btnExport.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnExport.Text = "EXPORT";
            btnExport.X = (10 * gv.uiSquareSize);
            btnExport.Y = (0 * gv.uiSquareSize);
            btnExport.Height = (int)(gv.ibbheight * gv.scaler);
            btnExport.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRight == null)
            {
                btnRight = new IbbButton(gv, 1.0f);
            }
            btnRight.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
            btnRight.Img2 = "ctrl_right_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_right_arrow);
            btnRight.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
            btnRight.X = (1 * gv.uiSquareSize) + (1 * gv.uiSquareSize / 4);
            btnRight.Y = (0 * gv.uiSquareSize);
            btnRight.Height = (int)(gv.ibbheight * gv.scaler);
            btnRight.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnReturn == null)
            {
                btnReturn = new IbbButton(gv, 1.0f);
            }
            btnReturn.Text = "RETURN";
            btnReturn.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnReturn.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnReturn.X = (10 * gv.uiSquareSize);
            btnReturn.Y = (6 * gv.uiSquareSize);
            btnReturn.Height = (int)(gv.ibbheight * gv.scaler);
            btnReturn.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        //PARTY SCREEN
        public void redrawPartyCampaign(SKCanvas c)
        {
            setControlsStart();
            btnReturn.Draw(c);

            int pW = (int)((float)gv.screenWidth / 100.0f);
            int pH = (int)((float)gv.screenHeight / 100.0f);
            int padH = gv.uiSquareSize / 8;
            if (sendingScreen.Equals("party"))
            {
                if (partiesList.Count == 0)
                {
                    loadPartiesList();
                }
            }
            if (partiesList.Count == 0)
            {
                gv.DrawText(c, "No Campaign Parties to Import", btnPartyName.X + btnPartyName.Width, btnPartyName.Y + padH, "yl");
                return;
            }
            string color1 = "yl";
            string color2 = "wh";
            string color3 = "bu";
            if ((partyIndex > 0) || (sendingScreen.Equals("partyBuild")))
            {
                color1 = "gy";
                color2 = "gy";
                color3 = "gy";
            }
            //Draw screen title name
            string text = partiesList[partyIndex].partyName;
            gv.DrawText(c, "Name: ", btnPartyName.X + btnPartyName.Width, btnPartyName.Y + padH, color1);
            gv.DrawText(c, "      " + text, btnPartyName.X + btnPartyName.Width, btnPartyName.Y + padH, color2);
            text = "Party Notes";
            gv.DrawText(c, text, btnPartyNotes.X + btnPartyNotes.Width, btnPartyNotes.Y + padH, color1);
            text = partiesList[partyIndex].partyFilename;
            gv.DrawText(c, "File: ", btnPartyFilename.X + btnPartyFilename.Width, btnPartyFilename.Y + padH, color1);
            gv.DrawText(c, "      " + text, btnPartyFilename.X + btnPartyFilename.Width, btnPartyFilename.Y + padH, color2);

            //DRAW EACH PC BUTTON
            this.refreshPlayerTokens();

            int cntPCs = 0;
            foreach (IbbButton btn in btnPartyIndex)
            {
                if (cntPCs < partiesList[partyIndex].playerList.Count)
                {
                    btn.Draw(c);
                }
                cntPCs++;
            }

            btnLeft.Draw(c);
            btnRight.Draw(c);
            
            if ((partyIndex > 0) && (sendingScreen.Equals("party")))
            {
                //only allow to export current party
                btnExport.Img = "btn_small_off";
            }
            else
            {
                btnExport.Img = "btn_small";
            }
            if (sendingScreen.Equals("party"))
            {
                btnExport.Text = "EXPORT";
            }
            else
            {
                btnExport.Text = "IMPORT";
            }
            btnExport.Draw(c);
            btnPartyName.Draw(c);
            btnPartyNotes.Draw(c);
            btnPartyFilename.Draw(c);

            int locY = 1 * gv.uiSquareSize;
            int locX = 1 * gv.uiSquareSize + (gv.uiSquareSize / 4);
            int spacing = (int)(gv.fontHeight + gv.fontLineSpacing);
            int tabX = 4 * gv.uiSquareSize + (padH * 3);
            int tabX2 = 7 * gv.uiSquareSize + gv.uiSquareSize / 2;
            int leftStartY = 5 * gv.uiSquareSize;

            int pcIdx = 1;
            foreach (Player pc in partiesList[partyIndex].playerList)
            {
                gv.sf.UpdateStats(pc);
                string gender = "female";
                if (pc.isMale)
                {
                    gender = "male";
                }

                int actext = 0;
                if (gv.mod.ArmorClassAscending) { actext = pc.AC; }
                else { actext = 20 - pc.AC; }

                string line = pc.name + ", " + gv.cc.getRace(pc.raceTag).name + ", " + gender + ", " + gv.cc.getPlayerClass(pc.classTag).name;
                gv.DrawText(c, line, locX, locY * pcIdx, color3);
                line = "LVL:" + pc.classLevel + ", XP:" + pc.XP + "/" + pc.XPNeeded + ", AC:" + actext + ",  BAB:" + pc.baseAttBonus + ", HP:" + pc.hp + "/" + pc.hpMax + ", SP:" + pc.sp + "/" + pc.spMax;
                gv.DrawText(c, line, locX, locY * pcIdx + spacing, color2);
                line = "STR:" + pc.strength + ", DEX:" + pc.dexterity + ", CON:" + pc.constitution + ", INT:" + pc.intelligence + ", WIS:" + pc.wisdom + ", CHA:" + pc.charisma;
                gv.DrawText(c, line, locX, locY * pcIdx + spacing + spacing, color2);
                pcIdx++;
            }
            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox(c);
            }
        }
        public void onTouchPartyCampaign(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnLeft.glowOn = false;
            btnRight.glowOn = false;
            btnReturn.glowOn = false;
            btnExport.glowOn = false;
            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }

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

                    if (btnLeft.getImpact(x, y))
                    {
                        btnLeft.glowOn = true;
                    }
                    else if (btnExport.getImpact(x, y))
                    {
                        btnExport.glowOn = true;
                    }
                    else if (btnRight.getImpact(x, y))
                    {
                        btnRight.glowOn = true;
                    }
                    else if (btnReturn.getImpact(x, y))
                    {
                        btnReturn.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnLeft.glowOn = false;
                    btnRight.glowOn = false;
                    btnExport.glowOn = false;
                    btnReturn.glowOn = false;
                    btnExport.glowOn = false;

                    if (gv.showMessageBox)
                    {
                        gv.messageBox.btnReturn.glowOn = false;
                    }
                    if (gv.showMessageBox)
                    {
                        if (gv.messageBox.btnReturn.getImpact(x, y))
                        {
                            //gv.PlaySound("btn_click");
                            gv.showMessageBox = false;
                        }
                        return;
                    }

                    if (btnLeft.getImpact(x, y))
                    {
                        //gv.PlaySound("btn_click");
                        //change index of pcList
                        //lastClickedPlayerList = false;
                        if (partyIndex > 0)
                        {
                            partyIndex--;
                        }
                    }
                    else if (btnExport.getImpact(x, y))
                    {
                        if ((sendingScreen.Equals("party")) && (partyIndex > 0))
                        {
                            return;
                        }
                        if (sendingScreen.Equals("party")) //EXPORT
                        {
                            //check to see if filename already exists and give error message if does
                            List<string> files = gv.GetAllFilesWithExtensionFromUserFolder("\\user", ".json");
                            bool foundOne = false;
                            foreach (string f in files)
                            {
                                if (f.Equals(this.partiesList[0].partyFilename))
                                {
                                    foundOne = true;
                                }
                            }
                            if (foundOne)
                            {
                                gv.sf.MessageBoxHtml("File with the same name already exists...choose a different filename.");
                            }
                            else if (this.partiesList[0].partyFilename.Equals("party00"))
                            {
                                gv.sf.MessageBoxHtml("Can not use the default 'party00' file name...choose a different filename.");
                            }
                            else
                            {
                                //ask again if they really want to export the party
                                doExportCheckSetup();
                            }
                        }
                        else //IMPORT
                        {
                            ImportParty(partiesList[partyIndex].partyFilename + ".json");
                            if (gv.mod.playerList.Count > 0)
                            {
                                if (sendingScreen.Equals("party"))
                                {
                                    gv.screenType = "main";
                                }
                                else
                                {
                                    gv.screenType = "partyBuild";
                                }
                            }
                        }
                    }
                    else if (btnRight.getImpact(x, y))
                    {
                        //gv.PlaySound("btn_click");
                        //change index of pcList
                        //lastClickedPlayerList = false;
                        if (partyIndex < partiesList.Count - 1)
                        {
                            partyIndex++;
                        }
                    }
                    else if (btnPartyName.getImpact(x, y))
                    {
                        if (partyIndex > 0) { return; }
                        if (sendingScreen.Equals("partyBuild")) { return; }
                        changePartyName();
                    }
                    else if (btnPartyNotes.getImpact(x, y))
                    {
                        if (partyIndex > 0) { return; }
                        if (sendingScreen.Equals("partyBuild")) { return; }
                        changePartyNotes();
                    }
                    else if (btnPartyFilename.getImpact(x, y))
                    {
                        if (partyIndex > 0) { return; }
                        if (sendingScreen.Equals("partyBuild")) { return; }
                        changePartyFilename();
                    }
                    else if (btnReturn.getImpact(x, y))
                    {
                        //gv.PlaySound("btn_click");
                        //if (gv.mod.playerList.Count > 0)
                        //{
                        if (sendingScreen.Equals("party"))
                        {
                            if (gv.mod.playerList.Count > 0)
                            {
                                gv.screenType = "main";
                            }
                        }
                        else
                        {
                            gv.screenType = "partyBuild";
                        }
                        //}
                    }
                    break;
            }
        }

        public void doExportCheckSetup()
        {
            List<string> actionList = new List<string> { "Yes Export Party", "Cancel Export" };
            gv.itemListSelector.setupIBminiItemListSelector(gv, actionList, "Do you wish to export the current party?", "exportcheck");
            gv.itemListSelector.showIBminiItemListSelector = true;
        }
        public void doExportCheck(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                ExportParty(this.partiesList[0].partyFilename + ".json", this.partiesList[0]);
            }
        }

        public async void changePartyName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Name for this party", partiesList[0].partyName);
            partiesList[0].partyName = myinput;
            gv.touchEnabled = true;
        }
        public async void changePartyNotes()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Notes for this party", partiesList[0].partyNotes);
            partiesList[0].partyNotes = myinput;
            gv.touchEnabled = true;
        }
        public async void changePartyFilename()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Unique filename for this party", partiesList[0].partyFilename);
            partiesList[0].partyFilename = myinput;
            gv.touchEnabled = true;
        }
    }
}
