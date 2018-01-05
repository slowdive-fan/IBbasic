﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    public class ToolsetScreenModule
    {
        public GameView gv;
        private IbbToggle btnModuleName = null;
        private IbbToggle btnModuleDescription = null;
        private IbbToggle btnModuleCredits = null;
        private IbbToggle btnModuleVersion = null;
        private IbbToggle btnStartingArea = null;
        private IbbToggle btnStartingLocX = null;
        private IbbToggle btnStartingLocY = null;
        private IbbToggle btnStartingGold = null;
        private IbbButton btnHelp = null;

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
            btnModuleName.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.scaler;
            btnModuleName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnModuleName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnModuleDescription == null)
            {
                btnModuleDescription = new IbbToggle(gv);
            }
            btnModuleDescription.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnModuleDescription.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnModuleDescription.X = 0 * gv.uiSquareSize;
            btnModuleDescription.Y = 1 * gv.uiSquareSize + gv.scaler;
            btnModuleDescription.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnModuleDescription.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnModuleCredits == null)
            {
                btnModuleCredits = new IbbToggle(gv);
            }
            btnModuleCredits.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
            btnModuleCredits.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
            btnModuleCredits.X = 0 * gv.uiSquareSize;
            btnModuleCredits.Y = 2 * gv.uiSquareSize + gv.scaler;
            btnModuleCredits.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnModuleCredits.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
            
            if (btnModuleVersion == null)
            {
                btnModuleVersion = new IbbToggle(gv);
            }
            btnModuleVersion.ImgOn = "mtgl_edit_btn";
            btnModuleVersion.ImgOff = "mtgl_edit_btn";
            btnModuleVersion.X = 0 * gv.uiSquareSize;
            btnModuleVersion.Y = 3 * gv.uiSquareSize + gv.scaler;
            btnModuleVersion.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnModuleVersion.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnStartingArea == null)
            {
                btnStartingArea = new IbbToggle(gv);
            }
            btnStartingArea.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnStartingArea.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnStartingArea.X = 0 * gv.uiSquareSize;
            btnStartingArea.Y = 3 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.scaler;
            btnStartingArea.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnStartingArea.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnStartingLocX == null)
            {
                btnStartingLocX = new IbbToggle(gv);
            }
            btnStartingLocX.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnStartingLocX.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnStartingLocX.X = 0 * gv.uiSquareSize;
            btnStartingLocX.Y = 4 * gv.uiSquareSize + gv.scaler;
            btnStartingLocX.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnStartingLocX.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnStartingLocY == null)
            {
                btnStartingLocY = new IbbToggle(gv);
            }
            btnStartingLocY.ImgOn = "mtgl_edit_btn";
            btnStartingLocY.ImgOff = "mtgl_edit_btn";
            btnStartingLocY.X = 0 * gv.uiSquareSize;
            btnStartingLocY.Y = 4 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.scaler;
            btnStartingLocY.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnStartingLocY.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnStartingGold == null)
            {
                btnStartingGold = new IbbToggle(gv);
            }
            btnStartingGold.ImgOn = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
            btnStartingGold.ImgOff = "mtgl_edit_btn"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
            btnStartingGold.X = 0 * gv.uiSquareSize;
            btnStartingGold.Y = 5 * gv.uiSquareSize + gv.scaler;
            btnStartingGold.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnStartingGold.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
                        
            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
            }
            btnHelp.Text = "HELP";
            btnHelp.Img = "btn_small"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
            btnHelp.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
            btnHelp.X = 10 * gv.uiSquareSize;
            btnHelp.Y = 6 * gv.uiSquareSize + gv.scaler;
            btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
            btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);
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

            btnModuleDescription.Draw();
            //Description
            int yLoc = btnModuleDescription.Y;
            description.tbXloc = btnModuleDescription.X + btnModuleDescription.Width + gv.scaler;
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
            description.tbXloc = btnModuleCredits.X + btnModuleCredits.Width + gv.scaler;
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
            btnStartingLocY.Draw();
            gv.DrawText(" Starting Location Y: " + gv.mod.startingPlayerPositionY, btnStartingLocY.X + btnStartingLocY.Width + gv.scaler, btnStartingLocY.Y + shiftForFont, "wh");
            btnStartingGold.Draw();
            gv.DrawText(" Starting Gold: " + gv.mod.partyGold, btnStartingGold.X + btnStartingGold.Width + gv.scaler, btnStartingGold.Y + shiftForFont, "wh");
            btnHelp.Draw();

            gv.tsMainMenu.redrawTsMainMenu();

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }
        public void onTouchTsModule(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnHelp.glowOn = false;
            
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

                    if (btnModuleName.getImpact(x, y))
                    {
                        changeModuleName();
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
                    else if (btnStartingGold.getImpact(x, y))
                    {
                        changeStartingGold();
                    }
                    else if (btnHelp.getImpact(x, y))
                    {
                        //incrementalSaveModule();
                    }                    
                    break;
            }
        }
        public void changeModuleName()
        {
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
        public void changeModuleDescription()
        {
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
        public void changeModuleCredits()
        {
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
        public void changeModuleVersion()
        {
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Enter the module version", gv.mod.moduleVersion))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.moduleVersion = itSel.numInput;
                }
            }*/
        }
        public void changeStartingArea()
        {
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
        public void changeStartingLocationX()
        {
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Enter the starting X location in starting area", gv.mod.startingPlayerPositionX))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.startingPlayerPositionX = itSel.numInput;
                }
            }*/
        }
        public void changeStartingLocationY()
        {
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Enter the starting X location in starting area", gv.mod.startingPlayerPositionY))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.startingPlayerPositionY = itSel.numInput;
                }
            }*/
        }
        public void changeStartingGold()
        {
            /*using (NumberSelectorDialog itSel = new NumberSelectorDialog(gv, "Enter the players starting gold amount", gv.mod.partyGold))
            {
                var ret = itSel.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    gv.mod.partyGold = itSel.numInput;
                }
            }*/
        }
    }
}
