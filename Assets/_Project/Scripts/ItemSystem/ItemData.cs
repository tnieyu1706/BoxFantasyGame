using System;
using System.Collections.Generic;
using EditorAttributes;
using Systems.EntityDataSystem;
using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
using TnieCustomPackage.SerializeInterface;
using TnieYuPackage.CustomAttributes;
using UnityEngine;

namespace Systems.ItemSystem
{
    [Serializable]
    public enum ItemRarity
    {
        Common,
        Rarely,
        Epic
    }

    [Serializable]
    public class ItemUsingConfig
    {
        [SerializeReference] [AbstractSupport(typeof(IUsingProcedure))]
        public IUsingProcedure usingProcedure;

        [SerializeReference] [AbstractSupport(typeof(IUsingEffect))]
        public List<IUsingEffect> usingEffects = new();

        public void Use(IEntity entity)
        {
            usingProcedure.HandleProcedure(entity, usingEffects);
        }
    }
    
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemSystem/ItemData")]
    public class ItemData : StaticObjectIdentify
    {
        #region PROPERTIES
        
        public string itemName;
        public string description;
        public ItemCategory itemCategory;
        public ItemRarity rarity;

        public Sprite itemSprite;
        public GameObject itemPrefab;

        public int sellingPrice;
        public int stackSize = 1;

        [SerializeField] private ItemUsingConfig primaryUsingConfig;
        [SerializeField] private ItemUsingConfig secondaryUsingConfig;
        
        #endregion
        
        #region METHODS

        public void UsePrimary(IEntity entity)
        {
            primaryUsingConfig.Use(entity);
        }

        public void UseSecondary(IEntity entity)
        {
            secondaryUsingConfig.Use(entity);
        }
        
        #endregion
        
        #region TestEditor

        [SerializeField] private InterfaceReference<IEntity> primaryEntityManual;

        [Button]
        private void UsePrimaryManual()
        {
            if (primaryEntityManual == null || primaryEntityManual.Value == null) return;
            UsePrimary(primaryEntityManual.Value);
        }

        #endregion

    }
}