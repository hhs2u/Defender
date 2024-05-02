using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderAttack" || collision.gameObject.tag == "Monster" || collision.gameObject.tag == "Clear")
        {
            Destroy(gameObject);
        }
    }
}
