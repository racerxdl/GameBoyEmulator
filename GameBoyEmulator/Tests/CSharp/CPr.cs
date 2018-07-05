        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void CPr{regI}() {{
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

                var halfCarry = (regBefore.A & 0xF) < (regBefore.{regI} & 0xF);
                var carry = regBefore.A < regBefore.{regI};

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == regBefore.{regI}, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion