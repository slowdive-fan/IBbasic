using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IBbasic
{
    public class ToolsetScreenArtEditor
    {
        public GameView gv;
        public SKBitmap myBitmapGDI;
        //public Bitmap myBitmapDX;
        public SKBitmap selectedColorBitmapGDI;
        //public Bitmap selectedColorBitmapDX;
        public IbRect src = null;
        public IbRect dst = null;
        public string button = "btn_small";
        public string slot = "item_slot";
        public string grass = "t_f_grass";
        public IbbButton btnNew = null;
        public IbbButton btnOpen = null;
        public IbbButton btnSave = null;
        public IbbButton btnSaveAs = null;
        public IbbButton btnAlphaAdjust = null;
        public IbbButton btnEraser = null;
        public IbbButton btnCanvasBackground = null;
        public IbbButton btnPreviewBackground = null;
        public IbbButton btnToggleLayer = null;
        public IbbButton btnShowLayers = null;
        public IbbButton btnGetColor = null;
        public IbbButton btnUndo = null;
        public IbbButton btnRedo = null;
        public IbPalette palette = null;
        public SKColor currentColor;
        public bool isIdleLayerShown = true;
        public bool showLayers = true;
        public bool getColorMode = false;
        public List<int> colorPaletteList = new List<int>();
        public int previewBackIndex = 0;
        public int canvasBackIndex = 0;
        public string filename = "newdrawing";
        public Stack<SKBitmap> undoStack = new Stack<SKBitmap>();
        public Stack<SKBitmap> redoStack = new Stack<SKBitmap>();
        public bool continuousDrawMode = false;
        public int mapStartLocXinPixels;
        public int mapSquareSizeScaler = 1;

        public ToolsetScreenArtEditor(GameView g)
        {
            gv = g;
            mapStartLocXinPixels = 1 * gv.uiSquareSize;
            //var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
            myBitmapGDI = new SKBitmap(48, 48);
            //updateBitmapDX();
            selectedColorBitmapGDI = new SKBitmap(1, 1);
            currentColor = new SKColor(255, 255, 255, 255); //RGBA
            selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
            //updateSelectedColorBitmapDX();
            setControlsStart();
        }

        public void setControlsStart()
        {
            if (btnNew == null)
            {
                btnNew = new IbbButton(gv, 1.0f);
            }
            btnNew.Img = "btn_small";
            btnNew.Glow = "btn_small_glow";
            btnNew.Text = "NEW";
            btnNew.X = 0 * gv.uiSquareSize;
            btnNew.Y = 0 * gv.uiSquareSize + gv.scaler;
            btnNew.Height = (int)(gv.ibbheight * gv.scaler);
            btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnOpen == null)
            {
                btnOpen = new IbbButton(gv, 1.0f);
            }
            btnOpen.Img = "btn_small";
            btnOpen.Glow = "btn_small_glow";
            btnOpen.Text = "OPEN";
            btnOpen.X = 0 * gv.uiSquareSize;
            btnOpen.Y = 1 * gv.uiSquareSize + gv.scaler;
            btnOpen.Height = (int)(gv.ibbheight * gv.scaler);
            btnOpen.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnSave == null)
            {
                btnSave = new IbbButton(gv, 1.0f);
            }
            btnSave.Img = "btn_small";
            btnSave.Glow = "btn_small_glow";
            btnSave.Text = "SAVE";
            btnSave.X = 0 * gv.uiSquareSize;
            btnSave.Y = 2 * gv.uiSquareSize + gv.scaler;
            btnSave.Height = (int)(gv.ibbheight * gv.scaler);
            btnSave.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnSaveAs == null)
            {
                btnSaveAs = new IbbButton(gv, 1.0f);
            }
            btnSaveAs.Img = "btn_small";
            btnSaveAs.Glow = "btn_small_glow";
            btnSaveAs.Text = "SAVEAS";
            btnSaveAs.X = 0 * gv.uiSquareSize;
            btnSaveAs.Y = 3 * gv.uiSquareSize + gv.scaler;
            btnSaveAs.Height = (int)(gv.ibbheight * gv.scaler);
            btnSaveAs.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnUndo == null)
            {
                btnUndo = new IbbButton(gv, 1.0f);
            }
            btnUndo.Img = "btn_small";
            btnUndo.Glow = "btn_small_glow";
            btnUndo.Text = "UNDO";
            btnUndo.X = 10 * gv.uiSquareSize;
            btnUndo.Y = 4 * gv.uiSquareSize + gv.scaler;
            btnUndo.Height = (int)(gv.ibbheight * gv.scaler);
            btnUndo.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRedo == null)
            {
                btnRedo = new IbbButton(gv, 1.0f);
            }
            btnRedo.Img = "btn_small";
            btnRedo.Glow = "btn_small_glow";
            btnRedo.Text = "REDO";
            btnRedo.X = 10 * gv.uiSquareSize;
            btnRedo.Y = 5 * gv.uiSquareSize + gv.scaler;
            btnRedo.Height = (int)(gv.ibbheight * gv.scaler);
            btnRedo.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnAlphaAdjust == null)
            {
                btnAlphaAdjust = new IbbButton(gv, 1.0f);
            }
            btnAlphaAdjust.Img = "btn_small";
            btnAlphaAdjust.Glow = "btn_small_glow";
            btnAlphaAdjust.Text = "ALPHASET";
            btnAlphaAdjust.X = 0 * gv.uiSquareSize;
            btnAlphaAdjust.Y = 4 * gv.uiSquareSize + gv.scaler;
            btnAlphaAdjust.Height = (int)(gv.ibbheight * gv.scaler);
            btnAlphaAdjust.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnPreviewBackground == null)
            {
                btnPreviewBackground = new IbbButton(gv, 1.0f);
            }
            btnPreviewBackground.Img = "btn_small";
            btnPreviewBackground.Glow = "btn_small_glow";
            btnPreviewBackground.Text = "PREVIEW";
            btnPreviewBackground.X = 10 * gv.uiSquareSize;
            btnPreviewBackground.Y = 6 * gv.uiSquareSize + gv.scaler;
            btnPreviewBackground.Height = (int)(gv.ibbheight * gv.scaler);
            btnPreviewBackground.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCanvasBackground == null)
            {
                btnCanvasBackground = new IbbButton(gv, 1.0f);
            }
            btnCanvasBackground.Img = "btn_small";
            btnCanvasBackground.Glow = "btn_small_glow";
            btnCanvasBackground.Text = "CANVAS";
            btnCanvasBackground.X = 0 * gv.uiSquareSize;
            btnCanvasBackground.Y = 5 * gv.uiSquareSize + gv.scaler;
            btnCanvasBackground.Height = (int)(gv.ibbheight * gv.scaler);
            btnCanvasBackground.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnToggleLayer == null)
            {
                btnToggleLayer = new IbbButton(gv, 1.0f);
                btnToggleLayer.Text = "TglLayer";
            }
            btnToggleLayer.Img = "btn_small";
            btnToggleLayer.Glow = "btn_small_glow";
            //btnToggleLayer.Text = "TglLayer";
            btnToggleLayer.X = 10 * gv.uiSquareSize;
            btnToggleLayer.Y = 2 * gv.uiSquareSize + gv.scaler;
            btnToggleLayer.Height = (int)(gv.ibbheight * gv.scaler);
            btnToggleLayer.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnShowLayers == null)
            {
                btnShowLayers = new IbbButton(gv, 1.0f);
                btnShowLayers.Text = "ShowLyrs";
            }
            btnShowLayers.Img = "btn_small";
            btnShowLayers.Glow = "btn_small_glow";
            //btnShowLayers.Text = "ShowLyrs";
            btnShowLayers.X = 10 * gv.uiSquareSize;
            btnShowLayers.Y = 3 * gv.uiSquareSize + gv.scaler;
            btnShowLayers.Height = (int)(gv.ibbheight * gv.scaler);
            btnShowLayers.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnEraser == null)
            {
                btnEraser = new IbbButton(gv, 1.0f);
            }
            btnEraser.Img = "btn_small";
            btnEraser.Glow = "btn_small_glow";
            btnEraser.Text = "Eraser";
            btnEraser.X = 10 * gv.uiSquareSize;
            btnEraser.Y = 1 * gv.uiSquareSize + gv.scaler;
            btnEraser.Height = (int)(gv.ibbheight * gv.scaler);
            btnEraser.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnGetColor == null)
            {
                btnGetColor = new IbbButton(gv, 1.0f);
                btnGetColor.Img = "btn_small";
            }
            //btnGetColor.Img = "btn_small";
            btnGetColor.Glow = "btn_small_glow";
            btnGetColor.Text = "GetColor";
            btnGetColor.X = 10 * gv.uiSquareSize;
            btnGetColor.Y = 0 * gv.uiSquareSize + gv.scaler;
            btnGetColor.Height = (int)(gv.ibbheight * gv.scaler);
            btnGetColor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (palette == null)
            {
                palette = new IbPalette(gv);
            }
            palette.Img = "color_palette";
            palette.X = (1 * gv.uiSquareSize) + (10 * gv.squareSize * gv.scaler);
            palette.Y = 0 * gv.uiSquareSize;
            palette.Width = (int)(2 * gv.uiSquareSize) - (gv.scaler * 2);
            palette.Height = (int)(palette.Width * 20f / 9f);

        }
        //public void updateBitmapDX()
        //{
            //myBitmapDX = gv.cc.ConvertGDIBitmapToD2D(myBitmapGDI);
        //}
        //public void updateSelectedColorBitmapDX()
        //{
            //selectedColorBitmapDX = gv.cc.ConvertGDIBitmapToD2D(selectedColorBitmapGDI);
        //}
        public async void doNewDialog()
        {
            List<string> items = new List<string>();
            items.Add("cancel");
            items.Add("24x24");
            items.Add("48x48");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a canvas size:");
            if (selected != "cancel")
            {
                if (selected.Equals("24x24"))
                {
                    myBitmapGDI = new SKBitmap(24, 24);
                    //updateBitmapDX();
                }
                else if (selected.Equals("48x48"))
                {
                    myBitmapGDI = new SKBitmap(48, 48);
                    //updateBitmapDX();
                }
                //gv.mod.companionPlayerList[playerListIndex].raceTag = selected;
            }
            gv.touchEnabled = true;
        }
        public async void doOpenDialog()
        {
            string prefix = "it_";
            //choose type of file to load
            List<string> items = new List<string>();
            items.Add("cancel");
            items.Add("item");
            items.Add("token");
            items.Add("prop");
            items.Add("ui");
            items.Add("tiles");
            items.Add("walls");
            items.Add("backdrops");
            items.Add("overlays");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select an image type to open:");
            if (selected != "cancel")
            {
                if (selected.Equals("item"))
                {
                    prefix = "it_";
                }
                else if (selected.Equals("token"))
                {
                    prefix = "tkn_";
                }
                else if (selected.Equals("prop"))
                {
                    prefix = "prp_";
                }
                else if (selected.Equals("ui"))
                {
                    prefix = "ui_";
                }
                else if (selected.Equals("tiles"))
                {
                    prefix = "t_";
                }
                else if (selected.Equals("walls"))
                {
                    prefix = "w_";
                }
                else if (selected.Equals("backdrops"))
                {
                    prefix = "bd_";
                }
                else if (selected.Equals("overlays"))
                {
                    prefix = "o_";
                }

                items = GetAllImagesList(prefix);
                items.Insert(0, "none");
                selected = await gv.ListViewPage(items, "Select an image to open:");
                if (selected != "none")
                {
                    filename = selected;
                    myBitmapGDI = gv.cc.LoadBitmap(selected);
                    //updateBitmapDX();
                }
            }
            gv.touchEnabled = true;
        }
        public List<string> GetAllImagesList(string prefix)
        {
            List<string> imageList = new List<string>();
            //MODULE SPECIFIC
            List<string> files = new List<string>();
            if ((prefix.Equals("it_")) || (prefix.Equals("tkn_")) || (prefix.Equals("prp_")))
            {
                files = gv.GetAllFilesWithExtensionFromBothFolders("\\graphics", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            }
            else if (prefix.Equals("ui_"))
            {
                files = gv.GetAllFilesWithExtensionFromBothFolders("\\ui", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            }
            else if ((prefix.Equals("bd_")) || (prefix.Equals("o_")) || (prefix.Equals("t_")) || (prefix.Equals("w_")))
            {
                files = gv.GetAllFilesWithExtensionFromBothFolders("\\tiles", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            }

            foreach (string f in files)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                if (filenameNoExt.StartsWith(prefix))
                {
                    imageList.Add(filenameNoExt);
                }
            }
            return imageList;
        }
        public async void doSaveAsDialog()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Filename for this image (do not include the extension)", filename);
            if ((myinput.Equals("newdrawing")) || myinput.Equals("none"))
            {
                gv.sf.MessageBoxHtml("you can't use a blank filename or 'newdrawing' filename.");
            }
            else
            {
                filename = myinput;
                doSaveDialog();
                gv.touchEnabled = true;
            }
            gv.touchEnabled = true;
        }
        public void doSaveDialog()
        {
            if (filename.Equals("newdrawing"))
            {
                doSaveAsDialog();
                return;
            }
            try
            {
                {
                    //gv.cc.createDirectory(gv.mainDirectory + "//modules//" + gv.mod.moduleName + "//graphics");
                    //myBitmapGDI.Save(gv.mainDirectory + "//modules//" + gv.mod.moduleName + "//graphics//" + filename + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    gv.SaveImage("\\modules\\" + gv.mod.moduleName + "\\graphics\\" + filename + ".png", myBitmapGDI);
                    gv.sf.MessageBoxHtml("file saved as: " + filename + ".png");
                }
            }
            catch (Exception ex)
            {
                gv.touchEnabled = true;
                gv.sf.MessageBoxHtml("Error saving file: " + filename + ".png");
            }
        }
        public void doAlphaAdjust()
        {
            for (int x = 0; x < myBitmapGDI.Width; x++)
            {
                for (int y = 0; y < myBitmapGDI.Height; y++)
                {
                    if (myBitmapGDI.GetPixel(x, y).Alpha < 30)
                    {
                        myBitmapGDI.SetPixel(x, y, new SKColor(myBitmapGDI.GetPixel(x, y).Red, myBitmapGDI.GetPixel(x, y).Green, myBitmapGDI.GetPixel(x, y).Blue, 0));
                    }
                    else
                    {
                        myBitmapGDI.SetPixel(x, y, new SKColor(myBitmapGDI.GetPixel(x, y).Red, myBitmapGDI.GetPixel(x, y).Green, myBitmapGDI.GetPixel(x, y).Blue, 255));
                    }
                }
            }
            //updateBitmapDX();
        }
        public void fillPaletteColorList()
        {
            /*
            //red
            colorPaletteList.add(Color.argb(255, 78, 7, 7));
            colorPaletteList.add(Color.argb(255, 156, 14, 14));
            colorPaletteList.add(Color.argb(255, 234, 21, 21));
            colorPaletteList.add(Color.argb(255, 241, 99, 99));
            colorPaletteList.add(Color.argb(255, 248, 177, 177));
            //orange
            colorPaletteList.add(Color.argb(255, 78, 18, 7));
            colorPaletteList.add(Color.argb(255, 156, 35, 14));
            colorPaletteList.add(Color.argb(255, 234, 53, 21));
            colorPaletteList.add(Color.argb(255, 241, 120, 99));
            colorPaletteList.add(Color.argb(255, 248, 188, 177));
            //brown
            colorPaletteList.add(Color.argb(255, 78, 30, 7));
            colorPaletteList.add(Color.argb(255, 156, 61, 14));
            colorPaletteList.add(Color.argb(255, 234, 90, 21));
            colorPaletteList.add(Color.argb(255, 241, 146, 99));
            colorPaletteList.add(Color.argb(255, 248, 200, 177));
            //gold
            colorPaletteList.add(Color.argb(255, 78, 51, 7));
            colorPaletteList.add(Color.argb(255, 156, 103, 14));
            colorPaletteList.add(Color.argb(255, 234, 154, 21));
            colorPaletteList.add(Color.argb(255, 241, 188, 99));
            colorPaletteList.add(Color.argb(255, 248, 221, 177));
            //yellow
            colorPaletteList.add(Color.argb(255, 78, 69, 7));
            colorPaletteList.add(Color.argb(255, 156, 138, 14));
            colorPaletteList.add(Color.argb(255, 234, 207, 21));
            colorPaletteList.add(Color.argb(255, 241, 223, 99));
            colorPaletteList.add(Color.argb(255, 248, 239, 177));
            //green
            colorPaletteList.add(Color.argb(255, 7, 78, 7));
            colorPaletteList.add(Color.argb(255, 14, 156, 14));
            colorPaletteList.add(Color.argb(255, 21, 234, 21));
            colorPaletteList.add(Color.argb(255, 99, 241, 99));
            colorPaletteList.add(Color.argb(255, 177, 248, 177));
            //green-blue
            colorPaletteList.add(Color.argb(255, 7, 78, 61));
            colorPaletteList.add(Color.argb(255, 14, 156, 121));
            colorPaletteList.add(Color.argb(255, 21, 234, 181));
            colorPaletteList.add(Color.argb(255, 99, 241, 206));
            colorPaletteList.add(Color.argb(255, 177, 248, 231));
            //blue
            colorPaletteList.add(Color.argb(255, 7, 43, 78));
            colorPaletteList.add(Color.argb(255, 14, 85, 156));
            colorPaletteList.add(Color.argb(255, 21, 128, 234));
            colorPaletteList.add(Color.argb(255, 99, 170, 241));
            colorPaletteList.add(Color.argb(255, 177, 213, 248));
            //violet
            colorPaletteList.add(Color.argb(255, 34, 7, 78));
            colorPaletteList.add(Color.argb(255, 67, 14, 156));
            colorPaletteList.add(Color.argb(255, 101, 21, 234));
            colorPaletteList.add(Color.argb(255, 152, 99, 241));
            colorPaletteList.add(Color.argb(255, 204, 177, 248));
            //magenta
            colorPaletteList.add(Color.argb(255, 78, 7, 78));
            colorPaletteList.add(Color.argb(255, 156, 14, 156));
            colorPaletteList.add(Color.argb(255, 234, 21, 234));
            colorPaletteList.add(Color.argb(255, 241, 99, 241));
            colorPaletteList.add(Color.argb(255, 248, 177, 248));
            //magenta-red
            colorPaletteList.add(Color.argb(255, 78, 7, 43));
            colorPaletteList.add(Color.argb(255, 156, 14, 85));
            colorPaletteList.add(Color.argb(255, 234, 21, 128));
            colorPaletteList.add(Color.argb(255, 241, 99, 170));
            colorPaletteList.add(Color.argb(255, 248, 177, 213));
            //black to white
            colorPaletteList.add(Color.argb(255, 0, 0, 0));
            colorPaletteList.add(Color.argb(255, 28, 28, 28));
            colorPaletteList.add(Color.argb(255, 56, 56, 56));
            colorPaletteList.add(Color.argb(255, 84, 84, 84));
            colorPaletteList.add(Color.argb(255, 112, 112, 112));
            colorPaletteList.add(Color.argb(255, 140, 140, 140));
            colorPaletteList.add(Color.argb(255, 168, 168, 168));
            colorPaletteList.add(Color.argb(255, 196, 196, 196));
            colorPaletteList.add(Color.argb(255, 224, 224, 224));
            colorPaletteList.add(Color.argb(255, 255, 255, 255));
            */
        }
        public void doUndo()
        {
            SKBitmap newBm = myBitmapGDI.Copy();
            redoStack.Push(newBm);
            myBitmapGDI = undoStack.Pop();
        }
        public void doRedo()
        {
            SKBitmap newBm = myBitmapGDI.Copy();
            undoStack.Push(newBm);
            myBitmapGDI = redoStack.Pop();
        }
        public void pushToUndoStack()
        {
            SKBitmap newBm = myBitmapGDI.Copy();
            undoStack.Push(newBm);
            redoStack.Clear();
        }
        public void doCanvasSizeSelectionDialog()
        {
            /*
            final CharSequence[] items = { "24x24 (standard)","48x48 (tiles)","24x48 (regular combat token)","48x96 (large combat token)"};
            // Creating and Building the Dialog
            AlertDialog.Builder builder = new AlertDialog.Builder(this.gameContext);
            builder.setTitle("Choose a Canvas Size.");
            builder.setItems(items, new DialogInterface.OnClickListener()
            {
                    public void onClick(DialogInterface dialog, int item)
            {
                if (item == 0)
                {
                    //24x24
                    myBitmap = Bitmap.createBitmap(24, 24, Bitmap.Config.ARGB_8888);
                }
                else if (item == 1)
                {
                    //48x48
                    myBitmap = Bitmap.createBitmap(48, 48, Bitmap.Config.ARGB_8888);
                }
                else if (item == 2)
                {
                    //24x48
                    myBitmap = Bitmap.createBitmap(24, 48, Bitmap.Config.ARGB_8888);
                }
                else if (item == 3)
                {
                    //48x96
                    myBitmap = Bitmap.createBitmap(48, 96, Bitmap.Config.ARGB_8888);
                }
                ActionDialog.dismiss();
                invalidate();
            }
        });
                this.ActionDialog = builder.create();
                this.ActionDialog.show();
            */
        }

        public void redrawTsArtEditor()
        {
            setControlsStart();

            int drawingSurfaceWidth = gv.squareSize * 10 * gv.scaler;
            int drawingSurfaceHeight = gv.squareSize * 10 * gv.scaler;
            int drawingPreviewWidth = (int)(gv.uiSquareSize * 1.5);
            int drawingPreviewHeight = (int)(gv.uiSquareSize * 1.5);
            int convoPanelLeftLocation = 1 * gv.uiSquareSize + 2 * gv.scaler;
            int convoPanelTopLocation = 2 * gv.scaler + gv.fontHeight + gv.fontLineSpacing;
            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            int shiftForFont = (gv.ibbMiniTglHeight * gv.scaler / 2) - (gv.fontHeight / 2);

            //determine canvas size
            if ((myBitmapGDI.Width == 48) && (myBitmapGDI.Height == 96)) //normal
            {
                drawingSurfaceWidth = gv.squareSize * 10 * gv.scaler;
                drawingSurfaceHeight = gv.squareSize * 10 * gv.scaler;
                drawingPreviewWidth = (int)(gv.uiSquareSize * 1.5);
                drawingPreviewHeight = (int)(gv.uiSquareSize * 1.5);
            }
            else if ((myBitmapGDI.Width == 96) && (myBitmapGDI.Height == 96)) //wide
            {
                drawingSurfaceWidth = gv.squareSize * 10 * gv.scaler;
                drawingSurfaceHeight = gv.squareSize * 5 * gv.scaler;
                drawingPreviewWidth = (int)(gv.uiSquareSize * 1.5);
                drawingPreviewHeight = (int)(gv.uiSquareSize * 0.75);
            }
            else if ((myBitmapGDI.Width == 48) && (myBitmapGDI.Height == 192)) //tall
            {
                drawingSurfaceWidth = gv.squareSize * 5 * gv.scaler;
                drawingSurfaceHeight = gv.squareSize * 10 * gv.scaler;
                drawingPreviewWidth = (int)(gv.uiSquareSize * 0.75);
                drawingPreviewHeight = (int)(gv.uiSquareSize * 1.5);
            }
            else if ((myBitmapGDI.Width == 96) && (myBitmapGDI.Height == 192)) //tall
            {
                drawingSurfaceWidth = gv.squareSize * 10 * gv.scaler;
                drawingSurfaceHeight = gv.squareSize * 10 * gv.scaler;
                drawingPreviewWidth = (int)(gv.uiSquareSize * 1.5);
                drawingPreviewHeight = (int)(gv.uiSquareSize * 1.5);
            }

            //CANVAS
            string background = grass;
            if (canvasBackIndex == 0)
            {
                background = grass;
            }
            else if (canvasBackIndex == 1)
            {
                background = slot;
            }
            else if (canvasBackIndex == 2)
            {
                background = button;
            }
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(background).Width, gv.cc.GetFromTileBitmapList(background).Height);
            dst = new IbRect(mapStartLocXinPixels, 0, drawingSurfaceWidth, drawingSurfaceHeight);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(background), src, dst);

            //if combat token, show idle or attack
            if ((myBitmapGDI.Width != myBitmapGDI.Height) || (myBitmapGDI.Height == 96))
            {
                if (isIdleLayerShown) //idle layer on top
                {
                    if (showLayers)
                    {
                        //draw attack first with low opacity
                        src = new IbRect(0, myBitmapGDI.Height / 2, myBitmapGDI.Width, myBitmapGDI.Height / 2);
                        dst = new IbRect(mapStartLocXinPixels, 0, drawingSurfaceWidth, drawingSurfaceHeight);
                        gv.DrawBitmap(myBitmapGDI, src, dst);
                    }
                    //draw idle at full opacity
                    src = new IbRect(0, 0, myBitmapGDI.Width, myBitmapGDI.Height / 2);
                    dst = new IbRect(mapStartLocXinPixels, 0, drawingSurfaceWidth, drawingSurfaceHeight);
                    gv.DrawBitmap(myBitmapGDI, src, dst);
                }
                else //attack layer on top
                {
                    if (showLayers)
                    {
                        //draw idle first with low opacity
                        src = new IbRect(0, 0, myBitmapGDI.Width, myBitmapGDI.Height / 2);
                        dst = new IbRect(mapStartLocXinPixels, 0, drawingSurfaceWidth, drawingSurfaceHeight);
                        gv.DrawBitmap(myBitmapGDI, src, dst);
                    }
                    //draw attack at full opacity
                    src = new IbRect(0, myBitmapGDI.Height / 2, myBitmapGDI.Width, myBitmapGDI.Height / 2);
                    dst = new IbRect(mapStartLocXinPixels, 0, drawingSurfaceWidth, drawingSurfaceHeight);
                    gv.DrawBitmap(myBitmapGDI, src, dst);
                }
            }
            else
            {
                src = new IbRect(0, 0, myBitmapGDI.Width, myBitmapGDI.Height);
                dst = new IbRect(mapStartLocXinPixels, 0, drawingSurfaceWidth, drawingSurfaceHeight);
                gv.DrawBitmap(myBitmapGDI, src, dst);
            }

            //DRAW GRID
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("grid_black").Width, gv.cc.GetFromTileBitmapList("grid_black").Height);
            int pixelSize = gv.squareSize * 10 * gv.scaler / myBitmapGDI.Width;
            if ((myBitmapGDI.Width == 48) && (myBitmapGDI.Height == 192))
            {
                pixelSize = gv.squareSize * 5 * gv.scaler / myBitmapGDI.Width;
            }
            for (int x = 0; x < myBitmapGDI.Width; x++)
            {
                for (int y = 0; y < myBitmapGDI.Height; y++)
                {
                    dst = new IbRect(mapStartLocXinPixels + x * pixelSize, y * pixelSize, pixelSize * 5, pixelSize * 5);
                    gv.DrawBitmap(gv.cc.GetFromTileBitmapList("grid_black"), src, dst);
                }
            }

            //draw border
            int width2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Width;
            int height2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Height;
            src = new IbRect(0, 0, width2, height2);
            dst = new IbRect(0 - (176 * gv.scaler), 0 - (106 * gv.scaler), (width2 * gv.scaler) + (gv.scaler * 16), (height2 * gv.scaler) + (gv.scaler * 10));
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png"), src, dst);

            //Page Title
            gv.DrawText("IBBASIC ART EDITOR: " + filename, 2 * gv.uiSquareSize, 2 * gv.scaler, "yl");

            //PREVIEW
            background = grass;
            if (previewBackIndex == 0)
            {
                background = grass;
            }
            else if (previewBackIndex == 1)
            {
                background = slot;
            }
            else if (previewBackIndex == 2)
            {
                background = button;
            }
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(background).Width, gv.cc.GetFromTileBitmapList(background).Height);
            dst = new IbRect((int)(8.25 * gv.uiSquareSize), (int)(5.5 * gv.uiSquareSize), drawingPreviewWidth, drawingPreviewHeight);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(background), src, dst);


            //if combat token, show idle or attack
            src = new IbRect(0, 0, myBitmapGDI.Width, myBitmapGDI.Height);
            if ((myBitmapGDI.Width != myBitmapGDI.Height) || (myBitmapGDI.Height == 96))
            {
                src = new IbRect(0, 0, myBitmapGDI.Width, myBitmapGDI.Height / 2);
                if (!isIdleLayerShown) //attack layer on top
                {
                    src = new IbRect(0, myBitmapGDI.Height / 2, myBitmapGDI.Width, myBitmapGDI.Height / 2);
                }
            }

            dst = new IbRect((int)(8.25 * gv.uiSquareSize), (int)(5.5 * gv.uiSquareSize), drawingPreviewWidth, drawingPreviewHeight);
            gv.DrawBitmap(myBitmapGDI, src, dst);


            //draw selected color square
            //selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
            //updateSelectedColorBitmapDX();
            //myBitmapDX = gv.cc.ConvertGDIBitmapToD2D(myBitmapGDI);
            src = new IbRect(0, 0, 1, 1);
            dst = new IbRect((int)(8.6 * gv.uiSquareSize), (int)(4.5 * gv.uiSquareSize), (int)(0.8 * gv.uiSquareSize), (int)(0.8 * gv.uiSquareSize));
            gv.DrawBitmap(selectedColorBitmapGDI, src, dst);

            //CONTROLS            
            btnNew.Draw();
            btnOpen.Draw();
            btnSave.Draw();
            btnSaveAs.Draw();
            btnToggleLayer.Draw();
            btnShowLayers.Draw();
            btnAlphaAdjust.Draw();
            btnEraser.Draw();
            btnGetColor.Draw();
            btnCanvasBackground.Draw();
            btnPreviewBackground.Draw();
            btnUndo.Draw();
            btnRedo.Draw();

            palette.Draw();

            gv.tsMainMenu.redrawTsMainMenu();

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox();
            }
        }

        public void onTouchTsArtEditor(int eX, int eY, MouseEventType.EventType eventType)
        {
            //btnHelp.glowOn = false;

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
                    //sqrSize = total area width / number of pixels
                    int pixelSize = gv.squareSize * 10 * gv.scaler / myBitmapGDI.Width;
                    if ((myBitmapGDI.Width == 48) && (myBitmapGDI.Height == 192))
                    {
                        pixelSize = gv.squareSize * 5 * gv.scaler / myBitmapGDI.Width;
                    }
                    int gridX = ((eX - mapStartLocXinPixels) / pixelSize);
                    int gridY = (eY / pixelSize);

                    if (gv.showMessageBox)
                    {
                        if (gv.messageBox.btnReturn.getImpact(x, y))
                        {
                            gv.messageBox.btnReturn.glowOn = true;
                        }
                        return;
                    }
                    //if (e.Button == MouseButtons.Left)
                    //{
                        if (getColorMode)
                        {
                            //check to see if in drawing area first
                            if (tapInMapViewport(x, y))
                            {
                                //check to see if combat token
                                if ((myBitmapGDI.Width != myBitmapGDI.Height) || (myBitmapGDI.Height == 96)) //combat token so get color from active layer
                                {
                                    if (isIdleLayerShown) //get color from idle layer
                                    {
                                        currentColor = myBitmapGDI.GetPixel(gridX, gridY);
                                        selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
                                        //updateSelectedColorBitmapDX();
                                    }
                                    else //get color from attack layer
                                    {
                                        currentColor = myBitmapGDI.GetPixel(gridX, gridY + myBitmapGDI.Height / 2);
                                        selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
                                        //updateSelectedColorBitmapDX();
                                    }
                                }
                                else //not a combat token
                                {
                                    currentColor = myBitmapGDI.GetPixel(gridX, gridY);
                                    selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
                                    //updateSelectedColorBitmapDX();
                                }
                            }
                        }
                        else //drawing mode
                        {
                            //check to see if in drawing area first
                            if (tapInMapViewport(x, y))
                            {
                                if (!continuousDrawMode)
                                {
                                    pushToUndoStack();
                                    continuousDrawMode = true;
                                }
                            }
                            if (tapInMapViewport(x, y))
                            {
                                if ((myBitmapGDI.Width != myBitmapGDI.Height) || (myBitmapGDI.Height == 96))//combat token so draw on active layer
                                {
                                    if (isIdleLayerShown) //draw on idle layer
                                    {
                                        myBitmapGDI.SetPixel(gridX, gridY, currentColor);
                                    }
                                    else //draw on attack layer
                                    {
                                        myBitmapGDI.SetPixel(gridX, gridY + myBitmapGDI.Height / 2, currentColor);
                                    }
                                }
                                else //not combat token
                                {
                                    myBitmapGDI.SetPixel(gridX, gridY, currentColor);
                                }
                                //updateBitmapDX();
                            }
                        }
                    //}
                    /*if (btnHelp.getImpact(x, y))
                    {
                        btnHelp.glowOn = true;
                    }*/
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    //btnHelp.glowOn = false;

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

                    if (continuousDrawMode)
                    {
                        continuousDrawMode = false;
                    }

                    if (btnSaveAs.getImpact(x, y))
                    {
                        doSaveAsDialog();
                    }
                    else if (btnSave.getImpact(x, y))
                    {
                        doSaveDialog();
                    }
                    else if (btnOpen.getImpact(x, y))
                    {
                        doOpenDialog();
                        undoStack.Clear();
                        redoStack.Clear();
                    }
                    else if (btnUndo.getImpact(x, y))
                    {
                        if (undoStack.Count != 0)
                        {
                            doUndo();
                        }
                    }
                    else if (btnRedo.getImpact(x, y))
                    {
                        if (redoStack.Count != 0)
                        {
                            doRedo();
                        }
                    }
                    else if (btnNew.getImpact(x, y))
                    {
                        doNewDialog();
                        /*performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);
                        new AlertDialog.Builder(this.gameContext)
                                .setIcon(android.R.drawable.ic_dialog_alert)
                                .setTitle("New Canvas")
                                .setMessage("Are you sure you want start a new canvas (existing drawing will be erased)?")
                                .setPositiveButton("Yes", new DialogInterface.OnClickListener()
                                {
                                            @Override
                                            public void onClick(DialogInterface dialog, int which)
                        {
                            doCanvasSizeSelectionDialog();
                            //myBitmap = Bitmap.createBitmap(24, 24, Bitmap.Config.ARGB_8888);
                            undoStack.clear();
                            redoStack.clear();
                            invalidate();
                        }
                    })
                                        .setNegativeButton("No", null)
                                        .show();*/
                    }
                    else if (btnAlphaAdjust.getImpact(x, y))
                    {
                        pushToUndoStack();
                        doAlphaAdjust();
                    }
                    else if (btnToggleLayer.getImpact(x, y))
                    {
                        if (isIdleLayerShown)
                        {
                            isIdleLayerShown = false;
                            btnToggleLayer.Text = "Attack";
                        }
                        else
                        {
                            isIdleLayerShown = true;
                            btnToggleLayer.Text = "Idle";
                        }
                    }
                    else if (btnShowLayers.getImpact(x, y))
                    {
                        if (showLayers)
                        {
                            showLayers = false;
                            btnShowLayers.Text = "HideLyrs";
                        }
                        else
                        {
                            showLayers = true;
                            btnShowLayers.Text = "ShowLyrs";
                        }
                    }
                    else if (btnGetColor.getImpact(x, y))
                    {
                        if (getColorMode)
                        {
                            getColorMode = false;
                            btnGetColor.Img = "btn_small_off";
                            //TODO btnGetColor.Img = BitmapFactory.decodeResource(getResources(), R.mipmap.btn_small);
                        }
                        else
                        {
                            getColorMode = true;
                            btnGetColor.Img = "btn_small_on";
                            //TODO btnGetColor.Img = BitmapFactory.decodeResource(getResources(), R.mipmap.item_slot);
                        }
                    }
                    else if (palette.getImpact(x, y))
                    {
                        palette.getColorClicked(x, y);
                        selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
                        //updateSelectedColorBitmapDX();
                        //myBitmapDX = gv.cc.ConvertGDIBitmapToD2D(myBitmapGDI);
                    }
                    else if (btnEraser.getImpact(x, y))
                    {
                        currentColor = new SKColor(0, 0, 0, 0);
                        selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
                        //updateSelectedColorBitmapDX();
                    }
                    else if (btnCanvasBackground.getImpact(x, y))
                    {
                        canvasBackIndex++;
                        if (canvasBackIndex > 2) { canvasBackIndex = 0; }
                    }
                    else if (btnPreviewBackground.getImpact(x, y))
                    {
                        previewBackIndex++;
                        if (previewBackIndex > 2) { previewBackIndex = 0; }
                    }


                    //if (btnHelp.getImpact(x, y))
                    //{
                    //incrementalSaveModule();
                    //}
                    break;
            }
        }

        public bool tapInMapViewport(int x, int y)
        {
            if (x < mapStartLocXinPixels) { return false; }
            if (y < 0) { return false; }
            if (x > mapStartLocXinPixels + gv.squareSize * gv.scaler * 10) { return false; }
            if (y > gv.squareSize * gv.scaler * 10) { return false; }
            return true;
        }
    }

    public class IbPalette
    {
        public string Img = null;
        public SKBitmap bmpGDI = null;
        public int X = 0;
        public int Y = 0;
        public int Width = 0;
        public int Height = 0;
        public GameView gv;
        public Coordinate lastClickedColorLocation = new Coordinate(0, 0);
        public IbRect src = null;
        public IbRect dst = null;

        public IbPalette(GameView g)
        {
            gv = g;
            bmpGDI = gv.cc.LoadBitmap("color_palette");
        }

        public bool getImpact(int x, int y)
        {
            if ((x >= X) && (x <= (X + this.Width)))
            {
                if ((y >= Y) && (y <= (Y + this.Height)))
                {
                    return true;
                }
            }
            return false;
        }
        public int getColorIndexClicked(int x, int y)
        {
            int squareSize = this.Width / 13;
            int col = (x - X) / squareSize;
            int row = (y - Y) / squareSize;
            int clickedIndex = col * 5 + row;
            if ((clickedIndex >= 0) && (clickedIndex <= 65))
            {
                return clickedIndex;
            }
            return -1;
        }
        public void getColorClicked(int x, int y)
        {
            float squareSize = this.Width / 9f;
            int col = (int)((x - X) / squareSize);
            int row = (int)((y - Y) / squareSize);
            gv.tsArtEditor.currentColor = bmpGDI.GetPixel(col, row);
            lastClickedColorLocation = new Coordinate((int)(col * squareSize), (int)(row * squareSize));
        }
        public void Draw()
        {
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(Img).Width, gv.cc.GetFromTileBitmapList(Img).Height);
            dst = new IbRect(this.X, this.Y, this.Width, this.Height);
            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(Img), src, dst);

            //draw selected info tile highlight
            int tlX = this.X + lastClickedColorLocation.X;
            int tlY = this.Y + lastClickedColorLocation.Y;
            int brX = this.Width / 9;
            int brY = this.Width / 9;
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
            dst = new IbRect(tlX, tlY, brX, brY);
            if ((lastClickedColorLocation.X > (5 * this.Width / 20)) && (lastClickedColorLocation.X < (12 * this.Width / 20)))
            {
                gv.DrawBitmap(gv.cc.GetFromTileBitmapList("highlight_magenta"), src, dst);
            }
            else
            {
                gv.DrawBitmap(gv.cc.GetFromTileBitmapList("highlight_greenTrig"), src, dst);
            }
        }
    }
}
