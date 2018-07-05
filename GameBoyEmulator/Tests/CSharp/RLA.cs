        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void RLA() {{
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

                var c = (regBefore.A >> 7) > 0;
                var f = regBefore.FlagCarry ? 1 : 0;
                var val = (byte) ((regBefore.A << 1) | f);

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(c, regAfter.FlagCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion