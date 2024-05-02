using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap1 : MonoBehaviour
{
    Thief thief;

    void Start()
    {
        thief = GameObject.Find("Thief").GetComponent<Thief>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TrapPoint")
        {
            thief.Skill_Trap_Install();
            Destroy(gameObject);
        }
    }
}
