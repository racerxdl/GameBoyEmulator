        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void PUSH{regA}{regB}() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.SP = (ushort) ((0xA1 << 8) + random.Next(0x00, 0xF0));

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                var valB = cpu.memory.ReadByte(regAfter.SP);
                var valA = cpu.memory.ReadByte(regAfter.SP+1);

                Assert.AreEqual(regBefore.SP - 2, regAfter.SP);
                Assert.AreEqual(regBefore.{regA}, valA);
                Assert.AreEqual(regBefore.{regB}, valB);

                {asserts}
                {flags}
            }}
        }}
        #endregion