using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace physics
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D square_texture;
        private List<GameObject> objects;
        private Physics physics;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            
            objects = new List<GameObject>();
            physics = new Physics();
            physics.SetWorldBounds(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            
            float dX = 1;
            float dY = -1;

            for(int i = 0; i < 100; i++)
            {
                var rand = new System.Random();
                float x = rand.Next(0, GraphicsDevice.Viewport.Width);
                float y = rand.Next(0, GraphicsDevice.Viewport.Height);
                objects.Add(new GameObject(x, y, 5, 5, new Vector2(dX, dY)));
                dX *= -1;
                dY *= -1;
            }

            foreach(GameObject obj in objects)
            {
                physics.AddCollider(new BoxCollider(0, 0, obj.width, obj.height, obj));
            }

            // objects.Add(new Object(10, 10, 10, 10, new Vector2(1, -1)));
            // objects.Add(new Object(100, 50, 10, 10, new Vector2(1, -1)));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            square_texture = Content.Load<Texture2D>("square");
        }

        protected override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float speed = 100;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach(GameObject obj in objects)
            {
                if(obj.x <= 0)
                    obj.direction.X = 1;
                if(obj.x + obj.width >= GraphicsDevice.Viewport.Width)
                    obj.direction.X = -1;
                
                if(obj.y <= 0)
                    obj.direction.Y = 1;
                if(obj.y + obj.height >= GraphicsDevice.Viewport.Height)
                    obj.direction.Y = -1;

                obj.x += speed * obj.direction.X * delta;
                obj.y += speed * obj.direction.Y * delta;

                if(obj.hasCollided)
                    obj.color = Color.Red;
                else
                    obj.color = Color.YellowGreen;
            }

            physics.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach(GameObject obj in objects)
            {
                _spriteBatch.Draw(square_texture, new Vector2(obj.x, obj.y), obj.color);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
