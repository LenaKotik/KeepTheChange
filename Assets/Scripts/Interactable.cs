using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public abstract class Interactable : MonoBehaviour
{
    public float highlightIntensity = 2f;
    public abstract void Interact();
    
}
