using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public static PlayerAnimator Instance { get; private set; }

    private const string IS_WALKING = "IsWalking";
    private const string ATTACK1 = "Attack1";
    private const string ATTACK2 = "Attack2";
    private const string ATTACK3 = "Attack3";
    private const string IS_DEAD = "IsDead";

    private List<string> animaList = new List<string>(new string[] {ATTACK1,ATTACK2,ATTACK3});
    private const string RESET = "Reset";

    public static int comboNum;
    private float lastClickTime = 0f;
    private float comboResetTime = 1f;
    private float fullComboResetTime = 3f;

    [SerializeField] private List<Collider> punchColliderList = new List<Collider>();
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, Player.Instance.IsWalking());
        animator.SetBool(IS_DEAD, PlayerHealth.Instance.IsDead());

        OnClickAttack();
    }

    private void ResetCombo()
    {
        animator.SetTrigger(RESET);
        comboNum = 0;
    }

    private void OnClickAttack()
    {
        if (!BoxingGameManager.Instance.IsGamePlaying()) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (comboNum < 3)
            {
                animator.SetTrigger(animaList[comboNum]);
                comboNum++;
                lastClickTime = Time.time;
            }
        }

        if (comboNum > 0)
        {
            if (comboNum == 3)
            {
                if (Time.time - lastClickTime > fullComboResetTime)
                {
                    ResetCombo();
                }
            }
            else if (Time.time - lastClickTime > comboResetTime)
            {
                ResetCombo();
            }
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
