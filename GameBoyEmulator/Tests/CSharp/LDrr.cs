        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void LDrr{Arg0}{Arg1}() {{
            var cpu = new CPU();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(regAfter.{Arg0}, regBefore.{Arg1});
                #endregion

                {asserts}
                {flags}
            }}
        }}
        #endregion