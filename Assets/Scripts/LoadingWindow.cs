using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingWindow : MonoBehaviour
{
    public static LoadingWindow Instance;
    List<Object> LoadingCallers = new List<Object>();


    void Awake()
    {
        Instance = this;
        this.gameObject.SetActive(false);
    }

    public void AddLoader(Object me)
    {
        LoadingCallers.Add(me);

        this.gameObject.SetActive(true);
    }

    public void RemoveLoader(Object me)
    {
        LoadingCallers.Remove(me);

        if (LoadingCallers.Count == 0) this.gameObject.SetActive(false);
    }
}
