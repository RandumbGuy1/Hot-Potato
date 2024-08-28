using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] KeyCode passKey;
    [SerializeField] SliderTimer timer;

    void Update()
    {
        if (Input.GetKeyDown(passKey))
        {
            timer.Finish();
        }
    }
}
