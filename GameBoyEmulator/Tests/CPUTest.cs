using System;
using GameBoyEmulator.Desktop.GBC;
using NUnit.Framework;

namespace GameBoyEmulator.Desktop.Tests {
    [TestFixture]
    public class CPUTest {
        private const int RUN_CYCLES = 100;

        #region 0x01 Test LD BC,d16
        [Test]
        public void LDBCnn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x01) \"LD BC,d16\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));
                var var0 = (byte) random.Next(0x00, 0xFF);
                var var1 = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, var0);
                cpu.memory.WriteByte(cpu.reg.PC + 1, var1);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x01](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(var0, regAfter.C);
                Assert.AreEqual(var1, regAfter.B);
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 12);
                Assert.AreEqual(regAfter.lastClockM, 3);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x02 Test LD [BC],A
        [Test]
        public void LDBCmA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x02) \"LD [BC],A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.B = 0xA0;
                cpu.reg.C = (byte) random.Next(0x00, 0xFF);

                var hl = (cpu.reg.B << 8) + cpu.reg.C;

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x02](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.A, cpu.memory.ReadByte(hl));
                Assert.AreEqual(regBefore.A, regAfter.A);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x06 Test LD B, d8
        [Test]
        public void LDrnB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x06) \"LD B, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                cpu.reg.PC = cpu.reg.HL; // Put PC in High Ram random value;

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x06](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(val, regAfter.B);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                #endregion

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x08 Test LD [a16], SP
        [Test]
        public void LDmmSP() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x08) \"LD [a16], SP\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));
                var addr = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));

                cpu.memory.WriteWord(cpu.reg.PC, addr);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x08](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadWord(addr), regAfter.SP);
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 20);
                Assert.AreEqual(regAfter.lastClockM, 5);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x09 Test ADD HL,BC
        [Test]
        public void ADDHLBC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x09) \"ADD HL,BC\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x09](cpu);
                var regAfter = cpu.reg.Clone();

                var ab = (regBefore.B << 8) + regBefore.C;
                var sum = regBefore.HL + ab;
                var halfCarry = (regBefore.HL & 0xFFF) + (ab & 0xFFF) > 0xFFF;

                Assert.AreEqual(sum & 0xFFFF, regAfter.HL);
                Assert.AreEqual(sum > 65535, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x0a Test LD A, [BC]
        [Test]
        public void LDABCm() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x0a) \"LD A, [BC]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.B = 0xA0;
                cpu.reg.C = (byte) random.Next(0x00, 0xFF);

                var hl = (cpu.reg.B << 8) + cpu.reg.C;
                var val = (byte) random.Next(0x00, 0xFF);
                cpu.memory.WriteByte(hl, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x0a](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x0e Test LD C, d8
        [Test]
        public void LDrnC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x0e) \"LD C, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                cpu.reg.PC = cpu.reg.HL; // Put PC in High Ram random value;

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x0e](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(val, regAfter.C);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                #endregion

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x11 Test LD DE, d16
        [Test]
        public void LDDEnn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x11) \"LD DE, d16\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));
                var var0 = (byte) random.Next(0x00, 0xFF);
                var var1 = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, var0);
                cpu.memory.WriteByte(cpu.reg.PC + 1, var1);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x11](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(var0, regAfter.E);
                Assert.AreEqual(var1, regAfter.D);
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 12);
                Assert.AreEqual(regAfter.lastClockM, 3);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x12 Test LD [DE], A
        [Test]
        public void LDDEmA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x12) \"LD [DE], A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.D = 0xA0;
                cpu.reg.E = (byte) random.Next(0x00, 0xFF);

                var hl = (cpu.reg.D << 8) + cpu.reg.E;

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x12](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.A, cpu.memory.ReadByte(hl));
                Assert.AreEqual(regBefore.A, regAfter.A);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x16 Test LD D, d8
        [Test]
        public void LDrnD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x16) \"LD D, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                cpu.reg.PC = cpu.reg.HL; // Put PC in High Ram random value;

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x16](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(val, regAfter.D);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                #endregion

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x19 Test ADD HL, DE
        [Test]
        public void ADDHLDE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x19) \"ADD HL, DE\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x19](cpu);
                var regAfter = cpu.reg.Clone();

                var ab = (regBefore.D << 8) + regBefore.E;
                var sum = regBefore.HL + ab;
                var halfCarry = (regBefore.HL & 0xFFF) + (ab & 0xFFF) > 0xFFF;

                Assert.AreEqual(sum & 0xFFFF, regAfter.HL);
                Assert.AreEqual(sum > 65535, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x1a Test LD A, [DE]
        [Test]
        public void LDADEm() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x1a) \"LD A, [DE]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.D = 0xA0;
                cpu.reg.E = (byte) random.Next(0x00, 0xFF);

                var hl = (cpu.reg.D << 8) + cpu.reg.E;
                var val = (byte) random.Next(0x00, 0xFF);
                cpu.memory.WriteByte(hl, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x1a](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x1e Test LD E, d8
        [Test]
        public void LDrnE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x1e) \"LD E, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                cpu.reg.PC = cpu.reg.HL; // Put PC in High Ram random value;

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x1e](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(val, regAfter.E);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                #endregion

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x21 Test LD HL, d16
        [Test]
        public void LDHLnn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x21) \"LD HL, d16\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));
                var var0 = (byte) random.Next(0x00, 0xFF);
                var var1 = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, var0);
                cpu.memory.WriteByte(cpu.reg.PC + 1, var1);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x21](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(var0, regAfter.L);
                Assert.AreEqual(var1, regAfter.H);
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 12);
                Assert.AreEqual(regAfter.lastClockM, 3);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x22 Test LD [HL+], A
        [Test]
        public void LDHLIA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x22) \"LD [HL+], A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x22](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(regBefore.HL), regBefore.A);
                Assert.AreEqual(regBefore.HL + 1, regAfter.HL);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x26 Test LD H,d8
        [Test]
        public void LDrnH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x26) \"LD H,d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                cpu.reg.PC = cpu.reg.HL; // Put PC in High Ram random value;

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x26](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(val, regAfter.H);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                #endregion

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x29 Test ADD HL, HL
        [Test]
        public void ADDHLHL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x29) \"ADD HL, HL\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x29](cpu);
                var regAfter = cpu.reg.Clone();

                var ab = (regBefore.H << 8) + regBefore.L;
                var sum = regBefore.HL + ab;
                var halfCarry = (regBefore.HL & 0xFFF) + (ab & 0xFFF) > 0xFFF;

                Assert.AreEqual(sum & 0xFFFF, regAfter.HL);
                Assert.AreEqual(sum > 65535, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x2a Test LD A, [HL+]
        [Test]
        public void LDAHLI() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x2a) \"LD A, [HL+]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x2a](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(regBefore.HL - 1, regAfter.HL);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x2e Test LD L, d8
        [Test]
        public void LDrnL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x2e) \"LD L, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                cpu.reg.PC = cpu.reg.HL; // Put PC in High Ram random value;

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x2e](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(val, regAfter.L);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                #endregion

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x31 Test LD SP, d16
        [Test]
        public void LDSPnn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x31) \"LD SP, d16\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));
                var var0 = (ushort) random.Next(0x0000, 0xFFFF);

                cpu.memory.WriteWord(cpu.reg.PC, var0);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x31](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(var0, regAfter.SP);
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 12);
                Assert.AreEqual(regAfter.lastClockM, 3);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x32 Test LD [HL-], A
        [Test]
        public void LDHLDA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x32) \"LD [HL-], A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x32](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(regBefore.HL), regBefore.A);
                Assert.AreEqual(regBefore.HL - 1, regAfter.HL);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x39 Test ADD HL, SP
        [Test]
        public void ADDHLSP() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x39) \"ADD HL, SP\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x39](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.HL + regBefore.SP;
                var halfCarry = (regBefore.HL & 0xFFF) + (regBefore.SP & 0xFFF) > 0xFFF;

                Assert.AreEqual(sum & 0xFFFF, regAfter.HL);
                Assert.AreEqual(sum > 65535, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x3a Test LD A,[HL-]
        [Test]
        public void LDAHLD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x3a) \"LD A,[HL-]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x3a](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(regBefore.HL - 1, regAfter.HL);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x3e Test LD A,d8
        [Test]
        public void LDrnA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x3e) \"LD A,d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                cpu.reg.PC = cpu.reg.HL; // Put PC in High Ram random value;

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x3e](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                #endregion

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x40 Test LD B, B
        [Test]
        public void LDrrBB() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x40) \"LD B, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x40](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.B, regBefore.B);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x41 Test LD B, C
        [Test]
        public void LDrrBC() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x41) \"LD B, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x41](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.B, regBefore.C);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x42 Test LD B, D
        [Test]
        public void LDrrBD() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x42) \"LD B, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x42](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.B, regBefore.D);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x43 Test LD B, E
        [Test]
        public void LDrrBE() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x43) \"LD B, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x43](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.B, regBefore.E);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x44 Test LD B, H
        [Test]
        public void LDrrBH() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x44) \"LD B, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x44](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.B, regBefore.H);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x45 Test LD B, L
        [Test]
        public void LDrrBL() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x45) \"LD B, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x45](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.B, regBefore.L);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x46 Test LD B, [HL]
        [Test]
        public void LDrHLmB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x46) \"LD B, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x46](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.B);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x47 Test LD B, A
        [Test]
        public void LDrrBA() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x47) \"LD B, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x47](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.B, regBefore.A);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x48 Test LD C, B
        [Test]
        public void LDrrCB() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x48) \"LD C, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x48](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.C, regBefore.B);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x49 Test LD C, C
        [Test]
        public void LDrrCC() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x49) \"LD C, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x49](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.C, regBefore.C);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x4a Test LD C, D
        [Test]
        public void LDrrCD() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x4a) \"LD C, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x4a](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.C, regBefore.D);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x4b Test LD C, E
        [Test]
        public void LDrrCE() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x4b) \"LD C, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x4b](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.C, regBefore.E);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x4c Test LD C, H
        [Test]
        public void LDrrCH() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x4c) \"LD C, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x4c](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.C, regBefore.H);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x4d Test LD C, L
        [Test]
        public void LDrrCL() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x4d) \"LD C, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x4d](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.C, regBefore.L);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x4e Test LD C, [HL]
        [Test]
        public void LDrHLmC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x4e) \"LD C, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x4e](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.C);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x4f Test LD C, A
        [Test]
        public void LDrrCA() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x4f) \"LD C, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x4f](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.C, regBefore.A);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x50 Test LD D, B
        [Test]
        public void LDrrDB() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x50) \"LD D, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x50](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.D, regBefore.B);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x51 Test LD D, C
        [Test]
        public void LDrrDC() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x51) \"LD D, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x51](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.D, regBefore.C);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x52 Test LD D, D
        [Test]
        public void LDrrDD() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x52) \"LD D, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x52](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.D, regBefore.D);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x53 Test LD D, E
        [Test]
        public void LDrrDE() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x53) \"LD D, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x53](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.D, regBefore.E);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x54 Test LD D, H
        [Test]
        public void LDrrDH() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x54) \"LD D, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x54](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.D, regBefore.H);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x55 Test LD D, L
        [Test]
        public void LDrrDL() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x55) \"LD D, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x55](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.D, regBefore.L);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x56 Test LD D, [HL]
        [Test]
        public void LDrHLmD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x56) \"LD D, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x56](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.D);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x57 Test LD D, A
        [Test]
        public void LDrrDA() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x57) \"LD D, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x57](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.D, regBefore.A);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x58 Test LD E, B
        [Test]
        public void LDrrEB() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x58) \"LD E, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x58](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.E, regBefore.B);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x59 Test LD E, C
        [Test]
        public void LDrrEC() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x59) \"LD E, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x59](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.E, regBefore.C);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x5a Test LD E, D
        [Test]
        public void LDrrED() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x5a) \"LD E, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x5a](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.E, regBefore.D);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x5b Test LD E, E
        [Test]
        public void LDrrEE() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x5b) \"LD E, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x5b](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.E, regBefore.E);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x5c Test LD E, H
        [Test]
        public void LDrrEH() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x5c) \"LD E, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x5c](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.E, regBefore.H);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x5d Test LD E, L
        [Test]
        public void LDrrEL() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x5d) \"LD E, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x5d](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.E, regBefore.L);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x5e Test LD E, [HL]
        [Test]
        public void LDrHLmE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x5e) \"LD E, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x5e](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.E);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x5f Test LD E, A
        [Test]
        public void LDrrEA() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x5f) \"LD E, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x5f](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.E, regBefore.A);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x60 Test LD H, B
        [Test]
        public void LDrrHB() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x60) \"LD H, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x60](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.H, regBefore.B);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x61 Test LD H, C
        [Test]
        public void LDrrHC() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x61) \"LD H, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x61](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.H, regBefore.C);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x62 Test LD H, D
        [Test]
        public void LDrrHD() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x62) \"LD H, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x62](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.H, regBefore.D);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x63 Test LD H, E
        [Test]
        public void LDrrHE() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x63) \"LD H, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x63](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.H, regBefore.E);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x64 Test LD H, H
        [Test]
        public void LDrrHH() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x64) \"LD H, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x64](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.H, regBefore.H);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x65 Test LD H, L
        [Test]
        public void LDrrHL() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x65) \"LD H, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x65](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.H, regBefore.L);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x66 Test LD H, [HL]
        [Test]
        public void LDrHLmH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x66) \"LD H, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x66](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.H);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x67 Test LD H, A
        [Test]
        public void LDrrHA() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x67) \"LD H, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x67](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.H, regBefore.A);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x68 Test LD L, B
        [Test]
        public void LDrrLB() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x68) \"LD L, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x68](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.L, regBefore.B);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x69 Test LD L, C
        [Test]
        public void LDrrLC() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x69) \"LD L, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x69](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.L, regBefore.C);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x6a Test LD L, D
        [Test]
        public void LDrrLD() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x6a) \"LD L, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x6a](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.L, regBefore.D);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x6b Test LD L, E
        [Test]
        public void LDrrLE() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x6b) \"LD L, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x6b](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.L, regBefore.E);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x6c Test LD L, H
        [Test]
        public void LDrrLH() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x6c) \"LD L, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x6c](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.L, regBefore.H);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x6d Test LD L, L
        [Test]
        public void LDrrLL() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x6d) \"LD L, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x6d](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.L, regBefore.L);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x6e Test LD L, [HL]
        [Test]
        public void LDrHLmL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x6e) \"LD L, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x6e](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.L);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x6f Test LD L, A
        [Test]
        public void LDrrLA() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x6f) \"LD L, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x6f](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.L, regBefore.A);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x70 Test LD [HL], B
        [Test]
        public void LDHLmrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x70) \"LD [HL], B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x70](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(cpu.reg.HL), regAfter.B);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x71 Test LD [HL], C
        [Test]
        public void LDHLmrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x71) \"LD [HL], C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x71](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(cpu.reg.HL), regAfter.C);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x72 Test LD [HL], D
        [Test]
        public void LDHLmrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x72) \"LD [HL], D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x72](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(cpu.reg.HL), regAfter.D);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x73 Test LD [HL], E
        [Test]
        public void LDHLmrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x73) \"LD [HL], E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x73](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(cpu.reg.HL), regAfter.E);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x74 Test LD [HL], H
        [Test]
        public void LDHLmrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x74) \"LD [HL], H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x74](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(cpu.reg.HL), regAfter.H);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x75 Test LD [HL], L
        [Test]
        public void LDHLmrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x75) \"LD [HL], L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x75](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(cpu.reg.HL), regAfter.L);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x77 Test LD [HL], A
        [Test]
        public void LDHLmrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x77) \"LD [HL], A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x77](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(cpu.reg.HL), regAfter.A);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x78 Test LD A, B
        [Test]
        public void LDrrAB() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x78) \"LD A, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x78](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.A, regBefore.B);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x79 Test LD A, C
        [Test]
        public void LDrrAC() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x79) \"LD A, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x79](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.A, regBefore.C);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x7a Test LD A, D
        [Test]
        public void LDrrAD() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x7a) \"LD A, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x7a](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.A, regBefore.D);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x7b Test LD A, E
        [Test]
        public void LDrrAE() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x7b) \"LD A, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x7b](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.A, regBefore.E);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x7c Test LD A, H
        [Test]
        public void LDrrAH() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x7c) \"LD A, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x7c](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.A, regBefore.H);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x7d Test LD A, L
        [Test]
        public void LDrrAL() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x7d) \"LD A, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x7d](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.A, regBefore.L);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x7e Test LD A, [HL]
        [Test]
        public void LDrHLmA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x7e) \"LD A, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x7e](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x7f Test LD A, A
        [Test]
        public void LDrrAA() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x7f) \"LD A, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x7f](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.A, regBefore.A);
                #endregion

                
                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0x80 Test ADD A, B
        [Test]
        public void ADDrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x80) \"ADD A, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x80](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A + regBefore.B;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.B & 0xF) > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("B" != "A") {
                    Assert.AreEqual(regBefore.B, regAfter.B);
                }

                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x81 Test ADD A, C
        [Test]
        public void ADDrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x81) \"ADD A, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x81](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A + regBefore.C;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.C & 0xF) > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("C" != "A") {
                    Assert.AreEqual(regBefore.C, regAfter.C);
                }

                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x82 Test ADD A, D
        [Test]
        public void ADDrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x82) \"ADD A, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x82](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A + regBefore.D;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.D & 0xF) > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("D" != "A") {
                    Assert.AreEqual(regBefore.D, regAfter.D);
                }

                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x83 Test ADD A, E
        [Test]
        public void ADDrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x83) \"ADD A, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x83](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A + regBefore.E;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.E & 0xF) > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("E" != "A") {
                    Assert.AreEqual(regBefore.E, regAfter.E);
                }

                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x84 Test ADD A, H
        [Test]
        public void ADDrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x84) \"ADD A, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x84](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A + regBefore.H;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.H & 0xF) > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("H" != "A") {
                    Assert.AreEqual(regBefore.H, regAfter.H);
                }

                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x85 Test ADD A, L
        [Test]
        public void ADDrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x85) \"ADD A, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x85](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A + regBefore.L;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.L & 0xF) > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("L" != "A") {
                    Assert.AreEqual(regBefore.L, regAfter.L);
                }

                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x86 Test ADD A, [HL]
        [Test]
        public void ADDHLm() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x86) \"ADD A, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)

                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x86](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A + val;
                var halfCarry = (regBefore.A & 0xF) + (val & 0xF) > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x87 Test ADD A, A
        [Test]
        public void ADDrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x87) \"ADD A, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x87](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A + regBefore.A;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.A & 0xF) > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("A" != "A") {
                    Assert.AreEqual(regBefore.A, regAfter.A);
                }

                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x88 Test ADC A, B
        [Test]
        public void ADCrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x88) \"ADC A, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x88](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A + regBefore.B + f;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.B & 0xF) + f > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("B" != "A") {
                    Assert.AreEqual(regBefore.B, regAfter.B);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x89 Test ADC A, C
        [Test]
        public void ADCrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x89) \"ADC A, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x89](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A + regBefore.C + f;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.C & 0xF) + f > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("C" != "A") {
                    Assert.AreEqual(regBefore.C, regAfter.C);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x8a Test ADC A, D
        [Test]
        public void ADCrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x8a) \"ADC A, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x8a](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A + regBefore.D + f;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.D & 0xF) + f > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("D" != "A") {
                    Assert.AreEqual(regBefore.D, regAfter.D);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x8b Test ADC A, E
        [Test]
        public void ADCrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x8b) \"ADC A, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x8b](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A + regBefore.E + f;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.E & 0xF) + f > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("E" != "A") {
                    Assert.AreEqual(regBefore.E, regAfter.E);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x8c Test ADC A, H
        [Test]
        public void ADCrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x8c) \"ADC A, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x8c](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A + regBefore.H + f;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.H & 0xF) + f > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("H" != "A") {
                    Assert.AreEqual(regBefore.H, regAfter.H);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x8d Test ADC A, L
        [Test]
        public void ADCrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x8d) \"ADC A, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x8d](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A + regBefore.L + f;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.L & 0xF) + f > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("L" != "A") {
                    Assert.AreEqual(regBefore.L, regAfter.L);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x8e Test ADC A, [HL]
        [Test]
        public void ADCHLm() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x8e) \"ADC A, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)

                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x8e](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A + val + f;
                var halfCarry = (regBefore.A & 0xF) + (val & 0xF) + f > 0xF;

                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x8f Test ADC A, A
        [Test]
        public void ADCrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x8f) \"ADC A, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x8f](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A + regBefore.A + f;
                var halfCarry = (regBefore.A & 0xF) + (regBefore.A & 0xF) + f > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("A" != "A") {
                    Assert.AreEqual(regBefore.A, regAfter.A);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x90 Test SUB A, B
        [Test]
        public void SUBrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x90) \"SUB A, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x90](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A - regBefore.B;
                var halfCarry = (regBefore.A & 0xF) < (regBefore.B & 0xF);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("B" != "A") {
                    Assert.AreEqual(regBefore.B, regAfter.B);
                }

                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x91 Test SUB A, C
        [Test]
        public void SUBrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x91) \"SUB A, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x91](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A - regBefore.C;
                var halfCarry = (regBefore.A & 0xF) < (regBefore.C & 0xF);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("C" != "A") {
                    Assert.AreEqual(regBefore.C, regAfter.C);
                }

                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x92 Test SUB A, D
        [Test]
        public void SUBrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x92) \"SUB A, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x92](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A - regBefore.D;
                var halfCarry = (regBefore.A & 0xF) < (regBefore.D & 0xF);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("D" != "A") {
                    Assert.AreEqual(regBefore.D, regAfter.D);
                }

                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x93 Test SUB A, E
        [Test]
        public void SUBrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x93) \"SUB A, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x93](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A - regBefore.E;
                var halfCarry = (regBefore.A & 0xF) < (regBefore.E & 0xF);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("E" != "A") {
                    Assert.AreEqual(regBefore.E, regAfter.E);
                }

                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x94 Test SUB A, H
        [Test]
        public void SUBrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x94) \"SUB A, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x94](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A - regBefore.H;
                var halfCarry = (regBefore.A & 0xF) < (regBefore.H & 0xF);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("H" != "A") {
                    Assert.AreEqual(regBefore.H, regAfter.H);
                }

                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x95 Test SUB A, L
        [Test]
        public void SUBrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x95) \"SUB A, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x95](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A - regBefore.L;
                var halfCarry = (regBefore.A & 0xF) < (regBefore.L & 0xF);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("L" != "A") {
                    Assert.AreEqual(regBefore.L, regAfter.L);
                }

                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x97 Test SUB A, A
        [Test]
        public void SUBrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x97) \"SUB A, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x97](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A - regBefore.A;
                var halfCarry = (regBefore.A & 0xF) < (regBefore.A & 0xF);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("A" != "A") {
                    Assert.AreEqual(regBefore.A, regAfter.A);
                }

                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 4);
                Assert.AreEqual(regAfter.lastClockM, 1);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xce Test ADC A, d8
        [Test]
        public void ADCn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xce) \"ADC A, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x00, 0xFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xce](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A + val + f;
                var halfCarry = (regBefore.A & 0xF) + (val & 0xF) + f > 0xF;

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xe0 Test LD [$FF00+a8], A
        [Test]
        public void LDIOnA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xe0) \"LD [$FF00+a8], A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + (byte) random.Next(0x00, 0xFF));

                cpu.memory.WriteByte(cpu.reg.PC, 0x80);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xe0](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.A, cpu.memory.ReadByte(0xFF80));
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 12);
                Assert.AreEqual(regAfter.lastClockM, 3);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0xe2 Test LD [$FF00+C], A
        [Test]
        public void LDIOCA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xe2) \"LD [$FF00+C], A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.C = 0x80;

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xe2](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.A, cpu.memory.ReadByte(0xFF80));

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0xe8 Test ADD SP, r8
        [Test]
        public void ADDSPn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xe8) \"ADD SP, r8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x00, 0xFF));
                var signedV = random.Next(-128, 0);
                var v = (byte) signedV;

                cpu.memory.WriteByte(cpu.reg.PC, v);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xe8](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.SP + signedV;
                var halfCarry = (regBefore.SP & 0xF) + (signedV & 0xF) > 0xF;
                var carry = (regBefore.SP & 0xFF) + (signedV & 0xFF) > 0xFF;

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(sum & 0xFFFF, regAfter.SP);
                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 16);
                Assert.AreEqual(regAfter.lastClockM, 4);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(false, regAfter.FlagZero);
                #endregion
            }
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x00, 0xFF));
                var signedV = random.Next(0, 127);
                var v = (byte) signedV;

                cpu.memory.WriteByte(cpu.reg.PC, v);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xe8](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.SP + signedV;
                var halfCarry = (regBefore.SP & 0xF) + (signedV & 0xF) > 0xF;
                var carry = (regBefore.SP & 0xFF) + (signedV & 0xFF) > 0xFF;

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(sum & 0xFFFF, regAfter.SP);
                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 16);
                Assert.AreEqual(regAfter.lastClockM, 4);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(false, regAfter.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0xea Test LD [a16], A
        [Test]
        public void LDmmA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xea) \"LD [a16], A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x000, 0xFFF));

                var addr = (ushort) ((0xA0 << 8) + random.Next(0x000, 0xFFF));

                cpu.memory.WriteWord(cpu.reg.PC, addr);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xea](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(addr), regBefore.A);
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 16);
                Assert.AreEqual(regAfter.lastClockM, 4);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0xf0 Test LD A, [$FF00+a8]
        [Test]
        public void LDAIOn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xf0) \"LD A, [$FF00+a8]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + (byte) random.Next(0x00, 0xFF));

                var val = (byte) random.Next(0x00, 0x10);

                cpu.memory.WriteByte(cpu.reg.PC, 0x80);
                cpu.memory.WriteByte(0xFF80, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xf0](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 12);
                Assert.AreEqual(regAfter.lastClockM, 3);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0xf2 Test LD A, [$FF00+C]
        [Test]
        public void LDAIOC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xf2) \"LD A, [$FF00+C]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.C = 0x80;
                var val = (byte) random.Next(0x00, 0x10);

                cpu.memory.WriteByte(0xFF80, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xf2](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0xf8 Test LD HL, SP+r8
        [Test]
        public void LDHLSPn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xf8) \"LD HL, SP+r8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));

                var signedV = random.Next(-128, 0);
                var v = (byte) signedV;

                cpu.memory.WriteByte(cpu.reg.PC, v);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xf8](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.SP + signedV, regAfter.HL);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual((regBefore.SP & 0xF) + (signedV & 0xF) > 0xF, regAfter.FlagHalfCarry);
                Assert.AreEqual((regBefore.SP & 0xFF) + (signedV & 0xFF) > 0xFF, regAfter.FlagCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 12);
                Assert.AreEqual(regAfter.lastClockM, 3);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(false, regAfter.FlagZero);
                #endregion
            }
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));

                var signedV = random.Next(0, 127);
                var v = (byte) signedV;

                cpu.memory.WriteByte(cpu.reg.PC, v);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xf8](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.SP + signedV, regAfter.HL);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual((regBefore.SP & 0xF) + (signedV & 0xF) > 0xF, regAfter.FlagHalfCarry);
                Assert.AreEqual((regBefore.SP & 0xFF) + (signedV & 0xFF) > 0xFF, regAfter.FlagCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 12);
                Assert.AreEqual(regAfter.lastClockM, 3);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(false, regAfter.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0xf9 Test LD HL, SP
        [Test]
        public void LDHLSPr() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xf9) \"LD HL, SP\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xf9](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.HL, regAfter.SP);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion
        #region 0xfa Test LD A, [a16]
        [Test]
        public void LDAmm() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xfa) \"LD A, [a16]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x000, 0xFFF));

                var addr = (ushort) ((0xA0 << 8) + random.Next(0x000, 0xFFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(addr, val);
                cpu.memory.WriteWord(cpu.reg.PC, addr);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xfa](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
                Assert.AreEqual(regAfter.F, regBefore.F);
                Assert.AreEqual(regAfter.H, regBefore.H);
                Assert.AreEqual(regAfter.L, regBefore.L);
                Assert.AreEqual(regAfter.HL, regBefore.HL);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 16);
                Assert.AreEqual(regAfter.lastClockM, 4);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);
                #endregion
            }
        }
        #endregion

    }
}