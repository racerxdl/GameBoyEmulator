        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void ADDr{Arg0}() {{
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

                var sum = regBefore.A + regBefore.{Arg0};
                var halfCarry = (sum & 0xF) < (regBefore.A & 0xF);

                Assert.AreEqual((byte) sum, regAfter.A);
                if ("{Arg0}" != "A") {{
                    Assert.AreEqual(regBefore.{Arg0}, regAfter.{Arg0});
                }}

                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion