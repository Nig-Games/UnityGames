using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    private Rigidbody boltRb;
    public float speed;

	// Use this for initialization
	void Start () {
        boltRb = GetComponent<Rigidbody>();
        boltRb.velocity = transform.forward * speed;
    }
}
