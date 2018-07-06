        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void POP{regA}{regB}() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.SP = (ushort) ((0xA1 << 8) + random.Next(0x00, 0xF0));

                var valA = (byte) random.Next(0x00, 0xFF);
                var valB = (byte) random.Next(0x00, 0xFF);

                cpu.reg.SP--;
                cpu.memory.WriteByte(cpu.reg.SP, valA);
                cpu.reg.SP--;
                cpu.memory.WriteByte(cpu.reg.SP, valB);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.SP + 2, regAfter.SP);
                Assert.AreEqual(valA, regAfter.{regA});
                Assert.AreEqual(valB, regAfter.{regB});

                {asserts}
                {flags}
            }}
        }}
        #endregion