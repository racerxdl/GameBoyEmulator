        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void LDHLSPr() {{
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

                Assert.AreEqual(regBefore.HL, regAfter.SP);

                {asserts}
                {flags}
            }}
        }}
        #endregion