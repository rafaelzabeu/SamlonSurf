using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCollectorBehaviour : MonoBehaviour {

    public GamePlayMenuController controller;

    public Text txt_score;
    public InputField ipt_nome;


    public void Show()
    {
        txt_score.text = controller.formatedScore;
    }

    public void OnContinueButton()
    {
        if(!string.IsNullOrEmpty( ipt_nome.text ))
        {
            LeaderboardInfo info = new LeaderboardInfo();
            info.Name = ipt_nome.text;
            info.Score = controller.manager.points;
            Leaderboard.AddScore(info);
            controller.ShowLeaderboard();
        }
    }
	
}
