using EscapeFromIsleMeinak;
using MeinakEsc.Components;
using System;
using System.Collections.Generic;

namespace MeinakEsc
{
    public interface ParseCallback
    {
        void OnPrint(string text);
        bool HasPreviousScene();
        void OnPreviousScene();
        void OnExitScene(Exit exit);
        void OnTakeItem(Item item, string itemLabel);
        void OnReadItem(Item item, ItemAction action, string label);
        void OnPunchEntity(Entity entity, string entityName);
    }

    public class InputParser
    {
        public ParseCallback Callback { get; set; }
        public bool Done { get; set; } = false;
        public Scene ActiveScene { get; set; }
        public Inventory Inventory { get; set; }
        private Check Check { get; } = new Check();
        private Drop Drop { get; } = new Drop();
        private Use Use { get; } = new Use();

        public InputParser(ParseCallback callback)
        {
            Callback = callback;
        }

        public void Reset()
        {
            Done = false;
        }

        public void Parse(Ctx ctx, string rawInput, Scene scene)
        {
            ActiveScene = scene;

            string input = rawInput.ToLower().Trim();

            string command;
            string[] arguments;

            ParseArguments(input, out command, out arguments);

            InputBundle bundle = new InputBundle(command, arguments);

            if (ParseQuit(ctx, command))
                Done = true;
            if (ParseGO(command, arguments))
                Done = true;
            // Testing a new interpreter design with parsers for each action.
            if (Drop.Parse(ctx, bundle))
                Done = true;
            if (Check.Parse(ctx, bundle))
                Done = true;
            if (Use.Parse(ctx, bundle))
                Done = true;
            if (ParseTAKE(command, arguments))
                Done = true;
            if (ParseActionREAD(command, arguments))
                Done = true;
            if (ParseActionHIT(command, arguments))
                Done = true;
            /*if (ParseActionUSE(command, arguments))
                Done = true;*/
        }

        private bool ParseQuit(Ctx ctx, string command)
        {
            if (command != Commands.QUIT && command != Commands.EXIT)
                return false;

            ctx.Game.PrintLine("Do you want to close the game? (y/n)");

            bool waitForInput = true;
            var readkey = Console.ReadKey(true);

            while (waitForInput)
            {
                if (readkey.Key == ConsoleKey.Y)
                {
                    ctx.Game.Running = false;
                    return true;
                }
                else if (readkey.Key == ConsoleKey.N)
                    return false;
                else
                    readkey = Console.ReadKey(true);
            }

            return false;
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
            Item item = FindItem(Inventory.Items, itemLabel);

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

        private Item FindItem(List<Item> items, string label)
        {
            foreach (Item item in items)
                if (item.Labels.Contains(label.ToLower()))
                    return item;
            return null;
        }
    }
}