using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float porrazabeu;

    Vector3 vec;

	void Update () {
        vec = new Vector3(player.transform.position.x, player.transform.position.y+porrazabeu, -12.74f);
        
        transform.position = vec;
	}
}
