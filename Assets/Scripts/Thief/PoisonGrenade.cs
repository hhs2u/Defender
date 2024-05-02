using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGrenade : MonoBehaviour
{
    Thief thief;

    void Start()
    {
        thief = GameObject.Find("Thief").GetComponent<Thief>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Monster")
        {
            thief.spreadPoison(collision.gameObject.transform.position);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "BorderAttack")
        {
            Destroy(gameObject);
        }
    }
}
