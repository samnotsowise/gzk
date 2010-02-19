#region Using Statements

using System;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace GZK {

    /// <summary>
    /// This class manages the loading, updating and drawing of all level data
    /// Includes 2 methods taken from ChaseAndEvade Sample from http://creators.xna.com/en-US/sample/chaseevade
    /// </summary>

    public class Level {

        #region Variable Declarations

        public int countScenery, countTargets, countZombies, countAgents, countBonuses;
        private Scenery[] scenery;
        private Target[] targets;
        private Zombie[] zombies;
        private Agent[] agents;
        private Bonus[] bonuses;
        private ContentManager content;
        private GameState gameState;
        private bool loaded, inLevel;
        private GameObject splashScreen, background, indicator, controlBar, paused, zimmer;
        private Texture2D[] textures, zombieTextures, agentTextures, bonusTextures;
        private float zimmerAngle;

        #endregion

        #region Class Constructor

        /// <summary>
        /// Level constructor, for initialising a new instance of the Level object
        /// </summary>
        /// <param name="gS">GameState object with global variables</param>
        /// <param name="s">Services from the main game</param>

        public Level(GameState gS, IServiceProvider s) {
            this.gameState = gS;
            this.content = new ContentManager(s);
            this.content.RootDirectory = "Content\\";
            this.loaded = false;
            this.inLevel = false;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates the level
        /// </summary>
        
        public void Update() {
            bool updateObstruction = false;

            //Load the level
            if(this.loaded == false) {

                //Reset buttons
                this.gameState.ResetButtons();

                //Enable the start button
                this.gameState.buttons[1].enabled = true;

                //Load the data file if one exists, otherwise, return to the menu
                if(File.Exists("Content/Data/Levels/" + this.gameState.level + ".xml")) {
                    this.LoadLevel(this.gameState.level);
                } else {
                    this.GameOver();
                }

            //Is it time to progress to the next level?
            } else if((this.gameState.clock > (60 * this.gameState.levelTime)) && this.gameState.level != 3) {
                this.gameState.level++;
                this.LevelOver();

            //Update Loaded level
            } else {

                //Splash Screen
                if(this.inLevel == false) {

                    //Check to see if we are ready to start the game
                    if(this.gameState.buttons[1].clicked == true) {
                        this.inLevel = true;
                        this.gameState.buttons[1].enabled = false;
                    }

                //Actual Level
                } else {
                    
                    //Enable the pause button
                    this.gameState.buttons[9].enabled = true;

                    //Make sure the game isn't paused
                    if(this.gameState.paused == false) {

                        //Update zimmer orientation
                        this.zimmerAngle = MathHelper.Clamp(TurnToFace(this.zimmer.currentPosition, this.gameState.mouse.currentPosition, this.zimmerAngle, (float)0.02), (float)-2, (float)-1.2);

                        //Check if a scenery object have been hit
                        for(int i = 0; i < this.countScenery; i++) {
                            if(updateObstruction == false) {
                                this.gameState.obstruction = 1;
                                if(this.scenery[i].obstructive == true && this.scenery[i].rectangle.Intersects(this.gameState.mouse.rectangle) && this.gameState.mouse.mouseUp == true) {
                                    this.gameState.obstruction = this.scenery[i].layer;
                                    updateObstruction = true;
                                }
                            }
                        }

                        //Update all active target objects, 
                        for(int i = 0; i < this.countZombies; i++) {
                            this.zombies[i].Update();
                        }
                        for(int i = 0; i < this.countAgents; i++) {
                            this.agents[i].Update();
                        }
                        for(int i = 0; i < this.countBonuses; i++) {
                            this.bonuses[i].Update();
                        }

                        //Tick the clock along
                        this.gameState.clock++;

                        //Set the alpha of the indicator according to infection levels (dividing by x.0 makes the number a float which is necessary to stop horrible integer division which breaks this)
                        this.indicator.alpha = (int)((this.gameState.infection / 10.0) * 255);
                        if(this.gameState.infection > 10) {
                            this.GameOver();
                        }
                    }

                    //Handle interactions with the pause button, setting the pause state accordingly
                    if(gameState.buttons[9].clicked == true) {
                        if(this.gameState.paused == false) {
                            this.gameState.paused = true;
                        } else {
                            this.gameState.paused = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draws the level
        /// </summary>
        /// <param name="gd">GraphicsDevice from main game</param>

        public void Draw(GraphicsDevice gd) {
            SpriteBatch spriteBatch = new SpriteBatch(gd);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.SaveState);
            if(this.loaded == true) {

                //Splash Screen
                if(this.inLevel == false) {
                    spriteBatch.Draw(this.splashScreen.texture, this.splashScreen.currentPosition, Color.White);
                
                //Level Draw
                } else {

                    //Core level components
                    spriteBatch.Draw(this.background.texture, this.background.rectangle, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                    spriteBatch.Draw(this.controlBar.texture, this.controlBar.rectangle, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, (float)0.001);
                    spriteBatch.Draw(this.indicator.texture, this.indicator.rectangle, null, new Color(255, 255, 255, (byte)this.indicator.alpha), 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    spriteBatch.Draw(this.zimmer.texture, this.zimmer.rectangle, null, Color.White, this.zimmerAngle, new Vector2(0, this.zimmer.height/2), SpriteEffects.None, 0);
                    
                    //Show the paused indicator if the game is paused
                    if(this.gameState.paused == true) {
                        spriteBatch.Draw(this.paused.texture, this.paused.rectangle, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    }

                    //Scenery
                    for(int i = 0; i < this.countScenery; i++) {
                        spriteBatch.Draw(this.scenery[i].texture, this.scenery[i].rectangle, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, this.scenery[i].layer);
                    }

                    //Active Targets
                    for(int i = 0; i < this.countZombies; i++) {
                        if(this.zombies[i].enabled == true) {
                            this.zombies[i].Draw(spriteBatch);
                        }
                    }
                    for(int i = 0; i < this.countAgents; i++) {
                        if(this.agents[i].enabled == true) {
                            this.agents[i].Draw(spriteBatch);
                        }
                    }
                    for(int i = 0; i < this.countBonuses; i++) {
                        if(this.bonuses[i].enabled == true) {
                            this.bonuses[i].Draw(spriteBatch);
                        }
                    }
                }
            }
            spriteBatch.End();
        }

        #endregion

        #region LoadLevel Method

        /// <summary>
        /// Loads data for the specified level
        /// </summary>
        /// <param name="l">Level to load</param>

        private void LoadLevel(int l) {

            //Method specific variables
            XmlDocument xmlData;
            XmlNodeList sceneryNodes, targetNodes;
            XmlNode sceneryNode, targetNode;
            SoundEffect[] sounds = {
                                       this.content.Load<SoundEffect>("Sounds\\Death1"),
                                       this.content.Load<SoundEffect>("Sounds\\Death2"),
                                       this.content.Load<SoundEffect>("Sounds\\Infection"),
                                       this.content.Load<SoundEffect>("Sounds\\Bonus")
            };

            //Load in key level objects
            this.background = new GameObject(800, 600, content.Load<Texture2D>("Textures\\Levels\\" + this.gameState.level + "\\Background"));
            this.controlBar = new GameObject(122, 42, 678, 0, content.Load<Texture2D>("Textures\\Global\\ControlBar"));
            this.indicator = new GameObject(40, 40, 680, 0, content.Load<Texture2D>("Textures\\Global\\Indicator"));
            this.paused = new GameObject(73, 16, 597, 12, content.Load<Texture2D>("Textures\\Global\\Paused"));
            this.splashScreen = new GameObject(800, 600, content.Load<Texture2D>("Textures\\Levels\\" + this.gameState.level + "\\SplashScreen"));
            this.zimmer = new GameObject(68, 99, 370, 620, content.Load<Texture2D>("Textures\\Global\\Zimmer"));

            this.zimmerAngle = -90;

            //Make sure the infection indicator isn't on for a split second at the start of the level
            this.indicator.alpha = 0;

            //Load the XML button data file
            xmlData = new XmlDocument();
            xmlData.Load("Content\\Data\\Levels\\" + this.gameState.level + ".xml");

            //Put each scenery and target object and it's attributes into a list
            sceneryNodes = xmlData.DocumentElement.SelectNodes("Scenery");
            targetNodes = xmlData.DocumentElement.SelectNodes("Target");
            this.countScenery = sceneryNodes.Count;
            this.countTargets = targetNodes.Count;

            //Create arrays for the scenery and buttons
            this.scenery = new Scenery[this.countScenery];
            this.targets = new Target[this.countTargets];

            //Loop through all scenery in the list
            for(int i = 0; i < this.countScenery; i++) {
                sceneryNode = sceneryNodes[i];

                //Create a scenery object in the array
                this.scenery[i] = new Scenery(int.Parse(sceneryNode.SelectSingleNode("width").InnerText), int.Parse(sceneryNode.SelectSingleNode("height").InnerText), int.Parse(sceneryNode.SelectSingleNode("pX").InnerText), int.Parse(sceneryNode.SelectSingleNode("pY").InnerText), content.Load<Texture2D>("Textures\\Levels\\" + this.gameState.level + "\\" + sceneryNode.SelectSingleNode("name").InnerText), bool.Parse(sceneryNode.SelectSingleNode("obstructive").InnerText), bool.Parse(sceneryNode.SelectSingleNode("destructible").InnerText), float.Parse(sceneryNode.SelectSingleNode("layer").InnerText));
            }

            //Load Target Textures
            zombieTextures = new Texture2D[21];
            agentTextures = new Texture2D[10];
            bonusTextures = new Texture2D[12];
            for(int i = 0; i < 21; i++) {
                zombieTextures[i] = content.Load<Texture2D>("Textures\\Targets\\Zombie\\" + i);
            }
            for(int i = 0; i < 10; i++) {
                agentTextures[i] = content.Load<Texture2D>("Textures\\Targets\\Agent\\" + i);
            }
            for(int i = 0; i < 12; i++) {
                bonusTextures[i] = content.Load<Texture2D>("Textures\\Targets\\Bonus\\" + i);
            }

            //Loop through all targets in the list
            for(int i = 0; i < this.countTargets; i++) {
                targetNode = targetNodes[i];

                switch(targetNode.SelectSingleNode("name").InnerText) {
                    case "zombie":
                        this.countZombies++;
                        this.textures = this.zombieTextures;
                        break;
                    case "agent":
                        this.countAgents++;
                        this.textures = this.agentTextures;
                        break;
                    case "bonus":
                        this.countBonuses++;
                        this.textures = this.bonusTextures;
                        break;
                }

                //Create a target object in the array
                this.targets[i] = new Target(int.Parse(targetNode.SelectSingleNode("width").InnerText), int.Parse(targetNode.SelectSingleNode("height").InnerText), int.Parse(targetNode.SelectSingleNode("pX").InnerText), int.Parse(targetNode.SelectSingleNode("pY").InnerText), this.gameState, this.textures, sounds, targetNode.SelectSingleNode("name").InnerText, float.Parse(targetNode.SelectSingleNode("layer").InnerText));
            }

            //Create final target type objects
            this.zombies = new Zombie[this.countZombies];
            this.agents = new Agent[this.countAgents];
            this.bonuses = new Bonus[this.countBonuses];
            for(int i = 0, j = 0; j < this.countZombies; i++) {
                if(this.targets[i].type == "zombie") {
                    this.zombies[j] = new Zombie(this.targets[i]);
                    j++;
                }
            }
            for(int i = 0, j = 0; j < this.countAgents; i++) {
                if(this.targets[i].type == "agent") {
                    this.agents[j] = new Agent(this.targets[i]);
                    j++;
                }
            }
            for(int i = 0, j = 0; j < this.countBonuses; i++) {
                if(this.targets[i].type == "bonus") {
                    this.bonuses[j] = new Bonus(this.targets[i]);
                    j++;
                }
            }
                
            //We can unset targets now, it's work here is done
            this.targets = null;

            //And just like that, loading is done
            this.loaded = true;
        }

        #endregion

        #region LevelOver Method

        /// <summary>
        /// Resets the level state for a new level
        /// </summary>

        private void LevelOver() {
            this.loaded = false;
            this.inLevel = false;
            this.content.Unload();
            this.background = null;
            this.controlBar = null;
            this.countScenery = 0;
            this.countTargets = 0;
            this.countZombies = 0;
            this.countAgents = 0;
            this.countBonuses = 0;
            this.zombies = null;
            this.agents = null;
            this.bonuses = null;
            this.targets = null;
            this.scenery = null;
            this.splashScreen = null;
            this.indicator = null;
            this.paused = null;
            this.gameState.clock = 0;
            this.gameState.infection = 0;
        }

        #endregion

        #region GameOver Method

        /// <summary>
        /// Resets the game
        /// </summary>

        private void GameOver() {
            LevelOver();
            gameState.Reset();
        }

        #endregion

        #region Foreign Methods

        /// <summary>
        /// Calculates the angle that an object should face, given its position, its
        /// target's position, its current angle, and its maximum turning speed.
        /// </summary>

        private float TurnToFace(Vector2 position, Vector2 faceThis, float currentAngle, float turnSpeed) {
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;
            float desiredAngle = (float)Math.Atan2(y, x);
            float difference = WrapAngle(desiredAngle - currentAngle);
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);
            return WrapAngle(currentAngle + difference);
        }

        /// <summary>
        /// Returns the angle expressed in radians between -Pi and Pi.
        /// <param name="radians">the angle to wrap, in radians.</param>
        /// <returns>the input value expressed in radians from -Pi to Pi.</returns>
        /// </summary>
        
        private float WrapAngle(float radians) {
            while(radians < -MathHelper.Pi) {
                radians += MathHelper.TwoPi;
            }
            while(radians > MathHelper.Pi) {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }

        #endregion

    }
}
