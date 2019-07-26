using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class PlayerController : MonoBehaviour{
	[Header("Physics")]
	[SerializeField]
	[Range(0.0f, 10.0f)]
	private float playerVelocity = 10;

	[Header("Jump")]
	[SerializeField]
	private Transform jumpRaycastPosition;

	[SerializeField]
	private float jumpRaycastRadius;

	[SerializeField]
	private LayerMask jumpLayerMask;

	[SerializeField]
	private float jumpForce = 5;

	[Header("Sounds")]
	/*[SerializeField]
	private MultiSoundsRandom soundsJump;
	[SerializeField]
	private MultiSoundsRandom soundsFire;
	[SerializeField]
	private MultiSoundsRandom soundsObjectCatch;*/

	[Header("Animation")]
	[SerializeField]
	private Animator playerAnimationController;

	#region PlayerVariables
	private Rigidbody2D rigidBody2D;
	#endregion PlayerVariables 

	private GameManager gameManager;
    private bool CanDoubleJump;


	void Start(){
        CanDoubleJump = false;
		rigidBody2D = GetComponent<Rigidbody2D>();
        playerAnimationController = GetComponentInChildren<Animator>();
        GameObject gameManagerObject = GameObject.Find ("GameManager");
		if (gameManagerObject != null)
			gameManager = gameManagerObject.GetComponent<GameManager> ();
	}

	void Update(){
		if (gameManager != null && !gameManager.CanPlay ()) {
			return;
		}

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		if (playerAnimationController != null)
			playerAnimationController.SetFloat("speedX", Mathf.Abs(horizontalInput));

		Vector2 velocity = new Vector2(horizontalInput * playerVelocity, 0);
		velocity.y = rigidBody2D.velocity.y;
        Debug.Log(velocity);
		rigidBody2D.velocity = velocity;
        RaycastHit2D hitInfo;

       
        //hitInfo = Physics2D.BoxCast();
        
		Vector3 scale = transform.localScale;
		if (velocity.x > 0) {
			scale.x = Mathf.Abs(scale.x);
		} else if (velocity.x < 0) {
			scale.x = -Mathf.Abs(scale.x);
		}
		transform.localScale = scale;
        

		bool isGrounded = Physics2D.OverlapCircle(jumpRaycastPosition.position, jumpRaycastRadius, jumpLayerMask);

		if (velocity.x == 0 || !isGrounded) {
			if (gameManager != null)
				gameManager.StopSFX ();
		} else {
			if (gameManager != null)
				gameManager.PlaySFX (SoundManager.RUN);
		}

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (gameManager != null)
                gameManager.PlaySFX(SoundManager.JUMP);
            if (playerAnimationController != null)
                playerAnimationController.SetTrigger("jump");
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            CanDoubleJump = true;

        }else if (Input.GetButtonDown("Jump") && !isGrounded && CanDoubleJump) {
            if (gameManager != null)
                gameManager.PlaySFX(SoundManager.JUMP);
            if (playerAnimationController != null)
                playerAnimationController.SetTrigger("jump");
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            CanDoubleJump = false;
        
        }
        playerAnimationController.SetBool("IsGrounded", isGrounded);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(jumpRaycastPosition.position, jumpRaycastRadius);
    }
}
		
	

