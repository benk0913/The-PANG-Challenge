using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField]
    Transform LeaderboardContainer;

    [SerializeField]
    string EntryPrefabKey;



    public void RefreshUI()
    {
        if (!this.gameObject.activeInHierarchy) return;

        ClearContainer();

        for (int i = 0; i < LeaderboardController.Instance.Leaderboard.Count; i++)
        {
            LeaderboardController.LeaderBoardEntryData entry = LeaderboardController.Instance.Leaderboard[i];
            ResourcesManager.Instance.LoadFromPool(EntryPrefabKey, (GameObject entryObject) =>
            {
                entryObject.GetComponent<ScoreUIView>().SetInfo((i+1) +" | "+entry.Score+" | " +entry.Name);
                entryObject.transform.SetParent(LeaderboardContainer, false);
                entryObject.transform.localScale = Vector3.one;
                entryObject.transform.position = Vector3.zero;
            });
        }
    }

    void ClearContainer()
    {
        while (LeaderboardContainer.childCount > 0)
        {
            LeaderboardContainer.GetChild(0).gameObject.SetActive(false);
            LeaderboardContainer.GetChild(0).SetParent(null);
        }
    }
}
