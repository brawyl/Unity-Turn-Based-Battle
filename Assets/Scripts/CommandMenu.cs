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
        hero.GetComponent<FighterAction>().SelectAction("attack", elementName);
    }

    public void skill(string elementName)
    {
        hero.GetComponent<FighterAction>().SelectAction("skill", elementName);
    }
}
