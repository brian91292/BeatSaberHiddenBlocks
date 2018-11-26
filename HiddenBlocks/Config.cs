using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IllusionPlugin;

namespace HiddenBlocks {
    class Config {
        public static float BlockHideDistance = 4.5f;
        public static bool EnableHiddenBlocks = true;
        public static bool WritePending = false;

        public static void Read() {
            // Read our config options, if they already exist
            EnableHiddenBlocks = ModPrefs.GetBool(Plugin.Instance.Name, "Enabled", false);
            BlockHideDistance = ModPrefs.GetFloat(Plugin.Instance.Name, "BlockHideDistance", 4.5f);

            // Don't allow the user to set lower values, as it can be used to gain an unfair advantage by seeing through notes
            if (BlockHideDistance < 4.5f) {
                BlockHideDistance = 4.5f;
            }
        }

        public static void Write() {
            // Write the updated values to the config file in case we haven't already
            ModPrefs.SetBool(Plugin.Instance.Name, "Enabled", EnableHiddenBlocks);
            ModPrefs.SetFloat(Plugin.Instance.Name, "BlockHideDistance", BlockHideDistance);

            WritePending = false;
        }
    }
}
