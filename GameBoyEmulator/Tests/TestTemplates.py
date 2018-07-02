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

def LoadTPL(tplname):
  f = open("CSharp/%s.cs" % tplname)
  tpl = f.read()
  f.close()
  return tpl

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

def LDrr(instr, opcode, args, cycles, flags):
  regI, regO = args
  asserts = '''
                #region Test no change to other regs\n'''

  for regA in regList:
    if regA != regI and not ((regI == "L" or regI == "H") and regA == "HL"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))
  return LoadTPL("LDrr").format(
    regI=regI,
    regO=regO,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDrHLm_(instr, opcode, args, cycles, flags):
  regO, = args
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regO and not ((regO == "L" or regO == "H") and regA == "HL"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDrHLm_").format(
    regO=regO,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDHLmr_(instr, opcode, args, cycles, flags):
  regI, = args
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regI:
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDHLmr_").format(
    regI=regI,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDrn_(instr, opcode, args, cycles, flags):
  regO, = args
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regO and regA != "PC" and not ((regO == "L" or regO == "H") and regA == "HL"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDrn_").format(
    regO=regO,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDHLmn(instr, opcode, args, cycles, flags):
  regO, = args
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regO and regA != "PC" and not ((regO == "L" or regO == "H") and regA == "HL"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDHLmn").format(
    regO=regO,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LD__m_(instr, opcode, args, cycles, flags):
  regH, regL, regI = args
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regI and not ((regI == "L" or regI == "H") and regA == "HL"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LD__m_").format(
    regH=regH,
    regL=regL,
    regI=regI,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDmm_(instr, opcode, args, cycles, flags):
  regI, = args
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regI and regA != "PC" and not ((regI == "L" or regI == "H") and regA == "HL"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDmm_").format(
    regI=regI,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LD___m(instr, opcode, args, cycles, flags):
  regO, regH, regL = args
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regO and not ((regO == "L" or regO == "H") and regA == "HL"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LD___m").format(
    regH=regH,
    regL=regL,
    regO=regO,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LD_mm(instr, opcode, args, cycles, flags):
  regO, = args
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regO and regA != "PC" and not ((regO == "L" or regO == "H") and regA == "HL"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LD_mm").format(
    regO=regO,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LD__nn(instr, opcode, args, cycles, flags):
  regO1, regO2 = args
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != regO1 and regA != regO2 and regA != "PC" and not ((regO1 == "L" or regO1 == "H" or regO2 == "L" or regO2 == "H") and regA == "HL"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LD__nn").format(
    regO1=regO1,
    regO2=regO2,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDSPnn(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "SP" and regA != "PC":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDSPnn").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDmmSP(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "SP" and regA != "PC":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDmmSP").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

TestTemplates = {
  "LDrr": LDrr,
  "LDrHLm_": LDrHLm_,
  "LDrn_": LDrn_,
  "LDHLmr_": LDHLmr_,
  "LD__m_": LD__m_,
  "LDmm_": LDmm_,
  "LD___m": LD___m,
  "LD_mm": LD_mm,
  "LD__nn": LD__nn,
  "LDSPnn": LDSPnn,
  "LDmmSP": LDmmSP,
}

#print TestTemplates["LDrr"]("LDrr A, B", 0x78, ["A", "B"], 4, {'carry': None, 'halfcarry': None, 'sub': None, 'zero': None})
#print TestTemplates["LDrHLm_"]("LD A, [HL]", 0x7E, "A", 8, {'carry': None, 'halfcarry': None, 'sub': None, 'zero': None})
#print TestTemplates["LDrn_"]("LD A, d8", 0x3E, "A", 8, {'carry': None, 'halfcarry': None, 'sub': None, 'zero': None})
