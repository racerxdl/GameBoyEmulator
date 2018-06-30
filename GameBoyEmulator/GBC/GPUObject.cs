using Microsoft.Xna.Framework;

namespace GameBoyEmulator.Desktop.GBC {
    public class GPUObject {
        public int Pos { get; set; }
        public int Palette { get; set; }
        public bool XFlip { get; set; }
        public bool YFlip { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool Prio { get; set; }
        public byte Tile { get; set; }
    }
}
