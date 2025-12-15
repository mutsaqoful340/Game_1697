using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Searcher;
using Unity.VisualScripting;
using static UnityEditor.Progress;

public class ItemActive : MonoBehaviour
{
    public PlayerActive player;
    public GameObject ContentUI;

    [Header("Item Data")]
    public Item _Item;
    
    //[SerializeField] private bool isShield;
    //[SerializeField] private bool isSword;
    //public string objName;
    //public List<string> itemNames = new List<string>();

    public ItemState state;

    public enum ItemState
    {
        Sword,
        Shield
    }

    private void Interaction_Handler()
    {if (_Item.IsEquipment)
        {
            var item = transform.GetChild(0);
            switch (state)
            {
                case ItemState.Sword:
                    item.SetParent(player.SlotPrimary);
                    break;
                case ItemState.Shield:
                    item.SetParent(player.SlotSecond);
                    break;
            }
            item.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            player.Weapon = item;
        }
        var inventory = GameObject.FindWithTag("HUD").GetComponent<HudGuiActive>();
        inventory.AddItem(_Item);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerActive>();
            player.OnInteraction = Interaction_Handler;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerActive>();
            player.OnInteraction = null;
        }
    }
}
