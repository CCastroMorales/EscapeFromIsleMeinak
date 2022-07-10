using EscapeFromIsleMeinak;
using EscapeFromIsleMeinak.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScriptingTests
{
    public class TestScene : Scene
    {
        public override void OnLoad()
        {
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_TEST;
        }
    }

    [TestClass]
    public class ScriptingTest
    {
        [TestMethod]
        public void TestSceneTitle()
        {
            string script = @"
            
            scene.title Test scene

            ";

            Scene scene = new TestScene();

            Scripting scripting = new Scripting();
            scripting.Scene = scene;
            scripting.Parse(script);

            Assert.AreEqual("Test scene", scene.Title);
        }

        [TestMethod]
        public void TestSceneAutoEnd()
        {
            string script = @"
            
            scene.title AutoEnd test scene
            scene.autoEnd 1

            ";

            Scene scene = new TestScene();

            Scripting scripting = new Scripting();
            scripting.Scene = scene;
            scripting.Parse(script);

            Assert.AreEqual("AutoEnd test scene", scene.Title);
            Assert.AreEqual(true, scene.AutoEnd);
        }

        [TestMethod]
        public void TestSceneInitialScript()
        {
            string script = @"
            
            scene.title Test scene 2
            scene.initialScriptBegin

            Hello world!

            scene.initialScriptEnd

            ";

            Scene scene = new TestScene();

            Scripting scripting = new Scripting();
            scripting.Scene = scene;
            scripting.Parse(script);

            Assert.AreEqual("Test scene 2", scene.Title);
            Assert.AreEqual(0, scene.Script.Count);
            Assert.AreEqual(1, scene.InitialScript.Count);
            Assert.AreEqual("Hello world!", scene.InitialScript[0]);
        }

        [TestMethod]
        public void TestSceneScript()
        {
            string script = @"
            
            scene.title Test scene 3
            scene.initialScriptBegin
            
            This is the initial script.
            
            scene.initialScriptEnd
            scene.scriptBegin

            This is the regular script.

            This script consists of three lines.

            scene.scriptEnd

            ";

            Scene scene = new TestScene();

            Scripting scripting = new Scripting();
            scripting.Scene = scene;
            scripting.Parse(script);

            Assert.AreEqual("Test scene 3", scene.Title);
            Assert.AreEqual(1, scene.InitialScript.Count);
            Assert.AreEqual(3, scene.Script.Count);
            Assert.AreEqual("This is the initial script.", scene.InitialScript[0]);
            Assert.AreEqual("This is the regular script.", scene.Script[0]);
            Assert.AreEqual("", scene.Script[1]);
            Assert.AreEqual("This script consists of three lines.", scene.Script[2]);
        }
    }
}
