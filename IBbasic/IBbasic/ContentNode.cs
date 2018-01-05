using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IBbasic
{
    [Serializable]
    public class ContentNode
    {        
        public int idNum = -1;
        public int orderNum = 0;
        public int parentIdNum = -1;
        public bool pcNode = true;
        public int linkTo = 0;
        public bool ShowOnlyOnce = false;
        public bool NodeIsActive = true;
        public string NodePortraitBitmap = "";
        public string NodeNpcName = "";
        public string conversationText = "Continue";
        public bool IsExpanded = true;
        public int indentMultiplier = 0;
        public List<ContentNode> subNodes = new List<ContentNode>();
        public List<Action> actions = new List<Action>();
        public List<Condition> conditions = new List<Condition>();
        public bool isLink = false;

        public ContentNode()
        {
        }

        public ContentNode SearchContentNodeById(int checkIdNum)
        {
            ContentNode tempNode = null;
            if (idNum == checkIdNum)
            {
                return this;
            }
            foreach (ContentNode subNode in subNodes)
            {
                tempNode = subNode.SearchContentNodeById(checkIdNum);
                if (tempNode != null)
                {
                    return tempNode;
                }
            }
            return null;
        }
        public ContentNode DuplicateContentNode(int nextIdNum)
        {
            ContentNode newNode = new ContentNode();
            newNode.conversationText = this.conversationText;
            //newNode.idNum = nextIdNum;
            newNode.parentIdNum = this.parentIdNum;
            newNode.pcNode = this.pcNode;
            newNode.linkTo = this.linkTo;
            newNode.NodePortraitBitmap = this.NodePortraitBitmap;
            //newNode.NodeSound = this.NodeSound;
            newNode.IsExpanded = this.IsExpanded;
            newNode.indentMultiplier = this.indentMultiplier;
            newNode.ShowOnlyOnce = this.ShowOnlyOnce;
            newNode.NodeIsActive = this.NodeIsActive;
            newNode.NodeNpcName = this.NodeNpcName;

            newNode.actions = new List<Action>();
            foreach (Action a in this.actions)
            {
                Action ac = a.DeepCopy();
                newNode.actions.Add(ac);
            }
            newNode.conditions = new List<Condition>();
            foreach (Condition c in this.conditions)
            {
                Condition cc = c.DeepCopy();
                newNode.conditions.Add(cc);
            }

            return newNode;
        }
        public ContentNode DuplicateContentNode()
        {
            ContentNode newNode = new ContentNode();
            newNode.conversationText = this.conversationText;
            newNode.idNum = this.idNum;
            newNode.parentIdNum = this.parentIdNum;
            newNode.pcNode = this.pcNode;
            newNode.linkTo = this.linkTo;
            newNode.NodePortraitBitmap = this.NodePortraitBitmap;
            //newNode.NodeSound = this.NodeSound;
            newNode.IsExpanded = this.IsExpanded;
            newNode.indentMultiplier = this.indentMultiplier;
            newNode.ShowOnlyOnce = this.ShowOnlyOnce;
            newNode.NodeIsActive = this.NodeIsActive;
            newNode.NodeNpcName = this.NodeNpcName;

            newNode.actions = new List<Action>();
            foreach (Action a in this.actions)
            {
                Action ac = a.DeepCopy();
                newNode.actions.Add(ac);
            }
            newNode.conditions = new List<Condition>();
            foreach (Condition c in this.conditions)
            {
                Condition cc = c.DeepCopy();
                newNode.conditions.Add(cc);
            }

            return newNode;
        }
    }        
}
