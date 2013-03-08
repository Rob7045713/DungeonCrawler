#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Inventory holds the players collectable items for later use
    /// </summary>
    public class Inventory
    {
        #region Variables
        private Stack<Item>[] inventory;
        private int selected;
        private bool addComplete;
        private Texture2D selectedImage;
        private Texture2D unselectedImage;
        internal int inventoryNumber;
        #endregion

        #region Constants
        public const int NUM_SLOTS = 8;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new inventory
        /// default constructor
        /// </summary>
        public Inventory(Texture2D selectedImage, Texture2D unselectedImage, int inventoryNumber)
        {
            this.selectedImage = selectedImage;
            this.unselectedImage = unselectedImage;
            this.inventoryNumber = inventoryNumber;
            inventory = new Stack<Item>[NUM_SLOTS];
            for (int i = 0; i < NUM_SLOTS; i++)
            {
                inventory[i] = new Stack<Item>();
            }
            selected = 0;
            addComplete = false;
        }
        #endregion

        #region Getters/Setters
        /// <summary>
        /// gets the selected index holder
        /// </summary>
        public int getSelected()
        {
            return selected;
        }

        /// <summary>
        /// gets the inventory list
        /// </summary>
        public Stack<Item>[] getInventory()
        {
            return inventory;
        }
        #endregion

        #region Methods

        /// <summary>
        /// adds an item to the given invetory
        /// </summary>
        /// <param name="item">item to add to inventory</param>
        public virtual void Add(Item item)
        {
            String itemID = item.getID();
            //go through every stack in the list
            for (int i = 0; i < inventory.Count(); i++)
            {
                Stack<Item> stack = inventory[i];
                //if the stack has items in it then
                if (stack.Count() > 0)
                {
                    //grab the item
                    Item it = stack.Peek();
                    //check to see if the item we are checking has the same ID as the item we are trying to add
                    if (it.getID().Equals(itemID))
                    {
                        //if they match ids then add it to the current item stack
                        stack.Push(item);
                        //add step is then completed
                        addComplete = true;
                    }
                }
            }
            //if addComplete is false then we did not find a stack containing the item we are trying to add and must create a new stack and add it to the list
            //with the item trying to be added to inventory
            if (!addComplete)
            {
                Stack<Item> myStack = new Stack<Item>();
                myStack.Push(item);
                FindFirstEmptySlot(myStack);
            }
            //reset addComplete
            addComplete = false;
        }

        /// <summary>
        /// places a stack of items into the frist slot int the inventory with nothing in it 
        /// </summary>
        /// <param name="stack">item to add to inventory</param>
        public void FindFirstEmptySlot(Stack<Item> stack)
        {
            for (int i = 0; i < NUM_SLOTS; i++)
            {
                if (inventory[i].Count == 0)
                {
                    inventory[i] = stack;
                    break;
                }
            }
        }

        /// <summary>
        /// moves the selected index holder 1 place ahead
        /// </summary>
        public void Next()
        {
            selected++;
            selected %= NUM_SLOTS;
        }

        /// <summary>
        /// moves the seleced index holder 1 place behind
        /// </summary>
        public void Previous()
        {
            selected--;
            selected += NUM_SLOTS;
            selected %= NUM_SLOTS;
        }

        /// <summary>
        /// sets the selected index holder to a given value
        /// </summary>
        /// <param name="selected">new index holder value</param>
        public void Select(int index)
        {
            if (index >= 0 && index < NUM_SLOTS)
            {
                selected = index;
            }
        }

        /// <summary>
        /// Use an item if it is contained in the selected stack
        /// </summary>
        public void Use()
        {
            //grab the stack that is currently selected
            if (inventory[selected].Count() > 0 && inventory[selected].Peek().isUsable)
            {
                //if there is an item to use then pop it off the stack and use it
                Item item = inventory[selected].Pop();
                item.OnUse(PlayerManager.players[inventoryNumber]);
            }
        }

        public bool TryUse(string id)
        {
            foreach (Stack<Item> stack in inventory)
            {
                if (stack.Count > 0 && stack.Peek().id.Equals(id))
                {
                    stack.Pop();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// draws the inventory to the screen
        /// </summary>
        /// <param name="batch">spriteBatch that should be drawn to</param>
        /// <param name="font">SpriteFont that should be used when message is created</param>
        /// <param name="screenSize">current size of the screen</param>
        public void Draw(SpriteBatch batch, Vector2 location, SpriteFont font)
        {
            //starts the toolbar off in the lower left hand corner and it will grow across the screen in positive X direction
            for (int i = 0; i < NUM_SLOTS; i++)
            {
                Texture2D slotFrame;
                if (i == selected)
                {
                    slotFrame = selectedImage;
                }
                else
                {
                    slotFrame = unselectedImage;
                }

                batch.Draw(slotFrame, new Vector2(location.X + i * Block.WIDTH, location.Y), Color.White);
                
                if (inventory[i].Count() > 0)
                {
                    //grab the first item from stack
                    Item item = inventory[i].Peek();
                    //get its sprite used
                    Texture2D sprite = item.getSpriteSheet();

                    //over lap the sprite image with the number of those items in inventory
                    // this needs to be fixed to remove the literals
                    Message myMessage = new Message("" + inventory[i].Count(), new Vector2(location.X + i * Block.WIDTH + 5, location.Y + 5), font);
                    //draw it to the screen
                    // this needs to be fixed to remove the literals
                    batch.Draw(sprite, new Vector2(location.X + i * Block.WIDTH + 4, location.Y + 4), Color.White);
                    myMessage.Draw(batch);
                }
            }
        }

        #endregion

    }
}
