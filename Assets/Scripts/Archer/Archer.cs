using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Archer : MonoBehaviour
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
    public float[] SkillDelay = new float[5] { 10, 10, 12, 10, 13 };
    public float maxSkill1Delay;
    public float curSkill1Delay;
    public float maxSkill2Delay;
    public float curSkill2Delay;
    bool TripleShotOnOff;
    public GameObject FastShot_Effect;
    public GameObject Focusing_Effect;
    public GameObject Singijeon;
    public GameObject HugeArrow;
    GameObject fastshot_effect;
    GameObject focusing_effect;
    GameObject singijeon;
    int singijeon_count;

    GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprRen = GetComponent<SpriteRenderer>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Skill1 = -1;
        Skill2 = -1;
        TripleShotOnOff = false;
        maxattackdelay = maxAttackDelay;
    }

    // Update is called once per frame
    void Update()
    {

        //타겟 방향
        if(isTarget)
            dist = target.transform.position - transform.position;
        //기본 공격 시작 위치
        if (dist.x > 0)
        {
            sprRen.flipX = true;
            NormalAttackPos = new Vector3(transform.position.x + 0.5f, transform.position.y, 5);
        }
        else
        {
            sprRen.flipX = false;
            NormalAttackPos = new Vector3(transform.position.x - 0.5f, transform.position.y, 5);
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

    //기본 공격 발사
    void Fire()
    {
        if(!isTarget || curAttackDelay < maxAttackDelay)
        {
            return;
        }
        //샷 애니메이션으로 전환
        if(ShotAnimation_End == true)
        {
            anim.SetBool("isShot", true);
            ShotAnimation_End = false;
        }

        curAttackDelay = 0;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Archer_Attack") &&
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

            if (TripleShotOnOff == true) //트리플샷 온
            {
                //화살생성
                GameObject arrow1 = Instantiate(NormalAttack[NormalAttackNum], NormalAttackPos, targetRot);
                arrow1.transform.position = new Vector3(NormalAttackPos.x, NormalAttackPos.y + 0.1f);
                GameObject arrow2 = Instantiate(NormalAttack[NormalAttackNum], NormalAttackPos, targetRot);
                GameObject arrow3 = Instantiate(NormalAttack[NormalAttackNum], NormalAttackPos, targetRot);
                arrow3.transform.position = new Vector3(NormalAttackPos.x, NormalAttackPos.y - 0.1f);
                if (NormalAttackNum == 0)
                {
                    Attack atk1 = arrow1.GetComponent<Attack>();
                    Attack atk2 = arrow2.GetComponent<Attack>();
                    Attack atk3 = arrow3.GetComponent<Attack>();
                    if (Critical == true)
                    {
                        atk1.damage = AttackDamage * CriticalMultiplier;
                        atk2.damage = AttackDamage * CriticalMultiplier;
                        atk3.damage = AttackDamage * CriticalMultiplier;
                    }
                    else
                    {
                        atk1.damage = AttackDamage;
                        atk2.damage = AttackDamage;
                        atk3.damage = AttackDamage;
                    }
                }
                else if (NormalAttackNum == 3)
                {
                    PenetratingAttack atk1 = arrow1.GetComponent<PenetratingAttack>();
                    PenetratingAttack atk2 = arrow2.GetComponent<PenetratingAttack>();
                    PenetratingAttack atk3 = arrow2.GetComponent<PenetratingAttack>();
                    if (Critical == true)
                    {
                        atk1.damage = AttackDamage * CriticalMultiplier;
                        atk2.damage = AttackDamage * CriticalMultiplier;
                        atk3.damage = AttackDamage * CriticalMultiplier;
                    }
                    else
                    {
                        atk1.damage = AttackDamage;
                        atk2.damage = AttackDamage;
                        atk3.damage = AttackDamage;
                    }

                }
                else if (NormalAttackNum == 1)
                {
                    CCAttack atk1 = arrow1.GetComponent<CCAttack>();
                    atk1.duration = 3;
                    atk1.dot = AttackDamage * 0.5f;
                    CCAttack atk2 = arrow2.GetComponent<CCAttack>();
                    atk2.duration = 3;
                    atk2.dot = AttackDamage * 0.5f;
                    CCAttack atk3 = arrow3.GetComponent<CCAttack>();
                    atk3.duration = 3;
                    atk3.dot = AttackDamage * 0.5f;
                    if (Critical == true)
                    {
                        atk1.damage = AttackDamage * CriticalMultiplier;
                        atk2.damage = AttackDamage * CriticalMultiplier;
                        atk3.damage = AttackDamage * CriticalMultiplier;
                    }
                    else
                    {
                        atk1.damage = AttackDamage;
                        atk2.damage = AttackDamage;
                        atk3.damage = AttackDamage;
                    }
                }
                else
                {
                    CCAttack atk1 = arrow1.GetComponent<CCAttack>();
                    atk1.duration = 3;
                    atk1.dot = AttackDamage * 0.3f;
                    CCAttack atk2 = arrow2.GetComponent<CCAttack>();
                    atk2.duration = 3;
                    atk2.dot = AttackDamage * 0.3f;
                    CCAttack atk3 = arrow3.GetComponent<CCAttack>();
                    atk3.duration = 3;
                    atk3.dot = AttackDamage * 0.3f;
                    if (Critical == true)
                    {
                        atk1.damage = AttackDamage * CriticalMultiplier;
                        atk2.damage = AttackDamage * CriticalMultiplier;
                        atk3.damage = AttackDamage * CriticalMultiplier;
                    }
                    else
                    {
                        atk1.damage = AttackDamage;
                        atk2.damage = AttackDamage;
                        atk3.damage = AttackDamage;
                    }
                }

                //화살발사
                Rigidbody2D rigid1 = arrow1.GetComponent<Rigidbody2D>();
                rigid1.AddForce(Attack_dist.normalized * 15f, ForceMode2D.Impulse);
                Rigidbody2D rigid2 = arrow2.GetComponent<Rigidbody2D>();
                rigid2.AddForce(Attack_dist.normalized * 15f, ForceMode2D.Impulse);
                Rigidbody2D rigid3 = arrow3.GetComponent<Rigidbody2D>();
                rigid3.AddForce(Attack_dist.normalized * 15f, ForceMode2D.Impulse);

                anim.SetBool("isShot", false);
                ShotAnimation_End = true;
            }
            else //트리플샷 오프
            {
                //화살생성
                GameObject arrow = Instantiate(NormalAttack[NormalAttackNum], NormalAttackPos, targetRot);
                if (NormalAttackNum == 0)
                {
                    Attack atk = arrow.GetComponent<Attack>();
                    if (Critical == true)
                    {
                        atk.damage = AttackDamage * CriticalMultiplier;
                    }
                    else
                    {
                        atk.damage = AttackDamage;
                    }
                }
                else if (NormalAttackNum == 3)
                {
                    PenetratingAttack atk = arrow.GetComponent<PenetratingAttack>();
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
                    CCAttack atk = arrow.GetComponent<CCAttack>();
                    atk.duration = 3;
                    atk.dot = AttackDamage * 0.5f;
                    if (Critical == true)
                    {
                        atk.damage = AttackDamage * CriticalMultiplier;
                    }
                    else
                    {
                        atk.damage = AttackDamage;
                    }
                }
                else
                {
                    CCAttack atk = arrow.GetComponent<CCAttack>();
                    atk.duration = 3;
                    atk.dot = AttackDamage * 0.3f;
                    if (Critical == true)
                    {
                        atk.damage = AttackDamage * CriticalMultiplier;
                    }
                    else
                    {
                        atk.damage = AttackDamage;
                    }
                }

                //화살발사
                Rigidbody2D rigid = arrow.GetComponent<Rigidbody2D>();
                rigid.AddForce(Attack_dist.normalized * 15f, ForceMode2D.Impulse);

                anim.SetBool("isShot", false);
                ShotAnimation_End = true;
            }
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Archer_Attack") &&
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

        if (Skill1 == 0)
        {
            Skill_TripleShotOn();
        }
        else if (Skill1 == 1)
        {
            Skill_FastShotOn();
        }
        else if (Skill1 == 2)
        {
            Skill_Singijeon();
        }
        else if (Skill1 == 3)
        {
            Skill_FocusingOn();
        }
        else if (Skill1 == 4)
        {
            Skill_HugeArrow();
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

        if (Skill2 == 0)
        {
            Skill_TripleShotOn();
        }
        else if (Skill2 == 1)
        {
            Skill_FastShotOn();
        }
        else if (Skill2 == 2)
        {
            Skill_Singijeon();
        }
        else if (Skill2 == 3)
        {
            Skill_FocusingOn();
        }
        else if (Skill2 == 4)
        {
            Skill_HugeArrow();
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
            if(target == collision.gameObject)
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
            gamemanager.ChangeSkillManage(1, skill_num);
        }
        else if (Skill1 == -1)
        {
            Skill1 = skill_num;
            maxSkill1Delay = SkillDelay[skill_num];
            curSkill1Delay = SkillDelay[skill_num];
            gamemanager.SkillManagefinish();
        }
        else
        {
            Skill2 = skill_num;
            maxSkill2Delay = SkillDelay[skill_num];
            curSkill2Delay = SkillDelay[skill_num];
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
            maxSkill1Delay = SkillDelay[skill_num];
            curSkill1Delay = SkillDelay[skill_num];
        }
        else
        {
            Skill2 = skill_num;
            maxSkill2Delay = SkillDelay[skill_num];
            curSkill2Delay = SkillDelay[skill_num];
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

    //트리플샷
    void Skill_TripleShotOn()
    {
        TripleShotOnOff = true;
        Invoke("Skill_TripleShotOff", 5f);
    }

    void Skill_TripleShotOff()
    {
        TripleShotOnOff = false;
    }

    //속사
    void Skill_FastShotOn()
    {
        maxAttackDelay /= 2.0f;
        fastshot_effect = Instantiate(FastShot_Effect);
        if (sprRen.flipX == false)
        {
            fastshot_effect.transform.position = new Vector3(transform.position.x + 0.16f, transform.position.y - 0.45f);
        }
        else
        {
            fastshot_effect.transform.position = new Vector3(transform.position.x - 0.16f, transform.position.y - 0.45f);
        }
        Invoke("Skill_FastShotOff", 5f);
    }

    void Skill_FastShotOff()
    {
        maxAttackDelay = maxattackdelay;
        Destroy(fastshot_effect.gameObject);
    }

    //신기전
    void Skill_Singijeon()
    {
        singijeon = Instantiate(Singijeon);
        if(sprRen.flipX == false)
        {
            singijeon.transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y + 0.1f);
        }
        else
        {
            singijeon.transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.1f);
            SpriteRenderer sprRen_singijeon = singijeon.GetComponent<SpriteRenderer>();
            sprRen_singijeon.flipX = true;
        }
        Invoke("Skill_Singijeon_Fire", 0.5f);
        Invoke("Skill_SingijeonOff", 1.5f);
    }

    void Skill_Singijeon_Fire()
    {
        GameObject arrow = Instantiate(NormalAttack[1], NormalAttackPos, targetRot);
        CCAttack atk = arrow.GetComponent<CCAttack>();
        atk.damage = AttackDamage * 2f;
        atk.duration = 3;
        atk.dot = AttackDamage * 0.5f;
        arrow.transform.position = new Vector3(singijeon.transform.position.x, singijeon.transform.position.y);
        Rigidbody2D rigid = arrow.GetComponent<Rigidbody2D>();
        rigid.AddForce(Attack_dist.normalized * 15f, ForceMode2D.Impulse);
        singijeon_count++;
        if (singijeon_count == 7) return;
        Skill_Singijeon_Reload();
    }

    void Skill_Singijeon_Reload()
    {
        Invoke("Skill_Singijeon_Fire", 0.1f);
    }

    void Skill_SingijeonOff()
    {
        singijeon_count = 0;
        Destroy(singijeon);
    }

    //집중
    void Skill_FocusingOn()
    {
        CriticalChance += 20.0f;
        focusing_effect = Instantiate(Focusing_Effect);
        focusing_effect.transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y + 0.8f);
        Invoke("Skill_FocusingOff", 5f);
    }

    void Skill_FocusingOff()
    {
        CriticalChance -= 20.0f;
        Destroy(focusing_effect.gameObject);
    }

    //큰화살
    void Skill_HugeArrow()
    {
        GameObject arrow = Instantiate(HugeArrow, NormalAttackPos, targetRot);
        PenetratingAttack atk = arrow.GetComponent<PenetratingAttack>();
        atk.damage = AttackDamage * 6f;
        Rigidbody2D rigid = arrow.GetComponent<Rigidbody2D>();
        rigid.AddForce(Attack_dist.normalized * 15f, ForceMode2D.Impulse);
    }

    //게임 리셋
    public void GameReset()
    {
        AttackDamage = 10f;
        maxAttackDelay = 0.7f;
        maxattackdelay = 0.7f;
        CriticalChance = 10f;
        SkillDelay = new float[5] { 10, 10, 12, 10, 13 };
        Skill1 = -1;
        Skill2 = -1;
        TripleShotOnOff = false;
        NormalAttackNum = 0;
        ShotAnimation_End = true;
    }
}
