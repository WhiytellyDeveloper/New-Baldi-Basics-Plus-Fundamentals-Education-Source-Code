using BepInEx;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace nbppfe.FundamentalsManager
{
    public static class AssetsLoader
    {
        public static Texture2D CreateTexture(string textureName, string folder)
        {
            if (assetMan.ContainsKey(textureName))
                return assetMan.Get<Texture2D>(textureName);

            Texture2D texture = AssetLoader.TextureFromMod(BasePlugin.Instance, Path.Combine(folder, textureName + ".png"));
            assetMan.Add(textureName, texture);
            return texture;
        }

        public static Texture2D[] CreateTextures(string folder)
        {
            Texture2D[] textures = AssetLoader.TexturesFromMod(BasePlugin.Instance, folder, Path.Combine("Textures"));

            foreach (Texture2D tex in textures)
                assetMan.Add(Path.GetFileNameWithoutExtension(tex.name), tex);
            return textures;
        }

        public static Sprite CreateSprite(string spriteName, string folder, int pixelPerUnit)
        {
            Sprite sprite = AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromMod(BasePlugin.Instance, Path.Combine(folder, spriteName + ".png")), pixelPerUnit);
            assetMan.Add(spriteName, sprite);
            return sprite;
        }

        public static List<Sprite> CreateSprites(float pixelsPerUnit, string folder, bool addToAssetMan = true)
        {
            string[] files = Directory.GetFiles(Path.Combine(AssetLoader.GetModPath(BasePlugin.Instance), "Textures", folder));
            List<Sprite> sprites = new List<Sprite>();
            for (int i = 0; i < files.Length; i++)
            {
                if (addToAssetMan)
                    Debug.Log(Path.GetFileNameWithoutExtension(files[i]));
                sprites.Add(AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(files[i]), pixelsPerUnit));

                for (int j = 0; j < sprites.Count; j++)
                    sprites[i].name = Path.GetFileNameWithoutExtension(files[i]);

                if (addToAssetMan)
                    assetMan.AddRange(sprites);
            }
            return sprites;
        }

        public static SoundObject CreateSound(string soundName, string folder, string subtitleKey, SoundType type, Color color, int vauleMultiplier, params SubtitleTimedKey[] stk)
        {
            SoundObject sound = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(BasePlugin.Instance, Path.Combine(folder, soundName + ".wav")), subtitleKey, type, color);
            sound.additionalKeys = stk;
            if (subtitleKey == "")
            {
                sound.subtitle = false;
                sound.name = soundName;
            }

            sound.volumeMultiplier = vauleMultiplier;
            assetMan.Add(soundName, sound);
            return sound;
        }

        public static Color SetHexaColor(string hexa)
        {
            Color _color = Color.white;
            ColorUtility.TryParseHtmlString(hexa, out _color);
            return _color;
        }

        public static string LoadMidi(string midiName, string id)
        {
            string midi = null;
            midi = AssetLoader.MidiFromFile(midiName, id);
            assetMan.Add(midiName, midi);
            return midi;
        }

        public static T Get<T>(string name) {
            return assetMan.Get<T>(name);
        }

        public static AssetManager assetMan = new AssetManager();
    }
}
