using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIView : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TextLabel;

    public void SetInfo(string text)
    {
        TextLabel.text = text;
    }
}
