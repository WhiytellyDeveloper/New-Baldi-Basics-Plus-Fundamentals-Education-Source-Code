using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using nbbpfe.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.BasicClasses.Extensions
{
    //Crispy+ compability; ok?
    public class WindowExtension : MonoBehaviour, IItemAcceptor
    {
        public void InsertItem(PlayerManager pm, EnvironmentController ec)
        {
        }

        public bool ItemFits(Items item) {
            return EnumExtensions.GetFromExtendedName<Items>(CustomItemsEnum.GenericHammer.ToString()) == item && !(bool)GetComponent<Window>().ReflectionGetVariable("broken");
        }
    }
}
