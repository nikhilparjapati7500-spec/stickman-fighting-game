using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all playable characters.
/// Handles core movement, animation, and interaction with combat systems.
/// </summary>
public abstract class Character : MonoBehaviour
{
    [SerializeField] protected CharacterData characterData;
    [SerializeField] protected float groundDragMultiplier = 0.95f;
    [SerializeField] protected float airDragMultiplier = 0.85f;
    [SerializeField] protected float jumpForce = 5f;
    
    protected Rigidbody rb;
    protected Animator animator;
    protected CharacterStats stats;
    protected CombatSystem combatSystem;
    protected bool isGrounded = true;
    protected bool isBlocking = false;
    protected bool isAttacking = false;
    protected Vector3 movementInput;
    protected float facingDirection = 1f; // 1 = right, -1 = left
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        stats = new CharacterStats();
        
        // Configure Rigidbody
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.drag = 0;
            rb.angularDrag = 0;
        }
    }
    
    protected virtual void Start()
    {
        // Initialize stats from character data
        stats = new CharacterStats();
    }
    
    protected virtual void Update()
    {
        stats.Update(Time.deltaTime);
    }
    
    protected virtual void FixedUpdate()
    {
        ApplyDrag();
    }
    
    #region Movement
    
    public virtual void Move(Vector2 direction)
    {
        if (isAttacking || isGrounded == false)
            return;
        
        movementInput = new Vector3(direction.x, 0, direction.y);
        float moveSpeed = stats.Speed * characterData.Speed;
        
        // Update facing direction
        if (direction.x > 0)
            facingDirection = 1f;
        else if (direction.x < 0)
            facingDirection = -1f;
        
        // Move character
        Vector3 moveForce = movementInput * moveSpeed;
        moveForce.y = rb.velocity.y; // Preserve Y velocity
        rb.velocity = moveForce;
        
        // Update animation
        UpdateAnimationBlend(direction);
    }
    
    public virtual void Jump()
    {
        if (!isGrounded)
            return;
        
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.velocity += Vector3.up * jumpForce;
        isGrounded = false;
    }
    
    private void UpdateAnimationBlend(Vector2 direction)
    {
        if (animator != null)
        {
            animator.SetFloat("InputX", direction.x);
            animator.SetFloat("InputY", direction.y);
            animator.SetFloat("Speed", direction.magnitude);
        }
    }
    
    private void ApplyDrag()
    {
        float dragMultiplier = isGrounded ? groundDragMultiplier : airDragMultiplier;
        rb.velocity *= dragMultiplier;
    }
    
    #endregion
    
    #region Combat
    
    public virtual void Attack(AttackData attackData)
    {
        if (!CanAttack(attackData))
            return;
        
        isAttacking = true;
        
        // Drain stamina
        stats.DrainStamina(attackData.staminaCost);
        
        // Play animation
        PlayAttackAnimation(attackData);
        
        // Start attack coroutine
        StartCoroutine(ExecuteAttackCoroutine(attackData));
    }
    
    protected virtual bool CanAttack(AttackData attackData)
    {
        if (isAttacking || !stats.CanAttack(attackData.staminaCost))
            return false;
        
        return true;
    }
    
    protected virtual void PlayAttackAnimation(AttackData attackData)
    {
        if (animator != null)
        {
            animator.SetTrigger(attackData.name);
        }
    }
    
    protected virtual IEnumerator ExecuteAttackCoroutine(AttackData attackData)
    {
        yield return new WaitForSeconds(attackData.animationDuration * 0.5f);
        
        // Create hitbox and check for hits
        CreateHitBox(attackData);
        
        yield return new WaitForSeconds(attackData.animationDuration * 0.5f);
        
        isAttacking = false;
    }
    
    protected virtual void CreateHitBox(AttackData attackData)
    {
        // This will be overridden by combat system integration
        Debug.Log($"{characterData.CharacterName} attacking with {attackData.name}");
    }
    
    public virtual void Block()
    {
        if (isAttacking)
            return;
        
        isBlocking = true;
        if (animator != null)
        {
            animator.SetBool("IsBlocking", true);
        }
    }
    
    public virtual void ReleaseBlock()
    {
        isBlocking = false;
        if (animator != null)
        {
            animator.SetBool("IsBlocking", false);
        }
    }
    
    public virtual void TakeDamage(float damage)
    {
        stats.TakeDamage(damage);
        
        // Play hit animation
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
        
        if (stats.CurrentHP <= 0)
        {
            OnDefeated();
        }
    }
    
    public virtual void OnDefeated()
    {
        isAttacking = false;
        isBlocking = false;
        enabled = false;
        
        if (animator != null)
        {
            animator.SetTrigger("Knockdown");
        }
    }
    
    #endregion
    
    #region Special Moves
    
    public abstract void ExecuteSpecialAttack();
    public abstract void ExecuteUltimateAttack();
    public abstract void ExecuteHiddenMove();
    
    #endregion
    
    #region Utility
    
    public virtual void ResetToDefault()
    {
        enabled = true;
        isAttacking = false;
        isBlocking = false;
        stats.ResetToFull();
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        
        if (animator != null)
        {
            animator.SetBool("IsBlocking", false);
            animator.Rebind();
        }
    }
    
    public CharacterStats GetStats() => stats;
    public CharacterData GetCharacterData() => characterData;
    public bool IsGrounded => isGrounded;
    public bool IsBlocking => isBlocking;
    public bool IsAttacking => isAttacking;
    public float FacingDirection => facingDirection;
    
    #endregion
}
