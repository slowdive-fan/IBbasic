using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    public class ToolsetScreenModule
    {
        public GameView gv;
        private IbbToggle btnModuleName = null;
        private IbbToggle btnModuleLabelName = null;
        private IbbToggle btnModuleTitleImage = null;
        private IbbToggle btnModuleDescription = null;
        private IbbToggle btnModuleCredits = null;
        private IbbToggle btnModuleVersion = null;
        private IbbToggle btnStartingArea = null;
        private IbbToggle btnStartingLocX = null;
        private IbbToggle btnStartingLocY = null;
        private IbbToggle btnMaxPartySize = null;
        private IbbToggle btnMaxPlayerMadePCs = null;
        private IbbToggle btnStartingGold = null;
        private IbbButton btnDataChk = null;

        private IBminiTextBox description;

        public ToolsetScreenModule(GameView g)
        {
            gv = g;
            setControlsStart();
            description = new IBminiTextBox(gv);
            description.tbXloc = 0 * gv.squareSize;
            description.tbYloc = 3 * gv.squareSize;
            description.tbWidth = 6 * gv.squareSize;
            description.tbHeight = 6 * gv.squareSize;
            description.showBoxBorder = false;
        }

        public void setControlsStart()
        {
            if (btnModuleName == null)
            {
                btnModuleName = new IbbToggle(gv);
            }
            btnModuleName.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnModuleName.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnModuleName.X = 0 * gv.uiSquareSize;
            btnModuleName.Y = (int)(0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.scaler);
            btnModuleName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnModuleName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnModuleLabelName == null)
            {
                btnModuleLabelName = new IbbToggle(gv);
            }
            btnModuleLabelName.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnModuleLabelName.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnModuleLabelName.X = 0 * gv.uiSquareSize;
            btnModuleLabelName.Y = (int)(1 * gv.uiSquareSize + gv.scaler);
            btnModuleLabelName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnModuleLabelName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnModuleDescription == null)
            {
                btnModuleDescription = new IbbToggle(gv);
            }
            btnModuleDescription.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnModuleDescription.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnModuleDescription.X = 0 * gv.uiSquareSize;
            btnModuleDescription.Y = (int)(1 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.scaler);
            btnModuleDescription.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnModuleDescription.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnModuleCredits == null)
            {
                btnModuleCredits = new IbbToggle(gv);
            }
            btnModuleCredits.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
            btnModuleCredits.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
            btnModuleCredits.X = 0 * gv.uiSquareSize;
            btnModuleCredits.Y = (int)(2 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.scaler);
            btnModuleCredits.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnModuleCredits.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnModuleVersion == null)
            {
                btnModuleVersion = new IbbToggle(gv);
            }
            btnModuleVersion.ImgOn = "mtgl_edit_btn";
            btnModuleVersion.ImgOff = "mtgl_edit_btn";
            btnModuleVersion.X = 0 * gv.uiSquareSize;
            btnModuleVersion.Y = (int)(3 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.scaler);
            btnModuleVersion.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnModuleVersion.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnStartingArea == null)
            {
                btnStartingArea = new IbbToggle(gv);
            }
            btnStartingArea.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnStartingArea.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnStartingArea.X = 0 * gv.uiSquareSize;
            btnStartingArea.Y = (int)(4 * gv.uiSquareSize + gv.scaler);
            btnStartingArea.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnStartingArea.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnStartingLocX == null)
            {
                btnStartingLocX = new IbbToggle(gv);
            }
            btnStartingLocX.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnStartingLocX.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnStartingLocX.X = 0 * gv.uiSquareSize;
            btnStartingLocX.Y = (int)(4 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.scaler);
            btnStartingLocX.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnStartingLocX.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnMaxPartySize == null)
            {
                btnMaxPartySize = new IbbToggle(gv);
            }
            btnMaxPartySize.ImgOn = "mtgl_edit_btn";
            btnMaxPartySize.ImgOff = "mtgl_edit_btn";
            btnMaxPartySize.X = 5 * gv.uiSquareSize;
            btnMaxPartySize.Y = (int)(4 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.scaler);
            btnMaxPartySize.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnMaxPartySize.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnStartingLocY == null)
            {
                btnStartingLocY = new IbbToggle(gv);
            }
            btnStartingLocY.ImgOn = "mtgl_edit_btn";
            btnStartingLocY.ImgOff = "mtgl_edit_btn";
            btnStartingLocY.X = 0 * gv.uiSquareSize;
            btnStartingLocY.Y = (int)(5 * gv.uiSquareSize + gv.scaler);
            btnStartingLocY.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnStartingLocY.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnMaxPlayerMadePCs == null)
            {
                btnMaxPlayerMadePCs = new IbbToggle(gv);
            }
            btnMaxPlayerMadePCs.ImgOn = "mtgl_edit_btn";
            btnMaxPlayerMadePCs.ImgOff = "mtgl_edit_btn";
            btnMaxPlayerMadePCs.X = 5 * gv.uiSquareSize;
            btnMaxPlayerMadePCs.Y = (int)(5 * gv.uiSquareSize + gv.scaler);
            btnMaxPlayerMadePCs.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnMaxPlayerMadePCs.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnStartingGold == null)
            {
                btnStartingGold = new IbbToggle(gv);
            }
            btnStartingGold.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnStartingGold.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnStartingGold.X = 0 * gv.uiSquareSize;
            btnStartingGold.Y = (int)(5 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.scaler);
            btnStartingGold.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnStartingGold.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnModuleTitleImage == null)
            {
                btnModuleTitleImage = new IbbToggle(gv);
            }
            btnModuleTitleImage.ImgOn = "mtgl_edit_btn";
            btnModuleTitleImage.ImgOff = "mtgl_edit_btn";
            btnModuleTitleImage.X = 1 * gv.uiSquareSize;
            btnModuleTitleImage.Y = (int)(6 * gv.uiSquareSize + gv.scaler);
            btnModuleTitleImage.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnModuleTitleImage.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);


            if (btnDataChk == null)
            {
                btnDataChk = new IbbButton(gv, 0.8f);
            }
            btnDataChk.Text = "DATA?";
            btnDataChk.Img = "btn_small"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
            btnDataChk.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
            btnDataChk.X = 10 * gv.uiSquareSize;
            btnDataChk.Y = (int)(6 * gv.uiSquareSize + gv.scaler);
            btnDataChk.Height = (int)(gv.ibbheight * gv.scaler);
            btnDataChk.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsModule()
        {
            setControlsStart();
            int shiftForFont = (btnModuleName.Height / 2) - (gv.fontHeight / 2);
            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            //Page Title
            gv.DrawText("MODULE SETTINGS", center - (7 * (gv.fontWidth + gv.fontCharSpacing)), 2 * gv.scaler, "yl");

            //btnModuleName.Text = gv.mod.moduleName;
            btnModuleName.Draw();
            gv.DrawText(" Module Name: " + gv.mod.moduleName, btnModuleName.X + btnModuleName.Width + gv.scaler, btnModuleName.Y + shiftForFont, "wh");

            btnModuleLabelName.Draw();
            gv.DrawText(" Module Label Name: " + gv.mod.moduleLabelName, btnModuleLabelName.X + btnModuleLabelName.Width + gv.scaler, btnModuleLabelName.Y + shiftForFont, "wh");

            btnModuleDescription.Draw();
            //Description
            int yLoc = btnModuleDescription.Y;
            description.tbXloc = (int)(btnModuleDescription.X + btnModuleDescription.Width + gv.scaler);
            description.tbYloc = yLoc;
            description.tbWidth = 11 * gv.uiSquareSize;
            description.tbHeight = 1 * gv.uiSquareSize;
            string textToSpan = "";
            textToSpan = "<gn> Description:</gn>" + Environment.NewLine;
            textToSpan += gv.mod.moduleDescription;
            description.linesList.Clear();
            description.AddFormattedTextToTextBox(textToSpan);
            description.onDrawTextBox();

            btnModuleCredits.Draw();
            //credits
            yLoc = btnModuleCredits.Y;
            description.tbXloc = (int)(btnModuleCredits.X + btnModuleCredits.Width + gv.scaler);
            description.tbYloc = yLoc;
            description.tbWidth = 11 * gv.uiSquareSize;
            description.tbHeight = 1 * gv.uiSquareSize;
            textToSpan = "";
            textToSpan = "<gn> Credits:</gn>" + Environment.NewLine;
            textToSpan += gv.mod.moduleCredits;
            description.linesList.Clear();
            description.AddFormattedTextToTextBox(textToSpan);
            description.onDrawTextBox();

            btnModuleVersion.Draw();
            gv.DrawText(" Module Version: " + gv.mod.moduleVersion, btnModuleVersion.X + btnModuleVersion.Width + gv.scaler, btnModuleVersion.Y + shiftForFont, "wh");
            btnStartingArea.Draw();
            gv.DrawText(" Starting Area: " + gv.mod.startingArea, btnStartingArea.X + btnStartingArea.Width + gv.scaler, btnStartingArea.Y + shiftForFont, "wh");
            btnStartingLocX.Draw();
            gv.DrawText(" Starting Location X: " + gv.mod.startingPlayerPositionX, btnStartingLocX.X + btnStartingLocX.Width + gv.scaler, btnStartingLocX.Y + shiftForFont, "wh");
            btnMaxPartySize.Draw();
            gv.DrawText(" Max Party Size: " + gv.mod.MaxPartySize, btnMaxPartySize.X + btnMaxPartySize.Width + gv.scaler, btnMaxPartySize.Y + shiftForFont, "wh");
            btnStartingLocY.Draw();
            gv.DrawText(" Starting Location Y: " + gv.mod.startingPlayerPositionY, btnStartingLocY.X + btnStartingLocY.Width + gv.scaler, btnStartingLocY.Y + shiftForFont, "wh");
            btnMaxPlayerMadePCs.Draw();
            gv.DrawText(" Max Player Made PCs: " + gv.mod.numberOfPlayerMadePcsAllowed, btnMaxPlayerMadePCs.X + btnMaxPlayerMadePCs.Width + gv.scaler, btnMaxPlayerMadePCs.Y + shiftForFont, "wh");
            btnStartingGold.Draw();
            gv.DrawText(" Starting Gold: " + gv.mod.partyGold, btnStartingGold.X + btnStartingGold.Width + gv.scaler, btnStartingGold.Y + shiftForFont, "wh");
            btnModuleTitleImage.Draw();
            gv.DrawText(" Title Image: " + gv.mod.titleImageName, btnModuleTitleImage.X + btnModuleTitleImage.Width + gv.scaler, btnModuleTitleImage.Y + shiftForFont, "wh");

            btnDataChk.Draw();

            gv.tsMainMenu.redrawTsMainMenu();

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }
        public void onTouchTsModule(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnDataChk.glowOn = false;

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

                    if (btnDataChk.getImpact(x, y))
                    {
                        btnDataChk.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnDataChk.glowOn = false;

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

                    if (btnModuleName.getImpact(x, y))
                    {
                        changeModuleName();
                    }
                    else if (btnModuleLabelName.getImpact(x, y))
                    {
                        changeModuleLabelName();
                    }
                    else if (btnModuleDescription.getImpact(x,y))
                    {
                        changeModuleDescription();
                    }
                    else if (btnModuleCredits.getImpact(x, y))
                    {
                        changeModuleCredits();
                    }
                    else if (btnModuleVersion.getImpact(x, y))
                    {
                        changeModuleVersion();
                    }
                    else if (btnStartingArea.getImpact(x, y))
                    {
                        changeStartingArea();
                    }
                    else if (btnStartingLocX.getImpact(x, y))
                    {
                        changeStartingLocationX();
                    }
                    else if (btnStartingLocY.getImpact(x, y))
                    {
                        changeStartingLocationY();
                    }
                    else if (btnMaxPartySize.getImpact(x, y))
                    {
                        changeMaxPartySize();
                    }
                    else if (btnMaxPlayerMadePCs.getImpact(x, y))
                    {
                        changeMaxPlayerMadePCs();
                    }
                    else if (btnStartingGold.getImpact(x, y))
                    {
                        changeStartingGold();
                    }
                    else if (btnModuleTitleImage.getImpact(x, y))
                    {
                        changeTitleImage();
                    }
                    else if (btnDataChk.getImpact(x, y))
                    {
                        gv.screenType = "tsDataCheck";
                    }
                    break;
            }
        }
        public async void changeModuleName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose a Name for this Module:", gv.mod.moduleName);
            gv.mod.moduleName = myinput;
            gv.touchEnabled = true;
            /*
            using (TextInputDialog itSel = new TextInputDialog(gv, "Choose a Name for this Module.", gv.mod.moduleName))
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
                        MessageBox.Show("Entering a blank name is not allowed");
                    }
                }
            }
            */
        }
        public async void changeModuleLabelName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose a Name for this Module to display in game:", gv.mod.moduleLabelName);
            gv.mod.moduleLabelName = myinput;
            gv.touchEnabled = true;
            /*using (TextInputDialog itSel = new TextInputDialog(gv, "Choose a Name for this Module to display in game.", gv.mod.moduleLabelName))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (itSel.textInput.Length > 0)
                    {
                        gv.mod.moduleLabelName = itSel.textInput;
                    }
                    else
                    {
                        MessageBox.Show("Entering a blank name is not allowed");
                    }
                }
            }*/
        }
        public async void changeModuleDescription()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Create a description for this Module:", gv.mod.moduleDescription);
            gv.mod.moduleDescription = myinput;
            gv.touchEnabled = true;
            /*using (TextInputDialog itSel = new TextInputDialog(gv, "Create a description for this Module.", gv.mod.moduleDescription))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (itSel.textInput.Length > 0)
                    {
                        gv.mod.moduleDescription = itSel.textInput;
                    }
                    else
                    {
                        MessageBox.Show("Entering a blank description is not allowed");
                    }
                }
            }*/
        }
        public async void changeModuleCredits()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Enter any credits for this Module:", gv.mod.moduleCredits);
            gv.mod.moduleCredits = myinput;
            gv.touchEnabled = true;
            /*using (TextInputDialog itSel = new TextInputDialog(gv, "Enter any credits for this Module.", gv.mod.moduleCredits))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (itSel.textInput.Length > 0)
                    {
                        gv.mod.moduleCredits = itSel.textInput;
                    }
                    else
                    {
                        MessageBox.Show("Entering a blank credits is not allowed");
                    }
                }
            }*/
        }
        public async void changeModuleVersion()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the module version (must be an integer):", gv.mod.moduleVersion);
            gv.mod.moduleVersion = myinput;
            gv.touchEnabled = true;
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Enter the module version", gv.mod.moduleVersion))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.moduleVersion = itSel.numInput;
                }
            }*/
        }
        public async void changeStartingArea()
        {
            gv.touchEnabled = false;

            List<string> areas = new List<string>();
            foreach (Area a in gv.mod.moduleAreasObjects)
            {
                areas.Add(a.Filename);
            }

            string selectedArea = await gv.ListViewPage(areas, "Select the starting area");
            gv.mod.startingArea = selectedArea;
                        
            gv.touchEnabled = true;

            /*List<string> areas = new List<string>();
            foreach (Area a in gv.mod.moduleAreasObjects)
            {
                areas.Add(a.Filename);
            }
            using (DropDownDialog itSel = new DropDownDialog(gv, "Select the starting area", areas, gv.mod.startingArea))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.startingArea = itSel.selectedAreaName;
                }
            }*/
        }
        public async void changeStartingLocationX()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the starting X location in starting area (must be an integer):", gv.mod.startingPlayerPositionX);
            gv.mod.startingPlayerPositionX = myinput;
            gv.touchEnabled = true;
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Enter the starting X location in starting area", gv.mod.startingPlayerPositionX))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.startingPlayerPositionX = itSel.numInput;
                }
            }*/
        }
        public async void changeStartingLocationY()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the starting X location in starting area (must be an integer):", gv.mod.startingPlayerPositionY);
            gv.mod.startingPlayerPositionY = myinput;
            gv.touchEnabled = true;
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Enter the starting X location in starting area", gv.mod.startingPlayerPositionY))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.startingPlayerPositionY = itSel.numInput;
                }
            }*/
        }
        public async void changeMaxPartySize()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the maximum number of PCs in the party. Must be between 1 and 6.", gv.mod.MaxPartySize);
            gv.mod.MaxPartySize = myinput;
            gv.touchEnabled = true;
        }
        public async void changeMaxPlayerMadePCs()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the maximum number of player made PCs allowed in the party. Must be between 0 and 6.", gv.mod.numberOfPlayerMadePcsAllowed);
            gv.mod.numberOfPlayerMadePcsAllowed = myinput;
            gv.touchEnabled = true;
        }
        public async void changeStartingGold()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Enter the players starting gold amount (must be an integer):", gv.mod.partyGold);
            gv.mod.partyGold = myinput;
            gv.touchEnabled = true;
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Enter the players starting gold amount", gv.mod.partyGold))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.partyGold = itSel.numInput;
                }
            }*/
        }
        public async void changeTitleImage()
        {
            List<string> items = GetTitleImageList();
            items.Insert(0, "default");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an image for the creature:");
            
            if (selected != "default")
            {
                gv.mod.titleImageName = selected;
            }
            else
            {
                gv.mod.titleImageName = "title";
            }
            
            gv.touchEnabled = true;            
        }
        public List<string> GetTitleImageList()
        {
            List<string> titleImageList = new List<string>();
            //MODULE SPECIFIC
            List<string> files = gv.GetAllFilesWithExtensionFromBothFolders("\\graphics", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            foreach (string f in files)
            {
                string filename = Path.GetFileName(f);
                if ((filename.StartsWith("fx_")) || (filename.StartsWith("it_")) || (filename.StartsWith("ptr_")) || (filename.StartsWith("prp_")) || (filename.StartsWith("t_")) || (filename.StartsWith("tkn_")))
                {
                    //ignore these files
                }
                else
                {
                    string fileNameWithOutExt = Path.GetFileNameWithoutExtension(f);
                    if (!titleImageList.Contains(fileNameWithOutExt))
                    {
                        titleImageList.Add(fileNameWithOutExt);
                    }
                }
            }
            return titleImageList;
        }
    }
}
