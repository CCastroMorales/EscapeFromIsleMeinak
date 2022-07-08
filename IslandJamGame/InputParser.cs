﻿using ScriptLibrary;
using System;
using System.Collections.Generic;

namespace IslandJamGame
{
    public interface ParseCallback
    {
        void OnPrint(string text);
        bool HasPreviousScene();
        void OnPreviousScene();
        void OnExitScene(string sceneId, string exit, string triggerEntityId);
        void OnCheckDescription(string objectName);
        void OnTakeItem(string itemLabel);
        void OnReadItem(Item item, ItemAction action, string label);
        void OnPunchEntity(string entityName);
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
            if (ParseCHECK(command, arguments))
                Done = true;
            if (ParseTAKE(command, arguments))
                Done = true;
            if (ParseActionREAD(command, arguments))
                Done = true;
            if (ParseActionHIT(command, arguments))
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
                bool canGoBack = Callback.HasPreviousScene();

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

            if (ActiveScene.HasExit(exitName))
            {
                string sceneId = ActiveScene.SceneFromExit(exitName);
                string triggerEntityId = ActiveScene.GetExitTriggerEntity(exitName);

                Callback.OnExitScene(sceneId, exitName, triggerEntityId);
                return true;
            }

            return false;
        }

        protected bool ParseCHECK(string command, string[] arguments)
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

            string itemLabel = arguments[0];

            if (ActiveScene.HasItem(itemLabel))
            {
                Callback.OnTakeItem(itemLabel);
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
            foreach (Item item in Inventory)
                foreach (string label in item.Labels)
                {
                    if (label.ToLower() == itemLabel.ToLower())
                    {
                        ItemAction action = item.GetAction("ACTION_READ");
                        Callback.OnReadItem(item, action, label);
                        actionReadTaken = true;
                        break;
                    }

                    if (actionReadTaken)
                        break;
                }

            if (!actionReadTaken) 
            {
                foreach (Item item in ActiveScene.Items)
                    foreach (string label in item.Labels)
                    {
                        if (label.ToLower() == itemLabel.ToLower())
                            if (item.HasAction("ACTION_READ"))
                            {
                                ItemAction action = item.GetAction("ACTION_READ");
                                Callback.OnReadItem(item, action, label);
                                actionReadTaken = true;
                                break;
                            }

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

            if (ActiveScene.HasEntity(arguments[0]))
            {
                Callback.OnPunchEntity(arguments[0]);
            }

            return false;
        }
    }
}