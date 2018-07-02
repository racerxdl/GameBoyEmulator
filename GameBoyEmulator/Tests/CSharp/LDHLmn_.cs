        #region Test {instr}
        [Test]
        public void LDHLmn{regO}() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xFF;
                cpu.reg.L = (byte) (0x80 + random.Next(0x00, 0x50));

                cpu.reg.PC = cpu.reg.HL; // Put PC in High Ram random value;

                var val = (byte) random.Next(0x00, 0x50);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                cpu.reg.H = 0xFF;
                cpu.reg.L = (byte) (0x80 + random.Next(0x00, 0x50));

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(cpu.memory.ReadByte(cpu.reg.HL), val);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(regAfter.{regO}, regBefore.{regO});

                {asserts}
                {flags}
            }}
        }}
        #endregion