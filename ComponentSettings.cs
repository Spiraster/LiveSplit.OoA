using System;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LiveSplit.OoA;

namespace LiveSplit.OoA
{
    public partial class OoASettings : UserControl
    {
        public InfoList CheckedSplits;
        public bool AutoStartTimer, AutoSelectFile, AutoReset;

        public OoASettings()
        {
            InitializeComponent();
            this.treeView1.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.treeView1.DrawNode += new DrawTreeNodeEventHandler(treeView1.tree_DrawNode);

            AutoStartTimer = false;
            AutoSelectFile = false;
            AutoReset = false;

            CheckedSplits = new InfoList();
            foreach (var _split in DefaultInfo.BaseSplits)
            {
                CheckedSplits.Add(new Info(_split.Name, false));
            }
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var settingsNode = document.CreateElement("Settings");

            settingsNode.AppendChild(ToElement(document, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

            settingsNode.AppendChild(ToElement(document, "AutoStartTimer", AutoStartTimer.ToString()));
            settingsNode.AppendChild(ToElement(document, "AutoSelectFile", AutoSelectFile.ToString()));
            settingsNode.AppendChild(ToElement(document, "AutoReset", AutoReset.ToString()));

            foreach (var _split in CheckedSplits)
            {
                settingsNode.AppendChild(ToElement(document, _split.Name, _split.isEnabled.ToString()));
            }

            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            var element = (XmlElement)settings;
            if (!element.IsEmpty)
            {
                Version version;
                if (element["Version"] != null)
                    version = Version.Parse(element["Version"].InnerText);
                else
                    version = new Version(1, 0, 0);

                if (element["AutoStartTimer"] != null)
                {
                    AutoStartTimer = Convert.ToBoolean(element["AutoStartTimer"].InnerText);
                    chkStartTimer.Checked = AutoStartTimer;
                }
                if (element["AutoSelectFile"] != null)
                {
                    AutoSelectFile = Convert.ToBoolean(element["AutoSelectFile"].InnerText);
                    chkSelectFile.Checked = AutoSelectFile;
                }
                if (element["AutoReset"] != null)
                {
                    AutoReset = Convert.ToBoolean(element["AutoReset"].InnerText);
                    chkAutoReset.Checked = AutoReset;
                }

                foreach (var _split in CheckedSplits)
                {
                    if (element[_split.Name] != null)
                    {
                        bool _bool = Convert.ToBoolean(element[_split.Name].InnerText);

                        _split.isEnabled = _bool;

                        var node = getTreeNode(_split.Name);
                        if (node != null)
                            node.Checked = _bool;
                    }
                }
            }
        }

        private TreeNode getTreeNode(string search)
        {
            foreach (TreeNode parent in treeView1.Nodes)
            {
                foreach (TreeNode node in parent.Nodes)
                {
                    if (node.Name == "node_" + search)
                        return node;
                }
            }

            return null;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            string name = e.Node.Name.Replace("node_", "");
            CheckedSplits[name].isEnabled = e.Node.Checked;
        }

        private void checkAutoReset_CheckedChanged(object sender, EventArgs e)
        {
            AutoReset = chkAutoReset.Checked;
        }

        private void checkStartTimer_CheckedChanged(object sender, EventArgs e)
        {
            AutoStartTimer = chkStartTimer.Checked;
        }

        private void checkSelectFile_CheckedChanged(object sender, EventArgs e)
        {
            AutoSelectFile = chkSelectFile.Checked;
        }

        private XmlElement ToElement<T>(XmlDocument document, String name, T value)
        {
            var element = document.CreateElement(name);
            element.InnerText = value.ToString();
            return element;
        }
    }

    public class NewTreeView : TreeView
    {
        // constants used to hide a checkbox
        public const int TVIF_STATE = 0x8;
        public const int TVIS_STATEIMAGEMASK = 0xF000;
        public const int TV_FIRST = 0x1100;
        public const int TVM_SETITEM = TV_FIRST + 63;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

        // struct used to set node properties
        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        public struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        public void tree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Level == 0)
                HideCheckBox(e.Node);

            e.DrawDefault = true;
        }

        private void HideCheckBox(TreeNode node)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            SendMessage(node.TreeView.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x203) // identified double click
            {
                var local_pos = PointToClient(Cursor.Position);
                var hit_test_info = HitTest(local_pos);

                if (hit_test_info.Location == TreeViewHitTestLocations.StateImage)
                {
                    m.Msg = 0x201; // if checkbox was clicked, turn into single click
                }

                base.WndProc(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
