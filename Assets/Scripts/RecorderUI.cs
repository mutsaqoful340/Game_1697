using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class RecorderUI : MonoBehaviour
{
    public TextMeshProUGUI[] DataText = new TextMeshProUGUI[5];
    public DataState State;
    public Hero Player;
    public int SaveIndex;

    public GameObject confirmPanel;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        confirmPanel.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            var hasFile = File.Exists(Application.persistentDataPath + $"/GameData{i}.json");
            DataText[i].text = hasFile ? $"Saved Data {i + 1}" : "No Data";
        }
    }

    public enum DataState { Save, Delete}

    public void Btn_Confirm_Yes()
    {
        switch (State)
        {
            case DataState.Save: 
                Btn_SaveInfo(); 
                break;
            case DataState.Delete:
                Btn_DeleteInfo();
                break;
        }
        confirmPanel.SetActive(false);
    }

    public void Btn_Confirm_No()
    {
        confirmPanel.SetActive(false);
    }

    public void Btn_Save()
    {
        if (DataText[SaveIndex].text == "No Data")
        {
            Btn_SaveInfo(); // Save new
        }
        else 
        {
            State = DataState.Save;
            confirmPanel.SetActive(true); // Save overwwrite
        }

    }

    public void Btn_Delete()
    {
        State = DataState.Delete;
        confirmPanel.SetActive(true);
    }

    private void Btn_SaveInfo()
    {
        var record = new Record
        {
            SaveSlotNumber = SaveIndex,
            Name = $"Save Slot {SaveIndex}",
            Class = new Hero
            {
                Name = Player.Name,
                TotalExp = Player.TotalExp,
                Strength = (Player.Strength.basicSTR, Player.Strength.bonusSTR),
                Intelligence = (Player.Intelligence.basicINT, Player.Intelligence.bonusINT),
            },
            DateTime = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"),
        };
        record.Class.HealthPoint.Current = Player.HealthPoint.Current;
        record.Class.HealthPoint.Maximum = Player.HealthPoint.Maximum;
        var json = JsonUtility.ToJson(record, false);
        gameObject.SetActive(false);

        // Save Progress
        var path = Application.persistentDataPath + $"/GameData{SaveIndex}.json";
        File.WriteAllText(path, json);
        for (int i = 0; i < 5; i++)
        {
            var hasFile = File.Exists(Application.persistentDataPath + $"/GameData{i}.json");
            DataText[i].text = hasFile ? $"Saved Data {i + 1}" : "No Data";
        }
        Debug.Log(path);
    }

    private void Btn_DeleteInfo()
    {
        var hasFile = Application.persistentDataPath + $"/GameData{SaveIndex}.json";
        if (File.Exists(hasFile))
        {
            File.Delete(hasFile);
            DataText[SaveIndex].text = "No Data";
        }
    }

    public void Btn_Back()
    {
        gameObject.SetActive(false);
    }

    public void Btn_Slot1()
    {
        SaveIndex = 0;
    }

    public void Btn_Slot2()
    {
        SaveIndex = 1;
    }

    public void Btn_Slot3()
    {
        SaveIndex = 2;
    }

    public void Btn_Slot4()
    {
        SaveIndex = 3;
    }

    public void Btn_Slot5()
    {
        SaveIndex = 4;

    }
}
