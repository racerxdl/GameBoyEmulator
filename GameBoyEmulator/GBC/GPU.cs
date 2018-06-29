using System;
using Microsoft.Xna.Framework;

namespace GameBoyEmulator.Desktop.GBC {
    public class GPU {

        private const int VRAM_OFFSET = 0x8000;

        private int modeClocks;
        private int line;
        private GPUModes mode;
        private CPU cpu;

        private int scrollX, scrollY;
        private bool bgMap;
        private int bgTile;
        private bool switchBg;
        private bool switchLCD;

        internal byte[] oam;

        private Color[] BgPallete = {
            Color.Black,
            Color.DarkGray,
            Color.Gray,
            Color.White,
        };
        
        private Color[] Obj0Pallete = {
            Color.Black,
            Color.DarkGray,
            Color.Gray,
            Color.White,
        };

        private Color[] Obj1Pallete = {
            Color.Black,
            Color.DarkGray,
            Color.Gray,
            Color.White,
        };
        
        internal byte[] registers;

        public Color[] TileBuffer;

        private GPUTile[] tileSet;
        
        public GPU(CPU cpu) {
            this.cpu = cpu;
            TileBuffer = new Color[512 * 8 * 8];
            for (var i = 0; i < TileBuffer.Length; i++) {
                TileBuffer[i] = Color.White;
            }
            registers = new byte[0xFF];
        }

        public byte ReadByte(int addr) {
            Console.WriteLine($"GPU 0x{addr:X4} Read");
            switch (addr) {
                case 0xFF40:
                    return (byte) (
                        (switchBg ? 0x01 : 0x00) | 
                        (bgMap ? 0x08 : 0x00) | 
                        (bgTile > 0 ? 0x10 : 0x00) | 
                        (switchLCD ? 0x80 : 0x00)
                        );
                case 0xFF42:
                    return (byte) scrollY;
                case 0xFF43:
                    return (byte) scrollX;
                case 0xFF44:
                    return (byte) line;
                default:
                    return registers[addr - 0xFF40];
            }

            return 0x00;
        }

        public void WriteByte(int addr, byte val) {
            Console.WriteLine($"GPU 0x{addr:X4} Write 0x{val:X2}");
            registers[addr - 0xFF40] = val;
            switch (addr) {
                case 0xFF40:
                    switchBg = (val & 0x01) > 0;
                    bgMap = (val & 0x08) > 0;
                    bgTile = (val & 0x10) > 0 ? 1 : 0;
                    switchLCD = (val & 0x80) > 0;
                    break;
                case 0xFF42:
                    scrollY = val;
                    break;
                case 0xFF43:
                    scrollX = val;
                    break;
                case 0xFF47:
                    Console.WriteLine("Writting BG Pallete");
                    for (var i = 0; i < 4; i++) {
                        var b = (val >> (i * 2)) & 3;
                        switch (b) {
                            case 0: BgPallete[i] = Color.FromNonPremultiplied(255, 255, 255, 255);
                                break;
                            case 1: BgPallete[i] = Color.FromNonPremultiplied(192, 192, 192, 255);
                                break;
                            case 2: BgPallete[i] = Color.FromNonPremultiplied(96, 96, 96, 255);
                                break;
                            case 3: BgPallete[i] = Color.FromNonPremultiplied(0, 0, 0, 255);
                                break;
                        }
                    }
                    break;
                case 0xFF48:
                    Console.WriteLine("Writting Obj0 Pallete");
                    for (var i = 0; i < 4; i++) {
                        var b = (val >> (i * 2)) & 3;
                        switch (b) {
                            case 0: Obj0Pallete[i] = Color.FromNonPremultiplied(255, 255, 255, 255);
                                break;
                            case 1: Obj0Pallete[i] = Color.FromNonPremultiplied(192, 192, 192, 255);
                                break;
                            case 2: Obj0Pallete[i] = Color.FromNonPremultiplied(96, 96, 96, 255);
                                break;
                            case 3: Obj0Pallete[i] = Color.FromNonPremultiplied(0, 0, 0, 255);
                                break;
                        }
                    }
                    break;
                case 0xFF49:
                    Console.WriteLine("Writting Obj1 Pallete");
                    for (var i = 0; i < 4; i++) {
                        var b = (val >> (i * 2)) & 3;
                        switch (b) {
                            case 0: Obj1Pallete[i] = Color.FromNonPremultiplied(255, 255, 255, 255);
                                break;
                            case 1: Obj1Pallete[i] = Color.FromNonPremultiplied(192, 192, 192, 255);
                                break;
                            case 2: Obj1Pallete[i] = Color.FromNonPremultiplied(96, 96, 96, 255);
                                break;
                            case 3: Obj1Pallete[i] = Color.FromNonPremultiplied(0, 0, 0, 255);
                                break;
                        }
                    }
                    break;
            }
        }

