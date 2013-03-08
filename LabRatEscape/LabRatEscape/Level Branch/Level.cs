#region Imports
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
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
#endregion

namespace LabRatEscape
{
    /// <summary>
    /// Singleton class which manages the level for the game
    /// </summary>
    public class Level
    {
        #region Variables
        private static Level uniqueInstance;
        public static GameValue currentLevel = new GameValue(0,0,100);
        public static Dictionary<String, Block> possibleBlocks = new Dictionary<string,Block>();
        public static Block defaultBlock;
        public static ArrayList levelPaths = new ArrayList();
        static Dictionary<String, Sprite> headerDictionary = new Dictionary<String, Sprite>();
        static String levelFile;
        static ArrayList fileLines;
        static List<String> header;
        public static Dictionary<String, Item> possibleItems;
        public Block[,] levelMap;
        internal bool hasEnterance;
        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor
        /// </summary>
        private Level()
        {
        }
        #endregion

        #region Getters/Setters

        /// <summary>
        /// Gets the unique instance of the Level class
        /// </summary>
        /// <returns>Unique instance of the Level class</returns>
        public static Level GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new Level();
            }
            return uniqueInstance;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Loads in a new level
        /// </summary>
        /// <param name="path">Path of the levels folder</param>
        /// <param name="level">Number of the level to load</param>
        public void LoadLevel(int level)
        {
            levelFile = levelPaths[level] as string;
            fileLines = new ArrayList();
            header = new List<String>();
            ItemManager.levelItems.Clear();
            ParseFile();
        }

        /// <summary>
        /// Gets a section of the level map
        /// </summary>
        /// <param name="startRow">First row to get</param>
        /// <param name="endRow">Last row to get</param>
        /// <param name="startCol">First column to get</param>
        /// <param name="endCol">Last column to get</param>
        /// <returns>List of blocks that are in the requested section</returns>
        public List<Sprite> MapSection(int startRow, int endRow, int startCol, int endCol)
        {
            List<Sprite> spriteList = new List<Sprite>();
            if (startRow < 0) { startRow = 0; }
            if (endRow >= levelMap.GetLength(0)) { endRow = levelMap.GetLength(0) - 1; }
            if (startCol < 0) { startCol = 0; }
            if (endCol >= levelMap.GetLength(1)) { endCol = levelMap.GetLength(1) - 1; }
            for (int r = startRow; r <= endRow; r++)
            {
                for (int c = startCol; c <= endCol; c++)
                {
                    spriteList.Add(levelMap[r, c]);
                }
            }
            return spriteList;
        }

        /// <summary>
        /// Reads in from the current level file the header into the header list and level into fileLines
        /// </summary>
        private void ParseFile()
        {
            // open up a new reader looking at the level file
            StreamReader reader = new StreamReader(levelFile);
            // read lines into the header until the character # is reached
            while (reader.Peek() >= 0)
            {
                String text = reader.ReadLine();
                if (text.Equals("#"))
                {
                    break;
                }
                header.Add(text);
            }
            // read the remaining lines into the level
            while (reader.Peek() >= 0)
            {
                String text = reader.ReadLine();
                fileLines.Add(text);
            }

            BuildHeaderDictionary(); // will be renamed, actually builds the dictionary from the header
            createMap();
        }

        /// <summary>
        /// Builds the heaer dictionary from the header list
        /// </summary>
        private static void BuildHeaderDictionary()
        {
            // steps through lines read into the header
            for (int i = 0; i < header.Count; i++)
            {
                String line = header[i];
                String objectChar = "";
                String typeOfObject = "";
                String objectID = "";
                // splits the line up by word
                String[] split = line.Split(' ');
                objectChar = split[0];
                typeOfObject = split[1];
                objectID = split[2];
                if (ItemManager.possibleItems.ContainsKey(objectID))
                {
                    Item item = ItemManager.possibleItems[objectID];
                    AddToHeaderDictionary(objectChar, item);
                }
                if (possibleBlocks.ContainsKey(objectID))
                {
                    Block block = possibleBlocks[objectID];
                    AddToHeaderDictionary(objectChar, block);
                }

            }
        }

        /// <summary>
        /// Adds a sprite to the haeder dictionary
        /// </summary>
        /// <param name="value">Id to associate the sprite with</param>
        /// <param name="sprite">Sprite to add to the dictionary</param>
        private static void AddToHeaderDictionary(String value, Sprite sprite)
        {
            if (!headerDictionary.ContainsKey(value))
            {
                headerDictionary.Add(value, sprite);
            }
        }

        /// <summary>
        /// Populates the level map from fileLines using the header dictionary
        /// </summary>
        private void createMap()
        {
            // this needs to be fixed to work for multiple players
            PlayerManager.players[0].drugIntensity.Reset();
            hasEnterance = false;

            // creates a new x by y matrix where x is the number of lines read in and y is the length of the first line
            levelMap = new Block[fileLines.Count, (fileLines[0] as String).Length];
            
            // steps through each line constructing the level
            for (int row = 0; row < fileLines.Count; row++)
            {
                String line = fileLines[row] as String;
                IEnumerator lineEnum = line.GetEnumerator();
                int col = 0;
                // steps through a single line adding gameObjects to the appropriate location
                while (lineEnum.MoveNext())
                {
                    // gets the next character
                    String id = "" + lineEnum.Current;
                    if (headerDictionary.ContainsKey(id)) // if the character is defined by the header
                    {
                        // gets the associated gameObject
                        Sprite sprite = headerDictionary[id];
                        sprite.AddToLevel(row, col);
                    }
                    else
                    {
                        int x = Block.WIDTH * col;
                        int y = Block.HEIGHT * row;
                        levelMap[row, col] = defaultBlock.copyBlockAt(new Vector2(x, y));
                    }
                    col++;
                }
            }
            if (!hasEnterance)
            {
                Console.WriteLine("Level.createMap() : No enterance");
            }

        }

        /// <summary>
        /// Advances to the next level
        /// </summary>
        public static void NextLevel()
        {
            currentLevel.Add(1);
            GetInstance().LoadLevel((int)currentLevel.getValue());
        }

        /// <summary>
        /// Restarts the current level
        /// </summary>
        public static void RestartLevel()
        {
            GetInstance().LoadLevel((int)currentLevel.getValue());
        }

        /// <summary>
        /// Updates the level
        /// </summary>
        /// <param name="elapsedTime">Time in seconds since the last update</param>
        public void Update(float elapsedTime)
        {
            for (int row = 0; row < levelMap.GetLength(0); row++ )
            {
                for (int col = 0; col < levelMap.GetLength(1); col++)
                {
                    levelMap[row, col].Update(elapsedTime);
                }
            }
        }

        /// <summary>
        /// Draws the level with double vision
        /// </summary>
        /// <param name="batch">Sprite batch to draw the level on</param>
        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < levelMap.GetLength(0); i++)
            {
                for (int j = 0; j < levelMap.GetLength(1); j++)
                {
                    if (levelMap[i, j] != null)
                    {
                        levelMap[i, j].DrawDoubleVision(batch);
                    }
                }
            }
            for (int i = 0; i < levelMap.GetLength(0); i++)
            {
                for (int j = 0; j < levelMap.GetLength(1); j++)
                {
                    if (levelMap[i, j] != null)
                    {
                        levelMap[i, j].Draw(batch);
                    }
                }
            }
        }

        #endregion
    }
}
