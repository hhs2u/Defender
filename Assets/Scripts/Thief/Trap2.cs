using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap2 : MonoBehaviour
{
    void Start()
    {
        Invoke("Destroy", 30f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
