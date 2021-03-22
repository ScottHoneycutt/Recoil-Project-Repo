using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    public enum PlayerState
    {
        FaceLeft,
        FaceRight,
        WalkLeft,
        WalkRight,
        Grounded,
        Jump
    }
    public class PlayerManager
    {
        //Aidan Kamp
        //3/22/21
        //Updated fields, added MovePlayer() method

        private Player playerObject;
        private List<PlayerWeapon> weaponList;
        private KeyboardState prevKBState;
        private KeyboardState kbState;
        private MouseState mState;
        private PlayerState playerState;


        public Player PlayerObject
        {
            get
            {
                return playerObject;
            }
        }

        public KeyboardState PrevKB
        {
            get { return prevKBState; }
            set { prevKBState = value; }
        }

        /// <summary>
        /// Method handles player movement using FSM
        /// </summary>
        public void MovePlayer()
        {
            kbState = Keyboard.GetState();

            switch (playerState)
            {
                //grounded and jump states since the direction you face is not impacted by the direction you're moving
                //direction faced will instead be dependent on mouse location (for shooting)
                case PlayerState.Grounded:
                    //can move or jump
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        playerObject.CenteredX -= 5;
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        playerObject.CenteredX += 5;
                    }
                    if (SingleKeyPress(Keys.W))
                    {
                        playerObject.CenteredY -= 5;
                        playerState = PlayerState.Jump;
                    }
                    break;
                case PlayerState.Jump:

                    //can move when you jump but it is less useful.
                    //can't jump again
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        playerObject.CenteredX -= 2;
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        playerObject.CenteredX += 2;
                    }
                    break;
            }
            
        }

        /// <summary>
        /// Detects when a key is pressed once
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool SingleKeyPress(Keys key)
        {
        return Keyboard.GetState().IsKeyDown(key) && prevKBState.IsKeyUp(key);
        }



    }
}
