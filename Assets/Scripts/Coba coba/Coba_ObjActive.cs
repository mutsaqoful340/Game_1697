using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coba_ObjActive : MonoBehaviour
{
    public GameObject obj;

    private bool isObjActive;

    private void Start()
    {
        obj.SetActive(false);
    }

    void Update()
    {
        Vector3[] screenPoints = {
        new Vector3(0, 0, 0), // Bottom-left
        new Vector3(Screen.width, 0, 0), // Bottom-right
        new Vector3(0, Screen.height, 0), // Top-left
        new Vector3(Screen.width, Screen.height, 0) // Top-right
    };

        foreach (Vector3 screenPoint in screenPoints)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has the desired tag
                if (hit.collider.CompareTag("ObjectA"))
                {
                    Debug.Log("Detected ObjectA object at screen corner.");
                }
            }
        }
    }




    private void SetGameObjActive()
    {
        isObjActive = true;

        if (isObjActive == true)
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);

        }
    }
}
