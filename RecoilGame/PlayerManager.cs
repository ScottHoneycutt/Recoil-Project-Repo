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
        //Updated fields, added MovePlayer() method,
        //PlayerState enum, added CheckForCollisions method

        private Player playerObject;
        private List<PlayerWeapon> weaponList;
        private KeyboardState prevKBState;
        private KeyboardState kbState;
        private MouseState mState;
        private PlayerState playerState;
        private float groundSpeedX;
        private float airSpeedX;
        private Vector2 playerVelocity;
        private Vector2 jumpVelocity;
        private Vector2 playerGravity;


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
        /// PlayerManger constructor takes in the player object and values for movement
        /// </summary>
        /// <param name="player"></param> instance of player object
        /// <param name="playerSpeedX"></param> the horizontal speed o fhte player
        /// <param name="yJumpVelocity"></param> the jump velocity vector y value
        /// <param name="yGravity"></param> the gravity vector y value
        public PlayerManager(Player player, float groundSpeedX, float airSpeedX, float yJumpVelocity, float yGravity)
        {
            playerObject = player;
            playerState = PlayerState.Grounded;
            this.groundSpeedX = groundSpeedX;
            this.airSpeedX = airSpeedX;

            //creates the vectors using the passed in y values
            jumpVelocity = new Vector2(0, yJumpVelocity);
            playerGravity = new Vector2(0, yGravity);
            playerVelocity = Vector2.Zero;
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
                        playerObject.XPos -= groundSpeedX;
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        playerObject.XPos += groundSpeedX;
                    }
                    if (SingleKeyPress(Keys.W))
                    {
                        playerVelocity.Y = jumpVelocity.Y;
                        playerState = PlayerState.Jump;
                    }
                    break;
                case PlayerState.Jump:

                    //can move when you jump but it is less useful (half as fast).
                    //can't jump again
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        playerObject.XPos -= (airSpeedX);
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        playerObject.XPos += (airSpeedX);
                    }
                    break;
            }
            
            //moving the rectangle
            playerObject.ConvertPosToRect();
            
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

        /// <summary>
        /// Handles collisions between player and the MapTiles
        /// </summary>
        public void CheckForCollisions()
        {
            Rectangle playerRect = playerObject.ObjectRect;
            for (int i = 0; i < Game1.levelManager.ListOfMapTiles.Count; i++)
            {
                Rectangle tileRect = Game1.levelManager.ListOfMapTiles[i].ObjectRect;
                if (playerRect.Intersects(tileRect))
                {
                    
                    Rectangle intersection = Rectangle.Intersect(
                        playerRect,
                        tileRect);

                    if (intersection.Width <= intersection.Height)
                    {
                        //checking to see which direction to move
                        if (playerRect.X < tileRect.X)
                        {
                            playerRect.X -= intersection.Width;
                        }
                        else
                        {
                            playerRect.X += intersection.Width;
                        }
                    }
                    //if height is less than width than the player is moved up or down
                    if (intersection.Height < intersection.Width)
                    {
                        //checking to see which direction to move
                        if (playerRect.Y < tileRect.Y)
                        {
                            playerRect.Y -= intersection.Height;
                            playerVelocity.Y = 0;

                            //the player is grounded at this state
                            playerState = PlayerState.Grounded;
                        }
                        else
                        {
                            playerRect.Y += intersection.Height;
                            playerVelocity.Y = 0;
                        }
                    }
                }
                //setting final positions
                playerObject.XPos = playerRect.X;
                playerObject.YPos = playerRect.Y;

                //Converting the position vector to a rectangle
                playerObject.ConvertPosToRect();
            }
        }

        /// <summary>
        /// Applies gravity to the player
        /// </summary>
        public void ApplyPlayerGravity()
        {
            
            playerVelocity += playerGravity;
            playerObject.Position += playerVelocity;

        }

        public void AddVelocity(Vector2 velocity)
        {
            playerVelocity += velocity;
        }
    }
}
