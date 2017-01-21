using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeixeFollow : MonoBehaviour {

    private Transform player;
    private Rigidbody player_rig;

    public Animator animator;

    public PlayerBehaviour m_playerBehaviour;

    private void Awake()
    {
        player = m_playerBehaviour.transform;
        player_rig = m_playerBehaviour.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_playerBehaviour.OnLeaveGround += OnPlayerLeaveGround;
        m_playerBehaviour.OnTouchGround += onPlayerTouchGround;
        m_playerBehaviour.OnPlayerChangesOrientation += onPlayerChangesOrientation;
    }

    private void OnDisable()
    {
        m_playerBehaviour.OnLeaveGround -= OnPlayerLeaveGround;
        m_playerBehaviour.OnTouchGround -= onPlayerTouchGround;
        m_playerBehaviour.OnPlayerChangesOrientation -= onPlayerChangesOrientation;
    }

    private void Update()
    {
        transform.position = player.position;
        
        

    }

    private void onPlayerChangesOrientation(bool goingDown)
    {
        StopAllCoroutines();
        StartCoroutine(turnDown(goingDown));
    }

    //for what?
    private IEnumerator turnDown(bool down)
    {
        float startZ = transform.eulerAngles.z;
        float endZ = down ? -22f : 22f;
        float time = 0;

        while(time < 0.5f)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.LerpAngle(startZ, endZ, time * 2));
            time += Time.deltaTime;
            yield return null;
        }

    }

    private void onPlayerTouchGround()
    {
        animator.SetTrigger("Nada");
    }

    private void OnPlayerLeaveGround()
    {
        animator.SetTrigger("Pula");
    }

}
