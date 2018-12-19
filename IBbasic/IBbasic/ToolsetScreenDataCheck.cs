using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IBbasic
{
    public class ToolsetScreenDataCheck
    {
        public GameView gv;
        public IbbButton btnCheckAreas = null;
        public IbbButton btnCheckEncounters = null;
        public IbbButton btnCheckConvos = null;
        public IbbButton btnCheckMisc = null;
        public Area chkArea;
        public Encounter chkEnc;
        public Convo chkConvo;
        public string report = "";
        public List<string> nodeImagesList = new List<string>();

        public ToolsetScreenDataCheck(GameView g)
        {
            gv = g;
            setControlsStart();
        }

        public void setControlsStart()
        {
            if (btnCheckAreas == null)
            {
                btnCheckAreas = new IbbButton(gv, 0.8f);
            }
            btnCheckAreas.Text = "Area";
            btnCheckAreas.Img = "btn_small";
            btnCheckAreas.Glow = "btn_small_glow";
            btnCheckAreas.X = 0 * gv.uiSquareSize;
            btnCheckAreas.Y = (int)(1 * gv.uiSquareSize + gv.scaler);
            btnCheckAreas.Height = (int)(gv.ibbheight * gv.scaler);
            btnCheckAreas.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCheckEncounters == null)
            {
                btnCheckEncounters = new IbbButton(gv, 0.8f);
            }
            btnCheckEncounters.Text = "Enc";
            btnCheckEncounters.Img = "btn_small";
            btnCheckEncounters.Glow = "btn_small_glow";
            btnCheckEncounters.X = 0 * gv.uiSquareSize;
            btnCheckEncounters.Y = (int)(2 * gv.uiSquareSize + gv.scaler);
            btnCheckEncounters.Height = (int)(gv.ibbheight * gv.scaler);
            btnCheckEncounters.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCheckConvos == null)
            {
                btnCheckConvos = new IbbButton(gv, 0.8f);
            }
            btnCheckConvos.Text = "Convo";
            btnCheckConvos.Img = "btn_small";
            btnCheckConvos.Glow = "btn_small_glow";
            btnCheckConvos.X = 0 * gv.uiSquareSize;
            btnCheckConvos.Y = (int)(3 * gv.uiSquareSize + gv.scaler);
            btnCheckConvos.Height = (int)(gv.ibbheight * gv.scaler);
            btnCheckConvos.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCheckMisc == null)
            {
                btnCheckMisc = new IbbButton(gv, 0.8f);
            }
            btnCheckMisc.Text = "Misc";
            btnCheckMisc.Img = "btn_small";
            btnCheckMisc.Glow = "btn_small_glow";
            btnCheckMisc.X = 0 * gv.uiSquareSize;
            btnCheckMisc.Y = (int)(4 * gv.uiSquareSize + gv.scaler);
            btnCheckMisc.Height = (int)(gv.ibbheight * gv.scaler);
            btnCheckMisc.Width = (int)(gv.ibbwidthR * gv.scaler);

        }

        public void redrawTsDataCheck(SKCanvas c)
        {
            setControlsStart();
            int shiftForFont = (btnCheckAreas.Height / 2) - (gv.fontHeight / 2);
            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            //Page Title
            gv.DrawText(c, "MODULE DATA CHECKING", center - (7 * (gv.fontWidth + gv.fontCharSpacing)), 2 * gv.scaler, "yl");

            btnCheckAreas.Draw(c);
            gv.DrawText(c, " Checking areas for data errors", btnCheckAreas.X + btnCheckAreas.Width + gv.scaler, btnCheckAreas.Y + shiftForFont - gv.fontHeight, "wh");
            gv.DrawText(c, " (this can take a few seconds", btnCheckAreas.X + btnCheckAreas.Width + gv.scaler, btnCheckAreas.Y + shiftForFont, "wh");
            gv.DrawText(c, " to complete)", btnCheckAreas.X + btnCheckAreas.Width + gv.scaler, btnCheckAreas.Y + shiftForFont + gv.fontHeight, "wh");

            btnCheckEncounters.Draw(c);
            gv.DrawText(c, " Checking encounters for data", btnCheckEncounters.X + btnCheckEncounters.Width + gv.scaler, btnCheckEncounters.Y + shiftForFont - gv.fontHeight, "wh");
            gv.DrawText(c, " errors (this can take a few", btnCheckEncounters.X + btnCheckEncounters.Width + gv.scaler, btnCheckEncounters.Y + shiftForFont, "wh");
            gv.DrawText(c, " seconds to complete)", btnCheckEncounters.X + btnCheckEncounters.Width + gv.scaler, btnCheckEncounters.Y + shiftForFont + gv.fontHeight, "wh");

            btnCheckConvos.Draw(c);
            gv.DrawText(c, " Checking conversations for data", btnCheckConvos.X + btnCheckConvos.Width + gv.scaler, btnCheckConvos.Y + shiftForFont - gv.fontHeight, "wh");
            gv.DrawText(c, " errors (this can take a few", btnCheckConvos.X + btnCheckConvos.Width + gv.scaler, btnCheckConvos.Y + shiftForFont, "wh");
            gv.DrawText(c, " seconds to complete)", btnCheckConvos.X + btnCheckConvos.Width + gv.scaler, btnCheckConvos.Y + shiftForFont + gv.fontHeight, "wh");

            btnCheckMisc.Draw(c);
            gv.DrawText(c, " Checking other miscellaneous", btnCheckMisc.X + btnCheckMisc.Width + gv.scaler, btnCheckMisc.Y + shiftForFont - gv.fontHeight, "wh");
            gv.DrawText(c, " data for errors (this can", btnCheckMisc.X + btnCheckMisc.Width + gv.scaler, btnCheckMisc.Y + shiftForFont, "wh");
            gv.DrawText(c, " take a few seconds to complete)", btnCheckMisc.X + btnCheckMisc.Width + gv.scaler, btnCheckMisc.Y + shiftForFont + gv.fontHeight, "wh");


            gv.tsMainMenu.redrawTsMainMenu(c);

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox(c);
            }
        }
        public void onTouchTsDataCheck(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnCheckAreas.glowOn = false;
            btnCheckEncounters.glowOn = false;
            btnCheckConvos.glowOn = false;
            btnCheckMisc.glowOn = false;

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

                    if (btnCheckAreas.getImpact(x, y))
                    {
                        btnCheckAreas.glowOn = true;
                    }
                    else if (btnCheckEncounters.getImpact(x, y))
                    {
                        btnCheckEncounters.glowOn = true;
                    }
                    else if (btnCheckConvos.getImpact(x, y))
                    {
                        btnCheckConvos.glowOn = true;
                    }
                    else if (btnCheckMisc.getImpact(x, y))
                    {
                        btnCheckMisc.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnCheckAreas.glowOn = false;
                    btnCheckEncounters.glowOn = false;
                    btnCheckConvos.glowOn = false;
                    btnCheckMisc.glowOn = false;

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

                    if (btnCheckAreas.getImpact(x, y))
                    {
                        report = "";
                        logText("Start Data Check." + Environment.NewLine);
                        CheckAreas();
                        logText(Environment.NewLine + "Completed Data Check.");
                        gv.IBMessageBox("CHECK AREAS", report);
                    }
                    else if (btnCheckEncounters.getImpact(x, y))
                    {
                        report = "";
                        logText("Start Data Check." + Environment.NewLine);
                        CheckEncounters();
                        logText(Environment.NewLine + "Completed Data Check.");
                        gv.IBMessageBox("CHECK ENCOUNTERS", report);
                    }
                    else if (btnCheckConvos.getImpact(x, y))
                    {
                        report = "";
                        logText("Start Data Check." + Environment.NewLine);
                        CheckConvos();
                        logText(Environment.NewLine + "Completed Data Check.");
                        gv.IBMessageBox("CHECK CONVOS", report);
                    }
                    else if (btnCheckMisc.getImpact(x, y))
                    {
                        report = "";
                        logText("Start Data Check." + Environment.NewLine);
                        CheckCasterNoKnownSpells();
                        CheckDuplicateTagsOrResRef();
                        CheckContainers();
                        CheckShops();
                        logText(Environment.NewLine + "Completed Data Check.");
                        gv.IBMessageBox("CHECK MISC", report);
                    }
                    break;
            }
        }

        public void logText(string text)
        {
            report += text + Environment.NewLine;
        }

        public void CheckAreas()
        {
            string textlist = "";
            //iterate through all areas from the area list and load one at a time and check
            bool foundStartingArea = false;
            List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".are");
            foreach (string areafilename in ret)
            {
                string s = gv.LoadStringFromEitherFolder("\\modules\\" + gv.mod.moduleName + "\\" + areafilename + ".are", "\\modules\\" + gv.mod.moduleName + "\\" + areafilename + ".are");
                using (StringReader sr = new StringReader(s))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    chkArea = (Area)serializer.Deserialize(sr, typeof(Area));
                    if (chkArea != null)
                    {
                        if (gv.mod.startingArea.Equals(chkArea.Filename))
                        {
                            foundStartingArea = true;
                        }
                        //check triggers for errors
                        foreach (Trigger trg in chkArea.Triggers)
                        {
                            //check if transition with no destination
                            if ((trg.Event1Type == "transition") && ((trg.Event1TransPointX == 0) && (trg.Event1TransPointY == 0)))
                            {
                                logText("TRIGGER ERROR: " + chkArea.Filename + ": trigger " + trg.TriggerTag + "event1 has a x=0 and y=0 location, is that intended?");
                            }
                            if ((trg.Event2Type == "transition") && ((trg.Event2TransPointX == 0) && (trg.Event2TransPointY == 0)))
                            {
                                logText("TRIGGER ERROR: " + chkArea.Filename + ": trigger " + trg.TriggerTag + "event2 has a x=0 and y=0 location, is that intended?");
                            }
                            if ((trg.Event3Type == "transition") && ((trg.Event3TransPointX == 0) && (trg.Event3TransPointY == 0)))
                            {
                                logText("TRIGGER ERROR: " + chkArea.Filename + ": trigger " + trg.TriggerTag + "event3 has a x=0 and y=0 location, is that intended?");
                            }
                            if ((trg.Event1Type != "none") && (trg.Event1FilenameOrTag == "none"))
                            {
                                logText("TRIGGER ERROR: " + chkArea.Filename + ": trigger " + trg.TriggerTag + ": event1 has type of " + trg.Event1Type + " but filename/tag of 'none'");
                            }
                            if ((trg.Event2Type != "none") && (trg.Event2FilenameOrTag == "none"))
                            {
                                logText("TRIGGER ERROR: " + chkArea.Filename + ": trigger " + trg.TriggerTag + ": event2 has type of " + trg.Event2Type + " but filename/tag of 'none'");
                            }
                            if ((trg.Event3Type != "none") && (trg.Event3FilenameOrTag == "none"))
                            {
                                logText("TRIGGER ERROR: " + chkArea.Filename + ": trigger " + trg.TriggerTag + ": event3 has type of " + trg.Event3Type + " but filename/tag of 'none'");
                            }
                        }
                        //remove trigger squares that are outside the map boundries
                        foreach (Trigger trg in chkArea.Triggers)
                        {
                            for (int x = trg.TriggerSquaresList.Count - 1; x >= 0; x--)
                            {
                                if ((trg.TriggerSquaresList[x].X >= chkArea.MapSizeX) || (trg.TriggerSquaresList[x].X < 0) || (trg.TriggerSquaresList[x].Y < 0) || (trg.TriggerSquaresList[x].Y >= chkArea.MapSizeY))
                                {
                                    trg.TriggerSquaresList.RemoveAt(x);
                                }
                            }
                        }
                        //remove triggers that have no locations
                        for (int x = chkArea.Triggers.Count - 1; x >= 0; x--)
                        {
                            if (chkArea.Triggers[x].TriggerSquaresList.Count == 0)
                            {
                                chkArea.Triggers.RemoveAt(x);
                            }
                        }
                        textlist += chkArea.Filename + ",";
                    }
                }
            }
            if (!foundStartingArea)
            {
                logText("MODULE ERROR: Starting Area: " + gv.mod.startingArea + " file is not found in the 'areas' list of your module");
            }
            logText("Completed Checking Areas: " + textlist);
        }
        public void CheckConvos()
        {
            nodeImagesList.Clear();
            nodeImagesList = gv.GetAllFilesWithExtensionFromBothFolders("\\graphics", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            string convoslist = "";
            //look for end points on red nodes
            List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".dlg");
            foreach (string cnvname in ret)
            {
                string s = gv.LoadStringFromEitherFolder("\\modules\\" + gv.mod.moduleName + "\\" + cnvname + ".dlg", "\\modules\\" + gv.mod.moduleName + "\\" + cnvname + ".dlg");
                using (StringReader sr = new StringReader(s))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    chkConvo = (Convo)serializer.Deserialize(sr, typeof(Convo));
                    if (chkConvo != null)
                    {
                        logText("CHECKING CONVO: " + chkConvo.ConvoFileName);
                        foreach (ContentNode subNode in chkConvo.subNodes)
                        {
                            checkConvoNode(subNode);
                            if ((subNode.subNodes.Count == 0) && (!subNode.pcNode) && (!subNode.isLink))
                            {
                                //this is a red node on an end point
                                logText("CONVO ERROR: node ID# " + subNode.idNum + " NPC node on end dialog node which is not allowed.");
                            }
                            //check to see if node image is in list
                            if (!subNode.NodePortraitBitmap.Equals(""))
                            {
                                if ((!nodeImagesList.Contains(subNode.NodePortraitBitmap)) && (!nodeImagesList.Contains(Path.GetFileNameWithoutExtension(subNode.NodePortraitBitmap))))
                                {
                                    logText("CONVO ERROR: node ID# " + subNode.idNum + " - Node image not found: " + subNode.NodePortraitBitmap);
                                }
                            }
                            //check to see if still using gaRemovePropByTag or gaRemovePropByIndex
                            foreach (Action a in subNode.actions)
                            {
                                bool foundOne = false;
                                foreach (ScriptObject so in gv.cc.scriptList)
                                {
                                    if (a.a_script.Contains(so.name))
                                    {
                                        foundOne = true;
                                    }
                                    if (a.a_script.Equals("gaGiveItem.cs"))
                                    {
                                        Item it = gv.cc.getItemByResRef(a.a_parameter_1);
                                        if (it == null)
                                        {
                                            logText("CONVOR ERROR: gaGiveItem - " + a.a_parameter_1 + " does not exist in the module's or default items list");
                                        }
                                    }
                                }
                                if (!foundOne)
                                {
                                    logText("CONVO ERROR: script " + a.a_script + " is not a script found in the scriptList (no longer a valid script)");
                                }
                            }
                            foreach (Condition c in subNode.conditions)
                            {
                                bool foundOne = false;
                                foreach (ScriptObject so in gv.cc.scriptList)
                                {
                                    if (c.c_script.Contains(so.name))
                                    {
                                        foundOne = true;
                                    }
                                }
                                if (!foundOne)
                                {
                                    logText("CONVO ERROR: script " + c.c_script + " is not a script found in the scriptList (no longer a valid script)");
                                }
                            }
                            //addToGraphicsList(subNode.NodePortraitBitmap);
                        }
                    }
                }
            }
            logText("Completed Checking Convos: " + convoslist);
        }
        public ContentNode checkConvoNode(ContentNode node)
        {
            ContentNode tempNode = null;
            //do all your checks here
            if (node != null)
            {
                if ((node.subNodes.Count == 0) && (!node.pcNode) && (!node.isLink))
                {
                    //this is a red node on an end point
                    logText("CONVO ERROR: node ID# " + node.idNum + " NPC node on end dialog node which is not allowed.");
                }
                //check to see if node image is in list
                if (!node.NodePortraitBitmap.Equals(""))
                {
                    if ((!nodeImagesList.Contains(node.NodePortraitBitmap)) && (!nodeImagesList.Contains(Path.GetFileNameWithoutExtension(node.NodePortraitBitmap))))
                    {
                        logText("CONVO ERROR: node ID# " + node.idNum + " - Node image not found: " + node.NodePortraitBitmap);
                    }
                }
                //check to see if still using gaRemovePropByTag or gaRemovePropByIndex
                foreach (Action a in node.actions)
                {
                    bool foundOne = false;
                    foreach (ScriptObject so in gv.cc.scriptList)
                    {
                        if (a.a_script.Contains(so.name))
                        {
                            foundOne = true;
                        }
                        if (a.a_script.Equals("gaGiveItem.cs"))
                        {
                            Item it = gv.cc.getItemByResRef(a.a_parameter_1);
                            if (it == null)
                            {
                                logText("CONVOR ERROR: gaGiveItem - " + a.a_parameter_1 + " does not exist in the module's or default items list");
                            }
                        }
                    }
                    if (!foundOne)
                    {
                        logText("CONVO ERROR: script " + a.a_script + " is not a script found in the scriptList (no longer a valid script)");
                    }
                }
                foreach (Condition c in node.conditions)
                {
                    bool foundOne = false;
                    foreach (ScriptObject so in gv.cc.scriptList)
                    {
                        if (c.c_script.Contains(so.name))
                        {
                            foundOne = true;
                        }
                    }
                    if (!foundOne)
                    {
                        logText("CONVO ERROR: script " + c.c_script + " is not a script found in the scriptList (no longer a valid script)");
                    }
                }
                //addToGraphicsList(node.NodePortraitBitmap);
            }
            foreach (ContentNode subNode in node.subNodes)
            {
                tempNode = checkConvoNode(subNode);
                if (tempNode != null)
                {
                    return tempNode;
                }
            }
            return tempNode;
        }
        public void CheckEncounters()
        {
            string textlist = "";
            //check for encounter with no creatures and/or no starting PC locations or less than 6
            List<string> ret = gv.GetAllFilesWithExtensionFromUserFolder("\\modules\\" + gv.mod.moduleName, ".enc");
            foreach (string encname in ret)
            {
                string s = gv.LoadStringFromEitherFolder("\\modules\\" + gv.mod.moduleName + "\\" + encname + ".enc", "\\modules\\" + gv.mod.moduleName + "\\" + encname + ".enc");
                using (StringReader sr = new StringReader(s))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    chkEnc = (Encounter)serializer.Deserialize(sr, typeof(Encounter));
                    if (chkEnc != null)
                    {
                        if (chkEnc.encounterCreatureRefsList.Count == 0)
                        {
                            logText("ENCOUNTER ERROR: " + chkEnc.encounterName + " has no creatures");
                        }
                        if (chkEnc.encounterPcStartLocations.Count < 6)
                        {
                            logText("ENCOUNTER ERROR: " + chkEnc.encounterName + " has less than 6 PC start locations (has " + chkEnc.encounterPcStartLocations.Count.ToString() + ")");
                        }
                        if ((chkEnc.MapSizeX != 10) && (chkEnc.MapSizeY != 10))
                        {
                            logText("ENCOUNTER ERROR: " + chkEnc.encounterName + " is not a 10x10 size map...it is " + chkEnc.MapSizeX + "x" + chkEnc.MapSizeY);
                        }
                        //check to see if has any non-valid drop items
                        foreach (ItemRefs ir in chkEnc.encounterInventoryRefsList)
                        {
                            Item it = gv.cc.getItemByResRef(ir.resref);
                            if (it == null)
                            {
                                logText("ENCOUNTER ERROR: " + chkEnc.encounterName + " has an item " + ir.name + " that does not exist in the module's or default items list");
                            }
                        }
                        //check to see if has any non-valid creatures
                        foreach (CreatureRefs cr in chkEnc.encounterCreatureRefsList)
                        {
                            Creature crt = gv.cc.getCreatureByResRef(cr.creatureResRef);
                            if (crt == null)
                            {
                                logText("ENCOUNTER ERROR: " + chkEnc.encounterName + " has a creature " + cr.creatureTag + " that does not exist in the module's or default creature list");
                            }
                        }
                        //check to see if any tiles are non-walkable
                        int foundOne = 0;
                        foreach (int t in chkEnc.Walkable)
                        {
                            if (t == 0)
                            {
                                foundOne++;
                                break;
                            }
                        }
                        if (foundOne == 0)
                        {
                            logText("ENCOUNTER ERROR: " + chkEnc.encounterName + " all tiles are walkable, was that intended?");
                        }
                        //go through all triggers and check for missing data
                        foreach (Trigger trg in chkEnc.Triggers)
                        {
                            //check if transition with no destination
                            if ((trg.Event1Type == "transition") && ((trg.Event1TransPointX == 0) && (trg.Event1TransPointY == 0)))
                            {
                                logText("TRIGGER ERROR: " + chkEnc.encounterName + ": trigger " + trg.TriggerTag + "event1 has a x=0 and y=0 location, is that intended?");
                            }
                            if ((trg.Event2Type == "transition") && ((trg.Event2TransPointX == 0) && (trg.Event2TransPointY == 0)))
                            {
                                logText("TRIGGER ERROR: " + chkEnc.encounterName + ": trigger " + trg.TriggerTag + "event2 has a x=0 and y=0 location, is that intended?");
                            }
                            if ((trg.Event3Type == "transition") && ((trg.Event3TransPointX == 0) && (trg.Event3TransPointY == 0)))
                            {
                                logText("TRIGGER ERROR: " + chkEnc.encounterName + ": trigger " + trg.TriggerTag + "event3 has a x=0 and y=0 location, is that intended?");
                            }
                            if ((trg.Event1Type != "none") && (trg.Event1FilenameOrTag == "none"))
                            {
                                logText("TRIGGER ERROR: " + chkEnc.encounterName + ": trigger " + trg.TriggerTag + ": event1 has type of " + trg.Event1Type + " but filename/tag of 'none'");
                            }
                            if ((trg.Event2Type != "none") && (trg.Event2FilenameOrTag == "none"))
                            {
                                logText("TRIGGER ERROR: " + chkEnc.encounterName + ": trigger " + trg.TriggerTag + ": event2 has type of " + trg.Event2Type + " but filename/tag of 'none'");
                            }
                            if ((trg.Event3Type != "none") && (trg.Event3FilenameOrTag == "none"))
                            {
                                logText("TRIGGER ERROR: " + chkEnc.encounterName + ": trigger " + trg.TriggerTag + ": event3 has type of " + trg.Event3Type + " but filename/tag of 'none'");
                            }
                        }
                        textlist += chkEnc.encounterName + ",";
                    }
                }
            }
            logText("Completed Checking Encounters: " + textlist);
        }
        public void CheckCasterNoKnownSpells()
        {
            foreach (Creature crt in gv.cc.allCreaturesList)
            {
                if ((crt.cr_ai == "GeneralCaster") && (crt.knownSpellsTags.Count == 0))
                {
                    logText("CREATURE ERROR: " + crt.cr_name + " has a 'GeneralCaster' AI, but has no 'KnownSpells' selected");
                }
            }
        }
        public void CheckDuplicateTagsOrResRef()
        {
            //check creatures, items, props, containers, shops, encounters, races, classes, spells, traits, journal
            foreach (Creature crt in gv.cc.allCreaturesList)
            {
                foreach (Creature crtck in gv.cc.allCreaturesList)
                {
                    if ((crt != crtck) && (crt.cr_tag.Equals(crtck.cr_tag)))
                    {
                        logText("CREATURE ERROR: " + crt.cr_name + " has the same tag as " + crtck.cr_name);
                    }
                    if ((crt != crtck) && (crt.cr_resref.Equals(crtck.cr_resref)))
                    {
                        logText("CREATURE ERROR: " + crt.cr_name + " has the same resref as " + crtck.cr_name);
                    }
                }
            }
            foreach (Item crt in gv.cc.allItemsList)
            {
                foreach (Item crtck in gv.cc.allItemsList)
                {
                    if ((crt != crtck) && (crt.tag.Equals(crtck.tag)))
                    {
                        logText("ITEM ERROR: " + crt.name + " has the same tag as " + crtck.name);
                    }
                    if ((crt != crtck) && (crt.resref.Equals(crtck.resref)))
                    {
                        logText("ITEM ERROR: " + crt.name + " has the same resref as " + crtck.name);
                    }
                }
            }
            foreach (Container it in gv.mod.moduleContainersList)
            {
                foreach (Container itck in gv.mod.moduleContainersList)
                {
                    if ((it != itck) && (it.containerTag.Equals(itck.containerTag)))
                    {
                        logText("CONTAINER ERROR: " + it.containerTag + " has the same tag as " + itck.containerTag);
                    }
                }
            }
            foreach (Shop it in gv.mod.moduleShopsList)
            {
                foreach (Shop itck in gv.mod.moduleShopsList)
                {
                    if ((it != itck) && (it.shopTag.Equals(itck.shopTag)))
                    {
                        logText("SHOP ERROR: " + it.shopName + " has the same tag as " + itck.shopName);
                    }
                }
            }
        }
        public void CheckAmmoTypes()
        {
            //go through all items and see if ammo type is used and find any items that have unique ammo name not used anywhere else
            //maybe state all types and how many times they show up
        }
        public void CheckContainers()
        {
            //check for empty containers
            foreach (Container c in gv.mod.moduleContainersList)
            {
                logText("Checking Container: " + c.containerTag);
                if (c.containerItemRefs.Count == 0)
                {
                    logText("CONTAINER ERROR: " + c.containerTag + " has no items");
                }
                //check to see if container has items that do not exist in allitemslist
                foreach (ItemRefs ir in c.containerItemRefs)
                {
                    Item it = gv.cc.getItemByResRef(ir.resref);
                    if (it == null)
                    {
                        logText("CONTAINER ERROR: " + c.containerTag + " has an item " + ir.name + " that does not exist in the module's or default items list");
                    }
                }
            }
        }
        public void CheckShops()
        {
            //check for empty containers
            foreach (Shop c in gv.mod.moduleShopsList)
            {
                logText("Checking Shops: " + c.shopTag);
                if (c.shopItemRefs.Count == 0)
                {
                    logText("CONTAINER ERROR: " + c.shopTag + " has no items");
                }
                //check to see if container has items that do not exist in allitemslist
                foreach (ItemRefs ir in c.shopItemRefs)
                {
                    Item it = gv.cc.getItemByResRef(ir.resref);
                    if (it == null)
                    {
                        logText("SHOP ERROR: " + c.shopTag + " has an item " + ir.name + " that does not exist in the module's or default items list");
                    }
                }
            }
        }
    }
}
