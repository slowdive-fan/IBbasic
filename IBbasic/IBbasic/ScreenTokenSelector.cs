using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ScreenTokenSelector
    {
        //public Module gv.mod;
	    public GameView gv;
        public Player pc;
	    private int tknPageIndex = 0;
	    private int tknSlotIndex = 0;
	    private int slotsPerPage = 15;
        private int maxPages = 40;
	    private List<IbbButton> btnTokenSlot = new List<IbbButton>();
	    private IbbButton btnTokensLeft = null;
	    private IbbButton btnTokensRight = null;
	    private IbbButton btnPageIndex = null;
	    private IbbButton btnAction = null;
        private IbbButton btnExit = null;
        public string callingScreen = "pcCreation"; //party, pcCreation, tsAreaEditor, tsEncEditor, tsConvoEditor
        public List<string> playerTokenList = new List<string>();

        public ScreenTokenSelector(Module m, GameView g)
	    {
		    //gv.mod = m;
		    gv = g;
	    }

        public void resetTokenSelector(string callingScreenToReturnTo, Player p)
        {
            pc = p;
            callingScreen = callingScreenToReturnTo;
            if (callingScreen.Equals("party"))
            {
                LoadPlayerTokenList();
            }
            else if (callingScreen.Equals("pcCreation"))
            {
                LoadPlayerTokenList();
            }
            else if (callingScreen.Equals("tsAreaEditor"))
            {
                LoadPropTokenList();
            }
            else if (callingScreen.Equals("tsEncEditor"))
            {
                LoadPropTokenList();
            }
            else if (callingScreen.Equals("tsConvoEditor"))
            {
                LoadPropTokenList();
            }
            else if (callingScreen.Equals("tsArtEditor"))
            {
                //LoadPropTokenList();
            }
        }

        public void LoadPlayerTokenList()
        {
            playerTokenList.Clear();
            //MODULE SPECIFIC
            try
            {
                //foreach (Bitmap b in gv.cc.commonBitmapList)
                foreach (KeyValuePair<string, SkiaSharp.SKBitmap> entry in gv.cc.moduleBitmapList)
                {
                    // do something with entry.Value or entry.Key
                    if (entry.Key.StartsWith("pc_"))
                    {
                        if (!playerTokenList.Contains(entry.Key))
                        {
                            playerTokenList.Add(entry.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gv.sf.MessageBox(ex.ToString());
                gv.errorLog(ex.ToString());
            }
            try
            {
                List<string> files = gv.GetAllFilesWithExtensionFromBothFolders("\\graphics", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
                //List<string> files = gv.GetGraphicsFiles(gv.mod.moduleName, ".png");
                //Load from PlayerTokens folder last
                //string[] files;
                //if (Directory.Exists(gv.mainDirectory + "\\default\\NewModule\\graphics"))
                //{
                //files = Directory.GetFiles(gv.mainDirectory + "\\\\default\\NewModule\\graphics", "*.png");
                //directory.mkdirs(); 
                foreach (string file in files)
                {
                    try
                    {
                        string filename = Path.GetFileName(file);
                        if (filename.StartsWith("pc_"))
                        {
                            string fileNameWithOutExt = Path.GetFileNameWithoutExtension(file);
                            if (!playerTokenList.Contains(fileNameWithOutExt))
                            {
                                playerTokenList.Add(fileNameWithOutExt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        gv.sf.MessageBox(ex.ToString());
                        gv.errorLog(ex.ToString());
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                gv.sf.MessageBox(ex.ToString());
                gv.errorLog(ex.ToString());
            }
        }
        public void LoadPropTokenList()
        {
            playerTokenList.Clear();            
            //MODULE SPECIFIC
            try
            {
                //foreach (Bitmap b in gv.cc.commonBitmapList)
                foreach (KeyValuePair<string, SkiaSharp.SKBitmap> entry in gv.cc.moduleBitmapList)
                {
                    // do something with entry.Value or entry.Key
                    if ((entry.Key.StartsWith("prp_")) || (entry.Key.StartsWith("tkn_")))
                    {
                        if (!playerTokenList.Contains(entry.Key))
                        {
                            playerTokenList.Add(entry.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gv.sf.MessageBox(ex.ToString());
                gv.errorLog(ex.ToString());
            }
            try
            {
                List<string> files = gv.GetAllFilesWithExtensionFromBothFolders("\\graphics", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
                //List<string> files = gv.GetGraphicsFiles(gv.mod.moduleName, ".png");
                //Load from PlayerTokens folder last
                //string[] files;
                //if (Directory.Exists(gv.mainDirectory + "\\default\\NewModule\\graphics"))
                //{
                //files = Directory.GetFiles(gv.mainDirectory + "\\\\default\\NewModule\\graphics", "*.png");
                //directory.mkdirs(); 
                foreach (string file in files)
                {
                    try
                    {
                        string filename = Path.GetFileName(file);
                        if ((filename.StartsWith("prp_")) || (filename.StartsWith("tkn_")))
                        {
                            string fileNameWithOutExt = Path.GetFileNameWithoutExtension(file);
                            if (!playerTokenList.Contains(fileNameWithOutExt))
                            {
                                playerTokenList.Add(fileNameWithOutExt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        gv.sf.MessageBox(ex.ToString());
                        gv.errorLog(ex.ToString());
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                gv.sf.MessageBox(ex.ToString());
                gv.errorLog(ex.ToString());
            }
        }

        public void setControlsStart()
	    {			
    	    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		    int padW = gv.uiSquareSize / 6;

            if (btnTokensLeft == null)
            {
                btnTokensLeft = new IbbButton(gv, 1.0f);
            }
			    btnTokensLeft.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnTokensLeft.Img2 = "ctrl_left_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_left_arrow);
			    btnTokensLeft.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnTokensLeft.X = 4 * gv.uiSquareSize;
			    btnTokensLeft.Y = (1 * gv.uiSquareSize / 2);
                btnTokensLeft.Height = (int)(gv.ibbheight * gv.scaler);
                btnTokensLeft.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnPageIndex == null)
            {
                btnPageIndex = new IbbButton(gv, 1.0f);
            }
			    btnPageIndex.Img = "btn_small_off"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_off);
			    btnPageIndex.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnPageIndex.Text = "1";
			    btnPageIndex.X = 5 * gv.uiSquareSize;
			    btnPageIndex.Y = (1 * gv.uiSquareSize / 2);
                btnPageIndex.Height = (int)(gv.ibbheight * gv.scaler);
                btnPageIndex.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnTokensRight == null)
            {
                btnTokensRight = new IbbButton(gv, 1.0f);
            }
			    btnTokensRight.Img = "btn_small"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small);
			    btnTokensRight.Img2 = "ctrl_right_arrow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.ctrl_right_arrow);
			    btnTokensRight.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			    btnTokensRight.X = 6 * gv.uiSquareSize;
			    btnTokensRight.Y = (1 * gv.uiSquareSize / 2);
                btnTokensRight.Height = (int)(gv.ibbheight * gv.scaler);
                btnTokensRight.Width = (int)(gv.ibbwidthR * gv.scaler);


            if (btnAction == null)
            {
                btnAction = new IbbButton(gv, 1.0f);
            }
                btnAction.Text = "USE SELECTED";
                btnAction.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
                btnAction.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
                btnAction.X = (gv.uiSquareSize * gv.uiSquaresInWidth / 2) - (int)(gv.ibbwidthL * gv.scaler);
                btnAction.Y = 6 * gv.uiSquareSize - pH;
                btnAction.Height = (int)(gv.ibbheight * gv.scaler);
                btnAction.Width = (int)(gv.ibbwidthL * gv.scaler);

            if (btnExit == null)
            {
                btnExit = new IbbButton(gv, 1.0f);
            }
                btnExit.Text = "EXIT";
                btnExit.Img = "btn_large"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large);
                btnExit.Glow = "btn_large_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_large_glow);
                btnExit.X = (gv.uiSquareSize * gv.uiSquaresInWidth / 2);
                btnExit.Y = 6 * gv.uiSquareSize - pH;
                btnExit.Height = (int)(gv.ibbheight * gv.scaler);
                btnExit.Width = (int)(gv.ibbwidthL * gv.scaler);

            for (int y = 0; y < slotsPerPage; y++)
		    {
			    IbbButton btnNew = new IbbButton(gv, 1.0f);
                //gv.cc.DisposeOfBitmap(ref btnNew.Img);
                btnNew.Img = "item_slot"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.item_slot);
                //gv.cc.DisposeOfBitmap(ref btnNew.Glow);
                btnNew.Glow = "btn_small_glow"; // BitmapFactory.decodeResource(gv.getResources(), R.drawable.btn_small_glow);
			
			    if (y < 5)
			    {
				    btnNew.X = ((y + 2) * gv.uiSquareSize + gv.uiSquareSize / 2) + (padW * (y + 1));
				    btnNew.Y = 2 * gv.uiSquareSize;
			    }
			    else if ((y >=5 ) && (y < 10))
			    {
				    btnNew.X = ((y - 5 + 2) * gv.uiSquareSize + gv.uiSquareSize / 2) + (padW * ((y - 5) + 1));
				    btnNew.Y = 3 * gv.uiSquareSize + padW;
			    }
                else
                {
                    btnNew.X = ((y - 10 + 2) * gv.uiSquareSize + gv.uiSquareSize / 2) + (padW * ((y - 10) + 1));
                    btnNew.Y = 4 * gv.uiSquareSize + (padW * 2);
                }

                btnNew.Height = (int)(gv.ibbheight * gv.scaler);
                btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);
			
			    btnTokenSlot.Add(btnNew);
		    }			
	    }
	
	    //INVENTORY SCREEN (COMBAT and MAIN)
        public void redrawTokenSelector()
        {
            //IF CONTROLS ARE NULL, CREATE THEM
    	    if (btnAction == null)
    	    {
    		    setControlsStart();
    	    }
    	
    	    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		
    	    int locY = 0;
            int locX = gv.uiSquareSize * 4 + pW * 2;

            int textH = (int)gv.fontHeight;
            int spacing = textH;
            int tabX = pW * 4;
    	    int tabX2 = 5 * gv.uiSquareSize + pW * 2;
    	    int leftStartY = pH * 4;
    	    int tabStartY = 5 * gv.uiSquareSize + pW * 10;
    	
            //DRAW TEXT		
		    locY = (pH * 2);
		    gv.DrawText("Token Selection", locX, locY, "wh");
		    
		    //DRAW LEFT/RIGHT ARROWS and PAGE INDEX
		    btnPageIndex.Draw();
		    btnTokensLeft.Draw();
		    btnTokensRight.Draw();		
		
		    //DRAW ALL INVENTORY SLOTS		
		    int cntSlot = 0;
		    foreach (IbbButton btn in btnTokenSlot)
		    {
			    if (cntSlot == tknSlotIndex) {btn.glowOn = true;}
			    else {btn.glowOn = false;}
			    if ((cntSlot + (tknPageIndex * slotsPerPage)) < playerTokenList.Count)
			    {
                    //gv.cc.DisposeOfBitmap(ref btn.Img2);
                    btn.Img2 = playerTokenList[cntSlot + (tknPageIndex * slotsPerPage)];
			    }
			    else
			    {
				    btn.Img2 = null;
			    }
			    btn.Draw();
			    cntSlot++;
		    }		
		    
		    btnAction.Draw();
            btnExit.Draw();
        }
        public void onTouchTokenSelector(int eX, int eY, MouseEventType.EventType eventType)
	    {
		    btnTokensLeft.glowOn = false;
		    btnTokensRight.glowOn = false;
		    btnAction.glowOn = false;
            btnExit.glowOn = false;
            
            switch (eventType)
		    {
		    case MouseEventType.EventType.MouseDown:
		    case MouseEventType.EventType.MouseMove:
			    int x = (int) eX;
			    int y = (int) eY;
			    if (btnTokensLeft.getImpact(x, y))
			    {
				    btnTokensLeft.glowOn = true;
			    }
			    else if (btnTokensRight.getImpact(x, y))
			    {
				    btnTokensRight.glowOn = true;
			    }
			    else if (btnAction.getImpact(x, y))
			    {
				    btnAction.glowOn = true;
			    }
                else if (btnExit.getImpact(x, y))
                {
                    btnExit.glowOn = true;
                }
                break;
			
		    case MouseEventType.EventType.MouseUp:
			    x = (int) eX;
			    y = (int) eY;
			
			    btnTokensLeft.glowOn = false;
			    btnTokensRight.glowOn = false;
			    btnAction.glowOn = false;
                btnExit.glowOn = false;
                
                for (int j = 0; j < slotsPerPage; j++)
			    {
				    if (btnTokenSlot[j].getImpact(x, y))
				    {
					    if (tknSlotIndex == j)
                        {                            
                            //return to calling screen
                            if (callingScreen.Equals("party"))
                            {
                                gv.screenParty.gv.mod.playerList[gv.cc.partyScreenPcIndex].tokenFilename = playerTokenList[GetIndex()];
                                gv.screenType = "party";
                                gv.screenParty.tokenLoad(gv.screenParty.gv.mod.playerList[gv.cc.partyScreenPcIndex]);
                            }
                            else if (callingScreen.Equals("pcCreation"))
                            {
                                //set PC token filename to the currently selected image
                                gv.screenPcCreation.pc.tokenFilename = playerTokenList[GetIndex()];
                                gv.screenType = "pcCreation";
                                gv.screenPcCreation.tokenLoad(gv.screenPcCreation.pc);
                            }
                            else if (callingScreen.Equals("tsAreaEditor"))
                            {
                                gv.tsAreaEditor.selectedTrigger.ImageFileName = playerTokenList[GetIndex()];
                                gv.screenType = "tsAreaEditor";
                            }                                
                            else if (callingScreen.Equals("tsEncEditor"))
                            {
                                gv.tsEncEditor.selectedTrigger.ImageFileName = playerTokenList[GetIndex()];
                                gv.screenType = "tsEncEditor";
                            }
                            else if (callingScreen.Equals("tsConvoEditor"))
                            {
                                gv.mod.currentConvo.NpcPortraitBitmap = playerTokenList[GetIndex()];
                                gv.screenType = "tsConvoEditor";
                            }
                            else if (callingScreen.Equals("tsArtEditor"))
                            {
                                gv.screenType = "tsArtEditor";
                                gv.tsArtEditor.OpenSelectedImage(playerTokenList[GetIndex()]);
                            }
                            doCleanUp();
                        }
					    tknSlotIndex = j;
				    }
			    }
			    if (btnTokensLeft.getImpact(x, y))
			    {
				    if (tknPageIndex > 0)
				    {
					    tknPageIndex--;
					    btnPageIndex.Text = (tknPageIndex + 1) + "";
				    }
			    }
			    else if (btnTokensRight.getImpact(x, y))
			    {
				    if (tknPageIndex < maxPages)
				    {
					    tknPageIndex++;
					    btnPageIndex.Text = (tknPageIndex + 1) + "";
				    }
			    }
			    else if (btnAction.getImpact(x, y))
			    {
				    //return to calling screen
                    if (callingScreen.Equals("party"))
                    {
                        gv.screenParty.gv.mod.playerList[gv.cc.partyScreenPcIndex].tokenFilename = playerTokenList[GetIndex()];
                        gv.screenType = "party";
                        gv.screenParty.tokenLoad(gv.screenParty.gv.mod.playerList[gv.cc.partyScreenPcIndex]);
                    }
                    else if (callingScreen.Equals("pcCreation"))
                    {
                        //set PC portrait filename to the currently selected image
                        gv.screenPcCreation.pc.tokenFilename = playerTokenList[GetIndex()];
                        gv.screenType = "pcCreation";
                        gv.screenPcCreation.tokenLoad(gv.screenPcCreation.pc);
                    }
                    else if (callingScreen.Equals("tsAreaEditor"))
                    {
                        gv.tsAreaEditor.selectedTrigger.ImageFileName = playerTokenList[GetIndex()];
                        gv.screenType = "tsAreaEditor";
                    }
                    else if (callingScreen.Equals("tsEncEditor"))
                    {
                        gv.tsEncEditor.selectedTrigger.ImageFileName = playerTokenList[GetIndex()];
                        gv.screenType = "tsEncEditor";
                    }
                    else if (callingScreen.Equals("tsConvoEditor"))
                    {
                        gv.mod.currentConvo.NpcPortraitBitmap = playerTokenList[GetIndex()];
                        gv.screenType = "tsConvoEditor";
                    }
                    else if (callingScreen.Equals("tsArtEditor"))
                    {
                        gv.screenType = "tsArtEditor";
                        gv.tsArtEditor.OpenSelectedImage(playerTokenList[GetIndex()]);
                    }
                    doCleanUp();						
			    }
                else if (btnExit.getImpact(x, y))
                {
                    //do nothing, return to calling screen
                    if (callingScreen.Equals("party"))
                    {
                        gv.screenType = "party";
                    }
                    else if (callingScreen.Equals("pcCreation"))
                    {
                        gv.screenType = "pcCreation";
                    }
                    else if (callingScreen.Equals("tsAreaEditor"))
                    {
                        gv.screenType = "tsAreaEditor";
                    }
                    else if (callingScreen.Equals("tsEncEditor"))
                    {
                        gv.screenType = "tsEncEditor";
                    }
                    else if (callingScreen.Equals("tsConvoEditor"))
                    {
                        gv.screenType = "tsConvoEditor";
                    }
                    else if (callingScreen.Equals("tsArtEditor"))
                    {
                        gv.screenType = "tsArtEditor";
                    }
                    doCleanUp();
                }                
			    break;		
		    }
	    }
        public void doCleanUp()
	    {
		    btnTokenSlot.Clear();
		    btnTokensLeft = null;
		    btnTokensRight = null;
		    btnPageIndex = null;
		    btnAction = null;
            btnExit = null;
	    }
	
	    public int GetIndex()
	    {
		    return tknSlotIndex + (tknPageIndex * slotsPerPage);
	    }	
	    public bool isSelectedPtrSlotInPortraitListRange()
	    {
            return GetIndex() < playerTokenList.Count;
	    }
    }
}
