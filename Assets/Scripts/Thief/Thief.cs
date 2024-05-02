using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thief : MonoBehaviour
{
    public GameObject[] NormalAttack;
    //스텟
    public float AttackDamage;
    public float attackdamage;
    public float maxAttackDelay;
    public float curAttackDelay;
    public float CriticalChance;
    public float CriticalMultiplier;

    Animator anim;
    public bool isTarget = false;
    public GameObject target;
    SpriteRenderer sprRen;
    Vector3 dist; //타겟과의 거리
    Vector2 Attack_dist; //타겟과 공격사이의 거리
    Vector3 dir; //타겟방향
    Quaternion targetRot;
    Vector3 NormalAttackPos; //기본공격 생성 위치
    float target_Angle; // 타겟과의 각도
    public bool ShotAnimation_End = true;
    public int NormalAttackNum = 0;

    //스킬 변수
    public int Skill1;
    public int Skill2;
    public float[] SkillDelay = new float[5] { 10, 15, 12, 10, 9 };
    public float maxSkill1Delay;
    public float curSkill1Delay;
    public float maxSkill2Delay;
    public float curSkill2Delay;
    public GameObject[] Poison;
    GameObject poison;
    public GameObject[] Assassination;
    GameObject assassination;
    public GameObject DamageUp;
    GameObject damageup_effect;
    public GameObject Focusing;
    GameObject focusing_effect;
    public GameObject[] Trap;
    GameObject trap;
    public Transform[] TrapPoints;
    int Point;

    GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprRen = GetComponent<SpriteRenderer>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Skill1 = -1;
        Skill2 = -1;
        attackdamage = AttackDamage;
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
            NormalAttackPos = new Vector3(transform.position.x, transform.position.y - 0.4f, 5);
        }
        else
        {
            sprRen.flipX = false;
            NormalAttackPos = new Vector3(transform.position.x, transform.position.y - 0.4f, 5);
        }

        if (isTarget)
        {
            Attack_dist = target.transform.position - NormalAttackPos;
            target_Angle = GetAngle(NormalAttackPos, target.transform.position);
            targetRot = Quaternion.Euler(new Vector3(0, 0, target_Angle + 180f));
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

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Thief_Attack") &&
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
            //기본공격생성
            GameObject Normalatk = Instantiate(NormalAttack[NormalAttackNum], NormalAttackPos, targetRot);
            if (NormalAttackNum == 0)
            {
                Attack atk = Normalatk.GetComponent<Attack>();
                if (Critical == true)
                {
                    atk.damage = AttackDamage * CriticalMultiplier;
                }
                else
                {
                    atk.damage = AttackDamage;
                }
            }
            else if (NormalAttackNum == 1)
            {
                Attack atk = Normalatk.GetComponent<Attack>();
                if (Critical == true)
                {
                    atk.damage = AttackDamage * 1.2f * CriticalMultiplier;
                }
                else
                {
                    atk.damage = AttackDamage * 1.2f;
                }
            }
            else if (NormalAttackNum == 2)
            {
                CCAttack atk = Normalatk.GetComponent<CCAttack>();
                if (Critical == true)
                {
                    atk.damage = AttackDamage * CriticalMultiplier;
                }
                else
                {
                    atk.damage = AttackDamage;
                }
                atk.duration = 5;
            }
            else if (NormalAttackNum == 3)
            {
                CCAttack atk = Normalatk.GetComponent<CCAttack>();
                if (Critical == true)
                {
                    atk.damage = AttackDamage * CriticalMultiplier;
                }
                else
                {
                    atk.damage = AttackDamage;
                }
                int poisonChance = Random.Range(0, 100) + 1;
                if (poisonChance <= 10)
                {
                    atk.duration = 3;
                    atk.dot = AttackDamage * 0.3f;
                }
            }


            //기본공격발사
            Rigidbody2D rigid = Normalatk.GetComponent<Rigidbody2D>();
            rigid.AddForce(Attack_dist.normalized * 15f, ForceMode2D.Impulse);
            anim.SetBool("isShot", false);
            ShotAnimation_End = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Thief_Attack") &&
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

        if (Skill1 == 16)
        {
            Skill_ThrowPoison();
        }
        else if (Skill1 == 17)
        {
            Skill_Assassination();
        }
        else if (Skill1 == 18)
        {
            Skill_DamageUpOn();
        }
        else if (Skill1 == 19)
        {
            Skill_FocusingOn();
        }
        else if (Skill1 == 20)
        {
            Skill_Trap();
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

        if (Skill2 == 16)
        {
            Skill_ThrowPoison();
        }
        else if (Skill2 == 17)
        {
            Skill_Assassination();
        }
        else if (Skill2 == 18)
        {
            Skill_DamageUpOn();
        }
        else if (Skill2 == 19)
        {
            Skill_FocusingOn();
        }
        else if (Skill2 == 20)
        {
            Skill_Trap();
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
            gamemanager.ChangeSkillManage(3, skill_num);
        }
        else if (Skill1 == -1)
        {
            Skill1 = skill_num;
            maxSkill1Delay = SkillDelay[skill_num - 16];
            curSkill1Delay = SkillDelay[skill_num - 16];
            gamemanager.SkillManagefinish();
        }
        else
        {
            Skill2 = skill_num;
            maxSkill2Delay = SkillDelay[skill_num - 16];
            curSkill2Delay = SkillDelay[skill_num - 16];
            gamemanager.SkillManagefinish();
        }
    }

    //스텟획득
    public void GetStatus(int num)
    {
        if (num == 0)
        {
            AttackDamage += 5f;
            attackdamage += 5f;
        }
        else if (num == 1)
        {
            maxAttackDelay *= 0.9f;
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
            maxSkill1Delay = SkillDelay[skill_num - 16];
            curSkill1Delay = SkillDelay[skill_num - 16];
        }
        else
        {
            Skill2 = skill_num;
            maxSkill2Delay = SkillDelay[skill_num - 16];
            curSkill2Delay = SkillDelay[skill_num - 16];
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
    void Skill_ThrowPoison()
    {
        GameObject poisongrenade = Instantiate(Poison[0], NormalAttackPos, targetRot);
        Rigidbody2D rigid = poisongrenade.GetComponent<Rigidbody2D>();
        rigid.AddForce(Attack_dist.normalized * 10f, ForceMode2D.Impulse);
    }

    public void spreadPoison(Vector3 Pos)
    {
        poison = Instantiate(Poison[1]);
        AreaCCAttack poisonAtk = poison.GetComponent<AreaCCAttack>();
        poisonAtk.damage = 0;
        poisonAtk.duration = 3;
        poisonAtk.dot = AttackDamage * 0.3f;
        poison.transform.position = Pos;
        Invoke("PoisonDestroy", 8f);
    }

    void PoisonDestroy()
    {
        ObjectDestroy destroypoison = poison.GetComponent<ObjectDestroy>();
        destroypoison.destroy();
    }

    void Skill_Assassination()
    {
        assassination = Instantiate(Assassination[0]);
        GameObject assassination_collision = Instantiate(Assassination[1]);
        CCAttack assassinationAtk = assassination_collision.GetComponent<CCAttack>();
        assassinationAtk.damage = AttackDamage * 10f;
        assassinationAtk.duration = 50;
        assassination.transform.position = target.transform.position;
        assassination_collision.transform.position = target.transform.position;
        Invoke("AssassinationDestroy", 1f);

    }

    void AssassinationDestroy()
    {
        ObjectDestroy assassinationDestroy = assassination.GetComponent<ObjectDestroy>();
        assassinationDestroy.destroy();
    }

    void Skill_DamageUpOn()
    {
        AttackDamage *= 2.0f;
        damageup_effect = Instantiate(DamageUp);
        damageup_effect.transform.position = new Vector3(transform.position.x, transform.position.y - 0.52f);
        Invoke("Skill_DamageUpOff", 6f);
    }

    void Skill_DamageUpOff()
    {
        AttackDamage = attackdamage;
        ObjectDestroy damageup_effectDestroy = damageup_effect.GetComponent<ObjectDestroy>();
        damageup_effectDestroy.destroy();
    }

    void Skill_FocusingOn()
    {
        CriticalChance += 10.0f;
        focusing_effect = Instantiate(Focusing);
        focusing_effect.transform.position = new Vector3(transform.position.x, transform.position.y + 1f);
        Invoke("Skill_FocusingOff", 7f);
    }

    void Skill_FocusingOff()
    {
        CriticalChance -= 10.0f;
        ObjectDestroy focusing_effectDestroy = focusing_effect.GetComponent<ObjectDestroy>();
        focusing_effectDestroy.destroy();
    }

    void Skill_Trap()
    {
        Point = Random.Range(0, 10);

        Vector2 TrapPoint_Dist = TrapPoints[Point].position - NormalAttackPos;
        float TrapPoint_Angle = GetAngle(NormalAttackPos, TrapPoints[Point].position);
        Quaternion TrapPointRot = Quaternion.Euler(new Vector3(0, 0, TrapPoint_Angle + 180f));

        trap = Instantiate(Trap[0], NormalAttackPos, TrapPointRot);
        Rigidbody2D rigid = trap.GetComponent<Rigidbody2D>();
        rigid.AddForce(TrapPoint_Dist.normalized * 2f, ForceMode2D.Impulse);
    }

    public void Skill_Trap_Install()
    {
        GameObject installed_trap = Instantiate(Trap[1]);
        CCAttack trapAtk = installed_trap.GetComponent<CCAttack>();
        trapAtk.damage = AttackDamage * 4f;
        trapAtk.duration = 3;
        installed_trap.transform.position = TrapPoints[Point].position;
    }

    //게임 리셋
    public void GameReset()
    {
        AttackDamage = 10f;
        attackdamage = 10f;
        maxAttackDelay = 0.8f;
        CriticalChance = 15f;
        SkillDelay = new float[5] { 10, 15, 12, 10, 9 };
        Skill1 = -1;
        Skill2 = -1;
        NormalAttackNum = 0;
        ShotAnimation_End = true;
    }
}
