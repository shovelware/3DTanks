using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Primitives
{
    /// <summary>
    /// Customised Streets Demo
    /// </summary>
    public class MainK : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        

        String[] map ={  
                    "╬═╦═╦═════╗",
                    "║-║0║11111║",
                    "║ ║0╠═════╣",
                    "╠═╩═╣77777║",
                    "║555║77777║",
                    "╠═╦═╬═════╣",
                    "║3║9║11111║",
                    "║3╚═╬══╗66║",
                    "║343╠══╝99║",
                    "╚═══╩═════╝"          
        };

        
        List<Street> streets;
        Rectangle blockSize;

        Camera3D camera;

        InputManager input;

        public MainK()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Services.AddService(typeof(ContentManager), Content);
            graphics.PreferMultiSampling = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            blockSize = new Rectangle(0, 0, 256, 256);

            input = new InputManager();
            streets = new List<Street>();

            StreetFactory.Init(this, blockSize);

            for (int z = 0; z < map.Length; z++)
            {
                for (int x = 0; x < map[z].Length; x++)
                {
                    char c = map[z][x];
                    if (StreetFactory.streetSymbols.Contains(c))
                    {
                        Street s = StreetFactory.makeStreet(map[z][x], new Vector2(x, z));
                        streets.Add(s);
                    }
                }
            }
            camera = new Camera3D(new Vector3(0, 10, 0), new Vector3(50, 10, 50), Vector3.Up, 1f, graphics.GraphicsDevice.Viewport.AspectRatio, 1, 5120);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void CheckInput()
        {
            #region Movement

            if (input.CheckKey(Keys.W) > 0)
            {
                camera.MoveForward();
            }
            if (input.CheckKey(Keys.S) > 0)
            {
                camera.MoveBackward();
            }

            if (input.CheckKey(Keys.A) > 0)
            {
                camera.MoveLeft();
            }
            if (input.CheckKey(Keys.D) > 0)
            {
                camera.MoveRight();
            }

            if (input.CheckKey(Keys.Q) > 0)
            {
                camera.MoveUp();
            }
            if (input.CheckKey(Keys.Z) > 0)
            {
                camera.MoveDown();
            }

            if (input.CheckKey(Keys.LeftShift) > 0)
            {
                camera.MoveFast();
            }
            if (input.CheckKey(Keys.LeftControl) > 0)
            {
                camera.MoveSlow();
            }

            #endregion

            #region Rotation

            if (input.CheckKey(Keys.J) > 0)
            {
                camera.RotateWorldY(0.1f);
            }
            if (input.CheckKey(Keys.L) > 0)
            {
                camera.RotateWorldY(-0.1f);
            }

            if (input.CheckKey(Keys.I) > 0)
            {
                camera.RotateX(0.1f);
            }
            if (input.CheckKey(Keys.K) > 0)
            {
                camera.RotateX(-0.1f);
            }

            if (input.CheckKey(Keys.U) > 0)
            {
                camera.RotateZ(-0.1f);
            }
            if (input.CheckKey(Keys.O) > 0)
            {
                camera.RotateZ(0.1f);
            }

            #endregion

            if (input.CheckKey(Keys.R) > 0)
            {
                camera.Reset();
            }

            //Juice
            if (input.CheckKey(Keys.OemComma) > 0)
            {
                camera.AspectTarget = 4f;
            }
            if (input.CheckKey(Keys.OemComma) <= 0)
            {
                camera.AspectReset();
            }


            if (input.CheckKey(Keys.OemPeriod) > 0)
            {
                camera.FoVTarget = 0.1f;
            }
            if (input.CheckKey(Keys.OemPeriod) <= 0)
            {
                camera.FoVReset();
            }

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            input.Update(gameTime);
            CheckInput();

            // TODO: Add your update logic here

            base.Update(gameTime);
            camera.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);

            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp; // need to do this on reach devices to allow non 2^n textures
            RasterizerState rs = RasterizerState.CullNone;
            
            GraphicsDevice.RasterizerState = rs;

            this.Window.Title = "AR: " + camera.Aspect + " FoV: " + camera.FoV + "";

            foreach (Street s in streets)
            {
                s.Draw(gameTime, camera);
            }
            base.Draw(gameTime);
        }
    }
}
