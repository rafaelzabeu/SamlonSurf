using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public GameObject menu;
    public LeaderboardBehaviour leaderbord;

    public AudioClip menuBGM;

    private void Start()
    {
        AudioController.Instance.Play(menuBGM, AudioController.SoundType.Music);
        Time.timeScale = 1;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowLeaderboard()
    {
        menu.SetActive(false);
        leaderbord.gameObject.SetActive(true);
        leaderbord.LoadLeaderboard();
    }

    public void ShowMenu()
    {
        leaderbord.gameObject.SetActive(false);
        menu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
	
}
