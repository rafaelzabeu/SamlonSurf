using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public PlayerBehaviour playerBehaviour;

    public AudioClip intro;
    public AudioClip loop;

    public float time;

    public float points;

    public float basePointsPerSecond;
    public float pointsMulti;

    [HideInInspector]
    public bool canAddPoints;

	void Start () {
        AudioController.Instance.Play(intro, AudioController.SoundType.Music);
        StartCoroutine(wait(intro.length, () =>
         {
             AudioController.Instance.Play(loop, AudioController.SoundType.Music, 1, true);
         }));
        points = 0;
        canAddPoints = false;
	}

    private void OnEnable()
    {
        playerBehaviour.OnTouchGround += onPlayerTouchGround;
        playerBehaviour.OnLeaveGround += onPlayerLeaveGround;
    }

    private void OnDisable()
    {
        playerBehaviour.OnTouchGround += onPlayerTouchGround;
        playerBehaviour.OnLeaveGround += onPlayerLeaveGround;
    }

    public void CalculateMultiplyer(float yOffset)
    {
        pointsMulti = 1 + yOffset;
    }

    public void OnGabeFound()
    {
        points *= 2;
    }

    private IEnumerator wait(float time ,Action onEnd)
    {
        yield return new WaitForSeconds(time);
        if (onEnd != null)
            onEnd();
    }

    private void Update()
    {
        if(canAddPoints)
        {
            points += basePointsPerSecond * pointsMulti * Time.unscaledDeltaTime;
        }
        time += Time.deltaTime;
    }

    private void onPlayerTouchGround()
    {
        canAddPoints = false;
    }

    private void onPlayerLeaveGround()
    {
        canAddPoints = true;
    }

}
