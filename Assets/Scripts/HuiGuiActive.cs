using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudGuiActive : MonoBehaviour
{
    public GameObject Dashboard;
    public List<Item> Inventories;
    public GameObject ContentItem;

    public void AddItem(Item item)
    {
        Inventories.Add(item);
        var slot = Resources.Load<GameObject>("ItemSlot");

        if (item.IsEquipment)
        {
            slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Name;
            Instantiate(slot, ContentItem.transform);
        }
        else
        {
            foreach(var selected in Inventories)
            {
                if (selected == item)
                {

                }
            }
        }
    }
}
