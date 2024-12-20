﻿using SkiaSharp;
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
        public string magenta = "ui_magentaback";
        public IbbButton btnNew = null;
        public IbbButton btnBucketFill = null;
        public IbbButton btnSwitchPalette = null;
        public IbbButton btnTransform = null;
        public IbbToggle tglSwapColor = null;
        public IbbButton btnEraser = null;
        public IbbButton btnCanvasBackground = null;
        public IbbButton btnPreviewBackground = null;
        public IbbButton btnToggleLayer = null;
        public IbbButton tglPencil = null;
        public IbbButton btnGetColor = null;
        public IbbButton btnUndo = null;
        public IbbButton btnRedo = null;
        public IbPalette palette = null;
        public IbbButton tglZoom = null;
        public SKColor currentColor;
        public bool isIdleLayerShown = true;
        //public bool showLayers = true;
        public bool getColorMode = false;
        public List<Coordinate> swapColorCoorList = new List<Coordinate>();
        public List<string> colorPaletteList = new List<string>();
        public int colorPaletteListIndex = 0;
        public int previewBackIndex = 0;
        public int canvasBackIndex = 0;
        public string filename = "newdrawing";
        public Stack<SKBitmap> undoStack = new Stack<SKBitmap>();
        public Stack<SKBitmap> redoStack = new Stack<SKBitmap>();
        public bool continuousDrawMode = false;
        public int mapStartLocXinPixels;
        public int zoomScaler = 1;
        public int pencilSize = 1;
        public int upperSquareX = 0;
        public int upperSquareY = 0;
        public int panSquareX = 0;
        public int panSquareY = 0;
        public int artSquareSize = 48;
        public float artScaler = 1;

        public ToolsetScreenArtEditor(GameView g)
        {
            gv = g;
            artScaler = (float)gv.scaler / 2f;
            mapStartLocXinPixels = 1 * gv.uiSquareSize;
            loadColorPaletteList();
            //var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
            myBitmapGDI = new SKBitmap(48, 48);
            //updateBitmapDX();
            selectedColorBitmapGDI = new SKBitmap(1, 1);
            currentColor = new SKColor(255, 255, 255, 255); //RGBA
            selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
            //updateSelectedColorBitmapDX();
            setControlsStart();
        }
        public void loadColorPaletteList()
        {
            colorPaletteList.Clear();
            colorPaletteList.Add("color_palette2");
            colorPaletteList.Add("color_palette");
            //USER SPECIFIC
            List<string> files = gv.GetAllFilesWithExtensionFromUserFolder("\\user", ".png");
            foreach (string f in files)
            {
                try
                {
                    if (f.StartsWith("pal_"))
                    {
                        colorPaletteList.Add(f);
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    gv.errorLog(ex.ToString());
                }
            }
        }
        public void setControlsStart()
        {
            if (btnNew == null)
            {
                btnNew = new IbbButton(gv, 1.0f);
            }
            btnNew.Img = "btn_small";
            btnNew.Img2 = "btndiskutils";
            btnNew.Glow = "btn_small_glow";
            //btnNew.Text = "NEW";
            btnNew.X = 0 * gv.uiSquareSize;
            btnNew.Y = 0 * gv.uiSquareSize + (int)(gv.scaler);
            btnNew.Height = (int)(gv.ibbheight * gv.scaler);
            btnNew.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnBucketFill == null)
            {
                btnBucketFill = new IbbButton(gv, 1.0f);
            }
            btnBucketFill.Img = "btn_small";
            btnBucketFill.Img2 = "btnbucketfill";
            btnBucketFill.Glow = "btn_small_glow";
            //btnOpen.Text = "OPEN";
            btnBucketFill.X = 0 * gv.uiSquareSize;
            btnBucketFill.Y = 1 * gv.uiSquareSize + (int)(gv.scaler);
            btnBucketFill.Height = (int)(gv.ibbheight * gv.scaler);
            btnBucketFill.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnSwitchPalette == null)
            {
                btnSwitchPalette = new IbbButton(gv, 1.0f);
            }
            btnSwitchPalette.Img = "btn_small";
            btnSwitchPalette.Img2 = "btnswitchpalette";
            btnSwitchPalette.Glow = "btn_small_glow";
            //btnSave.Text = "SAVE";
            btnSwitchPalette.X = 0 * gv.uiSquareSize;
            btnSwitchPalette.Y = 2 * gv.uiSquareSize + (int)(gv.scaler);
            btnSwitchPalette.Height = (int)(gv.ibbheight * gv.scaler);
            btnSwitchPalette.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnTransform == null)
            {
                btnTransform = new IbbButton(gv, 1.0f);
            }
            btnTransform.Img = "btn_small";
            btnTransform.Img2 = "btntransforms";
            btnTransform.Glow = "btn_small_glow";
            //btnSaveAs.Text = "SAVEAS";
            btnTransform.X = 0 * gv.uiSquareSize;
            btnTransform.Y = 3 * gv.uiSquareSize + (int)(gv.scaler);
            btnTransform.Height = (int)(gv.ibbheight * gv.scaler);
            btnTransform.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnUndo == null)
            {
                btnUndo = new IbbButton(gv, 1.0f);
            }
            btnUndo.Img = "btn_small";
            btnUndo.Img2 = "btnundo";
            btnUndo.Glow = "btn_small_glow";
            //btnUndo.Text = "UNDO";
            btnUndo.X = 10 * gv.uiSquareSize;
            btnUndo.Y = 4 * gv.uiSquareSize + (int)(gv.scaler);
            btnUndo.Height = (int)(gv.ibbheight * gv.scaler);
            btnUndo.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnRedo == null)
            {
                btnRedo = new IbbButton(gv, 1.0f);
            }
            btnRedo.Img = "btn_small";
            btnRedo.Img2 = "btnredo";
            btnRedo.Glow = "btn_small_glow";
            //btnRedo.Text = "REDO";
            btnRedo.X = 10 * gv.uiSquareSize;
            btnRedo.Y = 5 * gv.uiSquareSize + (int)(gv.scaler);
            btnRedo.Height = (int)(gv.ibbheight * gv.scaler);
            btnRedo.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (tglSwapColor == null)
            {
                tglSwapColor = new IbbToggle(gv);
                tglSwapColor.toggleOn = false;
            }
            tglSwapColor.ImgOn = "tgl_swapcolor_on";
            tglSwapColor.ImgOff = "tgl_swapcolor_off";
            tglSwapColor.X = 0 * gv.uiSquareSize;
            tglSwapColor.Y = 4 * gv.uiSquareSize + (int)(gv.scaler);
            tglSwapColor.Height = (int)(gv.ibbheight * gv.scaler);
            tglSwapColor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnPreviewBackground == null)
            {
                btnPreviewBackground = new IbbButton(gv, 1.0f);
            }
            btnPreviewBackground.Img = "btn_small";
            btnPreviewBackground.Img2 = "btncanvas";
            btnPreviewBackground.Glow = "btn_small_glow";
            //btnPreviewBackground.Text = "PREVIEW";
            btnPreviewBackground.X = 10 * gv.uiSquareSize;
            btnPreviewBackground.Y = 6 * gv.uiSquareSize + (int)(gv.scaler);
            btnPreviewBackground.Height = (int)(gv.ibbheight * gv.scaler);
            btnPreviewBackground.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCanvasBackground == null)
            {
                btnCanvasBackground = new IbbButton(gv, 1.0f);
            }
            btnCanvasBackground.Img = "btn_small";
            btnCanvasBackground.Img2 = "btncanvas";
            btnCanvasBackground.Glow = "btn_small_glow";
            //btnCanvasBackground.Text = "CANVAS";
            btnCanvasBackground.X = 0 * gv.uiSquareSize;
            btnCanvasBackground.Y = 5 * gv.uiSquareSize + (int)(gv.scaler);
            btnCanvasBackground.Height = (int)(gv.ibbheight * gv.scaler);
            btnCanvasBackground.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnToggleLayer == null)
            {
                btnToggleLayer = new IbbButton(gv, 1.0f);
                btnToggleLayer.Img = "tgl_idle_on";
            }
            btnToggleLayer.Glow = "btn_small_glow";
            btnToggleLayer.X = 10 * gv.uiSquareSize;
            btnToggleLayer.Y = 2 * gv.uiSquareSize + (int)(gv.scaler);
            btnToggleLayer.Height = (int)(gv.ibbheight * gv.scaler);
            btnToggleLayer.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (tglPencil == null)
            {
                tglPencil = new IbbButton(gv, 1.0f);
                tglPencil.Text = "1PX";
            }
            tglPencil.Img = "btn_small";
            tglPencil.Img2 = "btnpencil";
            tglPencil.Glow = "btn_small_glow";
            tglPencil.X = 10 * gv.uiSquareSize;
            tglPencil.Y = 3 * gv.uiSquareSize + (int)(gv.scaler);
            tglPencil.Height = (int)(gv.ibbheight * gv.scaler);
            tglPencil.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnEraser == null)
            {
                btnEraser = new IbbButton(gv, 1.0f);
            }
            btnEraser.Img = "btn_small";
            btnEraser.Img2 = "btneraser";
            btnEraser.Glow = "btn_small_glow";
            //btnEraser.Text = "Eraser";
            btnEraser.X = 10 * gv.uiSquareSize;
            btnEraser.Y = 1 * gv.uiSquareSize + (int)(gv.scaler);
            btnEraser.Height = (int)(gv.ibbheight * gv.scaler);
            btnEraser.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnGetColor == null)
            {
                btnGetColor = new IbbButton(gv, 1.0f);
                btnGetColor.Img = "btn_small";

            }
            btnGetColor.Img2 = "btneyedropper";
            btnGetColor.Glow = "btn_small_glow";
            //btnGetColor.Text = "GetColor";
            btnGetColor.X = 10 * gv.uiSquareSize;
            btnGetColor.Y = 0 * gv.uiSquareSize + (int)(gv.scaler);
            btnGetColor.Height = (int)(gv.ibbheight * gv.scaler);
            btnGetColor.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (palette == null)
            {
                palette = new IbPalette(gv);
                palette.Img = colorPaletteList[0];
            }            
            palette.X = (1 * gv.uiSquareSize) + (int)((10 * gv.squareSize * gv.scaler));
            palette.Y = 0 * gv.uiSquareSize;
            palette.Width = (int)(2 * gv.uiSquareSize) - (int)((gv.scaler * 2));
            palette.Height = (int)(palette.Width * 20f / 9f);

            if (tglZoom == null)
            {
                tglZoom = new IbbButton(gv, 1.0f);
                tglZoom.Text = "1X";
            }
            tglZoom.Img = "tgl_zoom_off";
            tglZoom.Glow = "btn_small_glow";
            tglZoom.X = 10 * gv.uiSquareSize;
            tglZoom.Y = 6 * gv.uiSquareSize + (int)(gv.scaler);
            tglZoom.Height = (int)(gv.ibbheight * gv.scaler);
            tglZoom.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        //public void updateBitmapDX()
        //{
        //myBitmapDX = gv.cc.ConvertGDIBitmapToD2D(myBitmapGDI);
        //}
        //public void updateSelectedColorBitmapDX()
        //{
        //selectedColorBitmapDX = gv.cc.ConvertGDIBitmapToD2D(selectedColorBitmapGDI);
        //}
        public async void doDiskUtilities()
        {
            List<string> items = new List<string>();
            items.Add("cancel");
            items.Add("Create New Image");
            items.Add("Open Image");
            items.Add("Save Image");
            items.Add("Save As (give new name)");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a disk function:");
            if (selected != "cancel")
            {
                if (selected.Equals("Create New Image"))
                {
                    doNewDialog();
                }
                else if (selected.Equals("Open Image"))
                {
                    doOpenDialog();
                }
                else if (selected.Equals("Save Image"))
                {
                    doSaveDialog();
                }
                else if (selected.Equals("Save As (give new name)"))
                {
                    doSaveAsDialog();
                }
            }
            gv.touchEnabled = true;
        }
        public async void doTransform()
        {
            List<string> items = new List<string>();
            items.Add("cancel");
            items.Add("Rotate Right");
            items.Add("Rotate Left");
            items.Add("Flip Horizontal");
            items.Add("Flip Vertical");
            items.Add("Shift All Down 1 pixel");
            items.Add("Shift All Up 1 pixel");
            items.Add("Shift All Right 1 pixel");
            items.Add("Shift All Left 1 pixel");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a transformation function:");
            if (selected != "cancel")
            {
                if (selected.Equals("Rotate Right"))
                {
                    doRotateRight();
                }
                else if (selected.Equals("Rotate Left"))
                {
                    doRotateLeft();
                }
                else if (selected.Equals("Flip Horizontal"))
                {
                    doFlipHorizontal();
                }
                else if (selected.Equals("Flip Vertical"))
                {
                    doFlipVertical();
                }
                else if (selected.Equals("Shift All Down 1 pixel"))
                {
                    doShiftDown();
                }
                else if (selected.Equals("Shift All Up 1 pixel"))
                {
                    doShiftUp();
                }
                else if (selected.Equals("Shift All Right 1 pixel"))
                {
                    doShiftRight();
                }
                else if (selected.Equals("Shift All Left 1 pixel"))
                {
                    doShiftLeft();
                }
            }
            gv.touchEnabled = true;
        }
        public void doRotateRight()
        {
            try
            {
                SKBitmap oldBm = myBitmapGDI.Copy();
                for (int x = 0; x < myBitmapGDI.Width; x++)
                {
                    for (int y = 0; y < myBitmapGDI.Height; y++)
                    {
                        SKColor clr = oldBm.GetPixel(y, oldBm.Height - 1 - x); //going through x when loop goes through y
                        myBitmapGDI.SetPixel(x, y, clr);
                    }
                }
                oldBm.Dispose();
                pushToUndoStack();
            }
            catch (Exception ex)
            {

            }
        }
        public void doRotateLeft()
        {
            try
            {
                SKBitmap oldBm = myBitmapGDI.Copy();
                for (int x = 0; x < myBitmapGDI.Width; x++)
                {
                    for (int y = 0; y < myBitmapGDI.Height; y++)
                    {
                        SKColor clr = oldBm.GetPixel(oldBm.Width - 1 - y, x); //going through x when loop goes through y
                        myBitmapGDI.SetPixel(x, y, clr);
                    }
                }
                oldBm.Dispose();
                pushToUndoStack();
            }
            catch (Exception ex)
            {

            }
        }
        public void doFlipVertical()
        {
            try
            {
                SKBitmap oldBm = myBitmapGDI.Copy();
                for (int x = 0; x < myBitmapGDI.Width; x++)
                {
                    for (int y = 0; y < myBitmapGDI.Height; y++)
                    {
                        SKColor clr = oldBm.GetPixel(oldBm.Width - 1 - x, y);
                        myBitmapGDI.SetPixel(x, y, clr);
                    }
                }
                oldBm.Dispose();
                pushToUndoStack();
            }
            catch (Exception ex)
            {

            }
        }
        public void doFlipHorizontal()
        {
            try
            {
                SKBitmap oldBm = myBitmapGDI.Copy();
                for (int x = 0; x < myBitmapGDI.Width; x++)
                {
                    for (int y = 0; y < myBitmapGDI.Height; y++)
                    {
                        SKColor clr = oldBm.GetPixel(x, oldBm.Height - 1 - y);
                        myBitmapGDI.SetPixel(x, y, clr);
                    }
                }
                oldBm.Dispose();
                pushToUndoStack();
            }
            catch (Exception ex)
            {

            }
        }
        public void doShiftUp()
        {
            try
            {
                SKBitmap oldBm = myBitmapGDI.Copy();
                for (int x = 0; x < myBitmapGDI.Width; x++)
                {
                    for (int y = 0; y < myBitmapGDI.Height; y++)
                    {
                        if (y < oldBm.Height - 1)
                        {
                            SKColor clr = oldBm.GetPixel(x, y + 1);
                            myBitmapGDI.SetPixel(x, y, clr);
                        }
                    }
                }
                oldBm.Dispose();
                pushToUndoStack();
            }
            catch (Exception ex)
            {

            }
        }
        public void doShiftDown()
        {
            try
            {
                SKBitmap oldBm = myBitmapGDI.Copy();
                for (int x = 0; x < myBitmapGDI.Width; x++)
                {
                    for (int y = 0; y < myBitmapGDI.Height; y++)
                    {
                        if (y > 0)
                        {
                            SKColor clr = oldBm.GetPixel(x, y - 1);
                            myBitmapGDI.SetPixel(x, y, clr);
                        }
                    }
                }
                oldBm.Dispose();
                pushToUndoStack();
            }
            catch (Exception ex)
            {

            }
        }
        public void doShiftLeft()
        {
            try
            {
                SKBitmap oldBm = myBitmapGDI.Copy();
                for (int x = 0; x < myBitmapGDI.Width; x++)
                {
                    for (int y = 0; y < myBitmapGDI.Height; y++)
                    {
                        if (x < oldBm.Width - 1)
                        {
                            SKColor clr = oldBm.GetPixel(x + 1, y);
                            myBitmapGDI.SetPixel(x, y, clr);
                        }
                    }
                }
                oldBm.Dispose();
                pushToUndoStack();
            }
            catch (Exception ex)
            {

            }
        }
        public void doShiftRight()
        {
            try
            {
                SKBitmap oldBm = myBitmapGDI.Copy();
                for (int x = 0; x < myBitmapGDI.Width; x++)
                {
                    for (int y = 0; y < myBitmapGDI.Height; y++)
                    {
                        if (x > 0)
                        {
                            SKColor clr = oldBm.GetPixel(x - 1, y);
                            myBitmapGDI.SetPixel(x, y, clr);
                        }
                    }
                }
                oldBm.Dispose();
                pushToUndoStack();
            }
            catch (Exception ex)
            {

            }
        }
        public async void doNewDialog()
        {
            List<string> items = new List<string>();
            items.Add("cancel");
            items.Add("item");
            items.Add("token_regular");
            items.Add("token_tall");
            items.Add("token_wide");
            items.Add("token_large");
            items.Add("prop");
            items.Add("ui");
            items.Add("tiles");
            items.Add("Player Tokens");
            //items.Add("3Dwallset");
            items.Add("3Dbackdrop");
            //items.Add("3Doverlay");

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(items, "Select a canvas size:");
            if (selected != "cancel")
            {
                if (selected.Equals("item"))
                {
                    filename = "it_newdrawing";
                    myBitmapGDI = new SKBitmap(48, 48);
                    clearAllPixels();
                }
                else if (selected.Equals("token_regular"))
                {
                    filename = "tkn_newdrawing";
                    myBitmapGDI = new SKBitmap(48, 96);
                    clearAllPixels();
                }
                else if (selected.Equals("Player Tokens"))
                {
                    filename = "pc_newdrawing";
                    myBitmapGDI = new SKBitmap(48, 96);
                    clearAllPixels();
                }
                else if (selected.Equals("token_tall"))
                {
                    filename = "tkn_newdrawing";
                    myBitmapGDI = new SKBitmap(48, 192);
                    clearAllPixels();
                }
                else if (selected.Equals("token_wide"))
                {
                    filename = "tkn_newdrawing";
                    myBitmapGDI = new SKBitmap(96, 96);
                    clearAllPixels();
                }
                else if (selected.Equals("token_large"))
                {
                    filename = "tkn_newdrawing";
                    myBitmapGDI = new SKBitmap(96, 192);
                    clearAllPixels();
                }
                else if (selected.Equals("prop"))
                {
                    filename = "prp_newdrawing";
                    myBitmapGDI = new SKBitmap(48, 48);
                    clearAllPixels();
                }
                else if (selected.Equals("ui"))
                {
                    filename = "ui_newdrawing";
                    myBitmapGDI = new SKBitmap(48, 48);
                    clearAllPixels();
                }
                else if (selected.Equals("tiles"))
                {
                    filename = "t_newdrawing";
                    myBitmapGDI = new SKBitmap(48, 48);
                    clearAllPixels();
                }
                else if (selected.Equals("3Dwallset"))
                {
                    filename = "w_w_newwallset";
                    myBitmapGDI = new SKBitmap(204, 88);
                    clearAllPixels();
                }
                else if (selected.Equals("3Dbackdrop"))
                {
                    filename = "w_w_newbackdrop";
                    myBitmapGDI = new SKBitmap(88, 88);
                    clearAllPixels();
                }
                else if (selected.Equals("3Doverlay"))
                {
                    filename = "w_w_newoverlay";
                    myBitmapGDI = new SKBitmap(204, 88);
                    clearAllPixels();
                }
            }
            gv.touchEnabled = true;
        }
        public async void doOpenDialog()
        {
            gv.cc.commonBitmapList.Clear();
            string prefix = "it_";
            //choose type of file to load
            List<string> items = new List<string>();
            items.Add("cancel");
            items.Add("item");
            items.Add("token");
            items.Add("prop");
            items.Add("ui");
            items.Add("tiles");
            items.Add("Player Tokens");
            //items.Add("3Dwallset");
            items.Add("3Dbackdrop");
            //items.Add("3Doverlay");

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
                else if (selected.Equals("3Dwallset"))
                {
                    prefix = "w_";
                }
                else if (selected.Equals("3Dbackdrop"))
                {
                    prefix = "bd_";
                }
                else if (selected.Equals("3Doverlay"))
                {
                    prefix = "o_";
                }
                else if (selected.Equals("Player Tokens"))
                {
                    prefix = "pc_";
                }

                items = GetAllImagesList(prefix);
                gv.screenType = "tokenSelector";
                gv.screenTokenSelector.resetTokenSelector("tsArtEditor", null);
                gv.screenTokenSelector.playerTokenList = items;
                /*items.Insert(0, "none");
                selected = await gv.ListViewPage(items, "Select an image to open:");
                if (selected != "none")
                {
                    filename = selected;
                    myBitmapGDI = gv.cc.LoadBitmap(selected);
                    //updateBitmapDX();
                }*/
            }
            gv.touchEnabled = true;
        }
        public void OpenSelectedImage(string selected)
        {
            if (selected != "none")
            {
                filename = selected;
                myBitmapGDI = gv.cc.LoadBitmap(selected);
            }
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
            else if (prefix.Equals("pc_"))
            {
                files = gv.GetAllFilesWithExtensionFromBothFolders("\\graphics", "\\user", ".png");
            }
            else if ((prefix.Equals("bd_")) || (prefix.Equals("o_")) || (prefix.Equals("t_")) || (prefix.Equals("w_")))
            {
                files = gv.GetAllFilesWithExtensionFromBothFolders("\\tiles", "\\modules\\" + gv.mod.moduleName + "\\graphics", ".png");
            }

            foreach (string f in files)
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(f);
                if ((filenameNoExt.StartsWith(prefix)) || (prefix.Equals("ui_")))
                {
                    imageList.Add(filenameNoExt);
                }
            }
            return imageList;
        }
        public async void doSaveAsDialog()
        {
            gv.touchEnabled = false;
            string header = "Filename for this image (do not include the extension). Files should use the following prefixes: "
                            + "it_ for items, tkn_ for creature tokens, prp_ for props, ui_ for ui elements, t_ for tiles.";
            string myinput = await gv.StringInputBox(header, filename);
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
                    if (filename.StartsWith("pc_"))
                    {
                        gv.SaveImage("\\user\\" + filename + ".png", myBitmapGDI);
                        gv.sf.MessageBoxHtml("file saved as: " + filename + ".png" + " in the 'user' directory.");
                    }
                    else
                    {
                        gv.SaveImage("\\modules\\" + gv.mod.moduleName + "\\graphics\\" + filename + ".png", myBitmapGDI);
                        gv.sf.MessageBoxHtml("file saved as: " + filename + ".png" + " in your module's 'graphics' folder.");
                    }
                    //gv.cc.createDirectory(gv.mainDirectory + "//modules//" + gv.mod.moduleName + "//graphics");
                    //myBitmapGDI.Save(gv.mainDirectory + "//modules//" + gv.mod.moduleName + "//graphics//" + filename + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    //gv.SaveImage("\\modules\\" + gv.mod.moduleName + "\\graphics\\" + filename + ".png", myBitmapGDI);
                    //gv.sf.MessageBoxHtml("file saved as: " + filename + ".png");
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
        public void doBucketFill()
        {
            for (int x = 0; x < myBitmapGDI.Width; x++)
            {
                for (int y = 0; y < myBitmapGDI.Height; y++)
                {
                    myBitmapGDI.SetPixel(x, y, currentColor);

                }
            }
        }
        public void clearAllPixels()
        {
            for (int x = 0; x < myBitmapGDI.Width; x++)
            {
                for (int y = 0; y < myBitmapGDI.Height; y++)
                {
                    myBitmapGDI.SetPixel(x, y, new SKColor(0, 0, 0, 0));
                }
            }
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

        public void redrawTsArtEditor(SKCanvas c)
        {
            setControlsStart();

            int drawingSurfaceSize = (int)(artSquareSize * 10); //default 480x480 no scale applied
            int drawingPreviewSize = (int)(artSquareSize * 2); //default 96x96 no scale applied

            int canvasScaler = 10; //multiply image size by this to fill canvas
            int previewScaler = 2; //multiply image size by this to fill preview

            int previewImageWidth = artSquareSize; //draw image size in default screen size
            int previewImageHeight = artSquareSize; //draw image size in default screen size

            float previewLocX = 8.25f * gv.uiSquareSize;
            float previewLocY = 5.5f * gv.uiSquareSize;

            int zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);

            //determine pixel size
            if ((myBitmapGDI.Width == 48) && (myBitmapGDI.Height == 96)) //normal
            {
                previewImageWidth = artSquareSize;
                previewImageHeight = artSquareSize;
                previewScaler = 2;
                canvasScaler = 10;
                zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
            }
            else if ((myBitmapGDI.Width == 96) && (myBitmapGDI.Height == 96)) //wide
            {
                previewImageWidth = artSquareSize * 2;
                previewImageHeight = artSquareSize;
                previewScaler = 1;
                canvasScaler = 5;
                zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
            }
            else if ((myBitmapGDI.Width == 48) && (myBitmapGDI.Height == 192)) //tall
            {
                previewImageWidth = artSquareSize;
                previewImageHeight = artSquareSize * 2;
                previewScaler = 1;
                canvasScaler = 5;
                zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
            }
            else if ((myBitmapGDI.Width == 96) && (myBitmapGDI.Height == 192)) //large
            {
                previewImageWidth = artSquareSize * 2;
                previewImageHeight = artSquareSize * 2;
                previewScaler = 1;
                canvasScaler = 5;
                zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
            }
            else if ((myBitmapGDI.Width == 88) && (myBitmapGDI.Height == 88)) //backdrop
            {
                previewImageWidth = 88;
                previewImageHeight = 88;
                previewScaler = 1;
                canvasScaler = 5;
                zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
            }
            else if ((myBitmapGDI.Width == 204) && (myBitmapGDI.Height == 88)) //wallset or overlay
            {
                previewImageWidth = 204;
                previewImageHeight = 88;
                previewScaler = 1;
                canvasScaler = 5;
                zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
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
            else if (canvasBackIndex == 3)
            {
                background = magenta;
            }

            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(background).Width, gv.cc.GetFromTileBitmapList(background).Height);
            dst = new IbRect(mapStartLocXinPixels, 0, (int)(drawingSurfaceSize * artScaler), (int)(drawingSurfaceSize * artScaler));
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(background), src, dst);

            int tknWidth = (int)(drawingSurfaceSize * artScaler * ((float)myBitmapGDI.Width / (float)zoomBoxSize));
            int tknHeight = (int)(drawingSurfaceSize * artScaler * ((float)(myBitmapGDI.Height / 2) / (float)zoomBoxSize));
            if (tknWidth > drawingSurfaceSize * artScaler) { tknWidth = (int)(drawingSurfaceSize * artScaler); }
            if (tknHeight > drawingSurfaceSize * artScaler) { tknHeight = (int)(drawingSurfaceSize * artScaler); }

            if (myBitmapGDI.Width == 204)
            {
                src = new IbRect(0, 0, myBitmapGDI.Width, myBitmapGDI.Height);
                if (zoomScaler > 1)
                {
                    src = new IbRect(panSquareX / previewScaler, panSquareY / previewScaler, zoomBoxSize, zoomBoxSize);
                }
                dst = new IbRect(mapStartLocXinPixels, 0, tknWidth, tknHeight);
                gv.DrawBitmap(c, myBitmapGDI, src, dst);
            }
            //if combat token, show idle or attack
            else if ((myBitmapGDI.Width != myBitmapGDI.Height) || (myBitmapGDI.Height == 96))
            {


                if (isIdleLayerShown) //idle layer on top
                {
                    //draw idle at full opacity
                    src = new IbRect(0, 0, myBitmapGDI.Width, myBitmapGDI.Height / 2);
                    if (zoomScaler > 1)
                    {
                        src = new IbRect(panSquareX / previewScaler, panSquareY / previewScaler, zoomBoxSize, zoomBoxSize);
                    }
                    dst = new IbRect(mapStartLocXinPixels, 0, tknWidth, tknHeight);
                    gv.DrawBitmap(c, myBitmapGDI, src, dst);
                }
                else //attack layer on top
                {
                    //draw attack at full opacity
                    src = new IbRect(0, myBitmapGDI.Height / 2, myBitmapGDI.Width, myBitmapGDI.Height / 2);
                    if (zoomScaler > 1)
                    {
                        src = new IbRect(panSquareX / previewScaler, myBitmapGDI.Height / 2 + panSquareY / previewScaler, zoomBoxSize, zoomBoxSize);
                    }
                    dst = new IbRect(mapStartLocXinPixels, 0, tknWidth, tknHeight);
                    gv.DrawBitmap(c, myBitmapGDI, src, dst);
                }
            }
            else
            {
                src = new IbRect(0, 0, myBitmapGDI.Width, myBitmapGDI.Height);
                if (zoomScaler > 1)
                {
                    src = new IbRect(panSquareX / previewScaler, panSquareY / previewScaler, zoomBoxSize, zoomBoxSize);
                }
                dst = new IbRect(mapStartLocXinPixels, 0, tknWidth, tknWidth);
                gv.DrawBitmap(c, myBitmapGDI, src, dst);
            }

            //DRAW GRID
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
            float pixelSize = (drawingSurfaceSize * artScaler * pencilSize / zoomBoxSize);
            if ((myBitmapGDI.Width == 48) && (myBitmapGDI.Height == 192)) //tall creature
            {
                pixelSize = (drawingSurfaceSize * artScaler * pencilSize / zoomBoxSize);
            }
            //int gridSqrSize = (int)(gv.squareSize * gv.scaler * pencilSize);
            //if (zoomScaler > 4) { gridSqrSize = gridSqrSize * 2 * pencilSize; }
            /*for (int x = 0; x < myBitmapGDI.Width; x++)
            {
                for (int y = 0; y < myBitmapGDI.Height; y++)
                {
                    dst = new IbRect(mapStartLocXinPixels + (int)(x * pixelSize), (int)(y * pixelSize), gridSqrSize, gridSqrSize);
                    gv.DrawBitmap(gv.cc.GetFromTileBitmapList("grid_black"), src, dst);
                }
            }*/
            for (int x = 0; x < myBitmapGDI.Height; x++)
            {
                gv.DrawLine(c, mapStartLocXinPixels + (int)(x * pixelSize), 0, mapStartLocXinPixels + (int)(x * pixelSize), (int)(myBitmapGDI.Height * pixelSize / pencilSize), "black", 1);
            }
            for (int y = 0; y < myBitmapGDI.Width; y++)
            {
                gv.DrawLine(c, mapStartLocXinPixels + 0, (int)(y * pixelSize), mapStartLocXinPixels + (int)(myBitmapGDI.Width * pixelSize / pencilSize), (int)(y * pixelSize), "black", 1);
            }

            //DRAW SWAP COLOR LIST
            if (swapColorCoorList.Count > 0)
            {
                for (int x = 0; x < myBitmapGDI.Width; x++)
                {
                    for (int y = 0; y < myBitmapGDI.Height; y++)
                    {
                        foreach (Coordinate cr in swapColorCoorList)
                        {
                            if ((cr.X == x) && (cr.Y == y))
                            {
                                dst = new IbRect(mapStartLocXinPixels + (int)(x * pixelSize / pencilSize), (int)(y * pixelSize / pencilSize), (int)(pixelSize / pencilSize), (int)(pixelSize / pencilSize));
                                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("highlight_magentaTrig"), src, dst);
                            }
                        }
                    }
                }
            }

            //draw border
            int width2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Width;
            int height2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Height;
            src = new IbRect(0, 0, width2, height2);
            dst = new IbRect(0 - (int)((176 * gv.scaler)), 0 - (int)((106 * gv.scaler)), (int)((width2 * gv.scaler) + (gv.scaler * 16)), (int)((height2 * gv.scaler) + (gv.scaler * 10)));
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png"), src, dst);

            //Page Title
            gv.DrawText(c, "IBBASIC ART EDITOR: " + filename, 2 * gv.uiSquareSize, 2 * gv.scaler, "yl");

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
            else if (previewBackIndex == 3)
            {
                background = magenta;
            }

            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(background).Width, gv.cc.GetFromTileBitmapList(background).Height);
            dst = new IbRect((int)previewLocX, (int)previewLocY, (int)(drawingPreviewSize * artScaler), (int)(drawingPreviewSize * artScaler));
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(background), src, dst);


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
            if (myBitmapGDI.Width == 204)
            {
                src = new IbRect(0, 0, myBitmapGDI.Width, myBitmapGDI.Height);
            }
            dst = new IbRect((int)previewLocX, (int)previewLocY, (int)(previewImageWidth * artScaler * previewScaler), (int)(previewImageHeight * artScaler * previewScaler));
            gv.DrawBitmap(c, myBitmapGDI, src, dst);


            //draw selected color square
            //selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
            //updateSelectedColorBitmapDX();
            //myBitmapDX = gv.cc.ConvertGDIBitmapToD2D(myBitmapGDI);
            src = new IbRect(0, 0, 1, 1);
            dst = new IbRect((int)(8.6 * gv.uiSquareSize), (int)(4.5 * gv.uiSquareSize), (int)(0.8 * gv.uiSquareSize), (int)(0.8 * gv.uiSquareSize));
            gv.DrawBitmap(c, selectedColorBitmapGDI, src, dst);

            if (zoomScaler > 1)
            {
                //draw pan selection square highlight
                int tlX = (int)(previewLocX + panSquareX * artScaler);
                int tlY = (int)(previewLocY + panSquareY * artScaler);
                int brX = (int)(drawingPreviewSize * artScaler / zoomScaler);
                int brY = (int)(drawingPreviewSize * artScaler / zoomScaler);
                src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magentaTrig").Width, gv.cc.GetFromTileBitmapList("highlight_magentaTrig").Height);
                dst = new IbRect(tlX, tlY, brX, brY);
                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("highlight_magentaTrig"), src, dst);
            }

            //CONTROLS            
            btnNew.Draw(c);
            btnBucketFill.Draw(c);
            btnSwitchPalette.Draw(c);
            btnTransform.Draw(c);
            btnToggleLayer.Draw(c);
            tglPencil.Draw(c);
            tglSwapColor.Draw(c);
            btnEraser.Draw(c);
            btnGetColor.Draw(c);
            btnCanvasBackground.Draw(c);
            //btnPreviewBackground.Draw();
            btnUndo.Draw(c);
            btnRedo.Draw(c);
            tglZoom.Draw(c);
            palette.Draw(c);

            gv.tsMainMenu.redrawTsMainMenu(c);

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox(c);
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
                        int drawingSurfaceSize = (int)(artSquareSize * 10); //default 480x480 no scale applied
                        int drawingPreviewSize = (int)(artSquareSize * 2); //default 96x96 no scale applied

                        int canvasScaler = 10; //multiply image size by this to fill canvas
                        int previewScaler = 2; //multiply image size by this to fill preview

                        int previewImageWidth = artSquareSize; //draw image size in default screen size
                        int previewImageHeight = artSquareSize; //draw image size in default screen size

                        float previewLocX = 8.25f * gv.uiSquareSize;
                        float previewLocY = 5.5f * gv.uiSquareSize;

                        int zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);

                        //int drawingSurfaceSize = gv.squareSize * 10 * gv.scaler;
                        //int drawingPreviewSize = gv.squareSize * 2 * gv.scaler;
                        //int zoomBoxSize = drawingPreviewSize / zoomScaler;
                        float pixelSize = (drawingSurfaceSize / zoomBoxSize) * artScaler;
                        //int previewImageWidth = gv.squareSize * 1 * gv.scaler;
                        //int previewImageHeight = gv.squareSize * 1 * gv.scaler;
                        //float previewLocX = 8.25f * gv.uiSquareSize;
                        //float previewLocY = 5.5f * gv.uiSquareSize;
                        //int previewScaler = 2;

                        float gridX = (((float)(eX - mapStartLocXinPixels) / (float)pixelSize) + (panSquareX / previewScaler));
                        float gridY = (((float)eY / (float)pixelSize) + (panSquareY / previewScaler));

                        if ((myBitmapGDI.Width == 48) && (myBitmapGDI.Height == 96)) //normal
                        {
                            previewImageWidth = artSquareSize;
                            previewImageHeight = artSquareSize;
                            previewScaler = 2;
                            canvasScaler = 10;
                            zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
                            pixelSize = (drawingSurfaceSize / zoomBoxSize) * artScaler;
                            gridX = (((float)(eX - mapStartLocXinPixels) / (float)pixelSize) + (panSquareX / previewScaler));
                            gridY = (((float)eY / (float)pixelSize) + (panSquareY / previewScaler));
                        }
                        else if ((myBitmapGDI.Width == 96) && (myBitmapGDI.Height == 96)) //wide
                        {
                            previewImageWidth = artSquareSize * 2;
                            previewImageHeight = artSquareSize;
                            previewScaler = 1;
                            canvasScaler = 5;
                            zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
                            pixelSize = (drawingSurfaceSize / zoomBoxSize) * artScaler;
                            gridX = (((float)(eX - mapStartLocXinPixels) / (float)pixelSize) + (panSquareX / previewScaler));
                            gridY = (((float)eY / (float)pixelSize) + (panSquareY / previewScaler));
                        }
                        else if ((myBitmapGDI.Width == 48) && (myBitmapGDI.Height == 192)) //tall
                        {
                            previewImageWidth = artSquareSize;
                            previewImageHeight = artSquareSize * 2;
                            previewScaler = 1;
                            canvasScaler = 5;
                            zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
                            pixelSize = (drawingSurfaceSize / zoomBoxSize) * artScaler;
                            gridX = (((float)(eX - mapStartLocXinPixels) / (float)pixelSize) + (panSquareX / previewScaler));
                            gridY = (((float)eY / (float)pixelSize) + (panSquareY / previewScaler));
                        }
                        else if ((myBitmapGDI.Width == 96) && (myBitmapGDI.Height == 192)) //large
                        {
                            previewImageWidth = artSquareSize * 2;
                            previewImageHeight = artSquareSize * 2;
                            previewScaler = 1;
                            canvasScaler = 5;
                            zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
                            pixelSize = (drawingSurfaceSize / zoomBoxSize) * artScaler;
                            gridX = (((float)(eX - mapStartLocXinPixels) / (float)pixelSize) + (panSquareX / previewScaler));
                            gridY = (((float)eY / (float)pixelSize) + (panSquareY / previewScaler));
                        }
                        else if ((myBitmapGDI.Width == 88) && (myBitmapGDI.Height == 88)) //backdrop
                        {
                            previewImageWidth = 88;
                            previewImageHeight = 88;
                            previewScaler = 1;
                            canvasScaler = 5;
                            zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
                            pixelSize = (drawingSurfaceSize / zoomBoxSize) * artScaler;
                            gridX = (((float)(eX - mapStartLocXinPixels) / (float)pixelSize) + (panSquareX / previewScaler));
                            gridY = (((float)eY / (float)pixelSize) + (panSquareY / previewScaler));
                        }
                        else if ((myBitmapGDI.Width == 204) && (myBitmapGDI.Height == 88)) //wallset or overlay
                        {
                            previewImageWidth = 204;
                            previewImageHeight = 88;
                            previewScaler = 1;
                            canvasScaler = 5;
                            zoomBoxSize = drawingPreviewSize / (zoomScaler * previewScaler);
                            pixelSize = (drawingSurfaceSize / zoomBoxSize) * artScaler;
                            gridX = (((float)(eX - mapStartLocXinPixels) / (float)pixelSize) + (panSquareX / previewScaler));
                            gridY = (((float)eY / (float)pixelSize) + (panSquareY / previewScaler));
                        }

                        if (tapInPreviewViewport(x, y))
                        {
                            panSquareX = (int)((x - previewLocX) / artScaler) - (int)(zoomBoxSize * previewScaler / 2);
                            panSquareY = (int)((y - previewLocY) / artScaler) - (int)(zoomBoxSize * previewScaler / 2);
                            panSquareX = panSquareX / 2;
                            panSquareX = panSquareX * 2;
                            panSquareY = panSquareY / 2;
                            panSquareY = panSquareY * 2;


                            if (panSquareX < 0) { panSquareX = 0; }
                            if (panSquareX > (int)(previewImageWidth * previewScaler - (zoomBoxSize * previewScaler))) { panSquareX = (int)(previewImageWidth * previewScaler - (zoomBoxSize * previewScaler)); }
                            if (panSquareY < 0) { panSquareY = 0; }
                            if (panSquareY > (int)(previewImageHeight * previewScaler - (zoomBoxSize * previewScaler))) { panSquareY = (int)(previewImageHeight * previewScaler - (zoomBoxSize * previewScaler)); }
                        }

                    if (tglSwapColor.toggleOn)
                    {
                        if (tapInMapViewport(x, y))
                        {

                        }
                        else if (palette.getImpact(x, y))
                        {
                            palette.getColorClicked(x, y);
                            selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
                            //updateSelectedColorBitmapDX();
                            foreach (Coordinate crd in swapColorCoorList)
                            {
                                myBitmapGDI.SetPixel(crd.X, crd.Y, currentColor);
                            }
                            //updateBitmapDX();
                            //myBitmapDX = gv.cc.ConvertGDIBitmapToD2D(myBitmapGDI);
                        }
                    }
                    else if (getColorMode)
                    {
                        //check to see if in drawing area first
                        if (tapInMapViewport(x, y))
                        {
                            //check to see if combat token
                            if ((myBitmapGDI.Width != myBitmapGDI.Height) || (myBitmapGDI.Height == 96)) //combat token so get color from active layer
                            {
                                if (isIdleLayerShown) //get color from idle layer
                                {
                                currentColor = myBitmapGDI.GetPixel((int)(gridX), (int)(gridY));
                                selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
                                    //updateSelectedColorBitmapDX();
                                }
                                else //get color from attack layer
                                {
                                currentColor = myBitmapGDI.GetPixel((int)(gridX), (int)(gridY) + myBitmapGDI.Height / 2);
                                selectedColorBitmapGDI.SetPixel(0, 0, currentColor);
                                    //updateSelectedColorBitmapDX();
                                }
                            }
                            else //not a combat token
                            {
                            currentColor = myBitmapGDI.GetPixel((int)(gridX), (int)(gridY));
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
                            int pGridX = (int)(gridX);
                            int pGridY = (int)(gridY);
                            if (pencilSize == 2)
                            {
                                pGridX = (int)(gridX / 2);
                                pGridY = (int)(gridY / 2);
                                pGridX = pGridX * 2;
                                pGridY = pGridY * 2;
                            }
                            if ((myBitmapGDI.Width != myBitmapGDI.Height) || (myBitmapGDI.Height == 96))//combat token so draw on active layer
                            {
                                if (isIdleLayerShown) //draw on idle layer
                                {
                                    myBitmapGDI.SetPixel(pGridX, pGridY, currentColor);
                                    if (pencilSize == 2)
                                    {
                                        myBitmapGDI.SetPixel(pGridX + 1, pGridY, currentColor);
                                        myBitmapGDI.SetPixel(pGridX, pGridY + 1, currentColor);
                                        myBitmapGDI.SetPixel(pGridX + 1, pGridY + 1, currentColor);
                                    }
                                }
                                else //draw on attack layer
                                {
                                    myBitmapGDI.SetPixel(pGridX, pGridY + myBitmapGDI.Height / 2, currentColor);
                                    if (pencilSize == 2)
                                    {
                                        myBitmapGDI.SetPixel(pGridX + 1, pGridY + myBitmapGDI.Height / 2, currentColor);
                                        myBitmapGDI.SetPixel(pGridX, pGridY + 1 + myBitmapGDI.Height / 2, currentColor);
                                        myBitmapGDI.SetPixel(pGridX + 1, pGridY + 1 + myBitmapGDI.Height / 2, currentColor);
                                    }
                                }
                            }
                            else //not combat token
                            {
                                myBitmapGDI.SetPixel(pGridX, pGridY, currentColor);
                                if (pencilSize == 2)
                                {
                                    myBitmapGDI.SetPixel(pGridX + 1, pGridY, currentColor);
                                    myBitmapGDI.SetPixel(pGridX, pGridY + 1, currentColor);
                                    myBitmapGDI.SetPixel(pGridX + 1, pGridY + 1, currentColor);
                                }
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

                    if (btnTransform.getImpact(x, y))
                    {
                        doTransform();
                    }
                    else if (btnSwitchPalette.getImpact(x, y))
                    {
                        if (colorPaletteListIndex >= colorPaletteList.Count - 1)
                        {
                            colorPaletteListIndex = 0;
                        }
                        else
                        {
                            colorPaletteListIndex++;
                        }
                        palette.Img = colorPaletteList[colorPaletteListIndex];
                        palette.bmpGDI = gv.cc.LoadBitmap(palette.Img);
                    }
                    else if (btnBucketFill.getImpact(x, y))
                    {
                        pushToUndoStack();
                        doBucketFill();
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
                        doDiskUtilities();                        
                    }
                    else if (tglSwapColor.getImpact(x, y))
                    {
                        tglSwapColor.toggleOn = !tglSwapColor.toggleOn;
                        if (tglSwapColor.toggleOn)
                        {
                            swapColorCoorList.Clear();
                            for (int xx = 0; xx < myBitmapGDI.Width; xx++)
                            {
                                for (int yy = 0; yy < myBitmapGDI.Height; yy++)
                                {
                                    if (myBitmapGDI.GetPixel(xx, yy) == currentColor)
                                    {
                                        swapColorCoorList.Add(new Coordinate(xx, yy));
                                    }
                                }
                            }
                        }
                        else
                        {
                            swapColorCoorList.Clear();
                        }
                        //pushToUndoStack();
                        //doAlphaAdjust();
                    }
                    else if (btnToggleLayer.getImpact(x, y))
                    {
                        if (isIdleLayerShown)
                        {
                            isIdleLayerShown = false;
                            btnToggleLayer.Img = "tgl_attack_on";
                        }
                        else
                        {
                            isIdleLayerShown = true;
                            btnToggleLayer.Img = "tgl_idle_on";
                        }
                    }
                    else if (tglPencil.getImpact(x, y))
                    {
                        if (pencilSize == 1)
                        {
                            pencilSize = 2;
                            tglPencil.Text = "2PX";
                        }
                        else
                        {
                            pencilSize = 1;
                            tglPencil.Text = "1PX";
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
                        if (canvasBackIndex > 3) { canvasBackIndex = 0; }
                        previewBackIndex++;
                        if (previewBackIndex > 3) { previewBackIndex = 0; }
                    }
                    //else if (btnPreviewBackground.getImpact(x, y))
                    //{
                    //    previewBackIndex++;
                    //    if (previewBackIndex > 2) { previewBackIndex = 0; }
                    //}
                    else if (tglZoom.getImpact(x, y))
                    {
                        upperSquareX = 0;
                        upperSquareY = 0;
                        panSquareX = 0;
                        panSquareY = 0;
                        if (zoomScaler == 1)
                        {
                            zoomScaler = 2;
                            tglZoom.Text = "2X";
                        }
                        else if (zoomScaler == 2)
                        {
                            zoomScaler = 4;
                            tglZoom.Text = "4X";
                        }
                        else if (zoomScaler == 4)
                        {
                            zoomScaler = 8;
                            tglZoom.Text = "8X";
                        }
                        else if (zoomScaler == 8)
                        {
                            zoomScaler = 1;
                            tglZoom.Text = "1X";
                        }
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

        public bool tapInPreviewViewport(int x, int y)
        {
            if (x < (int)(8.25 * gv.uiSquareSize)) { return false; }
            if (y < (int)(5.5 * gv.uiSquareSize)) { return false; }
            if (x > (int)(8.25 * gv.uiSquareSize) + (int)(gv.uiSquareSize * 1.5)) { return false; }
            if (y > (int)(5.5 * gv.uiSquareSize) + (int)(gv.uiSquareSize * 1.5)) { return false; }
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
            bmpGDI = gv.cc.LoadBitmap("color_palette2");
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
        public void Draw(SKCanvas c)
        {
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList(Img).Width, gv.cc.GetFromTileBitmapList(Img).Height);
            dst = new IbRect(this.X, this.Y, this.Width, this.Height);
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(Img), src, dst);

            //draw selected info tile highlight
            int tlX = this.X + lastClickedColorLocation.X;
            int tlY = this.Y + lastClickedColorLocation.Y;
            int brX = this.Width / 9;
            int brY = this.Width / 9;
            src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("highlight_magenta").Width, gv.cc.GetFromTileBitmapList("highlight_magenta").Height);
            dst = new IbRect(tlX, tlY, brX, brY);
            if ((lastClickedColorLocation.X > (5 * this.Width / 20)) && (lastClickedColorLocation.X < (12 * this.Width / 20)))
            {
                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("highlight_magenta"), src, dst);
            }
            else
            {
                gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("highlight_greenTrig"), src, dst);
            }
        }
    }
}
