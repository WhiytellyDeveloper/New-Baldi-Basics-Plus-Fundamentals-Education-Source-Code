using System;

namespace nbppfe.CustomData
{
    [Serializable]
    public class FileItemData
    {
        public string itemNameKey = "Itm_YourItmName", itemDescriptionKey = "Desc_YourItmDesc", postfixIconSmall = "_IconSmall", postfixIconLarge = "_IconLarge", pickupItemSoundName = "itemPickupSound";
        public bool autoUse = false, isFood = false, isDrink = false, isMultipleUse = false;
        public int price = 0, cost = 0;
        public string[] tags = ["none"];
    }
}
