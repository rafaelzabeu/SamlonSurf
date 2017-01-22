using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeixeFollow : MonoBehaviour
{


    public Animator animator;

    public PlayerBehaviour playerBehaviour;

    private bool m_isSinking = false;

    private Transform m_playerTransform;


    private void Awake()
    {
        m_playerTransform = playerBehaviour.transform;
    }


    private void OnEnable()
    {
        playerBehaviour.OnLeaveGround += onPlayerLeaveGround;
        playerBehaviour.OnTouchGround += onPlayerTouchGround;
        playerBehaviour.OnPlayerChangesOrientation += onPlayerChangesOrientation;
    }

    private void OnDisable()
    {
        playerBehaviour.OnLeaveGround -= onPlayerLeaveGround;
        playerBehaviour.OnTouchGround -= onPlayerTouchGround;
        playerBehaviour.OnPlayerChangesOrientation -= onPlayerChangesOrientation;
    }

    private void Update()
    {
        if (!m_isSinking)
            transform.position = m_playerTransform.position;

    }

    private void onPlayerChangesOrientation(bool goingDown)
    {
        StopCoroutine("turnDown");
        StartCoroutine("turnDown", goingDown);
    }

    //for what?
    private IEnumerator turnDown(bool down)
    {
        float startZ = transform.eulerAngles.z;
        float endZ = down ? -22f : 22f;
        float time = 0;

        while (time < 0.5f)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.LerpAngle(startZ, endZ, time * 2));
            time += Time.deltaTime;
            yield return null;
        }

    }

    private IEnumerator sink(bool down)
    {
        float start = transform.position.y;
        float end = down ? start - 0.5f : m_playerTransform.position.y;
        float time = 0;

        m_isSinking = true;

        while (time < 0.3f)
        {
            transform.position = new Vector3(m_playerTransform.position.x, Mathf.Lerp(start, end, time * 3.33f), m_playerTransform.position.z);
            time += Time.deltaTime;
            yield return null;
        }

        m_isSinking = false;
    }

    private void onPlayerTouchGround()
    {
        animator.SetTrigger("Nada");

        //if (force >= 1f)
        //{
        //    StopCoroutine("sink");
        //    StartCoroutine("sink", true);
        //}
    }

    private void onPlayerLeaveGround()
    {
        animator.SetTrigger("Pula");
        //StopCoroutine("sink");

        //if(m_isSinking)
        //{
        //    m_isSinking = false;
        //    StartCoroutine("sink", false);
        //}
    }



}
