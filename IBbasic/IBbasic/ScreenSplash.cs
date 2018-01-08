using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    public class ScreenSplash
    {
        //private Module gv.mod;
        private GameView gv;

        //private IbbButton btnUnzip = null;
        //private IbbButton btnZip = null;
        private IbbButton btnPlay = null;
        private IbbButton btnCreate = null;
        private SKBitmap titleBitmap;
        //private List<string> moduleFolderList = new List<string>();
        //private List<string> moduleZipList = new List<string>();
        //private string quickStartGuideString = "";

        public ScreenSplash(Module m, GameView g)
        {
            gv = g;
            setControlsStart();
            titleBitmap = gv.cc.GetFromBitmapList("iblogo.png");
        }

        public void setControlsStart()
        {
            int pH = (int)((float)gv.screenHeight / 100.0f);
            int wideX = (gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (int)(gv.ibbwidthL * gv.scaler / 2);

            if (btnPlay == null)
            {
                btnPlay = new IbbButton(gv, 1.2f);
                btnPlay.Text = "PLAY";
                btnPlay.Img = "btn_large";
                btnPlay.Glow = "btn_large_glow";
                btnPlay.X = wideX;
                btnPlay.Y = 2 * gv.uiSquareSize + pH * 4;
                btnPlay.Height = (int)(gv.ibbheight * gv.scaler);
                btnPlay.Width = (int)(gv.ibbwidthL * gv.scaler);
            }
            if (btnCreate == null)
            {
                btnCreate = new IbbButton(gv, 1.0f);
                btnCreate.Text = "CREATE";
                btnCreate.Img = "btn_large";
                btnCreate.Glow = "btn_large_glow";
                btnCreate.X = wideX;
                btnCreate.Y = 3 * gv.uiSquareSize + pH * 6;
                btnCreate.Height = (int)(gv.ibbheight * gv.scaler);
                btnCreate.Width = (int)(gv.ibbwidthL * gv.scaler);
            }
        }

        //TITLE SCREEN
        public void redrawSplash()
        {
            //DRAW TITLE SCREEN
            float dstHeight = ((float)gv.screenWidth / (float)titleBitmap.Width) * (float)titleBitmap.Height;
            //do narration with image setup
            IbRect src = new IbRect(0, 0, titleBitmap.Width, titleBitmap.Height);
            IbRect dst = new IbRect(0 - gv.oXshift, 0 - gv.oYshift, gv.screenWidth, (int)dstHeight);
            gv.DrawBitmap(titleBitmap, src, dst);

            btnPlay.Draw();
            btnCreate.Draw();

            //Draw IceBlink2RPG Engine Version Number
            int xLoc = (gv.uiSquaresInWidth * gv.uiSquareSize / 2) - (2 * gv.fontWidth);
            int pH = (int)((float)gv.screenHeight / 100.0f);
            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    gv.DrawText("v" + gv.versionNum, xLoc + x, (6 * gv.uiSquareSize) + (pH * 4) + y, "bk");
                }
            }
            gv.DrawText("v" + gv.versionNum, xLoc, (6 * gv.uiSquareSize) + (pH * 4), "wh");
        }

        public void onTouchSplash(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnPlay.glowOn = false;
            btnCreate.glowOn = false;

            //int eventAction = event.getAction();
            switch (eventType)
            {
                case MouseEventType.EventType.MouseUp:
                    int x = (int)eX;
                    int y = (int)eY;

                    btnPlay.glowOn = false;
                    btnCreate.glowOn = false;

                    if (btnPlay.getImpact(x, y))
                    {
                        btnPlay.Text = "Loading...";
                        if (gv.fixedModule.Equals(""))
                        {
                            gv.screenLauncher = new ScreenLauncher(gv.mod, gv);
                            gv.screenLauncher.loadModuleInfoFiles();
                            gv.screenType = "launcher";
                        }
                        else
                        {
                            gv.mod.moduleName = gv.fixedModule + "/" + gv.fixedModule;
                            gv.resetGame();
                        }
                    }
                    else if (btnCreate.getImpact(x, y))
                    {
                        //tutorialQuickStartGuide();
                        List<string> itlist = new List<string>();
                        itlist.Add("New Module");

                        var list2 = loadModuleNamesToList();
                        foreach (string s in list2)
                        {
                            itlist.Add(s);
                        }
                        
                        /*TODO using (ListItemSelector itSel = new ListItemSelector(gv, itlist, "Module to Edit"))
                        {
                            var ret = itSel.ShowDialog();

                            if (ret == DialogResult.OK)
                            {
                                //MessageBox.Show("you selected index: " + itSel.selectedIndex);
                                if (itSel.selectedIndex == 0)
                                {
                                    gv.mod = gv.cc.LoadModule("NewModule.mod");
                                    gv.resetGame();
                                    gv.screenType = "tsModule";
                                }
                                else if (itSel.selectedIndex > 0)
                                {
                                    //MessageBox.Show("opening an existing module");
                                    gv.mod = gv.cc.LoadModule(itlist[itSel.selectedIndex] + ".mod");
                                    gv.resetGame();
                                    gv.screenType = "tsModule";
                                }
                                else
                                {
                                    MessageBox.Show("didn't find a selection");
                                }
                            }
                        }*/
                    }
                    break;

                case MouseEventType.EventType.MouseMove:
                case MouseEventType.EventType.MouseDown:
                    x = (int) eX;
                    y = (int) eY;

                    if (btnPlay.getImpact(x, y))
                    {
                        btnPlay.glowOn = true;                                              
                    }
                    else if (btnCreate.getImpact(x, y))
                    {
                        btnCreate.glowOn = true;
                    }
                    break;
            }
        }

        public List<string> loadModuleNamesToList()
        {
            List<string> retList = new List<string>();
            string[] files;

            files = Directory.GetFiles(gv.mainDirectory + "\\modules", "*.mod", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (Path.GetFileName(file) != "NewModule.mod")
                {
                    // Process each file
                    Module modinfo = gv.cc.LoadModuleFileInfo(file);
                    if (modinfo == null)
                    {
                        gv.sf.MessageBox("returned a null module");
                    }
                    retList.Add(modinfo.moduleName);                    
                }
            }

            return retList;
        }
    }
}
