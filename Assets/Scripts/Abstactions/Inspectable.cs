using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public struct TransformSnapshot
{
    public TransformSnapshot(Vector3 P, Quaternion R)
    {
        Position = P;
        Rotation = R;
    }
    public Vector3 Position;
    public Quaternion Rotation;
}
public class Inspectable : MonoBehaviour
{
    TransformSnapshot? snapshot = null;
    public void SetUp(bool Return = false)
    {
        if (!Return)
            snapshot = new TransformSnapshot(transform.position, transform.rotation);
        else
        {
            if (snapshot == null) throw new System.InvalidOperationException("No state to return to");
            transform.position = snapshot.Value.Position;
            transform.rotation = snapshot.Value.Rotation;
            snapshot = null;
        }
        Rigidbody? r = GetComponent<Rigidbody>();
        if (r != null)
            r.isKinematic = !Return;
        Collider? c = GetComponent<Collider>();
        if (c != null)
            c.enabled = Return;
    }
}
