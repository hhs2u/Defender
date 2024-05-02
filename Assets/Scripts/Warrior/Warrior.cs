using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public GameObject[] NormalAttack;
    //����
    public float AttackDamage;
    public float maxAttackDelay;
    public float maxattackdelay;
    public float curAttackDelay;
    public float CriticalChance;
    public float CriticalMultiplier;

    Animator anim;
    public bool isTarget = false;
    public GameObject target;
    SpriteRenderer sprRen;
    Vector3 dist; //Ÿ�ٰ��� �Ÿ�
    public bool ShotAnimation_End = true;
    public int NormalAttackNum = 0;

    //��ų ����
    public int Skill1;
    public int Skill2;
    public float[] SkillDelay = new float[5] { 9, 10, 12, 8, 11 };
    public float maxSkill1Delay;
    public float curSkill1Delay;
    public float maxSkill2Delay;
    public float curSkill2Delay;
    public GameObject[] Swing;
    GameObject swing;
    public GameObject[] Strike;
    GameObject strikesword;
    GameObject strikeground;
    public GameObject RotateSword;
    GameObject[] rotatesword = new GameObject[3];
    bool Rotate = false;
    public float radius;
    float degree;
    public float rotateSpeed;
    public GameObject[] BreakDefend;
    GameObject breakdefend;
    public GameObject Howling;
    GameObject howling;
    bool NormalAttackSkill3 = false;
    float NormalAttackSkill3_Duration;

    GameManager gamemanager;
    Archer archer;
    Mage mage;
    Thief thief;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprRen = GetComponent<SpriteRenderer>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        archer = GameObject.Find("Archer").GetComponent<Archer>();
        mage = GameObject.Find("Mage").GetComponent<Mage>();
        thief = GameObject.Find("Thief").GetComponent<Thief>();
        Skill1 = -1;
        Skill2 = -1;
        maxattackdelay = maxAttackDelay;
    }

    // Update is called once per frame
    void Update()
    {

        //Ÿ�� ����
        if (isTarget)
            dist = target.transform.position - transform.position;
        //�⺻ ���� ���� ��ġ
        if (dist.x > 0)
        {
            sprRen.flipX = true;
        }
        else
        {
            sprRen.flipX = false;
        }

        Fire();
        Reload();
        Fire_SKill1();
        Reload_Skill1();
        Fire_SKill2();
        Reload_Skill2();


        if (!isTarget)
        {
            //�⺻ �ִϸ��̼����� ��ȯ
            anim.SetBool("isShot", false);
        }

        if (isTarget == false)
        {
            ShotAnimation_End = true;
        }

        if (Rotate == true) 
        {
            degree += Time.deltaTime * rotateSpeed;
            if (degree < 360)
            {
                for (int i = 0; i < rotatesword.Length; i++)
                {
                    var rad = Mathf.Deg2Rad * (degree + (i * (360 / rotatesword.Length)));
                    var x = radius * Mathf.Sin(rad);
                    var y = radius * Mathf.Cos(rad);
                    rotatesword[i].transform.position = transform.position + new Vector3(x, y);
                    rotatesword[i].transform.rotation = Quaternion.Euler(0, 0, (degree + (i * (360 / rotatesword.Length))) * -1);
                }

            }
            else
            {
                degree = 0;
            }
        }

        NormalAttackSkill3_Duration -= Time.deltaTime;
        if (NormalAttackSkill3_Duration <= 0 && NormalAttackSkill3 == true)
        {
            NormalAttackSkill3 = false;
            maxAttackDelay = maxattackdelay;
        }
    }

    void Fire()
    {
        if (!isTarget || curAttackDelay < maxAttackDelay)
        {
            return;
        }
        //�� �ִϸ��̼����� ��ȯ
        if (ShotAnimation_End == true)
        {
            anim.SetBool("isShot", true);
            ShotAnimation_End = false;
        }

        curAttackDelay = 0;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Warrior_Attack") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f
                && ShotAnimation_End == false)
        {
            //ġ��Ÿ ���
            int rand = Random.Range(0, 100) + 1;
            bool Critical = false;
            if (rand <= CriticalChance)
            {
                Critical = true;
            }
            //�⺻����
            MonsterStatus targetStatus = target.GetComponent<MonsterStatus>();
            Monster targetMonster = target.GetComponent<Monster>();
            if (Critical == true)
            {
                targetStatus.OnHit(AttackDamage * 2f);
            }
            else
            {
                targetStatus.OnHit(AttackDamage);
            }

            if (NormalAttackNum == 1)
            {
                int StunChance = Random.Range(0, 100) + 1;
                if (StunChance <= 15)
                {
                    targetMonster.Stun(3);
                }
            }
            else if (NormalAttackNum == 2)
            {
                int CooltimeChance = Random.Range(0, 100) + 1;
                if (CooltimeChance <= 3)
                {
                    curSkill1Delay += 1;
                    curSkill2Delay += 1;
                }
            }
            else if (NormalAttackNum == 3)
            {
                int AttackspeedChance = Random.Range(0, 100) + 1;
                if (AttackspeedChance <= 3)
                {
                    NormalAttackSkill3_Duration = 5;
                    if(NormalAttackSkill3 == false)
                    {
                        maxAttackDelay *= 0.7f;
                        NormalAttackSkill3 = true;
                    }
                }
            }

            anim.SetBool("isShot", false);
            ShotAnimation_End = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Warrior_Attack") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            anim.SetBool("isShot", false);
        }
    }

    //��ų1 �߻�
    void Fire_SKill1()
    {
        if (!isTarget || curSkill1Delay < maxSkill1Delay || Skill1 == -1)
        {
            return;
        }
        curSkill1Delay = 0;

        if (Skill1 == 24)
        {
            Skill_Swing();
        }
        else if (Skill1 == 25)
        {
            Skill_Strike();
        }
        else if (Skill1 == 26)
        {
            Skill_RotateSword();
        }
        else if (Skill1 == 27)
        {
            Skill_BreakDefend();
        }
        else if (Skill1 == 28)
        {
            Skill_Howling();
        }
    }

    //��ų2 �߻�
    void Fire_SKill2()
    {
        if (!isTarget || curSkill2Delay < maxSkill2Delay || Skill2 == -1)
        {
            return;
        }
        curSkill2Delay = 0;

        if (Skill2 == 24)
        {
            Skill_Swing();
        }
        else if (Skill2 == 25)
        {
            Skill_Strike();
        }
        else if (Skill2 == 26)
        {
            Skill_RotateSword();
        }
        else if (Skill2 == 27)
        {
            Skill_BreakDefend();
        }
        else if (Skill2 == 28)
        {
            Skill_Howling();
        }
    }

    //�⺻���� ������
    void Reload()
    {
        curAttackDelay += Time.deltaTime;
    }

    //��ų1 ��Ÿ��
    void Reload_Skill1()
    {
        curSkill1Delay += Time.deltaTime;
    }

    //��ų2 ��Ÿ��
    void Reload_Skill2()
    {
        curSkill2Delay += Time.deltaTime;
    }

    //���ݹ���, Ÿ����
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster" && isTarget == false)
        {
            isTarget = true;
            target = collision.gameObject;
            
        }
    }

    //Ÿ���� ���� ������ ������
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            if (target == collision.gameObject)
            {
                isTarget = false;
            }
        }
    }

    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    //��ų ȹ���
    public void GetSkill(int skill_num)
    {
        if (Skill1 != -1 && Skill2 != -1)
        {
            gamemanager.ChangeSkillManage(4, skill_num);
        }
        else if (Skill1 == -1)
        {
            Skill1 = skill_num;
            maxSkill1Delay = SkillDelay[skill_num - 24];
            curSkill1Delay = SkillDelay[skill_num - 24];
            gamemanager.SkillManagefinish();
        }
        else
        {
            Skill2 = skill_num;
            maxSkill2Delay = SkillDelay[skill_num - 24];
            curSkill2Delay = SkillDelay[skill_num - 24];
            gamemanager.SkillManagefinish();
        }
    }

    //����ȹ��
    public void GetStatus(int num)
    {
        if (num == 0)
        {
            AttackDamage += 5f;
        }
        else if (num == 1)
        {
            maxAttackDelay *= 0.9f;
            maxattackdelay *= 0.9f;
        }
        else if (num == 2)
        {
            maxSkill1Delay *= 0.9f;
            maxSkill2Delay *= 0.9f;
            for (int i = 0; i < SkillDelay.Length; i++)
            {
                SkillDelay[i] *= 0.9f;
            }
        }
        else if (num == 3)
        {
            CriticalChance += 10f;
        }
        gamemanager.SkillManagefinish();
    }

    //��ų 2�� ������ ��ü
    public void ChangeSkill(int skill_num, int num)
    {
        gamemanager.UI[5].SetActive(false);
        if (num == 1)
        {
            Skill1 = skill_num;
            maxSkill1Delay = SkillDelay[skill_num - 24];
            curSkill1Delay = SkillDelay[skill_num - 24];
        }
        else
        {
            Skill2 = skill_num;
            maxSkill2Delay = SkillDelay[skill_num - 24];
            curSkill2Delay = SkillDelay[skill_num - 24];
        }
        gamemanager.SkillManagefinish();
    }

    //�⺻������ȯ
    public void NormalAttackChange(int num)
    {
        NormalAttackNum = num + 1;
        gamemanager.SkillManagefinish();
    }

    //��ų
    void Skill_Swing()
    {
        swing = Instantiate(Swing[0]);
        GameObject Swing_collision = Instantiate(Swing[1]);

        int rand = Random.Range(0, 100) + 1;
        bool Critical = false;
        if (rand <= CriticalChance)
        {
            Critical = true;
        }

        Attack swingAtk = Swing_collision.GetComponent<Attack>();
        if (Critical == true)
        {
            swingAtk.damage = AttackDamage * 4f * CriticalMultiplier;
        }
        else
        {
            swingAtk.damage = AttackDamage * 4f;
        }
        swing.transform.position = target.transform.position;
        Swing_collision.transform.position = target.transform.position;
        Invoke("SwingDestroy", 0.7f);
    }

    void SwingDestroy()
    {
        ObjectDestroy swingDestroy = swing.GetComponent<ObjectDestroy>();
        swingDestroy.destroy();
    }

    void Skill_Strike()
    {
        strikesword = Instantiate(Strike[0]);
        strikesword.transform.position = target.transform.position;
        Invoke("StrikeGround", 0.7f);
    }

    void StrikeGround()
    {
        strikeground = Instantiate(Strike[1]);
        GameObject strikeground_collision = Instantiate(Strike[2]);
        int rand = Random.Range(0, 100) + 1;
        bool Critical = false;
        if (rand <= CriticalChance)
        {
            Critical = true;
        }

        CCAttack strikeAtk = strikeground_collision.GetComponent<CCAttack>();
        strikeAtk.duration = 3;
        if (Critical == true)
        {
            strikeAtk.damage = AttackDamage * 3f * CriticalMultiplier;
        }
        else
        {
            strikeAtk.damage = AttackDamage * 3f;
        }
        strikeground.transform.position = strikesword.transform.position;
        strikeground_collision.transform.position = strikesword.transform.position;

        Invoke("StrikeDestroy", 1f);
    }

    void StrikeDestroy()
    {
        ObjectDestroy strikeswordDestroy = strikesword.GetComponent<ObjectDestroy>();
        strikeswordDestroy.destroy();
        ObjectDestroy strikegroundDestroy = strikeground.GetComponent<ObjectDestroy>();
        strikegroundDestroy.destroy();
    }

    void Skill_RotateSword()
    {
        for (int i = 0; i < 3; i++)
        {
            rotatesword[i] = Instantiate(RotateSword);
            PenetratingAttack swordAtk = rotatesword[i].GetComponent<PenetratingAttack>();
            swordAtk.damage = AttackDamage * 1.5f;
        }
        Rotate = true;
        Invoke("RotateSwordEnd", 8f);
    }

    void RotateSwordEnd()
    {
        for (int i = 0; i < 3; i++)
        {
            ObjectDestroy swordDestroy = rotatesword[i].GetComponent<ObjectDestroy>();
            swordDestroy.destroy();
        }
        Rotate = false;
    }

    void Skill_BreakDefend()
    {
        breakdefend = Instantiate(BreakDefend[0]);
        GameObject breakdefend_collision = Instantiate(BreakDefend[1]);

        int rand = Random.Range(0, 100) + 1;
        bool Critical = false;
        if (rand <= CriticalChance)
        {
            Critical = true;
        }

        CCAttack breakdefendAtk = breakdefend_collision.GetComponent<CCAttack>();
        breakdefendAtk.duration = 3;
        if (Critical == true)
        {
            breakdefendAtk.damage = AttackDamage * 2f * CriticalMultiplier;
        }
        else
        {
            breakdefendAtk.damage = AttackDamage * 2f;
        }

        breakdefend.transform.position = target.transform.position;
        breakdefend_collision.transform.position = target.transform.position;
        Invoke("BreakDefendDestroy", 0.7f);
    }

    void BreakDefendDestroy()
    {
        ObjectDestroy breakdefendDestroy = breakdefend.GetComponent<ObjectDestroy>();
        breakdefendDestroy.destroy();
    }

    void Skill_Howling()
    {
        howling = Instantiate(Howling);
        if (sprRen.flipX == false)
        {
            howling.transform.position = new Vector3(transform.position.x + 0.15f, transform.position.x + 0.12f);
        }
        else
        {
            howling.transform.position = new Vector3(transform.position.x - 0.18f, transform.position.x + 0.12f);
        }
        archer.curSkill1Delay += 1;
        archer.curSkill2Delay += 1;
        mage.curSkill1Delay += 1;
        mage.curSkill2Delay += 1;
        thief.curSkill1Delay += 1;
        thief.curSkill2Delay += 1;
    }

    //���� ����
    public void GameReset()
    {
        AttackDamage = 20f;
        maxAttackDelay = 0.8f;
        maxattackdelay = 0.8f;
        CriticalChance = 10f;
        SkillDelay = new float[5] { 9, 10, 12, 8, 11 };
        Skill1 = -1;
        Skill2 = -1;
        NormalAttackNum = 0;
        ShotAnimation_End = true;
    }
}
