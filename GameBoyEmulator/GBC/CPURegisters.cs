namespace GameBoyEmulator.Desktop.GBC {
    public struct CPURegisters {
        public byte A, B, C, D, E, H, L, F;
        public byte ime;

        public ushort HL {
            get { return (ushort) ((H << 8) + L); }
        }
        public ushort PC, SP;
        public int lastClockM, lastClockT;

        public byte GetRegister(string reg) {
            switch (reg.ToLower()) {
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
            switch (reg.ToLower()) {
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
            A = 0;
            B = 0;
            C = 0;
            D = 0;
            E = 0;
            H = 0;
            L = 0;
            F = 0;
            PC = 0;
            ime = 0;
            lastClockM = 0;
            lastClockT = 0;
        }
    }
}
