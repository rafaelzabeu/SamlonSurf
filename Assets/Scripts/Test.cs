using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject go = new GameObject("Sin");
        GraphBehaviour gb = go.AddComponent<GraphBehaviour>();
        gb.Graph(0, 30, (x) => { return Mathf.Sin(x); });
	}
    


}
