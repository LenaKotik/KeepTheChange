using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Button : Interactable
{
    public List<ButtonListener> subscribers;
    public string Arg;

    public float AnimTime = 1f; // milliseconds
    public float AnimDelay = 0.2f; // time in between button movements
    public float AnimAmplitude = 1f;
    public bool igonreOnAnim = true;

    bool isAnim = false;
    float AnimStartTime;
    Vector3 startPos;
    
   float AnimFunction(float t)
    {
        float partTime = (AnimTime - AnimDelay) / 2;
        float offset;
        if (t <= partTime)  // increasing offset
            offset = t / partTime * AnimAmplitude;
        else if (t <= partTime + AnimDelay) // const offset
            offset = AnimAmplitude;
        else // decreasing offset
            offset = (1-(t - (partTime + AnimDelay)) / partTime) * AnimAmplitude;

        return offset;
    }
    private void Start()
    {
        startPos = transform.position;
    }
    public override void Interact()
    {
        if (igonreOnAnim && isAnim) return;
        foreach (ButtonListener s in subscribers) s.OnPressed(Arg);
        // TODO: play a sound
        AnimStartTime = Time.realtimeSinceStartup;
        isAnim = true;
    }
    private void Update()
    {
        if (!isAnim) return;

        float t = Time.realtimeSinceStartup - AnimStartTime;
        if (t > AnimTime)
        { 
            isAnim = false;
            return;
        }
        transform.position = startPos - transform.up * AnimFunction(t);
    }
}
