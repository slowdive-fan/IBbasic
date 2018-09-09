using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ToolsetScreenItemEditor
    {
        public GameView gv;
        public int itemListIndex = 0;
        public int classListIndex = 0;
        public IbbButton btnAddItem = null;
        public IbbButton btnRemoveItem = null;
        public IbbButton btnCopyItem = null;
        public string currentMode = "Main"; //Main, Attack, Misc, Regen, Mod, Resistance, Image, Behavior, Class
        public IbbToggle tglMain = null;
        public IbbToggle tglAttack = null;
        public IbbToggle tglMisc = null;
        public IbbToggle tglRegen = null;
        public IbbToggle tglMod = null;
        public IbbToggle tglResistance = null;
        public IbbToggle tglImage = null;
        public IbbToggle tglBehavior = null;
        public IbbToggle tglClass = null;
        public Dictionary<string, bool> categoryList = new Dictionary<string, bool>();
        public int numberOfLinesToShow = 23;
        public List<int> indexList = new List<int>();
        public Dictionary<int, string> categoryNameAtLine = new Dictionary<int, string>();

        //MAIN (8)
        private IbbToggle btnItName = null;
        private IbbToggle btnItResRef = null;  //assign same name to tag      
        private IbbToggle btnItDesc = null;
        private IbbToggle btnItDescFull = null;
        private IbbToggle btnItCategory = null; //catergory type (Armor, Ranged, Melee, General, Ring, Shield, Ammo)
        private IbbToggle btnItValue = null;
        private IbbToggle btnItAmmoType = null;
        private IbbToggle btnItParentNodeName = null; //for organizing
        //ATTACK-DEFEND (8)
        private IbbToggle btnItAttackBonus = null;
        private IbbToggle btnItAttackRange = null;
        private IbbToggle btnItDamageNumDice = null;
        private IbbToggle btnItDamageDie = null;
        private IbbToggle btnItDamageAdder = null;
        private IbbToggle btnItArmorBonus = null;
        private IbbToggle btnItMaxDexBonus = null;
        private IbbToggle btnItTypeOfDamage = null; //Normal,Acid,Cold,Electricity,Fire,Magic,Poison
        //MISC (8)
        private IbbToggle btnItCanNotBeUnequipped = null;
        private IbbToggle btnItPlotItem = null;
        private IbbToggle btnItIsRation = null;
        private IbbToggle btnItTwoHanded = null;
        private IbbToggle btnItQuantity = null;
        private IbbToggle btnItGroupSizeForSellingStackableItems = null;
        private IbbToggle btnItCharges = null;
        private IbbToggle btnItIsStackable = null;

        //REGENS-SAVES (8)
        private IbbToggle btnItSpRegenPerRoundInCombat = null;
        private IbbToggle btnItHpRegenPerRoundInCombat = null;
        private IbbToggle btnItRoundsPerSpRegenOutsideCombat = null;
        private IbbToggle btnItRoundsPerHpRegenOutsideCombat = null;
        private IbbToggle btnItSavingThrowModifierReflex = null;
        private IbbToggle btnItSavingThrowModifierFortitude = null;
        private IbbToggle btnItSavingThrowModifierWill = null;
        private IbbToggle btnItUseableInSituation = null; //InCombat, OutOfCombat, Always, Passive
        //MODIFIERS (8)
        private IbbToggle btnItAttributeBonusModifierStr = null;
        private IbbToggle btnItAttributeBonusModifierDex = null;
        private IbbToggle btnItAttributeBonusModifierInt = null;
        private IbbToggle btnItAttributeBonusModifierCha = null;
        private IbbToggle btnItAttributeBonusModifierCon = null;
        private IbbToggle btnItAttributeBonusModifierWis = null;
        private IbbToggle btnItMovementPointModifier = null;
        private IbbToggle btnItArmorWeightType = null; //Light, Medium, Heavy 
        //RESISTANCES (7)
        private IbbToggle btnItResistAcid = null;
        private IbbToggle btnItResistNormal = null;
        private IbbToggle btnItResistCold = null;
        private IbbToggle btnItResistElectricity = null;
        private IbbToggle btnItResistFire = null;
        private IbbToggle btnItResistMagic = null;
        private IbbToggle btnItResistPoison = null;

        //IMAGES-SOUNDS (5)
        private IbbToggle btnItImage = null;
        private IbbToggle btnItSpriteProjectileFilename = null;
        private IbbToggle btnItSpriteEndingFilename = null;
        private IbbToggle btnItOnUseSound = null;
        private IbbToggle btnItEndSound = null;
        //BEHAVIOR (8)
        private IbbToggle btnItAutomaticallyHitsTarget = null;
        private IbbToggle btnItAreaOfEffect = null;
        private IbbToggle btnItAoeShape = null;
        private IbbToggle btnItOnScoringHitCastSpellTag = null;
        private IbbToggle btnItOnUseItemCastSpellTag = null;
        private IbbToggle btnItDestroyItemAfterOnUseItemCastSpell = null;
        private IbbToggle btnItLevelOfItemForCastSpell = null;
        private IbbToggle btnItUsePlayerClassLevelForOnUseItemCastSpell = null;
        //CLASSES ALLOWED (2)
        //public List<string> classesAllowed = new List<string>();
        //maybe use toggles for classes to turn on/off
        public IbbButton btnAddClass = null;
        public IbbButton btnRemoveClass = null;

        private IbbButton btnHelp = null;

        public IbRect src = null;
        public IbRect dst = null;

        public ToolsetScreenItemEditor(GameView g)
        {
            gv = g;
            currentMode = "Main";
            setControlsStart();
            sortItemList();
            tglMain.toggleOn = true;
            tglAttack.toggleOn = false;
            tglImage.toggleOn = false;
            tglResistance.toggleOn = false;
            tglBehavior.toggleOn = false;
            tglClass.toggleOn = false;

            //initialize categoryList
            foreach (Item it in gv.cc.allItemsList)
            {
                if (!categoryList.ContainsKey(it.ItemCategoryName))
                {
                    categoryList.Add(it.ItemCategoryName, true);
                }
            }

            resetIndexList();
        }
        public void sortItemList()
        {
            //sort creatures based on category type
            gv.cc.allItemsList = gv.cc.allItemsList.OrderBy(x => x.name).ToList();
            gv.cc.allItemsList = gv.cc.allItemsList.OrderBy(x => x.ItemCategoryName).ToList();
        }
        public void resetIndexList()
        {
            int cnt = 0;
            int lineIndx = 0;
            string lastCategory = "";
            indexList.Clear();
            categoryNameAtLine.Clear();
            foreach (Item it in gv.cc.allItemsList)
            {
                if (!it.ItemCategoryName.Equals(lastCategory))
                {
                    lastCategory = it.ItemCategoryName;
                    //new category so add -1 to index list
                    indexList.Add(-1);
                    categoryNameAtLine.Add(lineIndx, it.ItemCategoryName);
                    lineIndx++;
                }
                if ((categoryList.ContainsKey(it.ItemCategoryName)) && (categoryList[it.ItemCategoryName]))
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
            if (btnAddItem == null)
            {
                btnAddItem = new IbbButton(gv, 0.8f);
            }
            //btnAddItem.Text = "ADD";
            btnAddItem.Img = "btn_small";
            btnAddItem.Img2 = "btnadd";
            btnAddItem.Glow = "btn_small_glow";
            btnAddItem.X = 0 * gv.uiSquareSize + 1 * gv.fontWidth;
            btnAddItem.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnAddItem.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddItem.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveItem == null)
            {
                btnRemoveItem = new IbbButton(gv, 0.8f);
            }
            //btnRemoveItem.Text = "REMOVE";
            btnRemoveItem.Img = "btn_small";
            btnRemoveItem.Img2 = "btnremove";
            btnRemoveItem.Glow = "btn_small_glow";
            btnRemoveItem.X = 1 * gv.uiSquareSize + 1 * gv.fontWidth;
            btnRemoveItem.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnRemoveItem.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveItem.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCopyItem == null)
            {
                btnCopyItem = new IbbButton(gv, 0.8f);
            }
            //btnCopyItem.Text = "COPY";
            btnCopyItem.Img = "btn_small";
            btnCopyItem.Img2 = "btncopy";
            btnCopyItem.Glow = "btn_small_glow";
            btnCopyItem.X = 2 * gv.uiSquareSize + 1 * gv.fontWidth;
            btnCopyItem.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnCopyItem.Height = (int)(gv.ibbheight * gv.scaler);
            btnCopyItem.Width = (int)(gv.ibbwidthR * gv.scaler);

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

            if (tglMisc == null)
            {
                tglMisc = new IbbToggle(gv);
            }
            tglMisc.ImgOn = "mtgl_rbtn_on";
            tglMisc.ImgOff = "mtgl_rbtn_off";
            tglMisc.X = 3 * gv.uiSquareSize + 2 * gv.fontWidth;
            tglMisc.Y = 1 * gv.uiSquareSize + 2 * gv.fontHeight;
            tglMisc.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglMisc.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglRegen == null)
            {
                tglRegen = new IbbToggle(gv);
            }
            tglRegen.ImgOn = "mtgl_rbtn_on";
            tglRegen.ImgOff = "mtgl_rbtn_off";
            tglRegen.X = 6 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglRegen.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            tglRegen.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglRegen.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglMod == null)
            {
                tglMod = new IbbToggle(gv);
            }
            tglMod.ImgOn = "mtgl_rbtn_on";
            tglMod.ImgOff = "mtgl_rbtn_off";
            tglMod.X = 6 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglMod.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 2 * gv.fontHeight;
            tglMod.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglMod.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglResistance == null)
            {
                tglResistance = new IbbToggle(gv);
            }
            tglResistance.ImgOn = "mtgl_rbtn_on";
            tglResistance.ImgOff = "mtgl_rbtn_off";
            tglResistance.X = 6 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglResistance.Y = 1 * gv.uiSquareSize + 2 * gv.fontHeight;
            tglResistance.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglResistance.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglImage == null)
            {
                tglImage = new IbbToggle(gv);
            }
            tglImage.ImgOn = "mtgl_rbtn_on";
            tglImage.ImgOff = "mtgl_rbtn_off";
            tglImage.X = 9 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglImage.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            tglImage.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglImage.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglBehavior == null)
            {
                tglBehavior = new IbbToggle(gv);
            }
            tglBehavior.ImgOn = "mtgl_rbtn_on";
            tglBehavior.ImgOff = "mtgl_rbtn_off";
            tglBehavior.X = 9 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglBehavior.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 2 * gv.fontHeight;
            tglBehavior.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglBehavior.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglClass == null)
            {
                tglClass = new IbbToggle(gv);
            }
            tglClass.ImgOn = "mtgl_rbtn_on";
            tglClass.ImgOff = "mtgl_rbtn_off";
            tglClass.X = 9 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglClass.Y = 1 * gv.uiSquareSize + 2 * gv.fontHeight;
            tglClass.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglClass.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);
        }

        public void setMainControlsStart()
        {

            if (btnItName == null)
            {
                btnItName = new IbbToggle(gv);
            }
            btnItName.ImgOn = "mtgl_edit_btn";
            btnItName.ImgOff = "mtgl_edit_btn";
            btnItName.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItName.Y = 2 * gv.uiSquareSize;
            btnItName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItResRef == null)
            {
                btnItResRef = new IbbToggle(gv);
            }
            btnItResRef.ImgOn = "mtgl_edit_btn";
            btnItResRef.ImgOff = "mtgl_edit_btn";
            btnItResRef.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResRef.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResRef.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItResRef.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItDesc == null)
            {
                btnItDesc = new IbbToggle(gv);
            }
            btnItDesc.ImgOn = "mtgl_edit_btn";
            btnItDesc.ImgOff = "mtgl_edit_btn";
            btnItDesc.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItDesc.Y = 3 * gv.uiSquareSize;
            btnItDesc.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItDesc.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItDescFull == null)
            {
                btnItDescFull = new IbbToggle(gv);
            }
            btnItDescFull.ImgOn = "mtgl_edit_btn";
            btnItDescFull.ImgOff = "mtgl_edit_btn";
            btnItDescFull.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItDescFull.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItDescFull.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItDescFull.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItCategory == null)
            {
                btnItCategory = new IbbToggle(gv);
            }
            btnItCategory.ImgOn = "mtgl_edit_btn";
            btnItCategory.ImgOff = "mtgl_edit_btn";
            btnItCategory.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItCategory.Y = 4 * gv.uiSquareSize;
            btnItCategory.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItCategory.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItValue == null)
            {
                btnItValue = new IbbToggle(gv);
            }
            btnItValue.ImgOn = "mtgl_edit_btn";
            btnItValue.ImgOff = "mtgl_edit_btn";
            btnItValue.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItValue.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItValue.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItValue.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItAmmoType == null)
            {
                btnItAmmoType = new IbbToggle(gv);
            }
            btnItAmmoType.ImgOn = "mtgl_edit_btn";
            btnItAmmoType.ImgOff = "mtgl_edit_btn";
            btnItAmmoType.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAmmoType.Y = 5 * gv.uiSquareSize;
            btnItAmmoType.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAmmoType.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItParentNodeName == null)
            {
                btnItParentNodeName = new IbbToggle(gv);
            }
            btnItParentNodeName.ImgOn = "mtgl_edit_btn";
            btnItParentNodeName.ImgOff = "mtgl_edit_btn";
            btnItParentNodeName.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItParentNodeName.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItParentNodeName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItParentNodeName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
            }
            //btnHelp.Text = "HELP";
            btnHelp.Img = "btn_small";
            btnHelp.Img2 = "btnhelp";
            btnHelp.Glow = "btn_small_glow";
            btnHelp.X = 10 * gv.uiSquareSize;
            btnHelp.Y = 6 * gv.uiSquareSize;
            btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
            btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        public void setAttackControlsStart()
        {

            if (btnItAttackBonus == null)
            {
                btnItAttackBonus = new IbbToggle(gv);
            }
            btnItAttackBonus.ImgOn = "mtgl_edit_btn";
            btnItAttackBonus.ImgOff = "mtgl_edit_btn";
            btnItAttackBonus.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttackBonus.Y = 2 * gv.uiSquareSize;
            btnItAttackBonus.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAttackBonus.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItAttackRange == null)
            {
                btnItAttackRange = new IbbToggle(gv);
            }
            btnItAttackRange.ImgOn = "mtgl_edit_btn";
            btnItAttackRange.ImgOff = "mtgl_edit_btn";
            btnItAttackRange.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttackRange.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttackRange.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAttackRange.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItDamageNumDice == null)
            {
                btnItDamageNumDice = new IbbToggle(gv);
            }
            btnItDamageNumDice.ImgOn = "mtgl_edit_btn";
            btnItDamageNumDice.ImgOff = "mtgl_edit_btn";
            btnItDamageNumDice.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItDamageNumDice.Y = 3 * gv.uiSquareSize;
            btnItDamageNumDice.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItDamageNumDice.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItDamageDie == null)
            {
                btnItDamageDie = new IbbToggle(gv);
            }
            btnItDamageDie.ImgOn = "mtgl_edit_btn";
            btnItDamageDie.ImgOff = "mtgl_edit_btn";
            btnItDamageDie.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItDamageDie.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItDamageDie.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItDamageDie.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItDamageAdder == null)
            {
                btnItDamageAdder = new IbbToggle(gv);
            }
            btnItDamageAdder.ImgOn = "mtgl_edit_btn";
            btnItDamageAdder.ImgOff = "mtgl_edit_btn";
            btnItDamageAdder.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItDamageAdder.Y = 4 * gv.uiSquareSize;
            btnItDamageAdder.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItDamageAdder.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItArmorBonus == null)
            {
                btnItArmorBonus = new IbbToggle(gv);
            }
            btnItArmorBonus.ImgOn = "mtgl_edit_btn";
            btnItArmorBonus.ImgOff = "mtgl_edit_btn";
            btnItArmorBonus.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItArmorBonus.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItArmorBonus.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItArmorBonus.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItMaxDexBonus == null)
            {
                btnItMaxDexBonus = new IbbToggle(gv);
            }
            btnItMaxDexBonus.ImgOn = "mtgl_edit_btn";
            btnItMaxDexBonus.ImgOff = "mtgl_edit_btn";
            btnItMaxDexBonus.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItMaxDexBonus.Y = 5 * gv.uiSquareSize;
            btnItMaxDexBonus.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItMaxDexBonus.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItTypeOfDamage == null)
            {
                btnItTypeOfDamage = new IbbToggle(gv);
            }
            btnItTypeOfDamage.ImgOn = "mtgl_edit_btn";
            btnItTypeOfDamage.ImgOff = "mtgl_edit_btn";
            btnItTypeOfDamage.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItTypeOfDamage.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItTypeOfDamage.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItTypeOfDamage.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }
        public void setMiscControlsStart()
        {
            if (btnItCanNotBeUnequipped == null)
            {
                btnItCanNotBeUnequipped = new IbbToggle(gv);
            }
            btnItCanNotBeUnequipped.ImgOn = "mtgl_rbtn_on";
            btnItCanNotBeUnequipped.ImgOff = "mtgl_rbtn_off";
            btnItCanNotBeUnequipped.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItCanNotBeUnequipped.Y = 2 * gv.uiSquareSize;
            btnItCanNotBeUnequipped.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItCanNotBeUnequipped.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItPlotItem == null)
            {
                btnItPlotItem = new IbbToggle(gv);
            }
            btnItPlotItem.ImgOn = "mtgl_rbtn_on";
            btnItPlotItem.ImgOff = "mtgl_rbtn_off";
            btnItPlotItem.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItPlotItem.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItPlotItem.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItPlotItem.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItIsRation == null)
            {
                btnItIsRation = new IbbToggle(gv);
            }
            btnItIsRation.ImgOn = "mtgl_rbtn_on";
            btnItIsRation.ImgOff = "mtgl_rbtn_off";
            btnItIsRation.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItIsRation.Y = 3 * gv.uiSquareSize;
            btnItIsRation.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItIsRation.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItTwoHanded == null)
            {
                btnItTwoHanded = new IbbToggle(gv);
            }
            btnItTwoHanded.ImgOn = "mtgl_rbtn_on";
            btnItTwoHanded.ImgOff = "mtgl_rbtn_off";
            btnItTwoHanded.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItTwoHanded.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItTwoHanded.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItTwoHanded.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItQuantity == null)
            {
                btnItQuantity = new IbbToggle(gv);
            }
            btnItQuantity.ImgOn = "mtgl_edit_btn";
            btnItQuantity.ImgOff = "mtgl_edit_btn";
            btnItQuantity.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItQuantity.Y = 4 * gv.uiSquareSize;
            btnItQuantity.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItQuantity.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItGroupSizeForSellingStackableItems == null)
            {
                btnItGroupSizeForSellingStackableItems = new IbbToggle(gv);
            }
            btnItGroupSizeForSellingStackableItems.ImgOn = "mtgl_edit_btn";
            btnItGroupSizeForSellingStackableItems.ImgOff = "mtgl_edit_btn";
            btnItGroupSizeForSellingStackableItems.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItGroupSizeForSellingStackableItems.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItGroupSizeForSellingStackableItems.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItGroupSizeForSellingStackableItems.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItCharges == null)
            {
                btnItCharges = new IbbToggle(gv);
            }
            btnItCharges.ImgOn = "mtgl_edit_btn";
            btnItCharges.ImgOff = "mtgl_edit_btn";
            btnItCharges.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItCharges.Y = 5 * gv.uiSquareSize;
            btnItCharges.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItCharges.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItIsStackable == null)
            {
                btnItIsStackable = new IbbToggle(gv);
            }
            btnItIsStackable.ImgOn = "mtgl_rbtn_on";
            btnItIsStackable.ImgOff = "mtgl_rbtn_off";
            btnItIsStackable.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItIsStackable.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItIsStackable.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItIsStackable.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }

        public void setRegenControlsStart()
        {
            if (btnItSpRegenPerRoundInCombat == null)
            {
                btnItSpRegenPerRoundInCombat = new IbbToggle(gv);
            }
            btnItSpRegenPerRoundInCombat.ImgOn = "mtgl_edit_btn";
            btnItSpRegenPerRoundInCombat.ImgOff = "mtgl_edit_btn";
            btnItSpRegenPerRoundInCombat.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItSpRegenPerRoundInCombat.Y = 2 * gv.uiSquareSize;
            btnItSpRegenPerRoundInCombat.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItSpRegenPerRoundInCombat.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItHpRegenPerRoundInCombat == null)
            {
                btnItHpRegenPerRoundInCombat = new IbbToggle(gv);
            }
            btnItHpRegenPerRoundInCombat.ImgOn = "mtgl_edit_btn";
            btnItHpRegenPerRoundInCombat.ImgOff = "mtgl_edit_btn";
            btnItHpRegenPerRoundInCombat.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItHpRegenPerRoundInCombat.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItHpRegenPerRoundInCombat.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItHpRegenPerRoundInCombat.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItRoundsPerSpRegenOutsideCombat == null)
            {
                btnItRoundsPerSpRegenOutsideCombat = new IbbToggle(gv);
            }
            btnItRoundsPerSpRegenOutsideCombat.ImgOn = "mtgl_edit_btn";
            btnItRoundsPerSpRegenOutsideCombat.ImgOff = "mtgl_edit_btn";
            btnItRoundsPerSpRegenOutsideCombat.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItRoundsPerSpRegenOutsideCombat.Y = 3 * gv.uiSquareSize;
            btnItRoundsPerSpRegenOutsideCombat.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItRoundsPerSpRegenOutsideCombat.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItRoundsPerHpRegenOutsideCombat == null)
            {
                btnItRoundsPerHpRegenOutsideCombat = new IbbToggle(gv);
            }
            btnItRoundsPerHpRegenOutsideCombat.ImgOn = "mtgl_edit_btn";
            btnItRoundsPerHpRegenOutsideCombat.ImgOff = "mtgl_edit_btn";
            btnItRoundsPerHpRegenOutsideCombat.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItRoundsPerHpRegenOutsideCombat.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItRoundsPerHpRegenOutsideCombat.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItRoundsPerHpRegenOutsideCombat.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItSavingThrowModifierReflex == null)
            {
                btnItSavingThrowModifierReflex = new IbbToggle(gv);
            }
            btnItSavingThrowModifierReflex.ImgOn = "mtgl_edit_btn";
            btnItSavingThrowModifierReflex.ImgOff = "mtgl_edit_btn";
            btnItSavingThrowModifierReflex.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItSavingThrowModifierReflex.Y = 4 * gv.uiSquareSize;
            btnItSavingThrowModifierReflex.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItSavingThrowModifierReflex.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItSavingThrowModifierFortitude == null)
            {
                btnItSavingThrowModifierFortitude = new IbbToggle(gv);
            }
            btnItSavingThrowModifierFortitude.ImgOn = "mtgl_edit_btn";
            btnItSavingThrowModifierFortitude.ImgOff = "mtgl_edit_btn";
            btnItSavingThrowModifierFortitude.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItSavingThrowModifierFortitude.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItSavingThrowModifierFortitude.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItSavingThrowModifierFortitude.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItSavingThrowModifierWill == null)
            {
                btnItSavingThrowModifierWill = new IbbToggle(gv);
            }
            btnItSavingThrowModifierWill.ImgOn = "mtgl_edit_btn";
            btnItSavingThrowModifierWill.ImgOff = "mtgl_edit_btn";
            btnItSavingThrowModifierWill.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItSavingThrowModifierWill.Y = 5 * gv.uiSquareSize;
            btnItSavingThrowModifierWill.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItSavingThrowModifierWill.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItUseableInSituation == null)
            {
                btnItUseableInSituation = new IbbToggle(gv);
            }
            btnItUseableInSituation.ImgOn = "mtgl_edit_btn";
            btnItUseableInSituation.ImgOff = "mtgl_edit_btn";
            btnItUseableInSituation.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItUseableInSituation.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItUseableInSituation.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItUseableInSituation.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }
        public void setModControlsStart()
        {
            if (btnItAttributeBonusModifierStr == null)
            {
                btnItAttributeBonusModifierStr = new IbbToggle(gv);
            }
            btnItAttributeBonusModifierStr.ImgOn = "mtgl_edit_btn";
            btnItAttributeBonusModifierStr.ImgOff = "mtgl_edit_btn";
            btnItAttributeBonusModifierStr.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttributeBonusModifierStr.Y = 2 * gv.uiSquareSize;
            btnItAttributeBonusModifierStr.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAttributeBonusModifierStr.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItAttributeBonusModifierDex == null)
            {
                btnItAttributeBonusModifierDex = new IbbToggle(gv);
            }
            btnItAttributeBonusModifierDex.ImgOn = "mtgl_edit_btn";
            btnItAttributeBonusModifierDex.ImgOff = "mtgl_edit_btn";
            btnItAttributeBonusModifierDex.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttributeBonusModifierDex.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttributeBonusModifierDex.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAttributeBonusModifierDex.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItAttributeBonusModifierInt == null)
            {
                btnItAttributeBonusModifierInt = new IbbToggle(gv);
            }
            btnItAttributeBonusModifierInt.ImgOn = "mtgl_edit_btn";
            btnItAttributeBonusModifierInt.ImgOff = "mtgl_edit_btn";
            btnItAttributeBonusModifierInt.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttributeBonusModifierInt.Y = 3 * gv.uiSquareSize;
            btnItAttributeBonusModifierInt.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAttributeBonusModifierInt.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItAttributeBonusModifierCha == null)
            {
                btnItAttributeBonusModifierCha = new IbbToggle(gv);
            }
            btnItAttributeBonusModifierCha.ImgOn = "mtgl_edit_btn";
            btnItAttributeBonusModifierCha.ImgOff = "mtgl_edit_btn";
            btnItAttributeBonusModifierCha.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttributeBonusModifierCha.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttributeBonusModifierCha.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAttributeBonusModifierCha.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItAttributeBonusModifierCon == null)
            {
                btnItAttributeBonusModifierCon = new IbbToggle(gv);
            }
            btnItAttributeBonusModifierCon.ImgOn = "mtgl_edit_btn";
            btnItAttributeBonusModifierCon.ImgOff = "mtgl_edit_btn";
            btnItAttributeBonusModifierCon.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttributeBonusModifierCon.Y = 4 * gv.uiSquareSize;
            btnItAttributeBonusModifierCon.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAttributeBonusModifierCon.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItAttributeBonusModifierWis == null)
            {
                btnItAttributeBonusModifierWis = new IbbToggle(gv);
            }
            btnItAttributeBonusModifierWis.ImgOn = "mtgl_edit_btn";
            btnItAttributeBonusModifierWis.ImgOff = "mtgl_edit_btn";
            btnItAttributeBonusModifierWis.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttributeBonusModifierWis.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAttributeBonusModifierWis.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAttributeBonusModifierWis.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItMovementPointModifier == null)
            {
                btnItMovementPointModifier = new IbbToggle(gv);
            }
            btnItMovementPointModifier.ImgOn = "mtgl_edit_btn";
            btnItMovementPointModifier.ImgOff = "mtgl_edit_btn";
            btnItMovementPointModifier.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItMovementPointModifier.Y = 5 * gv.uiSquareSize;
            btnItMovementPointModifier.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItMovementPointModifier.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItArmorWeightType == null)
            {
                btnItArmorWeightType = new IbbToggle(gv);
            }
            btnItArmorWeightType.ImgOn = "mtgl_edit_btn";
            btnItArmorWeightType.ImgOff = "mtgl_edit_btn";
            btnItArmorWeightType.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItArmorWeightType.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItArmorWeightType.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItArmorWeightType.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }
        public void setResistanceControlsStart()
        {
            if (btnItResistAcid == null)
            {
                btnItResistAcid = new IbbToggle(gv);
            }
            btnItResistAcid.ImgOn = "mtgl_edit_btn";
            btnItResistAcid.ImgOff = "mtgl_edit_btn";
            btnItResistAcid.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResistAcid.Y = 2 * gv.uiSquareSize;
            btnItResistAcid.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItResistAcid.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItResistNormal == null)
            {
                btnItResistNormal = new IbbToggle(gv);
            }
            btnItResistNormal.ImgOn = "mtgl_edit_btn";
            btnItResistNormal.ImgOff = "mtgl_edit_btn";
            btnItResistNormal.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResistNormal.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResistNormal.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItResistNormal.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItResistCold == null)
            {
                btnItResistCold = new IbbToggle(gv);
            }
            btnItResistCold.ImgOn = "mtgl_edit_btn";
            btnItResistCold.ImgOff = "mtgl_edit_btn";
            btnItResistCold.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResistCold.Y = 3 * gv.uiSquareSize;
            btnItResistCold.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItResistCold.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItResistElectricity == null)
            {
                btnItResistElectricity = new IbbToggle(gv);
            }
            btnItResistElectricity.ImgOn = "mtgl_edit_btn";
            btnItResistElectricity.ImgOff = "mtgl_edit_btn";
            btnItResistElectricity.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResistElectricity.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResistElectricity.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItResistElectricity.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItResistFire == null)
            {
                btnItResistFire = new IbbToggle(gv);
            }
            btnItResistFire.ImgOn = "mtgl_edit_btn";
            btnItResistFire.ImgOff = "mtgl_edit_btn";
            btnItResistFire.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResistFire.Y = 4 * gv.uiSquareSize;
            btnItResistFire.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItResistFire.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItResistMagic == null)
            {
                btnItResistMagic = new IbbToggle(gv);
            }
            btnItResistMagic.ImgOn = "mtgl_edit_btn";
            btnItResistMagic.ImgOff = "mtgl_edit_btn";
            btnItResistMagic.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResistMagic.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResistMagic.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItResistMagic.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItResistPoison == null)
            {
                btnItResistPoison = new IbbToggle(gv);
            }
            btnItResistPoison.ImgOn = "mtgl_edit_btn";
            btnItResistPoison.ImgOff = "mtgl_edit_btn";
            btnItResistPoison.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItResistPoison.Y = 5 * gv.uiSquareSize;
            btnItResistPoison.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItResistPoison.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }

        public void setImageControlsStart()
        {

            if (btnItImage == null)
            {
                btnItImage = new IbbToggle(gv);
            }
            btnItImage.ImgOn = "mtgl_edit_btn";
            btnItImage.ImgOff = "mtgl_edit_btn";
            btnItImage.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItImage.Y = 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItImage.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItImage.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItSpriteProjectileFilename == null)
            {
                btnItSpriteProjectileFilename = new IbbToggle(gv);
            }
            btnItSpriteProjectileFilename.ImgOn = "mtgl_edit_btn";
            btnItSpriteProjectileFilename.ImgOff = "mtgl_edit_btn";
            btnItSpriteProjectileFilename.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItSpriteProjectileFilename.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItSpriteProjectileFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItSpriteProjectileFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItSpriteEndingFilename == null)
            {
                btnItSpriteEndingFilename = new IbbToggle(gv);
            }
            btnItSpriteEndingFilename.ImgOn = "mtgl_edit_btn";
            btnItSpriteEndingFilename.ImgOff = "mtgl_edit_btn";
            btnItSpriteEndingFilename.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItSpriteEndingFilename.Y = 5 * gv.uiSquareSize;
            btnItSpriteEndingFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItSpriteEndingFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItOnUseSound == null)
            {
                btnItOnUseSound = new IbbToggle(gv);
            }
            btnItOnUseSound.ImgOn = "mtgl_edit_btn";
            btnItOnUseSound.ImgOff = "mtgl_edit_btn";
            btnItOnUseSound.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItOnUseSound.Y = 3 * gv.uiSquareSize;
            btnItOnUseSound.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItOnUseSound.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItEndSound == null)
            {
                btnItEndSound = new IbbToggle(gv);
            }
            btnItEndSound.ImgOn = "mtgl_edit_btn";
            btnItEndSound.ImgOff = "mtgl_edit_btn";
            btnItEndSound.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItEndSound.Y = 3 * gv.uiSquareSize;
            btnItEndSound.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItEndSound.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }
        public void setBehaviorControlsStart()
        {
            if (btnItAutomaticallyHitsTarget == null)
            {
                btnItAutomaticallyHitsTarget = new IbbToggle(gv);
            }
            btnItAutomaticallyHitsTarget.ImgOn = "mtgl_rbtn_on";
            btnItAutomaticallyHitsTarget.ImgOff = "mtgl_rbtn_off";
            btnItAutomaticallyHitsTarget.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAutomaticallyHitsTarget.Y = 2 * gv.uiSquareSize;
            btnItAutomaticallyHitsTarget.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAutomaticallyHitsTarget.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItAreaOfEffect == null)
            {
                btnItAreaOfEffect = new IbbToggle(gv);
            }
            btnItAreaOfEffect.ImgOn = "mtgl_edit_btn";
            btnItAreaOfEffect.ImgOff = "mtgl_edit_btn";
            btnItAreaOfEffect.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAreaOfEffect.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAreaOfEffect.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAreaOfEffect.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItAoeShape == null)
            {
                btnItAoeShape = new IbbToggle(gv);
            }
            btnItAoeShape.ImgOn = "mtgl_edit_btn";
            btnItAoeShape.ImgOff = "mtgl_edit_btn";
            btnItAoeShape.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItAoeShape.Y = 3 * gv.uiSquareSize;
            btnItAoeShape.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItAoeShape.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItOnScoringHitCastSpellTag == null)
            {
                btnItOnScoringHitCastSpellTag = new IbbToggle(gv);
            }
            btnItOnScoringHitCastSpellTag.ImgOn = "mtgl_edit_btn";
            btnItOnScoringHitCastSpellTag.ImgOff = "mtgl_edit_btn";
            btnItOnScoringHitCastSpellTag.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItOnScoringHitCastSpellTag.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItOnScoringHitCastSpellTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItOnScoringHitCastSpellTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItOnUseItemCastSpellTag == null)
            {
                btnItOnUseItemCastSpellTag = new IbbToggle(gv);
            }
            btnItOnUseItemCastSpellTag.ImgOn = "mtgl_edit_btn";
            btnItOnUseItemCastSpellTag.ImgOff = "mtgl_edit_btn";
            btnItOnUseItemCastSpellTag.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItOnUseItemCastSpellTag.Y = 4 * gv.uiSquareSize;
            btnItOnUseItemCastSpellTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItOnUseItemCastSpellTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItDestroyItemAfterOnUseItemCastSpell == null)
            {
                btnItDestroyItemAfterOnUseItemCastSpell = new IbbToggle(gv);
            }
            btnItDestroyItemAfterOnUseItemCastSpell.ImgOn = "mtgl_rbtn_on";
            btnItDestroyItemAfterOnUseItemCastSpell.ImgOff = "mtgl_rbtn_off";
            btnItDestroyItemAfterOnUseItemCastSpell.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItDestroyItemAfterOnUseItemCastSpell.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItDestroyItemAfterOnUseItemCastSpell.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItDestroyItemAfterOnUseItemCastSpell.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItLevelOfItemForCastSpell == null)
            {
                btnItLevelOfItemForCastSpell = new IbbToggle(gv);
            }
            btnItLevelOfItemForCastSpell.ImgOn = "mtgl_edit_btn";
            btnItLevelOfItemForCastSpell.ImgOff = "mtgl_edit_btn";
            btnItLevelOfItemForCastSpell.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItLevelOfItemForCastSpell.Y = 5 * gv.uiSquareSize;
            btnItLevelOfItemForCastSpell.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItLevelOfItemForCastSpell.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnItUsePlayerClassLevelForOnUseItemCastSpell == null)
            {
                btnItUsePlayerClassLevelForOnUseItemCastSpell = new IbbToggle(gv);
            }
            btnItUsePlayerClassLevelForOnUseItemCastSpell.ImgOn = "mtgl_rbtn_on";
            btnItUsePlayerClassLevelForOnUseItemCastSpell.ImgOff = "mtgl_rbtn_off";
            btnItUsePlayerClassLevelForOnUseItemCastSpell.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItUsePlayerClassLevelForOnUseItemCastSpell.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnItUsePlayerClassLevelForOnUseItemCastSpell.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnItUsePlayerClassLevelForOnUseItemCastSpell.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

        }
        public void setClassControlsStart()
        {
            if (btnAddClass == null)
            {
                btnAddClass = new IbbButton(gv, 0.8f);
            }
            //btnAddClass.Text = "ADD";
            btnAddClass.Img = "btn_small";
            btnAddClass.Img2 = "btnadd";
            btnAddClass.Glow = "btn_small_glow";
            btnAddClass.X = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 1 * gv.fontWidth;
            btnAddClass.Y = 2 * gv.uiSquareSize + 4 * gv.fontHeight;
            btnAddClass.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddClass.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveClass == null)
            {
                btnRemoveClass = new IbbButton(gv, 0.8f);
            }
            //btnRemoveClass.Text = "REMOVE";
            btnRemoveClass.Img = "btn_small";
            btnRemoveClass.Img2 = "btnremove";
            btnRemoveClass.Glow = "btn_small_glow";
            btnRemoveClass.X = 6 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 1 * gv.fontWidth;
            btnRemoveClass.Y = 2 * gv.uiSquareSize + 4 * gv.fontHeight;
            btnRemoveClass.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveClass.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsItemEditor()
        {
            sortItemList();
            setControlsStart();
            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            int shiftForFont = (tglMain.Height / 2) - (gv.fontHeight / 2);
            //Page Title
            gv.DrawText("ITEM EDITOR", center - (5 * (gv.fontWidth + gv.fontCharSpacing)), 2 * gv.scaler, "yl");

            //label      
            gv.DrawText("ITEMS", btnAddItem.X, btnAddItem.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            btnAddItem.Draw();
            btnRemoveItem.Draw();
            btnCopyItem.Draw();

            string lastCategory = "";
            numberOfLinesToShow = 23;
            int cnt = 0;
            int crtCnt = 0;
            int startY = btnAddItem.Y + btnAddItem.Height;
            foreach (Item it in gv.cc.allItemsList)
            {
                int tlX = btnAddItem.X;
                int tlY = startY + (gv.fontHeight + gv.fontLineSpacing) * cnt;
                //check if change in lastCategory and if so print category and then go to next line and print crt tag
                if (!it.ItemCategoryName.Equals(lastCategory))
                {
                    //new category
                    lastCategory = it.ItemCategoryName;
                    src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("mtgl_expand_off").Width, gv.cc.GetFromTileBitmapList("mtgl_expand_off").Height);
                    dst = new IbRect(tlX, tlY, gv.fontHeight, gv.fontHeight);
                    if ((categoryList.ContainsKey(it.ItemCategoryName)) && (categoryList[it.ItemCategoryName]))
                    {
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList("mtgl_expand_off"), src, dst);
                    }
                    else
                    {
                        gv.DrawBitmap(gv.cc.GetFromTileBitmapList("mtgl_expand_on"), src, dst);
                    }
                    gv.DrawText("  " + it.ItemCategoryName, tlX, tlY, "bu");
                    cnt++;
                }
                if ((categoryList.ContainsKey(it.ItemCategoryName)) && (categoryList[it.ItemCategoryName]))
                {
                    tlY = startY + (gv.fontHeight + gv.fontLineSpacing) * cnt;
                    if (crtCnt == itemListIndex)
                    {
                        gv.DrawText("   " + it.name, tlX, tlY, "gn");
                    }
                    else
                    {
                        if (it.moduleItem)
                        {
                            gv.DrawText("   " + it.name, tlX, tlY, "wh");
                        }
                        else
                        {
                            gv.DrawText("   " + it.name, tlX, tlY, "gy");
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
            tglMisc.Draw();
            gv.DrawText("MISC", tglMisc.X + tglMisc.Width + gv.scaler, tglMisc.Y + shiftForFont, "ma");

            tglRegen.Draw();
            gv.DrawText("REGENS/SAVES", tglRegen.X + tglRegen.Width + gv.scaler, tglRegen.Y + shiftForFont, "ma");
            tglMod.Draw();
            gv.DrawText("MODIFIERS", tglMod.X + tglMod.Width + gv.scaler, tglMod.Y + shiftForFont, "ma");
            tglResistance.Draw();
            gv.DrawText("RESISTANCES", tglResistance.X + tglResistance.Width + gv.scaler, tglResistance.Y + shiftForFont, "ma");

            tglImage.Draw();
            gv.DrawText("IMAGES", tglImage.X + tglImage.Width + gv.scaler, tglImage.Y + shiftForFont, "ma");
            tglBehavior.Draw();
            gv.DrawText("BEHAVIOR", tglBehavior.X + tglBehavior.Width + gv.scaler, tglBehavior.Y + shiftForFont, "ma");
            tglClass.Draw();
            gv.DrawText("CLASSES", tglClass.X + tglClass.Width + gv.scaler, tglClass.Y + shiftForFont, "ma");

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
            else if (currentMode.Equals("Misc"))
            {
                setMiscControlsStart();
                drawMisc();
            }
            else if (currentMode.Equals("Regen"))
            {
                setRegenControlsStart();
                drawRegen();
            }
            else if (currentMode.Equals("Mod"))
            {
                setModControlsStart();
                drawMod();
            }
            else if (currentMode.Equals("Resistance"))
            {
                setResistanceControlsStart();
                drawResistance();
            }
            else if (currentMode.Equals("Image"))
            {
                setImageControlsStart();
                drawImage();
            }
            else if (currentMode.Equals("Behavior"))
            {
                setBehaviorControlsStart();
                drawBehavior();
            }
            else if (currentMode.Equals("Class"))
            {
                setClassControlsStart();
                drawClass();
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

            int shiftForFont = (btnItName.Height / 2) - (gv.fontHeight / 2);
            btnItName.Draw();
            gv.DrawText("NAME: " + gv.cc.allItemsList[itemListIndex].name, btnItName.X + btnItName.Width + gv.scaler, btnItName.Y + shiftForFont, "wh");
            btnItResRef.Draw();
            gv.DrawText("RESREF: " + gv.cc.allItemsList[itemListIndex].resref, btnItResRef.X + btnItResRef.Width + gv.scaler, btnItResRef.Y + shiftForFont, "wh");
            btnItDesc.Draw();
            gv.DrawText("DESC: " + gv.cc.allItemsList[itemListIndex].desc, btnItDesc.X + btnItDesc.Width + gv.scaler, btnItDesc.Y + shiftForFont, "wh");
            btnItDescFull.Draw();
            gv.DrawText("DESC FULL: " + gv.cc.allItemsList[itemListIndex].descFull, btnItDescFull.X + btnItDescFull.Width + gv.scaler, btnItDescFull.Y + shiftForFont, "wh");
            btnItCategory.Draw();
            gv.DrawText("CATEGORY: " + gv.cc.allItemsList[itemListIndex].category, btnItCategory.X + btnItCategory.Width + gv.scaler, btnItCategory.Y + shiftForFont, "wh");
            btnItValue.Draw();
            gv.DrawText("VALUE: " + gv.cc.allItemsList[itemListIndex].value, btnItValue.X + btnItValue.Width + gv.scaler, btnItValue.Y + shiftForFont, "wh");
            btnItAmmoType.Draw();
            gv.DrawText("AMMO TYPE: " + gv.cc.allItemsList[itemListIndex].ammoType, btnItAmmoType.X + btnItAmmoType.Width + gv.scaler, btnItAmmoType.Y + shiftForFont, "wh");
            btnItParentNodeName.Draw();
            gv.DrawText("ORG CATEGORY: " + gv.cc.allItemsList[itemListIndex].ItemCategoryName, btnItParentNodeName.X + btnItParentNodeName.Width + gv.scaler, btnItParentNodeName.Y + shiftForFont, "wh");

        }
        public void drawAttack()
        {

            int shiftForFont = (btnItName.Height / 2) - (gv.fontHeight / 2);
            btnItAttackBonus.Draw();
            gv.DrawText("ATTACK BONUS: " + gv.cc.allItemsList[itemListIndex].attackBonus, btnItAttackBonus.X + btnItAttackBonus.Width + gv.scaler, btnItAttackBonus.Y + shiftForFont, "wh");
            btnItAttackRange.Draw();
            gv.DrawText("ATTACK RANGE: " + gv.cc.allItemsList[itemListIndex].attackRange, btnItAttackRange.X + btnItAttackRange.Width + gv.scaler, btnItAttackRange.Y + shiftForFont, "wh");
            btnItDamageNumDice.Draw();
            gv.DrawText("DAMAGE NUM OF DICE: " + gv.cc.allItemsList[itemListIndex].damageNumDice, btnItDamageNumDice.X + btnItDamageNumDice.Width + gv.scaler, btnItDamageNumDice.Y + shiftForFont, "wh");
            btnItDamageDie.Draw();
            gv.DrawText("DAMAGE DICE: " + gv.cc.allItemsList[itemListIndex].damageDie, btnItDamageDie.X + btnItDamageDie.Width + gv.scaler, btnItDamageDie.Y + shiftForFont, "wh");
            btnItDamageAdder.Draw();
            gv.DrawText("DAMAGE ADDER: " + gv.cc.allItemsList[itemListIndex].damageAdder, btnItDamageAdder.X + btnItDamageAdder.Width + gv.scaler, btnItDamageAdder.Y + shiftForFont, "wh");
            btnItArmorBonus.Draw();
            gv.DrawText("ARMOR BONUS: " + gv.cc.allItemsList[itemListIndex].armorBonus, btnItArmorBonus.X + btnItArmorBonus.Width + gv.scaler, btnItArmorBonus.Y + shiftForFont, "wh");
            btnItMaxDexBonus.Draw();
            gv.DrawText("MAX DEX BONUS: " + gv.cc.allItemsList[itemListIndex].maxDexBonus, btnItMaxDexBonus.X + btnItMaxDexBonus.Width + gv.scaler, btnItMaxDexBonus.Y + shiftForFont, "wh");
            btnItTypeOfDamage.Draw();
            gv.DrawText("TYPE OF DAMAGE: " + gv.cc.allItemsList[itemListIndex].typeOfDamage, btnItTypeOfDamage.X + btnItTypeOfDamage.Width + gv.scaler, btnItTypeOfDamage.Y + shiftForFont, "wh");

        }
        public void drawMisc()
        {
            int shiftForFont = (btnItName.Height / 2) - (gv.fontHeight / 2);

            if (gv.cc.allItemsList[itemListIndex].canNotBeUnequipped) { btnItCanNotBeUnequipped.toggleOn = true; }
            else { btnItCanNotBeUnequipped.toggleOn = false; }
            btnItCanNotBeUnequipped.Draw();
            gv.DrawText("CANNOT BE UNEQUIPPED", btnItCanNotBeUnequipped.X + btnItCanNotBeUnequipped.Width + gv.scaler, btnItCanNotBeUnequipped.Y + shiftForFont, "wh");

            if (gv.cc.allItemsList[itemListIndex].plotItem) { btnItPlotItem.toggleOn = true; }
            else { btnItPlotItem.toggleOn = false; }
            btnItPlotItem.Draw();
            gv.DrawText("IS PLOT ITEM", btnItPlotItem.X + btnItPlotItem.Width + gv.scaler, btnItPlotItem.Y + shiftForFont, "wh");

            if (gv.cc.allItemsList[itemListIndex].isRation) { btnItIsRation.toggleOn = true; }
            else { btnItIsRation.toggleOn = false; }
            btnItIsRation.Draw();
            gv.DrawText("IS RATION", btnItIsRation.X + btnItIsRation.Width + gv.scaler, btnItIsRation.Y + shiftForFont, "wh");

            if (gv.cc.allItemsList[itemListIndex].twoHanded) { btnItTwoHanded.toggleOn = true; }
            else { btnItTwoHanded.toggleOn = false; }
            btnItTwoHanded.Draw();
            gv.DrawText("IS TWO HANDED", btnItTwoHanded.X + btnItTwoHanded.Width + gv.scaler, btnItTwoHanded.Y + shiftForFont, "wh");

            btnItQuantity.Draw();
            gv.DrawText("QUANTITY: " + gv.cc.allItemsList[itemListIndex].quantity, btnItQuantity.X + btnItQuantity.Width + gv.scaler, btnItQuantity.Y + shiftForFont, "wh");
            btnItGroupSizeForSellingStackableItems.Draw();
            gv.DrawText("GROUP SIZE FOR SELLING STACKABLES: " + gv.cc.allItemsList[itemListIndex].groupSizeForSellingStackableItems, btnItGroupSizeForSellingStackableItems.X + btnItGroupSizeForSellingStackableItems.Width + gv.scaler, btnItGroupSizeForSellingStackableItems.Y + shiftForFont, "wh");
            btnItCharges.Draw();
            gv.DrawText("CHARGES: " + gv.cc.allItemsList[itemListIndex].charges, btnItCharges.X + btnItCharges.Width + gv.scaler, btnItCharges.Y + shiftForFont, "wh");

            if (gv.cc.allItemsList[itemListIndex].isStackable) { btnItIsStackable.toggleOn = true; }
            else { btnItIsStackable.toggleOn = false; }
            btnItIsStackable.Draw();
            gv.DrawText("IS STACKABLE", btnItIsStackable.X + btnItIsStackable.Width + gv.scaler, btnItIsStackable.Y + shiftForFont, "wh");

        }

        public void drawRegen()
        {
            int shiftForFont = (btnItName.Height / 2) - (gv.fontHeight / 2);

            btnItSpRegenPerRoundInCombat.Draw();
            gv.DrawText("SP REGEN IN COMBAT: " + gv.cc.allItemsList[itemListIndex].spRegenPerRoundInCombat, btnItSpRegenPerRoundInCombat.X + btnItSpRegenPerRoundInCombat.Width + gv.scaler, btnItSpRegenPerRoundInCombat.Y + shiftForFont, "wh");
            btnItHpRegenPerRoundInCombat.Draw();
            gv.DrawText("HP REGEN IN COMBAT: " + gv.cc.allItemsList[itemListIndex].hpRegenPerRoundInCombat, btnItHpRegenPerRoundInCombat.X + btnItHpRegenPerRoundInCombat.Width + gv.scaler, btnItHpRegenPerRoundInCombat.Y + shiftForFont, "wh");
            btnItRoundsPerSpRegenOutsideCombat.Draw();
            gv.DrawText("SP REGEN OUT OF COMBAT: " + gv.cc.allItemsList[itemListIndex].roundsPerSpRegenOutsideCombat, btnItRoundsPerSpRegenOutsideCombat.X + btnItRoundsPerSpRegenOutsideCombat.Width + gv.scaler, btnItRoundsPerSpRegenOutsideCombat.Y + shiftForFont, "wh");
            btnItRoundsPerHpRegenOutsideCombat.Draw();
            gv.DrawText("HP REGEN OUT OF COMBAT: " + gv.cc.allItemsList[itemListIndex].roundsPerHpRegenOutsideCombat, btnItRoundsPerHpRegenOutsideCombat.X + btnItRoundsPerHpRegenOutsideCombat.Width + gv.scaler, btnItRoundsPerHpRegenOutsideCombat.Y + shiftForFont, "wh");
            btnItSavingThrowModifierReflex.Draw();
            gv.DrawText("REFLEX: " + gv.cc.allItemsList[itemListIndex].savingThrowModifierReflex, btnItSavingThrowModifierReflex.X + btnItSavingThrowModifierReflex.Width + gv.scaler, btnItSavingThrowModifierReflex.Y + shiftForFont, "wh");
            btnItSavingThrowModifierFortitude.Draw();
            gv.DrawText("FORTITUDE: " + gv.cc.allItemsList[itemListIndex].savingThrowModifierFortitude, btnItSavingThrowModifierFortitude.X + btnItSavingThrowModifierFortitude.Width + gv.scaler, btnItSavingThrowModifierFortitude.Y + shiftForFont, "wh");
            btnItSavingThrowModifierWill.Draw();
            gv.DrawText("WILL: " + gv.cc.allItemsList[itemListIndex].savingThrowModifierWill, btnItSavingThrowModifierWill.X + btnItSavingThrowModifierWill.Width + gv.scaler, btnItSavingThrowModifierWill.Y + shiftForFont, "wh");
            btnItUseableInSituation.Draw();
            gv.DrawText("USEABLE IN SITUATION: " + gv.cc.allItemsList[itemListIndex].useableInSituation, btnItUseableInSituation.X + btnItUseableInSituation.Width + gv.scaler, btnItUseableInSituation.Y + shiftForFont, "wh");

        }
        public void drawMod()
        {
            int shiftForFont = (btnItName.Height / 2) - (gv.fontHeight / 2);

            btnItAttributeBonusModifierStr.Draw();
            gv.DrawText("STR MODIFIER: " + gv.cc.allItemsList[itemListIndex].attributeBonusModifierStr, btnItAttributeBonusModifierStr.X + btnItAttributeBonusModifierStr.Width + gv.scaler, btnItAttributeBonusModifierStr.Y + shiftForFont, "wh");
            btnItAttributeBonusModifierDex.Draw();
            gv.DrawText("DEX MODIFIER: " + gv.cc.allItemsList[itemListIndex].attributeBonusModifierDex, btnItAttributeBonusModifierDex.X + btnItAttributeBonusModifierDex.Width + gv.scaler, btnItAttributeBonusModifierDex.Y + shiftForFont, "wh");
            btnItAttributeBonusModifierInt.Draw();
            gv.DrawText("INT MODIFIER: " + gv.cc.allItemsList[itemListIndex].attributeBonusModifierInt, btnItAttributeBonusModifierInt.X + btnItAttributeBonusModifierInt.Width + gv.scaler, btnItAttributeBonusModifierInt.Y + shiftForFont, "wh");
            btnItAttributeBonusModifierCha.Draw();
            gv.DrawText("CHA MODIFIER: " + gv.cc.allItemsList[itemListIndex].attributeBonusModifierCha, btnItAttributeBonusModifierCha.X + btnItAttributeBonusModifierCha.Width + gv.scaler, btnItAttributeBonusModifierCha.Y + shiftForFont, "wh");
            btnItAttributeBonusModifierCon.Draw();
            gv.DrawText("CON MODIFIER: " + gv.cc.allItemsList[itemListIndex].attributeBonusModifierCon, btnItAttributeBonusModifierCon.X + btnItAttributeBonusModifierCon.Width + gv.scaler, btnItAttributeBonusModifierCon.Y + shiftForFont, "wh");
            btnItAttributeBonusModifierWis.Draw();
            gv.DrawText("WIS MODIFIER: " + gv.cc.allItemsList[itemListIndex].attributeBonusModifierWis, btnItAttributeBonusModifierWis.X + btnItAttributeBonusModifierWis.Width + gv.scaler, btnItAttributeBonusModifierWis.Y + shiftForFont, "wh");
            btnItMovementPointModifier.Draw();
            gv.DrawText("MOVEMENT MODIFIER: " + gv.cc.allItemsList[itemListIndex].MovementPointModifier, btnItMovementPointModifier.X + btnItMovementPointModifier.Width + gv.scaler, btnItMovementPointModifier.Y + shiftForFont, "wh");
            btnItArmorWeightType.Draw();
            gv.DrawText("ARMOR WEIGHT TYPE: " + gv.cc.allItemsList[itemListIndex].ArmorWeightType, btnItArmorWeightType.X + btnItArmorWeightType.Width + gv.scaler, btnItArmorWeightType.Y + shiftForFont, "wh");

        }
        public void drawResistance()
        {
            int shiftForFont = (btnItName.Height / 2) - (gv.fontHeight / 2);

            btnItResistAcid.Draw();
            gv.DrawText("RESIST ACID MODIFIER: " + gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueAcid, btnItResistAcid.X + btnItResistAcid.Width + gv.scaler, btnItResistAcid.Y + shiftForFont, "wh");
            btnItResistNormal.Draw();
            gv.DrawText("RESIST NORMAL MODIFIER: " + gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueNormal, btnItResistNormal.X + btnItResistNormal.Width + gv.scaler, btnItResistNormal.Y + shiftForFont, "wh");
            btnItResistCold.Draw();
            gv.DrawText("RESIST COLD MODIFIER: " + gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueCold, btnItResistCold.X + btnItResistCold.Width + gv.scaler, btnItResistCold.Y + shiftForFont, "wh");
            btnItResistElectricity.Draw();
            gv.DrawText("RESIST ELECTRICITY MODIFIER: " + gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueElectricity, btnItResistElectricity.X + btnItResistElectricity.Width + gv.scaler, btnItResistElectricity.Y + shiftForFont, "wh");
            btnItResistFire.Draw();
            gv.DrawText("RESIST FIRE MODIFIER: " + gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueFire, btnItResistFire.X + btnItResistFire.Width + gv.scaler, btnItResistFire.Y + shiftForFont, "wh");
            btnItResistMagic.Draw();
            gv.DrawText("RESIST MAGIC MODIFIER: " + gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueMagic, btnItResistMagic.X + btnItResistMagic.Width + gv.scaler, btnItResistMagic.Y + shiftForFont, "wh");
            btnItResistPoison.Draw();
            gv.DrawText("RESIST POISON MODIFIER: " + gv.cc.allItemsList[itemListIndex].damageTypeResistanceValuePoison, btnItResistPoison.X + btnItResistPoison.Width + gv.scaler, btnItResistPoison.Y + shiftForFont, "wh");

        }

        public void drawImage()
        {

            int shiftForFont = (btnItName.Height / 2) - (gv.fontHeight / 2);
            btnItImage.Draw();
            string token = gv.cc.allItemsList[itemListIndex].itemImage;
            int brX = (int)(gv.squareSize * gv.scaler);
            int brY = (int)(gv.squareSize * gv.scaler);
            gv.DrawText("IMAGE FILENAME: " + token, btnItImage.X + btnItImage.Width + gv.scaler, btnItImage.Y + shiftForFont, "wh");
            //only frame
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(token).Width, gv.cc.GetFromTileBitmapList(token).Height);
            dst = new IbRect(btnItImage.X, btnItImage.Y + (gv.uiSquareSize / 2), brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(token), src, dst);

            btnItSpriteProjectileFilename.Draw();
            gv.DrawText("PROJECTILE IMAGE: " + gv.cc.allItemsList[itemListIndex].projectileSpriteFilename, btnItSpriteProjectileFilename.X + btnItSpriteProjectileFilename.Width + gv.scaler, btnItSpriteProjectileFilename.Y + shiftForFont, "wh");
            token = gv.cc.allItemsList[itemListIndex].projectileSpriteFilename;
            brX = (int)(gv.squareSize * gv.scaler * 4);
            brY = (int)(gv.squareSize * gv.scaler);
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(token).Width, gv.cc.GetFromTileBitmapList(token).Height);
            dst = new IbRect(btnItSpriteProjectileFilename.X, btnItSpriteProjectileFilename.Y + (gv.uiSquareSize / 2), brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(token), src, dst);

            btnItSpriteEndingFilename.Draw();
            gv.DrawText("PROJECTILE ENDING IMAGE: " + gv.cc.allItemsList[itemListIndex].spriteEndingFilename, btnItSpriteEndingFilename.X + btnItSpriteEndingFilename.Width + gv.scaler, btnItSpriteEndingFilename.Y + shiftForFont, "wh");
            token = gv.cc.allItemsList[itemListIndex].spriteEndingFilename;
            brX = (int)(gv.squareSize * gv.scaler * 4);
            brY = (int)(gv.squareSize * gv.scaler);
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(token).Width, gv.cc.GetFromTileBitmapList(token).Height);
            dst = new IbRect(btnItSpriteEndingFilename.X, btnItSpriteEndingFilename.Y + (gv.uiSquareSize / 2), brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(token), src, dst);

            btnItOnUseSound.Draw();
            gv.DrawText("ON USE SOUND: " + gv.cc.allItemsList[itemListIndex].itemOnUseSound, btnItOnUseSound.X + btnItOnUseSound.Width + gv.scaler, btnItOnUseSound.Y + shiftForFont, "wh");

            btnItEndSound.Draw();
            gv.DrawText("ON USE ENDING SOUND: " + gv.cc.allItemsList[itemListIndex].itemEndSound, btnItEndSound.X + btnItEndSound.Width + gv.scaler, btnItEndSound.Y + shiftForFont, "wh");

        }
        public void drawBehavior()
        {
            int shiftForFont = (btnItName.Height / 2) - (gv.fontHeight / 2);

            if (gv.cc.allItemsList[itemListIndex].automaticallyHitsTarget) { btnItAutomaticallyHitsTarget.toggleOn = true; }
            else { btnItAutomaticallyHitsTarget.toggleOn = false; }
            btnItAutomaticallyHitsTarget.Draw();
            gv.DrawText("AUTOMATICALLY HITS TARGET", btnItAutomaticallyHitsTarget.X + btnItAutomaticallyHitsTarget.Width + gv.scaler, btnItAutomaticallyHitsTarget.Y + shiftForFont, "wh");

            btnItAreaOfEffect.Draw();
            gv.DrawText("AOE RADIUS: " + gv.cc.allItemsList[itemListIndex].AreaOfEffect, btnItAreaOfEffect.X + btnItAreaOfEffect.Width + gv.scaler, btnItAreaOfEffect.Y + shiftForFont, "wh");

            btnItAoeShape.Draw();
            gv.DrawText("AOE SHAPE: " + gv.cc.allItemsList[itemListIndex].aoeShape, btnItAoeShape.X + btnItAoeShape.Width + gv.scaler, btnItAoeShape.Y + shiftForFont, "wh");

            btnItOnScoringHitCastSpellTag.Draw();
            gv.DrawText("ON SCORING HIT SPELL: " + gv.cc.allItemsList[itemListIndex].onScoringHitCastSpellTag, btnItOnScoringHitCastSpellTag.X + btnItOnScoringHitCastSpellTag.Width + gv.scaler, btnItOnScoringHitCastSpellTag.Y + shiftForFont, "wh");

            btnItOnUseItemCastSpellTag.Draw();
            gv.DrawText("ON USE ITEM SPELL: " + gv.cc.allItemsList[itemListIndex].onUseItemCastSpellTag, btnItOnUseItemCastSpellTag.X + btnItOnUseItemCastSpellTag.Width + gv.scaler, btnItOnUseItemCastSpellTag.Y + shiftForFont, "wh");

            if (gv.cc.allItemsList[itemListIndex].destroyItemAfterOnUseItemCastSpell) { btnItDestroyItemAfterOnUseItemCastSpell.toggleOn = true; }
            else { btnItDestroyItemAfterOnUseItemCastSpell.toggleOn = false; }
            btnItDestroyItemAfterOnUseItemCastSpell.Draw();
            gv.DrawText("DESTROY ITEM AFTER ON USE ITEM SPELL", btnItDestroyItemAfterOnUseItemCastSpell.X + btnItDestroyItemAfterOnUseItemCastSpell.Width + gv.scaler, btnItDestroyItemAfterOnUseItemCastSpell.Y + shiftForFont, "wh");

            btnItLevelOfItemForCastSpell.Draw();
            gv.DrawText("LEVEL OF ITEM FOR CAST SPELL: " + gv.cc.allItemsList[itemListIndex].levelOfItemForCastSpell, btnItLevelOfItemForCastSpell.X + btnItLevelOfItemForCastSpell.Width + gv.scaler, btnItLevelOfItemForCastSpell.Y + shiftForFont, "wh");

            if (gv.cc.allItemsList[itemListIndex].usePlayerClassLevelForOnUseItemCastSpell) { btnItUsePlayerClassLevelForOnUseItemCastSpell.toggleOn = true; }
            else { btnItUsePlayerClassLevelForOnUseItemCastSpell.toggleOn = false; }
            btnItUsePlayerClassLevelForOnUseItemCastSpell.Draw();
            gv.DrawText("USE PC LEVEL FOR ON USE ITEM SPELL", btnItUsePlayerClassLevelForOnUseItemCastSpell.X + btnItUsePlayerClassLevelForOnUseItemCastSpell.Width + gv.scaler, btnItUsePlayerClassLevelForOnUseItemCastSpell.Y + shiftForFont, "wh");

        }
        public void drawClass()
        {
            int shiftForFont = (btnItName.Height / 2) - (gv.fontHeight / 2);
            //label      
            gv.DrawText("CLASSES THAT CAN USE ITEM:", btnAddClass.X, btnAddClass.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            btnAddClass.Draw();
            btnRemoveClass.Draw();
            //list all containers (tap on a container in the list to show elements for editing)
            int startX = btnAddClass.X;
            int startY = btnAddClass.Y + btnAddClass.Height - gv.fontHeight;
            int incY = gv.fontWidth + gv.fontLineSpacing;
            int cnt = 0;
            foreach (string c in gv.cc.allItemsList[itemListIndex].classesAllowed)
            {
                if (cnt == classListIndex)
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

        public void onTouchTsItemEditor(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnHelp.glowOn = false;

            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }

            bool ret = gv.tsMainMenu.onTouchTsMainMenu(eX, eY, eventType);
            if (ret) { return; } //did some action on the main menu so do nothing here

            //TODO only allow editing of module creatures
            if ((itemListIndex < gv.cc.allItemsList.Count) && (gv.cc.allItemsList[itemListIndex].moduleItem))
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
                else if (currentMode.Equals("Misc"))
                {
                    ret = onTouchMisc(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Settings panel so do nothing here
                }
                else if (currentMode.Equals("Regen"))
                {
                    ret = onTouchRegen(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Settings panel so do nothing here
                }
                else if (currentMode.Equals("Mod"))
                {
                    ret = onTouchMod(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Settings panel so do nothing here
                }
                else if (currentMode.Equals("Resistance"))
                {
                    ret = onTouchResistance(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Walk-LoS panel so do nothing here
                }
                else if (currentMode.Equals("Image"))
                {
                    ret = onTouchImage(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Info panel so do nothing here
                }
                else if (currentMode.Equals("Behavior"))
                {
                    ret = onTouchBehavior(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Walk-LoS panel so do nothing here
                }
                else if (currentMode.Equals("Class"))
                {
                    ret = onTouchClass(eX, eY, eventType);
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
                    if ((x > 0) && (x < tglMain.X) && (y > btnAddItem.Y + btnAddItem.Height))
                    {
                        //figure out which line was tapped and if that is a category do expand/collapse
                        int PanelLeftLocation = btnAddItem.X;
                        int PanelTopLocation = btnAddItem.Y + btnAddItem.Height;
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
                                itemListIndex = indexList[lineIndex];
                            }
                        }
                    }

                    if (tglMain.getImpact(x, y))
                    {
                        tglMain.toggleOn = true;
                        tglAttack.toggleOn = false;
                        tglMisc.toggleOn = false;
                        tglRegen.toggleOn = false;
                        tglMod.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglImage.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglClass.toggleOn = false;
                        currentMode = "Main";
                    }
                    else if (tglAttack.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = true;
                        tglMisc.toggleOn = false;
                        tglRegen.toggleOn = false;
                        tglMod.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglImage.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglClass.toggleOn = false;
                        currentMode = "Attack";
                    }
                    else if (tglMisc.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglMisc.toggleOn = true;
                        tglRegen.toggleOn = false;
                        tglMod.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglImage.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglClass.toggleOn = false;
                        currentMode = "Misc";
                    }
                    else if (tglRegen.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglMisc.toggleOn = false;
                        tglRegen.toggleOn = true;
                        tglMod.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglImage.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglClass.toggleOn = false;
                        currentMode = "Regen";
                    }
                    else if (tglMod.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglMisc.toggleOn = false;
                        tglRegen.toggleOn = false;
                        tglMod.toggleOn = true;
                        tglResistance.toggleOn = false;
                        tglImage.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglClass.toggleOn = false;
                        currentMode = "Mod";
                    }
                    else if (tglResistance.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglMisc.toggleOn = false;
                        tglRegen.toggleOn = false;
                        tglMod.toggleOn = false;
                        tglResistance.toggleOn = true;
                        tglImage.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglClass.toggleOn = false;
                        currentMode = "Resistance";
                    }
                    else if (tglImage.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglMisc.toggleOn = false;
                        tglRegen.toggleOn = false;
                        tglMod.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglImage.toggleOn = true;
                        tglBehavior.toggleOn = false;
                        tglClass.toggleOn = false;
                        currentMode = "Image";
                    }
                    else if (tglBehavior.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglMisc.toggleOn = false;
                        tglRegen.toggleOn = false;
                        tglMod.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglImage.toggleOn = false;
                        tglBehavior.toggleOn = true;
                        tglClass.toggleOn = false;
                        currentMode = "Behavior";
                    }
                    else if (tglClass.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttack.toggleOn = false;
                        tglMisc.toggleOn = false;
                        tglRegen.toggleOn = false;
                        tglMod.toggleOn = false;
                        tglResistance.toggleOn = false;
                        tglImage.toggleOn = false;
                        tglBehavior.toggleOn = false;
                        tglClass.toggleOn = true;
                        currentMode = "Class";
                    }
                    else if (btnAddItem.getImpact(x, y))
                    {
                        addItem();
                    }
                    else if (btnRemoveItem.getImpact(x, y))
                    {
                        removeItem();
                    }
                    else if (btnCopyItem.getImpact(x, y))
                    {
                        copyItem();
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

                    if (btnItName.getImpact(x, y))
                    {
                        changeItName();
                    }
                    else if (btnItResRef.getImpact(x, y))
                    {
                        changeItResRef();
                    }
                    else if (btnItDesc.getImpact(x, y))
                    {
                        changeItDesc();
                    }
                    else if (btnItDescFull.getImpact(x, y))
                    {
                        changeItDescFull();
                    }
                    else if (btnItCategory.getImpact(x, y))
                    {
                        changeItCategory();
                    }
                    else if (btnItValue.getImpact(x, y))
                    {
                        changeItValue();
                    }
                    else if (btnItAmmoType.getImpact(x, y))
                    {
                        changeItAmmoType();
                    }
                    else if (btnItParentNodeName.getImpact(x, y))
                    {
                        changeItParentNodeName();
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

                    if (btnItAttackBonus.getImpact(x, y))
                    {
                        changeItAttackBonus();
                    }
                    else if (btnItAttackRange.getImpact(x, y))
                    {
                        changeItAttackRange();
                    }
                    else if (btnItDamageNumDice.getImpact(x, y))
                    {
                        changeItDamageNumDice();
                    }
                    else if (btnItDamageDie.getImpact(x, y))
                    {
                        changeItDamageDie();
                    }
                    else if (btnItDamageAdder.getImpact(x, y))
                    {
                        changeItDamageAdder();
                    }
                    else if (btnItArmorBonus.getImpact(x, y))
                    {
                        changeItArmorBonus();
                    }
                    else if (btnItMaxDexBonus.getImpact(x, y))
                    {
                        changeItMaxDexBonus();
                    }
                    else if (btnItTypeOfDamage.getImpact(x, y))
                    {
                        changeItTypeOfDamage();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchMisc(int eX, int eY, MouseEventType.EventType eventType)
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

                    if (btnItCanNotBeUnequipped.getImpact(x, y))
                    {
                        changeItCanNotBeUnequipped();
                    }
                    else if (btnItPlotItem.getImpact(x, y))
                    {
                        changeItPlotItem();
                    }
                    else if (btnItIsRation.getImpact(x, y))
                    {
                        changeItIsRation();
                    }
                    else if (btnItTwoHanded.getImpact(x, y))
                    {
                        changeItTwoHanded();
                    }
                    else if (btnItQuantity.getImpact(x, y))
                    {
                        changeItQuantity();
                    }
                    else if (btnItGroupSizeForSellingStackableItems.getImpact(x, y))
                    {
                        changeItGroupSizeForSellingStackableItems();
                    }
                    else if (btnItCharges.getImpact(x, y))
                    {
                        changeItCharges();
                    }
                    else if (btnItIsStackable.getImpact(x, y))
                    {
                        changeItIsStackable();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchRegen(int eX, int eY, MouseEventType.EventType eventType)
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

                    if (btnItSpRegenPerRoundInCombat.getImpact(x, y))
                    {
                        changeItSpRegenPerRoundInCombat();
                    }
                    else if (btnItHpRegenPerRoundInCombat.getImpact(x, y))
                    {
                        changeItHpRegenPerRoundInCombat();
                    }
                    else if (btnItRoundsPerSpRegenOutsideCombat.getImpact(x, y))
                    {
                        changeItRoundsPerSpRegenOutsideCombat();
                    }
                    else if (btnItRoundsPerHpRegenOutsideCombat.getImpact(x, y))
                    {
                        changeItRoundsPerHpRegenOutsideCombat();
                    }
                    else if (btnItSavingThrowModifierReflex.getImpact(x, y))
                    {
                        changeItSavingThrowModifierReflex();
                    }
                    else if (btnItSavingThrowModifierFortitude.getImpact(x, y))
                    {
                        changeItSavingThrowModifierFortitude();
                    }
                    else if (btnItSavingThrowModifierWill.getImpact(x, y))
                    {
                        changeItSavingThrowModifierWill();
                    }
                    else if (btnItUseableInSituation.getImpact(x, y))
                    {
                        changeItUseableInSituation();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchMod(int eX, int eY, MouseEventType.EventType eventType)
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

                    if (btnItAttributeBonusModifierStr.getImpact(x, y))
                    {
                        changeItAttributeBonusModifierStr();
                    }
                    else if (btnItAttributeBonusModifierDex.getImpact(x, y))
                    {
                        changeItAttributeBonusModifierDex();
                    }
                    else if (btnItAttributeBonusModifierInt.getImpact(x, y))
                    {
                        changeItAttributeBonusModifierInt();
                    }
                    else if (btnItAttributeBonusModifierCha.getImpact(x, y))
                    {
                        changeItAttributeBonusModifierCha();
                    }
                    else if (btnItAttributeBonusModifierCon.getImpact(x, y))
                    {
                        changeItAttributeBonusModifierCon();
                    }
                    else if (btnItAttributeBonusModifierWis.getImpact(x, y))
                    {
                        changeItAttributeBonusModifierWis();
                    }
                    else if (btnItMovementPointModifier.getImpact(x, y))
                    {
                        changeItMovementPointModifier();
                    }
                    else if (btnItArmorWeightType.getImpact(x, y))
                    {
                        changeItArmorWeightType();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchResistance(int eX, int eY, MouseEventType.EventType eventType)
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

                    if (btnItResistAcid.getImpact(x, y))
                    {
                        changeItResistAcid();
                    }
                    else if (btnItResistNormal.getImpact(x, y))
                    {
                        changeItResistNormal();
                    }
                    else if (btnItResistCold.getImpact(x, y))
                    {
                        changeItResistCold();
                    }
                    else if (btnItResistElectricity.getImpact(x, y))
                    {
                        changeItResistElectricity();
                    }
                    else if (btnItResistFire.getImpact(x, y))
                    {
                        changeItResistFire();
                    }
                    else if (btnItResistMagic.getImpact(x, y))
                    {
                        changeItResistMagic();
                    }
                    else if (btnItResistPoison.getImpact(x, y))
                    {
                        changeItResistPoison();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchImage(int eX, int eY, MouseEventType.EventType eventType)
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

                    if (btnItOnUseSound.getImpact(x, y))
                    {
                        changeItOnUseSound();
                    }
                    else if (btnItEndSound.getImpact(x, y))
                    {
                        changeItEndSound();
                    }
                    else if (btnItImage.getImpact(x, y))
                    {
                        changeItImage();
                    }
                    else if (btnItSpriteProjectileFilename.getImpact(x, y))
                    {
                        changeItSpriteProjectileFilename();
                    }
                    else if (btnItSpriteEndingFilename.getImpact(x, y))
                    {
                        changeItSpriteEndingFilename();
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

                    if (btnItAutomaticallyHitsTarget.getImpact(x, y))
                    {
                        changeItAutomaticallyHitsTarget();
                    }
                    else if (btnItAreaOfEffect.getImpact(x, y))
                    {
                        changeItAreaOfEffect();
                    }
                    else if (btnItAoeShape.getImpact(x, y))
                    {
                        changeItAoeShape();
                    }
                    else if (btnItOnScoringHitCastSpellTag.getImpact(x, y))
                    {
                        changeItOnScoringHitCastSpellTag();
                    }
                    else if (btnItOnUseItemCastSpellTag.getImpact(x, y))
                    {
                        changeItOnUseItemCastSpellTag();
                    }
                    else if (btnItDestroyItemAfterOnUseItemCastSpell.getImpact(x, y))
                    {
                        changeItDestroyItemAfterOnUseItemCastSpell();
                    }
                    else if (btnItLevelOfItemForCastSpell.getImpact(x, y))
                    {
                        changeItLevelOfItemForCastSpell();
                    }
                    else if (btnItUsePlayerClassLevelForOnUseItemCastSpell.getImpact(x, y))
                    {
                        changeItUsePlayerClassLevelForOnUseItemCastSpell();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchClass(int eX, int eY, MouseEventType.EventType eventType)
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
                    if ((x > btnAddClass.X) && (y > btnAddClass.Y + btnAddClass.Height))
                    {
                        //figure out which line was tapped and if that is a category do expand/collapse
                        int PanelLeftLocation = btnAddClass.X;
                        int PanelTopLocation = btnAddClass.Y + btnAddClass.Height;
                        int lineIndex = (y - PanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing);
                        if ((lineIndex < gv.cc.allItemsList[itemListIndex].classesAllowed.Count) && (gv.cc.allItemsList[itemListIndex].classesAllowed.Count > 0))
                        {
                            classListIndex = lineIndex;
                        }
                    }

                    if (btnAddClass.getImpact(x, y))
                    {
                        addClass();
                    }
                    else if (btnRemoveClass.getImpact(x, y))
                    {
                        removeClass();
                    }
                    break;
            }
            return false;
        }

        //CREATURE PANEL
        public void addItem()
        {
            if (itemListIndex < gv.cc.allItemsList.Count)
            {
                Item newItem = new Item();
                newItem.ItemCategoryName = gv.cc.allItemsList[itemListIndex].ItemCategoryName;
                int nextId = gv.mod.getNextIdNumber();
                newItem.tag = "newTag_" + nextId;
                newItem.resref = "newResRef_" + nextId;
                newItem.moduleItem = true;
                gv.cc.allItemsList.Add(newItem);
                sortItemList();
                resetIndexList();
            }
        }
        public void removeItem()
        {
            if (itemListIndex < gv.cc.allItemsList.Count)
            {
                //TODO
                if (gv.cc.allItemsList.Count > 0)
                {
                    try
                    {
                        gv.cc.allItemsList.RemoveAt(itemListIndex);
                        itemListIndex = 0;
                    }
                    catch { }
                }
            }
        }
        public void copyItem()
        {
            if (itemListIndex < gv.cc.allItemsList.Count)
            {
                Item newItem = gv.cc.allItemsList[itemListIndex].DeepCopy();
                newItem.ItemCategoryName = gv.cc.allItemsList[itemListIndex].ItemCategoryName;
                int nextId = gv.mod.getNextIdNumber();
                newItem.tag = "newTag_" + nextId;
                newItem.resref = "newResRef_" + nextId;
                newItem.moduleItem = true;
                gv.cc.allItemsList.Add(newItem);
                sortItemList();
                resetIndexList();
            }
        }

        //MAIN
        public async void changeItName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Name for this item", gv.cc.allItemsList[itemListIndex].name);
            gv.cc.allItemsList[itemListIndex].name = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItResRef()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Enter a unique resref for this item.", gv.cc.allItemsList[itemListIndex].resref);
            gv.cc.allItemsList[itemListIndex].resref = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItDesc()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Enter a description for this item.", gv.cc.allItemsList[itemListIndex].desc);
            gv.cc.allItemsList[itemListIndex].desc = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItDescFull()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Enter a description for this item.", gv.cc.allItemsList[itemListIndex].descFull);
            gv.cc.allItemsList[itemListIndex].descFull = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItCategory()
        {
            //weapon, armor, general, head, etc.
        }
        public async void changeItValue()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the item's default value in gold pieces", gv.cc.allItemsList[itemListIndex].value);
            gv.cc.allItemsList[itemListIndex].value = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItAmmoType()
        {
            //arrow, stone, etc.
        }
        public async void changeItParentNodeName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Enter a category name for organizing items into groups for the items panel on the left.", gv.cc.allItemsList[itemListIndex].ItemCategoryName);
            gv.cc.allItemsList[itemListIndex].ItemCategoryName = myinput;
            gv.touchEnabled = true;                        
            if (!categoryList.ContainsKey(gv.cc.allItemsList[itemListIndex].ItemCategoryName))
            {
                categoryList.Add(gv.cc.allItemsList[itemListIndex].ItemCategoryName, false);
            }
            sortItemList();
            resetIndexList();
        }

        //ATTACK
        public async void changeItAttackBonus()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the attack bonus for this item", gv.cc.allItemsList[itemListIndex].attackBonus);
            gv.cc.allItemsList[itemListIndex].attackBonus = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItAttackRange()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the attack range for this item", gv.cc.allItemsList[itemListIndex].attackRange);
            gv.cc.allItemsList[itemListIndex].attackRange = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItDamageNumDice()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter number of dice rolled for damage (ex. the 2 in 2d4+1)", gv.cc.allItemsList[itemListIndex].damageNumDice);
            gv.cc.allItemsList[itemListIndex].damageNumDice = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItDamageDie()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter dice type used in damage roll (ex. the 4 in 2d4+1)", gv.cc.allItemsList[itemListIndex].damageDie);
            gv.cc.allItemsList[itemListIndex].damageDie = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItDamageAdder()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the damage adder (ex. the 1 in 2d4+1)", gv.cc.allItemsList[itemListIndex].damageAdder);
            gv.cc.allItemsList[itemListIndex].damageAdder = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItArmorBonus()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the armor bonus for this item", gv.cc.allItemsList[itemListIndex].armorBonus);
            gv.cc.allItemsList[itemListIndex].armorBonus = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItMaxDexBonus()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the maximum dexterity bonus for the PC when using this item", gv.cc.allItemsList[itemListIndex].maxDexBonus);
            gv.cc.allItemsList[itemListIndex].maxDexBonus = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItTypeOfDamage()
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
            string selected = await gv.ListViewPage(types, "Select the type of damage that this item does");
            if (selected != "none")
            {
                gv.cc.allItemsList[itemListIndex].typeOfDamage = selected;
            }
            gv.touchEnabled = true;            
        }

        //MISC
        public void changeItCanNotBeUnequipped()
        {
            btnItCanNotBeUnequipped.toggleOn = !btnItCanNotBeUnequipped.toggleOn;
            gv.cc.allItemsList[itemListIndex].canNotBeUnequipped = btnItCanNotBeUnequipped.toggleOn;
        }
        public void changeItPlotItem()
        {
            btnItPlotItem.toggleOn = !btnItPlotItem.toggleOn;
            gv.cc.allItemsList[itemListIndex].plotItem = btnItPlotItem.toggleOn;
        }
        public void changeItIsRation()
        {
            btnItIsRation.toggleOn = !btnItIsRation.toggleOn;
            gv.cc.allItemsList[itemListIndex].isRation = btnItIsRation.toggleOn;
        }
        public void changeItTwoHanded()
        {
            btnItTwoHanded.toggleOn = !btnItTwoHanded.toggleOn;
            gv.cc.allItemsList[itemListIndex].twoHanded = btnItTwoHanded.toggleOn;
        }
        public async void changeItQuantity()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the default quantity for the item", gv.cc.allItemsList[itemListIndex].quantity);
            gv.cc.allItemsList[itemListIndex].quantity = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItGroupSizeForSellingStackableItems()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the group size for selling stackable items", gv.cc.allItemsList[itemListIndex].groupSizeForSellingStackableItems);
            gv.cc.allItemsList[itemListIndex].groupSizeForSellingStackableItems = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItCharges()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the number of charges on the item", gv.cc.allItemsList[itemListIndex].charges);
            gv.cc.allItemsList[itemListIndex].charges = myinput;
            gv.touchEnabled = true;
        }
        public void changeItIsStackable()
        {
            btnItIsStackable.toggleOn = !btnItIsStackable.toggleOn;
            gv.cc.allItemsList[itemListIndex].isStackable = btnItIsStackable.toggleOn;
        }

        //REGEN
        public async void changeItSpRegenPerRoundInCombat()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the number of SP to increase after each round of combat", gv.cc.allItemsList[itemListIndex].spRegenPerRoundInCombat);
            gv.cc.allItemsList[itemListIndex].spRegenPerRoundInCombat = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItHpRegenPerRoundInCombat()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the number of HP to increase after each round of combat", gv.cc.allItemsList[itemListIndex].hpRegenPerRoundInCombat);
            gv.cc.allItemsList[itemListIndex].hpRegenPerRoundInCombat = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItRoundsPerSpRegenOutsideCombat()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the number of SP to increase after each move on maps outside of combat", gv.cc.allItemsList[itemListIndex].roundsPerSpRegenOutsideCombat);
            gv.cc.allItemsList[itemListIndex].roundsPerSpRegenOutsideCombat = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItRoundsPerHpRegenOutsideCombat()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the number of HP to increase after each move on maps outside of combat", gv.cc.allItemsList[itemListIndex].roundsPerHpRegenOutsideCombat);
            gv.cc.allItemsList[itemListIndex].roundsPerHpRegenOutsideCombat = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItSavingThrowModifierReflex()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the Reflex saving throw modifier when using this item", gv.cc.allItemsList[itemListIndex].savingThrowModifierReflex);
            gv.cc.allItemsList[itemListIndex].savingThrowModifierReflex = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItSavingThrowModifierFortitude()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the Fortitude saving throw modifier when using this item", gv.cc.allItemsList[itemListIndex].savingThrowModifierFortitude);
            gv.cc.allItemsList[itemListIndex].savingThrowModifierFortitude = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItSavingThrowModifierWill()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the Will saving throw modifier when using this item", gv.cc.allItemsList[itemListIndex].savingThrowModifierWill);
            gv.cc.allItemsList[itemListIndex].savingThrowModifierWill = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItUseableInSituation()
        {
            List<string> types = new List<string>();
            types.Add("Always");
            types.Add("InCombat");
            types.Add("OutOfCombat");
            types.Add("Passive");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(types, "Select the type of situation in which this item can be used");
            if (selected != "none")
            {
                gv.cc.allItemsList[itemListIndex].useableInSituation = selected;
            }
            gv.touchEnabled = true;            
        }

        //MOD
        public async void changeItAttributeBonusModifierStr()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter a modifier for strength while item is equipped", gv.cc.allItemsList[itemListIndex].attributeBonusModifierStr);
            gv.cc.allItemsList[itemListIndex].attributeBonusModifierStr = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItAttributeBonusModifierDex()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter a modifier for dexterity while item is equipped", gv.cc.allItemsList[itemListIndex].attributeBonusModifierDex);
            gv.cc.allItemsList[itemListIndex].attributeBonusModifierDex = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItAttributeBonusModifierInt()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter a modifier for intellegence while item is equipped", gv.cc.allItemsList[itemListIndex].attributeBonusModifierInt);
            gv.cc.allItemsList[itemListIndex].attributeBonusModifierInt = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItAttributeBonusModifierCha()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter a modifier for charisma while item is equipped", gv.cc.allItemsList[itemListIndex].attributeBonusModifierCha);
            gv.cc.allItemsList[itemListIndex].attributeBonusModifierCha = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItAttributeBonusModifierCon()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter a modifier for constitution while item is equipped", gv.cc.allItemsList[itemListIndex].attributeBonusModifierCon);
            gv.cc.allItemsList[itemListIndex].attributeBonusModifierCon = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItAttributeBonusModifierWis()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter a modifier for wisdom while item is equipped", gv.cc.allItemsList[itemListIndex].attributeBonusModifierWis);
            gv.cc.allItemsList[itemListIndex].attributeBonusModifierWis = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItMovementPointModifier()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter a modifier for movement distance while item is equipped", gv.cc.allItemsList[itemListIndex].MovementPointModifier);
            gv.cc.allItemsList[itemListIndex].MovementPointModifier = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItArmorWeightType()
        {
            List<string> types = new List<string>();
            types.Add("Light");
            types.Add("Medium");
            types.Add("Heavy");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(types, "Select the armor's weight type:");
            if (selected != "none")
            {
                gv.cc.allItemsList[itemListIndex].ArmorWeightType = selected;
            }
            gv.touchEnabled = true;
        }

        //RESISTANCES
        public async void changeItResistAcid()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the modifier for acid resistance:", gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueAcid);
            gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueAcid = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItResistNormal()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the modifier for normal resistance:", gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueNormal);
            gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueNormal = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItResistCold()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the modifier for cold resistance:", gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueCold);
            gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueCold = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItResistElectricity()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the modifier for electricity resistance:", gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueElectricity);
            gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueElectricity = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItResistFire()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the modifier for fire resistance:", gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueFire);
            gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueFire = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItResistMagic()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the modifier for magic resistance:", gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueMagic);
            gv.cc.allItemsList[itemListIndex].damageTypeResistanceValueMagic = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItResistPoison()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the modifier for poison resistance:", gv.cc.allItemsList[itemListIndex].damageTypeResistanceValuePoison);
            gv.cc.allItemsList[itemListIndex].damageTypeResistanceValuePoison = myinput;
            gv.touchEnabled = true;
        }

        //IMAGES
        public async void changeItOnUseSound()
        {
            List<string> items = GetCrtSoundList();
            items.Insert(0, "none");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a sound for this creature's default attack sound:");
            if (selected != "none")
            {
                gv.cc.allItemsList[itemListIndex].itemOnUseSound = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetCrtSoundList()
        {
            List<string> crtTokenList = new List<string>();
            //MODULE SPECIFIC

            //DEFAULTS
            try
            {
                string[] files;
                if (Directory.Exists(gv.mainDirectory + "\\default\\NewModule\\sounds"))
                {
                    files = Directory.GetFiles(gv.mainDirectory + "\\default\\NewModule\\sounds", "*.wav");
                    foreach (string file in files)
                    {
                        try
                        {
                            string fileNameWithOutExt = Path.GetFileNameWithoutExtension(file);
                            if (!crtTokenList.Contains(fileNameWithOutExt))
                            {
                                crtTokenList.Add(fileNameWithOutExt);
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.ToString());
                            gv.errorLog(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                gv.errorLog(ex.ToString());
            }
            return crtTokenList;
        }
        public async void changeItEndSound()
        {
            List<string> items = GetCrtSoundList();
            items.Insert(0, "none");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a sound for this creature's default attack sound:");
            if (selected != "none")
            {
                gv.cc.allItemsList[itemListIndex].itemEndSound = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetCrtEndSoundList()
        {
            List<string> crtTokenList = new List<string>();
            //MODULE SPECIFIC

            //DEFAULTS
            try
            {
                string[] files;
                if (Directory.Exists(gv.mainDirectory + "\\default\\NewModule\\sounds"))
                {
                    files = Directory.GetFiles(gv.mainDirectory + "\\default\\NewModule\\sounds", "*.wav");
                    foreach (string file in files)
                    {
                        try
                        {
                            string fileNameWithOutExt = Path.GetFileNameWithoutExtension(file);
                            if (!crtTokenList.Contains(fileNameWithOutExt))
                            {
                                crtTokenList.Add(fileNameWithOutExt);
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.ToString());
                            gv.errorLog(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                gv.errorLog(ex.ToString());
            }
            return crtTokenList;
        }
        public async void changeItImage()
        {
            List<string> items = GetItemTokenList();
            items.Insert(0, "none");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an image for the item:");
            if (selected != "none")
            {
                gv.cc.allItemsList[itemListIndex].itemImage = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetItemTokenList()
        {
            List<string> crtTokenList = new List<string>();
            //MODULE SPECIFIC

            //DEFAULTS
            try
            {
                string[] files;
                if (Directory.Exists(gv.mainDirectory + "\\default\\NewModule\\graphics"))
                {
                    files = Directory.GetFiles(gv.mainDirectory + "\\default\\NewModule\\graphics", "*.png");
                    foreach (string file in files)
                    {
                        try
                        {
                            string filename = Path.GetFileName(file);
                            if (filename.StartsWith("it_"))
                            {
                                string fileNameWithOutExt = Path.GetFileNameWithoutExtension(file);
                                if (!crtTokenList.Contains(fileNameWithOutExt))
                                {
                                    crtTokenList.Add(fileNameWithOutExt);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.ToString());
                            gv.errorLog(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                gv.errorLog(ex.ToString());
            }
            return crtTokenList;
        }
        public async void changeItSpriteProjectileFilename()
        {
            List<string> items = GetCrtProjectileImageList();
            items.Insert(0, "none");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an image for the creature's ranged projectile attack if it has one:");
            if (selected != "none")
            {
                gv.cc.allItemsList[itemListIndex].projectileSpriteFilename = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetCrtProjectileImageList()
        {
            List<string> crtTokenList = new List<string>();
            //MODULE SPECIFIC

            //DEFAULTS
            try
            {
                string[] files;
                if (Directory.Exists(gv.mainDirectory + "\\default\\NewModule\\graphics"))
                {
                    files = Directory.GetFiles(gv.mainDirectory + "\\default\\NewModule\\graphics", "*.png");
                    foreach (string file in files)
                    {
                        try
                        {
                            string filename = Path.GetFileName(file);
                            if (filename.StartsWith("fx_"))
                            {
                                string fileNameWithOutExt = Path.GetFileNameWithoutExtension(file);
                                if (!crtTokenList.Contains(fileNameWithOutExt))
                                {
                                    crtTokenList.Add(fileNameWithOutExt);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.ToString());
                            gv.errorLog(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                gv.errorLog(ex.ToString());
            }
            return crtTokenList;
        }
        public async void changeItSpriteEndingFilename()
        {
            List<string> items = GetCrtProjectileEndingImageList();
            items.Insert(0, "none");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an image for the creature's ranged attack ending effect if it has one:");
            if (selected != "none")
            {
                gv.cc.allItemsList[itemListIndex].spriteEndingFilename = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetCrtProjectileEndingImageList()
        {
            List<string> crtTokenList = new List<string>();
            //MODULE SPECIFIC

            //DEFAULTS
            try
            {
                string[] files;
                if (Directory.Exists(gv.mainDirectory + "\\default\\NewModule\\graphics"))
                {
                    files = Directory.GetFiles(gv.mainDirectory + "\\default\\NewModule\\graphics", "*.png");
                    foreach (string file in files)
                    {
                        try
                        {
                            string filename = Path.GetFileName(file);
                            if (filename.StartsWith("fx_"))
                            {
                                string fileNameWithOutExt = Path.GetFileNameWithoutExtension(file);
                                if (!crtTokenList.Contains(fileNameWithOutExt))
                                {
                                    crtTokenList.Add(fileNameWithOutExt);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.ToString());
                            gv.errorLog(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                gv.errorLog(ex.ToString());
            }
            return crtTokenList;
        }

        //BEHAVIOR
        public void changeItAutomaticallyHitsTarget()
        {
            btnItAutomaticallyHitsTarget.toggleOn = !btnItAutomaticallyHitsTarget.toggleOn;
            gv.cc.allItemsList[itemListIndex].automaticallyHitsTarget = btnItAutomaticallyHitsTarget.toggleOn;
        }
        public async void changeItAreaOfEffect()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the area of effect radius", gv.cc.allItemsList[itemListIndex].AreaOfEffect);
            gv.cc.allItemsList[itemListIndex].AreaOfEffect = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItAoeShape()
        {
            List<string> types = new List<string>();
            types.Add("Circle");
            types.Add("Cone");
            types.Add("Line");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(types, "Select the shape of the area of effect:");
            if (selected != "none")
            {
                if (selected.Equals("Circle"))
                {
                    gv.cc.allItemsList[itemListIndex].aoeShape = AreaOfEffectShape.Circle;
                }
                else if (selected.Equals("Cone"))
                {
                    gv.cc.allItemsList[itemListIndex].aoeShape = AreaOfEffectShape.Cone;
                }
                else if (selected.Equals("Line"))
                {
                    gv.cc.allItemsList[itemListIndex].aoeShape = AreaOfEffectShape.Line;
                }
            }
            gv.touchEnabled = true;
        }
        public async void changeItOnScoringHitCastSpellTag()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Tag for the spell to cast when scoring a hit with this item:", gv.cc.allItemsList[itemListIndex].onScoringHitCastSpellTag);
            gv.cc.allItemsList[itemListIndex].onScoringHitCastSpellTag = myinput;
            gv.touchEnabled = true;
        }
        public async void changeItOnUseItemCastSpellTag()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Tag for the spell to cast when using this item:", gv.cc.allItemsList[itemListIndex].onUseItemCastSpellTag);
            gv.cc.allItemsList[itemListIndex].onUseItemCastSpellTag = myinput;
            gv.touchEnabled = true;
        }
        public void changeItDestroyItemAfterOnUseItemCastSpell()
        {
            btnItDestroyItemAfterOnUseItemCastSpell.toggleOn = !btnItDestroyItemAfterOnUseItemCastSpell.toggleOn;
            gv.cc.allItemsList[itemListIndex].destroyItemAfterOnUseItemCastSpell = btnItDestroyItemAfterOnUseItemCastSpell.toggleOn;
        }
        public async void changeItLevelOfItemForCastSpell()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the level of the item for cast spell purposes", gv.cc.allItemsList[itemListIndex].levelOfItemForCastSpell);
            gv.cc.allItemsList[itemListIndex].levelOfItemForCastSpell = myinput;
            gv.touchEnabled = true;
        }
        public void changeItUsePlayerClassLevelForOnUseItemCastSpell()
        {
            btnItUsePlayerClassLevelForOnUseItemCastSpell.toggleOn = !btnItUsePlayerClassLevelForOnUseItemCastSpell.toggleOn;
            gv.cc.allItemsList[itemListIndex].usePlayerClassLevelForOnUseItemCastSpell = btnItUsePlayerClassLevelForOnUseItemCastSpell.toggleOn;
        }

        //SPELLS
        public async void addClass()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (PlayerClass sp in gv.cc.datafile.dataPlayerClassList)
            {
                items.Add(sp.tag);
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an class from the list to add to usable classes:");
            if (selected != "none")
            {
                PlayerClass cl = gv.cc.getPlayerClass(selected);
                if (!gv.cc.allItemsList[itemListIndex].classesAllowed.Contains(cl.tag))
                {
                    gv.cc.allItemsList[itemListIndex].classesAllowed.Add(cl.tag);
                }
            }
            gv.touchEnabled = true;
        }
        public void removeClass()
        {
            if (gv.cc.allItemsList[itemListIndex].classesAllowed.Count > 0)
            {
                try
                {
                    gv.cc.allItemsList[itemListIndex].classesAllowed.RemoveAt(classListIndex);
                    classListIndex = 0;
                }
                catch { }
            }
        }
    }
}
