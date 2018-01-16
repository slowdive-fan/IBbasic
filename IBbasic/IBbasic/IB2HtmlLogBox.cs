using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class IB2HtmlLogBox
    {
        [JsonIgnore]
        public GameView gv;
        public string tag = "";
        public List<string> tagStack = new List<string>();
        public List<IBminiFormattedLine> logLinesList = new List<IBminiFormattedLine>();
        public int currentTopLineIndex = 0;
        public int numberOfLinesToShow = 43;
        public float xLoc = 0;
        public int startY = 0;
        public int moveDeltaY = 0;
        public int tbHeight = 600;
        public int tbWidth = 400;
        public int tbXloc = 10;
        public int tbYloc = 10;
        //public int pnlLocX = 0;
        //public int pnlLocY = 0;
        public float fontHeightToWidthRatio = 1.0f;
        public bool touchIsDown = false;
        public int touchMoveDeltaY = 0;
        public int lastTouchMoveLocationY = 0;

        public IB2HtmlLogBox()
        {

        }

        public IB2HtmlLogBox(GameView g)
        {
            gv = g;
        }

        public void setupIB2HtmlLogBox(GameView g)
        {
            gv = g;
            numberOfLinesToShow = (int)((tbHeight) / (gv.fontHeight + gv.fontLineSpacing)) - 1;
        }

        public void DrawString(string text, float x, float y, string fontColor)
        {
            if ((y > -2) && (y <= (tbHeight) - gv.fontHeight))
            {
                gv.DrawText(text, x + gv.pS, y, fontColor);                
            }
        }

        public void AddHtmlTextToLog(string htmlText)
        {
            //Remove any '\r\n' hard returns from message
            htmlText = htmlText.Replace("\r\n", "<br>");
            htmlText = htmlText.Replace("\n\n", "<br>");
            htmlText = htmlText.Replace("\"", "'");

            if ((htmlText.EndsWith("<br>")) || (htmlText.EndsWith("<BR>")))
            {
                List<IBminiFormattedLine> lnList = gv.cc.ProcessHtmlString(htmlText, (int)(tbWidth + gv.fontWidth), tagStack, true);
                foreach (IBminiFormattedLine fl in lnList)
                {
                    logLinesList.Add(fl);
                }
            }
            else
            {
                List<IBminiFormattedLine> lnList = gv.cc.ProcessHtmlString(htmlText + "<br>", (int)(tbWidth + gv.fontWidth), tagStack, true);
                foreach (IBminiFormattedLine fl in lnList)
                {
                    logLinesList.Add(fl);
                }
            }
            scrollToEnd();
        }
        public void onDrawLogBox()
        {
            numberOfLinesToShow = (int)((tbHeight) / (gv.fontHeight + gv.fontLineSpacing)) - 1;
            //ratio of #lines to #pixels
            float ratio = (float)(logLinesList.Count) / (float)(tbHeight);
            if (ratio < 1.0f) { ratio = 1.0f; }
            if (moveDeltaY != 0)
            {
                int lineMove = (startY + moveDeltaY) * (int)ratio;
                SetCurrentTopLineAbsoluteIndex(lineMove);
            }
            //only draw lines needed to fill textbox
            float xLoc = 0.0f;
            float yLoc = 15.0f;
            int maxLines = currentTopLineIndex + numberOfLinesToShow;
            
            if (maxLines > logLinesList.Count) { maxLines = logLinesList.Count; }
            for (int i = currentTopLineIndex; i < maxLines; i++)
            {
                //loop through each line and print each word
                foreach (IBminiFormattedWord word in logLinesList[i].wordsList)
                {
                    int xLoc2 = (int)((this.tbXloc + xLoc));
                    int yLoc2 = (int)((this.tbYloc + yLoc));
                    DrawString(word.text + " ", xLoc2, yLoc2, word.color);
                    xLoc += (word.text.Length + 1) * (gv.fontWidth + gv.fontCharSpacing);
                }
                xLoc = 0;
                yLoc += gv.fontHeight + gv.fontLineSpacing;
            }
        }

        public void scrollToEnd()
        {
            SetCurrentTopLineIndex(logLinesList.Count);
        }
        public void SetCurrentTopLineIndex(int changeValue)
        {
            currentTopLineIndex += changeValue;
            if (currentTopLineIndex > logLinesList.Count - numberOfLinesToShow)
            {
                currentTopLineIndex = logLinesList.Count - numberOfLinesToShow;
            }
            if (currentTopLineIndex < 0)
            {
                currentTopLineIndex = 0;
            }
        }
        public void SetCurrentTopLineAbsoluteIndex(int absoluteValue)
        {
            currentTopLineIndex = absoluteValue;
            if (currentTopLineIndex > logLinesList.Count - numberOfLinesToShow)
            {
                currentTopLineIndex = logLinesList.Count - numberOfLinesToShow;
            }
            if (currentTopLineIndex < 0)
            {
                currentTopLineIndex = 0;
            }
        }
        private bool isTouchWithinTextBox(int eX, int eY)
        {
            if ((eX > (int)((tbXloc)))
                    && (eX < (int)(tbWidth) + (int)((tbXloc)))
                    && (eY > (int)((tbYloc)))
                    && (eY < (int)(tbHeight) + (int)((tbYloc))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void onTouchSwipe(int eX, int eY, MouseEventType.EventType eventType)
        {            
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:

                    if (isTouchWithinTextBox(eX, eY))
                    {
                        touchIsDown = true;
                        lastTouchMoveLocationY = eY;
                        touchMoveDeltaY = 0;
                    }
                    break;

                case MouseEventType.EventType.MouseMove:

                    if (touchIsDown)
                    {
                        if (isTouchWithinTextBox(eX, eY))
                        {
                            touchMoveDeltaY = lastTouchMoveLocationY - eY;
                            if (touchMoveDeltaY > gv.fontHeight)
                            {
                                SetCurrentTopLineIndex(1);
                                touchMoveDeltaY = 0;
                                lastTouchMoveLocationY = eY;
                            }
                            else if (touchMoveDeltaY < -1 * gv.fontHeight)
                            {
                                SetCurrentTopLineIndex(-1);
                                touchMoveDeltaY = 0;
                                lastTouchMoveLocationY = eY;
                            }
                        }
                    }
                    break;

                case MouseEventType.EventType.MouseUp:

                    touchIsDown = false;
                    touchMoveDeltaY = 0;
                    break;
            }            
        }
        /*public void onMouseWheel(object sender, MouseEventArgs e)
        {
            if (isMouseWithinTextBox(e))
            {
                // Update the drawing based upon the mouse wheel scrolling. 
                int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;

                if (numberOfTextLinesToMove != 0)
                {
                    SetCurrentTopLineIndex(-numberOfTextLinesToMove);
                }
            }
        }*/        
    }
}
