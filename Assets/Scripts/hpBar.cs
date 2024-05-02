using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpBar : MonoBehaviour
{
    public Transform player;
    public Slider hpbar;
    MonsterStatus MS;

    // Start is called before the first frame update
    void Start()
    {
        hpbar = GetComponent<Slider>();
        player = GetComponent<Transform>();
        MS = GetComponent<MonsterStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + new Vector3(0, 0.5f, 0);
        hpbar.value = (float)MS.nowhp / (float)MS.maxhp;
    }
}
