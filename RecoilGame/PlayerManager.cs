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
        Airborn,
        Jump
    }
    public class PlayerManager
    {
        //Aidan Kamp
        //3/22/21
        //Updated fields, added MovePlayer() method,
        //PlayerState enum, added CheckForCollisions method

        //Aidan Kamp
        //3/29/21
        //Debugged Collision Detection, Added an Add Velco

        private Player playerObject;
        private List<PlayerWeapon> weaponList;
        private PlayerWeapon currentWeapon;
        private KeyboardState prevKBState;
        private KeyboardState kbState;
        private MouseState prevMouseState;
        private MouseState mState;
        private PlayerState playerState;
        private float groundSpeedX;
        private float airSpeedX;
        private Vector2 playerVelocity;
        private Vector2 jumpVelocity;
        private Vector2 playerGravity;
        private Shotgun playerShotgun;
        private bool isColliding;

        public Player PlayerObject
        {
            get
            {
                return playerObject;
            }
        }

        public KeyboardState KBState
        {
            get { return kbState; }
            set { kbState = value; }
        }

        public KeyboardState PrevKB
        {
            get { return prevKBState; }
            set { prevKBState = value; }
        }

        public MouseState PrevMouse
        {
            get { return prevMouseState; }
            set { prevMouseState = value; }
        }

        public PlayerState State
        {
            get { return playerState; }
            set { playerState = value; }
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
            isColliding = false;
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


            switch (playerState)
            {
                //grounded and jump states since the direction you face is not impacted by the direction you're moving
                //direction faced will instead be dependent on mouse location (for shooting)
                case PlayerState.Grounded:

                    System.Diagnostics.Debug.WriteLine("Grounded State");
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
                case PlayerState.Airborn:

                    System.Diagnostics.Debug.WriteLine("Airborn State");

                    //player has reduced aerial movement but can still jump
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        playerObject.XPos -= (airSpeedX);
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        playerObject.XPos += (airSpeedX);
                    }
                    if (SingleKeyPress(Keys.W))
                    {
                        playerVelocity.Y = jumpVelocity.Y;
                        playerState = PlayerState.Jump;
                    }
                    break;
                case PlayerState.Jump:
                    System.Diagnostics.Debug.WriteLine("Jump State");

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
            isColliding = false;
            Rectangle playerRect = playerObject.ObjectRect;
            foreach (MapTile mapTile in Game1.levelManager.ListOfMapTiles)
            {
                Rectangle tileRect = mapTile.ObjectRect;
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
                            playerVelocity.X = 0;
                            //System.Diagnostics.Debug.WriteLine("Wall to the right");

                            playerObject.XPos = playerRect.X;
                            playerObject.YPos = playerRect.Y;

                            //Converting the position vector to a rectangle
                            playerObject.ConvertPosToRect();
                        }
                        else
                        {
                            playerRect.X += intersection.Width;
                            playerVelocity.X = 0;
                            //System.Diagnostics.Debug.WriteLine("Wall to the left");

                            playerObject.XPos = playerRect.X;
                            playerObject.YPos = playerRect.Y;

                            //Converting the position vector to a rectangle
                            playerObject.ConvertPosToRect();
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
                            playerVelocity.X = 0;


                            //the player is grounded at this state
                            isColliding = true;
                            playerState = PlayerState.Grounded;
                            
                            //System.Diagnostics.Debug.WriteLine("On the ground");

                            playerObject.XPos = playerRect.X;
                            playerObject.YPos = playerRect.Y;

                            //Converting the position vector to a rectangle
                            playerObject.ConvertPosToRect();
                        }
                        else
                        {

                            playerRect.Y += intersection.Height;
                            playerVelocity.Y = 0;
                            //System.Diagnostics.Debug.WriteLine("Wall above");

                            playerObject.XPos = playerRect.X;
                            playerObject.YPos = playerRect.Y;

                            //Converting the position vector to a rectangle
                            playerObject.ConvertPosToRect();
                        }
                    }
                }


                //setting final positions
                playerObject.XPos = playerRect.X;
                playerObject.YPos = playerRect.Y;

                //Converting the position vector to a rectangle
                playerObject.ConvertPosToRect();
            }

            if(isColliding == false && playerState != PlayerState.Jump)
            {
                //System.Diagnostics.Debug.WriteLine("The Game Thinks we're in the air");
                playerState = PlayerState.Airborn;
            }
        }

        /// <summary>
        /// Applies gravity to the player
        /// </summary>
        public void ApplyPlayerGravity()
        {

            playerVelocity += playerGravity;
            playerObject.Position += playerVelocity;
            playerObject.ConvertPosToRect();

        }

        /// <summary>
        /// Takes in a velocity vector and adds it to the current player velocity
        /// </summary>
        /// <param name="velocity"></param>
        public void AddVelocity(Vector2 velocity)
        {
            playerVelocity += velocity;
        }

        public void ShootingCapability()
        {
            float playerRecoil = 15;
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton != ButtonState.Pressed)
            {
                playerVelocity = Vector2.Zero;
                //get direction of vector
                int xMouse = mouseState.X;
                int yMouse = mouseState.Y;
                float xDirection = -1 * (xMouse - playerObject.CenteredX);
                float yDirection = -1 * (yMouse - playerObject.CenteredY);

                //get the magnitude so it can be normalized
                double magnitude = Math.Sqrt((xDirection * xDirection) + (yDirection * yDirection));
                float xNormalized = xDirection / (float)magnitude;
                float yNormalized = yDirection / (float)magnitude;
                Vector2 recoil = new Vector2(xNormalized * (playerRecoil / 2), yNormalized * playerRecoil);

                //Add velocity and adjust location
                playerVelocity += recoil;
                playerObject.ConvertPosToRect();

                //changes playerState to airborn (can still use jump) if done from the ground
                /*
                if(playerState == PlayerState.Grounded)
                {
                    playerState = PlayerState.Airborn;
                }
                */
                
            }
        }
    }
}