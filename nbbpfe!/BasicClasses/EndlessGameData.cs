using System;
using System.Collections.Generic;
using System.Text;

namespace nbppfe.BasicClasses
{
    //Makes the game infinite, even if you die or end the game. you can save and try the same items
    public class EndlessGameData
    {
        public ItemObject[] items = new ItemObject[99];
        public ItemObject[] greenLockerItems = new ItemObject[99];
        public int ytps;
    }
}
