// we know this is terrible code, and it will be changed for the final submssion, but it is quite useful for concurrent development

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace LabRatEscape
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ItemManager itemManager;
        Player player;
        EnemyManager enemyManager;
        Level level;
        Engine engine;
        int time;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Content.RootDirectory = "Content";
        }

        public int getTime()
        {
            return time;
        }
        public Player getPlayer()
        {
            return player;
        }
        public ItemManager getItemManager()
        {
            return itemManager;
        }
        public EnemyManager getEnemyManager()
        {
            return enemyManager;
        }
        public Level getLevel()
        {
            return level;
        }
        public Engine getEngine()
        {
            return engine;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Console.WriteLine("Choose what to test");
            Console.WriteLine("-------------------");
            Console.WriteLine("0. None");
            Console.WriteLine("1. Item branch");
            Console.WriteLine("2. Player branch");
            Console.WriteLine("3. Level branch");
            Console.WriteLine("4. Engine branch");
            Console.WriteLine("5. Enemy branch");
            Console.WriteLine("-------------------");
            //String choice = Console.ReadLine();
            String choice = "4";

            Vector2 screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            if (choice.Equals("1"))
            {
                Dictionary<String, Item> possibleItems = new Dictionary<String, Item>();
                itemManager = new ItemManagerTest(this, possibleItems);
            }
            else
            {
                Dictionary<String, Item> possibleItems = new Dictionary<String, Item>();
                possibleItems.Add("healthPotion", new Item("healthPotion", null, false, Content.Load<Texture2D>("healthpotion"), 32, 32, new Vector2(0, 0), screenSize));
                possibleItems.Add("drugPotion", new Item("drugPotion", null, false, Content.Load<Texture2D>("drugpotion"), 32, 32, new Vector2(0,0), screenSize));
                // add stuff here
                itemManager = new ItemManager(this, possibleItems);
            }

            if (choice.Equals("2"))
            {
                Console.WriteLine("Testing player");
                player = new PlayerTest(Content.Load<Texture2D>("player"), 32, 32, new Vector2(32,32), screenSize, this);
            }
            else
            {

                player = new Player(Content.Load<Texture2D>("player"), 32, 32, new Vector2(48, 48), screenSize, this);
            }

            if (choice.Equals("3"))
            {
                Dictionary<String, Block> possibleBlocks = new Dictionary<String, Block>();
                level = new LevelTest(this, possibleBlocks);
                
            }
            else
            {
                Dictionary<String, Block> possibleBlocks = new Dictionary<String, Block>();
                level = new Level(this, possibleBlocks);
            }

            if (choice.Equals("4"))
            {
                engine = new EngineTest(this);
            }
            else
            {
                engine = new Engine(this);
            }

            if (choice.Equals("5"))
            {
                //enemyManager = new TestEnemyManager(this);
            }
            else
            {
                //enemyManager = new EnemyManager(this);
            }
            

            base.Initialize();
        }

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
            time++;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            InputManager.ActKeyboard(Keyboard.GetState());

            // TODO: Add your update logic here
            engine.Update();          

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            engine.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        
    }
}
