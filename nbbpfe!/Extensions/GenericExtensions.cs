using MTM101BaldAPI;
using MTM101BaldAPI.Components;
using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbppfe.Enums;
using nbppfe.FundamentalsManager;
using PixelInternalAPI.Classes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.Extensions
{
    public static class GenericExtensions
    {
        public static BoxCollider AddBoxCollider(this GameObject g, Vector3 center, Vector3 size, bool isTrigger)
        {
            var c = g.AddComponent<BoxCollider>();
            c.center = center;
            c.size = size;
            c.isTrigger = isTrigger;
            return c;
        }

        public static GameCamera GetPlayerCamera(this PlayerManager player)
        {
            return Singleton<CoreGameManager>.Instance.GetCamera(player.playerNumber);
        }

        public static Items ToItemEnum(this CustomItemsEnum itemEnum)
        {
            return EnumExtensions.GetFromExtendedName<Items>(itemEnum.ToString());
        }

        public static ItemObject ToItem(this Items itemEnum)
        {
            return ItemMetaStorage.Instance.FindByEnum(itemEnum).value;
        }

        public static ItemObject ToItem(this CustomItemsEnum itemEnum)
        {
            return ItemMetaStorage.Instance.FindByEnumFromMod(itemEnum.ToItemEnum(), BasePlugin.Instance.Info).value;
        }

        public static RoomCategory ToRoomEnum(this CustomRoomsEnum roomEnum)
        {
            RoomCategory category = RoomCategory.Null;

            if (!enums.Contains(roomEnum.ToString()))
                EnumExtensions.ExtendEnum<RoomCategory>(roomEnum.ToString());

            category = EnumExtensions.GetFromExtendedName<RoomCategory>(roomEnum.ToString());
            return category;
        }

        public static RoomAssetMeta ToRoom(this RoomCategory roomEnum)
        {
            return RoomAssetMetaStorage.Instance.Get(roomEnum);
        }

        public static RoomAssetMeta ToRoom(this CustomRoomsEnum roomEnum)
        {
            return RoomAssetMetaStorage.Instance.Get(roomEnum.ToRoomEnum());
        }

        public static void ResizeCollider(this SpriteRenderer spriteRenderer, Collider colliderComponent)
        {
            Vector3 spriteSize = spriteRenderer.bounds.size;
            if (colliderComponent is BoxCollider)
                ((BoxCollider)colliderComponent).size = spriteSize;

            else if (colliderComponent is CapsuleCollider)
            {
                CapsuleCollider capsuleCollider = (CapsuleCollider)colliderComponent;
                float radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2f;
                float height = spriteSize.z;
                capsuleCollider.radius = radius;
                capsuleCollider.height = height;
            }
            else if (colliderComponent is SphereCollider)
            {
                float radius = Mathf.Max(spriteSize.x, Mathf.Max(spriteSize.y, spriteSize.z)) / 2f;
                ((SphereCollider)colliderComponent).radius = radius;
            }

            else
                Debug.LogError("Collider type is not supported by this script!");
        }


        public static float GetSpriteSize(this SpriteRenderer spriteRenderer)
        {
            Vector3 spriteSize = spriteRenderer.bounds.size;
            float radius = Mathf.Max(spriteSize.x, Mathf.Max(spriteSize.y, spriteSize.z)) / 2f;
            return radius;
        }

        public static float GetSpriteSize(this Sprite sprite)
        {
            Vector3 spriteSize = sprite.bounds.size;
            float radius = Mathf.Max(spriteSize.x, Mathf.Max(spriteSize.y, spriteSize.z)) / 2f;
            return radius;
        }

        public static void SetSpeed(this Navigator navigator, int speed, int maxSpeed)
        {
            navigator.SetSpeed(speed);
            navigator.maxSpeed = maxSpeed;
        }

        public static float GetDistanceFrom(this Vector3 pos1, Vector3 pos2)
        {
            return Vector3.Distance(pos1, pos2);
        }

        public static T Duplicate<T>(this T original) where T : ScriptableObject
        {
            if (original == null)
                return null;
            T novoObjeto = ScriptableObject.CreateInstance<T>();
            novoObjeto.CopyFrom(original);
            return novoObjeto;
        }

        private static void CopyFrom<T>(this T novoObjeto, T original) where T : ScriptableObject
        {
            var type = typeof(T);
            var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
                field.SetValue(novoObjeto, field.GetValue(original));
        }

        public static CustomSpriteAnimator AddAnimatorToSprite<T>(this SpriteRenderer renderer) where T : CustomSpriteAnimator
        {
            var animator = renderer.gameObject.AddComponent<T>();
            animator.spriteRenderer = renderer;
            return animator;
        }

        public static T ToWeighted<T, A>(this object obj, int weighted)
            where T : WeightedSelection<A>, new()
            where A : class
        {
            var castedObject = obj as A;

            if (castedObject == null)
                throw new InvalidCastException($"Não foi possível converter o objeto para o tipo {typeof(A).Name}.");

            T weightedSelection = new T
            {
                selection = castedObject,
                weight = weighted
            };

            return weightedSelection;
        }

        public static T CatchRandomItem<T>(this T[] array) where T : class
        {
            return array[UnityEngine.Random.RandomRangeInt(0, array.Length)];
        }

        public static T CatchRandomItem<T>(this List<T> array) where T : class
        {
            return array[UnityEngine.Random.RandomRangeInt(0, array.Count)];
        }

        public static void ApplyOverlay(this Texture2D baseTex, Texture2D overlayTex)
        {
            for (int x = 0; x < overlayTex.width; x++)
            {
                for (int y = 0; y < overlayTex.height; y++)
                {
                    Color overlayPixel = overlayTex.GetPixel(x, y);
                    if (overlayPixel.a > 0)
                        baseTex.SetPixel(x, y, overlayPixel);   
                }
            }

            baseTex.Apply();
        }


        //Made By Pixel Guy
        public static T SafeInstantiate<T>(this T obj) where T : Component
        {
            obj.gameObject.SetActive(false);
            var inst = UnityEngine.Object.Instantiate(obj); // Instantiate a deactivated object, so Awake() calls aren't *called* - Pixel guy Note
            obj.gameObject.SetActive(true);

            return inst;
        }

        public static Character ToNPCEnum(this CustomNPCsEnum itemEnum)
        {
            return EnumExtensions.GetFromExtendedName<Character>(itemEnum.ToString());
        }

        public static NPC ToNPC(this Character itemEnum)
        {
            return NPCMetaStorage.Instance.Get(itemEnum).value;
        }

        public static NPC ToNPC(this CustomNPCsEnum itemEnum)
        {
            return NPCMetaStorage.Instance.Get(itemEnum.ToNPCEnum()).value;
        }

        //Made By Pixel Guy
        public static void BlockAllDirs(this EnvironmentController ec, IntVector2 pos, bool block)
        {
            ec.FreezeNavigationUpdates(true);
            var origin = ec.CellFromPosition(pos);
            for (int i = 0; i < 4; i++)
            {
                var dir = (Direction)i;
                var cell = ec.CellFromPosition(pos + dir.ToIntVector2());
                if (origin.ConstNavigable(dir))
                    cell.Block(dir.GetOpposite(), block);
            }
            ec.FreezeNavigationUpdates(false);
        }

        private static List<string> enums = [];
    }
}
