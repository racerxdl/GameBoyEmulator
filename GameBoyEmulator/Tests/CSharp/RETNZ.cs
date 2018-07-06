        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void RETNZ() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.SP = (ushort) ((0xA1 << 8) + random.Next(0x00, 0xF0));

                var valA = (ushort) random.Next(0x0000, 0xFFFF);

                cpu.memory.WriteWord(cpu.reg.SP, valA);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                if (!regBefore.FlagZero) {{
                    Assert.AreEqual(valA, regAfter.PC);
                    Assert.AreEqual({cycles[0]}, regAfter.lastClockT);
                    Assert.AreEqual({cycles[0]} / 4, regAfter.lastClockM);
                    Assert.AreEqual(regBefore.SP + 2, regAfter.SP);
                }} else {{
                    Assert.AreEqual({cycles[1]}, regAfter.lastClockT);
                    Assert.AreEqual({cycles[1]} / 4, regAfter.lastClockM);
                    Assert.AreEqual(regBefore.SP, regAfter.SP);
                }}

                {asserts}
                {flags}
            }}
        }}
        #endregion