﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ScreenConvo 
    {
	    //public Module gv.mod;
	    public GameView gv;
	
	    public List<IbbButton> btnPartyIndex = new List<IbbButton>();
	
	    //Convo STUFF
	    //public Convo currentConvo = new Convo();
	    public string currentNpcNode = "";
	    public string currentPcNode = "";
	    public List<string> currentPcNodeList = new List<string>();
	    public List<IbRect> currentPcNodeRectList = new List<IbRect>();
	    public int pcNodeGlow = -1;
	    public int npcNodeEndY = 0;
	    public int originalSelectedPartyLeader = 0;
	    public int parentIdNum = 0;    
        public SkiaSharp.SKBitmap convoBitmap;
        private bool doActions = true;
        public List<int> nodeIndexList = new List<int>();
        private IBminiTextBox htmltext;

        public ScreenConvo(Module m, GameView g)
	    {
		    //gv.mod = m;
		    gv = g;
		    setControlsStart();
	    }

	    public void setControlsStart()
	    {		
    	    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		    int padW = gv.squareSize/6;

            htmltext = new IBminiTextBox(gv, 320, 100, 500, 300);
            htmltext.showBoxBorder = false;

		    for (int x = 0; x < 6; x++)
		    {
			    IbbButton btnNew = new IbbButton(gv, 1.0f);	
			    btnNew.Img = "item_slot"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.item_slot);
			    btnNew.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnNew.X = ((x+2) * gv.uiSquareSize) + (padW * (x+1));
			    btnNew.Y = 6 * gv.uiSquareSize - (pH * 2);
                btnNew.Height = (int)(gv.ibbheight * gv.scaler);
                btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);	
			
			    btnPartyIndex.Add(btnNew);
		    }
	    }
	
	    //CONVO SCREEN
	    public void redrawConvo()
        {
            drawPortrait();
		    drawNpcNode();
		    drawPcNode();	 
		
		    if (gv.mod.currentConvo.PartyChat)
		    {
			    //DRAW EACH PC BUTTON
			    int cntPCs = 0;
			    foreach (IbbButton btn in btnPartyIndex)
			    {
				    if (cntPCs < gv.mod.playerList.Count)
				    {
					    if (cntPCs == gv.mod.selectedPartyLeader) {btn.glowOn = true;}
					    else {btn.glowOn = false;}					
					    btn.Draw();
				    }
				    cntPCs++;
			    }
		    }
        }
	    public void drawPortrait()
	    {
            int pH = (int)((float)gv.screenHeight / 100.0f);
		    int sX = gv.uiSquareSize * 0 + gv.uiSquareSize / 4;
		    int sY = pH * 4;
            IbRect src = new IbRect(0, 0, convoBitmap.Width, convoBitmap.Height);
            IbRect dst = new IbRect(sX, sY, (int)(gv.uiSquareSize * 1.8), (int)(gv.uiSquareSize * 2.8));

            if (convoBitmap.Width == convoBitmap.Height)
            {
                dst = new IbRect(sX, sY, (int)(gv.uiSquareSize * 1.8), (int)(gv.uiSquareSize * 1.8));
            }
		    if (gv.mod.currentConvo.Narration)
            {
                if (!gv.mod.currentConvo.NpcPortraitBitmap.Equals("")) //Narration with image
                {
                    dst = new IbRect((gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (gv.uiSquareSize * 2), gv.uiSquareSize / 4, gv.uiSquareSize * 4, gv.uiSquareSize * 2);
                }
                else //Narration without image
                {
                    //do narration without image setup                                      
                }            
            }
		    if (convoBitmap != null)
		    {
			    gv.DrawBitmap(convoBitmap, src, dst);
                if ((gv.mod.useUIBackground) && (!gv.mod.currentConvo.Narration))
                {
                    IbRect srcFrame = new IbRect(0, 0, gv.cc.ui_portrait_frame.Width, gv.cc.ui_portrait_frame.Height);
                    IbRect dstFrame = new IbRect(dst.Left - (int)(2 * gv.scaler),
                                            dst.Top - (int)(2 * gv.scaler),
                                            (int)((float)dst.Width) + (int)(4 * gv.scaler),
                                            (int)((float)dst.Height) + (int)(4 * gv.scaler));
                    gv.DrawBitmap(gv.cc.ui_portrait_frame, srcFrame, dstFrame);
                }
            }
	    }
	    public void drawNpcNode()
	    {
            int pW = (int)((float)gv.screenWidth / 100.0f);
            int pH = (int)((float)gv.screenHeight / 100.0f);
            int startX = gv.uiSquareSize * 2 + (pW * 3);
            int startY = pH * 4;
            int width = gv.uiSquareSize * gv.uiSquaresInWidth - startX;

            if (gv.mod.currentConvo.Narration)
            {
                if (!gv.mod.currentConvo.NpcPortraitBitmap.Equals("")) //Narration with image
                {
                    //do narration with image setup
                    startX = (int)(gv.uiSquareSize * 0.5);
                    startY = (int)(gv.uiSquareSize * 2.5);
                    width = gv.uiSquareSize * gv.uiSquaresInWidth - (startX);
                }
                else //Narration without image
                {
                    //do narration without image setup                                      
                }            
            }
            //Node Rectangle Text
            string textToSpan = "";
            textToSpan = currentNpcNode;            
            
            htmltext.tbXloc = startX;
            htmltext.tbYloc = startY;
            htmltext.tbWidth = width;
            htmltext.tbHeight = pH * 50;
            htmltext.linesList.Clear();
            htmltext.AddFormattedTextToTextBox(textToSpan);
            htmltext.onDrawTextBox();
            int totalHeight = htmltext.linesList.Count * (gv.fontHeight + gv.fontLineSpacing);
            /*foreach (FormattedLine fl in htmltext.logLinesList)
            {
                totalHeight += fl.lineHeight;
            }*/
            npcNodeEndY = startY + totalHeight;
	    }
	    public void drawPcNode()
	    {
		    currentPcNodeRectList.Clear();

            int pH = (int)((float)gv.screenHeight / 100.0f);
		    int pad = (int)((float)gv.screenHeight / 100.0f);
		    int startX = gv.uiSquareSize * 0 + gv.uiSquareSize / 4;
            int sY = (int)((float)gv.screenHeight / 100.0f) * 4;
            int startY = (int)(gv.uiSquareSize * 3) + (pH * 5);
            int width = gv.uiSquareSize * gv.uiSquaresInWidth - (startX);

            if (gv.mod.currentConvo.Narration)
            {
                if (!gv.mod.currentConvo.NpcPortraitBitmap.Equals("")) //Narration with image
                {
                    //do narration with image setup
                    startY = (int)(gv.uiSquareSize * 3) + (pH * 5);
                }
                else //Narration without image
                {
                    //do narration without image setup... different startY value                                      
                }
            }
		    if (startY <= npcNodeEndY)
		    {
			    startY = npcNodeEndY + (pad * 4);
		    }

            int cnt = 1;
            foreach (string txt in currentPcNodeList)
            {
                string textToSpan = txt;
                if (pcNodeGlow == cnt)
                {
                    textToSpan = "<rd>" + txt + "</rd>";
                }
                else
                {
                    textToSpan = "<wh>" + txt + "</wh>";
                }
                
                htmltext.tbXloc = startX;
                htmltext.tbYloc = startY;
                htmltext.tbWidth = width;
                htmltext.tbHeight = pH * 50;
                htmltext.linesList.Clear();
                htmltext.AddFormattedTextToTextBox(textToSpan);
                htmltext.onDrawTextBox();

                int totalHeight = htmltext.linesList.Count * (gv.fontHeight + gv.fontLineSpacing);
                //float totalHeight = 0;
                int totalWidth = htmltext.tbWidth;
                /*foreach (FormattedLine fl in htmltext.logLinesList)
                {
                    totalHeight += fl.lineHeight;
                }*/
                currentPcNodeRectList.Add(new IbRect(startX, startY, totalWidth, totalHeight));

                startY += totalHeight + pad;
                cnt++;
            }
	    }

	    public void onTouchConvo(int eX, int eY, MouseEventType.EventType eventType)
	    {
		    pcNodeGlow = -1;
		
		    switch (eventType)
		    {
		    case MouseEventType.EventType.MouseDown:
		    case MouseEventType.EventType.MouseMove:
			    int x = (int) eX;
			    int y = (int) eY;
				
			    int cnt = 1;
			    foreach (IbRect r in currentPcNodeRectList)
			    {
				    if ((x >= r.Left) && (x <= r.Left + r.Width))
				    {
					    if ((y >= r.Top) && (y <= r.Top + r.Height))
					    {
						    pcNodeGlow = cnt;
					    }
				    }
				    cnt++;
			    }
											
			    break;
			
		    case MouseEventType.EventType.MouseUp:
			    x = (int) eX;
			    y = (int) eY;
				
			    pcNodeGlow = -1;
			
			    cnt = 0;
			    foreach (IbRect r in currentPcNodeRectList)
			    {
				    if ((x >= r.Left) && (x <= r.Left + r.Width))
				    {
					    if ((y >= r.Top) && (y <= r.Top + r.Height))
					    {
						    selectedLine(cnt);
					    }
				    }
				    cnt++;
			    }
			
			    if (gv.mod.currentConvo.PartyChat)
			    {
				    for (int j = 0; j < gv.mod.playerList.Count; j++)
				    {
					    if (btnPartyIndex[j].getImpact(x, y))
					    {
						    gv.mod.selectedPartyLeader = j;
						    doActions = false;
			                doConvo(parentIdNum);
					    }
				    }
			    }
											
			    break;	
		    }
	    }

        /*
        public void onKeyUp(Keys KeyCode)
        {
            if (((KeyCode == Keys.D1) || (KeyCode == Keys.NumPad1)) && (1 <= nodeIndexList.Count))
            {
                selectedLine(0);
            }
            else if (((KeyCode == Keys.D2) || (KeyCode == Keys.NumPad2)) && (2 <= nodeIndexList.Count))
            {
                selectedLine(1);
            }
            else if (((KeyCode == Keys.D3) || (KeyCode == Keys.NumPad3)) && (3 <= nodeIndexList.Count))
            {
                selectedLine(2);
            }
            else if (((KeyCode == Keys.D4) || (KeyCode == Keys.NumPad4)) && (4 <= nodeIndexList.Count))
            {
                selectedLine(3);
            }
            else if (((KeyCode == Keys.D5) || (KeyCode == Keys.NumPad5)) && (5 <= nodeIndexList.Count))
            {
                selectedLine(4);
            }
            else if (((KeyCode == Keys.D6) || (KeyCode == Keys.NumPad6)) && (6 <= nodeIndexList.Count))
            {
                selectedLine(5);
            }
            else if (((KeyCode == Keys.D7) || (KeyCode == Keys.NumPad7)) && (7 <= nodeIndexList.Count))
            {
                selectedLine(6);
            }
            else if (((KeyCode == Keys.D8) || (KeyCode == Keys.NumPad8)) && (8 <= nodeIndexList.Count))
            {
                selectedLine(7);
            }
            else if (((KeyCode == Keys.D9) || (KeyCode == Keys.NumPad9)) && (9 <= nodeIndexList.Count))
            {
                selectedLine(8);
            }            
        }
        */

	    //methods
	    public void startConvo()
        {
            if (gv.mod.currentConvo.SpeakToMainPcOnly)
		    {
                int x = 0;
                foreach (Player pc in gv.mod.playerList)
                {
                    if (pc.mainPc)
                    {
                        gv.mod.selectedPartyLeader = x;
                    }
                    x++;
                }
		    }
            if (gv.mod.playerList[gv.mod.selectedPartyLeader].isDead())
            {
                gv.cc.SwitchToNextAvailablePartyLeader();
            }
        
            //load all the current player token images to be used in party chat system
            int cntPCs = 0;
		    foreach (IbbButton btn in btnPartyIndex)
		    {
			    if (cntPCs < gv.mod.playerList.Count)
			    {
                    //gv.cc.DisposeOfBitmap(ref btn.Img2);
                    btn.Img2 = gv.mod.playerList[cntPCs].tokenFilename;						
			    }
			    cntPCs++;
		    }
		    //Remember who the party leader is so that when convo is over we can revert back to them
            originalSelectedPartyLeader = gv.mod.selectedPartyLeader;
            parentIdNum = 0;
		    SetNodeIsActiveFalseForAll();
            parentIdNum = getParentIdNum(parentIdNum);  
                
            if (gv.mod.currentConvo.Narration)
            {
                if (!gv.mod.currentConvo.NpcPortraitBitmap.Equals("")) //Narration with image
                {
                    //do narration with image setup
                }
                else //Narration without image
                {
                    //do narration without image setup                                      
                }            
            }
            // load image for convo
            loadNodePortrait();              
            doActions = true;            
            doConvo(parentIdNum); // load up the text for the NPC node and all PC responses
        }
	    private void loadNodePortrait()
        {		
            // load image for convo
            try
            {
                if ((gv.mod.currentConvo.GetContentNodeById(parentIdNum).NodePortraitBitmap.Equals("")) 
                    || (gv.mod.currentConvo.GetContentNodeById(parentIdNum).NodePortraitBitmap == null))
                {
                    if (gv.mod.currentConvo.NpcPortraitBitmap.Equals(""))
                    {
                        convoBitmap = gv.cc.GetFromBitmapList("npc_blob_portrait");
                    }
                    else
                    {
                        string filename = gv.mod.currentConvo.NpcPortraitBitmap;
                        string filenameNoExt = filename;
                        if (filename.Contains("."))
                        {
                            int lastPeriodPos = filename.LastIndexOf('.');
                            filenameNoExt = filename.Substring(0, lastPeriodPos);
                        }
                        //gv.cc.DisposeOfBitmap(ref convoBitmap);
                        //convoBitmap = gv.cc.LoadBitmap(filenameNoExt);
                        //if (convoBitmap == null)
                        //{
                        //    gv.cc.DisposeOfBitmap(ref convoBitmap);
                        //    convoBitmap = gv.cc.LoadBitmap("npc_blob_portrait");
                        //}
                        convoBitmap = gv.cc.GetFromBitmapList(filenameNoExt);
                    }
                }
                else
                {
                    string filename = gv.mod.currentConvo.GetContentNodeById(parentIdNum).NodePortraitBitmap;
                    string filenameNoExt = filename;
                    if (filename.Contains("."))
                    {
                        int lastPeriodPos = filename.LastIndexOf('.');
                        filenameNoExt = filename.Substring(0, lastPeriodPos);
                    }
                    //gv.cc.DisposeOfBitmap(ref convoBitmap);
                    //convoBitmap = gv.cc.LoadBitmap(filenameNoExt);
                    //if (convoBitmap == null)
                    //{
                    //    gv.cc.DisposeOfBitmap(ref convoBitmap);
                    //    convoBitmap = gv.cc.LoadBitmap("npc_blob_portrait");
                    //}
                    convoBitmap = gv.cc.GetFromBitmapList(filenameNoExt);
                }
            }
            catch (Exception ex)
            {
                //gv.cc.DisposeOfBitmap(ref convoBitmap);
                //convoBitmap = gv.cc.LoadBitmap("npc_blob_portrait");
                convoBitmap = gv.cc.GetFromBitmapList("npc_blob_portrait");
                gv.errorLog(ex.ToString());
            }
        }
	    private void SetNodeIsActiveFalseForAll()
        {
            foreach (ConvoSavedValues csv in gv.mod.moduleConvoSavedValuesList)
            {
                if (csv.ConvoFileName.Equals(gv.mod.currentConvo.ConvoFileName))
                {
            	    gv.mod.currentConvo.GetContentNodeById(csv.NodeNotActiveIdNum).NodeIsActive = false;
                }
            }
        }
	
	    private bool nodePassesConditional(ContentNode pnode)
	    {
		    //check to see if passes conditional
            bool check = true;
            ContentNode chkNode = new ContentNode();
            chkNode = pnode;
            // cycle through the conditions of each node and check for true
            // if one node is a link, then go to the linked node and check its conditional
            if (pnode.isLink)
            {
                chkNode = gv.mod.currentConvo.GetContentNodeById(pnode.linkTo);
            }
            bool AndStatments = true;
            foreach (Condition conditional in chkNode.conditions)
            {
                if (!conditional.c_and)
                {
                    AndStatments = false;
                    break;
                }
            }
            foreach (Condition conditional in chkNode.conditions)
            {
        	    gv.cc.doScriptBasedOnFilename(conditional.c_script, conditional.c_parameter_1, conditional.c_parameter_2, conditional.c_parameter_3, conditional.c_parameter_4);
                if (AndStatments) //this is an "and" set
                {
                    if ((gv.mod.returnCheck == false) && (conditional.c_not == false))
                    {
                        check = false;
                    }
                    if ((gv.mod.returnCheck == true) && (conditional.c_not == true))
                    {
                        check = false;
                    }
                }
                else //this is an "or" set
                {
                    if ((gv.mod.returnCheck == false) && (conditional.c_not == false))
                    {
                        check = false;
                    }
                    else if ((gv.mod.returnCheck == true) && (conditional.c_not == true))
                    {
                        check = false;
                    }
                    else //in "or" statement, if find one true then done
                    {
                        check = true;
                        break;
                    }
                }
            }
            return check;
	    }
		
	    private int getParentIdNum(int childIdNum) // Gets the first NPC node idNum that returns a true conditional
        {
            //first determine which NPC subNode to use by cycling through all children of parentNode until one returns true from conditionals
            foreach (ContentNode npcNode in gv.mod.currentConvo.GetContentNodeById(childIdNum).subNodes)
            {            
                bool check = nodePassesConditional(npcNode);
                if ((check == true) && (npcNode.NodeIsActive))
                {
                    if (npcNode.ShowOnlyOnce)
                    {
                	    npcNode.NodeIsActive = false;
                        saveNodeIsActiveFalseToModule(npcNode);
                    }
                    return npcNode.idNum;
                }
            }
            if (gv.mod.currentConvo.GetContentNodeById(childIdNum).subNodes[0].ShowOnlyOnce)
            {
        	    gv.mod.currentConvo.GetContentNodeById(childIdNum).subNodes[0].NodeIsActive = false;
                saveNodeIsActiveFalseToModule(gv.mod.currentConvo.GetContentNodeById(childIdNum).subNodes[0]);
            }
            return gv.mod.currentConvo.GetContentNodeById(childIdNum).subNodes[0].idNum;
        }
	    private void saveNodeIsActiveFalseToModule(ContentNode nod)
        {
            ConvoSavedValues newCSV = new ConvoSavedValues();
            newCSV.ConvoFileName = gv.mod.currentConvo.ConvoFileName;
            newCSV.NodeNotActiveIdNum = nod.idNum;
            gv.mod.moduleConvoSavedValuesList.Add(newCSV);
        }
        private void doConvo(int prntIdNum) // load up the text for the NPC node and all PC responses
        {
            string selectedPcOptions = "";
            string comparePcOptions = "";
            currentNpcNode = "";
            currentPcNodeList.Clear();        
            nodeIndexList.Clear();
        
            //NPC NODE STUFF
            //if the NPC node is a link, move to the actual node
            if (gv.mod.currentConvo.GetContentNodeById(prntIdNum).isLink)
            {
        	    parentIdNum = gv.mod.currentConvo.GetContentNodeById(prntIdNum).linkTo;
        	    prntIdNum = gv.mod.currentConvo.GetContentNodeById(prntIdNum).linkTo;
            }
        
            if (doActions)
            {
                foreach (Action action in gv.mod.currentConvo.GetContentNodeById(prntIdNum).actions)
                {
            	    gv.cc.doScriptBasedOnFilename(action.a_script, action.a_parameter_1, action.a_parameter_2, action.a_parameter_3, action.a_parameter_4);
                }
            }
            currentNpcNode = replaceText(gv.mod.currentConvo.GetContentNodeById(prntIdNum).conversationText);
        
            //PC NODE STUFF
            //Loop through all PC nodes and check to see if they should be visible
            int cnt = 0; 
            int trueCount = 1;
            foreach (ContentNode pcNode in gv.mod.currentConvo.GetContentNodeById(prntIdNum).subNodes)
            {        	
        	    bool check = nodePassesConditional(pcNode);
                if (check == true)
                {            	
                    selectedPcOptions += cnt + "";
                    nodeIndexList.Add(cnt);
            	    string pcNodeText = replaceText(pcNode.conversationText);
            	    currentPcNodeList.Add("<gn>" + trueCount + ") "  + "</gn>" + pcNodeText);
            	    trueCount++;
                }
                cnt++;
            }

            //PARTY CHAT SYSTEM other PCs NODE STUFF
            //Iterate through all other PCs and see what node options they have and indicate if different
            int PcIndx = 0;
            int originalPartyLeader = gv.mod.selectedPartyLeader; //remember who was the currently selected PC to check against for diffs
        
            //set all Img3 bitmaps to null to turn off convo plus bubble tag
            int cntPCs = 0;
		    foreach (IbbButton btn in btnPartyIndex)
		    {
			    if (cntPCs < gv.mod.playerList.Count)
			    {
				    btn.Img3 = null;						
			    }
			    cntPCs++;
		    }
		
            foreach (Player pc in gv.mod.playerList)
            {
                comparePcOptions = "";
                gv.mod.selectedPartyLeader = PcIndx;
                if (PcIndx != originalPartyLeader)
                {
                    //loop through all nodes and check to see if they should be visible
                    int cntr = 0;
                    foreach (ContentNode pcNode in gv.mod.currentConvo.GetContentNodeById(prntIdNum).subNodes)
                    {                	
                	    bool check = nodePassesConditional(pcNode);
                        if (check == true)
                        {
                            comparePcOptions += cntr + "";
                        }
                        cntr++;
                    }
                    //compare this PC to the selectedPartyLeader's options
                    if (comparePcOptions.Equals(selectedPcOptions))
                    {
                	    //no new options for this PC so no plus bubble marker 
                        btnPartyIndex[PcIndx].btnNotificationOn = false;
                	    btnPartyIndex[PcIndx].Img3 = null;
                    } 
                    else //new options available so show bubble plus marker
                    {
                        btnPartyIndex[PcIndx].btnNotificationOn = true;
                        //gv.cc.DisposeOfBitmap(ref btnPartyIndex[PcIndx].Img3);
                        btnPartyIndex[PcIndx].Img3 = "convoplus";
                    }
                }
                PcIndx++;
            }
        
            //return back to original selected PC after making checks for different node options available
            gv.mod.selectedPartyLeader = originalPartyLeader;

            //load node portrait and play node sound
            loadNodePortrait();
        }
        private void selectedLine(int btnIndex)
        {    	                 
            if (btnIndex < nodeIndexList.Count)
            {
        	    int index = nodeIndexList[btnIndex];
                string NPCname = "";
	            ContentNode selectedNod = gv.mod.currentConvo.GetContentNodeById(parentIdNum).subNodes[index];
	            if ((selectedNod.NodeNpcName.Equals("")) || (selectedNod.NodeNpcName == null) || (selectedNod.NodeNpcName.Length <= 0))
	            {
	                NPCname = gv.mod.currentConvo.DefaultNpcName;
	            }
	            else
	            {
	                NPCname = selectedNod.NodeNpcName;
	            }
	            string npcNode = replaceText(gv.mod.currentConvo.GetContentNodeById(parentIdNum).conversationText);
	            string pcNode = replaceText(selectedNod.conversationText);
	            //write to log
                gv.cc.addLogText("<yl>" + NPCname + ": </yl>" +
                                 "<gy>" + npcNode + "<br>" + "</gy>" +
                                 "<bu>" + gv.mod.playerList[gv.mod.selectedPartyLeader].name + ": </bu>" +
                                 "<gy>" + pcNode + "</gy>");
	
	            int childIdNum = gv.mod.currentConvo.GetContentNodeById(parentIdNum).subNodes[index].idNum;
	            // if PC node choosen was a linked node, then return the idNum of the linked node
	            if (gv.mod.currentConvo.GetContentNodeById(parentIdNum).subNodes[index].isLink)
	            {
	                childIdNum = gv.mod.currentConvo.GetContentNodeById(gv.mod.currentConvo.GetContentNodeById(parentIdNum).subNodes[index].linkTo).idNum;
	            }
	            //doAction() for current selected PC Node (all actions for node)
	            foreach (Action action in gv.mod.currentConvo.GetContentNodeById(childIdNum).actions)
	            {
	        	    gv.cc.doScriptBasedOnFilename(action.a_script, action.a_parameter_1, action.a_parameter_2, action.a_parameter_3, action.a_parameter_4);
	            }
	            if (gv.mod.currentConvo.GetContentNodeById(childIdNum).subNodes.Count < 1)
	            {
                    gv.cc.addLogText("[end convo]<br><br>"); //add a blank line to main screen log at the end of a conversation
	                gv.mod.selectedPartyLeader = originalSelectedPartyLeader;
	        	    if ((gv.screenType.Equals("shop")) || (gv.screenType.Equals("title")) || (gv.screenType.Equals("combat")))
	        	    {
	        		    //leave as shop and launch shop screen
	        	    }
	        	    else
	        	    {
	        		    gv.screenType = "main";
	        		    if (gv.cc.calledConvoFromProp)
	        		    {
                            //gv.mod.isRecursiveDoTriggerCallMovingProp = true;
                            //gv.mod.blockTriggerMovingProp = true;
                            //gv.mod.isRecursiveCall = true;
                            //gv.cc.doPropTriggers();
                            //gv.mod.isRecursiveCall = true;
                        }
	        		    else
	        		    {
	        			    gv.cc.doTrigger();
	        		    }
	        	    }
	            }
	            else
	            {
	                parentIdNum = getParentIdNum(childIdNum);
	                doActions = true;
	                doConvo(parentIdNum);
	            }
            }
        }    
        public string replaceText(string originalText)
        {
            //for The Raventhal, always assumed that main PC is talking
            Player pc = gv.mod.playerList[gv.mod.selectedPartyLeader];
            string newString = originalText;
        
            newString = newString.Replace("<FirstName>", pc.name);
            newString = newString.Replace("<FullName>", pc.name);
            newString = newString.Replace("<Class>", pc.playerClass.name);
            newString = newString.Replace("<class>", pc.playerClass.name.ToLower());
            newString = newString.Replace("<Race>", pc.race.name);
            newString = newString.Replace("<race>", pc.race.name.ToLower());
            if (pc.isMale)
            {
                newString = newString.Replace("<Sir/Madam>", "Sir");
                newString = newString.Replace("<sir/madam>", "sir");
                newString = newString.Replace("<His/Her>", "His");
                newString = newString.Replace("<his/her>", "his");
                newString = newString.Replace("<Him/Her>", "Him");
                newString = newString.Replace("<him/her>", "him");
                newString = newString.Replace("<He/She>", "He");
                newString = newString.Replace("<he/she>", "he");
                newString = newString.Replace("<Boy/Girl>", "Boy");
                newString = newString.Replace("<boy/girl>", "boy");
                newString = newString.Replace("<Lad/Lass>", "Lad");
                newString = newString.Replace("<lad/lass>", "lad");
                newString = newString.Replace("<Lord/Lady>", "Lord");
                newString = newString.Replace("<lord/lady>", "lord");
                newString = newString.Replace("<Man/Woman>", "Man");
                newString = newString.Replace("<man/woman>", "man");
                newString = newString.Replace("<Brother/Sister>", "Brother");
                newString = newString.Replace("<brother/sister>", "brother");
            }
            else
            {
                newString = newString.Replace("<Sir/Madam>", "Madam");
                newString = newString.Replace("<sir/madam>", "madam");
                newString = newString.Replace("<His/Her>", "Her");
                newString = newString.Replace("<his/her>", "her");
                newString = newString.Replace("<Him/Her>", "Her");
                newString = newString.Replace("<him/her>", "her");
                newString = newString.Replace("<He/She>", "She");
                newString = newString.Replace("<he/she>", "she");
                newString = newString.Replace("<Boy/Girl>", "Girl");
                newString = newString.Replace("<boy/girl>", "girl");
                newString = newString.Replace("<Lad/Lass>", "Lass");
                newString = newString.Replace("<lad/lass>", "lass");
                newString = newString.Replace("<Lord/Lady>", "Lady");
                newString = newString.Replace("<lord/lady>", "lady");
                newString = newString.Replace("<Man/Woman>", "Woman");
                newString = newString.Replace("<man/woman>", "woman");
                newString = newString.Replace("<Brother/Sister>", "Sister");
                newString = newString.Replace("<brother/sister>", "sister");
            }           
            return newString;
        }
    }
}
