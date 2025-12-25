using EditorAttributes;
using Systems.ItemSystem;
using TnieCustomPackage.SerializeInterface;
using UnityEngine;

namespace _Project.Test
{
    public class TestUsingItem : MonoBehaviour
    {
        public InterfaceReference<IItemUser> itemUser;
        public ItemData item;

        [Button]
        private void UsingItem()
        {
            if (itemUser.Value is null || item is null) return;
            
            ItemUsingSystem.UseItemPrimary(itemUser.Value, item);
        }
    }
}