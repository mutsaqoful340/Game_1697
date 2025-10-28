using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_Selector : MonoBehaviour
{
    public TextMeshProUGUI[] DataText = new TextMeshProUGUI[5];
    public GameObject selectorPanel;
    public GameObject confirmPanel;

    [SerializeField] private int _slotNumber;

    private void Start()
    {
        selectorPanel.SetActive(false);
        confirmPanel.SetActive(false);
    }

    public void LoadInfo(int index)
    {
        if (DataText[index].text != "No Data")
        {
            var path = Application.persistentDataPath + $"/GameData{index}.json";
            var json = File.ReadAllText(path);
            var record = JsonUtility.FromJson<Record>(json);

            Debug.Log(record);
        }
    }
    public void OnClick_Play()
    {
        SceneManager.LoadScene(2);
        Debug.Log("Pindah scene New Game");
    }

    public void OnClick_Load()
    {
        for (int i = 0; i < 5; i++)
        {
            var hasFile = File.Exists(Application.persistentDataPath + $"/GameData{i}.json");
            DataText[i].text = hasFile ? $"Saved Data {i + 1}" : "No Data";
        }

        Debug.Log(File.Exists(Application.persistentDataPath));
        selectorPanel.SetActive(true);
    }

    public void OnClick_Back()
    {
        selectorPanel.SetActive(false);
        Debug.Log("Back Button clicked");
    }

    public void OnClick_Slot1()
    {
        confirmPanel.SetActive(true);
        _slotNumber = 1;
        Debug.Log("Slot 1 clicked.");
    }

    public void OnClick_Slot2()
    {
        confirmPanel.SetActive(true);
        _slotNumber = 2;
        Debug.Log("Slot 2 clicked.");
    }

    public void OnClick_Slot3()
    {
        confirmPanel.SetActive(true);
        _slotNumber = 3;
        Debug.Log("Slot 3 clicked.");
    }

    public void OnClick_Slot4()
    {
        confirmPanel.SetActive(true);
        _slotNumber = 4;
        Debug.Log("Slot 4 clicked.");
    }

    public void OnClick_Slot5()
    {
        confirmPanel.SetActive(true);
        _slotNumber = 5;
        Debug.Log("Slot 5 clicked.");
    }

    public void OnClick_ConfirmYes()
    {
        confirmPanel.SetActive(false);
        Debug.Log("Confirmed, data Number " + _slotNumber + " is loaded.");
    }

    public void OnClick_ConfirmNo()
    {
        confirmPanel.SetActive(false);
        Debug.Log("No.");
    }
}
