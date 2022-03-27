using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public Vector3 torq;
    Rigidbody R;
    void Start()
    {
        R = GetComponent<Rigidbody>();
    }
    void Update()
    {
        R.AddTorque(torq, ForceMode.Impulse);
    }
}
