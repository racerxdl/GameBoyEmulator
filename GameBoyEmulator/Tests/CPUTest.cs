using System;
using GameBoyEmulator.Desktop.GBC;
using NUnit.Framework;

namespace GameBoyEmulator.Desktop.Tests {
    [TestFixture]
    public class CPUTest {
        private const int RUN_CYCLES = 100;

        #region Test LD BC,d16
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
        #region Test LD [BC],A
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
        #region Test LD B, d8
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
        #region Test LD A, [BC]
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
        #region Test LD C, d8
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
        #region Test LD DE, d16
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
        #region Test LD [DE], A
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
        #region Test LD D, d8
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
        #region Test LD A, [DE]
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
        #region Test LD E, d8
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
        #region Test LD HL, d16
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
        #region Test LD H,d8
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
        #region Test LD L, d8
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
        #region Test LD A,d8
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
        #region #region Test LD B, B
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
        #region #region Test LD B, C
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
        #region #region Test LD B, D
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
        #region #region Test LD B, E
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
        #region #region Test LD B, H
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
        #region #region Test LD B, L
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
        #region Test LD B, [HL]
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
        #region #region Test LD B, A
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
        #region #region Test LD C, B
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
        #region #region Test LD C, C
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
        #region #region Test LD C, D
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
        #region #region Test LD C, E
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
        #region #region Test LD C, H
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
        #region #region Test LD C, L
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
        #region Test LD C, [HL]
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
        #region #region Test LD C, A
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
        #region #region Test LD D, B
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
        #region #region Test LD D, C
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
        #region #region Test LD D, D
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
        #region #region Test LD D, E
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
        #region #region Test LD D, H
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
        #region #region Test LD D, L
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
        #region Test LD D, [HL]
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
        #region #region Test LD D, A
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
        #region #region Test LD E, B
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
        #region #region Test LD E, C
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
        #region #region Test LD E, D
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
        #region #region Test LD E, E
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
        #region #region Test LD E, H
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
        #region #region Test LD E, L
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
        #region Test LD E, [HL]
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
        #region #region Test LD E, A
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
        #region #region Test LD H, B
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
        #region #region Test LD H, C
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
        #region #region Test LD H, D
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
        #region #region Test LD H, E
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
        #region #region Test LD H, H
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
        #region #region Test LD H, L
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
        #region Test LD H, [HL]
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
        #region #region Test LD H, A
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
        #region #region Test LD L, B
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
        #region #region Test LD L, C
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
        #region #region Test LD L, D
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
        #region #region Test LD L, E
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
        #region #region Test LD L, H
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
        #region #region Test LD L, L
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
        #region Test LD L, [HL]
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
        #region #region Test LD L, A
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
        #region Test LD [HL], X
        [Test]
        public void LDHLmrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x70) \"LD [HL], X\"");
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
        #region Test LD [HL], X
        [Test]
        public void LDHLmrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x71) \"LD [HL], X\"");
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
        #region Test LD [HL], X
        [Test]
        public void LDHLmrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x72) \"LD [HL], X\"");
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
        #region Test LD [HL], X
        [Test]
        public void LDHLmrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x73) \"LD [HL], X\"");
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
        #region Test LD [HL], X
        [Test]
        public void LDHLmrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x74) \"LD [HL], X\"");
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
        #region Test LD [HL], X
        [Test]
        public void LDHLmrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x75) \"LD [HL], X\"");
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
        #region Test LD [HL], X
        [Test]
        public void LDHLmrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x77) \"LD [HL], X\"");
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
        #region #region Test LD A, B
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
        #region #region Test LD A, C
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
        #region #region Test LD A, D
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
        #region #region Test LD A, E
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
        #region #region Test LD A, H
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
        #region #region Test LD A, L
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
        #region Test LD A, [HL]
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
        #region #region Test LD A, A
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
        #region Test LD [a16], A
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
        #region Test LD A, [a16]
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