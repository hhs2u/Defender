using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mage : MonoBehaviour
{
    public GameObject[] NormalAttack;
    //����
    public float AttackDamage;
    public float maxAttackDelay;
    public float curAttackDelay;
    public float CriticalChance;
    public float CriticalMultiplier;

    Animator anim;
    public bool isTarget = false;
    public GameObject target;
    SpriteRenderer sprRen;
    Vector3 dist; //Ÿ�ٰ��� �Ÿ�
    Vector2 Attack_dist; //Ÿ�ٰ� ���ݻ����� �Ÿ�
    Vector3 dir; //Ÿ�ٹ���
    Quaternion targetRot;
    Vector3 NormalAttackPos; //�⺻���� ���� ��ġ
    float target_Angle; // Ÿ�ٰ��� ����
    public bool ShotAnimation_End = true;
    public int NormalAttackNum = 0;

    //��ų ����
    public int Skill1;
    public int Skill2;
    public float[] SkillDelay = new float[5] { 13, 13, 10, 10, 9 };
    public float maxSkill1Delay;
    public float curSkill1Delay;
    public float maxSkill2Delay;
    public float curSkill2Delay;
    public GameObject[] Meteor;
    public GameObject Ground;
    public GameObject[] Blizzard;
    public GameObject Tornado;
    public GameObject[] Lightning;
    public GameObject[] Lightbeam;

    GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprRen = GetComponent<SpriteRenderer>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Skill1 = -1;
        Skill2 = -1;
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
            NormalAttackPos = new Vector3(transform.position.x, transform.position.y + 0.5f, 5);
        }
        else
        {
            sprRen.flipX = false;
            NormalAttackPos = new Vector3(transform.position.x, transform.position.y + 0.5f, 5);
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
            //�⺻ �ִϸ��̼����� ��ȯ
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
        //�� �ִϸ��̼����� ��ȯ
        if (ShotAnimation_End == true)
        {
            anim.SetBool("isShot", true);
            ShotAnimation_End = false;
        }

        curAttackDelay = 0;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Mage_Attack") &&
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
            //�⺻���ݻ���
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
            else
            {
                CCAttack atk = Normalatk.GetComponent<CCAttack>();
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

            //�⺻���ݹ߻�
            Rigidbody2D rigid = Normalatk.GetComponent<Rigidbody2D>();
            rigid.AddForce(Attack_dist.normalized * 15f, ForceMode2D.Impulse);
            anim.SetBool("isShot", false);
            ShotAnimation_End = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Mage_Attack") &&
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

        if (Skill1 == 8)
        {
            Skill_Meteor();
        }
        else if (Skill1 == 9)
        {
            Skill_Blizzard();
        }
        else if (Skill1 == 10)
        {
            Skill_Tornado();
        }
        else if (Skill1 == 11)
        {
            Skill_Lightning();
        }
        else if (Skill1 == 12)
        {
            Skill_Lightbeam();
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

        if (Skill2 == 8)
        {
            Skill_Meteor();
        }
        else if (Skill2 == 9)
        {
            Skill_Blizzard();
        }
        else if (Skill2 == 10)
        {
            Skill_Tornado();
        }
        else if (Skill2 == 11)
        {
            Skill_Lightning();
        }
        else if (Skill2 == 12)
        {
            Skill_Lightbeam();
        }
    }

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
            gamemanager.ChangeSkillManage(2, skill_num);
        }
        else if (Skill1 == -1)
        {
            Skill1 = skill_num;
            maxSkill1Delay = SkillDelay[skill_num - 8];
            curSkill1Delay = SkillDelay[skill_num - 8];
            gamemanager.SkillManagefinish();
        }
        else
        {
            Skill2 = skill_num;
            maxSkill2Delay = SkillDelay[skill_num - 8];
            curSkill2Delay = SkillDelay[skill_num - 8];
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
            maxSkill1Delay = SkillDelay[skill_num - 8];
            curSkill1Delay = SkillDelay[skill_num - 8];
        }
        else
        {
            Skill2 = skill_num;
            maxSkill2Delay = SkillDelay[skill_num - 8];
            curSkill2Delay = SkillDelay[skill_num - 8];
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

    //���׿�
    void Skill_Meteor()
    {
        GameObject circle = Instantiate(Meteor[0]);
        circle.transform.position = new Vector3(target.transform.position.x + 2f, target.transform.position.y + 3f);

        float meteor_target_Angle = GetAngle(circle.transform.position, target.transform.position);
        Quaternion meteor_targetRot = Quaternion.Euler(new Vector3(0, 0, meteor_target_Angle + 180f));

        GameObject ground = Instantiate(Ground);
        ground.transform.position = new Vector3(target.transform.position.x, target.transform.position.y - 0.5f);

        GameObject meteor = Instantiate(Meteor[1], circle.transform.position, meteor_targetRot);
        Rigidbody2D rigid = meteor.GetComponent<Rigidbody2D>();
        Vector2 atk_dist = target.transform.position - circle.transform.position;
        rigid.AddForce(atk_dist.normalized * 5f, ForceMode2D.Impulse);
    }

    public void SKill_Meteor_Explosion(Vector3 GenPos)
    {
        GameObject explosion = Instantiate(Meteor[2]);
        GameObject collider = Instantiate(Meteor[3]);

        //ġ��Ÿ���
        int rand = Random.Range(0, 100) + 1;
        bool Critical = false;
        if (rand <= CriticalChance)
        {
            Critical = true;
        }
        CCAttack atk = collider.GetComponent<CCAttack>();
        atk.duration = 3;
        atk.dot = 0.5f * AttackDamage;
        if (Critical == true)
        {
            atk.damage = AttackDamage * 5f * CriticalMultiplier;
        }
        else
        {
            atk.damage = AttackDamage * 5f;
        }
        explosion.transform.position = GenPos;
        collider.transform.position = GenPos;
    }

    //���ڵ�
    void Skill_Blizzard()
    {
        GameObject blizzard = Instantiate(Blizzard[0]);
        blizzard.transform.position = target.transform.position;
        Invoke("Skill_Blizzard_Damage", 0.5f);
    }

    void Skill_Blizzard_Damage()
    {
        GameObject blizzard_collision = Instantiate(Blizzard[1]);
        //ġ��Ÿ���
        int rand = Random.Range(0, 100) + 1;
        bool Critical = false;
        if (rand <= CriticalChance)
        {
            Critical = true;
        }
        CCAttack atk = blizzard_collision.GetComponent<CCAttack>();
        atk.duration = 3;
        if (Critical == true)
        {
            atk.damage = AttackDamage * 4f * CriticalMultiplier;
        }
        else
        {
            atk.damage = AttackDamage * 4f;
        }
        blizzard_collision.transform.position = target.transform.position;
    }

    //����̵�
    void Skill_Tornado()
    {
        GameObject tornado = Instantiate(Tornado);

        //ġ��Ÿ���
        int rand = Random.Range(0, 100) + 1;
        bool Critical = false;
        if (rand <= CriticalChance)
        {
            Critical = true;
        }
        PenetratingAttack atk = tornado.GetComponent<PenetratingAttack>();
        if (Critical == true)
        {
            atk.damage = AttackDamage * 5f * CriticalMultiplier;
        }
        else
        {
            atk.damage = AttackDamage * 5f;
        }
        tornado.transform.position = NormalAttackPos;

        Rigidbody2D rigid = tornado.GetComponent<Rigidbody2D>();
        rigid.AddForce(Attack_dist.normalized * 5f, ForceMode2D.Impulse);
    }

    //����Ʈ��
    void Skill_Lightning()
    {
        GameObject lightning = Instantiate(Lightning[0]);
        GameObject lightning_collision = Instantiate(Lightning[1]);

        //ġ��Ÿ���
        int rand = Random.Range(0, 100) + 1;
        bool Critical = false;
        if (rand <= CriticalChance)
        {
            Critical = true;
        }
        CCAttack atk = lightning_collision.GetComponent<CCAttack>();
        atk.duration = 3;
        if (Critical == true)
        {
            atk.damage = AttackDamage * 8f * CriticalMultiplier;
        }
        else
        {
            atk.damage = AttackDamage * 8f;
        }
        lightning.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1);
        lightning_collision.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1);
    }

    //����Ʈ��
    Vector3 atkpos;
    float lightbeam_target_Angle;
    Quaternion lightbeam_targetRot;
    void Skill_Lightbeam()
    {
        if(sprRen.flipX == false)
        {
            atkpos = new Vector3(gameObject.transform.position.x - 0.3f, gameObject.transform.position.y - 0.2f);
        }
        else
        {
            atkpos = new Vector3(gameObject.transform.position.x + 0.3f, gameObject.transform.position.y - 0.2f);
        }
        lightbeam_target_Angle = GetAngle(atkpos, target.transform.position);
        lightbeam_targetRot = Quaternion.Euler(new Vector3(0, 0, lightbeam_target_Angle + 180f));

        GameObject lightbeam = Instantiate(Lightbeam[0], atkpos, lightbeam_targetRot);
        Invoke("Skill_Lightbeam_Damage", 1f);
    }

    void Skill_Lightbeam_Damage()
    {
        GameObject lightbeam_collision = Instantiate(Lightbeam[1], atkpos, lightbeam_targetRot);

        //ġ��Ÿ���
        int rand = Random.Range(0, 100) + 1;
        bool Critical = false;
        if (rand <= CriticalChance)
        {
            Critical = true;
        }
        CCAttack atk = lightbeam_collision.GetComponent<CCAttack>();
        atk.duration = 3;
        if (Critical == true)
        {
            atk.damage = AttackDamage * 3f * CriticalMultiplier;
        }
        else
        {
            atk.damage = AttackDamage * 3f;
        }
    }

    //���� ����
    public void GameReset()
    {
        AttackDamage = 10f;
        maxAttackDelay = 0.8f;
        CriticalChance = 10f;
        SkillDelay = new float[5] { 13, 13, 10, 10, 9 };
        Skill1 = -1;
        Skill2 = -1;
        NormalAttackNum = 0;
        ShotAnimation_End = true;
    }

}
