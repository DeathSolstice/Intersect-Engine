﻿using System;
using System.Collections.Generic;
using Intersect.Models;

namespace Intersect.GameObjects
{
    public class ShopBase : DatabaseObject<ShopBase>
    {
        public List<ShopItem> BuyingItems = new List<ShopItem>();

        //Buying List
        public bool BuyingWhitelist = true;

        public int DefaultCurrency = 0;

        //Selling List
        public List<ShopItem> SellingItems = new List<ShopItem>();

        public ShopBase(int id) : base(id)
        {
            Name = "New Shop";
        }

        public override void Load(byte[] packet)
        {
            var myBuffer = new ByteBuffer();
            myBuffer.WriteBytes(packet);
            Name = myBuffer.ReadString();
            DefaultCurrency = myBuffer.ReadInteger();
            SellingItems.Clear();
            BuyingItems.Clear();
            var sellingCount = myBuffer.ReadInteger();
            for (int i = 0; i < sellingCount; i++)
            {
                SellingItems.Add(new ShopItem(myBuffer));
            }
            BuyingWhitelist = Convert.ToBoolean(myBuffer.ReadByte());
            var buyingCount = myBuffer.ReadInteger();
            for (int i = 0; i < buyingCount; i++)
            {
                BuyingItems.Add(new ShopItem(myBuffer));
            }
            myBuffer.Dispose();
        }

        public byte[] ShopData()
        {
            var myBuffer = new ByteBuffer();
            myBuffer.WriteString(Name);
            myBuffer.WriteInteger(DefaultCurrency);
            myBuffer.WriteInteger(SellingItems.Count);
            for (int i = 0; i < SellingItems.Count; i++)
            {
                myBuffer.WriteBytes(SellingItems[i].Data());
            }
            myBuffer.WriteByte(Convert.ToByte(BuyingWhitelist));
            myBuffer.WriteInteger(BuyingItems.Count);
            for (int i = 0; i < BuyingItems.Count; i++)
            {
                myBuffer.WriteBytes(BuyingItems[i].Data());
            }

            return myBuffer.ToArray();
        }

        public override byte[] BinaryData => ShopData();
    }

    public class ShopItem
    {
        public int CostItemNum;
        public int CostItemVal;
        public int ItemNum;

        public ShopItem(ByteBuffer myBuffer)
        {
            ItemNum = myBuffer.ReadInteger();
            CostItemNum = myBuffer.ReadInteger();
            CostItemVal = myBuffer.ReadInteger();
        }

        public ShopItem(int itemNum, int costItemNum, int costVal)
        {
            ItemNum = itemNum;
            CostItemNum = costItemNum;
            CostItemVal = costVal;
        }

        public byte[] Data()
        {
            ByteBuffer myBuffer = new ByteBuffer();
            myBuffer.WriteInteger(ItemNum);
            myBuffer.WriteInteger(CostItemNum);
            myBuffer.WriteInteger(CostItemVal);
            return myBuffer.ToArray();
        }
    }
}