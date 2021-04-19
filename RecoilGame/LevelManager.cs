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
    /// 3/22/2021----
    /// Manager class for the game's levels----
    /// </summary>
    public class LevelManager
    {
        //Map stuff----
        private List<MapTile> listOfMapTiles;
        private List<MapTile> textureTiles;
        private int currentLevel;
        private int numberOfLevels;
        private MapTile objectiveTile;
        private int playerSpawnX;
        private int playerSpawnY;
        private Game1 gameRef;

        //UI elements----
        private Texture2D testSprite;
        private SpriteFont arial20;
        private Texture2D shotgunUI;
        private Texture2D shotgunUIUnequipped;
        private Texture2D rocketLauncherUI;
        private Texture2D rocketUIUnequipped;
        //General----
        private Rectangle healthBarBackground;
        private Rectangle healthBar;
        private Text levelDisplay;
        //Weapon statuses----
        private Rectangle shotgunBG;
        private Rectangle shotgunCD;
        private Rectangle rocketBG;
        private Rectangle rocketCD;

        //Property to easily get the list of all MapTiles----
        public List<MapTile> ListOfMapTiles
        {
            get
            {
                return listOfMapTiles;
            }
        }

        //Get property for the current level----
        public int CurrentLevel
        {
            get
            {
                return currentLevel;
            }
        }

        public void ResetCurrentLevel()
        {
            currentLevel--;
            //GenerateLevelFromFile("level" +currentLevel);
        }

        /// <summary>
        /// Constructor for the LevelManager class. Takes in the instance of Game1 so that
        /// assets can be loaded in from Content----
        /// </summary>
        /// <param name="game">The instance of Game1 used to access Content----</param>
        public LevelManager(Game1 game)
        {
            listOfMapTiles = new List<MapTile>();
            textureTiles = new List<MapTile>();
            currentLevel = 0;
            
            //Setting objectiveTile to null until one appears in a level----
            objectiveTile = null;
            numberOfLevels = 3;

            //Loading in sprites and spritefonts----
            testSprite = game.Content.Load<Texture2D>("square");
            shotgunUI = game.Content.Load<Texture2D>("recoil shotgun UI");
            shotgunUIUnequipped = game.Content.Load<Texture2D>("recoil shotgun UI unequipped");
            rocketLauncherUI = game.Content.Load<Texture2D>("recoil rocket launcher UI");
            rocketUIUnequipped = game.Content.Load<Texture2D>("recoil rocket launcher Unequipped");

            arial20 = game.Content.Load<SpriteFont>("Arial20");

            //Setting up UI elements----
            healthBarBackground = new Rectangle(20, 20, 500, 20);
            healthBar = new Rectangle(20, 20, 500, 20);
            levelDisplay = new Text(arial20, new Vector2(20, 45), "Level " + currentLevel);

            //Setting up weapon cooldown displays for the UI----
            shotgunBG = new Rectangle(1290, 930, 50, 50);
            shotgunCD = new Rectangle(1290, 930, 50, 0);
            rocketBG = new Rectangle(1360, 930, 50, 50);
            rocketCD = new Rectangle(1360, 930, 50, 0);
        }

        /// <summary>
        /// Generates a test level for... testing, duh----
        /// </summary>
        public void GenerateTestLevel()
        {
            //Left boundary----
            listOfMapTiles.Add(new MapTile(0, 0, 50, 500, testSprite, true, false));
            //Floor----
            listOfMapTiles.Add(new MapTile(0, 500, 550, 50, testSprite, true, false));
            //Right boundary----
            listOfMapTiles.Add(new MapTile(500, 0, 50, 500, testSprite, true, false));
            //Platform----
            listOfMapTiles.Add(new MapTile(150, 300, 200, 50, testSprite, true, false));
            //Objective----
            objectiveTile = new MapTile(450, 450, 50, 50, testSprite, true, true);

            //Spawning in the player----
            Game1.playerManager.PlayerObject.Position = new Vector2(100, 100);
            Game1.playerManager.PlayerObject.ConvertPosToRect();

            //Creating a single enemy for testing purposes (does not need to be stored because
            //it automatically stores itself in the EnemyManager)----
            new Enemy(250, 250, 50, 50, testSprite, true, new Vector2(0, 0), 10, 3, 10);
        }

        /// <summary>
        /// The main method run in Update() that controls level generation and progression----
        /// </summary>
        /// <returns>True if the game should stay in level state, false if it should move to victory----</returns>
        public bool RunLevel()
        {
            //Starting the first level----
            if (currentLevel == 0)
            {
                currentLevel++;

                Game1.weaponManager.AddWeapon(currentLevel);

                //GenerateLevelFromFile();
                GenerateTestLevel();
            }

            //If the objective has been completed----
            if (ObjectiveReached())
            {
                //Removing all explosions and projectiles----
                Game1.projectileManager.ClearAll();


                if (currentLevel == numberOfLevels)
                {
                    currentLevel = 0;
                    return false;
                }

                currentLevel++;

                foreach(PlayerWeapon weapon in Game1.weaponManager.Weapons)
                {
                    weapon.UpdateCooldown(0);
                }

                Game1.weaponManager.AddWeapon(currentLevel);

                //Cleaning up the old level and transitioning to the new one----
                listOfMapTiles.Clear();
                objectiveTile = null;
                Game1.enemyManager.ListOfEnemies.Clear();
                //GenerateLevelFromFile();
                GenerateTestLevel();
            }
            return true;
        }

        /// <summary>
        /// Draws all Maptiles in the current level to the screen----
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the MapTiles----</param>
        public void DrawLevel(SpriteBatch sb)
        {
            //Drawing objective first (if it exists)----
            if (objectiveTile != null)
            {
                objectiveTile.Draw(sb, Color.Yellow);
            }

            //Drawing all other tiles----
            foreach (MapTile tile in listOfMapTiles)
            {
                tile.Draw(sb, Color.White);
            }
        }

        /// <summary>
        /// Helper method. Converts a text file into a level that can be run in-game.
        /// Used for level progression----
        /// </summary>
        /// <param name="fileName">The name of the file to access for the level to generate----</param>
        private void GenerateLevelFromFile(string fileName)
        {
            //NEEDS TO BE WORKED ON IN THE FUTURE----
        }

        /// <summary>
        /// Helper method. Determines whether or not the player is intersecting with the objective 
        /// tile if it exists, or if all the enemies are dead if it does not exist----
        /// </summary>
        /// <returns>True if the player is touching the objective, false otherwise----</returns>
        private bool ObjectiveReached()
        {
            //objectiveTile exists (and thus is the objective)----
            if (objectiveTile != null)
            {
                if (Game1.playerManager.PlayerObject.ObjectRect.Intersects(objectiveTile.ObjectRect))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //objectiveTile does not exist (and thus killing all enemies is the objective)----
            else
            {
                if (Game1.enemyManager.ListOfEnemies.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            
        }

        /// <summary>
        /// Updates the UI. The nuts and bolt of the UI that's handled in Update rather than Draw----
        /// </summary>
        public void UpdateUI()
        {
            //Grabbing a reference to the player----
            Player player = Game1.playerManager.PlayerObject;

            //Updating player's health bar to match the player's health----
            healthBar.Width = (int)((float)player.Health / (float)player.MaxHealth * 
                (float)healthBarBackground.Width);

            //Updating level display----
            levelDisplay.TextString = "Level " + currentLevel;

            //Updating weapon cooldown displays----
            float shotgunMaxCD = Game1.weaponManager.Weapons.First.Value.CooldownAmt;
            float shotgunCDfloat = Game1.weaponManager.Weapons.First.Value.CurrentCooldown;

            //Shotgun cooldown display----
            //Preventing division by 0----
            if (shotgunCDfloat <= 0)
            {
                shotgunCD.Height = 0;
            }
            else
            {
                shotgunCD.Height = (int)((float)shotgunBG.Height * shotgunCDfloat / shotgunMaxCD);
            }

            //Only continue if the rocket launcher exists in the player's list of weapons----
            if (Game1.weaponManager.Weapons.First.Next != null)
            {
                float rocketMaxCD = Game1.weaponManager.Weapons.First.Next.Value.CooldownAmt;
                float rocketCDfloat = Game1.weaponManager.Weapons.First.Next.Value.CurrentCooldown;

                //Rocket Launcher cooldown display----
                //Preventing division by 0----
                if (rocketCDfloat <= 0)
                {
                    rocketCD.Height = 0;
                }
                else
                {
                    rocketCD.Height = (int)((float)rocketBG.Height * rocketCDfloat / rocketMaxCD);
                }
            }
        }

        /// <summary>
        /// Draws the player's UI to the screen. This should be called after every other draw method
        /// because it should overlay over everything else----
        /// </summary>
        /// <param name="sb">Spritebatch used to draw everthing----</param>
        public void DrawUI(SpriteBatch sb)
        {
            //Drawing the health bar in the top left corner----
            //Health bar background----
            sb.Draw(testSprite, healthBarBackground, Color.White);
            //Health bar (remaining health)----
            sb.Draw(testSprite, healthBar, Color.DeepPink);

            //Current level display----
            levelDisplay.Draw(sb);

            //Drawing the UI for the weapons----
            //Backgrounds, color based upon whether or not they are selected----

            if (Game1.weaponManager.CurrentWeapon != null && Game1.weaponManager.Weapons.First != null)
            {
                if (Game1.weaponManager.CurrentWeapon == Game1.weaponManager.Weapons.First.Value)
                {
                    //Current weapon is the shotgun----
                    sb.Draw(shotgunUI, shotgunBG, Color.White);
                    sb.Draw(rocketUIUnequipped, rocketBG, Color.White);
                }
                else
                {
                    //Current weapon is the rocket launcher----
                    sb.Draw(rocketLauncherUI, rocketBG, Color.White);
                    sb.Draw(shotgunUIUnequipped, shotgunBG, Color.White);
                }

                //Cooldowns----
                sb.Draw(testSprite, shotgunCD, Color.LightGray);
                sb.Draw(testSprite, rocketCD, Color.LightGray);
            }
        }
    }
}
