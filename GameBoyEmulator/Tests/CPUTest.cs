using System;
using System.Linq;
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
        #region 0x04 Test INC B
        [Test]
        public void INCrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x04) \"INC B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x04](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.B + 1);
                var halfCarry = (regBefore.B & 0xF) + 1 > 0xF;

                Assert.AreEqual(val, regAfter.B);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x05 Test DEC B
        [Test]
        public void DECrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x05) \"DEC B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x05](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.B - 1);
                var halfCarry = (regBefore.B & 0xF) == 0x00;

                Assert.AreEqual(val, regAfter.B);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagSub);
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
        #region 0x07 Test RLCA
        [Test]
        public void RLCA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x07) \"RLCA\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x07](cpu);
                var regAfter = cpu.reg.Clone();

                var c = (regBefore.A >> 7) & 0x1;
                var val = (byte) ((regBefore.A << 1) | c);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(c > 0, regAfter.FlagCarry);

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
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(false, regAfter.FlagZero);
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
        #region 0x0c Test INC C
        [Test]
        public void INCrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x0c) \"INC C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x0c](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.C + 1);
                var halfCarry = (regBefore.C & 0xF) + 1 > 0xF;

                Assert.AreEqual(val, regAfter.C);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x0d Test DEC C
        [Test]
        public void DECrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x0d) \"DEC C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x0d](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.C - 1);
                var halfCarry = (regBefore.C & 0xF) == 0x00;

                Assert.AreEqual(val, regAfter.C);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagSub);
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
        #region 0x0f Test RRCA
        [Test]
        public void RRCA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x0f) \"RRCA\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x0f](cpu);
                var regAfter = cpu.reg.Clone();

                var c = regBefore.A & 1;
                var val = (byte) ((regBefore.A >> 1) | (c << 7));

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(c > 0, regAfter.FlagCarry);

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
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(false, regAfter.FlagZero);
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
        #region 0x14 Test INC D
        [Test]
        public void INCrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x14) \"INC D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x14](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.D + 1);
                var halfCarry = (regBefore.D & 0xF) + 1 > 0xF;

                Assert.AreEqual(val, regAfter.D);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x15 Test DEC D
        [Test]
        public void DECrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x15) \"DEC D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x15](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.D - 1);
                var halfCarry = (regBefore.D & 0xF) == 0x00;

                Assert.AreEqual(val, regAfter.D);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagSub);
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
        #region 0x17 Test RLA
        [Test]
        public void RLA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x17) \"RLA\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x17](cpu);
                var regAfter = cpu.reg.Clone();

                var c = (regBefore.A >> 7) > 0;
                var f = regBefore.FlagCarry ? 1 : 0;
                var val = (byte) ((regBefore.A << 1) | f);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(c, regAfter.FlagCarry);

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
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(false, regAfter.FlagZero);
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
        #region 0x1c Test INC E
        [Test]
        public void INCrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x1c) \"INC E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x1c](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.E + 1);
                var halfCarry = (regBefore.E & 0xF) + 1 > 0xF;

                Assert.AreEqual(val, regAfter.E);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x1d Test DEC E
        [Test]
        public void DECrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x1d) \"DEC E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x1d](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.E - 1);
                var halfCarry = (regBefore.E & 0xF) == 0x00;

                Assert.AreEqual(val, regAfter.E);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagSub);
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
        #region 0x1f Test RRA
        [Test]
        public void RRA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x1f) \"RRA\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x1f](cpu);
                var regAfter = cpu.reg.Clone();

                var c = regBefore.A & 1;
                var f = regBefore.FlagCarry ? 1 : 0;

                var val = (byte)((regBefore.A >> 1) | (f << 7));

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(c > 0, regAfter.FlagCarry);

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
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                Assert.AreEqual(false, regAfter.FlagZero);
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
        #region 0x24 Test INC H
        [Test]
        public void INCrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x24) \"INC H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x24](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.H + 1);
                var halfCarry = (regBefore.H & 0xF) + 1 > 0xF;

                Assert.AreEqual(val, regAfter.H);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
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
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x25 Test DEC H
        [Test]
        public void DECrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x25) \"DEC H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x25](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.H - 1);
                var halfCarry = (regBefore.H & 0xF) == 0x00;

                Assert.AreEqual(val, regAfter.H);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
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
                Assert.AreEqual(true, regAfter.FlagSub);
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
        #region 0x27 Test DAA
        [Test]
        public void DAA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x27) \"DAA\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x27](cpu);
                var regAfter = cpu.reg.Clone();

                var a = (int)regBefore.A;

                if (regBefore.FlagSub) {
                    if (regBefore.FlagHalfCarry) {
                        a = a - 0x6;
                    } else {
                        a -= 0x60;
                    }
                } else {
                    if (regBefore.FlagHalfCarry || (a & 0xF) > 0x9) {
                        a += 0x06;
                    } else {
                        a += 0x60;
                    }
                }

                var zero = a == 0;
                var carry = ((a & 0x100) == 0x100) || regBefore.FlagCarry;

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(zero, regAfter.FlagZero);
                Assert.AreEqual(a & 0xFF, regAfter.A);

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
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);
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
        #region 0x2c Test INC L
        [Test]
        public void INCrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x2c) \"INC L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x2c](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.L + 1);
                var halfCarry = (regBefore.L & 0xF) + 1 > 0xF;

                Assert.AreEqual(val, regAfter.L);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
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
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x2d Test DEC L
        [Test]
        public void DECrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x2d) \"DEC L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x2d](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.L - 1);
                var halfCarry = (regBefore.L & 0xF) == 0x00;

                Assert.AreEqual(val, regAfter.L);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
                Assert.AreEqual(regAfter.B, regBefore.B);
                Assert.AreEqual(regAfter.C, regBefore.C);
                Assert.AreEqual(regAfter.D, regBefore.D);
                Assert.AreEqual(regAfter.E, regBefore.E);
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
                Assert.AreEqual(true, regAfter.FlagSub);
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
        #region 0x2f Test CPL
        [Test]
        public void CPL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x2f) \"CPL\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x2f](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) ~regBefore.A;

                Assert.AreEqual(val, regAfter.A);

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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagHalfCarry);
                Assert.AreEqual(true, regAfter.FlagSub);
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
        #region 0x33 Test INC SP
        [Test]
        public void INCSP() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x33) \"INC SP\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x33](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (ushort) (regBefore.SP + 1);
                Assert.AreEqual(val, regAfter.SP);

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
        #region 0x34 Test INC [HL]
        [Test]
        public void INCHLm() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x34) \"INC [HL]\"");
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
                CPUInstructions.opcodes[0x34](cpu);
                var regAfter = cpu.reg.Clone();

                var valAfter = cpu.memory.ReadByte(regBefore.HL);

                var newVal = (byte) (val + 1);
                var halfCarry = (val & 0xF) + 1 > 0xF;

                Assert.AreEqual(newVal, valAfter);
                Assert.AreEqual(newVal == 0, regAfter.FlagZero);
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
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 12);
                Assert.AreEqual(regAfter.lastClockM, 3);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x35 Test DEC [HL]
        [Test]
        public void DECHLm() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x35) \"DEC [HL]\"");
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
                CPUInstructions.opcodes[0x35](cpu);
                var regAfter = cpu.reg.Clone();

                var valAfter = cpu.memory.ReadByte(regBefore.HL);

                var newVal = (byte) (val - 1);
                var halfCarry = (val & 0xF)  == 0x00;

                Assert.AreEqual(newVal, valAfter);
                Assert.AreEqual(newVal == 0, regAfter.FlagZero);
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
                Assert.AreEqual(regAfter.PC, regBefore.PC);
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 12);
                Assert.AreEqual(regAfter.lastClockM, 3);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x37 Test SCF
        [Test]
        public void SCF() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x37) \"SCF\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x37](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
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
                Assert.AreEqual(true, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
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
        #region 0x3b Test DEC SP
        [Test]
        public void DECSP() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x3b) \"DEC SP\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x3b](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (ushort) (regBefore.SP - 1);
                Assert.AreEqual(val, regAfter.SP);

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
        #region 0x3c Test INC A
        [Test]
        public void INCrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x3c) \"INC A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x3c](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A + 1);
                var halfCarry = (regBefore.A & 0xF) + 1 > 0xF;

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x3d Test DEC A
        [Test]
        public void DECrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x3d) \"DEC A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x3d](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A - 1);
                var halfCarry = (regBefore.A & 0xF) == 0x00;

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);
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
                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagSub);
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
        #region 0x3f Test CCF
        [Test]
        public void CCF() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x3f) \"CCF\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x3f](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(!regBefore.FlagCarry, regAfter.FlagCarry);

                #region Test no change to other regs
                Assert.AreEqual(regAfter.A, regBefore.A);
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
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
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
        #region 0x96 Test SUB A, [HL]
        [Test]
        public void SUBHL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x96) \"SUB A, [HL]\"");
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
                CPUInstructions.opcodes[0x96](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A - val;
                var halfCarry = (regBefore.A & 0xF) < (val & 0xF);

                Assert.AreEqual((byte) sum, regAfter.A);
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
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
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
        #region 0x98 Test SBC A, B
        [Test]
        public void SBCrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x98) \"SBC A, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x98](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A - regBefore.B - f;
                var halfCarry = (regBefore.A & 0xF) < ((regBefore.B & 0xF) + f);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("B" != "A") {
                    Assert.AreEqual(regBefore.B, regAfter.B);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
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
        #region 0x99 Test SBC A, C
        [Test]
        public void SBCrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x99) \"SBC A, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x99](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A - regBefore.C - f;
                var halfCarry = (regBefore.A & 0xF) < ((regBefore.C & 0xF) + f);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("C" != "A") {
                    Assert.AreEqual(regBefore.C, regAfter.C);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
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
        #region 0x9a Test SBC A, D
        [Test]
        public void SBCrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x9a) \"SBC A, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x9a](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A - regBefore.D - f;
                var halfCarry = (regBefore.A & 0xF) < ((regBefore.D & 0xF) + f);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("D" != "A") {
                    Assert.AreEqual(regBefore.D, regAfter.D);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
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
        #region 0x9b Test SBC A, E
        [Test]
        public void SBCrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x9b) \"SBC A, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x9b](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A - regBefore.E - f;
                var halfCarry = (regBefore.A & 0xF) < ((regBefore.E & 0xF) + f);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("E" != "A") {
                    Assert.AreEqual(regBefore.E, regAfter.E);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
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
        #region 0x9c Test SBC A, H
        [Test]
        public void SBCrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x9c) \"SBC A, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x9c](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A - regBefore.H - f;
                var halfCarry = (regBefore.A & 0xF) < ((regBefore.H & 0xF) + f);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("H" != "A") {
                    Assert.AreEqual(regBefore.H, regAfter.H);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
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
        #region 0x9d Test SBC A, L
        [Test]
        public void SBCrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x9d) \"SBC A, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x9d](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A - regBefore.L - f;
                var halfCarry = (regBefore.A & 0xF) < ((regBefore.L & 0xF) + f);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("L" != "A") {
                    Assert.AreEqual(regBefore.L, regAfter.L);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
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
        #region 0x9e Test SBC A, [HL]
        [Test]
        public void SBCHLm() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x9e) \"SBC A, [HL]\"");
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
                CPUInstructions.opcodes[0x9e](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A - val - f;
                var halfCarry = (regBefore.A & 0xF) < ((val & 0xF) + f);

                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
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
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0x9f Test SBC A, A
        [Test]
        public void SBCrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x9f) \"SBC A, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x9f](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A - regBefore.A - f;
                var halfCarry = (regBefore.A & 0xF) < ((regBefore.A & 0xF) + f);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("A" != "A") {
                    Assert.AreEqual(regBefore.A, regAfter.A);
                }
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
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
        #region 0xa0 Test AND A, B
        [Test]
        public void ANDrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xa0) \"AND A, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xa0](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A & regBefore.B);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xa1 Test AND A, C
        [Test]
        public void ANDrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xa1) \"AND A, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xa1](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A & regBefore.C);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xa2 Test AND A, D
        [Test]
        public void ANDrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xa2) \"AND A, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xa2](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A & regBefore.D);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xa3 Test AND A, E
        [Test]
        public void ANDrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xa3) \"AND A, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xa3](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A & regBefore.E);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xa4 Test AND A, H
        [Test]
        public void ANDrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xa4) \"AND A, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xa4](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A & regBefore.H);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xa5 Test AND A, L
        [Test]
        public void ANDrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xa5) \"AND A, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xa5](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A & regBefore.L);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xa6 Test AND A, [HL]
        [Test]
        public void ANDHL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xa6) \"AND A, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xa6](cpu);
                var regAfter = cpu.reg.Clone();

                val = (byte) (regBefore.A & val);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xa7 Test AND A, A
        [Test]
        public void ANDrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xa7) \"AND A, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xa7](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A & regBefore.A);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xa8 Test XOR A, B
        [Test]
        public void XORrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xa8) \"XOR A, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xa8](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A ^ regBefore.B);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xa9 Test XOR A, C
        [Test]
        public void XORrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xa9) \"XOR A, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xa9](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A ^ regBefore.C);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xaa Test XOR A, D
        [Test]
        public void XORrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xaa) \"XOR A, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xaa](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A ^ regBefore.D);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xab Test XOR A, E
        [Test]
        public void XORrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xab) \"XOR A, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xab](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A ^ regBefore.E);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xac Test XOR A, H
        [Test]
        public void XORrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xac) \"XOR A, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xac](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A ^ regBefore.H);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xad Test XOR A, L
        [Test]
        public void XORrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xad) \"XOR A, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xad](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A ^ regBefore.L);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xae Test XOR A, [HL]
        [Test]
        public void XORHL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xae) \"XOR A, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xae](cpu);
                var regAfter = cpu.reg.Clone();

                val = (byte) (regBefore.A ^ val);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xaf Test XOR A, A
        [Test]
        public void XORrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xaf) \"XOR A, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xaf](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A ^ regBefore.A);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xb0 Test OR A, B
        [Test]
        public void ORrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xb0) \"OR A, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xb0](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A | regBefore.B);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xb1 Test OR A, C
        [Test]
        public void ORrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xb1) \"OR A, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xb1](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A | regBefore.C);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xb2 Test OR A, D
        [Test]
        public void ORrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xb2) \"OR A, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xb2](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A | regBefore.D);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xb3 Test OR A, E
        [Test]
        public void ORrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xb3) \"OR A, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xb3](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A | regBefore.E);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xb4 Test OR A, H
        [Test]
        public void ORrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xb4) \"OR A, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xb4](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A | regBefore.H);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xb5 Test OR A, L
        [Test]
        public void ORrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xb5) \"OR A, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xb5](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A | regBefore.L);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xb6 Test OR A, [HL]
        [Test]
        public void ORHL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xb6) \"OR A, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xb6](cpu);
                var regAfter = cpu.reg.Clone();

                val = (byte) (regBefore.A | val);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xb7 Test OR A, A
        [Test]
        public void ORrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xb7) \"OR A, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xb7](cpu);
                var regAfter = cpu.reg.Clone();

                var val = (byte) (regBefore.A | regBefore.A);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xb8 Test CP A, B
        [Test]
        public void CPrB() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xb8) \"CP A, B\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xb8](cpu);
                var regAfter = cpu.reg.Clone();

                var halfCarry = (regBefore.A & 0xF) < (regBefore.B & 0xF);
                var carry = regBefore.A < regBefore.B;

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == regBefore.B, regAfter.FlagZero);
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
        #region 0xb9 Test CP A, C
        [Test]
        public void CPrC() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xb9) \"CP A, C\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xb9](cpu);
                var regAfter = cpu.reg.Clone();

                var halfCarry = (regBefore.A & 0xF) < (regBefore.C & 0xF);
                var carry = regBefore.A < regBefore.C;

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == regBefore.C, regAfter.FlagZero);
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
        #region 0xba Test CP A, D
        [Test]
        public void CPrD() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xba) \"CP A, D\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xba](cpu);
                var regAfter = cpu.reg.Clone();

                var halfCarry = (regBefore.A & 0xF) < (regBefore.D & 0xF);
                var carry = regBefore.A < regBefore.D;

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == regBefore.D, regAfter.FlagZero);
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
        #region 0xbb Test CP A, E
        [Test]
        public void CPrE() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xbb) \"CP A, E\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xbb](cpu);
                var regAfter = cpu.reg.Clone();

                var halfCarry = (regBefore.A & 0xF) < (regBefore.E & 0xF);
                var carry = regBefore.A < regBefore.E;

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == regBefore.E, regAfter.FlagZero);
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
        #region 0xbc Test CP A, H
        [Test]
        public void CPrH() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xbc) \"CP A, H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xbc](cpu);
                var regAfter = cpu.reg.Clone();

                var halfCarry = (regBefore.A & 0xF) < (regBefore.H & 0xF);
                var carry = regBefore.A < regBefore.H;

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == regBefore.H, regAfter.FlagZero);
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
        #region 0xbd Test CP A, L
        [Test]
        public void CPrL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xbd) \"CP A, L\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xbd](cpu);
                var regAfter = cpu.reg.Clone();

                var halfCarry = (regBefore.A & 0xF) < (regBefore.L & 0xF);
                var carry = regBefore.A < regBefore.L;

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == regBefore.L, regAfter.FlagZero);
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
        #region 0xbe Test CP A, [HL]
        [Test]
        public void CPHL() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xbe) \"CP A, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xbe](cpu);
                var regAfter = cpu.reg.Clone();

                var halfCarry = (regBefore.A & 0xF) < (val & 0xF);
                var carry = regBefore.A < val;

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == val, regAfter.FlagZero);
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
        #region 0xbf Test CP A, A
        [Test]
        public void CPrA() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xbf) \"CP A, A\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xbf](cpu);
                var regAfter = cpu.reg.Clone();

                var halfCarry = (regBefore.A & 0xF) < (regBefore.A & 0xF);
                var carry = regBefore.A < regBefore.A;

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == regBefore.A, regAfter.FlagZero);
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
        #region 0xc6 Test ADD A, d8
        [Test]
        public void ADDn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xc6) \"ADD A, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x00, 0xFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xc6](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A + val;
                var halfCarry = (regBefore.A & 0xF) + (val & 0xF) > 0xF;

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
        #region 0xc7 Test RST 00H
        [Test]
        public void RST00() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xc7) \"RST 00H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xc7](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual((ushort) 0, regAfter.PC);
                Assert.AreEqual(regBefore.SP - 2, regAfter.SP);

                regAfter.RandomizeRegisters();
                regAfter.LoadRegs();

                var regs = new [] {"A", "B", "C", "D", "E", "F", "H", "L"};
                var savedMatch = regs
                    .Select((reg) => regBefore.GetRegister(reg) == regAfter.GetRegister(reg))
                    .Aggregate((a, b) => a && b);

                Assert.IsTrue(savedMatch);

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
        #region 0xcf Test RST 08H
        [Test]
        public void RST08() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xcf) \"RST 08H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xcf](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual((ushort) 8, regAfter.PC);
                Assert.AreEqual(regBefore.SP - 2, regAfter.SP);

                regAfter.RandomizeRegisters();
                regAfter.LoadRegs();

                var regs = new [] {"A", "B", "C", "D", "E", "F", "H", "L"};
                var savedMatch = regs
                    .Select((reg) => regBefore.GetRegister(reg) == regAfter.GetRegister(reg))
                    .Aggregate((a, b) => a && b);

                Assert.IsTrue(savedMatch);

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
        #region 0xd6 Test SUB A, d8
        [Test]
        public void SUBn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xd6) \"SUB A, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x00, 0xFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xd6](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A - val;
                var halfCarry = (regBefore.A & 0xF) < (val & 0xF);

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(sum & 0xFF, regAfter.A);

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
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xd7 Test RST 10H
        [Test]
        public void RST10() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xd7) \"RST 10H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xd7](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual((ushort) 16, regAfter.PC);
                Assert.AreEqual(regBefore.SP - 2, regAfter.SP);

                regAfter.RandomizeRegisters();
                regAfter.LoadRegs();

                var regs = new [] {"A", "B", "C", "D", "E", "F", "H", "L"};
                var savedMatch = regs
                    .Select((reg) => regBefore.GetRegister(reg) == regAfter.GetRegister(reg))
                    .Aggregate((a, b) => a && b);

                Assert.IsTrue(savedMatch);

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
        #region 0xde Test SBC A, d8
        [Test]
        public void SBCn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xde) \"SBC A, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x00, 0xFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xde](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A - val - f;
                var halfCarry = (regBefore.A & 0xF) < (val & 0xF) + f;

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(sum & 0xFF, regAfter.A);

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
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xdf Test RST 18H
        [Test]
        public void RST18() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xdf) \"RST 18H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xdf](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual((ushort) 24, regAfter.PC);
                Assert.AreEqual(regBefore.SP - 2, regAfter.SP);

                regAfter.RandomizeRegisters();
                regAfter.LoadRegs();

                var regs = new [] {"A", "B", "C", "D", "E", "F", "H", "L"};
                var savedMatch = regs
                    .Select((reg) => regBefore.GetRegister(reg) == regAfter.GetRegister(reg))
                    .Aggregate((a, b) => a && b);

                Assert.IsTrue(savedMatch);

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
        #region 0xe6 Test AND A, d8
        [Test]
        public void ANDn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xe6) \"AND A, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x00, 0xFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xe6](cpu);
                var regAfter = cpu.reg.Clone();

                val = (byte) (regBefore.A & val);

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(true, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xe7 Test RST 20H
        [Test]
        public void RST20() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xe7) \"RST 20H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xe7](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual((ushort) 32, regAfter.PC);
                Assert.AreEqual(regBefore.SP - 2, regAfter.SP);

                regAfter.RandomizeRegisters();
                regAfter.LoadRegs();

                var regs = new [] {"A", "B", "C", "D", "E", "F", "H", "L"};
                var savedMatch = regs
                    .Select((reg) => regBefore.GetRegister(reg) == regAfter.GetRegister(reg))
                    .Aggregate((a, b) => a && b);

                Assert.IsTrue(savedMatch);

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
        #region 0xee Test XOR A, d8
        [Test]
        public void XORn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xee) \"XOR A, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x00, 0xFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xee](cpu);
                var regAfter = cpu.reg.Clone();

                val = (byte) (regBefore.A ^ val);

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xef Test RST 28H
        [Test]
        public void RST28() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xef) \"RST 28H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xef](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual((ushort) 40, regAfter.PC);
                Assert.AreEqual(regBefore.SP - 2, regAfter.SP);

                regAfter.RandomizeRegisters();
                regAfter.LoadRegs();

                var regs = new [] {"A", "B", "C", "D", "E", "F", "H", "L"};
                var savedMatch = regs
                    .Select((reg) => regBefore.GetRegister(reg) == regAfter.GetRegister(reg))
                    .Aggregate((a, b) => a && b);

                Assert.IsTrue(savedMatch);

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
        #region 0xf6 Test OR A, d8
        [Test]
        public void ORn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xf6) \"OR A, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x00, 0xFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xf6](cpu);
                var regAfter = cpu.reg.Clone();

                val = (byte) (regBefore.A | val);

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

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
                Assert.AreEqual(false, regAfter.FlagCarry);
                Assert.AreEqual(false, regAfter.FlagHalfCarry);
                Assert.AreEqual(false, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xf7 Test RST 30H
        [Test]
        public void RST30() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xf7) \"RST 30H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xf7](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual((ushort) 48, regAfter.PC);
                Assert.AreEqual(regBefore.SP - 2, regAfter.SP);

                regAfter.RandomizeRegisters();
                regAfter.LoadRegs();

                var regs = new [] {"A", "B", "C", "D", "E", "F", "H", "L"};
                var savedMatch = regs
                    .Select((reg) => regBefore.GetRegister(reg) == regAfter.GetRegister(reg))
                    .Aggregate((a, b) => a && b);

                Assert.IsTrue(savedMatch);

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
        #region 0xfe Test CP A, d8
        [Test]
        public void CPn() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xfe) \"CP A, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x00, 0xFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);


                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xfe](cpu);
                var regAfter = cpu.reg.Clone();

                var halfCarry = (regBefore.A & 0xF) < (val & 0xF);
                var carry = regBefore.A < val;

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == val, regAfter.FlagZero);
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
                Assert.AreEqual(regAfter.SP, regBefore.SP);
                #endregion
                
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, 8);
                Assert.AreEqual(regAfter.lastClockM, 2);
                #endregion

                
                #region Flag Tests
                Assert.AreEqual(true, regAfter.FlagSub);
                #endregion
            }
        }
        #endregion
        #region 0xff Test RST 38H
        [Test]
        public void RST38() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0xff) \"RST 38H\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0xff](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual((ushort) 56, regAfter.PC);
                Assert.AreEqual(regBefore.SP - 2, regAfter.SP);

                regAfter.RandomizeRegisters();
                regAfter.LoadRegs();

                var regs = new [] {"A", "B", "C", "D", "E", "F", "H", "L"};
                var savedMatch = regs
                    .Select((reg) => regBefore.GetRegister(reg) == regAfter.GetRegister(reg))
                    .Aggregate((a, b) => a && b);

                Assert.IsTrue(savedMatch);

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