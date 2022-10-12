using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerPM : KinematicObjectPM
{
    public AudioClip jumpAudio;
    public AudioClip respawnAudio;
    public AudioClip ouchAudio;
    public AudioClip respawnPointReachedAudio;
    public AudioClip healingAudio;
    public AudioClip interactAudio;
    public AudioClip keyTouchAudio;
    public AudioClip whirlwindJumpAudio;

    public GameObject currentRespawnPoint;

    /// <summary>
    /// Max horizontal speed of the player.
    /// </summary>
    public float maxSpeed = 7;
    public PlayerState playerState = PlayerState.Idle;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
    public float jumpTakeOffSpeed = 7;

    public JumpState jumpState = JumpState.Grounded;
    private bool stopJump;
    /*internal new*/        
    public Collider2D collider2d;
    public AudioSource audioSource;
    public HealthPM health;
    public bool controlEnabled = true;

    public bool GoalReached { get; set; } = false;

    bool jump;
    Vector2 move;
    SpriteRenderer spriteRenderer;
    internal Animator animator;
    readonly PlatformerModelPM model = new PlatformerModelPM();

    public Bounds Bounds => collider2d.bounds;

    //Variabila pentru pastrarea ultimului contact cu un obiect
    private GameObject lastObjectCollided;

    //Constante pentru realizarea unui flip decent
    private bool isFacingRight = true;
    [SerializeField]private float flipConstant = 5.0f;
    

    [SerializeField] private bool isDragingSomething = false;
    [SerializeField] private DragableObjectPM dragableObject;
    [SerializeField] private Text hintForInteraction;

    void Awake()
    {
        health = GetComponent<HealthPM>();
        audioSource = GetComponent<AudioSource>();
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
       
    }
    

    protected override void Update()
    {
        if (controlEnabled)
        {
            move.x = Input.GetAxis("Horizontal");

            if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump") && !isDragingSomething)
            {
                audioSource.PlayOneShot(jumpAudio);
                jumpState = JumpState.PrepareToJump;
            }
            else if (Input.GetButtonUp("Jump") )
            {
                stopJump = true;
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                Interact();
            }
        }
        else
        {
            move.x = 0;
        }
        UpdateJumpState();
        UpdateAnimator();

        //Cu cat player-ul are viata mai mica cu atata devine mai transparent
        Color playerColor = spriteRenderer.color;
        playerColor.a = Mathf.Clamp((float)health.currentHP / health.maxHP, 0.5f, 1);

        spriteRenderer.color = playerColor;

        base.Update();
    }

    void UpdateAnimator()
    {
        if (playerState == PlayerState.Active)
        {

            if (jumpState == JumpState.Grounded)
            {
                animator.SetBool("Grounded", true);
            }
            else
            {
                animator.SetBool("Grounded", false);

            }
            
        }
        if(jumpState == JumpState.Jumping || jumpState == JumpState.PrepareToJump)
        {
            animator.SetTrigger("Jump");
        }
        


    }

    void Interact()
    {
        //Debug.Log($"Player-ul se {(isDragingSomething?"detaseaza":"ataseaza")} de ceva.");
        if (dragableObject != null)
        {
            if (isDragingSomething)
            {
                
                isDragingSomething = false;
                dragableObject.UnHold();

            }
            else if (!isDragingSomething)
            {
                audioSource.PlayOneShot(interactAudio);
                isDragingSomething = true;
                dragableObject.Hold(this.gameObject);
            }
            audioSource.PlayOneShot(interactAudio);
        }

        if (lastObjectCollided && lastObjectCollided.tag.Equals("Dice"))
        {
            DiceControllerPM dice = lastObjectCollided.GetComponent<DiceControllerPM>();
            dice.Roll(false);
            audioSource.PlayOneShot(interactAudio);
        }

        
    }
    void UpdateJumpState()
    {
        jump = false;
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                jumpState = JumpState.Jumping;
                jump = true;
                stopJump = false;
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    jumpState = JumpState.InFlight;
                }
                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
                    jumpState = JumpState.Landed;
                }
                break;
            case JumpState.Landed:
                jumpState = JumpState.Grounded;
                break;
        }
    }

    protected override void ComputeVelocity()
    {
        if (jump && IsGrounded)
        {
            velocity.y = jumpTakeOffSpeed * model.jumpModifier;
            jump = false;
        }
        else if (stopJump)
        {
            stopJump = false;
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * model.jumpDeceleration;
            }
        }

        //Facem flip la caracter odata ce schimbam sensul de deplasare
        /// - player-ul nu trabuie sa traga nimic cu el!
        if (!isDragingSomething)
        {
            if (move.x > 0.01f && !isFacingRight)
            {
                Flip();
            }
            else if (move.x < -0.01f && isFacingRight)
            {
                Flip();
            }
        }

        if (velocity.magnitude > 0.5f)
        {
            playerState = PlayerState.Active;
            animator.SetBool("Idle", false);
        }
        else
        {
            playerState = PlayerState.Idle;
            animator.SetBool("Idle", true);
        }

        if (velocity.y < 0)
        {
            animator.SetBool("Falling", true);
        }
        else
        {
            animator.SetBool("Falling", false);
        }


        animator.SetBool("Grounded", IsGrounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x));
        animator.SetFloat("velocityY", velocity.y);

        targetVelocity = move * maxSpeed;

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScalePlayer = transform.localScale;
        Vector3 positionPlayer = transform.position;

        localScalePlayer.x *= -1;
        transform.localScale = localScalePlayer;

        //repozitionam player-ul pentru un flip corect
        positionPlayer.x += -localScalePlayer.x * flipConstant;
        
        transform.position = positionPlayer;

        


    }

    public void Respawn()
    {
        audioSource.PlayOneShot(respawnAudio);
        Teleport(currentRespawnPoint.transform.position);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)//Realizam interactiunea cu diverse capcane/obiecte/etc in functie de tag
    {

        if (collision.tag.Equals("Trap"))
        {
            audioSource.PlayOneShot(ouchAudio);
            velocity = Vector2.zero;
            Bounce(10f);
            health.Decrement(10);
        }else
        if (collision.tag.Equals("HealingItem") && health.currentHP!=health.maxHP)
        {
            audioSource.PlayOneShot(healingAudio);
            health.Increment(20);
            Destroy(collision.gameObject);
        }else
        if (collision.tag.Equals("RespawnPoint"))
        {
            if(currentRespawnPoint.transform.position.x <= collision.gameObject.transform.position.x)
            {
                audioSource.PlayOneShot(respawnPointReachedAudio);
                currentRespawnPoint = collision.gameObject;
            }
            
        }else
        if (collision.tag.Equals("Key"))
        {
            audioSource.PlayOneShot(keyTouchAudio);
            collision.gameObject.SetActive(false);
        }else
        if (collision.gameObject.name.StartsWith("WhirlWind"))
        {
            Bounce(25f);
            audioSource.PlayOneShot(whirlwindJumpAudio);
        }
        else
        if (collision.tag.Equals("FinishPoint"))
        {
            GoalReached = true;         
        }



        if (!isDragingSomething)
        {
            dragableObject = collision.gameObject.GetComponent<DragableObjectPM>();
            if (dragableObject && dragableObject.isDragable == false)
            {
                dragableObject = null;
            }
            if (isParentInteractive(collision))
            {
                setInteractHint(true);
            }
        }

        lastObjectCollided = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isDragingSomething)
        {
            dragableObject = null;
        }
        lastObjectCollided = null;


        if (isParentInteractive(collision))
        {
            setInteractHint(false);
        }
    }

    private bool isParentInteractive(Collider2D collision)
    {
        DiceControllerPM diceController = collision.GetComponent<DiceControllerPM>();
        if (diceController && diceController.enabled)
        {   
            return true;     
        }

        DragableObjectPM dragableObject = collision.gameObject.GetComponent<DragableObjectPM>();

        if(dragableObject && dragableObject.isDragable)
        {
            return true;
        }

        return false;
    }

    private void setInteractHint(bool active)
    {
        if (hintForInteraction != null)
        {
            if (active)
            {
                hintForInteraction.text = "Press F          to interact";
            }
            else
            {
                hintForInteraction.text = "";
            }
            
        }
    }

    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }

    public enum PlayerState
    {
        Idle,
        Active
    }
    
}
