using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Explosion : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Meteor_Explosion") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            Destroy(gameObject);
        }
    }
}
