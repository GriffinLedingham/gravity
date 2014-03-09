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

        public int index = 0;

        public Body City;
        public Vector2 _citySize;

        public bool left;

        public Vector2 CitySize
        {
            get
            {
                return _citySize * unitToPixel;
            }
        }

        public Vector2 CityPosition
        {
            get
            {
                return City.Position * unitToPixel;
            }
        }


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
        public DeathStar(Vector2 size, Vector2 pos, Vector2 pull, World world, bool left)
        {
            PlanetSize = size * pixelToUnit;
            this.world = world;

            this.Pull = pull;
            this.left = left;

            index = Game1.Game1Random.Next() % 3;

            //Planet = BodyFactory.CreateRectangle(world, PlanetSize.X, PlanetSize.Y, 10f);
            Planet = BodyFactory.CreateCircle(world, PlanetSize.X / 2, 10f);
            Planet.BodyType = BodyType.Static;
            Planet.Position = pos * pixelToUnit;

            _citySize = new Vector2(75, 25) * pixelToUnit;
            City = BodyFactory.CreateRectangle(world, _citySize.X, _citySize.Y, 10f);
            City.BodyType = BodyType.Static;
            City.Position = Planet.Position + new Vector2(PlanetSize.X/2  + _citySize.X / 2, 0) *(left? -1:1);

        }

    }
}
