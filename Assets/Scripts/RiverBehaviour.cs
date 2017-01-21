using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverBehaviour : MonoBehaviour {

    public RiverBehaviour otherRiver;
    public Transform marker;

    public void ChangePlace()
    {
        Vector3 mk = otherRiver.marker.transform.position;
        mk.z = transform.position.z;
        transform.position = mk;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            otherRiver.ChangePlace();
        }
    }

}
