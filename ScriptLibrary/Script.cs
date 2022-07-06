using System;
using System.Collections.Generic;

namespace ScriptLibrary
{
    public class Option
    {
        public string text { get; set; } = "";
        public string Command { get; set; } = "";
        public string NextScene { get; set; } = "";
    }

    public class Options
    {
        public List<Option> options = new List<Option>();   
    }

    public class Exit
    {
        public string Name { get; set; } = "";
        public string Scene { get; set; } = "";
    }

    public class Item
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }

    public class Container
    {
        public string Name { get; set; } = "";
        public List<string> Items { get; set; } = new List<string>();
    }

    public class InteractiveObject
    {
        public string Name { get; set; } = "";
        public List<string> Text { get; set; } = new List<string>();
    }

    public class Scene
    {
        public string SceneId { get; set; } = "";
        public string Title { get; set; } = "";
        public List<string> text = new List<string>();
        public string NextScene { get; set; } = "";
        public List<Option> Options { get; set; } = new List<Option>();
        public List<Exit> Exits { get; set; } = new List<Exit>();
        public List<Container> Containers { get; set; } = new List<Container>();
        public List<InteractiveObject> Objects { get; set; } = new List<InteractiveObject>();
        public bool HasOptions { get => Options.Count > 0; }

        public bool HasExit(string exitName)
        {
            foreach (Exit exit in Exits)
                if (exit.Name == exitName)
                    return true;
            return false;
        }

        public string SceneFromExit(string exitName)
        {
            foreach (Exit exit in Exits)
                if (exit.Name == exitName)
                    return exit.Scene;
            return null;
        }
        
        public bool HasContainer(string containerName)
        {
            foreach (Container container in Containers)
                if (container.Name == containerName)
                    return true;
            return false;
        }

        public Container Container(string containerName)
        {
            foreach (Container container in Containers)
                if (container.Name == containerName)
                    return container;
            return null;
        }

        public bool HasInteractiveObject(string objectName)
        {
            foreach (InteractiveObject interactiveObject in Objects)
                if (interactiveObject.Name == objectName)
                    return true;
            return false;
        }

        public InteractiveObject ObjectbyName(string objectName)
        {
            foreach (InteractiveObject interactiveObject in Objects)
                if (interactiveObject.Name == objectName)
                    return interactiveObject;
            return null;
        }
    }

    public class Script
    {
        public List<Scene> Scenes = new List<Scene>();

        public bool SceneExists(string sceneId)
        {
            foreach (Scene scene in Scenes)
                if (scene.SceneId == sceneId)
                    return true;
            return false;
        }
    }
}
