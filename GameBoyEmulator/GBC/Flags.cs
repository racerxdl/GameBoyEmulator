namespace GameBoyEmulator.Desktop.GBC {
    public static class Flags {
        public static readonly byte INT_VBLANK = 0x01;
        public static readonly byte INT_LCDSTAT = 0x02;
        public static readonly byte INT_TIMER = 0x04;
        public static readonly byte INT_SERIAL = 0x08;
        public static readonly byte INT_JOYPAD = 0x10;
        
        #region Registers

        public const int FLAG_CARRY = 0x10;
        public const int FLAG_HALF_CARRY = 0x20;
        public const int FLAG_SUB = 0x40;
        public const int FLAG_ZERO = 0x80;

        #endregion
    }
}
