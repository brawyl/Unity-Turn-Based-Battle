using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAction : MonoBehaviour
{
    public GameObject owner;

    [SerializeField]
    private string animationName;

    [SerializeField]
    private bool magicAttack;

    [SerializeField]
    private float magicCost;

    [SerializeField]
    private float minAttackMultiplier;
    [SerializeField]
    private float maxAttackMultiplier;

    [SerializeField]
    private float minDefenseMultiplier;
    [SerializeField]
    private float maxDefenseMultiplier;

    public string elementType;

    private FighterStats attackerStats;
    private FighterStats targetStats;
    private float damage = 0.0f;

    public void Attack(GameObject victim, string element="neutral")
    {
        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = victim.GetComponent<FighterStats>();

        targetStats.stopDelayCounter();

        bool selfDamage = false;

        float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
        float elementalBonus = 1.0f;

        if (magicAttack)
        {
            damage = multiplier * attackerStats.range;

            if (attackerStats.magic >= magicCost)
            {
                attackerStats.updateMagicFill(magicCost);
            }
            else
            {
                selfDamage = true;
            }
        }
        else
        {
            damage = multiplier * attackerStats.melee;
        }

        damage = Mathf.Max(1, damage - targetStats.defense);

        var animator = owner.GetComponent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float animationLength = 0;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                animationLength = clip.length;
                break;
            }
        }

        if (!element.Equals("neutral"))
        {
            GameObject elementalEffect = GameObject.Find("GameControllerObject").GetComponent<GameController>().elementEffect;
            var main = elementalEffect.GetComponent<ParticleSystem>().main;
            Color woodColor = new Color(76/255f, 175/255f, 80/255f, .8f);
            Color fireColor = new Color(255/255f, 82/255f, 82/255f, .8f);
            Color earthColor = new Color(121/255f, 85/255f, 72/255f, .8f);
            Color metalColor = new Color(215/255f, 204/255f, 200/255f, .8f);
            Color waterColor = new Color(33/255f, 150/255f, 243/255f, .8f);

            string enemyElement = targetStats.elementType;

            switch (element)
            {
                case "wood":
                    main.startColor = woodColor;
                    if (enemyElement.Equals("water"))
                    {
                        elementalBonus = 2.0f;
                    }
                    else if (enemyElement.Equals("fire"))
                    {
                        elementalBonus = 0.5f;
                    }
                    else if (enemyElement.Equals("earth"))
                    {
                        elementalBonus = 1.5f;
                    }
                    else if (enemyElement.Equals("metal"))
                    {
                        elementalBonus = 0.75f;
                    }
                    break;
                case "fire":
                    main.startColor = fireColor;
                    if (enemyElement.Equals("wood"))
                    {
                        elementalBonus = 2.0f;
                    }
                    else if (enemyElement.Equals("earth"))
                    {
                        elementalBonus = 0.5f;
                    }
                    else if (enemyElement.Equals("metal"))
                    {
                        elementalBonus = 1.5f;
                    }
                    else if (enemyElement.Equals("water"))
                    {
                        elementalBonus = 0.75f;
                    }
                    break;
                case "earth":
                    main.startColor = earthColor;
                    if (enemyElement.Equals("fire"))
                    {
                        elementalBonus = 2.0f;
                    }
                    else if (enemyElement.Equals("metal"))
                    {
                        elementalBonus = 0.5f;
                    }
                    else if (enemyElement.Equals("water"))
                    {
                        elementalBonus = 1.5f;
                    }
                    else if (enemyElement.Equals("wood"))
                    {
                        elementalBonus = 0.75f;
                    }
                    break;
                case "metal":
                    main.startColor = metalColor;
                    if (enemyElement.Equals("earth"))
                    {
                        elementalBonus = 2.0f;
                    }
                    else if (enemyElement.Equals("water"))
                    {
                        elementalBonus = 0.5f;
                    }
                    else if (enemyElement.Equals("wood"))
                    {
                        elementalBonus = 1.5f;
                    }
                    else if (enemyElement.Equals("fire"))
                    {
                        elementalBonus = 0.75f;
                    }
                    break;
                case "water":
                    main.startColor = waterColor;
                    if (enemyElement.Equals("metal"))
                    {
                        elementalBonus = 2.0f;
                    }
                    else if (enemyElement.Equals("wood"))
                    {
                        elementalBonus = 0.5f;
                    }
                    else if (enemyElement.Equals("fire"))
                    {
                        elementalBonus = 1.5f;
                    }
                    else if (enemyElement.Equals("earth"))
                    {
                        elementalBonus = 0.75f;
                    }
                    break;
            }
            elementalEffect.SetActive(true);
        }

        animator.Play(animationName);

        if (selfDamage)
        {
            //take damage when using a magic skill with no MP left
            attackerStats.ReceiveDamage(Mathf.CeilToInt(magicCost * 10), animationLength, true, 1.0f);
        }

        targetStats.ReceiveDamage(Mathf.CeilToInt(damage), animationLength, false, elementalBonus);
    }

    void SkipTurnContinueGame()
    {
        GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    }
}
