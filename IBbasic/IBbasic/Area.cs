using System;
using System.Collections.Generic;

namespace IBbasic
{
    public class Area 
    {
        public bool Is3dArea = false;
        public string Filename = "newArea";
        public int AreaVisibleDistance = 4;
        public bool RestingAllowed = false;
        public bool smallMap = false;
        public int MapSizeX = 20; //10x10 or 20x20
        public int MapSizeY = 20; //10x10 or 20x20
        public bool UseMiniMapFogOfWar = true;
	    public bool areaDark = false;
	    public bool UseDayNightCycle = false;
        public int TimePerSquare = 6; //in minutes for now
	    public List<string> Layer1Filename = new List<string>();
        //public List<int> Layer1Rotate = new List<int>();
        //public List<int> Layer1Mirror = new List<int>();
        public List<string> Layer2Filename = new List<string>();
        //public List<int> Layer2Rotate = new List<int>();
        //public List<int> Layer2Mirror = new List<int>();
        public List<string> Layer3Filename = new List<string>();
        //public List<int> Layer3Rotate = new List<int>();
        //public List<int> Layer3Mirror = new List<int>();
        public List<int> Walkable = new List<int>();
        public List<int> LoSBlocked = new List<int>();
        public List<int> Visible = new List<int>();
        //public List<Prop> Props = new List<Prop>();
	    //public List<string> InitialAreaPropTagsList = new List<string>();
	    public List<Trigger> Triggers = new List<Trigger>();
	    public int NextIdNumber = 100;
        //public string OnHeartBeatIBScript = "none";
        //public string OnHeartBeatIBScriptParms = "";
	    public string inGameAreaName = "newArea";
        /*
        public List<int> NorthBlocked = new List<int>(); //0 = open, 1 = blocked, 2 = open secret door
        public List<string> NorthWallTile = new List<string>();
        public List<string> NorthWallTileOverlay = new List<string>();
        public List<string> NorthWallBackDrop = new List<string>();

        public List<int> SouthBlocked = new List<int>();
        public List<string> SouthWallTile = new List<string>();
        public List<string> SouthWallTileOverlay = new List<string>();
        public List<string> SouthWallBackDrop = new List<string>();

        public List<int> EastBlocked = new List<int>();
        public List<string> EastWallTile = new List<string>();
        public List<string> EastWallTileOverlay = new List<string>();
        public List<string> EastWallBackDrop = new List<string>();

        public List<int> WestBlocked = new List<int>();
        public List<string> WestWallTile = new List<string>();
        public List<string> WestWallTileOverlay = new List<string>();
        public List<string> WestWallBackDrop = new List<string>();
        */
        public Area()
	    {	
	    }
        public void SetAllToGrass()
        {
            for (int index = 0; index < (this.MapSizeX * this.MapSizeY); index++)
            {
                this.Layer1Filename.Add("t_f_grass");
                //this.Layer1Rotate.Add(0);
                //this.Layer1Mirror.Add(0);
                this.Layer2Filename.Add("t_a_blank");
                //this.Layer2Rotate.Add(0);
                //this.Layer2Mirror.Add(0);
                this.Layer3Filename.Add("t_a_blank");
                //this.Layer3Rotate.Add(0);
                //this.Layer3Mirror.Add(0);
                this.Walkable.Add(1);
                this.LoSBlocked.Add(0);
                this.Visible.Add(0);
            }
        }
        public void SetAllToGrass3D()
        {
            for (int index = 0; index < (this.MapSizeX * this.MapSizeY); index++)
            {
                this.Layer1Filename.Add("bd_dd1");
                //this.Layer1Rotate.Add(0);
                //this.Layer1Mirror.Add(0);
                this.Layer2Filename.Add("w_a_blank");
                //this.Layer2Rotate.Add(0);
                //this.Layer2Mirror.Add(0);
                this.Layer3Filename.Add("o_a_blank");
                //this.Layer3Rotate.Add(0);
                //this.Layer3Mirror.Add(0);
                this.Walkable.Add(1);
                this.LoSBlocked.Add(0);
                this.Visible.Add(0);
            }
        }

        public bool GetBlocked(int playerXPosition, int playerYPosition)
        {        
            if (this.Walkable[playerYPosition * this.MapSizeX + playerXPosition] == 0)
            {
                return true;
            }
            /*foreach (Prop p in this.Props)
            {
                if ((p.LocationX == playerXPosition) && (p.LocationY == playerYPosition))
                {
                    if (p.HasCollision)
                    {
                        return true;
                    }
                }
            }*/
            return false;
        }
	
	    public Trigger getTriggerByLocation(int x, int y)
        {
            foreach (Trigger t in this.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    if ((p.X == x) && (p.Y == y))
                    {
                        return t;
                    }
                }
            }
            return null;
        }
	    public Trigger getTriggerByTag(String tag)
        {
            foreach (Trigger t in this.Triggers)
            {
                if (t.TriggerTag.Equals(tag))
                {
            	    return t;
                }
            }
            return null;
        }
	    /*public Prop getPropByLocation(int x, int y)
        {
            foreach (Prop p in this.Props)
            {
                if ((p.LocationX == x) && (p.LocationY == y))
                {
                    return p;
                }            
            }
            return null;
        }*/
	    /*public Prop getPropByTag(String tag)
        {
            foreach (Prop p in this.Props)
            {
                if (p.PropTag.Equals(tag))
                {
            	    return p;
                }
            }
            return null;
        }*/
    }
}
