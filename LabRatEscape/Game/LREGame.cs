// Copyright 12.13.2010
// Rob Argue
// David Edge
// Leanna Helton
// Joe Kiernen

#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Reflection;
#endregion

namespace LabRatEscape
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LREGame : Microsoft.Xna.Framework.Game
    {
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;    
        Random random;
        public static float time;
        char[] delimeter = { ' ', '\t' };
        string rootPath = "";
        Dictionary<string, Type> classes = new Dictionary<string, Type>();
        #endregion

        #region Constructors
        public LREGame()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;
            Content.RootDirectory = "";
            random = new Random();
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                classes.Add(type.Name, type);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Vector2 screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            
            // engine intialization

            GameAction skip = new GameAction(
              null,
              Level.GetInstance().GetType().GetMethod("NextLevel"),
              new object[0]);
            InputManager.AddToKeyboardMap(Keys.I, skip);

            Dictionary<string, string> gameFiles = new Dictionary<string, string>();
            gameFiles.Add("playersFile", "");
            gameFiles.Add("itemsFile", "");
            gameFiles.Add("blocksFile", "");
            gameFiles.Add("enemiesFile", "");
            gameFiles.Add("levelsFile", "");

            StreamReader reader = new StreamReader("Content/LRE_game.txt");
            rootPath = reader.ReadLine().Split(delimeter, StringSplitOptions.RemoveEmptyEntries)[1] + "/";
            Content.RootDirectory = rootPath;
            while (reader.Peek() >= 0)
            {
                string[] line = reader.ReadLine().Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                if(gameFiles.ContainsKey(line[0]))
                {
                    gameFiles[line[0]] = rootPath + line[1];
                }
            }

            // sound initialization
            //SoundManager.effects.Add("playerHit", Content.Load<SoundEffect>("RecievedDamage"));
            //SoundManager.effects.Add("itemCollected", Content.Load<SoundEffect>("ObtainedItem"));
            //SoundManager.effects.Add("playerDied", Content.Load<SoundEffect>("Death"));
            //SoundManager.effects.Add("itemUsed", Content.Load<SoundEffect>("UsedItem"));
            //SoundManager.effects.Add("doorUnlocked", Content.Load<SoundEffect>("UnlockADoor"));
            //SoundManager.songs.Add("mainMenu", Content.Load<Song>("StartScreenAudioLoop"));
            //SoundManager.songs.Add("winLevelMenu", Content.Load<Song>("LevelCompleteSplash"));
            //SoundManager.songs.Add("winGameMenu", Content.Load<Song>("YouWin(thegame)"));
            //SoundManager.songs.Add("dieMenu", Content.Load<Song>("YouHaveDiedSplash"));
            //SoundManager.songs.Add("game", Content.Load<Song>("Kalimba(InGameAudio)"));

            // player manager intialization
            PlayerManager.AddPlayer(Content.Load<Texture2D>("Players/whiterat"), 40, 40, Content.Load<Texture2D>("HUD/selected"),
                Content.Load<Texture2D>("HUD/unselected"), Content.Load<Texture2D>("HUD/inventory"), screenSize);

            // item manager intialization
            LoadItems(gameFiles["itemsFile"]);

            // Enemy initialization
            List<Effect> hazmatEffects = new List<Effect>();
            hazmatEffects.Add(new Effect("drugIntensity", new Modifier("+", ".33")));
            EnemyManager.possibleEnemies.Add("enemy", new Enemy(Content.Load<Texture2D>("Enemies/hazmat"), 56, 56, new Vector2(0, 0),
                new DoNothingAI(), new TeleportToHomeAI(), new BeelineAI(250), hazmatEffects, 256, 1024));
            
            List<Effect> brainEffects = new List<Effect>();
            brainEffects.Add(new Effect("drugIntensity", new Modifier("=", ".99")));
            EnemyManager.possibleEnemies.Add("brain", new Enemy(Content.Load<Texture2D>("Enemies/brains"), 60, 60, new Vector2(0, 0),
                new TeleportToHomeAI(), new TeleportToHomeAI(), new BeelineAI(300), brainEffects, 256, 1024));

            

            // HUD intialization
            HUD.GetInstance().setInventory(ItemManager.inventories[0]);
            HUD.GetInstance().setScreenSize(screenSize);
            SpriteFont font = Content.Load<SpriteFont>("Arial");
            SpriteFont small = Content.Load<SpriteFont>("Small");
            HUD.GetInstance().setScoreData(new Vector2(0, 0), font);
            HUD.GetInstance().setTimerData(new Vector2(0, 20), font);
            HUD.GetInstance().setFont(font);
            HUD.GetInstance().setSmallFont(small);
            HUD.GetInstance().overlay = new Sprite(Content.Load<Texture2D>("HUD/overlay"), new Vector2(screenSize.X/2, screenSize.Y/2));

            // level intialization
            LoadBlocks(gameFiles["blocksFile"]);

            LoadLevels(gameFiles["levelsFile"]);
            // load first level
            Level.GetInstance().LoadLevel(0);
            GameAction startGame = new GameAction(
             this,
             Engine.GetInstance().GetType().GetMethod("Play"),
             new object[0]);

            GameAction exitGame = new GameAction(
                this,
                this.GetType().GetMethod("Exit"),
                new object[0]);

            GameAction restartGame = new GameAction(
                Engine.GetInstance(),
                Engine.GetInstance().GetType().GetMethod("Reset"),
                new object[0]);

            List<MenuItem> startMenuItems = new List<MenuItem>();
            startMenuItems.Add(new MenuItem("Press Enter to Play!", startGame));
            startMenuItems.Add(new MenuItem("Press Enter to Exit!", exitGame));

            Menu mainMenu = new Menu(Content.Load<Texture2D>("Menu/Splash"), screenSize, font, "mainMenu", "Press Space to Begin", startMenuItems, Color.Red, Color.White);
            mainMenu.setUp();
            Engine.GetInstance().mainMenu = mainMenu;

            List<MenuItem> pauseMenuItems = new List<MenuItem>();
            pauseMenuItems.Add(new MenuItem("Resume", startGame));
            pauseMenuItems.Add(new MenuItem("Main Menu", restartGame));
            pauseMenuItems.Add(new MenuItem("Quit", exitGame));

            Menu pauseMenu = new Menu(Content.Load<Texture2D>("Menu/Splash"), screenSize, font, "pauseMenu", "Press Space to Begin", pauseMenuItems, Color.Red, Color.White);
            pauseMenu.setUp();
            Engine.GetInstance().pauseMenu = pauseMenu;

            List<MenuItem> dieMenuItems = new List<MenuItem>();
            dieMenuItems.Add(new MenuItem("Retry", startGame));
            dieMenuItems.Add(new MenuItem("Main Menu", restartGame));
            dieMenuItems.Add(new MenuItem("Quit", exitGame));

            Menu dieMenu = new Menu(Content.Load<Texture2D>("Menu/LabRat-Dead"), screenSize, font, "dieMenu", "Press Space to Begin", dieMenuItems, Color.Red, Color.White);
            dieMenu.setUp();
            Engine.GetInstance().dieMenu = dieMenu;

            List<MenuItem> winLevelMenuItems = new List<MenuItem>();
            winLevelMenuItems.Add(new MenuItem("Continue", startGame));
            winLevelMenuItems.Add(new MenuItem("Main Menu", restartGame));
            winLevelMenuItems.Add(new MenuItem("Quit", exitGame));

            Menu winLevel = new Menu(Content.Load<Texture2D>("Menu/LabRat-Level"), screenSize, font, "winLevelMenu", "Press Space to Begin", winLevelMenuItems, Color.Red, Color.White);
            winLevel.setUp();
            Engine.GetInstance().winLevelMenu = winLevel;

            List<MenuItem> winGameMenuItems = new List<MenuItem>();
            winGameMenuItems.Add(new MenuItem("Main Menu", restartGame));
            winGameMenuItems.Add(new MenuItem("Quit", exitGame));

            Menu winGameMenu = new Menu(Content.Load<Texture2D>("Menu/LabRat-Won"), screenSize, font, "winGameMenu", "Press Space to Begin", winGameMenuItems, Color.Red, Color.White);
            winGameMenu.setUp();
            Engine.GetInstance().winGameMenu = winGameMenu;

            List<MenuItem> cheeseMenuItems = new List<MenuItem>();
            cheeseMenuItems.Add(new MenuItem("Main Menu", restartGame));
            cheeseMenuItems.Add(new MenuItem("Quit", exitGame));

            Menu cheeseMenu = new Menu(Content.Load<Texture2D>("Menu/LabRat-WonCheese"), screenSize, font, "cheeseMenu", "Press Space to Begin", cheeseMenuItems, Color.Red, Color.White);
            cheeseMenu.setUp();
            Engine.GetInstance().cheeseMenu = cheeseMenu;
            
            Engine.Menu(Engine.GetInstance().mainMenu);

            base.Initialize();
        }


        #region Loaders

        /// <summary>
        /// Loads possible items into ItemManager from a text file  
        /// </summary>
        /// <param name="itemsFile">File to read in item data from</param>
        public void LoadItems(string itemsFile)
        {
            
            StreamReader reader = new StreamReader(itemsFile);
            while (reader.Peek() >= 0)
            {
                Dictionary<string, string> itemLoader = new Dictionary<string, string>();
                itemLoader.Add("id", "");
                itemLoader.Add("imageFile", "");
                itemLoader.Add("numFrames", "1");
                itemLoader.Add("numAnimations", "1");
                itemLoader.Add("isCollectable", "false");
                itemLoader.Add("isUsable", "true");
                itemLoader.Add("isKept", "true");
                List<Effect> effects = new List<Effect>();
                while (reader.Peek() != '#')
                {
                    String[] line = reader.ReadLine().Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                    if (itemLoader.ContainsKey(line[0]))
                    {
                        itemLoader[line[0]] = line[1];
                    }
                    if (line[0].Equals("effect"))
                    {
                        // check to see if its a class
                        if (classes.ContainsKey(line[1]))
                        {
                            effects.Add(new Effect(new GameAction(classes[line[1]], classes[line[1]].GetMethod(line[2]), new object[0])));
                        }
                        else
                        {
                            // assume (gameValueKey, operator, value)
                            if (line.Length == 4)
                            {
                                effects.Add(new Effect(line[1], new Modifier(line[2], line[3])));
                            }
                        }
                    }
                }
                reader.ReadLine();

                string id = itemLoader["id"];
                bool isCollectable = Boolean.Parse(itemLoader["isCollectable"]);
                bool isUsable = Boolean.Parse(itemLoader["isUsable"]);
                bool isKept= Boolean.Parse(itemLoader["isKept"]);
                Texture2D spriteSheet = Content.Load<Texture2D>("Items/" + itemLoader["imageFile"]);
                int frameWidth = spriteSheet.Width / Int32.Parse(itemLoader["numFrames"]);
                int frameHeight = spriteSheet.Height / Int32.Parse(itemLoader["numAnimations"]);

                ItemManager.possibleItems.Add(id, new Item(id, effects, isCollectable, isUsable, isKept, spriteSheet, frameWidth, frameHeight, new Vector2(0, 0)));

            }
                
        }

        /// <summary>
        /// Loads all possible blocks into Level from a text file
        /// </summary>
        /// <param name="blocksFile">File to read in block data from</param>
        public void LoadBlocks(string blocksFile)
        {
            StreamReader reader = new StreamReader(blocksFile);
            while (reader.Peek() >= 0)
            {
                Dictionary<string, string> blockLoader = new Dictionary<string, string>();
                blockLoader.Add("id", "");
                blockLoader.Add("imageFile", "");
                blockLoader.Add("activePassability", "FULLY_PASSABLE");
                blockLoader.Add("inactivePassability", "FULLY_PASSABLE");
                blockLoader.Add("onStep", "false");
                blockLoader.Add("activeTime", Int32.MaxValue.ToString());
                blockLoader.Add("inactiveTime", Int32.MaxValue.ToString());
                blockLoader.Add("frameTime", "0.25");
                blockLoader.Add("blockType", "Block");
                blockLoader.Add("isDefault", "false");
                blockLoader.Add("effectDelay", "0.1");

                List<Effect> effects = new List<Effect>();
                Condition passabilityCondition = new Condition();

                while (reader.Peek() != '#')
                {
                    String[] line = reader.ReadLine().Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                    if (blockLoader.ContainsKey(line[0]))
                    {
                        blockLoader[line[0]] = line[1];
                    }
                    if (line[0].Equals("effect"))
                    {
                        // check to see if its a class
                        if (classes.ContainsKey(line[1]))
                        {
                            effects.Add(new Effect(new GameAction(classes[line[1]], classes[line[1]].GetMethod(line[2]), new object[0])));
                        }
                        else
                        {
                            // assume (gameValueKey, operator, value)
                            if (line.Length == 4)
                            {
                                effects.Add(new Effect(line[1], new Modifier(line[2], line[3])));
                            }
                        }
                    }
                    if (line[0].Equals("passabilityCondition"))
                    {
                        if (classes.ContainsKey(line[1]))
                        {
                            passabilityCondition = new Condition(classes[line[1]].GetMethod(line[2]), new object[0]);
                        }
                    }
                }
                reader.ReadLine();

                string id = blockLoader["id"];
                Texture2D spriteSheet = Content.Load<Texture2D>("Blocks/" + blockLoader["imageFile"]);
                Vector2 position = new Vector2(0, 0);
                bool onStep = Boolean.Parse(blockLoader["onStep"]);

                int[,] activePassability = classes["Block"].GetField(blockLoader["activePassability"]).GetValue(new Block("null", spriteSheet, position)) as int[,];
                int[,] inactivePassability = classes["Block"].GetField(blockLoader["inactivePassability"]).GetValue(new Block("null", spriteSheet, position)) as int[,];
                
                float activeTime = (float)Double.Parse(blockLoader["activeTime"]);
                float inActiveTime = (float)Double.Parse(blockLoader["inactiveTime"]);
                float frameTime = (float)Double.Parse(blockLoader["frameTime"]);
                float effectDelay = (float)Double.Parse(blockLoader["effectDelay"]);

                object[] constructorParameters = { id, spriteSheet, position, effects, onStep, activePassability, inactivePassability, passabilityCondition, activeTime,
                                                     inActiveTime, frameTime, effectDelay };
                Type[] constructorParameterTypes = { id.GetType(), spriteSheet.GetType(), position.GetType(), effects.GetType(), onStep.GetType(), 
                                                       activePassability.GetType(), inactivePassability.GetType(), passabilityCondition.GetType(), activeTime.GetType(),
                                                       inActiveTime.GetType(), frameTime.GetType(), effectDelay.GetType()};
                ConstructorInfo constructor = classes[blockLoader["blockType"]].GetConstructor(constructorParameterTypes);
                Block block = constructor.Invoke(constructorParameters) as Block;
                Level.possibleBlocks.Add(id, block);
                if(Boolean.Parse(blockLoader["isDefault"]))
                {
                    Level.defaultBlock = block;
                }
            }
                
        }

        /// <summary>
        /// Loads the level paths into Level froma a text file
        /// </summary>
        /// <param name="levelsFile">File to read in level path data from</param>
        public void LoadLevels(string levelsFile)
        {
            StreamReader reader = new StreamReader(levelsFile);
            while (reader.Peek() >= 0)
            {
                string path = rootPath + "Levels/" + reader.ReadLine() + ".txt";
                Level.AddLevel(path);
            }
        }
        #endregion

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
            time += elapsedTime;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            InputManager.ActKeyboard(Keyboard.GetState());

            // TODO: Add your update logic here
            Engine.Update(elapsedTime / 1000.0f);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            Engine.Draw(spriteBatch);

            base.Draw(gameTime);
        }
        #endregion
    }
}
