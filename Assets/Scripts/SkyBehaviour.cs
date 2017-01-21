using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBehaviour : MonoBehaviour {

    public PlayerBehaviour playerBehaviour;

    public float yMin;
    public float yMax;

    public float xMin;
    public float xMax;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = playerBehaviour.transform;
    }

    private void Update()
    {

    }

}
