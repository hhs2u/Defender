using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCCAttack : MonoBehaviour
{
    public float damage;
    public int duration;
    public float dot;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Clear")
        {
            Destroy(gameObject);
        }
    }
}
