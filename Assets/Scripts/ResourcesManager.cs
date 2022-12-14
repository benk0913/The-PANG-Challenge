using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance;

    [SerializeField]
    bool PrewarmOnStart;

    [SerializeField]
    List<string> PrewarmAssets = new List<string>();

    Dictionary<string, GameObject> loadedObjects = new Dictionary<string, GameObject>();

    public bool IsPrewarming
    {
        get
        {
            return PrewarmRoutineInstance != null;
        }
    }
    Coroutine PrewarmRoutineInstance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (PrewarmOnStart)
        {
            if (PrewarmRoutineInstance != null) StopCoroutine(PrewarmRoutineInstance);

            PrewarmRoutineInstance = StartCoroutine(PrewarmRoutine());
        }
    }



    public void LoadObject(string resourceKey, Action<GameObject> onComplete = null)
    {
        if (loadedObjects.ContainsKey(resourceKey))
        {
            onComplete?.Invoke(loadedObjects[resourceKey]);
            return;
        }

        StartCoroutine(LoadObjectAsyncRoutine(resourceKey, onComplete));
    }

    IEnumerator LoadObjectAsyncRoutine(string resourceKey, Action<GameObject> onComplete = null)
    {
        LoadingWindow.Instance.AddLoader(this);

        AsyncOperationHandle<GameObject> objectOpHandle;
        objectOpHandle = Addressables.LoadAssetAsync<GameObject>(resourceKey);

        yield return objectOpHandle;

        LoadingWindow.Instance.RemoveLoader(this);

        loadedObjects.Add(resourceKey, objectOpHandle.Result);
        onComplete?.Invoke(objectOpHandle.Result);
    }

    IEnumerator PrewarmRoutine()
    {
        LoadingWindow.Instance.AddLoader(this);
        for (int i = 0; i < PrewarmAssets.Count; i++)
        {
            string resourceKey = PrewarmAssets[i];

            if (loadedObjects.ContainsKey(resourceKey))
            {


                AsyncOperationHandle<GameObject> objectOpHandle;
                objectOpHandle = Addressables.LoadAssetAsync<GameObject>(resourceKey);

                yield return objectOpHandle;



                loadedObjects.Add(resourceKey, objectOpHandle.Result);
            }
        }

        yield return 0;

        LoadingWindow.Instance.RemoveLoader(this);
    }
}
