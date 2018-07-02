        #region Test {instr}
        [Test]
        public void LD{regH}{regL}m{regI}() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.{regH} = 0xA0;
                cpu.reg.{regL} = (byte) random.Next(0x00, 0xFF);

                var hl = (cpu.reg.{regH} << 8) + cpu.reg.{regL};

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.{regI}, cpu.memory.ReadByte(hl));
                Assert.AreEqual(regBefore.{regI}, regAfter.{regI});

                {asserts}
                {flags}
            }}
        }}
        #endregion