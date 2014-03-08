using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravity
{
    class DeathStar
    {
        const float unitToPixel = 100.0f;
        const float pixelToUnit = 1 / unitToPixel;

        public Body Planet;
        private Vector2 PlanetSize;

        public Vector2 Pull {get;set;}
        public World world;

        public Vector2 Position
        {
            get
            {
                return Planet.Position * unitToPixel;
            }
        }

        public Vector2 Size
        {
            get
            {
                return PlanetSize * unitToPixel;
            }
        }




        /// <summary>
        /// !!!!!!PASS IN VARIABLES ****NOT**** PIXELTOUNIT'D!!!!!!! (ie. pixel values)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="pos"></param>
        /// <param name="world"></param>
        public DeathStar(Vector2 size, Vector2 pos, Vector2 pull, World world)
        {
            PlanetSize = size * pixelToUnit;
            this.world = world;

            this.Pull = pull;

            Planet = BodyFactory.CreateRectangle(world, PlanetSize.X, PlanetSize.Y, 10f);
            Planet.BodyType = BodyType.Static;
            Planet.Position = pos * pixelToUnit;




        }

    }
}
