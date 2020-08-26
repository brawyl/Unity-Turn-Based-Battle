using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    private List<FighterStats> fighterStats;

    [SerializeField]
    private GameObject battleMenu;

    public Text heroMessage;
    public Text enemyMessage;
    public Text heroDelay;

    public Text mainMessage;
    public Text turnOrder;

    public GameObject endScreenMenu;
    public TMP_Text resultMessage;

    private float delayCounter;
    public bool heroTurn;

    private bool gameOver = false;

    public GameObject elementEffect;

    private string[] elements = { "wood", "fire", "earth", "metal", "water" };

    // Start is called before the first frame update
    void Start()
    {
        fighterStats = new List<FighterStats>();
        GameObject hero = GameObject.FindGameObjectWithTag("Hero");
        FighterStats currentHeroStats = hero.GetComponent<FighterStats>();
        currentHeroStats.CalculateNextTurn(0);
        fighterStats.Add(currentHeroStats);

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        FighterStats currentEnemyStats = enemy.GetComponent<FighterStats>();

        //randomize enemy element
        int enemyElement = Random.Range(0, elements.Length);
        currentEnemyStats.elementType = elements[enemyElement];

        currentEnemyStats.CalculateNextTurn(0);
        fighterStats.Add(currentEnemyStats);

        fighterStats.Sort();
        this.battleMenu.SetActive(false);
        gameOver = false;

        delayCounter = 0.0f;

        endScreenMenu.gameObject.SetActive(false);

        ShowMainMessage("FIGHT!", 1);

        NextTurn();
    }

    void Update()
    {
        UpdateTimerUI();
    }
    //call this on update
    public void UpdateTimerUI()
    {
        //set timer UI
        delayCounter += Time.deltaTime;

        if (heroTurn)
        {
            heroDelay.text = delayCounter.ToString("0.00");
        }
    }

    public void ShowMainMessage(string message, int timeShown)
    {
        mainMessage.gameObject.SetActive(false);
        mainMessage.text = message;
        mainMessage.gameObject.SetActive(true);
        Invoke("HideMainMessage", timeShown);
    }

    private void HideMainMessage()
    {
        mainMessage.gameObject.SetActive(false);
    }

    public void NextTurn()
    {
        heroMessage.gameObject.SetActive(false);
        enemyMessage.gameObject.SetActive(false);
        delayCounter = 0.0f;
        heroTurn = false;

        FighterStats currentFighterStats = fighterStats[0];
        fighterStats.Remove(currentFighterStats);

        if (!currentFighterStats.GetDead())
        {
            GameObject currentUnit = currentFighterStats.gameObject;
            float delayValue = float.Parse(heroDelay.text);
            currentFighterStats.CalculateNextTurn(currentFighterStats.nextActTurn, delayValue);
            fighterStats.Add(currentFighterStats);
            fighterStats.Sort();
            turnOrder.text = "Next Turn:\n";
            foreach(FighterStats stat in fighterStats)
            {
                turnOrder.text += stat.tag + "\n";
            }
            if (currentUnit.tag.Equals("Hero"))
            {
                this.battleMenu.SetActive(true);
                heroTurn = true;
            }
            else
            {
                this.battleMenu.SetActive(false);
                string attackType = Random.Range(0, 2) == 1 ? "attack" : "skill";
                currentUnit.GetComponent<FighterAction>().SelectAction(attackType, "neutral");
            }
        }
        else
        {
            NextTurn();
        }
    }

    public void EndGame(string tag)
    {
        HideMainMessage();
        endScreenMenu.gameObject.SetActive(true);
        gameOver = true;

        if (tag.Equals("Hero"))
        {
            resultMessage.text = "YOU LOSE";
        }
        else
        {
            resultMessage.text = "YOU WIN";
        }
    }
}
