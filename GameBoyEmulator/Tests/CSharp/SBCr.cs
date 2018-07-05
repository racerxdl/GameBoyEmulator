        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void SBCr{regI}() {{
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

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A - regBefore.{regI} - f;
                var halfCarry = (regBefore.A & 0xF) < ((regBefore.{regI} & 0xF) + f);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("{regI}" != "A") {{
                    Assert.AreEqual(regBefore.{regI}, regAfter.{regI});
                }}
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(sum < 0, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion