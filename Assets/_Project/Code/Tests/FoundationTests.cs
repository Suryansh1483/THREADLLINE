using System;
using System.Linq;
using NUnit.Framework;
using Threadline.Editor;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Threadline.Tests.Editor
{
    public class FoundationTests
    {
        private const string AssetRoot = "Assets/_Project/";

        [TestCase("-Djdk.net.unixdomain.tmpdir=C:/Android/java-sockets", "C:/Android/java-sockets")]
        [TestCase("-Xmx4g -Djdk.net.unixdomain.tmpdir=\"C:/Java Sockets\"", "C:/Java Sockets")]
        [TestCase("-Dother.property=C:/Temp", null)]
        [TestCase(null, null)]
        public void SocketDirectoryUsesOnlyTheExplicitJdkOption(string options, string expected)
        {
            Assert.That(AndroidGradleSocketDirectory.FindConfiguredDirectory(options), Is.EqualTo(expected));
        }

        [Test]
        public void AndroidBuildTempOverrideRestoresTheOriginalEnvironment()
        {
            var previousTemp = Environment.GetEnvironmentVariable("TEMP");
            var previousTmp = Environment.GetEnvironmentVariable("TMP");
            var directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ThreadlineSocket-" + Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(directory);
            try
            {
                AndroidGradleSocketDirectory.Apply(directory);
                Assert.That(Environment.GetEnvironmentVariable("TEMP"), Is.EqualTo(directory));
                Assert.That(Environment.GetEnvironmentVariable("TMP"), Is.EqualTo(directory));
            }
            finally
            {
                AndroidGradleSocketDirectory.Restore();
                System.IO.Directory.Delete(directory);
            }
            Assert.That(Environment.GetEnvironmentVariable("TEMP"), Is.EqualTo(previousTemp));
            Assert.That(Environment.GetEnvironmentVariable("TMP"), Is.EqualTo(previousTmp));
        }

        [Test]
        public void EveryQualityTierHasAValidUrpRenderer()
        {
            string[] names = { "PC High", "PC Medium", "Android High", "Android Medium", "Android Low" };
            CollectionAssert.AreEqual(names, QualitySettings.names);
            foreach (var name in names)
            {
                var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(
                    AssetRoot + "Settings/Rendering/" + name.Replace(" ", "") + "_RPAsset.asset");
                Assert.That(pipeline, Is.Not.Null, name);
                var renderers = new SerializedObject(pipeline).FindProperty("m_RendererDataList");
                Assert.That(renderers.arraySize, Is.GreaterThan(0), name);
                Assert.That(renderers.GetArrayElementAtIndex(0).objectReferenceValue, Is.Not.Null, name);
                Assert.That(pipeline.useSRPBatcher, Is.True, name);
            }
            Assert.That(GraphicsSettings.defaultRenderPipeline, Is.InstanceOf<UniversalRenderPipelineAsset>());
        }

        [Test]
        public void ProductionSceneListStartsAtBootstrapAndExcludesRenderTest()
        {
            CollectionAssert.AreEqual(new[]
            {
                AssetRoot + "Scenes/Bootstrap/00_Bootstrap.unity",
                AssetRoot + "Scenes/MainMenu/01_MainMenu.unity",
                AssetRoot + "Scenes/Gameplay/02_Gameplay.unity"
            }, EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray());
        }

        [Test]
        public void GameplayActionsHaveKeyboardAndGamepadBindingsAndPreserveUi()
        {
            var asset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(AssetRoot + "Settings/Input/ThreadlineInput.inputactions");
            Assert.That(asset, Is.Not.Null);
            Assert.That(asset.FindActionMap("UI", false), Is.Not.Null);
            var gameplay = asset.FindActionMap("Gameplay", true);
            string[] names = { "MoveLeft", "MoveRight", "Jump", "Slide", "RealitySwitch", "Pause" };
            CollectionAssert.AreEquivalent(names, gameplay.actions.Select(action => action.name).ToArray());
            foreach (var action in gameplay.actions)
            {
                Assert.That(action.type, Is.EqualTo(InputActionType.Button), action.name);
                Assert.That(action.bindings.Any(binding => binding.path.StartsWith("<Keyboard>")), Is.True, action.name);
                Assert.That(action.bindings.Any(binding => binding.path.StartsWith("<Gamepad>")), Is.True, action.name);
            }
        }

        [Test]
        public void FoundationScenesHaveNoMissingScriptsOrMaterials()
        {
            foreach (var guid in AssetDatabase.FindAssets("t:Scene", new[] { AssetRoot + "Scenes" }))
            {
                var scene = EditorSceneManager.OpenPreviewScene(AssetDatabase.GUIDToAssetPath(guid));
                try
                {
                    foreach (var root in scene.GetRootGameObjects())
                    {
                        foreach (var transform in root.GetComponentsInChildren<Transform>(true))
                            Assert.That(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(transform.gameObject), Is.Zero, transform.name);
                        foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
                        {
                            foreach (var material in renderer.sharedMaterials)
                            {
                                Assert.That(material, Is.Not.Null, renderer.name);
                                Assert.That(material.shader.name, Does.StartWith("Universal Render Pipeline/"), material.name);
                            }
                        }
                    }
                    Assert.That(scene.GetRootGameObjects().Sum(root => root.GetComponentsInChildren<Camera>(true).Length), Is.EqualTo(1), scene.path);
                }
                finally
                {
                    EditorSceneManager.ClosePreviewScene(scene);
                }
            }
        }

        [Test]
        public void AndroidFoundationUsesArm64Il2CppAndPlaceholderIdentity()
        {
            Assert.That(PlayerSettings.GetScriptingBackend(NamedBuildTarget.Android), Is.EqualTo(ScriptingImplementation.IL2CPP));
            Assert.That(PlayerSettings.Android.targetArchitectures, Is.EqualTo(AndroidArchitecture.ARM64));
            Assert.That(PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android), Is.EqualTo("com.example.threadline"));
            Assert.That(PlayerSettings.defaultInterfaceOrientation, Is.EqualTo(UIOrientation.LandscapeLeft));
            Assert.That(PlayerSettings.colorSpace, Is.EqualTo(ColorSpace.Linear));
            Assert.That((int)PlayerSettings.Android.minSdkVersion, Is.GreaterThanOrEqualTo(26));
        }

        [Test]
        public void ShaderGraphAndUrpResolveToTheSameVersion()
        {
            var packages = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages();
            Assert.That(packages.Single(package => package.name == "com.unity.shadergraph").version,
                Is.EqualTo(packages.Single(package => package.name == "com.unity.render-pipelines.universal").version));
        }

        [Test]
        public void FoundationTestsAreExcludedFromPlayerAssemblies()
        {
            Assert.That(CompilationPipeline.GetAssemblies(AssembliesType.Player)
                .Any(assembly => assembly.name == "Threadline.Foundation.EditorTests" || assembly.name == "Threadline.Editor"), Is.False);
        }
    }
}
