using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : MonoBehaviour
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

    private FighterStats attackerStats;
    private FighterStats targetStats;
    private float damage = 0.0f;

    public void Attack(GameObject victim)
    {
        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = victim.GetComponent<FighterStats>();

        targetStats.stopDelayCounter();

        if (magicAttack && attackerStats.magic < magicCost)
        {
            SkipTurnContinueGame();
        }
        else
        {
            float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);

            if (attackerStats.magic >= magicCost && magicAttack)
            {
                if (magicCost > 0)
                {
                    attackerStats.updateMagicFill(magicCost);
                }
                damage = multiplier * attackerStats.magicRange;
            }
            else
            {
                damage = multiplier * attackerStats.melee;
            }

            float defenseMultiplier = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);
            damage = Mathf.Max(0, damage - (defenseMultiplier * targetStats.defense));

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

            animator.Play(animationName);

            targetStats.ReceiveDamage(Mathf.CeilToInt(damage), animationLength);
        }
    }

    void SkipTurnContinueGame()
    {
        GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    }
}
