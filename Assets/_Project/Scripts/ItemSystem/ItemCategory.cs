using UnityEngine;

namespace Systems.ItemSystem
{
    [CreateAssetMenu(fileName = "ItemCategory", menuName = "Scriptable Objects/ItemSystem/ItemCategory")]
    public class ItemCategory : ScriptableObject
    {
        public string categoryName;
    }
}