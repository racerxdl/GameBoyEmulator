        #region Test {instr}
        [Test]
        public void LDIOnA() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + (byte) random.Next(0x00, 0xFF));

                cpu.memory.WriteByte(cpu.reg.PC, 0x80);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.A, cpu.memory.ReadByte(0xFF80));
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);

                {asserts}
                {flags}
            }}
        }}
        #endregion