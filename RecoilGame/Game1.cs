using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{

    /// <summary>
    /// Scott Honeycutt----
    /// 3/28/2021----
    /// Set up finite state machine to transition from main menu to the test level----
    /// </summary>
 
    public enum GameState
    {
        MainMenu,
        Level,
        Victory
    }

    public class Game1 : Game
    {

        /// <summary>
        /// Aidan Kamp - 3/22/21
        /// Tested player fields, player physics in the update method, and drawing the player
        /// All methods are commented out
        /// </summary>
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Finite state machine enum----
        private GameState currentGameState;

        //User input containers----
        private MouseState currentMouseState;
        private MouseState prevMousState;
        private KeyboardState currentKeyboardState;
        private KeyboardState prevKeyboardState;

        //Start button for the main menu----
        private Button startButton;
        private Texture2D startHoverSprite;
        private Texture2D startUpSprite;

        //Manager classes----
        public static ProjectileManager projectileManager;
        public static PlayerManager playerManager;
        public static EnemyManager enemyManager;
        public static LevelManager levelManager;

        private Shotgun shotgun;
        private PlayerWeapon currentWeapon;
        private List<PlayerWeapon> weapons; 

        //Player
        Texture2D playerSprite;
        Player player;
        KeyboardState kbState;

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

            //Setting up manager classes (except PlayerManager, which gets set up in load)----
            projectileManager = new ProjectileManager();
            enemyManager = new EnemyManager(this);
            levelManager = new LevelManager(this);

            //Setting up keyboard and mouse states----
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
            prevMousState = currentMouseState;
            prevKeyboardState = currentKeyboardState;

            weapons = new List<PlayerWeapon>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSprite = Content.Load<Texture2D>("square");

            //Aidan - I had to initialize these after the player sprite was loaded or
            //I'd get a null pointer error for the sprite texture
            
            player = new Player(200, 200, 40, 40, playerSprite, true, 100);
            playerManager = new PlayerManager(player, 6, 3, -10.5f, .6f);

            levelManager.GenerateTestLevel();


            // TODO: use this.Content to load your game content here


            //Creating the start button for the main menu----
            startHoverSprite = Content.Load<Texture2D>("start_ovr");
            startUpSprite = Content.Load<Texture2D>("start_up");
            startButton = new Button(startUpSprite, startHoverSprite, 150, 300, 200, 100);

            shotgun = new Shotgun((int)player.XPos, (int)player.YPos, 40, 40, playerSprite, true, 1, 1, 0, playerSprite);
            currentWeapon = shotgun;
            weapons.Add(shotgun);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            kbState = Keyboard.GetState();
            playerManager.KBState = kbState;
            //Refreshing current inputs-----
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();

            //FINITE STATE MACHINE: PUT YOUR STUFF IN HERE----
            //Main menu----
            if (currentGameState == GameState.MainMenu)
            {
                //Clicking the start button puts the game into the Level state----
                if (startButton.CheckForClick(currentMouseState, prevMousState))
                {
                    currentGameState = GameState.Level;
                }
            }
            //Level state----
            else if (currentGameState == GameState.Level)
            {
                //Updates the weapons position based on player's position
                currentWeapon.XPos = player.XPos;
                currentWeapon.CenteredY = player.CenteredY;

                if(currentMouseState.LeftButton == ButtonState.Pressed && prevMousState.LeftButton == ButtonState.Released)
                {
                    currentWeapon.Shoot();
                }

                foreach(PlayerWeapon weapon in weapons)
                {
                    weapon.UpdateCooldown(gameTime);
                }

                //Player Physics
                playerManager.MovePlayer();
                playerManager.ApplyPlayerGravity();
                playerManager.CheckForCollisions();

                //Running enemy behaviors----
                enemyManager.MoveEnemies();
                enemyManager.SimulateBehaviors(gameTime);

                //Simulating projectiles----
                projectileManager.Simulate(gameTime);

                //Checking map objectives----
                levelManager.RunLevel();

                //Garbage collection methods----
                projectileManager.CollectGarbage();
                enemyManager.RemoveDeadEnemies();
            }
            //Victory screen----
            else if (currentGameState == GameState.Victory)
            {

            }

            //THIS SHOULD ALWAYS REMAIN LAST IN UPDATE. Refreshing previous inputs----
            prevMousState = currentMouseState;
            prevKeyboardState = currentKeyboardState;
            playerManager.PrevKB = currentKeyboardState;
            playerManager.PrevMouse = prevMousState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            //Using the finite the finite state machine to determine what gets drawn to the screen----
            //Main menu----
            if (currentGameState == GameState.MainMenu)
            {
                startButton.Draw(_spriteBatch);
            }
            //Level state----
            else if (currentGameState == GameState.Level)
            {
                levelManager.DrawLevel(_spriteBatch);

                enemyManager.Draw(_spriteBatch, Color.Red);

                projectileManager.Draw(_spriteBatch, Color.Red);

                projectileManager.Simulate(gameTime);
                
                player.Draw(_spriteBatch, Color.Blue);
            }
            //Victory screen----
            else if (currentGameState == GameState.Victory)
            {

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
