using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace Interactables
{
    public sealed class TestCube : Interactable
    {
        public float jumpForce = 10f;
        public Color color = Color.white;
        private void Start()
        {
            Material? m = GetComponent<Renderer>()?.material;
            if (m == null) return;
            m.color = color;
        }
        public override void Interact()
        {
            Rigidbody r = GetComponent<Rigidbody>();
            r.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
        }
    }
}