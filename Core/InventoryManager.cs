using System.Collections.Generic;
using UnityEngine;

public class InventoryManager: MonoBehaviour
{
    private Dictionary<Item, int> inventory = new Dictionary<Item, int>();

    public void AddItem(Item item)
    {
        Debug.Log(item);
        
        if (inventory.ContainsKey(item))
        {
            inventory[item]++;
        }
        else
        {
            inventory[item] = 1;
        }

        UpdatePlayerStats();
        Debug.Log(inventory);
    }

    public void RemoveItem(Item item)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item]--;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
        }

        UpdatePlayerStats();
    }

    public int GetStackCount(Item item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }

    private void UpdatePlayerStats()
    {
        player player = GetComponent<player>();
        //if (player != null) { return; }
        player.ResetStats();

        List<Item> itemsToModify = new List<Item>();

        foreach (var kvp in inventory)
        {
            Item item = kvp.Key;
            int stackCount = kvp.Value;

            switch (item.itemType)
            {

                //Mathf.Pow(1+item.multiplier, stackCount)
                case "Health":
                    player.maxHealth += 1 + item.multiplier * stackCount;
                    break;
                case "Damage":
                    player.projectileDamage *= 1 + item.multiplier * stackCount; break;
                case "Speed":
                    player.speed *= 1 + item.multiplier * stackCount; break;
                case "extraProjCount":
                    player.extraProjCount += (int)(item.multiplier * stackCount); break;
                case "ProjectileSpeed":
                    player.projectileSpeed *= 1 + item.multiplier * stackCount; break;
                case "AtkSpd":
                    player.hitsPerSecond *= 1 + item.multiplier * stackCount; break;
                case "Crit":
                    player.critChance += item.multiplier * stackCount; break;
                case "Pierce":
                    player.pierce += (int)(item.multiplier * stackCount); break;
            }
        }
        if (player.speed > 36f) {
            player.speed = 36f;
        }
        player.updateWeaponStats();
    }


}
