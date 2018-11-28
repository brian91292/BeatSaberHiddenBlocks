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

namespace HiddenBlocks
{
    public class Plugin : IPlugin
    {
        public string Name => "HiddenBlocks";
        public string Version => "1.2.1";

        public static Plugin Instance;
        public static bool NegativeNoteJumpSpeed = false;

        private Sprite _hiddenBlocksIcon = null;
        private GameHooks _gameHooks = null;
        private StandardLevelSceneSetupDataSO _mainGameSceneSetupData = null;

        public void OnApplicationStart()
        {
            Instance = this;

            _gameHooks = new GameObject().AddComponent<GameHooks>();
            Config.Read();
            Config.Write();

            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            if (scene.name == "GameCore")
            {
                NegativeNoteJumpSpeed = _mainGameSceneSetupData.difficultyBeatmap.noteJumpMovementSpeed < 0;
            }
        }



        public void AddModMenuButton()
        {
            if (_hiddenBlocksIcon == null)
                _hiddenBlocksIcon = UIUtilities.LoadSpriteFromResources("HiddenBlocks.Resources.HiddenIcon.png");
            
            var toggle = GameplaySettingsUI.CreateToggleOption("Hidden Blocks", "Makes notes and bombs invisible as they approach your position.", _hiddenBlocksIcon);
            toggle.AddConflict("Disappearing Arrows");

            toggle.GetValue = Config.EnableHiddenBlocks;
            toggle.OnToggle += ((bool e) =>
            {
                Utilities.Log(e ? "Enabled" : "Disabled");
                Config.EnableHiddenBlocks = e;
                Config.WritePending = true;
            });

            Utilities.Log("Added test button!");
        }
        

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "Menu")
                AddModMenuButton();
        }

        public void OnApplicationQuit()
        {
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

            if (_mainGameSceneSetupData == null)
                _mainGameSceneSetupData = Resources.FindObjectsOfTypeAll<StandardLevelSceneSetupDataSO>().FirstOrDefault();
        }

        public void OnFixedUpdate()
        {
        }
    }
}
