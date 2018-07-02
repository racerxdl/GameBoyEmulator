using System;
using System.Diagnostics;
using GameBoyEmulator.Desktop.GBC;
using NUnit.Framework;

namespace GameBoyEmulator.Desktop.Tests {
    [TestFixture]
    public class CPUTest {
        private const int RUN_CYCLES = 10;
        
        [Test]
        public void LDrr_AB() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x78) \"LDrr A, B\"");
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
  

        [Test]
        public void LDrHLm_A() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x7e) \"LD A, [HL]\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xFF;
                cpu.reg.L = (byte) (0x80 + random.Next(0x00, 0x50));

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x7e](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regAfter.A, val);

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
  

        [Test]
        public void LDrn_A() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x3e) \"LD A, d8\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xFF;
                cpu.reg.L = (byte) (0x80 + random.Next(0x00, 0x50));

                cpu.reg.PC = cpu.reg.HL; // Put PC in High Ram random value;

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x3e](cpu);
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

    }
}
