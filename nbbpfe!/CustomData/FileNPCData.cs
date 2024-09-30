using System.Collections.Generic;

namespace nbppfe.CustomData
{
    public class FileNPCData
    {
        public string name, pri_namekey = "PST_PRI_Name1", pri_descriptionkey = "PST_PRI_Description2";

        public Dictionary<string, int> sprites = new Dictionary<string, int>() {
            { "nameSprite", 100 }
        };

        public Dictionary<string, Dictionary<int, Dictionary<int, int>>> spritesSheets = new Dictionary<string, Dictionary<int, Dictionary<int, int>>>() {
           { "nameSpriteSheet", new Dictionary<int, Dictionary<int, int>> { { 100, new Dictionary<int, int> { { 1, 1 } } } } }
        };


        public Dictionary<string, string> audioClips = new Dictionary<string, string>() {
            { "audioExemple", "StartExtemples:Sfx, Vfx, Mfx" }
        };
    }
}
