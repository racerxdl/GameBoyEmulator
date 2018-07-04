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

def CheckFlagChange(flags):
  return not (flags["carry"] == None and flags["sub"] == None and flags["halfcarry"] == None and flags["zero"] == None)

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

def LDHLIA(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "H" and regA != "L" and regA != "HL":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDHLIA").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDAHLI(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "H" and regA != "L" and regA != "HL" and regA != "A":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDAHLI").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDHLDA(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "H" and regA != "L" and regA != "HL":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDHLDA").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDAHLD(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "H" and regA != "L" and regA != "HL" and regA != "A":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDAHLD").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDAIOn(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "PC" and regA != "A":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDAIOn").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDAIOnA(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "PC" and regA != "A":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDAIOnA").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDIOnA(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "PC":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDIOnA").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDAIOC(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "A":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDAIOC").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDIOCA(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "A":
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDIOCA").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def LDHLSPn(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "HL" and regA != "PC" and regA != "H" and regA != "L" and not (CheckFlagChange(flags) and regA == "F"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDHLSPn").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )


def LDHLSPr(instr, opcode, args, cycles, flags):
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "SP" and not (CheckFlagChange(flags) and regA == "F"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("LDHLSPr").format(
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def ADDr(instr, opcode, args, cycles, flags):
  regI, = args
  asserts = '''#region Test no change to other regs\n'''

  for regA in regList:
    if regA != "A" and regA != regI and not (CheckFlagChange(flags) and regA == "F"):
      asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

  asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

  return LoadTPL("ADDr").format(
    regI=regI,
    opcode=opcode,
    instr=instr,
    asserts=asserts,
    flags=GenFlagAssert(flags)
  )

def ADDHL(instr, opcode, args, cycles, flags):
  if len(args) == 0:
    asserts = '''#region Test no change to other regs\n'''

    for regA in regList:
      if regA != "A" and not (CheckFlagChange(flags) and regA == "F"):
        asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

    asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

    return LoadTPL("ADDHLm").format(
      opcode=opcode,
      instr=instr,
      asserts=asserts,
      flags=GenFlagAssert(flags)
    )
  else:
    regA_, regB_ = args
    asserts = '''#region Test no change to other regs\n'''

    for regA in regList:
      if regA != "HL" and regA != "H" and regA != "L" and not (CheckFlagChange(flags) and regA == "F"):
        asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

    asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

    return LoadTPL("ADDHLrr").format(
      regA = regA_,
      regB = regB_,
      opcode=opcode,
      instr=instr,
      asserts=asserts,
      flags=GenFlagAssert(flags)
    )


def ADDHLSP(instr, opcode, args, cycles, flags):
  if len(args) == 0:
    asserts = '''#region Test no change to other regs\n'''

    for regA in regList:
      if regA != "HL" and regA != "H" and regA != "L" and not (CheckFlagChange(flags) and regA == "F"):
        asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

    asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

    return LoadTPL("ADDHLSP").format(
      opcode=opcode,
      instr=instr,
      asserts=asserts,
      flags=GenFlagAssert(flags)
    )

def ADDSPn(instr, opcode, args, cycles, flags):
  if len(args) == 0:
    asserts = '''#region Test no change to other regs\n'''

    for regA in regList:
      if regA != "SP" and regA != "PC" and not (CheckFlagChange(flags) and regA == "F"):
        asserts = asserts + ("                Assert.AreEqual(regAfter.%s, regBefore.%s);\n" % (regA, regA))

    asserts = asserts + "                #endregion\n                %s" %(cycleTestTemplate %(cycles, cycles/4))

    return LoadTPL("ADDSPn").format(
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
  "LDHLIA": LDHLIA,
  "LDAHLI": LDAHLI,
  "LDHLDA": LDHLDA,
  "LDAHLD": LDAHLD,
  "LDAIOn": LDAIOn,
  "LDAIOn": LDAIOn,
  "LDIOnA": LDIOnA,
  "LDAIOC": LDAIOC,
  "LDIOCA": LDIOCA,
  "LDHLSPn": LDHLSPn,
  "LDHLSPr": LDHLSPr,
  "ADDr": ADDr,
  "ADDHL": ADDHL,
  "ADDHLSP": ADDHLSP,
  "ADDSPn": ADDSPn,
}

#print TestTemplates["LDrr"]("LDrr A, B", 0x78, ["A", "B"], 4, {'carry': None, 'halfcarry': None, 'sub': None, 'zero': None})
#print TestTemplates["LDrHLm_"]("LD A, [HL]", 0x7E, "A", 8, {'carry': None, 'halfcarry': None, 'sub': None, 'zero': None})
#print TestTemplates["LDrn_"]("LD A, d8", 0x3E, "A", 8, {'carry': None, 'halfcarry': None, 'sub': None, 'zero': None})
