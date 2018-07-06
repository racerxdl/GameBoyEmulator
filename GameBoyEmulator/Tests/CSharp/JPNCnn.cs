        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void JPNCnn() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.PC = (ushort) ((0xA1 << 8) + random.Next(0x00, 0xF0));

                var valA = (ushort) random.Next(0x0000, 0xFFFF);

                cpu.memory.WriteWord(cpu.reg.PC, valA);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                if (!regBefore.FlagCarry) {{
                    Assert.AreEqual(valA, regAfter.PC);
                    Assert.AreEqual(regAfter.lastClockT, {cycles[0]});
                    Assert.AreEqual(regAfter.lastClockM, {cycles[0]} / 4);
                }} else {{
                    Assert.AreEqual(regBefore.PC + 2, regAfter.PC);
                    Assert.AreEqual(regAfter.lastClockT, {cycles[1]});
                    Assert.AreEqual(regAfter.lastClockM, {cycles[1]} / 4);
                }}

                {asserts}
                {flags}
            }}
        }}
        #endregion