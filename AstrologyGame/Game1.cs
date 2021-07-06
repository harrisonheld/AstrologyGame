using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics;
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

        // for input
        private const int INPUT_STAGGER = 1000 / 8; // the input stagger in milliseconds
        private int timeSinceLastInput = 0; // in milliseconds
        private bool inputLastFrame = false; // did the player do any input the frame prior?
        private OrderedPair movePair;

        // menu
        private static List<Menu> menus = new List<Menu>();
        private static LookMenu lookMenu;
        // an easy way to get the last menu in the list, which is the only one that should accept input
        private static Menu CurrentMenu
        {
            get
            {
                if (menus.Count == 0)
                    return null;

                // return the last element in menus
                return menus.Last();
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

        private static GameState gameState = GameState.FreeRoam;
        private static InteractType interactType = InteractType.General;
        // should the zone do a tick this frame?
        private bool doTick = false; 

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
            Input.Update();
            timeSinceLastInput += gameTime.ElapsedGameTime.Milliseconds;
            
            // if the Input Stagger time has elapsed, or if the user didn't press anything during the last frame
            // and if the user pressed a control
            if((timeSinceLastInput > INPUT_STAGGER || !inputLastFrame) && Input.Controls.Count != 0)
            {
                // THIS LOOP EXECUTING MEANS A TURN IS HAPPENING!
                movePair = Input.ControlsToMovePair(Input.Controls);

                switch (gameState)
                {
                    case GameState.FreeRoam:
                        Update_FreeRoam();
                        break;

                    case GameState.InMenu:
                        Update_InMenu();
                        break;

                    case GameState.InteractMode:
                        Update_Interacting();
                        break;

                    case GameState.LookMode:
                        Update_Looking();
                        break;
                }

                // Toggle fullscreen
                if (Input.Controls.Contains(Control.Fullscreen))
                {
                    if (graphicsDeviceManager.IsFullScreen)
                    {
                        ScreenSize = (1024, 576);
                    }
                    else
                    {
                        ScreenSize = (1920, 1080);
                    }

                    graphicsDeviceManager.ToggleFullScreen();
                }

                // tick the level
                if (doTick)
                {
                    Zone.Tick();
                    doTick = false;
                }

                timeSinceLastInput = 0;
            }

            if (Input.Controls.Count == 0)
                inputLastFrame = false;
            else
                inputLastFrame = true;

            base.Update(gameTime);
        }

        private void Update_FreeRoam()
        {
            // TODO: we have a lot of if-else statements chained together, and it runs every frame. not good.
            // a switch statement might be suitable, but we don't have just one control, we have a list of them,
            // and the user may be pressing multiple at once.

            // open the pause menu
            if (Input.Controls.Contains(Control.Back))
            {
                Menu pauseMenu = new Menu();
                pauseMenu.Text = "[Paused]";
                OpenMenu(pauseMenu);
                return;
            }
            // open the dev menu
            if (Input.Controls.Contains(Control.DevFunc1))
            {
                DevSpawnMenu devMenu = new DevSpawnMenu();
                OpenMenu(devMenu);
                return;
            }
            // start interact mode
            else if (Input.Controls.Contains(Control.Interact))
            {
                gameState = GameState.InteractMode;

                if (Input.Controls.Contains(Control.Alternate))
                    interactType = InteractType.Specific;
                else
                    interactType = InteractType.General;

                return;
            }
            // open the get menu
            else if(Input.Controls.Contains(Control.Get))
            {
                List<Entity> itemsHere = new List<Entity>();

                foreach(Entity o in Zone.GetEntitiesAtPosition(Zone.Player.GetComponent<Position>().Pos))
                {
                    if(o.HasComponent<Item>())
                        itemsHere.Add(o);
                }

                Menu getMenu = new ItemMenu( itemsHere);
                OpenMenu(getMenu);
                return;
            }
            else if (Input.Controls.Contains(Control.Look))
            {
                // put the cursor at the player
                OrderedPair cursorPos = Zone.Player.GetComponent<Position>().Pos;
                cursorX = cursorPos.X;
                cursorY = cursorPos.Y;
                gameState = GameState.LookMode;
                return;
            }
            else if (Input.Controls.Contains(Control.Inventory))
            {
                // open inventory here
                Menu menu = new ItemMenu(Zone.Player.GetComponent<Inventory>().Contents);
                OpenMenu(menu);

                return;
            }
            else if (Input.Controls.Contains(Control.Here))
            {
                doTick = true;
                return;
            }

            OrderedPair movePair = Input.ControlsToMovePair(Input.Controls);
            OrderedPair playerPos = Zone.Player.GetComponent<Position>().Pos;

            if (movePair.Equals(OrderedPair.Zero)) // no movent given. stop here
                return;

            int newX = playerPos.X + movePair.X;
            int newY = playerPos.Y + movePair.Y;

            Zone.Player.GetComponent<Position>().Pos = (newX, newY);
            bool moveSuccessful = true; // TODO: check if there was a rock in the way or some shit

            if (moveSuccessful)
            {
                // THIS CODE IS EXECUTED WHEN THE PLAYER SUCCESSFULLY MOVES TO A NEW TILE
                doTick = true;

                // generate a new zone if the player exits the current one
                if (newX >= Zone.WIDTH || newX < 0 || newY >= Zone.HEIGHT || newY < 0)
                {
                    if (newX >= Zone.WIDTH)
                    {
                        Zone.Player.GetComponent<Position>().X = 0;
                        World.ZoneX += 1;
                    }
                    else if (newX < 0)
                    {
                        Zone.Player.GetComponent<Position>().X = Zone.WIDTH - 1;
                        World.ZoneX -= 1;
                    }
                    if (newY >= Zone.HEIGHT)
                    {
                        Zone.Player.GetComponent<Position>().Y = 0;
                        World.ZoneY -= 1;
                    }
                    else if (newY < 0)
                    {
                        Zone.Player.GetComponent<Position>().Y = Zone.HEIGHT - 1;
                        World.ZoneY += 1;
                    }

                    World.GenerateCurrentZone();
                }
            }
        }
        private void Update_InMenu()
        {
            CurrentMenu.HandleInput(Input.Controls);
        }
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
                doTick = true;
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

        /// <summary>
        /// Create and open a LookMenu for the given object.
        /// </summary>
        /// <param name="o"></param>
        private void Look(Entity o)
        {
            lookMenu = new LookMenu(o);
            OpenMenu(lookMenu);
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
            if (gameState == GameState.LookMode)
            {
                Rectangle destinationRectangle = new Rectangle(
                    cursorX * Utility.SCALE, cursorY * Utility.SCALE, Utility.SCALE, Utility.SCALE);
                _spriteBatch.Draw(cursor, destinationRectangle, Color.White);
            }
            // draw all the menus
            foreach (Menu m in menus)
                m.Draw();

            // draw debug info
            if (Input.Controls.Contains(Control.DevInfo))
            {
                OrderedPair playerPos = Zone.Player.GetComponent<Position>().Pos;
                _spriteBatch.DrawString(font, $"Zone: ({World.ZoneX}, {World.ZoneY})", new Vector2(0, 0), DEBUG_COLOR);
                _spriteBatch.DrawString(font, $"Player Pos: ({playerPos.X}, {playerPos.Y})", new Vector2(0, 20), DEBUG_COLOR);
                _spriteBatch.DrawString(font, $"Gamestate: {gameState}", new Vector2(0, 40), DEBUG_COLOR);
                Color fpsColor = gameTime.IsRunningSlowly ? DEBUG_COLOR_BAD : DEBUG_COLOR; // change color depending on if game is running slow
                _spriteBatch.DrawString(font, $"FPS: {1000 / gameTime.ElapsedGameTime.Milliseconds}", new Vector2(0, 60), fpsColor);

                Utility.RenderMarkupText("That's an awfully <c:#ee5612>hot</c> <c:#814428>coffee</c> pot", new Vector2(0, 80));
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void OpenMenu(Menu newMenu)
        {
            menus.Add(newMenu);

            if (newMenu.PauseWhenOpened)
                gameState = GameState.InMenu;
        }
        public static void CloseMenu(Menu menuToClose)
        {
            menus.Remove(menuToClose);

            // if none of the remaining open menus have .pauseWhenOpened, set gamestate to FreeRoam.
            if(gameState == GameState.InMenu)
            {
                foreach (Menu m in menus)
                    if (m.PauseWhenOpened)
                        return;

                gameState = GameState.FreeRoam;
            }
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

        public enum GameState
        {
            FreeRoam, //the player is walking around
            InMenu, // the player is in a menu and must exit it before doing anything else
            InteractMode, // the player wants to interact with an adjacent tile
            LookMode // the player is looking at things
        }

        public enum InteractType
        {
            General, // the chosen object will have its first interaction chosen automatically
            Specific // the chosen object will bring up a list of possible interactions
        }
    }
}
