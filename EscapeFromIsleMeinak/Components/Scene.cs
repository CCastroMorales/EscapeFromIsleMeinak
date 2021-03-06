using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace EscapeFromIsleMeinak.Components
{
    public abstract class Scene
    {
        public Id Id { get; set; }
        public string Title { get; set; } = "";
        public List<string> InitialScript { get; } = new List<string>();
        public List<string> Script { get; } = new List<string>();
        public List<Item> Items { get; } = new List<Item>();
        public List<Item> DroppedItems { get; } = new List<Item>();
        public List<Entity> Entities { get; } = new List<Entity>();
        public List<CheckObject> Objects { get; } = new List<CheckObject>();
        public List<Exit> Exits { get; } = new List<Exit>();
        public bool AllowGoBack { get; set; } = true;
        public bool InitialVisit { get; set; } = true;
        public bool AutoEnd { get; set; } = false;

        public void LoadTextFromResource(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string path = $"{assembly.GetName().Name}.res.{resource}";

            bool resourceExits = false;
            foreach (string resourceName in assembly.GetManifestResourceNames())
                if (resourceName == path)
                {
                    resourceExits = true;
                    break;
                }

            if (!resourceExits)
            {
                string message = $"Resource {path} is not embedded.";
                Debug.WriteLine(message);
                throw new Exception(message);
            }

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    string[] lines = json.Split('\n');

                    Title = lines[0];

                    for (int i = 2; i < lines.Length; i++)
                        Script.Add(lines[i]);
                }
            }
        }

        public abstract Id OnRegisterId();

        // TODO: Change to Init as Load implies being loaded in-game by the player.
        public void Load()
        {
            Id = OnRegisterId();
            string id = Id.ToString();
            //LoadTextFromResource($"{id}.txt");
            OnLoad();
        }

        // TODO: Change to OnInit as OnLoad implies being loaded in-game by the player.
        public abstract void OnLoad();

        public void Restore()
        {
            OnRestore();
        }

        public virtual void OnRestore()
        {
            // Do nothing
        }

        protected void AddExit(Id id, Id triggerEntityId, string[] commands)
        {
            Exit exit = new Exit();
            exit.Destination = id;
            exit.Commands.AddRange(commands);
            exit.TriggerEntityId = triggerEntityId;
            Exits.Add(exit);
        }

        protected void AddExit(Id id, string[] commands)
        {
            Exit exit = new Exit();
            exit.Destination = id;
            exit.Commands.AddRange(commands);
            Exits.Add(exit);
        }

        protected void AddExit(Id id, string command)
        {
            AddExit(id, new string[] { command });
        }

        public Exit FindExit(string exitCommand)
        {
            foreach (Exit exit in Exits)
                foreach (string command in exit.Commands)
                    if (command.ToLower() == exitCommand.ToLower())
                        return exit;
            return null;
        }

        public Item SpawnItem<T>() where T : Item, new()
        {
            T t = new T();
            Item item = t;
            Items.Add(item);
            return item;
        }

        public Item FindItem(string itemLabel)
        {
            foreach (Item item in Items)
                if (item.Labels.Contains(itemLabel.ToLower()))
                    return item;

            foreach (Item item in DroppedItems)
                if (item.Labels.Contains(itemLabel.ToLower()))
                    return item;

            foreach (CheckObject @object in Objects)
                foreach (Item item in @object.Items)
                    if (item.Labels.Contains(itemLabel.ToLower()))
                        return item;

            return null;
        }

        public bool RemoveItem(Item item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
                return true;
            }
            else if (DroppedItems.Contains(item))
            {
                DroppedItems.Remove(item);
                return true;
            }
            else
                foreach (CheckObject @object in Objects)
                    if (@object.Items.Contains(item))
                    {
                        @object.Items.Remove(item);
                        return true;
                    }

            return false;
        }

        public void SpawnEntity<T>() where T : Entity, new()
        {
            Entity entity = new T();
            Entities.Add(entity);
        }

        public Entity FindEntity(string searchLabel)
        {
            foreach (Entity entity in Entities)
                if (entity.Labels.Contains(searchLabel.ToLower()))
                    return entity;
            return null;
        }

        public Entity FindEntity(Id entityId)
        {
            foreach (Entity entity in Entities)
                if (entity.Id == entityId)
                    return entity;
            return null;
        }

        public CheckObject FindCheckObject(Id objectId)
        {
            foreach (CheckObject checkObject in Objects)
                if (checkObject.Id == objectId)
                    return checkObject;
            return null;
        }

        public CheckObject FindCheckObject(string objectName)
        {
            foreach (CheckObject checkObject in Objects)
                if (checkObject.Labels.Contains(objectName))
                    return checkObject;
            return null;
        }

        public CheckObject RegisterCheckObject<T>() where T : CheckObject, new()
        {
            CheckObject checkObject = new T();
            Objects.Add(checkObject);
            return checkObject;
        }
    }
}
