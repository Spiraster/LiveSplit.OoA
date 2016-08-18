using LiveSplit.ComponentUtil;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace LiveSplit.OoA
{
    class OoAMemory
    {
        public Emulator emulator { get; set; }
        private GameVersion version { get; set; }

        private OoAData data;
        private InfoList splits;

        public OoAMemory()
        {

        }

        public void setPointers()
        {
            data = new OoAData(emulator);
        }

        public void setSplits(OoASettings settings)
        {
            splits = new InfoList();
            splits.AddRange(DefaultInfo.BaseSplits);

            foreach (var _setting in settings.CheckedSplits)
            {
                if (!_setting.isEnabled)
                    splits.Remove(splits[_setting.Name]);
            }
        }

        public void getVersion(Process game)
        {
            data["VersionCheck"].Update(game);

            byte _byte = Convert.ToByte(data["VersionCheck"].Current);
            if (_byte == 0x32)
                version = GameVersion.OoS;
            else if (_byte == 0x33)
                version = GameVersion.OoA;
        }

        public bool doStart(Process game)
        {
            data["FileSelect1"].Update(game);
            data["FileSelect2"].Update(game);

            byte _byte = Convert.ToByte(data["FileSelect1"].Current);
            short _short = Convert.ToInt16(data["FileSelect2"].Current);
            if (_byte == 0x23 && _short == 0x0301)
                return true;

            return false;
        }

        public bool doReset(Process game)
        {
            data["ResetCheck"].Update(game);

            byte _byte = Convert.ToByte(data["ResetCheck"].Current);
            if (_byte == 0xFF)
                return true;

            return false;
        }

        public bool doSplit(string segment, Process game, OoASettings settings)
        {
            data.UpdateAll(game);

            foreach (var _split in splits)
            {
                int count = 0;
                foreach (var _trigger in _split.Triggers)
                {
                    int _int = Convert.ToInt32(data[_trigger.Key].Current);
                    if (_int == _trigger.Value)
                        count++;
                }

                if (count == _split.Triggers.Count)
                {
                    splits.Remove(_split);
                    return true;
                }
            }

            return false;
        }

        private enum GameVersion
        {
            unknown,
            OoA,
            OoS
        }
    }

    class OoAData : MemoryWatcherList
    {
        private int ptrBase;
        private List<int>[] ptrOffsets;

        public OoAData(Emulator emulator)
        {
            if (emulator == Emulator.bgb151)
            {
                ptrBase = 0x000FC5CC;
                ptrOffsets = new List<int>[] { new List<int> { 0xF5C, 0x1A8 }, new List<int> { 0xF5C, 0x264 } };
            }
            else if (emulator == Emulator.bgb152)
            {
                ptrBase = 0x000FE5CC;
                ptrOffsets = new List<int>[] { new List<int> { 0xC4C, 0x1B0 }, new List<int> { 0xC4C, 0x26C } };
            }
            else if (emulator == Emulator.gambatte571)
            {
                ptrBase = 0x00552038;
                ptrOffsets = new List<int>[] { new List<int> { 0x1C, 0x10, 0x10, 0x110, 0x6C }, new List<int> { 0x1C, 0x10, 0x10, 0x110, 0x8C } };
            }

            foreach (var _ptr in DefaultInfo.Pointers)
            {
                if (_ptr.Type == "byte")
                    this.Add(new MemoryWatcher<byte>(new DeepPointer(ptrBase, getOffsets(_ptr.Index, _ptr.Offset))) { Name = _ptr.Name });
                else if (_ptr.Type == "short")
                    this.Add(new MemoryWatcher<short>(new DeepPointer(ptrBase, getOffsets(_ptr.Index, _ptr.Offset))) { Name = _ptr.Name });
                else if (_ptr.Type == "int")
                    this.Add(new MemoryWatcher<int>(new DeepPointer(ptrBase, getOffsets(_ptr.Index, _ptr.Offset))) { Name = _ptr.Name });
            }
        }

        private int[] getOffsets(int index, int offset)
        {
            var list = new List<int>();
            list.AddRange(ptrOffsets[index]);
            list.Add(offset);

            return list.ToArray();
        }
    }

    public enum Emulator
    {
        unknown,
        bgb151,
        bgb152,
        gambatte571
    }
}
