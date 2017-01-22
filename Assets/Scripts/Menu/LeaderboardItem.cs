using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    public Text Name;
    public Text Score;

    public void SetItem(LeaderboardInfo info)
    {
        if (info.Name == null)
        {
            Name.text = "--";
            Score.text = "--";
        }
        else
        {
            Name.text = info.Name;
            Score.text = info.formatedScore + " pts";
        }
    }
}