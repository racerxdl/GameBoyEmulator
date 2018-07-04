        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void LDAIOn() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + (byte) random.Next(0x00, 0xFF));

                var val = (byte) random.Next(0x00, 0x10);

                cpu.memory.WriteByte(cpu.reg.PC, 0x80);
                cpu.memory.WriteByte(0xFF80, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);

                {asserts}
                {flags}
            }}
        }}
        #endregion