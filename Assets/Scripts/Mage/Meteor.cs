using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    Mage mage;
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        mage = GameObject.Find("Mage").GetComponent<Mage>();
        target = mage.target;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            mage.SKill_Meteor_Explosion(gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
