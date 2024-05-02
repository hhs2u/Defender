using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Mage_Skill_Blizzard") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            Destroy(gameObject);
        }
    }
}
