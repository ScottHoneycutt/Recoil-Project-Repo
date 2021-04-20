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

        private Texture2D onHoverSprite;
        private Texture2D onUpSprite;
        private Texture2D offHoverSprite;
        private Texture2D offUpSprite;
        private Button godModeToggleOn;
        private Button godModeToggleOff;

        private Texture2D menuBGSprite;
        private Rectangle menuBGRect;

        //Victory variables----
        private Button nextButton;
        private Texture2D nextHoverSprite;
        private Texture2D nextUpSprite;

        private Texture2D victoryBGSprite;

        //Manager classes----
        public static ProjectileManager projectileManager;
        public static PlayerManager playerManager;
        public static EnemyManager enemyManager;
        public static LevelManager levelManager;
        public static WeaponManager weaponManager;

        //Level background
        public static Texture2D background;

        //Player
        private Texture2D playerSprite;
        private Player player;
        private KeyboardState kbState;


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
            _graphics.PreferredBackBufferWidth = 1500;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();

            //Setting up manager classes (except PlayerManager, which gets set up in load)----
            projectileManager = new ProjectileManager(this);
            enemyManager = new EnemyManager(this, .6f);
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

            playerSprite = Content.Load<Texture2D>("PinkGuyMid");

            //Aidan - I had to initialize these after the player sprite was loaded or
            //I'd get a null pointer error for the sprite texture
            
            player = new Player(200, 200, 60, 80, playerSprite, true, 100);
            playerManager = new PlayerManager(player, 4, 4, -10.5f, .6f, 1, .25f, 1, this);

            // TODO: use this.Content to load your game content here
            //load bg for levels
            background = Content.Load<Texture2D>("GameBG");

            //Creating the main menu----
            //Start button----
            startHoverSprite = Content.Load<Texture2D>("start_ovr");
            startUpSprite = Content.Load<Texture2D>("start_up");
            startButton = new Button(startUpSprite, startHoverSprite, 600, 800, 300, 150);

            //God mode toggle----
            onHoverSprite = Content.Load<Texture2D>("on button hover");
            onUpSprite = Content.Load<Texture2D>("on button up");
            offHoverSprite = Content.Load<Texture2D>("off button hover");
            offUpSprite = Content.Load<Texture2D>("off button up");
            godModeToggleOn = new Button(onUpSprite, onHoverSprite, 940, 630, 200, 100);
            godModeToggleOn.IsActive = false;
            godModeToggleOff = new Button(offUpSprite, offHoverSprite, 940, 630, 200, 100);

            //Background image for main menu----
            menuBGSprite = Content.Load<Texture2D>("Recoil start menu");
            menuBGRect = new Rectangle(0, 0, 1500, 1000);

            //Creating the victory menu----
            //Next button
            nextHoverSprite = Content.Load<Texture2D>("next_ovr");
            nextUpSprite = Content.Load<Texture2D>("next_up");
            nextButton = new Button(nextUpSprite, nextHoverSprite, 650, 700, 200, 100);

            //Victory background----
            victoryBGSprite = Content.Load<Texture2D>("Victory background");
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
                //Toggle god mode buttons----
                //Turning god mode on----
                if (godModeToggleOff.CheckForClick(currentMouseState, prevMousState))
                {
                    playerManager.PlayerObject.SetGodMode(true);
                    //Swapping active buttons----
                    godModeToggleOff.IsActive = false;
                    godModeToggleOn.IsActive = true;
                }
                //Turning god mode off----
                else if (godModeToggleOn.CheckForClick(currentMouseState, prevMousState))
                {
                    playerManager.PlayerObject.SetGodMode(false);
                    //Swapping active buttons----
                    godModeToggleOff.IsActive = true;
                    godModeToggleOn.IsActive = false;
                }

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

                weaponManager.UpdatePosition();
                IsMouseVisible = false;

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
                playerManager.UpdateAnimation(gameTime);
                playerManager.HandleMovementState();

                //Running enemy behaviors----
                
                enemyManager.MoveEnemies();
                enemyManager.CheckForCollisions();
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
                //Draw the background image first----
                _spriteBatch.Draw(menuBGSprite, menuBGRect, Color.White);

                //Start button----
                startButton.Draw(_spriteBatch);

                //God mode buttons (draw method handles whether they should be drawn based upon active/inactive)----
                godModeToggleOff.Draw(_spriteBatch);
                godModeToggleOn.Draw(_spriteBatch);
            }
            //Level state----
            else if (currentGameState == GameState.Level)
            {
                //crops out a section of the background and fits it to the window
                _spriteBatch.Draw(background,
                    new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                    new Rectangle(0, 0, 300, 300), Color.White);
                levelManager.DrawLevel(_spriteBatch);

                enemyManager.Draw(_spriteBatch, Color.White);

                projectileManager.Draw(_spriteBatch, Color.White);

                projectileManager.Simulate(gameTime);

                //player.Draw(_spriteBatch, Color.White);
                playerManager.AnimatePlayer(_spriteBatch);

                if(weaponManager.CurrentWeapon != null)
                {
                    weaponManager.Draw(_spriteBatch, Color.White);
                }

                //Drawing UI----
                levelManager.DrawUI(_spriteBatch);

                //Draw crosshair
                weaponManager.DrawCrosshair(_spriteBatch);
            }
            //Victory screen----
            else if (currentGameState == GameState.Victory)
            {
                //Drawing the background----
                _spriteBatch.Draw(victoryBGSprite, menuBGRect, Color.White);

                //Drawing the "next" button----
                nextButton.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
