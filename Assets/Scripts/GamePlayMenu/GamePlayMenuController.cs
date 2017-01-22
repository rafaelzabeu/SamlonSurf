using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayMenuController : MonoBehaviour {

    public GameManager manager;

    public ScoreCollectorBehaviour scoreCollector;
    public LeaderboardBehaviour learderboard;

    public Text txt_timer;
    public Text txt_points;

    public string formatedTime
    {
        get
        {
            return (((int)manager.timeEnd) - ((int)manager.time)).ToString();
        }
    }
    public string formatedScore
    {
        get
        {
            return ((int)manager.points).ToString();
        }
    }

    private void Start()
    {
        StartCoroutine(updateLabels());
    }

    public void ShowGameOver()
    {
        StopAllCoroutines();
        scoreCollector.gameObject.SetActive(true);
        scoreCollector.Show();
    }

    public void ShowLeaderboard()
    {
        scoreCollector.gameObject.SetActive(false);
        learderboard.gameObject.SetActive(true);
        learderboard.LoadLeaderboard();
    }

    public void OnMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator updateLabels()
    {
        while(true)
        {
            txt_timer.text = formatedTime;
            txt_points.text = formatedScore + " pts";
            yield return new WaitForSeconds(0.3f);
        }
    }
}
