using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    public class ToolsetScreenConvoEditor
    {
        public GameView gv;
        //public Convo currentConvo = new Convo();
        public ContentNode currentNode = null;
        public ContentNode parentNode = null;
        public ContentNode editNode = null;
        public Condition copiedConditional = new Condition();
        public Action copiedAction = new Action();
        public int copyToClipboardNode = 1;
        public int copyToClipboardNodeParentNode = 1;
        public int pasteFromClipboardNode = 1;
        public List<ContentNode> nodeList = new List<ContentNode>();
        public string currentMode = "Info"; //Info, Add, Remove, Copy, Paste, Settings
        public string currentNpcNode = "";
        public string currentPcNode = "";
        public List<string> currentPcNodeList = new List<string>();
        public List<IbRect> currentPcNodeRectList = new List<IbRect>();
        public int pcNodeGlow = -1;
        public int npcNodeEndY = 0;
        public int parentIdNum = 0;
        public List<int> nodeIndexList = new List<int>();
        private IBminiTextBox description;
        public Stack<Convo> undoConvoStack = new Stack<Convo>();
        public Stack<Convo> redoConvoStack = new Stack<Convo>();

        public int panelTopLocation = 0;
        public int panelLeftLocation = 0;
        public IbRect src = null;
        public IbRect dst = null;
        private IbbButton btnNode = null;
        private IbbButton btnConditionals = null;
        private IbbButton btnActions = null;
        private IbbButton btnExpandAllNodes = null;
        private IbbButton btnCollapseAllNodes = null;
        private IbbButton btnSettings = null;
        
        //touch scrolling stuff
        public bool touchIsDown = false;
        public int touchMoveDeltaY = 0;
        public int lastTouchMoveLocationY = 0;

        //Convo Nodes
        public IbbToggle tglParentNode = null;
        public IbbToggle tglSelectedNode = null;
        public List<IbbToggle> tglChildNode = new List<IbbToggle>();
        public int currentTopLineIndex = 0;
        public int numberOfLinesToShow = 23;

        //Node Properties Panel        
        public IbbToggle tglNodeText = null;
        public IbbToggle tglNodeNpcName = null;
        public IbbToggle tglNodeNpcPortraitBitmap = null;
        public IbbToggle tglShowOnlyOnce = null;
        public IbbButton btnNodeAdd = null;
        public IbbButton btnNodeMoveUp = null;
        public IbbButton btnNodeMoveDown = null;
        public IbbButton btnNodeRemove = null;
        public IbbButton btnNodeCopy = null;
        public IbbButton btnNodePaste = null;
        public IbbButton btnNodePasteAsLink = null;
        public IbbButton btnNodeRelocateCopiedNodes = null;
        public IbbButton btnFollowLink = null;

        //Convo Properties Panel
        public IbbToggle tglSettingConvoFileName = null;
        public IbbToggle tglSettingDefaultNpcName = null;
        public IbbToggle tglSettingNpcPortraitBitmap = null;
        public IbbToggle tglSettingNarration = null;
        public IbbToggle tglSettingPartyChat = null;
        public IbbToggle tglSettingSpeakToMainPcOnly = null;

        //Conditionals Panel
        public IbbButton btnCondAdd = null;
        public IbbButton btnCondMoveUp = null;
        public IbbButton btnCondMoveDown = null;
        public IbbButton btnCondRemove = null;
        public IbbButton btnCondCopy = null;
        public IbbButton btnCondPaste = null;
        public IbbToggle tglCond1Radio = null;
        public IbbToggle tglCond2Radio = null;
        public IbbToggle tglCond3Radio = null;
        public IbbToggle tglCond4Radio = null;
        public IbbToggle tglCondScript = null;
        public IbbToggle tglCondParm1 = null;
        public IbbToggle tglCondParm2 = null;
        public IbbToggle tglCondParm3 = null;
        public IbbToggle tglCondParm4 = null;

        //Actions Panel
        public IbbButton btnActionAdd = null;
        public IbbButton btnActionMoveUp = null;
        public IbbButton btnActionMoveDown = null;
        public IbbButton btnActionRemove = null;
        public IbbButton btnActionCopy = null;
        public IbbButton btnActionPaste = null;
        public IbbToggle tglAction1Radio = null;
        public IbbToggle tglAction2Radio = null;
        public IbbToggle tglAction3Radio = null;
        public IbbToggle tglAction4Radio = null;
        public IbbToggle tglActionScript = null;
        public IbbToggle tglActionParm1 = null;
        public IbbToggle tglActionParm2 = null;
        public IbbToggle tglActionParm3 = null;
        public IbbToggle tglActionParm4 = null;

        public int mapStartLocXinPixels;

        public ToolsetScreenConvoEditor(GameView g)
        {
            gv = g;
            mapStartLocXinPixels = 1 * gv.uiSquareSize;
            setControlsStart();
            setupNodePropertiesPanelControls();
            //setupConvoNodeControls();
            setupConvoPropertiesPanelControls();            
            setupNodeConditionalsPanelControls();
            setupNodeActionsPanelControls();
            
            description = new IBminiTextBox(gv);
            description.tbXloc = (int)(mapStartLocXinPixels + (gv.scaler * 4));
            description.tbYloc = (int)(10 * gv.squareSize * gv.scaler - (gv.scaler * 4));
            description.tbWidth = (int)(10 * gv.squareSize * gv.scaler);
            description.tbHeight = (int)(5 * gv.squareSize * gv.scaler);
            description.showBoxBorder = false;
        }
        public void resetAllParentIds()
        {
            ResetParentIdNum(gv.mod.currentConvo.subNodes[0]);
        }
        public void refillNodeList()
        {
            ResetNodeList(gv.mod.currentConvo.subNodes[0]);
        }
        public void ResetTreeView()
        {
            nodeList.Clear();
            refillNodeList();
        }
        public void expandAllNodes()
        {
            ExpandAllNodes(gv.mod.currentConvo.subNodes[0]);
        }
        public void collapseAllNodes()
        {
            CollapseAllNodes(gv.mod.currentConvo.subNodes[0]);
        }

        public void setControlsStart()
        {
            if (btnNode == null)
            {
                btnNode = new IbbButton(gv, 1.0f);
            }
            btnNode.Img = "btn_small";
            btnNode.Img2 = "btnnode";
            btnNode.Glow = "btn_small_glow";
            //btnNode.Text = "NODE";
            btnNode.X = 0 * gv.uiSquareSize;
            btnNode.Y = 0 * gv.uiSquareSize + (int)(gv.scaler);
            btnNode.Height = (int)(gv.ibbheight * gv.scaler);
            btnNode.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnConditionals == null)
            {
                btnConditionals = new IbbButton(gv, 1.0f);
            }
            btnConditionals.Img = "btn_small";
            btnConditionals.Img2 = "btnconditional";
            btnConditionals.Glow = "btn_small_glow";
            //btnConditionals.Text = "COND";
            btnConditionals.X = 0 * gv.uiSquareSize;
            btnConditionals.Y = 1 * gv.uiSquareSize + (int)(gv.scaler);
            btnConditionals.Height = (int)(gv.ibbheight * gv.scaler);
            btnConditionals.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnActions == null)
            {
                btnActions = new IbbButton(gv, 0.8f);
            }
            btnActions.Img = "btn_small";
            btnActions.Img2 = "btnaction";
            btnActions.Glow = "btn_small_glow";
            //btnActions.Text = "ACTN";
            btnActions.X = 0 * gv.uiSquareSize;
            btnActions.Y = 2 * gv.uiSquareSize + (int)(gv.scaler);
            btnActions.Height = (int)(gv.ibbheight * gv.scaler);
            btnActions.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnExpandAllNodes == null)
            {
                btnExpandAllNodes = new IbbButton(gv, 1.0f);
            }
            //btnExpandAllNodes.Text = "EXPAND";
            btnExpandAllNodes.Img = "btn_small";
            btnExpandAllNodes.Img2 = "btnexpand";
            btnExpandAllNodes.Glow = "btn_small_glow";
            btnExpandAllNodes.X = 0 * gv.uiSquareSize;
            btnExpandAllNodes.Y = 3 * gv.uiSquareSize + (int)(gv.scaler);
            btnExpandAllNodes.Height = (int)(gv.ibbheight * gv.scaler);
            btnExpandAllNodes.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCollapseAllNodes == null)
            {
                btnCollapseAllNodes = new IbbButton(gv, 1.0f);
            }
            //btnCollapseAllNodes.Text = "COLLAPSE";
            btnCollapseAllNodes.Img = "btn_small";
            btnCollapseAllNodes.Img2 = "btncollapse";
            btnCollapseAllNodes.Glow = "btn_small_glow";
            btnCollapseAllNodes.X = 0 * gv.uiSquareSize;
            btnCollapseAllNodes.Y = 4 * gv.uiSquareSize + (int)(gv.scaler);
            btnCollapseAllNodes.Height = (int)(gv.ibbheight * gv.scaler);
            btnCollapseAllNodes.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnSettings == null)
            {
                btnSettings = new IbbButton(gv, 1.0f);
            }
            //btnSettings.Text = "CONVO";
            btnSettings.Img = "btn_small";
            btnSettings.Img2 = "btnsettings2";
            btnSettings.Glow = "btn_small_glow";
            btnSettings.X = 0 * gv.uiSquareSize;
            btnSettings.Y = 5 * gv.uiSquareSize + (int)(gv.scaler);
            btnSettings.Height = (int)(gv.ibbheight * gv.scaler);
            btnSettings.Width = (int)(gv.ibbwidthR * gv.scaler);
                        
        }
        public void setupConvoNodeControls()
        {
            panelLeftLocation = (8 * gv.uiSquareSize) + (int)((2 * gv.scaler));
            panelTopLocation = 0;

            int xLoc = (int)(1 * gv.uiSquareSize + 2 * gv.scaler);
            int xIndent = gv.uiSquareSize / 2;
            int yLoc = (int)(2 * gv.scaler + gv.fontHeight + gv.fontLineSpacing);
            int yAdder = gv.fontHeight + gv.fontLineSpacing;
            string cnvNodeImg = "cnv_normal.png";

            //TILES PANEL
            if (tglParentNode == null)
            {
                tglParentNode = new IbbToggle(gv);
            }
            if (parentNode != null)
            {
                if ((parentNode.conditions.Count > 0) && (parentNode.actions.Count > 0)) { cnvNodeImg = "cnv_cond_act.png"; }
                else if (parentNode.conditions.Count > 0) { cnvNodeImg = "cnv_conditional.png"; }
                else if (parentNode.actions.Count > 0) { cnvNodeImg = "cnv_action.png"; }
                else { cnvNodeImg = "cnv_normal.png"; }
            }
            tglParentNode.ImgOn = cnvNodeImg;
            tglParentNode.ImgOff = cnvNodeImg;
            tglParentNode.X = panelLeftLocation;
            tglParentNode.Y = panelTopLocation + 0 * gv.uiSquareSize;
            tglParentNode.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglParentNode.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSelectedNode == null)
            {
                tglSelectedNode = new IbbToggle(gv);
            }
            if (currentNode != null)
            {
                if ((currentNode.conditions.Count > 0) && (currentNode.actions.Count > 0)) { cnvNodeImg = "cnv_cond_act.png"; }
                else if (currentNode.conditions.Count > 0) { cnvNodeImg = "cnv_conditional.png"; }
                else if (currentNode.actions.Count > 0) { cnvNodeImg = "cnv_action.png"; }
                else { cnvNodeImg = "cnv_normal.png"; }
            }
            tglSelectedNode.ImgOn = cnvNodeImg;
            tglSelectedNode.ImgOff = cnvNodeImg;
            tglSelectedNode.X = panelLeftLocation + xIndent;
            tglSelectedNode.Y = panelTopLocation + 0 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglSelectedNode.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSelectedNode.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            tglChildNode.Clear();
            int cnt = 0;
            foreach (ContentNode chd in currentNode.subNodes)
            {
                if ((chd.conditions.Count > 0) && (chd.actions.Count > 0)) { cnvNodeImg = "cnv_cond_act.png"; }
                else if (chd.conditions.Count > 0) { cnvNodeImg = "cnv_conditional.png"; }
                else if (chd.actions.Count > 0) { cnvNodeImg = "cnv_action.png"; }
                else { cnvNodeImg = "cnv_normal.png"; }
                IbbToggle newTgl = new IbbToggle(gv);
                newTgl.ImgOn = cnvNodeImg;
                newTgl.ImgOff = cnvNodeImg;
                newTgl.X = panelLeftLocation + xIndent + xIndent;
                newTgl.Y = panelTopLocation + 1 * gv.uiSquareSize + (cnt * (gv.uiSquareSize / 2));
                newTgl.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
                newTgl.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
                tglChildNode.Add(newTgl);
                cnt++;
            }
        }
        public void setupNodePropertiesPanelControls()
        {
            panelLeftLocation = (int)((8 * gv.uiSquareSize) + (2 * gv.scaler));
            panelTopLocation = 0;

            //SETTINGS PANEL
            if (tglNodeText == null)
            {
                tglNodeText = new IbbToggle(gv);
            }
            tglNodeText.ImgOn = "mtgl_edit_btn";
            tglNodeText.ImgOff = "mtgl_edit_btn";
            tglNodeText.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglNodeText.Y = panelTopLocation + 0 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglNodeText.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglNodeText.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglNodeNpcName == null)
            {
                tglNodeNpcName = new IbbToggle(gv);
            }
            tglNodeNpcName.ImgOn = "mtgl_edit_btn";
            tglNodeNpcName.ImgOff = "mtgl_edit_btn";
            tglNodeNpcName.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglNodeNpcName.Y = panelTopLocation + 1 * gv.uiSquareSize;
            tglNodeNpcName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglNodeNpcName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglNodeNpcPortraitBitmap == null)
            {
                tglNodeNpcPortraitBitmap = new IbbToggle(gv);
            }
            tglNodeNpcPortraitBitmap.ImgOn = "mtgl_edit_btn";
            tglNodeNpcPortraitBitmap.ImgOff = "mtgl_edit_btn";
            tglNodeNpcPortraitBitmap.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglNodeNpcPortraitBitmap.Y = panelTopLocation + 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglNodeNpcPortraitBitmap.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglNodeNpcPortraitBitmap.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglShowOnlyOnce == null)
            {
                tglShowOnlyOnce = new IbbToggle(gv);
            }
            tglShowOnlyOnce.ImgOn = "mtgl_rbtn_on";
            tglShowOnlyOnce.ImgOff = "mtgl_rbtn_off";
            tglShowOnlyOnce.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglShowOnlyOnce.Y = panelTopLocation + 2 * gv.uiSquareSize;
            tglShowOnlyOnce.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglShowOnlyOnce.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (btnNodeAdd == null)
            {
                btnNodeAdd = new IbbButton(gv, 1.0f);
            }
            btnNodeAdd.Img = "btn_small";
            btnNodeAdd.Img2 = "btnadd";
            btnNodeAdd.Glow = "btn_small_glow";
            //btnNodeAdd.Text = "ADD";
            btnNodeAdd.X = panelLeftLocation;
            btnNodeAdd.Y = panelTopLocation + (4 * gv.uiSquareSize);
            btnNodeAdd.Height = (int)(gv.ibbheight * gv.scaler);
            btnNodeAdd.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnNodeMoveUp == null)
            {
                btnNodeMoveUp = new IbbButton(gv, 1.0f);
            }
            btnNodeMoveUp.Img = "btn_small";
            btnNodeMoveUp.Img2 = "ctrl_up_arrow";
            btnNodeMoveUp.Glow = "btn_small_glow";
            //btnNodeMoveUp.Text = "UP";
            btnNodeMoveUp.X = panelLeftLocation + 1 * gv.uiSquareSize;
            btnNodeMoveUp.Y = panelTopLocation + (4 * gv.uiSquareSize);
            btnNodeMoveUp.Height = (int)(gv.ibbheight * gv.scaler);
            btnNodeMoveUp.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnNodeMoveDown == null)
            {
                btnNodeMoveDown = new IbbButton(gv, 1.0f);
            }
            btnNodeMoveDown.Img = "btn_small";
            btnNodeMoveDown.Img2 = "ctrl_down_arrow";
            btnNodeMoveDown.Glow = "btn_small_glow";
            //btnNodeMoveDown.Text = "DOWN";
            btnNodeMoveDown.X = panelLeftLocation + 2 * gv.uiSquareSize;
            btnNodeMoveDown.Y = panelTopLocation + (4 * gv.uiSquareSize);
            btnNodeMoveDown.Height = (int)(gv.ibbheight * gv.scaler);
            btnNodeMoveDown.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnNodeRemove == null)
            {
                btnNodeRemove = new IbbButton(gv, 1.0f);
            }
            btnNodeRemove.Img = "btn_small";
            btnNodeRemove.Img2 = "btnremove";
            btnNodeRemove.Glow = "btn_small_glow";
            //btnNodeRemove.Text = "REMOVE";
            btnNodeRemove.X = panelLeftLocation;
            btnNodeRemove.Y = panelTopLocation + (5 * gv.uiSquareSize);
            btnNodeRemove.Height = (int)(gv.ibbheight * gv.scaler);
            btnNodeRemove.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnNodeCopy == null)
            {
                btnNodeCopy = new IbbButton(gv, 1.0f);
            }
            btnNodeCopy.Img = "btn_small";
            btnNodeCopy.Img2 = "btncopy";
            btnNodeCopy.Glow = "btn_small_glow";
            //btnNodeCopy.Text = "COPY";
            btnNodeCopy.X = panelLeftLocation + 1 * gv.uiSquareSize;
            btnNodeCopy.Y = panelTopLocation + (5 * gv.uiSquareSize);
            btnNodeCopy.Height = (int)(gv.ibbheight * gv.scaler);
            btnNodeCopy.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnNodePaste == null)
            {
                btnNodePaste = new IbbButton(gv, 1.0f);
            }
            btnNodePaste.Img = "btn_small";
            btnNodePaste.Img2 = "btnpaste";
            btnNodePaste.Glow = "btn_small_glow";
            //btnNodePaste.Text = "PASTE";
            btnNodePaste.X = panelLeftLocation + 2 * gv.uiSquareSize;
            btnNodePaste.Y = panelTopLocation + (5 * gv.uiSquareSize);
            btnNodePaste.Height = (int)(gv.ibbheight * gv.scaler);
            btnNodePaste.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnNodePasteAsLink == null)
            {
                btnNodePasteAsLink = new IbbButton(gv, 1.0f);
            }
            btnNodePasteAsLink.Img = "btn_small";
            btnNodePasteAsLink.Img2 = "btnlink";
            btnNodePasteAsLink.Glow = "btn_small_glow";
            //btnNodePasteAsLink.Text = "LINK";
            btnNodePasteAsLink.X = panelLeftLocation + 0 * gv.uiSquareSize;
            btnNodePasteAsLink.Y = panelTopLocation + (6 * gv.uiSquareSize);
            btnNodePasteAsLink.Height = (int)(gv.ibbheight * gv.scaler);
            btnNodePasteAsLink.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnNodeRelocateCopiedNodes == null)
            {
                btnNodeRelocateCopiedNodes = new IbbButton(gv, 1.0f);
            }
            btnNodeRelocateCopiedNodes.Img = "btn_small";
            btnNodeRelocateCopiedNodes.Img2 = "btnrelocate";
            btnNodeRelocateCopiedNodes.Glow = "btn_small_glow";
            //btnNodeRelocateCopiedNodes.Text = "RELOCATE";
            btnNodeRelocateCopiedNodes.X = panelLeftLocation + 1 * gv.uiSquareSize;
            btnNodeRelocateCopiedNodes.Y = panelTopLocation + (6 * gv.uiSquareSize);
            btnNodeRelocateCopiedNodes.Height = (int)(gv.ibbheight * gv.scaler);
            btnNodeRelocateCopiedNodes.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnFollowLink == null)
            {
                btnFollowLink = new IbbButton(gv, 0.8f);
            }
            //btnHelp.Text = "HELP";
            btnFollowLink.Img = "btn_small";
            btnFollowLink.Img2 = "btnhelp";
            btnFollowLink.Glow = "btn_small_glow";
            btnFollowLink.X = panelLeftLocation + 2 * gv.uiSquareSize;
            btnFollowLink.Y = panelTopLocation + (6 * gv.uiSquareSize);
            btnFollowLink.Height = (int)(gv.ibbheight * gv.scaler);
            btnFollowLink.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        public void setupConvoPropertiesPanelControls()
        {
            panelLeftLocation = (int)((8 * gv.uiSquareSize) + (2 * gv.scaler));
            panelTopLocation = 0;

            //SETTINGS PANEL
            if (tglSettingConvoFileName == null)
            {
                tglSettingConvoFileName = new IbbToggle(gv);
            }
            tglSettingConvoFileName.ImgOn = "mtgl_edit_btn";
            tglSettingConvoFileName.ImgOff = "mtgl_edit_btn";
            tglSettingConvoFileName.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglSettingConvoFileName.Y = panelTopLocation + 0 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglSettingConvoFileName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingConvoFileName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingDefaultNpcName == null)
            {
                tglSettingDefaultNpcName = new IbbToggle(gv);
            }
            tglSettingDefaultNpcName.ImgOn = "mtgl_edit_btn";
            tglSettingDefaultNpcName.ImgOff = "mtgl_edit_btn";
            tglSettingDefaultNpcName.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglSettingDefaultNpcName.Y = panelTopLocation + 1 * gv.uiSquareSize;
            tglSettingDefaultNpcName.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingDefaultNpcName.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingNpcPortraitBitmap == null)
            {
                tglSettingNpcPortraitBitmap = new IbbToggle(gv);
            }
            tglSettingNpcPortraitBitmap.ImgOn = "mtgl_edit_btn";
            tglSettingNpcPortraitBitmap.ImgOff = "mtgl_edit_btn";
            tglSettingNpcPortraitBitmap.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglSettingNpcPortraitBitmap.Y = panelTopLocation + 1 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglSettingNpcPortraitBitmap.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingNpcPortraitBitmap.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingNarration == null)
            {
                tglSettingNarration = new IbbToggle(gv);
            }
            tglSettingNarration.ImgOn = "mtgl_rbtn_on";
            tglSettingNarration.ImgOff = "mtgl_rbtn_off";
            tglSettingNarration.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglSettingNarration.Y = panelTopLocation + 2 * gv.uiSquareSize;
            tglSettingNarration.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingNarration.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingPartyChat == null)
            {
                tglSettingPartyChat = new IbbToggle(gv);
            }
            tglSettingPartyChat.ImgOn = "mtgl_rbtn_on";
            tglSettingPartyChat.ImgOff = "mtgl_rbtn_off";
            tglSettingPartyChat.X = panelLeftLocation;
            tglSettingPartyChat.Y = panelTopLocation + 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglSettingPartyChat.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingPartyChat.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglSettingSpeakToMainPcOnly == null)
            {
                tglSettingSpeakToMainPcOnly = new IbbToggle(gv);
            }
            tglSettingSpeakToMainPcOnly.ImgOn = "mtgl_rbtn_on";
            tglSettingSpeakToMainPcOnly.ImgOff = "mtgl_rbtn_off";
            tglSettingSpeakToMainPcOnly.X = panelLeftLocation;
            tglSettingSpeakToMainPcOnly.Y = panelTopLocation + 3 * gv.uiSquareSize;
            tglSettingSpeakToMainPcOnly.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglSettingSpeakToMainPcOnly.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);
        }
        public void setupNodeConditionalsPanelControls()
        {
            panelLeftLocation = (int)((8 * gv.uiSquareSize) + (2 * gv.scaler));
            panelTopLocation = 0;

            //CONDITIONALS PANEL
            if (tglCond1Radio == null)
            {
                tglCond1Radio = new IbbToggle(gv);
            }
            tglCond1Radio.ImgOn = "mtgl_rbtn_on";
            tglCond1Radio.ImgOff = "mtgl_rbtn_off";
            tglCond1Radio.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglCond1Radio.Y = panelTopLocation + 0 * gv.uiSquareSize + gv.fontHeight + gv.fontLineSpacing;
            tglCond1Radio.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCond1Radio.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCond2Radio == null)
            {
                tglCond2Radio = new IbbToggle(gv);
            }
            tglCond2Radio.ImgOn = "mtgl_rbtn_on";
            tglCond2Radio.ImgOff = "mtgl_rbtn_off";
            tglCond2Radio.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglCond2Radio.Y = panelTopLocation + 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.fontHeight + gv.fontLineSpacing;
            tglCond2Radio.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCond2Radio.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCond3Radio == null)
            {
                tglCond3Radio = new IbbToggle(gv);
            }
            tglCond3Radio.ImgOn = "mtgl_rbtn_on";
            tglCond3Radio.ImgOff = "mtgl_rbtn_off";
            tglCond3Radio.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglCond3Radio.Y = panelTopLocation + 1 * gv.uiSquareSize + gv.fontHeight + gv.fontLineSpacing;
            tglCond3Radio.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCond3Radio.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCond4Radio == null)
            {
                tglCond4Radio = new IbbToggle(gv);
            }
            tglCond4Radio.ImgOn = "mtgl_rbtn_on";
            tglCond4Radio.ImgOff = "mtgl_rbtn_off";
            tglCond4Radio.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglCond4Radio.Y = panelTopLocation + 1 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.fontHeight + gv.fontLineSpacing;
            tglCond4Radio.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCond4Radio.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);



            if (tglCondScript == null)
            {
                tglCondScript = new IbbToggle(gv);
            }
            tglCondScript.ImgOn = "mtgl_edit_btn";
            tglCondScript.ImgOff = "mtgl_edit_btn";
            tglCondScript.X = panelLeftLocation;
            tglCondScript.Y = panelTopLocation + 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglCondScript.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCondScript.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCondParm1 == null)
            {
                tglCondParm1 = new IbbToggle(gv);
            }
            tglCondParm1.ImgOn = "mtgl_edit_btn";
            tglCondParm1.ImgOff = "mtgl_edit_btn";
            tglCondParm1.X = panelLeftLocation;
            tglCondParm1.Y = panelTopLocation + 3 * gv.uiSquareSize;
            tglCondParm1.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCondParm1.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCondParm2 == null)
            {
                tglCondParm2 = new IbbToggle(gv);
            }
            tglCondParm2.ImgOn = "mtgl_edit_btn";
            tglCondParm2.ImgOff = "mtgl_edit_btn";
            tglCondParm2.X = panelLeftLocation;
            tglCondParm2.Y = panelTopLocation + 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglCondParm2.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCondParm2.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCondParm3 == null)
            {
                tglCondParm3 = new IbbToggle(gv);
            }
            tglCondParm3.ImgOn = "mtgl_edit_btn";
            tglCondParm3.ImgOff = "mtgl_edit_btn";
            tglCondParm3.X = panelLeftLocation;
            tglCondParm3.Y = panelTopLocation + 4 * gv.uiSquareSize;
            tglCondParm3.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCondParm3.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglCondParm4 == null)
            {
                tglCondParm4 = new IbbToggle(gv);
            }
            tglCondParm4.ImgOn = "mtgl_edit_btn";
            tglCondParm4.ImgOff = "mtgl_edit_btn";
            tglCondParm4.X = panelLeftLocation;
            tglCondParm4.Y = panelTopLocation + 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglCondParm4.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglCondParm4.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);


            if (btnCondAdd == null)
            {
                btnCondAdd = new IbbButton(gv, 1.0f);
            }
            btnCondAdd.Img = "btn_small";
            btnCondAdd.Img2 = "btnadd";
            btnCondAdd.Glow = "btn_small_glow";
            //btnCondAdd.Text = "ADD";
            btnCondAdd.X = panelLeftLocation;
            btnCondAdd.Y = panelTopLocation + (5 * gv.uiSquareSize);
            btnCondAdd.Height = (int)(gv.ibbheight * gv.scaler);
            btnCondAdd.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCondMoveUp == null)
            {
                btnCondMoveUp = new IbbButton(gv, 1.0f);
            }
            btnCondMoveUp.Img = "btn_small";
            btnCondMoveUp.Img2 = "ctrl_up_arrow";
            btnCondMoveUp.Glow = "btn_small_glow";
            //btnCondMoveUp.Text = "UP";
            btnCondMoveUp.X = panelLeftLocation + 1 * gv.uiSquareSize;
            btnCondMoveUp.Y = panelTopLocation + (5 * gv.uiSquareSize);
            btnCondMoveUp.Height = (int)(gv.ibbheight * gv.scaler);
            btnCondMoveUp.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCondMoveDown == null)
            {
                btnCondMoveDown = new IbbButton(gv, 1.0f);
            }
            btnCondMoveDown.Img = "btn_small";
            btnCondMoveDown.Img2 = "ctrl_down_arrow";
            btnCondMoveDown.Glow = "btn_small_glow";
            //btnCondMoveDown.Text = "DOWN";
            btnCondMoveDown.X = panelLeftLocation + 2 * gv.uiSquareSize;
            btnCondMoveDown.Y = panelTopLocation + (5 * gv.uiSquareSize);
            btnCondMoveDown.Height = (int)(gv.ibbheight * gv.scaler);
            btnCondMoveDown.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCondRemove == null)
            {
                btnCondRemove = new IbbButton(gv, 1.0f);
            }
            btnCondRemove.Img = "btn_small";
            btnCondRemove.Img2 = "btnremove";
            btnCondRemove.Glow = "btn_small_glow";
            //btnCondRemove.Text = "REMOVE";
            btnCondRemove.X = panelLeftLocation;
            btnCondRemove.Y = panelTopLocation + (6 * gv.uiSquareSize);
            btnCondRemove.Height = (int)(gv.ibbheight * gv.scaler);
            btnCondRemove.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCondCopy == null)
            {
                btnCondCopy = new IbbButton(gv, 1.0f);
            }
            btnCondCopy.Img = "btn_small";
            btnCondCopy.Img2 = "btncopy";
            btnCondCopy.Glow = "btn_small_glow";
            //btnCondCopy.Text = "COPY";
            btnCondCopy.X = panelLeftLocation + 1 * gv.uiSquareSize;
            btnCondCopy.Y = panelTopLocation + (6 * gv.uiSquareSize);
            btnCondCopy.Height = (int)(gv.ibbheight * gv.scaler);
            btnCondCopy.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnCondPaste == null)
            {
                btnCondPaste = new IbbButton(gv, 1.0f);
            }
            btnCondPaste.Img = "btn_small";
            btnCondPaste.Img2 = "btnpaste";
            btnCondPaste.Glow = "btn_small_glow";
            //btnCondPaste.Text = "PASTE";
            btnCondPaste.X = panelLeftLocation + 2 * gv.uiSquareSize;
            btnCondPaste.Y = panelTopLocation + (6 * gv.uiSquareSize);
            btnCondPaste.Height = (int)(gv.ibbheight * gv.scaler);
            btnCondPaste.Width = (int)(gv.ibbwidthR * gv.scaler);
        }
        public void setupNodeActionsPanelControls()
        {
            panelLeftLocation = (int)((8 * gv.uiSquareSize) + (2 * gv.scaler));
            panelTopLocation = 0;

            //CONDITIONALS PANEL
            if (tglAction1Radio == null)
            {
                tglAction1Radio = new IbbToggle(gv);
            }
            tglAction1Radio.ImgOn = "mtgl_rbtn_on";
            tglAction1Radio.ImgOff = "mtgl_rbtn_off";
            tglAction1Radio.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglAction1Radio.Y = panelTopLocation + 0 * gv.uiSquareSize + gv.fontHeight + gv.fontLineSpacing;
            tglAction1Radio.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglAction1Radio.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglAction2Radio == null)
            {
                tglAction2Radio = new IbbToggle(gv);
            }
            tglAction2Radio.ImgOn = "mtgl_rbtn_on";
            tglAction2Radio.ImgOff = "mtgl_rbtn_off";
            tglAction2Radio.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglAction2Radio.Y = panelTopLocation + 0 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.fontHeight + gv.fontLineSpacing;
            tglAction2Radio.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglAction2Radio.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglAction3Radio == null)
            {
                tglAction3Radio = new IbbToggle(gv);
            }
            tglAction3Radio.ImgOn = "mtgl_rbtn_on";
            tglAction3Radio.ImgOff = "mtgl_rbtn_off";
            tglAction3Radio.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglAction3Radio.Y = panelTopLocation + 1 * gv.uiSquareSize + gv.fontHeight + gv.fontLineSpacing;
            tglAction3Radio.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglAction3Radio.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglAction4Radio == null)
            {
                tglAction4Radio = new IbbToggle(gv);
            }
            tglAction4Radio.ImgOn = "mtgl_rbtn_on";
            tglAction4Radio.ImgOff = "mtgl_rbtn_off";
            tglAction4Radio.X = panelLeftLocation + 0 * gv.uiSquareSize;
            tglAction4Radio.Y = panelTopLocation + 1 * gv.uiSquareSize + (gv.uiSquareSize / 2) + gv.fontHeight + gv.fontLineSpacing;
            tglAction4Radio.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglAction4Radio.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);



            if (tglActionScript == null)
            {
                tglActionScript = new IbbToggle(gv);
            }
            tglActionScript.ImgOn = "mtgl_edit_btn";
            tglActionScript.ImgOff = "mtgl_edit_btn";
            tglActionScript.X = panelLeftLocation;
            tglActionScript.Y = panelTopLocation + 2 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglActionScript.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglActionScript.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglActionParm1 == null)
            {
                tglActionParm1 = new IbbToggle(gv);
            }
            tglActionParm1.ImgOn = "mtgl_edit_btn";
            tglActionParm1.ImgOff = "mtgl_edit_btn";
            tglActionParm1.X = panelLeftLocation;
            tglActionParm1.Y = panelTopLocation + 3 * gv.uiSquareSize;
            tglActionParm1.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglActionParm1.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglActionParm2 == null)
            {
                tglActionParm2 = new IbbToggle(gv);
            }
            tglActionParm2.ImgOn = "mtgl_edit_btn";
            tglActionParm2.ImgOff = "mtgl_edit_btn";
            tglActionParm2.X = panelLeftLocation;
            tglActionParm2.Y = panelTopLocation + 3 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglActionParm2.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglActionParm2.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglActionParm3 == null)
            {
                tglActionParm3 = new IbbToggle(gv);
            }
            tglActionParm3.ImgOn = "mtgl_edit_btn";
            tglActionParm3.ImgOff = "mtgl_edit_btn";
            tglActionParm3.X = panelLeftLocation;
            tglActionParm3.Y = panelTopLocation + 4 * gv.uiSquareSize;
            tglActionParm3.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglActionParm3.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);

            if (tglActionParm4 == null)
            {
                tglActionParm4 = new IbbToggle(gv);
            }
            tglActionParm4.ImgOn = "mtgl_edit_btn";
            tglActionParm4.ImgOff = "mtgl_edit_btn";
            tglActionParm4.X = panelLeftLocation;
            tglActionParm4.Y = panelTopLocation + 4 * gv.uiSquareSize + (gv.uiSquareSize / 2);
            tglActionParm4.Height = (int)(gv.ibbMiniTglHeight * gv.scaler);
            tglActionParm4.Width = (int)(gv.ibbMiniTglWidth * gv.scaler);


            if (btnActionAdd == null)
            {
                btnActionAdd = new IbbButton(gv, 1.0f);
            }
            btnActionAdd.Img = "btn_small";
            btnActionAdd.Img2 = "btnadd";
            btnActionAdd.Glow = "btn_small_glow";
            //btnActionAdd.Text = "ADD";
            btnActionAdd.X = panelLeftLocation;
            btnActionAdd.Y = panelTopLocation + (5 * gv.uiSquareSize);
            btnActionAdd.Height = (int)(gv.ibbheight * gv.scaler);
            btnActionAdd.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnActionMoveUp == null)
            {
                btnActionMoveUp = new IbbButton(gv, 1.0f);
            }
            btnActionMoveUp.Img = "btn_small";
            btnActionMoveUp.Img2 = "ctrl_up_arrow";
            btnActionMoveUp.Glow = "btn_small_glow";
            //btnActionMoveUp.Text = "UP";
            btnActionMoveUp.X = panelLeftLocation + 1 * gv.uiSquareSize;
            btnActionMoveUp.Y = panelTopLocation + (5 * gv.uiSquareSize);
            btnActionMoveUp.Height = (int)(gv.ibbheight * gv.scaler);
            btnActionMoveUp.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnActionMoveDown == null)
            {
                btnActionMoveDown = new IbbButton(gv, 1.0f);
            }
            btnActionMoveDown.Img = "btn_small";
            btnActionMoveDown.Img2 = "ctrl_down_arrow";
            btnActionMoveDown.Glow = "btn_small_glow";
            //btnActionMoveDown.Text = "DOWN";
            btnActionMoveDown.X = panelLeftLocation + 2 * gv.uiSquareSize;
            btnActionMoveDown.Y = panelTopLocation + (5 * gv.uiSquareSize);
            btnActionMoveDown.Height = (int)(gv.ibbheight * gv.scaler);
            btnActionMoveDown.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnActionRemove == null)
            {
                btnActionRemove = new IbbButton(gv, 1.0f);
            }
            btnActionRemove.Img = "btn_small";
            btnActionRemove.Img2 = "btnremove";
            btnActionRemove.Glow = "btn_small_glow";
            //btnActionRemove.Text = "REMOVE";
            btnActionRemove.X = panelLeftLocation;
            btnActionRemove.Y = panelTopLocation + (6 * gv.uiSquareSize);
            btnActionRemove.Height = (int)(gv.ibbheight * gv.scaler);
            btnActionRemove.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnActionCopy == null)
            {
                btnActionCopy = new IbbButton(gv, 1.0f);
            }
            btnActionCopy.Img = "btn_small";
            btnActionCopy.Img2 = "btncopy";
            btnActionCopy.Glow = "btn_small_glow";
            //btnActionCopy.Text = "COPY";
            btnActionCopy.X = panelLeftLocation + 1 * gv.uiSquareSize;
            btnActionCopy.Y = panelTopLocation + (6 * gv.uiSquareSize);
            btnActionCopy.Height = (int)(gv.ibbheight * gv.scaler);
            btnActionCopy.Width = (int)(gv.ibbwidthR * gv.scaler);

            if (btnActionPaste == null)
            {
                btnActionPaste = new IbbButton(gv, 1.0f);
            }
            btnActionPaste.Img = "btn_small";
            btnActionPaste.Img2 = "btnpaste";
            btnActionPaste.Glow = "btn_small_glow";
            //btnActionPaste.Text = "PASTE";
            btnActionPaste.X = panelLeftLocation + 2 * gv.uiSquareSize;
            btnActionPaste.Y = panelTopLocation + (6 * gv.uiSquareSize);
            btnActionPaste.Height = (int)(gv.ibbheight * gv.scaler);
            btnActionPaste.Width = (int)(gv.ibbwidthR * gv.scaler);
        }

        public void redrawTsConvoEditor(SKCanvas c)
        {
            setControlsStart();
            setupConvoNodeControls();

            int convoPanelLeftLocation = (int)(1 * gv.uiSquareSize + 2 * gv.scaler);
            int convoPanelTopLocation = (int)(2 * gv.scaler + gv.fontHeight + gv.fontLineSpacing);
            int center = 6 * gv.uiSquareSize - (gv.uiSquareSize / 2);
            int shiftForFont = (int)((gv.ibbMiniTglHeight * gv.scaler / 2) - (gv.fontHeight / 2));

            //Page Title
            string convoToEdit = "none";
            if (gv.mod.currentConvo != null)
            {
                convoToEdit = gv.mod.currentConvo.ConvoFileName;
            }
            gv.DrawText(c, "CONVO EDITOR: " + convoToEdit, 2 * gv.uiSquareSize, 2 * gv.scaler, "yl");

            numberOfLinesToShow = 23;
            int cnt = 0;
            int nodeLineIndex = 0;
            foreach (ContentNode n in nodeList)
            {
                if ((nodeLineIndex >= currentTopLineIndex) && (nodeLineIndex < currentTopLineIndex + numberOfLinesToShow))
                {
                    string nodeColor = "wh";
                    if (n.pcNode == false) { nodeColor = "rd"; }
                    else { nodeColor = "bu"; }
                    if (n.linkTo != 0) { nodeColor = "gy"; }
                    if (editNode == n) { nodeColor = "gn"; }
                    int tlX = convoPanelLeftLocation + (int)((gv.ibbMiniTglWidth / 2) * gv.scaler * n.indentMultiplier);
                    int tlY = convoPanelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * cnt);
                    if (n.subNodes.Count > 0)
                    {
                        src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("mtgl_expand_off").Width, gv.cc.GetFromTileBitmapList("mtgl_expand_off").Height);
                        dst = new IbRect(tlX, tlY, gv.fontHeight, gv.fontHeight);
                        if (n.IsExpanded)
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("mtgl_expand_off"), src, dst);
                        }
                        else
                        {
                            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("mtgl_expand_on"), src, dst);
                        }
                    }
                    else
                    {
                        src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("mtgl_expand_none").Width, gv.cc.GetFromTileBitmapList("mtgl_expand_none").Height);
                        dst = new IbRect(tlX, tlY, gv.fontHeight, gv.fontHeight);
                        gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("mtgl_expand_none"), src, dst);
                    }

                    string cnvNodeImg = "cnv_normal";
                    if ((n.conditions.Count > 0) && (n.actions.Count > 0)) { cnvNodeImg = "cnv_cond_act"; }
                    else if (n.conditions.Count > 0) { cnvNodeImg = "cnv_conditional"; }
                    else if (n.actions.Count > 0) { cnvNodeImg = "cnv_action"; }
                    src = new IbRect(0, 0, gv.cc.GetFromTileBitmapList("cnv_normal").Width, gv.cc.GetFromTileBitmapList("cnv_normal").Height);
                    dst = new IbRect(tlX + gv.fontHeight + gv.fontCharSpacing, tlY, gv.fontHeight, gv.fontHeight);
                    gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList(cnvNodeImg), src, dst);

                    gv.DrawText(c, n.conversationText, tlX + ((gv.fontHeight + gv.fontCharSpacing) * 2), tlY, nodeColor);
                    cnt++;
                }
                nodeLineIndex++;
            }

                /*
                //draw parent node
                if (parentNode != null)
                {
                    string nodeColor1 = "wh";
                    if (parentNode.isLink)
                    {
                        //treenode.Text = f_convo.GetContentNodeById(node.linkTo).conversationText;
                        //node.conversationText = f_convo.GetContentNodeById(node.linkTo).conversationText;
                    }
                    if (parentNode.pcNode == false) { nodeColor1 = "rd"; }
                    else { nodeColor1 = "bu"; }
                    if (editNode == parentNode) { nodeColor1 = "gn"; }
                    tglParentNode.Draw();
                    gv.DrawText(parentNode.conversationText, tglParentNode.X + tglParentNode.Width + gv.scaler, tglParentNode.Y + (gv.fontHeight + gv.fontLineSpacing) / 2, nodeColor1);
                }

                //draw current node
                string nodeColor = "wh";
                if (currentNode.isLink)
                {
                    //treenode.Text = f_convo.GetContentNodeById(node.linkTo).conversationText;
                    //node.conversationText = f_convo.GetContentNodeById(node.linkTo).conversationText;
                }
                if (currentNode.pcNode == false) { nodeColor = "rd"; }
                else { nodeColor = "bu"; }
                if (editNode == currentNode) { nodeColor = "gn"; }
                tglSelectedNode.Draw();
                gv.DrawText(currentNode.conversationText, tglSelectedNode.X + tglSelectedNode.Width + gv.scaler, tglSelectedNode.Y + (gv.fontHeight + gv.fontLineSpacing) / 2, nodeColor);

                //draw each child node
                int cnt = 0;
                foreach (IbbToggle child in tglChildNode)
                {
                    nodeColor = "wh";
                    if (currentNode.subNodes[cnt].isLink)
                    {
                        //treenode.Text = f_convo.GetContentNodeById(node.linkTo).conversationText;
                        //node.conversationText = f_convo.GetContentNodeById(node.linkTo).conversationText;
                    }
                    if (currentNode.subNodes[cnt].pcNode == false) { nodeColor = "rd"; }
                    else { nodeColor = "bu"; }
                    if (editNode == currentNode.subNodes[cnt]) { nodeColor = "gn"; }
                    child.Draw();
                    gv.DrawText(currentNode.subNodes[cnt].conversationText, child.X + child.Width + gv.scaler, child.Y + (gv.fontHeight + gv.fontLineSpacing) / 2, nodeColor);
                    cnt++;
                }
                */
            

            //draw convo text of selected editNode
            if (editNode != null)
            {                
                description.linesList.Clear();
                description.AddFormattedTextToTextBox(editNode.conversationText);
                int raise = description.linesList.Count;
                description.tbYloc = (int)(10 * gv.squareSize * gv.scaler - (gv.scaler * 4) - (raise * (gv.fontHeight + gv.fontLineSpacing)));
                int txtheight = (raise * (gv.fontHeight + gv.fontLineSpacing)) + (int)(gv.scaler * 2);
                src = new IbRect(0, 0, gv.cc.GetFromBitmapList("ui_bg_tooltip").Width, gv.cc.GetFromBitmapList("ui_bg_tooltip").Height);
                dst = new IbRect((int)(description.tbXloc - (gv.scaler * 4)), (int)(description.tbYloc - (int)(gv.scaler * 2)), 6 * gv.uiSquareSize + (3 * gv.uiSquareSize / 4), txtheight);
                gv.DrawBitmap(c, gv.cc.GetFromBitmapList("ui_bg_tooltip"), src, dst);
                description.onDrawTextBox(c);
            }

            int width2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Width;
            int height2 = gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png").Height;
            IbRect src2 = new IbRect(0, 0, width2, height2);
            IbRect dst2 = new IbRect(0 - (int)((170 * gv.scaler)), 0 - (int)((102 * gv.scaler)), (int)(width2 * gv.scaler), (int)(height2 * gv.scaler));
            gv.DrawBitmap(c, gv.cc.GetFromTileBitmapList("ui_bg_fullscreen_2d.png"), src2, dst2);
            
            if (currentMode.Equals("Node"))
            {
                setupNodePropertiesPanelControls();
                drawNodePanel(c);
            }
            else if (currentMode.Equals("Conditionals"))
            {
                setupNodeConditionalsPanelControls();
                drawCondPanel(c);
            }
            else if (currentMode.Equals("Actions"))
            {
                setupNodeActionsPanelControls();
                drawActionsPanel(c);
            }
            else if (currentMode.Equals("Copy"))
            {
                //setupWalkLoSPanelControls();
                //drawWalkLoSPanel();
            }
            else if (currentMode.Equals("Paste"))
            {
                //setup3DPreviewControls();
                //draw3DPreviewPanel();
                //drawMiniMap();
            }
            else if (currentMode.Equals("Settings"))
            {
                setupConvoPropertiesPanelControls();
                drawSettingsPanel(c);
            }

            btnNode.Draw(c);
            btnConditionals.Draw(c);
            btnActions.Draw(c);
            btnExpandAllNodes.Draw(c);
            btnCollapseAllNodes.Draw(c);
            btnSettings.Draw(c);

            gv.tsMainMenu.redrawTsMainMenu(c);

            if (gv.showMessageBox)
            {
                gv.messageBox.onDrawLogBox(c);
            }
        }
        public void drawSettingsPanel(SKCanvas c)
        {
            //Description     
            gv.DrawText(c, "CONVO PROPERTIES", panelLeftLocation, panelTopLocation, "gn");

            int shiftForFont = (tglSettingConvoFileName.Height / 2) - (gv.fontHeight / 2);
            tglSettingConvoFileName.Draw(c);
            gv.DrawText(c, "Convo Filename:", tglSettingConvoFileName.X + tglSettingConvoFileName.Width + gv.scaler, tglSettingConvoFileName.Y, "gy");
            gv.DrawText(c, gv.mod.currentConvo.ConvoFileName, tglSettingConvoFileName.X + tglSettingConvoFileName.Width + gv.scaler, tglSettingConvoFileName.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            tglSettingDefaultNpcName.Draw(c);
            gv.DrawText(c, "NPC Name:", tglSettingDefaultNpcName.X + tglSettingDefaultNpcName.Width + gv.scaler, tglSettingDefaultNpcName.Y, "gy");
            gv.DrawText(c, gv.mod.currentConvo.DefaultNpcName, tglSettingDefaultNpcName.X + tglSettingDefaultNpcName.Width + gv.scaler, tglSettingDefaultNpcName.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            tglSettingNpcPortraitBitmap.Draw(c);
            gv.DrawText(c, "NPC Portrait:", tglSettingNpcPortraitBitmap.X + tglSettingNpcPortraitBitmap.Width + gv.scaler, tglSettingNpcPortraitBitmap.Y, "gy");
            gv.DrawText(c, gv.mod.currentConvo.NpcPortraitBitmap, tglSettingNpcPortraitBitmap.X + tglSettingNpcPortraitBitmap.Width + gv.scaler, tglSettingNpcPortraitBitmap.Y + gv.fontHeight + gv.fontLineSpacing, "wh");
            
            if (gv.mod.currentConvo.Narration) { tglSettingNarration.toggleOn = true; }
            else { tglSettingNarration.toggleOn = false; }
            tglSettingNarration.Draw(c);
            gv.DrawText(c, "Is Narration", tglSettingNarration.X + tglSettingNarration.Width + gv.scaler, tglSettingNarration.Y, "gy");
            gv.DrawText(c, "Type Convo:", tglSettingNarration.X + tglSettingNarration.Width + gv.scaler, tglSettingNarration.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

            if (gv.mod.currentConvo.PartyChat) { tglSettingPartyChat.toggleOn = true; }
            else { tglSettingPartyChat.toggleOn = false; }
            tglSettingPartyChat.Draw(c);
            gv.DrawText(c, "Is Party Chat", tglSettingPartyChat.X + tglSettingPartyChat.Width + gv.scaler, tglSettingPartyChat.Y, "gy");
            gv.DrawText(c, "Type Convo:", tglSettingPartyChat.X + tglSettingPartyChat.Width + gv.scaler, tglSettingPartyChat.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

            if (gv.mod.currentConvo.SpeakToMainPcOnly) { tglSettingSpeakToMainPcOnly.toggleOn = true; }
            else { tglSettingSpeakToMainPcOnly.toggleOn = false; }
            tglSettingSpeakToMainPcOnly.Draw(c);
            gv.DrawText(c, "Speak To Main", tglSettingSpeakToMainPcOnly.X + tglSettingSpeakToMainPcOnly.Width + gv.scaler, tglSettingSpeakToMainPcOnly.Y, "gy");
            gv.DrawText(c, "PC Only:", tglSettingSpeakToMainPcOnly.X + tglSettingSpeakToMainPcOnly.Width + gv.scaler, tglSettingSpeakToMainPcOnly.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

        }
        public void drawNodePanel(SKCanvas c)
        {
            //Description     
            gv.DrawText(c, "NODE PROPERTIES", panelLeftLocation, panelTopLocation, "gn");

            if (editNode == null) { return; }

            if ((editNode != null) && (editNode != gv.mod.currentConvo.subNodes[0]))
            {
                int shiftForFont = (tglNodeText.Height / 2) - (gv.fontHeight / 2);
                tglNodeText.Draw(c);
                gv.DrawText(c, "Node Text:", tglNodeText.X + tglNodeText.Width + gv.scaler, tglNodeText.Y, "gy");
                gv.DrawText(c, editNode.conversationText, tglNodeText.X + tglNodeText.Width + gv.scaler, tglNodeText.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                tglNodeNpcName.Draw(c);
                gv.DrawText(c, "Node NPC Name:", tglNodeNpcName.X + tglNodeNpcName.Width + gv.scaler, tglNodeNpcName.Y, "gy");
                gv.DrawText(c, editNode.NodeNpcName, tglNodeNpcName.X + tglNodeNpcName.Width + gv.scaler, tglNodeNpcName.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                tglNodeNpcPortraitBitmap.Draw(c);
                gv.DrawText(c, "Node NPC Portrait:", tglNodeNpcPortraitBitmap.X + tglNodeNpcPortraitBitmap.Width + gv.scaler, tglNodeNpcPortraitBitmap.Y, "gy");
                gv.DrawText(c, editNode.NodePortraitBitmap, tglNodeNpcPortraitBitmap.X + tglNodeNpcPortraitBitmap.Width + gv.scaler, tglNodeNpcPortraitBitmap.Y + gv.fontHeight + gv.fontLineSpacing, "wh");

                if (editNode.ShowOnlyOnce) { tglShowOnlyOnce.toggleOn = true; }
                else { tglShowOnlyOnce.toggleOn = false; }
                tglShowOnlyOnce.Draw(c);
                gv.DrawText(c, "Show This Node", tglShowOnlyOnce.X + tglShowOnlyOnce.Width + gv.scaler, tglShowOnlyOnce.Y, "gy");
                gv.DrawText(c, "Only Once:", tglShowOnlyOnce.X + tglShowOnlyOnce.Width + gv.scaler, tglShowOnlyOnce.Y + gv.fontHeight + gv.fontLineSpacing, "gy");

                gv.DrawText(c, "Node Id#: " + editNode.idNum, tglShowOnlyOnce.X + (gv.fontHeight + gv.fontLineSpacing) * 1, tglShowOnlyOnce.Y + (gv.fontHeight + gv.fontLineSpacing) * 3, "gy");
                gv.DrawText(c, "Order Index: " + editNode.orderNum, tglShowOnlyOnce.X + (gv.fontHeight + gv.fontLineSpacing) * 1, tglShowOnlyOnce.Y + (gv.fontHeight + gv.fontLineSpacing) * 4, "gy");
                gv.DrawText(c, "Link To Id#: " + editNode.linkTo, tglShowOnlyOnce.X + (gv.fontHeight + gv.fontLineSpacing) * 1, tglShowOnlyOnce.Y + (gv.fontHeight + gv.fontLineSpacing) * 5, "gy");
                gv.DrawText(c, "Parent Id#: " + editNode.parentIdNum, tglShowOnlyOnce.X + (gv.fontHeight + gv.fontLineSpacing) * 1, tglShowOnlyOnce.Y + (gv.fontHeight + gv.fontLineSpacing) * 6, "gy");
            }
            btnNodeAdd.Draw(c);
            btnNodeMoveUp.Draw(c);
            btnNodeMoveDown.Draw(c);
            btnNodeRemove.Draw(c);
            btnNodeCopy.Draw(c);
            btnNodePaste.Draw(c);
            btnNodePasteAsLink.Draw(c);
            btnNodeRelocateCopiedNodes.Draw(c);
            btnFollowLink.Draw(c);
        }
        public void drawCondPanel(SKCanvas c)
        {
            //Description     
            gv.DrawText(c, "NODE CONDITIONALS", panelLeftLocation, panelTopLocation, "gn");

            if (editNode == null) { return; }

            if (editNode.conditions.Count > 0)
            {
                tglCond1Radio.Draw(c);
                int shiftForFont = (tglCond1Radio.Height / 2) - (gv.fontHeight / 2);                
                gv.DrawText(c, editNode.conditions[0].c_script, tglCond1Radio.X + tglCond1Radio.Width + gv.scaler, tglCond1Radio.Y, "wh");
                string parm1 = "";
                string parm2 = "";
                string parm3 = "";
                string parm4 = "";
                if (editNode.conditions[0].c_parameter_1 != null) { parm1 = editNode.conditions[0].c_parameter_1; }
                if (editNode.conditions[0].c_parameter_2 != null) { parm2 = editNode.conditions[0].c_parameter_2; }
                if (editNode.conditions[0].c_parameter_3 != null) { parm3 = editNode.conditions[0].c_parameter_3; }
                if (editNode.conditions[0].c_parameter_4 != null) { parm4 = editNode.conditions[0].c_parameter_4; }
                string total = "(" + parm1 + "," + parm2 + "," + parm3 + "," + parm4 + ")";
                gv.DrawText(c, total, tglCond1Radio.X + tglCond1Radio.Width + gv.scaler, tglCond1Radio.Y + shiftForFont * 2, "wh");
            }
            if (editNode.conditions.Count > 1)
            {
                tglCond2Radio.Draw(c);
                int shiftForFont = (tglCond2Radio.Height / 2) - (gv.fontHeight / 2);
                gv.DrawText(c, editNode.conditions[1].c_script, tglCond2Radio.X + tglCond2Radio.Width + gv.scaler, tglCond2Radio.Y, "wh");
                string parm1 = "";
                string parm2 = "";
                string parm3 = "";
                string parm4 = "";
                if (editNode.conditions[1].c_parameter_1 != null) { parm1 = editNode.conditions[1].c_parameter_1; }
                if (editNode.conditions[1].c_parameter_2 != null) { parm2 = editNode.conditions[1].c_parameter_2; }
                if (editNode.conditions[1].c_parameter_3 != null) { parm3 = editNode.conditions[1].c_parameter_3; }
                if (editNode.conditions[1].c_parameter_4 != null) { parm4 = editNode.conditions[1].c_parameter_4; }
                string total = "(" + parm1 + "," + parm2 + "," + parm3 + "," + parm4 + ")";
                gv.DrawText(c, total, tglCond2Radio.X + tglCond2Radio.Width + gv.scaler, tglCond2Radio.Y + shiftForFont * 2, "wh");
            }
            if (editNode.conditions.Count > 2)
            {
                tglCond3Radio.Draw(c);
                int shiftForFont = (tglCond3Radio.Height / 2) - (gv.fontHeight / 2);
                gv.DrawText(c, editNode.conditions[2].c_script, tglCond3Radio.X + tglCond3Radio.Width + gv.scaler, tglCond3Radio.Y, "wh");
                string parm1 = "";
                string parm2 = "";
                string parm3 = "";
                string parm4 = "";
                if (editNode.conditions[2].c_parameter_1 != null) { parm1 = editNode.conditions[2].c_parameter_1; }
                if (editNode.conditions[2].c_parameter_2 != null) { parm2 = editNode.conditions[2].c_parameter_2; }
                if (editNode.conditions[2].c_parameter_3 != null) { parm3 = editNode.conditions[2].c_parameter_3; }
                if (editNode.conditions[2].c_parameter_4 != null) { parm4 = editNode.conditions[2].c_parameter_4; }
                string total = "(" + parm1 + "," + parm2 + "," + parm3 + "," + parm4 + ")";
                gv.DrawText(c, total, tglCond3Radio.X + tglCond3Radio.Width + gv.scaler, tglCond3Radio.Y + shiftForFont * 2, "wh");
            }
            if (editNode.conditions.Count > 3)
            {
                tglCond4Radio.Draw(c);
                int shiftForFont = (tglCond4Radio.Height / 2) - (gv.fontHeight / 2);
                gv.DrawText(c, editNode.conditions[3].c_script, tglCond4Radio.X + tglCond4Radio.Width + gv.scaler, tglCond4Radio.Y, "wh");
                string parm1 = "";
                string parm2 = "";
                string parm3 = "";
                string parm4 = "";
                if (editNode.conditions[3].c_parameter_1 != null) { parm1 = editNode.conditions[3].c_parameter_1; }
                if (editNode.conditions[3].c_parameter_2 != null) { parm2 = editNode.conditions[3].c_parameter_2; }
                if (editNode.conditions[3].c_parameter_3 != null) { parm3 = editNode.conditions[3].c_parameter_3; }
                if (editNode.conditions[3].c_parameter_4 != null) { parm4 = editNode.conditions[3].c_parameter_4; }
                string total = "(" + parm1 + "," + parm2 + "," + parm3 + "," + parm4 + ")";
                gv.DrawText(c, total, tglCond4Radio.X + tglCond4Radio.Width + gv.scaler, tglCond4Radio.Y + shiftForFont * 2, "wh");
            }

            int index = 0;
            if (tglCond2Radio.toggleOn) { index = 1; }
            else if (tglCond3Radio.toggleOn) { index = 2; }
            else if (tglCond4Radio.toggleOn) { index = 3; }
            else { tglCond1Radio.toggleOn = true; }
            if (index >= editNode.conditions.Count)
            {
                tglCond1Radio.toggleOn = true;
                tglCond2Radio.toggleOn = false;
                tglCond3Radio.toggleOn = false;
                tglCond4Radio.toggleOn = false;
                index = 0;
            }
            if (editNode.conditions.Count > 0)
            {
                int shiftForFont = (tglCond1Radio.Height / 2) - (gv.fontHeight / 2);
                string parm1 = "";
                string parm2 = "";
                string parm3 = "";
                string parm4 = "";
                if (editNode.conditions[index].c_parameter_1 != null) { parm1 = editNode.conditions[index].c_parameter_1; }
                if (editNode.conditions[index].c_parameter_2 != null) { parm2 = editNode.conditions[index].c_parameter_2; }
                if (editNode.conditions[index].c_parameter_3 != null) { parm3 = editNode.conditions[index].c_parameter_3; }
                if (editNode.conditions[index].c_parameter_4 != null) { parm4 = editNode.conditions[index].c_parameter_4; }
                gv.DrawText(c, "SCRIPT + PARMS", panelLeftLocation, tglCondScript.Y - gv.fontHeight, "gn");
                tglCondScript.Draw(c);
                gv.DrawText(c, editNode.conditions[index].c_script, tglCondScript.X + tglCondScript.Width + gv.scaler, tglCondScript.Y + shiftForFont, "wh");
                tglCondParm1.Draw(c);
                gv.DrawText(c, "P1:" + parm1, tglCondParm1.X + tglCondParm1.Width + gv.scaler, tglCondParm1.Y + shiftForFont, "wh");
                tglCondParm2.Draw(c);
                gv.DrawText(c, "P2:" + parm2, tglCondParm2.X + tglCondParm2.Width + gv.scaler, tglCondParm2.Y + shiftForFont, "wh");
                tglCondParm3.Draw(c);
                gv.DrawText(c, "P3:" + parm3, tglCondParm3.X + tglCondParm3.Width + gv.scaler, tglCondParm3.Y + shiftForFont, "wh");
                tglCondParm4.Draw(c);
                gv.DrawText(c, "P4:" + parm4, tglCondParm4.X + tglCondParm4.Width + gv.scaler, tglCondParm4.Y + shiftForFont, "wh");
            }

            btnCondAdd.Draw(c);
            btnCondMoveUp.Draw(c);
            btnCondMoveDown.Draw(c);
            btnCondRemove.Draw(c);
            btnCondCopy.Draw(c);
            btnCondPaste.Draw(c);
        }
        public void drawActionsPanel(SKCanvas c)
        {
            //Description     
            gv.DrawText(c, "NODE ACTIONS", panelLeftLocation, panelTopLocation, "gn");

            if (editNode == null) { return; }

            if (editNode.actions.Count > 0)
            {
                tglAction1Radio.Draw(c);
                int shiftForFont = (tglAction1Radio.Height / 2) - (gv.fontHeight / 2);
                gv.DrawText(c, editNode.actions[0].a_script, tglAction1Radio.X + tglAction1Radio.Width + gv.scaler, tglAction1Radio.Y, "wh");
                string parm1 = "";
                string parm2 = "";
                string parm3 = "";
                string parm4 = "";
                if (editNode.actions[0].a_parameter_1 != null) { parm1 = editNode.actions[0].a_parameter_1; }
                if (editNode.actions[0].a_parameter_2 != null) { parm2 = editNode.actions[0].a_parameter_2; }
                if (editNode.actions[0].a_parameter_3 != null) { parm3 = editNode.actions[0].a_parameter_3; }
                if (editNode.actions[0].a_parameter_4 != null) { parm4 = editNode.actions[0].a_parameter_4; }
                string total = "(" + parm1 + "," + parm2 + "," + parm3 + "," + parm4 + ")";
                gv.DrawText(c, total, tglAction1Radio.X + tglAction1Radio.Width + gv.scaler, tglAction1Radio.Y + shiftForFont * 2, "wh");
            }
            if (editNode.actions.Count > 1)
            {
                tglAction2Radio.Draw(c);
                int shiftForFont = (tglAction2Radio.Height / 2) - (gv.fontHeight / 2);
                gv.DrawText(c, editNode.actions[1].a_script, tglAction2Radio.X + tglAction2Radio.Width + gv.scaler, tglAction2Radio.Y, "wh");
                string parm1 = "";
                string parm2 = "";
                string parm3 = "";
                string parm4 = "";
                if (editNode.actions[1].a_parameter_1 != null) { parm1 = editNode.actions[1].a_parameter_1; }
                if (editNode.actions[1].a_parameter_2 != null) { parm2 = editNode.actions[1].a_parameter_2; }
                if (editNode.actions[1].a_parameter_3 != null) { parm3 = editNode.actions[1].a_parameter_3; }
                if (editNode.actions[1].a_parameter_4 != null) { parm4 = editNode.actions[1].a_parameter_4; }
                string total = "(" + parm1 + "," + parm2 + "," + parm3 + "," + parm4 + ")";
                gv.DrawText(c, total, tglAction2Radio.X + tglAction2Radio.Width + gv.scaler, tglAction2Radio.Y + shiftForFont * 2, "wh");
            }
            if (editNode.actions.Count > 2)
            {
                tglAction3Radio.Draw(c);
                int shiftForFont = (tglAction3Radio.Height / 2) - (gv.fontHeight / 2);
                gv.DrawText(c, editNode.actions[2].a_script, tglAction3Radio.X + tglAction3Radio.Width + gv.scaler, tglAction3Radio.Y, "wh");
                string parm1 = "";
                string parm2 = "";
                string parm3 = "";
                string parm4 = "";
                if (editNode.actions[2].a_parameter_1 != null) { parm1 = editNode.actions[2].a_parameter_1; }
                if (editNode.actions[2].a_parameter_2 != null) { parm2 = editNode.actions[2].a_parameter_2; }
                if (editNode.actions[2].a_parameter_3 != null) { parm3 = editNode.actions[2].a_parameter_3; }
                if (editNode.actions[2].a_parameter_4 != null) { parm4 = editNode.actions[2].a_parameter_4; }
                string total = "(" + parm1 + "," + parm2 + "," + parm3 + "," + parm4 + ")";
                gv.DrawText(c, total, tglAction3Radio.X + tglAction3Radio.Width + gv.scaler, tglAction3Radio.Y + shiftForFont * 2, "wh");
            }
            if (editNode.actions.Count > 3)
            {
                tglAction4Radio.Draw(c);
                int shiftForFont = (tglAction4Radio.Height / 2) - (gv.fontHeight / 2);
                gv.DrawText(c, editNode.actions[3].a_script, tglAction4Radio.X + tglAction4Radio.Width + gv.scaler, tglAction4Radio.Y, "wh");
                string parm1 = "";
                string parm2 = "";
                string parm3 = "";
                string parm4 = "";
                if (editNode.actions[3].a_parameter_1 != null) { parm1 = editNode.actions[3].a_parameter_1; }
                if (editNode.actions[3].a_parameter_2 != null) { parm2 = editNode.actions[3].a_parameter_2; }
                if (editNode.actions[3].a_parameter_3 != null) { parm3 = editNode.actions[3].a_parameter_3; }
                if (editNode.actions[3].a_parameter_4 != null) { parm4 = editNode.actions[3].a_parameter_4; }
                string total = "(" + parm1 + "," + parm2 + "," + parm3 + "," + parm4 + ")";
                gv.DrawText(c, total, tglAction4Radio.X + tglAction4Radio.Width + gv.scaler, tglAction4Radio.Y + shiftForFont * 2, "wh");
            }

            int index = 0;
            if (tglAction2Radio.toggleOn) { index = 1; }
            else if (tglAction3Radio.toggleOn) { index = 2; }
            else if (tglAction4Radio.toggleOn) { index = 3; }
            else { tglAction1Radio.toggleOn = true; }
            if (index >= editNode.actions.Count)
            {
                tglAction1Radio.toggleOn = true;
                tglAction2Radio.toggleOn = false;
                tglAction3Radio.toggleOn = false;
                tglAction4Radio.toggleOn = false;
                index = 0;
            }
            if (editNode.actions.Count > 0)
            {
                int shiftForFont = (tglAction1Radio.Height / 2) - (gv.fontHeight / 2);
                string parm1 = "";
                string parm2 = "";
                string parm3 = "";
                string parm4 = "";
                if (editNode.actions[index].a_parameter_1 != null) { parm1 = editNode.actions[index].a_parameter_1; }
                if (editNode.actions[index].a_parameter_2 != null) { parm2 = editNode.actions[index].a_parameter_2; }
                if (editNode.actions[index].a_parameter_3 != null) { parm3 = editNode.actions[index].a_parameter_3; }
                if (editNode.actions[index].a_parameter_4 != null) { parm4 = editNode.actions[index].a_parameter_4; }
                gv.DrawText(c, "SCRIPT + PARMS", panelLeftLocation, tglActionScript.Y - gv.fontHeight, "gn");
                tglActionScript.Draw(c);
                gv.DrawText(c, editNode.actions[index].a_script, tglActionScript.X + tglActionScript.Width + gv.scaler, tglActionScript.Y + shiftForFont, "wh");
                tglActionParm1.Draw(c);
                gv.DrawText(c, "P1:" + parm1, tglActionParm1.X + tglActionParm1.Width + gv.scaler, tglActionParm1.Y + shiftForFont, "wh");
                tglActionParm2.Draw(c);
                gv.DrawText(c, "P2:" + parm2, tglActionParm2.X + tglActionParm2.Width + gv.scaler, tglActionParm2.Y + shiftForFont, "wh");
                tglActionParm3.Draw(c);
                gv.DrawText(c, "P3:" + parm3, tglActionParm3.X + tglActionParm3.Width + gv.scaler, tglActionParm3.Y + shiftForFont, "wh");
                tglActionParm4.Draw(c);
                gv.DrawText(c, "P4:" + parm4, tglActionParm4.X + tglActionParm4.Width + gv.scaler, tglActionParm4.Y + shiftForFont, "wh");
            }

            btnActionAdd.Draw(c);
            btnActionMoveUp.Draw(c);
            btnActionMoveDown.Draw(c);
            btnActionRemove.Draw(c);
            btnActionCopy.Draw(c);
            btnActionPaste.Draw(c);
        }

        public void onTouchTsConvoEditor(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnNode.glowOn = false;
            btnConditionals.glowOn = false;
            btnActions.glowOn = false;
            btnExpandAllNodes.glowOn = false;
            btnCollapseAllNodes.glowOn = false;
            btnSettings.glowOn = false;
            
            if (gv.showMessageBox)
            {
                gv.messageBox.btnReturn.glowOn = false;
            }

            gv.showTooltip = false;

            bool ret = gv.tsMainMenu.onTouchTsMainMenu(eX, eY, eventType);
            if (ret) { return; } //did some action on the main menu so do nothing here

            if (currentMode.Equals("Conditionals"))
            {
                ret = onTouchCondPanel(eX, eY, eventType);
                if (ret) { return; } //did some action on the conditionals panel so do nothing here
            }
            else if (currentMode.Equals("Settings"))
            {
                ret = onTouchSettingsPanel(eX, eY, eventType);
                if (ret) { return; } //did some action on the Settings panel so do nothing here
            }
            else if (currentMode.Equals("Node"))
            {
                ret = onTouchNodePanel(eX, eY, eventType);
                if (ret) { return; } //did some action on the Info panel so do nothing here
            }
            else if (currentMode.Equals("Actions"))
            {
                ret = onTouchActionsPanel(eX, eY, eventType);
                if (ret) { return; } //did some action on the Walk-LoS panel so do nothing here
            }
            else if (currentMode.Equals("Copy"))
            {
                ResetTreeView();
                //ret = onTouchTriggerPanel(eX, eY, e, eventType);
                //if (ret) { return; } //did some action on the Walk-LoS panel so do nothing here
            }
            else if (currentMode.Equals("Paste"))
            {
                //ret = onTouch3DPreviewPanel(eX, eY, e, eventType);
                //if (ret) { return; } //did some action on the 3DPreview panel so do nothing here
            }

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (gv.showMessageBox)
                    {
                        if (gv.messageBox.btnReturn.getImpact(x, y))
                        {
                            gv.messageBox.btnReturn.glowOn = true;
                        }
                        return;
                    }

                    if (btnNode.getImpact(x, y))
                    {
                        btnNode.glowOn = true;
                    }
                    else if (btnConditionals.getImpact(x, y))
                    {
                        btnConditionals.glowOn = true;
                    }
                    else if (btnActions.getImpact(x, y))
                    {
                        btnActions.glowOn = true;
                    }
                    else if (btnExpandAllNodes.getImpact(x, y))
                    {
                        btnExpandAllNodes.glowOn = true;
                    }
                    else if (btnCollapseAllNodes.getImpact(x, y))
                    {
                        btnCollapseAllNodes.glowOn = true;
                    }
                    else if (btnSettings.getImpact(x, y))
                    {
                        btnSettings.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnNode.glowOn = false;
                    btnConditionals.glowOn = false;
                    btnActions.glowOn = false;
                    btnExpandAllNodes.glowOn = false;
                    btnCollapseAllNodes.glowOn = false;
                    btnSettings.glowOn = false;
                    
                    if (gv.showMessageBox)
                    {
                        gv.messageBox.btnReturn.glowOn = false;
                    }
                    if (gv.showMessageBox)
                    {
                        if (gv.messageBox.btnReturn.getImpact(x, y))
                        {
                            gv.showMessageBox = false;
                        }
                        return;
                    }
                    
                    //figure out if tapped on a map square
                    if (tapInMapViewport(x, y))
                    {
                        int convoPanelLeftLocation = (int)(1 * gv.uiSquareSize + 2 * gv.scaler);
                        int convoPanelTopLocation = (int)(2 * gv.scaler + gv.fontHeight + gv.fontLineSpacing);
                        //int tlX = panelLeftLocation + (gv.ibbMiniTglWidth / 2 * gv.scaler * n.indentMultiplier);
                        //int tlY = panelTopLocation + ((gv.fontHeight + gv.fontLineSpacing) * cnt);
                        int lineIndex = (y - convoPanelTopLocation) / (gv.fontHeight + gv.fontLineSpacing) + currentTopLineIndex;
                        if (lineIndex < nodeList.Count)
                        {
                            tglCond1Radio.toggleOn = true;
                            tglCond2Radio.toggleOn = false;
                            tglCond3Radio.toggleOn = false;
                            tglCond4Radio.toggleOn = false;
                            tglAction1Radio.toggleOn = true;
                            tglAction2Radio.toggleOn = false;
                            tglAction3Radio.toggleOn = false;
                            tglAction4Radio.toggleOn = false;

                            editNode = nodeList[lineIndex];
                            int tlX = (int)(convoPanelLeftLocation + ((gv.ibbMiniTglWidth / 2) * gv.scaler * editNode.indentMultiplier));
                            if ((x > tlX) && (x < tlX + (gv.ibbMiniTglWidth / 2) * gv.scaler))
                            {
                                editNode.IsExpanded = !editNode.IsExpanded;
                                ResetTreeView();
                            }
                        }
                    }
                    

                    if (btnNode.getImpact(x, y))
                    {
                        currentMode = "Node";
                    }
                    else if (btnConditionals.getImpact(x, y))
                    {
                        currentMode = "Conditionals";
                    }
                    else if (btnActions.getImpact(x, y))
                    {
                        currentMode = "Actions";
                    }
                    else if (btnExpandAllNodes.getImpact(x, y))
                    {
                        expandAllNodes();
                        ResetTreeView();
                    }
                    else if (btnCollapseAllNodes.getImpact(x, y))
                    {
                        collapseAllNodes();
                        ResetTreeView();
                    }
                    else if (btnSettings.getImpact(x, y))
                    {
                        currentMode = "Settings";
                    }
                    break;
            }
        }
        public bool onTouchSettingsPanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    if (tglSettingConvoFileName.getImpact(x, y))
                    {
                        changeConvoFileName();
                    }
                    else if (tglSettingDefaultNpcName.getImpact(x, y))
                    {
                        changeDefaultNpcName();
                    }
                    else if (tglSettingNpcPortraitBitmap.getImpact(x, y))
                    {
                        gv.screenType = "portraitSelector";
                        gv.screenPortraitSelector.resetPortraitSelector("tsConvoEditor", null);
                    }
                    else if (tglSettingNarration.getImpact(x, y))
                    {
                        tglSettingNarration.toggleOn = !tglSettingNarration.toggleOn;
                        gv.mod.currentConvo.Narration = tglSettingNarration.toggleOn;
                    }
                    else if (tglSettingPartyChat.getImpact(x, y))
                    {
                        tglSettingPartyChat.toggleOn = !tglSettingPartyChat.toggleOn;
                        gv.mod.currentConvo.PartyChat = tglSettingPartyChat.toggleOn;
                    }
                    else if (tglSettingSpeakToMainPcOnly.getImpact(x, y))
                    {
                        tglSettingSpeakToMainPcOnly.toggleOn = !tglSettingSpeakToMainPcOnly.toggleOn;
                        gv.mod.currentConvo.SpeakToMainPcOnly = tglSettingSpeakToMainPcOnly.toggleOn;
                    }
                    break;
            }
            return false;
        }
        public bool onTouchNodePanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnNodeAdd.glowOn = false;
            btnNodeCopy.glowOn = false;
            btnNodeMoveDown.glowOn = false;
            btnNodeMoveUp.glowOn = false;
            btnNodeRemove.glowOn = false;
            btnNodePaste.glowOn = false;
            btnNodePasteAsLink.glowOn = false;
            btnNodeRelocateCopiedNodes.glowOn = false;
            btnFollowLink.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if (btnNodeAdd.getImpact(x, y))
                    {
                        btnNodeAdd.glowOn = true;
                        gv.tooltip.AddHtmlTextToLog("<yl>Add Node</yl><br>Adds a child node to the currently selected node.");
                        gv.showTooltip = true;
                        return true;
                    }
                    else if (btnNodeMoveUp.getImpact(x, y))
                    {
                        btnNodeMoveUp.glowOn = true;
                        gv.tooltip.AddHtmlTextToLog("<yl>Move Node Up</yl><br>Moves the selected node up in the list of child nodes.");
                        gv.showTooltip = true;
                        return true;
                    }
                    else if (btnNodeMoveDown.getImpact(x, y))
                    {
                        btnNodeMoveDown.glowOn = true;
                        gv.tooltip.AddHtmlTextToLog("<yl>Move Node Down</yl><br>Moves the selected node down in the list of child nodes.");
                        gv.showTooltip = true;
                        return true;
                    }
                    else if (btnNodeRemove.getImpact(x, y))
                    {
                        btnNodeRemove.glowOn = true;
                        gv.tooltip.AddHtmlTextToLog("<yl>Remove Node</yl><br>Removes the selected node.");
                        gv.showTooltip = true;
                        return true;
                    }
                    else if (btnNodeCopy.getImpact(x, y))
                    {
                        btnNodeCopy.glowOn = true;
                        gv.tooltip.AddHtmlTextToLog("<yl>Copy Node</yl><br>Copies the selected node (and subnodes if any) to the clipboard for pasting/moving/linking later.");
                        gv.showTooltip = true;
                        return true;
                    }
                    else if (btnNodePaste.getImpact(x, y))
                    {
                        btnNodePaste.glowOn = true;
                        gv.tooltip.AddHtmlTextToLog("<yl>Paste Node</yl><br>Pastes the previously copied node (and subnodes if any) as a child of the currently selected node.");
                        gv.showTooltip = true;
                        return true;
                    }
                    else if (btnNodePasteAsLink.getImpact(x, y))
                    {
                        btnNodePasteAsLink.glowOn = true;
                        gv.tooltip.AddHtmlTextToLog("<yl>Paste as Link Node</yl><br>Pastes a link only of the previously copied node to the currently selected node as a child.");
                        gv.showTooltip = true;
                        return true;
                    }
                    else if (btnNodeRelocateCopiedNodes.getImpact(x, y))
                    {
                        btnNodeRelocateCopiedNodes.glowOn = true;
                        gv.tooltip.AddHtmlTextToLog("<yl>Relocate Node</yl><br>Moves the previously copied node (and subnodes if any) to the currently selected node as a child.");
                        gv.showTooltip = true;
                        return true;
                    }
                    else if (btnFollowLink.getImpact(x, y))
                    {
                        btnFollowLink.glowOn = true;
                        gv.tooltip.AddHtmlTextToLog("<yl>Follow Link</yl><br>select a 'grey' text node and then click this button and it will take you to the actual node that this node is linked to...will highlight the linked node with green text.");
                        gv.showTooltip = true;
                        return true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnNodeAdd.glowOn = false;
                    btnNodeCopy.glowOn = false;
                    btnNodeMoveDown.glowOn = false;
                    btnNodeMoveUp.glowOn = false;
                    btnNodeRemove.glowOn = false;
                    btnNodePaste.glowOn = false;
                    btnNodePasteAsLink.glowOn = false;
                    btnNodeRelocateCopiedNodes.glowOn = false;
                    btnFollowLink.glowOn = false;

                    if ((editNode != null) && (editNode != gv.mod.currentConvo.subNodes[0]))
                    {
                        if (tglNodeText.getImpact(x, y))
                        {
                            changeNodeText();
                        }
                        else if (tglNodeNpcName.getImpact(x, y))
                        {
                            changeNodeNpcName();
                        }
                        else if (tglNodeNpcPortraitBitmap.getImpact(x, y))
                        {
                            gv.screenType = "tokenSelector";
                            gv.screenTokenSelector.resetTokenSelector("tsConvoEditor", null);
                        }
                        else if (tglShowOnlyOnce.getImpact(x, y))
                        {
                            tglShowOnlyOnce.toggleOn = !tglShowOnlyOnce.toggleOn;
                            editNode.ShowOnlyOnce = tglShowOnlyOnce.toggleOn;
                        }
                    }
                    if (btnNodeAdd.getImpact(x, y))
                    {
                        AddNode();
                    }
                    else if (btnNodeRemove.getImpact(x, y))
                    {
                        RemoveNode();
                    }
                    else if (btnNodeCopy.getImpact(x, y))
                    {
                        CopyNodes();
                    }
                    else if (btnNodePaste.getImpact(x, y))
                    {
                        PasteNodes();
                    }
                    else if (btnNodePasteAsLink.getImpact(x, y))
                    {
                        PasteAsLink();
                    }
                    else if (btnNodeMoveUp.getImpact(x, y))
                    {
                        MoveUp();
                    }
                    else if (btnNodeMoveDown.getImpact(x, y))
                    {
                        MoveDown();
                    }
                    else if (btnNodeRelocateCopiedNodes.getImpact(x, y))
                    {
                        PasteAsRelocatedNodes();
                    }
                    else if (btnFollowLink.getImpact(x, y))
                    {
                        FollowLink();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchCondPanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnCondAdd.glowOn = false;
            btnCondMoveUp.glowOn = false;
            btnCondMoveDown.glowOn = false;
            btnCondRemove.glowOn = false;
            btnCondCopy.glowOn = false;
            btnCondPaste.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if ((editNode != null) && (editNode != gv.mod.currentConvo.subNodes[0]))
                    {
                        if (btnCondAdd.getImpact(x, y))
                        {
                            btnCondAdd.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Add Conditional</yl><br>Adds a conditional to the currently selected node.");
                            gv.showTooltip = true;
                            return true;
                        }
                        else if (btnCondMoveUp.getImpact(x, y))
                        {
                            btnCondMoveUp.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Move Up Conditional</yl><br>Moves the currently selected conditional up one spot in the list of conditionals on this node.");
                            gv.showTooltip = true;
                            return true;
                        }
                        else if (btnCondMoveDown.getImpact(x, y))
                        {
                            btnCondMoveDown.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Move Down Conditional</yl><br>Moves the currently selected conditional down one spot in the list of conditionals on this node.");
                            gv.showTooltip = true;
                            return true;
                        }
                        else if (btnCondRemove.getImpact(x, y))
                        {
                            btnCondRemove.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Remove Conditional</yl><br>Removes the currently selected conditional.");
                            gv.showTooltip = true;
                            return true;
                        }
                        else if (btnCondCopy.getImpact(x, y))
                        {
                            btnCondCopy.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Copy Conditional</yl><br>Adds the currently selected conditional to the clipboard for pasting later.");
                            gv.showTooltip = true;
                            return true;
                        }
                        else if (btnCondPaste.getImpact(x, y))
                        {
                            btnCondPaste.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Paste Conditional</yl><br>Pastes the previously copied conditional from the clipboard to this node.");
                            gv.showTooltip = true;
                            return true;
                        }
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnCondAdd.glowOn = false;
                    btnCondMoveUp.glowOn = false;
                    btnCondMoveDown.glowOn = false;
                    btnCondRemove.glowOn = false;
                    btnCondCopy.glowOn = false;
                    btnCondPaste.glowOn = false;

                    if ((editNode != null) && (editNode != gv.mod.currentConvo.subNodes[0]))
                    {
                        if (tglCond1Radio.getImpact(x, y))
                        {
                            tglCond1Radio.toggleOn = true;
                            tglCond2Radio.toggleOn = false;
                            tglCond3Radio.toggleOn = false;
                            tglCond4Radio.toggleOn = false;
                        }
                        else if (tglCond2Radio.getImpact(x, y))
                        {
                            tglCond1Radio.toggleOn = false;
                            tglCond2Radio.toggleOn = true;
                            tglCond3Radio.toggleOn = false;
                            tglCond4Radio.toggleOn = false;
                        }
                        else if (tglCond3Radio.getImpact(x, y))
                        {
                            tglCond1Radio.toggleOn = false;
                            tglCond2Radio.toggleOn = false;
                            tglCond3Radio.toggleOn = true;
                            tglCond4Radio.toggleOn = false;
                        }
                        else if (tglCond4Radio.getImpact(x, y))
                        {
                            tglCond1Radio.toggleOn = false;
                            tglCond2Radio.toggleOn = false;
                            tglCond3Radio.toggleOn = false;
                            tglCond4Radio.toggleOn = true;
                        }
                    }
                    if (btnCondAdd.getImpact(x, y))
                    {
                        AddCond();
                    }
                    else if (btnCondMoveUp.getImpact(x, y))
                    {
                        MoveUpCond();
                    }
                    else if (btnCondMoveDown.getImpact(x, y))
                    {
                        MoveDownCond();
                    }
                    else if (btnCondRemove.getImpact(x, y))
                    {
                        RemoveCond();
                    }
                    else if (btnCondCopy.getImpact(x, y))
                    {
                        CopyCond();
                    }
                    else if (btnCondPaste.getImpact(x, y))
                    {
                        PasteCond();
                    }
                    else if (tglCondScript.getImpact(x,y))
                    {
                        changeCondScript();
                    }
                    else if (tglCondParm1.getImpact(x, y))
                    {
                        changeCondParm1();
                    }
                    else if (tglCondParm2.getImpact(x, y))
                    {
                        changeCondParm2();
                    }
                    else if (tglCondParm3.getImpact(x, y))
                    {
                        changeCondParm3();
                    }
                    else if (tglCondParm4.getImpact(x, y))
                    {
                        changeCondParm4();
                    }
                    break;
            }
            return false;
        }
        public bool onTouchActionsPanel(int eX, int eY, MouseEventType.EventType eventType)
        {
            btnActionAdd.glowOn = false;
            btnActionMoveUp.glowOn = false;
            btnActionMoveDown.glowOn = false;
            btnActionRemove.glowOn = false;
            btnActionCopy.glowOn = false;
            btnActionPaste.glowOn = false;

            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)eX;
                    int y = (int)eY;

                    if ((editNode != null) && (editNode != gv.mod.currentConvo.subNodes[0]))
                    {
                        if (btnActionAdd.getImpact(x, y))
                        {
                            btnActionAdd.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Add Action</yl><br>Adds an action to the currently selected node.");
                            gv.showTooltip = true;
                            return true;
                        }
                        else if (btnActionMoveUp.getImpact(x, y))
                        {
                            btnActionMoveUp.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Move Up Action</yl><br>Moves the currently selected action up one spot in the list of conditionals on this node.");
                            gv.showTooltip = true;
                            return true;
                        }
                        else if (btnActionMoveDown.getImpact(x, y))
                        {
                            btnActionMoveDown.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Move Down Action</yl><br>Moves the currently selected action down one spot in the list of conditionals on this node.");
                            gv.showTooltip = true;
                            return true;
                        }
                        else if (btnActionRemove.getImpact(x, y))
                        {
                            btnActionRemove.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Remove Action</yl><br>Removes the currently selected action.");
                            gv.showTooltip = true;
                            return true;
                        }
                        else if (btnActionCopy.getImpact(x, y))
                        {
                            btnActionCopy.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Copy Action</yl><br>Adds the currently selected action to the clipboard for pasting later.");
                            gv.showTooltip = true;
                            return true;
                        }
                        else if (btnActionPaste.getImpact(x, y))
                        {
                            btnActionPaste.glowOn = true;
                            gv.tooltip.AddHtmlTextToLog("<yl>Paste Action</yl><br>Pastes the previously copied action from the clipboard to this node.");
                            gv.showTooltip = true;
                            return true;
                        }
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)eX;
                    y = (int)eY;

                    btnActionAdd.glowOn = false;
                    btnActionMoveUp.glowOn = false;
                    btnActionMoveDown.glowOn = false;
                    btnActionRemove.glowOn = false;
                    btnActionCopy.glowOn = false;
                    btnActionPaste.glowOn = false;

                    if ((editNode != null) && (editNode != gv.mod.currentConvo.subNodes[0]))
                    {
                        if (tglAction1Radio.getImpact(x, y))
                        {
                            tglAction1Radio.toggleOn = true;
                            tglAction2Radio.toggleOn = false;
                            tglAction3Radio.toggleOn = false;
                            tglAction4Radio.toggleOn = false;
                        }
                        else if (tglAction2Radio.getImpact(x, y))
                        {
                            tglAction1Radio.toggleOn = false;
                            tglAction2Radio.toggleOn = true;
                            tglAction3Radio.toggleOn = false;
                            tglAction4Radio.toggleOn = false;
                        }
                        else if (tglAction3Radio.getImpact(x, y))
                        {
                            tglAction1Radio.toggleOn = false;
                            tglAction2Radio.toggleOn = false;
                            tglAction3Radio.toggleOn = true;
                            tglAction4Radio.toggleOn = false;
                        }
                        else if (tglAction4Radio.getImpact(x, y))
                        {
                            tglAction1Radio.toggleOn = false;
                            tglAction2Radio.toggleOn = false;
                            tglAction3Radio.toggleOn = false;
                            tglAction4Radio.toggleOn = true;
                        }
                    }
                    if (btnActionAdd.getImpact(x, y))
                    {
                        AddAction();
                    }
                    else if (btnActionMoveUp.getImpact(x, y))
                    {
                        MoveUpAction();
                    }
                    else if (btnActionMoveDown.getImpact(x, y))
                    {
                        MoveDownAction();
                    }
                    else if (btnActionRemove.getImpact(x, y))
                    {
                        RemoveAction();
                    }
                    else if (btnActionCopy.getImpact(x, y))
                    {
                        CopyAction();
                    }
                    else if (btnActionPaste.getImpact(x, y))
                    {
                        PasteAction();
                    }
                    else if (tglCondScript.getImpact(x, y))
                    {
                        changeActionScript();
                    }
                    else if (tglCondParm1.getImpact(x, y))
                    {
                        changeActionParm1();
                    }
                    else if (tglCondParm2.getImpact(x, y))
                    {
                        changeActionParm2();
                    }
                    else if (tglCondParm3.getImpact(x, y))
                    {
                        changeActionParm3();
                    }
                    else if (tglCondParm4.getImpact(x, y))
                    {
                        changeActionParm4();
                    }
                    break;
            }
            return false;
        }

        public void scrollToEnd()
        {
            SetCurrentTopLineIndex(nodeList.Count);
        }
        public void SetCurrentTopLineIndex(int changeValue)
        {
            currentTopLineIndex += changeValue;
            if (currentTopLineIndex > nodeList.Count - numberOfLinesToShow)
            {
                currentTopLineIndex = nodeList.Count - numberOfLinesToShow;
            }
            if (currentTopLineIndex < 0)
            {
                currentTopLineIndex = 0;
            }
        }
        public void SetCurrentTopLineAbsoluteIndex(int absoluteValue)
        {
            currentTopLineIndex = absoluteValue;
            if (currentTopLineIndex > nodeList.Count - numberOfLinesToShow)
            {
                currentTopLineIndex = nodeList.Count - numberOfLinesToShow;
            }
            if (currentTopLineIndex < 0)
            {
                currentTopLineIndex = 0;
            }
        }
        /*public void onMouseWheel(object sender, MouseEventArgs e)
        {
            int eX = e.X - gv.oXshift;
            int eY = e.Y - gv.oYshift;

            if (isMouseWithinTextBox(eX, eY))
            {
                // Update the drawing based upon the mouse wheel scrolling. 
                int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;

                if (numberOfTextLinesToMove != 0)
                {
                    SetCurrentTopLineIndex(-numberOfTextLinesToMove);
                }
            }
        }*/
        private bool isMouseWithinTextBox(int x, int y)
        {
            if (x < mapStartLocXinPixels) { return false; }
            if (y < 0) { return false; }
            if (x > mapStartLocXinPixels + gv.squareSize * gv.scaler * 10) { return false; }
            if (y > gv.squareSize * gv.scaler * 10) { return false; }
            return true;

        }
        public void onTouchSwipe(int eX, int eY, MouseEventType.EventType eventType)
        {
            if (isMouseWithinTextBox(eX, eY))
            {                
                switch (eventType)
                {
                    case MouseEventType.EventType.MouseDown:

                        touchIsDown = true;
                        lastTouchMoveLocationY = eY;
                        touchMoveDeltaY = 0;
                        break;

                    case MouseEventType.EventType.MouseMove:

                        touchMoveDeltaY = lastTouchMoveLocationY - eY;
                        if (touchMoveDeltaY > gv.fontHeight)
                        {
                            SetCurrentTopLineIndex(1);
                            touchMoveDeltaY = 0;
                            lastTouchMoveLocationY = eY;
                        }
                        else if (touchMoveDeltaY< -1 * gv.fontHeight)
                        {
                            SetCurrentTopLineIndex(-1);
                            touchMoveDeltaY = 0;
                            lastTouchMoveLocationY = eY;
                        }
                        break;

                    case MouseEventType.EventType.MouseUp:

                        touchIsDown = false;
                        touchMoveDeltaY = 0;
                        break;
                }
            }
        }
        public int getIndexOfCondSelected()
        {
            if ((tglCond1Radio.toggleOn) && (editNode.conditions.Count > 0))
            {
                return 0;
            }
            else if ((tglCond2Radio.toggleOn) && (editNode.conditions.Count > 1))
            {
                return 1;
            }
            else if ((tglCond3Radio.toggleOn) && (editNode.conditions.Count > 2))
            {
                return 2;
            }
            else if ((tglCond4Radio.toggleOn) && (editNode.conditions.Count > 3))
            {
                return 3;
            }
            return 0;
        }
        public int getIndexOfActionSelected()
        {
            if ((tglAction1Radio.toggleOn) && (editNode.actions.Count > 0))
            {
                return 0;
            }
            else if ((tglAction2Radio.toggleOn) && (editNode.actions.Count > 1))
            {
                return 1;
            }
            else if ((tglAction3Radio.toggleOn) && (editNode.actions.Count > 2))
            {
                return 2;
            }
            else if ((tglAction4Radio.toggleOn) && (editNode.actions.Count > 3))
            {
                return 3;
            }
            return 0;
        }

        public async void changeConvoFileName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Change the Conversation Filename:", gv.mod.currentConvo.ConvoFileName);
            gv.mod.currentConvo.ConvoFileName = myinput;
            gv.touchEnabled = true;
            //string title = "Change the Conversation Filename.";
            //gv.mod.currentConvo.ConvoFileName = gv.DialogReturnString(title, gv.mod.currentConvo.ConvoFileName);
        }
        public async void changeDefaultNpcName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Change the default NPC name that will be shown in the log box:", gv.mod.currentConvo.DefaultNpcName);
            gv.mod.currentConvo.DefaultNpcName = myinput;
            gv.touchEnabled = true;
            //string title = "Change the default NPC name that will be shown in the log box.";
            //gv.mod.currentConvo.DefaultNpcName = gv.DialogReturnString(title, gv.mod.currentConvo.DefaultNpcName);            
        }
        public async void changeNodeText()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Change this node's conversation text:", editNode.conversationText);
            editNode.conversationText = myinput;
            gv.touchEnabled = true;
            //string title = "Change this node's conversation text.";
            //editNode.conversationText = gv.DialogReturnString(title, editNode.conversationText);            
        }
        public async void changeNodeNpcName()
        {
            gv.touchEnabled = false;
            string myinput = await gv.StringInputBox("Change the node's NPC name that will be shown in the log box:", editNode.NodeNpcName);
            editNode.NodeNpcName = myinput;
            gv.touchEnabled = true;
            //string title = "Change the node's NPC name that will be shown in the log box.";
            //editNode.NodeNpcName = gv.DialogReturnString(title, editNode.NodeNpcName);
        }

        public async void changeActionScript()
        {            
            if (editNode == null) { return; }

            List<string> types = new List<string>(); //container, transition, conversation, encounter, script
            types.Add("none");
            foreach (ScriptObject s in gv.cc.scriptList)
            {
                if ((s.name.StartsWith("ga")) || (s.name.StartsWith("os")))
                {
                    types.Add(s.name);
                }
            }

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(types, "Select a script from the list");
            if ((editNode.actions[getIndexOfActionSelected()].a_script.Equals(selected)) || (editNode.actions[getIndexOfActionSelected()].a_script.Equals(selected + ".cs")))
            {
                //don't change it
            }
            else
            {
                editNode.actions[getIndexOfActionSelected()].a_script = selected;
                editNode.actions[getIndexOfActionSelected()].a_parameter_1 = null;
                editNode.actions[getIndexOfActionSelected()].a_parameter_2 = null;
                editNode.actions[getIndexOfActionSelected()].a_parameter_3 = null;
                editNode.actions[getIndexOfActionSelected()].a_parameter_4 = null;
            }
            gv.touchEnabled = true;           
        }
        public async void changeActionParm1()
        {
            try
            {
                if (editNode == null) { return; }
                ScriptObject script = gv.cc.GetScriptObjectByName(editNode.actions[getIndexOfActionSelected()].a_script);
                if (script == null) { return; }
                if (script.name.Equals("none")) { return; }
                if (script.parmType1 == parmType.none) { return; }

                string title = script.description + Environment.NewLine + "Enter the first parameter for this script:" + Environment.NewLine + script.parmDescription1;
                //check if a string entry or list type
                if (script.parmType1 == parmType.strg)
                {
                    gv.touchEnabled = false;
                    string myinput = await gv.StringInputBox(title, editNode.actions[getIndexOfActionSelected()].a_parameter_1);
                    editNode.actions[getIndexOfActionSelected()].a_parameter_1 = myinput;
                    gv.touchEnabled = true;
                }
                else if ((script.parmType1 == parmType.large_int) || (script.parmType1 == parmType.mapX) || (script.parmType1 == parmType.mapY))
                {
                    gv.touchEnabled = false;
                    int myinput = await gv.NumInputBox(title, Convert.ToInt32(editNode.actions[getIndexOfActionSelected()].a_parameter_1));
                    editNode.actions[getIndexOfActionSelected()].a_parameter_1 = myinput.ToString();
                    gv.touchEnabled = true;
                }
                else //must be a list type
                {
                    List<string> items = gv.cc.GetListOfItems(script.parmType1);
                    if (items.Count == 0) { return; }
                    gv.touchEnabled = false;
                    string selected = await gv.ListViewPage(items, title);
                    if (selected != "none")
                    {
                        if (selected.Equals("create a new variable"))
                        {
                            string myinput = await gv.StringInputBox("Enter a new variable name:", "");
                            editNode.actions[getIndexOfActionSelected()].a_parameter_1 = myinput;
                            if (!gv.mod.moduleVariablesList.Contains(myinput))
                            {
                                gv.mod.moduleVariablesList.Add(myinput);
                            }
                        }
                        else
                        {
                            editNode.actions[getIndexOfActionSelected()].a_parameter_1 = selected;
                        }
                    }
                    gv.touchEnabled = true;
                }
            }
            catch (Exception ex)
            {
                gv.touchEnabled = true;
            }
        }
        public async void changeActionParm2()
        {
            try
            {
                if (editNode == null) { return; }
                ScriptObject script = gv.cc.GetScriptObjectByName(editNode.actions[getIndexOfActionSelected()].a_script);
                if (script == null) { return; }
                if (script.name.Equals("none")) { return; }
                if (script.parmType2 == parmType.none) { return; }

                string title = script.description + Environment.NewLine + "Enter the second parameter for this script:" + Environment.NewLine + script.parmDescription2;
                //check if a string entry or list type
                if (script.parmType2 == parmType.strg)
                {
                    gv.touchEnabled = false;
                    string myinput = await gv.StringInputBox(title, editNode.actions[getIndexOfActionSelected()].a_parameter_2);
                    editNode.actions[getIndexOfActionSelected()].a_parameter_2 = myinput;
                    gv.touchEnabled = true;
                }
                else if ((script.parmType2 == parmType.large_int) || (script.parmType2 == parmType.mapX) || (script.parmType2 == parmType.mapY))
                {
                    gv.touchEnabled = false;
                    int myinput = await gv.NumInputBox(title, Convert.ToInt32(editNode.actions[getIndexOfActionSelected()].a_parameter_2));
                    editNode.actions[getIndexOfActionSelected()].a_parameter_2 = myinput.ToString();
                    gv.touchEnabled = true;
                }
                else //must be a list type
                {
                    List<string> items = gv.cc.GetListOfItems(script.parmType2);
                    if (items.Count == 0) { return; }
                    gv.touchEnabled = false;
                    string selected = await gv.ListViewPage(items, title);
                    if (selected != "none")
                    {
                        if (selected.Equals("create a new variable"))
                        {
                            string myinput = await gv.StringInputBox("Enter a new variable name:", "");
                            editNode.actions[getIndexOfActionSelected()].a_parameter_2 = myinput;
                            if (!gv.mod.moduleVariablesList.Contains(myinput))
                            {
                                gv.mod.moduleVariablesList.Add(myinput);
                            }
                        }
                        else
                        {
                            editNode.actions[getIndexOfActionSelected()].a_parameter_2 = selected;
                        }
                    }
                }
                gv.touchEnabled = true;
            }
            catch (Exception ex)
            {
                gv.touchEnabled = true;
            }
        }
        public async void changeActionParm3()
        {
            try
            {
                if (editNode == null) { return; }
                ScriptObject script = gv.cc.GetScriptObjectByName(editNode.actions[getIndexOfActionSelected()].a_script);
                if (script == null) { return; }
                if (script.name.Equals("none")) { return; }
                if (script.parmType3 == parmType.none) { return; }

                string title = script.description + Environment.NewLine + "Enter the third parameter for this script:" + Environment.NewLine + script.parmDescription3;
                //check if a string entry or list type
                if (script.parmType3 == parmType.strg)
                {
                    gv.touchEnabled = false;
                    string myinput = await gv.StringInputBox(title, editNode.actions[getIndexOfActionSelected()].a_parameter_3);
                    editNode.actions[getIndexOfActionSelected()].a_parameter_3 = myinput;
                    gv.touchEnabled = true;
                }
                else if ((script.parmType3 == parmType.large_int) || (script.parmType3 == parmType.mapX) || (script.parmType3 == parmType.mapY))
                {
                    gv.touchEnabled = false;
                    int myinput = await gv.NumInputBox(title, Convert.ToInt32(editNode.actions[getIndexOfActionSelected()].a_parameter_3));
                    editNode.actions[getIndexOfActionSelected()].a_parameter_3 = myinput.ToString();
                    gv.touchEnabled = true;
                }
                else //must be a list type
                {
                    List<string> items = gv.cc.GetListOfItems(script.parmType3);
                    if (items.Count == 0) { return; }
                    gv.touchEnabled = false;
                    string selected = await gv.ListViewPage(items, title);
                    if (selected != "none")
                    {
                        if (selected.Equals("create a new variable"))
                        {
                            string myinput = await gv.StringInputBox("Enter a new variable name:", "");
                            editNode.actions[getIndexOfActionSelected()].a_parameter_3 = myinput;
                            if (!gv.mod.moduleVariablesList.Contains(myinput))
                            {
                                gv.mod.moduleVariablesList.Add(myinput);
                            }
                        }
                        else
                        {
                            editNode.actions[getIndexOfActionSelected()].a_parameter_3 = selected;
                        }
                    }
                    gv.touchEnabled = true;
                }
            }
            catch (Exception ex)
            {
                gv.touchEnabled = true;
            }
        }
        public async void changeActionParm4()
        {
            try
            {
                if (editNode == null) { return; }
                ScriptObject script = gv.cc.GetScriptObjectByName(editNode.actions[getIndexOfActionSelected()].a_script);
                if (script == null) { return; }
                if (script.name.Equals("none")) { return; }
                if (script.parmType4 == parmType.none) { return; }

                string title = script.description + Environment.NewLine + "Enter the fourth parameter for this script:" + Environment.NewLine + script.parmDescription4;
                //check if a string entry or list type
                if (script.parmType4 == parmType.strg)
                {
                    gv.touchEnabled = false;
                    string myinput = await gv.StringInputBox(title, editNode.actions[getIndexOfActionSelected()].a_parameter_4);
                    editNode.actions[getIndexOfActionSelected()].a_parameter_4 = myinput;
                    gv.touchEnabled = true;
                }
                else if ((script.parmType4 == parmType.large_int) || (script.parmType4 == parmType.mapX) || (script.parmType4 == parmType.mapY))
                {
                    gv.touchEnabled = false;
                    int myinput = await gv.NumInputBox(title, Convert.ToInt32(editNode.actions[getIndexOfActionSelected()].a_parameter_4));
                    editNode.actions[getIndexOfActionSelected()].a_parameter_4 = myinput.ToString();
                    gv.touchEnabled = true;
                }
                else //must be a list type
                {
                    List<string> items = gv.cc.GetListOfItems(script.parmType4);
                    if (items.Count == 0) { return; }
                    gv.touchEnabled = false;
                    string selected = await gv.ListViewPage(items, title);
                    if (selected != "none")
                    {
                        if (selected.Equals("create a new variable"))
                        {
                            string myinput = await gv.StringInputBox("Enter a new variable name:", "");
                            editNode.actions[getIndexOfActionSelected()].a_parameter_4 = myinput;
                            if (!gv.mod.moduleVariablesList.Contains(myinput))
                            {
                                gv.mod.moduleVariablesList.Add(myinput);
                            }
                        }
                        else
                        {
                            editNode.actions[getIndexOfActionSelected()].a_parameter_4 = selected;
                        }
                    }
                    gv.touchEnabled = true;
                }
            }
            catch (Exception ex)
            {
                gv.touchEnabled = true;
            }
        }

        public async void changeCondScript()
        {            
            if (editNode == null) { return; }

            List<string> types = new List<string>(); //container, transition, conversation, encounter, script
            types.Add("none");
            foreach (ScriptObject s in gv.cc.scriptList)
            {
                if (s.name.StartsWith("gc"))
                {
                    types.Add(s.name);
                }
            }

            gv.touchEnabled = false;
            string selected = await gv.ListViewPage(types, "Select a script from the list");
            if ((editNode.conditions[getIndexOfCondSelected()].c_script.Equals(selected)) || (editNode.conditions[getIndexOfCondSelected()].c_script.Equals(selected + ".cs")))
            {
                //don't change it
            }
            else
            {
                editNode.conditions[getIndexOfCondSelected()].c_script = selected;
                editNode.conditions[getIndexOfCondSelected()].c_parameter_1 = null;
                editNode.conditions[getIndexOfCondSelected()].c_parameter_2 = null;
                editNode.conditions[getIndexOfCondSelected()].c_parameter_3 = null;
                editNode.conditions[getIndexOfCondSelected()].c_parameter_4 = null;
            }
            gv.touchEnabled = true;
        }
        public async void changeCondParm1()
        {
            try
            {
                if (editNode == null) { return; }
                ScriptObject script = gv.cc.GetScriptObjectByName(editNode.conditions[getIndexOfCondSelected()].c_script);
                if (script == null) { return; }
                if (script.name.Equals("none")) { return; }
                if (script.parmType1 == parmType.none) { return; }

                string title = script.description + Environment.NewLine + "Enter the first parameter for this script:" + Environment.NewLine + script.parmDescription1;
                //check if a string entry or list type
                if (script.parmType1 == parmType.strg)
                {
                    gv.touchEnabled = false;
                    string myinput = await gv.StringInputBox(title, editNode.conditions[getIndexOfCondSelected()].c_parameter_1);
                    editNode.conditions[getIndexOfCondSelected()].c_parameter_1 = myinput;
                    gv.touchEnabled = true;
                }
                else if ((script.parmType1 == parmType.large_int) || (script.parmType1 == parmType.mapX) || (script.parmType1 == parmType.mapY))
                {
                    gv.touchEnabled = false;
                    int myinput = await gv.NumInputBox(title, Convert.ToInt32(editNode.conditions[getIndexOfCondSelected()].c_parameter_1));
                    editNode.conditions[getIndexOfCondSelected()].c_parameter_1 = myinput.ToString();
                    gv.touchEnabled = true;
                }
                else //must be a list type
                {
                    List<string> items = gv.cc.GetListOfItems(script.parmType1);
                    if (items.Count == 0) { return; }
                    gv.touchEnabled = false;
                    string selected = await gv.ListViewPage(items, title);
                    if (selected != "none")
                    {
                        if (selected.Equals("create a new variable"))
                        {
                            string myinput = await gv.StringInputBox("Enter a new variable name:", "");
                            editNode.conditions[getIndexOfCondSelected()].c_parameter_1 = myinput;
                            if (!gv.mod.moduleVariablesList.Contains(myinput))
                            {
                                gv.mod.moduleVariablesList.Add(myinput);
                            }
                        }
                        else
                        {
                            editNode.conditions[getIndexOfCondSelected()].c_parameter_1 = selected;
                        }
                    }
                    gv.touchEnabled = true;
                }
            }
            catch (Exception ex)
            {
                gv.touchEnabled = true;
            }
        }
        public async void changeCondParm2()
        {
            try
            {
                if (editNode == null) { return; }
                ScriptObject script = gv.cc.GetScriptObjectByName(editNode.conditions[getIndexOfCondSelected()].c_script);
                if (script == null) { return; }
                if (script.name.Equals("none")) { return; }
                if (script.parmType2 == parmType.none) { return; }

                string title = script.description + Environment.NewLine + "Enter the first parameter for this script:" + Environment.NewLine + script.parmDescription2;
                //check if a string entry or list type
                if (script.parmType2 == parmType.strg)
                {
                    gv.touchEnabled = false;
                    string myinput = await gv.StringInputBox(title, editNode.conditions[getIndexOfCondSelected()].c_parameter_2);
                    editNode.conditions[getIndexOfCondSelected()].c_parameter_2 = myinput;
                    gv.touchEnabled = true;
                }
                else if ((script.parmType2 == parmType.large_int) || (script.parmType2 == parmType.mapX) || (script.parmType2 == parmType.mapY))
                {
                    gv.touchEnabled = false;
                    int myinput = await gv.NumInputBox(title, Convert.ToInt32(editNode.conditions[getIndexOfCondSelected()].c_parameter_2));
                    editNode.conditions[getIndexOfCondSelected()].c_parameter_2 = myinput.ToString();
                    gv.touchEnabled = true;
                }
                else //must be a list type
                {
                    List<string> items = gv.cc.GetListOfItems(script.parmType2);
                    if (items.Count == 0) { return; }
                    gv.touchEnabled = false;
                    string selected = await gv.ListViewPage(items, title);
                    if (selected != "none")
                    {
                        if (selected.Equals("create a new variable"))
                        {
                            string myinput = await gv.StringInputBox("Enter a new variable name:", "");
                            editNode.conditions[getIndexOfCondSelected()].c_parameter_2 = myinput;
                            if (!gv.mod.moduleVariablesList.Contains(myinput))
                            {
                                gv.mod.moduleVariablesList.Add(myinput);
                            }
                        }
                        else
                        {
                            editNode.conditions[getIndexOfCondSelected()].c_parameter_2 = selected;
                        }
                    }
                    gv.touchEnabled = true;
                }
            }
            catch (Exception ex)
            {
                gv.touchEnabled = true;
            }
        }
        public async void changeCondParm3()
        {
            try
            {
                if (editNode == null) { return; }
                ScriptObject script = gv.cc.GetScriptObjectByName(editNode.conditions[getIndexOfCondSelected()].c_script);
                if (script == null) { return; }
                if (script.name.Equals("none")) { return; }
                if (script.parmType3 == parmType.none) { return; }

                string title = script.description + Environment.NewLine + "Enter the first parameter for this script:" + Environment.NewLine + script.parmDescription3;
                //check if a string entry or list type
                if (script.parmType3 == parmType.strg)
                {
                    gv.touchEnabled = false;
                    string myinput = await gv.StringInputBox(title, editNode.conditions[getIndexOfCondSelected()].c_parameter_3);
                    editNode.conditions[getIndexOfCondSelected()].c_parameter_3 = myinput;
                    gv.touchEnabled = true;
                }
                else if ((script.parmType3 == parmType.large_int) || (script.parmType3 == parmType.mapX) || (script.parmType3 == parmType.mapY))
                {
                    gv.touchEnabled = false;
                    int myinput = await gv.NumInputBox(title, Convert.ToInt32(editNode.conditions[getIndexOfCondSelected()].c_parameter_3));
                    editNode.conditions[getIndexOfCondSelected()].c_parameter_3 = myinput.ToString();
                    gv.touchEnabled = true;
                }
                else //must be a list type
                {
                    List<string> items = gv.cc.GetListOfItems(script.parmType3);
                    if (items.Count == 0) { return; }
                    gv.touchEnabled = false;
                    string selected = await gv.ListViewPage(items, title);
                    if (selected != "none")
                    {
                        if (selected.Equals("create a new variable"))
                        {
                            string myinput = await gv.StringInputBox("Enter a new variable name:", "");
                            editNode.conditions[getIndexOfCondSelected()].c_parameter_3 = myinput;
                            if (!gv.mod.moduleVariablesList.Contains(myinput))
                            {
                                gv.mod.moduleVariablesList.Add(myinput);
                            }
                        }
                        else
                        {
                            editNode.conditions[getIndexOfCondSelected()].c_parameter_3 = selected;
                        }
                    }
                    gv.touchEnabled = true;
                }
            }
            catch (Exception ex)
            {
                gv.touchEnabled = true;
            }
        }
        public async void changeCondParm4()
        {
            try
            {
                if (editNode == null) { return; }
                ScriptObject script = gv.cc.GetScriptObjectByName(editNode.conditions[getIndexOfCondSelected()].c_script);
                if (script == null) { return; }
                if (script.name.Equals("none")) { return; }
                if (script.parmType4 == parmType.none) { return; }

                string title = script.description + Environment.NewLine + "Enter the first parameter for this script:" + Environment.NewLine + script.parmDescription4;
                //check if a string entry or list type
                if (script.parmType4 == parmType.strg)
                {
                    gv.touchEnabled = false;
                    string myinput = await gv.StringInputBox(title, editNode.conditions[getIndexOfCondSelected()].c_parameter_4);
                    editNode.conditions[getIndexOfCondSelected()].c_parameter_4 = myinput;
                    gv.touchEnabled = true;
                }
                else if ((script.parmType4 == parmType.large_int) || (script.parmType4 == parmType.mapX) || (script.parmType4 == parmType.mapY))
                {
                    gv.touchEnabled = false;
                    int myinput = await gv.NumInputBox(title, Convert.ToInt32(editNode.conditions[getIndexOfCondSelected()].c_parameter_4));
                    editNode.conditions[getIndexOfCondSelected()].c_parameter_4 = myinput.ToString();
                    gv.touchEnabled = true;
                }
                else //must be a list type
                {
                    List<string> items = gv.cc.GetListOfItems(script.parmType4);
                    if (items.Count == 0) { return; }
                    gv.touchEnabled = false;
                    string selected = await gv.ListViewPage(items, title);
                    if (selected != "none")
                    {
                        if (selected.Equals("create a new variable"))
                        {
                            string myinput = await gv.StringInputBox("Enter a new variable name:", "");
                            editNode.conditions[getIndexOfCondSelected()].c_parameter_4 = myinput;
                            if (!gv.mod.moduleVariablesList.Contains(myinput))
                            {
                                gv.mod.moduleVariablesList.Add(myinput);
                            }
                        }
                        else
                        {
                            editNode.conditions[getIndexOfCondSelected()].c_parameter_4 = selected;
                        }
                    }
                    gv.touchEnabled = true;
                }
            }
            catch (Exception ex)
            {
                gv.touchEnabled = true;
            }
        }

        public void PushToUndoStack()
        {
            Convo newConvo = new Convo();
            //newConvo = gv.mod.currentConvo.Clone();
            undoConvoStack.Push(newConvo);
        }

        public void AddNode()
        {
            try
            {
                if (editNode != null)
                {
                    if (!editNode.isLink)
                    {
                        //PushToUndoStack();
                        gv.mod.currentConvo.NextIdNum++;
                        ContentNode newNode = new ContentNode();
                        newNode.idNum = gv.mod.currentConvo.NextIdNum;
                        newNode.orderNum = editNode.subNodes.Count;
                        newNode.parentIdNum = editNode.idNum;
                        if (editNode == gv.mod.currentConvo.subNodes[0]) //root so make child NPC
                        {
                            newNode.pcNode = false;
                        }
                        else if (editNode.pcNode == true) //node is PC so make child NPC 
                        {
                            newNode.pcNode = false;
                        }
                        else //node is NPC so make child PC
                        {
                            newNode.pcNode = true;
                        }
                        editNode.subNodes.Add(newNode);
                        ResetTreeView();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("add node failed: " + ex.ToString());
            }
        }
        public void RemoveNode()
        {
            try
            {
                //check to ignore if selected node is root
                if (editNode == gv.mod.currentConvo.subNodes[0])
                {
                    return;
                }
                //PushToUndoStack();
                //do a check for linked nodes pointing to this node or subnodes
                //delete any linked nodes first before deleting this node
                //find corresponding contentNode (same IdNum) and delete it and subNodes
                removeAllLinksToNodeAndSubnodes(editNode);
                gv.mod.currentConvo.GetContentNodeById(editNode.parentIdNum).subNodes.Remove(editNode);
                ResetTreeView();
            }
            catch { gv.sf.MessageBox("remove node failed...make sure there are no remaining links referring to this node or sub nodes of this node. A link node pointing to a deleted node will cause problems."); }
        }
        public void CopyNodes()
        {
            try
            {
                //int cnod = Convert.ToInt32(treeView1.SelectedNode.Name);
                //int prnod = Convert.ToInt32(treeView1.SelectedNode.Parent.Name);
                //ContentNode chdnod = new ContentNode();
                //chdnod = f_convo.GetContentNodeById(cnod);
                if (!editNode.isLink)
                {
                    copyToClipboardNode = editNode.idNum;
                    copyToClipboardNodeParentNode = editNode.parentIdNum;
                    //prntForm.logText("You selected idNum = " + copyToClipboardNode.ToString() + " to copy to the clipboard");
                }
            }
            catch { gv.sf.MessageBox("copy node failed"); }
        }
        public void PasteNodes()
        {
            pasteFromClipboardNode = editNode.idNum;
            if (gv.mod.currentConvo.GetContentNodeById(copyToClipboardNode).pcNode == gv.mod.currentConvo.GetContentNodeById(pasteFromClipboardNode).pcNode)
            {
                gv.sf.MessageBox("You can't paste a PC node to a PC node or a NPC node to a NPC node");
            }
            else
            {
                try
                {
                    //int cnod = Convert.ToInt32(treeView1.SelectedNode.Name);
                    //ContentNode chdnod = new ContentNode();
                    //chdnod = f_convo.GetContentNodeById(cnod);
                    if (!editNode.isLink)
                    {
                        //PushToUndoStack();
                        //prntForm.logText("You selected idNum = " + pasteFromClipboardNode.ToString() + " to paste from the clipboard");
                        ContentNode copy = duplicateNode(gv.mod.currentConvo.GetContentNodeById(copyToClipboardNode));
                        //ContentNode copy = f_convo.GetContentNodeById(copyToClipboardNode).Clone();
                        gv.mod.currentConvo.GetContentNodeById(pasteFromClipboardNode).subNodes.Add(copy);
                        resetAllParentIds();
                        ResetTreeView();
                    }
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("paste node failed: " + ex.ToString());
                }
            }
        }
        public void PasteAsRelocatedNodes()
        {
            int relocateFromClipboardToThisNode = editNode.idNum;
            if (gv.mod.currentConvo.GetContentNodeById(copyToClipboardNode).pcNode == gv.mod.currentConvo.GetContentNodeById(relocateFromClipboardToThisNode).pcNode)
            {
                gv.sf.MessageBox("You can't paste a PC node to a PC node or a NPC node to a NPC node");
            }
            else
            {
                try
                {
                    //get the clipboardNode
                    ContentNode clipboardNode = gv.mod.currentConvo.GetContentNodeById(copyToClipboardNode);
                    ContentNode clipboardNodeParentNode = gv.mod.currentConvo.GetContentNodeById(copyToClipboardNodeParentNode);
                    ContentNode relocateToNode = gv.mod.currentConvo.GetContentNodeById(relocateFromClipboardToThisNode);
                    //add this node to the relocateToNode's subNodes list
                    //remove 
                    //int cnod = Convert.ToInt32(treeView1.SelectedNode.Name);
                    //ContentNode chdnod = new ContentNode();
                    //chdnod = f_convo.GetContentNodeById(cnod);
                    if (!editNode.isLink)
                    {
                        //PushToUndoStack();
                        //prntForm.logText("You selected idNum = " + relocateFromClipboardToThisNode.ToString() + " to relocate from the clipboard");
                        ContentNode copy = duplicateNodeKeepIds(clipboardNode);
                        //ContentNode copy = f_convo.GetContentNodeById(copyToClipboardNode).Clone();
                        relocateToNode.subNodes.Add(copy);
                        clipboardNodeParentNode.subNodes.Remove(clipboardNode);
                        //f_convo.GetContentNodeById(pasteFromClipboardNode).AddNodeToSubNode(copy);
                        //f_convo.GetContentNodeById(prnod).RemoveNodeFromSubNode(f_convo.GetContentNodeById(rnod));
                        resetAllParentIds();
                        ResetTreeView();
                    }
                }
                catch (Exception ex)
                {
                    gv.sf.MessageBox("paste node failed: " + ex.ToString());
                }
            }
        }
        public void PasteAsLink()
        {
            pasteFromClipboardNode = editNode.idNum;
            try
            {
                //int cnod = Convert.ToInt32(treeView1.SelectedNode.Name);
                //ContentNode chdnod = new ContentNode();
                //chdnod = f_convo.GetContentNodeById(cnod);
                if (!editNode.isLink)
                {
                    //PushToUndoStack();
                    //MessageBox.Show("You selected idNum = " + pasteFromClipboardNode.ToString() + " to paste as link from the clipboard");
                    ContentNode copy = createLinkNode(gv.mod.currentConvo.GetContentNodeById(copyToClipboardNode));
                    copy.linkTo = copyToClipboardNode;
                    ContentNode subcnt = gv.mod.currentConvo.GetContentNodeById(pasteFromClipboardNode);
                    copy.orderNum = subcnt.subNodes.Count;
                    gv.mod.currentConvo.GetContentNodeById(pasteFromClipboardNode).subNodes.Add(copy);
                    ResetTreeView();
                }
            }
            catch { gv.sf.MessageBox("paste as link node failed"); }
        }
        public void FollowLink()
        {
            try
            {
                editNode = gv.mod.currentConvo.GetContentNodeById(editNode.linkTo);
                expandAllNodes();
                ResetTreeView();
                //int cnod = Convert.ToInt32(treeView1.SelectedNode.Name);
                //ContentNode chdnod = new ContentNode();
                //chdnod = f_convo.GetContentNodeById(cnod);
                //TreeNode[] tn = treeView1.Nodes.Find(chdnod.linkTo.ToString(), true);
                //TreeNode[] tn = treeView1.Nodes.Find(f_convo.NextIdNum.ToString(), true);
                //if (tn[0] != null)
                //{
                    //treeView1.SelectedNode = tn[0];
                    //currentSelectedNode = tn[0];
                //}
            }
            catch { gv.sf.MessageBox("follow link node failed"); }
        }
        public void MoveUp()
        {
            if ((editNode != null) && (editNode != gv.mod.currentConvo.subNodes[0]))
            {
                //PushToUndoStack();
                //int pnod = Convert.ToInt32(treeView1.SelectedNode.Parent.Name);
                //int cnod = Convert.ToInt32(treeView1.SelectedNode.Name);
                //ContentNode parnod = new ContentNode();
                //ContentNode chdnod = new ContentNode();
                //parnod = f_convo.GetContentNodeById(pnod);
                //chdnod = f_convo.GetContentNodeById(cnod);
                ContentNode parnod = gv.mod.currentConvo.GetContentNodeById(editNode.parentIdNum);
                //ContentNode chdnod = gv.mod.currentConvo.GetContentNodeById(Convert.ToInt32(treeView1.SelectedNode.Name));
                if (editNode.orderNum > 0)
                {
                    int tempNodeIndx = editNode.orderNum;
                    parnod.subNodes[tempNodeIndx - 1].orderNum++;
                    editNode.orderNum--;
                    SortConversation(gv.mod.currentConvo);
                    ResetTreeView();
                }
                //TreeNode[] tn = treeView1.Nodes.Find(currentSelectedNode.Name, true);
                //if (tn[0] != null)
                //{
                //    treeView1.SelectedNode = tn[0];
                //    currentSelectedNode = tn[0];
                //}
            }
        }
        public void MoveDown()
        {
            if ((editNode != null) && (editNode != gv.mod.currentConvo.subNodes[0]))
            {
                //PushToUndoStack();
                //int pnod = Convert.ToInt32(treeView1.SelectedNode.Parent.Name);
                //int cnod = Convert.ToInt32(treeView1.SelectedNode.Name);
                //ContentNode parnod = new ContentNode();
                //ContentNode chdnod = new ContentNode();
                //parnod = f_convo.GetContentNodeById(pnod);
                //chdnod = f_convo.GetContentNodeById(cnod);
                ContentNode parnod = gv.mod.currentConvo.GetContentNodeById(editNode.parentIdNum);
                if (editNode.orderNum < parnod.subNodes.Count - 1)
                {
                    int tempNodeIndx = editNode.orderNum;
                    parnod.subNodes[tempNodeIndx + 1].orderNum--;
                    editNode.orderNum++;
                    SortConversation(gv.mod.currentConvo);
                    ResetTreeView();
                }
                //TreeNode[] tn = treeView1.Nodes.Find(currentSelectedNode.Name, true);
                //if (tn[0] != null)
                //{
                //    treeView1.SelectedNode = tn[0];
                //    currentSelectedNode = tn[0];
                //}
            }
        }

        public void AddCond()
        {
            if (editNode.conditions.Count < 4)
            {
                Condition newCondition = new Condition();
                newCondition.c_not = false;
                newCondition.c_script = "none";
                newCondition.c_parameter_1 = null;
                newCondition.c_parameter_2 = null;
                newCondition.c_parameter_3 = null;
                newCondition.c_parameter_4 = null;
                editNode.conditions.Add(newCondition);
                ResetTreeView();
            }
        }
        public void RemoveCond()
        {
            editNode.conditions.RemoveAt(getIndexOfCondSelected());
            ResetTreeView();
        }
        public void MoveUpCond()
        {
            int oldIndex = getIndexOfCondSelected();
            int newIndex = oldIndex - 1;

            if (newIndex >= 0)
            {
                var item = editNode.conditions[oldIndex];
                editNode.conditions.RemoveAt(oldIndex);
                editNode.conditions.Insert(newIndex, item);
                ResetTreeView();
            }
        }
        public void MoveDownCond()
        {
            int oldIndex = getIndexOfCondSelected();
            int newIndex = oldIndex + 1;

            if ((newIndex < editNode.conditions.Count) && (oldIndex >= 0))
            {
                var item = editNode.conditions[oldIndex];
                editNode.conditions.RemoveAt(oldIndex);
                editNode.conditions.Insert(newIndex, item);
                ResetTreeView();
            }
        }
        public void CopyCond()
        {
            copiedConditional = editNode.conditions[getIndexOfCondSelected()];
        }
        public void PasteCond()
        {
            editNode.conditions.Add(copiedConditional.DeepCopy());
            ResetTreeView();
        }

        public void AddAction()
        {
            if (editNode.actions.Count < 4)
            {
                Action newAction = new Action();
                newAction.a_script = "none";
                newAction.a_parameter_1 = null;
                newAction.a_parameter_2 = null;
                newAction.a_parameter_3 = null;
                newAction.a_parameter_4 = null;
                editNode.actions.Add(newAction);
                ResetTreeView();
            }
        }
        public void RemoveAction()
        {
            editNode.actions.RemoveAt(getIndexOfActionSelected());
            ResetTreeView();
        }
        public void MoveUpAction()
        {
            int oldIndex = getIndexOfActionSelected();
            int newIndex = oldIndex - 1;

            if (newIndex >= 0)
            {
                var item = editNode.actions[oldIndex];
                editNode.actions.RemoveAt(oldIndex);
                editNode.actions.Insert(newIndex, item);
                ResetTreeView();
            }
        }
        public void MoveDownAction()
        {
            int oldIndex = getIndexOfActionSelected();
            int newIndex = oldIndex + 1;

            if ((newIndex < editNode.actions.Count) && (oldIndex >= 0))
            {
                var item = editNode.actions[oldIndex];
                editNode.actions.RemoveAt(oldIndex);
                editNode.actions.Insert(newIndex, item);
                ResetTreeView();
            }
        }
        public void CopyAction()
        {
            copiedAction = editNode.actions[getIndexOfActionSelected()];
        }
        public void PasteAction()
        {
            editNode.actions.Add(copiedAction.DeepCopy());
            ResetTreeView();
        }

        public ContentNode createLinkNode(ContentNode copiedNode)
        {
            gv.mod.currentConvo.NextIdNum++;
            ContentNode copy = new ContentNode();
            copy.conversationText = copiedNode.conversationText;
            copy.idNum = gv.mod.currentConvo.NextIdNum;
            return copy;
        }
        public static void SortConversation(Convo toSort)
        {
            int i;
            if (toSort.subNodes.Count > 0)
            {
                for (i = 0; i < toSort.subNodes.Count; i++)
                {
                    SortSubNodes(toSort.subNodes[i]);
                }
            }
        }
        public static void SortSubNodes(ContentNode myNode)
        {
            foreach (ContentNode subNode in myNode.subNodes)
            {
                SortSubNodes(subNode);
            }
            myNode.subNodes = SortList(myNode.subNodes);
        }
        public static List<ContentNode> SortList(List<ContentNode> thisList)
        {
            List<ContentNode> returnList = new List<ContentNode>();
            returnList = thisList.OrderBy(o => o.orderNum).ToList();
            return returnList;
        }
        public ContentNode duplicateNode(ContentNode copiedNode)
        {
            gv.mod.currentConvo.NextIdNum++;
            ContentNode copy = new ContentNode();
            copy = copiedNode.DuplicateContentNode(gv.mod.currentConvo.NextIdNum);
            copy.idNum = gv.mod.currentConvo.NextIdNum;
            foreach (ContentNode node in copiedNode.subNodes)
            {
                copy.subNodes.Add(duplicateNode(node));
            }
            return copy;
        }
        public ContentNode duplicateNodeKeepIds(ContentNode copiedNode)
        {
            ContentNode copy = new ContentNode();
            copy = copiedNode.DuplicateContentNode();
            foreach (ContentNode node in copiedNode.subNodes)
            {
                copy.subNodes.Add(duplicateNodeKeepIds(node));
            }
            return copy;
        }
        public List<int> foundLinkedNodesIdList = new List<int>();
        public void removeAllLinksToNodeAndSubnodes(ContentNode node)
        {
            //clear find list
            foundLinkedNodesIdList.Clear();
            //find all nodes that link to this node
            findAllLinkedNodesToGivenNodeId(gv.mod.currentConvo.subNodes[0], node.idNum);
            //delete all nodes in found list
            foreach (int id in foundLinkedNodesIdList)
            {
                ContentNode n = getParentNodeById(gv.mod.currentConvo.subNodes[0], id);
                if (n != null)
                {
                    foreach (ContentNode sn in n.subNodes)
                    {
                        if (sn.idNum == id)
                        {
                            n.subNodes.Remove(sn);
                            break;
                        }
                    }
                }
            }
            foreach (ContentNode subNode in node.subNodes)
            {
                removeAllLinksToNodeAndSubnodes(subNode);
            }
        }
        public void findAllLinkedNodesToGivenNodeId(ContentNode node, int idPointedTo)
        {
            //go through entire convo and find all nodes that linkTo the given node 'idPointedTo'
            if (node.linkTo == idPointedTo)
            {
                foundLinkedNodesIdList.Add(node.idNum);
            }
            foreach (ContentNode subNode in node.subNodes)
            {
                findAllLinkedNodesToGivenNodeId(subNode, idPointedTo);
            }
        }
        public ContentNode getParentNodeById(ContentNode node, int idOfChild)
        {
            foreach (ContentNode subn in node.subNodes)
            {
                if (subn.idNum == idOfChild)
                {
                    return node;
                }
            }
            foreach (ContentNode sn in node.subNodes)
            {
                ContentNode n = getParentNodeById(sn, idOfChild);
                if (n != null)
                {
                    return n;
                }
            }
            return null;
        }

        public bool tapInMapViewport(int x, int y)
        {
            if (x < mapStartLocXinPixels) { return false; }
            if (y < 0) { return false; }
            if (x > mapStartLocXinPixels + gv.squareSize * gv.scaler * 10) { return false; }
            if (y > gv.squareSize * gv.scaler * 10) { return false; }
            return true;
        }

        public void ResetParentIdNum(ContentNode node)
        {
            for (int i = 0; i < node.subNodes.Count; i++)
            {
                node.subNodes[i].parentIdNum = node.idNum;
            }
            foreach (ContentNode n in node.subNodes)
            {
                ResetParentIdNum(n);
            }
        }
        public void ResetNodeList(ContentNode node)
        {
            ContentNode pnod = gv.mod.currentConvo.GetContentNodeById(node.parentIdNum);
            if (pnod != null)
            {
                node.indentMultiplier = pnod.indentMultiplier + 1;
            }
            nodeList.Add(node);
            if (!node.IsExpanded) { return; }
            foreach (ContentNode n in node.subNodes)
            {
                ResetNodeList(n);
            }
        }
        public void ExpandAllNodes(ContentNode node)
        {
            if (node != gv.mod.currentConvo.subNodes[0])
            {
                node.IsExpanded = true;
            }
            foreach (ContentNode n in node.subNodes)
            {
                ExpandAllNodes(n);
            }
        }
        public void CollapseAllNodes(ContentNode node)
        {
            if (node != gv.mod.currentConvo.subNodes[0])
            {
                node.IsExpanded = false;
            }
            foreach (ContentNode n in node.subNodes)
            {
                CollapseAllNodes(n);
            }
        }
    }
}
