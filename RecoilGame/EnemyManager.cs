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
        public EnemyManager(Game1 game)
        {
            listOfEnemies = new List<Enemy>();
            deadEnemies = new List<Enemy>();
            projectileSprite = game.Content.Load<Texture2D>("square");
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
                    enemy.Move();
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
    }
}
