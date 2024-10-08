using MTM101BaldAPI;
using MTM101BaldAPI.Registers;
using nbbpfe.Enums;
using nbbpfe.FundamentalsManager;
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

        public static GameCamera GetPlayerCamera(this PlayerManager player) {
            return Singleton<CoreGameManager>.Instance.GetCamera(player.playerNumber);
        }

        public static Items ToItemEnum(this CustomItemsEnum itemEnum) {
            return EnumExtensions.GetFromExtendedName<Items>(itemEnum.ToString());
        }

        public static ItemObject ToItem(this Items itemEnum) {
            return ItemMetaStorage.Instance.FindByEnum(itemEnum).value;
        }

        public static ItemObject ToItem(this CustomItemsEnum itemEnum) {
            return ItemMetaStorage.Instance.FindByEnumFromMod(ToItemEnum(itemEnum), BasePlugin.Instance.Info).value;
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

        public static void SetSpeed(this Navigator navigator, int speed, int maxSpeed)
        {
            navigator.SetSpeed(speed);
            navigator.maxSpeed = maxSpeed;
        }

        public static float GetDistanceFrom(this Vector3 pos1, Vector3 pos2) {
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
    }
}
