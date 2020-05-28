using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace rpg
{
    enum Dir {
        Down,
        Up,
        Left,
        Right
    }

    public class Game1 : Game
    {        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D player_Sprite;
        Texture2D playerDown;
        Texture2D playerUp;
        Texture2D playerLeft;
        Texture2D playerRight;

        Texture2D eyeEnemy_Sprite;
        Texture2D snakeEnemy_Sprite;

        Texture2D bush_Sprite;
        Texture2D tree_Sprite;

        Texture2D heart_Sprite;
        Texture2D bullet_Sprite;
        
        TiledMap myMap;
        TiledMapRenderer mapRenderer;
        
        Player player = new Player();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 720;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            mapRenderer = new TiledMapRenderer(GraphicsDevice);
            base.Initialize();            
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player_Sprite = Content.Load<Texture2D>("Player/player");
            playerDown = Content.Load<Texture2D>("Player/playerDown");
            playerUp = Content.Load<Texture2D>("Player/playerUp");
            playerLeft = Content.Load<Texture2D>("Player/playerLeft");
            playerRight = Content.Load<Texture2D>("Player/playerRight");

            eyeEnemy_Sprite = Content.Load<Texture2D>("Enemies/eyeEnemy");
            snakeEnemy_Sprite = Content.Load<Texture2D>("Enemies/snakeEnemy");

            bush_Sprite = Content.Load<Texture2D>("Obstacles/bush");
            tree_Sprite = Content.Load<Texture2D>("Obstacles/tree");

            bullet_Sprite = Content.Load<Texture2D>("Misc/bullet");
            heart_Sprite = Content.Load<Texture2D>("Misc/heart");
           
            player.animations[0] = new AnimatedSprite(playerDown,1,4);
            player.animations[1] = new AnimatedSprite(playerUp,1,4);
            player.animations[2] = new AnimatedSprite(playerLeft,1,4);
            player.animations[3] = new AnimatedSprite(playerRight,1,4);

            myMap = Content.Load<TiledMap>("Misc/gameMap");
            mapRenderer.LoadMap(myMap);

            Enemy.enemies.Add(new Snake(new Vector2(100,400)));
            Enemy.enemies.Add(new Eye(new Vector2(300,450)));

            Obstacle.obstacles.Add(new Tree(new Vector2(500,100)));
            Obstacle.obstacles.Add(new Bush(new Vector2(700,300)));

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (player.Health > 0){
                player.Update(gameTime);
            }

            foreach (Projectile proj in Projectile.projectiles){
                proj.Update(gameTime);
            }
            foreach(Enemy en in Enemy.enemies){
                en.Update(gameTime, player.Position);
            }
            foreach(Projectile proj in Projectile.projectiles){
                foreach(Enemy en in Enemy.enemies){
                    int sum = proj.Radius + en.Radius;
                    if(Vector2.Distance(proj.Position, en.Position) < sum){
                        proj.Collided = true;
                        en.Health--;                    
                    }
                }   
                if (Obstacle.didCollide(proj.Position,proj.Radius)){
                    proj.Collided = true;
                }         
            }
            foreach (Enemy en in Enemy.enemies){                
                int sum = player.Radius + en.Radius;
                if(Vector2.Distance(player.Position,en.Position) < sum  && player.HealthTimer <= 0){
                    player.Health--;
                    player.HealthTimer = 1.5f;
                }
            }
            Projectile.projectiles.RemoveAll(p => p.Collided);
            Enemy.enemies.RemoveAll(e => e.Health <= 0);
            
            mapRenderer.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);            

            mapRenderer.Draw();

            if(player.Health > 0) {
                player.anim.Draw(_spriteBatch, new Vector2(player.Position.X - 48, player.Position.Y - 48));
            }
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            foreach (Enemy en in Enemy.enemies) {
                Texture2D spriteToDraw;
                int rad;
                if (en.GetType() == typeof(Snake)){
                    spriteToDraw = snakeEnemy_Sprite;
                    rad = 50;
                } else {
                    spriteToDraw = eyeEnemy_Sprite;
                    rad = 73;
                }
                _spriteBatch.Draw(spriteToDraw, new Vector2(en.Position.X - rad, en.Position.Y - rad ), Color.White);
            }

            foreach (Obstacle o in Obstacle.obstacles) {        
                Texture2D spriteToDraw;
                if (o.GetType() == typeof(Tree)){
                    spriteToDraw = tree_Sprite;
                } else{
                    spriteToDraw = bush_Sprite;
                }
                _spriteBatch.Draw(spriteToDraw, o.Position, Color.White);
            }
            foreach (Projectile proj in Projectile.projectiles){
                _spriteBatch.Draw(bullet_Sprite, new Vector2(proj.Position.X - proj.Radius, proj.Position.Y - proj.Radius), Color.White);
            }
            for(int i =0; i < player.Health; i++){
                _spriteBatch.Draw(heart_Sprite, new Vector2(3 + i * 63, 3), Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
