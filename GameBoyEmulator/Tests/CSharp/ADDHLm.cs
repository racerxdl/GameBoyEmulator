        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void ADDHLr() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)

                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.HL, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.A + val;
                var halfCarry = (regBefore.A & 0xF) + (val & 0xF) > 0xF;

                Assert.AreEqual((byte) sum, regAfter.A);
                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion