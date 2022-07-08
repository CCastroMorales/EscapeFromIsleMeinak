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

    public static class Action
    {
        public static readonly string READ = "ACTION_READ";
    }

    public class ItemAction
    {
        public string Action { get; set; } = "";
        public string Text { get; set; } = "";
    }

    public class Item
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Description { get; set; } = "";
        public string InventoryDescription { get; set; } = "";
        public List<ItemAction> Actions { get; set; } = new List<ItemAction>();
        public List<string> Labels { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }

        public bool HasAction(string hasAction)
        {
            foreach (ItemAction action in Actions)
                if (action.Action == hasAction)
                    return true;
            return false;
        }

        public ItemAction GetAction(string actionId)
        {
            foreach (ItemAction action in Actions)
                if (action.Action == actionId)
                    return action;
            return null;
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

    public class Entity
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Dead { get; set; } = false;
        public bool ShowDescriptionWhenDead { get; set; } = true;
        public List<string> KillBy { get; set; } = new List<string>();
        public Item DropItem { get; set; } = null;
        public bool HasDropItem { get => DropItem != null; }

        public override string ToString()
        {
            return Id;
        }

        public Item Kill()
        {
            Dead = true;
            return DropItem;
        }
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
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Entity> Entities { get; set; } = new List<Entity>();
        public bool HasOptions { get => Options.Count > 0; }

        public override string ToString()
        {
            return $"{SceneId} ({Title}), exits: {Exits.Count}";
        }

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

        public bool HasItem(string itemLabel)
        {
            foreach (Item item in Items)
                foreach (string label in item.Labels)
                    if (label.ToLower() == itemLabel.ToLower())
                        return true;
            return false;
        }

        public Item Take(string itemLabel)
        {
            foreach (Item item in Items)
                foreach (string label in item.Labels)
                    if (label.ToLower() == itemLabel.ToLower())
                    {
                        Item takenItem = item;
                        Items.Remove(item);
                        return takenItem;
                    }
            return null;
        }

        public bool HasEntity(string entityName)
        {
            foreach (Entity entity in Entities)
                if (entity.Name.ToLower() == entityName.ToLower())
                    return true;
            return false;
        }

        public Entity EntityByName(string entityName)
        {
            foreach (Entity entity in Entities)
                if (entity.Name.ToLower() == entityName.ToLower())
                    return entity;
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
