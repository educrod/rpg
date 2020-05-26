using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace rpg
{
    class Enemy
    {
        private Vector2 position; 
        protected int health;
        protected int speed;
        protected int radius;

        public static List<Enemy> enemies = new List<Enemy>();

        public int Health {
            get {return health;}
            set { health = value;}
        }
        
        public Vector2 Position {
            get {return position;}

        }
        public int Radius {
            get {return radius;}
        }
        public Enemy(Vector2 newPos) {
            position = newPos;
        }
        public void Update(GameTime gameTime, Vector2 playerPos){
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 moveDir = playerPos - position;
            moveDir.Normalize();
            position += moveDir * speed * dt;
        }
    }
    class Snake : Enemy {
        public Snake(Vector2 newPos) : base(newPos) {
            speed = 160;
            radius = 42;
            health = 3;
        }
    }

    class Eye : Enemy {
        public Eye(Vector2 newPos) : base(newPos) {
            speed = 80;
            radius = 45;
            health = 5;
        }
    }
}