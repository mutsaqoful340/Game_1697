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
    {
        var item = transform.GetChild(0);
        var itemUI = transform.GetChild(1);


        switch (state)
        {
            case ItemState.Sword:
                item.SetParent(player.SlotPrimary);
                itemUI.parent = ContentUI.transform;
                break;
            case ItemState.Shield:
                item.SetParent(player.SlotSecond);
                break;
        }

        item.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        player.Weapon = item;
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
