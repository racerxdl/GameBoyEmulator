        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void ADDSPn() {{
            var cpu = new CPU();
            var random = new Random();
            Console.WriteLine("Testing (0x{opcode:02x}) \"{instr}\"");
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x00, 0xFF));
                var signedV = random.Next(-128, 0);
                var v = (byte) signedV;

                cpu.memory.WriteByte(cpu.reg.PC, v);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.SP + signedV;
                var halfCarry = (regBefore.SP & 0xF) + (signedV & 0xF) > 0xF;
                var carry = (regBefore.SP & 0xFF) + (signedV & 0xFF) > 0xFF;

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(sum & 0xFFFF, regAfter.SP);
                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
            for (var i = 0; i < RUN_CYCLES; i++) {{
                cpu.Reset();
                cpu.reg.RandomizeRegisters();
                cpu.memory.RandomizeMemory();

                // Force write to Catridge Ram Random Address (avoid writting to non writeable addresses)
                cpu.reg.PC = (ushort)((0xA0 << 8) + random.Next(0x00, 0xFF));
                var signedV = random.Next(0, 127);
                var v = (byte) signedV;

                cpu.memory.WriteByte(cpu.reg.PC, v);

                var regBefore = cpu.reg.Clone();
                CPUInstructions.opcodes[0x{opcode:02x}](cpu);
                var regAfter = cpu.reg.Clone();

                var sum = regBefore.SP + signedV;
                var halfCarry = (regBefore.SP & 0xF) + (signedV & 0xF) > 0xF;
                var carry = (regBefore.SP & 0xFF) + (signedV & 0xFF) > 0xFF;

                Assert.AreEqual(regBefore.PC + 1, regAfter.PC);
                Assert.AreEqual(sum & 0xFFFF, regAfter.SP);
                Assert.AreEqual(carry, regAfter.FlagCarry);
                Assert.AreEqual(halfCarry, regAfter.FlagHalfCarry);

                {asserts}
                {flags}
            }}
        }}
        #endregion