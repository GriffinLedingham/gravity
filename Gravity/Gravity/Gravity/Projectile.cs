using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravity
{
    class Projectile
    {
        const float unitToPixel = 100.0f;
        const float pixelToUnit = 1 / unitToPixel;

        public Body Proj;
        private Vector2 ProjectileSize;

        public World world;

        public bool Mine = false;
        public bool THISVARIABLEFUCKINGSUCKS = false;

        public Vector2 Force { get; set; }

        public Vector2 OldPos { get; set; }

        public Vector2 Position
        {
            get
            {
                return Proj.Position * unitToPixel;
            }
            set
            {
                Proj.Position = value * pixelToUnit;
            }
        }

        public Vector2 Size
        {
            get
            {
                return ProjectileSize * unitToPixel;
            }
        }

        /// <summary>
        /// !!!!!!PASS IN VARIABLES ****NOT**** PIXELTOUNIT'D!!!!!!! (ie. pixel values)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="pos"></param>
        /// <param name="world"></param>
        public Projectile(Vector2 size, Vector2 pos, World world, bool mine)
        {
            ProjectileSize = size * pixelToUnit;
            this.world = world;


            Proj = BodyFactory.CreateRectangle(world, ProjectileSize.X, ProjectileSize.Y, 10f);
            Proj.BodyType = BodyType.Dynamic;
            Proj.Position = pos * pixelToUnit;

        }

    }
}
