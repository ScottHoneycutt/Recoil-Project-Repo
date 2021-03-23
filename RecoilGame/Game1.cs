using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{

 
    public class Game1 : Game
    {

        /// <summary>
        /// Aidan Kamp - 3/22/21
        /// Tested player fields, player physics in the update method, and drawing the player
        /// All methods are commented out
        /// </summary>
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        //Manager classes----
        public static ProjectileManager projectileManager;
        public static PlayerManager playerManager;
        public static EnemyManager enemyManager;
        public static LevelManager levelManager;

        //Player
        Texture2D playerSprite;
        Player player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Changing screen size----
            _graphics.PreferredBackBufferWidth = 550;
            _graphics.PreferredBackBufferHeight = 550;
            _graphics.ApplyChanges();

            projectileManager = new ProjectileManager();
            
            enemyManager = new EnemyManager();
            levelManager = new LevelManager(this);

            levelManager.GenerateTestLevel();
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSprite = Content.Load<Texture2D>("square");

            //Aidan - I had to initialize these after the player sprite was loaded or
            //I'd get a null pointer error for the sprite texture
            
            player = new Player(200, 200, 40, 40, playerSprite, true, 100);
            playerManager = new PlayerManager(player, 6, 3, -10, .25f);

            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Player Physics
            
            //playerManager.MovePlayer();
            //playerManager.CheckForCollisions();
            //playerManager.ApplyPlayerGravity();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            levelManager.DrawLevel(_spriteBatch);

            //player.Draw(_spriteBatch, Color.White);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
