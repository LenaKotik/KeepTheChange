using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

public class TestCubeWButton : ButtonListener
{
    public float jumpForce = 10f;
    public Color color = Color.white;
    private void Start()
    {
        Material? m = GetComponent<Renderer>()?.material;
        if (m == null) return;
        m.color = color;
    }
    public override void OnPressed(string Arg)
    {

        int multiplier;
        if(!int.TryParse(Arg, out multiplier))multiplier=1;
        GetComponent<Rigidbody>()?.AddForce(Vector3.up * jumpForce * multiplier, ForceMode.Impulse);
    }
}
