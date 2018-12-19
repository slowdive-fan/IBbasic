using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBbasic
{
    public class IBbasicToolTip
    {
        public GameView gv;
        public List<string> tagStack = new List<string>();
        public List<IBminiFormattedLine> logLinesList = new List<IBminiFormattedLine>();
        public int currentLocX = 0;
        public int currentLocY = 0;
        public int Width = 192;
        public int Height = 120;
        public int tbHeight = 240;
        public int tbWidth = 192;
        public int tbXloc = 10;

        public IBbasicToolTip(GameView g)
        {
            gv = g;
        }

        public void DrawString(SKCanvas canvas, string text, float x, float y, string fontColor)
        {
            if ((y > -2) && (y <= (tbHeight * gv.scaler) - gv.fontHeight))
            {
                gv.DrawText(canvas, text, x + tbXloc + gv.pS, y, fontColor);
            }
        }

        public void AddHtmlTextToLog(string htmlText)
        {
            logLinesList.Clear();
            //Remove any '\r\n' hard returns from message
            htmlText = htmlText.Replace("\r\n", "<br>");
            htmlText = htmlText.Replace("\n\n", "<br>");
            htmlText = htmlText.Replace("\"", "'");

            if ((htmlText.EndsWith("<br>")) || (htmlText.EndsWith("<BR>")))
            {
                List<IBminiFormattedLine> lnList = gv.cc.ProcessHtmlString(htmlText, (int)(tbWidth * gv.scaler), tagStack, true);
                foreach (IBminiFormattedLine fl in lnList)
                {
                    logLinesList.Add(fl);
                }
            }
            else
            {
                List<IBminiFormattedLine> lnList = gv.cc.ProcessHtmlString(htmlText + "<br>", (int)(tbWidth * gv.scaler), tagStack, true);
                foreach (IBminiFormattedLine fl in lnList)
                {
                    logLinesList.Add(fl);
                }
            }
        }
        public void onDrawLogBox(SKCanvas canvas)
        {
            currentLocX = 2 * gv.uiSquareSize;
            currentLocY = 60;
            IbRect src = new IbRect(0, 0, gv.cc.GetFromBitmapList("ui_bg_tooltip").Width, gv.cc.GetFromBitmapList("ui_bg_tooltip").Height);
            IbRect dst = new IbRect((int)(currentLocX), (int)(currentLocY * gv.scaler), (int)(Width * gv.scaler), (int)(Height * gv.scaler));
            gv.DrawBitmap(canvas, gv.cc.GetFromBitmapList("ui_bg_tooltip"), src, dst);

            //only draw lines needed to fill textbox
            float xLoc = 0.0f;
            float yLoc = 15.0f;
            for (int i = 0; i < logLinesList.Count; i++)
            {
                //loop through each line and print each word
                foreach (IBminiFormattedWord word in logLinesList[i].wordsList)
                {
                    int xLoc2 = (int)((currentLocX + xLoc));
                    int yLoc2 = (int)((currentLocY * gv.scaler + yLoc));
                    DrawString(canvas, word.text + " ", xLoc2, yLoc2, word.color);
                    xLoc += (word.text.Length + 1) * (gv.fontWidth + gv.fontCharSpacing);
                }
                xLoc = 0;
                yLoc += gv.fontHeight + gv.fontLineSpacing;
            }
        }
    }
}
