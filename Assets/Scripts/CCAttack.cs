using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAttack : MonoBehaviour
{
    public float damage;
    public int duration;
    public float dot;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderAttack" || collision.gameObject.tag == "Monster" || collision.gameObject.tag == "Clear")
        {
            Destroy(gameObject);
        }
    }
}
