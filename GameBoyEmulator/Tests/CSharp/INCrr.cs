        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void INC{regA}{regB}() {{
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

                var valA = (byte) (regBefore.{regB} + 1);
                var valB = (byte) (valA == 0 ? regBefore.{regA} + 1 : regBefore.{regA});

                Assert.AreEqual(valA, regAfter.{regB});
                Assert.AreEqual(valB, regAfter.{regA});

                {asserts}
                {flags}
            }}
        }}
        #endregion