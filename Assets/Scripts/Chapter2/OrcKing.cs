using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrcKing : MonoBehaviour
{
    MonsterStatus monsterstatus;
    Vector2 target = new Vector2(-2.5f, 1.5f);
    public GameObject prfhpBar; //hpbar
    public GameObject canvas; //hpbar 생성할 캔버스
    Image nowhpBar;
    Animator anim;
    GameManager gamemanager;
    SpriteRenderer sprRen; //상태이상 색 변경용
    float BurnDuration;
    float PoisonDuration;
    float StunDuration;
    float FreezeDuration;
    float ElectricDuration;
    float SlowDuration;
    float LDPDuration;
    bool BurnOff = true;
    bool PoisonOff = true;
    public bool StunOff = true;
    public bool FreezeOff = true;
    bool ElectricOff = true;
    bool SlowOff = true;
    bool LDPOff = true;
    float BurnTimer;
    float PoisonTimer;
    float BurnDot;
    float PoisonDot;
    public bool isSkill = false;
    public int MovePoint = 0;
    bool inShield = false;

    RectTransform hpBar;

    float maxSkillDelay = 5f;
    float curSkillDelay;


    void Start()
    {
        canvas = GameObject.Find("Canvas");
        hpBar = Instantiate(prfhpBar, canvas.transform).GetComponent<RectTransform>();
        nowhpBar = hpBar.GetComponent<Image>();
        anim = GetComponent<Animator>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sprRen = GetComponent<SpriteRenderer>();
        monsterstatus = gameObject.GetComponent<MonsterStatus>();

        if (MovePoint == 1)
        {
            target = new Vector2(-2.5f, -2.5f);
        }
        else if (MovePoint == 2)
        {
            target = new Vector2(2.5f, -2.5f);
        }
        else if (MovePoint == 3)
        {
            target = new Vector2(2.5f, 1.5f);
        }
        else if (MovePoint == 4)
        {
            target = new Vector2(12f, 1.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1f, 0));
        hpBar.transform.position = hpBarPos;

        nowhpBar.fillAmount = (float)monsterstatus.nowhp / (float)monsterstatus.maxhp;

        if (monsterstatus.nowhp <= 0)
        {
            gamemanager.Monster_Die();
            Destroy(gameObject);
        }
        else if (StunDuration <= 0 && FreezeDuration <= 0 && isSkill == false)
        {
            if (SlowDuration <= 0)
            {
                Move(monsterstatus.speed);
            }
            else
            {
                Move(monsterstatus.speed * 0.7f);
            }
        }

        if (BurnDuration > 0)
        {
            BurnDuration -= Time.deltaTime;
            BurnTimer += Time.deltaTime;
            if (BurnTimer >= 1.0f)
            {
                BurnTimer = 0;
                if (FreezeOff == false)
                {
                    Dot(BurnDot * 2.0f);
                }
                else
                {
                    Dot(BurnDot);
                }
            }
        }
        else if (BurnDuration <= 0 && BurnOff == false)
        {
            BurnOff = true;
            BurnTimer = 0;
        }

        if (PoisonDuration > 0)
        {
            PoisonDuration -= Time.deltaTime;
            PoisonTimer += Time.deltaTime;
            if (PoisonTimer >= 1.0f)
            {
                PoisonTimer = 0;
                Dot(PoisonDot);
            }
        }
        else if (PoisonDuration <= 0 && PoisonOff == false)
        {
            monsterstatus.defense *= 2f;
            PoisonOff = true;
            PoisonTimer = 0;
        }

        if (StunDuration > 0)
        {
            StunDuration -= Time.deltaTime;
        }
        else if (StunDuration <= 0 && StunOff == false)
        {
            StunOff = true;
        }

        if (FreezeDuration > 0)
        {
            FreezeDuration -= Time.deltaTime;
        }
        else if (FreezeDuration <= 0 && FreezeOff == false)
        {
            FreezeOff = true;
        }

        if (ElectricDuration > 0)
        {
            ElectricDuration -= Time.deltaTime;
        }
        else if (ElectricDuration <= 0 && ElectricOff == false)
        {
            monsterstatus.damageMultiplier = 1.0f;
            ElectricOff = true;
        }

        if (SlowDuration > 0)
        {
            SlowDuration -= Time.deltaTime;
        }
        else if (SlowDuration <= 0 && SlowOff == false)
        {
            SlowOff = true;
        }

        if (LDPDuration > 0)
        {
            LDPDuration -= Time.deltaTime;
        }
        else if (LDPDuration <= 0 && LDPOff == false)
        {
            monsterstatus.defense *= 2.0f;
            LDPOff = true;
        }

        Fire_SKill();
        Reload_Skill();

        ShowColor();
    }

    //이동 함수
    void Move(float speed)
    {
        if (transform.position.x == -2.5f && transform.position.y == 1.5f)
        {
            target = new Vector2(-2.5f, -2.5f);
            MovePoint = 1;
        }
        else if (transform.position.x == -2.5 && transform.position.y == -2.5f)
        {
            target = new Vector2(2.5f, -2.5f);
            MovePoint = 2;
        }
        else if (transform.position.x == 2.5f && transform.position.y == -2.5f)
        {
            target = new Vector2(2.5f, 1.5f);
            MovePoint = 3;
        }
        else if (transform.position.x == 2.5f && transform.position.y == 1.5f)
        {
            target = new Vector2(12f, 1.5f);
            MovePoint = 4;
        }
        transform.position = Vector2.MoveTowards(transform.position, target, 0.02f * speed);
    }

    void Fire_SKill()
    {
        if (curSkillDelay < maxSkillDelay)
        {
            return;
        }
        curSkillDelay = 0;
        SelfHeal();
    }

    void Reload_Skill()
    {
        curSkillDelay += Time.deltaTime;
    }

    void SelfHeal()
    {
        monsterstatus.OnHit(monsterstatus.maxhp / -20f);
    }

    //데미지
    void OnTriggerEnter2D(Collider2D collision)
    {
        //쉴드확인
        if (inShield == true) return;

        if (collision.gameObject.tag == "Attack")
        {
            Attack attack = collision.gameObject.GetComponent<Attack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "BurnAttack")
        {
            CCAttack attack = collision.gameObject.GetComponent<CCAttack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "PoisonAttack")
        {
            CCAttack attack = collision.gameObject.GetComponent<CCAttack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "StunAttack")
        {
            CCAttack attack = collision.gameObject.GetComponent<CCAttack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "FreezeAttack")
        {
            CCAttack attack = collision.gameObject.GetComponent<CCAttack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "ElectricAttack")
        {
            CCAttack attack = collision.gameObject.GetComponent<CCAttack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "PenetratingAttack")
        {
            PenetratingAttack attack = collision.gameObject.GetComponent<PenetratingAttack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "SlowAttack")
        {
            CCAttack attack = collision.gameObject.GetComponent<CCAttack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "KillAttack")
        {
            CCAttack attack = collision.gameObject.GetComponent<CCAttack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "PoisonAreaAttack")
        {
            AreaCCAttack attack = collision.gameObject.GetComponent<AreaCCAttack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "LowerDefAttack")
        {
            CCAttack attack = collision.gameObject.GetComponent<CCAttack>();
            monsterstatus.OnHit(attack.damage);
        }
        else if (collision.gameObject.tag == "EndPoint")
        {
            monsterstatus.OnHit(100000);
            gamemanager.Life_decrease(1);
        }
        else if (collision.gameObject.tag == "Heal")
        {
            Attack attack = collision.gameObject.GetComponent<Attack>();
            monsterstatus.OnHit((monsterstatus.maxhp * attack.damage / 100.0f) * -1.0f);
        }
        else if (collision.gameObject.tag == "Shield")
        {
            inShield = true;
        }
        else if (collision.gameObject.tag == "Clear")
        {
            monsterstatus.OnHit(100000);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shield")
        {
            inShield = false;
        }
    }

    //상태이상
    void Burn(int dur, float dot)
    {
        BurnDuration = Mathf.Max(dur * 1.0f, BurnDuration);
        BurnDot = Mathf.Max(dot, BurnDot);
        BurnOff = false;
    }

    void Poison(int dur, float dot)
    {
        PoisonDuration = Mathf.Max(dur * 1.0f, PoisonDuration);
        PoisonDot = Mathf.Max(dot, PoisonDot);
        if (PoisonOff == true)
            monsterstatus.defense *= 0.5f;
    }

    void Stun(int dur)
    {
        StunDuration = Mathf.Max(StunDuration, dur * 1.0f);
    }

    void Freeze(int dur)
    {
        FreezeDuration = Mathf.Max(FreezeDuration, dur * 1.0f);
    }

    void Electric(int dur)
    {
        ElectricDuration = Mathf.Max(ElectricDuration, dur * 1.0f);
        monsterstatus.damageMultiplier = 1.1f;
    }

    void Slow(int dur)
    {
        SlowDuration = Mathf.Max(SlowDuration, dur * 1.0f);
    }

    void Death(int dur)
    {
        int chance = Random.Range(0, 100) + 1;
        if (chance <= dur) monsterstatus.OnHit(100000);
    }

    void LowerDefensivePower(int dur)
    {
        LDPDuration = Mathf.Max(LDPDuration, dur * 1.0f);
        if (LDPOff == true)
            monsterstatus.defense /= 2.0f;
    }

    //도트데미지
    void Dot(float damage)
    {
        monsterstatus.OnHit(damage);
    }

    //몬스터색상
    void ShowColor()
    {
        if (BurnDuration > PoisonDuration && BurnDuration > FreezeDuration && BurnDuration > ElectricDuration)
        {
            sprRen.color = new Color(255, 0, 0, 255);
        }
        else if (PoisonDuration > BurnDuration && PoisonDuration > FreezeDuration && PoisonDuration > ElectricDuration)
        {
            sprRen.color = new Color(0, 200, 0, 255);
        }
        else if (FreezeDuration > BurnDuration && FreezeDuration > PoisonDuration && FreezeDuration > ElectricDuration)
        {
            sprRen.color = new Color(0, 190, 255, 255);
        }
        else if (ElectricDuration > BurnDuration && ElectricDuration > PoisonDuration && ElectricDuration > FreezeDuration)
        {
            sprRen.color = new Color(255, 255, 100, 255);
        }
        else
        {
            sprRen.color = new Color(255, 255, 255, 255);
        }
    }
}
