using System;

namespace GameBoyEmulator.Desktop.GBC {
    public class Memory {
        private readonly byte[] _videoRam = new byte[0x2000];
        private readonly byte[] _workRam0 = new byte[0x1000];
        private readonly byte[] _workRam1 = new byte[0x1000];
        private readonly byte[] _highRam = new byte[0x7E];
        private byte _interruptEnable;
        private byte[] _romData;
        private byte[] _catridgeRam;
        private byte _currentBank;
        // private byte _currentRamBank;

        public Memory() {
            Reset();
        }

        public void Reset() {
            _currentBank = 0;
            // _currentRamBank = 0;
            _interruptEnable = 0;
            for (var i = 0; i < 0x2000; i++) {
                _videoRam[i] = 0x00;
            }
            for (var i = 0; i < 0x1000; i++) {
                _workRam0[i] = 0x00;
                _workRam1[i] = 0x00;
            }

            for (var i = 0; i < 0x7E; i++) {
                _highRam[i] = 0x00;
            }
        }

        public void WriteByte(int addr, byte val) {
            if (addr <= 0x3FFF) {                          // Catridge ROM
                // _romData[addr] = val;
            } else if (addr >= 0x4000 && addr <= 0x7FFF) { // Catridge Bank N
                // _romData[addr - 0x4000 + 0x4000 * _currentBank] = val;
            } else if (addr >= 0x8000 && addr <= 0x9FFF) { // Video RAM
                _videoRam[addr - 0x8000] = val;
            } else if (addr >= 0xA000 && addr <= 0xBFFF) { // Catridge RAM
                _catridgeRam[addr - 0xA000] = val;
            } else if (addr >= 0xC000 && addr <= 0xCFFF) { // Work RAM Bank 0
                _workRam0[addr - 0xC000] = val;
            } else if (addr >= 0xD000 && addr <= 0xDFFF) { // Work RAM Bank 1
                _workRam1[addr - 0xD000] = val;
            } else if (addr >= 0xE000 && addr <= 0xFDFF) { // Mirror from 0xC000
                _workRam0[addr - 0xE000] = val;
            } else if (addr >= 0xFEA0 && addr <= 0xFEFF) { // Not usable, ... yet ...
                //
            } else if (addr >= 0xFF00 && addr <= 0xFF7F) { // I/O Ports
                //
            } else if (addr >= 0xFF80 && addr <= 0xFFFE) { // High RAM
                _highRam[addr - 0xFF80] = val;
            } else if (addr == 0xFFFF) {                   // Interrupt Enable Register  
                _interruptEnable = val;
            }
        }

        public byte ReadByte(int addr) {
            if (addr <= 0x3FFF) {                          // Catridge ROM
                return _romData[addr];
            } else if (addr >= 0x4000 && addr <= 0x7FFF) { // Catridge ROM Bank N
                return _romData[addr - 0x4000 + 0x4000 * _currentBank];
            } else if (addr >= 0x8000 && addr <= 0x9FFF) { // Video RAM
                return _videoRam[addr - 0x8000];
            } else if (addr >= 0xA000 && addr <= 0xBFFF) { // Catridge RAM
                return _catridgeRam[addr - 0xA000];
            } else if (addr >= 0xC000 && addr <= 0xCFFF) { // Work RAM Bank 0
                return _workRam0[addr - 0xC000];
            } else if (addr >= 0xD000 && addr <= 0xDFFF) { // Work RAM Bank 1
                return _workRam1[addr - 0xD000];
            } else if (addr >= 0xE000 && addr <= 0xFDFF) { // Mirror from 0xC000
                return _workRam0[addr - 0xE000];
            } else if (addr >= 0xFEA0 && addr <= 0xFEFF) { // Not usable, ... yet ...
                //
            } else if (addr >= 0xFF00 && addr <= 0xFF7F) { // I/O Ports
                //
            } else if (addr >= 0xFF80 && addr <= 0xFFFE) { // High RAM
                return _highRam[addr - 0xFF80];
            } else if (addr == 0xFFFF) {                   // Interrupt Enable Register  
                return _interruptEnable;
            }

            return 0x00;
        }

        public ushort ReadWord(ushort addr) {
            var b0 = ReadByte(addr);
            var b1 = ReadByte((ushort)(addr + 1));

            return (ushort) (b0 << 8 + b1);
        }

        public void WriteWord(ushort addr, ushort val) {
            var b0 = (byte) ((val & 0xFF00) >> 8);
            var b1 = (byte) (val & 0xFF);
            WriteByte(addr, b0);
            WriteByte(addr + 1, b1);
        }

        public void LoadROM(byte[] romData) {
            _romData = romData;
        }
    }
}
