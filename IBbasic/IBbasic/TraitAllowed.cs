﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class TraitAllowed 
    {
	    public string name = "";
	    public string tag = "";
	    public int atWhatLevelIsAvailable = 0;
	    public bool automaticallyLearned = false;
	    public bool allow = true;
	
	    public TraitAllowed()
	    {
		
	    }
	
	    public TraitAllowed DeepCopy()
	    {
		    TraitAllowed copy = new TraitAllowed();
		    copy.name = this.name;
		    copy.tag = this.tag;
		    copy.atWhatLevelIsAvailable = this.atWhatLevelIsAvailable;
		    copy.automaticallyLearned = this.automaticallyLearned;
		    copy.allow = this.allow;		
		    return copy;
	    }
    }
}
