﻿using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using nbppfe.Enums;
using nbppfe.Extensions;
using UnityEngine;

namespace nbppfe.BasicClasses.Extensions
{
    //Crispy+ compability; ok?
    public class WindowExtension : MonoBehaviour, IItemAcceptor
    {
        public void InsertItem(PlayerManager pm, EnvironmentController ec)
        {
        }

        public bool ItemFits(Items item)
        {
            return CustomItemsEnum.GenericHammer.ToItemEnum() == item && !(bool)GetComponent<Window>().ReflectionGetVariable("broken");
        }
    }
}
