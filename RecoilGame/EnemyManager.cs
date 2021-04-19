using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace RecoilGame
{
    // Name: Jack Walsh
    // Date: 3/17/2021
    // Purpose: 
    public class EnemyManager
    {
        // Variables
        private List<Enemy> listOfEnemies;
        private List<Enemy> deadEnemies;
        private Texture2D projectileSprite;
        private float enemyGravity;

        // Properties
        public List<Enemy> ListOfEnemies
        {
            get
            {
                return listOfEnemies;
            }
        }

        /// <summary>
        /// Constructor for EnemyManager
        /// </summary>
        public EnemyManager(Game1 game, float gravity)
        {
            listOfEnemies = new List<Enemy>();
            deadEnemies = new List<Enemy>();
            projectileSprite = game.Content.Load<Texture2D>("square");
            enemyGravity = gravity;

        }

        /// <summary>
        /// Move enemies if active or send them to dead enemies
        /// </summary>
        public void MoveEnemies()
        {
            foreach(Enemy enemy in listOfEnemies)
            {
                if (enemy.IsActive)
                {

                    //adds all the velocity vectors up and applies to position
                    enemy.YVelocity += enemyGravity;
                    enemy.Position += enemy.Velocity;
                    enemy.ConvertPosToRect();
                    //System.Diagnostics.Debug.WriteLine(enemy.ObjectRect.X);
                    //System.Diagnostics.Debug.WriteLine(enemy.YPos);

                } else
                {
                    deadEnemies.Add(enemy);
                }
            }
        }

        /// <summary>
        /// Simulates the shooting behavior of all enemies. Should be called after MoveEnemies----
        /// </summary>
        public void SimulateBehaviors(GameTime gameTime)
        {
            //Tell each enemy to simulate themselves----
            foreach (Enemy enemy in listOfEnemies)
            {
                enemy.SimulateBehaviors(Game1.playerManager.PlayerObject, gameTime, projectileSprite);
            }
        }

        /// <summary>
        /// Reports the existence of new enemies to the EnemyManager so that it can be added to the
        /// list of active enemies----
        /// </summary>
        /// /// <param name="newEnemy">The enemy to add to the list----</param>
        public void ReportExists(Enemy newEnemy)
        {
            listOfEnemies.Add(newEnemy);
        }

        /// <summary>
        /// Remove dead enemies from list of enemies and clears deadEnemy list
        /// </summary>
        public void RemoveDeadEnemies()
        {
            foreach(Enemy deadEnemy in deadEnemies)
            {
                listOfEnemies.Remove(deadEnemy);
            }
            deadEnemies.Clear();
        }

        /// <summary>
        /// Tells all enemies to draw themselves----
        /// </summary>
        /// <param name="sb">The spritebatch to be used for the drawing----</param>
        /// <param name="tint">The color to draw the sprites in----</param>
        public void Draw(SpriteBatch sb, Color tint)
        {
            foreach (Enemy enemy in listOfEnemies)
            {
                enemy.Draw(sb, tint);
            }
        }

        /// <summary>
        /// Handles collisions between enemy and the MapTiles
        /// </summary>
        public void CheckForCollisions()
        {
            foreach(Enemy enemy in listOfEnemies)
            {
                Rectangle enemyRect = enemy.ObjectRect;
                enemy.ConvertPosToRect();

                //Checking for collisions between the player and all maptiles----
                foreach (MapTile mapTile in Game1.levelManager.ListOfMapTiles)
                {

                    Rectangle tileRect = mapTile.ObjectRect;

                    //If a collision is detected between player and a maptile----
                    if (enemyRect.Intersects(tileRect))
                    {

                        Rectangle intersection = Rectangle.Intersect(
                            enemyRect,
                            tileRect);

                        if (intersection.Width < intersection.Height)
                        {
                            //checking to see which direction to move
                            if (enemyRect.X < tileRect.X)
                            {
                                enemyRect.X -= intersection.Width;
                                //System.Diagnostics.Debug.WriteLine("Wall to the right");

                                //Resetting x velocity components moving towards the wall----
                                if (enemy.Velocity.X > 0)
                                {
                                    enemy.XVelocity = 0;
                                }

                            }
                            else
                            {
                                enemyRect.X += intersection.Width;
                                //System.Diagnostics.Debug.WriteLine("Wall to the left");

                                //Resetting x velocity components moving towards the wall----
                                if (enemy.Velocity.X < 0)
                                {
                                    enemy.XVelocity = 0;
                                }
                            }
                        }
                        //if height is less than width then the player is moved up or down
                        if (intersection.Height <= intersection.Width)
                        {
                            //checking to see which direction to move
                            //Tile is below the player----
                            if (enemyRect.Y <= tileRect.Y)
                            {
                                enemyRect.Y -= intersection.Height;
                                //Velocity has to add to 1.0 with gravity to maintain consistent collision detection
                                //with the floor
                                enemy.YVelocity = 1 - enemyGravity;

                            }
                            //Tile is above the player----
                            else
                            {

                                enemyRect.Y += intersection.Height;
                                enemy.YVelocity = 0;
                                //System.Diagnostics.Debug.WriteLine("Wall above");
                            }
                        }

                    }

                }
                //setting final positions
                enemy.XPos = enemyRect.X;
                enemy.YPos = enemyRect.Y;

                //System.Diagnostics.Debug.WriteLine(enemy.XPos);
                //System.Diagnostics.Debug.WriteLine(enemy.YPos);

                //Converting the position vector to a rectangle
                enemy.ConvertPosToRect();
                //System.Diagnostics.Debug.WriteLine(enemy.ObjectRect.X);
                //System.Diagnostics.Debug.WriteLine(enemy.ObjectRect.Y);
            }
            
        }
    }
}
