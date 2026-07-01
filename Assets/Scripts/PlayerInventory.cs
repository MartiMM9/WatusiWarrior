using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    PlayerController player;

    [Header("Crafting Materials")]
    public int hornShards;
    public int watusiWool;
    public int humpGrease;

    [Header("Items")]
    public int bullets;
    public int spears;
    public int traps;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void AddMaterial(string materialName, int value)
    {
        switch (materialName)
        {
            case "Horn Shard":
                hornShards += value;
                break;
            case "Watusi Wool":
                watusiWool += value;
                break;
            case "Hump Grease":
                humpGrease += value;
                break;
            default:
                Debug.LogWarning("Unknown material: " + materialName);
                break;
        }
    }

    public void AddItem(string itemName)
    {
        switch (itemName)
        {
            case "Bullet":
                if(hornShards >= 1)
                {
                    bullets ++;
                    hornShards -= 1;
                }
                else
                {
                    Debug.LogWarning("Not enough materials to craft Bullet.");
                }
                    break;
            case "Spear":
                if(hornShards >= 5 && watusiWool >= 1)
                {
                    spears ++;
                    hornShards -= 5;
                    watusiWool -= 1;
                }
                else
                {
                    Debug.LogWarning("Not enough materials to craft Spear.");
                }
                    break;
            case "Trap":
                if(hornShards >= 5 && watusiWool >= 2 && humpGrease >= 1)
                {
                    traps ++;
                    hornShards -= 5;
                    watusiWool -= 2;
                    humpGrease -= 1;
                }
                else
                {
                    Debug.LogWarning("Not enough materials to craft Trap.");
                }
                    break;
            default:
                Debug.LogWarning("Unknown item: " + itemName);
                break;
        }
    }
}
