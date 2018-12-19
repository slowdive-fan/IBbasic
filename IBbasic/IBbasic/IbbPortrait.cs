using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class IbbPortrait
    {
        //this class is handled differently than Android version
        public string ImgBG = null;
        public string Img = null;
        public string ImgLU = null; //used for level up icon
        public string Glow = null;
        public bool glowOn = false;
        public bool levelUpOn = false;
        public string TextHP = "";
        public string TextSP = "";
        public int X = 0;
        public int Y = 0;
        public int Width = 0;
        public int Height = 0;
        public float scaler = 1.0f;
        public bool playedHoverSound = false;
        public bool show = false;
        public GameView gv;

        public IbbPortrait(GameView g, float sc)
        {
            gv = g;
            scaler = sc;
        }

        public bool getImpact(int x, int y)
        {
            if (show)
            {
                if ((x >= X) && (x <= (X + this.Width)))
                {
                    if ((y >= Y) && (y <= (Y + this.Height)))
                    {
                        if (!playedHoverSound)
                        {
                            playedHoverSound = true;
                            //gv.playerButtonEnter.Play();
                        }
                        return true;
                    }
                }
                playedHoverSound = false;
            }
            return false;
        }

        public void Draw(SKCanvas c)
        {
            if (show)
            {
                int pH = (int)((float)gv.screenHeight / 200.0f);
                int pW = (int)((float)gv.screenHeight / 200.0f);
                float fSize = (float)(gv.squareSize / 4) * scaler;

                IbRect src = new IbRect(0, 0, gv.cc.GetFromBitmapList(ImgBG).Width, gv.cc.GetFromBitmapList(ImgBG).Width);
                IbRect src2 = new IbRect(0, 0, 0, 0);
                IbRect src3 = new IbRect(0, 0, 0, 0);
                IbRect dstLU = new IbRect(0, 0, 0, 0);

                if (this.Img != null)
                {
                    src2 = new IbRect(0, 0, gv.cc.GetFromBitmapList(Img).Width, gv.cc.GetFromBitmapList(Img).Width);
                }
                if (this.ImgLU != null)
                {
                    src3 = new IbRect(0, 0, gv.cc.GetFromBitmapList(ImgLU).Width, gv.cc.GetFromBitmapList(ImgLU).Width);
                }
                IbRect dstBG = new IbRect(this.X - (int)(1 * gv.screenDensity),
                                            this.Y - (int)(1 * gv.screenDensity),
                                            (int)((float)this.Width) + (int)(2 * gv.screenDensity),
                                            (int)((float)this.Height) + (int)(2 * gv.screenDensity));
                IbRect dst = new IbRect(this.X, this.Y, (int)((float)this.Width), (int)((float)this.Height));
                if (this.ImgLU != null)
                {
                    dstLU = new IbRect(this.X, this.Y, gv.cc.GetFromBitmapList(ImgLU).Width, gv.cc.GetFromBitmapList(ImgLU).Width);
                }
                IbRect srcGlow = new IbRect(0, 0, gv.cc.GetFromBitmapList(Glow).Width, gv.cc.GetFromBitmapList(Glow).Width);
                IbRect dstGlow = new IbRect(this.X - (int)(2 * gv.screenDensity),
                                            this.Y - (int)(2 * gv.screenDensity),
                                            (int)((float)this.Width) + (int)(4 * gv.screenDensity),
                                            (int)((float)this.Height) + (int)(4 * gv.screenDensity));

                
                if ((this.glowOn) && (this.Glow != null))
                {
                    gv.DrawBitmap(c, gv.cc.GetFromBitmapList(Glow), srcGlow, dstGlow);
                }

                gv.DrawBitmap(c, gv.cc.GetFromBitmapList(ImgBG), src, dstBG);

                if (this.Img != null)
                {
                    gv.DrawBitmap(c, gv.cc.GetFromBitmapList(Img), src2, dst);
                }

                if (this.ImgLU != null)
                {
                    if (levelUpOn)
                    {
                        gv.DrawBitmap(c, gv.cc.GetFromBitmapList(ImgLU), src3, dstLU);
                    }
                }

                /*if (gv.mod.useUIBackground)
                {
                    IbRect srcFrame = new IbRect(0, 0, gv.cc.ui_portrait_frame.Width, gv.cc.ui_portrait_frame.Height);
                    IbRect dstFrame = new IbRect(this.X - (int)(1 * gv.screenDensity),
                                            this.Y - (int)(1 * gv.screenDensity),
                                            (int)((float)this.Width) + (int)(2 * gv.screenDensity),
                                            (int)((float)this.Height) + (int)(2 * gv.screenDensity));
                    gv.DrawBitmap(gv.cc.ui_portrait_frame, srcFrame, dstFrame);
                }*/

                //DRAW HP/HPmax
                int ulX = pW * 0;
                int ulY = this.Height - (gv.fontHeight * 2);

                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 2; y++)
                    {
                        gv.DrawText(c, TextHP, this.X + ulX + x, this.Y + ulY - pH + y, "bk");
                    }
                }
                gv.DrawText(c, TextHP, this.X + ulX, this.Y + ulY - pH, "gn");


                //DRAW SP/SPmax
                ulX = pW * 1;
                ulY = this.Height - (gv.fontHeight * 1);

                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 2; y++)
                    {
                        gv.DrawText(c, TextSP, this.X + ulX - pW + x, this.Y + ulY - pH + y, "bk");
                    }
                }
                gv.DrawText(c, TextSP, this.X + ulX - pW, this.Y + ulY - pH, "yl");
            }
        }
    }
}
