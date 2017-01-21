using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public PlayerBehaviour playerBehaviour;

    public GameObject player;
    public float verticalOffset;

    public float FarZ;
    public float CloseZ;

    Vector3 vec;

    private void OnEnable()
    {
        playerBehaviour.OnLeaveGround += onPlayerLeaveGround;
        playerBehaviour.OnTouchGround += onPlayerTouchGround;
    }

    private void OnDisable()
    {
        playerBehaviour.OnLeaveGround -= onPlayerLeaveGround;
        playerBehaviour.OnTouchGround -= onPlayerTouchGround;
    }

    void Update () {
        vec = new Vector3(player.transform.position.x, player.transform.position.y+verticalOffset, transform.position.z);
        
        transform.position = vec;
    }

    private void onPlayerLeaveGround()
    {
        StopAllCoroutines();
        StartCoroutine(wait(0.3f, () => { StartCoroutine(ZoomOut(false)); }));
    }

    private void onPlayerTouchGround()
    {
        StopAllCoroutines();
        StartCoroutine(wait(0.3f, () => { StartCoroutine(ZoomOut(true)); }));
    }

    private IEnumerator wait(float timeWait, Action onWaitEnd)
    {
        yield return new WaitForSeconds(timeWait);
        onWaitEnd();
    }

    private IEnumerator ZoomOut(bool zoom)
    {
        float startZ = transform.position.z;
        float endZ = zoom ? FarZ : CloseZ;
        float endTime = zoom ? 0.5f : 5f;
        float time = 0;

        Vector3 vec = transform.position;

        while(time < endTime)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(startZ, endZ, time/endTime));
            time += Time.deltaTime;
            yield return null;
        }

    }


}
