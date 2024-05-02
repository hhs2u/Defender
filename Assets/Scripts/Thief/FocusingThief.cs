using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusingThief : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Thief_Skill_Focusing") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            gameObject.SetActive(false);
        }
    }
}
