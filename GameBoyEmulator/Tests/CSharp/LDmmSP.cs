        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void LDmmSP() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));
                var addr = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));

                cpu.memory.WriteWord(cpu.reg.PC, addr);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadWord(addr), regAfter.SP);
                Assert.AreEqual(regBefore.PC + 2, regAfter.PC);

                {asserts}
                {flags}
            }}
        }}
        #endregion