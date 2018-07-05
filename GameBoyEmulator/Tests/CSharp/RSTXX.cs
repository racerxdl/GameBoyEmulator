        #region 0x{opcode:02x} Test {instr}
        [Test]
        public void RST{addr:02x}() {{
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

                Assert.AreEqual((ushort) {addr}, regAfter.PC);
                Assert.AreEqual(regBefore.SP - 2, regAfter.SP);

                regAfter.RandomizeRegisters();
                regAfter.LoadRegs();

                var regs = new [] {{"A", "B", "C", "D", "E", "F", "H", "L"}};
                var savedMatch = regs
                    .Select((reg) => regBefore.GetRegister(reg) == regAfter.GetRegister(reg))
                    .Aggregate((a, b) => a && b);

                Assert.IsTrue(savedMatch);

                {asserts}
                {flags}
            }}
        }}
        #endregion