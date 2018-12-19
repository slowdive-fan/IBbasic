using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class FloatyText
    {
        public Coordinate location = new Coordinate();
        public string value = "";
        public string color = "red"; //red, yellow, blue, green, white
        public int timer = 0;
        public int timerLength = 4000; //time in ms
        public int timeToLive = 2000;
        public int z = 0; //float height multiplier

        public GameView gv;
        public List<string> tagStack = new List<string>();
        public List<IBminiFormattedLine> linesList = new List<IBminiFormattedLine>();
        public int tbHeight = 200;
        public int tbWidth = 300;
        public bool showShadow = false;

        public FloatyText()
        {

        }
        public FloatyText(int X, int Y, string val)
        {
            location = new Coordinate(X, Y);
            value = val;
            color = "red";
        }
        public FloatyText(int X, int Y, string val, string clr, int length)
        {
            location = new Coordinate(X, Y);
            value = val;
            color = clr;
            timerLength = length;
            timeToLive = length;
        }
        public FloatyText(Coordinate coor, string val)
        {
            location = coor;
            value = val;
            color = "red";
        }
        public FloatyText(Coordinate coor, string val, string clr)
        {
            location = coor;
            value = val;
            color = clr;
        }

        public void DrawString(SKCanvas canvas, string text, float x, float y, string fontColor)
        {
            //if ((y > -2) && (y <= tbHeight - gv.fontHeight))
            //{
                if (showShadow)
                {
                    for (int xx = 0; xx <= 2; xx++)
                    {
                        for (int yy = 0; yy <= 2; yy++)
                        {
                            gv.DrawText(canvas, text, x + xx, y + yy, "bk");
                        }
                    }
                }
                string colr = "wh";
                if (color.Equals("yellow"))
                {
                    colr = "yl";
                }
                else if (color.Equals("blue"))
                {
                    colr = "bu";
                }
                else if (color.Equals("green"))
                {
                    colr = "gn";
                }
                else if (color.Equals("red"))
                {
                    colr = "rd";
                }
                gv.DrawText(canvas, text, x , y , colr);
            //}
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

            timerLength = timerLength * linesList.Count / 2; 
            timeToLive = timeToLive * linesList.Count / 2;
        }

        public void onDrawTextBox(SKCanvas canvas)
        {
            //location.X should be the the props actual map location in squares (not screen location)            
            //only draw lines needed to fill textbox
            float xLoc = (location.X + gv.playerOffsetX - 2 - gv.mod.PlayerLocationX) * (gv.squareSize * gv.scaler) + gv.screenMainMap.mapStartLocXinPixels;
            float yLoc = ((location.Y + gv.playerOffsetY + 2 - gv.mod.PlayerLocationY) * (gv.squareSize * gv.scaler)) - (z);
            //loop through all lines from current index point
            for (int i = 0; i < linesList.Count; i++)
            {
                //loop through each line and print each word
                foreach (IBminiFormattedWord word in linesList[i].wordsList)
                {
                    DrawString(canvas, word.text + " ", xLoc, yLoc, word.color);
                    xLoc += (word.text.Length + 1) * (gv.fontWidth + gv.fontCharSpacing);
                }
                xLoc = (location.X + gv.playerOffsetX - 2 - gv.mod.PlayerLocationX) * (gv.squareSize * gv.scaler) + gv.screenMainMap.mapStartLocXinPixels;
                yLoc += gv.fontHeight + gv.fontLineSpacing;
            }
        }
    }
}
