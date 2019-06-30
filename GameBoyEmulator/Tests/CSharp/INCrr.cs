        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void INC{Arg0}{Arg1}() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                var valA = (byte) (regBefore.{Arg1} + 1);
                var valB = (byte) (valA == 0 ? regBefore.{Arg0} + 1 : regBefore.{Arg0});

                Assert.AreEqual(valA, regAfter.{Arg1});
                Assert.AreEqual(valB, regAfter.{Arg0});

                {asserts}
                {flags}
            }}
        }}
        #endregion