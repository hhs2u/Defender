using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singijeon : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Archer_Skill_Singijeon_Appear") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            anim.SetBool("Finish", true);
        }
    }
}
