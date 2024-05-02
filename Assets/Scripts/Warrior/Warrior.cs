using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public GameObject[] NormalAttack;
    //스텟
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
    Vector3 dist; //타겟과의 거리
    public bool ShotAnimation_End = true;
    public int NormalAttackNum = 0;

    //스킬 변수
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

        //타겟 방향
        if (isTarget)
            dist = target.transform.position - transform.position;
        //기본 공격 시작 위치
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
            //기본 애니메이션으로 전환
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
        //샷 애니메이션으로 전환
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
            //치명타 계산
            int rand = Random.Range(0, 100) + 1;
            bool Critical = false;
            if (rand <= CriticalChance)
            {
                Critical = true;
            }
            //기본공격
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

    //스킬1 발사
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

    //스킬2 발사
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

    //기본공격 딜레이
    void Reload()
    {
        curAttackDelay += Time.deltaTime;
    }

    //스킬1 쿨타임
    void Reload_Skill1()
    {
        curSkill1Delay += Time.deltaTime;
    }

    //스킬2 쿨타임
    void Reload_Skill2()
    {
        curSkill2Delay += Time.deltaTime;
    }

    //공격범위, 타겟팅
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster" && isTarget == false)
        {
            isTarget = true;
            target = collision.gameObject;
            
        }
    }

    //타겟이 범위 밖으로 나갈시
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

    //스킬 획득시
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

    //스텟획득
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

    //스킬 2개 소지시 교체
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

    //기본공격전환
    public void NormalAttackChange(int num)
    {
        NormalAttackNum = num + 1;
        gamemanager.SkillManagefinish();
    }

    //스킬
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

    //게임 리셋
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
