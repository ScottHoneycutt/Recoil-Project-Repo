﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilGame
{
    // Name: Jack Walsh
    // Date: 3/17/2021
    // Purpose: 
    public class EnemyManager
    {
        private List<Enemy> listOfEnemies;

        public EnemyManager()
        {
            listOfEnemies = new List<Enemy>();
        }

        public void MoveEnemies()
        {
            foreach(Enemy enemy in listOfEnemies)
            {
                if (enemy.IsActive)
                {
                    enemy.Move();
                }
            }
        }
    }
}
