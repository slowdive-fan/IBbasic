using System;
using System.Collections.Generic;
using System.Text;

namespace IBbasic
{
    public class PartyCampaign
    {
        public string partyName = "Party Name";
        public string partyNotes = "Any notes you want to add go here";
        public string partyFilename = "party00";
        public List<Player> playerList = new List<Player>();
        public List<ItemRefs> partyInventoryRefsList = new List<ItemRefs>();
        public List<Item> partyNonStandardItemsList = new List<Item>();

        public PartyCampaign()
        {

        }
    }
}
