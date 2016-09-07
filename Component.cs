using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LiveSplit.OoA
{
    class OoAComponent : LogicComponent
    {
        [DllImport("user32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        public override string ComponentName => "OoA Auto Splitter";

        public OoASettings settings { get; set; }

        private Process game { get; set; }
        private TimerModel model { get; set; }
        private OoAMemory memory { get; set; }

        private Timer processTimer;

        private bool AllowFS = true;

        public OoAComponent(LiveSplitState state)
        {
            settings = new OoASettings();
            
            model = new TimerModel() { CurrentState = state };
            model.CurrentState.OnStart += timer_OnStart;

            processTimer = new Timer() { Interval = 2000, Enabled = true };
            processTimer.Tick += processTimer_OnTick;

            memory = new OoAMemory();
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (game != null && !game.HasExited)
            {
                if (state.CurrentPhase == TimerPhase.NotRunning)
                {
                    if (settings.AutoStartTimer)
                    {
                        if (memory.doStart(game))
                        {
                            AllowFS = false;
                            model.Start();
                        }
                    }
                }
                else
                {
                    if (settings.AutoReset)
                    {
                        if (memory.doReset(game))
                            model.Reset();
                    }
                }

                if (state.CurrentPhase == TimerPhase.Running)
                {
                    if (memory.doSplit(game))
                        model.Split();
                }
            }
            else if (!processTimer.Enabled)
            {
                processTimer.Enabled = true;
            }
        }

        Process getGameProcess()
        {
            Process process = null;

            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (p.MainModule.ModuleMemorySize == 5656576 || p.MainModule.ModuleMemorySize == 1691648 || p.MainModule.ModuleMemorySize == 1699840)
                    {
                        process = p;
                        break;
                    }
                }
                catch { }
            }
            
            if (process != null)
            {
                if (process.MainModule.ModuleMemorySize == 1691648)
                    memory.emulator = Emulator.bgb151;
                else if (process.MainModule.ModuleMemorySize == 1699840)
                    memory.emulator = Emulator.bgb152;
                else if (process.MainModule.ModuleMemorySize == 5656576)
                    memory.emulator = Emulator.gambatte571;
                
                memory.setPointers();

                return process;
            }

            return null;
        }

        void timer_OnStart(object sender, EventArgs e)
        {
            if (game != null && !game.HasExited)
            {
                //memory.getVersion(game);
                memory.setSplits(settings);
            }

            if (AllowFS)
            {
                if (settings.AutoSelectFile) //auto file select
                {
                    SetForegroundWindow(game.MainWindowHandle);
                    SendKeys.SendWait("{] 20}");
                }
            }
            else
            {
                AllowFS = true;
            }
        }

        void processTimer_OnTick(object sender, EventArgs e)
        {
            if (game == null || game.HasExited)
            {
                game = getGameProcess();
            }
            else
            {
                processTimer.Enabled = false;
            }
        }

        public override void Dispose()
        {
            model.CurrentState.OnStart -= timer_OnStart;
            processTimer.Tick -= processTimer_OnTick;
        }

        public override System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return settings.GetSettings(document);
        }

        public override System.Windows.Forms.Control GetSettingsControl(UI.LayoutMode mode)
        {
            return settings;
        }

        public override void SetSettings(System.Xml.XmlNode settings)
        {
            this.settings.SetSettings(settings);
        }
    }
}
