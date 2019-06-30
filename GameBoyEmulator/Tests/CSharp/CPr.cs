        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void CPr{Arg0}() {{
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

                var halfCarry = (regBefore.A & 0xF) < (regBefore.{Arg0} & 0xF);
                var carry = regBefore.A < regBefore.{Arg0};

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == regBefore.{Arg0}, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion