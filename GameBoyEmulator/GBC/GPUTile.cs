namespace GameBoyEmulator.Desktop.GBC {
    public class GPUTile {
        public byte[][] TileData;

        public GPUTile() {
            TileData  = new byte[8][];
            for (var i = 0; i < 8; i++) {
                TileData[i] = new byte[8] { 0,0,0,0, 0,0,0,0 };                
            }
        }
    }
}
