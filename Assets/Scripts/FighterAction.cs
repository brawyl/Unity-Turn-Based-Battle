using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterAction : MonoBehaviour
{
    private GameObject enemy;
    private GameObject hero;

    [SerializeField]
    private GameObject meleePrefab;

    [SerializeField]
    private GameObject rangePrefab;

    [SerializeField]
    private Sprite faceIcon;

    void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    public void SelectAttack(string btn)
    {
        GameObject victim = tag == "Hero" ? enemy : hero;

        if (btn.CompareTo("melee") == 0)
        {
            meleePrefab.GetComponent<AttackAction>().Attack(victim);
        }
        else if (btn.CompareTo("range") == 0)
        {
            rangePrefab.GetComponent<AttackAction>().Attack(victim);
        }
        else
        {
            Debug.Log(btn + " function not available");
            GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
        }
    }
}
