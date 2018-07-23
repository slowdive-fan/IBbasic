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
        public parmType parmType1 = parmType.none;
        public string parmDescription1 = "";
        public parmType parmType2 = parmType.none;
        public string parmDescription2 = "";
        public parmType parmType3 = parmType.none;
        public string parmDescription3 = "";
        public parmType parmType4 = parmType.none;
        public string parmDescription4 = "";
    }

    public enum parmType
    {
        none,
        strg,
        variable,
        boolean,
        item_resref_list,
        race_tag_list,
        class_tag_list,
        creature_resref_list,
        trait_tag_list,
        spell_tag_list,
        area_list,
        convo_list,
        encounter_list,
        shop_tag_list,
        attribute,
        journalCategoryTag,
        small_int,
        large_int,
        this_obj,
        simple_oper,
        basic_chk_oper,
        basic_oper,
        full_oper,
        player_index,
        mapX,
        mapY,
        colors,
        pc_filename
    };
}
