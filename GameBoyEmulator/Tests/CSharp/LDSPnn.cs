        #region Test {instr}
        [Test]
        public void LDSPnn() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));
                var var0 = (ushort) random.Next(0x0000, 0xFFFF);

                cpu.memory.WriteWord(cpu.reg.PC, var0);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(var0, regAfter.SP);
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                {asserts}
                {flags}
            }}
        }}
        #endregion