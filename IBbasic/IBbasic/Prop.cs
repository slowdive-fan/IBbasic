using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using Newtonsoft.Json;

namespace IBbasic
{
    public class Prop 
    {
        public bool moduleProp = false;
        public int LocationX = 0;
	    public int LocationY = 0;
        public string ImageFileName = "blank";
        public bool PropFacingLeft = true;
	    public string MouseOverText = "none";
	    public bool HasCollision = false;
	    public bool isShown = true;
	    public bool isActive = true;
	    public string PropTag = "newProp";
	    public string PropCategoryName = "newCategory";
	    public string ConversationWhenOnPartySquare = "none";
	    public string EncounterWhenOnPartySquare = "none";
	    public bool DeletePropWhenThisEncounterIsWon = false;
        public string OnEnterSquareScript = "none";
        public string OnEnterSquareScriptParm1 = "none";
        public string OnEnterSquareScriptParm2 = "none";
        public string OnEnterSquareScriptParm3 = "none";
        public string OnEnterSquareScriptParm4 = "none";
        public bool canBeTriggeredByPc = true;
        public bool canBeTriggeredByCreature = true;
        public int numberOfScriptCallsRemaining = 999;
        public bool isTrap = false;
        public int trapDCforDisableCheck = 10;
        public List<LocalInt> PropLocalInts = new List<LocalInt>();
	    public List<LocalString> PropLocalStrings = new List<LocalString>();
	    
    
        public Prop()
        {
    	
        }
    
        public Prop DeepCopy()
        {
    	    Prop copy = new Prop();
		    copy.LocationX = this.LocationX;
		    copy.LocationY = this.LocationY;
            copy.ImageFileName = this.ImageFileName;
		    copy.PropFacingLeft = this.PropFacingLeft;
		    copy.MouseOverText = this.MouseOverText;
		    copy.HasCollision = this.HasCollision;
		    copy.isShown = this.isShown;
		    copy.isActive = this.isActive;
		    copy.PropTag = this.PropTag;
		    copy.PropCategoryName = this.PropCategoryName;
		    copy.ConversationWhenOnPartySquare = this.ConversationWhenOnPartySquare;
		    copy.EncounterWhenOnPartySquare = this.EncounterWhenOnPartySquare;
		    copy.DeletePropWhenThisEncounterIsWon = this.DeletePropWhenThisEncounterIsWon;
            copy.OnEnterSquareScript = this.OnEnterSquareScript;
            copy.OnEnterSquareScriptParm1 = this.OnEnterSquareScriptParm1;
            copy.OnEnterSquareScriptParm2 = this.OnEnterSquareScriptParm1;
            copy.OnEnterSquareScriptParm3 = this.OnEnterSquareScriptParm1;
            copy.OnEnterSquareScriptParm4 = this.OnEnterSquareScriptParm1;
            copy.canBeTriggeredByPc = this.canBeTriggeredByPc;
            copy.canBeTriggeredByCreature = this.canBeTriggeredByCreature;
            copy.numberOfScriptCallsRemaining = this.numberOfScriptCallsRemaining;
            copy.isTrap = this.isTrap;
            copy.trapDCforDisableCheck = this.trapDCforDisableCheck;
            copy.PropLocalInts = new List<LocalInt>();
            foreach (LocalInt l in this.PropLocalInts)
            {
                LocalInt Lint = new LocalInt();
                Lint.Key = l.Key;
                Lint.Value = l.Value;
                copy.PropLocalInts.Add(Lint);
            }        
            copy.PropLocalStrings = new List<LocalString>();
            foreach (LocalString l in this.PropLocalStrings)
            {
                LocalString Lstr = new LocalString();
                Lstr.Key = l.Key;
                Lstr.Value = l.Value;
                copy.PropLocalStrings.Add(Lstr);
            }            
		    return copy;
        }
    }
}
