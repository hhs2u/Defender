using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Howling : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Warrior_Skill_Howling") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            Destroy(gameObject);
        }
    }
}
