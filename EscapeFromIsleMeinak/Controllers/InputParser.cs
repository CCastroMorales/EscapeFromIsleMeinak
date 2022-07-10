using EscapeFromIsleMainak.Components;
using System.Collections.Generic;

namespace EscapeFromIsleMainak
{
    public interface ParseCallback
    {
        void OnPrint(string text);
        bool HasPreviousScene();
        void OnPreviousScene();
        void OnExitScene(Exit exit);
        void OnCheckDescription(string objectName);
        void OnTakeItem(Item item, string itemLabel);
        void OnReadItem(Item item, ItemAction action, string label);
        void OnPunchEntity(Entity entity, string entityName);
        void OnUse(Item item, string itemName, string target);
    }

    public class InputParser
    {
        public ParseCallback Callback { get; set; }
        public bool Done { get; set; } = false;
        public Scene ActiveScene { get; set; }
        public List<Item> Inventory { get; set; }

        public InputParser(ParseCallback callback)
        {
            Callback = callback;
        }

        public void Reset()
        {
            Done = false;
        }

        public void Parse(string rawInput, Scene scene)
        {
            ActiveScene = scene;

            string input = rawInput.ToLower().Trim();

            string command;
            string[] arguments;

            ParseArguments(input, out command, out arguments);


            if (ParseGO(command, arguments))
                Done = true;
            /*if (ParseCHECK(command, arguments))
                Done = true;*/
            if (ParseTAKE(command, arguments))
                Done = true;
            if (ParseActionREAD(command, arguments))
                Done = true;
            if (ParseActionHIT(command, arguments))
                Done = true;
            if (ParseActionUSE(command, arguments))
                Done = true;
        }

        private string[] ParseArguments(string input, out string command, out string[] arguments)
        {
            string[] splitInput = input.Split(' ');
            arguments = new string[splitInput.Length - 1];
            command = splitInput[0];

            for (int i = 0; i < arguments.Length; i++)
                arguments[i] = splitInput[i + 1];

            return splitInput;
        }

        protected bool ParseGO(string command, string[] arguments)
        {
            if (command != Commands.GO)
                return false;

            if (arguments.Length == 0)
            {
                Callback.OnPrint("Go where?");
                return false;
            }

            string exitName = arguments[0];

            if (exitName == Arguments.BACK)
            {
                bool canGoBack = Callback.HasPreviousScene() && ActiveScene.AllowGoBack;

                if (canGoBack)
                {
                    Callback.OnPreviousScene();
                    return true;
                }
                else
                {
                    Callback.OnPrint("There's nowhere to go back to.");
                    return false;
                }
            }


            Exit exit = ActiveScene.FindExit(exitName);

            if (exit != null)
            {
                Callback.OnExitScene(exit);
                return true;
            }

            return false;
        }

        /*protected bool ParseCHECK(string command, string[] arguments)
        {
            if (command != Commands.CHECK)
                return false;

            if (arguments.Length == 0)
            {
                Callback.OnPrint("Check what?");
                return false;
            }

            string objectName = arguments[0];

            if (ActiveScene.HasInteractiveObject(objectName))
            {
                Callback.OnCheckDescription(objectName);
                return false;
            }

            return false;
        }*/

        protected bool ParseTAKE(string command, string[] arguments)
        {
            if (command != Commands.TAKE)
                return false;

            if (arguments.Length == 0)
            {
                Callback.OnPrint("Take what?");
                return false;
            }

            string label = arguments[0];
            Item item = ActiveScene.FindItem(label);

            if (item != null)
            {
                Callback.OnTakeItem(item, label);
                return false;
            }

            return false;
        }

        protected bool ParseActionREAD(string command, string[] arguments)
        {
            if (command != Commands.READ)
                return false;

            if (arguments.Length == 0)
            {
                Callback.OnPrint("Read what?");
                return false;
            }

            string itemLabel = arguments[0];
            bool actionReadTaken = false;

            // Check for item in inventory first.
            Item item = FindItem(Inventory, itemLabel);

            if (item != null)
            {
                ItemAction action = item.GetAction(Id.ACTION_READ);
                Callback.OnReadItem(item, action, itemLabel);
                actionReadTaken = true;
            }
            else 
            {
                item = ActiveScene.FindItem(itemLabel);

                if (item != null)
                {
                    ItemAction action = item.GetAction(Id.ACTION_READ);
                    Callback.OnReadItem(item, action, itemLabel);
                    actionReadTaken = true;
                }
            }

            if (!actionReadTaken)
                Callback.OnPrint("You can't do that.");
            
            return false;
        }

        private bool ParseActionHIT(string command, string[] arguments)
        {
            if (command != Commands.HIT && command != Commands.PUNCH)
                return false;

            if (arguments.Length == 0)
            {
                Callback.OnPrint("You punch a couple of times in the air. You look very cool while doing it.");
            }

            Entity entity = ActiveScene.FindEntity(arguments[0]);

            if (entity != null)
            {
                Callback.OnPunchEntity(entity, arguments[0]);
            }

            return false;
        }

        private bool ParseActionUSE(string command, string[] arguments)
        {
            if (command != Commands.USE)
                return false;

            if (arguments.Length == 0)
            {
                Callback.OnPrint("What do you want to use it on?");
                return false;
            }

            string itemName = arguments[0];

            if (arguments.Length == 1)
            {
                // Check for special jeep use case
                Item item = FindItem(Inventory, itemName);
                if (item != null && item.Id == Id.ITEM_JEEP_KEY && ActiveScene.Id == Id.SCENE_SPECIAL_VEHICLE_JEEP)
                {
                    Callback.OnUse(item, itemName, "VEHICLE_JEEP");
                    return true;
                }
                // Check for special boat use case
                else if (item != null && item.Id == Id.ITEM_BOAT_KEY && ActiveScene.Id == Id.SCENE_SPECIAL_VEHICLE_BOAT)
                {
                    Callback.OnUse(item, itemName, "VEHICLE_BOAT_46");
                    return true;
                }
                else
                    Callback.OnPrint("What do you want to use it on?");
                return false;
            }

            if (arguments.Length >= 2)
            {
                string target = arguments[1];

                // Check for "on" preposition
                if (arguments.Length >= 3 && arguments[1].ToLower() == "on")
                    target = arguments[2];

                // Check for item in inventory first
                Item item = FindItem(Inventory, itemName);

                // Check for special jeep use case
                if (item != null && item.Id == Id.ITEM_JEEP_KEY && ActiveScene.Id == Id.SCENE_SPECIAL_VEHICLE_JEEP)
                {
                    Callback.OnUse(item, itemName, "VEHICLE_JEEP");
                    return true;
                }
                // Check for special boat use case
                else if (item != null && item.Id == Id.ITEM_BOAT_KEY && ActiveScene.Id == Id.SCENE_SPECIAL_VEHICLE_BOAT)
                {
                    Callback.OnUse(item, itemName, "VEHICLE_BOAT_46");
                    return true;
                }
                else if (item != null)
                    Callback.OnUse(item, itemName, target);
                else
                // Check for item in scene.
                {
                    item = ActiveScene.FindItem(itemName);

                    if (item != null)
                        Callback.OnUse(item, itemName, target);
                }

                return false;
            }

            Callback.OnPrint($"You can't use that!");
            return false;
        }

        private Item FindItem(List<Item> items, string label)
        {
            foreach (Item item in items)
                if (item.Labels.Contains(label.ToLower()))
                    return item;
            return null;
        }
    }
}