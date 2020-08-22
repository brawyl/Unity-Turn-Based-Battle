﻿using System.Collections;
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

    [Header("Stats")]
    public float health;
    public float magic;
    public float melee;
    public float magicRange;
    public float defense;
    public float speed;
    public float experience;

    private float startHealth;
    private float startMagic;

    [HideInInspector]
    public int nextActTurn;

    private bool dead = false;

    //Resize health and magic bar
    private Transform healthTransform;
    private Transform magicTransform;

    private Vector2 healthScale;
    private Vector2 magicScale;

    private float xNewHealthScale;
    private float xNewMagicScale;

    private GameObject gameControllerObject;

    void Awake()
    {
        healthTransform = healthFill.GetComponent<RectTransform>();
        healthScale = healthFill.transform.localScale;

        magicTransform = magicFill.GetComponent<RectTransform>();
        magicScale = magicFill.transform.localScale;

        startHealth = health;
        startMagic = magic;

        gameControllerObject = GameObject.Find("GameControllerObject");
    }

    public void ReceiveDamage(float damage)
    {
        StartCoroutine(takeDamage(damage));
    }

    IEnumerator takeDamage(float damage)
    {
        yield return new WaitForSeconds(0.5f);

        health -= damage;
        animator.Play("damage");

        if (health <= 0)
        {
            dead = true;
            gameObject.tag = "Dead";
            Destroy(healthFill);
            Destroy(gameObject);
        }
        else if (damage > 0)
        {
            xNewHealthScale = healthScale.x * (health / startHealth);
            healthFill.transform.localScale = new Vector2(xNewHealthScale, healthScale.y);
        }

        if (gameObject.tag == "Hero")
        {
            gameControllerObject.GetComponent<GameController>().heroMessage.gameObject.SetActive(true);
            gameControllerObject.GetComponent<GameController>().heroMessage.text = damage.ToString();
        }
        else if (gameObject.tag == "Enemy")
        {
            gameControllerObject.GetComponent<GameController>().enemyMessage.gameObject.SetActive(true);
            gameControllerObject.GetComponent<GameController>().enemyMessage.text = damage.ToString();
        }
        Invoke("ContinueGame", 1);
    }

    public void updateMagicFill(float cost)
    {
        if (cost > 0)
        {
            magic -= cost;
            xNewMagicScale = magicScale.x * (magic / startMagic);
            magicFill.transform.localScale = new Vector2(xNewMagicScale, magicScale.y);
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

    public void CalculateNextTurn(int currentTurn)
    {
        nextActTurn = currentTurn + Mathf.CeilToInt(100f / speed);
    }

    public int CompareTo(object otherStats)
    {
        int nex = nextActTurn.CompareTo(((FighterStats)otherStats).nextActTurn);
        return nex;
    }
}