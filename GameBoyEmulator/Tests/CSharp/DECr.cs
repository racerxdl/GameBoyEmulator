        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void DECr{Arg0}() {{
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

                var val = (byte) (regBefore.{Arg0} - 1);
                var halfCarry = (regBefore.{Arg0} & 0xF) == 0x00;

                Assert.AreEqual(val, regAfter.{Arg0});
                Assert.AreEqual(val == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion