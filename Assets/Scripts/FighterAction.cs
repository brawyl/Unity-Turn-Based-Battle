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

    public void SelectAction(string action)
    {
        GameObject victim = tag.Equals("Hero") ? enemy : hero;

        if (action.Equals("attack"))
        {
            meleePrefab.GetComponent<AttackAction>().Attack(victim);
        }
        else if (action.Equals("skill"))
        {
            rangePrefab.GetComponent<AttackAction>().Attack(victim);
        }
        else
        {
            Debug.Log(action + " function not available");
            GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
        }
    }
}
