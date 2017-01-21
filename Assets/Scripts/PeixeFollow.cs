using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeixeFollow : MonoBehaviour {

    private Transform player;

    public Animator animator;

    public PlayerBehaviour m_playerBehaviour;

    private void Awake()
    {
        player = m_playerBehaviour.transform;
        
    }

    private void OnEnable()
    {
        m_playerBehaviour.OnLeaveGround += OnPlayerLeaveGround;
        m_playerBehaviour.OnTouchGround += onPlayerTouchGround;
    }

    private void OnDisable()
    {
        m_playerBehaviour.OnLeaveGround -= OnPlayerLeaveGround;
        m_playerBehaviour.OnTouchGround -= onPlayerTouchGround;
    }

    private void Update()
    {
        transform.position = player.position;
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
