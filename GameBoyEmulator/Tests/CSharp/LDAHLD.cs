        #region Test {instr}
        [Test]
        public void LDAHLD() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(regBefore.HL - 1, regAfter.HL);

                {asserts}
                {flags}
            }}
        }}
        #endregion