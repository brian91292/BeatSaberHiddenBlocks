using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HiddenBlocks
{
    public class HiddenBlock : MonoBehaviour
    {
        private NoteController _note = null;
        private Camera _mainCamera = null;
        private bool _resetRenderers = false;

        public void Awake()
        {
            // Grab the NoteController object associated with the current GameObject
            _note = this.GetComponent<NoteController>();

            // Grab a reference to the main camera so we can use it to determine the position of the hmd
            _mainCamera = Camera.main;
        }

        public void Update()
        {
            if (Config.EnableHiddenBlocks)
            {
                if (_note)
                {
                    // When the note is within Plugin.blockHideDistance of the main camera
                    if ((!Plugin.NegativeNoteJumpSpeed && _note.transform.position.z <= _mainCamera.transform.position.z + Config.BlockHideDistance) || (Plugin.NegativeNoteJumpSpeed && _note.transform.position.z >= _mainCamera.transform.position.z - Config.BlockHideDistance))
                    {
                        // If we've already disabled our renderers, wait to do so again until they have been reset
                        if (!_resetRenderers)
                        {
                            // Iterate through each renderer and disable them all
                            _note.GetComponentsInChildren<Renderer>().ToList().ForEach(r => r.enabled = false);

                            _resetRenderers = true;
                        }
                    }
                    else if (_resetRenderers)
                    {
                        // Reenable some of the renderers, otherwise when the object gets reused the note/bomb will be invisible!
                        var renderers = _note.GetComponentsInChildren<Renderer>();
                        
                        renderers.First().enabled = true;

                        _resetRenderers = false;
                    }
                }
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
