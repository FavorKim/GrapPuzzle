using UnityEngine;
using System;

public enum CharacterState {
	Idle = 0,
	Walking = 1,
	Trotting = 2,
	Running = 3,
	Jumping = 4,
}

[RequireComponent(typeof(CharacterController))]

public class ThirdPersonController:MonoBehaviour
{
    
    // Require a character controller to be attached to the same game object
    
    public AnimationClip idleAnimation;
    public AnimationClip walkAnimation;
    public AnimationClip runAnimation;
    public AnimationClip jumpPoseAnimation;
    
    public float walkMaxAnimationSpeed = 0.75f;
    public float trotMaxAnimationSpeed = 1.0f;
    public float runMaxAnimationSpeed = 1.0f;
    public float jumpAnimationSpeed = 1.15f;
    public float landAnimationSpeed = 1.0f;
    
    Animation _animation;

    CharacterState _characterState;
    
    // The speed when walking
    public float walkSpeed = 2.0f;
    // after trotAfterSeconds of walking we trot with trotSpeed
    public float trotSpeed = 4.0f;
    // when pressing "Fire3" button (cmd) we start running
    public float runSpeed = 6.0f;
    
    public float inAirControlAcceleration = 3.0f;
    
    // How high do we jump when pressing jump and letting go immediately
    public float jumpHeight = 0.5f;
    
    // The gravity for the character
    public float gravity = 20.0f;
    // The gravity in controlled descent mode
    public float speedSmoothing = 10.0f;
    public float rotateSpeed = 500.0f;
    public float trotAfterSeconds = 3.0f;
    
    public bool canJump = true;
    
    float jumpRepeatTime = 0.05f;
    float jumpTimeout = 0.15f;
    float groundedTimeout = 0.25f;
    
    // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
    float lockCameraTimer = 0.0f;
    
    // The current move direction in x-z
    Vector3 moveDirection = Vector3.zero;
    // The current vertical speed
    float verticalSpeed = 0.0f;
    // The current x-z move speed
    float moveSpeed = 0.0f;
    
    // The last collision flags returned from controller.Move
    CollisionFlags collisionFlags; 
    
    // Are we jumping? (Initiated with jump button and not grounded yet)
    bool jumping = false;
    bool jumpingReachedApex = false;
    
    // Are we moving backwards (This locks the camera to not do a 180 degree spin)
    bool movingBack = false;
    // Is the user pressing any keys?
    bool isMoving = false;
    // When did the user start walking (Used for going into trot after a while)
    float walkTimeStart = 0.0f;
    // Last time the jump button was clicked down
    float lastJumpButtonTime = -10.0f;
    // Last time we performed a jump
    float lastJumpTime = -1.0f;
    Vector3 inAirVelocity = Vector3.zero;
    
    float lastGroundedTime = 0.0f;
    
    
    bool isControllable = true;
    
    public void Awake()
    {
    	moveDirection = transform.TransformDirection(Vector3.forward);
    	
    	_animation = GetComponent<Animation>();
    	if(_animation == null)
    		Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
    	
    	/*
    public var idleAnimation : AnimationClip;
    public var walkAnimation : AnimationClip;
    public var runAnimation : AnimationClip;
    public var jumpPoseAnimation : AnimationClip;	
    	*/
    	if(idleAnimation == null) {
    		_animation = null;
    		Debug.Log("No idle animation found. Turning off animations.");
    	}
    	if(walkAnimation == null) {
    		_animation = null;
    		Debug.Log("No walk animation found. Turning off animations.");
    	}
    	if(runAnimation == null) {
    		_animation = null;
    		Debug.Log("No run animation found. Turning off animations.");
    	}
    	if((jumpPoseAnimation == null) && canJump) {
    		_animation = null;
    		Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
    	}
    			
    }
    
    
    public void UpdateSmoothedMovementDirection()
    {
    	Transform cameraTransform = Camera.main.transform;
    	bool grounded = IsGrounded();
    	
    	// Forward vector relative to the camera along the x-z plane	
    	Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
    	forward.y = 0.0f;
    	forward = forward.normalized;
    
    	// Right vector relative to the camera
    	// Always orthogonal to the forward vector
    	Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);
    
