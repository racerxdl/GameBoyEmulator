        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void CPn() {{
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


                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                var halfCarry = (regBefore.A & 0xF) < (val & 0xF);
                var carry = regBefore.A < val;

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(regBefore.A == val, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion