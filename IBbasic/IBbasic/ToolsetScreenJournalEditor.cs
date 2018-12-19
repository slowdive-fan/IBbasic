using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBbasic
{
    public class ToolsetScreenJournalEditor
    {
        public GameView gv;
        //journal name and tag
        //entry title, tag, id, text
        public IbbToggle btnJournalTag = null;
        public IbbToggle btnJournalName = null;
        public IbbToggle btnEntryName = null;
        public IbbToggle btnEntryTag = null;
        public IbbToggle btnEntryID = null;
        public IbbToggle btnEntryText = null;
        public IbbButton btnAddJournal = null;
        public IbbButton btnRemoveJournal = null;
        public IbbButton btnAddEntry = null;
        public IbbButton btnRemoveEntry = null;
        public IbbButton btnHelp = null;
        public int journalListIndex = 0;
        public int entryListIndex = 0;

        private IBminiTextBox description;

        public ToolsetScreenJournalEditor(GameView g)
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
            if (btnJournalName == null)
            {
                btnJournalName = new IbbToggle(gv);
            }
            btnJournalName.ImgOn = "mtgl_edit_btn";
            btnJournalName.ImgOff = "mtgl_edit_btn";
            btnJournalName.X = 4 * gv.uiSquareSize;
            btnJournalName.Y = 0 * gv.uiSquareSize + (gv.uiSquareSize / 4);
            btnJournalName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnJournalName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnJournalTag == null)
            {
                btnJournalTag = new IbbToggle(gv);
            }
            btnJournalTag.ImgOn = "mtgl_edit_btn";
            btnJournalTag.ImgOff = "mtgl_edit_btn";
            btnJournalTag.X = 4 * gv.uiSquareSize;
            btnJournalTag.Y = 0 * gv.uiSquareSize + (3 * gv.uiSquareSize / 4);
            btnJournalTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnJournalTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnEntryName == null)
            {
                btnEntryName = new IbbToggle(gv);
            }
            btnEntryName.ImgOn = "mtgl_edit_btn";
            btnEntryName.ImgOff = "mtgl_edit_btn";
            btnEntryName.X = 6 * gv.uiSquareSize + gv.uiSquareSize / 2;
            btnEntryName.Y = 1 * gv.uiSquareSize + (2 * gv.uiSquareSize / 4);
            btnEntryName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnEntryName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnEntryTag == null)
            {
                btnEntryTag = new IbbToggle(gv);
            }
            btnEntryTag.ImgOn = "mtgl_edit_btn";
            btnEntryTag.ImgOff = "mtgl_edit_btn";
            btnEntryTag.X = 6 * gv.uiSquareSize + gv.uiSquareSize / 2;
            btnEntryTag.Y = 1 * gv.uiSquareSize + (4 * gv.uiSquareSize / 4);
            btnEntryTag.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnEntryTag.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnEntryID == null)
            {
                btnEntryID = new IbbToggle(gv);
            }
            btnEntryID.ImgOn = "mtgl_edit_btn";
            btnEntryID.ImgOff = "mtgl_edit_btn";
            btnEntryID.X = 6 * gv.uiSquareSize + gv.uiSquareSize / 2;
            btnEntryID.Y = 2 * gv.uiSquareSize + (2 * gv.uiSquareSize / 4);
            btnEntryID.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnEntryID.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnEntryText == null)
            {
                btnEntryText = new IbbToggle(gv);
            }
            btnEntryText.ImgOn = "mtgl_edit_btn";
            btnEntryText.ImgOff = "mtgl_edit_btn";
            btnEntryText.X = 6 * gv.uiSquareSize + gv.uiSquareSize / 2;
            btnEntryText.Y = 2 * gv.uiSquareSize + (4 * gv.uiSquareSize / 4);
            btnEntryText.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            btnEntryText.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnAddJournal == null)
            {
                btnAddJournal = new IbbButton(gv, 0.8f);
            }
            //btnAddJournal.Text = "ADD";
            btnAddJournal.Img = "btn_small";
            btnAddJournal.Img2 = "btnadd";
            btnAddJournal.Glow = "btn_small_glow";
            btnAddJournal.X = 0 * gv.uiSquareSize + gv.fontWidth;
            btnAddJournal.Y = 3 * gv.fontHeight;
            btnAddJournal.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddJournal.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveJournal == null)
            {
                btnRemoveJournal = new IbbButton(gv, 0.8f);
            }
            //btnRemoveJournal.Text = "REMOVE";
            btnRemoveJournal.Img = "btn_small";
            btnRemoveJournal.Img2 = "btnremove";
            btnRemoveJournal.Glow = "btn_small_glow";
            btnRemoveJournal.X = 1 * gv.uiSquareSize + gv.fontWidth;
            btnRemoveJournal.Y = 3 * gv.fontHeight;
            btnRemoveJournal.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveJournal.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnAddEntry == null)
            {
                btnAddEntry = new IbbButton(gv, 0.8f);
            }
            //btnAddEntry.Text = "ADD";
            btnAddEntry.Img = "btn_small";
            btnAddEntry.Img2 = "btnadd";
            btnAddEntry.Glow = "btn_small_glow";
            btnAddEntry.X = 4 * gv.uiSquareSize;
            btnAddEntry.Y = 1 * gv.uiSquareSize + 3 * gv.fontHeight;
            btnAddEntry.Height = (int)(gv.ibbheight * gv.scaler);
            btnAddEntry.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRemoveEntry == null)
            {
                btnRemoveEntry = new IbbButton(gv, 0.8f);
            }
            //btnRemoveEntry.Text = "REMOVE";
            btnRemoveEntry.Img = "btn_small";
            btnRemoveEntry.Img2 = "btnremove";
            btnRemoveEntry.Glow = "btn_small_glow";
            btnRemoveEntry.X = 5 * gv.uiSquareSize;
            btnRemoveEntry.Y = 1 * gv.uiSquareSize + 3 * gv.fontHeight;
            btnRemoveEntry.Height = (int)(gv.ibbheight * gv.scaler);
            btnRemoveEntry.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnHelp == null)
            {
                btnHelp = new IbbButton(gv, 0.8f);
            }
            //btnHelp.Text = "HELP";
            btnHelp.Img = "btn_small"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
            btnHelp.Img2 = "btnhelp";
            btnHelp.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
            btnHelp.X = 10 * gv.uiSquareSize;
            btnHelp.Y = (int)(6 * gv.uiSquareSize + gv.scaler);
            btnHelp.Height = (int)(gv.ibbheight * gv.scaler);
            btnHelp.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsJournalEditor(SKCanvas c)
        {
            setControlsStart();
            int shiftForFont = (btnJournalTag.Height / 2) - (gv.fontHeight / 2);
            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            //Page Title
            gv.DrawText(c, "JOURNAL EDITOR", center - (9 * gv.fontWidth), 2 * gv.scaler, "yl");

            //label for Containers in module      
            gv.DrawText(c, "JOURNAL CATEGORIES", btnAddJournal.X, btnAddJournal.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
            //on the left draw the add and remove buttons
            btnAddJournal.Draw(c);
            btnRemoveJournal.Draw(c);
            //list all containers (tap on a container in the list to show elements for editing)
            int startX = 1 * gv.fontWidth;
            int startY = btnAddJournal.Y + btnAddJournal.Height - gv.fontHeight;
            int incY = gv.fontHeight + gv.fontLineSpacing;
            int cnt = 0;
            foreach (JournalQuest c2 in gv.mod.moduleJournal)
            {
                if (cnt == journalListIndex)
                {
                    gv.DrawText(c, c2.Tag, startX, startY += incY, "gn");
                }
                else
                {
                    gv.DrawText(c, c2.Tag, startX, startY += incY, "wh");
                }
                cnt++;
            }

            if (gv.mod.moduleJournal.Count > 0)
            {
                btnJournalTag.Draw(c);
                gv.DrawText(c, "JOURNAL TAG: " + gv.mod.moduleJournal[journalListIndex].Tag, btnJournalTag.X + btnJournalTag.Width + gv.scaler, btnJournalTag.Y + shiftForFont, "wh");
                btnJournalName.Draw(c);
                gv.DrawText(c, "JOURNAL NAME: " + gv.mod.moduleJournal[journalListIndex].Name, btnJournalName.X + btnJournalName.Width + gv.scaler, btnJournalName.Y + shiftForFont, "wh");

                //label for item list
                gv.DrawText(c, "JOURNAL ENTRIES", btnAddEntry.X, btnAddEntry.Y - gv.fontHeight - gv.fontLineSpacing, "yl");
                //on the left draw the add and remove buttons
                btnAddEntry.Draw(c);
                btnRemoveEntry.Draw(c);
                //list all containers (tap on a container in the list to show elements for editing)
                startX = btnAddEntry.X;
                startY = btnAddEntry.Y + btnAddEntry.Height - gv.fontHeight;
                incY = gv.fontHeight + gv.fontLineSpacing;
                cnt = 0;
                int columncount = 0;
                int longestline = 0;
                foreach (JournalEntry itr in gv.mod.moduleJournal[journalListIndex].Entries)
                {
                    if (itr.Tag.Length > longestline)
                    {
                        longestline = itr.Tag.Length;
                    }
                    //need to check if should start new column after list hits end of screen
                    if (cnt == entryListIndex)
                    {
                        gv.DrawText(c, itr.Tag, startX, startY += incY, "gn");
                    }
                    else
                    {
                        gv.DrawText(c, itr.Tag, startX, startY += incY, "wh");
                    }
                    cnt++;
                    columncount++;
                    if (columncount > 19)
                    {
                        columncount = 0;
                        startY = btnAddEntry.Y + btnAddEntry.Height - gv.fontHeight;
                        startX = btnAddEntry.X + (longestline + 1) * gv.fontWidth;
                    }
                }

                if (gv.mod.moduleJournal[journalListIndex].Entries.Count > 0)
                {
                    btnEntryName.Draw(c);
                    gv.DrawText(c, "NAME: " + gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].EntryTitle, btnEntryName.X + btnEntryName.Width + gv.scaler, btnEntryName.Y + shiftForFont, "wh");
                    btnEntryTag.Draw(c);
                    gv.DrawText(c, "TAG: " + gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].Tag, btnEntryTag.X + btnEntryTag.Width + gv.scaler, btnEntryTag.Y + shiftForFont, "wh");
                    btnEntryID.Draw(c);
                    gv.DrawText(c, "ID: " + gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].EntryId, btnEntryID.X + btnEntryID.Width + gv.scaler, btnEntryID.Y + shiftForFont, "wh");
                    btnEntryText.Draw(c);
                    gv.DrawText(c, "TEXT: " + gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].EntryText, btnEntryText.X + btnEntryText.Width + gv.scaler, btnEntryText.Y + shiftForFont, "wh");

                    //Entry Text
                    description.tbXloc = 1 * gv.uiSquareSize;
                    description.tbYloc = 5 * gv.uiSquareSize;
                    description.tbWidth = 10 * gv.uiSquareSize;
                    description.tbHeight = 10 * gv.uiSquareSize;
                    string textToSpan = "";
                    textToSpan = "<yl>ENTRY TEXT:</yl>" + Environment.NewLine;
                    textToSpan += gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].EntryText;
                    description.linesList.Clear();
                    description.AddFormattedTextToTextBox(textToSpan);
                    description.onDrawTextBox(c);
                }
            }

            btnHelp.Draw(c);

            gv.tsMainMenu.redrawTsMainMenu(c);

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox(c);
            }
        }
        public void onTouchTsJournalEditor(int eX, int eY, MouseEventType.EventType eventType)
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
                    
                    //figure out if tapped on a container list
                    if (x < btnAddEntry.X)
                    {
                        int containerPanelTopLocation = btnAddJournal.Y + btnAddJournal.Height - gv.fontHeight;
                        int lineIndex = (y - containerPanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing) - 1;
                        if ((lineIndex < gv.mod.moduleJournal.Count) && (lineIndex >= 0))
                        {
                            journalListIndex = lineIndex;
                            entryListIndex = 0;
                        }
                    }
                    else //tapped in item list
                    {
                        if (gv.mod.moduleJournal.Count > 0)
                        {
                            if ((x > btnAddEntry.X) && (x < btnEntryName.X))
                            {
                                //left column clicked 
                                int itemPanelTopLocation = btnAddEntry.Y + btnAddEntry.Height - gv.fontHeight;
                                int lineIndex = (y - itemPanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing) - 1;
                                if ((lineIndex < gv.mod.moduleJournal[journalListIndex].Entries.Count) && (lineIndex >= 0))
                                {
                                    entryListIndex = lineIndex;
                                }
                            }
                        }
                    }                   
                    
                    if (btnAddJournal.getImpact(x, y))
                    {
                        JournalQuest newCategory = new JournalQuest();
                        newCategory.Tag = "newCategoryTag";
                        newCategory.Name = "newCategoryName";
                        gv.mod.moduleJournal.Add(newCategory);
                    }
                    else if (btnRemoveJournal.getImpact(x, y))
                    {
                        if (gv.mod.moduleJournal.Count > 0)
                        {
                            try
                            {
                                gv.mod.moduleJournal.RemoveAt(journalListIndex);
                                journalListIndex = 0;
                            }
                            catch { }
                        }
                    }
                    else if (btnHelp.getImpact(x, y))
                    {
                        //incrementalSaveModule();
                    }

                    if (gv.mod.moduleJournal.Count > 0)
                    {
                        if (btnJournalTag.getImpact(x, y))
                        {
                            changeJournalTag();
                        }
                        else if (btnJournalName.getImpact(x, y))
                        {
                            changeJournalName();
                        }
                        else if (btnAddEntry.getImpact(x, y))
                        {
                            if (gv.mod.moduleJournal.Count > 0)
                            {
                                //bring up selection list and choose an item
                                JournalEntry newChild = new JournalEntry();
                                newChild.Tag = "entryTag";
                                gv.mod.moduleJournal[journalListIndex].Entries.Add(newChild);
                            }
                        }
                        else if (btnRemoveEntry.getImpact(x, y))
                        {
                            if (gv.mod.moduleJournal.Count > 0)
                            {
                                try
                                {
                                    if (gv.mod.moduleJournal[journalListIndex].Entries.Count > 0)
                                    {
                                        gv.mod.moduleJournal[journalListIndex].Entries.RemoveAt(entryListIndex);
                                        entryListIndex = 0;
                                    }
                                }
                                catch { }
                            }
                        }
                        if (gv.mod.moduleJournal[journalListIndex].Entries.Count > 0)
                        {
                            if (btnEntryName.getImpact(x, y))
                            {
                                changeEntryName();
                            }
                            else if (btnEntryTag.getImpact(x, y))
                            {
                                changeEntryTag();
                            }
                            else if (btnEntryID.getImpact(x, y))
                            {
                                changeEntryID();
                            }
                            else if (btnEntryText.getImpact(x, y))
                            {
                                changeEntryText();
                            }
                        }
                    }

                    break;
            }
        }
        public async void changeJournalTag()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose a unique tag for this Journal Category.", gv.mod.moduleJournal[journalListIndex].Tag);
            gv.mod.moduleJournal[journalListIndex].Tag = myinput;
            gv.touchEnabled = true;
        }
        public async void changeJournalName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose a name for this Journal Category.", gv.mod.moduleJournal[journalListIndex].Name);
            gv.mod.moduleJournal[journalListIndex].Name = myinput;
            gv.touchEnabled = true;
        }
        public async void changeEntryName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose the name for this Journal Entry.", gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].EntryTitle);
            gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].EntryTitle = myinput;
            gv.touchEnabled = true;
        }
        public async void changeEntryTag()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Choose a unique tag for this Journal Entry.", gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].Tag);
            gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].Tag = myinput;
            gv.touchEnabled = true;
        }
        public async void changeEntryID()
        {
            gv.touchEnabled = false;
            int myinput = await gv.NumInputBox("Choose an ID for this Journal Entry", gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].EntryId);
            gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].EntryId = myinput;
            gv.touchEnabled = true;            
        }
        public async void changeEntryText()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Enter the text for this Journal Entry.", gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].EntryText);
            gv.mod.moduleJournal[journalListIndex].Entries[entryListIndex].EntryText = myinput;
            gv.touchEnabled = true;
        }
    }
}
