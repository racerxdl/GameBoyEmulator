using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameBoyEmulator.Desktop {
    public class KeyboardManager {
        private KeyboardState state;

        public delegate void KeyPressEventHandler(object sender, KeyPressEvent e);
        
        public event KeyPressEventHandler OnKeyPress;
        
        public KeyboardManager() {
            state = new KeyboardState();
        }

        public void Update() {
            var currState = Keyboard.GetState();
            var pressedKeys = currState.GetPressedKeys().ToList();
            var lastPressedKeys = state.GetPressedKeys().ToList();
            pressedKeys.ForEach((k) => {
                if (!lastPressedKeys.Contains(k)) {
                    OnKeyPress?.Invoke(this, new KeyPressEvent { key = k });
                }
            });
            state = currState;
        }
    }
}
