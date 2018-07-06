#!/usr/bin/env python

import pprint
import re

templateRgx = re.compile("(.*)\[(.*)\]")

def LoadTPL(tplname):
  f = open("CSharp/%s.cs" % tplname)
  tpl = f.read()
  f.close()
  return tpl

def GenDisasm(opcodes, cbopcodes):
  instructions = ""
  cbinstructions = ""
  tpl = LoadTPL("Instruction")
  for ins in opcodes:
    instructions += tpl.format(
      flags = "\", \"".join(ins["simpleFlags"]),
      value = ins["instruction"],
      opcode = ins["code"],
      length = ins["size"],
    )
    instructions += "\n"
  for ins in cbopcodes:
    cbinstructions += tpl.format(
      flags = "\", \"".join(ins["simpleFlags"]),
      value = ins["instruction"],
      opcode = ins["code"],
      length = ins["size"],
    )
    instructions += "\n"

  return LoadTPL("DisasmInstructions").format(
    Instructions = instructions,
    CBInstructions = cbinstructions,
  )

def Gen(data):
  opcodes = []
  code = 0x00

  for i in data:
    if not i.startswith("//") and not i.startswith("#") and not len(i.strip()) == 0 and i != None:
      z = filter(None, i.split("|"))
      if len(z) == 5:
        name, instruction, cycles, flags, template = z
        numBytes = 1
      else:
        name, instruction, cycles, flags, template, numBytes = z
      if "/" in cycles:
        cycles = [ int(z) for z in cycles.split("/") ]
      else:
        cycles = int(cycles)

      flagResult = {
        "zero":       True if flags[0] == "1" else False if flags[0] == "0" else "Z" if flags[0] == "Z" else None,
        "sub":        True if flags[1] == "1" else False if flags[1] == "0" else "N" if flags[1] == "N" else None,
        "halfcarry":  True if flags[2] == "1" else False if flags[2] == "0" else "H" if flags[2] == "H" else None,
        "carry":      True if flags[3] == "1" else False if flags[3] == "0" else "C" if flags[3] == "C" else None
      }

      z = templateRgx.match(template)

      if z == None:
        raise "Error matching template on %s" %template

      templateName, templateArgs = z.groups()

      opcodes.append({
        "code": code,
        "hexCode": ("%02x" %code).upper(),
        "name": name,
        "instruction": instruction,
        "cycles": cycles,
        "flagResult": flagResult,
        "templateData": {
          "name": templateName,
          "args": filter(lambda x: len(x) != 0, [ x.replace("\"", "").strip() for x in templateArgs.split(",") ])
        },
        "simpleFlags": list(flags),
        "size": numBytes,
      })
      code = code + 1

  return opcodes


print "Generating instructions.py"

f = open("instructions.txt")
data = f.read().split("\n")
f.close()

opcodes = Gen(data)
formattedOpcodes = pprint.pformat(opcodes)

f = open("instructions.py", "w")
f.write("#!/usr/bin/env python\n\n")
f.write("# AUTO GENERATED FILE - PLEASE CHECK gen.py! #\n\n")
f.write("Instructions = %s" %formattedOpcodes)
f.write("\n")
f.close()

print "Generating instructions_cb.py"
f = open("instructions_cb.txt")
data = f.read().split("\n")
f.close()

cbopcodes = Gen(data)
formattedOpcodes = pprint.pformat(cbopcodes)

f = open("instructions_cb.py", "w")
f.write("#!/usr/bin/env python\n\n")
f.write("# AUTO GENERATED FILE - PLEASE CHECK gen.py! #\n\n")
f.write("InstructionsCB = %s" %formattedOpcodes)
f.write("\n")
f.close()

disasm = GenDisasm(opcodes, cbopcodes)

f = open("../Disasm/DisasmInstructions.cs", "w")
f.write(disasm)
f.write("\n")
f.close()