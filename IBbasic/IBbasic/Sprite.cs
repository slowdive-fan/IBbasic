﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class Sprite
    {
        public string bitmap = "blank";               // filename of bitmap, do NOT include filename extension
        public PointF position = new PointF(0, 0);  // The current position of the sprite        
        public PointF velocity = new PointF(0, 0);  // The speed of the sprite at the current instance
        public float angle = 0;                       // The current angle of rotation of the sprite
        public float angularVelocity = 0;             // The speed that the angle is changing
        public float scaleX = 1.0f;                    // The X-scale of the sprite
        public float scaleY = 1.0f;                    // The Y-scale of the sprite
        public int timeToLiveInMilliseconds = 1000;   // The 'time to live' of the sprite in milliseconds after the startTimeInMilliseconds
        public int millisecondsPerFrame = 100;        // The amount of time (ms) before switching to next frame  
        public bool permanent = false;
        public bool movesIndependentlyFromPlayerPosition = false; //when party moves sprite position is adjusted in Common Code's doUpdate function to make it move independently

        //new stuff yet to add into the calculations
        public float opacity = 1.0f;                  //The transparency of the sprite, at 1.0f it's totally solid, at 0 it's invisible
        public string movementMethod = "linear";      //They way the sprite is moved across screen (i.e. how the velocities are used to determine new position)
        public float mass = 0;                        //Might be used later for determining the effects of collission
        public string spriteType = "normalSprite";    //to make different types of srpites identifiable for the calculations in update(elapsed)
        public float screenWidth = 0;
        public float screenHeight = 0;
        public float xShift = 0;
        public bool reverseXShift = false;
        //This is an alterntive frame counter for animations stitched together from several separate bitmaps (vs. a horizintal sprite sheet)
        //a value of 0 inidcates that no sprite chnaging animations is called for
        //a value of 1 or more is the number of the current frame starting with 1, the number is always added to the end of the bitmap(s) name
        //like flame1.bmp, flame2.bmp, flame3.bmp and so forth 
        public int numberOFFramesForAnimationsMadeFromSeveralBitmaps = 0;

        //more ideas for later: isShadowCaster, extend vector and position by z-coordinate, hardness (simulate shatter on impact as well as speed loss on collision due energy going into deformation)

        //mostly internal to this class only
        public int currentFrameIndex = 0;
        public int numberOfFrames = 1;
        public int frameHeight = 0;
        public int totalElapsedTime = 0;

        //overloaded constructor: complexSprite 
        public Sprite(GameView gv, string bitmap, float positionX, float positionY, float velocityX, float velocityY, float angle, float angularVelocity, float scaleX, float scaleY, int timeToLiveInMilliseconds, bool permanent, int msPerFrame, float opacity, float mass, string movementMethod, bool movesIndependentlyFromPlayerPosition, int numberOFFramesForAnimationsMadeFromSeveralBitmaps)
        {
            this.bitmap = bitmap;
            this.position = new PointF(positionX, positionY);
            this.velocity = new PointF(velocityX, velocityY);
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.timeToLiveInMilliseconds = timeToLiveInMilliseconds;
            this.millisecondsPerFrame = msPerFrame;
            this.permanent = permanent;
            this.opacity = opacity;
            this.mass = mass;
            this.movementMethod = movementMethod;
            this.spriteType = "complexSprite";
            this.screenHeight = gv.screenHeight;
            this.screenWidth = gv.screenWidth;
            this.movesIndependentlyFromPlayerPosition = movesIndependentlyFromPlayerPosition;
            this.xShift = xShift;
            this.reverseXShift = reverseXShift;
            this.numberOFFramesForAnimationsMadeFromSeveralBitmaps = numberOFFramesForAnimationsMadeFromSeveralBitmaps;

            if (millisecondsPerFrame == 0) { millisecondsPerFrame = 100; }
            frameHeight = gv.cc.GetFromBitmapList(bitmap).Height;
            numberOfFrames = gv.cc.GetFromBitmapList(bitmap).Width / frameHeight;
        }

        //lean constructor: normal Sprite
        public Sprite(GameView gv, string bitmap, float positionX, float positionY, float velocityX, float velocityY, float angle, float angularVelocity, float scale, int timeToLiveInMilliseconds, bool permanent, int msPerFrame)
        {
            this.bitmap = bitmap;
            this.position = new PointF(positionX, positionY);
            this.velocity = new PointF(velocityX, velocityY);
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.scaleX = scale;
            this.scaleY = scale;
            this.timeToLiveInMilliseconds = timeToLiveInMilliseconds;
            this.millisecondsPerFrame = msPerFrame;
            this.permanent = permanent;
            this.spriteType = "normalSprite";

            if (millisecondsPerFrame == 0) { millisecondsPerFrame = 100; }
            frameHeight = gv.cc.GetFromBitmapList(bitmap).Height;
            numberOfFrames = gv.cc.GetFromBitmapList(bitmap).Width / frameHeight;
        }

        public void Update(int elapsed, GameView gv)
        {
            timeToLiveInMilliseconds -= elapsed;
            totalElapsedTime += elapsed;
            if (movementMethod == "linear")
            {
                position.X += velocity.X * elapsed;
                position.Y += velocity.Y * elapsed;
                angle += angularVelocity * elapsed;
            }

            if (this.numberOFFramesForAnimationsMadeFromSeveralBitmaps > 0)
            {
                numberOfFrames = this.numberOFFramesForAnimationsMadeFromSeveralBitmaps;
            }
            if (numberOfFrames == 0) { numberOfFrames = 1; }
            if (numberOFFramesForAnimationsMadeFromSeveralBitmaps == 0)
            {
                int x = totalElapsedTime % (numberOfFrames * millisecondsPerFrame);
                currentFrameIndex = x / millisecondsPerFrame;
            }
            else
            {
                int x = (totalElapsedTime % (numberOfFrames * millisecondsPerFrame)) ;
                currentFrameIndex = (x / millisecondsPerFrame) + 1;
            }
        }

        public void Draw(SKCanvas c, GameView gv)
        {
            IbRect src = new IbRect(currentFrameIndex * frameHeight, 0, frameHeight, frameHeight);
            IbRect dst = new IbRect(0, 0, 0, 0);
            //assumes frames of equal proportions
            if (numberOFFramesForAnimationsMadeFromSeveralBitmaps != 0)
            {
                src = new IbRect(0, 0, 150, 150);
            }
            dst = new IbRect((int)this.position.X, (int)this.position.Y, (int)(gv.squareSize * this.scaleX), (int)(gv.squareSize * this.scaleY));
            if (gv.screenType.Equals("combat"))
            {
                dst = new IbRect((int)this.position.X, (int)this.position.Y, (int)((int)(gv.squareSize * gv.scaler) * this.scaleX), (int)((int)(gv.squareSize * gv.scaler) * this.scaleY));
            }
            if (gv.screenType.Equals("main"))
            {
                dst = new IbRect((int)this.position.X, (int)this.position.Y, (int)((int)(gv.squareSize * gv.scaler) * this.scaleX), (int)((int)(gv.squareSize * gv.scaler) * this.scaleY));
            }
            if (numberOFFramesForAnimationsMadeFromSeveralBitmaps == 0)
            {
                gv.DrawBitmap(c, gv.cc.GetFromBitmapList(bitmap), src, dst, angle, false);
            }
            else
            {
                gv.DrawBitmap(c, gv.cc.GetFromBitmapList(bitmap + currentFrameIndex.ToString()), src, dst, angle, false);
            }   
        }
    }    
}
