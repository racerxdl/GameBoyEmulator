namespace GameBoyEmulator.Desktop.GBC {
    public static class Addresses {
        public const int VRAMBASE = 0x8000;
        public const ushort INT_VBLANK = 0x40;
        public const ushort INT_LCDSTAT = 0x48;
        public const ushort INT_TIMER = 0x50;
        public const ushort INT_SERIAL = 0x58;
        public const ushort INT_JOYPAD = 0x60;
    }
}