        public void Reset() {
            modeClocks = 0;
            scrollX = 0;
            scrollY = 0;
            line = 0;
            bgMap = false;
            mode = GPUModes.OAM_READ;
            tileSet = new GPUTile[512];
            for (var i = 0; i < 512; i++) {
                tileSet[i] = new GPUTile();
            }
            bgTile = 0;
            oam = new byte[160];
        }

        public void RenderScanline() {
            if (switchLCD) {
                if (switchBg) {
                    var mapOffset = bgMap ? 0x1C00 : 0x1800;
                    mapOffset += (((line + scrollY) & 0xFF) >> 3) << 5;

                    var lineOffset = (scrollX >> 3) & 31;
                    var y = (line + scrollY) & 0x07;
                    var x = scrollX & 0x07;

                    var bufferOffset = line * 160;
                    var tile = (int) cpu.memory.ReadByte(VRAM_OFFSET + mapOffset + lineOffset);

                    if (bgTile == 1 && tile < 128) {
                        tile += 256;
                    }

                    var tileRow = tileSet[tile].TileData[y];

                    for (var i = 0; i < 160; i++) {
                        var color = BgPallete[tileRow[x]];
                        cpu.memory.videoBuffer[bufferOffset] = color;
                        bufferOffset++;
                        x++;
                        if (x != 8) continue;

                        x = 0;
                        lineOffset = (lineOffset + 1) & 31;
                        tile = cpu.memory.ReadByte(VRAM_OFFSET + mapOffset + lineOffset);
                        if (bgTile == 1 && tile < 128) {
                            tile += 256;
                        }

                        tileRow = tileSet[tile].TileData[y];
                    }
                }
            }
        }

        public void PutImage() {
            // TODO
        }

        private void RefreshTileData() {
            var i = 0;
            foreach (var tileData in tileSet) {
                // 128 x 256 Buffer
                // 16 x 32 tiles
                for (var y = 0; y < 8; y++) {
                    for (var x = 0; x < 8; x++) {
                        var px = (i % 16) * 8 + x;
                        var py = (i / 16) * 8 + y;
                        var p = py * 128 + px;
                        
                        TileBuffer[p] = BgPallete[tileData.TileData[y][x]];
                    }
                }
                i++;
            }
        }

        public void UpdateTile(int addr) {
            var relAddr = addr & 0x1FFF;
            if ((addr & 1) > 0) {
                addr--;
                relAddr--;
            }
            var tile = (relAddr >> 4) & 511;
            var y = (relAddr >> 1) & 7;

            for (var x = 0; x < 8; x++) {
                var sx = 1 << (7 - x);
                var b0 = cpu.memory.ReadByte(addr);
                var b1 = cpu.memory.ReadByte(addr+1);
                tileSet[tile].TileData[y][x] = (byte) (((b0 & sx) != 0 ? 1 : 0) + ((b1 & sx) != 0 ? 2 : 0));
            }
            RefreshTileData();
        } 
        
        public void Cycle() {
            modeClocks += cpu.clockM;
            
            switch (mode) {
                case GPUModes.HBLANK:
                    if (modeClocks >= 51) {
                        modeClocks = 0;
                        line++;

                        if (line == 143) {
                            mode = GPUModes.VBLANK;
                        } else {
                            mode = GPUModes.OAM_READ;
                        }
                    }
                    break;
                case GPUModes.VBLANK:
                    if (modeClocks >= 114) {
                        modeClocks = 0;
                        line++;
                        if (line > 153) {
                            mode = GPUModes.OAM_READ;
                            line = 0;
                        }
                    }

                    break;
                case GPUModes.OAM_READ:
                    if (modeClocks >= 20) {
                        modeClocks = 0;
                        mode = GPUModes.VRAM_READ;
                    }
                    break;
                case GPUModes.VRAM_READ:
                    if (modeClocks >= 43) {
                        modeClocks = 0;
                        mode = GPUModes.HBLANK;
                        
                        RenderScanline();
                    }

                    break;
            }
        }
    }
}
