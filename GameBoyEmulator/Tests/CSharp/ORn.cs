        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void ORn() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x00, 0xFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                val = (byte) (regBefore.A | val);

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(val, regAfter.A);
                Assert.AreEqual(val == 0, regAfter.FlagZero);

                {asserts}
                {flags}
            }}
        }}
        #endregion