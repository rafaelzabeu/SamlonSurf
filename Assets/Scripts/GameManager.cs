using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public AudioClip intro;
    public AudioClip loop;

	void Start () {
        AudioController.Instance.Play(intro, AudioController.SoundType.Music);
        StartCoroutine(wait(intro.length, () =>
         {
             AudioController.Instance.Play(loop, AudioController.SoundType.Music, 1, true);
         }));
	}
	
    private IEnumerator wait(float time ,Action onEnd)
    {
        yield return new WaitForSeconds(time);
        if (onEnd != null)
            onEnd();
    }

}
