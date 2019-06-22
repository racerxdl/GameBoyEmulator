namespace GameBoyEmulator.Desktop.GBC {
    public static class Flags {
        #region Interrupts
        public static readonly byte INT_VBLANK = 0x01;
        public static readonly byte INT_LCDSTAT = 0x02;
        public static readonly byte INT_TIMER = 0x04;
        public static readonly byte INT_SERIAL = 0x08;
        public static readonly byte INT_JOYPAD = 0x10;
        #endregion
        #region LCDSTAT

        public const int FLAG_LYC_LY = 0x40;
        public const int FLAG_OAM_MODE = 0x20;
        public const int FLAG_VBLANK_MODE = 0x10;
        public const int FLAG_HBLANK_MODE = 0x08;

        #endregion
        #region Registers

        public const int FLAG_CARRY = 0x10;
        public const int FLAG_HALF_CARRY = 0x20;
        public const int FLAG_SUB = 0x40;
        public const int FLAG_ZERO = 0x80;

        public const int INV_FLAG_CARRY = 0xEF;
        public const int INV_FLAG_HALF_CARRY = 0xDF;
        public const int INV_FLAG_SUB = 0xBF;
        public const int INV_FLAG_ZERO = 0x7F;

        #endregion
    }
}
