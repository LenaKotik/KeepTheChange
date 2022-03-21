using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TestCubeToLookAt : Inspectable
{
    public Color color = Color.white;

    private void Start() => GetComponent<Renderer>().material.color = color;
}
