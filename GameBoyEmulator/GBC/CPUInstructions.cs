﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OpenGL;

namespace GameBoyEmulator.Desktop.GBC {
    public static class CPUInstructions {
        #region Load / Store Instructions
        /// <summary>
        /// Set regI = regO
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regI"></param>
        /// <param name="regO"></param>
        private static void LDrr(CPU cpu, string regI, string regO) {
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
        private static void LDrHLm_(CPU cpu, string regO) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            reg.SetRegister(regO, b);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        /// <summary>
        /// Write regI to Memory Position (H/L)
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regI"></param>
        private static void LDHLmr_(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            cpu.memory.WriteByte(reg.HL, b);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        /// <summary>
        /// Loads the byte from Program Memory into regO. Increments Program Counter
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regO"></param>
        private static void LDrn_(CPU cpu, string regO) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            reg.SetRegister(regO, b);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
/*
       private static void LDHLmr_(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            cpu.memory.WriteByte(reg.HL, b);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
 */

        /// <summary>
        /// Writes byte from Program Memory into Memory (H/L). Increments Program Counter
        /// </summary>
        /// <param name="cpu"></param>
        private static void LDHLmn(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            cpu.memory.WriteByte(reg.HL, b);
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        /// <summary>
        /// Writes regI value into regH/regL memory.
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regH"></param>
        /// <param name="regL"></param>
        /// <param name="regI"></param>
        private static void LD__m_(CPU cpu, string regH, string regL, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var h = reg.GetRegister(regH);
            var l = reg.GetRegister(regL);
            var hl = (h << 8) + l;
            cpu.memory.WriteByte(hl, b);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        /// <summary>
        /// Stores value of regI in memory position of PC
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regI"></param>
        private static void LDmm_(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var addr = cpu.memory.ReadWord(reg.PC);
            reg.PC += 2;
            cpu.memory.WriteByte(addr, b);
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
        private static void LD___m(CPU cpu, string regO, string regH, string regL) {
            var reg = cpu.reg;
            var h = reg.GetRegister(regH);
            var l = reg.GetRegister(regL);
            var hl = (h << 8) + l;
            var b = cpu.memory.ReadByte(hl);
            reg.SetRegister(regO, b);
            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        /// <summary>
        /// Read from memory at word pointed by PC and stores at regO
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regO"></param>
        private static void LD_mm(CPU cpu, string regO) {
            var reg = cpu.reg;
            var addr = cpu.memory.ReadWord(reg.PC);
            var b = cpu.memory.ReadByte(addr);
            reg.SetRegister(regO, b);
            reg.PC += 2;
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        /// <summary>
        /// Reads from memory pointed by PC to regO1 and PC+1 to regO2
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="regO1"></param>
        /// <param name="regO2"></param>
        private static void LD__nn(CPU cpu, string regO1, string regO2) {
            var reg = cpu.reg;

            var b = cpu.memory.ReadByte(reg.PC);
            reg.SetRegister(regO2, b);
            reg.PC++;

            b = cpu.memory.ReadByte(reg.PC);
            reg.SetRegister(regO1, b);
            reg.PC++;

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        /// <summary>
        /// Reads word from Program Counter and stores in SP
        /// </summary>
        /// <param name="cpu"></param>
        private static void LDSPnn(CPU cpu) {
            var reg = cpu.reg;
            var u = cpu.memory.ReadWord(reg.PC);
            reg.PC += 2;
            reg.SP = u;
            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void LDmmSP(CPU cpu) {
            var reg = cpu.reg;
            var addr = cpu.memory.ReadWord(reg.PC);
            reg.PC += 2;
            cpu.memory.WriteWord(addr, reg.SP);

            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        /// <summary>
        /// Sets A to Memory at H/L and increments HL.
        /// </summary>
        /// <param name="cpu"></param>
        private static void LDHLIA(CPU cpu) {
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
        private static void LDAHLI(CPU cpu) {
            var reg = cpu.reg;
            reg.A = cpu.memory.ReadByte(reg.HL);
            reg.L++;
            if (reg.L == 0) {
                reg.H++;
            }

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void LDHLDA(CPU cpu) {
            var reg = cpu.reg;
            cpu.memory.WriteByte(reg.HL, reg.A);

            reg.L--;

            if (reg.L == 255) {
                reg.H--;
            }

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void LDAHLD(CPU cpu) {
            var reg = cpu.reg;
            reg.A = cpu.memory.ReadByte(reg.HL);
            reg.L--;
            if (reg.L == 255) {
                reg.H--;
            }

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void LDAIOn(CPU cpu) {
            var reg = cpu.reg;
            var addr = cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            reg.A = cpu.memory.ReadByte(0xFF00 + addr);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void LDIOnA(CPU cpu) {
            var reg = cpu.reg;
            var addr = cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            cpu.memory.WriteByte(0xFF00 + addr, reg.A);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void LDAIOC(CPU cpu) {
            var reg = cpu.reg;
            reg.A = cpu.memory.ReadByte(0xFF00 + reg.C);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void LDIOCA(CPU cpu) {
            var reg = cpu.reg;
            cpu.memory.WriteByte(0xFF00 + reg.C, reg.A);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void LDHLSPn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int) cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            reg.FlagZero = false;
            reg.FlagSub = false;
            reg.FlagHalfCarry = (reg.SP & 0xF) + (v & 0xF) > 0xF;
            reg.FlagCarry = (reg.SP & 0xFF) + (v & 0xFF) > 0xFF;

            v += reg.SP;
            reg.H = (byte) (v >> 8);
            reg.L = (byte) v;

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void LDHLSPr(CPU cpu) {
            var reg = cpu.reg;
            reg.SP = reg.HL;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }
        #endregion
        #region Data Processing

        private static void ADDr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var z = (int)reg.GetRegister(regI);
            var sum = reg.A + z;

            reg.FlagCarry = sum > 255;
            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = (reg.A & 0xF) + (z & 0xF) > 0xF;

            reg.A = (byte) sum;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void ADDHL(CPU cpu) {
            var reg = cpu.reg;
            var z = (int) cpu.memory.ReadByte(reg.HL);
            var sum = reg.A + z;

            reg.FlagCarry = sum > 255;
            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = ((reg.A & 0xF) + (z & 0xF)) > 0xF;

            reg.A = (byte) sum;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void ADDn(CPU cpu) {
            var reg = cpu.reg;
            var z = (int) cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            var sum = reg.A + z;

            reg.FlagCarry = sum > 255;
            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = ((reg.A & 0xF) + (z & 0xF)) > 0xF;

            reg.A = (byte) sum;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void ADDHL(CPU cpu, string regA, string regB) {
            var reg = cpu.reg;
            var hl = reg.HL;
            var a = reg.GetRegister(regA);
            var b = reg.GetRegister(regB);
            var ab = (a << 8) + b;
            var sum = hl + ab;

            reg.FlagCarry = sum > 65535;
            reg.FlagSub = false;
            reg.FlagHalfCarry = ((ab & 0xFFF) + (hl & 0xFFF)) > 0xFFF;

            reg.H = (byte) (sum >> 8);
            reg.L = (byte) sum;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void ADDHLSP(CPU cpu) {
            var reg = cpu.reg;
            var hl = (int) reg.HL;
            var sum = hl + reg.SP;

            reg.FlagCarry = sum > 65535;
            reg.FlagSub = false;
            reg.FlagHalfCarry = ((reg.SP & 0xFFF) + (hl & 0xFFF)) > 0xFFF;

            reg.H = (byte) (sum >> 8);
            reg.L = (byte) sum;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void ADDSPn(CPU cpu) {
            var reg = cpu.reg;
            var a = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            if (a > 127) {
                a = -((~a + 1) & 0xFF);
            }

            reg.FlagZero = false;
            reg.FlagSub = false;
            reg.FlagCarry = (reg.SP & 0xFF) + (a & 0xFF) > 0xFF;
            reg.FlagHalfCarry = ((reg.SP & 0xF) + (a & 0xF)) > 0xF;

            reg.SP = (ushort) (reg.SP + a);

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        private static void ADCr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = (int) reg.GetRegister(regI);
            var f = reg.FlagCarry ? 1 : 0;
            var sum = (reg.A + b + f);

            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagCarry = sum > 255;
            reg.FlagSub = false;
            reg.FlagHalfCarry = ((reg.A & 0xF) + (b & 0xF) + f) > 0xF;

            reg.A = (byte) sum;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void ADCHL(CPU cpu) {
            var reg = cpu.reg;
            var a = reg.A;
            var b = (int) cpu.memory.ReadByte(reg.HL);
            var f = reg.FlagCarry ? 1 : 0;
            var sum = (reg.A + b + f);

            reg.FlagZero = sum == 0;
            reg.FlagCarry = sum > 255;
            reg.FlagSub = false;
            reg.FlagHalfCarry = ((reg.A & 0xF) + (b & 0xF) + f) > 0xF;

            reg.A = (byte) sum;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void ADCn(CPU cpu) {
            var reg = cpu.reg;
            var b = (int) cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            var f = reg.FlagCarry ? 1 : 0;
            var sum = (reg.A + b + f);

            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagCarry = sum > 255;
            reg.FlagSub = false;
            reg.FlagHalfCarry = ((reg.A & 0xF) + (b & 0xF) + f) > 0xF;

            reg.A = (byte) sum;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void SUBr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var a = reg.A;
            var b = (int) reg.GetRegister(regI);
            var sum = reg.A - b;

            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagSub = true;
            reg.FlagCarry = sum < 0;
            reg.FlagHalfCarry = (reg.A & 0xF) < (b & 0xF);

            reg.A = (byte) sum;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void SUBHL(CPU cpu) {
            var reg = cpu.reg;
            var a = reg.A;
            var z = (int) cpu.memory.ReadByte(reg.HL);
            var sum = reg.A - z;

            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagSub = true;
            reg.FlagCarry = sum < 0;
            reg.FlagHalfCarry = (reg.A & 0xF) < (z & 0xF);

            reg.A = (byte) sum;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void SUBn(CPU cpu) {
            var reg = cpu.reg;
            var a = reg.A;
            var z = (int) cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            var sum = reg.A - z;

            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagSub = true;
            reg.FlagCarry = sum < 0;
            reg.FlagHalfCarry = (reg.A & 0xF) < (z & 0xF);

            reg.A = (byte) sum;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void SBCr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = (int) reg.GetRegister(regI);
            var f = reg.FlagCarry ? 1 : 0;
            var sum = reg.A - b - f;

            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagSub = true;
            reg.FlagCarry = sum < 0;
            reg.FlagHalfCarry = (reg.A & 0xF) < ((b & 0xF) + f);

            reg.A = (byte) sum;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void SBCHL(CPU cpu) {
            var reg = cpu.reg;
            var b = (int) cpu.memory.ReadByte(reg.HL);
            var f = reg.FlagCarry ? 1 : 0;
            var sum = reg.A - b - f;

            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagSub = true;
            reg.FlagCarry = sum < 0;
            reg.FlagHalfCarry = (reg.A & 0xF) < ((b & 0xF) + f);

            reg.A = (byte) sum;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void SBCn(CPU cpu) {
            var reg = cpu.reg;
            var b = (int) cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            var f = reg.FlagCarry ? 1 : 0;
            var sum = reg.A - b - f;

            reg.FlagZero = (sum & 0xFF) == 0;
            reg.FlagSub = true;
            reg.FlagCarry = sum < 0;
            reg.FlagHalfCarry = (reg.A & 0xF) < ((b & 0xF) + f);

            reg.A = (byte) sum;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void CPr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var a = (int) reg.A;
            var b = reg.GetRegister(regI);

            reg.FlagZero = a == b;
            reg.FlagSub = true;
            reg.FlagHalfCarry = (a & 0xF) < (b & 0xF);
            reg.FlagCarry = a < b;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void CPHL(CPU cpu) {
            var reg = cpu.reg;
            var a = (int) reg.A;
            var b = cpu.memory.ReadByte(reg.HL);

            reg.FlagZero = a == b;
            reg.FlagSub = true;
            reg.FlagHalfCarry = (a & 0xF) < (b & 0xF);
            reg.FlagCarry = a < b;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void CPn(CPU cpu) {
            var reg = cpu.reg;
            var a = (int) reg.A;
            var b =  cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            reg.FlagZero = a == b;
            reg.FlagSub = true;
            reg.FlagHalfCarry = (a & 0xF) < (b & 0xF);
            reg.FlagCarry = a < b;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void DAA(CPU cpu) {
            var reg = cpu.reg;
            var a = (int) reg.A;

            if (reg.FlagSub) {
                if (reg.FlagHalfCarry) {
                    a = a - 0x6;
                } else {
                    a -= 0x60;
                }
            } else {
                if (reg.FlagHalfCarry || (a & 0xF) > 0x9) {
                    a += 0x06;
                } else {
                    a += 0x60;
                }
            }

            reg.A = (byte)a;

            reg.FlagZero = a == 0;
            reg.FlagHalfCarry = false;
            if ((a & 0x100) == 0x100) {
                reg.FlagCarry = true;
            }


            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void ANDr(CPU cpu, string regI) {
            var reg = cpu.reg;
            reg.A &= reg.GetRegister(regI);

            reg.FlagZero = reg.A == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = true;
            reg.FlagCarry = false;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void ANDHL(CPU cpu) {
            var reg = cpu.reg;
            reg.A &= cpu.memory.ReadByte(reg.HL);

            reg.FlagZero = reg.A == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = true;
            reg.FlagCarry = false;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void ANDn(CPU cpu) {
            var reg = cpu.reg;
            reg.A &= cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            reg.FlagZero = reg.A == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = true;
            reg.FlagCarry = false;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void ORr(CPU cpu, string regI) {
            var reg = cpu.reg;
            reg.A |= reg.GetRegister(regI);

            reg.FlagZero = reg.A == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = false;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void ORHL(CPU cpu) {
            var reg = cpu.reg;
            reg.A |= cpu.memory.ReadByte(reg.HL);

            reg.FlagZero = reg.A == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = false;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void ORn(CPU cpu) {
            var reg = cpu.reg;
            reg.A |= cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            reg.FlagZero = reg.A == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = false;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void XORr(CPU cpu, string regI) {
            var reg = cpu.reg;
            reg.A ^= reg.GetRegister(regI);

            reg.FlagZero = reg.A == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = false;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void XORHL(CPU cpu) {
            var reg = cpu.reg;
            reg.A ^= cpu.memory.ReadByte(reg.HL);

            reg.FlagZero = reg.A == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = false;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void XORn(CPU cpu) {
            var reg = cpu.reg;
            reg.A ^= cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            reg.FlagZero = reg.A == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = false;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void INCr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var v = reg.GetRegister(regI);
            var v2 = (byte) (v + 1);
            reg.SetRegister(regI, v2);

            reg.FlagSub = false;
            reg.FlagHalfCarry = (v & 0xF) + 1 > 0xF;
            reg.FlagZero = ((byte) v2) == 0;
//            reg.FlagCarry = v + 1 > 255;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void INCHLm(CPU cpu) {
            var reg = cpu.reg;
            var v = cpu.memory.ReadByte(reg.HL);
            var v2 = (v + 1);
            cpu.memory.WriteByte(reg.HL, (byte) v2);

            reg.FlagSub = false;
            reg.FlagHalfCarry = (v & 0xF) + 1 > 0xF;
            reg.FlagZero = ((byte) v2) == 0;
            reg.FlagCarry = v2 > 255;

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void DECr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var v = reg.GetRegister(regI);
            var v2 = (v - 1);
            reg.SetRegister(regI, (byte) v2);

            reg.FlagSub = true;
            reg.FlagHalfCarry = (v & 0xF) == 0;
            reg.FlagZero = ((byte) v2) == 0;
//            reg.FlagCarry = v2 < 0;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void DECHLm(CPU cpu) {
            var reg = cpu.reg;
            var v = cpu.memory.ReadByte(reg.HL);
            var v2 = (v - 1);
            cpu.memory.WriteByte(reg.HL, (byte) v2);

            reg.FlagSub = true;
            reg.FlagHalfCarry = (v & 0xF) == 0;
            reg.FlagZero = ((byte) v2) == 0;
            reg.FlagCarry = v2 < 0;

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void INC(CPU cpu, string regA, string regB) {
            var reg = cpu.reg;
            var v = (byte) (reg.GetRegister(regB) + 1);
            reg.SetRegister(regB, v);

            if (v == 0) {
                var z = (byte) (reg.GetRegister(regA) + 1);
                reg.SetRegister(regA, z);
            }

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void INCHL(CPU cpu) {
            INC(cpu, "H", "L");
        }

        private static void INCSP(CPU cpu) {
            var reg = cpu.reg;
            reg.SP++;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void DEC(CPU cpu, string regA, string regB) {
            var reg = cpu.reg;
            var v = (byte) (reg.GetRegister(regB) - 1);
            reg.SetRegister(regB, v);

            if (v == 255) {
                var z = (byte) (reg.GetRegister(regA) - 1);
                reg.SetRegister(regA, z);
            }

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        private static void DECHL(CPU cpu) {
            DEC(cpu, "H", "L");
        }

        private static void DECSP(CPU cpu) {
            var reg = cpu.reg;
            reg.SP--;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        #endregion
        #region Bit Manipulation

        private static void RLA(CPU cpu) {
            var reg = cpu.reg;
            var c = (reg.A >> 7) > 0;
            var f = reg.FlagCarry ? 1 : 0;

            reg.A = (byte) ((reg.A << 1) | f);

            reg.FlagCarry = c;
            reg.FlagZero = false;
            reg.FlagHalfCarry = false;
            reg.FlagSub = false;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        private static void RLCA(CPU cpu) {
            var reg = cpu.reg;

            var c = (reg.A >> 7) & 0x1;

            reg.A = (byte) ((reg.A << 1) | c);

            reg.FlagCarry = c > 0;
            reg.FlagZero = false;
            reg.FlagHalfCarry = false;
            reg.FlagSub = false;


            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        private static void RRA(CPU cpu) {
            var reg = cpu.reg;

            var c = reg.A & 1;
            var f = reg.FlagCarry ? 1 : 0;

            reg.A = (byte)((reg.A >> 1) | (f << 7));

            reg.FlagCarry = c > 0;
            reg.FlagHalfCarry = false;
            reg.FlagZero = false;
            reg.FlagSub = false;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        private static void RRCA(CPU cpu) {
            var reg = cpu.reg;
            var c = reg.A & 1;
            reg.A = (byte) ((reg.A >> 1) | (c << 7));

            reg.FlagCarry = c > 0;
            reg.FlagHalfCarry = false;
            reg.FlagZero = false;
            reg.FlagSub = false;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void CPL(CPU cpu) {
            var reg = cpu.reg;
            reg.A = (byte) ~reg.A;

            reg.FlagHalfCarry = true;
            reg.FlagSub = true;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void CCF(CPU cpu) {
            var reg = cpu.reg;

            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = !reg.FlagCarry;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void SCF(CPU cpu) {
            var reg = cpu.reg;

            reg.FlagCarry = true;
            reg.FlagHalfCarry = false;
            reg.FlagSub = false;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        #endregion
        #region Interrupt Calls
        internal static void RSTXX(CPU cpu, ushort addr) {
            // Console.WriteLine($"Calling Interrupt 0x{addr:X4}");
            var reg = cpu.reg;
            reg.SaveRegs();
            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, reg.PC);
            reg.PC = addr;

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }
        #endregion
        #region Stack Management

        private static void PUSH(CPU cpu, string regA, string regB) {
            var reg = cpu.reg;
            reg.SP--;
            var b = reg.GetRegister(regA);
            cpu.memory.WriteByte(reg.SP, b);
            reg.SP--;
            b = reg.GetRegister(regB);
            cpu.memory.WriteByte(reg.SP, b);

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        private static void POP(CPU cpu, string regA, string regB) {
            var reg = cpu.reg;
            reg.SetRegister(regB, cpu.memory.ReadByte(reg.SP));
            reg.SP++;
            reg.SetRegister(regA, cpu.memory.ReadByte(reg.SP));
            reg.SP++;

            reg.lastClockM = 3;
            reg.lastClockT = 12;

        }
        #endregion
        #region Jumps

        private static void JPnn(CPU cpu) {
            var reg = cpu.reg;
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        private static void JPHL(CPU cpu) {
            var reg = cpu.reg;
            reg.PC = reg.HL;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void JPNZnn(CPU cpu) {
            var reg = cpu.reg;

            if (reg.FlagZero) {
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                reg.PC += 2;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.PC);
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        private static void JPZnn(CPU cpu) {
            var reg = cpu.reg;

            if (!reg.FlagZero) {
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                reg.PC += 2;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.PC);
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        private static void JPNCnn(CPU cpu) {
            var reg = cpu.reg;

            if (reg.FlagCarry) {
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                reg.PC += 2;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.PC);
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        private static void JPCnn(CPU cpu) {
            var reg = cpu.reg;

            if (!reg.FlagCarry) {
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                reg.PC += 2;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.PC);
            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        private static void JRn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            reg.PC = (ushort)(reg.PC + v);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void JRNZn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            if (reg.FlagZero) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }

            reg.PC = (ushort)(reg.PC + v);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void JRZn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            if (!reg.FlagZero) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }

            reg.PC = (ushort)(reg.PC + v);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void JRNCn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            if (reg.FlagCarry) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }

            reg.PC = (ushort)(reg.PC + v);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void JRCn(CPU cpu) {
            var reg = cpu.reg;
            var v = (int)cpu.memory.ReadByte(reg.PC);
            reg.PC++;

            if (v > 127) {
                v = -((~v + 1) & 0xFF);
            }

            if (!reg.FlagCarry) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }

            reg.PC = (ushort)(reg.PC + v);

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        private static void Stop(CPU cpu) {
            var reg = cpu.reg;
            cpu.stopped = true;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void CALLnn(CPU cpu) {
            var reg = cpu.reg;
            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, (ushort) (reg.PC + 2));
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 6;
            reg.lastClockT = 24;
        }

        private static void CALLNZnn(CPU cpu) {
            var reg = cpu.reg;

            if (reg.FlagZero) {
                reg.PC += 2;
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                return;
            }

            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, (ushort) (reg.PC + 2));
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 6;
            reg.lastClockT = 24;
        }

        private static void CALLZnn(CPU cpu) {
            var reg = cpu.reg;

            if (!reg.FlagZero) {
                reg.PC += 2;
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                return;
            }

            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, (ushort) (reg.PC + 2));
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 6;
            reg.lastClockT = 24;
        }

        private static void CALLNCnn(CPU cpu) {
            var reg = cpu.reg;

            if (reg.FlagCarry) {
                reg.PC += 2;
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                return;
            }

            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, (ushort) (reg.PC + 2));
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 6;
            reg.lastClockT = 24;
        }

        private static void CALLCnn(CPU cpu) {
            var reg = cpu.reg;

            if (!reg.FlagCarry) {
                reg.PC += 2;
                reg.lastClockM = 3;
                reg.lastClockT = 12;
                return;
            }

            reg.SP -= 2;
            cpu.memory.WriteWord(reg.SP, (ushort) (reg.PC + 2));
            reg.PC = cpu.memory.ReadWord(reg.PC);

            reg.lastClockM = 6;
            reg.lastClockT = 24;
        }

        private static void RET(CPU cpu) {
            var reg = cpu.reg;
            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        private static void RETI(CPU cpu) {
            var reg = cpu.reg;
            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.InterruptEnable = true;

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        private static void RETNZ(CPU cpu) {
            var reg = cpu.reg;

            if (reg.FlagZero) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        private static void RETNC(CPU cpu) {
            var reg = cpu.reg;

            if (reg.FlagCarry) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        private static void RETC(CPU cpu) {
            var reg = cpu.reg;

            if (!reg.FlagCarry) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        private static void RETZ(CPU cpu) {
            var reg = cpu.reg;

            if (!reg.FlagZero) {
                reg.lastClockM = 2;
                reg.lastClockT = 8;
                return;
            }

            reg.PC = cpu.memory.ReadWord(reg.SP);
            reg.SP += 2;
            reg.lastClockM = 5;
            reg.lastClockT = 20;
        }

        private static void DI(CPU cpu) {
            var reg = cpu.reg;
            reg.InterruptEnable = false;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        private static void EI(CPU cpu) {
            var reg = cpu.reg;
            reg.InterruptEnable = true;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        private static void NOP(CPU cpu) {
            var reg = cpu.reg;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        private static void NOPWARN(CPU cpu, int opcode) {
            var reg = cpu.reg;
            Console.WriteLine($"Unimplemented Opcode!!! 0x{opcode:X2} at 0x{reg.PC-1:X2}");
            reg.lastClockM = 0;
            reg.lastClockT = 0;
        }

        private static void HALT(CPU cpu) {
            Console.WriteLine("HALT");
            var reg = cpu.reg;
            cpu._halt = true;
            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }
        #endregion
        #region 0xCB Calls

        #region Call Implementation
        static void RLr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var v = reg.GetRegister(regI);
            var c = (v >> 7) > 0;
            var f = reg.FlagCarry ? 1 : 0;

            v = (byte) ((v << 1) | f);
            reg.SetRegister(regI, v);

            reg.FlagZero = v == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void RLHL(CPU cpu) {
            var reg = cpu.reg;
            var v = cpu.memory.ReadByte(reg.HL);
            var c = (v >> 7) > 0;
            var f = reg.FlagCarry ? 1 : 0;

            v = (byte) ((v << 1) | f);

            cpu.memory.WriteByte(reg.HL, v);

            reg.FlagZero = v == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c;

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void RLCr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var v = reg.GetRegister(regI);
            var c = v >> 7;

            v = (byte) ((v << 1) | c);

            reg.SetRegister(regI, v);

            reg.FlagZero = v == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c > 0;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void RLCHL(CPU cpu) {
            var reg = cpu.reg;
            var v = cpu.memory.ReadByte(reg.HL);
            var c = v >> 7;

            v = (byte) ((v << 1) | c);
            cpu.memory.WriteByte(reg.HL, v);

            reg.FlagZero = v == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c > 0;

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void RRr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var c = b & 1;
            var f = reg.FlagCarry ? 1 : 0;

            b = (byte) ((b >> 1) | (f << 7));

            reg.FlagZero = b == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c > 0;

            reg.SetRegister(regI, b);

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void RRHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var c = b & 1;
            var f = reg.FlagCarry ? 1 : 0;

            b = (byte) ((b >> 1) | (f << 7));

            reg.FlagZero = b == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c > 0;

            cpu.memory.WriteByte(reg.HL, b);

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void RRCr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var c = b & 1;

            b = (byte) ((b >> 1) | (c << 7));

            reg.SetRegister(regI, b);

            reg.FlagZero = b == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c > 0;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void RRCHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var c = b & 1;

            b = (byte) ((b >> 1) | (c << 7));

            cpu.memory.WriteByte(reg.HL, b);

            reg.FlagZero = b == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c > 0;

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void SLAr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var c = b >> 7;
            b = (byte) (b << 1);

            reg.SetRegister(regI, b);

            reg.FlagZero = b == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c > 0;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void SLAHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var c = b >> 7;
            b = (byte) (b << 1);

            cpu.memory.WriteByte(reg.HL, b);

            reg.FlagZero = b == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c > 0;

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void SRAr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var c = (b & 1) > 0;
            var ext = b & 0x80;

            b = (byte) ((b >> 1) | ext);

            reg.SetRegister(regI, b);

            reg.FlagZero = b == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void SRAHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var c = (b & 1) > 0;
            var ext = b & 0x80;

            b = (byte) ((b >> 1) | ext);

            cpu.memory.WriteByte(reg.HL, b);

            reg.FlagZero = b == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c;

            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void SWAPr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var swapped = ((b & 0x0F) << 4) | ((b & 0xF0) >> 4);
            reg.SetRegister(regI, (byte) swapped);

            reg.FlagZero = swapped == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = false;

            reg.lastClockM = 1;
            reg.lastClockT = 4;
        }

        static void SWAPHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var swapped = ((b & 0x0F) << 4) | ((b & 0xF0) >> 4);
            cpu.memory.WriteByte(reg.HL, (byte) swapped);

            reg.FlagZero = swapped == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = false;

            reg.lastClockM = 2;
            reg.lastClockT = 8;
        }

        static void SRLr(CPU cpu, string regI) {
            var reg = cpu.reg;
            var b = reg.GetRegister(regI);
            var c = (b & 1) > 0;

            b = (byte) (b >> 1);

            reg.SetRegister(regI, b);

            reg.FlagZero = b == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c;

            reg.lastClockM = 4;
            reg.lastClockT = 8;
        }

        static void SRLHL(CPU cpu) {
            var reg = cpu.reg;
            var b = cpu.memory.ReadByte(reg.HL);
            var c = (b & 1) > 0;

            b = (byte) (b >> 1);

            cpu.memory.WriteByte(reg.HL, b);

            reg.FlagZero = b == 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;
            reg.FlagCarry = c;


            reg.lastClockM = 4;
            reg.lastClockT = 16;
        }

        static void BIT(CPU cpu, int n, string regI) {
            var reg = cpu.reg;

            reg.FlagZero = (reg.GetRegister(regI) & (1 << n)) != 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;

            reg.lastClockM = 2;
            reg.lastClockT = 4;
        }

        static void BITm(CPU cpu, int n) {
            var reg = cpu.reg;

            reg.FlagZero = (cpu.memory.ReadByte(reg.HL) & (1 << n)) != 0;
            reg.FlagSub = false;
            reg.FlagHalfCarry = false;

            reg.lastClockM = 3;
            reg.lastClockT = 12;
        }

        static void RES(CPU cpu, int n, string regI) {
            var reg = cpu.reg;

            var b = reg.GetRegister(regI);
            b &= (byte) (~(1 << n));
            reg.SetRegister(regI, b);

            reg.lastClockM = 4;
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

        internal static readonly List<Action<CPU>> CBOPS = new List<Action<CPU>> {
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
            (cpu) => RES(cpu, 5, "B"),
            (cpu) => RES(cpu, 5, "C"),
            (cpu) => RES(cpu, 5, "D"),
            (cpu) => RES(cpu, 5, "E"),
            (cpu) => RES(cpu, 5, "H"),
            (cpu) => RES(cpu, 5, "L"),
            (cpu) => RESHL(cpu, 5),
            (cpu) => RES(cpu, 5, "A"),
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
        private static void CBCall(CPU cpu) {
            var reg = cpu.reg;
            var v = cpu.memory.ReadByte(reg.PC);
            reg.PC++;
            CBOPS[v](cpu);
        }
        #endregion
        #region CPU Instructions
        internal static readonly List<Action<CPU>> opcodes = new List<Action<CPU>> {
            #region 0x00 Group
            (cpu) => NOP(cpu),
            (cpu) => LD__nn(cpu, "B", "C"),
            (cpu) => LD__m_(cpu, "B", "C", "A"),
            (cpu) => INC(cpu, "B", "C"),
            (cpu) => INCr(cpu, "B"),
            (cpu) => DECr(cpu, "B"),
            (cpu) => LDrn_(cpu, "B"),
            (cpu) => RLCA(cpu),
            (cpu) => LDmmSP(cpu),
            (cpu) => ADDHL(cpu, "B", "C"),
            (cpu) => LD___m(cpu, "A", "B", "C"),
            (cpu) => DEC(cpu, "B", "C"),
            (cpu) => INCr(cpu, "C"),
            (cpu) => DECr(cpu, "C"),
            (cpu) => LDrn_(cpu, "C"),
            (cpu) => RRCA(cpu),
            #endregion
            #region 0x10 Group
            (cpu) => Stop(cpu),
            (cpu) => LD__nn(cpu, "D", "E"),
            (cpu) => LD__m_(cpu, "D", "E", "A"),
            (cpu) => INC(cpu, "D", "E"),
            (cpu) => INCr(cpu, "D"),
            (cpu) => DECr(cpu, "D"),
            (cpu) => LDrn_(cpu, "D"),
            (cpu) => RLA(cpu),
            (cpu) => JRn(cpu),
            (cpu) => ADDHL(cpu, "D", "E"),
            (cpu) => LD___m(cpu, "A", "D", "E"),
            (cpu) => DEC(cpu, "D", "E"),
            (cpu) => INCr(cpu, "E"),
            (cpu) => DECr(cpu, "E"),
            (cpu) => LDrn_(cpu, "E"),
            (cpu) => RRA(cpu),
            #endregion
            #region 0x20 Group
            (cpu) => JRNZn(cpu),
            (cpu) => LD__nn(cpu, "H", "L"),
            (cpu) => LDHLIA(cpu),
            (cpu) => INCHL(cpu),
            (cpu) => INCr(cpu, "H"),
            (cpu) => DECr(cpu, "H"),
            (cpu) => LDrn_(cpu, "H"),
            (cpu) => DAA(cpu),
            (cpu) => JRZn(cpu),
            (cpu) => ADDHL(cpu, "H", "L"),
            (cpu) => LDAHLI(cpu),
            (cpu) => DECHL(cpu),
            (cpu) => INCr(cpu, "L"),
            (cpu) => DECr(cpu, "L"),
            (cpu) => LDrn_(cpu, "L"),
            (cpu) => CPL(cpu),
            #endregion
            #region 0x30 Group
            (cpu) => JRNCn(cpu),
            (cpu) => LDSPnn(cpu),
            (cpu) => LDHLDA(cpu),
            (cpu) => INCSP(cpu),
            (cpu) => INCHLm(cpu),
            (cpu) => DECHLm(cpu),
            (cpu) => LDHLmn(cpu),
            (cpu) => SCF(cpu),
            (cpu) => JRCn(cpu),
            (cpu) => ADDHLSP(cpu),
            (cpu) => LDAHLD(cpu),
            (cpu) => DECSP(cpu),
            (cpu) => INCr(cpu, "A"),
            (cpu) => DECr(cpu, "A"),
            (cpu) => LDrn_(cpu, "A"),
            (cpu) => CCF(cpu),
            #endregion
            #region 0x40 Group
            (cpu) => LDrr(cpu, "B", "B"),
            (cpu) => LDrr(cpu, "B", "C"),
            (cpu) => LDrr(cpu, "B", "D"),
            (cpu) => LDrr(cpu, "B", "E"),
            (cpu) => LDrr(cpu, "B", "H"),
            (cpu) => LDrr(cpu, "B", "L"),
            (cpu) => LDrHLm_(cpu, "B"),
            (cpu) => LDrr(cpu, "B", "A"),
            (cpu) => LDrr(cpu, "C", "B"),
            (cpu) => LDrr(cpu, "C", "C"),
            (cpu) => LDrr(cpu, "C", "D"),
            (cpu) => LDrr(cpu, "C", "E"),
            (cpu) => LDrr(cpu, "C", "H"),
            (cpu) => LDrr(cpu, "C", "L"),
            (cpu) => LDrHLm_(cpu, "C"),
            (cpu) => LDrr(cpu, "C", "A"),
            #endregion
            #region 0x50 Group
            (cpu) => LDrr(cpu, "D", "B"),
            (cpu) => LDrr(cpu, "D", "C"),
            (cpu) => LDrr(cpu, "D", "D"),
            (cpu) => LDrr(cpu, "D", "E"),
            (cpu) => LDrr(cpu, "D", "H"),
            (cpu) => LDrr(cpu, "D", "L"),
            (cpu) => LDrHLm_(cpu, "D"),
            (cpu) => LDrr(cpu, "D", "A"),
            (cpu) => LDrr(cpu, "E", "B"),
            (cpu) => LDrr(cpu, "E", "C"),
            (cpu) => LDrr(cpu, "E", "D"),
            (cpu) => LDrr(cpu, "E", "E"),
            (cpu) => LDrr(cpu, "E", "H"),
            (cpu) => LDrr(cpu, "E", "L"),
            (cpu) => LDrHLm_(cpu, "E"),
            (cpu) => LDrr(cpu, "E", "A"),
            #endregion
            #region 0x60 Group
            (cpu) => LDrr(cpu, "H", "B"),
            (cpu) => LDrr(cpu, "H", "C"),
            (cpu) => LDrr(cpu, "H", "D"),
            (cpu) => LDrr(cpu, "H", "E"),
            (cpu) => LDrr(cpu, "H", "H"),
            (cpu) => LDrr(cpu, "H", "L"),
            (cpu) => LDrHLm_(cpu, "H"),
            (cpu) => LDrr(cpu, "H", "A"),
            (cpu) => LDrr(cpu, "L", "B"),
            (cpu) => LDrr(cpu, "L", "C"),
            (cpu) => LDrr(cpu, "L", "D"),
            (cpu) => LDrr(cpu, "L", "E"),
            (cpu) => LDrr(cpu, "L", "H"),
            (cpu) => LDrr(cpu, "L", "L"),
            (cpu) => LDrHLm_(cpu, "L"),
            (cpu) => LDrr(cpu, "L", "A"),
            #endregion
            #region 0x70 Group
            (cpu) => LDHLmr_(cpu, "B"),
            (cpu) => LDHLmr_(cpu, "C"),
            (cpu) => LDHLmr_(cpu, "D"),
            (cpu) => LDHLmr_(cpu, "E"),
            (cpu) => LDHLmr_(cpu, "H"),
            (cpu) => LDHLmr_(cpu, "L"),
            (cpu) => HALT(cpu),
            (cpu) => LDHLmr_(cpu, "A"),
            (cpu) => LDrr(cpu, "A", "B"),
            (cpu) => LDrr(cpu, "A", "C"),
            (cpu) => LDrr(cpu, "A", "D"),
            (cpu) => LDrr(cpu, "A", "E"),
            (cpu) => LDrr(cpu, "A", "H"),
            (cpu) => LDrr(cpu, "A", "L"),
            (cpu) => LDrHLm_(cpu, "A"),
            (cpu) => LDrr(cpu, "A", "A"),
            #endregion
            #region 0x80 Group
            (cpu) => ADDr(cpu, "B"),
            (cpu) => ADDr(cpu, "C"),
            (cpu) => ADDr(cpu, "D"),
            (cpu) => ADDr(cpu, "E"),
            (cpu) => ADDr(cpu, "H"),
            (cpu) => ADDr(cpu, "L"),
            (cpu) => ADDHL(cpu),
            (cpu) => ADDr(cpu, "A"),
            (cpu) => ADCr(cpu, "B"),
            (cpu) => ADCr(cpu, "C"),
            (cpu) => ADCr(cpu, "D"),
            (cpu) => ADCr(cpu, "E"),
            (cpu) => ADCr(cpu, "H"),
            (cpu) => ADCr(cpu, "L"),
            (cpu) => ADCHL(cpu),
            (cpu) => ADCr(cpu, "A"),
            #endregion
            #region 0x90 Group
            (cpu) => SUBr(cpu, "B"),
            (cpu) => SUBr(cpu, "C"),
            (cpu) => SUBr(cpu, "D"),
            (cpu) => SUBr(cpu, "E"),
            (cpu) => SUBr(cpu, "H"),
            (cpu) => SUBr(cpu, "L"),
            (cpu) => SUBHL(cpu),
            (cpu) => SUBr(cpu, "A"),
            (cpu) => SBCr(cpu, "B"),
            (cpu) => SBCr(cpu, "C"),
            (cpu) => SBCr(cpu, "D"),
            (cpu) => SBCr(cpu, "E"),
            (cpu) => SBCr(cpu, "H"),
            (cpu) => SBCr(cpu, "L"),
            (cpu) => SBCHL(cpu),
            (cpu) => SBCr(cpu, "A"),
            #endregion
            #region 0xA0 Group
            (cpu) => ANDr(cpu, "B"),
            (cpu) => ANDr(cpu, "C"),
            (cpu) => ANDr(cpu, "D"),
            (cpu) => ANDr(cpu, "E"),
            (cpu) => ANDr(cpu, "H"),
            (cpu) => ANDr(cpu, "L"),
            (cpu) => ANDHL(cpu),
            (cpu) => ANDr(cpu, "A"),
            (cpu) => XORr(cpu, "B"),
            (cpu) => XORr(cpu, "C"),
            (cpu) => XORr(cpu, "D"),
            (cpu) => XORr(cpu, "E"),
            (cpu) => XORr(cpu, "H"),
            (cpu) => XORr(cpu, "L"),
            (cpu) => XORHL(cpu),
            (cpu) => XORr(cpu, "A"),
            #endregion
            #region 0xB0 Group
            (cpu) => ORr(cpu, "B"),
            (cpu) => ORr(cpu, "C"),
            (cpu) => ORr(cpu, "D"),
            (cpu) => ORr(cpu, "E"),
            (cpu) => ORr(cpu, "H"),
            (cpu) => ORr(cpu, "L"),
            (cpu) => ORHL(cpu),
            (cpu) => ORr(cpu, "A"),
            (cpu) => CPr(cpu, "B"),
            (cpu) => CPr(cpu, "C"),
            (cpu) => CPr(cpu, "D"),
            (cpu) => CPr(cpu, "E"),
            (cpu) => CPr(cpu, "H"),
            (cpu) => CPr(cpu, "L"),
            (cpu) => CPHL(cpu),
            (cpu) => CPr(cpu, "A"),
            #endregion
            #region 0xC0 Group
            (cpu) => RETNZ(cpu),
            (cpu) => POP(cpu, "B", "C"),
            (cpu) => JPNZnn(cpu),
            (cpu) => JPnn(cpu),
            (cpu) => CALLNZnn(cpu),
            (cpu) => PUSH(cpu, "B", "C"),
            (cpu) => ADDn(cpu),
            (cpu) => RSTXX(cpu, 0x00),
            (cpu) => RETZ(cpu),
            (cpu) => RET(cpu),
            (cpu) => JPZnn(cpu),
            (cpu) => CBCall(cpu),
            (cpu) => CALLZnn(cpu),
            (cpu) => CALLnn(cpu),
            (cpu) => ADCn(cpu),
            (cpu) => RSTXX(cpu, 0x08),
            #endregion
            #region 0xD0 Group
            (cpu) => RETNC(cpu),
            (cpu) => POP(cpu, "D", "E"),
            (cpu) => JPNCnn(cpu),
            (cpu) => NOPWARN(cpu, 0xD3),
            (cpu) => CALLNCnn(cpu),
            (cpu) => PUSH(cpu, "D", "E"),
            (cpu) => SUBn(cpu),
            (cpu) => RSTXX(cpu, 0x10),
            (cpu) => RETC(cpu),
            (cpu) => RETI(cpu),
            (cpu) => JPCnn(cpu),
            (cpu) => NOPWARN(cpu, 0xDB),
            (cpu) => CALLCnn(cpu),
            (cpu) => NOPWARN(cpu, 0xDD),
            (cpu) => SBCn(cpu),
            (cpu) => RSTXX(cpu, 0x18),
            #endregion
            #region 0xE0 Group
            (cpu) => LDIOnA(cpu),
            (cpu) => POP(cpu, "H", "L"),
            (cpu) => LDIOCA(cpu),
            (cpu) => NOPWARN(cpu, 0xE3),
            (cpu) => NOPWARN(cpu, 0xE4),
            (cpu) => PUSH(cpu, "H", "L"),
            (cpu) => ANDn(cpu),
            (cpu) => RSTXX(cpu, 0x20),
            (cpu) => ADDSPn(cpu),
            (cpu) => JPHL(cpu),
            (cpu) => LDmm_(cpu, "A"),
            (cpu) => NOPWARN(cpu, 0xEB),
            (cpu) => NOPWARN(cpu, 0xEC),
            (cpu) => NOPWARN(cpu, 0xED),
            (cpu) => XORn(cpu),
            (cpu) => RSTXX(cpu, 0x28),
            #endregion
            #region 0xF0 Group
            (cpu) => LDAIOn(cpu),
            (cpu) => POP(cpu, "A", "F"),
            (cpu) => LDAIOC(cpu),
            (cpu) => DI(cpu),
            (cpu) => NOPWARN(cpu, 0xF4),
            (cpu) => PUSH(cpu, "A", "F"),
            (cpu) => ORn(cpu),
            (cpu) => RSTXX(cpu, 0x30),
            (cpu) => LDHLSPn(cpu),
            (cpu) => LDHLSPr(cpu),
            (cpu) => LD_mm(cpu, "A"),
            (cpu) => EI(cpu),
            (cpu) => NOPWARN(cpu, 0xFC),
            (cpu) => NOPWARN(cpu, 0xFD),
            (cpu) => CPn(cpu),
            (cpu) => RSTXX(cpu, 0x38),
            #endregion
        };
        #endregion
    }
}
