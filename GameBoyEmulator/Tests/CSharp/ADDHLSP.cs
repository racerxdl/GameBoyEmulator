        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void ADDHLSP() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.HL + regBefore.SP;
                var halfCarry = (regBefore.HL & 0xFFF) + (regBefore.SP & 0xFFF) > 0xFFF;

                Assert.AreEqual(sum & 0xFFFF, regAfter.HL);
                Assert.AreEqual(sum > 65535, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion