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
        private List<MapTile> listOfMapTiles;
        private int currentLevel;
        private int numberOfLevels;
        private MapTile objectiveTile;
        private Texture2D testSprite;
        private int playerSpawnX;
        private int playerSpawnY;
        private Game1 gameRef;

        //Property to easily get the list of all MapTiles----
        public List<MapTile> ListOfMapTiles
        {
            get
            {
                return listOfMapTiles;
            }
        }

        /// <summary>
        /// Constructor for the LevelManager class. Takes in the instance of Game1 so that
        /// assets can be loaded in from Content----
        /// </summary>
        /// <param name="game">The instance of Game1 used to access Content----</param>
        public LevelManager(Game1 game)
        {
            listOfMapTiles = new List<MapTile>();
            currentLevel = 0;
            testSprite = game.Content.Load<Texture2D>("square");
            //Setting objectiveTile to null until one appears in a level----
            objectiveTile = null;
            numberOfLevels = 3;
        }

        public int CurrentLevel
        {
            get { return currentLevel; }
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
            System.Diagnostics.Debug.WriteLine(currentLevel);
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

    }
}
