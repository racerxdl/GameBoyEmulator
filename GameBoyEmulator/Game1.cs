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
        private SpriteFont debuggerFont;
        private string Registers = "PC: 0\nA: 0x00 B: 0x00\nC: 0x00 D: 0x00\nE: 0x00 F: 0x00\nH: 0x00 L: 0x00";
        private KeyboardManager keyboardManager;
        private Timer cpuTimer;
        
        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            cpu = new CPU();
            keyboardManager = new KeyboardManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            videoTexture = new Texture2D(GraphicsDevice, 160, 144, false, SurfaceFormat.Color);
            tileBuffer = new Texture2D(GraphicsDevice, 128, 256, false, SurfaceFormat.Color);
            tileBuffer.SetData(cpu.gpu.TileBuffer);
            debuggerFont = Content.Load<SpriteFont>("Debugger");
            var f = File.ReadAllBytes("opus5.gb");
            cpu.memory.LoadROM(f);
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
            tileBuffer.SetData(cpu.gpu.TileBuffer);
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
            
            videoTexture.SetData(cpu.memory.GetVideoBuffer());
            var reg = cpu.reg;
            Registers = $"PC: {reg.PC}\nSP: 0x{reg.SP:X4}\nClock: {cpu.clockM}\nA: 0x{reg.A:X2} B: 0x{reg.B:X2}\nC: 0x{reg.C:X2} D: 0x{reg.D:X2}\nE: 0x{reg.E:X2} F: 0x{reg.F:X2}\nH: 0x{reg.H:X2} L: 0x{reg.L:X2}";
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            // spriteBatch.Draw(videoTexture, new Vector2(100, 100), null, Color.White);
            spriteBatch.Draw(videoTexture, new Rectangle(20, 20, videoTexture.Width * 2, videoTexture.Height * 2),
                Color.White);
            spriteBatch.DrawString(debuggerFont, Registers, new Vector2(videoTexture.Width * 2 + 50, 20), Color.Black);
            
            spriteBatch.Draw(tileBuffer, new Rectangle(600, 20, tileBuffer.Width * 2, tileBuffer.Height * 2),
                Color.White);
            spriteBatch.End();
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
