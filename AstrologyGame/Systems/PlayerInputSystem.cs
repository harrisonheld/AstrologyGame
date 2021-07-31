using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using AstrologyGame.Entities;
using AstrologyGame.Components;
using AstrologyGame.MapData;
using AstrologyGame.Menus;

namespace AstrologyGame.Systems
{
    public sealed class PlayerInputSystem : ISystem
    {
        ComponentFilter ISystem.Filter => new ComponentFilter()
             .AddNecessary(typeof(PlayerControlled));

        private bool finished = false;
        public bool Finished { get { return finished;  } }

        private InputMode inputMode = InputMode.FreeRoam;
        private InteractType interactType = InteractType.General;
        private int timeSinceLastInput = 0; // in milliseconds
        private bool inputLastUpdate = false;

        void ISystem.OperateOnEntity(Entity entity)
        {
            Input.Update();

            timeSinceLastInput += Game1.DeltaTime;

            // if the Input Stagger time has elapsed, or if the user didn't press anything during the last frame
            // and if the user pressed a control
            if ((timeSinceLastInput > Utility.INPUT_STAGGER || !inputLastUpdate) && Input.Controls.Count != 0)
            {
                // define some variables that are generally useful in all Input Modes
                OrderedPair movePair = Input.ControlsToMovePair(Input.Controls);
                OrderedPair entityPos = entity.GetComponent<Position>().Pos;

                // if player was just in menu mode, but no menu requires input
                if (inputMode == InputMode.MenuInput)
                {
                    if (Game1.InputMenu == null)
                        inputMode = InputMode.FreeRoam;
                }
                else if (Game1.InputMenu != null)
                    inputMode = InputMode.MenuInput;

                // handle any inputs here
                switch (inputMode)
                {
                    #region Free Roam Mode
                    case InputMode.FreeRoam:
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
                            inputMode = InputMode.Interacting;

                            if (Input.Controls.Contains(Control.Alternate))
                                interactType = InteractType.Specific;
                            else
                                interactType = InteractType.General;

                            return;
                        }

                        // open the get menu
                        else if (Input.Controls.Contains(Control.Get))
                        {
                            List<Entity> itemsHere = new List<Entity>();
                            OrderedPair pos = entity.GetComponent<Position>().Pos;

                            foreach (Entity o in Zone.GetEntitiesAtPosition(pos))
                            {
                                if (o.HasComponent<Item>())
                                    itemsHere.Add(o);
                            }

                            Menu getMenu = new ItemMenu(entity, itemsHere);
                            OpenMenu(getMenu);
                            return;
                        }
                        // go to look mode
                        else if(Input.Controls.Contains(Control.Look))
                        {
                            inputMode = InputMode.Looking;
                            return;
                        }

                        // open the inventory
                        else if (Input.Controls.Contains(Control.Inventory))
                        {
                            // open inventory here
                            Menu menu = new ItemMenu(entity, entity.GetComponent<Inventory>().Contents);
                            OpenMenu(menu);

                            return;
                        }

                        if (movePair.Equals(OrderedPair.Zero)) // no movent given. stop here
                            return;

                        OrderedPair newPos = entityPos + movePair;
                        bool moveSuccessful = MoveFunctions.CanMove(entity, newPos);

                        if(moveSuccessful)
                        {
                            MoveFunctions.Move(entity, newPos);
                            finished = true;
                        }

                        break;
                    #endregion
                    #region In Menu Mode
                    case InputMode.MenuInput:
                        Game1.InputMenu.HandleInput(Input.Controls);
                        break;
                    #endregion
                    #region Interact Mode
                    case InputMode.Interacting:
                        // player wants to leave Interact mode, change to free roam and return
                        if (Input.Controls.Contains(Control.Back))
                        {
                            inputMode = InputMode.FreeRoam;
                            return;
                        }

                        // if the player didnt use a directional key, or select his current space, do nothing and return
                        if (movePair.X == 0 && movePair.Y == 0 && !Input.Controls.Contains(Control.Here))
                            return;

                        int interactX = entityPos.X + movePair.X;
                        int interactY = entityPos.Y + movePair.Y;

                        List<Entity> entitiesHere = Zone.GetEntitiesAtPosition(new OrderedPair(interactX, interactY));
                        // there are no entities here, return
                        if (entitiesHere.Count == 0)
                            return;

                        Entity toInteractWith = entitiesHere.Last();

                        if (interactType == InteractType.Specific)
                        {
                            InteractionMenu interactionMenu = new InteractionMenu(entity, toInteractWith);
                            OpenMenu(interactionMenu);
                        }
                        else // interactType == InteractType.General
                        {
                            List<Interaction> interactions = toInteractWith.GetInteractions();
                            // if entity has no interactions, return
                            if (interactions.Count == 0)
                                return;

                            // do the first interaction in the list
                            interactions[0].Perform(entity);

                            // if we're stil in interact mode, go back to Free Roam
                            if (inputMode == InputMode.Interacting)
                                inputMode = InputMode.FreeRoam;
                        }

                        break;
                    #endregion
                    #region Look Mode
                    case InputMode.Looking:
                        // do stuff
                        break;
                    #endregion
                }

                timeSinceLastInput = 0;
            }

            if (Input.Controls.Count == 0)
                inputLastUpdate = false;
            else
                inputLastUpdate = true;
        }

        public void Reset()
        {
            inputMode = InputMode.FreeRoam;
            finished = false;
        }

        private void OpenMenu(Menu toOpen)
        {
            Game1.AddMenu(toOpen);
        }

        private enum InputMode
        {
            FreeRoam, //the player is walking around
            MenuInput, // the player is in a menu and must exit it before doing anything else
            Interacting, // the player wants to interact with an adjacent tile
            Looking // the player is looking at things
        }
        public enum InteractType
        {
            General, // the chosen object will have its first interaction chosen automatically
            Specific // the chosen object will bring up a list of possible interactions
        }
    }
}
