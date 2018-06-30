using Microsoft.Xna.Framework.Input;

namespace GameBoyEmulator.Desktop.GBC {
    public class GBKeys {
        byte directional;
        byte keys;
        private byte selectedInput;
        private CPU cpu;

        public GBKeys(CPU cpu) {
            this.cpu = cpu;
            Reset();
        }
        
        public void Reset() {
            keys = 0x0F;
            directional = 0x0F;
            selectedInput = 0;
        }

        public void Write(byte val) {
            selectedInput = (byte) (val & 0x30);
        }

        public byte Read() {
            switch (selectedInput) {
                case 0x10: return directional;
                case 0x20: return keys;
                default: return 0x00;
            }
        }

        private void SetDirectionalBit(int bit, bool val) {
            if (!val) {
                directional |= (byte) (1 << bit);
            } else {
                directional &= (byte) ((~(1 << bit)) & 0xF);
            }
        }

        private void SetKeysBit(int bit, bool val) {
            if (!val) {
                keys |= (byte) (1 << bit);
            } else {
                keys &= (byte) ((~(1 << bit)) & 0xF);
            }
        }

        public void Update(KeyboardState state) {
            var bDirectional = directional;
            var bKeys = keys;

            SetDirectionalBit(1, state.IsKeyDown(Keys.Right));
            SetDirectionalBit(2, state.IsKeyDown(Keys.Left));
            SetDirectionalBit(3, state.IsKeyDown(Keys.Up));
            SetDirectionalBit(4, state.IsKeyDown(Keys.Down));

            SetKeysBit(1, state.IsKeyDown(Keys.Z));
            SetKeysBit(2, state.IsKeyDown(Keys.X));
            SetKeysBit(3, state.IsKeyDown(Keys.Space));
            SetKeysBit(4, state.IsKeyDown(Keys.Enter));
            
            if ((bDirectional != directional) || (bKeys != keys)) {
                cpu.reg.TriggerInterrupts |= Flags.INT_JOYPAD;
            } 
        }
    }
}
