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

    public GameObject endScreenMenu;
    public TMP_Text resultMessage;

    private float delayCounter;
    public bool heroTurn;

    private bool gameOver = false;

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
        currentEnemyStats.CalculateNextTurn(0);
        fighterStats.Add(currentEnemyStats);

        fighterStats.Sort();
        this.battleMenu.SetActive(false);
        gameOver = false;

        delayCounter = 0.0f;

        endScreenMenu.gameObject.SetActive(false);

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
            currentFighterStats.CalculateNextTurn(currentFighterStats.nextActTurn);
            fighterStats.Add(currentFighterStats);
            fighterStats.Sort();
            if (currentUnit.tag == "Hero")
            {
                this.battleMenu.SetActive(true);
                heroTurn = true;
            }
            else
            {
                this.battleMenu.SetActive(false);
                string attackType = Random.Range(0, 2) == 1 ? "melee" : "range";
                currentUnit.GetComponent<FighterAction>().SelectAttack(attackType);
            }
        }
        else
        {
            NextTurn();
        }
    }

    public void EndGame(string tag)
    {
        endScreenMenu.gameObject.SetActive(true);
        gameOver = true;

        if (tag == "Hero")
        {
            resultMessage.text = "YOU LOSE";
        }
        else
        {
            resultMessage.text = "YOU WIN";
        }
    }
}
