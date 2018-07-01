using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Xna.Framework;

namespace GameBoyEmulator.Desktop.GBC {
    public class GPU {
        private int modeClocks;
        private int line;
        private GPUModes mode;
        private CPU cpu;

        internal int scrollX, scrollY;
        internal bool switchBg;
        internal bool switchLCD;
        internal bool objSize;
        internal bool switchObj;
        private byte raster;
        private byte enabledInterrupts;
        private byte interruptsFired;
        private byte[] currentRow;

        internal ushort bgTileBase;
        internal ushort bgMapBase;
        internal ushort winMapBase;

        internal byte[] oam;

        internal GPUObject[] objs;
        internal GPUObject[] prioObjects;
        
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
        public Color[] VRamBuffer;

        private GPUTile[] tileSet;
        
        public GPU(CPU cpu) {
            this.cpu = cpu;
            TileBuffer = new Color[144 * 288];
            for (var i = 0; i < TileBuffer.Length; i++) {
                var x = i % 144;
                var y = i / 144;
                TileBuffer[i] = (x % 9 == 8) || (y % 9 == 8) ? Color.Transparent : Color.White;
            }
            registers = new byte[0xFF];
            currentRow = new byte[160];
            for (var i = 0; i < 160; i++) {
                currentRow[i] = 0x00;
            }
            Reset();
        }

        public void Reset() {
            modeClocks = 0;
            scrollX = 0;
            scrollY = 0;
            line = 0;
            mode = GPUModes.OAM_READ;
            tileSet = new GPUTile[512];
            for (var i = 0; i < 512; i++) {
                tileSet[i] = new GPUTile();
            }
            VRamBuffer = new Color[256*256];
            for (var i = 0; i < 256 * 256; i++) {
                VRamBuffer[i] = Color.White;
            }
            oam = new byte[160];
            switchLCD = false;
            switchBg = false;
            objSize = false;
            objs = new GPUObject[40];
            prioObjects = new GPUObject[40];
            for (var i = 0; i < 40; i++) {
                objs[i] = new GPUObject {
                    Pos = i,
                    Y = -16,
                    X = -8,
                    Tile = 0,
                    Palette = 0,
                    YFlip = false,
                    XFlip = false
                };
                prioObjects[i] = objs[i];
            }

            switchObj = false;
            raster = 0;
            enabledInterrupts = 0;
            interruptsFired = 0;
            bgTileBase = 0x0000;
            bgMapBase = 0x1800;
            winMapBase = 0x1800;
        }

        public byte ReadByte(int addr) {
            // Console.WriteLine($"GPU 0x{addr:X4} Read");
            switch (addr) {
                case 0xFF40:
                    return (byte) (
                        (switchBg ? 0x01 : 0x00) | 
                        (switchObj ? 0x02 : 0x00) |
                        (objSize ? 0x04 : 0x00) |
                        (bgMapBase == 0x1C00 ? 0x08 : 0x00) | 
                        (bgTileBase == 0x0000 ? 0x10 : 0x00) | 
                        (switchLCD ? 0x80 : 0x00)
                        );
                case 0xFF41:
                    var ift = interruptsFired;
                    interruptsFired = 0x00;
                    return (byte) ((interruptsFired << 3) | (line == raster ? 4 : 0) | (int)mode);
                case 0xFF42:
                    return (byte) scrollY;
                case 0xFF43:
                    return (byte) scrollX;
                case 0xFF44:
                    return (byte) line;
                case 0xFF45:
                    return raster;
                default:
                    return registers[addr - 0xFF40];
            }
        }

