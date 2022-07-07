using Newtonsoft.Json;
using ScriptLibrary;
using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ScriptEditor
{
    public partial class Form1 : Form
    {
        public string Filename { get; set; }
        public Script Script { get; set; }
        public TreeNode NodeScenes { get; set; }

        public Form1()
        {
            InitializeComponent();

            textBox1.Font = new Font("Segoe UI", 12.0f);
            textBox2.Font = textBox1.Font;

            ReadCommandLineArguments();
            SetupTreeNodes();
            openToolStripMenuItem_Click(null, new EventArgs());
        }

        private void SetupTreeNodes()
        {
            NodeScenes = treeView1.Nodes.Add("Scenes");
            NodeScenes.ImageIndex = 1;
            NodeScenes.SelectedImageIndex = 1;
            NodeScenes.ContextMenuStrip = contextMenuStrip1;
        }

        private void ReadCommandLineArguments()
        {
            string[] args = Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "+filename" && i < args.Length - 1)
                    Filename = args[i + 1];
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string json = File.ReadAllText(Filename);
            Script = JsonConvert.DeserializeObject<Script>(json);
            BuildUIFromScript(Script);
            Analyze();
        }

        private void BuildUIFromScript(Script script)
        {
            NodeScenes.Nodes.Clear();

            foreach (Scene scene in script.Scenes)
            {
                TreeNode sceneNode = NodeScenes.Nodes.Add($"{scene.SceneId}");
                sceneNode.ImageIndex = 1;
                sceneNode.SelectedImageIndex = 1;
                sceneNode.ContextMenuStrip = contextMenuStrip1;
                sceneNode.Tag = scene;

                TreeNode exitParentNode = sceneNode.Nodes.Add("Exits");
                exitParentNode.ImageIndex = 2;
                exitParentNode.SelectedImageIndex = 2;

                foreach (Exit exit in scene.Exits)
                {
                    TreeNode exitNode = exitParentNode.Nodes.Add(exit.Name);
                    TreeNode exitSceneIdNode = exitNode.Nodes.Add(exit.Scene);

                    exitNode.ImageIndex = 3;
                    exitNode.SelectedImageIndex = 3;

                    exitSceneIdNode.ImageIndex = 1;
                    exitSceneIdNode.SelectedImageIndex = 1;
                }
            }

            NodeScenes.Expand();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            /*if (e.Node.Parent == NodeScenes)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Scene scene = (Scene)e.Node.Tag;
                    ShowSceneTextInTextbox(scene);
                } 
                else if (e.Button == MouseButtons.Right)
                {
                }

            }*/
        }

        private void ShowSceneTextInTextbox(Scene scene)
        {
            textBox2.Tag = null;
            textBox2.Text = scene.Title;
            textBox2.Tag = scene;

            textBox1.Tag = null;
            textBox1.Clear();
            textBox1.Tag = scene;
            textBox1.Lines = scene.text.ToArray();
        }

        private void Analyze()
        {
            listView1.Items.Clear();

            foreach (TreeNode node in NodeScenes.Nodes)
            {
                Scene scene = (Scene)node.Tag;
                
                if (scene.SceneId.Trim() == "")
                {
                    string msg = $"CRITICAL: Scene lacks ID.";
                    string sceneId = $"{scene.SceneId}";
                    Debug.WriteLine(msg);

                    var item = new ListViewItem(new[] { msg, sceneId });
                    listView1.Items.Add(item);
                }

                // Analyze exits
                foreach (Exit exit in scene.Exits)
                {
                    if (!Script.SceneExists(exit.Scene))
                    {
                        string msg = $"ERROR: Scene exit {exit.Scene} doesn't exist.";
                        string sceneId = $"{scene.SceneId}";
                        Debug.WriteLine(msg);

                        var item = new ListViewItem(new[] {msg, sceneId});
                        listView1.Items.Add(item);
                    }
                }

                // Analyze title
                if ((Scene)NodeScenes.Nodes[0].Tag != scene && scene.Title.Trim() == "")
                {
                    string msg = $"ERROR: Scene has no title.";
                    string sceneId = $"{scene.SceneId}";
                    Debug.WriteLine(msg);

                    var item = new ListViewItem(new[] { msg, sceneId });
                    listView1.Items.Add(item);
                }

                // Analyze exits
                if (scene.Exits.Count == 0)
                {
                    string msg = $"ERROR: Scene has no exits.";
                    string sceneId = $"{scene.SceneId}";
                    Debug.WriteLine(msg);

                    var item = new ListViewItem(new[] { msg, sceneId });
                    listView1.Items.Add(item);
                }

                // Analyze texts
                string text = "";
                foreach (string t in scene.text)
                {
                    text += t;
                    text.Trim();
                }

                if (text == "")
                {
                    string msg = $"ERROR: Scene lacks text.";
                    string sceneId = $"{scene.SceneId}";
                    Debug.WriteLine(msg);

                    var item = new ListViewItem(new[] { msg, sceneId });
                    listView1.Items.Add(item);
                }

                // Analyze if inaccessible
                // Check if this is the first scene, if so don't do the accessibility test.
                bool accessible = (Scene)NodeScenes.Nodes[0].Tag == scene ? true : false;
                foreach (Scene compiledScene in CompiledScenes())
                {
                    foreach (Exit exit in compiledScene.Exits)
                    {
                        if (exit.Scene == scene.SceneId)
                        {
                            accessible = true;
                            break;
                        }
                    }
                }

                if (!accessible)
                {
                    string msg = $"ERROR: Scene {scene.SceneId} is not accessible.";
                    string sceneId = $"{scene.SceneId}";
                    Debug.WriteLine(msg);

                    var item = new ListViewItem(new[] { msg, sceneId });
                    listView1.Items.Add(item);
                }
            }

            listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private Scene[] CompiledScenes()
        {
            List<Scene> scenes = new List<Scene>();
            foreach (TreeNode node in NodeScenes.Nodes)
                scenes.Add((Scene)node.Tag);
            return scenes.ToArray();
        }

        private void addSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string guid = Guid.NewGuid().ToString();

            Scene freshScene = new Scene();
            freshScene.SceneId = $"SCENE_{guid}";

            TreeNode node = new TreeNode(freshScene.SceneId);
            node.ContextMenuStrip = contextMenuStrip1;
            node.Tag = freshScene;
            NodeScenes.Nodes.Add(node);
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Parent == NodeScenes)
            {
                Scene scene = (Scene)e.Node.Tag;
                scene.SceneId = e.Label;
                Analyze();
            }
            else if (e.Node.Parent.Text == "Exits")
            {
                e.Node.Text = e.Label;

                TreeNode parentExitsNode = e.Node.Parent;

                Scene scene = (Scene)parentExitsNode.Parent.Tag;
                scene.Exits.Clear();

                foreach (TreeNode node in parentExitsNode.Nodes)
                {
                    Exit exit = new Exit();
                    exit.Name = node.Text;
                    exit.Scene = node.Nodes[0].Text;
                    scene.Exits.Add(exit);
                }

                Analyze();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Script script = CompileScript();
            string json = JsonConvert.SerializeObject(script, Formatting.Indented);
            //Debug.WriteLine(json);
            File.WriteAllText(Filename, json);
            Analyze();
        }

        private Script CompileScript()
        {
            Script script = Script;
            script.Scenes.Clear();

            foreach (TreeNode node in NodeScenes.Nodes)
            {
                Scene sceneFromNode = (Scene)node.Tag;
                script.Scenes.Add(sceneFromNode);
            }

            return script;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Scene scene = (Scene)textBox1.Tag;
            
            // Check for null or else this will trigger when changing scenes.
            if (scene != null)
            {
                scene.text.Clear();
                scene.text.AddRange(textBox1.Lines);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Scene scene = (Scene)textBox2.Tag;

            // Check for null or else this will trigger when changing scenes.
            if (scene != null)
                scene.Title = textBox2.Text;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Scene scene = (Scene)e.Node.Tag;
            if (scene != null)
                ShowSceneTextInTextbox(scene);
        }

        private void deleteSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Parent == NodeScenes)
            {
                Scene scene = (Scene)treeView1.SelectedNode.Tag;
                string message = $"Delete scene \"{scene.SceneId}\" with title \"{scene.Title}\"?";
                
                if (MessageBox.Show(message, "Delete scene", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    NodeScenes.Nodes.Remove(treeView1.SelectedNode);
                    Analyze();
                }
            }
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.ExpandAll();
        }

        /* Drag and drop */
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Copy);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            Point point = treeView1.PointToClient(new Point(e.X, e.Y));
            treeView1.SelectedNode = treeView1.GetNodeAt(point);
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            Point point = treeView1.PointToClient(new Point(e.X, e.Y));
            TreeNode dropNode = treeView1.GetNodeAt(point);
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (draggedNode != dropNode && dropNode.Text == "Exits")
            {
                string sceneId = draggedNode.Text;
                string command = draggedNode.Text.ToLower();

                TreeNode commandNode = dropNode.Nodes.Add(command);
                TreeNode scenePointerNode = commandNode.Nodes.Add(sceneId);
                dropNode.ExpandAll();

                commandNode.ImageIndex = 3;
                commandNode.SelectedImageIndex = 3;
                scenePointerNode.ImageIndex = 1;
                scenePointerNode.SelectedImageIndex = 1;

                Analyze();
            }
        }
    }
}
