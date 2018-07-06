using System;
using GameBoyEmulator.Desktop.GBC;

namespace GameBoyEmulator.Desktop.Disasm {
    public class Disassembler {
        private int valuePadding = 0;
        
        public Disassembler() {
            foreach (var ins in DisasmInstructions.Instructions) {
                var strData = ins.Value;
                if (ins.Value.IndexOf("a8", StringComparison.InvariantCultureIgnoreCase) > -1) {
                    // Replace a8 by byte.
                    strData = strData.Replace("a8", $"{0xFF}");
                } else if (strData.IndexOf("r8", StringComparison.InvariantCultureIgnoreCase) > -1) {
                    // Replace r8 by byte
                    strData = strData.Replace("r8", $"{0xFF}");
                } else if (strData.IndexOf("a16", StringComparison.InvariantCultureIgnoreCase) > -1) {
                    // Replace a16 by word
                    var val = 0xFFFF;
                    strData = strData.Replace("a16", $"0x{val:X4}");
                }
                valuePadding = Math.Max(valuePadding, strData.Length);
            }

            valuePadding = Math.Max(valuePadding, "UNK 0x00".Length);
        }

        public string DisasmMemoryForLines(Memory memory, ushort offset, int lines) {
            var output = "";
            var cLines = 0;
            var off = offset;
            var bytesConsumed = 0;
            while (cLines < lines) {
                var bytes = memory.ReadBytes(off, 32);
                output += Disasm(off, bytes, out bytesConsumed, 28);
                off += (ushort) bytesConsumed;
                cLines = output.Split('\n').Length;
            }
            return output;
        }

        public string Disasm(int baseAddr, byte[] data, int max = -1) {
            var readBytes = 0;
            return Disasm(baseAddr, data, out readBytes, max);
        }
        
        public string Disasm(int baseAddr, byte[] data, out int readBytes, int max = -1) {
            var output = "";
            var i = 0;
            var c = 0;
            var bytesConsumed = 0;
            max = max == -1 ? data.Length : max;
            while (i < max) {
                var ins = DisasmInstructions.Instructions[data[i]];
                var addr = baseAddr + i;
                var strData = ins.Value == "UNDEFINED" ? $"UNK 0x{ins.Opcode:X2}" : ins.Value;

                if (strData.IndexOf("a8", StringComparison.InvariantCultureIgnoreCase) > -1) {
                    // Replace a8 by byte.
                    strData = strData.Replace("a8", $"{data[i + 1]}");
                } else if (strData.IndexOf("r8", StringComparison.InvariantCultureIgnoreCase) > -1) {
                    // Replace r8 by byte
                    strData = strData.Replace("r8", $"{data[i + 1]}");
                } else if (strData.IndexOf("d8", StringComparison.InvariantCultureIgnoreCase) > -1) {
                    // Replace r8 by byte
                    strData = strData.Replace("d8", $"{data[i + 1]}");
                } else if (strData.IndexOf("a16", StringComparison.InvariantCultureIgnoreCase) > -1) {
                    // Replace a16 by word
                    var val = data[i + 1] + (data[i + 2] << 8);
                    strData = strData.Replace("a16", $"0x{val:X4}");
                }

//                var bytes = "";
//                for (var z = 0; z < ins.Length; z++) {
//                    bytes += $"0x{data[i + z]:X2} ";
//                }
                
                output += $"{addr:X4}: {strData.PadRight(valuePadding)} ;\n";
                i += ins.Length;
                bytesConsumed += ins.Length;
                c++;
                
                if (c <= data.Length) continue;
                Console.WriteLine($"Ran {c} cycles but only {data.Length} bytes!!!");
                break;
            }

            readBytes = bytesConsumed;
            return output;
        }
    }
}
