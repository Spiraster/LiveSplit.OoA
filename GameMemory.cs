using System.Diagnostics;

namespace LiveSplit.OoA
{
    class OoAMemory
    {
        public Emulator emulator { get; set; }
        public GameVersion version { get; set; }

        public int ptrBase;
        public int[] ptrOffsetsA;
        public int[] ptrOffsetsB;

        public DeepPointer Sword, D1Ess, D2Ess, D3Ess, D6Ess, BossHP;
        public DeepPointer D1Enter, D2Enter, D3Enter, D4Enter, D5Enter, D6Enter;
        public DeepPointer Satchel, Harp1, Feather, D2Skip, Raft, CrescentIsland, Shooter, Flute, Harp2, Cane, TuniNut, Moblin, SwitchHook, LavaJuice, MermaidSuit, D6BossKey, NayruHP;
        public DeepPointer VersionCheck, FileSelect1, FileSelect2, ResetCheck;

        public OoAMemory()
        {

        }

        public void setOffsets()
        {
            if (emulator == Emulator.bgb151)
            {
                ptrBase = 0x000FC5CC;
                ptrOffsetsA = new int[] { 0xF5C, 0x1A8 };
                ptrOffsetsB = new int[] { 0xF5C, 0x264 };
            }
            else if (emulator == Emulator.bgb152)
            {
                ptrBase = 0x000FE5CC;
                ptrOffsetsA = new int[] { 0xC4C, 0x1B0 };
                ptrOffsetsB = new int[] { 0xC4C, 0x26C };
            }
            else if (emulator == Emulator.gambatte571)
            {
                ptrBase = 0x00552038;
                ptrOffsetsA = new int[] { 0x1C, 0x10, 0x10, 0x110, 0x6C };
                ptrOffsetsB = new int[] { 0x1C, 0x10, 0x10, 0x110, 0x8C };
            }

            VersionCheck = new DeepPointer(ptrBase, ptrOffsetsA, 0x2FD);
            FileSelect1 = new DeepPointer(ptrBase, ptrOffsetsA, 0xB00);
            FileSelect2 = new DeepPointer(ptrBase, ptrOffsetsA, 0xBB3);
            ResetCheck = new DeepPointer(ptrBase, ptrOffsetsA, 0);

            setPointers();
        }

        public void setPointers()
        {
            //set A
            Sword = new DeepPointer(ptrBase, ptrOffsetsA, 0x6B2);
            D1Ess = new DeepPointer(ptrBase, ptrOffsetsA, 0x911);
            D2Ess = new DeepPointer(ptrBase, ptrOffsetsA, 0x938);
            D3Ess = new DeepPointer(ptrBase, ptrOffsetsA, 0x949);
            D6Ess = new DeepPointer(ptrBase, ptrOffsetsA, 0xA37);
            D1Enter = new DeepPointer(ptrBase, ptrOffsetsA, 0x924);
            D2Enter = new DeepPointer(ptrBase, ptrOffsetsA, 0x946);
            D3Enter = new DeepPointer(ptrBase, ptrOffsetsA, 0x966);
            D4Enter = new DeepPointer(ptrBase, ptrOffsetsA, 0x991);
            D5Enter = new DeepPointer(ptrBase, ptrOffsetsA, 0x9BB);
            D6Enter = new DeepPointer(ptrBase, ptrOffsetsA, 0xA44);

            Satchel = new DeepPointer(ptrBase, ptrOffsetsA, 0x738);
            Harp1 = new DeepPointer(ptrBase, ptrOffsetsA, 0x8AE);
            Feather = new DeepPointer(ptrBase, ptrOffsetsA, 0x928);
            D2Skip = new DeepPointer(ptrBase, ptrOffsetsA, 0x82E);
            Raft = new DeepPointer(ptrBase, ptrOffsetsA, 0x6A4); //04 for rope, 10 for chart
            CrescentIsland = new DeepPointer(ptrBase, ptrOffsetsA, 0x8AA);
            Shooter = new DeepPointer(ptrBase, ptrOffsetsA, 0x958);
            Flute = new DeepPointer(ptrBase, ptrOffsetsA, 0x6B5); //1 for ricky, 2 for dimitri, 3 for moosh
            Harp2 = new DeepPointer(ptrBase, ptrOffsetsA, 0x88F);
            Cane = new DeepPointer(ptrBase, ptrOffsetsA, 0x9A5);
            TuniNut = new DeepPointer(ptrBase, ptrOffsetsA, 0x6C2); //1 during cart game, 2 when fixed
            Moblin = new DeepPointer(ptrBase, ptrOffsetsA, 0x709);
            SwitchHook = new DeepPointer(ptrBase, ptrOffsetsA, 0x987);
            LavaJuice = new DeepPointer(ptrBase, ptrOffsetsA, 0x8E7);
            MermaidSuit = new DeepPointer(ptrBase, ptrOffsetsA, 0xA13);
            D6BossKey = new DeepPointer(ptrBase, ptrOffsetsA, 0xA1C);

            //set B
            BossHP = new DeepPointer(ptrBase, ptrOffsetsB, 0x0A9);
            NayruHP = new DeepPointer(ptrBase, ptrOffsetsB, 0x0B3);
        }