        public void WriteByte(int addr, byte val) {
            // Console.WriteLine($"GPU 0x{addr:X4} Write 0x{val:X2}");
            registers[addr - 0xFF40] = val;
            switch (addr) {
                case 0xFF40:
                    switchBg = (val & 0x01) > 0;
                    switchObj = (val & 0x02) > 0;
                    objSize = (val & 0x04) > 0;
                    bgMapBase = (ushort) ((val & 0x08) > 0 ? 0x1C00 : 0x1800);
                    bgTileBase = (ushort) ((val & 0x10) > 0 ? 0x0000 : 0x0800);
                    switchLCD = (val & 0x80) > 0;
                    break;
                case 0xFF41:
                    enabledInterrupts = (byte) ((val >> 3) & 0xF);
                    break;
                case 0xFF42:
                    scrollY = val;
                    break;
                case 0xFF43:
                    scrollX = val;
                    break;
                case 0xFF45:
                    raster = val;
                    break;
                case 0xFF46:
                    for (var i = 0; i < 160; i++) {
                        var v = cpu.memory.ReadByte((val << 8) + i);
                        oam[i] = v;
                        UpdateOAM(0xFE00 + i, v, false);
                    }
                    SortOAM();
                    break;
                case 0xFF47:
                    // Console.WriteLine("Writting BG Pallete");
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
                    
                    RefreshTileData(-1);
                    break;
                case 0xFF48:
                    // Console.WriteLine("Writting Obj0 Pallete");
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
                    // Console.WriteLine("Writting Obj1 Pallete");
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

        public void RenderScanline() {
            if (switchLCD) {
                #region Background Draw
                if (switchBg) {
                    var vramOffset = Addresses.VRAMBASE;
                    vramOffset += bgMapBase;
                    vramOffset += (((line + scrollY) & 0xFF) / 8) * 32;
                    
                    var y = (line + scrollY) % 8;
                    var x = scrollX % 8;
                    var tileOffset = (scrollX / 8) % 32;
                    var bufferOffset = line * 160;

                    var tile = (int) cpu.memory.ReadByte(vramOffset + tileOffset);
                    
                    if (bgTileBase != 0x0000 && tile < 128) {
                        tile += 256;
                    }

                    var tileRow = tileSet[tile].TileData[y];

                    for (var i = 0; i < 160; i++) {
                        var color = BgPallete[tileRow[x]];
                        currentRow[i] = tileRow[x];
                        cpu.memory.videoBuffer[bufferOffset] = color;
                        bufferOffset++;
                        x++;
                        if (x != 8) continue;

                        x = 0;
                        tileOffset = (tileOffset + 1) % 32;
                        tile = cpu.memory.ReadByte(vramOffset + tileOffset);
                        if (bgTileBase != 0x0000 && tile < 128) {
                            tile += 256;
                        }

                        tileRow = tileSet[tile].TileData[y];
                    }
                }
                #endregion
                #region Object Draw 

                if (switchObj) {
                    for (var i = 0; i < 40; i++) {
                        var obj = prioObjects[i];
                        if (obj.Y <= line && (obj.Y + 8) > line) {
                            var tileData = tileSet[obj.Tile];
                            var tileRow = obj.YFlip
                                ? tileData.TileData[7 - (line - obj.Y)]
                                : tileData.TileData[line - obj.Y];

                            var pallete = obj.Palette != 0 ? Obj0Pallete : Obj1Pallete;
                            var bufferOffset = (line * 160) + obj.X;
                            for (var x = 0; x < 8; x++) {
                                var color = obj.XFlip ? pallete[tileRow[7 - x]] : pallete[tileRow[x]];
                                if (tileRow[x] != 0x00 && obj.X + x >= 0 && obj.X + x < 160 && (obj.Prio || currentRow[x] == 0x00)) {
                                    cpu.memory.videoBuffer[bufferOffset] = color;
                                }

                                bufferOffset++;
                            }    
                        }
                    }
                }
                #endregion
            }
        }

        private void RefreshTileData(int tileNum) {
            if (tileNum == -1) {
                var i = 0;
                foreach (var tileData in tileSet) {
                    // 16 x 32 tiles with 1px spacing
                    // 16 * 9 x 32 * 9
                    // 144 x 288 Buffer
                    for (var y = 0; y < 8; y++) {
                        for (var x = 0; x < 8; x++) {
                            var px = (i % 16) * 9 + x;
                            var py = (i / 16) * 9 + y;
                            var p = py * 144 + px;
    
                            TileBuffer[p] = BgPallete[tileData.TileData[y][x]];
                        }
                    }
                    i++;
                }
            } else {
                var i = tileNum;
                var tileData = tileSet[tileNum];
                // 16 x 32 tiles with 1px spacing
                // 16 * 9 x 32 * 9
                // 144 x 288 Buffer
                for (var y = 0; y < 8; y++) {
                    for (var x = 0; x < 8; x++) {
                        var px = (i % 16) * 9 + x;
                        var py = (i / 16) * 9 + y;
                        var p = py * 144 + px;

                        TileBuffer[p] = BgPallete[tileData.TileData[y][x]];
                    }
                }
            }
        }

        public void UpdateVRAM() {
            for (var i = 0; i < VRamBuffer.Length; i++) {
                var py = (i / 256);
                var px = (i % 256);
                var tileNum = (px / 8) + ((py / 8) * 32);
                var v = cpu.memory.ReadByte(Addresses.VRAMBASE + bgMapBase + tileNum);
                var tile = tileSet[v];
                var x = px % 8;
                var y = py % 8;
                VRamBuffer[i] = BgPallete[tile.TileData[y][x]];
            }
        }
        
        public void UpdateTile(int addr, byte val) {
            var relAddr = addr & 0x1FFF;
            if ((addr & 1) > 0) {
                addr--;
                relAddr--;
            }
            var tile = (relAddr >> 4) & 511;
            var y = (relAddr >> 1) & 7;
            var b0 = cpu.memory.ReadByte(addr);
            var b1 = cpu.memory.ReadByte(addr+1);

            for (var x = 0; x < 8; x++) {
                var sx = 1 << (7 - x);
                tileSet[tile].TileData[y][x] = (byte) (((b0 & sx) != 0 ? 1 : 0) + ((b1 & sx) != 0 ? 2 : 0));
            }
            RefreshTileData(tile);
        }

        public void UpdateOAM(int addr, byte val, bool sort=true) {
            var relAddr = addr - 0xFE00;
            var obj = relAddr >> 2;
            if (obj < 40) {
                switch (relAddr & 3) {
                    case 0: 
                        objs[obj].Y = val - 16;
                        break;
                    case 1:
                        objs[obj].X = val - 8;
                        break;
                    case 2:
                        objs[obj].Tile = (objSize) ? (byte) (val & 0xFE) : val;
                        break;
                    case 3:
                        objs[obj].Palette = (val & 0x10) != 0 ? 1 : 0;
                        objs[obj].XFlip = (val & 0x20) != 0;
                        objs[obj].YFlip = (val & 0x40) != 0;
                        objs[obj].Prio = (val & 0x80) != 0;
                        break;
                }
            }

            if (sort) {
                SortOAM();
            }
        }

        public void SortOAM() {
            
            var sorted = objs.ToList();
            sorted.Sort((a, b) => {
                if (a.X > b.X) {
                    return -1;
                }

                if (a.X < b.X) {
                    return 1;
                }

                if (a.Pos > b.Pos) {
                    return -1;
                }

                if (a.Pos < b.Pos) {
                    return 1;
                }

                return 0;
            });
            prioObjects = sorted.ToArray();
        }
        
        public void Cycle() {
            modeClocks += cpu.reg.lastClockM;
            
            switch (mode) {
                case GPUModes.HBLANK:
                    if (modeClocks >= 51) {
                        if (line == 143) {
                            mode = GPUModes.VBLANK;
                            cpu.reg.TriggerInterrupts |= Flags.INT_VBLANK;
                        } else {
                            mode = GPUModes.OAM_READ;
                        }
                        
                        modeClocks = 0;
                        line++;
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
                default:
                    Console.WriteLine($"BUG! Went to a unknown state: {mode}");
                    break;
            }
        }
    }
}
