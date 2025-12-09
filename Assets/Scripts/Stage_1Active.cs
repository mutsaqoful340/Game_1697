using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StageActive : MonoBehaviour
{
    public enum StageType
    {
        FindSword,
        RuneStone,
        FeedSword,
        TakeSowrd,
        Destroy,
    }

    public StageType State;
    public List<Quest> CheckData;
    public Transform Player;
    public int Index;

    [Serializable]
    public class Quest
    {
        public string Name;
        public int Qty;
    }

    public StageType StageRoute
    {
        get
        {
            return State;
        }
        set
        {
            switch (State)
            {
                case StageType.FindSword:
                    Act1_FindSword(true);
                    break;
                case StageType.RuneStone:
                    Act2_RuneStone(true);
                    break;
            }
        }
    }

    private void Act1_FindSword(bool setup)
    {
        if (setup) //prepare
        {
            CheckData = new List<Quest>()
            {
                new() { Name = "Talk to the NPC.", Qty = 0 },
                new() { Name = "Find the Legendary Sword.", Qty = 0},
            };
            var obj = transform.Find("Act_1").GetChild(0);
            obj.gameObject.SetActive(true);
            obj.GetChild(0).GetComponent<DialogActive>().OnFinishedAction = InteractionAct1_Handler;
            Debug.Log(CheckData[0].Name);
            Index = 0;
        }
        else //monitoring
        {
            if (Index >= CheckData.Count)
            {
                var obj = transform.Find("Act_1").GetChild(0);
                obj.GetChild(0).GetComponent<DialogActive>().OnFinishedAction = null;
                Destroy(obj.parent.gameObject);
                StageRoute = StageType.RuneStone;
                return;
            }
            var target = transform.Find("Act_1").GetChild(0);
            CheckData[Index].Qty = (int)Vector3.Distance(Player.position, target.position);
            Debug.Log(CheckData[0].Name + " berjarak " + CheckData[0].Qty);
        }
    }
    private void InteractionAct1_Handler()
    {
        if (CheckData[Index].Qty <= 1)
        {
            if (++Index < CheckData.Count)
            {
                var point = transform.Find("Act_1").GetChild(1);
                point.GetChild(0).GetComponent<DialogActive>().OnFinishedAction = InteractionAct1_Handler;
                point.gameObject.SetActive(true);
                transform.Find("Act_1").GetChild(0).SetAsLastSibling();
            }
        }
    }

    private void Act2_RuneStone(bool setup)
    {
        if (setup) //prepare
        {
            CheckData = new List<Quest>()
            {
                new() { Name = "Talk to NPC1.", Qty = 0},
                new() { Name = "Talk to NPC2.", Qty = 0},
                new() { Name = "Talk to NPC3.", Qty = 0},
            };
            var obj = transform.Find("Act_2").GetChild(0);
            obj.gameObject.SetActive(true);
            obj.GetChild(0).GetComponent<DialogActive>().OnFinishedAction = InteractionAct2_Handler;
            Debug.Log(CheckData[0].Name);
            Index = 2;
        }
        else //monitoring
        {
            if (Index >= CheckData.Count)
            {
                var obj = transform.Find("Act_2").GetChild(0);
                obj.GetChild(0).GetComponent<DialogActive>().OnFinishedAction = null;
                Destroy(obj.parent.gameObject);
                StageRoute = StageType.FeedSword;
                return;
            }
            var target = transform.Find("Act_2").GetChild(0);
            CheckData[Index].Qty = (int)Vector3.Distance(Player.position, target.position);
            Debug.Log(CheckData[0].Name + " berjarak " + CheckData[0].Qty);
        }
    }

    private void InteractionAct2_Handler()
    {
        if (CheckData[Index].Qty <= 3)
        {
            if (++Index < CheckData.Count)
            {
                var point = transform.Find("Act_2").GetChild(1);
                point.GetChild(0).GetComponent<DialogActive>().OnFinishedAction = InteractionAct1_Handler;
                point.gameObject.SetActive(true);
                transform.Find("Act_2").GetChild(0).SetAsLastSibling();
            }
        }
        if (CheckData[Index].Qty <= 4)
        {
            if (++Index < CheckData.Count)
            {
                var point = transform.Find("Act_2").GetChild(2);
                point.GetChild(0).GetComponent<DialogActive>().OnFinishedAction = InteractionAct1_Handler;
                point.gameObject.SetActive(true);
                transform.Find("Act_2").GetChild(0).SetAsLastSibling();
            }
        }
    }

    private void Start()
    {
        StageRoute = StageType.FindSword;
    }

    private void Update()
    {
        Debug.Log(CheckData.Count);
        switch (State)
        {
            case StageType.FindSword:
                Act1_FindSword(false);
                break;
            case StageType.RuneStone:
                Act2_RuneStone(false);
                break;
        }
    }
}
