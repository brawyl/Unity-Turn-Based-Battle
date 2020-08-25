using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenu : MonoBehaviour
{

    private GameObject hero;

    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
    }

    public void attack(string elementName)
    {
        hero.GetComponent<FighterAction>().SelectAction("attack");
    }

    public void skill(string elementName)
    {
        hero.GetComponent<FighterAction>().SelectAction("skill");
    }

    public void item(string itemName)
    {
        hero.GetComponent<FighterAction>().SelectAction("item");
    }

    public void defend(string elementName)
    {
        hero.GetComponent<FighterAction>().SelectAction("defend");
    }

    public void run()
    {
        hero.GetComponent<FighterAction>().SelectAction("run");
    }
}
