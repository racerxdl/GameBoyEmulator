#!/usr/bin/env python

import sys

sys.path.append('../')

from Generators.instructions import Instructions
from Generators.instructions_cb import InstructionsCB
from TestTemplates import *


print "Generating CPU Tests at CPUTest.cs"


tests = ""

for i in Instructions:
    if TestTemplates.has_key(i["templateData"]["name"]):
        tpl = TestTemplates[i["templateData"]["name"]]
        tests = tests + tpl(i["instruction"], i["code"], i["templateData"]["args"], i["cycles"], i["flagResult"])
        tests = tests + "\n"


f = open("CPUTest.cs", "w")
f.write(LoadTPL("CPUTest").format(tests=tests))
f.close()

