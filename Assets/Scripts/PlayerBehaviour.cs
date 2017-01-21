using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public delegate void PlayerCallback();
    public event PlayerCallback OnTouchGround;
    public event PlayerCallback OnLeaveGround;

    public delegate void PlayerGoingDownCallback(bool goingDown);
    public event PlayerGoingDownCallback OnPlayerChangesOrientation;

    public bool isFlying = false;

    public bool isGoingDown = true;

    public float oldXPos;
    public float oldYPos;

    private Rigidbody m_rigibody;

    [SerializeField]
    private bool m_canBoost = false;

    [SerializeField]
    private float m_gravity;

    

    private Transform m_transform;

    private void Awake()
    {
        m_transform = transform;
        m_rigibody = FindObjectOfType<Rigidbody>();
        m_gravity = Physics.gravity.y;
        oldYPos = m_transform.position.y;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_rigibody.velocity = new Vector3(m_rigibody.velocity.x, m_rigibody.velocity.y - (2 * Time.deltaTime), m_rigibody.velocity.z);
            Physics.gravity = new Vector3(0f, -9.8f, 0f);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) && m_canBoost)
        {
            m_rigibody.velocity = new Vector3(m_rigibody.velocity.x + Mathf.Abs(m_rigibody.velocity.y * 2), m_rigibody.velocity.y * 2, m_rigibody.velocity.z);
        }

        if (m_rigibody.velocity.x < 0.5f)
        {
            m_rigibody.velocity = new Vector3(0.5f, m_rigibody.velocity.y, m_rigibody.velocity.z);
        }

        Debug.DrawLine(m_transform.position, new Vector3(m_transform.position.x, m_transform.position.y - 4f, m_transform.position.z));

        RaycastHit hit;

        if (Physics.Raycast(m_transform.position, Vector3.down, out hit, 0.1f) && hit.collider.CompareTag("Ground"))
        {
            if (isFlying)
            {
                OnTouchGround();
                m_canBoost = true;
            }
            isFlying = false;
        }
        else
        {
            if (!isFlying)
            {
                OnLeaveGround();
                m_canBoost = false;
            }
            isFlying = true;
        }



        if (m_rigibody.velocity.y > 0)
        {
            if (isFlying)
            {
                m_gravity -= 0.5f;
            }
            else
            {
                m_gravity = -2f;
            }
            Physics.gravity = new Vector3(0f, m_gravity, 0f);
        }

        if (oldYPos < m_transform.position.y)
        {
            if (isGoingDown)
                OnPlayerChangesOrientation(false);
            isGoingDown = false;
        }
        else
        {
            if (!isGoingDown)
                OnPlayerChangesOrientation(true);
            isGoingDown = true;
        }

        oldYPos = transform.position.y;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Unstucker"))
            m_rigibody.velocity = new Vector3(m_rigibody.velocity.x + 2f, m_rigibody.velocity.y + 2f, m_rigibody.velocity.z);
    }
}
