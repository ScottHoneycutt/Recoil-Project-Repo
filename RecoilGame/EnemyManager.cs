using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilGame
{
    // Name: Jack Walsh
    // Date: 3/17/2021
    // Purpose: 
    class EnemyManager
    {
        public static List<Enemy> listOfEnemies;

        public void MoveEnemies()
        {
            foreach(Enemy enemy in listOfEnemies)
            {
                if (enemy.IsActive)
                {
                }
            }
        }
    }
}
