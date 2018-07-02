#!/usr/bin/env python

import pprint
import re

templateRgx = re.compile("(.*)\[(.*)\]")

def Gen(data):
  opcodes = []
  code = 0x00

  for i in data:
    if not i.startswith("//") and not i.startswith("#") and not len(i.strip()) == 0 and i != None:
      name, instruction, cycles, flags, template = filter(None, i.split("|"))
      if "/" in cycles:
        cycles = [ int(z) for z in cycles.split("/") ]
      else:
        cycles = int(cycles)

      flagResult = {
        "zero":       True if flags[0] == "1" else False if flags[0] == "0" else "Z" if flags[0] == "Z" else None,
        "sub":        True if flags[1] == "1" else False if flags[1] == "0" else "N" if flags[0] == "N" else None,
        "halfcarry":  True if flags[2] == "1" else False if flags[2] == "0" else "H" if flags[0] == "H" else None,
        "carry":      True if flags[3] == "1" else False if flags[3] == "0" else "C" if flags[0] == "C" else None
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
          "args": [ x.replace("\"", "").strip() for x in templateArgs.split(",")]
        }
      })
      code = code + 1

  return pprint.pformat(opcodes)


print "Generating instructions.py"

f = open("instructions.txt")
data = f.read().split("\n")
f.close()
formattedOpcodes = Gen(data)

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

formattedOpcodes = Gen(data)

f = open("instructions_cb.py", "w")
f.write("#!/usr/bin/env python\n\n")
f.write("# AUTO GENERATED FILE - PLEASE CHECK gen.py! #\n\n")
f.write("InstructionsCB = %s" %formattedOpcodes)
f.write("\n")
f.close()
