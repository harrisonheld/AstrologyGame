using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AstrologyGame.MapData;
using AstrologyGame.Menus;
using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.Entities.Factories;
using AstrologyGame.Systems;

namespace AstrologyGame
{
    public class Game1 : Game
    {
        // things for drawing
        private static GraphicsDeviceManager graphicsDeviceManager;
        private static SpriteBatch _spriteBatch;
        private static SpriteFont font;

        private static OrderedPair screenSize = new OrderedPair(1024, 576);
        public static OrderedPair ScreenSize
        {
            get
            {
                return screenSize;
            }
            set
            {
                screenSize = value;

                graphicsDeviceManager.PreferredBackBufferWidth = screenSize.X;
                graphicsDeviceManager.PreferredBackBufferHeight = screenSize.Y;
            }
        }

        // ellapsed miliseconds
        private static int deltaTime = 0; 
        public static int DeltaTime { get { return deltaTime; } }

        // menu
        private static List<Menu> menus = new List<Menu>();
        private static LookMenu lookMenu;
        // an easy way to get the last menu in the list, which is the only one that should accept input
        public static Menu InputMenu
        {
            get
            {
                // return the last element in menus
                return menus.FindLast((Menu m) => m.TakesInput);
            }
        }
        // should we refresh the menus this frame?
        private static bool refreshAllMenusQueued = false;

        // the look cursor
        private static Texture2D cursor;
        private static int cursorX = 0;
        private static int cursorY = 0;
        public static OrderedPair CursorPosition
        {
            get
            {
                return new OrderedPair(cursorX, cursorY);
            }
        }

        // dev stuff
        Color DEBUG_COLOR = Color.White;
        Color DEBUG_COLOR_BAD = Color.Red;

        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Utility.Initialize(Content, GraphicsDevice, _spriteBatch);
            World.Seed = "nuts 2";
            World.GenerateCurrentZone();

            Entity knight = EntityFactory.EntityFromId("knight", 0, 0);
            knight.AddComponent(new PlayerControlled());
            knight.GetComponent<Equipment>().SlotDict.Add(Slot.Body, null);
            Zone.AddEntity(knight);
            Zone.Player = knight;

            // is the game fullscreen
            graphicsDeviceManager.PreferredBackBufferWidth = ScreenSize.X;
            graphicsDeviceManager.PreferredBackBufferHeight = ScreenSize.Y;
            graphicsDeviceManager.IsFullScreen = false;
            graphicsDeviceManager.ApplyChanges();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Utility.Initialize(Content, GraphicsDevice, _spriteBatch);
            cursor = Utility.GetTexture("cursor1");
            font = Content.Load<SpriteFont>("font1");

            Menu.Font = font;
            Utility.Font = font;

            Menu.Initialize();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            Content.Dispose();

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            deltaTime = gameTime.ElapsedGameTime.Milliseconds;
            Zone.Update();
        }

