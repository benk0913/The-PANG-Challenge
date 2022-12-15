using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    public string SoundInstancePrefabKey;

    void Awake()
    {
        Instance = this;
    }

    public void PlaySound(string clipKey)
    {
        ResourcesManager.Instance.LoadFromPool(SoundInstancePrefabKey, (GameObject soundInstance) =>
        {
            ResourcesManager.Instance.LoadSound(clipKey, (AudioClip clip) =>
            {
                soundInstance.GetComponent<SoundInstance>().SetInfo(clip, () => soundInstance.SetActive(false));
            });
        });
    }

}
