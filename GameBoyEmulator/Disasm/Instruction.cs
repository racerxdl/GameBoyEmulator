namespace GameBoyEmulator.Desktop.Disasm {
    public struct Instruction {
        public string Value { get; set; }
        public byte Opcode { get; set; }
        public int Length { get; set; }
        public string[] Flags { get; set; }
    }
}
