using System;
using System.Collections.Generic;
using System.Text;

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
        public EnemyManager()
        {
            listOfEnemies = new List<Enemy>();
            deadEnemies = new List<Enemy>();
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
        /// Remove dead enemies from list of enemies and clears deadEnemy list
        /// </summary>
        public void RemoveDeadEnemies()
        {
            foreach(Enemy deadEnemy in deadEnemies)
            {
                listOfEnemies.Remove(deadEnemy);
                deadEnemies.Clear();
            }
        }
    }
}
