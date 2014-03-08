using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System.Diagnostics;

namespace Gravity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Texture2D Pixi;
        public static Random Game1Random = new Random();

        const float unitToPixel = 100.0f;
        const float pixelToUnit = 1 / unitToPixel;

        private World world;

        private Body Planet;
        private Vector2 PlanetSize = new Vector2(100 * pixelToUnit, 100 * pixelToUnit);

        private Vector2 PlanetGrav = new Vector2(10 * pixelToUnit, 10 * pixelToUnit);

        private Body Projectile;
        private Vector2 ProjectileSize = new Vector2(20 * pixelToUnit, 20 * pixelToUnit);

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
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
            TouchPanel.EnabledGestures = GestureType.Flick;
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

            Pixi = Content.Load<Texture2D>("pixi");

            world = new World(new Vector2(0, 0)); // Grav 0 cause we in space hommie

            Planet = BodyFactory.CreateRectangle(world, 100 * pixelToUnit, 100 * pixelToUnit, 10f);
            Planet.BodyType = BodyType.Static;
            Planet.Position = new Vector2(400 * pixelToUnit, 220 * pixelToUnit);

            Projectile = BodyFactory.CreateRectangle(world, 20 * pixelToUnit, 20 * pixelToUnit, 10f);
            Projectile.BodyType = BodyType.Dynamic;
            Projectile.Position = new Vector2(200 * pixelToUnit, 50 * pixelToUnit);
            Projectile.ApplyForce(new Vector2(20, 20));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            Vector2 projForce = new Vector2(-0.1f, 0.1f);

            Vector2 forcedir = (Planet.Position + PlanetSize / 2) * unitToPixel - (Projectile.Position + ProjectileSize / 2) * unitToPixel;
            float len = forcedir.Length();
            forcedir.Normalize();

            forcedir = forcedir * 0.5f;

            projForce += forcedir;


            Projectile.ApplyForce(projForce);

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(Game1.Pixi, Planet.Position * unitToPixel, null, Color.Red, Planet.Rotation, new Vector2(Game1.Pixi.Width / 2.0f, Game1.Pixi.Height / 2.0f), new Vector2(PlanetSize.X * unitToPixel, PlanetSize.Y * unitToPixel), SpriteEffects.None, 0);
            spriteBatch.Draw(Game1.Pixi, Projectile.Position * unitToPixel, null, Color.Yellow, Projectile.Rotation, new Vector2(Game1.Pixi.Width / 2.0f, Game1.Pixi.Height / 2.0f), new Vector2(ProjectileSize.X * unitToPixel, ProjectileSize.Y * unitToPixel), SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
