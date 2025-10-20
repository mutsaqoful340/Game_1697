using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class RecorderUI : MonoBehaviour
{
    public TextMeshProUGUI[] DataText = new TextMeshProUGUI[5];
    public int SaveIndex;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            var hasFile = File.Exists(Application.persistentDataPath + $"/GameData{i}.json");
            DataText[i].text = hasFile ? $"Saved Data {i + 1}" : "No Data";
        }
    }

    public void Btn_Save()
    {
        gameObject.SetActive(false);

        // Save Progress
        var path = Application.persistentDataPath + $"/GameData{SaveIndex}.json";
        File.WriteAllText(path, "");
        for (int i = 0; i < 5; i++)
        {
            var hasFile = File.Exists(Application.persistentDataPath + $"/GameData{i}.json");
            DataText[i].text = hasFile ? $"Saved Data {i + 1}" : "No Data";
        }
        Debug.Log(path);
    }

    public void Btn_Back()
    {
        gameObject.SetActive(false);
    }

    public void Btn_Delete()
    {
        var hasFile = Application.persistentDataPath + $"/GameData{SaveIndex}.json";
        if (File.Exists(hasFile))
        {
            File.Delete(hasFile);
            DataText[SaveIndex].text = "No Data";
        }
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
