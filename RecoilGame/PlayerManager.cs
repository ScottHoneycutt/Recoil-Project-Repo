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

        /// <summary>
        /// Scott Honeycutt----
        /// 4/6/2021----
        /// Started work on movement acceleration, separating the player inputs from external
        /// effect velocity vectors. Had to rework a lot of stuff for this.----
        /// </summary>

        private Player playerObject;
        private List<PlayerWeapon> weaponList;
        private PlayerWeapon currentWeapon;
        private KeyboardState prevKBState;
        private KeyboardState kbState;
        private MouseState prevMouseState;
        private MouseState mState;
        private PlayerState playerState;
        private Vector2 playerGravity;
        private bool isColliding;

        //Fields created by Scott----
        private Vector2 inputsVelocity;
        private Vector2 effectsVelocity;
        private Vector2 gravityVelocity;
        private float maxGroundSpeed;
        private float maxAirSpeed;
        private float airResistance;
        private float groundFriction;
        private float yJumpVelocity;
        private float inputAcceleration;


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
        /// Constructor for the playermanager class. Contains a reference to the player----
        /// </summary>
        /// <param name="player">The player reference.</param>
        /// <param name="maxInputGroundSpeed">The maximum speed the player can move on the ground
        /// without an external force acting on them----</param>
        /// <param name="maxInputAirSpeed">The maximum speed the player can move in the air
        /// without an external force acting on them----</param>
        /// <param name="yJumpVelocity">The initial velocity when jumping----</param>
        /// <param name="yGravity">The acceleration from gravity----</param>
        /// <param name="friction">The resistance applied to external effects on the ground----</param>
        /// <param name="airResistance">The resistance applied to external effects in the air----</param>
        /// <param name="inputAcceleration">The rate of acceleration for the player's inputs. Air 
        /// acceleration is half the ground acceleration (ground is the value set here)----</param>
        public PlayerManager(Player player, float maxInputGroundSpeed, float maxInputAirSpeed, float yJumpVelocity, 
            float yGravity, float friction, float airResistance, float inputAcceleration)
        {
            playerObject = player;
            playerState = PlayerState.Grounded;
            isColliding = false;
            //creates the vectors using the passed in y values
            playerGravity = new Vector2(0, yGravity);

            //Two different velocity vectors that determine the overall velocity of the player
            //(other than gravity)----
            inputsVelocity = new Vector2(0, 0);
            effectsVelocity = new Vector2(0, 0);

            //These variables only affect inputsVelocity----
            maxGroundSpeed = maxInputGroundSpeed;
            maxAirSpeed = maxInputAirSpeed;
            this.inputAcceleration = inputAcceleration;
            this.yJumpVelocity = yJumpVelocity;

            //These two variables only affect effectsVelocity----
            groundFriction = friction;
            this.airResistance = .25f;

            //Total gravity accumulation----
            gravityVelocity = Vector2.Zero;
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

                    //System.Diagnostics.Debug.WriteLine("Grounded State");
                    //can move or jump

                    //Resetting inputsVector's Y to 0 until the player jumps again----
                    inputsVelocity.Y = 0;
                    gravityVelocity = Vector2.Zero;

                    //Moving left----
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        //Cannot exceed the maximum ground input speed----
                        if (inputsVelocity.X > -maxGroundSpeed)
                        {
                            inputsVelocity.X -= inputAcceleration;
                        }
                    }

                    //Moving right----
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        //Cannot exceed the maximum ground input speed----
                        if (inputsVelocity.X < maxGroundSpeed)
                        {
                            inputsVelocity.X += inputAcceleration;
                        }
                    }

                    //No inputs result in movement stopping at the same rate as if the direction was reversed----
                    if (!kbState.IsKeyDown(Keys.D) && !kbState.IsKeyDown(Keys.A))
                    {
                        if (inputsVelocity.X > inputAcceleration)
                        {
                            inputsVelocity.X -= inputAcceleration;
                        }
                        else if (inputsVelocity.X < -inputAcceleration)
                        {
                            inputsVelocity.X += inputAcceleration;
                        }
                        //Preventing the acceleration from "overshooting" 0----
                        else
                        {
                            inputsVelocity.X = 0;
                        }
                    }

                    //Jumping----
                    if (SingleKeyPress(Keys.W))
                    {
                        inputsVelocity.Y = yJumpVelocity;
                        playerState = PlayerState.Jump;
                    }

                    //Applying ground friction to the effectsVelocity vector to prevent a sudden rigid stop----
                    if (effectsVelocity.X > groundFriction)
                    {
                        effectsVelocity.X -= groundFriction;
                    }
                    else if (effectsVelocity.X < -groundFriction)
                    {
                        effectsVelocity.X += groundFriction;
                    }
                    //Preventing the friction from "overshooting" 0----
                    else
                    {
                        effectsVelocity.X = 0;
                    }


                    break;

                    //Airborne----
                case PlayerState.Airborn:

                    //System.Diagnostics.Debug.WriteLine("Airborn State");

                    //player has reduced aerial movement but can still jump
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        //Cannot exceed the maximum air input speed. Acceleration is half as fast----
                        if (inputsVelocity.X > -maxAirSpeed)
                        {
                            inputsVelocity.X -= inputAcceleration / 2;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        // Cannot exceed the maximum air input speed. Acceleration is half as fast----
                        if (inputsVelocity.X < maxAirSpeed)
                        {
                            inputsVelocity.X += inputAcceleration / 2;
                        }
                    }
                    if (SingleKeyPress(Keys.W))
                    {
                        inputsVelocity.Y = yJumpVelocity;
                        playerState = PlayerState.Jump;
                        gravityVelocity = Vector2.Zero;
                        effectsVelocity.Y = 0;
                    }

                    //No inputs result in movement stopping at the same rate as if the direction was reversed----
                    if (!kbState.IsKeyDown(Keys.D) && !kbState.IsKeyDown(Keys.A))
                    {
                        if (inputsVelocity.X > inputAcceleration / 2)
                        {
                            inputsVelocity.X -= inputAcceleration / 2;
                        }
                        else if (inputsVelocity.X < -inputAcceleration / 2)
                        {
                            inputsVelocity.X += inputAcceleration / 2;
                        }
                        //Preventing the acceleration from "overshooting" 0----
                        else
                        {
                            inputsVelocity.X = 0;
                        }
                    }

                    //Applying air resistance to the effectsVelocity vector to cause decay before hitting the ground----
                    if (effectsVelocity.X > airResistance)
                    {
                        effectsVelocity.X -= airResistance;
                    }
                    else if (effectsVelocity.X < -airResistance)
                    {
                        effectsVelocity.X += airResistance;
                    }
                    //Preventing the air resistance from "overshooting" 0----
                    else
                    {
                        effectsVelocity.X = 0;
                    }
                    break;

                    //Jump has been expended----
                case PlayerState.Jump:

                    //System.Diagnostics.Debug.WriteLine("Jump State");

                    //can move when you jump but it is less useful (half as fast).
                    //can't jump again
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        //Cannot exceed the maximum air input speed. Acceleration is half as fast----
                        if (inputsVelocity.X > -maxAirSpeed)
                        {
                            inputsVelocity.X -= inputAcceleration / 2;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        // Cannot exceed the maximum air input speed. Acceleration is half as fast----
                        if (inputsVelocity.X < maxAirSpeed)
                        {
                            inputsVelocity.X += inputAcceleration / 2;
                        }
                        //playerObject.XPos += (airSpeedX);
                    }

                    //No inputs result in input movements stopping at the same rate as if the direction was reversed----
                    if (!kbState.IsKeyDown(Keys.D) && !kbState.IsKeyDown(Keys.A))
                    {
                        if (inputsVelocity.X > inputAcceleration / 2)
                        {
                            inputsVelocity.X -= inputAcceleration / 2;
                        }
                        else if (inputsVelocity.X < -inputAcceleration / 2)
                        {
                            inputsVelocity.X += inputAcceleration / 2;
                        }
                        //Preventing the acceleration from "overshooting" 0----
                        else
                        {
                            inputsVelocity.X = 0;
                        }
                    }

                    //Applying air resistance to the effectsVelocity vector to cause decay before hitting the ground----
                    if (effectsVelocity.X > airResistance)
                    {
                        effectsVelocity.X -= airResistance;
                    }
                    else if (effectsVelocity.X < -airResistance)
                    {
                        effectsVelocity.X += airResistance;
                    }
                    //Preventing the air resistance from "overshooting" 0----
                    else
                    {
                        effectsVelocity.X = 0;
                    }

                    break;
            }

            //Applying gravity, adding all the vectors up, and simulating movement----
            gravityVelocity += playerGravity;
            playerObject.Position += effectsVelocity + inputsVelocity + gravityVelocity;
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
            //System.Diagnostics.Debug.WriteLine($"{playerVelocity.Y}");

            isColliding = false;
            Rectangle playerRect = playerObject.ObjectRect;
            playerObject.ConvertPosToRect();

            //Checking for collisions between the player and all maptiles----
            foreach (MapTile mapTile in Game1.levelManager.ListOfMapTiles)
            {
                
                Rectangle tileRect = mapTile.ObjectRect;
                
                //If a collision is detected between player and a maptile----
                if (playerRect.Intersects(tileRect))
                {

                    Rectangle intersection = Rectangle.Intersect(
                        playerRect,
                        tileRect);

                    if (intersection.Width < intersection.Height)
                    {
                        //checking to see which direction to move
                        if (playerRect.X < tileRect.X)
                        {
                            playerRect.X -= intersection.Width;
                            //System.Diagnostics.Debug.WriteLine("Wall to the right");

                        }
                        else
                        {
                            playerRect.X += intersection.Width;
                            //System.Diagnostics.Debug.WriteLine("Wall to the left");


                        }
                    }
                    //if height is less than width then the player is moved up or down
                    if (intersection.Height <= intersection.Width)
                    {
                        //checking to see which direction to move
                        //Tile is below the player----
                        if (playerRect.Y <= tileRect.Y)
                        {
                            isColliding = true;

                            playerRect.Y -= intersection.Height;
                            //Velocity has to add to 1.0 with gravity to maintain consistent collision detection
                            //with the floor
                            effectsVelocity.Y = 1 - playerGravity.Y;

                            playerState = PlayerState.Grounded;
                            //System.Diagnostics.Debug.WriteLine($"{isColliding}");
                            //System.Diagnostics.Debug.WriteLine($"{playerObject.YPos}");
                            //System.Diagnostics.Debug.WriteLine($"{playerObject.ObjectRect.Y}");

                        }
                        //Tile is above the player----
                        else
                        {

                            playerRect.Y += intersection.Height;
                            effectsVelocity.Y = 0;
                            inputsVelocity.Y = 0;
                            //System.Diagnostics.Debug.WriteLine("Wall above");
                        }
                    }
                    
                }

            }
            //setting final positions
            playerObject.XPos = playerRect.X;
            playerObject.YPos = playerRect.Y;

            //Converting the position vector to a rectangle
            playerObject.ConvertPosToRect();
            
            if (isColliding == false && playerState != PlayerState.Jump)
            {
                //System.Diagnostics.Debug.WriteLine("The Game Thinks we're in the air");
                playerState = PlayerState.Airborn;
            }
        }

        /// <summary>
        /// Takes in a velocity vector and adds it to the current player velocity
        /// </summary>
        /// <param name="velocity"></param>
        public void AddVelocity(Vector2 velocity)
        {
            effectsVelocity += velocity;

            //Resetting other velocity vectors----
            inputsVelocity.Y = 0;
            gravityVelocity = Vector2.Zero;
        }

        /// <summary>
        /// Moves player proper recoil distance when they shoot the shotgun
        /// </summary>
        public void ShootingCapability()
        {
            float playerRecoil = 15;
            MouseState mouseState = Mouse.GetState();

            //Checking for a single mouse click----
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton != ButtonState.Pressed)
            {
                //Reset input horizontal velocity----
                inputsVelocity.X = 0; 

                //get direction of vector
                int xMouse = mouseState.X;
                int yMouse = mouseState.Y;
                float xDirection = -1 * (xMouse - playerObject.CenteredX);
                float yDirection = -1 * (yMouse - playerObject.CenteredY);

                //get the magnitude so it can be normalized
                //this is so that the direction is maintained but the distance of the mouse from the player
                //doesn't matter
                double magnitude = Math.Sqrt((xDirection * xDirection) + (yDirection * yDirection));
                float xNormalized = xDirection / (float)magnitude;
                float yNormalized = yDirection / (float)magnitude;
                Vector2 recoil = new Vector2(xNormalized * (playerRecoil), yNormalized * playerRecoil);

                //Add velocity
                effectsVelocity = recoil;

                //Resetting other velocity vectors----
                inputsVelocity.Y = 0;
                gravityVelocity = Vector2.Zero;

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