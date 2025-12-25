using UnityEngine;

namespace Systems.ItemSystem
{
    public class ItemUserBehaviour : MonoBehaviour, IItemUser
    {
        [SerializeField]
        private Component user;
        public Component User => user;
    }
}