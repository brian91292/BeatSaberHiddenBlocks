using Harmony;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;


namespace HiddenBlocks
{
    [HarmonyPatch(typeof(BeatmapObjectSpawnController))]
    [HarmonyPatch("SetNoteControllerEventCallbacks", MethodType.Normal)]
    public class BeatmapObjectSpawnControllerDetours
    {
        public static void Postfix(NoteController noteController)
        {
            if (Config.EnableHiddenBlocks && !noteController.GetComponent<HiddenBlock>())
                noteController.gameObject.AddComponent<HiddenBlock>();
        }
    }
}
