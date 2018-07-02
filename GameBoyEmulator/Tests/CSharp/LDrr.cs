        #region #region Test {instr}
        [Test]
        public void LDrr{regI}{regO}() {{
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
                Assert.AreEqual(regAfter.{regI}, regBefore.{regO});
                #endregion

                {asserts}
                {flags}
            }}
        }}
        #endregion