using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class TestCube : Interactable
    {
        public float jumpForce = 10f;
        public override void Interact()
        {
            Rigidbody r = GetComponent<Rigidbody>();
            r.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
        }
    }
}