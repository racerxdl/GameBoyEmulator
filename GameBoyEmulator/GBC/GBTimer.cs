using System.Timers;

namespace GameBoyEmulator.Desktop.GBC {
    public class GBTimer {
        private int div, tma, tima, tac;
        private int clockMain, clockSub, clockDiv;

        private CPU cpu;
        
        public GBTimer(CPU cpu) {
            this.cpu = cpu;
            Reset();
        }

        public void Reset() {
            div = 0;
            tma = 0;
            tma = 0;
            tima = 0;
            tac = 0;
            clockMain = 0;
            clockSub = 0;
            clockDiv = 0;
        }

        public void Cycle() {
            tima++;
            clockMain = 0;
            if (tima > 255) {
                tima = tma;
                cpu.reg.TriggerInterrupts |= Flags.INT_TIMER;
            }
        }

        public void Increment() {
            clockSub += cpu.reg.lastClockM;

            if (clockSub > 3) {
                clockMain++;
                clockSub -= 4;
                clockDiv++;

                if (clockDiv == 16) {
                    clockDiv = 0;
                    div++;
                    div %= 256;
                }
            }

            if ((tac & 0x04) != 0x00) {
                switch (tac & 0x03) {
                    case 0: 
                        if (clockMain >= 64) 
                            Cycle();
                        break;
                    case 1:
                        if (clockMain >= 1)
                            Cycle();
                        break;
                    case 2:
                        if (clockMain >= 4)
                            Cycle();
                        break;
                    case 3:
                        if (clockMain >= 16)
                            Cycle();
                        break;
                }
            }
        }

        public byte Read(int addr) {
            switch (addr) {
                case 0xFF04: return (byte) div;
                case 0xFF05: return (byte) tima;
                case 0xFF06: return (byte) tma;
                case 0xFF07: return (byte) tac;
            }

            return 0x00;
        }

        public void Write(int addr, byte val) {
            switch (addr) {
                case 0xFF04: 
                    div = 0;
                    break;
                case 0xFF05: 
                    tima = val;
                    break;
                case 0xFF06: 
                    tma = val;
                    break;
                case 0xFF07:
                    tac = val & 0x07;
                    break;
            }
        }
    }
}
