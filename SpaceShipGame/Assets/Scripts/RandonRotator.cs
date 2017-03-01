using UnityEngine;
using System.Collections;

public class RandonRotator : MonoBehaviour {

    public float tumble;

    private Rigidbody asteroid;

    void Start()
    {
        asteroid = GetComponent<Rigidbody>();
        asteroid.angularVelocity = Random.insideUnitSphere * tumble;
    }
}
