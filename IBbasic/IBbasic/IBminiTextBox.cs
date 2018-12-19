﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    public class IBminiTextBox
    {
        public GameView gv;
        public List<string> tagStack = new List<string>();
        public List<IBminiFormattedLine> linesList = new List<IBminiFormattedLine>();
        public int tbHeight = 200;
        public int tbWidth = 300;
        public int tbXloc = 10;
        public int tbYloc = 10;
        public bool showBoxBorder = false;
        public bool showShadow = false;

        public IBminiTextBox(GameView g, int locX, int locY, int width, int height)
        {
            gv = g;
            tbXloc = locX;
            tbYloc = locY;
            tbWidth = width;
            tbHeight = height;
        }
        public IBminiTextBox(GameView g)
        {
            gv = g;
        }

        public void DrawString(SKCanvas c, string text, float x, float y, string fontColor)
        {
            if ((y > -2) && (y <= tbHeight - gv.fontHeight))
            {
                if (showShadow)
                {
                    for (int xx = 0; xx <= 2; xx++)
                    {
                        for (int yy = 0; yy <= 2; yy++)
                        {
                            gv.DrawText(c, text, x + tbXloc + xx, y + tbYloc + yy, "bk");
                        }
                    }
                }
                gv.DrawText(c, text, x + tbXloc, y + tbYloc, fontColor);
            }
        }

        public void AddFormattedTextToTextBox(string formattedText)
        {
            formattedText = formattedText.Replace("\r\n", "<br>");
            formattedText = formattedText.Replace("\n\n", "<br>");
            formattedText = formattedText.Replace("\"", "'");

            if ((formattedText.EndsWith("<br>")) || (formattedText.EndsWith("<BR>")))
            {
                List<IBminiFormattedLine> lnList = gv.cc.ProcessHtmlString(formattedText, tbWidth, tagStack, true);
                foreach (IBminiFormattedLine fl in lnList)
                {
                    linesList.Add(fl);
                }
            }
            else
            {
                List<IBminiFormattedLine> lnList = gv.cc.ProcessHtmlString(formattedText + "<br>", tbWidth, tagStack, true);
                foreach (IBminiFormattedLine fl in lnList)
                {
                    linesList.Add(fl);
                }
            }
        }
        
        public void onDrawTextBox(SKCanvas c)
        {
            //only draw lines needed to fill textbox
            float xLoc = 0;
            float yLoc = 0;
            int linesToPrint = this.tbHeight / (gv.fontHeight + gv.fontLineSpacing);
            //loop through 5 lines from current index point
            for (int i = 0; i < linesList.Count; i++)
            {
                //loop through each line and print each word
                foreach (IBminiFormattedWord word in linesList[i].wordsList)
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
