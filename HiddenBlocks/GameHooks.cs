using PlayHooky;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using System.Diagnostics;

namespace HiddenBlocks {
    public class BeatmapObjectSpawnControllerDetours {
        public static void SetNoteControllerEventCallbacks(BeatmapObjectSpawnController t, NoteController noteController) {
            if (Plugin.enableHiddenBlocks)
                noteController.gameObject.AddComponent<HiddenBlock>();

            noteController.noteDidStartJumpEvent += t.HandleNoteDidStartJump;
            noteController.noteDidFinishJumpEvent += t.HandleNoteDidFinishJump;
            noteController.noteWasCutEvent += t.HandleNoteWasCut;
            noteController.noteWasMissedEvent += t.HandleNoteWasMissed;
        }
    }

    public class GameHooks : MonoBehaviour {
        private HookManager hookManager;
        private Dictionary<string, MethodInfo> hooks;

        private void Awake() {
            UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object)((UnityEngine.Component)this).gameObject);
            this.hookManager = new HookManager();
            this.hooks = new Dictionary<string, MethodInfo>();
            
            this.Hook("BeatmapObjectSpawnController", typeof(BeatmapObjectSpawnController).GetMethod("SetNoteControllerEventCallbacks"), typeof(BeatmapObjectSpawnControllerDetours).GetMethod("SetNoteControllerEventCallbacks"));
        }

        private void OnDestroy() {
            foreach (string key in this.hooks.Keys)
                this.UnHook(key);
        }

        private bool Hook(string key, MethodInfo target, MethodInfo hook) {
            if (this.hooks.ContainsKey(key))
                return false;
            try {
                this.hooks.Add(key, target);
                this.hookManager.Hook(target, hook);
                Utilities.Log($"{key} hooked!");
                return true;
            }
            catch (Win32Exception ex) {
                Utilities.Log($"Unrecoverable Windows API error: {(object)ex}");
                return false;
            }
            catch (Exception ex) {
                Utilities.Log($"Unable to hook method, : {(object)ex}");
                return false;
            }
        }

        private bool UnHook(string key) {
            MethodInfo original;
            if (!this.hooks.TryGetValue(key, out original))
                return false;
            this.hookManager.Unhook(original);
            return true;
        }
    }
}
