﻿using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbbpfe.Enums;
using nbbpfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomItems
{
    public class ITM_BaseBall : Item, IItemPrefab
    {
        public void Setup()
        {
            var spriteBilboard = ObjectCreationExtensions.CreateSpriteBillboard(ItemMetaStorage.Instance.FindByEnum(EnumExtensions.GetFromExtendedName<Items>(CustomItemsEnum.Baseball.ToString())).value.itemSpriteLarge);
            spriteBilboard.transform.SetParent(transform);

            entity = gameObject.CreateEntity(0.5f, 0.5f, spriteBilboard.transform);
        }

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            transform.position = pm.transform.position;
            entity.Initialize(pm.ec, transform.position);
            direction = pm.transform.forward;

            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(FundamentalLoaderManager.GenericThrowSound);

            entity.OnEntityMoveInitialCollision += (hit) =>
                    Destroy(gameObject);
            
            return true;
        }

        private void Update()
        {
            entity.UpdateInternalMovement(direction * 100 * pm.ec.EnvironmentTimeScale);

            if (Physics.Raycast(transform.position, direction, out hit, 5, LayerMask.GetMask("Default", "Windows"))) //I don't care how bad it is, if it works it's good
            {
                if (hit.transform.GetComponent<Window>())
                {
                    Window component = hit.transform.GetComponent<Window>();
                    if (!(bool)component.ReflectionGetVariable("broken"))
                        component.Break(true);

                }
            }
        }
        

        public Entity entity;
        public Vector3 direction;
        private RaycastHit hit;
    }
}