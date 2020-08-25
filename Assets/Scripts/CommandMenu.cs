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
        Debug.Log(elementName + " attack");
        hero.GetComponent<FighterAction>().SelectAttack("melee");
    }

    public void skill(string elementName)
    {
        Debug.Log(elementName + " skill");
        hero.GetComponent<FighterAction>().SelectAttack("range");
    }

    public void item(string itemName)
    {
        Debug.Log(itemName + " item");
        hero.GetComponent<FighterAction>().SelectAttack("item");
    }

    public void defend(string elementName)
    {
        Debug.Log(elementName + " defend");
        hero.GetComponent<FighterAction>().SelectAttack("defend");
    }

    public void run()
    {
        Debug.Log("run away");
        hero.GetComponent<FighterAction>().SelectAttack("run");
    }
}
