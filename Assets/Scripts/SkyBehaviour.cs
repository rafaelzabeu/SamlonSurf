using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkyBehaviour : MonoBehaviour {

    [HideInInspector]
    public GameManager gameManager;

    public PlayerBehaviour playerBehaviour;

    public SpriteRenderer gabeTheGod;

    public float yMin;
    public float yMax;

    private Transform playerTransform;

    private Material m_material;

    private Vector2 m_offset;

    //private float preCalculatedEpsilon = float.Epsilon * 100;

    private bool m_isShowingGabe = false;

    private void Awake()
    {
        playerTransform = playerBehaviour.transform;
        m_material = GetComponent<Renderer>().material;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        Color c = gabeTheGod.color;
        c.a = 0;
        gabeTheGod.color = c;
    }


    private void Update()
    {
        m_material.mainTextureOffset = new Vector2(getOffsetX(), getOffsetY());
        if (m_material.mainTextureOffset.y >= yMax - yMax * 0.1)
        {
            if (!m_isShowingGabe)
            {
                StopAllCoroutines();
                StartCoroutine(showGabe(true));
                m_isShowingGabe = true;
            }
        }
        else
        {
            if (m_isShowingGabe)
            {
                StopAllCoroutines();
                StartCoroutine(showGabe(false));
                m_isShowingGabe = false;
            }
        }
    }

    private float getOffsetX()
    {
        float result = m_material.mainTextureOffset.x + (playerTransform.position.x - playerBehaviour.oldXPos) * Time.deltaTime;
        //if (Mathf.Abs(result % 1) <= preCalculatedEpsilon)
        //    result = 0;
        return result;
    }

    private float getOffsetY()
    {
        float result = m_material.mainTextureOffset.y + (playerTransform.position.y - playerBehaviour.oldYPos) * Time.deltaTime;
        result = Mathf.Clamp(result, yMin, yMax);
        gameManager.CalculateMultiplyer(result);
        return result;
    }

    private IEnumerator showGabe(bool show)
    {
        Color c = gabeTheGod.color;
        float start = c.a;
        float end = show ? 1 : 0;
        float time = 0;

        while (time < 0.5f)
        {
            c.a = Mathf.Lerp(start, end, time * 2);
            gabeTheGod.color = c;
            time += Time.deltaTime;
            yield return null;
        }

        gameManager.OnGabeFound();
    }
}
