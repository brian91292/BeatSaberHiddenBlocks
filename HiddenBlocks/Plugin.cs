//using IllusionPlugin;
using System;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRUIControls;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using IllusionPlugin;
using UnityEngine.UI;
using CustomUI;
using CustomUI.GameplaySettings;
using CustomUI.Utilities;
using CustomUI.MenuButton;
using CustomUI.Settings;
using Harmony;
using CustomUI.BeatSaber;
using HMUI;
using static VRUI.FlowCoordinator;
using IllusionInjector;
using VRUI;

namespace HiddenBlocks
{
    public class Plugin : IPlugin
    {
        public string Name => "HiddenBlocks";
        public string Version => "1.2.4";

        public static Plugin Instance;
        public static bool NegativeNoteJumpSpeed = false;

        private Sprite _hiddenBlocksIcon = null;
        private HarmonyInstance _harmony;

        public void OnApplicationStart()
        {
            Instance = this;

            Config.Read();
            Config.Write();

            _harmony = HarmonyInstance.Create("com.brian91292.beatsaber.hiddenblocks");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            if (scene.name == "GameCore")
            {
                NegativeNoteJumpSpeed = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpMovementSpeed < 0;
            }
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "MenuCore")
                AddModMenuButton();
        }

        public void AddModMenuButton()
        {
            if (_hiddenBlocksIcon == null)
                _hiddenBlocksIcon = UIUtilities.LoadSpriteFromResources("HiddenBlocks.Resources.HiddenIcon.png");

            var toggle = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersLeft, "Hidden Blocks", "Makes notes and bombs invisible as they approach your position.", UIUtilities.EditIcon);
            toggle.AddConflict("Disappearing Arrows");
            toggle.AddConflict("Ghost Notes");

            toggle.GetValue = Config.EnableHiddenBlocks;
            toggle.OnToggle += ((bool e) =>
            {
                Utilities.Log(e ? "Enabled" : "Disabled");
                Config.EnableHiddenBlocks = e;
                Config.WritePending = true;
            });
        }
        
        public void OnApplicationQuit()
        {
            _harmony.UnpatchAll("com.brian91292.beatsaber.hiddenblocks");

            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
            if (Config.WritePending)
            {
                Config.Write();
            }
        }

        public void OnFixedUpdate()
        {
        }
    }
}
