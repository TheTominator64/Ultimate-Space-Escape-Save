using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrops : MonoBehaviour
{
    [System.Serializable]
    public class DropCurrency
    {
        public string name;
        public GameObject item;
        public int dropRarity;
    }

    public List <DropCurrency> LootTable = new List<DropCurrency>();
    public int dropChance;

    //Assigns the range for chances that loot can spawn, and also allows this public void to be attached to buttons and called on demand to play these actions
    public void calculateLoot()
    {
        int calc_dropChance = Random.Range(0, 101);

        //Big complicated thing to calculate drop chance for loot
        if (calc_dropChance <= dropChance)
        {
            int itemWeight = 0;

            for(int i = 0;i < LootTable.Count; i++)
            {
                itemWeight += LootTable[i].dropRarity;
            }

            int randomValue = Random.Range(0, itemWeight);//80

            for(int j =0; j < LootTable.Count; j++)
            {
                if(randomValue <= LootTable[j].dropRarity)
                    {
                    Instantiate(LootTable[j].item,transform.position,Quaternion.identity);
                    return;
                    }
                randomValue -= LootTable[j].dropRarity;
            }
        }
    }
     
}
