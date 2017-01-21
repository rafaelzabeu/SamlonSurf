using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public delegate void PlayerCallback();
    public event PlayerCallback OnTouchGround;
    public event PlayerCallback OnLeaveGround;

    private Rigidbody m_rigibody;

    [SerializeField]
    private float m_gravity;

    public bool isFlying = false;

    private void Awake()
    {
        m_rigibody = FindObjectOfType<Rigidbody>();
        m_gravity = Physics.gravity.y;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.DownArrow))
        {
            m_rigibody.velocity = new Vector3(m_rigibody.velocity.x, m_rigibody.velocity.y - (2 * Time.deltaTime), m_rigibody.velocity.z);
            Physics.gravity = new Vector3(0f, -9.8f, 0f);
        }

        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            m_rigibody.velocity = new Vector3(m_rigibody.velocity.x  + Mathf.Abs( m_rigibody.velocity.y * 2), m_rigibody.velocity.y * 2, m_rigibody.velocity.z);
        }

        if (m_rigibody.velocity.x < 0.5f)
        {
            m_rigibody.velocity = new Vector3(0.5f, m_rigibody.velocity.y, m_rigibody.velocity.z);
        }

        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 4f, transform.position.z));

        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f) && hit.collider.CompareTag("Ground"))
        {
            if (isFlying)
                OnTouchGround();
            isFlying = false;
        }
        else
        {
            if (!isFlying)
                OnLeaveGround();
            isFlying = true;
        }



        if(m_rigibody.velocity.y > 0)
        {
            if(isFlying)
            {
                m_gravity -= 0.5f;
            }
            else
            {
                m_gravity = -2f;
            }
            Physics.gravity = new Vector3(0f, m_gravity, 0f);
        }

        print(m_rigibody.velocity.y);
    }

}
