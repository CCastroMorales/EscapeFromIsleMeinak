using ScriptLibrary;
using System;

namespace IslandJamGame
{
    public interface ParseCallback
    {
        bool HasPreviousScene();
        void OnPreviousScene();
        void OnExitScene(string sceneId, string exit);
        void OnCheckDescription(string objectName);
        void OnTakeItem(string itemLabel);
    }

    public class InputParser
    {
        public ParseCallback Callback { get; set; }
        public Scene ActiveScene { get; set; }
        public bool Done { get; set; } = false;

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
                Console.WriteLine("Go where?");
                return false;
            }

            string argument = arguments[0];

            if (argument == Arguments.BACK)
            {
                bool canGoBack = Callback.HasPreviousScene();

                if (canGoBack)
                {
                    Callback.OnPreviousScene();
                    return true;
                }
                else
                {
                    Console.WriteLine("There's nowhere to go back to.");
                    return false;
                }
            }

            if (ActiveScene.HasExit(argument))
            {
                string sceneId = ActiveScene.SceneFromExit(argument);
                Callback.OnExitScene(sceneId, argument);
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
                Console.WriteLine("Check what?");
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

        protected bool ParseTAKE(string commands, string[] arguments)
        {
            if (commands != Commands.TAKE)
                return false;

            if (arguments.Length == 0)
            {
                Console.WriteLine("Take what?");
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
    }
}