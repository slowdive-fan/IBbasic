using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class IbbHtmlTextBox
    {
        public GameView gv;
        public List<string> tagStack = new List<string>();
        public List<IBminiFormattedLine> logLinesList = new List<IBminiFormattedLine>();
        public int tbHeight = 200;
        public int tbWidth = 300;
        public int tbXloc = 10;
        public int tbYloc = 10;
        public float fontHeightToWidthRatio = 1.0f;
        public bool showBoxBorder = false;

        public IbbHtmlTextBox(GameView g, int locX, int locY, int width, int height)
        {
            gv = g;
            tbXloc = locX;
            tbYloc = locY;
            tbWidth = width;
            tbHeight = height;
        }
        public IbbHtmlTextBox(GameView g)
        {
            gv = g;
        }

        public void DrawBitmap(SKCanvas c, SKBitmap bmp, int x, int y)
        {
            IbRect src = new IbRect(0, 0, bmp.Width, bmp.Height);
            IbRect dst = new IbRect(x + tbXloc, y + tbYloc, bmp.Width, bmp.Height);
            gv.DrawBitmap(c, bmp, src, dst);
        }
        public void DrawString(SKCanvas c, string text, float x, float y, string fontColor)
        {
            if ((y > -2) && (y <= tbHeight - gv.fontHeight))
            {
                gv.DrawText(c, text, x + tbXloc, y + tbYloc, fontColor);
            }
        }

        public void AddHtmlTextToLog(string htmlText)
        {            
            htmlText = htmlText.Replace("\r\n", "<br>");
            htmlText = htmlText.Replace("\n\n", "<br>");
            htmlText = htmlText.Replace("\"", "'");

            if ((htmlText.EndsWith("<br>")) || (htmlText.EndsWith("<BR>")))
            {
                List<IBminiFormattedLine> linesList = gv.cc.ProcessHtmlString(htmlText, tbWidth, tagStack, true);
                foreach (IBminiFormattedLine fl in linesList)
                {
                    logLinesList.Add(fl);
                }
            }
            else
            {
                List<IBminiFormattedLine> linesList = gv.cc.ProcessHtmlString(htmlText + "<br>", tbWidth, tagStack, true);
                foreach (IBminiFormattedLine fl in linesList)
                {
                    logLinesList.Add(fl);
                }
            }
        }
        
        public void onDrawLogBox(SKCanvas c)
        {
            //only draw lines needed to fill textbox
            float xLoc = 0;
            float yLoc = 0;
            //loop through 5 lines from current index point
            for (int i = 0; i < logLinesList.Count; i++)
            {
                //loop through each line and print each word
                foreach (IBminiFormattedWord word in logLinesList[i].wordsList)
                {
                    DrawString(c, word.text + " ", xLoc, yLoc, word.color);
                    xLoc += (word.text.Length + 1) * (gv.fontWidth + gv.fontCharSpacing);
                }
                xLoc = 0;
                yLoc += gv.fontHeight + gv.fontLineSpacing;
            }

            //draw border for debug info
            if (showBoxBorder)
            {
                gv.DrawRectangle(c, new IbRect(tbXloc, tbYloc, tbWidth, tbHeight), SKColors.DimGray, 1);
            }
        }
    }
}
