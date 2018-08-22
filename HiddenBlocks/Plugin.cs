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


namespace HiddenBlocks {
    public class Plugin : IPlugin {
        public string Name => "HiddenBlocks";
        public string Version => "1.0.0";

        public static float blockHideDistance = 4.5f;
        public static bool enableHiddenBlocks = true;
        public static bool shouldWriteConfig = false;
        
        private GameOptionToggle _hiddenBlocksToggle = null;
        private GameHooks _hiddenMod;
        
        public void OnApplicationStart() {
            this._hiddenMod = new GameObject().AddComponent<GameHooks>();

            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            
            // Initialize our settings from modprefs.ini
            ReadConfig();
            WriteConfig();
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene) {
            // Setup our custom toggle
            if (scene.name == "Menu") {
                AddModMenuButton();
            }

           Utilities.Log($"Scene: {scene.name} has index {scene.buildIndex}");
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {

        }

        public void OnApplicationQuit() {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level) {
        }

        public void OnLevelWasInitialized(int level) {
        }
        
        public void OnUpdate() {
            // If the user toggled hidden blocks on/off in the menu, update it in the config file.
            if (shouldWriteConfig) {
                WriteConfig();
            }
        }

        public void OnFixedUpdate() {
        }

        public void AddModMenuButton() {
            Sprite hiddenBlocksIcon = Utilities.LoadNewSprite(BuiltInResources.HiddenBlocksIcon);

            MainMenuViewController _mainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().First();
            GameplayOptionsViewController _gameplayOptionsViewController = Resources.FindObjectsOfTypeAll<GameplayOptionsViewController>().First();
            Transform switches = _gameplayOptionsViewController.transform.Cast<RectTransform>().ToArray().ToList().FirstOrDefault(c => c.name == "Switches");
            _gameplayOptionsViewController.transform.Find("InfoText").gameObject.SetActive(false);
            RectTransform container = (RectTransform)switches.transform.Find("Container");
            //container.sizeDelta = new Vector2(container.sizeDelta.x, container.sizeDelta.y + 14f);
            //container.Translate(new Vector3(0, -0.1f, 0));
            Transform noEnergy = switches.transform.GetComponentsInChildren<Component>().ToList().FirstOrDefault(comp => comp.name == "NoEnergy").transform;  //container.Find("NoEnergy");

            _hiddenBlocksToggle = new GameOptionToggle(container.gameObject, noEnergy.gameObject, "HiddenBlocks", hiddenBlocksIcon, "Hidden Blocks", enableHiddenBlocks);
        }

        private void ReadConfig() {
            // Read our config options, if they already exist
            enableHiddenBlocks = ModPrefs.GetBool(Name, "Enabled", true);
            blockHideDistance = ModPrefs.GetFloat(Name, "BlockHideDistance", 4.5f);

            // Don't allow the user to set lower values, as it can be used to gain an unfair advantage by seeing through notes
            if (blockHideDistance < 4.5f) {
                blockHideDistance = 4.5f;
            }
        }

        private void WriteConfig() {
            // Write the updated values to the config file in case we haven't already
            ModPrefs.SetBool(Name, "Enabled", enableHiddenBlocks);
            ModPrefs.SetFloat(Name, "BlockHideDistance", blockHideDistance);

            shouldWriteConfig = false;
        }
    }
}
