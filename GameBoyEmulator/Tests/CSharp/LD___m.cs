        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void LD{Arg0}{Arg1}{Arg2}m() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.{Arg1} = 0xA0;
                cpu.reg.{Arg2} = (byte) random.Next(0x00, 0xFF);

                var hl = (cpu.reg.{Arg1} << 8) + cpu.reg.{Arg2};
                var val = (byte) random.Next(0x00, 0xFF);
                cpu.memory.WriteByte(hl, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(val, regAfter.{Arg0});

                {asserts}
                {flags}
            }}
        }}
        #endregion