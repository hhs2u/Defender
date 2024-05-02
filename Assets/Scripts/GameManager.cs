using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int Chapter;
    public int Life;
    public int Gold;
    public GameObject[] stages;
    public GameObject[] UI; //0. ¸ÞÀÎUI, 1. °ÔÀÓUI, 2. ¸Ê, 3. º¸»ó, 4. º¸»óÈ¹µæ, 5. Ä«µå±³Ã¼
    public GameObject HowtoPlay;
    public GameObject[] GameStages;
    public int x;
    public int y;
    public Sprite[] PlayerHpbar;
    Image playerHpbar;
    int[] mapCount = new int[50];
    List<Image> mapBarList = new List<Image>();
    public GameObject[] HowtoPlayPage;
    int Page = 0;
    bool MainSettingOnOff = false;

    public Transform MapPos;
    RectTransform MapRectPos;
    List<List<int>> MapList = new List<List<int>>();
    Button[,] MapStageList = new Button[20, 20];
    public GameObject[] MapBar;
    float DiscountRate;

    public Transform Spawnpoint;
    public GameObject[] Chapter1_Monster;
    public GameObject[] Chapter2_Monster;
    public GameObject[] Chapter3_Monster;
    public int Monster_Amount = -1;
    public GameObject Clearer;
    GameObject clearer;
    int laststage;

    public Transform RewardCanvasPos;
    public GameObject[] RewardBoards;
    public Transform GetCardRewardCanvasPos;
    public GameObject[] SkillCards;
    int Card_num1, Card_num2, Card_num3;
    bool getCard = false;
    bool getArtifact = false;
    bool getEquipment = false;
    Button reward_card;
    Button reward_artifact;
    Button reward_equipment;
    Button Card1;
    Button Card2;
    Button Card3;
    public Transform ChangeCardCanvasPos;
    Button selectCard;
    Button changeCard1;
    Button changeCard2;

    public Transform GameUICanvasPos;
    public GameObject[] Archer_Icons;
    public GameObject[] Mage_Icons;
    public GameObject[] Theif_Icons;
    public GameObject[] Warrior_Icons;
    GameObject Archer_Icon1;
    GameObject Archer_Icon2;
    GameObject Mage_Icon1;
    GameObject Mage_Icon2;
    GameObject Thief_Icon1;
    GameObject Thief_Icon2;
    GameObject Warrior_Icon1;
    GameObject Warrior_Icon2;
    public GameObject CoolTimeImage;
    GameObject Archer_Icon1_Image;
    GameObject Archer_Icon2_Image;
    GameObject Mage_Icon1_Image;
    GameObject Mage_Icon2_Image;
    GameObject Thief_Icon1_Image;
    GameObject Thief_Icon2_Image;
    GameObject Warrior_Icon1_Image;
    GameObject Warrior_Icon2_Image;
    public GameObject CoolTimeText;
    GameObject Archer_Icon1_Text;
    GameObject Archer_Icon2_Text;
    GameObject Mage_Icon1_Text;
    GameObject Mage_Icon2_Text;
    GameObject Thief_Icon1_Text;
    GameObject Thief_Icon2_Text;
    GameObject Warrior_Icon1_Text;
    GameObject Warrior_Icon2_Text;

    public GameObject[] Artifact_Image;
    public GameObject[] Artifact_Reward;
    List<GameObject> Artifact_List = new List<GameObject>(); //0~22
    int Artifact_Count = 0;
    Button Artifact;
    public Transform BagCanvasPos;
    bool BagOnOff = false;
    public Transform GetArtifactRewardCanvasPos;

    Button treasure;
    public GameObject HPrecover;
    Button hprecover;
    Text GoldText;
    public GameObject ShopCard;
    Button shopCard;
    public GameObject ShopArtifact;
    Button shopArtifact;

    Text GameOverText;
    bool GameSettingOnOff = false;
    Text ChapterText;

    //¾Æ±ºÀ¯´Ö
    Archer archer;
    Mage mage;
    Thief thief;
    Warrior warrior;

    Image img;

    void Start()
    {
        archer = GameObject.Find("Archer").GetComponent<Archer>();
        mage = GameObject.Find("Mage").GetComponent<Mage>();
        thief = GameObject.Find("Thief").GetComponent<Thief>();
        warrior = GameObject.Find("Warrior").GetComponent<Warrior>();
        playerHpbar = GameObject.Find("HealthPoint").GetComponent<Image>();
        GoldText = GameObject.Find("GoldText").GetComponent<Text>();
        GoldText.text = Gold.ToString();
        ChapterText = GameObject.Find("ChapterText").GetComponent<Text>();
        Show_Life();
    }

    // Update is called once per frame
    void Update()
    {
        if (Monster_Amount == 0) 
        {
            Monster_Amount = -1;
            clearer = Instantiate(Clearer);
            Invoke("ClearerDestroy", 0.5f);
            GetReward(laststage);
        }
        ShowCoolTime();

        if (Input.GetKeyDown(KeyCode.Escape) && UI[0].activeSelf == false)
        {
            GameSettingUI();
        }
    }

    public void NextChapter()
    {
        if(Chapter == 3)
        {
            MapStageList[12, 1].interactable = false;
            UI[10].SetActive(true);
            GameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
            GameOverText.text = "GameClear";
        }
        else
        {
            Chapter++;
            if (Chapter > 0)
            {
                UI[0].SetActive(false);
                UI[1].SetActive(true);
                UI[2].SetActive(true);
                MapGenerate();
                if (Chapter > 1)
                {
                    stages[Chapter - 1].SetActive(false);
                    stages[Chapter].SetActive(true);
                }
                x = 1;
            }
            ChapterText.text = "Chapter" + Chapter.ToString();
        }
    }

    public void HowtoPlay_Show()
    {
        UI[0].SetActive(false);
        HowtoPlay.SetActive(true);
    }

    public void HowtoPlay_Hide()
    {
        HowtoPlay.SetActive(false);
        UI[0].SetActive(true);
    }

    public void HowtoPlay_NextPage()
    {
        if (Page >= HowtoPlayPage.Length - 1) return;
        HowtoPlayPage[Page].SetActive(false);
        Page++;
        HowtoPlayPage[Page].SetActive(true);
    }

    public void HowtoPlay_BackPage()
    {
        if (Page <= 0) return;
        HowtoPlayPage[Page].SetActive(false);
        Page--;
        HowtoPlayPage[Page].SetActive(true);
    }

    public void Life_decrease(int x)
    {
        Life -= x;
        if (Life > 10) Life = 10;
        if(Life <= 0 )
        {
            UI[10].SetActive(true);
            GameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
            GameOverText.text = "GameOver";
        }
        if (Life < 0) Life = 0;
        Show_Life();
    }

    public void GameSettingUI()
    {
        if (GameSettingOnOff == true)
        {
            UI[9].SetActive(false);
            GameSettingOnOff = false;
        }
        else
        {
            UI[9].SetActive(true);
            GameSettingOnOff = true;
        }
    }

    public void MainSettingUI()
    {
        if (MainSettingOnOff == true)
        {
            UI[0].SetActive(true);
            UI[11].SetActive(false);
            MainSettingOnOff = false;
        }
        else
        {
            UI[0].SetActive(false);
            UI[11].SetActive(true);
            MainSettingOnOff = true;
        }
    }

    void Show_Life()
    {
        playerHpbar.sprite = PlayerHpbar[Life];
    }

    void MapGenerate()
    {
        //¸ÊUIÅ¬¸®¾î
        for (int i = 1; i <= 11; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                if(MapStageList[i, j])
                {
                    ObjectDestroy stageDestroy = MapStageList[i, j].GetComponent<ObjectDestroy>();
                    stageDestroy.destroy();
                }
            }
        }
        if (MapStageList[12, 1])
        {
            ObjectDestroy BossDestroy = MapStageList[12, 1].GetComponent<ObjectDestroy>();
            BossDestroy.destroy();
        }
        MapList = new List<List<int>>();
        MapStageList = new Button[20, 20];
        for (int i = 0; i < mapBarList.Count; i++)
        {
            if(mapBarList[i])
            {
                ObjectDestroy mapbarDestroy = mapBarList[i].GetComponent<ObjectDestroy>();
                mapbarDestroy.destroy();
            }
        }
        mapBarList = new List<Image>();
        for (int i = 1; i <= 44; i++)
        {
            mapCount[i] = 0;
        }
        //¸Ê ¾ÆÀÌÄÜ »ý¼º
        for (int i = 1; i < 13; i++) 
        {
            for (int j = -3; j <= 3; j+=2)
            {
                int result = Random.Range(0, 100) + 1;
                int Y;

                if (j == -3) Y = 4;
                else if (j == -1) Y = 3;
                else if (j == 1) Y = 2;
                else Y = 1;
                if (i == 1) //1Ãþ ÀÏ¹Ý½ºÅ×ÀÌÁö
                {
                    Button stage = Instantiate(GameStages[0], MapPos).GetComponent<Button>();
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 300 - 2100, j * 100);
                    stage.onClick.AddListener(() => SetY(Y));
                    stage.onClick.AddListener(() => Normal());
                    MapStageList[i, Y] = stage;
                }
                else if (i == 8) //8Ãþ »óÁ¡
                {
                    Button stage = Instantiate(GameStages[4], MapPos).GetComponent<Button>();
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 300 - 2100, j * 100);
                    stage.onClick.AddListener(() => SetY(Y));
                    stage.onClick.AddListener(() => Shop());
                    stage.interactable = false;
                    MapStageList[i, Y] = stage;
                }
                else if (i == 12) //12Ãþ º¸½º
                {
                    Button stage = Instantiate(GameStages[2], MapPos).GetComponent<Button>();
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 300 - 2000, 0);
                    stage.onClick.AddListener(() => Boss());
                    stage.interactable = false;
                    MapStageList[i, 1] = stage;
                    break;
                }
                else if (1 <= result && result <= 45) //ÀÏ¹Ý½ºÅ×ÀÌÁö 45%
                {
                    Button stage = Instantiate(GameStages[0], MapPos).GetComponent<Button>();
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 300 - 2100, j * 100);
                    stage.onClick.AddListener(() => SetY(Y));
                    stage.onClick.AddListener(() => Normal());
                    stage.interactable = false;
                    MapStageList[i, Y] = stage;
                }
                else if (46 <= result && result <= 61) //¿¤¸®Æ® 16%
                {
                    Button stage = Instantiate(GameStages[1], MapPos).GetComponent<Button>();
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 300 - 2100, j * 100);
                    stage.onClick.AddListener(() => SetY(Y));
                    stage.onClick.AddListener(() => Elite());
                    stage.interactable = false;
                    MapStageList[i, Y] = stage;
                }
                else if (62 <= result && result <= 72) //º¸¹° 11%
                {
                    Button stage = Instantiate(GameStages[5], MapPos).GetComponent<Button>();
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 300 - 2100, j * 100);
                    stage.onClick.AddListener(() => SetY(Y));
                    stage.onClick.AddListener(() => Treasure());
                    stage.interactable = false;
                    MapStageList[i, Y] = stage;
                }
                else if (73 <= result && result <= 84) //ÈÞ½Ä 12%
                {
                    Button stage = Instantiate(GameStages[3], MapPos).GetComponent<Button>();
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 300 - 2100, j * 100);
                    stage.onClick.AddListener(() => SetY(Y));
                    stage.onClick.AddListener(() => Rest());
                    stage.interactable = false;
                    MapStageList[i, Y] = stage;
                }
                else if (85 <= result && result <= 89) //»óÁ¡ 5%
                {
                    Button stage = Instantiate(GameStages[4], MapPos).GetComponent<Button>();
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 300 - 2100, j * 100);
                    stage.onClick.AddListener(() => SetY(Y));
                    stage.onClick.AddListener(() => Shop());
                    stage.interactable = false;
                    MapStageList[i, Y] = stage;
                }
                else //¹°À½Ç¥ 11%
                {
                    Button stage = Instantiate(GameStages[6], MapPos).GetComponent<Button>();
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 300 - 2100, j * 100);
                    stage.onClick.AddListener(() => SetY(Y));
                    stage.onClick.AddListener(() => Unknown());
                    stage.interactable = false;
                    MapStageList[i, Y] = stage;
                }
            }
        }

        //Áöµµ ¿¬°á
        int MapGenX, MapGenY;
        for (int i = 0; i < 4; i++) 
        {
            MapGenX = 1;
            MapGenY = i + 1;
            List<int> maplist = new List<int>();
            maplist.Add(MapGenY);
            mapCount[MapGenY] = 1;
            for (int j = 0; j < 11; j++)
            {
                int next;
                if (MapGenY == 1)
                {
                    next = Random.Range(0, 2);
                }
                else if(MapGenY == 4)
                {
                    next = Random.Range(0, 2) - 1;
                }
                else
                {
                    next = Random.Range(0, 3) - 1;
                }

                if (j == 10) 
                {
                    if (MapGenY == 1)
                    {
                        Image mapbar = Instantiate(MapBar[0], MapPos).GetComponent<Image>();
                        float barX = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.x;
                        float barY = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.y;
                        mapbar.GetComponent<RectTransform>().anchoredPosition = new Vector3(barX + 150, barY - 100);
                        mapBarList.Add(mapbar);
                    }
                    else if (MapGenY == 4)
                    {
                        Image mapbar = Instantiate(MapBar[2], MapPos).GetComponent<Image>();
                        float barX = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.x;
                        float barY = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.y;
                        mapbar.GetComponent<RectTransform>().anchoredPosition = new Vector3(barX + 150, barY + 100);
                        mapBarList.Add(mapbar);
                    }
                    else
                    {
                        Image mapbar = Instantiate(MapBar[1], MapPos).GetComponent<Image>();
                        float barX = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.x;
                        float barY = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.y;
                        mapbar.GetComponent<RectTransform>().anchoredPosition = new Vector3(barX + 150, barY);
                        mapBarList.Add(mapbar);
                    }
                }
                else if (next == 0)
                {
                    Image mapbar = Instantiate(MapBar[1], MapPos).GetComponent<Image>();
                    float barX = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.x;
                    float barY = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.y;
                    mapbar.GetComponent<RectTransform>().anchoredPosition = new Vector3(barX + 150, barY);
                    mapBarList.Add(mapbar);
                }
                else if (next == 1)
                {
                    Image mapbar = Instantiate(MapBar[0], MapPos).GetComponent<Image>();
                    float barX = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.x;
                    float barY = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.y;
                    mapbar.GetComponent<RectTransform>().anchoredPosition = new Vector3(barX + 150, barY - 100);
                    mapBarList.Add(mapbar);
                }
                else
                {
                    Image mapbar = Instantiate(MapBar[2], MapPos).GetComponent<Image>();
                    float barX = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.x;
                    float barY = MapStageList[j + 1, MapGenY].GetComponent<RectTransform>().anchoredPosition.y;
                    mapbar.GetComponent<RectTransform>().anchoredPosition = new Vector3(barX + 150, barY + 100);
                    mapBarList.Add(mapbar);
                }

                maplist.Add((MapGenX * 4) + (MapGenY + next));
                mapCount[(MapGenX * 4) + (MapGenY + next)] = 1;
                MapGenX++;
                MapGenY += next;

            }
            MapList.Add(maplist);
        }
        for (int i = 1; i <= 44; i++)
        {
            if (mapCount[i] == 0)
            {
                int dx = (i - 1) / 4 + 1;
                int dy = (i - 1) % 4 + 1;
                ObjectDestroy stageDestroy = MapStageList[dx, dy].GetComponent<ObjectDestroy>();
                stageDestroy.destroy();
            }
        }
    }

    void NextStage()
    {
        if(x == 11)
        {
            MapStageList[x, 1].interactable = false;
            MapStageList[x, 2].interactable = false;
            MapStageList[x, 3].interactable = false;
            MapStageList[x, 4].interactable = false;
            MapStageList[12, 1].interactable = true;
        }
        else if(x == 12)
        {
            //NextChapter();
        }
        else
        {
            MapStageList[x, 1].interactable = false;
            MapStageList[x, 2].interactable = false;
            MapStageList[x, 3].interactable = false;
            MapStageList[x, 4].interactable = false;
            for (int i = 0; i < 4; i++)
            {
                if (MapList[i][x - 1] == (x - 1) * 4 + y)
                {
                    MapStageList[(MapList[i][x] - 1) / 4 + 1, (MapList[i][x] - 1) % 4 + 1].interactable = true;
                }
            }
        }
        x++;
    }

    public void Normal()
    {
        NextStage();
        laststage = 1;
        UI[2].SetActive(false);
        if (Chapter == 1)
        {
            if (x <= 4)
            {
                for (int i = 0; i <= 1; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 2f);
                }

                for (int i = 0; i <= 1; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 2f + 45f);
                }
                Monster_Amount = 4;
            }
            else if (x <= 8)
            {
                for (int i = 0; i <= 2; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 2f);
                }
                for (int i = 3; i <= 5; i++)
                {
                    Invoke("Spawn_FastSlime", i * 2f);
                }

                for (int i = 0; i <= 2; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 2f + 55f);
                }
                for (int i = 3; i <= 5; i++)
                {
                    Invoke("Spawn_FastSlime", i * 2f + 55f);
                }

                Monster_Amount = 12;
            }
            else
            {
                Spawn_BigSlime();
                for (int i = 1; i <= 5; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 2f);
                }

                Invoke("Spawn_BigSlime", 55f);
                for (int i = 1; i <= 6; i++)
                {
                    Invoke("Spawn_FastSlime", i * 2f + 55f);
                }

                Monster_Amount = 13;
            }
        }
        else if (Chapter == 2)
        {
            if (x <= 4)
            {
                for (int i = 0; i <= 2; i++)
                {
                    Invoke("Spawn_Goblin", i * 2f);
                }
                for (int i = 3; i <= 4; i++)
                {
                    Invoke("Spawn_Kobold", i * 2f);
                }

                for (int i = 0; i <= 2; i++)
                {
                    Invoke("Spawn_Goblin", i * 2f + 55f);
                }
                for (int i = 3; i <= 4; i++)
                {
                    Invoke("Spawn_Kobold", i * 2f + 55f);
                }

                Monster_Amount = 10;
            }
            else if (x <= 8)
            {
                Spawn_Orc();
                for (int i = 1; i <= 2; i++)
                {
                    Invoke("Spawn_Goblin", i * 2f + 1f);
                }
                for (int i = 3; i <= 7; i++)
                {
                    Invoke("Spawn_Kobold", i * 2f + 1f);
                }

                Invoke("Spawn_Orc", 60f);
                for (int i = 1; i <= 2; i++)
                {
                    Invoke("Spawn_Goblin", i * 2f + 1f + 60f);
                }
                for (int i = 3; i <= 7; i++)
                {
                    Invoke("Spawn_Kobold", i * 2f + 1f + 60f);
                }

                Monster_Amount = 16;
            }
            else
            {
                Spawn_Orc();
                for (int i = 1; i <= 3; i++)
                {
                    Invoke("Spawn_Goblin", i * 2f + 1f);
                }
                Invoke("Spawn_Orc", 8f);
                for (int i = 5; i <= 7; i++)
                {
                    Invoke("Spawn_Kobold", i * 2f + 1f);
                }

                Invoke("Spawn_Orc", 60f);
                for (int i = 1; i <= 3; i++)
                {
                    Invoke("Spawn_Goblin", i * 2f + 1f + 60f);
                }
                Invoke("Spawn_Orc", 8f + 60f);
                for (int i = 5; i <= 7; i++)
                {
                    Invoke("Spawn_Kobold", i * 2f + 1f + 60f);
                }

                Monster_Amount = 16;
            }
        }
        else
        {
            if (x <= 4)
            {
                for (int i = 0; i <= 3; i++)
                {
                    Invoke("Spawn_Skelleton", i * 2f);
                }
                for (int i = 4; i <= 5; i++)
                {
                    Invoke("Spawn_SkelletonDog", i * 2f);
                }

                for (int i = 0; i <= 3; i++)
                {
                    Invoke("Spawn_Skelleton", i * 2f + 55f);
                }
                for (int i = 4; i <= 5; i++)
                {
                    Invoke("Spawn_SkelletonDog", i * 2f + 55f);
                }

                Monster_Amount = 12;
            }
            else if (x <= 8)
            {
                Spawn_BigSkelleton();
                for (int i = 1; i <= 3; i++)
                {
                    Invoke("Spawn_Skelleton", i * 2f);
                }
                for (int i = 4; i <= 6; i++)
                {
                    Invoke("Spawn_SkelletonDog", i * 2f);
                }

                Invoke("Spawn_BigSkelleton", 65f);
                for (int i = 1; i <= 3; i++)
                {
                    Invoke("Spawn_Skelleton", i * 2f + 65f);
                }
                for (int i = 4; i <= 6; i++)
                {
                    Invoke("Spawn_SkelletonDog", i * 2f + 65f);
                }

                Monster_Amount = 14;
            }
            else
            {
                for (int i = 0; i <= 1; i++)
                {
                    Invoke("Spawn_BigSkelleton", i * 2f);
                }
                for (int i = 2; i <= 3; i++)
                {
                    Invoke("Spawn_Skelleton", i * 2f);
                }
                for (int i = 4; i <= 5; i++)
                {
                    Invoke("Spawn_SkelletonDog", i * 2f);
                }

                for (int i = 0; i <= 1; i++)
                {
                    Invoke("Spawn_BigSkelleton", i * 2f + 60f);
                }
                for (int i = 2; i <= 3; i++)
                {
                    Invoke("Spawn_Skelleton", i * 2f + 60f);
                }
                for (int i = 4; i <= 5; i++)
                {
                    Invoke("Spawn_SkelletonDog", i * 2f + 60f);
                }

                Monster_Amount = 12;
            }
        }
    }

    public void Elite()
    {
        NextStage();
        laststage = 2;
        UI[2].SetActive(false);
        if (Chapter == 1)
        {
            if (x <= 4)
            {
                Spawn_BigSlime();
                for (int i = 1; i <= 2; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 1.8f);
                }
                for (int i = 3; i <= 4; i++)
                {
                    Invoke("Spawn_FastSlime", i * 2f);
                }

                Invoke("Spawn_HealSlime", 55f);
                for (int i = 1; i <= 2; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 1.8f + 55f);
                }
                for (int i = 3; i <= 4; i++)
                {
                    Invoke("Spawn_FastSlime", i * 2f + 55f);
                }

                Monster_Amount = 10;
            }
            else if (x <= 8)
            {
                Spawn_BigSlime();
                for (int i = 1; i <= 2; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 2f);
                }
                for (int i = 3; i <= 7; i++)
                {
                    Invoke("Spawn_FastSlime", i * 2f);
                }

                Invoke("Spawn_HealSlime", 55f);
                Invoke("Spawn_BigSlime", 56.8f);
                for (int i = 2; i <= 3; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 1.8f + 55f);
                }
                for (int i = 4; i <= 5; i++)
                {
                    Invoke("Spawn_FastSlime", i * 2f + 55f);
                }

                Monster_Amount = 14;
            }
            else
            {
                for (int i = 0; i <= 1; i++)
                {
                    Invoke("Spawn_BigSlime", i * 2f);
                }
                for (int i = 2; i <= 4; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 2f);
                }
                for (int i = 5; i <= 7; i++)
                {
                    Invoke("Spawn_FastSlime", i * 2f);
                }

                Invoke("Spawn_BigSlime", 60f);
                Invoke("Spawn_HealSlime", 1.8f + 60f);
                Invoke("Spawn_BigSlime", 3.6f + 60f);
                for (int i = 3; i <= 5; i++)
                {
                    Invoke("Spawn_BasicSlime", i * 1.8f + 60f);
                }

                Monster_Amount = 14;
            }
        }
        else if(Chapter == 2)
        {
            if (x <= 4)
            {
                Spawn_Orc();
                for (int i = 1; i <= 4; i++)
                {
                    Invoke("Spawn_Goblin", i * 2f);
                }

                Invoke("Spawn_GoblinMage", 55f);
                for (int i = 1; i <= 4; i++)
                {
                    Invoke("Spawn_Goblin", i * 1.5f + 55f);
                }
                for (int i = 4; i <= 5; i++)
                {
                    Invoke("Spawn_Kobold", i * 2f + 55f);
                }

                Monster_Amount = 12;
            }
            else if (x <= 8)
            {
                Spawn_Orc();
                for (int i = 1; i <= 3; i++)
                {
                    Invoke("Spawn_Goblin", i * 2f);
                }
                for (int i = 4; i <= 7; i++)
                {
                    Invoke("Spawn_Kobold", i * 2f + 1f);
                }

                Invoke("Spawn_Orc", 60f);
                Invoke("Spawn_GoblinMage", 62f);
                for (int i = 1; i <= 5; i++)
                {
                    Invoke("Spawn_Goblin", i * 1.5f + 2f + 60f);
                }

                Monster_Amount = 15;
            }
            else
            {
                for (int i = 1; i <= 2; i++)
                {
                    Invoke("Spawn_Orc", i * 2f);
                }
                for (int i = 4; i <= 8; i++)
                {
                    Invoke("Spawn_Goblin", i * 2f + 1f);
                }

                Invoke("Spawn_Orc", 65f);
                Invoke("Spawn_GoblinMage", 67f);
                for (int i = 1; i <= 3; i++)
                {
                    Invoke("Spawn_Goblin", i * 1.5f + 2f + 65f);
                }
                for (int i = 4; i <= 6; i++)
                {
                    Invoke("Spawn_Kobold", i * 2f + 1f + 65f);
                }

                Monster_Amount = 15;
            }
        }
        else
        {
            if (x <= 4)
            {
                Spawn_BigSkelleton();
                for (int i = 1; i <= 2; i++)
                {
                    Invoke("Spawn_Skelleton", i * 2f);
                }
                for (int i = 3; i <= 4; i++)
                {
                    Invoke("Spawn_SkelletonDog", i * 2f);
                }

                Invoke("Spawn_Deathknight", 55f);
                for (int i = 1; i <= 3; i++)
                {
                    Invoke("Spawn_Skelleton", i * 2f + 55f);
                }
                for (int i = 4; i <= 5; i++)
                {
                    Invoke("Spawn_SkelletonDog", i * 2f + 55f);
                }

                Monster_Amount = 11;
            }
            else if (x <= 8)
            {
                Spawn_BigSkelleton();
                for (int i = 1; i <= 6; i++)
                {
                    Invoke("Spawn_Skelleton", i * 2f);
                }

                Invoke("Spawn_Deathknight", 60f);
                for (int i = 1; i <= 5; i++)
                {
                    Invoke("Spawn_Skelleton", i * 2f + 60f);
                }

                Monster_Amount = 13;
            }
            else
            {
                for (int i = 0; i <= 2; i++)
                {
                    Invoke("Spawn_BigSkelleton", i * 4f);
                    Invoke("Spawn_Skelleton", i * 4f + 2f);
                }

                for (int i = 0; i <= 1; i++)
                {
                    Invoke("Spawn_Deathknight", i * 2f + 60f);
                }

                Monster_Amount = 8;
            }
        }
    }

    public void Rest()
    {
        NextStage();
        UI[2].SetActive(false);
        UI[8].SetActive(true);
        hprecover = Instantiate(HPrecover, UI[8].transform).GetComponent<Button>();
        hprecover.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0);
        hprecover.onClick.AddListener(() => GetHp());
    }

    void GetHp()
    {
        Life_decrease(-2);
        ObjectDestroy DestroyHPrecover = hprecover.GetComponent<ObjectDestroy>();
        DestroyHPrecover.destroy();
        UI[2].SetActive(true);
        UI[8].SetActive(false);
    }

    public void Shop()
    {
        NextStage();
        UI[2].SetActive(false);
        UI[8].SetActive(true);
        GenCard();
        GenArtifact();
        shopCard = Instantiate(ShopCard, UI[8].transform).GetComponent<Button>();
        shopCard.GetComponent<RectTransform>().anchoredPosition = new Vector3(-800, 0);
        shopCard.onClick.AddListener(() => GetCardShop());
        shopArtifact = Instantiate(ShopArtifact, UI[8].transform).GetComponent<Button>();
        shopArtifact.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0);
        shopArtifact.onClick.AddListener(() => GetArtifactShop());
    }

    public void Boss()
    {
        laststage = 3;
        UI[2].SetActive(false);
        if (Chapter == 1)
        {
            Spawn_BigSlime();
            Invoke("Spawn_BasicSlime", 2f);
            Invoke("Spawn_BasicSlime", 4f);
            Invoke("Spawn_BasicSlime", 6f);
            Invoke("Spawn_BigSlime", 8f);
            Invoke("Spawn_BasicSlime", 10f);
            Invoke("Spawn_BasicSlime", 12f);
            Invoke("Spawn_FastSlime", 14f);
            Invoke("Spawn_FastSlime", 16f);

            Invoke("Spawn_KingSlime", 60f);
            Invoke("Spawn_BasicSlime", 62f);
            Invoke("Spawn_BasicSlime", 64f);
            Invoke("Spawn_FastSlime", 66f);
            Invoke("Spawn_FastSlime", 68f);

            Monster_Amount = 16;

        }
        else if (Chapter == 2)
        {
            Spawn_Orc();
            Invoke("Spawn_Orc", 2f);
            Invoke("Spawn_Orc", 4f);

            Invoke("Spawn_OrcKing", 60f);
            Invoke("Spawn_Goblin", 62f);
            Invoke("Spawn_Goblin", 64f);
            Invoke("Spawn_Goblin", 66f);

            Monster_Amount = 7;
        }
        else
        {
            Spawn_BigSkelleton();
            Invoke("Spawn_Skelleton", 2f);
            Invoke("Spawn_Skelleton", 4f);
            Invoke("Spawn_Skelleton", 6f);

            Invoke("Spawn_BigSkelleton", 16f);
            Invoke("Spawn_Skelleton", 18f);
            Invoke("Spawn_Skelleton", 20f);
            Invoke("Spawn_Skelleton", 22f);
            Invoke("Spawn_Skelleton", 24f);

            Invoke("Spawn_Deathknight", 70f);
            Invoke("Spawn_BigSkelleton", 72f);
            Invoke("Spawn_Lich", 75f);

            Monster_Amount = 12;
        }
    }

    public void Unknown()
    {
        int randomStage = Random.Range(0, 5);
        switch(randomStage)
        {
            case 0:
                Normal();
                break;
            case 1:
                Elite();
                break;
            case 2:
                Rest();
                break;
            case 3:
                Shop();
                break;
            case 4:
                Treasure();
                break;
        }
    }

    public void Treasure()
    {
        NextStage();
        UI[2].SetActive(false);
        UI[8].SetActive(true);
        int num;
        while (true)
        {
            num = Random.Range(0, Artifact_Image.Length);
            if (num != 14) break;
        }
        treasure = Instantiate(Artifact_Reward[num], UI[8].transform).GetComponent<Button>();
        treasure.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0);
        treasure.onClick.AddListener(() => TreasureFinish(num));
    }

    public void TreasureNext()
    {
        if(treasure)
        {
            ObjectDestroy DestroyTreasure = treasure.GetComponent<ObjectDestroy>();
            DestroyTreasure.destroy();
        }
        if(shopCard)
        {
            ObjectDestroy DestroyshopCard = shopCard.GetComponent<ObjectDestroy>();
            DestroyshopCard.destroy();
        }
        if (shopArtifact)
        {
            ObjectDestroy DestroyshopArtifact = shopArtifact.GetComponent<ObjectDestroy>();
            DestroyshopArtifact.destroy();
        }
        UI[2].SetActive(true);
        UI[8].SetActive(false);
    }

    void TreasureFinish(int num)
    {
        Artifact_Count++;
        Artifact_Abillity(num);
        GameObject artifact_image = Instantiate(Artifact_Image[num], BagCanvasPos);
        artifact_image.GetComponent<RectTransform>().anchoredPosition = new Vector3(BagCanvasPos.position.x - 420 + ((Artifact_Count - 1) % 13 * 70), BagCanvasPos.position.y + 270 - ((Artifact_Count - 1) / 13 * 70));
        Artifact_List.Add(artifact_image);
        ObjectDestroy DestroyTreasure = treasure.GetComponent<ObjectDestroy>();
        DestroyTreasure.destroy();
        UI[2].SetActive(true);
        UI[8].SetActive(false);

    }

    //Ã©ÅÍ1 ¸ó½ºÅÍ ¼ÒÈ¯
    void Spawn_BasicSlime()
    {
        Instantiate(Chapter1_Monster[0], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_FastSlime()
    {
        Instantiate(Chapter1_Monster[1], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_BigSlime()
    {
        Instantiate(Chapter1_Monster[2], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_HealSlime()
    {
        Instantiate(Chapter1_Monster[3], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_KingSlime()
    {
        Instantiate(Chapter1_Monster[4], Spawnpoint.position, Quaternion.identity);
    }

    //Ã©ÅÍ2 ¸ó½ºÅÍ ¼ÒÈ¯
    void Spawn_Goblin()
    {
        Instantiate(Chapter2_Monster[0], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_Kobold()
    {
        Instantiate(Chapter2_Monster[1], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_Orc()
    {
        Instantiate(Chapter2_Monster[2], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_GoblinMage()
    {
        Instantiate(Chapter2_Monster[3], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_OrcKing()
    {
        Instantiate(Chapter2_Monster[4], Spawnpoint.position, Quaternion.identity);
    }

    //Ã©ÅÍ3 ¸ó½ºÅÍ ¼ÒÈ¯
    void Spawn_Skelleton()
    {
        Instantiate(Chapter3_Monster[0], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_SkelletonDog()
    {
        Instantiate(Chapter3_Monster[1], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_BigSkelleton()
    {
        Instantiate(Chapter3_Monster[2], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_Deathknight()
    {
        Instantiate(Chapter3_Monster[3], Spawnpoint.position, Quaternion.identity);
    }
    void Spawn_Lich()
    {
        Instantiate(Chapter3_Monster[4], Spawnpoint.position, Quaternion.identity);
    }

    //º¸»óÈ¹µæ´Ü°è·Î ³Ñ¾î°¡±â
    void GetReward(int stage)
    {
        int Artifact_Chance = Random.Range(0, 100) + 1;
        //int Equipment = Random.Range(0, 100) + 1;
        getCard = true;
        getArtifact = false;
        //getEquipment = false;
        int count = 1;

        if (stage == 1) //³ë¸»½ºÅ×ÀÌÁö
        {
            if (Artifact_Chance > 70) getArtifact = true;
            //if (Equipment > 90) getEquipment = true;
            Gold += (10 + 2 * x + (Chapter - 1) * 3);
        }
        else if(stage == 2) //¿¤¸®Æ®½ºÅ×ÀÌÁö
        {
            if (Artifact_Chance <= 70) getArtifact = true;
            //if (Equipment > 50) getEquipment = true;
            Gold += (20 + 2 * x + (Chapter - 1) * 3);
        }
        else //º¸½º½ºÅ×ÀÌÁö
        {
            getArtifact = true;
            //getEquipment = true;
            Gold += (30 + 2 * x + (Chapter - 1) * 3);
        }
        GoldText.text = Gold.ToString();
        UI[3].SetActive(true);
        if (getCard == true)
        {
            reward_card = Instantiate(RewardBoards[0], RewardCanvasPos).GetComponent<Button>();
            reward_card.GetComponent<RectTransform>().anchoredPosition = new Vector3(RewardCanvasPos.position.x, RewardCanvasPos.position.y + count * 200);
            reward_card.onClick.AddListener(() => GetCard());
            count--;
        }
        if (getArtifact == true)
        {
            reward_artifact = Instantiate(RewardBoards[1], RewardCanvasPos).GetComponent<Button>();
            reward_artifact.GetComponent<RectTransform>().anchoredPosition = new Vector3(RewardCanvasPos.position.x, RewardCanvasPos.position.y + count * 200);
            reward_artifact.onClick.AddListener(() => GetArtifact());
            count--;
        }
        /*
        if (getEquipment == true)
        {
            reward_equipment = Instantiate(RewardBoards[2], RewardCanvasPos).GetComponent<Button>();
            reward_equipment.GetComponent<RectTransform>().anchoredPosition = new Vector3(RewardCanvasPos.position.x, RewardCanvasPos.position.y + count * 200);
            reward_equipment.onClick.AddListener(() => GetEquipment());
        }
        */

        //Ä«µå»Ì±â
        GenCard();


        //À¯¹°»Ì±â
        if (getArtifact == true)
        {
            GenArtifact();
        }

        //Àåºñ»Ì±â
    }

    void GenCard()
    {
        while (true)
        {
            Card_num1 = Random.Range(0, SkillCards.Length);
            if (Card_num1 != archer.Skill1 && Card_num1 != archer.Skill2 && (Card_num1 != (archer.NormalAttackNum + 4) || archer.NormalAttackNum == 0) &&
                Card_num1 != mage.Skill1 && Card_num1 != mage.Skill2 && (Card_num1 != (mage.NormalAttackNum + 12) || mage.NormalAttackNum == 0) &&
                Card_num1 != thief.Skill1 && Card_num1 != thief.Skill2 && (Card_num1 != (thief.NormalAttackNum + 20) || thief.NormalAttackNum == 0) &&
                Card_num1 != warrior.Skill1 && Card_num1 != warrior.Skill2 && (Card_num1 != (warrior.NormalAttackNum + 28) || warrior.NormalAttackNum == 0)) break;
        }
        while (true)
        {
            Card_num2 = Random.Range(0, SkillCards.Length);
            if (Card_num2 != archer.Skill1 && Card_num2 != archer.Skill2 && (Card_num2 != (archer.NormalAttackNum + 4) || archer.NormalAttackNum == 0) &&
                Card_num2 != mage.Skill1 && Card_num2 != mage.Skill2 && (Card_num2 != (mage.NormalAttackNum + 12) || mage.NormalAttackNum == 0) &&
                Card_num2 != thief.Skill1 && Card_num2 != thief.Skill2 && (Card_num2 != (thief.NormalAttackNum + 20) || thief.NormalAttackNum == 0) &&
                Card_num2 != warrior.Skill1 && Card_num2 != warrior.Skill2 && (Card_num2 != (warrior.NormalAttackNum + 28) || warrior.NormalAttackNum == 0) &&
                Card_num2 != Card_num1) break;
        }
        while (true)
        {
            Card_num3 = Random.Range(0, SkillCards.Length);
            if (Card_num3 != archer.Skill1 && Card_num3 != archer.Skill2 && (Card_num3 != (archer.NormalAttackNum + 4) || archer.NormalAttackNum == 0) &&
                Card_num3 != mage.Skill1 && Card_num3 != mage.Skill2 && (Card_num3 != (mage.NormalAttackNum + 12) || mage.NormalAttackNum == 0) &&
                Card_num3 != thief.Skill1 && Card_num3 != thief.Skill2 && (Card_num3 != (thief.NormalAttackNum + 20) || thief.NormalAttackNum == 0) &&
                Card_num3 != warrior.Skill1 && Card_num3 != warrior.Skill2 && (Card_num3 != (warrior.NormalAttackNum + 28) || warrior.NormalAttackNum == 0) &&
                Card_num3 != Card_num1 && Card_num3 != Card_num2) break;
        }

        Card1 = Instantiate(SkillCards[Card_num1], GetCardRewardCanvasPos).GetComponent<Button>();
        Card1.GetComponent<RectTransform>().anchoredPosition = new Vector3(GetCardRewardCanvasPos.position.x - 600, GetCardRewardCanvasPos.position.y);
        Card1.onClick.AddListener(() => GetSkillManage(Card_num1));

        Card2 = Instantiate(SkillCards[Card_num2], GetCardRewardCanvasPos).GetComponent<Button>();
        Card2.GetComponent<RectTransform>().anchoredPosition = new Vector3(GetCardRewardCanvasPos.position.x, GetCardRewardCanvasPos.position.y);
        Card2.onClick.AddListener(() => GetSkillManage(Card_num2));

        Card3 = Instantiate(SkillCards[Card_num3], GetCardRewardCanvasPos).GetComponent<Button>();
        Card3.GetComponent<RectTransform>().anchoredPosition = new Vector3(GetCardRewardCanvasPos.position.x + 600, GetCardRewardCanvasPos.position.y);
        Card3.onClick.AddListener(() => GetSkillManage(Card_num3));
    }

    void GenArtifact()
    {
        int Artifact_num;
        while (true)
        {
            Artifact_num = Random.Range(0, Artifact_Image.Length);
            if (Artifact_num != 14) break;
        }

        Artifact = Instantiate(Artifact_Reward[Artifact_num], GetArtifactRewardCanvasPos).GetComponent<Button>(); ;
        Artifact.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0);
        Artifact.onClick.AddListener(() => GetArtifactManage(Artifact_num));
    }

    void GenEquipment()
    {

    }

    //º¸»ó È¹µæ Á¾·á
    public void GetRewardFinish()
    {
        if (getCard == true)
        {
            ObjectDestroy DestroyRewardCard = reward_card.GetComponent<ObjectDestroy>();
            DestroyRewardCard.destroy();
        }
        if (getArtifact == true)
        {
            ObjectDestroy DestroyRewardArtifact = reward_artifact.GetComponent<ObjectDestroy>();
            DestroyRewardArtifact.destroy();
        }
        if (getEquipment == true)
        {
            ObjectDestroy DestroyRewardEquipment = reward_equipment.GetComponent<ObjectDestroy>();
            DestroyRewardEquipment.destroy();
        }
        if (Card1)
        {
            ObjectDestroy DestroyCard1 = Card1.GetComponent<ObjectDestroy>();
            DestroyCard1.destroy();
        }
        if (Card2)
        {
            ObjectDestroy DestroyCard2 = Card2.GetComponent<ObjectDestroy>();
            DestroyCard2.destroy();
        }
        if (Card3)
        {
            ObjectDestroy DestroyCard3 = Card3.GetComponent<ObjectDestroy>();
            DestroyCard3.destroy();
        }
        if (Artifact)
        {
            ObjectDestroy DestroyArtifact = Artifact.GetComponent<ObjectDestroy>();
            DestroyArtifact.destroy();
        }
        UI[3].SetActive(false);
        UI[2].SetActive(true);

        archer.ShotAnimation_End = true;
        mage.ShotAnimation_End = true;
        thief.ShotAnimation_End = true;
        warrior.ShotAnimation_End = true;

        archer.curSkill1Delay = archer.maxSkill1Delay;
        archer.curSkill2Delay = archer.maxSkill2Delay;
        mage.curSkill1Delay = mage.maxSkill1Delay;
        mage.curSkill2Delay = mage.maxSkill2Delay;
        thief.curSkill1Delay = thief.maxSkill1Delay;
        thief.curSkill2Delay = thief.maxSkill2Delay;
        warrior.curSkill1Delay = warrior.maxSkill1Delay;
        warrior.curSkill2Delay = warrior.maxSkill2Delay;

        if (laststage == 3)
        {
            NextChapter();
        }
    }

    //Ä«µå È¹µæ
    void GetCard()
    {
        //UI[3].SetActive(false);
        UI[4].SetActive(true);
    }

    void GetCardShop()
    {
        if (Gold < 150) return;
        //UI[3].SetActive(false);
        Gold -= 150;
        GoldText.text = Gold.ToString();
        UI[4].SetActive(true);
        shopCard.interactable = false;
    }

    //À¯¹° È¹µæ
    void GetArtifact()
    {
        //UI[3].SetActive(false);
        UI[7].SetActive(true);
    }

    void GetArtifactShop()
    {
        if (Gold < 100) return;
        //UI[3].SetActive(false);
        Gold -= 100;
        GoldText.text = Gold.ToString();
        UI[7].SetActive(true);
        shopArtifact.interactable = false;
    }

    //Àåºñ È¹µæ
    void GetEquipment()
    {

    }

    public void GetCardRewardBackSpace()
    {
        UI[4].SetActive(false);
        //UI[3].SetActive(true);
    }

    public void GetArtifactRewardBackSpace()
    {
        UI[7].SetActive(false);
        //UI[3].SetActive(true);
    }

    //½ºÅ³ È¹µæ °ü¸®
    void GetSkillManage(int Skill_num)
    {
        if (0 <= Skill_num && Skill_num <= 4) 
        {
            archer.GetSkill(Skill_num);
        }
        else if (5 <= Skill_num && Skill_num <= 7)
        {
            archer.NormalAttackChange(Skill_num - 5);
        }
        else if (8 <= Skill_num && Skill_num <= 12)
        {
            mage.GetSkill(Skill_num);
        }
        else if (13 <= Skill_num && Skill_num <= 15)
        {
            mage.NormalAttackChange(Skill_num - 13);
        }
        else if (16 <= Skill_num && Skill_num <= 20)
        {
            thief.GetSkill(Skill_num);
        }
        else if (21 <= Skill_num && Skill_num <= 23)
        {
            thief.NormalAttackChange(Skill_num - 21);
        }
        else if (24 <= Skill_num && Skill_num <= 28)
        {
            warrior.GetSkill(Skill_num);
        }
        else if (29 <= Skill_num && Skill_num <= 31)
        {
            warrior.NormalAttackChange(Skill_num - 29);
        }
        else if (32 <= Skill_num && Skill_num <= 35)
        {
            archer.GetStatus(Skill_num - 32);
        }
        else if (36 <= Skill_num && Skill_num <= 39)
        {
            mage.GetStatus(Skill_num - 36);
        }
        else if (40 <= Skill_num && Skill_num <= 43)
        {
            thief.GetStatus(Skill_num - 40);
        }
        else if (44 <= Skill_num && Skill_num <= 47)
        {
            warrior.GetStatus(Skill_num - 44);
        }
    }

    //À¯¹° È¹µæ °ü¸®
    void GetArtifactManage(int artifact_num)
    {
        Artifact_Count++;
        Artifact_Abillity(artifact_num);
        GameObject artifact_image = Instantiate(Artifact_Image[artifact_num], BagCanvasPos);
        artifact_image.GetComponent<RectTransform>().anchoredPosition = new Vector3(BagCanvasPos.position.x - 420 + ((Artifact_Count - 1) % 13 * 70), BagCanvasPos.position.y + 270 - ((Artifact_Count - 1) / 13 * 70));
        Artifact_List.Add(artifact_image);
        reward_artifact.interactable = false;
        ObjectDestroy DestroyArtifact = Artifact.GetComponent<ObjectDestroy>();
        DestroyArtifact.destroy();
        //UI[3].SetActive(true);
        if (shopArtifact)
        {
            shopArtifact.interactable = false;
        }
        UI[7].SetActive(false);
    }

    void Artifact_Abillity(int artifact_num)
    {
        switch(artifact_num)
        {
            case 0:
                archer.AttackDamage += 1;
                mage.AttackDamage += 1;
                thief.AttackDamage += 1;
                thief.attackdamage += 1;
                warrior.AttackDamage += 1;
                break;
            case 1:
                warrior.maxSkill1Delay *= 0.9f;
                warrior.maxSkill2Delay *= 0.9f;
                for (int i = 0; i < warrior.SkillDelay.Length; i++)
                {
                    warrior.SkillDelay[i] *= 0.9f;
                }
                break;
            case 2:
                warrior.AttackDamage -= 5;
                warrior.maxSkill1Delay *= 0.9f;
                warrior.maxSkill2Delay *= 0.9f;
                for (int i = 0; i < warrior.SkillDelay.Length; i++)
                {
                    warrior.SkillDelay[i] *= 0.9f;
                }
                break;
            case 3:
                archer.maxSkill1Delay *= 0.9f;
                archer.maxSkill2Delay *= 0.9f;
                for (int i = 0; i < archer.SkillDelay.Length; i++)
                {
                    archer.SkillDelay[i] *= 0.9f;
                }
                break;
            case 4:
                archer.AttackDamage += 5;
                break;
            case 5:
                archer.AttackDamage += 1;
                mage.AttackDamage += 1;
                thief.AttackDamage += 1;
                thief.attackdamage += 1;
                warrior.AttackDamage += 1;
                break;
            case 6:
                Life_decrease(-1);
                break;
            case 7:
                archer.maxAttackDelay *= 0.9f;
                archer.maxattackdelay *= 0.9f;
                mage.maxAttackDelay *= 0.9f;
                thief.maxAttackDelay *= 0.9f;
                warrior.maxAttackDelay *= 0.9f;
                warrior.maxattackdelay *= 0.9f;

                archer.maxSkill1Delay *= 1.05f;
                archer.maxSkill2Delay *= 1.05f;
                for (int i = 0; i < archer.SkillDelay.Length; i++)
                {
                    archer.SkillDelay[i] *= 1.05f;
                }
                mage.maxSkill1Delay *= 1.05f;
                mage.maxSkill2Delay *= 1.05f;
                for (int i = 0; i < mage.SkillDelay.Length; i++)
                {
                    mage.SkillDelay[i] *= 1.05f;
                }
                thief.maxSkill1Delay *= 1.05f;
                thief.maxSkill2Delay *= 1.05f;
                for (int i = 0; i < thief.SkillDelay.Length; i++)
                {
                    thief.SkillDelay[i] *= 1.05f;
                }
                warrior.maxSkill1Delay *= 1.05f;
                warrior.maxSkill2Delay *= 1.05f;
                for (int i = 0; i < warrior.SkillDelay.Length; i++)
                {
                    warrior.SkillDelay[i] *= 1.05f;
                }
                break;
            case 8:
                thief.maxSkill1Delay *= 0.9f;
                thief.maxSkill2Delay *= 0.9f;
                for (int i = 0; i < thief.SkillDelay.Length; i++)
                {
                    thief.SkillDelay[i] *= 0.9f;
                }
                break;
            case 9:
                thief.AttackDamage += 5;
                thief.attackdamage += 5;
                break;
            case 10:
                mage.AttackDamage -= 5;
                mage.maxSkill1Delay *= 0.9f;
                mage.maxSkill2Delay *= 0.9f;
                for (int i = 0; i < mage.SkillDelay.Length; i++)
                {
                    mage.SkillDelay[i] *= 0.9f;
                }
                break;
            case 11:
                thief.maxAttackDelay *= 0.9f;
                break;
            case 12:
                archer.AttackDamage -= 5;
                mage.AttackDamage += 5;
                break;
            case 13:
                mage.maxSkill1Delay *= 0.9f;
                mage.maxSkill2Delay *= 0.9f;
                for (int i = 0; i < mage.SkillDelay.Length; i++)
                {
                    mage.SkillDelay[i] *= 0.9f;
                }
                break;
            case 14:
                DiscountRate = 10f;
                break;
            case 15:
                warrior.AttackDamage -= 5;
                thief.AttackDamage += 5;
                thief.attackdamage += 5;
                break;
            case 16:
                warrior.maxAttackDelay *= 0.9f;
                warrior.maxattackdelay *= 0.9f;
                break;
            case 17:
                archer.maxAttackDelay *= 0.9f;
                archer.maxattackdelay *= 0.9f;
                break;
            case 18:
                Life_decrease(-1);
                break;
            case 19:
                warrior.AttackDamage += 5;
                break;
            case 20:
                mage.AttackDamage += 5;
                break;
            case 21:
                mage.maxAttackDelay *= 0.9f;
                break;
            case 22:
                archer.maxAttackDelay *= 0.95f;
                archer.maxattackdelay *= 0.95f;
                mage.maxAttackDelay *= 0.95f;
                thief.maxAttackDelay *= 0.95f;
                warrior.maxAttackDelay *= 0.95f;
                warrior.maxattackdelay *= 0.95f;
                break;
        }
    }

    public void ChangeSkillManage(int job, int skill_num) // 1. ¾ÆÃ³, 2. ¸¶¹ý»ç, 3. µµÀû, 4. Àü»ç
    {
        UI[5].SetActive(true);
        UI[4].SetActive(false);
        if (job == 1)
        {
            selectCard = Instantiate(SkillCards[skill_num], ChangeCardCanvasPos).GetComponent<Button>();
            selectCard.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x - 650, ChangeCardCanvasPos.position.y);

            changeCard1 = Instantiate(SkillCards[archer.Skill1], ChangeCardCanvasPos).GetComponent<Button>();
            changeCard1.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x + 200, ChangeCardCanvasPos.position.y);
            changeCard1.onClick.AddListener(() => archer.ChangeSkill(skill_num, 1));

            changeCard2 = Instantiate(SkillCards[archer.Skill2], ChangeCardCanvasPos).GetComponent<Button>();
            changeCard2.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x + 800, ChangeCardCanvasPos.position.y);
            changeCard2.onClick.AddListener(() => archer.ChangeSkill(skill_num, 2));
        }
        else if (job == 2)
        {
            selectCard = Instantiate(SkillCards[skill_num], ChangeCardCanvasPos).GetComponent<Button>();
            selectCard.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x - 650, ChangeCardCanvasPos.position.y);

            changeCard1 = Instantiate(SkillCards[mage.Skill1], ChangeCardCanvasPos).GetComponent<Button>();
            changeCard1.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x + 200, ChangeCardCanvasPos.position.y);
            changeCard1.onClick.AddListener(() => mage.ChangeSkill(skill_num, 1));

            changeCard2 = Instantiate(SkillCards[mage.Skill2], ChangeCardCanvasPos).GetComponent<Button>();
            changeCard2.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x + 800, ChangeCardCanvasPos.position.y);
            changeCard2.onClick.AddListener(() => mage.ChangeSkill(skill_num, 2));
        }
        else if (job == 3)
        {
            selectCard = Instantiate(SkillCards[skill_num], ChangeCardCanvasPos).GetComponent<Button>();
            selectCard.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x - 650, ChangeCardCanvasPos.position.y);

            changeCard1 = Instantiate(SkillCards[thief.Skill1], ChangeCardCanvasPos).GetComponent<Button>();
            changeCard1.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x + 200, ChangeCardCanvasPos.position.y);
            changeCard1.onClick.AddListener(() => thief.ChangeSkill(skill_num, 1));

            changeCard2 = Instantiate(SkillCards[thief.Skill2], ChangeCardCanvasPos).GetComponent<Button>();
            changeCard2.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x + 800, ChangeCardCanvasPos.position.y);
            changeCard2.onClick.AddListener(() => thief.ChangeSkill(skill_num, 2));
        }
        else
        {
            selectCard = Instantiate(SkillCards[skill_num], ChangeCardCanvasPos).GetComponent<Button>();
            selectCard.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x - 650, ChangeCardCanvasPos.position.y);

            changeCard1 = Instantiate(SkillCards[warrior.Skill1], ChangeCardCanvasPos).GetComponent<Button>();
            changeCard1.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x + 200, ChangeCardCanvasPos.position.y);
            changeCard1.onClick.AddListener(() => warrior.ChangeSkill(skill_num, 1));

            changeCard2 = Instantiate(SkillCards[warrior.Skill2], ChangeCardCanvasPos).GetComponent<Button>();
            changeCard2.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChangeCardCanvasPos.position.x + 800, ChangeCardCanvasPos.position.y);
            changeCard2.onClick.AddListener(() => warrior.ChangeSkill(skill_num, 2));
        }
    }

    public void SkillManagefinish()
    {
        reward_card.interactable = false;
        ObjectDestroy DestroyCard1 = Card1.GetComponent<ObjectDestroy>();
        DestroyCard1.destroy();
        ObjectDestroy DestroyCard2 = Card2.GetComponent<ObjectDestroy>();
        DestroyCard2.destroy();
        ObjectDestroy DestroyCard3 = Card3.GetComponent<ObjectDestroy>();
        DestroyCard3.destroy();
        if (shopCard)
        {
            shopCard.interactable = false;
        }
        //UI[3].SetActive(true);
        UI[4].SetActive(false);
        Show_Skill_Icons();
        ShowCoolTimeImage();
        ShowCoolTimeText();
    }

    public void Show_Skill_Icons()
    {
        if(Archer_Icon1)
        {
            ObjectDestroy DestroyArcherIcon1 = Archer_Icon1.GetComponent<ObjectDestroy>();
            DestroyArcherIcon1.destroy();
        }
        if (Archer_Icon2)
        {
            ObjectDestroy DestroyArcherIcon2 = Archer_Icon2.GetComponent<ObjectDestroy>();
            DestroyArcherIcon2.destroy();
        }
        if (Mage_Icon1)
        {
            ObjectDestroy DestroyMageIcon1 = Mage_Icon1.GetComponent<ObjectDestroy>();
            DestroyMageIcon1.destroy();
        }
        if (Mage_Icon2)
        {
            ObjectDestroy DestroyMageIcon2 = Mage_Icon2.GetComponent<ObjectDestroy>();
            DestroyMageIcon2.destroy();
        }
        if (Thief_Icon1)
        {
            ObjectDestroy DestroyThiefIcon1 = Thief_Icon1.GetComponent<ObjectDestroy>();
            DestroyThiefIcon1.destroy();
        }
        if (Thief_Icon2)
        {
            ObjectDestroy DestroyThiefIcon2 = Thief_Icon2.GetComponent<ObjectDestroy>();
            DestroyThiefIcon2.destroy();
        }
        if (Warrior_Icon1)
        {
            ObjectDestroy DestroyWarriorIcon1 = Warrior_Icon1.GetComponent<ObjectDestroy>();
            DestroyWarriorIcon1.destroy();
        }
        if (Warrior_Icon2)
        {
            ObjectDestroy DestroyWarriorIcon2 = Warrior_Icon2.GetComponent<ObjectDestroy>();
            DestroyWarriorIcon2.destroy();
        }
        
        if (archer.Skill1 != -1)
        {
            Archer_Icon1 = Instantiate(Archer_Icons[archer.Skill1], GameUICanvasPos);
            Archer_Icon1.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 1002, GameUICanvasPos.position.y - 475);
        }
        if (archer.Skill2 != -1)
        {
            Archer_Icon2 = Instantiate(Archer_Icons[archer.Skill2], GameUICanvasPos);
            Archer_Icon2.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 852, GameUICanvasPos.position.y - 475);
        }
        if (mage.Skill1 != -1)
        {
            Mage_Icon1 = Instantiate(Mage_Icons[mage.Skill1 - 8], GameUICanvasPos);
            Mage_Icon1.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 402, GameUICanvasPos.position.y - 475);
        }
        if (mage.Skill2 != -1)
        {
            Mage_Icon2 = Instantiate(Mage_Icons[mage.Skill2 - 8], GameUICanvasPos);
            Mage_Icon2.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 252, GameUICanvasPos.position.y - 475);
        }
        if (thief.Skill1 != -1) 
        {
            Thief_Icon1 = Instantiate(Theif_Icons[thief.Skill1 - 16], GameUICanvasPos);
            Thief_Icon1.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 248, GameUICanvasPos.position.y - 475);
        }
        if (thief.Skill2 != -1)
        {
            Thief_Icon2 = Instantiate(Theif_Icons[thief.Skill2 - 16], GameUICanvasPos);
            Thief_Icon2.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 398, GameUICanvasPos.position.y - 475);
        }
        if (warrior.Skill1 != -1)
        {
            Warrior_Icon1 = Instantiate(Warrior_Icons[warrior.Skill1 - 24], GameUICanvasPos);
            Warrior_Icon1.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 848, GameUICanvasPos.position.y - 475);
        }
        if (warrior.Skill2 != -1)
        {
            Warrior_Icon2 = Instantiate(Warrior_Icons[warrior.Skill2 - 24], GameUICanvasPos);
            Warrior_Icon2.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 998, GameUICanvasPos.position.y - 475);
        }
    }

    void ShowCoolTimeImage()
    {
        if (Archer_Icon1_Image)
        {
            ObjectDestroy DestroyArcherIcon1_Image = Archer_Icon1_Image.GetComponent<ObjectDestroy>();
            DestroyArcherIcon1_Image.destroy();
        }
        if (Archer_Icon2_Image)
        {
            ObjectDestroy DestroyArcherIcon2_Image = Archer_Icon2_Image.GetComponent<ObjectDestroy>();
            DestroyArcherIcon2_Image.destroy();
        }
        if (Mage_Icon1_Image)
        {
            ObjectDestroy DestroyMageIcon1_Image = Mage_Icon1_Image.GetComponent<ObjectDestroy>();
            DestroyMageIcon1_Image.destroy();
        }
        if (Mage_Icon2_Image)
        {
            ObjectDestroy DestroyMageIcon2_Image = Mage_Icon2_Image.GetComponent<ObjectDestroy>();
            DestroyMageIcon2_Image.destroy();
        }
        if (Thief_Icon1_Image)
        {
            ObjectDestroy DestroyThiefIcon1_Image = Thief_Icon1_Image.GetComponent<ObjectDestroy>();
            DestroyThiefIcon1_Image.destroy();
        }
        if (Thief_Icon2_Image)
        {
            ObjectDestroy DestroyThiefIcon2_Image = Thief_Icon2_Image.GetComponent<ObjectDestroy>();
            DestroyThiefIcon2_Image.destroy();
        }
        if (Warrior_Icon1_Image)
        {
            ObjectDestroy DestroyWarriorIcon1_Image = Warrior_Icon1_Image.GetComponent<ObjectDestroy>();
            DestroyWarriorIcon1_Image.destroy();
        }
        if (Warrior_Icon2_Image)
        {
            ObjectDestroy DestroyWarriorIcon2_Image = Warrior_Icon2_Image.GetComponent<ObjectDestroy>();
            DestroyWarriorIcon2_Image.destroy();
        }

        if (archer.Skill1 != -1)
        {
            Archer_Icon1_Image = Instantiate(CoolTimeImage, GameUICanvasPos);
            Archer_Icon1_Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 1002, GameUICanvasPos.position.y - 475);
            Image CoolTimeimage = Archer_Icon1_Image.GetComponent<Image>();
            CoolTimeimage.fillAmount = 0;
        }
        if (archer.Skill2 != -1)
        {
            Archer_Icon2_Image = Instantiate(CoolTimeImage, GameUICanvasPos);
            Archer_Icon2_Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 852, GameUICanvasPos.position.y - 475);
            Image CoolTimeimage = Archer_Icon2_Image.GetComponent<Image>();
            CoolTimeimage.fillAmount = 0;
        }
        if (mage.Skill1 != -1)
        {
            Mage_Icon1_Image = Instantiate(CoolTimeImage, GameUICanvasPos);
            Mage_Icon1_Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 402, GameUICanvasPos.position.y - 475);
            Image CoolTimeimage = Mage_Icon1_Image.GetComponent<Image>();
            CoolTimeimage.fillAmount = 0;
        }
        if (mage.Skill2 != -1)
        {
            Mage_Icon2_Image = Instantiate(CoolTimeImage, GameUICanvasPos);
            Mage_Icon2_Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 252, GameUICanvasPos.position.y - 475);
            Image CoolTimeimage = Mage_Icon2_Image.GetComponent<Image>();
            CoolTimeimage.fillAmount = 0;
        }
        if (thief.Skill1 != -1)
        {
            Thief_Icon1_Image = Instantiate(CoolTimeImage, GameUICanvasPos);
            Thief_Icon1_Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 248, GameUICanvasPos.position.y - 475);
            Image CoolTimeimage = Thief_Icon1_Image.GetComponent<Image>();
            CoolTimeimage.fillAmount = 0;
        }
        if (thief.Skill2 != -1)
        {
            Thief_Icon2_Image = Instantiate(CoolTimeImage, GameUICanvasPos);
            Thief_Icon2_Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 398, GameUICanvasPos.position.y - 475);
            Image CoolTimeimage = Thief_Icon2_Image.GetComponent<Image>();
            CoolTimeimage.fillAmount = 0;
        }
        if (warrior.Skill1 != -1)
        {
            Warrior_Icon1_Image = Instantiate(CoolTimeImage, GameUICanvasPos);
            Warrior_Icon1_Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 848, GameUICanvasPos.position.y - 475);
            Image CoolTimeimage = Warrior_Icon1_Image.GetComponent<Image>();
            CoolTimeimage.fillAmount = 0;
        }
        if (warrior.Skill2 != -1)
        {
            Warrior_Icon2_Image = Instantiate(CoolTimeImage, GameUICanvasPos);
            Warrior_Icon2_Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 998, GameUICanvasPos.position.y - 475);
            Image CoolTimeimage = Warrior_Icon2_Image.GetComponent<Image>();
            CoolTimeimage.fillAmount = 0;
        }
    }

    void ShowCoolTimeText()
    {
        if (Archer_Icon1_Text)
        {
            ObjectDestroy DestroyArcherIcon1_Text = Archer_Icon1_Text.GetComponent<ObjectDestroy>();
            DestroyArcherIcon1_Text.destroy();
        }
        if (Archer_Icon2_Text)
        {
            ObjectDestroy DestroyArcherIcon2_Text = Archer_Icon2_Text.GetComponent<ObjectDestroy>();
            DestroyArcherIcon2_Text.destroy();
        }
        if (Mage_Icon1_Text)
        {
            ObjectDestroy DestroyMageIcon1_Text = Mage_Icon1_Text.GetComponent<ObjectDestroy>();
            DestroyMageIcon1_Text.destroy();
        }
        if (Mage_Icon2_Text)
        {
            ObjectDestroy DestroyMageIcon2_Text = Mage_Icon2_Text.GetComponent<ObjectDestroy>();
            DestroyMageIcon2_Text.destroy();
        }
        if (Thief_Icon1_Text)
        {
            ObjectDestroy DestroyThiefIcon1_Text = Thief_Icon1_Text.GetComponent<ObjectDestroy>();
            DestroyThiefIcon1_Text.destroy();
        }
        if (Thief_Icon2_Text)
        {
            ObjectDestroy DestroyThiefIcon2_Text = Thief_Icon2_Text.GetComponent<ObjectDestroy>();
            DestroyThiefIcon2_Text.destroy();
        }
        if (Warrior_Icon1_Text)
        {
            ObjectDestroy DestroyWarriorIcon1_Text = Warrior_Icon1_Text.GetComponent<ObjectDestroy>();
            DestroyWarriorIcon1_Text.destroy();
        }
        if (Warrior_Icon2_Text)
        {
            ObjectDestroy DestroyWarriorIcon2_Text = Warrior_Icon2_Text.GetComponent<ObjectDestroy>();
            DestroyWarriorIcon2_Text.destroy();
        }

        if (archer.Skill1 != -1)
        {
            Archer_Icon1_Text = Instantiate(CoolTimeText, GameUICanvasPos);
            Archer_Icon1_Text.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 1002, GameUICanvasPos.position.y - 475);
        }
        if (archer.Skill2 != -1)
        {
            Archer_Icon2_Text = Instantiate(CoolTimeText, GameUICanvasPos);
            Archer_Icon2_Text.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 852, GameUICanvasPos.position.y - 475);
        }
        if (mage.Skill1 != -1)
        {
            Mage_Icon1_Text = Instantiate(CoolTimeText, GameUICanvasPos);
            Mage_Icon1_Text.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 402, GameUICanvasPos.position.y - 475);
        }
        if (mage.Skill2 != -1)
        {
            Mage_Icon2_Text = Instantiate(CoolTimeText, GameUICanvasPos);
            Mage_Icon2_Text.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x - 252, GameUICanvasPos.position.y - 475);
        }
        if (thief.Skill1 != -1)
        {
            Thief_Icon1_Text = Instantiate(CoolTimeText, GameUICanvasPos);
            Thief_Icon1_Text.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 248, GameUICanvasPos.position.y - 475);
        }
        if (thief.Skill2 != -1)
        {
            Thief_Icon2_Text = Instantiate(CoolTimeText, GameUICanvasPos);
            Thief_Icon2_Text.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 398, GameUICanvasPos.position.y - 475);
        }
        if (warrior.Skill1 != -1)
        {
            Warrior_Icon1_Text = Instantiate(CoolTimeText, GameUICanvasPos);
            Warrior_Icon1_Text.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 848, GameUICanvasPos.position.y - 475);
        }
        if (warrior.Skill2 != -1)
        {
            Warrior_Icon2_Text = Instantiate(CoolTimeText, GameUICanvasPos);
            Warrior_Icon2_Text.GetComponent<RectTransform>().anchoredPosition = new Vector3(GameUICanvasPos.position.x + 998, GameUICanvasPos.position.y - 475);
        }
    }

    void ShowCoolTime()
    {
        if(Archer_Icon1 && Archer_Icon1_Image && Archer_Icon1_Text)
        {
            Image CoolTimeimage = Archer_Icon1_Image.GetComponent<Image>();
            Text CoolTimetext = Archer_Icon1_Text.GetComponent<Text>();
            if(archer.maxSkill1Delay - archer.curSkill1Delay > 0)
            {
                CoolTimeimage.fillAmount = (archer.maxSkill1Delay - archer.curSkill1Delay) / archer.maxSkill1Delay;
                CoolTimetext.text = ((int)archer.maxSkill1Delay - (int)archer.curSkill1Delay).ToString();
            }
            else
            {
                CoolTimeimage.fillAmount = 0;
                CoolTimetext.text = "";
            }
        }
        if (Archer_Icon2 && Archer_Icon2_Image && Archer_Icon2_Text)
        {
            Image CoolTimeimage = Archer_Icon2_Image.GetComponent<Image>();
            Text CoolTimetext = Archer_Icon2_Text.GetComponent<Text>();
            if (archer.maxSkill2Delay - archer.curSkill2Delay > 0)
            {
                CoolTimeimage.fillAmount = (archer.maxSkill2Delay - archer.curSkill2Delay) / archer.maxSkill2Delay;
                CoolTimetext.text = ((int)archer.maxSkill2Delay - (int)archer.curSkill2Delay).ToString();
            }
            else
            {
                CoolTimeimage.fillAmount = 0;
                CoolTimetext.text = "";
            }
        }
        if (Mage_Icon1 && Mage_Icon1_Image && Mage_Icon1_Text)
        {
            Image CoolTimeimage = Mage_Icon1_Image.GetComponent<Image>();
            Text CoolTimetext = Mage_Icon1_Text.GetComponent<Text>();
            if (mage.maxSkill1Delay - mage.curSkill1Delay > 0)
            {
                CoolTimeimage.fillAmount = (mage.maxSkill1Delay - mage.curSkill1Delay) / mage.maxSkill1Delay;
                CoolTimetext.text = ((int)mage.maxSkill1Delay - (int)mage.curSkill1Delay).ToString();
            }
            else
            {
                CoolTimeimage.fillAmount = 0;
                CoolTimetext.text = "";
            }
        }
        if (Mage_Icon2 && Mage_Icon2_Image && Mage_Icon2_Text)
        {
            Image CoolTimeimage = Mage_Icon2_Image.GetComponent<Image>();
            Text CoolTimetext = Mage_Icon2_Text.GetComponent<Text>();
            if (mage.maxSkill2Delay - mage.curSkill2Delay > 0)
            {
                CoolTimeimage.fillAmount = (mage.maxSkill2Delay - mage.curSkill2Delay) / mage.maxSkill2Delay;
                CoolTimetext.text = ((int)mage.maxSkill2Delay - (int)mage.curSkill2Delay).ToString();
            }
            else
            {
                CoolTimeimage.fillAmount = 0;
                CoolTimetext.text = "";
            }
        }
        if (Thief_Icon1 && Thief_Icon1_Image && Thief_Icon1_Text)
        {
            Image CoolTimeimage = Thief_Icon1_Image.GetComponent<Image>();
            Text CoolTimetext = Thief_Icon1_Text.GetComponent<Text>();
            if (thief.maxSkill1Delay - thief.curSkill1Delay > 0)
            {
                CoolTimeimage.fillAmount = (thief.maxSkill1Delay - thief.curSkill1Delay) / thief.maxSkill1Delay;
                CoolTimetext.text = ((int)thief.maxSkill1Delay - (int)thief.curSkill1Delay).ToString();
            }
            else
            {
                CoolTimeimage.fillAmount = 0;
                CoolTimetext.text = "";
            }
        }
        if (Thief_Icon2 && Thief_Icon2_Image && Thief_Icon2_Text)
        {
            Image CoolTimeimage = Thief_Icon2_Image.GetComponent<Image>();
            Text CoolTimetext = Thief_Icon2_Text.GetComponent<Text>();
            if (thief.maxSkill2Delay - thief.curSkill2Delay > 0)
            {
                CoolTimeimage.fillAmount = (thief.maxSkill2Delay - thief.curSkill2Delay) / thief.maxSkill2Delay;
                CoolTimetext.text = ((int)thief.maxSkill2Delay - (int)thief.curSkill2Delay).ToString();
            }
            else
            {
                CoolTimeimage.fillAmount = 0;
                CoolTimetext.text = "";
            }
        }
        if (Warrior_Icon1 && Warrior_Icon1_Image && Warrior_Icon1_Text)
        {
            Image CoolTimeimage = Warrior_Icon1_Image.GetComponent<Image>();
            Text CoolTimetext = Warrior_Icon1_Text.GetComponent<Text>();
            if (warrior.maxSkill1Delay - warrior.curSkill1Delay > 0)
            {
                CoolTimeimage.fillAmount = (warrior.maxSkill1Delay - warrior.curSkill1Delay) / warrior.maxSkill1Delay;
                CoolTimetext.text = ((int)warrior.maxSkill1Delay - (int)warrior.curSkill1Delay).ToString();
            }
            else
            {
                CoolTimeimage.fillAmount = 0;
                CoolTimetext.text = "";
            }
        }
        if (Warrior_Icon2 && Warrior_Icon2_Image && Warrior_Icon2_Text)
        {
            Image CoolTimeimage = Warrior_Icon2_Image.GetComponent<Image>();
            Text CoolTimetext = Warrior_Icon2_Text.GetComponent<Text>();
            if (warrior.maxSkill2Delay - warrior.curSkill2Delay > 0)
            {
                CoolTimeimage.fillAmount = (warrior.maxSkill2Delay - warrior.curSkill2Delay) / warrior.maxSkill2Delay;
                CoolTimetext.text = ((int)warrior.maxSkill2Delay - (int)warrior.curSkill2Delay).ToString();
            }
            else
            {
                CoolTimeimage.fillAmount = 0;
                CoolTimetext.text = "";
            }
        }
    }

    public void ShowBagOnOff()
    {
        if(BagOnOff == true)
        {
            UI[6].SetActive(false);
            BagOnOff = false;
        }
        else
        {
            UI[6].SetActive(true);
            BagOnOff = true;
        }
    }

    public void SetY(int Y)
    {
        y = Y;
    }

    public void Monster_Die()
    {
        Monster_Amount--;
    }

    void ClearerDestroy()
    {
        if(clearer)
        {
            ObjectDestroy clearerdestroy = clearer.GetComponent<ObjectDestroy>();
            clearerdestroy.destroy();
        }
    }

    public void GameReset()
    {
        CancelInvoke();
        Chapter = 0;
        Life = 10;
        Show_Life();
        Gold = 0;
        GoldText.text = Gold.ToString();
        UI[0].SetActive(true);
        UI[1].SetActive(true);
        UI[2].SetActive(false);
        UI[3].SetActive(false);
        UI[4].SetActive(false);
        UI[5].SetActive(false);
        UI[6].SetActive(false);
        UI[7].SetActive(false);
        UI[8].SetActive(false);
        UI[9].SetActive(false);
        UI[10].SetActive(false);
        Monster_Amount = -1;
        clearer = Instantiate(Clearer);
        Invoke("ClearerDestroy", 0.5f);
        archer.GameReset();
        mage.GameReset();
        thief.GameReset();
        warrior.GameReset();
        if(reward_card)
        {
            ObjectDestroy rewardCardDestroy = reward_card.GetComponent<ObjectDestroy>();
            rewardCardDestroy.destroy();
        }
        if(reward_artifact)
        {
            ObjectDestroy rewardArtifactDestroy = reward_artifact.GetComponent<ObjectDestroy>();
            rewardArtifactDestroy.destroy();
        }
        if (Card1)
        {
            ObjectDestroy DestroyCard1 = Card1.GetComponent<ObjectDestroy>();
            DestroyCard1.destroy();
        }
        if (Card2)
        {
            ObjectDestroy DestroyCard2 = Card2.GetComponent<ObjectDestroy>();
            DestroyCard2.destroy();
        }
        if (Card3)
        {
            ObjectDestroy DestroyCard3 = Card3.GetComponent<ObjectDestroy>();
            DestroyCard3.destroy();
        }
        if(selectCard)
        {
            ObjectDestroy selectCardDestroy = selectCard.GetComponent<ObjectDestroy>();
            selectCardDestroy.destroy();
        }
        if (changeCard1)
        {
            ObjectDestroy changeCard1Destroy = changeCard1.GetComponent<ObjectDestroy>();
            changeCard1Destroy.destroy();
        }
        if (changeCard2)
        {
            ObjectDestroy changeCard2Destroy = changeCard2.GetComponent<ObjectDestroy>();
            changeCard2Destroy.destroy();
        }
        if (Artifact)
        {
            ObjectDestroy DestroyArtifact = Artifact.GetComponent<ObjectDestroy>();
            DestroyArtifact.destroy();
        }
        if(treasure)
        {
            ObjectDestroy treasureDestroy = treasure.GetComponent<ObjectDestroy>();
            treasureDestroy.destroy();
        }
        if (hprecover)
        {
            ObjectDestroy hprecoverDestroy = hprecover.GetComponent<ObjectDestroy>();
            hprecoverDestroy.destroy();
        }
        if (shopCard)
        {
            ObjectDestroy shopCardDestroy = shopCard.GetComponent<ObjectDestroy>();
            shopCardDestroy.destroy();
        }
        if (shopArtifact)
        {
            ObjectDestroy shopArtifactDestroy = shopArtifact.GetComponent<ObjectDestroy>();
            shopArtifactDestroy.destroy();
        }
        Show_Skill_Icons();
        ShowCoolTimeImage();
        ShowCoolTimeText();
        for (int i = Artifact_List.Count - 1; i >= 0; i--)
        {
            ObjectDestroy artifactDestroy = Artifact_List[i].GetComponent<ObjectDestroy>();
            artifactDestroy.destroy();
        }
        Artifact_List = new List<GameObject>();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
