using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ToolsetScreenPlayerEditor
    {
        public GameView gv;
        public int playerListIndex = 0;
        public int knownSpellListIndex = 0;
        public int knownTraitListIndex = 0;
        public IbbButton btnAddPlayer = null;
        public IbbButton btnRemovePlayer = null;
        public IbbButton btnCopyPlayer = null;
        public string currentMode = "Main"; //Main, Attributes, Images, Known Spells, Known Traits
        public IbbToggle tglMain = null;
        public IbbToggle tglAttributes = null;
        public IbbToggle tglImages = null;
        public IbbToggle tglItems = null;
        public IbbToggle tglTraits = null;
        public IbbToggle tglSpells = null;
        public int numberOfLinesToShow = 23;

        //MAIN
        private IbbToggle btnPcName = null;
        private IbbToggle btnPcTag = null;  //assign same name to tag      
        private IbbToggle btnPcLevel = null;
        private IbbToggle btnPcClass = null;
        private IbbToggle btnPcRace = null;
        private IbbToggle btnGender = null;
        private IbbToggle btnPcXP = null;
        private IbbToggle btnNonRemoveable = null; //for organizing
        //ATTRIBUTES
        private IbbToggle btnPcStrBase = null;
        private IbbToggle btnPcDexBase = null;
        private IbbToggle btnPcConBase = null;
        private IbbToggle btnPcIntBase = null;
        private IbbToggle btnPcWisBase = null;
        private IbbToggle btnPcChaBase = null;
        private IbbToggle btnPcReflex = null;
        private IbbToggle btnPcWill = null;
        private IbbToggle btnPcFortitude = null;
        //IMAGES
        private IbbToggle btnPcTokenFilename = null;
        //EQUIPPED ITEMS
        private IbbToggle btnPcHead = null;
        private IbbToggle btnPcNeck = null;
        private IbbToggle btnPcBody = null;
        private IbbToggle btnPcMainHand = null;
        private IbbToggle btnPcOffHand = null;
        private IbbToggle btnPcRing1 = null;
        private IbbToggle btnPcRing2 = null;
        private IbbToggle btnPcFeet = null;
        private IbbToggle btnPcAmmo = null;
        //KNOWN TRAITS
        public IbbButton btnAddKnownTrait = null;
        public IbbButton btnRemoveKnownTrait = null;
        //KNOWN SPELLS
        public IbbButton btnAddKnownSpell = null;
        public IbbButton btnRemoveKnownSpell = null;

        private IbbButton btnHelp = null;

        public IbRect src = null;
        public IbRect dst = null;

        public ToolsetScreenPlayerEditor(GameView g)
        {
            gv = g;
            currentMode = "Main";
            setControlsStart();
            sortPlayerList();
            tglMain.toggleOn = true;
            tglAttributes.toggleOn = false;
            tglImages.toggleOn = false;
            tglItems.toggleOn = false;
            tglTraits.toggleOn = false;
            tglSpells.toggleOn = false;
        }
        public void sortPlayerList()
        {
            //sort players based on name
            gv.mod.companionPlayerList = gv.mod.companionPlayerList.OrderBy(x => x.name).ToList();
            foreach (Player pc in gv.mod.companionPlayerList)
            {
                try { pc.race = gv.cc.getRace(pc.raceTag).DeepCopy(); }
                catch (Exception ex) { gv.errorLog(ex.ToString()); }
                try { pc.playerClass = gv.cc.getPlayerClass(pc.classTag).DeepCopy(); }
                catch (Exception ex) { gv.errorLog(ex.ToString()); }
            }
        }

        public void setControlsStart()
        {
            if (btnAddPlayer == null)
            {
                btnAddPlayer = new IbbButton(gv, 0.8f);
            }
            //btnAddPlayer.Text = "ADD";
            btnAddPlayer.Img = "btn_small";
            btnAddPlayer.Img2 = "btnadd";
            btnAddPlayer.Glow = "btn_small_glow";
            btnAddPlayer.X = 0 * gv.uiSquareSize + 1 * gv.fontWidth;
            btnAddPlayer.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnAddPlayer.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddPlayer.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemovePlayer == null)
            {
                btnRemovePlayer = new IbbButton(gv, 0.8f);
            }
            //btnRemovePlayer.Text = "REMOVE";
            btnRemovePlayer.Img = "btn_small";
            btnRemovePlayer.Img2 = "btnremove";
            btnRemovePlayer.Glow = "btn_small_glow";
            btnRemovePlayer.X = 1 * gv.uiSquareSize + 1 * gv.fontWidth;
            btnRemovePlayer.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnRemovePlayer.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemovePlayer.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCopyPlayer == null)
            {
                btnCopyPlayer = new IbbButton(gv, 0.8f);
            }
            //btnCopyPlayer.Text = "COPY";
            btnCopyPlayer.Img = "btn_small";
            btnCopyPlayer.Img2 = "btncopy";
            btnCopyPlayer.Glow = "btn_small_glow";
            btnCopyPlayer.X = 2 * gv.uiSquareSize + 1 * gv.fontWidth;
            btnCopyPlayer.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            btnCopyPlayer.Height = (int)(gv.ibbheight * gv.scaler);
            btnCopyPlayer.Width = (int)(gv.ibbwidthR * gv.scaler);

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

            if (tglAttributes == null)
            {
                tglAttributes = new IbbToggle(gv);
            }
            tglAttributes.ImgOn = "mtgl_rbtn_on";
            tglAttributes.ImgOff = "mtgl_rbtn_off";
            tglAttributes.X = 3 * gv.uiSquareSize + 2 * gv.fontWidth;
            tglAttributes.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 2 * gv.fontHeight;
            tglAttributes.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglAttributes.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

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

            if (tglItems == null)
            {
                tglItems = new IbbToggle(gv);
            }
            tglItems.ImgOn = "mtgl_rbtn_on";
            tglItems.ImgOff = "mtgl_rbtn_off";
            tglItems.X = 6 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglItems.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 2 * gv.fontHeight;
            tglItems.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglItems.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

            if (tglTraits == null)
            {
                tglTraits = new IbbToggle(gv);
            }
            tglTraits.ImgOn = "mtgl_rbtn_on";
            tglTraits.ImgOff = "mtgl_rbtn_off";
            tglTraits.X = 9 * gv.uiSquareSize + 1 * gv.fontWidth;
            tglTraits.Y = 0 * gv.uiSquareSize + 2 * gv.fontHeight;
            tglTraits.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglTraits.Width = (int)(gv.ibbMiniTglHeight * gv.scaler);

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
        public void setMainControlsStart()
        {
            if (btnPcName == null)
            {
                btnPcName = new IbbToggle(gv);
            }
            btnPcName.ImgOn = "mtgl_edit_btn";
            btnPcName.ImgOff = "mtgl_edit_btn";
            btnPcName.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcName.Y = 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcTag == null)
            {
                btnPcTag = new IbbToggle(gv);
            }
            btnPcTag.ImgOn = "mtgl_edit_btn";
            btnPcTag.ImgOff = "mtgl_edit_btn";
            btnPcTag.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcTag.Y = 2 * gv.uiSquareSize;
            btnPcTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnNonRemoveable == null)
            {
                btnNonRemoveable = new IbbToggle(gv);
            }
            btnNonRemoveable.ImgOn = "mtgl_edit_btn";
            btnNonRemoveable.ImgOff = "mtgl_edit_btn";
            btnNonRemoveable.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnNonRemoveable.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnNonRemoveable.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnNonRemoveable.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcLevel == null)
            {
                btnPcLevel = new IbbToggle(gv);
            }
            btnPcLevel.ImgOn = "mtgl_edit_btn";
            btnPcLevel.ImgOff = "mtgl_edit_btn";
            btnPcLevel.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcLevel.Y = 3 * gv.uiSquareSize;
            btnPcLevel.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcLevel.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcClass == null)
            {
                btnPcClass = new IbbToggle(gv);
            }
            btnPcClass.ImgOn = "mtgl_edit_btn";
            btnPcClass.ImgOff = "mtgl_edit_btn";
            btnPcClass.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcClass.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcClass.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcClass.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcRace == null)
            {
                btnPcRace = new IbbToggle(gv);
            }
            btnPcRace.ImgOn = "mtgl_edit_btn";
            btnPcRace.ImgOff = "mtgl_edit_btn";
            btnPcRace.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcRace.Y = 4 * gv.uiSquareSize;
            btnPcRace.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcRace.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnGender == null)
            {
                btnGender = new IbbToggle(gv);
            }
            btnGender.ImgOn = "mtgl_edit_btn";
            btnGender.ImgOff = "mtgl_edit_btn";
            btnGender.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnGender.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnGender.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnGender.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcXP == null)
            {
                btnPcXP = new IbbToggle(gv);
            }
            btnPcXP.ImgOn = "mtgl_edit_btn";
            btnPcXP.ImgOff = "mtgl_edit_btn";
            btnPcXP.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcXP.Y = 5 * gv.uiSquareSize;
            btnPcXP.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcXP.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcTokenFilename == null)
            {
                btnPcTokenFilename = new IbbToggle(gv);
            }
            btnPcTokenFilename.ImgOn = "mtgl_edit_btn";
            btnPcTokenFilename.ImgOff = "mtgl_edit_btn";
            btnPcTokenFilename.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcTokenFilename.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcTokenFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcTokenFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setAttributesControlsStart()
        {
            if (btnPcStrBase == null)
            {
                btnPcStrBase = new IbbToggle(gv);
            }
            btnPcStrBase.ImgOn = "mtgl_edit_btn";
            btnPcStrBase.ImgOff = "mtgl_edit_btn";
            btnPcStrBase.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcStrBase.Y = 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcStrBase.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcStrBase.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcDexBase == null)
            {
                btnPcDexBase = new IbbToggle(gv);
            }
            btnPcDexBase.ImgOn = "mtgl_edit_btn";
            btnPcDexBase.ImgOff = "mtgl_edit_btn";
            btnPcDexBase.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcDexBase.Y = 2 * gv.uiSquareSize;
            btnPcDexBase.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcDexBase.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcConBase == null)
            {
                btnPcConBase = new IbbToggle(gv);
            }
            btnPcConBase.ImgOn = "mtgl_edit_btn";
            btnPcConBase.ImgOff = "mtgl_edit_btn";
            btnPcConBase.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcConBase.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcConBase.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcConBase.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcIntBase == null)
            {
                btnPcIntBase = new IbbToggle(gv);
            }
            btnPcIntBase.ImgOn = "mtgl_edit_btn";
            btnPcIntBase.ImgOff = "mtgl_edit_btn";
            btnPcIntBase.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcIntBase.Y = 3 * gv.uiSquareSize;
            btnPcIntBase.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcIntBase.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcWisBase == null)
            {
                btnPcWisBase = new IbbToggle(gv);
            }
            btnPcWisBase.ImgOn = "mtgl_edit_btn";
            btnPcWisBase.ImgOff = "mtgl_edit_btn";
            btnPcWisBase.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcWisBase.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcWisBase.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcWisBase.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcChaBase == null)
            {
                btnPcChaBase = new IbbToggle(gv);
            }
            btnPcChaBase.ImgOn = "mtgl_edit_btn";
            btnPcChaBase.ImgOff = "mtgl_edit_btn";
            btnPcChaBase.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcChaBase.Y = 4 * gv.uiSquareSize;
            btnPcChaBase.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcChaBase.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcReflex == null)
            {
                btnPcReflex = new IbbToggle(gv);
            }
            btnPcReflex.ImgOn = "mtgl_edit_btn";
            btnPcReflex.ImgOff = "mtgl_edit_btn";
            btnPcReflex.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcReflex.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcReflex.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcReflex.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcWill == null)
            {
                btnPcWill = new IbbToggle(gv);
            }
            btnPcWill.ImgOn = "mtgl_edit_btn";
            btnPcWill.ImgOff = "mtgl_edit_btn";
            btnPcWill.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcWill.Y = 5 * gv.uiSquareSize;
            btnPcWill.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcWill.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcFortitude == null)
            {
                btnPcFortitude = new IbbToggle(gv);
            }
            btnPcFortitude.ImgOn = "mtgl_edit_btn";
            btnPcFortitude.ImgOff = "mtgl_edit_btn";
            btnPcFortitude.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcFortitude.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcFortitude.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcFortitude.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setImagesControlsStart()
        {
            if (btnPcTokenFilename == null)
            {
                btnPcTokenFilename = new IbbToggle(gv);
            }
            btnPcTokenFilename.ImgOn = "mtgl_edit_btn";
            btnPcTokenFilename.ImgOff = "mtgl_edit_btn";
            btnPcTokenFilename.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcTokenFilename.Y = 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcTokenFilename.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcTokenFilename.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setEquippedItemsControlsStart()
        {
            if (btnPcHead == null)
            {
                btnPcHead = new IbbToggle(gv);
            }
            btnPcHead.ImgOn = "mtgl_edit_btn";
            btnPcHead.ImgOff = "mtgl_edit_btn";
            btnPcHead.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcHead.Y = 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcHead.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcHead.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcNeck == null)
            {
                btnPcNeck = new IbbToggle(gv);
            }
            btnPcNeck.ImgOn = "mtgl_edit_btn";
            btnPcNeck.ImgOff = "mtgl_edit_btn";
            btnPcNeck.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcNeck.Y = 2 * gv.uiSquareSize;
            btnPcNeck.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcNeck.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcBody == null)
            {
                btnPcBody = new IbbToggle(gv);
            }
            btnPcBody.ImgOn = "mtgl_edit_btn";
            btnPcBody.ImgOff = "mtgl_edit_btn";
            btnPcBody.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcBody.Y = 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcBody.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcBody.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcMainHand == null)
            {
                btnPcMainHand = new IbbToggle(gv);
            }
            btnPcMainHand.ImgOn = "mtgl_edit_btn";
            btnPcMainHand.ImgOff = "mtgl_edit_btn";
            btnPcMainHand.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcMainHand.Y = 3 * gv.uiSquareSize;
            btnPcMainHand.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcMainHand.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcOffHand == null)
            {
                btnPcOffHand = new IbbToggle(gv);
            }
            btnPcOffHand.ImgOn = "mtgl_edit_btn";
            btnPcOffHand.ImgOff = "mtgl_edit_btn";
            btnPcOffHand.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcOffHand.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcOffHand.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcOffHand.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcRing1 == null)
            {
                btnPcRing1 = new IbbToggle(gv);
            }
            btnPcRing1.ImgOn = "mtgl_edit_btn";
            btnPcRing1.ImgOff = "mtgl_edit_btn";
            btnPcRing1.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcRing1.Y = 4 * gv.uiSquareSize;
            btnPcRing1.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcRing1.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcRing2 == null)
            {
                btnPcRing2 = new IbbToggle(gv);
            }
            btnPcRing2.ImgOn = "mtgl_edit_btn";
            btnPcRing2.ImgOff = "mtgl_edit_btn";
            btnPcRing2.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcRing2.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcRing2.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcRing2.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcFeet == null)
            {
                btnPcFeet = new IbbToggle(gv);
            }
            btnPcFeet.ImgOn = "mtgl_edit_btn";
            btnPcFeet.ImgOff = "mtgl_edit_btn";
            btnPcFeet.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcFeet.Y = 5 * gv.uiSquareSize;
            btnPcFeet.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcFeet.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnPcAmmo == null)
            {
                btnPcAmmo = new IbbToggle(gv);
            }
            btnPcAmmo.ImgOn = "mtgl_edit_btn";
            btnPcAmmo.ImgOff = "mtgl_edit_btn";
            btnPcAmmo.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcAmmo.Y = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            btnPcAmmo.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnPcAmmo.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setTraitsControlsStart()
        {
            if (btnAddKnownTrait == null)
            {
                btnAddKnownTrait = new IbbButton(gv, 0.8f);
            }
            //btnAddKnownTrait.Text = "ADD";
            btnAddKnownTrait.Img = "btn_small";
            btnAddKnownTrait.Img2 = "btnadd";
            btnAddKnownTrait.Glow = "btn_small_glow";
            btnAddKnownTrait.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 1 * gv.fontWidth;
            btnAddKnownTrait.Y = 1 * gv.uiSquareSize + 4 * gv.fontHeight;
            btnAddKnownTrait.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddKnownTrait.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveKnownTrait == null)
            {
                btnRemoveKnownTrait = new IbbButton(gv, 0.8f);
            }
            //btnRemoveKnownTrait.Text = "REMOVE";
            btnRemoveKnownTrait.Img = "btn_small";
            btnRemoveKnownTrait.Img2 = "btnremove";
            btnRemoveKnownTrait.Glow = "btn_small_glow";
            btnRemoveKnownTrait.X = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 1 * gv.fontWidth;
            btnRemoveKnownTrait.Y = 1 * gv.uiSquareSize + 4 * gv.fontHeight;
            btnRemoveKnownTrait.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveKnownTrait.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        public void setSpellsControlsStart()
        {
            if (btnAddKnownSpell == null)
            {
                btnAddKnownSpell = new IbbButton(gv, 0.8f);
            }
            //btnAddKnownSpell.Text = "ADD";
            btnAddKnownSpell.Img = "btn_small";
            btnAddKnownSpell.Img2 = "btnadd";
            btnAddKnownSpell.Glow = "btn_small_glow";
            btnAddKnownSpell.X = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 1 * gv.fontWidth;
            btnAddKnownSpell.Y = 1 * gv.uiSquareSize + 4 * gv.fontHeight;
            btnAddKnownSpell.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddKnownSpell.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveKnownSpell == null)
            {
                btnRemoveKnownSpell = new IbbButton(gv, 0.8f);
            }
            //btnRemoveKnownSpell.Text = "REMOVE";
            btnRemoveKnownSpell.Img = "btn_small";
            btnRemoveKnownSpell.Img2 = "btnremove";
            btnRemoveKnownSpell.Glow = "btn_small_glow";
            btnRemoveKnownSpell.X = 5 * gv.uiSquareSize + (gv.uiSquareSize / 2) + 1 * gv.fontWidth;
            btnRemoveKnownSpell.Y = 1 * gv.uiSquareSize + 4 * gv.fontHeight;
            btnRemoveKnownSpell.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveKnownSpell.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsPlayerEditor()
        {
            sortPlayerList();
            setControlsStart();
            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            int shiftForFont = (tglMain.Height / 2) - (gv.fontHeight / 2);
            //Page Title
            gv.DrawText("PLAYER EDITOR", center - (7 * (gv.fontWidth + gv.fontCharSpacing)), 2 * gv.scaler, "yl");

            //label      
            gv.DrawText("PLAYERS", btnAddPlayer.X, btnAddPlayer.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            btnAddPlayer.Draw();
            btnRemovePlayer.Draw();
            btnCopyPlayer.Draw();

            string lastCategory = "";
            numberOfLinesToShow = 23;
            int cnt = 0;
            int startX = 1 * gv.fontWidth;
            int incY = gv.fontHeight + gv.fontLineSpacing;
            int startY = btnAddPlayer.Y + btnAddPlayer.Height - incY;


            foreach (Player pc in gv.mod.companionPlayerList)
            {
                if (cnt == playerListIndex)
                {
                    gv.DrawText(pc.name, startX, startY += incY, "gn");
                }
                else
                {
                    gv.DrawText(pc.name, startX, startY += incY, "wh");
                }
                cnt++;
            }

            tglMain.Draw();
            gv.DrawText("MAIN", tglMain.X + tglMain.Width + gv.scaler, tglMain.Y + shiftForFont, "ma");
            tglAttributes.Draw();
            gv.DrawText("ATTRIBUTES", tglAttributes.X + tglAttributes.Width + gv.scaler, tglAttributes.Y + shiftForFont, "ma");
            tglImages.Draw();
            gv.DrawText("IMAGES", tglImages.X + tglImages.Width + gv.scaler, tglImages.Y + shiftForFont, "ma");
            tglItems.Draw();
            gv.DrawText("EQUIP ITEMS", tglItems.X + tglItems.Width + gv.scaler, tglItems.Y + shiftForFont, "ma");
            tglTraits.Draw();
            gv.DrawText("TRAITS", tglTraits.X + tglTraits.Width + gv.scaler, tglTraits.Y + shiftForFont, "ma");
            tglSpells.Draw();
            gv.DrawText("SPELLS", tglSpells.X + tglSpells.Width + gv.scaler, tglSpells.Y + shiftForFont, "ma");

            if (gv.mod.companionPlayerList.Count > 0)
            {
                if (gv.mod.companionPlayerList[playerListIndex].hp < gv.mod.companionPlayerList[playerListIndex].hpMax)
                {
                    gv.mod.companionPlayerList[playerListIndex].hp = gv.mod.companionPlayerList[playerListIndex].hpMax;
                }
                if (gv.mod.companionPlayerList[playerListIndex].sp < gv.mod.companionPlayerList[playerListIndex].spMax)
                {
                    gv.mod.companionPlayerList[playerListIndex].sp = gv.mod.companionPlayerList[playerListIndex].spMax;
                }
                gv.sf.UpdateStats(gv.mod.companionPlayerList[playerListIndex]);

                if (currentMode.Equals("Main"))
                {
                    setMainControlsStart();
                    drawMain();
                }
                else if (currentMode.Equals("Attributes"))
                {
                    setAttributesControlsStart();
                    drawAttributes();
                }
                else if (currentMode.Equals("Images"))
                {
                    setImagesControlsStart();
                    drawImages();
                }
                else if (currentMode.Equals("Items"))
                {
                    setEquippedItemsControlsStart();
                    drawItems();
                }
                else if (currentMode.Equals("Traits"))
                {
                    setTraitsControlsStart();
                    drawTraits();
                }
                else if (currentMode.Equals("Spells"))
                {
                    setSpellsControlsStart();
                    drawSpells();
                }
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
            int shiftForFont = (btnPcName.Height / 2) - (gv.fontHeight / 2);
            btnPcName.Draw();
            gv.DrawText("NAME: " + gv.mod.companionPlayerList[playerListIndex].name, btnPcName.X + btnPcName.Width + gv.scaler, btnPcName.Y + shiftForFont, "wh");
            btnPcTag.Draw();
            gv.DrawText("TAG: " + gv.mod.companionPlayerList[playerListIndex].tag, btnPcTag.X + btnPcTag.Width + gv.scaler, btnPcTag.Y + shiftForFont, "wh");
            btnNonRemoveable.Draw();
            gv.DrawText("NON-REMOVEABLE: " + gv.mod.companionPlayerList[playerListIndex].nonRemoveablePc, btnNonRemoveable.X + btnNonRemoveable.Width + gv.scaler, btnNonRemoveable.Y + shiftForFont, "wh");
            btnPcLevel.Draw();
            gv.DrawText("LEVEL: " + gv.mod.companionPlayerList[playerListIndex].classLevel, btnPcLevel.X + btnPcLevel.Width + gv.scaler, btnPcLevel.Y + shiftForFont, "wh");
            btnPcClass.Draw();
            gv.DrawText("CALSS TAG: " + gv.mod.companionPlayerList[playerListIndex].classTag, btnPcClass.X + btnPcClass.Width + gv.scaler, btnPcClass.Y + shiftForFont, "wh");
            btnPcRace.Draw();
            gv.DrawText("RACE TAG: " + gv.mod.companionPlayerList[playerListIndex].raceTag, btnPcRace.X + btnPcRace.Width + gv.scaler, btnPcRace.Y + shiftForFont, "wh");
            btnGender.Draw();
            gv.DrawText("IS MALE: " + gv.mod.companionPlayerList[playerListIndex].isMale, btnGender.X + btnGender.Width + gv.scaler, btnGender.Y + shiftForFont, "wh");
            btnPcXP.Draw();
            gv.DrawText("XP: " + gv.mod.companionPlayerList[playerListIndex].XP, btnPcXP.X + btnPcXP.Width + gv.scaler, btnPcXP.Y + shiftForFont, "wh");
        }
        public void drawAttributes()
        {
            int shiftForFont = (btnPcName.Height / 2) - (gv.fontHeight / 2);
            btnPcStrBase.Draw();
            gv.DrawText("STR: " + gv.mod.companionPlayerList[playerListIndex].baseStr, btnPcStrBase.X + btnPcStrBase.Width + gv.scaler, btnPcStrBase.Y + shiftForFont, "wh");
            btnPcDexBase.Draw();
            gv.DrawText("DEX: " + gv.mod.companionPlayerList[playerListIndex].baseDex, btnPcDexBase.X + btnPcDexBase.Width + gv.scaler, btnPcDexBase.Y + shiftForFont, "wh");
            btnPcConBase.Draw();
            gv.DrawText("CON: " + gv.mod.companionPlayerList[playerListIndex].baseCon, btnPcConBase.X + btnPcConBase.Width + gv.scaler, btnPcConBase.Y + shiftForFont, "wh");
            btnPcIntBase.Draw();
            gv.DrawText("INT: " + gv.mod.companionPlayerList[playerListIndex].baseInt, btnPcIntBase.X + btnPcIntBase.Width + gv.scaler, btnPcIntBase.Y + shiftForFont, "wh");
            btnPcWisBase.Draw();
            gv.DrawText("WIS: " + gv.mod.companionPlayerList[playerListIndex].baseWis, btnPcWisBase.X + btnPcWisBase.Width + gv.scaler, btnPcWisBase.Y + shiftForFont, "wh");
            btnPcChaBase.Draw();
            gv.DrawText("CHA: " + gv.mod.companionPlayerList[playerListIndex].baseCha, btnPcChaBase.X + btnPcChaBase.Width + gv.scaler, btnPcChaBase.Y + shiftForFont, "wh");
            int yLoc = btnPcChaBase.Y;
            //btnPcReflex.Draw();
            gv.DrawText("Reflex: " + gv.mod.companionPlayerList[playerListIndex].reflex, btnPcReflex.X, yLoc + (2 * gv.fontHeight), "wh");
            //btnPcWill.Draw();
            gv.DrawText("Will: " + gv.mod.companionPlayerList[playerListIndex].will, btnPcWill.X, yLoc + (3 * gv.fontHeight), "wh");
            //btnPcFortitude.Draw();
            gv.DrawText("Fortitude: " + gv.mod.companionPlayerList[playerListIndex].fortitude, btnPcFortitude.X, yLoc + (4 * gv.fontHeight), "wh");
            gv.DrawText("HP: " + gv.mod.companionPlayerList[playerListIndex].hp + " HPmax: " + gv.mod.companionPlayerList[playerListIndex].hpMax,
                        btnPcFortitude.X, yLoc + (5 * gv.fontHeight), "wh");
            gv.DrawText("SP: " + gv.mod.companionPlayerList[playerListIndex].sp + " SPmax: " + gv.mod.companionPlayerList[playerListIndex].spMax,
                        btnPcFortitude.X, yLoc + (6 * gv.fontHeight), "wh");

        }
        public void drawImages()
        {
            int shiftForFont = (btnPcName.Height / 2) - (gv.fontHeight / 2);
            btnPcTokenFilename.Draw();
            string token = gv.mod.companionPlayerList[playerListIndex].tokenFilename;
            int brX = (int)(gv.squareSize * gv.scaler);
            int brY = (int)(gv.squareSize * gv.scaler);
            gv.DrawText("TOKEN FILENAME: " + token, btnPcTokenFilename.X + btnPcTokenFilename.Width + gv.scaler, btnPcTokenFilename.Y + shiftForFont, "wh");
            //top frame
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(token).Width, gv.cc.GetFromTileBitmapList(token).Height / 2);
            dst = new IbRect(btnPcTokenFilename.X, btnPcTokenFilename.Y + (gv.uiSquareSize / 2), brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(token), src, dst);
            //bottom frame
            src = new IbRect(0, gv.cc.GetFromTileBitmapList(token).Height / 2, gv.cc.GetFromTileBitmapList(token).Width, gv.cc.GetFromTileBitmapList(token).Height / 2);
            dst = new IbRect(btnPcTokenFilename.X + brX, btnPcTokenFilename.Y + (gv.uiSquareSize / 2), brX, brY);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(token), src, dst);

        }
        public void drawItems()
        {
            int shiftForFont = (btnPcName.Height / 2) - (gv.fontHeight / 2);
            btnPcHead.Draw();
            gv.DrawText("HEAD: " + gv.mod.companionPlayerList[playerListIndex].HeadRefs.name, btnPcHead.X + btnPcHead.Width + gv.scaler, btnPcHead.Y + shiftForFont, "wh");
            btnPcNeck.Draw();
            gv.DrawText("NECK: " + gv.mod.companionPlayerList[playerListIndex].NeckRefs.name, btnPcNeck.X + btnPcNeck.Width + gv.scaler, btnPcNeck.Y + shiftForFont, "wh");
            btnPcBody.Draw();
            gv.DrawText("BODY: " + gv.mod.companionPlayerList[playerListIndex].BodyRefs.name, btnPcBody.X + btnPcBody.Width + gv.scaler, btnPcBody.Y + shiftForFont, "wh");
            btnPcMainHand.Draw();
            gv.DrawText("MAIN HAND: " + gv.mod.companionPlayerList[playerListIndex].MainHandRefs.name, btnPcMainHand.X + btnPcMainHand.Width + gv.scaler, btnPcMainHand.Y + shiftForFont, "wh");
            btnPcOffHand.Draw();
            gv.DrawText("OFF HAND: " + gv.mod.companionPlayerList[playerListIndex].OffHandRefs.name, btnPcOffHand.X + btnPcOffHand.Width + gv.scaler, btnPcOffHand.Y + shiftForFont, "wh");
            btnPcRing1.Draw();
            gv.DrawText("RING 1: " + gv.mod.companionPlayerList[playerListIndex].RingRefs.name, btnPcRing1.X + btnPcRing1.Width + gv.scaler, btnPcRing1.Y + shiftForFont, "wh");
            btnPcRing2.Draw();
            gv.DrawText("RING 2: " + gv.mod.companionPlayerList[playerListIndex].Ring2Refs.name, btnPcRing2.X + btnPcRing2.Width + gv.scaler, btnPcRing2.Y + shiftForFont, "wh");
            btnPcFeet.Draw();
            gv.DrawText("FEET: " + gv.mod.companionPlayerList[playerListIndex].FeetRefs.name, btnPcFeet.X + btnPcFeet.Width + gv.scaler, btnPcFeet.Y + shiftForFont, "wh");
            btnPcAmmo.Draw();
            gv.DrawText("AMMO: " + gv.mod.companionPlayerList[playerListIndex].AmmoRefs.name, btnPcAmmo.X + btnPcAmmo.Width + gv.scaler, btnPcAmmo.Y + shiftForFont, "wh");

        }
        public void drawTraits()
        {
            int shiftForFont = (btnPcName.Height / 2) - (gv.fontHeight / 2);
            //label      
            gv.DrawText("KNOWN TRAITS", btnAddKnownTrait.X, btnAddKnownTrait.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            btnAddKnownTrait.Draw();
            btnRemoveKnownTrait.Draw();
            //list all containers (tap on a container in the list to show elements for editing)
            int startX = btnAddKnownTrait.X;
            int startY = btnAddKnownTrait.Y + btnAddKnownTrait.Height - gv.fontHeight;
            int incY = gv.fontHeight + gv.fontLineSpacing;
            int cnt = 0;
            foreach (string c in gv.mod.companionPlayerList[playerListIndex].knownTraitsTags)
            {
                if (cnt == knownTraitListIndex)
                {
                    gv.DrawText(gv.cc.getTraitByTag(c).name, startX, startY += incY, "gn");
                }
                else
                {
                    gv.DrawText(gv.cc.getTraitByTag(c).name, startX, startY += incY, "wh");
                }
                cnt++;
            }
        }
        public void drawSpells()
        {
            int shiftForFont = (btnPcName.Height / 2) - (gv.fontHeight / 2);
            //label      
            gv.DrawText("KNOWN SPELLS", btnAddKnownSpell.X, btnAddKnownSpell.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            btnAddKnownSpell.Draw();
            btnRemoveKnownSpell.Draw();
            //list all containers (tap on a container in the list to show elements for editing)
            int startX = btnAddKnownSpell.X;
            int startY = btnAddKnownSpell.Y + btnAddKnownSpell.Height - gv.fontHeight;
            int incY = gv.fontHeight + gv.fontLineSpacing;
            int cnt = 0;
            foreach (string c in gv.mod.companionPlayerList[playerListIndex].knownSpellsTags)
            {
                if (cnt == knownSpellListIndex)
                {
                    gv.DrawText(gv.cc.getSpellByTag(c).name, startX, startY += incY, "gn");
                }
                else
                {
                    gv.DrawText(gv.cc.getSpellByTag(c).name, startX, startY += incY, "wh");
                }
                cnt++;
            }
        }

        public void onTouchTsPlayerEditor(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnHelp.glowOn = false;

            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }

            bool ret = gv.tsMainMenu.onTouchTsMainMenu(eX, eY, eventType);
            if (ret) { return; } //did some action on the main menu so do nothing here

            //TODO only allow editing of module creatures
            if (playerListIndex < gv.mod.companionPlayerList.Count)
            {
                if (currentMode.Equals("Main"))
                {
                    ret = onTouchMain(eX, eY, eventType);
                    if (ret) { return; } //did some action on the tile panel so do nothing here
                }
                else if (currentMode.Equals("Attributes"))
                {
                    ret = onTouchAttribute(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Settings panel so do nothing here
                }
                else if (currentMode.Equals("Images"))
                {
                    ret = onTouchImages(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Info panel so do nothing here
                }
                else if (currentMode.Equals("Items"))
                {
                    ret = onTouchItems(eX, eY, eventType);
                    if (ret) { return; } //did some action on the Walk-LoS panel so do nothing here
                }
                else if (currentMode.Equals("Traits"))
                {
                    ret = onTouchTraits(eX, eY, eventType);
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
                    if (gv.mod.companionPlayerList.Count > 0)
                    {
                        if ((x > 0) && (x < tglMain.X) && (y > btnAddPlayer.Y + btnAddPlayer.Height))
                        {
                            //left column clicked 
                            int PanelLeftLocation = btnAddPlayer.X;
                            int PanelTopLocation = btnAddPlayer.Y + btnAddPlayer.Height;
                            int lineIndex = (y - PanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing);
                            if ((lineIndex < gv.mod.companionPlayerList.Count) && (lineIndex >= 0))
                            {
                                playerListIndex = lineIndex;
                            }
                        }
                    }

                    if (tglMain.getImpact(x, y))
                    {
                        tglMain.toggleOn = true;
                        tglAttributes.toggleOn = false;
                        tglImages.toggleOn = false;
                        tglItems.toggleOn = false;
                        tglTraits.toggleOn = false;
                        tglSpells.toggleOn = false;
                        currentMode = "Main";
                    }
                    else if (tglAttributes.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttributes.toggleOn = true;
                        tglImages.toggleOn = false;
                        tglItems.toggleOn = false;
                        tglTraits.toggleOn = false;
                        tglSpells.toggleOn = false;
                        currentMode = "Attributes";
                    }
                    else if (tglImages.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttributes.toggleOn = false;
                        tglImages.toggleOn = true;
                        tglItems.toggleOn = false;
                        tglTraits.toggleOn = false;
                        tglSpells.toggleOn = false;
                        currentMode = "Images";
                    }
                    else if (tglItems.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttributes.toggleOn = false;
                        tglImages.toggleOn = false;
                        tglItems.toggleOn = true;
                        tglTraits.toggleOn = false;
                        tglSpells.toggleOn = false;
                        currentMode = "Items";
                    }
                    else if (tglTraits.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttributes.toggleOn = false;
                        tglImages.toggleOn = false;
                        tglItems.toggleOn = false;
                        tglTraits.toggleOn = true;
                        tglSpells.toggleOn = false;
                        currentMode = "Traits";
                    }
                    else if (tglSpells.getImpact(x, y))
                    {
                        tglMain.toggleOn = false;
                        tglAttributes.toggleOn = false;
                        tglImages.toggleOn = false;
                        tglItems.toggleOn = false;
                        tglTraits.toggleOn = false;
                        tglSpells.toggleOn = true;
                        currentMode = "Spells";
                    }
                    else if (btnAddPlayer.getImpact(x, y))
                    {
                        addPlayer();
                    }
                    else if (btnRemovePlayer.getImpact(x, y))
                    {
                        removePlayer();
                    }
                    else if (btnCopyPlayer.getImpact(x, y))
                    {
                        copyPlayer();
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

                    if (btnPcName.getImpact(x, y))
                    {
                        changePcName();
                    }
                    else if (btnPcTag.getImpact(x, y))
                    {
                        changePcTag();
                    }
                    else if (btnNonRemoveable.getImpact(x, y))
                    {
                        //toggle
                    }
                    else if (btnPcLevel.getImpact(x, y))
                    {
                        changePcLevel();
                    }
                    else if (btnPcClass.getImpact(x, y))
                    {
                        changePcClass();
                    }
                    else if (btnPcRace.getImpact(x, y))
                    {
                        changePcRace();
                    }
                    else if (btnGender.getImpact(x, y))
                    {
                        //toggle
                    }
                    else if (btnPcXP.getImpact(x, y))
                    {
                        changePcXP();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchAttribute(int eX, int eY, MouseEventType.EventType eventType)
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

                    if (btnPcStrBase.getImpact(x, y))
                    {
                        changePcStr();
                    }
                    else if (btnPcDexBase.getImpact(x, y))
                    {
                        changePcDex();
                    }
                    else if (btnPcConBase.getImpact(x, y))
                    {
                        changePcCon();
                    }
                    else if (btnPcIntBase.getImpact(x, y))
                    {
                        changePcInt();
                    }
                    else if (btnPcWisBase.getImpact(x, y))
                    {
                        changePcWis();
                    }
                    else if (btnPcChaBase.getImpact(x, y))
                    {
                        changePcCha();
                    }
                    else if (btnPcReflex.getImpact(x, y))
                    {
                        //changePcReflex();
                    }
                    else if (btnPcFortitude.getImpact(x, y))
                    {
                        //changePcFortitude();
                    }
                    else if (btnPcWill.getImpact(x, y))
                    {
                        //changePcWill();
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

                    if (btnPcTokenFilename.getImpact(x, y))
                    {
                        changePcToken();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchItems(int eX, int eY, MouseEventType.EventType eventType)
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

                    if (btnPcHead.getImpact(x, y))
                    {
                        changePcHead();
                    }
                    else if (btnPcNeck.getImpact(x, y))
                    {
                        changePcNeck();
                    }
                    else if (btnPcBody.getImpact(x, y))
                    {
                        changePcBody();
                    }
                    else if (btnPcMainHand.getImpact(x, y))
                    {
                        changePcMainHand();
                    }
                    else if (btnPcOffHand.getImpact(x, y))
                    {
                        changePcOffHand();
                    }
                    else if (btnPcRing1.getImpact(x, y))
                    {
                        changePcRing1();
                    }
                    else if (btnPcRing2.getImpact(x, y))
                    {
                        changePcRing2();
                    }
                    else if (btnPcFeet.getImpact(x, y))
                    {
                        changePcFeet();
                    }
                    else if (btnPcAmmo.getImpact(x, y))
                    {
                        changePcAmmo();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchTraits(int eX, int eY, MouseEventType.EventType eventType)
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
                    if ((x > btnAddKnownTrait.X) && (y > btnAddKnownTrait.Y + btnAddKnownTrait.Height))
                    {
                        //figure out which line was tapped and if that is a category do expand/collapse
                        int PanelLeftLocation = btnAddKnownTrait.X;
                        int PanelTopLocation = btnAddKnownTrait.Y + btnAddKnownTrait.Height;
                        int lineIndex = (y - PanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing);
                        if ((lineIndex < gv.mod.companionPlayerList[playerListIndex].knownTraitsTags.Count) && (gv.mod.companionPlayerList[playerListIndex].knownTraitsTags.Count > 0))
                        {
                            knownTraitListIndex = lineIndex;
                        }
                    }

                    if (btnAddKnownTrait.getImpact(x, y))
                    {
                        addKnownTrait();
                    }
                    else if (btnRemoveKnownTrait.getImpact(x, y))
                    {
                        removeKnownTrait();
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
                        if ((lineIndex < gv.mod.companionPlayerList[playerListIndex].knownSpellsTags.Count) && (gv.mod.companionPlayerList[playerListIndex].knownSpellsTags.Count > 0))
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

        //PLAYER PANEL
        public void addPlayer()
        {
            if (playerListIndex < gv.mod.companionPlayerList.Count)
            {
                Player newPc = new Player();
                newPc.name = "NewPlayerName";
                newPc.tag = "newplayername";
                gv.mod.companionPlayerList.Add(newPc);
                sortPlayerList();
            }
        }
        public void removePlayer()
        {
            if (playerListIndex < gv.mod.companionPlayerList.Count)
            {
                //TODO
                if (gv.mod.companionPlayerList.Count > 0)
                {
                    try
                    {
                        gv.mod.companionPlayerList.RemoveAt(playerListIndex);
                        playerListIndex = 0;
                    }
                    catch { }
                }
            }
        }
        public void copyPlayer()
        {
            if (playerListIndex < gv.mod.companionPlayerList.Count)
            {
                Player newPc = gv.mod.companionPlayerList[playerListIndex].DeepCopy();
                newPc.name = gv.mod.companionPlayerList[playerListIndex].name + "_copy";
                newPc.tag = gv.mod.companionPlayerList[playerListIndex].tag + "_copy";
                gv.mod.companionPlayerList.Add(newPc);
                sortPlayerList();
            }
        }

        //MAIN
        public async void changePcName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Name for this player", gv.mod.companionPlayerList[playerListIndex].name);
            gv.mod.companionPlayerList[playerListIndex].name = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcTag()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Enter a unique tag for this player (typically use the lower case version of the player's name with no spaces).", gv.mod.companionPlayerList[playerListIndex].tag);
            gv.mod.companionPlayerList[playerListIndex].tag = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcLevel()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter level of the player", gv.mod.companionPlayerList[playerListIndex].classLevel);
            gv.mod.companionPlayerList[playerListIndex].classLevel = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcClass()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (PlayerClass p in gv.cc.datafile.dataPlayerClassList)
            {
                items.Add(p.tag);
            }

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select the class for the player:");
            if (selected != "none")
            {
                gv.mod.companionPlayerList[playerListIndex].classTag = selected;
            }
            gv.touchEnabled = true;
        }
        public async void changePcRace()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Race rc in gv.cc.datafile.dataRacesList)
            {
                items.Add(rc.tag);
            }

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select the race for the player:");
            if (selected != "none")
            {
                gv.mod.companionPlayerList[playerListIndex].raceTag = selected;
            }
            gv.touchEnabled = true;
        }
        public async void changePcXP()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the player's XP", gv.mod.companionPlayerList[playerListIndex].XP);
            gv.mod.companionPlayerList[playerListIndex].hpMax = myinput;
            gv.touchEnabled = true;
        }

        //ATTRIBUTES
        public async void changePcStr()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the base strength for this player", gv.mod.companionPlayerList[playerListIndex].baseStr);
            gv.mod.companionPlayerList[playerListIndex].baseStr = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcDex()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the base dexterity for this player", gv.mod.companionPlayerList[playerListIndex].baseDex);
            gv.mod.companionPlayerList[playerListIndex].baseDex = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcCon()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the base constitution for this player", gv.mod.companionPlayerList[playerListIndex].baseCon);
            gv.mod.companionPlayerList[playerListIndex].baseCon = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcInt()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the base intelligence for this player", gv.mod.companionPlayerList[playerListIndex].baseInt);
            gv.mod.companionPlayerList[playerListIndex].baseInt = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcWis()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the base wisdom for this player", gv.mod.companionPlayerList[playerListIndex].baseWis);
            gv.mod.companionPlayerList[playerListIndex].baseWis = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcCha()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the base charisma for this player", gv.mod.companionPlayerList[playerListIndex].baseCha);
            gv.mod.companionPlayerList[playerListIndex].baseCha = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcReflex()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the reflex saving throw bonus for this player", gv.mod.companionPlayerList[playerListIndex].reflex);
            gv.mod.companionPlayerList[playerListIndex].reflex = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcWill()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the will saving throw bonus for this player", gv.mod.companionPlayerList[playerListIndex].will);
            gv.mod.companionPlayerList[playerListIndex].will = myinput;
            gv.touchEnabled = true;
        }
        public async void changePcFortitude()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the fortitude saving throw bonus for this player", gv.mod.companionPlayerList[playerListIndex].fortitude);
            gv.mod.companionPlayerList[playerListIndex].fortitude = myinput;
            gv.touchEnabled = true;
        }

        //IMAGES
        public async void changePcToken()
        {
            List<string> items = GetPcTokenList();
            items.Insert(0, "none");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an image for the player:");
            if (selected != "none")
            {
                gv.mod.companionPlayerList[playerListIndex].tokenFilename = selected;
            }
            gv.touchEnabled = true;
        }
        public List<string> GetPcTokenList()
        {
            List<string> pcTokenList = new List<string>();
            //MODULE SPECIFIC
            List<string> files = gv.GetAllFilesWithExtensionFromBothFolders("\\graphics", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            foreach (string f in files)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                if (filenameNoExt.StartsWith("tkn_"))
                {
                    pcTokenList.Add(filenameNoExt);
                }
            }
            return pcTokenList;
        }

        //EQUIPPED ITEMS
        public async void changePcHead()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Item it in gv.cc.allItemsList)
            {
                if (it.category.Equals("Head"))
                {
                    items.Add(it.name);
                }
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a head item for the player:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                if (it != null)
                {
                    gv.mod.companionPlayerList[playerListIndex].HeadRefs = gv.cc.createItemRefsFromItem(it);
                }
            }
            gv.touchEnabled = true;
        }
        public async void changePcNeck()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Item it in gv.cc.allItemsList)
            {
                if (it.category.Equals("Neck"))
                {
                    items.Add(it.name);
                }
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a neck item for the player:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                if (it != null)
                {
                    gv.mod.companionPlayerList[playerListIndex].NeckRefs = gv.cc.createItemRefsFromItem(it);
                }
            }
            gv.touchEnabled = true;
        }
        public async void changePcBody()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Item it in gv.cc.allItemsList)
            {
                if (it.category.Equals("Armor"))
                {
                    items.Add(it.name);
                }
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an armor item for the player:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                if (it != null)
                {
                    gv.mod.companionPlayerList[playerListIndex].BodyRefs = gv.cc.createItemRefsFromItem(it);
                }
            }
            gv.touchEnabled = true;
        }
        public async void changePcMainHand()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Item it in gv.cc.allItemsList)
            {
                if ((it.category == "Melee") || (it.category == "Ranged"))
                {
                    items.Add(it.name);
                }
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a weapon item for the player:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                if (it != null)
                {
                    gv.mod.companionPlayerList[playerListIndex].MainHandRefs = gv.cc.createItemRefsFromItem(it);
                }
            }
            gv.touchEnabled = true;
        }
        public async void changePcOffHand()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Item it in gv.cc.allItemsList)
            {
                if (it.category.Equals("Shield"))
                {
                    items.Add(it.name);
                }
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a shield item for the player:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                if (it != null)
                {
                    gv.mod.companionPlayerList[playerListIndex].OffHandRefs = gv.cc.createItemRefsFromItem(it);
                }
            }
            gv.touchEnabled = true;
        }
        public async void changePcRing1()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Item it in gv.cc.allItemsList)
            {
                if (it.category.Equals("Ring"))
                {
                    items.Add(it.name);
                }
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a ring item for the player:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                if (it != null)
                {
                    gv.mod.companionPlayerList[playerListIndex].RingRefs = gv.cc.createItemRefsFromItem(it);
                }
            }
            gv.touchEnabled = true;
        }
        public async void changePcRing2()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Item it in gv.cc.allItemsList)
            {
                if (it.category.Equals("Ring"))
                {
                    items.Add(it.name);
                }
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a ring item for the player:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                if (it != null)
                {
                    gv.mod.companionPlayerList[playerListIndex].Ring2Refs = gv.cc.createItemRefsFromItem(it);
                }
            }
            gv.touchEnabled = true;
        }
        public async void changePcFeet()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Item it in gv.cc.allItemsList)
            {
                if (it.category.Equals("Feet"))
                {
                    items.Add(it.name);
                }
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a foot item for the player:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                if (it != null)
                {
                    gv.mod.companionPlayerList[playerListIndex].FeetRefs = gv.cc.createItemRefsFromItem(it);
                }
            }
            gv.touchEnabled = true;
        }
        public async void changePcAmmo()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Item it in gv.cc.allItemsList)
            {
                if (it.category.Equals("Ammo"))
                {
                    items.Add(it.name);
                }
            }
            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an ammo item for the player:");
            if (selected != "none")
            {
                Item it = gv.cc.getItemByName(selected);
                if (it != null)
                {
                    gv.mod.companionPlayerList[playerListIndex].AmmoRefs = gv.cc.createItemRefsFromItem(it);
                }
            }
            gv.touchEnabled = true;
        }

        //TRAITS
        public async void addKnownTrait()
        {
            List<string> items = new List<string>();
            items.Add("none");
            foreach (Trait sp in gv.cc.datafile.dataTraitsList)
            {
                items.Add(sp.name);
            }

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an item from the list to add to creaure's known traits:");
            if (selected != "none")
            {
                Trait sp = gv.cc.getTraitByName(selected);
                gv.mod.companionPlayerList[playerListIndex].knownTraitsTags.Add(sp.tag);
            }
            gv.touchEnabled = true;
        }
        public void removeKnownTrait()
        {
            if (gv.mod.companionPlayerList[playerListIndex].knownTraitsTags.Count > 0)
            {
                try
                {
                    gv.mod.companionPlayerList[playerListIndex].knownTraitsTags.RemoveAt(knownTraitListIndex);
                    knownTraitListIndex = 0;
                }
                catch { }
            }
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
                gv.mod.companionPlayerList[playerListIndex].knownSpellsTags.Add(sp.tag);
            }
            gv.touchEnabled = true;
        }
        public void removeKnownSpell()
        {
            if (gv.mod.companionPlayerList[playerListIndex].knownSpellsTags.Count > 0)
            {
                try
                {
                    gv.mod.companionPlayerList[playerListIndex].knownSpellsTags.RemoveAt(knownSpellListIndex);
                    knownSpellListIndex = 0;
                }
                catch { }
            }
        }
    }
}
