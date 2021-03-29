using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    [Serializable]
    public class ShopItem
    {
        public string itemName;
        public int spriteID;
        public float price;
    }
    [Serializable]
    public class ShopItems
    {
        public ShopItem[] shopItems;
    }
}
