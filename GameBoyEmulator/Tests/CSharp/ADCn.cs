        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void ADCn() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort) ((0xA0 << 8) + random.Next(0x00, 0xFF));
                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                var f = regBefore.FlagCarry ? 1 : 0;
                var sum = regBefore.A + val + f;
                var halfCarry = (regBefore.A & 0xF) + (val & 0xF) + f > 0xF;

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(sum & 0xFF, regAfter.A);

                Assert.AreEqual(sum > 255, regAfter.FlagCarry);
                Assert.AreEqual((sum & 0xFF) == 0, regAfter.FlagZero);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion