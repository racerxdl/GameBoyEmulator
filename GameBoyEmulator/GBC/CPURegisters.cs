using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

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

        public bool FlagZero {
            get => (F & Flags.FLAG_ZERO) > 0;
            set {
                if (value) {
                    F |= Flags.FLAG_ZERO;
                } else {
                    F &= Flags.INV_FLAG_ZERO;
                }
            }
        }

        public bool FlagSub {
            get => (F & Flags.FLAG_SUB) > 0;
            set {
                if (value) {
                    F |= Flags.FLAG_SUB;
                } else {
                    F &= Flags.INV_FLAG_SUB;
                }
            }
        }

        public bool FlagHalfCarry {
            get => (F & Flags.FLAG_HALF_CARRY) > 0;
            set {
                if (value) {
                    F |= Flags.FLAG_HALF_CARRY;
                } else {
                    F &= Flags.INV_FLAG_HALF_CARRY;
                }
            }
        }

        public bool FlagCarry {
            get => (F & Flags.FLAG_CARRY) > 0;
            set {
                if (value) {
                    F |= Flags.FLAG_CARRY;
                } else {
                    F &= Flags.INV_FLAG_CARRY;
                }
            }
        }

        public CPURegisters Clone() {
            var reg = new CPURegisters();
            reg.A = A;
            reg.B = B;
            reg.C = C;
            reg.D = D;
            reg.E = E;
            reg.F = F;
            reg.H = H;
            reg.L = L;
            
            reg._A = _A;
            reg._B = _B;
            reg._C = _C;
            reg._D = _D;
            reg._E = _E;
            reg._F = _F;
            reg._H = _H;
            reg._L = _L;

            reg.InterruptEnable = InterruptEnable;
            reg.EnabledInterrupts = EnabledInterrupts;
            reg.TriggerInterrupts = TriggerInterrupts;
            reg.CycleCount = CycleCount;
            reg.PC = PC;
            reg.SP = SP;
            reg.lastClockM = lastClockM;
            reg.lastClockT = lastClockT;

            return reg;
        }

        public void RandomizeRegisters() {
            var random = new Random();

            var regs = new [] {"A", "B", "C", "D", "E", "F", "H", "L"};
            regs.ToList().ForEach((reg) => {
                SetRegister(reg, (byte) random.Next(0, 0xFF));
            });

            PC = (ushort) random.Next(0, 0xFFFF);
            SP = (ushort) random.Next(0, 0xFFFF);

            InterruptEnable = random.Next(0, 1) > 0;
            EnabledInterrupts = (byte) random.Next(0, 0xFF);
            TriggerInterrupts = (byte) random.Next(0, 0xFF);
        }

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
            // Console.WriteLine("Resetting Registers");
        }
    }
}
