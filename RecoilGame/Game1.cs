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

        //Menu variables----
        private Button startButton;
        private Texture2D startHoverSprite;
        private Texture2D startUpSprite;
        private Button nextButton;
        private Texture2D nextHoverSprite;
        private Texture2D nextUpSprite;

        //Manager classes----
        public static ProjectileManager projectileManager;
        public static PlayerManager playerManager;
        public static EnemyManager enemyManager;
        public static LevelManager levelManager;
        public static WeaponManager weaponManager;

        //background
        public static Texture2D background;

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
            projectileManager = new ProjectileManager(this);
            enemyManager = new EnemyManager(this);
            levelManager = new LevelManager(this);
            weaponManager = new WeaponManager(this);

            //Setting up keyboard and mouse states----
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
            prevMousState = currentMouseState;
            prevKeyboardState = currentKeyboardState;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSprite = Content.Load<Texture2D>("square");

            //Aidan - I had to initialize these after the player sprite was loaded or
            //I'd get a null pointer error for the sprite texture
            
            player = new Player(200, 200, 40, 40, playerSprite, true, 100);
            playerManager = new PlayerManager(player, 4, 4, -10.5f, .6f, 1, .25f, 1);

            // TODO: use this.Content to load your game content here
            //load bg
            background = Content.Load<Texture2D>("GameBG");

            //Creating the main menu----
            startHoverSprite = Content.Load<Texture2D>("start_ovr");
            startUpSprite = Content.Load<Texture2D>("start_up");
            startButton = new Button(startUpSprite, startHoverSprite, 150, 300, 200, 100);


            //Creating the victory menu----
            nextHoverSprite = Content.Load<Texture2D>("next_ovr");
            nextUpSprite = Content.Load<Texture2D>("next_up");
            nextButton = new Button(nextUpSprite, nextHoverSprite, 150, 300, 200, 100);
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
                
                if (weaponManager.CurrentWeapon != null)
                {
                    weaponManager.CurrentWeapon.XPos = player.XPos;
                    weaponManager.CurrentWeapon.CenteredY = player.CenteredY;
                }

                if (currentMouseState.ScrollWheelValue != prevMousState.ScrollWheelValue)
                {
                    weaponManager.SwitchWeapon(currentMouseState.ScrollWheelValue, prevMousState.ScrollWheelValue);
                }

                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMousState.LeftButton == ButtonState.Released)
                {
                    weaponManager.CurrentWeapon.Shoot();
                }

                foreach(PlayerWeapon weapon in weaponManager.Weapons)
                {
                    weapon.UpdateCooldown(gameTime);
                }

                //Player Physics
                playerManager.MovePlayer();
                playerManager.CheckForCollisions();

                //Running enemy behaviors----
                enemyManager.MoveEnemies();
                enemyManager.SimulateBehaviors(gameTime);

                //Simulating projectiles----
                projectileManager.Simulate(gameTime);

                //Checking map objectives and updating UI----
                bool shouldStayLevel = levelManager.RunLevel();
                levelManager.UpdateUI();

                //Garbage collection methods----
                projectileManager.CollectGarbage();
                enemyManager.RemoveDeadEnemies();

                //Progress to the victory screen if all levels have been completed----
                if (!shouldStayLevel)
                {
                    currentGameState = GameState.Victory;
                }
            }
            //Victory screen----
            else if (currentGameState == GameState.Victory)
            {
                //Clicking the next button puts the game into the MainMenu state----
                if (nextButton.CheckForClick(currentMouseState, prevMousState))
                {
                    weaponManager.Weapons.Clear();
                    currentGameState = GameState.MainMenu;
                }
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
                //crops out a section of the background and fits it to the window
                _spriteBatch.Draw(background,
                    new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                    new Rectangle(0, 0, 300, 300), Color.White);
                levelManager.DrawLevel(_spriteBatch);

                enemyManager.Draw(_spriteBatch, Color.Red);

                projectileManager.Draw(_spriteBatch, Color.Red);

                projectileManager.Simulate(gameTime);
                
                player.Draw(_spriteBatch, Color.Blue);

                //Drawing UI----
                levelManager.DrawUI(_spriteBatch);
            }
            //Victory screen----
            else if (currentGameState == GameState.Victory)
            {
                nextButton.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
