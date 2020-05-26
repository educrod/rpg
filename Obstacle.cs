using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace rpg
{
    class Obstacle
    {
        protected Vector2 position;
        protected int radius;

        public Vector2 Position{
            get {return position;}
        }
        public int Radius{
            get {return radius;}
        }
        public Obstacle(Vector2 newPos){
            position = newPos;
        }

    }
}