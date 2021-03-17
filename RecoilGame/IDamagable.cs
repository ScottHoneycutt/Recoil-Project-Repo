using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilGame
{
    // Name: Jack Walsh
    // Date: 3/17/2021
    // Purpose: Set up base for damage taking for interface users
    interface IDamagable
    {
        /// <summary>
        /// Reduces object's health based on damage taken
        /// </summary>
        /// <param name="damage"> Damage taken </param>
        /// <returns> Remaining health </returns>
        int TakeDamage(int damage);
    }
}
