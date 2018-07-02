        #region Test {instr}
        [Test]
        public void LD{regO}{regH}{regL}m() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.{regH} = 0xFF;
                cpu.reg.{regL} = (byte) (0x80 + random.Next(0x00, 0x50));

                var hl = (cpu.reg.{regH} << 8) + cpu.reg.{regL};
                var val = (byte) (0x80 + random.Next(0x00, 0xFF));
                cpu.memory.WriteByte(hl, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.{regO});

                {asserts}
                {flags}
            }}
        }}
        #endregion