    	float v = Input.GetAxisRaw("Vertical");
    	float h = Input.GetAxisRaw("Horizontal");
    
    	// Are we moving backwards or looking backwards
    	if (v < -0.2f)
    		movingBack = true;
    	else
    		movingBack = false;
    	
    	bool wasMoving = isMoving;
    	isMoving = Mathf.Abs (h) > 0.1f || Mathf.Abs (v) > 0.1f;
    		
    	// Target direction relative to the camera
    	Vector3 targetDirection = h * right + v * forward;
    	
    	// Grounded controls
    	if (grounded)
    	{
    		// Lock camera for short period when transitioning moving & standing still
    		lockCameraTimer += Time.deltaTime;
    		if (isMoving != wasMoving)
    			lockCameraTimer = 0.0f;
    
    		// We store speed and direction seperately,
    		// so that when the character stands still we still have a valid forward direction
    		// moveDirection is always normalized, and we only update it if there is user input.
    		if (targetDirection != Vector3.zero)
    		{
    			// If we are really slow, just snap to the target direction
    			if (moveSpeed < walkSpeed * 0.9f && grounded)
    			{
    				moveDirection = targetDirection.normalized;
    			}
    			// Otherwise smoothly turn towards it
    			else
    			{
    				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
    				
    				moveDirection = moveDirection.normalized;
    			}
    		}
    		
    		// Smooth the speed based on the current target direction
    		float curSmooth = speedSmoothing * Time.deltaTime;
    		
    		// Choose target speed
    		//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
    		float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
    	
    		_characterState = CharacterState.Idle;
    		
    		// Pick speed modifier
    		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
    		{
    			targetSpeed *= runSpeed;
    			_characterState = CharacterState.Running;
    		}
    		else if (Time.time - trotAfterSeconds > walkTimeStart)
    		{
    			targetSpeed *= trotSpeed;
    			_characterState = CharacterState.Trotting;
    		}
    		else
    		{
    			targetSpeed *= walkSpeed;
    			_characterState = CharacterState.Walking;
    		}
    		
    		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
    		
    		// Reset walk time start when we slow down
    		if (moveSpeed < walkSpeed * 0.3f)
    			walkTimeStart = Time.time;
    	}
    	// In air controls
    	else
    	{
    		// Lock camera while in air
    		if (jumping)
    			lockCameraTimer = 0.0f;
    
    		if (isMoving)
    			inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
    	}
    	
    
    		
    }
    
    
    public void ApplyJumping()
    {
    	// Prevent jumping too fast after each other
    	if (lastJumpTime + jumpRepeatTime > Time.time)
    		return;
    
    	if (IsGrounded()) {
    		// Jump
    		// - Only when pressing the button down
    		// - With a timeout so you can press the button slightly before landing		
    		if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
    			verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
    			SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
    		}
    	}
    }
    
    
    public void ApplyGravity()
    {
    	if (isControllable)	// don't move player at all if not controllable.
    	{
    		// When we reach the apex of the jump we send out a message
    		if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
    		{
    			jumpingReachedApex = true;
    			SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
    		}
    	
    		if (IsGrounded ())
    			verticalSpeed = 0.0f;
    		else
    			verticalSpeed -= gravity * Time.deltaTime;
    	}
    }
    
    public float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
    	// From the jump height and gravity we deduce the upwards speed 
    	// for the character to reach at the apex.
    	return Mathf.Sqrt(2 * targetJumpHeight * gravity);
    }
    
    public void DidJump()
    {
    	jumping = true;
    	jumpingReachedApex = false;
    	lastJumpTime = Time.time;
    	lastJumpButtonTime = -10.0f;
    	
    	_characterState = CharacterState.Jumping;
    }
    
    public void Update() {
    	
    	if (!isControllable)
    	{
    		// kill all inputs if not controllable.
    		Input.ResetInputAxes();
    	}
    
    	if (Input.GetButtonDown ("Jump"))
    	{
    		lastJumpButtonTime = Time.time;
    	}
    
    	UpdateSmoothedMovementDirection();
    	
    	// Apply gravity
    	// - extra power jump modifies gravity
    	// - controlledDescent mode modifies gravity
    	ApplyGravity ();
    
    	// Apply jumping logic
    	ApplyJumping ();
    	
    	// Calculate actual motion
    	Vector3 movement = moveDirection * moveSpeed + new Vector3 (0.0f, verticalSpeed, 0.0f) + inAirVelocity;
    	movement *= Time.deltaTime;
    	
    	// Move the controller
    	CharacterController controller = GetComponent<CharacterController>();
    	collisionFlags = controller.Move(movement);
    	
    	// ANIMATION sector
    	if(_animation != null) {
    		if(_characterState == CharacterState.Jumping) 
    		{
    			if(!jumpingReachedApex) {
    				_animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
    				_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
    				_animation.CrossFade(jumpPoseAnimation.name);
    			} else {
    				_animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
    				_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
    				_animation.CrossFade(jumpPoseAnimation.name);				
    			}
    		} 
    		else 
    		{
    			if(controller.velocity.sqrMagnitude < 0.1f) {
    				_animation.CrossFade(idleAnimation.name);
    			}
    			else 
    			{
    				if(_characterState == CharacterState.Running) {
    					_animation[runAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
    					_animation.CrossFade(runAnimation.name);	
    				}
    				else if(_characterState == CharacterState.Trotting) {
    					_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, trotMaxAnimationSpeed);
    					_animation.CrossFade(walkAnimation.name);	
    				}
    				else if(_characterState == CharacterState.Walking) {
    					_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
    					_animation.CrossFade(walkAnimation.name);	
    				}
    				
    			}
    		}
    	}
    	// ANIMATION sector
    	
    	// Set rotation to the move direction
    	if (IsGrounded())
    	{
    		
    		transform.rotation = Quaternion.LookRotation(moveDirection);
    			
    	}	
    	else
    	{
    		Vector3 xzMove = movement;
    		xzMove.y = 0.0f;
    		if (xzMove.sqrMagnitude > 0.001f)
    		{
    			transform.rotation = Quaternion.LookRotation(xzMove);
    		}
    	}	
    	
    	// We are in jump mode but just became grounded
    	if (IsGrounded())
    	{
    		lastGroundedTime = Time.time;
    		inAirVelocity = Vector3.zero;
    		if (jumping)
    		{
    			jumping = false;
    			SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
    		}
    	}
    }
    
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
    //	Debug.DrawRay(hit.point, hit.normal);
    	if (hit.moveDirection.y > 0.01f) 
    		return;
    }
    
    public float GetSpeed() {
    	return moveSpeed;
    }
    
    public bool IsJumping() {
    	return jumping;
    }
    
    public bool IsGrounded() {
    	return (int)(collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }
    
    public Vector3 GetDirection() {
    	return moveDirection;
    }
    
    public bool IsMovingBackwards() {
    	return movingBack;
    }
    
    public float GetLockCameraTimer() 
    {
    	return lockCameraTimer;
    }
    
    public bool IsMoving()
    {
    	return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
    }
    
    public bool HasJumpReachedApex()
    {
    	return jumpingReachedApex;
    }
    
    public bool IsGroundedWithTimeout()
    {
    	return lastGroundedTime + groundedTimeout > Time.time;
    }
    
    public void Reset()
    {
    	gameObject.tag = "Player";
    }


}