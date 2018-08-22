using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HiddenBlocks {
    class Utilities {
        public static void Log(string msg) {
        #if DEBUG
            using (StreamWriter w = File.AppendText($"HiddenBlocks.log")) {
                w.WriteLine("{0}", msg);
            }
        #endif
        }

        public static Texture2D LoadTexture(string FilePath) {
            if (File.Exists(FilePath)) {
                return LoadTexture(File.ReadAllBytes(FilePath));
            }
            return null;
        }

        public static Texture2D LoadTexture(byte[] file) {
            if (file.Count() > 0) {
                Texture2D Tex2D = new Texture2D(2, 2); 
                if (Tex2D.LoadImage(file)) {
                    return Tex2D; 
                }
            }
            return null; 
        }

        public static Sprite LoadNewSprite(Texture2D SpriteTexture, float PixelsPerUnit = 100.0f) {
            if (SpriteTexture) {
                return Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);
            }
            return null;
        }

        public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f) {
            return LoadNewSprite(LoadTexture(FilePath), PixelsPerUnit);
        }

        public static Sprite LoadNewSprite(byte[] image, float PixelsPerUnit = 100.0f) {
            return LoadNewSprite(LoadTexture(image), PixelsPerUnit);
        }

    }
}
