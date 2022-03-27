using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGUN : Pickable
{
    public GameObject Projectile;
    public float Force = 5f;
    public override void Use()
    {
        GameObject p = Instantiate(Projectile, transform.position + transform.forward*0.5f, new Quaternion());
        p.GetComponent<Rigidbody>().AddForce(transform.forward * Force, ForceMode.Impulse);
    }
}
