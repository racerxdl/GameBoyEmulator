using System;
using System.IO;
using System.Timers;
using GameBoyEmulator.Desktop.GBC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameBoyEmulator.Desktop {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private CPU cpu;
        private Texture2D videoTexture;
        private Texture2D tileBuffer;
        private Texture2D vramBuffer;
        private SpriteFont debuggerFont;
        private string Registers = "PC: 0\nA: 0x00 B: 0x00\nC: 0x00 D: 0x00\nE: 0x00 F: 0x00\nH: 0x00 L: 0x00";
        private KeyboardManager keyboardManager;
        private string GameName = "Not loaded";
        
        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            cpu = new CPU();
            keyboardManager = new KeyboardManager();
        }

//        /// <summary>
//        /// Allows the game to perform any initialization it needs to before starting to run.
//        /// This is where it can query for any required services and load any non-graphic
//        /// related content.  Calling base.Initialize will enumerate through any components
//        /// and initialize them as well.
//        /// </summary>
//        protected override void Initialize() {
//            // TODO: Add your initialization logic here
//
//            base.Initialize();
//        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            videoTexture = new Texture2D(GraphicsDevice, 160, 144, false, SurfaceFormat.Color);
            tileBuffer = new Texture2D(GraphicsDevice, 144, 288, false, SurfaceFormat.Color);
            vramBuffer = new Texture2D(GraphicsDevice, 256, 256, false, SurfaceFormat.Color);
            tileBuffer.SetData(cpu.gpu.TileBuffer);
            debuggerFont = Content.Load<SpriteFont>("Debugger");
            var f = File.ReadAllBytes("opus5.gb");
            cpu.memory.LoadROM(f);
            GameName = cpu.memory.GetRomName();
            Window.Title = $"GameBoyEmulator: ({GameName}) [{cpu.memory.GetRomSize()} - {cpu.memory.GetCatridgeRamSize()}]";
            cpu.Start();
            keyboardManager.OnKeyPress += OnKeyPress;
            cpu.OnPause += OnPause;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            cpu.Stop();
        }

        private void OnPause() {
            Console.WriteLine("On Pause");
            // cpu.gpu.UpdateVRAM();
            // tileBuffer.SetData(cpu.gpu.TileBuffer);
        }

        private void OnKeyPress(object sender, KeyPressEvent keyEvent) {
            switch (keyEvent.key) {
                case Keys.R:
                    Console.WriteLine("Reseting CPU");
                    cpu.Reset();
                    break;
                case Keys.P:
                    Console.WriteLine("Pausing");
                    cpu.Pause();
                    break;
                case Keys.C:
                    Console.WriteLine("Continuing");
                    cpu.Continue();
                    break;
                case Keys.F7:
                    Console.WriteLine("Step");
                    cpu.Step();
                    break;
                case Keys.Escape:
                    Console.WriteLine("Got Exit");
                    Exit();
                    break;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            keyboardManager.Update();
            cpu.GbKeys.Update(Keyboard.GetState());
            tileBuffer.SetData(cpu.gpu.TileBuffer);
            videoTexture.SetData(cpu.memory.GetVideoBuffer());
            cpu.gpu.UpdateVRAM();
            vramBuffer.SetData(cpu.gpu.VRamBuffer);
            var reg = cpu.reg;
            Registers = $"PC: {reg.PC}\n" +
                        $"SP: 0x{reg.SP:X4}\n" +
                        $"Cycles: {cpu.reg.CycleCount}\n" +
                        $"A: 0x{reg.A:X2} B: 0x{reg.B:X2}\n" +
                        $"C: 0x{reg.C:X2} D: 0x{reg.D:X2}\n" +
                        $"E: 0x{reg.E:X2}\n" +
                        $"H: 0x{reg.H:X2} L: 0x{reg.L:X2}\n" +
                        $"F: 0b{Convert.ToString(reg.F, 2).PadLeft(8, '0')}\n" +
                        $"In BIOS: {cpu.memory.inBIOS}\n" +
                        $"Interrupts Enabled: {reg.InterruptEnable}\n" +
                        $"Enabled Interrupts: 0b{Convert.ToString(reg.EnabledInterrupts, 2).PadLeft(8, '0')}\n" +
                        $"Trigger Interrupts: 0b{Convert.ToString(reg.TriggerInterrupts, 2).PadLeft(8, '0')}\n" +
                        $"Last Cycle Time: {cpu.lastCycleTimeMs:F3} ms\n" +
                        $"Last Cycle Frequency: {(1000.0 / cpu.lastCycleTimeMs):F0} Hz\n" +
                        "\nGPU:\n" +
                        $"SCX: {cpu.gpu.scrollX:D4} SCY: {cpu.gpu.scrollY:D4}\n" +
                        $"BG On: {cpu.gpu.switchBg}\n" +
                        $"Screen On: {cpu.gpu.switchLCD}\n" +
                        $"Obj On: {cpu.gpu.switchObj}\n" +
                        $"Obj Big: {cpu.gpu.objSize}\n" +
                        $"BG Tile Base: {cpu.gpu.bgTileBase:X4}\n" +
                        $"BG Map Base: {cpu.gpu.bgMapBase:X4}\n" +
                        $"BG Win Map Base: {cpu.gpu.winMapBase:X4}\n";
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.DrawString(debuggerFont, "LCD", new Vector2(10, 10), Color.Black);
            spriteBatch.Draw(videoTexture, new Rectangle(10, 40, videoTexture.Width * 2, videoTexture.Height * 2), Color.White);
            
            spriteBatch.DrawString(debuggerFont, "Registers", new Vector2(videoTexture.Width * 2 + 50, 10), Color.Black);
            spriteBatch.DrawString(debuggerFont, Registers, new Vector2(videoTexture.Width * 2 + 50, 30), Color.Black);
            
            spriteBatch.DrawString(debuggerFont, "Tile Memory", new Vector2(750, 10), Color.Black);
            spriteBatch.Draw(tileBuffer, new Rectangle(750, 30, tileBuffer.Width * 2, tileBuffer.Height * 2), Color.White);

            spriteBatch.DrawString(debuggerFont, "GPU VRAM", new Vector2(10, 400), Color.Black);
            spriteBatch.Draw(vramBuffer, new Rectangle(10, 420, vramBuffer.Width, vramBuffer.Height), Color.White);
            
            spriteBatch.End();
            
           
            base.Draw(gameTime);
        }
    }
}
