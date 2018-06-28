using System;
using System.Collections.Generic;

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
            var b = reg.GetRegister(regI);
            cpu.memory.WriteByte(reg.HL, b);
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
            var b = cpu.memory.ReadByte(reg.HL);
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

        internal static void LDHLDA(CPU cpu) {
            var reg = cpu.reg;
            cpu.memory.WriteByte(reg.HL, reg.A);
            var v = reg.L - 1;
            reg.L = (byte) (v & 0xFF);
            if (v < 0) {
                reg.H--;
            }

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void LDAHLD(CPU cpu) {
            var reg = cpu.reg;
            reg.A = cpu.memory.ReadByte(reg.HL);
            var v = reg.L - 1;
            reg.L = (byte) (v & 0xFF);
            if (v < 0) {
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

        internal static void LDAIOC(CPU cpu) {
            var reg = cpu.reg;
            var addr = cpu.memory.ReadByte(reg.C) + 0xFF00;
            var b = cpu.memory.ReadByte(addr);
            reg.A = b;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void LDIOCA(CPU cpu) {
            var reg = cpu.reg;
            cpu.memory.WriteByte(0xFF00 + reg.C, reg.A);
            
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
            ResetFlag(cpu, sum, false);
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
            ResetFlag(cpu, sum, false);
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
            ResetFlag(cpu, sum, false);
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

        internal static void ADCr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = (int) reg.GetRegister(regI);
            var sum = reg.A + b + ((reg.F & 0x10) == 0x10 ? 1 : 0);
            ResetFlag(cpu, sum, false);
            if (sum > 255) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        internal static void ADCHL(CPU cpu) {
            var reg = cpu.reg;
            var b = (int) cpu.memory.ReadByte(reg.HL);
            var sum = reg.A + b + ((reg.F & 0x10) == 0x10 ? 1 : 0);
            ResetFlag(cpu, sum, false);
            if (sum > 255) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        internal static void ADCn(CPU cpu) {
            var reg = cpu.reg;
            var b = (int) cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            var sum = reg.A + b + ((reg.F & 0x10) == 0x10 ? 1 : 0);
            ResetFlag(cpu, sum, false);
            if (sum > 255) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        internal static void SUBr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = (int) reg.GetRegister(regI);
            var sum = reg.A - b;
            ResetFlag(cpu, sum, true);
            if (sum < 0) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void SUBHL(CPU cpu) {
            var reg = cpu.reg;
            var z = (int) cpu.memory.ReadByte(reg.HL);
            var sum = reg.A - z;
            ResetFlag(cpu, sum, true);
            if (sum < 0) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        internal static void SUBn(CPU cpu) {
            var reg = cpu.reg;
            var z = (int) cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            var sum = reg.A - z;
            ResetFlag(cpu, sum, true);
            if (sum > 255) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void SBCr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = (int) reg.GetRegister(regI);
            var sum = reg.A - b - ((reg.F & 0x10) == 0x10 ? 1 : 0);
            ResetFlag(cpu, sum, true);
            if (sum < 0) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void SBCHL(CPU cpu) {
            var reg = cpu.reg;
            var b = (int) cpu.memory.ReadByte(reg.HL);
            var sum = reg.A - b - ((reg.F & 0x10) == 0x10 ? 1 : 0);
            ResetFlag(cpu, sum, true);
            if (sum < 0) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void SBCn(CPU cpu) {
            var reg = cpu.reg;
            var b = (int) cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            var sum = reg.A - b - ((reg.F & 0x10) == 0x10 ? 1 : 0);
            ResetFlag(cpu, sum, true);
            if (sum < 0) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (sum & 0xFF);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        internal static void CPr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var a = (int) reg.A;
            a -= reg.GetRegister(regI);
            ResetFlag(cpu, a, true);

            if (a < 0) {
                reg.F |= 0x10;
            }
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        internal static void CPHL(CPU cpu) {
            var reg = cpu.reg;
            var a = (int) reg.A;
            a -= cpu.memory.ReadByte(reg.HL);
            ResetFlag(cpu, a, true);

            if (a < 0) {
                reg.F |= 0x10;
            }
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void CPn(CPU cpu) {
            var reg = cpu.reg;
            var a = (int) reg.A;
            a -= cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            ResetFlag(cpu, a, true);

            if (a < 0) {
                reg.F |= 0x10;
            }
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void ANDr(CPU cpu, string regI) {
            var reg = cpu.reg;
            reg.A &= reg.GetRegister(regI);
            ResetFlag(cpu, reg.A, false);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void ANDHL(CPU cpu) {
            var reg = cpu.reg;
            reg.A &= cpu.memory.ReadByte(reg.HL);
            ResetFlag(cpu, reg.A, false);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        internal static void ANDn(CPU cpu) {
            var reg = cpu.reg;
            reg.A &= cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            ResetFlag(cpu, reg.A, false);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void ORr(CPU cpu, string regI) {
            var reg = cpu.reg;
            reg.A |= reg.GetRegister(regI);
            ResetFlag(cpu, reg.A, false);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void ORHL(CPU cpu) {
            var reg = cpu.reg;
            reg.A |= cpu.memory.ReadByte(reg.HL);
            ResetFlag(cpu, reg.A, false);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        internal static void ORn(CPU cpu) {
            var reg = cpu.reg;
            reg.A |= cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            ResetFlag(cpu, reg.A, false);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        internal static void XORr(CPU cpu, string regI) {
            var reg = cpu.reg;
            reg.A ^= reg.GetRegister(regI);
            ResetFlag(cpu, reg.A, false);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void XORHL(CPU cpu) {
            var reg = cpu.reg;
            reg.A ^= cpu.memory.ReadByte(reg.HL);
            ResetFlag(cpu, reg.A, false);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        internal static void XORn(CPU cpu) {
            var reg = cpu.reg;
            reg.A ^= cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            ResetFlag(cpu, reg.A, false);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void INCr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var v = reg.GetRegister(regI) + 1; 
            ResetFlag(cpu, v, false);
            reg.SetRegister(regI, (byte) (v & 0xFF));
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void INCHLm(CPU cpu) {
            var reg = cpu.reg;
            var v = cpu.memory.ReadByte(reg.HL) + 1;
            ResetFlag(cpu, v, false);
            cpu.memory.WriteByte(reg.HL, (byte) (v & 0xFF));
            
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }
        
        internal static void DECr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var v = reg.GetRegister(regI) - 1; 
            ResetFlag(cpu, v, true);
            reg.SetRegister(regI, (byte) (v & 0xFF));
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void DECHLm(CPU cpu) {
            var reg = cpu.reg;
            var v = cpu.memory.ReadByte(reg.HL) - 1;
            ResetFlag(cpu, v, true);
            cpu.memory.WriteByte(reg.HL, (byte) (v & 0xFF));
            
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }
        
        internal static void INC(CPU cpu, string regA, string regB) {
            var reg = cpu.reg;
            var v = reg.GetRegister(regB) + 1; 
            reg.SetRegister(regB, (byte) (v & 0xFF));

            if ((v & 0xFF) == 0) {
                reg.SetRegister(regA, (byte) ((reg.GetRegister(regA) + 1) & 0xFF));
            }
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void INCHL(CPU cpu) {
            INC(cpu, "H", "L");
        }

        internal static void INCSP(CPU cpu) {
            var reg = cpu.reg;
            reg.SP++;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        internal static void DEC(CPU cpu, string regA, string regB) {
            var reg = cpu.reg;
            var v = reg.GetRegister(regB) - 1;
            reg.SetRegister(regB, (byte)(v & 0xFF));

            if (v < 0) {
                reg.SetRegister(regA, (byte) ((reg.GetRegister(regA) - 1) & 0xFF));
            }
        }
        
        internal static void DECHL(CPU cpu) {
            DEC(cpu, "H", "L");
        }
        
        internal static void DECSP(CPU cpu) {
            var reg = cpu.reg;
            reg.SP--;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        #endregion
        #region Bit Manipulation

        internal static void RLA(CPU cpu) {
            var reg = cpu.reg;
            var ci = (reg.F & 0x10) != 0x00 ? 0x01 : 0x00;
            var co = (reg.A & 0x80) != 0x00 ? 0x10 : 0x00;

            reg.A = (byte) ((reg.A << 1) + ci);
            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }  
        internal static void RLCA(CPU cpu) {
            var reg = cpu.reg;
            var ci = (reg.A & 0x80) != 0x00 ? 0x01 : 0x00;
            var co = (reg.A & 0x80) != 0x00 ? 0x10 : 0x00;

            reg.A = (byte) ((reg.A << 1) + ci);
            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        internal static void RRA(CPU cpu) {
            var reg = cpu.reg;
            var ci = (reg.F & 0x10) != 0 ? 0x80 : 0x00;
            var co = (reg.A & 0x01) != 0 ? 0x10 : 0x00;

            reg.A = (byte) ((reg.A >> 1) + ci);
            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        internal static void RRCA(CPU cpu) {
            var reg = cpu.reg;
            var ci = (reg.A & 0x01) != 0 ? 0x80 : 0x00;
            var co = (reg.A & 0x01) != 0 ? 0x10 : 0x00;

            reg.A = (byte) ((reg.A >> 1) + ci);
            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        internal static void CPL(CPU cpu) {
            var reg = cpu.reg;
            reg.A = (byte) ((~reg.A) & 0xFF);
            ResetFlag(cpu, reg.A, true);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        internal static void NEG(CPU cpu) {
            var reg = cpu.reg;

            var v = 0 - reg.A;
            ResetFlag(cpu, v, true);

            if (v < 0) {
                reg.F |= 0x10;
            }

            reg.A = (byte) (v & 0xFF);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        internal static void CCF(CPU cpu) {
            var reg = cpu.reg;
            var ci = (reg.F & 0x10) != 0x00 ? 0x10 : 0x00;

            reg.F = (byte) ((reg.F & 0xEF) + ci);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        internal static void SCF(CPU cpu) {
            var reg = cpu.reg;
            reg.F |= 0x10;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        #endregion
        #region Interrupt Calls
        internal static void RSTXX(CPU cpu, ushort addr) {
            var reg = cpu.reg;
            reg.SaveRegs();
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

        internal static void JPnn(CPU cpu) {
            var reg = cpu.reg;
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void JPHL(CPU cpu) {
            var reg = cpu.reg;
            reg.PC = reg.HL;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        
        internal static void JPNZnn(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x80) != 0x00) {
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                reg.PC += 2;
                return;
            }
            
            reg.PC = cpu.memory.ReadWord(reg.PC);
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }
        
        internal static void JPZnn(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x80) != 0x80) {
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                reg.PC += 2;
                return;
            }
            
            reg.PC = cpu.memory.ReadWord(reg.PC);
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }
        
        internal static void JPNCnn(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x10) != 0x00) {
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                reg.PC += 2;
                return;
            }
            
            reg.PC = cpu.memory.ReadWord(reg.PC);
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }
        
        internal static void JPCnn(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x10) != 0x10) {
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                reg.PC += 2;
                return;
            }
            
            reg.PC = cpu.memory.ReadWord(reg.PC);
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        internal static void JRn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            reg.PC = (ushort)((reg.PC + v) & 0xFFFF);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void JRNZn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            if ((reg.F & 0x80) != 0x00) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }
            
            reg.PC = (ushort)((reg.PC + v) & 0xFFFF);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void JRZn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            if ((reg.F & 0x80) != 0x80) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }
            
            reg.PC = (ushort)((reg.PC + v) & 0xFFFF);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }
        
        internal static void JRNCn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            if ((reg.F & 0x10) != 0x00) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }
            
            reg.PC = (ushort)((reg.PC + v) & 0xFFFF);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void JRCn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            if ((reg.F & 0x10) != 0x10) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }
            
            reg.PC = (ushort)((reg.PC + v) & 0xFFFF);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void DJNZn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            reg.B--;

            if (reg.B == 0x00) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }
            
            reg.PC = (ushort)((reg.PC + v) & 0xFFFF);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void CALLnn(CPU cpu) {
            var reg = cpu.reg;
            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, (ushort) (reg.PC + 2));
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        internal static void CALLNZnn(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x80) != 0x00) {
                reg.PC += 2;
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                return;
            }

            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, (ushort) (reg.PC + 2));
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        internal static void CALLZnn(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x80) != 0x80) {
                reg.PC += 2;
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                return;
            }

            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, (ushort) (reg.PC + 2));
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        internal static void CALLNCnn(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x10) != 0x00) {
                reg.PC += 2;
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                return;
            }

            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, (ushort) (reg.PC + 2));
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        internal static void CALLCnn(CPU cpu) {
            var reg = cpu.reg;

            if ((reg.F & 0x10) != 0x10) {
                reg.PC += 2;
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                return;
            }

            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, (ushort) (reg.PC + 2));
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }
        
        internal static void RET(CPU cpu) {
            var reg = cpu.reg;
            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        internal static void RETI(CPU cpu) {
            var reg = cpu.reg;
            reg.LoadRegs();
            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.InterruptEnable = 1;

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
            reg.InterruptEnable = 0;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        internal static void EI(CPU cpu) {
            var reg = cpu.reg;
            reg.InterruptEnable = 1;
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
        internal static void ResetFlag(CPU cpu, int v, bool isUnderflow) {
            var reg = cpu.reg;
            reg.F = 0;
            if (v != 0) {
                reg.F |= 128;
            }

            reg.F |= (byte) (isUnderflow ? 0x40 : 0x00);
        }
        #endregion
        #region 0xCB Calls

        #region Call Implementation
        static void RLr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var v = (int) reg.GetRegister(regI);
            var ci = (reg.F & 0x10) != 0 ? 0x01 : 0x00;
            var co = (v & 0x80) != 0 ? 0x10 : 0x00;
            v = (((v << 1) + ci) & 0xFF);
            reg.SetRegister(regI, (byte) v);
            ResetFlag(cpu, v, false);

            reg.F = (byte) (((reg.F & 0xEF) + co) & 0xFF);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void RLHL(CPU cpu) {
            var reg = cpu.reg;
            var v = (int) cpu.memory.ReadByte(reg.HL);
            var ci = (reg.F & 0x10) != 0 ? 0x01 : 0x00;
            var co = (v & 0x80) != 0 ? 0x10 : 0x00;
            v = (((v << 1) + ci) & 0xFF);
            cpu.memory.WriteByte(reg.HL, (byte) v);
            ResetFlag(cpu, v, false);

            reg.F = (byte) (((reg.F & 0xEF) + co) & 0xFF);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        static void RLCr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var v = (int) reg.GetRegister(regI);
            var ci = (v & 0x80) != 0 ? 0x01 : 0x00;
            var co = (v & 0x80) != 0 ? 0x80 : 0x00;

            v = (v << 1) + ci;
            reg.SetRegister(regI, (byte)(v & 0xFF));
            ResetFlag(cpu, v & 0xFF, false);

            reg.F = (byte) ((reg.F & 0xEF) + co);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void RLCHL(CPU cpu) {
            var reg = cpu.reg;
            var v = (int) cpu.memory.ReadByte(reg.HL);
            var ci = (v & 0x80) != 0 ? 0x01 : 0x00;
            var co = (v & 0x80) != 0 ? 0x80 : 0x00;

            v = (v << 1) + ci;
            cpu.memory.WriteByte(reg.HL, (byte) (v & 0xFF));
            ResetFlag(cpu, v & 0xFF, false);

            reg.F = (byte) ((reg.F & 0xEF) + co);

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void RRr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var ci = (reg.F & 0x10) != 0x00 ? 0x80 : 0x00;
            var co = (b & 0x01) != 0x00 ? 0x10 : 0x00;

            b = (byte) (((b >> 1) + ci) & 0xFF);
            ResetFlag(cpu, b, false);

            reg.SetRegister(regI, b);

            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void RRHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var ci = (reg.F & 0x10) != 0x00 ? 0x80 : 0x00;
            var co = (b & 0x01) != 0x00 ? 0x10 : 0x00;

            b = (byte) (((b >> 1) + ci) & 0xFF);
            ResetFlag(cpu, b, false);

            cpu.memory.WriteByte(reg.HL, b);

            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }
        
        static void RRCr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var ci = (b & 0x01) != 0 ? 0x80 : 0x00;
            var co = (b & 0x01) != 0 ? 0x10 : 0x00;
            
            b = (byte) (((b >> 1) + ci) & 0xFF);
            ResetFlag(cpu, b, false);
            reg.SetRegister(regI, b);

            reg.F = (byte) ((reg.F & 0xEF) + co);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        static void RRCHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var ci = (b & 0x01) != 0 ? 0x80 : 0x00;
            var co = (b & 0x01) != 0 ? 0x10 : 0x00;
            
            b = (byte) (((b >> 1) + ci) & 0xFF);
            ResetFlag(cpu, b, false);
            cpu.memory.WriteByte(reg.HL, b);

            reg.F = (byte) ((reg.F & 0xEF) + co);
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void SLAr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var co = (b & 0x80) != 0x00 ? 0x10 : 0x00;
            b = (byte) ((b << 1) & 0xFF);
            ResetFlag(cpu, b, false);
            
            reg.SetRegister(regI, b);

            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void SLAHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var co = (b & 0x80) != 0x00 ? 0x10 : 0x00;
            b = (byte) ((b << 1) & 0xFF);
            ResetFlag(cpu, b, false);

            cpu.memory.WriteByte(reg.HL, b);
            
            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void SRAr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var co = (b & 0x80) != 0x00 ? 0x80 : 0x00;
            var ci = (b & 0x01) != 0x00 ? 0x10 : 0x00;
            b = (byte) (((b >> 1) + ci) & 0xFF);
            ResetFlag(cpu, b, false);
            
            reg.SetRegister(regI, b);

            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void SRAHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var co = (b & 0x80) != 0x00 ? 0x80 : 0x00;
            var ci = (b & 0x01) != 0x00 ? 0x10 : 0x00;
            b = (byte) (((b >> 1) + ci) & 0xFF);
            ResetFlag(cpu, b, false);

            cpu.memory.WriteByte(reg.HL, b);
            
            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void SWAPr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var swapped = ((b & 0x0F) << 4) | ((b & 0xF0) >> 4);
            reg.SetRegister(regI, (byte) swapped);
            
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        static void SWAPHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var swapped = ((b & 0x0F) << 4) | ((b & 0xF0) >> 4);
            cpu.memory.WriteByte(reg.HL, (byte) swapped);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void SRLr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var co = (b & 0x01) != 0 ? 0x10 : 0x00;
            b = (byte) ((b >> 1) & 0xFF);
            reg.SetRegister(regI, b);
            ResetFlag(cpu, b, false);
            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void SRLHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var co = (b & 0x01) != 0 ? 0x10 : 0x00;
            b = (byte) ((b >> 1) & 0xFF);
            cpu.memory.WriteByte(reg.HL, b);
            ResetFlag(cpu, b, false);
            reg.F = (byte) ((reg.F & 0xEF) + co);
            
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void BIT(CPU cpu, int n, string regI) {
            var reg = cpu.reg;
            ResetFlag(cpu, reg.GetRegister(regI) & (1 << n), false);
            
            reg.lastClockM = 2;
            reg.lastClockT = 4;
        }
        
        static void BITm(CPU cpu, int n) {
            var reg = cpu.reg;
            ResetFlag(cpu, cpu.memory.ReadByte(reg.HL) & n, false);
            
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        static void RES(CPU cpu, int n, string regI) {
            var reg = cpu.reg;

            var b = reg.GetRegister(regI);
            b &= (byte) (~(1 << n));
            reg.SetRegister(regI, b);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        static void RESHL(CPU cpu, int n) {
            var reg = cpu.reg;

            var b = cpu.memory.ReadByte(reg.HL);
            b &= (byte) (~(1 << n));
            cpu.memory.WriteByte(reg.HL, b);
            
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        static void SET(CPU cpu, int n, string regI) {
            var reg = cpu.reg;

            var b = reg.GetRegister(regI);
            b |= (byte) (1 << n);
            reg.SetRegister(regI, b);
            
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        
        static void SETHL(CPU cpu, int n) {
            var reg = cpu.reg;

            var b = cpu.memory.ReadByte(reg.HL);
            b |= (byte) (1 << n);
            cpu.memory.WriteByte(reg.HL, b);
            
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }
        
        #endregion
        
        private static readonly List<Action<CPU>> CBOPS = new List<Action<CPU>> {
            #region CB00 Group
            (cpu) => RLCr(cpu, "B"),
            (cpu) => RLCr(cpu, "C"),
            (cpu) => RLCr(cpu, "D"),
            (cpu) => RLCr(cpu, "E"),
            (cpu) => RLCr(cpu, "H"),
            (cpu) => RLCr(cpu, "L"),
            (cpu) => RLCHL(cpu),
            (cpu) => RLCr(cpu, "A"),
            (cpu) => RRCr(cpu, "B"),
            (cpu) => RRCr(cpu, "C"),
            (cpu) => RRCr(cpu, "D"),
            (cpu) => RRCr(cpu, "E"),
            (cpu) => RRCr(cpu, "H"),
            (cpu) => RRCr(cpu, "L"),
            (cpu) => RRCHL(cpu),
            (cpu) => RRCr(cpu, "A"),
            #endregion
            #region CB10 Group
            (cpu) => RLr(cpu, "B"),
            (cpu) => RLr(cpu, "C"),
            (cpu) => RLr(cpu, "D"),
            (cpu) => RLr(cpu, "E"),
            (cpu) => RLr(cpu, "H"),
            (cpu) => RLr(cpu, "L"),
            (cpu) => RLHL(cpu),
            (cpu) => RLr(cpu, "A"),
            (cpu) => RRr(cpu, "B"),
            (cpu) => RRr(cpu, "C"),
            (cpu) => RRr(cpu, "D"),
            (cpu) => RRr(cpu, "E"),
            (cpu) => RRr(cpu, "H"),
            (cpu) => RRr(cpu, "L"),
            (cpu) => RRHL(cpu),
            (cpu) => RRr(cpu, "A"),
            #endregion
            #region CB20 Group
            (cpu) => SLAr(cpu, "B"),
            (cpu) => SLAr(cpu, "C"),
            (cpu) => SLAr(cpu, "D"),
            (cpu) => SLAr(cpu, "E"),
            (cpu) => SLAr(cpu, "H"),
            (cpu) => SLAr(cpu, "L"),
            (cpu) => SLAHL(cpu),
            (cpu) => SLAr(cpu, "A"),
            (cpu) => SRAr(cpu, "B"),
            (cpu) => SRAr(cpu, "C"),
            (cpu) => SRAr(cpu, "D"),
            (cpu) => SRAr(cpu, "E"),
            (cpu) => SRAr(cpu, "H"),
            (cpu) => SRAr(cpu, "L"),
            (cpu) => SRAHL(cpu),
            (cpu) => SRAr(cpu, "A"),
            #endregion
            #region CB30 Group
            (cpu) => SWAPr(cpu, "B"),
            (cpu) => SWAPr(cpu, "C"),
            (cpu) => SWAPr(cpu, "D"),
            (cpu) => SWAPr(cpu, "E"),
            (cpu) => SWAPr(cpu, "H"),
            (cpu) => SWAPr(cpu, "L"),
            (cpu) => SWAPHL(cpu),
            (cpu) => SWAPr(cpu, "A"),
            (cpu) => SRLr(cpu, "B"),
            (cpu) => SRLr(cpu, "C"),
            (cpu) => SRLr(cpu, "D"),
            (cpu) => SRLr(cpu, "E"),
            (cpu) => SRLr(cpu, "H"),
            (cpu) => SRLr(cpu, "L"),
            (cpu) => SRLHL(cpu),
            (cpu) => SRLr(cpu, "A"),
            #endregion
            #region CB40 Group
            (cpu) => BIT(cpu, 0, "B"),
            (cpu) => BIT(cpu, 0, "C"),
            (cpu) => BIT(cpu, 0, "D"),
            (cpu) => BIT(cpu, 0, "E"),
            (cpu) => BIT(cpu, 0, "H"),
            (cpu) => BIT(cpu, 0, "L"),
            (cpu) => BITm(cpu, 0),
            (cpu) => BIT(cpu, 0, "A"),
            (cpu) => BIT(cpu, 1, "B"),
            (cpu) => BIT(cpu, 1, "C"),
            (cpu) => BIT(cpu, 1, "D"),
            (cpu) => BIT(cpu, 1, "E"),
            (cpu) => BIT(cpu, 1, "H"),
            (cpu) => BIT(cpu, 1, "L"),
            (cpu) => BITm(cpu, 1),
            (cpu) => BIT(cpu, 1, "A"),
            #endregion
            #region CB50 Group
            (cpu) => BIT(cpu, 2, "B"),
            (cpu) => BIT(cpu, 2, "C"),
            (cpu) => BIT(cpu, 2, "D"),
            (cpu) => BIT(cpu, 2, "E"),
            (cpu) => BIT(cpu, 2, "H"),
            (cpu) => BIT(cpu, 2, "L"),
            (cpu) => BITm(cpu, 2),
            (cpu) => BIT(cpu, 2, "A"),
            (cpu) => BIT(cpu, 3, "B"),
            (cpu) => BIT(cpu, 3, "C"),
            (cpu) => BIT(cpu, 3, "D"),
            (cpu) => BIT(cpu, 3, "E"),
            (cpu) => BIT(cpu, 3, "H"),
            (cpu) => BIT(cpu, 3, "L"),
            (cpu) => BITm(cpu, 3),
            (cpu) => BIT(cpu, 3, "A"),
            #endregion
            #region CB60 Group
            (cpu) => BIT(cpu, 4, "B"),
            (cpu) => BIT(cpu, 4, "C"),
            (cpu) => BIT(cpu, 4, "D"),
            (cpu) => BIT(cpu, 4, "E"),
            (cpu) => BIT(cpu, 4, "H"),
            (cpu) => BIT(cpu, 4, "L"),
            (cpu) => BITm(cpu, 4),
            (cpu) => BIT(cpu, 4, "A"),
            (cpu) => BIT(cpu, 5, "B"),
            (cpu) => BIT(cpu, 5, "C"),
            (cpu) => BIT(cpu, 5, "D"),
            (cpu) => BIT(cpu, 5, "E"),
            (cpu) => BIT(cpu, 5, "H"),
            (cpu) => BIT(cpu, 5, "L"),
            (cpu) => BITm(cpu, 5),
            (cpu) => BIT(cpu, 5, "A"),
            #endregion
            #region CB70 Group
            (cpu) => BIT(cpu, 6, "B"),
            (cpu) => BIT(cpu, 6, "C"),
            (cpu) => BIT(cpu, 6, "D"),
            (cpu) => BIT(cpu, 6, "E"),
            (cpu) => BIT(cpu, 6, "H"),
            (cpu) => BIT(cpu, 6, "L"),
            (cpu) => BITm(cpu, 6),
            (cpu) => BIT(cpu, 6, "A"),
            (cpu) => BIT(cpu, 7, "B"),
            (cpu) => BIT(cpu, 7, "C"),
            (cpu) => BIT(cpu, 7, "D"),
            (cpu) => BIT(cpu, 7, "E"),
            (cpu) => BIT(cpu, 7, "H"),
            (cpu) => BIT(cpu, 7, "L"),
            (cpu) => BITm(cpu, 7),
            (cpu) => BIT(cpu, 7, "A"),
            #endregion
            #region CB80 Group
            (cpu) => RES(cpu, 0, "B"),
            (cpu) => RES(cpu, 0, "C"),
            (cpu) => RES(cpu, 0, "D"),
            (cpu) => RES(cpu, 0, "E"),
            (cpu) => RES(cpu, 0, "H"),
            (cpu) => RES(cpu, 0, "L"),
            (cpu) => RESHL(cpu, 0),
            (cpu) => RES(cpu, 0, "A"),
            (cpu) => RES(cpu, 1, "B"),
            (cpu) => RES(cpu, 1, "C"),
            (cpu) => RES(cpu, 1, "D"),
            (cpu) => RES(cpu, 1, "E"),
            (cpu) => RES(cpu, 1, "H"),
            (cpu) => RES(cpu, 1, "L"),
            (cpu) => RESHL(cpu, 1),
            (cpu) => RES(cpu, 1, "A"),
            #endregion
            #region CB90 Group
            (cpu) => RES(cpu, 2, "B"),
            (cpu) => RES(cpu, 2, "C"),
            (cpu) => RES(cpu, 2, "D"),
            (cpu) => RES(cpu, 2, "E"),
            (cpu) => RES(cpu, 2, "H"),
            (cpu) => RES(cpu, 2, "L"),
            (cpu) => RESHL(cpu, 2),
            (cpu) => RES(cpu, 2, "A"),
            (cpu) => RES(cpu, 3, "B"),
            (cpu) => RES(cpu, 3, "C"),
            (cpu) => RES(cpu, 3, "D"),
            (cpu) => RES(cpu, 3, "E"),
            (cpu) => RES(cpu, 3, "H"),
            (cpu) => RES(cpu, 3, "L"),
            (cpu) => RESHL(cpu, 3),
            (cpu) => RES(cpu, 3, "A"),
            #endregion
            #region CBA0 Group
            (cpu) => RES(cpu, 4, "B"),
            (cpu) => RES(cpu, 4, "C"),
            (cpu) => RES(cpu, 4, "D"),
            (cpu) => RES(cpu, 4, "E"),
            (cpu) => RES(cpu, 4, "H"),
            (cpu) => RES(cpu, 4, "L"),
            (cpu) => RESHL(cpu, 4),
            (cpu) => RES(cpu, 4, "A"),
            (cpu) => RES(cpu, 6, "B"),
            (cpu) => RES(cpu, 6, "C"),
            (cpu) => RES(cpu, 6, "D"),
            (cpu) => RES(cpu, 6, "E"),
            (cpu) => RES(cpu, 6, "H"),
            (cpu) => RES(cpu, 6, "L"),
            (cpu) => RESHL(cpu, 6),
            (cpu) => RES(cpu, 6, "A"),
            #endregion
            #region CBB0 Group
            (cpu) => RES(cpu, 6, "B"),
            (cpu) => RES(cpu, 6, "C"),
            (cpu) => RES(cpu, 6, "D"),
            (cpu) => RES(cpu, 6, "E"),
            (cpu) => RES(cpu, 6, "H"),
            (cpu) => RES(cpu, 6, "L"),
            (cpu) => RESHL(cpu, 6),
            (cpu) => RES(cpu, 6, "A"),
            (cpu) => RES(cpu, 7, "B"),
            (cpu) => RES(cpu, 7, "C"),
            (cpu) => RES(cpu, 7, "D"),
            (cpu) => RES(cpu, 7, "E"),
            (cpu) => RES(cpu, 7, "H"),
            (cpu) => RES(cpu, 7, "L"),
            (cpu) => RESHL(cpu, 7),
            (cpu) => RES(cpu, 7, "A"),
            #endregion
            #region CBC0 Group
            (cpu) => SET(cpu, 0, "B"),
            (cpu) => SET(cpu, 0, "C"),
            (cpu) => SET(cpu, 0, "D"),
            (cpu) => SET(cpu, 0, "E"),
            (cpu) => SET(cpu, 0, "H"),
            (cpu) => SET(cpu, 0, "L"),
            (cpu) => SETHL(cpu, 0),
            (cpu) => SET(cpu, 0, "A"),
            (cpu) => SET(cpu, 1, "B"),
            (cpu) => SET(cpu, 1, "C"),
            (cpu) => SET(cpu, 1, "D"),
            (cpu) => SET(cpu, 1, "E"),
            (cpu) => SET(cpu, 1, "H"),
            (cpu) => SET(cpu, 1, "L"),
            (cpu) => SETHL(cpu, 1),
            (cpu) => SET(cpu, 1, "A"),
            #endregion
            #region CBD0 Group
            (cpu) => SET(cpu, 2, "B"),
            (cpu) => SET(cpu, 2, "C"),
            (cpu) => SET(cpu, 2, "D"),
            (cpu) => SET(cpu, 2, "E"),
            (cpu) => SET(cpu, 2, "H"),
            (cpu) => SET(cpu, 2, "L"),
            (cpu) => SETHL(cpu, 2),
            (cpu) => SET(cpu, 2, "A"),
            (cpu) => SET(cpu, 3, "B"),
            (cpu) => SET(cpu, 3, "C"),
            (cpu) => SET(cpu, 3, "D"),
            (cpu) => SET(cpu, 3, "E"),
            (cpu) => SET(cpu, 3, "H"),
            (cpu) => SET(cpu, 3, "L"),
            (cpu) => SETHL(cpu, 3),
            (cpu) => SET(cpu, 3, "A"),
            #endregion
            #region CBE0 Group
            (cpu) => SET(cpu, 4, "B"),
            (cpu) => SET(cpu, 4, "C"),
            (cpu) => SET(cpu, 4, "D"),
            (cpu) => SET(cpu, 4, "E"),
            (cpu) => SET(cpu, 4, "H"),
            (cpu) => SET(cpu, 4, "L"),
            (cpu) => SETHL(cpu, 4),
            (cpu) => SET(cpu, 4, "A"),
            (cpu) => SET(cpu, 5, "B"),
            (cpu) => SET(cpu, 5, "C"),
            (cpu) => SET(cpu, 5, "D"),
            (cpu) => SET(cpu, 5, "E"),
            (cpu) => SET(cpu, 5, "H"),
            (cpu) => SET(cpu, 5, "L"),
            (cpu) => SETHL(cpu, 5),
            (cpu) => SET(cpu, 5, "A"),
            #endregion
            #region CBF0 Group
            (cpu) => SET(cpu, 6, "B"),
            (cpu) => SET(cpu, 6, "C"),
            (cpu) => SET(cpu, 6, "D"),
            (cpu) => SET(cpu, 6, "E"),
            (cpu) => SET(cpu, 6, "H"),
            (cpu) => SET(cpu, 6, "L"),
            (cpu) => SETHL(cpu, 6),
            (cpu) => SET(cpu, 6, "A"),
            (cpu) => SET(cpu, 7, "B"),
            (cpu) => SET(cpu, 7, "C"),
            (cpu) => SET(cpu, 7, "D"),
            (cpu) => SET(cpu, 7, "E"),
            (cpu) => SET(cpu, 7, "H"),
            (cpu) => SET(cpu, 7, "L"),
            (cpu) => SETHL(cpu, 7),
            (cpu) => SET(cpu, 7, "A")
            #endregion
        };
        internal static void CBCall(CPU cpu) {
            var reg = cpu.reg;
            var v = cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            CBOPS[v](cpu);
        }
        #endregion
    }
}
