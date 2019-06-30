        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void LD{Arg0}{Arg1}m{Arg2}() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.{Arg0} = 0xA0;
                cpu.reg.{Arg1} = (byte) random.Next(0x00, 0xFF);

                var hl = (cpu.reg.{Arg0} << 8) + cpu.reg.{Arg1};

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.{Arg2}, cpu.memory.ReadByte(hl));
                Assert.AreEqual(regBefore.{Arg2}, regAfter.{Arg2});

                {asserts}
                {flags}
            }}
        }}
        #endregion