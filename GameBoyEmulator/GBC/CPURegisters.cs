using System;
using System.Runtime.CompilerServices;

namespace GameBoyEmulator.Desktop.GBC {
    public class CPURegisters {
        public byte A, B, C, D, E, H, L, F;
        private byte _A, _B, _C, _D, _E, _H, _L, _F;

        public bool InterruptEnable;
        public byte EnabledInterrupts;
        public byte TriggerInterrupts;
        public int CycleCount;
        public ushort HL => (ushort) ((H << 8) + L);
        public ushort PC, SP;
        public int lastClockM, lastClockT;

        public void SaveRegs() {
            _A = A;
            _B = B;
            _C = C;
            _D = D;
            _E = E;
            _H = H;
            _L = L;
            _F = F;
        }

        public void LoadRegs() {
            A = _A;
            B = _B;
            C = _C;
            D = _D;
            E = _E;
            H = _H;
            L = _L;
            F = _F;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetRegister(string reg) {
            switch (reg.ToUpper()) {
                case "A": return A;
                case "B": return B;
                case "C": return C;
                case "D": return D;
                case "E": return E;
                case "H": return H;
                case "L": return L;
                case "F": return F;
                default:
                    Console.WriteLine($"Unknown Register: {reg}");
                    return 0x00;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRegister(string reg, byte val) {
            switch (reg.ToUpper()) {
                case "A": A = val;
                    break;
                case "B": B = val;
                    break;
                case "C": C = val;
                    break;
                case "D": D = val;
                    break;
                case "E": E = val;
                    break;
                case "H": H = val;
                    break;
                case "L": L = val;
                    break;
                case "F": F = val;
                    break;
                default:
                    Console.WriteLine($"Unknown Register: {reg}");
                    break;
            }
        }

        public void Reset() {
            CycleCount = 0;
            A = 0;
            B = 0;
            C = 0;
            D = 0;
            E = 0;
            H = 0;
            L = 0;
            F = 0;
            PC = 0;
            SP = 0;
            InterruptEnable = true;
            TriggerInterrupts = 0;
            lastClockM = 0;
            lastClockT = 0;
            EnabledInterrupts = 0;
            Console.WriteLine("Resetting Registers");
        }
    }
}
