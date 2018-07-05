        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void DAA() {{
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

                var a = (int)regBefore.A;

                if (regBefore.FlagSub) {{
                    if (regBefore.FlagHalfCarry) {{
                        a = a - 0x6;
                    }} else {{
                        a -= 0x60;
                    }}
                }} else {{
                    if (regBefore.FlagHalfCarry || (a & 0xF) > 0x9) {{
                        a += 0x06;
                    }} else {{
                        a += 0x60;
                    }}
                }}

                var zero = a == 0;
                var carry = ((a & 0x100) == 0x100) || regBefore.FlagCarry;

                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(zero, regAfter.FlagZero);
                Assert.AreEqual(a & 0xFF, regAfter.A);

                {asserts}
                {flags}
            }}
        }}
        #endregion