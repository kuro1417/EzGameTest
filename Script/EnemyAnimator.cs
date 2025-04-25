using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public static EnemyAnimator Instance { get; private set; }

    private const string IS_WALKING = "IsWalking";
    private const string ON_ATTACK = "OnAttack";

    [SerializeField] private List<Collider> punchColliderList = new List<Collider>();
    [SerializeField] private EnemyControl enemyControl;
    private Animator animator;
    private void Awake()
    {
        Instance = this;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, enemyControl.IsWalking());

        if (enemyControl.IsAttacking())
        {
            animator.SetTrigger(ON_ATTACK);
        }
    }

    public void EnablePunchCollider()
    {
            foreach (Collider collider in punchColliderList)
            {
                collider.enabled = true;
            }
    }

    public void DisablePunchCollider()
    {
        foreach (Collider collider in punchColliderList)
        {
            collider.enabled = false;
        }
    }
}
