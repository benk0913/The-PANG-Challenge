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

    Dictionary<string, object> loadedAssets = new Dictionary<string, object>();

    Dictionary<string, List<GameObject>> GeneralObjectPool = new Dictionary<string, List<GameObject>>();

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

    public void LoadFromPool(string resourceKey, Action<GameObject> onComplete = null)
    {
        if (!GeneralObjectPool.ContainsKey(resourceKey))
        {
            GeneralObjectPool.Add(resourceKey, new List<GameObject>());
            LoadObject(resourceKey, (GameObject loadedObject) =>
            {
                loadedObject = Instantiate(loadedObject);
                GeneralObjectPool[resourceKey].Add(loadedObject);
                onComplete?.Invoke(loadedObject);
            });

            return;
        }

        GameObject foundObject = GeneralObjectPool[resourceKey].Find(x => !x.activeInHierarchy);

        if (foundObject == null)
        {
            LoadObject(resourceKey, (GameObject loadedObject) =>
              {
                  loadedObject = Instantiate(loadedObject);
                  GeneralObjectPool[resourceKey].Add(loadedObject);
                  onComplete?.Invoke(loadedObject);

              });

            return;
        }

        foundObject.SetActive(true);
        onComplete?.Invoke(foundObject);

    }

    public void LoadObject(string resourceKey, Action<GameObject> onComplete = null)
    {
        LoadAsset(resourceKey, (object asset) =>
        {
            onComplete?.Invoke((GameObject)asset);
        });
    }

    public void LoadSound(string resourceKey, Action<AudioClip> onComplete = null)
    {
        LoadAsset(resourceKey, (object asset) =>
        {
            onComplete?.Invoke((AudioClip)asset);
        });
    }


    public void LoadAsset(string resourceKey, Action<object> onComplete = null)
    {
        if (loadedAssets.ContainsKey(resourceKey))
        {
            onComplete?.Invoke(loadedAssets[resourceKey]);
            return;
        }

        StartCoroutine(LoadObjectAsyncRoutine(resourceKey, onComplete));
    }

    IEnumerator LoadObjectAsyncRoutine(string resourceKey, Action<object> onComplete = null)
    {
        // LoadingWindow.Instance.AddLoader(this);

        AsyncOperationHandle<object> objectOpHandle;
        objectOpHandle = Addressables.LoadAssetAsync<object>(resourceKey);

        yield return objectOpHandle;

        // LoadingWindow.Instance.RemoveLoader(this);

        if (!loadedAssets.ContainsKey(resourceKey))
        {
            loadedAssets.Add(resourceKey, objectOpHandle.Result);
            onComplete?.Invoke(objectOpHandle.Result);
        }
        else
        {
            onComplete?.Invoke(loadedAssets[resourceKey]);
        }
    }


    IEnumerator PrewarmRoutine()
    {
        LoadingWindow.Instance.AddLoader(this);
        for (int i = 0; i < PrewarmAssets.Count; i++)
        {
            string resourceKey = PrewarmAssets[i];

            if (loadedAssets.ContainsKey(resourceKey))
            {


                AsyncOperationHandle<GameObject> objectOpHandle;
                objectOpHandle = Addressables.LoadAssetAsync<GameObject>(resourceKey);

                yield return objectOpHandle;



                loadedAssets.Add(resourceKey, objectOpHandle.Result);
            }
        }

        yield return 0;

        LoadingWindow.Instance.RemoveLoader(this);
    }
}
