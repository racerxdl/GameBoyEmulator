#!/usr/bin/env python

'''
Test Templates
'''

regList = ["A", "B", "C", "D", "E", "F", "H", "L", "HL", "PC", "SP"]

cpuTestFile = '''
namespace GameBoyEmulator.Desktop.Tests {
    [TestFixture]
    public class CPUTest {
        private const int RUN_CYCLES = 10;

        {TESTS}
    }
}
'''

baseTestTemplate = '''
        [Test]
        public void TestOpcode{OPCODE}() {
            var cpu = new CPU();
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{OPCODE}](cpu);
                var regAfter = cpu.reg.Clone();

                {CHECKS}
            }
        }
'''

baseTestCBTemplate = '''
        [Test]
        public void TestOpcodeCB{OPCODE}() {
            var cpu = new CPU();
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.CBOPS[0x{OPCODE}](cpu);
                var regAfter = cpu.reg.Clone();

                {CHECKS}
            }
        }
'''

cycleTestTemplate = '''
                #region Test Cycles
                Assert.AreEqual(regAfter.lastClockT, %s);
                Assert.AreEqual(regAfter.lastClockM, %s);
                #endregion
'''

def GenFlagAssert(flags):
  flagAssert = '''
                #region Flag Tests\n'''
  if flags["carry"] == False or flags["carry"] == True:
    flagAssert = flagAssert + "                Assert.AreEqual(%s, regAfter.FlagCarry);\n" % str(flags["carry"]).lower()
  elif flags["carry"] == None:
    flagAssert = flagAssert + "                Assert.AreEqual(regAfter.FlagCarry, regBefore.FlagCarry);\n"

  if flags["halfcarry"] == False or flags["halfcarry"] == True:
    flagAssert = flagAssert + "                Assert.AreEqual(%s, regAfter.FlagHalfCarry);\n" % str(flags["halfcarry"]).lower()
  elif flags["halfcarry"] == None:
    flagAssert = flagAssert + "                Assert.AreEqual(regAfter.FlagHalfCarry, regBefore.FlagHalfCarry);\n"

  if flags["sub"] == False or flags["sub"] == True:
    flagAssert = flagAssert + "                Assert.AreEqual(%s, regAfter.FlagSub);\n" % str(flags["sub"]).lower()
  elif flags["sub"] == None:
    flagAssert = flagAssert + "                Assert.AreEqual(regAfter.FlagSub, regBefore.FlagSub);\n"

  if flags["zero"] == False or flags["zero"] == True:
    flagAssert = flagAssert + "                Assert.AreEqual(%s, regAfter.FlagZero);\n" % str(flags["zero"]).lower()
  elif flags["zero"] == None:
    flagAssert = flagAssert + "                Assert.AreEqual(regAfter.FlagZero, regBefore.FlagZero);\n"

  flagAssert = flagAssert + "                #endregion"
  return flagAssert

def LDrr(instr, opcode, regI, regO, cycles, flags):
  asserts = '''#region Test Difference
                Assert.AreEqual(regAfter.%s, regBefore.%s);
                #endregion\n
                #region Test no change to other regs\n''' % (regI, regO)

  for regA in regList:
    if regA != regI:
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return '''
        [Test]
        public void LDrr_%s%s() {
            var cpu = new CPU();
            Console.WriteLine("Testing (0x%02x) \\"%s\\"");
            for (var i = 0; i < RUN_CYCLES; i++) {
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x%02x](cpu);
                var regAfter = cpu.reg.Clone();

                %s
                %s
            }
        }
  ''' %(regI, regO, opcode, instr, opcode, asserts, GenFlagAssert(flags))

def LDrHLm_(instr, opcode, regO, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regO:
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return '''
        [Test]
        public void LDrHLm_%s() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x%02x) \\"%s\\"");
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
                CPUInstructions.opcodes[0x%02x](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regAfter.%s, val);

                %s
                %s
            }
        }
  ''' %(regO, opcode, instr, opcode, regO, asserts, GenFlagAssert(flags))

def LDrn_(instr, opcode, regO, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regO and regA != "PC":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return '''
        [Test]
        public void LDrn_%s() {
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x%02x) \\"%s\\"");
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
                CPUInstructions.opcodes[0x%02x](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.%s);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);

                %s
                %s
            }
        }
  ''' %(regO, opcode, instr, opcode, regO, asserts, GenFlagAssert(flags))



TestTemplates = {
  "LDrr": LDrr,
  "LDrHLm_": LDrHLm_,
  "LDrn_": LDrn_
}

print TestTemplates["LDrr"]("LDrr A, B", 0x78, "A", "B", 4, {'carry': None, 'halfcarry': None, 'sub': None, 'zero': None})
print TestTemplates["LDrHLm_"]("LD A, [HL]", 0x7E, "A", 8, {'carry': None, 'halfcarry': None, 'sub': None, 'zero': None})
print TestTemplates["LDrn_"]("LD A, d8", 0x3E, "A", 8, {'carry': None, 'halfcarry': None, 'sub': None, 'zero': None})