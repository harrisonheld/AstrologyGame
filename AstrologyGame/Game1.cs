using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AstrologyGame.MapData;
using AstrologyGame.DynamicObjects;
using System.Diagnostics;
using System.Linq;

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
        private List<Control> controls; // list of all controls pressed this frame
        private bool inputLastFrame = false; // did the player do any input the frame prior?
        private OrderedPair movePair;

        // menu
        private static List<Menu> menus = new List<Menu>();
        private static NearbyObjectsMenu nearbyObjectsMenu;
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
        public static Vector2 CursorPosition
        {
            get
            {
                return new Vector2(cursorX, cursorY);
            }
        }

        private static GameState gameState = GameState.FreeRoam;
        // should the zone do a tick this frame?
        private bool doTick = false; 

        // dev stuff
        Color DEBUG_COLOR = Color.Red;

        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Utility.Initialize(Content, GraphicsDevice, _spriteBatch);
            World.GenerateCurrentZone();

            DynamicObject knight = new Humanoid();
            knight.Color = Color.Bisque;
            knight.Name = "Knight";
            knight.TextureName = "human2";
            knight.X = 1;
            knight.Y = 0;
            knight.Children.Add(new Flintlock());
            knight.Children.Add(new Book("Poets to Come"));
            Zone.Objects.Add(knight);
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
            cursor = Utility.TryLoadTexture("cursor1");
            font = Content.Load<SpriteFont>("font1");
            Menu.Initalize(font);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            Content.Dispose();

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            controls = Input.GetInput();

            timeSinceLastInput += gameTime.ElapsedGameTime.Milliseconds;
            

            // if the Input Stagger time has elapsed, or if the user didn't press anything during the last frame
            // and if the user pressed a control
            if((timeSinceLastInput > INPUT_STAGGER || !inputLastFrame) && controls.Count != 0)
            {
                // THIS LOOP EXECUTING MEANS A TURN IS HAPPENING!
                movePair = Input.ControlsToMovePair(controls);

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
                if (controls.Contains(Control.Fullscreen))
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

            if (controls.Count == 0)
                inputLastFrame = false;
            else
                inputLastFrame = true;

            base.Update(gameTime);
        }

        private void Update_FreeRoam()
        {
            if (controls.Contains(Control.Interact))
            {
                gameState = GameState.InteractMode;
                return;
            }
            else if(controls.Contains(Control.Get))
            {
                List<Item> itemsHere = new List<Item>();

                foreach(DynamicObject o in Zone.ObjectsAtPosition(Zone.Player.X, Zone.Player.Y))
                {
                    if(o is Item)
                        itemsHere.Add(o as Item);
                }

                Menu getMenu = new GetMenu(itemsHere);
                OpenMenu(getMenu);
            }
            else if (controls.Contains(Control.Look))
            {
                cursorX = Zone.Player.X;
                cursorY = Zone.Player.Y;
                gameState = GameState.LookMode;
                return;
            }
            else if (controls.Contains(Control.Inventory))
            {
                // open inventory here
                Menu menu = new InventoryMenu(Zone.Player);
                OpenMenu(menu);

                return;
            }
            else if (controls.Contains(Control.Here))
            {
                doTick = true;
                return;
            }

            OrderedPair orderedPair = Input.ControlsToMovePair(controls);

            int newX = Zone.Player.X + movePair.X;
            int newY = Zone.Player.Y + movePair.Y;

            bool moveSuccessful = ((Creature)(Zone.Player)).TryMove(newX, newY);

            if (moveSuccessful)
            {
                // THIS CODE IS EXECUTED WHEN THE PLAYER SUCCESSFULLY MOVES TO A NEW TILE
                doTick = true;
                CloseMenu(nearbyObjectsMenu); // close Nearby Objects Menu because we moved

                List<DynamicObject> nearbyObjects = Zone.ObjectsAtPosition(Zone.Player.X, Zone.Player.Y);
                nearbyObjects.Remove(Zone.Player);

                if(nearbyObjects.Count > 0)
                {
                    nearbyObjectsMenu = new NearbyObjectsMenu(nearbyObjects);
                    OpenMenu(nearbyObjectsMenu);
                }
            }

            // generate a new zone if the player exits the current one
            if (Zone.Player.X >= Zone.WIDTH || Zone.Player.X < 0 || Zone.Player.Y >= Zone.HEIGHT || Zone.Player.Y < 0)
            {
                if (Zone.Player.X >= Zone.WIDTH)
                {
                    Zone.Player.X = 0;
                    World.ZoneX += 1;
                }
                else if (Zone.Player.X < 0)
                {
                    Zone.Player.X = Zone.WIDTH - 1;
                    World.ZoneX -= 1;
                }
                if (Zone.Player.Y >= Zone.HEIGHT)
                {
                    Zone.Player.Y = 0;
                    World.ZoneY -= 1;
                }
                else if (Zone.Player.Y < 0)
                {
                    Zone.Player.Y = Zone.HEIGHT - 1;
                    World.ZoneY += 1;
                }

                World.GenerateCurrentZone();
            }

            if (controls.Contains(Control.Back))
                // TODO: pause
                ;
        }
        private void Update_InMenu()
        {
            CurrentMenu.HandleInput(controls);
        }
        private void Update_Interacting()
        {
            // the player hit escape, so change player state to free roam and break
            if (controls.Contains(Control.Back))
            {
                gameState = GameState.FreeRoam;
                return;
            }

            // if the player didnt use a directional key, or select his current space, do nothing and break
            if (movePair.X == 0 && movePair.Y == 0 && !(controls.Contains(Control.Here)) )
            {
                return;
            }

            int interactX = Zone.Player.X + movePair.X;
            int interactY = Zone.Player.Y + movePair.Y;

            foreach (DynamicObject o in Zone.Objects)
            {
                if (o.X == interactX && o.Y == interactY)
                {
                    if (o != Zone.Player)
                    {
                        o.Interact(Interaction.Open, Zone.Player);
                        break;
                    }
                    // TODO
                    // currently, most objects interact methods will set the gameState to InMenu
                    // however, if not, you may remain in the state Interacting. So fix this some time
                }
            }
        }
        private void Update_Looking()
        {
            if(controls.Contains(Control.Back))
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

                // TODO: pick an object to look at
                DynamicObject objectToLookAt = null;

                foreach (DynamicObject o in Zone.Objects)
                {
                    if (o.X == cursorX && o.Y == cursorY)
                    {
                        objectToLookAt = o;
                        break;
                    }
                }

                // look at the tile if there are no objects
                if(objectToLookAt is null)
                {
                    objectToLookAt = Zone.tiles[cursorX, cursorY];
                }

                Look(objectToLookAt);
            }
        }

        /// <summary>
        /// Create and open a LookMenu for the given object.
        /// </summary>
        /// <param name="o"></param>
        private void Look(DynamicObject o)
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

            // draw the zone
            Zone.Draw();

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
            if (controls.Contains(Control.DevInfo))
            {
                _spriteBatch.DrawString(font, $"Zone: ({World.ZoneX}, {World.ZoneY})", new Vector2(576 - 16, 0), DEBUG_COLOR);
                _spriteBatch.DrawString(font, $"Player Pos: ({Zone.Player.X}, {Zone.Player.Y})", new Vector2(576 - 16, 20), DEBUG_COLOR);
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
    }
}
