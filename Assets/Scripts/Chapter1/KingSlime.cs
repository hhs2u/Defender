using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingSlime : MonoBehaviour
{
    public GameObject MiniKingSlime;
    Monster_KingSlime monster;
    MonsterStatus monsterstatus;

    void Start()
    {
        monster = GetComponent<Monster_KingSlime>();
        monsterstatus = GetComponent<MonsterStatus>();
    }

    public void divide()
    {
        GameObject minikingslime1 = Instantiate(MiniKingSlime);
        GameObject minikingslime2 = Instantiate(MiniKingSlime);
        Monster slime1 = minikingslime1.GetComponent<Monster>();
        Monster slime2 = minikingslime2.GetComponent<Monster>();
        if (monster.MovePoint == 0 || monster.MovePoint == 2 || monster.MovePoint == 4)
        {
            minikingslime1.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y);
            minikingslime2.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y);
        }
        else
        {
            minikingslime1.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f);
            minikingslime2.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f);
        }
        slime1.MovePoint = monster.MovePoint;
        slime2.MovePoint = monster.MovePoint;
    }
}
