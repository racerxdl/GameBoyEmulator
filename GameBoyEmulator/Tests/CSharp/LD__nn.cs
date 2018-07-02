        #region Test {instr}
        [Test]
        public void LD{regO1}{regO2}nn() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));
                var var0 = (byte) random.Next(0x00, 0xFF);
                var var1 = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, var0);
                cpu.memory.WriteByte(cpu.reg.PC + 1, var1);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(var0, regAfter.{regO2});
                Assert.AreEqual(var1, regAfter.{regO1});
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                {asserts}
                {flags}
            }}
        }}
        #endregion