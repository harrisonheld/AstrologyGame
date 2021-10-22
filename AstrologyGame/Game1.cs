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

        private static OrderedPair screenSize = new OrderedPair(1024*3, 576*3);
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

        // menus
        private static StatusMenu statusMenu;
        private static List<Menu> menus = new List<Menu>();

        public static StatusMenu StatusMenu { get => statusMenu; }

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
            World.Seed = "nuts 2";
            World.GenerateCurrentZone();

            Entity knight = EntityFactory.EntityFromId("knight", 20, 5);
            knight.AddComponent(new PlayerControlled());
            knight.GetComponent<Inventory>().Slots.AddRange(new List<Slot>()
            {
                new Slot { Type = SlotType.Body },
                new Slot { Type = SlotType.Legs }
            });
            Zone.AddEntity(knight);

            Entity pisces = EntityFactory.EntityFromId("pisces", 0, 0);
            pisces.GetComponent<FactionInfo>().SetReputation(Faction.Human, -100);
            Zone.AddEntity(pisces);

            statusMenu = new StatusMenu(knight);
            menus.Add(statusMenu);

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

            font = Content.Load<SpriteFont>("font1");

            GameManager.Initialize(Content, GraphicsDevice, _spriteBatch, font);
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(default, default, SamplerState.PointClamp);

            // draw the game
            RenderingFunctions.RenderZone();
            // draw the look cursor
            RenderingFunctions.RenderLookCursor();
            // draw the menus
            RenderingFunctions.RenderMenus(menus);

            // draw debug info
            if (Input.Controls.Contains(Control.DevInfo))
            {
                Color fpsColor = gameTime.IsRunningSlowly ? DEBUG_COLOR_BAD : DEBUG_COLOR; // change color depending on if game is running slow
                _spriteBatch.DrawString(font, $"FPS: {1000 / gameTime.ElapsedGameTime.Milliseconds}", new Vector2(0, 0), fpsColor);
                _spriteBatch.DrawString(font, $"Tick Count: {Zone.tickCount}", new Vector2(0, 20), DEBUG_COLOR);

                GameManager.RenderMarkupText("That's an awfully <c:#ee5612>hot</c> <c:#814428>coffee</c> pot", new Vector2(0, 40));
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
    }
}
