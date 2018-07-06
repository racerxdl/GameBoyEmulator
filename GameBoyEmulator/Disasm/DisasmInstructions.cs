using System.Collections.Generic;

namespace GameBoyEmulator.Desktop.Disasm {
    public static class DisasmInstructions {
        public static readonly List<Instruction> Instructions = new List<Instruction> {
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x00,
                Value = "NOP"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0x01,
                Value = "LD BC, d16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x02,
                Value = "LD [BC], A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x03,
                Value = "INC BC"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "-" },
                Length = 1,
                Opcode = 0x04,
                Value = "INC B"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "-" },
                Length = 1,
                Opcode = 0x05,
                Value = "DEC B"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x06,
                Value = "LD B, d8"
            },
            new Instruction {
                Flags = new []{ "0", "0", "0", "C" },
                Length = 1,
                Opcode = 0x07,
                Value = "RLCA"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0x08,
                Value = "LD [a16], SP"
            },
            new Instruction {
                Flags = new []{ "-", "0", "H", "C" },
                Length = 1,
                Opcode = 0x09,
                Value = "ADD HL, BC"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x0a,
                Value = "LD A, [BC]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x0b,
                Value = "DEC BC"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "-" },
                Length = 1,
                Opcode = 0x0c,
                Value = "INC C"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "-" },
                Length = 1,
                Opcode = 0x0d,
                Value = "DEC C"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x0e,
                Value = "LD C, d8"
            },
            new Instruction {
                Flags = new []{ "0", "0", "0", "C" },
                Length = 1,
                Opcode = 0x0f,
                Value = "RRCA"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x10,
                Value = "STOP"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0x11,
                Value = "LD DE, d16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x12,
                Value = "LD [DE], A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x13,
                Value = "INC DE"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "-" },
                Length = 1,
                Opcode = 0x14,
                Value = "INC D"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "-" },
                Length = 1,
                Opcode = 0x15,
                Value = "DEC D"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x16,
                Value = "LD D, d8"
            },
            new Instruction {
                Flags = new []{ "0", "0", "0", "C" },
                Length = 1,
                Opcode = 0x17,
                Value = "RLA"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x18,
                Value = "JR r8"
            },
            new Instruction {
                Flags = new []{ "-", "0", "H", "C" },
                Length = 1,
                Opcode = 0x19,
                Value = "ADD HL, DE"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x1a,
                Value = "LD A, [DE]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x1b,
                Value = "DEC DE"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "-" },
                Length = 1,
                Opcode = 0x1c,
                Value = "INC E"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "-" },
                Length = 1,
                Opcode = 0x1d,
                Value = "DEC E"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x1e,
                Value = "LD E, d8"
            },
            new Instruction {
                Flags = new []{ "0", "0", "0", "C" },
                Length = 1,
                Opcode = 0x1f,
                Value = "RRA"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x20,
                Value = "JR NZ, r8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x21,
                Value = "LD HL, d16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x22,
                Value = "LD [HL+], A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x23,
                Value = "INC HL"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "-" },
                Length = 1,
                Opcode = 0x24,
                Value = "INC H"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "-" },
                Length = 1,
                Opcode = 0x25,
                Value = "DEC H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x26,
                Value = "LD H, d8"
            },
            new Instruction {
                Flags = new []{ "Z", "-", "0", "C" },
                Length = 1,
                Opcode = 0x27,
                Value = "DAA"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x28,
                Value = "JR Z, r8"
            },
            new Instruction {
                Flags = new []{ "-", "0", "H", "C" },
                Length = 1,
                Opcode = 0x29,
                Value = "ADD HL, HL"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x2a,
                Value = "LD A, [HL+]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x2b,
                Value = "DEC HL"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "-" },
                Length = 1,
                Opcode = 0x2c,
                Value = "INC L"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "-" },
                Length = 1,
                Opcode = 0x2d,
                Value = "DEC L"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x2e,
                Value = "LD L, d8"
            },
            new Instruction {
                Flags = new []{ "-", "1", "1", "-" },
                Length = 1,
                Opcode = 0x2f,
                Value = "CPL"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x30,
                Value = "JR NC, r8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x31,
                Value = "LD SP, d16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x32,
                Value = "LD [HL-], A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x33,
                Value = "INC SP"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "-" },
                Length = 1,
                Opcode = 0x34,
                Value = "INC [HL]"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "-" },
                Length = 1,
                Opcode = 0x35,
                Value = "DEC [HL]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x36,
                Value = "LD [HL], d8"
            },
            new Instruction {
                Flags = new []{ "-", "0", "0", "1" },
                Length = 1,
                Opcode = 0x37,
                Value = "SCF"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x38,
                Value = "JR C, r8"
            },
            new Instruction {
                Flags = new []{ "-", "0", "H", "C" },
                Length = 1,
                Opcode = 0x39,
                Value = "ADD HL, SP"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x3a,
                Value = "LD A, [HL-]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x3b,
                Value = "DEC SP"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "-" },
                Length = 1,
                Opcode = 0x3c,
                Value = "INC A"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "-" },
                Length = 1,
                Opcode = 0x3d,
                Value = "DEC A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0x3e,
                Value = "LD A, d8"
            },
            new Instruction {
                Flags = new []{ "-", "0", "0", "C" },
                Length = 1,
                Opcode = 0x3f,
                Value = "CCF"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x40,
                Value = "LD B, B"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x41,
                Value = "LD B, C"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x42,
                Value = "LD B, D"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x43,
                Value = "LD B, E"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x44,
                Value = "LD B, H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x45,
                Value = "LD B, L"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x46,
                Value = "LD B, [HL]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x47,
                Value = "LD B, A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x48,
                Value = "LD C, B"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x49,
                Value = "LD C, C"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x4a,
                Value = "LD C, D"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x4b,
                Value = "LD C, E"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x4c,
                Value = "LD C, H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x4d,
                Value = "LD C, L"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x4e,
                Value = "LD C, [HL]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x4f,
                Value = "LD C, A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x50,
                Value = "LD D, B"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x51,
                Value = "LD D, C"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x52,
                Value = "LD D, D"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x53,
                Value = "LD D, E"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x54,
                Value = "LD D, H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x55,
                Value = "LD D, L"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x56,
                Value = "LD D, [HL]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x57,
                Value = "LD D, A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x58,
                Value = "LD E, B"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x59,
                Value = "LD E, C"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x5a,
                Value = "LD E, D"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x5b,
                Value = "LD E, E"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x5c,
                Value = "LD E, H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x5d,
                Value = "LD E, L"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x5e,
                Value = "LD E, [HL]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x5f,
                Value = "LD E, A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x60,
                Value = "LD H, B"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x61,
                Value = "LD H, C"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x62,
                Value = "LD H, D"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x63,
                Value = "LD H, E"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x64,
                Value = "LD H, H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x65,
                Value = "LD H, L"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x66,
                Value = "LD H, [HL]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x67,
                Value = "LD H, A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x68,
                Value = "LD L, B"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x69,
                Value = "LD L, C"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x6a,
                Value = "LD L, D"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x6b,
                Value = "LD L, E"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x6c,
                Value = "LD L, H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x6d,
                Value = "LD L, L"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x6e,
                Value = "LD L, [HL]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x6f,
                Value = "LD L, A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x70,
                Value = "LD [HL], B"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x71,
                Value = "LD [HL], C"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x72,
                Value = "LD [HL], D"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x73,
                Value = "LD [HL], E"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x74,
                Value = "LD [HL], H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x75,
                Value = "LD [HL], L"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x76,
                Value = "HALT"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x77,
                Value = "LD [HL], A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x78,
                Value = "LD A, B"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x79,
                Value = "LD A, C"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x7a,
                Value = "LD A, D"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x7b,
                Value = "LD A, E"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x7c,
                Value = "LD A, H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x7d,
                Value = "LD A, L"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x7e,
                Value = "LD A, [HL]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x7f,
                Value = "LD A, A"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x80,
                Value = "ADD A, B"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x81,
                Value = "ADD A, C"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x82,
                Value = "ADD A, D"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x83,
                Value = "ADD A, E"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x84,
                Value = "ADD A, H"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x85,
                Value = "ADD A, L"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x86,
                Value = "ADD A, [HL]"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x87,
                Value = "ADD A, A"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x88,
                Value = "ADC A, B"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x89,
                Value = "ADC A, C"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x8a,
                Value = "ADC A, D"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x8b,
                Value = "ADC A, E"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x8c,
                Value = "ADC A, H"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x8d,
                Value = "ADC A, L"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x8e,
                Value = "ADC A, [HL]"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 1,
                Opcode = 0x8f,
                Value = "ADC A, A"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x90,
                Value = "SUB A, B"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x91,
                Value = "SUB A, C"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x92,
                Value = "SUB A, D"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x93,
                Value = "SUB A, E"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x94,
                Value = "SUB A, H"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x95,
                Value = "SUB A, L"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x96,
                Value = "SUB A, [HL]"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x97,
                Value = "SUB A, A"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x98,
                Value = "SBC A, B"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x99,
                Value = "SBC A, C"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x9a,
                Value = "SBC A, D"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x9b,
                Value = "SBC A, E"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x9c,
                Value = "SBC A, H"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x9d,
                Value = "SBC A, L"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x9e,
                Value = "SBC A, [HL]"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0x9f,
                Value = "SBC A, A"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "1", "0" },
                Length = 1,
                Opcode = 0xa0,
                Value = "AND A, B"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "1", "0" },
                Length = 1,
                Opcode = 0xa1,
                Value = "AND A, C"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "1", "0" },
                Length = 1,
                Opcode = 0xa2,
                Value = "AND A, D"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "1", "0" },
                Length = 1,
                Opcode = 0xa3,
                Value = "AND A, E"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "1", "0" },
                Length = 1,
                Opcode = 0xa4,
                Value = "AND A, H"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "1", "0" },
                Length = 1,
                Opcode = 0xa5,
                Value = "AND A, L"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "1", "0" },
                Length = 1,
                Opcode = 0xa6,
                Value = "AND A, [HL]"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "1", "0" },
                Length = 1,
                Opcode = 0xa7,
                Value = "AND A, A"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xa8,
                Value = "XOR A, B"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xa9,
                Value = "XOR A, C"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xaa,
                Value = "XOR A, D"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xab,
                Value = "XOR A, E"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xac,
                Value = "XOR A, H"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xad,
                Value = "XOR A, L"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xae,
                Value = "XOR A, [HL]"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xaf,
                Value = "XOR A, A"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xb0,
                Value = "OR A, B"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xb1,
                Value = "OR A, C"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xb2,
                Value = "OR A, D"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xb3,
                Value = "OR A, E"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xb4,
                Value = "OR A, H"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xb5,
                Value = "OR A, L"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xb6,
                Value = "OR A, [HL]"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0xb7,
                Value = "OR A, A"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0xb8,
                Value = "CP A, B"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0xb9,
                Value = "CP A, C"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0xba,
                Value = "CP A, D"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0xbb,
                Value = "CP A, E"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0xbc,
                Value = "CP A, H"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0xbd,
                Value = "CP A, L"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0xbe,
                Value = "CP A, [HL]"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 1,
                Opcode = 0xbf,
                Value = "CP A, A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc0,
                Value = "RET NZ"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc1,
                Value = "POP BC"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xc2,
                Value = "JP NZ, a16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xc3,
                Value = "JP a16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xc4,
                Value = "CALL NZ, a16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc5,
                Value = "PUSH BC"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 2,
                Opcode = 0xc6,
                Value = "ADD A, d8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc7,
                Value = "RST 00H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc8,
                Value = "RET Z"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc9,
                Value = "RET"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xca,
                Value = "JP Z, a16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 2,
                Opcode = 0xcb,
                Value = "PREFIX CB"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xcc,
                Value = "CALL Z, a16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xcd,
                Value = "CALL a16"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "H", "C" },
                Length = 2,
                Opcode = 0xce,
                Value = "ADC A, d8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xcf,
                Value = "RST 08H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd0,
                Value = "RET NC"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd1,
                Value = "POP DE"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xd2,
                Value = "JP NC, a16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd3,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xd4,
                Value = "CALL NC, a16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd5,
                Value = "PUSH DE"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 2,
                Opcode = 0xd6,
                Value = "SUB A, d8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd7,
                Value = "RST 10H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd8,
                Value = "RET C"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd9,
                Value = "RETI"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xda,
                Value = "JP C, a16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xdb,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xdc,
                Value = "CALL C, a16"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xdd,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 2,
                Opcode = 0xde,
                Value = "SBC A, d8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xdf,
                Value = "RST 18H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe0,
                Value = "LD [$FF00 + a8], A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe1,
                Value = "POP HL"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe2,
                Value = "LD [$FF00 + C], A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe3,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe4,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe5,
                Value = "PUSH HL"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "1", "0" },
                Length = 2,
                Opcode = 0xe6,
                Value = "AND A, d8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe7,
                Value = "RST 20H"
            },
            new Instruction {
                Flags = new []{ "0", "0", "H", "C" },
                Length = 2,
                Opcode = 0xe8,
                Value = "ADD SP, r8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe9,
                Value = "JP [HL]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xea,
                Value = "LD [a16], A"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xeb,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xec,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xed,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 2,
                Opcode = 0xee,
                Value = "XOR A, d8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xef,
                Value = "RST 28H"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf0,
                Value = "LD A, [$FF00 + a8]"
            },
            new Instruction {
                Flags = new []{ "Z", "N", "H", "C" },
                Length = 1,
                Opcode = 0xf1,
                Value = "POP AF"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf2,
                Value = "LD A, [$FF00 + C]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf3,
                Value = "DI"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf4,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf5,
                Value = "PUSH AF"
            },
            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 2,
                Opcode = 0xf6,
                Value = "OR A, d8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf7,
                Value = "RST 30H"
            },
            new Instruction {
                Flags = new []{ "0", "0", "H", "C" },
                Length = 2,
                Opcode = 0xf8,
                Value = "LD HL, SP + r8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf9,
                Value = "LD HL, SP"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 3,
                Opcode = 0xfa,
                Value = "LD A, [a16]"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xfb,
                Value = "EI"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xfc,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xfd,
                Value = "UNDEFINED"
            },
            new Instruction {
                Flags = new []{ "Z", "1", "H", "C" },
                Length = 2,
                Opcode = 0xfe,
                Value = "CP A, d8"
            },
            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xff,
                Value = "RST 38H"
            },

































































































































































































































































        };

        public static readonly List<Instruction> CBInstructions = new List<Instruction> {
            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x00,
                Value = "RLC B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x01,
                Value = "RLC C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x02,
                Value = "RLC D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x03,
                Value = "RLC E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x04,
                Value = "RLC H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x05,
                Value = "RLC L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x06,
                Value = "RLC [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x07,
                Value = "RLC A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x08,
                Value = "RRC B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x09,
                Value = "RRC C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x0a,
                Value = "RRC D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x0b,
                Value = "RRC E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x0c,
                Value = "RRC H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x0d,
                Value = "RRC L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x0e,
                Value = "RRC [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x0f,
                Value = "RRC A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x10,
                Value = "RL B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x11,
                Value = "RL C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x12,
                Value = "RL D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x13,
                Value = "RL E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x14,
                Value = "RL H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x15,
                Value = "RL L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x16,
                Value = "RL [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x17,
                Value = "RL X"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x18,
                Value = "RR B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x19,
                Value = "RR C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x1a,
                Value = "RR D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x1b,
                Value = "RR E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x1c,
                Value = "RR H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x1d,
                Value = "RR L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x1e,
                Value = "RR [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x1f,
                Value = "RR A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x20,
                Value = "SLA B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x21,
                Value = "SLA C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x22,
                Value = "SLA D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x23,
                Value = "SLA E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x24,
                Value = "SLA H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x25,
                Value = "SLA L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x26,
                Value = "SLA [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x27,
                Value = "SLA A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x28,
                Value = "SRA B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x29,
                Value = "SRA C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x2a,
                Value = "SRA D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x2b,
                Value = "SRA E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x2c,
                Value = "SRA H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x2d,
                Value = "SRA L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x2e,
                Value = "SRA [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x2f,
                Value = "SRA A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x30,
                Value = "SWAP B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x31,
                Value = "SWAP C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x32,
                Value = "SWAP D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x33,
                Value = "SWAP E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x34,
                Value = "SWAP H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x35,
                Value = "SWAP L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x36,
                Value = "SWAP [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "0" },
                Length = 1,
                Opcode = 0x37,
                Value = "SWAP A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x38,
                Value = "SRL B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x39,
                Value = "SRL C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x3a,
                Value = "SRL D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x3b,
                Value = "SRL E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x3c,
                Value = "SRL H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x3d,
                Value = "SRL L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x3e,
                Value = "SRL [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "0", "C" },
                Length = 1,
                Opcode = 0x3f,
                Value = "SRL A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x40,
                Value = "BIT 0, B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x41,
                Value = "BIT 0, C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x42,
                Value = "BIT 0, D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x43,
                Value = "BIT 0, E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x44,
                Value = "BIT 0, H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x45,
                Value = "BIT 0, L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x46,
                Value = "BIT 0, [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x47,
                Value = "BIT 0, A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x48,
                Value = "BIT 1, B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x49,
                Value = "BIT 1, C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x4a,
                Value = "BIT 1, D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x4b,
                Value = "BIT 1, E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x4c,
                Value = "BIT 1, H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x4d,
                Value = "BIT 1, L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x4e,
                Value = "BIT 1, [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x4f,
                Value = "BIT 1, A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x50,
                Value = "BIT 2, B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x51,
                Value = "BIT 2, C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x52,
                Value = "BIT 2, D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x53,
                Value = "BIT 2, E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x54,
                Value = "BIT 2, H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x55,
                Value = "BIT 2, L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x56,
                Value = "BIT 2, [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x57,
                Value = "BIT 2, A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x58,
                Value = "BIT 3, B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x59,
                Value = "BIT 3, C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x5a,
                Value = "BIT 3, D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x5b,
                Value = "BIT 3, E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x5c,
                Value = "BIT 3, H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x5d,
                Value = "BIT 3, L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x5e,
                Value = "BIT 3, [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x5f,
                Value = "BIT 3, A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x60,
                Value = "BIT 4, B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x61,
                Value = "BIT 4, C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x62,
                Value = "BIT 4, D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x63,
                Value = "BIT 4, E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x64,
                Value = "BIT 4, H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x65,
                Value = "BIT 4, L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x66,
                Value = "BIT 4, [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x67,
                Value = "BIT 4, A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x68,
                Value = "BIT 5, B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x69,
                Value = "BIT 5, C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x6a,
                Value = "BIT 5, D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x6b,
                Value = "BIT 5, E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x6c,
                Value = "BIT 5, H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x6d,
                Value = "BIT 5, L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x6e,
                Value = "BIT 5, [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x6f,
                Value = "BIT 5, A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x70,
                Value = "BIT 6, B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x71,
                Value = "BIT 6, C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x72,
                Value = "BIT 6, D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x73,
                Value = "BIT 6, E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x74,
                Value = "BIT 6, H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x75,
                Value = "BIT 6, L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x76,
                Value = "BIT 6, [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x77,
                Value = "BIT 6, A"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x78,
                Value = "BIT 7, B"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x79,
                Value = "BIT 7, C"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x7a,
                Value = "BIT 7, D"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x7b,
                Value = "BIT 7, E"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x7c,
                Value = "BIT 7, H"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x7d,
                Value = "BIT 7, L"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x7e,
                Value = "BIT 7, [HL]"
            },            new Instruction {
                Flags = new []{ "Z", "0", "1", "-" },
                Length = 1,
                Opcode = 0x7f,
                Value = "BIT 7, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x80,
                Value = "RES 0, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x81,
                Value = "RES 0, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x82,
                Value = "RES 0, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x83,
                Value = "RES 0, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x84,
                Value = "RES 0, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x85,
                Value = "RES 0, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x86,
                Value = "RES 0, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x87,
                Value = "RES 0, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x88,
                Value = "RES 1, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x89,
                Value = "RES 1, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x8a,
                Value = "RES 1, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x8b,
                Value = "RES 1, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x8c,
                Value = "RES 1, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x8d,
                Value = "RES 1, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x8e,
                Value = "RES 1, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x8f,
                Value = "RES 1, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x90,
                Value = "RES 2, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x91,
                Value = "RES 2, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x92,
                Value = "RES 2, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x93,
                Value = "RES 2, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x94,
                Value = "RES 2, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x95,
                Value = "RES 2, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x96,
                Value = "RES 2, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x97,
                Value = "RES 2, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x98,
                Value = "RES 3, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x99,
                Value = "RES 3, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x9a,
                Value = "RES 3, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x9b,
                Value = "RES 3, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x9c,
                Value = "RES 3, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x9d,
                Value = "RES 3, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x9e,
                Value = "RES 3, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0x9f,
                Value = "RES 3, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xa0,
                Value = "RES 4, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xa1,
                Value = "RES 4, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xa2,
                Value = "RES 4, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xa3,
                Value = "RES 4, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xa4,
                Value = "RES 4, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xa5,
                Value = "RES 4, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xa6,
                Value = "RES 4, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xa7,
                Value = "RES 4, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xa8,
                Value = "RES 5, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xa9,
                Value = "RES 5, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xaa,
                Value = "RES 5, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xab,
                Value = "RES 5, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xac,
                Value = "RES 5, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xad,
                Value = "RES 5, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xae,
                Value = "RES 5, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xaf,
                Value = "RES 5, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xb0,
                Value = "RES 6, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xb1,
                Value = "RES 6, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xb2,
                Value = "RES 6, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xb3,
                Value = "RES 6, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xb4,
                Value = "RES 6, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xb5,
                Value = "RES 6, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xb6,
                Value = "RES 6, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xb7,
                Value = "RES 6, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xb8,
                Value = "RES 7, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xb9,
                Value = "RES 7, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xba,
                Value = "RES 7, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xbb,
                Value = "RES 7, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xbc,
                Value = "RES 7, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xbd,
                Value = "RES 7, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xbe,
                Value = "RES 7, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xbf,
                Value = "RES 7, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc0,
                Value = "SET 0, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc1,
                Value = "SET 0, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc2,
                Value = "SET 0, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc3,
                Value = "SET 0, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc4,
                Value = "SET 0, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc5,
                Value = "SET 0, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc6,
                Value = "SET 0, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc7,
                Value = "SET 0, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc8,
                Value = "SET 1, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xc9,
                Value = "SET 1, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xca,
                Value = "SET 1, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xcb,
                Value = "SET 1, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xcc,
                Value = "SET 1, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xcd,
                Value = "SET 1, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xce,
                Value = "SET 1, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xcf,
                Value = "SET 1, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd0,
                Value = "SET 2, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd1,
                Value = "SET 2, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd2,
                Value = "SET 2, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd3,
                Value = "SET 2, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd4,
                Value = "SET 2, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd5,
                Value = "SET 2, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd6,
                Value = "SET 2, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd7,
                Value = "SET 2, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd8,
                Value = "SET 3, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xd9,
                Value = "SET 3, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xda,
                Value = "SET 3, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xdb,
                Value = "SET 3, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xdc,
                Value = "SET 3, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xdd,
                Value = "SET 3, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xde,
                Value = "SET 3, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xdf,
                Value = "SET 3, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe0,
                Value = "SET 4, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe1,
                Value = "SET 4, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe2,
                Value = "SET 4, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe3,
                Value = "SET 4, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe4,
                Value = "SET 4, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe5,
                Value = "SET 4, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe6,
                Value = "SET 4, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe7,
                Value = "SET 4, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe8,
                Value = "SET 5, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xe9,
                Value = "SET 5, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xea,
                Value = "SET 5, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xeb,
                Value = "SET 5, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xec,
                Value = "SET 5, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xed,
                Value = "SET 5, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xee,
                Value = "SET 5, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xef,
                Value = "SET 5, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf0,
                Value = "SET 6, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf1,
                Value = "SET 6, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf2,
                Value = "SET 6, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf3,
                Value = "SET 6, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf4,
                Value = "SET 6, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf5,
                Value = "SET 6, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf6,
                Value = "SET 6, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf7,
                Value = "SET 6, A"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf8,
                Value = "SET 7, B"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xf9,
                Value = "SET 7, C"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xfa,
                Value = "SET 7, D"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xfb,
                Value = "SET 7, E"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xfc,
                Value = "SET 7, H"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xfd,
                Value = "SET 7, L"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xfe,
                Value = "SET 7, [HL]"
            },            new Instruction {
                Flags = new []{ "-", "-", "-", "-" },
                Length = 1,
                Opcode = 0xff,
                Value = "SET 7, A"
            },
        };
    }
}

