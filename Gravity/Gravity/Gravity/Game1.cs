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
using WebSocket4Net;
using SimpleJson;

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

        private bool objectMoving = false;
        private bool objectDrag = false;

        private int windowWidth, windowHeight;
        private float camX, camY;

        private bool freeMove = false;

        private Vector2 originalPos = Vector2.Zero;

        private DeathStar[] DeathStars = new DeathStar[2];
        private bool pinching = false;

        private float lastScale = 1.0f;

        private float zoom = 1.0f;

        WebSocket websocket;

        private List<Projectile> Projectiles = new List<Projectile>();

        private Projectile CurrentProjectile = null;
        
        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            websocket = new WebSocket("ws://192.168.0.150:8142/");
            websocket.EnableAutoSendPing = true;
            websocket.Opened += websocket_Opened;
            websocket.Closed += websocket_Closed;
            websocket.Error += websocket_Error;
            websocket.MessageReceived += websocket_MessageReceived;
            websocket.Open();

        }

        void websocket_Opened(object sender, EventArgs e)
        {
            Debug.WriteLine("I CONNECTED");
        }

        void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Debug.WriteLine(e);
        }

        void websocket_Closed(object sender, EventArgs e)
        {

        }

        void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {

            JsonObject msg = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(e.Message);
            string type = (string)msg["type"];
            switch (type)
            {
                case "fire":
                    handleFire(
                        float.Parse(msg["vel_y"].ToString()),
                        float.Parse(msg["vel_y"].ToString()),
                        float.Parse(msg["pos_x"].ToString()),
                        float.Parse(msg["pos_y"].ToString()));
                    break;
            }
        }



        void handleFire(float velx, float vely, float posx, float posy)
        {

            CurrentProjectile.Proj.LinearVelocity = new Vector2(velx, vely);

            CurrentProjectile.Position = new Vector2(posx, posy);
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
            TouchPanel.EnabledGestures = GestureType.Pinch | GestureType.PinchComplete;
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

            DeathStars[0] = new DeathStar(new Vector2(100, 100), new Vector2(100, 100), new Vector2(3), world);
            DeathStars[1] = new DeathStar(new Vector2(100, 100), new Vector2(1300, 100), new Vector2(3), world);

            CurrentProjectile = new Projectile(new Vector2(20, 20), new Vector2(400, 250), world, true);

            windowWidth = Window.ClientBounds.Width;
            windowHeight = Window.ClientBounds.Height;

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
            {
                objectMoving = false;
                objectDrag = false;

                Projectiles.Clear();

                camY = 0;

                camX = 0;

                zoom = 1.0f;
                lastScale = 1.0f;

                freeMove = false;
                CurrentProjectile = new Projectile(new Vector2(20, 20), new Vector2(400, 250), world, true);

            }

            Vector2 projForce = Vector2.Zero;

            if (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                switch (gs.GestureType)
                {
                    case GestureType.Pinch:

                        break;
                    case GestureType.PinchComplete:

                        if (zoom >= 1)
                        {
                            zoom = 0.25f;
                        }
                        else
                        {
                            zoom = 1.0f;
                        }

                        //blah
                        break;
                }
            }
            else if (!objectMoving)
            {
                TouchCollection touchCollection = TouchPanel.GetState();
                foreach (TouchLocation tl in touchCollection)
                {
                    if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved))
                    {

                        TouchLocation old;

                        tl.TryGetPreviousLocation(out old);
                        if (CurrentProjectile != null && Vector2.Distance(tl.Position - new Vector2(camX, camY), CurrentProjectile.Position) < 100)
                        {

                            if (old.Position.X > 0)
                                CurrentProjectile.Position = (CurrentProjectile.Position + (tl.Position - old.Position));
                            else
                                originalPos = tl.Position;
                            objectDrag = true;
                        }
                        else
                        {

                            if (old.Position.X > 0)
                            {
                                camX = camX + (tl.Position.X - old.Position.X);
                                camY = camY + (tl.Position.Y - old.Position.Y);
                            }
                            else
                                originalPos = tl.Position;

                        }


                    }
                    else if (tl.State == TouchLocationState.Released && objectDrag)
                    {
                        objectMoving = true;
                        objectDrag = false;

                        projForce = originalPos - tl.Position;
                        Vector2 oldForce = projForce;
                        float len = projForce.Length() * 15;
                        projForce.Normalize();

                        projForce = projForce * len * pixelToUnit;

                        camX = CurrentProjectile.Position.X;
                        camY = CurrentProjectile.Position.Y;

                    }
                }
            }
            else
            {
                TouchCollection touchCollection = TouchPanel.GetState();
                foreach (TouchLocation tl in touchCollection)
                {
                    if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved))
                    {

                        TouchLocation old;

                        tl.TryGetPreviousLocation(out old);
                        if (Vector2.Distance(tl.Position - new Vector2(camX, camY), CurrentProjectile.Position) > 100)
                        {
                            if (old.Position.X > 0)
                            {
                                camX = camX + (tl.Position.X - old.Position.X);
                                camY = camY + (tl.Position.Y - old.Position.Y);
                            }
                            else
                                originalPos = tl.Position;
                            freeMove = true;
                        }
                    }
                }
            }

            if (objectMoving)
            {


                foreach (DeathStar d in DeathStars)
                {
                    Vector2 forcedir = (d.Position) - CurrentProjectile.Position;
                    float len = forcedir.Length();
                    forcedir.Normalize();


                    if (len < 1000)
                    {
                        forcedir = forcedir * (1 - len / 1000) * d.Pull;
                        projForce += forcedir;

                    }

                }
                if (!float.IsNaN(projForce.X))
                    CurrentProjectile.Proj.ApplyForce(projForce);

                if (i == 0)
                {
                    JsonObject msg = new JsonObject();
                    msg["type"] = "fire";
                    msg["vel_x"] = CurrentProjectile.Proj.LinearVelocity.X;
                    msg["vel_y"] = CurrentProjectile.Proj.LinearVelocity.Y;
                    msg["pos_x"] = CurrentProjectile.Position.X;
                    msg["pos_y"] = CurrentProjectile.Position.Y;

                    websocket.Send(SimpleJson.SimpleJson.SerializeObject(msg));
                    i++;
                }
                else
                {
                    i = (i + 1) % 2;
                }
            }

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (objectMoving && !freeMove)
            {

                camY = (int)(-CurrentProjectile.Position.Y + windowHeight / 2.0f);

                camX = (int)(-CurrentProjectile.Position.X + windowWidth / 2.0f);

            }

            base.Update(gameTime);
        }

        int i = 0;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Vector3 transVector = new Vector3(camX, camY, 0.0f);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateTranslation(transVector) * Matrix.CreateScale(new Vector3(zoom, zoom, 1)));

            foreach (DeathStar d in DeathStars)
            {
                spriteBatch.Draw(Game1.Pixi, d.Position, null, Color.Red, d.Planet.Rotation, new Vector2(Game1.Pixi.Width / 2.0f, Game1.Pixi.Height / 2.0f), d.Size, SpriteEffects.None, 0);

            }
            if (CurrentProjectile != null)
                spriteBatch.Draw(Game1.Pixi, CurrentProjectile.Position, null, Color.Yellow, CurrentProjectile.Proj.Rotation, new Vector2(Game1.Pixi.Width / 2.0f, Game1.Pixi.Height / 2.0f), CurrentProjectile.Size, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
