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
        protected Vector2 hitPos;
        public static List<Obstacle> obstacles = new List<Obstacle>();

        public Vector2 HitPos {
            get {return hitPos;}
        }
        public Vector2 Position{
            get {return position;}
        }
        public int Radius{
            get {return radius;}
        }
        public Obstacle(Vector2 newPos){
            position = newPos;
        }
        public static bool didCollide(Vector2 otherPos, int otherRad){
            foreach(Obstacle o in Obstacle.obstacles){
                int sum = o.Radius + otherRad;
                if(Vector2.Distance(o.HitPos, otherPos) < sum ){
                    return true;
                }
            }
            return false;
        }

    }
    class Tree : Obstacle
    {
        public Tree(Vector2 newPos) : base(newPos){
            radius = 20;
            hitPos = new Vector2(position.X + 64, position.Y + 150);
        }
    }
    class Bush : Obstacle
    {
        public Bush(Vector2 newPos) : base(newPos){
            radius = 32;
            hitPos = new Vector2(position.X + 56, position.Y + 57);
        }
    }
}