using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    public class ScriptObject
    {
        //TYPES
        //variable, small_int (0-100, ++, --), large_int (100+), string, this, basic_chk_oper (!=><), basic_oper (+-=><), full_oper (+-=*/<>), 
        //item_tags, player_index, mapX, mapY, pc_filename, 
        public string name = "none";
        public string description = "";
        public string parmType1 = "";
        public string parmDescription1 = "";
        public string parmType2 = "";
        public string parmDescription2 = "";
        public string parmType3 = "";
        public string parmDescription3 = "";
        public string parmType4 = "";
        public string parmDescription4 = "";
    }
}
