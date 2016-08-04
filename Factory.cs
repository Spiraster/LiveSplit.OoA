using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;

[assembly: ComponentFactory(typeof(LiveSplit.OoA.OoAFactory))]

namespace LiveSplit.OoA
{
    public class OoAFactory : IComponentFactory
    {
        public string ComponentName => "OoA Auto Splitter";
        public string Description => "Autosplitter for Oracle of Ages with BGB and Gambatte";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public ComponentCategory Category => ComponentCategory.Control;

        public string UpdateName => ComponentName;
        public string XMLURL => "https://raw.githubusercontent.com/Spiraster/LiveSplit.OoA/master/";
        public string UpdateURL => "Components/update.LiveSplit.OoA.xml";

        public IComponent Create(LiveSplitState state)
        {
            return new OoAComponent(state);
        }
    }
}
