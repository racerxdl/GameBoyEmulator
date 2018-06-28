namespace GameBoyEmulator.Desktop.GBC {
    public class CPURegisters {
        public byte A, B, C, D, E, H, L, F;
        public byte _A, _B, _C, _D, _E, _H, _L, _F;

        public byte InterruptEnable;
        public int CycleCount;
        public ushort HL {
            get { return (ushort) ((H << 8) + L); }
        }
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
                default: return 0x00;
            }
        }

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
            InterruptEnable = 0;
            lastClockM = 0;
            lastClockT = 0;
        }
    }
}
