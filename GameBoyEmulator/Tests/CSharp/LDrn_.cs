        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void LDrn{Arg0}() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to High Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.H = 0xA0;
                cpu.reg.L = (byte) random.Next(0x00, 0xFF);

                cpu.reg.PC = cpu.reg.HL; // Put PC in High Ram random value;

                var val = (byte) random.Next(0x00, 0xFF);

                cpu.memory.WriteByte(cpu.reg.PC, val);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                #region Test Difference
                Assert.AreEqual(val, regAfter.{Arg0});
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                #endregion

                {asserts}
                {flags}
            }}
        }}
        #endregion