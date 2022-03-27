using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    [NonSerialized]
    public bool IsHeld;
    public float AnimTime = 1f;
    public float PlacementDistance = 3f;

    bool isAnim = false;
    float AnimStartime;
    Vector3 AnimDir;
    Vector3 AnimStart;
    public abstract void Use(); // M1 while held

    /// <returns>true if placed successfully, false otherwise</returns>
    public bool Place() // M2 while held
    {
        if (!IsHeld) throw new InvalidOperationException("Can't be placed while not held");

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit res = new RaycastHit();
        if (!Physics.Raycast(r, out res, PlacementDistance)) return false;
        AnimStartime = Time.realtimeSinceStartup;
        AnimStart = transform.position;
        AnimDir = res.point - transform.position;
        
        return true;
    }
    void Update()
    {
        if (!isAnim) return;

        float t = Time.realtimeSinceStartup;
        t -= AnimStartime;
        if (t > AnimTime)
        {
            isAnim = false;
            return;
        }
        t = Mathf.Max(t, 0.001f);
        transform.position = AnimStart + AnimDir * t;
    }
}
