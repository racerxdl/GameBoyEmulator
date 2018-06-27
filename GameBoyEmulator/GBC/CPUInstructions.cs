namespace GameBoyEmulator.Desktop.GBC {
    public static class CPUInstructions {
        #region Load / Store Instructions
        /// <summary>
        /// Set regI = regO
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regI"></param>
        /// <param name="regO"></param>
        internal static void LDrr(CPU cpu, string regI, string regO) {
            var reg = cpu.reg;
            reg.SetRegister(regI, reg.GetRegister(regO));
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        /// <summary>
        /// Read Memory position (H/L) to regO
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regO"></param>
        internal static void LDrHLm_(CPU cpu, string regO) {
            var reg = cpu.reg;
            var hl = reg.HL;
            var b = cpu.memory.ReadByte(hl);
            reg.SetRegister(regO, b);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        /// <summary>
        /// Write regI to Memory Position (H/L)
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regI"></param>
        internal static void LDHLmr_(CPU cpu, string regI) {
            var reg = cpu.reg;
            var hl = reg.HL;
            var b = reg.GetRegister(regI);
            cpu.memory.WriteByte(hl, b);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        /// <summary>
        /// Loads the byte from Program Memory into regO. Increments Program Counter
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regO"></param>
        internal static void LDrn_(CPU cpu, string regO) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            reg.SetRegister(regO, b);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        /// <summary>
        /// Writes byte from Program Memory into Memory (H/L). Increments Program Counter
        /// </summary>
        /// <param name="cpu"></param>
        internal static void LDHLmn(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            cpu.memory.WriteByte(reg.HL, b);
            reg.lastClockM += 3;
            reg.lastClockT += 12;
        }

        /// <summary>
        /// Writes regI value into regH/regL memory.
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regH"></param>
        /// <param name="regL"></param>
        /// <param name="regI"></param>
        internal static void LD__m_(CPU cpu, string regH, string regL, string regI) {
            var reg = cpu.reg;
            var h = reg.GetRegister(regH);
            var l = reg.GetRegister(regL);
            var hl = (h << 8) + l;
            var b = reg.GetRegister(regI);
            cpu.memory.WriteByte(hl, b);
            reg.lastClockM += 2;
            reg.lastClockT += 8;
        }

        /// <summary>
        /// Stores value of regI in memory position of PC
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regI"></param>
        internal static void LDmm_(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            cpu.memory.WriteByte(reg.PC, b);
            reg.lastClockM += 4;
            reg.lastClockT += 16;
        }

        /// <summary>
        /// Reads from memory regH/regL and stores in regO
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regH"></param>
        /// <param name="regL"></param>
        /// <param name="regO"></param>
        internal static void LD___m(CPU cpu, string regO, string regH, string regL) {
            var reg = cpu.reg;
            var h = reg.GetRegister(regH);
            var l = reg.GetRegister(regL);
            var hl = (h << 8) + l;
            var b = cpu.memory.ReadByte(hl);
            reg.PC += 2;
            reg.SetRegister(regO, b);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        /// <summary>
        /// Read from memory at word pointed by PC and stores at regO
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regO"></param>
        internal static void LD_mm(CPU cpu, string regO) {
            var reg = cpu.reg;
            var addr = cpu.memory.ReadWord(reg.PC);
            var b = cpu.memory.ReadByte(addr);
            reg.SetRegister(regO, b);
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        /// <summary>
        /// Reads from memory pointed by PC to regO1 and PC+1 to regO2
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regO1"></param>
        /// <param name="regO2"></param>
        internal static void LD__nn(CPU cpu, string regO1, string regO2) {
            var reg = cpu.reg;
            var b0 = cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            var b1 = cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            reg.SetRegister(regO1, b0);
            reg.SetRegister(regO2, b1);
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        /// <summary>
        /// Reads word from Program Counter and stores in SP
        /// </summary>
        /// <param name="cpu"></param>
        internal static void LDSPnn(CPU cpu) {
            var reg = cpu.reg;
            var u = cpu.memory.ReadWord(reg.PC);
            reg.PC += 2;
            reg.SP = u;
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        /// <summary>
        /// Reads an address from PC position and writes addr to L and addr + 1 to H
        /// </summary>
        /// <param name="cpu"></param>
        internal static void LDHLmm(CPU cpu) {
            var reg = cpu.reg;
            var u = cpu.memory.ReadWord(reg.PC);
            reg.PC += 2;
            reg.L = cpu.memory.ReadByte(u);
            reg.H = cpu.memory.ReadByte(u + 1);
            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        /// <summary>
        /// Reads an address from PC position and writes word from H/L
        /// </summary>
        /// <param name="cpu"></param>
        internal static void LDmmHL(CPU cpu) {
            var reg = cpu.reg;
            var u = cpu.memory.ReadWord(reg.PC);
            reg.PC += 2;
            cpu.memory.WriteWord(u, reg.HL);
            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        /// <summary>
        /// Sets A to Memory at H/L and increments HL.
        /// </summary>
        /// <param name="cpu"></param>
        internal static void LDHLIA(CPU cpu) {
            var reg = cpu.reg;
            var b = reg.A;
            cpu.memory.WriteByte(reg.HL, b);
            reg.L++;
            if (reg.L == 0) {
                reg.H++;
            }

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        /// <summary>
        /// Reads byte from H/L into A and decrements H/L
        /// </summary>
        /// <param name="cpu"></param>
        internal static void LDAHLI(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            reg.A = b;
            reg.L--;
            if (reg.L == 255) {
                reg.H--;
            }

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void LDAIOn(CPU cpu) {
            var reg = cpu.reg;
            var addr = cpu.memory.ReadByte(reg.PC) + 0xFF00;
            reg.PC++;
            var b = cpu.memory.ReadByte(addr);
            reg.A = b;

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void LDIOnA(CPU cpu) {
            var reg = cpu.reg;
            var addr = cpu.memory.ReadByte(reg.PC) + 0xFF00;
            reg.PC++;
            cpu.memory.WriteByte(addr, reg.A);
            
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void LDCIOn(CPU cpu) {
            var reg = cpu.reg;
            var addr = cpu.memory.ReadByte(reg.C) + 0xFF00;
            var b = cpu.memory.ReadByte(addr);
            reg.A = b;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void LDIOnC(CPU cpu) {
            var reg = cpu.reg;
            var addr = cpu.memory.ReadByte(reg.C) + 0xFF00;
            cpu.memory.WriteByte(addr, reg.A);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void LDHLSPn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int) cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            v += reg.SP;
            reg.H = (byte) ((v >> 8) & 0xFF);
            reg.L = (byte) (v & 0xFF);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        /// <summary>
        /// Swaps value in regS with memory at addres H/L
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regS"></param>
        internal static void SWAPr__(CPU cpu, string regS) {
            var reg = cpu.reg;
            var tr = reg.GetRegister(regS);
            var b = cpu.memory.ReadByte(reg.HL);
            reg.SetRegister(regS, b);
            cpu.memory.WriteByte(reg.HL, tr);

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }
        #endregion
        #region Data Processing

        internal static void ADDr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var z = (int)reg.GetRegister(regI);
            var sum = reg.A + z;
            fz(cpu, sum, false);
            if (sum > 255) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void ADDHL(CPU cpu) {
            var reg = cpu.reg;
            var z = (int) cpu.memory.ReadByte(reg.HL);
            var sum = reg.A + z;
            fz(cpu, sum, false);
            if (sum > 255) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        internal static void ADDn(CPU cpu) {
            var reg = cpu.reg;
            var z = (int) cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            var sum = reg.A + z;
            fz(cpu, sum, false);
            if (sum > 255) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        internal static void ADDHL(CPU cpu, string regA, string regB) {
            var reg = cpu.reg;
            var hl = reg.HL;
            var a = reg.GetRegister(regA);
            var b = reg.GetRegister(regB);

            var sum = hl + (a << 8) + b;
            
            if (sum > 65535) {
                reg.F |= 0x10;
            } else {
                reg.F &= 0xEF;
            }

            reg.H = (byte) ((sum >> 8) & 0xFF);
            reg.L = (byte) (sum & 0xFF);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }
        
        internal static void ADDHLSP(CPU cpu) {
            var reg = cpu.reg;
            var hl = (int) reg.HL;
            var sum = hl + reg.SP;
            
            if (sum > 65535) {
                reg.F |= 0x10;
            } else {
                reg.F &= 0xEF;
            }

            reg.H = (byte) ((sum >> 8) & 0xFF);
            reg.L = (byte) (sum & 0xFF);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void ADDSPn(CPU cpu) {
            var reg = cpu.reg;
            var a = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            if (a > 127) {
                a = -((~a + 1) & 0xFF);
            }

            reg.SP = (ushort) (reg.SP + a);
            
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }
        
        #endregion
        #region Interrupt Calls
        internal static void RSTXX(CPU cpu, ushort addr) {
            var reg = cpu.reg;
            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, reg.PC);
            reg.PC = addr;

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }
        #endregion
        #region Stack Management

        internal static void PUSH(CPU cpu, string regA, string regB) {
            var reg = cpu.reg;
            reg.SP--;
            var b = reg.GetRegister(regA);
            cpu.memory.WriteByte(reg.SP, b);
            reg.SP--;
            b = reg.GetRegister(regB);
            cpu.memory.WriteByte(reg.SP, b);
            
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void POP(CPU cpu, string regA, string regB) {
            var reg = cpu.reg;
            reg.SetRegister(regA, cpu.memory.ReadByte(reg.SP));
            reg.SP++;
            reg.SetRegister(regB, cpu.memory.ReadByte(reg.SP));
            reg.SP++;
            
            reg.lastClockM = 3;
            reg.lastClockT = 12;

        }
        #endregion
        #region Jumps

        internal static void RET(CPU cpu) {
            var reg = cpu.reg;
            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void RETI(CPU cpu) {
            var reg = cpu.reg;
            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.ime = 1;

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void RETNZ(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x80) != 0x80) {
                reg.lastClockM = 1;
                reg.lastClockT = 4;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void RETNC(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x10) != 0x00) {
                reg.lastClockM = 1;
                reg.lastClockT = 4;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        internal static void RETC(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x10) != 0x10) {
                reg.lastClockM = 1;
                reg.lastClockT = 4;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void RETZ(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x80) != 0x00) {
                reg.lastClockM = 1;
                reg.lastClockT = 4;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        internal static void DI(CPU cpu) {
            var reg = cpu.reg;
            reg.ime = 0;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        internal static void EI(CPU cpu) {
            var reg = cpu.reg;
            reg.ime = 1;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        internal static void NOP(CPU cpu) {
            var reg = cpu.reg;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        internal static void HALT(CPU cpu) {
            var reg = cpu.reg;
            cpu._halt = true;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        #endregion
        #region Helper Functions
        internal static void fz(CPU cpu, int v, bool a) {
            var reg = cpu.reg;
            reg.F = 0;
            if (v != 0) {
                reg.F |= 128;
            }

            reg.F |= (byte) (a ? 0x40 : 0x00);
        }
        #endregion
    }
}
