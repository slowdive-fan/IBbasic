using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBbasic
{
    public class ConvoSavedValues
    {
	    public string ConvoFileName = "";
	    public int NodeNotActiveIdNum = -1;

        public ConvoSavedValues()
        {
        }
    
        public ConvoSavedValues DeepCopy()
        {
    	    ConvoSavedValues copy = new ConvoSavedValues();
		    copy.ConvoFileName = this.ConvoFileName;
		    copy.NodeNotActiveIdNum = this.NodeNotActiveIdNum;
		    return copy;
        }
    }
}
