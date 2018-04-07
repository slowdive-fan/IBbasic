using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ToolsetScreenCreatureEditor
    {
        public GameView gv;
        public int creatureListIndex = 0;
        public int knownSpellListIndex = 0;
        public IbbButton btnAddCreature = null;
        public IbbButton btnRemoveCreature = null;
        public IbbButton btnCopyCreature = null;
        public string currentMode = "Main"; //Main, Attack, Images, Resistance, Behavior, Spells
        public IbbToggle tglMain = null;
        public IbbToggle tglAttack = null;
        public IbbToggle tglImages = null;
        public IbbToggle tglResistance = null;
        public IbbToggle tglBehavior = null;
        public IbbToggle tglSpells = null;
        public Dictionary<string, bool> categoryList = new Dictionary<string, bool>();
        public int numberOfLinesToShow = 23;
        public List<int> indexList = new List<int>();
        public Dictionary<int, string> categoryNameAtLine = new Dictionary<int, string>();

        //MAIN
        private IbbToggle btnCrtName = null;
        private IbbToggle btnCrtResRef = null;  //assign same name to tag      
        private IbbToggle btnCrtLevel = null;
        private IbbToggle btnCrtHP = null;
        private IbbToggle btnCrtHPMax = null;
        private IbbToggle btnCrtSP = null;
        private IbbToggle btnCrtXP = null;
        private IbbToggle btnCrtParentNodeName = null; //for organizing
        //ATTACK-DEFEND
        private IbbToggle btnCrtAC = null;
        private IbbToggle btnCrtAttack = null;
        private IbbToggle btnCrtAttackRange = null;
        private IbbToggle btnCrtDamageNumOfDice = null;
        private IbbToggle btnCrtDamageDice = null;
        private IbbToggle btnCrtDamageAdder = null;
        private IbbToggle btnCrtNumberOfAttacks = null;
        private IbbToggle btnCrtAttackCategory = null;
        private IbbToggle btnCrtTypeOfDamage = null;
        //IMAGES
        private IbbToggle btnCrtTokenFilename = null;
        private IbbToggle btnCrtAttackSound = null;
        private IbbToggle btnCrtSpriteProjectileFilename = null;
        private IbbToggle btnCrtSpriteEndingFilename = null;
        //RESISTANCES (reflex, will, fort, damResist)
        private IbbToggle btnCrtReflex = null;
        private IbbToggle btnCrtWill = null;
        private IbbToggle btnCrtFortitude = null;
        private IbbToggle btnCrtResistAcid = null;
        private IbbToggle btnCrtResistNormal = null;
        private IbbToggle btnCrtResistCold = null;
        private IbbToggle btnCrtResistElectricity = null;
        private IbbToggle btnCrtResistFire = null;
        private IbbToggle btnCrtResistMagic = null;
        private IbbToggle btnCrtResistPoison = null;
        //BEHAVIOR (ai, inibonus, movedist, scripts, spellOnHit)
        private IbbToggle btnCrtAI = null;
        private IbbToggle btnCrtIniBonus = null;
        private IbbToggle btnCrtMoveDistance = null;
        private IbbToggle btnCrtOnDeathScript = null;
        private IbbToggle btnCrtOnDeathScriptParms = null;
        private IbbToggle btnCrtOnScoreHitCastSpell = null;
        //KNOWN SPELLS
        public IbbButton btnAddKnownSpell = null;
        public IbbButton btnRemoveKnownSpell = null;

        private IbbButton btnHelp = null;

        public IbRect src = null;
        public IbRect dst = null;

        public ToolsetScreenCreatureEditor(GameView g)
        {
            gv = g;
            currentMode = "Main";
            setControlsStart();
            sortCreatureList();
            tglMain.toggleOn = true;
            tglAttack.toggleOn = false;
            tglImages.toggleOn = false;
            tglResistance.toggleOn = false;
            tglBehavior.toggleOn = false;
            tglSpells.toggleOn = false;

            //initialize categoryList
            foreach (Creature crt in gv.cc.allCreaturesList)
            {
                if (!categoryList.ContainsKey(crt.cr_parentNodeName))
                {
                    categoryList.Add(crt.cr_parentNodeName, true);
                }
            }

            resetIndexList();
        }
        public void sortCreatureList()
        {
            //sort creatures based on category type
            gv.cc.allCreaturesList = gv.cc.allCreaturesList.OrderBy(x => x.cr_name).ToList();
            gv.cc.allCreaturesList = gv.cc.allCreaturesList.OrderBy(x => x.cr_parentNodeName).ToList();
        }
        public void resetIndexList()
        {
            int cnt = 0;
            int lineIndx = 0;
            string lastCategory = "";
            indexList.Clear();
            categoryNameAtLine.Clear();
            foreach (Creature crt in gv.cc.allCreaturesList)
            {
                if (!crt.cr_parentNodeName.Equals(lastCategory))
                {
                    lastCategory = crt.cr_parentNodeName;
                    //new category so add -1 to index list
                    indexList.Add(-1);
                    categoryNameAtLine.Add(lineIndx, crt.cr_parentNodeName);
                    lineIndx++;
                }
                if ((categoryList.ContainsKey(crt.cr_parentNodeName)) && (categoryList[crt.cr_parentNodeName]))
                {
                    //is expanded so add cnt to indexList
                    indexList.Add(cnt);
                    lineIndx++;
                }
                cnt++;
            }
        }
        public void setControlsStart()
        {
            if (btnAddCreature == null)
            {
                btnAddCreature = new IbbButton(gv, 0.8f);
            }
            btnAddCreature.Text = "ADD";
            btnAddCreature.Img = "btn_small";
            btnAddCreature.Glow = "btn_small_glow";
            btnAddCreature.X = 0 * gv.uiSquareSize + 1 * gv.fontWidth;
            btnAddCreature.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnAddCreature.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddCreature.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveCreature == null)
            {
                btnRemoveCreature = new IbbButton(gv, 0.8f);
            }
            btnRemoveCreature.Text = "REMOVE";
            btnRemoveCreature.Img = "btn_small";
            btnRemoveCreature.Glow = "btn_small_glow";
            btnRemoveCreature.X = 1 * gv.uiSquareSize + 1 * gv.fontWidth;
            btnRemoveCreature.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnRemoveCreature.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveCreature.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCopyCreature == null)
            {
                btnCopyCreature = new IbbButton(gv, 0.8f);
            }
            btnCopyCreature.Text = "COPY";
            btnCopyCreature.Img = "btn_small";
            btnCopyCreature.Glow = "btn_small_glow";
            btnCopyCreature.X = 2 * gv.uiSquareSize + 1 * gv.fontWidth;
            btnCopyCreature.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnCopyCreature.Height = (int)(gv.ibbheight * gv.scaler);
            btnCopyCreature.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (tglMain == null)
            {
                tglMain = new IbbToggle(gv);
            }
            tglMain.ImgOn = "mtgl_rbtn_on";
            tglMain.ImgOff = "mtgl_rbtn_off";
            tglMain.X = 3 * gv.uiSquareSize + 2 * gv.fontWidth;
            tglMain.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            tglMain.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglMain.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglAttack == null)
            {
                tglAttack = new IbbToggle(gv);
            }
            tglAttack.ImgOn = "mtgl_rbtn_on";
            tglAttack.ImgOff = "mtgl_rbtn_off";
            tglAttack.X = 3 * gv.uiSquareSize + 2 * gv.fontWidth;
            tglAttack.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 2 * gv.fontHeight;
            tglAttack.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglAttack.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglImages == null)
            {
                tglImages = new IbbToggle(gv);
            }
            tglImages.ImgOn = "mtgl_rbtn_on";
            tglImages.ImgOff = "mtgl_rbtn_off";
            tglImages.X = 6 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglImages.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            tglImages.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglImages.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglResistance == null)
            {
                tglResistance = new IbbToggle(gv);
            }
            tglResistance.ImgOn = "mtgl_rbtn_on";
            tglResistance.ImgOff = "mtgl_rbtn_off";
            tglResistance.X = 6 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglResistance.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 2 * gv.fontHeight;
            tglResistance.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglResistance.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglBehavior == null)
            {
                tglBehavior = new IbbToggle(gv);
            }
            tglBehavior.ImgOn = "mtgl_rbtn_on";
            tglBehavior.ImgOff = "mtgl_rbtn_off";
            tglBehavior.X = 9 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglBehavior.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            tglBehavior.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglBehavior.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglSpells == null)
            {
                tglSpells = new IbbToggle(gv);
            }
            tglSpells.ImgOn = "mtgl_rbtn_on";
            tglSpells.ImgOff = "mtgl_rbtn_off";
            tglSpells.X = 9 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglSpells.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 2 * gv.fontHeight;
            tglSpells.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSpells.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);
        }
        public void setMainControlsStart()
        {
            if (btnCrtName == null)
            {
                btnCrtName = new IbbToggle(gv);
            }
            btnCrtName.ImgOn = "mtgl_edit_btn";
            btnCrtName.ImgOff = "mtgl_edit_btn";
            btnCrtName.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtName.Y = 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtResRef == null)
            {
                btnCrtResRef = new IbbToggle(gv);
            }
            btnCrtResRef.ImgOn = "mtgl_edit_btn";
            btnCrtResRef.ImgOff = "mtgl_edit_btn";
            btnCrtResRef.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResRef.Y = 2 * gv.uiSquareSize;
            btnCrtResRef.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtResRef.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtParentNodeName == null)
            {
                btnCrtParentNodeName = new IbbToggle(gv);
            }
            btnCrtParentNodeName.ImgOn = "mtgl_edit_btn";
            btnCrtParentNodeName.ImgOff = "mtgl_edit_btn";
            btnCrtParentNodeName.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtParentNodeName.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtParentNodeName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtParentNodeName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtLevel == null)
            {
                btnCrtLevel = new IbbToggle(gv);
            }
            btnCrtLevel.ImgOn = "mtgl_edit_btn";
            btnCrtLevel.ImgOff = "mtgl_edit_btn";
            btnCrtLevel.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtLevel.Y = 3 * gv.uiSquareSize;
            btnCrtLevel.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtLevel.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtHP == null)
            {
                btnCrtHP = new IbbToggle(gv);
            }
            btnCrtHP.ImgOn = "mtgl_edit_btn";
            btnCrtHP.ImgOff = "mtgl_edit_btn";
            btnCrtHP.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtHP.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtHP.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtHP.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtHPMax == null)
            {
                btnCrtHPMax = new IbbToggle(gv);
            }
            btnCrtHPMax.ImgOn = "mtgl_edit_btn";
            btnCrtHPMax.ImgOff = "mtgl_edit_btn";
            btnCrtHPMax.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtHPMax.Y = 4 * gv.uiSquareSize;
            btnCrtHPMax.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtHPMax.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtSP == null)
            {
                btnCrtSP = new IbbToggle(gv);
            }
            btnCrtSP.ImgOn = "mtgl_edit_btn";
            btnCrtSP.ImgOff = "mtgl_edit_btn";
            btnCrtSP.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtSP.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtSP.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtSP.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtXP == null)
            {
                btnCrtXP = new IbbToggle(gv);
            }
            btnCrtXP.ImgOn = "mtgl_edit_btn";
            btnCrtXP.ImgOff = "mtgl_edit_btn";
            btnCrtXP.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtXP.Y = 5 * gv.uiSquareSize;
            btnCrtXP.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtXP.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtTokenFilename == null)
            {
                btnCrtTokenFilename = new IbbToggle(gv);
            }
            btnCrtTokenFilename.ImgOn = "mtgl_edit_btn";
            btnCrtTokenFilename.ImgOff = "mtgl_edit_btn";
            btnCrtTokenFilename.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtTokenFilename.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtTokenFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtTokenFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
            }
            btnHelp.Text = "HELP";
            btnHelp.Img = "btn_small";
            btnHelp.Glow = "btn_small_glow";
            btnHelp.X = 10 * gv.uiSquareSize;
            btnHelp.Y = 6 * gv.uiSquareSize;
            btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
            btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        public void setAttackControlsStart()
        {
            if (btnCrtAC == null)
            {
                btnCrtAC = new IbbToggle(gv);
            }
            btnCrtAC.ImgOn = "mtgl_edit_btn";
            btnCrtAC.ImgOff = "mtgl_edit_btn";
            btnCrtAC.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtAC.Y = 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtAC.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtAC.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtAttack == null)
            {
                btnCrtAttack = new IbbToggle(gv);
            }
            btnCrtAttack.ImgOn = "mtgl_edit_btn";
            btnCrtAttack.ImgOff = "mtgl_edit_btn";
            btnCrtAttack.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtAttack.Y = 2 * gv.uiSquareSize;
            btnCrtAttack.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtAttack.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtAttackRange == null)
            {
                btnCrtAttackRange = new IbbToggle(gv);
            }
            btnCrtAttackRange.ImgOn = "mtgl_edit_btn";
            btnCrtAttackRange.ImgOff = "mtgl_edit_btn";
            btnCrtAttackRange.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtAttackRange.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtAttackRange.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtAttackRange.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtDamageNumOfDice == null)
            {
                btnCrtDamageNumOfDice = new IbbToggle(gv);
            }
            btnCrtDamageNumOfDice.ImgOn = "mtgl_edit_btn";
            btnCrtDamageNumOfDice.ImgOff = "mtgl_edit_btn";
            btnCrtDamageNumOfDice.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtDamageNumOfDice.Y = 3 * gv.uiSquareSize;
            btnCrtDamageNumOfDice.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtDamageNumOfDice.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtDamageDice == null)
            {
                btnCrtDamageDice = new IbbToggle(gv);
            }
            btnCrtDamageDice.ImgOn = "mtgl_edit_btn";
            btnCrtDamageDice.ImgOff = "mtgl_edit_btn";
            btnCrtDamageDice.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtDamageDice.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtDamageDice.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtDamageDice.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtDamageAdder == null)
            {
                btnCrtDamageAdder = new IbbToggle(gv);
            }
            btnCrtDamageAdder.ImgOn = "mtgl_edit_btn";
            btnCrtDamageAdder.ImgOff = "mtgl_edit_btn";
            btnCrtDamageAdder.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtDamageAdder.Y = 4 * gv.uiSquareSize;
            btnCrtDamageAdder.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtDamageAdder.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtNumberOfAttacks == null)
            {
                btnCrtNumberOfAttacks = new IbbToggle(gv);
            }
            btnCrtNumberOfAttacks.ImgOn = "mtgl_edit_btn";
            btnCrtNumberOfAttacks.ImgOff = "mtgl_edit_btn";
            btnCrtNumberOfAttacks.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtNumberOfAttacks.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtNumberOfAttacks.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtNumberOfAttacks.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtAttackCategory == null)
            {
                btnCrtAttackCategory = new IbbToggle(gv);
            }
            btnCrtAttackCategory.ImgOn = "mtgl_edit_btn";
            btnCrtAttackCategory.ImgOff = "mtgl_edit_btn";
            btnCrtAttackCategory.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtAttackCategory.Y = 5 * gv.uiSquareSize;
            btnCrtAttackCategory.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtAttackCategory.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtTypeOfDamage == null)
            {
                btnCrtTypeOfDamage = new IbbToggle(gv);
            }
            btnCrtTypeOfDamage.ImgOn = "mtgl_edit_btn";
            btnCrtTypeOfDamage.ImgOff = "mtgl_edit_btn";
            btnCrtTypeOfDamage.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtTypeOfDamage.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtTypeOfDamage.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtTypeOfDamage.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }
        public void setImagesControlsStart()
        {
            if (btnCrtTokenFilename == null)
            {
                btnCrtTokenFilename = new IbbToggle(gv);
            }
            btnCrtTokenFilename.ImgOn = "mtgl_edit_btn";
            btnCrtTokenFilename.ImgOff = "mtgl_edit_btn";
            btnCrtTokenFilename.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtTokenFilename.Y = 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtTokenFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtTokenFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtAttackSound == null)
            {
                btnCrtAttackSound = new IbbToggle(gv);
            }
            btnCrtAttackSound.ImgOn = "mtgl_edit_btn";
            btnCrtAttackSound.ImgOff = "mtgl_edit_btn";
            btnCrtAttackSound.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtAttackSound.Y = 3 * gv.uiSquareSize;
            btnCrtAttackSound.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtAttackSound.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtSpriteProjectileFilename == null)
            {
                btnCrtSpriteProjectileFilename = new IbbToggle(gv);
            }
            btnCrtSpriteProjectileFilename.ImgOn = "mtgl_edit_btn";
            btnCrtSpriteProjectileFilename.ImgOff = "mtgl_edit_btn";
            btnCrtSpriteProjectileFilename.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtSpriteProjectileFilename.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtSpriteProjectileFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtSpriteProjectileFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtSpriteEndingFilename == null)
            {
                btnCrtSpriteEndingFilename = new IbbToggle(gv);
            }
            btnCrtSpriteEndingFilename.ImgOn = "mtgl_edit_btn";
            btnCrtSpriteEndingFilename.ImgOff = "mtgl_edit_btn";
            btnCrtSpriteEndingFilename.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtSpriteEndingFilename.Y = 5 * gv.uiSquareSize;
            btnCrtSpriteEndingFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtSpriteEndingFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setResistanceControlsStart()
        {
            if (btnCrtReflex == null)
            {
                btnCrtReflex = new IbbToggle(gv);
            }
            btnCrtReflex.ImgOn = "mtgl_edit_btn";
            btnCrtReflex.ImgOff = "mtgl_edit_btn";
            btnCrtReflex.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtReflex.Y = 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtReflex.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtReflex.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtWill == null)
            {
                btnCrtWill = new IbbToggle(gv);
            }
            btnCrtWill.ImgOn = "mtgl_edit_btn";
            btnCrtWill.ImgOff = "mtgl_edit_btn";
            btnCrtWill.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtWill.Y = 2 * gv.uiSquareSize;
            btnCrtWill.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtWill.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtFortitude == null)
            {
                btnCrtFortitude = new IbbToggle(gv);
            }
            btnCrtFortitude.ImgOn = "mtgl_edit_btn";
            btnCrtFortitude.ImgOff = "mtgl_edit_btn";
            btnCrtFortitude.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtFortitude.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtFortitude.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtFortitude.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtResistAcid == null)
            {
                btnCrtResistAcid = new IbbToggle(gv);
            }
            btnCrtResistAcid.ImgOn = "mtgl_edit_btn";
            btnCrtResistAcid.ImgOff = "mtgl_edit_btn";
            btnCrtResistAcid.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResistAcid.Y = 3 * gv.uiSquareSize;
            btnCrtResistAcid.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtResistAcid.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtResistNormal == null)
            {
                btnCrtResistNormal = new IbbToggle(gv);
            }
            btnCrtResistNormal.ImgOn = "mtgl_edit_btn";
            btnCrtResistNormal.ImgOff = "mtgl_edit_btn";
            btnCrtResistNormal.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResistNormal.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResistNormal.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtResistNormal.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtResistCold == null)
            {
                btnCrtResistCold = new IbbToggle(gv);
            }
            btnCrtResistCold.ImgOn = "mtgl_edit_btn";
            btnCrtResistCold.ImgOff = "mtgl_edit_btn";
            btnCrtResistCold.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResistCold.Y = 4 * gv.uiSquareSize;
            btnCrtResistCold.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtResistCold.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtResistElectricity == null)
            {
                btnCrtResistElectricity = new IbbToggle(gv);
            }
            btnCrtResistElectricity.ImgOn = "mtgl_edit_btn";
            btnCrtResistElectricity.ImgOff = "mtgl_edit_btn";
            btnCrtResistElectricity.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResistElectricity.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResistElectricity.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtResistElectricity.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtResistFire == null)
            {
                btnCrtResistFire = new IbbToggle(gv);
            }
            btnCrtResistFire.ImgOn = "mtgl_edit_btn";
            btnCrtResistFire.ImgOff = "mtgl_edit_btn";
            btnCrtResistFire.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResistFire.Y = 5 * gv.uiSquareSize;
            btnCrtResistFire.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtResistFire.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtResistMagic == null)
            {
                btnCrtResistMagic = new IbbToggle(gv);
            }
            btnCrtResistMagic.ImgOn = "mtgl_edit_btn";
            btnCrtResistMagic.ImgOff = "mtgl_edit_btn";
            btnCrtResistMagic.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResistMagic.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResistMagic.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtResistMagic.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtResistPoison == null)
            {
                btnCrtResistPoison = new IbbToggle(gv);
            }
            btnCrtResistPoison.ImgOn = "mtgl_edit_btn";
            btnCrtResistPoison.ImgOff = "mtgl_edit_btn";
            btnCrtResistPoison.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtResistPoison.Y = 6 * gv.uiSquareSize;
            btnCrtResistPoison.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtResistPoison.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
            }
            btnHelp.Text = "HELP";
            btnHelp.Img = "btn_small";
            btnHelp.Glow = "btn_small_glow";
            btnHelp.X = 10 * gv.uiSquareSize;
            btnHelp.Y = 6 * gv.uiSquareSize;
            btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
            btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        public void setBehaviorControlsStart()
        {
            if (btnCrtAI == null)
            {
                btnCrtAI = new IbbToggle(gv);
            }
            btnCrtAI.ImgOn = "mtgl_edit_btn";
            btnCrtAI.ImgOff = "mtgl_edit_btn";
            btnCrtAI.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtAI.Y = 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtAI.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtAI.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtIniBonus == null)
            {
                btnCrtIniBonus = new IbbToggle(gv);
            }
            btnCrtIniBonus.ImgOn = "mtgl_edit_btn";
            btnCrtIniBonus.ImgOff = "mtgl_edit_btn";
            btnCrtIniBonus.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtIniBonus.Y = 2 * gv.uiSquareSize;
            btnCrtIniBonus.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtIniBonus.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtMoveDistance == null)
            {
                btnCrtMoveDistance = new IbbToggle(gv);
            }
            btnCrtMoveDistance.ImgOn = "mtgl_edit_btn";
            btnCrtMoveDistance.ImgOff = "mtgl_edit_btn";
            btnCrtMoveDistance.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtMoveDistance.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtMoveDistance.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtMoveDistance.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtOnDeathScript == null)
            {
                btnCrtOnDeathScript = new IbbToggle(gv);
            }
            btnCrtOnDeathScript.ImgOn = "mtgl_edit_btn";
            btnCrtOnDeathScript.ImgOff = "mtgl_edit_btn";
            btnCrtOnDeathScript.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtOnDeathScript.Y = 3 * gv.uiSquareSize;
            btnCrtOnDeathScript.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtOnDeathScript.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtOnDeathScriptParms == null)
            {
                btnCrtOnDeathScriptParms = new IbbToggle(gv);
            }
            btnCrtOnDeathScriptParms.ImgOn = "mtgl_edit_btn";
            btnCrtOnDeathScriptParms.ImgOff = "mtgl_edit_btn";
            btnCrtOnDeathScriptParms.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtOnDeathScriptParms.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtOnDeathScriptParms.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtOnDeathScriptParms.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnCrtOnScoreHitCastSpell == null)
            {
                btnCrtOnScoreHitCastSpell = new IbbToggle(gv);
            }
            btnCrtOnScoreHitCastSpell.ImgOn = "mtgl_edit_btn";
            btnCrtOnScoreHitCastSpell.ImgOff = "mtgl_edit_btn";
            btnCrtOnScoreHitCastSpell.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnCrtOnScoreHitCastSpell.Y = 4 * gv.uiSquareSize;
            btnCrtOnScoreHitCastSpell.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnCrtOnScoreHitCastSpell.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }
        public void setSpellsControlsStart()
        {
            if (btnAddKnownSpell == null)
            {
                btnAddKnownSpell = new IbbButton(gv, 0.8f);
            }
            btnAddKnownSpell.Text = "ADD";
            btnAddKnownSpell.Img = "btn_small";
            btnAddKnownSpell.Glow = "btn_small_glow";
            btnAddKnownSpell.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 1 * gv.fontWidth;
            btnAddKnownSpell.Y = 1 * gv.uiSquareSize + 4 * gv.fontHeight;
            btnAddKnownSpell.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddKnownSpell.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveKnownSpell == null)
            {
                btnRemoveKnownSpell = new IbbButton(gv, 0.8f);
            }
            btnRemoveKnownSpell.Text = "REMOVE";
            btnRemoveKnownSpell.Img = "btn_small";
            btnRemoveKnownSpell.Glow = "btn_small_glow";
            btnRemoveKnownSpell.X = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 1 * gv.fontWidth;
            btnRemoveKnownSpell.Y = 1 * gv.uiSquareSize + 4 * gv.fontHeight;
            btnRemoveKnownSpell.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveKnownSpell.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsCreatureEditor()
        {
            sortCreatureList();
            setControlsStart();
            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            int shiftForFont = (tglMain.Height / 2) - (gv.fontHeight / 2);
            //Page Title
            gv.DrawText("CREATURE EDITOR", center - (7 * (gv.fontWidth + gv.fontCharSpacing)), 2 * gv.scaler, "yl");

            //label      
            gv.DrawText("CREATURES", btnAddCreature.X, btnAddCreature.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            btnAddCreature.Draw();
            btnRemoveCreature.Draw();
            btnCopyCreature.Draw();

            string lastCategory = "";
            numberOfLinesToShow = 23;
            int cnt = 0;
            int crtCnt = 0;
            int startY = btnAddCreature.Y + btnAddCreature.Height;
            foreach (Creature crt in gv.cc.allCreaturesList)
            {
                int tlX = btnAddCreature.X;
                int tlY = startY + (gv.fontHeight + gv.fontLineSpacing) * cnt;
                //check if change in lastCategory and if so print category and then go to next line and print crt tag
                if (!crt.cr_parentNodeName.Equals(lastCategory))
                {
                    //new category
                    lastCategory = crt.cr_parentNodeName;
                    src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("mtgl_expand_off").Width, gv.cc.GetFromTileBitmapList("mtgl_expand_off").Height);
                    dst = new IbRect(tlX, tlY, gv.fontHeight, gv.fontWidth);
                    if ((categoryList.ContainsKey(crt.cr_parentNodeName)) && (categoryList[crt.cr_parentNodeName]))
                    {
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList("mtgl_expand_off"), src, dst);
                    }
                    else
                    {
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList("mtgl_expand_on"), src, dst);
                    }
                    gv.DrawText(" " + crt.cr_parentNodeName, tlX, tlY, "bu");
                    cnt++;
                }
                if ((categoryList.ContainsKey(crt.cr_parentNodeName)) && (categoryList[crt.cr_parentNodeName]))
                {
                    tlY = startY + (gv.fontHeight + gv.fontLineSpacing) * cnt;
                    if (crtCnt == creatureListIndex)
                    {
                        gv.DrawText("  " + crt.cr_name, tlX, tlY, "gn");
                    }
                    else
                    {
                        if (crt.moduleCreature)
                        {
                            gv.DrawText("  " + crt.cr_name, tlX, tlY, "wh");
                        }
                        else
                        {
                            gv.DrawText("  " + crt.cr_name, tlX, tlY, "gy");
                        }
                    }
                    cnt++;
                }
                crtCnt++;
            }

            tglMain.Draw();
            gv.DrawText("MAIN", tglMain.X + tglMain.Width + gv.scaler, tglMain.Y + shiftForFont, "ma");
            tglAttack.Draw();
            gv.DrawText("ATTACK/DEFEND", tglAttack.X + tglAttack.Width + gv.scaler, tglAttack.Y + shiftForFont, "ma");
            tglImages.Draw();
            gv.DrawText("IMAGES/SOUNDS", tglImages.X + tglImages.Width + gv.scaler, tglImages.Y + shiftForFont, "ma");
            tglResistance.Draw();
            gv.DrawText("RESISTANCES", tglResistance.X + tglResistance.Width + gv.scaler, tglResistance.Y + shiftForFont, "ma");
            tglBehavior.Draw();
            gv.DrawText("BEHAVIOR", tglBehavior.X + tglBehavior.Width + gv.scaler, tglBehavior.Y + shiftForFont, "ma");
            tglSpells.Draw();
            gv.DrawText("SPELLS", tglSpells.X + tglSpells.Width + gv.scaler, tglSpells.Y + shiftForFont, "ma");

            if (currentMode.Equals("Main"))
            {
                setMainControlsStart();
                drawMain();
            }
            else if (currentMode.Equals("Attack"))
            {
                setAttackControlsStart();
                drawAttack();
            }
            else if (currentMode.Equals("Images"))
            {
                setImagesControlsStart();
                drawImages();
            }
            else if (currentMode.Equals("Resistance"))
            {
                setResistanceControlsStart();
                drawResistances();
            }
            else if (currentMode.Equals("Behavior"))
            {
                setBehaviorControlsStart();
                drawBehavior();
            }
            else if (currentMode.Equals("Spells"))
            {
                setSpellsControlsStart();
                drawSpells();
            }


            btnHelp.Draw();

            gv.tsMainMenu.redrawTsMainMenu();

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }
        public void drawMain()
        {
            int shiftForFont = (btnCrtName.Height / 2) - (gv.fontHeight / 2);
            btnCrtName.Draw();
            gv.DrawText("NAME: " + gv.cc.allCreaturesList[creatureListIndex].cr_name, btnCrtName.X + btnCrtName.Width + gv.scaler, btnCrtName.Y + shiftForFont, "wh");
            btnCrtResRef.Draw();
            gv.DrawText("RESREF: " + gv.cc.allCreaturesList[creatureListIndex].cr_resref, btnCrtResRef.X + btnCrtResRef.Width + gv.scaler, btnCrtResRef.Y + shiftForFont, "wh");
            btnCrtParentNodeName.Draw();
            gv.DrawText("CATEGORY: " + gv.cc.allCreaturesList[creatureListIndex].cr_parentNodeName, btnCrtParentNodeName.X + btnCrtParentNodeName.Width + gv.scaler, btnCrtParentNodeName.Y + shiftForFont, "wh");
            btnCrtLevel.Draw();
            gv.DrawText("LEVEL: " + gv.cc.allCreaturesList[creatureListIndex].cr_level, btnCrtLevel.X + btnCrtLevel.Width + gv.scaler, btnCrtLevel.Y + shiftForFont, "wh");
            btnCrtHP.Draw();
            gv.DrawText("HP: " + gv.cc.allCreaturesList[creatureListIndex].hp, btnCrtHP.X + btnCrtHP.Width + gv.scaler, btnCrtHP.Y + shiftForFont, "wh");
            btnCrtHPMax.Draw();
            gv.DrawText("HPMAX: " + gv.cc.allCreaturesList[creatureListIndex].hpMax, btnCrtHPMax.X + btnCrtHPMax.Width + gv.scaler, btnCrtHPMax.Y + shiftForFont, "wh");
            btnCrtSP.Draw();
            gv.DrawText("SP: " + gv.cc.allCreaturesList[creatureListIndex].sp, btnCrtSP.X + btnCrtSP.Width + gv.scaler, btnCrtSP.Y + shiftForFont, "wh");
            btnCrtXP.Draw();
            gv.DrawText("XP: " + gv.cc.allCreaturesList[creatureListIndex].cr_XP, btnCrtXP.X + btnCrtXP.Width + gv.scaler, btnCrtXP.Y + shiftForFont, "wh");
        }
        public void drawAttack()
        {
            int shiftForFont = (btnCrtName.Height / 2) - (gv.fontHeight / 2);
            btnCrtAC.Draw();
            gv.DrawText("AC: " + gv.cc.allCreaturesList[creatureListIndex].AC, btnCrtAC.X + btnCrtAC.Width + gv.scaler, btnCrtAC.Y + shiftForFont, "wh");
            btnCrtAttack.Draw();
            gv.DrawText("ATTACK: " + gv.cc.allCreaturesList[creatureListIndex].cr_att, btnCrtAttack.X + btnCrtAttack.Width + gv.scaler, btnCrtAttack.Y + shiftForFont, "wh");
            btnCrtAttackRange.Draw();
            gv.DrawText("ATTACK RANGE: " + gv.cc.allCreaturesList[creatureListIndex].cr_attRange, btnCrtAttackRange.X + btnCrtAttackRange.Width + gv.scaler, btnCrtAttackRange.Y + shiftForFont, "wh");
            btnCrtDamageNumOfDice.Draw();
            gv.DrawText("DAMAGE NUM OF DICE: " + gv.cc.allCreaturesList[creatureListIndex].cr_damageNumDice, btnCrtDamageNumOfDice.X + btnCrtDamageNumOfDice.Width + gv.scaler, btnCrtDamageNumOfDice.Y + shiftForFont, "wh");
            btnCrtDamageDice.Draw();
            gv.DrawText("DAMAGE DICE: " + gv.cc.allCreaturesList[creatureListIndex].cr_damageDie, btnCrtDamageDice.X + btnCrtDamageDice.Width + gv.scaler, btnCrtDamageDice.Y + shiftForFont, "wh");
            btnCrtDamageAdder.Draw();
            gv.DrawText("DAMAGE ADDER: " + gv.cc.allCreaturesList[creatureListIndex].cr_damageAdder, btnCrtDamageAdder.X + btnCrtDamageAdder.Width + gv.scaler, btnCrtDamageAdder.Y + shiftForFont, "wh");
            btnCrtNumberOfAttacks.Draw();
            gv.DrawText("NUMBER OF ATTACKS: " + gv.cc.allCreaturesList[creatureListIndex].cr_numberOfAttacks, btnCrtNumberOfAttacks.X + btnCrtNumberOfAttacks.Width + gv.scaler, btnCrtNumberOfAttacks.Y + shiftForFont, "wh");
            btnCrtAttackCategory.Draw();
            gv.DrawText("ATTACK TYPE: " + gv.cc.allCreaturesList[creatureListIndex].cr_category, btnCrtAttackCategory.X + btnCrtAttackCategory.Width + gv.scaler, btnCrtAttackCategory.Y + shiftForFont, "wh");
            btnCrtTypeOfDamage.Draw();
            gv.DrawText("TYPE OF DAMAGE: " + gv.cc.allCreaturesList[creatureListIndex].cr_typeOfDamage, btnCrtTypeOfDamage.X + btnCrtTypeOfDamage.Width + gv.scaler, btnCrtTypeOfDamage.Y + shiftForFont, "wh");
        }
        public void drawImages()
        {
            int shiftForFont = (btnCrtName.Height / 2) - (gv.fontHeight / 2);
            btnCrtTokenFilename.Draw();
            string token = gv.cc.allCreaturesList[creatureListIndex].cr_tokenFilename;
            int brX = gv.squareSize * gv.scaler;
            int brY = gv.squareSize * gv.scaler;
            gv.DrawText("TOKEN FILENAME: " + token, btnCrtTokenFilename.X + btnCrtTokenFilename.Width + gv.scaler, btnCrtTokenFilename.Y + shiftForFont, "wh");
            //top frame
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(token).Width, gv.cc.GetFromTileBitmapList(token).Height / 2);
            dst = new IbRect(btnCrtTokenFilename.X, btnCrtTokenFilename.Y + (gv.uiSquareSize / 2), brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(token), src, dst);
            //bottom frame
            src = new IbRect(0, gv.cc.GetFromTileBitmapList(token).Height / 2, gv.cc.GetFromTileBitmapList(token).Width, gv.cc.GetFromTileBitmapList(token).Height / 2);
            dst = new IbRect(btnCrtTokenFilename.X + brX, btnCrtTokenFilename.Y + (gv.uiSquareSize / 2), brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(token), src, dst);

            btnCrtAttackSound.Draw();
            gv.DrawText("ATTACK SOUND: " + gv.cc.allCreaturesList[creatureListIndex].cr_attackSound, btnCrtAttackSound.X + btnCrtAttackSound.Width + gv.scaler, btnCrtAttackSound.Y + shiftForFont, "wh");

            btnCrtSpriteProjectileFilename.Draw();
            gv.DrawText("PROJECTILE IMAGE: " + gv.cc.allCreaturesList[creatureListIndex].cr_projSpriteFilename, btnCrtSpriteProjectileFilename.X + btnCrtSpriteProjectileFilename.Width + gv.scaler, btnCrtSpriteProjectileFilename.Y + shiftForFont, "wh");
            token = gv.cc.allCreaturesList[creatureListIndex].cr_projSpriteFilename;
            brX = gv.squareSize * gv.scaler * 4;
            brY = gv.squareSize * gv.scaler;
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(token).Width, gv.cc.GetFromTileBitmapList(token).Height);
            dst = new IbRect(btnCrtSpriteProjectileFilename.X, btnCrtSpriteProjectileFilename.Y + (gv.uiSquareSize / 2), brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(token), src, dst);

            btnCrtSpriteEndingFilename.Draw();
            gv.DrawText("PROJECTILE ENDING IMAGE: " + gv.cc.allCreaturesList[creatureListIndex].cr_spriteEndingFilename, btnCrtSpriteEndingFilename.X + btnCrtSpriteEndingFilename.Width + gv.scaler, btnCrtSpriteEndingFilename.Y + shiftForFont, "wh");
            token = gv.cc.allCreaturesList[creatureListIndex].cr_spriteEndingFilename;
            brX = gv.squareSize * gv.scaler * 4;
            brY = gv.squareSize * gv.scaler;
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(token).Width, gv.cc.GetFromTileBitmapList(token).Height);
            dst = new IbRect(btnCrtSpriteEndingFilename.X, btnCrtSpriteEndingFilename.Y + (gv.uiSquareSize / 2), brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(token), src, dst);
        }
        public void drawResistances()
        {
            int shiftForFont = (btnCrtName.Height / 2) - (gv.fontHeight / 2);
            btnCrtReflex.Draw();
            gv.DrawText("REFLEX: " + gv.cc.allCreaturesList[creatureListIndex].reflex, btnCrtReflex.X + btnCrtReflex.Width + gv.scaler, btnCrtReflex.Y + shiftForFont, "wh");
            btnCrtWill.Draw();
            gv.DrawText("WILL: " + gv.cc.allCreaturesList[creatureListIndex].will, btnCrtWill.X + btnCrtWill.Width + gv.scaler, btnCrtWill.Y + shiftForFont, "wh");
            btnCrtFortitude.Draw();
            gv.DrawText("FORTITUDE: " + gv.cc.allCreaturesList[creatureListIndex].fortitude, btnCrtFortitude.X + btnCrtFortitude.Width + gv.scaler, btnCrtFortitude.Y + shiftForFont, "wh");
            btnCrtResistAcid.Draw();
            gv.DrawText("% RESIST ACID: " + gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueAcid, btnCrtResistAcid.X + btnCrtResistAcid.Width + gv.scaler, btnCrtResistAcid.Y + shiftForFont, "wh");
            btnCrtResistNormal.Draw();
            gv.DrawText("% RESIST NORMAL: " + gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueNormal, btnCrtResistNormal.X + btnCrtResistNormal.Width + gv.scaler, btnCrtResistNormal.Y + shiftForFont, "wh");
            btnCrtResistCold.Draw();
            gv.DrawText("% RESIST COLD: " + gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueCold, btnCrtResistCold.X + btnCrtResistCold.Width + gv.scaler, btnCrtResistCold.Y + shiftForFont, "wh");
            btnCrtResistElectricity.Draw();
            gv.DrawText("% RESIST ELECTRICITY: " + gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueElectricity, btnCrtResistElectricity.X + btnCrtResistElectricity.Width + gv.scaler, btnCrtResistElectricity.Y + shiftForFont, "wh");
            btnCrtResistFire.Draw();
            gv.DrawText("% RESIST FIRE: " + gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueFire, btnCrtResistFire.X + btnCrtResistFire.Width + gv.scaler, btnCrtResistFire.Y + shiftForFont, "wh");
            btnCrtResistMagic.Draw();
            gv.DrawText("% RESIST MAGIC: " + gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueMagic, btnCrtResistMagic.X + btnCrtResistMagic.Width + gv.scaler, btnCrtResistMagic.Y + shiftForFont, "wh");
            btnCrtResistPoison.Draw();
            gv.DrawText("% RESIST POISON: " + gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValuePoison, btnCrtResistPoison.X + btnCrtResistPoison.Width + gv.scaler, btnCrtResistPoison.Y + shiftForFont, "wh");
        }
        public void drawBehavior()
        {
            int shiftForFont = (btnCrtName.Height / 2) - (gv.fontHeight / 2);
            btnCrtAI.Draw();
            gv.DrawText("AI TYPE: " + gv.cc.allCreaturesList[creatureListIndex].cr_ai, btnCrtAI.X + btnCrtAI.Width + gv.scaler, btnCrtAI.Y + shiftForFont, "wh");
            btnCrtIniBonus.Draw();
            gv.DrawText("INITIATIVE BONUS: " + gv.cc.allCreaturesList[creatureListIndex].initiativeBonus, btnCrtIniBonus.X + btnCrtIniBonus.Width + gv.scaler, btnCrtIniBonus.Y + shiftForFont, "wh");
            btnCrtMoveDistance.Draw();
            gv.DrawText("MOVE DISTANCE: " + gv.cc.allCreaturesList[creatureListIndex].moveDistance, btnCrtMoveDistance.X + btnCrtMoveDistance.Width + gv.scaler, btnCrtMoveDistance.Y + shiftForFont, "wh");
            btnCrtOnDeathScript.Draw();
            gv.DrawText("ON DEATH SCRIPT: " + gv.cc.allCreaturesList[creatureListIndex].onDeathIBScript, btnCrtOnDeathScript.X + btnCrtOnDeathScript.Width + gv.scaler, btnCrtOnDeathScript.Y + shiftForFont, "wh");
            btnCrtOnDeathScriptParms.Draw();
            gv.DrawText("ON DEATH SCRIPT PARAMETERS: " + gv.cc.allCreaturesList[creatureListIndex].onDeathIBScriptParms, btnCrtOnDeathScriptParms.X + btnCrtOnDeathScriptParms.Width + gv.scaler, btnCrtOnDeathScriptParms.Y + shiftForFont, "wh");
            btnCrtOnScoreHitCastSpell.Draw();
            gv.DrawText("ON SCORE HIT CAST SPELL: " + gv.cc.allCreaturesList[creatureListIndex].onScoringHitCastSpellTag, btnCrtOnScoreHitCastSpell.X + btnCrtOnScoreHitCastSpell.Width + gv.scaler, btnCrtOnScoreHitCastSpell.Y + shiftForFont, "wh");

        }
        public void drawSpells()
        {
            int shiftForFont = (btnCrtName.Height / 2) - (gv.fontHeight / 2);
            //label      
            gv.DrawText("KNOWN SPELLS", btnAddKnownSpell.X, btnAddKnownSpell.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            btnAddKnownSpell.Draw();
            btnRemoveKnownSpell.Draw();
            //list all containers (tap on a container in the list to show elements for editing)
            int startX = btnAddKnownSpell.X;
            int startY = btnAddKnownSpell.Y + btnAddKnownSpell.Height - gv.fontHeight;
            int incY = gv.fontWidth + gv.fontLineSpacing;
            int cnt = 0;
            foreach (string c in gv.cc.allCreaturesList[creatureListIndex].knownSpellsTags)
            {
                if (cnt == knownSpellListIndex)
                {
                    gv.DrawText(c, startX, startY += incY, "gn");
                }
                else
                {
                    gv.DrawText(c, startX, startY += incY, "wh");
                }
                cnt++;
            }
        }

        public void onTouchTsCreatureEditor(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnHelp.glowOn = false;

            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }

            bool ret = gv.tsMainMenu.onTouchTsMainMenu(eX, eY, eventType);
            if (ret) { return; } //did some action on the main menu so do nothing here

            //TODO only allow editing of module creatures
            if ((creatureListIndex < gv.cc.allCreaturesList.Count) && (gv.cc.allCreaturesList[creatureListIndex].moduleCreature))
            {
                if (currentMode.Equals("Main"))
                {
                    ret = onTouchMain(eX, eY, eventType);
                    if (ret) { return; } //did some action on the tile panel so do nothing here
                }
                else if (currentMode.Equals("Attack"))
                {
                    ret = onTouchAttack(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Settings panel so do nothing here
                }
                else if (currentMode.Equals("Images"))
                {
                    ret = onTouchImages(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Info panel so do nothing here
                }
                else if (currentMode.Equals("Resistance"))
                {
                    ret = onTouchResistances(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Walk-LoS panel so do nothing here
                }
                else if (currentMode.Equals("Behavior"))
                {
                    ret = onTouchBehavior(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Walk-LoS panel so do nothing here
                }
                else if (currentMode.Equals("Spells"))
                {
                    ret = onTouchSpells(eX, eY, eventType);
                    if (ret) { return; } //did some action on the 3DPreview panel so do nothing here
                }
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

                    if (btnHelp.getImpact(x, y))
                    {
                        btnHelp.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnHelp.glowOn = false;

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

                    //check if tapped in the creature list area
                    if ((x > 0) && (x < tglMain.X) && (y > btnAddCreature.Y + btnAddCreature.Height))
                    {
                        //figure out which line was tapped and if that is a category do expand/collapse
                        int PanelLeftLocation = btnAddCreature.X;
                        int PanelTopLocation = btnAddCreature.Y + btnAddCreature.Height;
                        int lineIndex = (y - PanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing);
                        if (lineIndex < numberOfLinesToShow)
                        {
                            if (indexList[lineIndex] == -1)
                            {
                                string catName = categoryNameAtLine[lineIndex];
                                if (categoryList.ContainsKey(catName))
                                {
                                    //do collapse/expand
                                    categoryList[catName] = !categoryList[catName];
                                    //reset indexList
                                    resetIndexList();
                                }
                            }
                            else
                            {
                                creatureListIndex = indexList[lineIndex];
                            }
                        }
                    }

                    if (tglMain.getImpact(x, y))
                    {
                        tglMain.toggleOn = true;
                        tglAttack.toggleOn = false;
                        tglImages.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglSpells.toggleOn = false;
                        currentMode = "Main";
                    }
                    else if (tglAttack.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = true;
                        tglImages.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglSpells.toggleOn = false;
                        currentMode = "Attack";
                    }
                    else if (tglImages.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglImages.toggleOn = true;
                        tglResistance.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglSpells.toggleOn = false;
                        currentMode = "Images";
                    }
                    else if (tglResistance.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglImages.toggleOn = false;
                        tglResistance.toggleOn = true;
                        tglBehavior.toggleOn = false;
                        tglSpells.toggleOn = false;
                        currentMode = "Resistance";
                    }
                    else if (tglBehavior.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglImages.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglBehavior.toggleOn = true;
                        tglSpells.toggleOn = false;
                        currentMode = "Behavior";
                    }
                    else if (tglSpells.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglImages.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglSpells.toggleOn = true;
                        currentMode = "Spells";
                    }
                    else if (btnAddCreature.getImpact(x, y))
                    {
                        addCreature();
                    }
                    else if (btnRemoveCreature.getImpact(x, y))
                    {
                        removeCreature();
                    }
                    else if (btnCopyCreature.getImpact(x, y))
                    {
                        copyCreature();
                    }


                    else if (btnHelp.getImpact(x, y))
                    {
                        //incrementalSaveModule();
                    }
                    break;
            }
        }

        public bool onTouchMain(int eX, int eY, MouseEventType.EventType eventType)
        {
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnHelp.glowOn = false;

                    if (btnCrtName.getImpact(x, y))
                    {
                        changeCrtName();
                    }
                    else if (btnCrtResRef.getImpact(x, y))
                    {
                        changeCrtResRef();
                    }
                    else if (btnCrtParentNodeName.getImpact(x, y))
                    {
                        changeParentNodeName();                        
                    }
                    else if (btnCrtLevel.getImpact(x, y))
                    {
                        changeCrtLevel();
                    }
                    else if (btnCrtHP.getImpact(x, y))
                    {
                        changeCrtHP();
                    }
                    else if (btnCrtHPMax.getImpact(x, y))
                    {
                        changeCrtHPMax();
                    }
                    else if (btnCrtSP.getImpact(x, y))
                    {
                        changeCrtSP();
                    }
                    else if (btnCrtXP.getImpact(x, y))
                    {
                        changeCrtXP();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchAttack(int eX, int eY, MouseEventType.EventType eventType)
        {
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnHelp.glowOn = false;

                    if (btnCrtAC.getImpact(x, y))
                    {
                        changeCrtAC();
                    }
                    else if (btnCrtAttack.getImpact(x, y))
                    {
                        changeCrtAttack();
                    }
                    else if (btnCrtAttackRange.getImpact(x, y))
                    {
                        changeCrtAttackRange();
                    }
                    else if (btnCrtDamageNumOfDice.getImpact(x, y))
                    {
                        changeCrtDamageNumOfDice();
                    }
                    else if (btnCrtDamageDice.getImpact(x, y))
                    {
                        changeCrtDamageDice();
                    }
                    else if (btnCrtDamageAdder.getImpact(x, y))
                    {
                        changeCrtDamageAdder();
                    }
                    else if (btnCrtNumberOfAttacks.getImpact(x, y))
                    {
                        changeCrtNumberOfAttacks();
                    }
                    else if (btnCrtAttackCategory.getImpact(x, y))
                    {
                        changeCrtAttackCategory();
                    }
                    else if (btnCrtTypeOfDamage.getImpact(x, y))
                    {
                        changeCrtTypeOfDamage();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchImages(int eX, int eY, MouseEventType.EventType eventType)
        {
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnHelp.glowOn = false;

                    if (btnCrtTokenFilename.getImpact(x, y))
                    {
                        changeCrtToken();
                    }
                    else if (btnCrtAttackSound.getImpact(x, y))
                    {
                        changeCrtSound();
                    }
                    else if (btnCrtSpriteProjectileFilename.getImpact(x, y))
                    {
                        changeCrtProjectileImage();
                    }
                    else if (btnCrtSpriteEndingFilename.getImpact(x, y))
                    {
                        changeCrtProjectileEndingImage();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchResistances(int eX, int eY, MouseEventType.EventType eventType)
        {
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnHelp.glowOn = false;

                    if (btnCrtReflex.getImpact(x, y))
                    {
                        changeCrtReflex();
                    }
                    else if (btnCrtFortitude.getImpact(x, y))
                    {
                        changeCrtFortitude();
                    }
                    else if (btnCrtWill.getImpact(x, y))
                    {
                        changeCrtWill();
                    }
                    else if (btnCrtResistAcid.getImpact(x, y))
                    {
                        changeCrtResistAcid();
                    }
                    else if (btnCrtResistNormal.getImpact(x, y))
                    {
                        changeCrtResistNormal();
                    }
                    else if (btnCrtResistCold.getImpact(x, y))
                    {
                        changeCrtResistCold();
                    }
                    else if (btnCrtResistElectricity.getImpact(x, y))
                    {
                        changeCrtResistElectricity();
                    }
                    else if (btnCrtResistFire.getImpact(x, y))
                    {
                        changeCrtResistFire();
                    }
                    else if (btnCrtResistMagic.getImpact(x, y))
                    {
                        changeCrtResistMagic();
                    }
                    else if (btnCrtResistPoison.getImpact(x, y))
                    {
                        changeCrtResistPoison();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchBehavior(int eX, int eY, MouseEventType.EventType eventType)
        {
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnHelp.glowOn = false;

                    if (btnCrtAI.getImpact(x, y))
                    {
                        changeCrtAI();
                    }
                    else if (btnCrtIniBonus.getImpact(x, y))
                    {
                        changeCrtIniBonus();
                    }
                    else if (btnCrtMoveDistance.getImpact(x, y))
                    {
                        changeCrtMoveDistance();
                    }
                    else if (btnCrtOnDeathScript.getImpact(x, y))
                    {
                        changeCrtOnDeathScript();
                    }
                    else if (btnCrtOnDeathScriptParms.getImpact(x, y))
                    {
                        changeCrtOnDeathScriptParms();
                    }
                    else if (btnCrtOnScoreHitCastSpell.getImpact(x, y))
                    {
                        changeCrtOnScoreHitCastSpell();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchSpells(int eX, int eY, MouseEventType.EventType eventType)
        {
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnHelp.glowOn = false;

                    //TODO check if tapped in the known spells list area
                    if ((x > btnAddKnownSpell.X) && (y > btnAddKnownSpell.Y + btnAddKnownSpell.Height))
                    {
                        //figure out which line was tapped and if that is a category do expand/collapse
                        int PanelLeftLocation = btnAddKnownSpell.X;
                        int PanelTopLocation = btnAddKnownSpell.Y + btnAddKnownSpell.Height;
                        int lineIndex = (y - PanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing);
                        if ((lineIndex < gv.cc.allCreaturesList[creatureListIndex].knownSpellsTags.Count) && (gv.cc.allCreaturesList[creatureListIndex].knownSpellsTags.Count > 0))
                        {
                            knownSpellListIndex = lineIndex;
                        }
                    }

                    if (btnAddKnownSpell.getImpact(x, y))
                    {
                        addKnownSpell();
                    }
                    else if (btnRemoveKnownSpell.getImpact(x, y))
                    {
                        removeKnownSpell();
                    }
                    break;
            }
            return false;
        }

        //CREATURE PANEL
        public void addCreature()
        {
            if (creatureListIndex < gv.cc.allCreaturesList.Count)
            {
                Creature newCreature = new Creature();
                newCreature.cr_parentNodeName = gv.cc.allCreaturesList[creatureListIndex].cr_parentNodeName;
                int nextId = gv.mod.getNextIdNumber();
                newCreature.cr_tag = "newTag_" + nextId;
                newCreature.cr_resref = "newResRef_" + nextId;
                newCreature.moduleCreature = true;
                gv.cc.allCreaturesList.Add(newCreature);
                sortCreatureList();
                resetIndexList();
            }
        }
        public void removeCreature()
        {
            if (creatureListIndex < gv.cc.allCreaturesList.Count)
            {
                //TODO
                if (gv.cc.allCreaturesList.Count > 0)
                {
                    try
                    {
                        gv.cc.allCreaturesList.RemoveAt(creatureListIndex);
                        creatureListIndex = 0;
                    }
                    catch { }
                }
            }
        }
        public void copyCreature()
        {
            if (creatureListIndex < gv.cc.allCreaturesList.Count)
            {
                Creature newCreature = gv.cc.allCreaturesList[creatureListIndex].DeepCopy();
                newCreature.cr_parentNodeName = gv.cc.allCreaturesList[creatureListIndex].cr_parentNodeName;
                int nextId = gv.mod.getNextIdNumber();
                newCreature.cr_tag = "newTag_" + nextId;
                newCreature.cr_resref = "newResRef_" + nextId;
                newCreature.moduleCreature = true;
                gv.cc.allCreaturesList.Add(newCreature);
                sortCreatureList();
                resetIndexList();
            }
        }

        //MAIN
        public async void changeCrtName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Name for this creature", gv.cc.allCreaturesList[creatureListIndex].cr_name);
            gv.cc.allCreaturesList[creatureListIndex].cr_name = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtResRef()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Enter a unique resref for this creature.", gv.cc.allCreaturesList[creatureListIndex].cr_resref);
            gv.cc.allCreaturesList[creatureListIndex].cr_resref = myinput;
            gv.touchEnabled = true;
        }
        public async void changeParentNodeName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Enter a category name for organizing creatures into groups for the creatures panel on the left.", gv.cc.allCreaturesList[creatureListIndex].cr_parentNodeName);
            gv.cc.allCreaturesList[creatureListIndex].cr_parentNodeName = myinput;
            gv.touchEnabled = true;

            if (!categoryList.ContainsKey(gv.cc.allCreaturesList[creatureListIndex].cr_parentNodeName))
            {
                categoryList.Add(gv.cc.allCreaturesList[creatureListIndex].cr_parentNodeName, false);
            }
            sortCreatureList();
            resetIndexList();
        }
        public async void changeCrtLevel()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter level of the creature", gv.cc.allCreaturesList[creatureListIndex].cr_level);
            gv.cc.allCreaturesList[creatureListIndex].cr_level = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtHP()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the creature's hit points", gv.cc.allCreaturesList[creatureListIndex].hp);
            gv.cc.allCreaturesList[creatureListIndex].hp = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtHPMax()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the creature's maximum hit points", gv.cc.allCreaturesList[creatureListIndex].hpMax);
            gv.cc.allCreaturesList[creatureListIndex].hpMax = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtSP()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the creature's starting spell points", gv.cc.allCreaturesList[creatureListIndex].sp);
            gv.cc.allCreaturesList[creatureListIndex].sp = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtXP()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the amount of XP for defeating this creature", gv.cc.allCreaturesList[creatureListIndex].cr_XP);
            gv.cc.allCreaturesList[creatureListIndex].cr_XP = myinput;
            gv.touchEnabled = true;
        }

        //ATTACK
        public async void changeCrtAC()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the Armor Class for this creature", gv.cc.allCreaturesList[creatureListIndex].AC);
            gv.cc.allCreaturesList[creatureListIndex].AC = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtAttack()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the attack value for this creature", gv.cc.allCreaturesList[creatureListIndex].cr_att);
            gv.cc.allCreaturesList[creatureListIndex].cr_att = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtAttackRange()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the creature's attack range", gv.cc.allCreaturesList[creatureListIndex].cr_attRange);
            gv.cc.allCreaturesList[creatureListIndex].cr_attRange = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtDamageNumOfDice()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter number of dice rolled for damage (ex. the 2 in 2d4+1)", gv.cc.allCreaturesList[creatureListIndex].cr_damageNumDice);
            gv.cc.allCreaturesList[creatureListIndex].cr_damageNumDice = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtDamageDice()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter dice type used in damage roll (ex. the 4 in 2d4+1)", gv.cc.allCreaturesList[creatureListIndex].cr_damageDie);
            gv.cc.allCreaturesList[creatureListIndex].cr_damageDie = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtDamageAdder()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the damage adder (ex. the 1 in 2d4+1)", gv.cc.allCreaturesList[creatureListIndex].cr_damageAdder);
            gv.cc.allCreaturesList[creatureListIndex].cr_damageAdder = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtNumberOfAttacks()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the creature's number of attacks per round", gv.cc.allCreaturesList[creatureListIndex].cr_numberOfAttacks);
            gv.cc.allCreaturesList[creatureListIndex].cr_numberOfAttacks = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtAttackCategory()
        {
            List<string> items = new List<string>();
            items.Add("none");
            items.Add("Melee");
            items.Add("Ranged");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an attack type from the list:");
            if (selected != "none")
            {
                gv.cc.allCreaturesList[creatureListIndex].cr_category = selected;
            }
            gv.touchEnabled = true;
        }
        public async void changeCrtTypeOfDamage()
        {
            List<string> types = new List<string>(); //container, transition, conversation, encounter, script
            types.Add("none");
            types.Add("Normal");
            types.Add("Acid");
            types.Add("Cold");
            types.Add("Electricity");
            types.Add("Fire");
            types.Add("Magic");
            types.Add("Poison");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(types, "Select a type of damage from the list:");
            if (selected != "none")
            {
                gv.cc.allCreaturesList[creatureListIndex].cr_typeOfDamage = selected;
            }
            gv.touchEnabled = true;
        }

        //IMAGES
        public async void changeCrtToken()
        {
            List<string> items = GetCrtTokenList();
            items.Insert(0, "none");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an image for the creature:");
            if (selected != "none")
            {
                gv.cc.allCreaturesList[creatureListIndex].cr_tokenFilename = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetCrtTokenList()
        {
            List<string> crtTokenList = new List<string>();
            //MODULE SPECIFIC
            List<string> files = gv.GetAllFilesWithExtensionFromBothFolders("\\graphics", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            foreach (string f in files)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                if (filenameNoExt.StartsWith("tkn_"))
                {
                    crtTokenList.Add(filenameNoExt);
                }
            }            
            return crtTokenList;
        }
        public async void changeCrtSound()
        {
            List<string> items = GetCrtSoundList();
            items.Insert(0, "none");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a sound for this creature's default attack sound:");
            if (selected != "none")
            {
                gv.cc.allCreaturesList[creatureListIndex].cr_attackSound = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetCrtSoundList()
        {
            List<string> crtTokenList = new List<string>();
            //MODULE SPECIFIC
            List<string> files = gv.GetAllFilesWithExtensionFromBothFolders("\\sounds", "\\modules\\" + gv.mod.moduleName + "\\sounds", ".wav");
            foreach (string f in files)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                crtTokenList.Add(filenameNoExt);                
            }            
            return crtTokenList;
        }
        public async void changeCrtProjectileImage()
        {
            List<string> items = GetCrtProjectileImageList();
            items.Insert(0, "none");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an image for the creature's ranged projectile attack if it has one:");
            if (selected != "none")
            {
                gv.cc.allCreaturesList[creatureListIndex].cr_projSpriteFilename = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetCrtProjectileImageList()
        {
            List<string> crtTokenList = new List<string>();
            //MODULE SPECIFIC
            List<string> files = gv.GetAllFilesWithExtensionFromBothFolders("\\graphics", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            foreach (string f in files)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                if (filenameNoExt.StartsWith("fx_"))
                {
                    crtTokenList.Add(filenameNoExt);
                }
            }
            return crtTokenList;
        }
        public async void changeCrtProjectileEndingImage()
        {
            List<string> items = GetCrtProjectileEndingImageList();
            items.Insert(0, "none");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an image for the creature's ranged attack ending effect if it has one:");
            if (selected != "none")
            {
                gv.cc.allCreaturesList[creatureListIndex].cr_spriteEndingFilename = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetCrtProjectileEndingImageList()
        {
            List<string> crtTokenList = new List<string>();
            //MODULE SPECIFIC
            List<string> files = gv.GetAllFilesWithExtensionFromBothFolders("\\graphics", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            foreach (string f in files)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                if (filenameNoExt.StartsWith("fx_"))
                {
                    crtTokenList.Add(filenameNoExt);
                }
            }
            return crtTokenList;
        }

        //RESISTANCES
        public async void changeCrtReflex()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the reflex saving throw bonus for this creature", gv.cc.allCreaturesList[creatureListIndex].reflex);
            gv.cc.allCreaturesList[creatureListIndex].reflex = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtWill()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the will saving throw bonus for this creature", gv.cc.allCreaturesList[creatureListIndex].will);
            gv.cc.allCreaturesList[creatureListIndex].will = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtFortitude()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the fortitude saving throw bonus for this creature", gv.cc.allCreaturesList[creatureListIndex].fortitude);
            gv.cc.allCreaturesList[creatureListIndex].fortitude = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtResistAcid()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the acid resistance amount for this creature", gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueAcid);
            gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueAcid = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtResistNormal()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the normal resistance amount for this creature", gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueNormal);
            gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueNormal = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtResistCold()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the cold resistance amount for this creature", gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueCold);
            gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueCold = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtResistElectricity()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the electricity resistance amount for this creature", gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueElectricity);
            gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueElectricity = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtResistFire()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the fire resistance amount for this creature", gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueFire);
            gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueFire = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtResistMagic()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the magic resistance amount for this creature", gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueMagic);
            gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValueMagic = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtResistPoison()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the poison resistance amount for this creature", gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValuePoison);
            gv.cc.allCreaturesList[creatureListIndex].damageTypeResistanceValuePoison = myinput;
            gv.touchEnabled = true;
        }

        //BEHAVIOR
        public async void changeCrtAI()
        {
            List<string> types = new List<string>(); //container, transition, conversation, encounter, script
            types.Add("BasicAttacker");
            types.Add("GeneralCaster");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(types, "Select a type of AI for the creature:");
            if (selected != "none")
            {
                gv.cc.allCreaturesList[creatureListIndex].cr_ai = selected;
            }
            gv.touchEnabled = true;
        }
        public async void changeCrtIniBonus()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the initiative bonus for this creature", gv.cc.allCreaturesList[creatureListIndex].initiativeBonus);
            gv.cc.allCreaturesList[creatureListIndex].initiativeBonus = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtMoveDistance()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the move distance for this creature", gv.cc.allCreaturesList[creatureListIndex].moveDistance);
            gv.cc.allCreaturesList[creatureListIndex].moveDistance = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtOnDeathScript()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("The script to run when the creature dies", gv.cc.allCreaturesList[creatureListIndex].onDeathIBScript);
            gv.cc.allCreaturesList[creatureListIndex].onDeathIBScript = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtOnDeathScriptParms()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("The script input parameters for the OnDeathScript", gv.cc.allCreaturesList[creatureListIndex].onDeathIBScriptParms);
            gv.cc.allCreaturesList[creatureListIndex].onDeathIBScriptParms = myinput;
            gv.touchEnabled = true;
        }
        public async void changeCrtOnScoreHitCastSpell()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("The spell script to run when the creature scores a sucessful hit", gv.cc.allCreaturesList[creatureListIndex].onScoringHitCastSpellTag);
            gv.cc.allCreaturesList[creatureListIndex].onScoringHitCastSpellTag = myinput;
            gv.touchEnabled = true;
        }

        //SPELLS
        public async void addKnownSpell()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Spell sp in gv.cc.datafile.dataSpellsList)
            {
                items.Add(sp.name);
            }

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an item from the list to add to creaure's known spells:");
            if (selected != "none")
            {
                Spell sp = gv.cc.getSpellByName(selected);
                gv.cc.allCreaturesList[creatureListIndex].knownSpellsTags.Add(sp.tag);
            }
            gv.touchEnabled = true;
        }
        public void removeKnownSpell()
        {
            if (gv.cc.allCreaturesList[creatureListIndex].knownSpellsTags.Count > 0)
            {
                try
                {
                    gv.cc.allCreaturesList[creatureListIndex].knownSpellsTags.RemoveAt(knownSpellListIndex);
                    knownSpellListIndex = 0;
                }
                catch { }
            }
        }
    }
}
