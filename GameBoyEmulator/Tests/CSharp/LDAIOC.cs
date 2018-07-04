        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void LDAIOC() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.C = 0x80;
                var val = (byte) random.Next(0x00, 0x10);

                cpu.memory.WriteByte(0xFF80, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);

                {asserts}
                {flags}
            }}
        }}
        #endregion