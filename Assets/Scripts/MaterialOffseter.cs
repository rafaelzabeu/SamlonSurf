using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOffseter : MonoBehaviour {

    public float offsetPerSec;

    private Material m_material;

    private Vector2 m_offset;

    private void Start()
    {
       m_material = GetComponent<Renderer>().material;
       m_offset = m_material.mainTextureOffset;
    }

    private void Update()
    {
        m_offset = new Vector2(m_offset.x + offsetPerSec * Time.deltaTime, m_offset.y);
        if(m_offset.x > 250)
        {
            m_offset.x = 0;
        }
        m_material.mainTextureOffset = m_offset; 
    }

}
