using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FighterStats : MonoBehaviour, IComparable
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject healthFill;

    [SerializeField]
    private GameObject magicFill;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text magicText;

    [Header("Stats")]
    public float health;
    public float magic;
    public float melee;
    public float range;
    public float defense;
    public float speed;
    public float experience;
    public string elementType;

    private float startHealth;
    private float startMagic;

    [HideInInspector]
    public int nextActTurn;

    private bool dead = false;

    private Vector2 healthScale;
    private Vector2 magicScale;

    private float xNewHealthScale;
    private float xNewMagicScale;

    private GameObject gameControllerObject;

    void Awake()
    {
        healthScale = healthFill.transform.localScale;

        magicScale = magicFill.transform.localScale;

        startHealth = health;
        startMagic = magic;

        if (healthText != null)
        {
            healthText.text = health + " / " + startHealth + " HP";
        }

        if (magicText != null)
        {
            magicText.text = magic + " / " + startMagic + " MP";
        }

        gameControllerObject = GameObject.Find("GameControllerObject");
    }

    public void stopDelayCounter()
    {
        gameControllerObject.GetComponent<GameController>().heroTurn = false;
    }

    public void ReceiveDamage(float damage, float length, bool selfDamage=false, float elementalBonus=1.0f)
    {
        StartCoroutine(damageUI(damage, length, selfDamage, elementalBonus));
    }

    IEnumerator damageUI(float damage, float length, bool selfDamage, float elementalBonus)
    {
        yield return new WaitForSeconds(length);

        gameControllerObject.GetComponent<GameController>().elementEffect.SetActive(false);

        if (gameObject.tag.Equals("Enemy") && elementalBonus != 1.0f)
        {
            string battleMessage = "";
            if (elementalBonus < 1.0f)
            {
                battleMessage += "Weak Matchup!\n";
            }
            else
            {
                battleMessage += "Strong Matchup!\n";
            }

            battleMessage += elementalBonus + "x damage";
            gameControllerObject.GetComponent<GameController>().ShowMainMessage(battleMessage, 2);
            damage = elementalBonus * damage;
        }

        health -= damage;
        animator.Play("damage");

        if (health <= 0)
        {
            gameControllerObject.GetComponent<GameController>().EndGame(gameObject.tag);

            dead = true;
            gameObject.tag = "Dead";
            Destroy(healthFill);
            Destroy(gameObject);
        }
        else
        {
            xNewHealthScale = healthScale.x * (health / startHealth);
            healthFill.transform.localScale = new Vector2(xNewHealthScale, healthScale.y);
            if (healthText != null)
            {
                healthText.text = health + " / " + startHealth + " HP";
            }

            if (gameObject.tag.Equals("Hero"))
            {
                gameControllerObject.GetComponent<GameController>().heroMessage.gameObject.SetActive(true);
                gameControllerObject.GetComponent<GameController>().heroMessage.text = damage.ToString();
            }
            else if (gameObject.tag.Equals("Enemy"))
            {
                gameControllerObject.GetComponent<GameController>().enemyMessage.gameObject.SetActive(true);
                gameControllerObject.GetComponent<GameController>().enemyMessage.text = damage.ToString();
            }
        }
        if (!selfDamage)
        {
            Invoke("ContinueGame", 1);
        }
    }

    public void updateMagicFill(float cost)
    {
        if (cost > 0)
        {
            magic -= cost;
            xNewMagicScale = magicScale.x * (magic / startMagic);
            magicFill.transform.localScale = new Vector2(xNewMagicScale, magicScale.y);

            if (magicText != null)
            {
                magicText.text = magic + " / " + startMagic + " MP";
            }
        }
    }

    public bool GetDead()
    {
        return dead;
    }

    void ContinueGame()
    {
        GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    }

    public void CalculateNextTurn(int currentTurn, float delayValue=0)
    {
        int delay = Mathf.CeilToInt(delayValue);
        int speedFactor = Mathf.CeilToInt(100f / speed);
        if (tag.Equals("Enemy"))
        {
            //randomize enemy delay based on speed stat
            delay = UnityEngine.Random.Range(0, speedFactor);
        }

        nextActTurn = currentTurn + speedFactor + delay;

        Debug.Log(tag + " Delay value: " + delay);
    }

    public int CompareTo(object otherStats)
    {
        int nex = nextActTurn.CompareTo(((FighterStats)otherStats).nextActTurn);
        return nex;
    }

    public void playMeleeSound()
    {
        if (gameObject.tag.Equals("Hero"))
        {
            GameObject.Find("MeleeSound1").GetComponent<AudioSource>().Play();
        }
        else
        {
            GameObject.Find("MeleeSound2").GetComponent<AudioSource>().Play();
        }
    }

    public void playSkillSound()
    {
        if (gameObject.tag.Equals("Hero"))
        {
            GameObject.Find("SkillSound1").GetComponent<AudioSource>().Play();
        }
        else
        {
            GameObject.Find("SkillSound2").GetComponent<AudioSource>().Play();
        }
    }
}
