#!/usr/bin/env python

import sys
import re

if len(sys.argv) < 2:
  print "Usage: sharptogo.py file.cs"
  exit(1)

csfile = sys.argv[1]
gofile = sys.argv[1].replace(".cs", ".gotpl").replace("CSharp", "GoLang")

print "Converting %s -> %s" %(csfile, gofile)

f = open(csfile)
data = f.read()
f.close()

data = data\
  .replace(r"#region", "// region") \
  .replace(r"#endregion", "// endregion") \
  .replace(r"[Test]", "") \
  .replace(r"{instr}", "{{.Instruction}}") \
  .replace(r"var cpu = new CPU();", "cpu := MakeCore()") \
  .replace(r"cpu.Reset();", "cpu.Reset()") \
  .replace(r"cpu.reg.RandomizeRegisters();", "cpu.Registers.Randomize()") \
  .replace(r"cpu.memory.RandomizeMemory();", "cpu.Memory.Randomize()") \
  .replace(r"var regBefore = cpu.reg.Clone();", "regBefore := cpu.Registers.Clone()") \
  .replace(r"var regAfter = cpu.reg.Clone();", "regAfter := cpu.Registers.Clone()") \
  .replace(r"CPUInstructions.opcodes[0x{opcode:02x}](cpu);", r"GBInstructions[0x{{.OpCodeX}}](cpu)") \
  .replace(r"for (var i = 0; i < RUN_CYCLES; i++) {{", "for i := 0; i < RunCycles; i++ {") \
  .replace(r"Console.WriteLine", "// Console.WriteLine") \
  .replace(r"var random = new Random();", "") \
  .replace(r"cpu.reg.", "cpu.Registers.") \
  .replace(r".FlagCarry", ".GetCarry()") \
  .replace(r".FlagHalfCarry", ".GetHalfCarry()") \
  .replace(r".FlagSub", ".GetSub()") \
  .replace(r".FlagZero", ".GetZero()") \
  .replace(r"cpu.memory", "cpu.Memory") \
  .replace(r"{opcode:02x}", r"{{.OpCodeX}}") \
  .replace(r";\n", "\n") \
  .replace(r"{asserts}", r"{{.Asserts}}") \
  .replace(r"{flags}", r"{{.Flags}}") \
  .replace(r"cpu.Registers.HL", "cpu.Registers.HL()") \
  .replace("regO", "RegO") \
  .replace("regO1", "RegO1") \
  .replace("regO2", "RegO2") \
  .replace("regI", "RegI") \
  .replace("regI1", "RegI1") \
  .replace("regI2", "RegI2") \
  .replace("regH", "RegH") \
  .replace("regL", "RegL") \
  .replace("regA", "RegA") \
  .replace("regB", "RegB") \

data = re.sub(r'public void (.*?)\(\) {{', r'func Test\g<1>(t *testing.T) {', data)
data = re.sub(r'\(byte\)(.*?)\n', r'uint8(\g<1>)\n', data)
data = re.sub(r'\(ushort\)(.*?)\n', r'uint16(\g<1>)\n', data)
data = re.sub(r'Assert.AreEqual\((.*?), (.*?)\);\n', r'''
                if (\g<1>) != (\g<2>) {
                    t.Errorf("Expected \g<1> to be %v but got %v", \g<1>, \g<2>)
                }''', data)
data = re.sub(r'if \((.*)\) {{', r'if \g<1> {', data)
data = re.sub(r'var (.*?) = (.*?) \? (.*?) : (.*?)', r'''
                \g<1> := \g<4>

                if \g<2> {
                  \g<1> = \g<3>
                }
''', data)
data = re.sub(r'~(.*)', r'^\g<1>', data)
data = re.sub(r'random.Next\((.*?), (.*?)\);', 'rand.Intn(\g<2>)', data)
data = re.sub(r'random.Next\((.*?), (.*?)\)', 'rand.Intn(\g<2>)', data)
data = data  .replace("{{", "{").replace("}}", "}")
data = re.sub(r'{(.*?)}', r'%%%%%%\g<1>%%%%%%', data)
data = re.sub(r'%%%%%%(.*?)%%%%%%', r'{{\g<1>}}', data)
data = re.sub(r'{{\.(.*?)}}', r'{{\g<1>}}', data)
data = re.sub(r'{{(.*?)}}', r'{{.\g<1>}}', data)
data = data.replace(".cycles", ".Cycles")
data = re.sub(r'{{\.(.*?)\[(.*?)\]}}', r'{{index .\g<1> \g<2>}}', data)
data = data.replace(";\n", "\n")

# regx = "public void ADCr{regI}() {{"

#print data
f = open(gofile, "w")
f.write(data)
f.close()