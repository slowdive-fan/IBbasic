using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace IBbasic
{
    public class Module
    {
        public string moduleName = "none";
        public string moduleLabelName = "none";
        public string titleImageName = "title";
        public int moduleVersion = 1;
        public string saveName = "empty";
        public string defaultPlayerFilename = "drin.json";
        public List<StringForDropDownList> defaultPlayerFilenameList = new List<StringForDropDownList>();
        public bool mustUsePreMadePC = false;
        public int numberOfPlayerMadePcsAllowed = 1;
        public int MaxPartySize = 6;
        public string moduleDescription = "";
        public string moduleCredits = "";
        public int nextIdNumber = 100;
        public int WorldTime = 0;
        public int TimePerRound = 6;
        public bool debugMode = false;
        public float diagonalMoveCost = 1.5f;
        public bool allowSave = true;
        public bool hideRoster = false;
        public bool use3d6 = false;
        public bool useUIBackground = true;
        public int logNumberOfLines = 28;
        public string goldLabelSingular = "Gold";
        public string goldLabelPlural = "Gold";
        public bool ArmorClassAscending = true;
        public List<Container> moduleContainersList = new List<Container>();
        public List<Shop> moduleShopsList = new List<Shop>();

        public List<Item> moduleItemsList = new List<Item>();            
        public List<Creature> moduleCreaturesList = new List<Creature>();
        public List<Prop> modulePropsList = new List<Prop>();
        public List<JournalQuest> moduleJournal = new List<JournalQuest>();

        [JsonIgnore]
        public List<ImageData> moduleImageDataList = new List<ImageData>();
        [JsonIgnore]
        public List<Area> moduleAreasObjects = new List<Area>();
        [JsonIgnore]
        public List<Encounter> moduleEncountersList = new List<Encounter>();        
        [JsonIgnore]
        public List<Convo> moduleConvoList = new List<Convo>();

        public List<GlobalInt> moduleGlobalInts = new List<GlobalInt>();
        public List<GlobalString> moduleGlobalStrings = new List<GlobalString>();
        public List<ConvoSavedValues> moduleConvoSavedValuesList = new List<ConvoSavedValues>();
        public string startingArea = "";
        public int startingPlayerPositionX = 0;
        public int startingPlayerPositionY = 0;
        public int PlayerLocationX = 4;
        public int PlayerLocationY = 1;
        public int PlayerFacingDirection = 0; //0 = north, 1 = east, 2 = south, 3 = west
        public int PlayerLastLocationX = 4;
        public int PlayerLastLocationY = 1;
        [JsonIgnore]
        public bool PlayerFacingLeft = true;
        public Area currentArea = new Area();
        [JsonIgnore]
        public Encounter currentEncounter = new Encounter();
        public int partyGold = 0;
        public bool showPartyToken = false;
        public string partyTokenFilename = "prp_party";
        public List<Player> playerList = new List<Player>();
        public List<Player> partyRosterList = new List<Player>();
        public List<Player> companionPlayerList = new List<Player>();
        public List<ItemRefs> partyInventoryRefsList = new List<ItemRefs>();
        public List<JournalQuest> partyJournalQuests = new List<JournalQuest>();
        public List<JournalQuest> partyJournalCompleted = new List<JournalQuest>();
        public string partyJournalNotes = "";
        public int selectedPartyLeader = 0;
        [JsonIgnore]
        public bool returnCheck = false;
        [JsonIgnore]
        public bool addPCScriptFired = false;
        [JsonIgnore]
        public bool uncheckConvo = false;
        [JsonIgnore]
        public bool removeCreature = false;
        [JsonIgnore]
        public bool deleteItemUsedScript = false;
        [JsonIgnore]
        public int indexOfPCtoLastUseItem = 0;
        public bool com_showGrid = false;
        public bool map_showGrid = false;
        public bool playMusic = false;
        public bool playSoundFx = false;
        public bool playButtonSounds = false;
        public bool playButtonHaptic = false;
        public bool showTutorialParty = true;
        public bool showTutorialInventory = true;
        public bool showTutorialCombat = true;
        public bool showAutosaveMessage = true;
        public bool allowAutosave = true;
        public int combatAnimationSpeed = 100;
        public bool fastMode = false;
        public int attackAnimationSpeed = 100;
        public float soundVolume = 1.0f;
        public string OnHeartBeatIBScript = "none";
        public string OnHeartBeatIBScriptParms = "";
        public bool showInteractionState = false;
        public bool avoidInteraction = false;
        public int attackFromBehindToHitModifier = 2;
        public int attackFromBehindDamageModifier = 0;        
        public bool doConvo = true;
        public int noTriggerLocX = -1;
        public int noTriggerLocY = -1;
        public bool firstTriggerCall = true;
        public bool isRecursiveCall = false;
        public bool useRationSystem = false;
        public int numberOfRationsRemaining = 0;
        public int maxNumberOfRationsAllowed = 7;
        public int minutesSinceLastRationConsumed = 0;

        public Module()
        {

        }

        public bool setCurrentArea(string areaFilename, GameView gv)
        {
            try
            {
                foreach (Area area in this.moduleAreasObjects)
                {
                    if (area.Filename.Equals(areaFilename))
                    {
                        this.currentArea = area;
                        return true;
                    }
                }
                //didn't find the area in the mod list so try and load it
                string s = gv.GetModuleAssetFileString(this.moduleName, areaFilename + ".are");
                using (StringReader sr = new StringReader(s))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Area are = (Area)serializer.Deserialize(sr, typeof(Area));
                    if (are != null)
                    {
                        this.moduleAreasObjects.Add(are);
                        this.currentArea = are;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
                return false;
            }
        }
        public bool setCurrentEncounter(string EncFilename, GameView gv)
        {
            try
            {
                foreach (Encounter enc in this.moduleEncountersList)
                {
                    if (enc.encounterName.Equals(EncFilename))
                    {
                        this.currentEncounter = enc;
                        return true;
                    }
                }
                //didn't find the area in the mod list so try and load it
                string s = gv.GetModuleAssetFileString(this.moduleName, EncFilename + ".enc");
                using (StringReader sr = new StringReader(s))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Encounter enc = (Encounter)serializer.Deserialize(sr, typeof(Encounter));
                    if (enc != null)
                    {
                        this.moduleEncountersList.Add(enc);
                        this.currentEncounter = enc;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
                return false;
            }
        }
        public bool setCurrentConvo(string ConvoFilename, GameView gv)
        {
            try
            {
                foreach (Convo cnv in this.moduleConvoList)
                {
                    if (cnv.ConvoFileName.Equals(ConvoFilename))
                    {
                        gv.screenConvo.currentConvo = cnv;
                        return true;
                    }
                }
                //didn't find the area in the mod list so try and load it
                string s = gv.GetModuleAssetFileString(this.moduleName, ConvoFilename + ".dlg");
                using (StringReader sr = new StringReader(s))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Convo cnv = (Convo)serializer.Deserialize(sr, typeof(Convo));
                    if (cnv != null)
                    {
                        this.moduleConvoList.Add(cnv);
                        gv.screenConvo.currentConvo = cnv;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
                return false;
            }
        }

        public int getNextIdNumber()
        {
            this.nextIdNumber++;
            return this.nextIdNumber;
        }
        public Player getPlayerByName(string tag)
        {
            foreach (Player pc in this.playerList)
            {
                if (pc.name.Equals(tag))
                {
                    return pc;
                }
            }
            return null;
        }
       public ItemRefs getItemRefsInInventoryByResRef(string resref)
        {
            foreach (ItemRefs itr in this.partyInventoryRefsList)
            {
                if (itr.resref.Equals(resref)) return itr;
            }
            return null;
        }
        public ItemRefs createItemRefsFromItem(Item it)
        {
            ItemRefs newIR = new ItemRefs();
            newIR.tag = it.tag + "_" + this.getNextIdNumber();
            newIR.name = it.name;
            newIR.resref = it.resref;
            newIR.canNotBeUnequipped = it.canNotBeUnequipped;
            newIR.quantity = it.quantity;
            return newIR;
        }
        public Container getContainerByTag(string tag)
        {
            foreach (Container it in this.moduleContainersList)
            {
                if (it.containerTag.Equals(tag)) return it;
            }
            return null;
        }
        public Shop getShopByTag(string tag)
        {
            foreach (Shop s in this.moduleShopsList)
            {
                if (s.shopTag.Equals(tag)) return s;
            }
            return null;
        }
        public Encounter getEncounter(string name)
        {
            foreach (Encounter e in this.moduleEncountersList)
            {
                if (e.encounterName.Equals(name)) return e;
            }
            return null;
        }
        public Convo getConvoByName(string name)
        {
            foreach (Convo e in this.moduleConvoList)
            {
                if (e.ConvoFileName.Equals(name)) return e;
            }
            return null;
        }
        public Creature getCreatureInCurrentEncounterByTag(string tag)
        {
            foreach (Creature crt in this.currentEncounter.encounterCreatureList)
            {
                if (crt.cr_tag.Equals(tag)) return crt;
            }
            return null;
        }
        public JournalQuest getJournalCategoryByTag(string tag)
        {
            foreach (JournalQuest it in this.moduleJournal)
            {
                if (it.Tag.Equals(tag)) return it;
            }
            return null;
        }
        public JournalQuest getPartyJournalActiveCategoryByTag(string tag)
        {
            foreach (JournalQuest it in this.partyJournalQuests)
            {
                if (it.Tag.Equals(tag)) return it;
            }
            return null;
        }
        public JournalQuest getPartyJournalCompletedCategoryByTag(string tag)
        {
            foreach (JournalQuest it in this.partyJournalCompleted)
            {
                if (it.Tag.Equals(tag)) return it;
            }
            return null;
        }
    }
}
