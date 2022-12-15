using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePopUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ScoreLabel;

    public void SetInfo(string text)
    {
        ScoreLabel.text = text;
    }
}
