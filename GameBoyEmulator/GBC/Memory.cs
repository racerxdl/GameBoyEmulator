using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace GameBoyEmulator.Desktop.GBC {
    public class Memory {
        private readonly byte[] _bios = {
            0x31, 0xFE, 0xFF, 0xAF, 0x21, 0xFF, 0x9F, 0x32, 0xCB, 0x7C, 0x20, 0xFB, 0x21, 0x26, 0xFF, 0x0E,
            0x11, 0x3E, 0x80, 0x32, 0xE2, 0x0C, 0x3E, 0xF3, 0xE2, 0x32, 0x3E, 0x77, 0x77, 0x3E, 0xFC, 0xE0,
            0x47, 0x11, 0x04, 0x01, 0x21, 0x10, 0x80, 0x1A, 0xCD, 0x95, 0x00, 0xCD, 0x96, 0x00, 0x13, 0x7B,
            0xFE, 0x34, 0x20, 0xF3, 0x11, 0xD8, 0x00, 0x06, 0x08, 0x1A, 0x13, 0x22, 0x23, 0x05, 0x20, 0xF9,
            0x3E, 0x19, 0xEA, 0x10, 0x99, 0x21, 0x2F, 0x99, 0x0E, 0x0C, 0x3D, 0x28, 0x08, 0x32, 0x0D, 0x20,
            0xF9, 0x2E, 0x0F, 0x18, 0xF3, 0x67, 0x3E, 0x64, 0x57, 0xE0, 0x42, 0x3E, 0x91, 0xE0, 0x40, 0x04,
            0x1E, 0x02, 0x0E, 0x0C, 0xF0, 0x44, 0xFE, 0x90, 0x20, 0xFA, 0x0D, 0x20, 0xF7, 0x1D, 0x20, 0xF2,
            0x0E, 0x13, 0x24, 0x7C, 0x1E, 0x83, 0xFE, 0x62, 0x28, 0x06, 0x1E, 0xC1, 0xFE, 0x64, 0x20, 0x06,
            0x7B, 0xE2, 0x0C, 0x3E, 0x87, 0xF2, 0xF0, 0x42, 0x90, 0xE0, 0x42, 0x15, 0x20, 0xD2, 0x05, 0x20,
            0x4F, 0x16, 0x20, 0x18, 0xCB, 0x4F, 0x06, 0x04, 0xC5, 0xCB, 0x11, 0x17, 0xC1, 0xCB, 0x11, 0x17,
            0x05, 0x20, 0xF5, 0x22, 0x23, 0x22, 0x23, 0xC9, 0xCE, 0xED, 0x66, 0x66, 0xCC, 0x0D, 0x00, 0x0B,
            0x03, 0x73, 0x00, 0x83, 0x00, 0x0C, 0x00, 0x0D, 0x00, 0x08, 0x11, 0x1F, 0x88, 0x89, 0x00, 0x0E,
            0xDC, 0xCC, 0x6E, 0xE6, 0xDD, 0xDD, 0xD9, 0x99, 0xBB, 0xBB, 0x67, 0x63, 0x6E, 0x0E, 0xEC, 0xCC,
            0xDD, 0xDC, 0x99, 0x9F, 0xBB, 0xB9, 0x33, 0x3E, 0x3c, 0x42, 0xB9, 0xA5, 0xB9, 0xA5, 0x42, 0x4C,
            0x21, 0x04, 0x01, 0x11, 0xA8, 0x00, 0x1A, 0x13, 0xBE, 0x20, 0xFE, 0x23, 0x7D, 0xFE, 0x34, 0x20,
            0xF5, 0x06, 0x19, 0x78, 0x86, 0x23, 0x05, 0x20, 0xFB, 0x86, 0x20, 0xFE, 0x3E, 0x01, 0xE0, 0x50
        };
        
        private readonly byte[] _videoRam = new byte[0x2000];
        private readonly byte[] _workRam = new byte[0x2000];
        private readonly byte[] _highRam = new byte[0x7F];
        private byte[] _romData = new byte[0x8000];
        private byte[] _catridgeRam = new byte[0x2000];
        private byte _currentBank;

        internal Color[] videoBuffer;

        internal bool inBIOS;

        private CPU cpu;

        public Memory(CPU cpu) {
            videoBuffer = new Color[160*144];
            for (var i = 0; i < videoBuffer.Length; i++) {
                videoBuffer[i] = Color.White;
            }

            this.cpu = cpu;
            Reset();
        }

        public Color[] GetVideoBuffer() {
            return videoBuffer;
        }

        public void Reset() {
            _currentBank = 0;
            // _currentRamBank = 0;

            for (var i = 0; i < 0x7FFF; i++) {
                _romData[i] = 0x00;
            }
            
            for (var i = 0; i < 0x2000; i++) {
                _videoRam[i] = 0x00;
                _catridgeRam[i] = 0x00;
                _workRam[i] = 0x00;
            }
            
            for (var i = 0; i < 0x7F; i++) {
                _highRam[i] = 0x00;
            }

            inBIOS = true;
        }

        public void WriteByte(int addr, byte val) {
            // Console.WriteLine($"Writting 0x{addr:X4} val 0x{val:X2}");
            if (addr <= 0x3FFF) {                          // Catridge ROM
                // _romData[addr] = val;
            } else if (addr >= 0x4000 && addr <= 0x7FFF) { // Catridge Bank N
                // _romData[addr + 0x4000 * _currentBank] = val;
            } else if (addr >= 0x8000 && addr <= 0x9FFF) { // Video RAM
                _videoRam[addr - 0x8000] = val;
                cpu.gpu.UpdateTile(addr, val);
            } else if (addr >= 0xA000 && addr <= 0xBFFF) { // Catridge RAM
                _catridgeRam[addr - 0xA000] = val;
            } else if (addr >= 0xC000 && addr <= 0xEFFF) { // Work RAM
                _workRam[addr & 0x1FFF] = val;
            } else if (addr >= 0xFE00 && addr <= 0xFE9F) {
                cpu.gpu.UpdateOAM(addr, val);
                cpu.gpu.oam[addr - 0xFE00] = val;
            } else if (addr >= 0xFEA0 && addr <= 0xFEFF) { // Not usable, ... yet ...
                //
            } else if (addr >= 0xFF00 && addr <= 0xFF7F) { // I/O Ports
                var baseAddr = addr - 0xFF00;
                switch (baseAddr & 0x00F0) {
                    case 0x00:
                        if ((addr & 0xF) == 0) {
                            cpu.GbKeys.Write(val);
                        }
                        if ((addr & 0xF) == 15) {
                            cpu.reg.TriggerInterrupts = val;
                        }
                        break;
                    case 0x10:
                    case 0x20:
                    case 0x30:
                        break;
                    case 0x40:
                    case 0x50:
                    case 0x60:
                    case 0x70:
                        cpu.gpu.WriteByte(addr, val);
                        break;
                }
            } else if (addr >= 0xFF80 && addr <= 0xFFFE) { // High RAM
                _highRam[addr - 0xFF80] = val;
            } else if (addr == 0xFFFF) {                   // Interrupt Enable Register  p
                cpu.reg.EnabledInterrupts = val;
            }
        }

        public byte ReadByte(int addr) {
            // Console.WriteLine($"Reading 0x{addr:X4}");
            if (addr <= 0x3FFF) {                          // Catridge ROM
                if (inBIOS) {
                    if (addr < 0x100) {
                        return _bios[addr];
                    }

                    if (addr == 0x100) {
                        Console.WriteLine("Jumping out BIOS");
                        inBIOS = false;
                    }
                }
                return _romData[addr];
            } else if (addr >= 0x4000 && addr <= 0x7FFF) { // Catridge ROM Bank N
                return _romData[addr + 0x4000 * _currentBank];
            } else if (addr >= 0x8000 && addr <= 0x9FFF) { // Video RAM
                return _videoRam[addr - 0x8000];
            } else if (addr >= 0xA000 && addr <= 0xBFFF) { // Catridge RAM
                return _catridgeRam[addr - 0xA000];
            } else if (addr >= 0xC000 && addr <= 0xEFFF) { // Work RAM
                return _workRam[addr & 0x1FFF];
            } else if (addr >= 0xFE00 && addr <= 0xFE9F) {
                return cpu.gpu.oam[addr - 0xFE00];
            } else if (addr >= 0xFEA0 && addr <= 0xFEFF) { // Not usable, ... yet ...
                //
            } else if (addr >= 0xFF00 && addr <= 0xFF7F) { // I/O Ports
                var baseAddr = addr - 0xFF00;
                switch (baseAddr & 0x00F0) {
                    case 0x00:
                        if ((addr & 0xF) == 0) {
                            return cpu.GbKeys.Read();
                        }
                        return ((addr & 0xF) == 15) ? cpu.reg.TriggerInterrupts : (byte) 0x00;
                    case 0x10:
                    case 0x20:
                    case 0x30:
                        return 0;
                    case 0x40:
                    case 0x50:
                    case 0x60:
                    case 0x70:
                        return cpu.gpu.ReadByte(addr);
                }
            } else if (addr >= 0xFF80 && addr <= 0xFFFE) { // High RAM
                return _highRam[addr - 0xFF80];
            } else if (addr == 0xFFFF) {                   // Interrupt Enable Register  
                return cpu.reg.EnabledInterrupts;
            }

            return 0x00;
        }

        public ushort ReadWord(ushort addr) {
            var b0 = ReadByte(addr);
            var b1 = ReadByte(addr + 1);

            return (ushort) ((b1 << 8) + b0);
        }

        public void WriteWord(ushort addr, ushort val) {
            var b0 = (byte) (val >> 8);
            var b1 = (byte) (val & 0xFF);
            WriteByte(addr, b1);
            WriteByte(addr + 1, b0);
        }

        public string GetRomName() {
            var slice = _romData.Skip(0x134).Take(0xE).ToArray();
            return Encoding.ASCII.GetString(slice);
        }

        public RamSize GetCatridgeRamSize() {
            return (RamSize) _romData[0x149];
        }

        public RomSize GetRomSize() {
            return (RomSize) _romData[0x148];
        }

        public void LoadROM(byte[] romData) {
            _romData = romData;
        }
    }
}
