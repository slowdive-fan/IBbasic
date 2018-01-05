using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IBbasic
{
    public class CommonCode
    {
        //this class is handled differently than Android version
        public GameView gv;

        public bool blockSecondPropTriggersCall = false;
        public List<FloatyText> floatyTextList = new List<FloatyText>();
        public int floatyTextCounter = 0;
        public bool floatyTextOn = false;
        public IbbButton btnHelp = null;
        public int partyScreenPcIndex = 0;
        public int partyItemSlotIndex = 0;
        public string slotA = "Autosave";
        public string slot0 = "Quicksave";
        public string slot1 = "";
        public string slot2 = "";
        public string slot3 = "";
        public string slot4 = "";
        public string slot5 = "";

        public SKBitmap walkPass;
        public SKBitmap walkBlocked;
        public SKBitmap losBlocked;
        public SKBitmap hitSymbol;
        public SKBitmap missSymbol;
        public SKBitmap highlight_green;
        public SKBitmap highlight_red;
        public SKBitmap black_tile;
        public SKBitmap turn_marker;
        public SKBitmap map_marker;
        public SKBitmap pc_dead;
        public SKBitmap pc_stealth;
        public SKBitmap tint_dawn;
        public SKBitmap tint_sunrise;
        public SKBitmap tint_sunset;
        public SKBitmap tint_dusk;
        public SKBitmap tint_night;
        public SKBitmap ui_bg_fullscreen;
        public SKBitmap ui_portrait_frame;
        public SKBitmap facing1;
        public SKBitmap facing2;
        public SKBitmap facing3;
        public SKBitmap facing4;
        public SKBitmap facing6;
        public SKBitmap facing7;
        public SKBitmap facing8;
        public SKBitmap facing9;
        
        //public Dictionary<string, Bitmap> tileBitmapList = new Dictionary<string, Bitmap>();
        public Dictionary<string, SKBitmap> commonBitmapList = new Dictionary<string, SKBitmap>();
        public Dictionary<string, SKBitmap> moduleBitmapList = new Dictionary<string, SKBitmap>();
        public Dictionary<string, SKBitmap> tileGDIBitmapList = new Dictionary<string, SKBitmap>();
        public Data datafile = new Data();
        public List<Item> allItemsList = new List<Item>();
        public List<Creature> allCreaturesList = new List<Creature>();
        public List<Prop> allPropsList = new List<Prop>();
        public List<ScriptObject> scriptList = new List<ScriptObject>();

        public Spell currentSelectedSpell = new Spell();
        public Trait currentSelectedTrait = new Trait();
        public string floatyText = "";
        public string floatyText2 = "";
        public string floatyText3 = "";
        public Coordinate floatyTextLoc = new Coordinate();
        public int creatureIndex = 0;
        public bool calledConvoFromProp = false;
        public bool calledEncounterFromProp = false;
        public int currentPlayerIndexUsingItem = 0;

        public string stringBeginnersGuide = "";
        public string stringPlayersGuide = "";
        public string stringPcCreation = "";
        public string stringMessageCombat = "";
        public string stringMessageInventory = "";
        public string stringMessageParty = "";
        public string stringMessageMainMap = "";

        public bool doOnEnterAreaUpdate = false;

        //Script options lists
        public List<string> variablesUsedList = new List<string>();
        public List<string> samllIntList = new List<string>();
        public List<string> largeIntList = new List<string>();
        public List<string> basicOpList = new List<string>();
        public List<string> fullOpList = new List<string>();
        public List<string> itemTagsList = new List<string>();
        public List<string> playerIndexList = new List<string>();
        public List<string> mapXList = new List<string>();
        public List<string> mapYList = new List<string>();
        public List<string> pcFilenameList = new List<string>();


        public CommonCode(GameView g)
        {
            gv = g;
            LoadAllScripts();
        }

        //LOAD SCRIPTS
        public void LoadAllScripts()
        {
            ScriptObject newScript = new ScriptObject();
            newScript.name = "gaSetGlobalInt";
            newScript.description = "Set a global Int (create a new one if currently doesn't exist)";
            newScript.parmType1 = "variable";
            newScript.parmDescription1 = "(string) global variable name";
            newScript.parmType2 = "small_int";
            newScript.parmDescription2 = "(int) value or (string) increment '++' or (string) decrement '--'";
            scriptList.Add(newScript);

            newScript = null;
            newScript = new ScriptObject();
            newScript.name = "gcCheckGlobalInt";
            newScript.description = "Check to see if a global Int has a value";
            newScript.parmType1 = "variable";
            newScript.parmDescription1 = "(string) global variable name";
            newScript.parmType2 = "basic_chk_oper";
            newScript.parmDescription2 = "(string) compare type ( = , < , > , ! )";
            newScript.parmType3 = "small_int";
            newScript.parmDescription3 = "(int) value";
            scriptList.Add(newScript);
        }
        public void ResetAllVariablesUsedList()
        {
            variablesUsedList.Clear();
            //add portrait images to the graphics needed list
            foreach (Convo cnv in gv.mod.moduleConvoList)
            {
                foreach (ContentNode subNode in cnv.subNodes)
                {
                    getAllNodeVariables(subNode);
                }
            }
        }
        public void getAllNodeVariables(ContentNode node)
        {
            foreach (Condition c in node.conditions)
            {
                if (c.c_script.Equals("gcCheckGlobalInt.cs"))
                {
                    addToVariablesList(c.c_parameter_1);
                }                      
            }
            foreach (Action a in node.actions)
            {
                if (a.a_script.Equals("gaSetGlobalInt.cs"))
                {
                    addToVariablesList(a.a_parameter_1);
                }
            }
            foreach (ContentNode subNode in node.subNodes)
            {
                getAllNodeVariables(subNode);
            }
        }
        public void addToVariablesList(string toAdd)
        {
            if (!variablesUsedList.Contains(toAdd))
            {
                variablesUsedList.Add(toAdd);
            }
        }
        public ScriptObject GetScriptObjectByName(string name)
        {
            foreach (ScriptObject s in scriptList)
            {
                if (s.name.Equals(name))
                {
                    return s;
                }
            }
            return new ScriptObject();
        }

        //LOAD FILES
        public void LoadTestParty()
        {
            if ((gv.mod.defaultPlayerFilename.Equals("")) || (gv.mod.defaultPlayerFilename.Equals("none")))
            {
                if (gv.mod.defaultPlayerFilenameList.Count > 0)
                {
                    foreach (StringForDropDownList pcname in gv.mod.defaultPlayerFilenameList)
                    {
                        gv.sf.AddCharacterToParty(pcname.stringValue);
                    }
                }
            }
            else
            {
                gv.sf.AddCharacterToParty(gv.mod.defaultPlayerFilename); //drin.json is default
            }            
            gv.mod.partyTokenFilename = "prp_party";
        }
        public Player LoadPlayer(string filename)
        {
            Player toReturn = null;

            //try finding player in module companionPlayerList first
            string nameMinusJson = filename.Replace(".json", "");
            foreach (Player p in gv.mod.companionPlayerList)
            {
                if (p.name.Equals(nameMinusJson))
                {
                    return p.DeepCopy();
                }
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(gv.mainDirectory + "\\default\\NewModule\\data\\" + filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (Player)serializer.Deserialize(file, typeof(Player));
            }
            return toReturn;
        }
        public void QuickSave()
        {
            try
            {
                //QuickSave();
                Player pc = gv.mod.playerList[0];
                if (pc == null) { return; }
                gv.mod.saveName = pc.name + ", Lvl:" + pc.classLevel + ", XP:" + pc.XP + ", Time:" + gv.mod.WorldTime;
                slot0 = "Quicksave - " + gv.mod.saveName;
                SaveSaveGame("quicksave.json");
            }
            catch (Exception ex)
            {
                gv.sf.MessageBox("Failed to Save: Not enough free memory(RAM) on device, try and free up some memory and try again.");
                gv.errorLog(ex.ToString());
            }
        }
        public void doSavesDialog(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                Player pc = gv.mod.playerList[0];
                if (pc == null) { return; }
                gv.mod.saveName = pc.name + ", Lvl:" + pc.classLevel + ", XP:" + pc.XP + ", Time:" + gv.mod.WorldTime;
                slot0 = "Quicksave - " + gv.mod.saveName;
                try
                {
                    SaveSaveGame("quicksave.json");
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("Failed to Save: Not enough free memory(RAM) on device, try and free up some memory and try again.");
                    gv.errorLog(ex.ToString());
                }
            }
            else if (selectedIndex == 1)
            {
                Player pc = gv.mod.playerList[0];
                gv.mod.saveName = pc.name + ", Level:" + pc.classLevel + ", XP:" + pc.XP + ", WorldTime:" + gv.mod.WorldTime;
                slot1 = gv.mod.saveName;
                try
                {
                    SaveSaveGame("slot1.json");
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("Failed to Save: Not enough free memory(RAM) on device, try and free up some memory and try again.");
                    gv.errorLog(ex.ToString());
                }
            }
            else if (selectedIndex == 2)
            {
                Player pc = gv.mod.playerList[0];
                gv.mod.saveName = pc.name + ", Level:" + pc.classLevel + ", XP:" + pc.XP + ", WorldTime:" + gv.mod.WorldTime;
                slot2 = gv.mod.saveName;
                try
                {
                    SaveSaveGame("slot2.json");
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("Failed to Save: Not enough free memory(RAM) on device, try and free up some memory and try again.");
                    gv.errorLog(ex.ToString());
                }
            }
            else if (selectedIndex == 3)
            {
                Player pc = gv.mod.playerList[0];
                gv.mod.saveName = pc.name + ", Level:" + pc.classLevel + ", XP:" + pc.XP + ", WorldTime:" + gv.mod.WorldTime;
                slot3 = gv.mod.saveName;
                try
                {
                    SaveSaveGame("slot3.json");
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("Failed to Save: Not enough free memory(RAM) on device, try and free up some memory and try again.");
                    gv.errorLog(ex.ToString());
                }
            }
            else if (selectedIndex == 4)
            {
                Player pc = gv.mod.playerList[0];
                gv.mod.saveName = pc.name + ", Level:" + pc.classLevel + ", XP:" + pc.XP + ", WorldTime:" + gv.mod.WorldTime;
                slot4 = gv.mod.saveName;
                try
                {
                    SaveSaveGame("slot4.json");
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("Failed to Save: Not enough free memory(RAM) on device, try and free up some memory and try again.");
                    gv.errorLog(ex.ToString());
                }
            }
            else if (selectedIndex == 5)
            {
                Player pc = gv.mod.playerList[0];
                gv.mod.saveName = pc.name + ", Level:" + pc.classLevel + ", XP:" + pc.XP + ", WorldTime:" + gv.mod.WorldTime;
                slot5 = gv.mod.saveName;
                try
                {
                    SaveSaveGame("slot5.json");
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("Failed to Save: Not enough free memory(RAM) on device, try and free up some memory and try again.");
                    gv.errorLog(ex.ToString());
                }
            }
            else if (selectedIndex == 6)
            {
                //ask if they really want to exit, remind to save first
                doVerifyReturnToMainSetup();
            }
        }
        public void doSavesSetupDialog()
        {
            List<string> saveList = new List<string> { slot0, slot1, slot2, slot3, slot4, slot5, "Return to Main Menu" };
            gv.itemListSelector.setupIBminiItemListSelector(gv, saveList, "Choose a slot to save game.", "savegame");
            gv.itemListSelector.showIBminiItemListSelector = true;
        }
        public void doVerifyReturnToMainSetup()
        {
            List<string> actionList = new List<string> { "Yes, Return To Main Menu", "No, Keep Playing" };
            gv.itemListSelector.setupIBminiItemListSelector(gv, actionList, "Are you sure you wish to exit to Main Menu?", "verifyreturntomain");
            gv.itemListSelector.showIBminiItemListSelector = true;
        }
        public void doVerifyReturnToMain(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                //go to launcher screen
                if (gv.fixedModule.Equals("")) //this is the IceBlink Engine app
                {
                    if (gv.screenLauncher == null)
                    {
                        gv.screenLauncher = new ScreenLauncher(gv.mod, gv);
                    }
                    if (gv.screenPartyBuild == null)
                    {
                        gv.screenPartyBuild = new ScreenPartyBuild(gv.mod, gv);
                    }
                    if (gv.screenPcCreation == null)
                    {
                        gv.screenPcCreation = new ScreenPcCreation(gv.mod, gv);
                    }
                    if (gv.screenTitle == null)
                    {
                        gv.screenTitle = new ScreenTitle(gv.mod, gv);
                    }
                    //TODO make sure this works
                    gv.screenLauncher.loadModuleInfoFiles();
                    gv.screenType = "launcher";
                }
                else //this is a fixed module
                {
                    gv.mod = gv.cc.LoadModule(gv.fixedModule + "/" + gv.fixedModule + ".mod");
                    gv.resetGame();
                    gv.cc.LoadSaveListItems();
                    gv.screenType = "title";
                }
            }
            if (selectedIndex == 1)
            {
                //keep playing
            }
        }
        public void doLoadSaveGameDialog(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                bool result = LoadSaveGame("autosave.json");
                if (result)
                {
                    gv.screenType = "main";
                    doUpdate();
                }
                else
                {
                    //Toast.makeText(gv.gameContext, "Save file not found", Toast.LENGTH_SHORT).show();
                }
            }
            else if (selectedIndex == 1)
            {
                bool result = LoadSaveGame("quicksave.json");
                if (result)
                {
                    gv.screenType = "main";
                    doUpdate();
                }
                else
                {
                    //Toast.makeText(gv.gameContext, "Save file not found", Toast.LENGTH_SHORT).show();
                }
            }
            else if (selectedIndex == 2)
            {
                bool result = LoadSaveGame("slot1.json");
                if (result)
                {
                    gv.screenType = "main";
                    doUpdate();
                }
                else
                {
                    //Toast.makeText(gv.gameContext, "Save file not found", Toast.LENGTH_SHORT).show();
                }
            }
            else if (selectedIndex == 3)
            {
                bool result = LoadSaveGame("slot2.json");
                if (result)
                {
                    gv.screenType = "main";
                    doUpdate();
                }
                else
                {
                    //Toast.makeText(gv.gameContext, "Save file not found", Toast.LENGTH_SHORT).show();
                }
            }
            else if (selectedIndex == 4)
            {
                bool result = LoadSaveGame("slot3.json");
                if (result)
                {
                    gv.screenType = "main";
                    doUpdate();
                }
                else
                {
                    //Toast.makeText(gv.gameContext, "Save file not found", Toast.LENGTH_SHORT).show();
                }
            }
            else if (selectedIndex == 5)
            {
                bool result = LoadSaveGame("slot4.json");
                if (result)
                {
                    gv.screenType = "main";
                    doUpdate();
                }
                else
                {
                    //Toast.makeText(gv.gameContext, "Save file not found", Toast.LENGTH_SHORT).show();
                }
            }
            else if (selectedIndex == 6)
            {
                bool result = LoadSaveGame("slot5.json");
                if (result)
                {
                    gv.screenType = "main";
                    doUpdate();
                }
                else
                {
                    //Toast.makeText(gv.gameContext, "Save file not found", Toast.LENGTH_SHORT).show();
                }
            }
        }
        public void doLoadSaveGameSetupDialog()
        {
            List<string> saveList = new List<string> { slotA, slot0, slot1, slot2, slot3, slot4, slot5 };
            gv.itemListSelector.setupIBminiItemListSelector(gv, saveList, "Choose a Saved Game to Load.", "loadsavegame");
            gv.itemListSelector.showIBminiItemListSelector = true;
        }
        public SaveGame LoadModuleInfo(string filename)
        {
            SaveGame m = new SaveGame();
            try
            {
                using (StreamReader file = File.OpenText(gv.mainDirectory + "\\saves\\" + gv.mod.moduleName + "\\" + filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    m = (SaveGame)serializer.Deserialize(file, typeof(SaveGame));
                }
            }
            catch { }
            return m;
        }
        public void LoadSaveListItems()
        {
            slot0 = "Quicksave - " + LoadModuleInfo("quicksave.json").saveName;
            slot1 = LoadModuleInfo("slot1.json").saveName;
            slot2 = LoadModuleInfo("slot2.json").saveName;
            slot3 = LoadModuleInfo("slot3.json").saveName;
            slot4 = LoadModuleInfo("slot4.json").saveName;
            slot5 = LoadModuleInfo("slot5.json").saveName;
        }

        //SAVE SAVEGAME
        public void SaveSaveGame(string filename)
        {
            SaveGame saveMod = new SaveGame();

            saveMod.saveName = gv.mod.saveName;
            saveMod.playerList = new List<Player>();
            foreach (Player pc in gv.mod.playerList)
            {
                saveMod.playerList.Add(pc.DeepCopy());
            }
            saveMod.partyRosterList = new List<Player>();
            foreach (Player pc in gv.mod.partyRosterList)
            {
                saveMod.partyRosterList.Add(pc.DeepCopy());
            }
            saveMod.partyJournalQuests.Clear();
            foreach (JournalQuest jq in gv.mod.partyJournalQuests)
            {
                JournalQuest savJQ = jq.DeepCopy();
                saveMod.partyJournalQuests.Add(savJQ);                
            }
            saveMod.partyInventoryRefsList.Clear();
            foreach (ItemRefs s in gv.mod.partyInventoryRefsList)
            {
                saveMod.partyInventoryRefsList.Add(s.DeepCopy());
            }
            saveMod.moduleShopsList.Clear();
            foreach (Shop shp in gv.mod.moduleShopsList)
            {
                saveMod.moduleShopsList.Add(shp.DeepCopy());
            }
            saveMod.moduleAreasObjects.Clear();
            foreach (Area ar in gv.mod.moduleAreasObjects)
            {
                AreaSave sar = new AreaSave();
                sar.Filename = ar.Filename;
                sar.Visible.Clear();
                foreach (int vis in ar.Visible)
                {
                    sar.Visible.Add(vis);
                }
                /*sar.Props.Clear();
                foreach (Prop prp in ar.Props)
                {
                    PropSave sprp = new PropSave();
                    sprp.PropTag = prp.PropTag;
                    sprp.LocationX = prp.LocationX;
                    sprp.LocationY = prp.LocationY;
                    sprp.PropFacingLeft = prp.PropFacingLeft;
                    sprp.isShown = prp.isShown;
                    sprp.isActive = prp.isActive;
                    sar.Props.Add(sprp);
                }
                sar.InitialAreaPropTagsList.Clear();
                foreach (string prp in ar.InitialAreaPropTagsList)
                {                    
                    sar.InitialAreaPropTagsList.Add(prp);
                }*/
                sar.Triggers.Clear();
                foreach (Trigger tr in ar.Triggers)
                {
                    TriggerSave str = new TriggerSave();
                    str.TriggerTag = tr.TriggerTag;
                    str.Enabled = tr.Enabled;
                    str.EnabledEvent1 = tr.EnabledEvent1;
                    str.EnabledEvent2 = tr.EnabledEvent2;
                    str.EnabledEvent3 = tr.EnabledEvent3;
                    sar.Triggers.Add(str);
                }
                saveMod.moduleAreasObjects.Add(sar);
            }
            saveMod.currentAreaFilename = gv.mod.currentArea.Filename;
            saveMod.moduleContainersList.Clear();
            foreach (Container cnt in gv.mod.moduleContainersList)
            {
                saveMod.moduleContainersList.Add(cnt.DeepCopy());
            }
            saveMod.moduleConvoSavedValuesList.Clear();
            foreach (ConvoSavedValues csv in gv.mod.moduleConvoSavedValuesList)
            {
                saveMod.moduleConvoSavedValuesList.Add(csv.DeepCopy());
            }
            saveMod.moduleEncountersCompletedList.Clear();
            foreach (Encounter enc in gv.mod.moduleEncountersList)
            {
                EncounterSave senc = new EncounterSave();
                senc.encounterName = enc.encounterName;
                if (enc.encounterCreatureRefsList.Count <= 0)
                {
                    senc.completed = true;
                }
                else
                {
                    senc.completed = false;
                }
                saveMod.moduleEncountersCompletedList.Add(senc);
            }
            saveMod.moduleGlobalInts.Clear();
            foreach (GlobalInt g in gv.mod.moduleGlobalInts)
            {
                saveMod.moduleGlobalInts.Add(g.DeepCopy());
            }
            saveMod.moduleGlobalStrings.Clear();
            foreach (GlobalString g in gv.mod.moduleGlobalStrings)
            {
                saveMod.moduleGlobalStrings.Add(g.DeepCopy());
            }
            saveMod.partyGold = gv.mod.partyGold;
            saveMod.WorldTime = gv.mod.WorldTime;
            saveMod.PlayerLocationY = gv.mod.PlayerLocationY;
            saveMod.PlayerLocationX = gv.mod.PlayerLocationX;
            saveMod.PlayerLastLocationY = gv.mod.PlayerLastLocationY;
            saveMod.PlayerLastLocationX = gv.mod.PlayerLastLocationX;
            saveMod.selectedPartyLeader = gv.mod.selectedPartyLeader;
            saveMod.showTutorialCombat = gv.mod.showTutorialCombat;
            saveMod.showTutorialInventory = gv.mod.showTutorialInventory;
            saveMod.showTutorialParty = gv.mod.showTutorialParty;
            saveMod.showTutorialCombat = gv.mod.showTutorialCombat;
            saveMod.showTutorialInventory = gv.mod.showTutorialInventory;
            saveMod.showTutorialParty = gv.mod.showTutorialParty;
            saveMod.minutesSinceLastRationConsumed = gv.mod.minutesSinceLastRationConsumed;

            //SAVE THE FILE
            string filepath = gv.mainDirectory + "\\saves\\" + gv.mod.moduleName + "\\" + filename;
            MakeDirectoryIfDoesntExist(filepath);
            string json = JsonConvert.SerializeObject(saveMod, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.Write(json.ToString());
            }

            //SAVE THE FILE
            //filepath = "C:\\Users\\Slowdive\\Dropbox\\IceBlink2mini\\saves\\" + gv.mod.moduleName + "\\" + filename;
            //MakeDirectoryIfDoesntExist(filepath);
            //json = JsonConvert.SerializeObject(saveMod, Newtonsoft.Json.Formatting.Indented);
            //using (StreamWriter sw = new StreamWriter(filepath))
            //{
            //    sw.Write(json.ToString());
            //}
        }
        //LOAD SAVEGAME
        public bool LoadSaveGame(string filename)
        {
            //  load a new module (actually already have a new module at this point from launch screen		
            //  load the saved game module
            SaveGame saveMod = null;
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(gv.mainDirectory + "\\saves\\" + gv.mod.moduleName + "\\" + filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                saveMod = (SaveGame)serializer.Deserialize(file, typeof(SaveGame));
            }
            if (saveMod == null) { return false; }
            //  replace parts of new module with parts of saved game module
            //
            // U = update from save file	 
            //
            //U  "saveName": "Drin, Level:1, XP:150, WorldTime:24", (use all save)
            gv.mod.saveName = saveMod.saveName;
            //U  "playerList": [], (use all save)  Update PCs later further down
            gv.mod.playerList = new List<Player>();
            foreach (Player pc in saveMod.playerList)
            {
                gv.mod.playerList.Add(pc.DeepCopy());
            }
            //backwards compatible with Elderin Stone Paladin
            foreach (Player pc in gv.mod.playerList)
            {
                if (pc.classTag.Equals("newPlayerClassTag_525"))
                {
                    pc.classTag = "paladin";
                }
            }
            setMainPc();
            //U  "partyRosterList": [], (use all save)  Update PCs later further down
            gv.mod.partyRosterList = new List<Player>();
            foreach (Player pc in saveMod.partyRosterList)
            {
                gv.mod.partyRosterList.Add(pc.DeepCopy());
            }
            //backwards compatible with Elderin Stone Paladin
            foreach (Player pc in gv.mod.partyRosterList)
            {
                if (pc.classTag.Equals("newPlayerClassTag_525"))
                {
                    pc.classTag = "paladin";
                }
            }
            //U  "partyJournalQuests": [], (use tags from save to get all from new)
            gv.mod.partyJournalQuests.Clear();
            foreach (JournalQuest jq in saveMod.partyJournalQuests)
            {
                foreach (JournalEntry je in jq.Entries)
                {
                    gv.sf.AddJournalEntryNoMessages(jq.Tag, je.Tag);
                }
            }
            //U  "partyInventoryTagList": [], (use all save) update Items later on down
            gv.mod.partyInventoryRefsList.Clear();
            foreach (ItemRefs s in saveMod.partyInventoryRefsList)
            {
                gv.mod.partyInventoryRefsList.Add(s.DeepCopy());
            }
            //U  "moduleShopsList": [], (have an original shop items tags list and the current tags list to see what to add or delete from the save tags list)
            this.updateShops(saveMod);
            //U  "moduleAreasObjects": [],
            //                (triggers: use save trigger "enabled" value to update new)
            //                (tiles: use save "visible" to update new)
            //                (props: have an original props tags list and the current tags list to see what to add or delete from the save tags list)		               
            this.updateAreas(saveMod);
            //
            //U  "currentArea": {},
            bool foundArea = gv.mod.setCurrentArea(saveMod.currentAreaFilename, gv);
            if (!foundArea)
            {
                gv.sf.MessageBox("Area: " + saveMod.currentAreaFilename + " does not exist in the module...maybe the area in the module was changed since this save game was made.");
            }
            //U  "moduleContainersList": [], (have an original containers items tags list and the current tags list to see what to add or delete from the save tags list)
            this.updateContainers(saveMod);
            //U  "moduleConvoSavedValuesList": [], (use all save)
            gv.mod.moduleConvoSavedValuesList.Clear();
            foreach (ConvoSavedValues csv in saveMod.moduleConvoSavedValuesList)
            {
                gv.mod.moduleConvoSavedValuesList.Add(csv.DeepCopy());
            }
            //U  "moduleEncountersList": [], (use new except delete those completed already in save)
            foreach (EncounterSave enc in saveMod.moduleEncountersCompletedList)
            {
                if (enc.completed)
                {
                    //if the encounter was completed in the saveMod then clear all creatures in the newMod
                    Encounter e = gv.mod.getEncounter(enc.encounterName);
                    e.encounterCreatureList.Clear();
                    e.encounterCreatureRefsList.Clear();
                }
            }
            //U  "moduleGlobalInts": [], (use all save)
            gv.mod.moduleGlobalInts.Clear();
            foreach (GlobalInt g in saveMod.moduleGlobalInts)
            {
                gv.mod.moduleGlobalInts.Add(g.DeepCopy());
            }
            //U  "moduleGlobalStrings": [], (use all save)
            gv.mod.moduleGlobalStrings.Clear();
            foreach (GlobalString g in saveMod.moduleGlobalStrings)
            {
                gv.mod.moduleGlobalStrings.Add(g.DeepCopy());
            }
            //U  "partyGold": 70, (use all save)
            gv.mod.partyGold = saveMod.partyGold;
            //U  "WorldTime": 24, (use all save)
            gv.mod.WorldTime = saveMod.WorldTime;
            //U  "PlayerLocationY": 2, (use all save)
            gv.mod.PlayerLocationY = saveMod.PlayerLocationY;
            //U  "PlayerLocationX": 1, (use all save)
            gv.mod.PlayerLocationX = saveMod.PlayerLocationX;
            //U  "PlayerLastLocationY": 1, (use all save)
            gv.mod.PlayerLastLocationY = saveMod.PlayerLastLocationY;
            //U  "PlayerLastLocationX": 2, (use all save)
            gv.mod.PlayerLastLocationX = saveMod.PlayerLastLocationX;
            //U  "selectedPartyLeader": 0, (use all save)
            gv.mod.selectedPartyLeader = saveMod.selectedPartyLeader;
            //U  "showTutorialCombat": true, (use all save)
            gv.mod.showTutorialCombat = saveMod.showTutorialCombat;
            //U  "showTutorialInventory": true, (use all save)
            gv.mod.showTutorialInventory = saveMod.showTutorialInventory;
            //U  "showTutorialParty": true, (use all save)
            gv.mod.showTutorialParty = saveMod.showTutorialParty;
            gv.mod.minutesSinceLastRationConsumed = saveMod.minutesSinceLastRationConsumed;
            
            //gv.initializeSounds();

            gv.mod.partyTokenFilename = "prp_party";
            //gv.mod.partyTokenBitmap = this.LoadBitmap(gv.mod.partyTokenFilename);

            this.updatePlayers();
            this.updatePartyRosterPlayers();

            gv.createScreens();
            gv.screenMainMap.resetMiniMapBitmap();
            return true;
        }
        public void updateShops(SaveGame saveMod)
        {
            foreach (Shop saveShp in saveMod.moduleShopsList)
            {
                Shop updatedShop = gv.mod.getShopByTag(saveShp.shopTag);
                if (updatedShop != null)
                {
                    //use the buyback and sell modifier values from the save
                    updatedShop.buybackModifier = saveShp.buybackModifier;
                    updatedShop.sellModifier = saveShp.sellModifier;
                    //this shop in the save also exists in the newMod so clear it out and add everything in the save
                    updatedShop.shopItemRefs.Clear();
                    foreach (ItemRefs it in saveShp.shopItemRefs)
                    {
                        Item newItem = gv.cc.getItemByResRef(it.resref);
                        if (newItem != null)
                        {
                            updatedShop.shopItemRefs.Add(it.DeepCopy());
                        }
                    }
                    //compare lists and add items that are new
                    for (int i = updatedShop.initialShopItemRefs.Count - 1; i >= 0; i--)
                    {
                        ItemRefs itemRef = updatedShop.initialShopItemRefs[i];
                        if (!saveShp.containsInitialItemWithResRef(itemRef.resref))
                        {
                            //item is not in the saved game initial container item list so add it to the container
                            Item newItem = gv.cc.getItemByResRef(itemRef.resref);
                            if (newItem != null)
                            {
                                updatedShop.shopItemRefs.Add(itemRef.DeepCopy());
                                //make sure to add to initial list so it doesn't keep getting duplicated with every load save
                                updatedShop.initialShopItemRefs.Add(itemRef.DeepCopy());
                            }
                        }
                    }
                }
            }
        }
        public void updateAreas(SaveGame saveMod)
        {
            foreach (Area ar in gv.mod.moduleAreasObjects)
            {
                foreach (AreaSave sar in saveMod.moduleAreasObjects)
                {
                    if (sar.Filename.Equals(ar.Filename)) //sar is saved game, ar is new game from toolset version
                    {
                        //tiles
                        for (int index = 0; index < ar.Visible.Count; index++)
                        {
                            ar.Visible[index] = sar.Visible[index];
                        }

                        //props
                        //start at the end of the newMod prop list and work up
                        //if the prop tag is found in the save game, update it
                        //else if not found in saved game, but exists in the 
                        //saved game initial list (the toolset version of the prop list), remove prop
                        //else leave it alone
                        /*for (int index = ar.Props.Count - 1; index >= 0; index--)
                        {
                            Prop prp = ar.Props[index];
                            bool foundOne = false;
                            foreach (PropSave sprp in sar.Props) //sprp is the saved game prop
                            {
                                if (prp.PropTag.Equals(sprp.PropTag))
                                {
                                    foundOne = true; //the prop tag exists in the saved game
                                    //replace the one in the toolset with the one from the saved game
                                    prp.LocationX = sprp.LocationX;
                                    prp.LocationY = sprp.LocationY;
                                    prp.PropFacingLeft = sprp.PropFacingLeft;
                                    prp.isShown = sprp.isShown;
                                    prp.isActive = sprp.isActive;
                                    break;
                                }
                            }
                            if (!foundOne) //didn't find the prop tag in the saved game
                            {
                                if (sar.InitialAreaPropTagsList.Contains(prp.PropTag))
                                {
                                    //was once on the map, but was deleted so remove from the newMod prop list
                                    ar.Props.RemoveAt(index);
                                }
                                else
                                {
                                    //is new to the mod so leave it alone, don't remove from the prop list
                                }
                            }
                        }*/
                        //triggers
                        foreach (Trigger tr in ar.Triggers)
                        {
                            foreach (TriggerSave str in sar.Triggers)
                            {
                                if (tr.TriggerTag.Equals(str.TriggerTag))
                                {
                                    tr.Enabled = str.Enabled;
                                    tr.EnabledEvent1 = str.EnabledEvent1;
                                    tr.EnabledEvent2 = str.EnabledEvent2;
                                    tr.EnabledEvent3 = str.EnabledEvent3;
                                    //may want to copy the trigger's squares list from the save game if builders can modify the list with scripts
                                }
                            }
                        }
                    }
                }
            }
        }
        public void updateContainers(SaveGame saveMod)
        {
            foreach (Container saveCnt in saveMod.moduleContainersList)
            {
                //fill container with items that are still in the saved game 
                Container updatedCont = gv.mod.getContainerByTag(saveCnt.containerTag);
                if (updatedCont != null)
                {
                    //this container in the save also exists in the newMod so clear it out and add everything in the save
                    updatedCont.containerItemRefs.Clear();
                    foreach (ItemRefs it in saveCnt.containerItemRefs)
                    {
                        //check to see if item resref in save game container still exists in toolset
                        Item newItem = gv.cc.getItemByResRef(it.resref);
                        if (newItem != null)
                        {
                            updatedCont.containerItemRefs.Add(it.DeepCopy());
                        }
                    }
                    //compare lists and add items that are new
                    for (int i = updatedCont.initialContainerItemRefs.Count - 1; i >= 0; i--)
                    {
                        ItemRefs itemRef = updatedCont.initialContainerItemRefs[i];
                        //check to see if item in toolset does not exist in save initial list so it is new and add it
                        if (!saveCnt.containsInitialItemWithResRef(itemRef.resref))
                        {
                            //item is not in the saved game initial container item list so add it to the container
                            //check to see if item resref in save game container still exists in toolset
                            Item newItem = gv.cc.getItemByResRef(itemRef.resref);
                            if (newItem != null)
                            {
                                updatedCont.containerItemRefs.Add(itemRef.DeepCopy());
                                //make sure to add to initial list so it doesn't keep getting duplicated with every load save
                                updatedCont.initialContainerItemRefs.Add(itemRef.DeepCopy());
                            }
                        }
                    }
                }
            }
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
        public void updatePartyRosterPlayers()
        {
            foreach (Player pc in gv.mod.partyRosterList)
            {
                try { pc.race = gv.cc.getRace(pc.raceTag).DeepCopy(); }
                catch (Exception ex) { gv.errorLog(ex.ToString()); }
                try { pc.playerClass = gv.cc.getPlayerClass(pc.classTag).DeepCopy(); }
                catch (Exception ex) { gv.errorLog(ex.ToString()); }
            }
        }
        public void setMainPc()
        {
            foreach (Player pc in gv.mod.playerList)
            {
                if (pc.mainPc)
                {
                    return;
                }
            }
            if (gv.mod.playerList.Count > 0)
            {
                gv.mod.playerList[0].mainPc = true;
            }
        }

        //LOAD MODULE
        public Module LoadModule(string folderAndFilename)
        {
            Module toReturn = new Module();
            try
            {
                //used for opening the entire module files
                using (StreamReader sr = File.OpenText(GetModulePath(folderAndFilename) + "\\" + folderAndFilename))
                {
                    string s = "";
                    string keyword = "";
                    gv.mod.moduleImageDataList.Clear();
                    moduleBitmapList.Clear();
                    ImageData imd;
                    //toReturn.moduleAreasObjects.Clear();
                    Area ar;
                    //toReturn.moduleEncountersList.Clear();
                    Encounter enc;
                    //toReturn.moduleConvoList.Clear();
                    Convo cnv;

                    for (int i = 0; i < 99999; i++)
                    {
                        s = sr.ReadLine();

                        #region Look for keyword line
                        if (s == null)
                        {
                            break;
                        }
                        else if (s.Equals("END"))
                        {
                            break;
                        }
                        else if (s.Equals(""))
                        {
                            continue;
                        }
                        else if (s.Equals("MODULEINFO"))
                        {
                            keyword = "MODULEINFO";
                            continue;
                        }
                        else if (s.Equals("TITLEIMAGE"))
                        {
                            keyword = "TITLEIMAGE";
                            continue;
                        }
                        else if (s.Equals("MODULE"))
                        {
                            keyword = "MODULE";
                            continue;
                        }
                        else if (s.Equals("AREAS"))
                        {
                            keyword = "AREAS";
                            continue;
                        }
                        else if (s.Equals("ENCOUNTERS"))
                        {
                            keyword = "ENCOUNTERS";
                            continue;
                        }
                        else if (s.Equals("CONVOS"))
                        {
                            keyword = "CONVOS";
                            continue;
                        }
                        else if (s.Equals("IMAGES"))
                        {
                            keyword = "IMAGES";
                            continue;
                        }
                        #endregion

                        #region Process line if not a keyword line
                        if (keyword.Equals("MODULEINFO"))
                        {
                            continue;
                        }
                        else if (keyword.Equals("TITLEIMAGE"))
                        {
                            imd = (ImageData)JsonConvert.DeserializeObject(s, typeof(ImageData));
                            toReturn.moduleImageDataList.Add(imd);
                            //TODO moduleBitmapList.Add(imd.name, ConvertGDIBitmapToD2D(gv.bsc.ConvertImageDataToBitmap(imd)));
                        }
                        else if (keyword.Equals("MODULE"))
                        {
                            toReturn = (Module)JsonConvert.DeserializeObject(s, typeof(Module));
                        }
                        else if (keyword.Equals("AREAS"))
                        {
                            ar = (Area)JsonConvert.DeserializeObject(s, typeof(Area));
                            toReturn.moduleAreasObjects.Add(ar);
                        }
                        else if (keyword.Equals("ENCOUNTERS"))
                        {
                            enc = (Encounter)JsonConvert.DeserializeObject(s, typeof(Encounter));
                            toReturn.moduleEncountersList.Add(enc);
                        }
                        else if (keyword.Equals("CONVOS"))
                        {
                            cnv = (Convo)JsonConvert.DeserializeObject(s, typeof(Convo));
                            toReturn.moduleConvoList.Add(cnv);
                        }
                        else if (keyword.Equals("IMAGES"))
                        {
                            imd = (ImageData)JsonConvert.DeserializeObject(s, typeof(ImageData));
                            if (!toReturn.moduleImageDataList.Contains(imd))
                            {
                                toReturn.moduleImageDataList.Add(imd);
                            }
                            if (!moduleBitmapList.ContainsKey(imd.name))
                            {
                                //TODO moduleBitmapList.Add(imd.name, ConvertGDIBitmapToD2D(gv.bsc.ConvertImageDataToBitmap(imd)));
                            }
                        }
                        #endregion
                    }                    
                }
            }
            catch (Exception ex)
            {
                gv.sf.MessageBox("Failed to read Module for " + folderAndFilename + ": " + ex.ToString());
            }

            if (File.Exists("override/data.json"))
            {
                this.datafile = this.datafile.loadDataFile("override/data.json");
            }
            else if (File.Exists("default/NewModule/data/data.json"))
            {
                this.datafile = this.datafile.loadDataFile("default/NewModule/data/data.json");
            }
            //ITEMS
            allItemsList.Clear();
            foreach (Item it in datafile.dataItemsList)
            {
                allItemsList.Add(it.DeepCopy());
            }
            foreach (Item it in gv.mod.moduleItemsList)
            {
                bool foundOne = false;
                foreach (Item it2 in datafile.dataItemsList)
                {
                    if (it.resref == it2.resref)
                    {
                        foundOne = true;
                    }
                }
                if (!foundOne)
                {
                    //it.moduleItem = true;
                    allItemsList.Add(it.DeepCopy());
                }
            }
            //CREATURES
            allCreaturesList.Clear();
            foreach (Creature it in datafile.dataCreaturesList)
            {
                allCreaturesList.Add(it.DeepCopy());
            }
            foreach (Creature it in gv.mod.moduleCreaturesList)
            {
                bool foundOne = false;
                foreach (Creature it2 in this.datafile.dataCreaturesList)
                {
                    if (it.cr_resref == it2.cr_resref)
                    {
                        foundOne = true;
                    }
                }
                if (!foundOne)
                {
                    //it.moduleCreature = true;
                    allCreaturesList.Add(it.DeepCopy());
                }
            }

            return toReturn;
        }
        public ModuleInfo LoadModuleFileInfo(string folderAndFilename)
        {
            ModuleInfo toReturn = null;
            try
            {
                //used for loading up the launcher screen only
                using (StreamReader sr = File.OpenText(folderAndFilename))
                {
                    string s = "";
                    string keyword = "";
                    moduleBitmapList.Clear();
                    ImageData imd;

                    for (int i = 0; i < 99999; i++)
                    {
                        s = sr.ReadLine();

                        if (s == null)
                        {
                            break;
                        }
                        else if (s.Equals(""))
                        {
                            continue;
                        }
                        else if (s.Equals("MODULEINFO"))
                        {
                            keyword = "MODULEINFO";
                            continue;
                        }
                        else if (s.Equals("TITLEIMAGE"))
                        {
                            keyword = "TITLEIMAGE";
                            continue;
                        }
                        else if (s.Equals("MODULE"))
                        {
                            keyword = "MODULE";
                            break;
                        }

                        if (keyword.Equals("MODULEINFO"))
                        {
                            toReturn = (ModuleInfo)JsonConvert.DeserializeObject(s, typeof(ModuleInfo));
                        }
                        else if (keyword.Equals("TITLEIMAGE"))
                        {
                            imd = (ImageData)JsonConvert.DeserializeObject(s, typeof(ImageData));
                            //TODO moduleBitmapList.Add(imd.name, ConvertGDIBitmapToD2D(gv.bsc.ConvertImageDataToBitmap(imd)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gv.sf.MessageBox("Failed to open ModuleInfo for " + folderAndFilename + ": " + ex.ToString());
            }  
            
            return toReturn;
        }        
        public string GetModulePath(string modname)
        {
            if (modname.Equals("NewModule.mod"))
            {
                return gv.mainDirectory + "\\default\\NewModule";
            }
            return gv.mainDirectory + "\\modules";
        }        
        //SAVE MODULE
        public void saveFiles()
        {
            if ((gv.mod.startingArea == null) || (gv.mod.startingArea == ""))
            {
                gv.sf.MessageBox("Starting area was not detected, please type in the starting area's name in module properties (Edit/Modules Properties). Your module will not work without a starting area defined.");
                //return;
            }
            if ((gv.mod.moduleName.Length == 0) || (gv.mod.moduleName == "NewModule"))
            {
                saveAsFiles();
                return;
            }
            string file = gv.mainDirectory + "\\modules\\" + gv.mod.moduleName + ".mod";
            try
            {
                createFiles(file);
                gv.sf.MessageBox("Moduled saved");
            }
            catch (Exception e)
            {
                gv.sf.MessageBox("failed to save module: " + e.ToString());
            }
        }
        public void saveAsFiles()
        {
            /*
            using (TextInputDialog itSel = new TextInputDialog(gv, "Enter the new file save name for this Module.", gv.mod.moduleName))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (itSel.textInput.Length > 0)
                    {
                        gv.mod.moduleName = itSel.textInput;
                    }
                    else
                    {
                        gv.sf.MessageBox("Blank entry is not allowed");
                    }
                }
            }
            saveFiles();
            */
        }
        public void incrementalSave() //incremental save option
        {
            if ((gv.mod.startingArea == null) || (gv.mod.startingArea == ""))
            {
                gv.sf.MessageBox("Starting area was not detected, please select a starting area in the module's properties. Your module will not work without a starting area defined.");
            }
            else
            {
                // save a backup with an incremental file name
                string workingDir = gv.mainDirectory + "\\modules";
                if (!Directory.Exists(workingDir)) // if folder does not exist, create it and copy contents from previous folder
                {
                    createDirectory(workingDir);
                }
                string backupDir = gv.mainDirectory + "\\module_backups";
                if (!Directory.Exists(backupDir)) // if folder does not exist, create it and copy contents from previous folder
                {
                    createDirectory(backupDir);
                }
                string fileName = gv.mod.moduleName;
                string incrementFileName = "";
                for (int i = 0; i < 999; i++) // add an incremental save option (uses directoryName plus number for folder name)
                {
                    if (!File.Exists(backupDir + "\\" + fileName + "(" + i.ToString() + ").mod"))
                    {
                        incrementFileName = fileName + "(" + i.ToString() + ").mod";
                        createFiles(backupDir + "\\" + incrementFileName);
                        break;
                    }
                }
                gv.sf.MessageBox("Moduled backup " + incrementFileName + " was saved");

                // save over original module
                string file = gv.mainDirectory + "\\modules\\" + gv.mod.moduleName + ".mod";
                try
                {
                    createFiles(file);
                    gv.sf.MessageBox("Moduled saved");
                }
                catch
                {
                    gv.sf.MessageBox("failed to save module");
                }
            }
        }
        public void createFiles(string fullPathFilename)
        {
            try
            {
                //clean up the spellsAllowed and traitsAllowed
                foreach (PlayerClass pcls in datafile.dataPlayerClassList)
                {
                    for (int i = pcls.spellsAllowed.Count - 1; i >= 0; i--)
                    {
                        if (!pcls.spellsAllowed[i].allow)
                        {
                            pcls.spellsAllowed.RemoveAt(i);
                        }
                    }
                    for (int i = pcls.traitsAllowed.Count - 1; i >= 0; i--)
                    {
                        if (!pcls.traitsAllowed[i].allow)
                        {
                            pcls.traitsAllowed.RemoveAt(i);
                        }
                    }
                }
                //save module info
                saveModuleInfo(gv.mod, fullPathFilename);
                //save title image
                saveTitleImage(gv.mod, fullPathFilename);
                //fill module items, creatures, props
                gv.mod.moduleItemsList.Clear();
                foreach (Item it in allItemsList)
                {
                    if (it.moduleItem)
                    {
                        gv.mod.moduleItemsList.Add(it.DeepCopy());
                    }
                }
                gv.mod.moduleCreaturesList.Clear();
                foreach (Creature it in allCreaturesList)
                {
                    if (it.moduleCreature)
                    {
                        gv.mod.moduleCreaturesList.Add(it.DeepCopy());
                    }
                }
                gv.mod.modulePropsList.Clear();
                foreach (Prop it in allPropsList)
                {
                    if (it.moduleProp)
                    {
                        gv.mod.modulePropsList.Add(it.DeepCopy());
                    }
                }
                //save mod
                saveModule(gv.mod, fullPathFilename);
                //save areas
                saveAreas(gv.mod, fullPathFilename);
                //save encounters
                saveEncounters(gv.mod, fullPathFilename);
                //save convos
                saveConvos(gv.mod, fullPathFilename);
                //save images
                saveImages(gv.mod, fullPathFilename);

                //fill datafile items, creatures, props
                datafile.dataItemsList.Clear();
                foreach (Item it in allItemsList)
                {
                    if (!it.moduleItem)
                    {
                        datafile.dataItemsList.Add(it.DeepCopy());
                    }
                }
                datafile.dataCreaturesList.Clear();
                foreach (Creature it in allCreaturesList)
                {
                    if (!it.moduleCreature)
                    {
                        datafile.dataCreaturesList.Add(it.DeepCopy());
                    }
                }
                datafile.dataPropsList.Clear();
                foreach (Prop it in allPropsList)
                {
                    if (!it.moduleProp)
                    {
                        datafile.dataPropsList.Add(it.DeepCopy());
                    }
                }

                try
                {
                    string directory = gv.mainDirectory + "\\override";
                    if (!Directory.Exists(directory)) // if folder does not exist, create it and copy contents from previous folder
                    {
                        createDirectory(directory);
                    }
                    //save data file
                    this.datafile.saveDataFile(gv.mainDirectory + "\\override\\data.json", true);
                }
                catch (Exception e)
                {
                    gv.sf.MessageBox("failed to save 'data.json' to 'override' directory: " + e.ToString());
                }                
            }
            catch { gv.sf.MessageBox("failed to createFiles"); }
        }
        public void saveModuleInfo(Module mod, string moduleFullPathFilename)
        {
            File.WriteAllText(moduleFullPathFilename, "MODULEINFO" + Environment.NewLine);
            //create ModuleInfo object
            ModuleInfo modinfo = new ModuleInfo();
            modinfo.moduleName = mod.moduleName;
            modinfo.moduleLabelName = mod.moduleLabelName;
            modinfo.moduleVersion = mod.moduleVersion;
            modinfo.moduleDescription = mod.moduleDescription;
            modinfo.titleImageName = mod.titleImageName;
            string output = JsonConvert.SerializeObject(modinfo, Formatting.None);
            File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
        }
        public void saveTitleImage(Module mod, string moduleFullPathFilename)
        {
            File.AppendAllText(moduleFullPathFilename, "TITLEIMAGE" + Environment.NewLine);
            foreach (ImageData imd in mod.moduleImageDataList)
            {
                //save out title image if one exists in the imagelist
                if (imd.name.Equals(mod.titleImageName))
                {
                    string output = JsonConvert.SerializeObject(imd, Formatting.None);
                    File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
                }
            }
        }
        public void saveModule(Module mod, string moduleFullPathFilename)
        {
            File.AppendAllText(moduleFullPathFilename, "MODULE" + Environment.NewLine);
            string output = JsonConvert.SerializeObject(mod, Formatting.None);
            File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
        }
        public void saveAreas(Module mod, string moduleFullPathFilename)
        {
            File.AppendAllText(moduleFullPathFilename, "AREAS" + Environment.NewLine);
            foreach (Area a in mod.moduleAreasObjects)
            {
                string output = JsonConvert.SerializeObject(a, Formatting.None);
                File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
            }
        }
        public void saveEncounters(Module mod, string moduleFullPathFilename)
        {
            File.AppendAllText(moduleFullPathFilename, "ENCOUNTERS" + Environment.NewLine);
            foreach (Encounter enc in mod.moduleEncountersList)
            {
                string output = JsonConvert.SerializeObject(enc, Formatting.None);
                File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
            }
        }
        public void saveConvos(Module mod, string moduleFullPathFilename)
        {
            File.AppendAllText(moduleFullPathFilename, "CONVOS" + Environment.NewLine);
            foreach (Convo c in mod.moduleConvoList)
            {
                string output = JsonConvert.SerializeObject(c, Formatting.None);
                File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
            }
        }
        public void saveImages(Module mod, string moduleFullPathFilename)
        {
            File.AppendAllText(moduleFullPathFilename, "IMAGES" + Environment.NewLine);
            foreach (ImageData imd in mod.moduleImageDataList)
            {
                string output = JsonConvert.SerializeObject(imd, Formatting.None);
                File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
            }
        }
        public void createDirectory(string fullPath)
        {
            try
            {
                DirectoryInfo dir = Directory.CreateDirectory(fullPath);
            }
            catch
            {
                gv.sf.MessageBox("failed to create the directory: " + fullPath);
            }
        }
        
        //GENERAL
        public void nullOutControls()
        {
            btnHelp = null;
        }        
        public void setControlsStart()
        {
            int pH = (int)((float)gv.screenHeight / 100.0f);
            int padW = gv.squareSize / 6;
            
            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
                btnHelp.Text = "HELP";
                btnHelp.Img = "btn_small"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
                btnHelp.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
                btnHelp.X = 6 * gv.squareSize + padW * 1;
                btnHelp.Y = 9 * gv.squareSize + pH * 2;
                btnHelp.Height = (int)(gv.ibbheight * gv.screenDensity);
                btnHelp.Width = (int)(gv.ibbwidthR * gv.screenDensity);
            }
        }
        
        //TUTORIAL MESSAGES
        public void tutorialMessageMainMap()
        {
            gv.sf.MessageBoxHtml(this.stringMessageMainMap);
        }
        public void tutorialMessageParty(bool helpCall)
        {
            if ((gv.mod.showTutorialParty) || (helpCall))
            {
                gv.sf.MessageBoxHtml(this.stringMessageParty);
                gv.mod.showTutorialParty = false;
            }
        }
        public void tutorialMessageInventory(bool helpCall)
        {
            if ((gv.mod.showTutorialInventory) || (helpCall))
            {
                gv.sf.MessageBoxHtml(this.stringMessageInventory);
                gv.mod.showTutorialInventory = false;
            }
        }
        public void tutorialMessageCombat(bool helpCall)
        {
            if ((gv.mod.showTutorialCombat) || (helpCall))
            {
                gv.sf.MessageBoxHtml(this.stringMessageCombat);
                gv.mod.showTutorialCombat = false;
            }
        }
        public void tutorialPcCreation()
        {
            gv.sf.MessageBoxHtml(this.stringPcCreation);            
        }
        public void tutorialPlayersGuide()
        {
            gv.sf.MessageBoxHtml(this.stringPlayersGuide);
        }
        public void tutorialBeginnersGuide()
        {
            gv.sf.MessageBoxHtml(this.stringBeginnersGuide);
        }
        public void doAboutDialog()
        {
            gv.sf.MessageBoxHtml(gv.mod.moduleCredits);
        }
        
        public void addLogText(string color, string text)
        {
            if (color.Equals("red"))
            {
                gv.log.AddHtmlTextToLog("<rd>" + text + "</rd>");
            }
            else if (color.Equals("lime"))
            {
                gv.log.AddHtmlTextToLog("<gn>" + text + "</gn>");
            }
            else if (color.Equals("yellow"))
            {
                gv.log.AddHtmlTextToLog("<yl>" + text + "</yl>");
            }
            else if (color.Equals("teal"))
            {
                gv.log.AddHtmlTextToLog("<bu>" + text + "</bu>");
            }
            else if (color.Equals("blue"))
            {
                gv.log.AddHtmlTextToLog("<bu>" + text + "</bu>");
            }
            else if (color.Equals("fuchsia"))
            {
                gv.log.AddHtmlTextToLog("<ma>" + text + "</ma>");
            }
            else
            {
                gv.log.AddHtmlTextToLog("<wh>" + text + "</wh>");
            }
            /*
            <?xml version="1.0" encoding="utf-8"?>
            <resources>
             <color name="white">#FFFFFF</color>
             <color name="yellow">#FFFF00</color>
             <color name="fuchsia">#FF00FF</color>
             <color name="red">#FF0000</color>
             <color name="silver">#C0C0C0</color>
             <color name="gray">#808080</color>
             <color name="olive">#808000</color>
             <color name="purple">#800080</color>
             <color name="maroon">#800000</color>
             <color name="aqua">#00FFFF</color>
             <color name="lime">#00FF00</color>
             <color name="teal">#008080</color>
             <color name="green">#008000</color>
             <color name="blue">#0000FF</color>
             <color name="navy">#000080</color>
             <color name="black">#000000</color>
            </resources>
            */
        }
        public void addLogText(string text)
        {
            gv.log.AddHtmlTextToLog(text);		
        }
        public void addFloatyText(Coordinate coorInSquares, string value)
        {
            int txtH = (int)gv.fontHeight;
            int x = ((coorInSquares.X * gv.squareSize) + (gv.squareSize / 2)) - (txtH / 2);
            int y = ((coorInSquares.Y * gv.squareSize) + (gv.squareSize / 2) + txtH) - (txtH / 2);
            if (gv.screenType.Equals("combat"))
            {
                x = ((coorInSquares.X * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2)) - (txtH / 2);
                y = ((coorInSquares.Y * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2) + txtH) - (txtH / 2);
            }
            if (gv.screenType.Equals("main"))
            {
                x = ((coorInSquares.X * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2)) - (txtH / 2);
                y = ((coorInSquares.Y * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2) + txtH) - (txtH / 2);
            }
            Coordinate coor = new Coordinate(x, y);
            floatyTextList.Add(new FloatyText(coor, value));
        }
        public void addFloatyText(Coordinate coorInSquares, string value, string color)
        {
            int txtH = (int)gv.fontHeight;
            int x = ((coorInSquares.X * gv.squareSize) + (gv.squareSize / 2)) - (txtH / 2);
            int y = ((coorInSquares.Y * gv.squareSize) + (gv.squareSize / 2) + txtH) - (txtH / 2);
            if (gv.screenType.Equals("combat"))
            {
                x = ((coorInSquares.X * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2)) - (txtH / 2);
                y = ((coorInSquares.Y * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2) + txtH) - (txtH / 2);
            }
            if (gv.screenType.Equals("main"))
            {
                x = ((coorInSquares.X * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2)) - (txtH / 2);
                y = ((coorInSquares.Y * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2) + txtH) - (txtH / 2);
            }
            Coordinate coor = new Coordinate(x, y);
            floatyTextList.Add(new FloatyText(coor, value, color));
        }
        public void addFloatyText(Coordinate coorInSquares, string value, int shiftUp)
        {
            int txtH = (int)gv.fontHeight;
            int x = ((coorInSquares.X * gv.squareSize) + (gv.squareSize / 2)) - (txtH / 2);
            int y = ((coorInSquares.Y * gv.squareSize) + (gv.squareSize / 2) + txtH) - (txtH / 2) - shiftUp;
            if (gv.screenType.Equals("combat"))
            {
                x = ((coorInSquares.X * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2)) - (txtH / 2);
                y = ((coorInSquares.Y * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2) + txtH) - (txtH / 2) - shiftUp;
            }
            if (gv.screenType.Equals("main"))
            {
                x = ((coorInSquares.X * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2)) - (txtH / 2);
                y = ((coorInSquares.Y * (int)(gv.squareSize * gv.scaler)) + ((int)(gv.squareSize * gv.scaler) / 2) + txtH) - (txtH / 2) - shiftUp;
            }
            Coordinate coor = new Coordinate(x, y);
            floatyTextList.Add(new FloatyText(coor, value));
        }

        public void doUpdate()
        {
            handleRations();
            //in case whole party is unconscious and bleeding, end the game (outside combat here)
            bool endGame = true;
            foreach (Player pc in gv.mod.playerList)
            {
                if (pc.hp >= 0)
                {
                    endGame = false;
                    break;
                }
            }

            if (endGame == true)
            {
                gv.resetGame();
                gv.screenType = "title";
                gv.sf.MessageBoxHtml("Everybody is unconscious and bleeding - your party has been defeated!");
                return;
            }

            //CLEAN UP START SCREENS IF DONE WITH THEM
            if (gv.screenSplash != null)
            {
                gv.screenSplash = null;
                gv.screenLauncher = null;
                gv.screenPartyBuild = null;
                gv.screenPcCreation = null;
                gv.screenTitle = null;
            }

            gv.sf.dsWorldTime();
            //apply effects
            applyEffects();

            blockSecondPropTriggersCall = false;
            //do Conversation and/or Encounter if on Prop (check before props move)
            gv.triggerPropIndex = 0;
            gv.triggerIndex = 0;
            //doPropTriggers();
                        
            //do Conversation and/or Encounter if on Prop (check after props move)
            if (!blockSecondPropTriggersCall)
            {
                gv.triggerPropIndex = 0;
                gv.triggerIndex = 0;
                //doPropTriggers();
            }

            //check for levelup available and switch button image
            checkLevelUpAvailable(); //move this to on update and use a plus overlay in top left            
        }
        public void handleRations()
        {            
            //code for discardign surplus resource items
            bool discardedRations = false;
            for (int i = gv.mod.partyInventoryRefsList.Count - 1; i >= 0; i--)
            //foreach (ItemRefs itRef in gv.mod.partyInventoryRefsList)
            {
                //code for capping number of rations and light sources
                if ((gv.cc.getItemByResRef(gv.mod.partyInventoryRefsList[i].resref).isRation) && (gv.mod.numberOfRationsRemaining > gv.mod.maxNumberOfRationsAllowed))
                {
                    //gv.mod.numberOfRationsRemaining--;
                    gv.mod.numberOfRationsRemaining--;
                    //itRef.quantity = gv.mod.maxNumberOfRationsAllowed;
                    if (gv.mod.partyInventoryRefsList[i].quantity < 1)
                    {
                        gv.mod.partyInventoryRefsList.Remove(gv.mod.partyInventoryRefsList[i]);
                    }
                    else
                    {
                        //gv.mod.partyInventoryRefsList[i].quantity = gv.mod.maxNumberOfRationsAllowed;
                        gv.mod.partyInventoryRefsList.Remove(gv.mod.partyInventoryRefsList[i]);
                        //gv.mod.partyInventoryRefsList[i].quantity = gv.mod.maxNumberOfRationsAllowed;
                    }
                    discardedRations = true;
                }
            }            

            if (discardedRations)
            {
                gv.screenMainMap.addFloatyText(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY, "Discarding excess rations, too heavy", "white", 4000);
            }

            //ration consumption and damage code
            if (gv.mod.useRationSystem)
            {
                if (gv.mod.minutesSinceLastRationConsumed < 1440)
                {
                    gv.mod.minutesSinceLastRationConsumed += gv.mod.currentArea.TimePerSquare;
                }
                else
                {
                    gv.mod.minutesSinceLastRationConsumed = 0;
                    bool foundRation = false;
                    foreach (ItemRefs it in gv.mod.partyInventoryRefsList)
                    {
                        if (gv.cc.getItemByResRef(it.resref).isRation)
                        {
                            it.quantity--;
                            gv.mod.numberOfRationsRemaining--;
                            if (it.quantity < 1)
                            {
                                gv.mod.partyInventoryRefsList.Remove(it);
                            }
                            gv.screenMainMap.addFloatyText(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY, "Ration consumed", "white", 4000);
                            foundRation = true;
                            //gv.mod.onLastRation = false;
                            break;
                        }
                    }
                    if (!foundRation)
                    {
                        gv.screenMainMap.addFloatyText(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY, "Very deprived by lack of supplies... HP & SP lost", "red", 4000);
                        foreach (Player p in gv.mod.playerList)
                        {
                            int healthReduction = (int)(p.hpMax / 5f);
                            if (healthReduction < 1)
                            {
                                healthReduction = 1;
                            }
                            if (p.hp > -20)
                            {
                                p.hp -= healthReduction;
                            }

                            int spReduction = (int)(p.spMax / 5f);
                            if (spReduction < 1)
                            {
                                spReduction = 1;
                            }
                            if (p.sp >= spReduction)
                            {
                                p.sp -= spReduction;
                            }

                        }
                    }
                    //prepare final warning
                    gv.mod.numberOfRationsRemaining = 0;
                    foreach (ItemRefs it in gv.mod.partyInventoryRefsList)
                    {
                        if (gv.cc.getItemByResRef(it.resref).isRation)
                        {
                            gv.mod.numberOfRationsRemaining += it.quantity;
                        }
                    }

                    if (gv.mod.numberOfRationsRemaining == 1)
                    {
                        gv.screenMainMap.addFloatyText(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY, "On your last ration... 48h left to resupply", "red", 4000);
                    }

                    if ((gv.mod.numberOfRationsRemaining == 0) && (foundRation))
                    {
                        gv.screenMainMap.addFloatyText(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY, "No rations... you are in dire need to resupply", "red", 4000);
                    }

                }
            }
            //always have correct ration count
            gv.mod.numberOfRationsRemaining = 0;
            foreach (ItemRefs it in gv.mod.partyInventoryRefsList)
            {
                if (gv.cc.getItemByResRef(it.resref).isRation)
                {
                    //gv.mod.numberOfRationsRemaining = it.quantity;
                    if (it.quantity <= 1)
                    {
                        gv.mod.numberOfRationsRemaining++;
                    }
                    else
                    {
                        gv.mod.numberOfRationsRemaining += it.quantity;
                    }
                }
            }
        }
        public void SwitchToNextAvailablePartyLeader()
        {
            int idx = 0;
            foreach (Player pc in gv.mod.playerList)
            {
                if (!pc.isDead())
                {
                    gv.mod.selectedPartyLeader = idx;
                    return;
                }
                idx++;
            }
        }
        public void checkLevelUpAvailable()
        {            
            if (gv.mod.playerList.Count > 0)
            {
                //if (gv.mod.playerList[0].IsReadyToAdvanceLevel()) { gv.cc.ptrPc0.levelUpOn = true; }
                //else { gv.cc.ptrPc0.levelUpOn = false; }
            }
            if (gv.mod.playerList.Count > 1)
            {
                //if (gv.mod.playerList[1].IsReadyToAdvanceLevel()) { gv.cc.ptrPc1.levelUpOn = true; }
                //else { gv.cc.ptrPc1.levelUpOn = false; }
            }
            if (gv.mod.playerList.Count > 2)
            {
                //if (gv.mod.playerList[2].IsReadyToAdvanceLevel()) { gv.cc.ptrPc2.levelUpOn = true; }
                //else { gv.cc.ptrPc2.levelUpOn = false; }
            }
            if (gv.mod.playerList.Count > 3)
            {
                //if (gv.mod.playerList[3].IsReadyToAdvanceLevel()) { gv.cc.ptrPc3.levelUpOn = true; }
                //else { gv.cc.ptrPc3.levelUpOn = false; }
            }
            if (gv.mod.playerList.Count > 4)
            {
                //if (gv.mod.playerList[4].IsReadyToAdvanceLevel()) { gv.cc.ptrPc4.levelUpOn = true; }
                //else { gv.cc.ptrPc4.levelUpOn = false; }
            }
            if (gv.mod.playerList.Count > 5)
            {
                //if (gv.mod.playerList[5].IsReadyToAdvanceLevel()) { gv.cc.ptrPc5.levelUpOn = true; }
                //else { gv.cc.ptrPc5.levelUpOn = false; }
            }
        }
        public void applyEffects()
        {
            try
            {
                foreach (Player pc in gv.mod.playerList)
                {
                    foreach (Effect ef in pc.effectsList)
                    {
                        //decrement duration of all
                        ef.durationInUnits -= gv.mod.currentArea.TimePerSquare;
                        if (!ef.usedForUpdateStats) //not used for stat updates
                        {
                            doEffectScript(pc, ef);
                        }
                    }
                }
                //if remaining duration <= 0, remove from list
                foreach (Player pc in gv.mod.playerList)
                {
                    for (int i = pc.effectsList.Count; i > 0; i--)
                    {
                        if (pc.effectsList[i - 1].durationInUnits <= 0)
                        {
                            pc.effectsList.RemoveAt(i - 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
            }
        }
        public void doEffectScript(object src, Effect ef)
        {
            if (ef.effectScript.Equals("efGeneric"))
            {
                gv.sf.efGeneric(src, ef);
            }
        }
        /*public void doPropTriggers()
        {
            try
            {
                //search area for any props that share the party location
                bool foundOne = false;
                foreach (Prop prp in gv.mod.currentArea.Props)
                {
                    bool doNotTriggerProp = false;
                    if ((prp.LocationX == gv.mod.PlayerLocationX) && (prp.LocationY == gv.mod.PlayerLocationY) && (prp.isActive) && (doNotTriggerProp == false))
                    {
                        //prp.wasTriggeredLastUpdate = true;
                        foundOne = true;
                        blockSecondPropTriggersCall = true;
                        gv.triggerPropIndex++;
                        if ((gv.triggerPropIndex == 1) && (!prp.ConversationWhenOnPartySquare.Equals("none")))
                        {
                            calledConvoFromProp = true;
                            gv.sf.ThisProp = prp;                                
                            doConversationBasedOnTag(prp.ConversationWhenOnPartySquare);
                            break;                            
                        }
                        else if ((gv.triggerPropIndex == 2) && (!prp.EncounterWhenOnPartySquare.Equals("none")))
                        {
                            calledEncounterFromProp = true;
                            gv.sf.ThisProp = prp;                            
                            doEncounterBasedOnTag(prp.EncounterWhenOnPartySquare);
                            break;
                        }
                        else if (gv.triggerPropIndex < 3)
                        {
                            gv.mod.isRecursiveCall = true;
                            doPropTriggers();
                            gv.mod.isRecursiveCall = false;
                            break;
                        }
                        if (gv.triggerPropIndex > 2)
                        {
                            gv.triggerPropIndex = 0;
                            //set flags back to false
                            calledConvoFromProp = false;
                            calledEncounterFromProp = false;
                            foundOne = false;
                            //delete prop if flag is set to do so and break foreach loop
                            if (prp.DeletePropWhenThisEncounterIsWon)
                            {
                                gv.mod.currentArea.Props.Remove(prp);
                            }
                            break;
                        }
                    }
                }
                if (!foundOne)
                {
                    doTrigger();
                }
            }
            catch (Exception ex)
            {
                if (gv.mod.debugMode)
                {
                    gv.sf.MessageBox("failed to do prop trigger: " + ex.ToString());
                    gv.errorLog(ex.ToString());
                }
            }
        }*/
        public void doTrigger()
        {            
            try
            {
                Trigger trig = gv.mod.currentArea.getTriggerByLocation(gv.mod.PlayerLocationX, gv.mod.PlayerLocationY);
                if ((trig != null) && (trig.Enabled))
                {
                    blockSecondPropTriggersCall = true;
                    //iterate through each event                  
                    #region Event1 stuff
                    //check to see if enabled and parm not "none"                    
                    gv.triggerIndex++;

                    if ((gv.triggerIndex == 1) && (trig.EnabledEvent1) && (!trig.Event1FilenameOrTag.Equals("none")))
                    {
                        //check to see what type of event
                        if (trig.Event1Type.Equals("container"))
                        {
                            doContainerBasedOnTag(trig.Event1FilenameOrTag);
                            doTrigger();
                        }
                        else if (trig.Event1Type.Equals("transition"))
                        {
                            doTransitionBasedOnAreaLocation(trig.Event1FilenameOrTag, trig.Event1TransPointX, trig.Event1TransPointY);
                        }
                        else if (trig.Event1Type.Equals("conversation"))
                        {
                            //if (trig.conversationCannotBeAvoided == true)
                            //{
                                doConversationBasedOnTag(trig.Event1FilenameOrTag);
                            //}
                            //else if (gv.mod.avoidInteraction == false)
                            //{
                                //doConversationBasedOnTag(trig.Event1FilenameOrTag);
                            //}
                        }
                        else if (trig.Event1Type.Equals("encounter"))
                        {
                            doEncounterBasedOnTag(trig.Event1FilenameOrTag);
                        }
                        else if (trig.Event1Type.Equals("script"))
                        {
                            doScriptBasedOnFilename(trig.Event1FilenameOrTag, trig.Event1Parm1, trig.Event1Parm2, trig.Event1Parm3, trig.Event1Parm4);
                            doTrigger();
                        }
                        //do that event
                        if (trig.DoOnceOnlyEvent1)
                        {
                            trig.EnabledEvent1 = false;
                        }
                    }
                    #endregion
                    #region Event2 stuff
                    //check to see if enabled and parm not "none"
                    else if ((gv.triggerIndex == 2) && (trig.EnabledEvent2) && (!trig.Event2FilenameOrTag.Equals("none")))
                    {
                        //check to see what type of event
                        if (trig.Event2Type.Equals("container"))
                        {
                            doContainerBasedOnTag(trig.Event2FilenameOrTag);
                            doTrigger();
                        }
                        else if (trig.Event2Type.Equals("transition"))
                        {
                            doTransitionBasedOnAreaLocation(trig.Event2FilenameOrTag, trig.Event2TransPointX, trig.Event2TransPointY);
                        }
                        else if (trig.Event2Type.Equals("conversation"))
                        {
                            //if (trig.conversationCannotBeAvoided == true)
                            //{
                            //    doConversationBasedOnTag(trig.Event2FilenameOrTag);
                            //}
                            //else if (gv.mod.avoidInteraction == false)
                            //{
                                doConversationBasedOnTag(trig.Event2FilenameOrTag);
                            //}
                        }
                        else if (trig.Event2Type.Equals("encounter"))
                        {
                            doEncounterBasedOnTag(trig.Event2FilenameOrTag);
                        }
                        else if (trig.Event2Type.Equals("script"))
                        {
                            doScriptBasedOnFilename(trig.Event2FilenameOrTag, trig.Event2Parm1, trig.Event2Parm2, trig.Event2Parm3, trig.Event2Parm4);
                            doTrigger();
                        }
                        //do that event
                        if (trig.DoOnceOnlyEvent2)
                        {
                            trig.EnabledEvent2 = false;
                        }
                    }
                    #endregion
                    #region Event3 stuff
                    //check to see if enabled and parm not "none"
                    else if ((gv.triggerIndex == 3) && (trig.EnabledEvent3) && (!trig.Event3FilenameOrTag.Equals("none")))
                    {
                        //check to see what type of event
                        if (trig.Event3Type.Equals("container"))
                        {
                            doContainerBasedOnTag(trig.Event3FilenameOrTag);
                            doTrigger();
                        }
                        else if (trig.Event3Type.Equals("transition"))
                        {
                            doTransitionBasedOnAreaLocation(trig.Event3FilenameOrTag, trig.Event3TransPointX, trig.Event3TransPointY);
                        }
                        else if (trig.Event3Type.Equals("conversation"))
                        {
                            //if (trig.conversationCannotBeAvoided == true)
                            //{
                            //    doConversationBasedOnTag(trig.Event3FilenameOrTag);
                            //}
                            //else if (gv.mod.avoidInteraction == false)
                            //{
                                doConversationBasedOnTag(trig.Event3FilenameOrTag);
                            //}
                        }
                        else if (trig.Event3Type.Equals("encounter"))
                        {
                            doEncounterBasedOnTag(trig.Event3FilenameOrTag);
                        }
                        else if (trig.Event3Type.Equals("script"))
                        {
                            doScriptBasedOnFilename(trig.Event3FilenameOrTag, trig.Event3Parm1, trig.Event3Parm2, trig.Event3Parm3, trig.Event3Parm4);
                            doTrigger();
                        }
                        //do that event
                        if (trig.DoOnceOnlyEvent3)
                        {
                            trig.EnabledEvent3 = false;
                        }
                    }
                    else if (gv.triggerIndex < 4)
                    {
                        doTrigger();
                    }
                    #endregion
                    if (gv.triggerIndex > 3)
                    {
                        gv.triggerIndex = 0;
                        if (trig.DoOnceOnly)
                        {
                            trig.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (gv.mod.debugMode)
                {
                    gv.sf.MessageBox("failed to do trigger: " + ex.ToString());
                    gv.errorLog(ex.ToString());
                }
            }
        }
        public void doContainerBasedOnTag(string tag)
        {
            try
            {
                Container container = gv.mod.getContainerByTag(tag);
                gv.screenType = "itemSelector";
                gv.screenItemSelector.resetItemSelector(container.containerItemRefs, "container", "main");
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
            }
        }
        public void doConversationBasedOnTag(string tag)
        {
            try
            {
                gv.screenConvo.currentConvo = gv.mod.getConvoByName(tag);
                if (gv.screenConvo.currentConvo != null)
                {
                    gv.screenType = "convo";
                    gv.screenConvo.startConvo();
                }
                else
                {
                    gv.sf.MessageBox("failed to find conversation in list with tag: " + tag);
                }
            }
            catch (Exception ex)
            {
                gv.sf.MessageBox("failed to open conversation with tag: " + tag);
                gv.errorLog(ex.ToString());
            }
        }
        public void doSpellBasedOnScriptOrEffectTag(Spell spell, object source, object target, bool outsideCombat)
        {
            gv.sf.AoeTargetsList.Clear();

            if (!spell.spellEffectTag.Equals("none"))
            {
                gv.sf.spGeneric(spell, source, target, outsideCombat);
            }

            else if (spell.spellEffectTagList.Count > 0)
            {
                gv.sf.spGeneric(spell, source, target, outsideCombat);
            }

            //WIZARD SPELLS
            else if (spell.spellScript.Equals("spDimensionDoor"))
            {
                gv.sf.spDimensionDoor(source, target);
            }
                        
            //CLERIC SPELLS
            else if (spell.tag.Equals("minorHealing"))
            {
                //gv.sf.spHeal(source, target, 8);
            }

            //THIEF SKILL
            else if (spell.spellScript.Equals("trRemoveTrap"))
            {
                gv.sf.trRemoveTrap(source, target);
            }
        }
        public void doTraitBasedOnScriptOrEffectTag(Trait trait, object source, object target, bool outsideCombat)
        {
            gv.sf.AoeTargetsList.Clear();

            if (trait.traitEffectTagList.Count > 0)
            {
                gv.sf.trGeneric(trait, source, target, outsideCombat);
            }

            //THIEF SKILL
            else if (trait.traitScript.Equals("trRemoveTrap"))
            {
                gv.sf.trRemoveTrap(source, target);
            }
        }
        public void doScriptBasedOnFilename(string filename, string prm1, string prm2, string prm3, string prm4)
        {
            if (!filename.Equals("none"))
            {
                //send to ga, gc, og, or os controllers
                if (filename.StartsWith("gc"))
                {
                    gv.sf.gcController(filename, prm1, prm2, prm3, prm4);
                }
                else if (filename.StartsWith("ga"))
                {
                    gv.sf.gaController(filename, prm1, prm2, prm3, prm4);
                }
                else if (filename.StartsWith("og"))
                {
                    gv.sf.ogController(filename, prm1, prm2, prm3, prm4);
                }
                else if (filename.StartsWith("os"))
                {
                    gv.sf.osController(filename, prm1, prm2, prm3, prm4);
                }
            }
        }
        public void doEncounterBasedOnTag(string name)
        {
            try
            {
                gv.mod.currentEncounter = gv.mod.getEncounter(name);
                if (gv.mod.currentEncounter.encounterCreatureRefsList.Count > 0)
                {
                    gv.screenCombat.doCombatSetup();
                    int foundOnePc = 0;
                    foreach (Player chr in gv.mod.playerList)
                    {
                        if (chr.hp > 0)
                        {
                            foundOnePc = 1;
                        }
                    }
                    if (foundOnePc == 0)
                    {
                        //IBMessageBox.Show(game, "Party is wiped out...game over");
                    }
                }
                else
                {
                    //IBMessageBox.Show(game, "no creatures left here"); 
                }
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
            }
        }
        public void doTransitionBasedOnAreaLocation(string areaFilename, int x, int y)
        {
            try
            {
                bool foundArea = gv.mod.setCurrentArea(areaFilename, gv);
                if (!foundArea)
                {
                    gv.sf.MessageBox("Area: " + areaFilename + " does not exist in the module...check the spelling of the 'area.Filename'");
                    return;
                }
                gv.mod.PlayerLocationX = x;
                gv.mod.PlayerLocationY = y;
                gv.screenMainMap.resetMiniMapBitmap();
                doOnEnterAreaUpdate = true;
                doOnEnterAreaUpdate = false;
                gv.triggerIndex = 0;
                doTrigger();                
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
            }            
        }
        /*public void doItemScriptBasedOnUseItem(Player pc, ItemRefs itRef, bool destroyItemAfterUse)
        {
            Item it = gv.cc.getItemByResRefForInfo(itRef.resref);
            bool foundScript = false;
            if (it.onUseItem.Equals("itHealLight.cs"))
            {
                gv.sf.itHeal(pc, it, 8);
                foundScript = true;
            }
            else if (it.onUseItem.Equals("itHealMedium.cs"))
            {
                gv.sf.itHeal(pc, it, 16);
                foundScript = true;
            }
            else if (it.onUseItem.Equals("itRegenSPLight.cs"))
            {
                gv.sf.itSpHeal(pc, it, 20);
                foundScript = true;
            }
            else if (it.onUseItem.Equals("itForceRest.cs"))
            {
                gv.sf.itForceRest();
                foundScript = true;
            }
            if ((foundScript) && (destroyItemAfterUse))
            {
                gv.sf.RemoveItemFromInventory(itRef, 1);
            }
        }*/
                        
        //MISC FUNCTIONS
        public int getDistance(Coordinate start, Coordinate end)
        {
            int dist = 0;
            int deltaX = (int)Math.Abs((start.X - end.X));
            int deltaY = (int)Math.Abs((start.Y - end.Y));
            if (deltaX > deltaY)
                dist = deltaX;
            else
                dist = deltaY;
            return dist;
        }
        public int getCreatureSize(string tokenfilename)
        {
            //1=normal, 2=wide, 3=tall, 4=large
            int width = gv.cc.GetFromBitmapList(tokenfilename).Width;
            int height = gv.cc.GetFromBitmapList(tokenfilename).Height;
            //normal
            if ((width == gv.standardTokenSize) && (height == gv.standardTokenSize * 2))
            {
                return 1;
            }
            //wide
            else if ((width == gv.standardTokenSize * 2) && (height == gv.standardTokenSize * 2))
            {
                return 2;
            }
            //tall
            else if ((width == gv.standardTokenSize) && (height == gv.standardTokenSize * 4))
            {
                return 3;
            }
            //large
            else if ((width == gv.standardTokenSize * 2) && (height == gv.standardTokenSize * 4))
            {
                return 4;
            }
            return 1;
        }
        /*
        public System.Drawing.Bitmap LoadBitmapGDI(string filename) //change this to LoadBitmapGDI
        {
            System.Drawing.Bitmap bm = null;
            bm = LoadBitmapGDI(filename, gv.mod); //change this to LoadBitmapGDI
            return bm;
        }
        public System.Drawing.Bitmap LoadBitmapGDI(string filename, Module mdl) //change this to LoadBitmapGDI
        {
            System.Drawing.Bitmap bm = null;

            try
            {
                //default graphics locations
                if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\graphics\\" + filename + ".png"))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\graphics\\" + filename + ".png");
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\graphics\\" + filename + ".PNG"))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\graphics\\" + filename + ".PNG");
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\graphics\\" + filename + ".jpg"))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\graphics\\" + filename + ".jpg");
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\graphics\\" + filename))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\graphics\\" + filename);
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\tiles\\" + filename + ".png"))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\tiles\\" + filename + ".png");
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\tiles\\" + filename + ".PNG"))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\tiles\\" + filename + ".PNG");
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\tiles\\" + filename))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\tiles\\" + filename);
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".png"))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".png");
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".PNG"))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".PNG");
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".jpg"))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".jpg");
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\portraits\\" + filename))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\portraits\\" + filename);
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\ui\\" + filename + ".png"))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\ui\\" + filename + ".png");
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\ui\\" + filename + ".jpg"))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\ui\\" + filename + ".jpg");
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\ui\\" + filename))
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\ui\\" + filename);
                }

                else
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\graphics\\missingtexture.png");
                }		
            }
            catch (Exception ex)
            {
                if (bm == null)
                {
                    bm = new System.Drawing.Bitmap(gv.mainDirectory + "\\default\\NewModule\\graphics\\missingtexture.png");
                    return bm;
                }
                gv.errorLog(ex.ToString());
            }

            return bm;
        }
        */
        public string loadTextToString(string filename)
        {
            string txt = null;
            try
            {
                if (File.Exists(GetModulePath(filename) + "\\data\\" + filename))
                {
                    txt = File.ReadAllText(GetModulePath(filename) + "\\data\\" + filename);
                }
                else if (File.Exists(gv.mainDirectory + "\\default\\NewModule\\data\\" + filename))
                {
                    txt = File.ReadAllText(gv.mainDirectory + "\\default\\NewModule\\data\\" + filename);
                }
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
                return null;
            }
            return txt;
        }
        public void MakeDirectoryIfDoesntExist(string filenameAndFullPath)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(filenameAndFullPath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
        }
        
        //DIRECT2D STUFF
        public SKBitmap GetFromBitmapList(string fileNameWithOutExt)
        {
            //check to see if in list already and return bitmap it if found
            if ((commonBitmapList.ContainsKey(fileNameWithOutExt)) || (moduleBitmapList.ContainsKey(fileNameWithOutExt)))
            {
                if (commonBitmapList.ContainsKey(fileNameWithOutExt))
                {
                    return commonBitmapList[fileNameWithOutExt];
                }
                else
                {
                    return moduleBitmapList[fileNameWithOutExt];
                }
            }
            //try loading and adding to list and return bitmap
            else
            {
                commonBitmapList.Add(fileNameWithOutExt, LoadBitmap(fileNameWithOutExt));
                return commonBitmapList[fileNameWithOutExt];
            }
        }
        public SKBitmap GetFromTileBitmapList(string fileNameWithOutExt)
        {
            return GetFromBitmapList(fileNameWithOutExt);
            //check to see if in list already and return bitmap it if found
            /*if (commonBitmapList.ContainsKey(fileNameWithOutExt))
            {
                return commonBitmapList[fileNameWithOutExt];
            }
            //try loading and adding to list and return bitmap
            else
            {
                commonBitmapList.Add(fileNameWithOutExt, LoadBitmap(fileNameWithOutExt));
                return commonBitmapList[fileNameWithOutExt];
            }*/
        }
        /*public SKBitmap GetFromTileGDIBitmapList(string fileNameWithOutExt)
        {
            //check to see if in list already and return bitmap it if found
            if (tileGDIBitmapList.ContainsKey(fileNameWithOutExt))
            {
                return tileGDIBitmapList[fileNameWithOutExt];
            }
            //try loading and adding to list and return bitmap
            else
            {
                tileGDIBitmapList.Add(fileNameWithOutExt, LoadBitmapGDI(fileNameWithOutExt));
                return tileGDIBitmapList[fileNameWithOutExt];
            }
        }*/
        public void DisposeOfBitmap(ref SKBitmap bmp)
        {
            if (bmp != null)
            {
                bmp.Dispose();
                bmp = null;
            }
        }
        public SKBitmap LoadBitmap(string file) //change this to LoadBitmap
        {
            /*
            // Loads from file using System.Drawing.Image
            using (var bitmap = LoadBitmapGDI(file)) //change this to LoadBitmapGDI
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }
                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;
                    return new SharpDX.Direct2D1.Bitmap(gv.renderTarget2D, size, tempStream, stride, bitmapProperties);
                }
            }*/
            return new SKBitmap();
        }
        /*public SharpDX.Direct2D1.Bitmap ConvertGDIBitmapToD2D(System.Drawing.Bitmap gdibitmap)
        {
            var sourceArea = new System.Drawing.Rectangle(0, 0, gdibitmap.Width, gdibitmap.Height);
            var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
            var size = new Size2(gdibitmap.Width, gdibitmap.Height);

            // Transform pixels from BGRA to RGBA
            int stride = gdibitmap.Width * sizeof(int);
            using (var tempStream = new DataStream(gdibitmap.Height * stride, true, true))
            {
                // Lock System.Drawing.Bitmap
                var bitmapData = gdibitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                // Convert all pixels 
                for (int y = 0; y < gdibitmap.Height; y++)
                {
                    int offset = bitmapData.Stride * y;
                    for (int x = 0; x < gdibitmap.Width; x++)
                    {
                        // Not optimized 
                        byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        int rgba = R | (G << 8) | (B << 16) | (A << 24);
                        tempStream.Write(rgba);
                    }
                }
                gdibitmap.UnlockBits(bitmapData);
                tempStream.Position = 0;
                return new SharpDX.Direct2D1.Bitmap(gv.renderTarget2D, size, tempStream, stride, bitmapProperties);
            }
        }*/
        public List<IBminiFormattedLine> ProcessHtmlString(string text, int width, List<string> tagStack, bool IBmini)
        {
            bool tagMode = false;
            string tag = "";
            IBminiFormattedWord newWord = new IBminiFormattedWord();
            IBminiFormattedLine newLine = new IBminiFormattedLine();
            List<IBminiFormattedLine> logLinesList = new List<IBminiFormattedLine>();
            float xLoc = 0;

            char previousChar = ' ';
            char nextChar = ' ';
            int charIndex = -1;
            foreach (char c in text)
            {
                charIndex++;

                //get the previous char and the next char, used to get ' < ' and ' >= '
                if (charIndex > 0)
                {
                    previousChar = text[charIndex - 1];
                }
                if (charIndex < text.Length - 1)
                {
                    nextChar = text[charIndex + 1];
                }
                string combinedChars = previousChar.ToString() + c.ToString() + nextChar.ToString();

                #region Start/Stop Tags
                //start a tag and check for end of word
                if ((c == '<') && (!combinedChars.Contains("<=")) && (!combinedChars.Equals(" < ")))
                {
                    tagMode = true;

                    if (newWord.text != "")
                    {
                        newWord.color = GetColor(tagStack);
                        int wordWidth = (newWord.text.Length + 1) * (gv.fontWidth + gv.fontCharSpacing);
                        if (xLoc + wordWidth > (width) - (gv.fontWidth * 2)) //word wrap
                        {
                            //end last line and add it to the log
                            logLinesList.Add(newLine);
                            //start a new line and add this word
                            newLine = new IBminiFormattedLine();
                            newLine.wordsList.Add(newWord);
                            xLoc = 0;
                        }
                        else //no word wrap, just add word
                        {
                            newLine.wordsList.Add(newWord);
                        }
                        //instead of drawing, just add to line list 
                        xLoc += wordWidth;
                        newWord = new IBminiFormattedWord();
                    }
                    continue;
                }
                //end a tag
                else if ((c == '>') && (!combinedChars.Equals(" > ")) && (!combinedChars.Contains(">=")))
                {
                    //check for ending type tag
                    if (tag.StartsWith("/"))
                    {
                        //if </>, remove corresponding tag from stack
                        string tagMinusSlash = tag.Substring(1);
                        if (tag.StartsWith("/font"))
                        {
                            for (int i = tagStack.Count - 1; i > 0; i--)
                            {
                                if (tagStack[i].StartsWith("font"))
                                {
                                    tagStack.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            tagStack.Remove(tagMinusSlash);
                        }
                    }
                    else
                    {
                        //check for line break
                        if ((tag.ToLower() == "br") || (tag == "BR"))
                        {
                            newWord.color = GetColor(tagStack);
                            //end last line and add it to the log
                            logLinesList.Add(newLine);
                            //start a new line and add this word
                            newLine = new IBminiFormattedLine();
                            xLoc = 0;
                        }
                        //else if <>, add this tag to the stack
                        tagStack.Add(tag);
                    }
                    tagMode = false;
                    tag = "";
                    continue;
                }
                #endregion

                #region Words
                if (!tagMode)
                {
                    if (c != ' ') //keep adding to word until hit a space
                    {
                        newWord.text += c;
                    }
                    else //hit a space so end word
                    {
                        newWord.color = GetColor(tagStack);
                        int wordWidth = (newWord.text.Length + 1) * (gv.fontWidth + gv.fontCharSpacing);
                        if (xLoc + wordWidth > (width) - (gv.fontWidth * 2)) //word wrap
                        {
                            //end last line and add it to the log
                            logLinesList.Add(newLine);
                            //start a new line and add this word
                            newLine = new IBminiFormattedLine();
                            newLine.wordsList.Add(newWord);
                            xLoc = 0;
                        }
                        else //no word wrap, just add word
                        {
                            newLine.wordsList.Add(newWord);
                        }
                        //instead of drawing, just add to line list 
                        xLoc += wordWidth;
                        newWord = new IBminiFormattedWord();
                    }
                }
                else if (tagMode)
                {
                    tag += c;
                }
                #endregion
            }
            tagStack.Clear();
            return logLinesList;
        }
        private string GetColor(List<string> tagStack)
        {
            //will end up using the last color on the stack
            string clr = "wh";
            foreach (string s in tagStack)
            {
                if ((s.Equals("Bk")) || (s.Equals("bk")) || (s.Equals("font color='black'")))
                {
                    clr = "bk";
                }
                else if ((s.Equals("Bu")) || (s.Equals("bu")) || (s.Equals("font color='blue'")))
                {
                    clr = "bu";
                }
                else if ((s.Equals("Gn")) || (s.Equals("gn")) || (s.Equals("font color='green'")) || (s.Equals("font color='lime'")))
                {
                    clr = "gn";
                }
                else if ((s.Equals("Gy")) || (s.Equals("gy")) || (s.Equals("font color='grey'")) || (s.Equals("font color='gray'")))
                {
                    clr = "gy";
                }
                else if ((s.Equals("Ma")) || (s.Equals("ma")) || (s.Equals("font color='magenta'")) || (s.Equals("font color='fuchsia'")))
                {
                    clr = "ma";
                }
                else if ((s.Equals("Rd")) || (s.Equals("rd")) || (s.Equals("font color='red'")))
                {
                    clr = "rd";
                }
                else if ((s.Equals("Yl")) || (s.Equals("yl")) || (s.Equals("font color='yellow'")))
                {
                    clr = "yl";
                }
            }
            return clr;
        }

        public Item getItemByTag(string tag)
        {
            foreach (Item it in this.allItemsList)
            {
                if (it.tag.Equals(tag)) return it;
            }
            return null;
        }
        public Item getItemByResRef(string resref)
        {
            foreach (Item it in this.allItemsList)
            {
                if (it.resref.Equals(resref)) return it;
            }
            return null;
        }
        public Item getItemByResRefForInfo(string resref)
        {
            foreach (Item it in this.allItemsList)
            {
                if (it.resref.Equals(resref)) return it;
            }
            return new Item();
        }
        public Spell getSpellByTag(string tag)
        {
            foreach (Spell s in this.datafile.dataSpellsList)
            {
                if (s.tag.Equals(tag)) return s;
            }
            return null;
        }
        public Trait getTraitByTag(string tag)
        {
            foreach (Trait t in this.datafile.dataTraitsList)
            {
                if (t.tag.Equals(tag)) return t;
            }
            return null;
        }
        public Effect getEffectByTag(string tag)
        {
            foreach (Effect ef in this.datafile.dataEffectsList)
            {
                if (ef.tag.Equals(tag)) return ef;
            }
            return null;
        }
        public PlayerClass getPlayerClass(string tag)
        {
            foreach (PlayerClass p in this.datafile.dataPlayerClassList)
            {
                if (p.tag.Equals(tag)) return p;
            }
            return null;
        }
        public Race getRace(string tag)
        {
            foreach (Race r in this.datafile.dataRacesList)
            {
                if (r.tag.Equals(tag)) return r;
            }
            return null;
        }
    }
}
