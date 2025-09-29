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

    private void Update()
    {
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
