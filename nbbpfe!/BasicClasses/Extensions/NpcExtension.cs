using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.Extensions;
using UnityEngine;

namespace nbppfe.BasicClasses.Extensions
{
    public class NpcExtension : MonoBehaviour, IItemAcceptor
    {
        public void InsertItem(PlayerManager pm, EnvironmentController ec)
        {
        }

        public bool ItemFits(Items item)
        {
            return item.ToItem().item.GetComponent<NPCItem>();
        }
    }
}
