using System;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;

namespace LiveSplit.OoA
{
    public partial class OoASettings : UserControl
    {
        public string[][] SplitInfo;

        public bool AutoStartTimer, AutoSelectFile, AutoReset;

        public OoASettings()
        {
            InitializeComponent();

            AutoStartTimer = false;
            AutoSelectFile = false;
            AutoReset = false;

            var list = new List<string[]>();

            list.Add(new string[] { "L1Sword", "sword" });
            list.Add(new string[] { "D1", "eternal spirit" });
            list.Add(new string[] { "D2", "ancient wood" });
            list.Add(new string[] { "D3", "echoing howl" });
            list.Add(new string[] { "D4", "burning flame" });
            list.Add(new string[] { "D5", "sacred soil" });
            list.Add(new string[] { "D6", "bereft peak" });
            list.Add(new string[] { "D7", "rolling sea" });
            list.Add(new string[] { "D8", "falling star" });
            list.Add(new string[] { "Veran", "defeat veran" });

            list.Add(new string[] { "ED1", "enter d1" });
            list.Add(new string[] { "ED2", "enter d2" });
            list.Add(new string[] { "ED3", "enter d3" });
            list.Add(new string[] { "ED4", "enter d4" });
            list.Add(new string[] { "ED5", "enter d5" });
            list.Add(new string[] { "ED6", "enter d6" });
            list.Add(new string[] { "ED7", "enter d7" });
            list.Add(new string[] { "ED8", "enter d8" });
            list.Add(new string[] { "EBT", "enter black tower" });

            list.Add(new string[] { "Satchel", "seed satchel" });
            list.Add(new string[] { "Harp1", "harp of ages" });
            list.Add(new string[] { "Feather", "feather" });
            list.Add(new string[] { "D2Skip", "d2 skip" });
            list.Add(new string[] { "Rope", "rope" });
            list.Add(new string[] { "Chart", "chart" });
            list.Add(new string[] { "CI", "crescent island" });
            list.Add(new string[] { "Shooter", "seed shooter" });
            list.Add(new string[] { "Flute", "dimitri's flute" });
            list.Add(new string[] { "Harp2", "currents tune" });
            list.Add(new string[] { "Moblin", "great moblin" });
            list.Add(new string[] { "Cane", "cane of somaria" });
            list.Add(new string[] { "Tuni", "tuni nut" });
            list.Add(new string[] { "SwitchHook", "switch hook" });
            list.Add(new string[] { "LavaJuice", "lava juice" });
            list.Add(new string[] { "MermaidSuit", "mermaid suit" });
            list.Add(new string[] { "D6BK", "d6 boss key" });
            list.Add(new string[] { "Nayru", "save nayru" });

            SplitInfo = list.ToArray();

            LoadSplits(null);
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var settingsNode = document.CreateElement("Settings");

            settingsNode.AppendChild(ToElement(document, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

            settingsNode.AppendChild(ToElement(document, "AutoStartTimer", AutoStartTimer.ToString()));
            settingsNode.AppendChild(ToElement(document, "AutoSelectFile", AutoSelectFile.ToString()));
            settingsNode.AppendChild(ToElement(document, "AutoReset", AutoReset.ToString()));

            foreach (string[] split in SplitInfo)
            {
                settingsNode.AppendChild(ToElement(document, split[0], split[1]));
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

                LoadSplits(element);
                SaveSplits();
            }
        }

        private TextBox GetTextBox(string search)
        {
            foreach (Control page in tabControl.Controls)
            {
                foreach (Control table in page.Controls)
                {
                    foreach (Control c in table.Controls)
                    {
                        if (c.Name == "txt_" + search)
                        {
                            return ((TextBox)c);
                        }
                    }
                }
            }
            return null;
        }

        private void LoadSplits(XmlElement element)
        {
            foreach (string[] split in SplitInfo)
            {
                var textBox = GetTextBox(split[0]);

                if (textBox != null)
                {
                    if (element != null && element[split[0]] != null && element[split[0]].InnerText != "")
                        textBox.Text = element[split[0]].InnerText;
                    else
                        textBox.Text = split[1];
                }
            }
        }

        private void SaveSplits()
        {
            foreach (string[] split in SplitInfo)
            {
                var textBox = GetTextBox(split[0]);

                if (textBox != null)
                {
                    split[1] = textBox.Text.ToLower();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveSplits();
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
}
