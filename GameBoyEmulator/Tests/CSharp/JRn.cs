        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void JRn() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.PC = (ushort) ((0xA1 << 8) + random.Next(0x00, 0xF0));

                var signedV = random.Next(-128, 0);
                var v = (byte) signedV;

                cpu.memory.WriteByte(cpu.reg.PC, v);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual((regBefore.PC + signedV + 1) & 0xFFFF, regAfter.PC);

                {asserts}
                {flags}
            }}
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                cpu.reg.PC = (ushort) ((0xA1 << 8) + random.Next(0x00, 0xF0));

                var signedV = random.Next(0, 127);
                var v = (byte) signedV;

                cpu.memory.WriteByte(cpu.reg.PC, v);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual((regBefore.PC + signedV + 1) & 0xFFFF, regAfter.PC);

                {asserts}
                {flags}
            }}
        }}
        #endregion