        /*
        private void Update_Interacting()
        {
            // player wants to leave Interact mode, change to free roam and return
            if (Input.Controls.Contains(Control.Back))
            {
                gameState = GameState.FreeRoam;
                return;
            }

            // if the player didnt use a directional key, or select his current space, do nothing and return
            if (movePair.X == 0 && movePair.Y == 0 && !Input.Controls.Contains(Control.Here) )
                return;

            OrderedPair playerPos = Zone.Player.GetComponent<Position>().Pos;
            int interactX = playerPos.X + movePair.X;
            int interactY = playerPos.Y + movePair.Y;

            List<Entity> entitiesHere = Zone.GetEntitiesAtPosition(new OrderedPair(interactX, interactY));
            // there are no entities here, return
            if (entitiesHere.Count == 0)
                return;

            Entity toInteractWith = entitiesHere.Last();

            if(interactType == InteractType.Specific)
            {
                InteractionMenu interactionMenu = new InteractionMenu(Zone.Player, toInteractWith);
                OpenMenu(interactionMenu);
            }
            else // interactType == InteractType.General
            {
                List<Interaction> interactions = toInteractWith.GetInteractions();
                // if entity has no interactions, return
                if (interactions.Count == 0)
                    return;

                // do the first interaction in the list
                interactions[0].Perform(Zone.Player);
            }
        }

        private void Update_Looking()
        {
            if(Input.Controls.Contains(Control.Back))
            {
                CloseMenu(lookMenu); // close the menu in case there is a LookMenu open
                gameState = GameState.FreeRoam;
                return;
            }

            cursorX += movePair.X;
            cursorY += movePair.Y;

            // if the cursor is outside the bounds of map, put it back inside.
            if (cursorX >= Zone.WIDTH)
                cursorX = Zone.WIDTH - 1;
            else if (cursorX < 0)
                cursorX = 0;
            if (cursorY >= Zone.HEIGHT)
                cursorY = Zone.HEIGHT - 1;
            else if (cursorY < 0)
                cursorY = 0;

            // this boolean will be true if the cursor moved this input frame
            bool moved = !(movePair.Equals(OrderedPair.Zero));

            if (moved)
            {
                CloseMenu(lookMenu); // close the Look Menu because the cursor moved off whatever it was looking at

                Entity objectToLookAt = null;

                foreach (Entity o in Zone.GetEntitiesAtPosition(new OrderedPair(cursorX, cursorY)))
                {
                    // TODO: pick an object to look at rather than just taking the first one

                    objectToLookAt = o;
                    break;
                }

                // look at the tile if there are no objects
                if(objectToLookAt is null)
                {
                    objectToLookAt = Zone.GetTileAtPosition(new OrderedPair(cursorX, cursorY));
                }

                Look(objectToLookAt);
            }
        }
        */

        /// <summary>
        /// Create and open a LookMenu for the given object.
        /// </summary>
        /// <param name="o"></param>
        private void Look(Entity o)
        {
            lookMenu = new LookMenu(o);
            AddMenu(lookMenu);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (refreshAllMenusQueued)
                RefreshAllMenus();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(default, default, SamplerState.PointClamp);

            // draw the game
            RenderingFunctions.RenderZone();

            // draw the look cursor
            /*
            if (gameState == GameState.LookMode)
            {
                Rectangle destinationRectangle = new Rectangle(
                    cursorX * Utility.SCALE, cursorY * Utility.SCALE, Utility.SCALE, Utility.SCALE);
                _spriteBatch.Draw(cursor, destinationRectangle, Color.White);
            }
            */

            // draw all the menus
            foreach (Menu m in menus)
                m.Draw();

            // draw debug info
            if (Input.Controls.Contains(Control.DevInfo))
            {
                OrderedPair playerPos = Zone.Player.GetComponent<Position>().Pos;
                _spriteBatch.DrawString(font, $"Zone: ({World.ZoneX}, {World.ZoneY})", new Vector2(0, 0), DEBUG_COLOR);
                _spriteBatch.DrawString(font, $"Player Pos: ({playerPos.X}, {playerPos.Y})", new Vector2(0, 20), DEBUG_COLOR);
                Color fpsColor = gameTime.IsRunningSlowly ? DEBUG_COLOR_BAD : DEBUG_COLOR; // change color depending on if game is running slow
                _spriteBatch.DrawString(font, $"FPS: {1000 / gameTime.ElapsedGameTime.Milliseconds}", new Vector2(0, 60), fpsColor);
                _spriteBatch.DrawString(font, $"Tick Count: {Zone.tickCount}", new Vector2(0, 80), DEBUG_COLOR);

                Utility.RenderMarkupText("That's an awfully <c:#ee5612>hot</c> <c:#814428>coffee</c> pot", new Vector2(0, 100));
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

       
        public static void AddMenu(Menu newMenu)
        {
            menus.Add(newMenu);
        }
        public static void RemoveMenu(Menu menuToClose)
        {
            menus.Remove(menuToClose);
        }

        /// <summary>
        /// Returns true if any menus are open that are interactable.
        /// </summary>
        /// <returns></returns>
        public static bool PauseMenuOpen()
        {
            foreach (Menu m in menus)
                if (m.TakesInput)
                    return true;

            return false;
        }

        public static void QueueRefreshAllMenus()
        {
            // i wrote this method to prevent RefreshAllMenus from being called multiple times a frame
            refreshAllMenusQueued = true;
        }
        private static void RefreshAllMenus()
        {
            foreach (Menu menu in menus)
                menu.Refresh();

            refreshAllMenusQueued = false;
        }
    }
}
