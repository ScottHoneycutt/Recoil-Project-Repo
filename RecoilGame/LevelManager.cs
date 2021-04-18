using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Reflection;
using System.Diagnostics;

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
        public void RunLevel()
        {
            //Starting the first level----
            if (currentLevel == 0)
            {
                currentLevel++;
                //GenerateLevelFromFile();
                GenerateTestLevel();
            }

            //If the objective has been completed----
            if (ObjectiveReached())
            {
                currentLevel++;

                //Cleaning up the old level and transitioning to the new one----
                listOfMapTiles.Clear();
                objectiveTile = null;
                Game1.enemyManager.ListOfEnemies.Clear();
                GenerateLevelFromFile(game, "testLevel");
                //GenerateTestLevel();
            }
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

            StreamReader input = null;
            try
            {
                //get file path
                String path = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location);
                if (String.Compare(path.Substring(path.Length-1, 1), "\\") != 0)
                {
                    path += "\\";
                }

                Stream inStream = new FileStream(path + "..\\data\\" + fileName, FileMode.Open);
                input = new StreamReader(inStream);

                // first two pieces are length / width of map
                int tilesAcross = Convert.ToInt32(input.ReadLine());
                int tilesDown = Convert.ToInt32(input.ReadLine());

                Vector2 playerPos = default;

                // convert character to enum
                for (int i = 0; i < tilesAcross; i++)
                {
                    string rowOfChar = input.ReadLine();
                    for (int j = 0; j < tilesDown; j++)
                    {
                        char charTileToPlace = rowOfChar[j];

                        Texture2D textureFromChar =
                            GetTextureFromChar(charTileToPlace, game);

                        listOfMapTiles.Add(
                            new MapTile(
                                i,
                                j,
                                16,
                                16,
                                textureFromChar,
                                true,
                                charTileToPlace == 'o'
                                ));

                        if (charTileToPlace == 'p')
                        {
                            playerPos = new Vector2(i * 16, j * 16);
                        }

                    }
                }

                Game1.playerManager.PlayerObject.Position = playerPos;
                Game1.playerManager.PlayerObject.ConvertPosToRect();
            } catch (Exception e)
            {

            }
        }

        private Texture2D GetTextureFromChar(char charRepresentingTexture)
        {
            Texture2D charAsTexture = null;
            switch (charRepresentingTexture)
            {
                case 'w':
                    charAsTexture = Game1. // Content.Load<Texture2D>("wall");
                    break;
                case 'f':
                    charAsTexture = game.Content.Load<Texture2D>("floor");
                    break;
                case 'a':
                    charAsTexture = game.Content.Load<Texture2D>("air");
                    break;
                case 'l':
                    charAsTexture = game.Content.Load<Texture2D>("platformleft");
                    break;
                case 'm':
                    charAsTexture = game.Content.Load<Texture2D>("platformmiddle");
                    break;
                case 'r':
                    charAsTexture = game.Content.Load<Texture2D>("platformright");
                    break;
                case 'o':
                    charAsTexture = game.Content.Load<Texture2D>("objective");
                    break;
                case 'p':
                    charAsTexture = game.Content.Load<Texture2D>("player");
                    break;
                case 'e':
                    charAsTexture = game.Content.Load<Texture2D>("enemy");
                    break;
            }

            return charAsTexture;
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