        public void getVersion(Process game)
        {
            byte _byte;

            VersionCheck.Deref<byte>(game, out _byte);
            if (_byte == 0x32)
                version = GameVersion.OoS;
            else if (_byte == 0x33)
                version = GameVersion.OoA;

            setPointers();
        }

        public bool doStart(Process game)
        {
            byte _byte;
            short _short;

            FileSelect1.Deref<byte>(game, out _byte);
            if (_byte == 0x23)
            {
                FileSelect2.Deref<short>(game, out _short);
                if (_short == 0x0301)
                    return true;
            }

            return false;
        }

        public bool doReset(Process game)
        {
            byte _byte;

            ResetCheck.Deref<byte>(game, out _byte);
            if (_byte == 0xFF)
                return true;

            return false;
        }

        public bool doSplit(string segment, Process game, OoASettings settings)
        {
            byte _byte;
            
            //dungeon endings
            if (segment == settings.SplitInfo[(int)Splits.L1Sword][1])
            {
                Sword.Deref<byte>(game, out _byte);
                if (_byte == 1)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.D1Ess][1])
            {
                D1Ess.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.D2Ess][1])
            {
                D2Ess.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.D3Ess][1])
            {
                D3Ess.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.D6Ess][1])
            {
                D6Ess.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Veran][1])
            {
                BossHP.Deref<byte>(game, out _byte);
                if (_byte == 0x01)
                    return true;
            }

            //dungeon entrances
            else if (segment == settings.SplitInfo[(int)Splits.EnterD1][1])
            {
                D1Enter.Deref<byte>(game, out _byte);
                if (_byte == 0x10)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.EnterD2][1])
            {
                D2Enter.Deref<byte>(game, out _byte);
                if (_byte == 0x10)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.EnterD3][1])
            {
                D3Enter.Deref<byte>(game, out _byte);
                if (_byte == 0x10)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.EnterD4][1])
            {
                D4Enter.Deref<byte>(game, out _byte);
                if (_byte == 0x10)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.EnterD5][1])
            {
                D5Enter.Deref<byte>(game, out _byte);
                if (_byte == 0x10)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.EnterD6][1])
            {
                D6Enter.Deref<byte>(game, out _byte);
                if (_byte == 0x12)
                    return true;
            }

            //items and others
            else if (segment == settings.SplitInfo[(int)Splits.Satchel][1])
            {
                Satchel.Deref<byte>(game, out _byte);
                if (_byte == 0xB0)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Harp1][1])
            {
                Harp1.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Feather][1])
            {
                Feather.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.D2Skip][1])
            {
                D2Skip.Deref<byte>(game, out _byte);
                if (_byte == 0x10)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Rope][1])
            {
                Raft.Deref<byte>(game, out _byte);
                if (_byte == 0x04)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Chart][1])
            {
                Raft.Deref<byte>(game, out _byte);
                if (_byte == 0x10)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.CI][1])
            {
                CrescentIsland.Deref<byte>(game, out _byte);
                if (_byte == 0x10)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Shooter][1])
            {
                Shooter.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Flute][1])
            {
                Flute.Deref<byte>(game, out _byte);
                if (_byte == 0x01 || _byte == 0x02 || _byte == 0x03)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Harp2][1])
            {
                Harp2.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Moblin][1])
            {
                Moblin.Deref<byte>(game, out _byte);
                if (_byte == 0x11)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Cane][1])
            {
                Cane.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Tuni][1])
            {
                TuniNut.Deref<byte>(game, out _byte);
                if (_byte == 0x02)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.SwitchHook][1])
            {
                SwitchHook.Deref<byte>(game, out _byte);
                if (_byte == 0x34)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.LavaJuice][1])
            {
                LavaJuice.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.MermaidSuit][1])
            {
                MermaidSuit.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.D6BK][1])
            {
                D6BossKey.Deref<byte>(game, out _byte);
                if (_byte == 0x30)
                    return true;
            }
            else if (segment == settings.SplitInfo[(int)Splits.Nayru][1])
            {
                NayruHP.Deref<byte>(game, out _byte);
                if (_byte == 0)
                    return true;
            }

            return false;
        }

        private enum Splits
        {
            L1Sword, D1Ess, D2Ess, D3Ess, D4Ess, D5Ess, D6Ess, D7Ess, D8Ess, Veran,
            EnterD1, EnterD2, EnterD3, EnterD4, EnterD5, EnterD6, EnterD7, EnterD8, EnterBT,
            //ages specific
            Satchel, Harp1, Feather, D2Skip, Rope, Chart, CI, Shooter, Flute, Harp2, Moblin, Cane, Tuni, SwitchHook, LavaJuice, MermaidSuit, D6BK, Nayru
        }
    }

    public enum GameVersion
    {
        unknown,
        OoS,
        OoA
    }

    public enum Emulator
    {
        unknown,
        bgb151,
        bgb152,
        gambatte571
    }
}
