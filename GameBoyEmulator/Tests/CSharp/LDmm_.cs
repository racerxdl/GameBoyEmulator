        #region Test {instr}
        [Test]
        public void LDmm{regI}() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xFF << 8) + (0x80 + random.Next(0x00, 0x50)));

                var addr = (ushort) ((0xFF << 8) + (0x80 + random.Next(0x00, 0x50)));

                cpu.memory.WriteWord(cpu.reg.PC, addr);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(addr), regBefore.{regI});
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                {asserts}
                {flags}
            }}
        }}
        #endregion