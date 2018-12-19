using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    public class IbbToggle
    {
        public GameView gv;
        public string ImgOn = "";
        public string ImgOff = "";
        public bool toggleOn = false;
        public int X = 0;
        public int Y = 0;
        public int Width = 0;
        public int Height = 0;
        public bool show = true;

        public IbbToggle(GameView g)
        {
            gv = g;
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

        public void Draw(SKCanvas c)
        {
            if (show)
            {
                IbRect src = new IbRect(0, 0, gv.cc.GetFromBitmapList(ImgOn).Width, gv.cc.GetFromBitmapList(ImgOn).Height);
                IbRect dst = new IbRect(this.X, this.Y, (int)((float)this.Width), (int)((float)this.Height));                
                if (toggleOn)
                {
                    gv.DrawBitmap(c, gv.cc.GetFromBitmapList(ImgOn), src, dst);
                }
                else
                {
                    gv.DrawBitmap(c, gv.cc.GetFromBitmapList(ImgOff), src, dst);
                }
            }
        }
    }
}
