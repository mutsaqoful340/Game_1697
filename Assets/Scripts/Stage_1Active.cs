using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_1Active : MonoBehaviour
{
    public enum StageType
    {
        FindSword,
        TakeSword,
        Destroy
    }


    public StageType state;
    public List<Quest> CheckData;
    public Transform player;
    public int Index;

    [Serializable]
    public class Quest
    {
        public string name;
        public int Qty;
    }

    public StageType StageRoute
    {
        get
        {
            return state;
        }
        set
        {
            switch (state)
            {
                case StageType.FindSword:
                    Act1_FindSword(true);
                    break;
            }
        }
    }

    private void Act1_FindSword(bool setup)
    {
        if (setup) // Preparation
        {
            CheckData = new List<Quest>()
            {
                new()
                {
                    name = "Talk to the NPC.", Qty = 0
                },
                new()
                {
                    name = "Find the Legendary Sword.", Qty = 0
                }
            };
            var obj = transform.Find("Act_1").GetChild(0);
            obj.gameObject.SetActive(true);
            obj.GetChild(0).GetComponent<DialogActive>().OnFinishedAction = InteractionAct1_Handler;
            Debug.Log(CheckData[0].name);
            Index = 0;
        }
        else // Monitoring
        {
            var target = transform.Find("Act_1").GetChild(0);
            CheckData[Index].Qty = (int)Vector3.Distance(player.position, target.position);
            Debug.Log(CheckData[0].name + " berjarak" + CheckData[0].Qty);
        }
    }

    private void InteractionAct1_Handler()
    {

    }

    private void Start()
    {
        StageRoute = StageType.FindSword;
    }

    private void Update()
    {
        switch (state)
        {
            case StageType.FindSword:
                Act1_FindSword(false);
                break;
        }
    }
}
