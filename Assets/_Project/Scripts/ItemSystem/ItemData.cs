using System;
using System.Collections.Generic;
using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
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
        public bool isNeedTarget;

        [SerializeReference] [AbstractSupport(typeof(IUsingTargetTracking))]
        public IUsingTargetTracking usingTargetTracking;

        [SerializeReference] [AbstractSupport(typeof(IUsingProcedure))]
        public IUsingProcedure usingProcedure;

        [SerializeReference] [AbstractSupport(typeof(IUsingEffect))]
        public List<IUsingEffect> usingEffects = new();
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

        public ItemUsingConfig primaryUsingConfig;
        public ItemUsingConfig secondaryUsingConfig;

        #endregion
    }
}