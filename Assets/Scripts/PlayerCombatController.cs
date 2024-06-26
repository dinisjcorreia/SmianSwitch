using System.Collections;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private bool combatEnabled;
    [SerializeField] private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField] private Transform attack1HitBoxPos;
    [SerializeField] private LayerMask whatIsDamageable;

    private bool gotInput, isAttacking, isFirstAttack;

    private float lastInputTime = Mathf.NegativeInfinity;

    private Animator anim;

    private PlayerController PC;
    private PlayerStats PS;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    public Animator animator;
    public RuntimeAnimatorController macaco;
    public RuntimeAnimatorController pessoa;
    public RuntimeAnimatorController ninja;
    public GameObject dialogo;

    private int i =0;
    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (combatEnabled)
            {
                if (dialogo.activeSelf==false){
                    gotInput = true;
                lastInputTime = Time.time;
                }
                
               
            }
        }
    }



    private void CheckAttacks()
    {
        if (gotInput && !isAttacking)
        {
            Debug.Log("entrei");
            gotInput = false;
            isAttacking = true;
            isFirstAttack = !isFirstAttack;
            anim.SetBool("attack1", true);
            anim.SetBool("firstAttack", isFirstAttack);
            anim.SetBool("isAttacking", isAttacking);
            Debug.Log("Attack triggered: " + isFirstAttack);
            audioManager.PlaySound(audioManager.swordSound);
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            // Wait for new input
            gotInput = false;
             
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        float[] attackDetails = { attack1Damage, transform.position.x };

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
            // Instantiate hit particle
        }
    }

    private void FinishAttack1()
    {
        Debug.Log("finis");
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);

       
    }

    private void FinishAttack2()
    {
        Debug.Log("finis");
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);

        
    }

    private void Damage(float[] attackDetails)
    {
        if (!PC.GetDashStatus())
        {
            int direction = attackDetails[1] < transform.position.x ? 1 : -1;

            PS.DecreaseHealth(attackDetails[0]);
            PC.Knockback(direction);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}
