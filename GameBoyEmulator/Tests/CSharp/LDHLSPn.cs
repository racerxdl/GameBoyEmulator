        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void LDHLSPn() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));

                var signedV = random.Next(-128, 0);
                var v = (byte) signedV;

                cpu.memory.WriteByte(cpu.reg.PC, v);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.SP + signedV, regAfter.HL);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual((regBefore.SP & 0xF) + (signedV & 0xF) > 0xF, regAfter.FlagHalfCarry);
                Assert.AreEqual((regBefore.SP & 0xFF) + (signedV & 0xFF) > 0xFF, regAfter.FlagCarry);

                {asserts}
                {flags}
            }}
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x000, 0xFFF));

                var signedV = random.Next(0, 127);
                var v = (byte) signedV;

                cpu.memory.WriteByte(cpu.reg.PC, v);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                Assert.AreEqual(regBefore.SP + signedV, regAfter.HL);
                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual((regBefore.SP & 0xF) + (signedV & 0xF) > 0xF, regAfter.FlagHalfCarry);
                Assert.AreEqual((regBefore.SP & 0xFF) + (signedV & 0xFF) > 0xFF, regAfter.FlagCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion