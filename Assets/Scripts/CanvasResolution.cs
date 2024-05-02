using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasResolution : MonoBehaviour
{
    void Awake()
    {
        Screen.SetResolution(2340, 1080, true);
    }
}
