using System;
using Systems.CollisionSystem;
using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
using TnieYuPackage.SOAP.Data;
using UnityEngine;

namespace Systems.EntityDataSystem.Player
{
    [CreateAssetMenu(fileName = "PlayerEntityData", menuName = "Scriptable Objects/EntityData/Player")]
    public sealed class PlayerEntityData : StaticObjectIdentify, IEntityData
    {
        #region Singleton

        private static PlayerEntityData instance;

        public static PlayerEntityData Instance
        {
            get
            {
                if (instance == null)
                {
#if UNITY_EDITOR
                    // Trong Editor thì tìm bằng AssetDatabase cho tiện
                    string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(PlayerEntityData)}");
                    if (guids.Length == 0)
                    {
                        Debug.LogError($"No assets found in '{nameof(PlayerEntityData)}'!");
                        return null;
                    }

                    if (guids.Length > 0)
                    {
                        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerEntityData>(path);
                    }
#else
                // Runtime thì load từ Resources
                    instance = Resources.Load<PlayerEntityData>(nameof(PlayerEntityData));
                    if (instance == null) {
                        Debug.LogError($"No assets found in '{nameof(PlayerEntityData)}'!");
                    }
#endif
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                // Không nên xóa, chỉ cảnh báo thôi
                Debug.LogWarning($"Duplicate {nameof(PlayerEntityData)} detected: {name}");
            }
        }

        #endregion

        [SerializeField] private SoapData<int> health;
        [SerializeField] private int attackedDamage;
        [SerializeField] private float moveSpeed;

        public int Health
        {
            get => health.Value;
            set => health.Value = value;
        }

        public Action<int> OnHealthChanged
        {
            get => health.OnValueChange;
            set => health.OnValueChange = value;
        }

        public int AttackedDamage => attackedDamage;
        public float MoveSpeed => moveSpeed;
    }
    
}