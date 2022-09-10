using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public struct AttackData {
	public float RawDamage;
	public float BlockDamage;
	public float Hitstun;
	public float Blockstun;

	public float HeatGain;


	public Vector2 HitForce;
	public Vector2 BlockForce;

	public AttackData(float rawDamage, float blockDamage, float hitstun, float blockstun, float heatGain, Vector2 hitForce, Vector2 blockForce) {
		this.RawDamage = rawDamage;
		this.BlockDamage = blockDamage;
		this.Hitstun = hitstun;
		this.Blockstun = blockstun;

		this.HeatGain = heatGain;

		this.HitForce = hitForce;
		this.BlockForce = blockForce;
	}
}



public enum FighterState {
	Idling, //Standing idle
	Crouching, //Crouching without blocking
	Walking, //Walking
	Jumping,
	Attacking, //Attacking
	AttackingLow, //Attacking
	AttackingHigh, //Attacking
	Blocking, //Blocking high
	BlockingLow, //Blocking Low
	Recovery, //Recovery after a combo
	Hitstunned, //yo you got hit
	Blockstunned,
	Endlag, //After attacking
	Dashing,
	Backdashing

}

/// <summary>
/// A custom class used to handle characters. Can be used for player characters or NPCs.
/// </summary>
/// <remarks>
/// <para>
/// This class can be easily instantiated and destroyed, using the Maid class. Note that this 
/// </para>
/// </remarks>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Fighter : SpriteFrame, IControllable<InputActionsClasses.Default> {
	public Animator Animator;
	public Rigidbody2D Rigidbody;
	public Transform GroundCheck;

	public Transform Target;

	public InputActionsClasses.Default ControlledInputAction;

	private HitboxCollisionInfo lastInfo;

	[Header("Health")]
	public int MaxHealth = 100;
	public Variable<float> Health = new Variable<float>(100);
	public bool Dead = false;
	public int RegenRate = 5;

	[Header("Movement")]
	public bool IsGrounded = false;
	public bool CanMove = true;
	public float StopSpeed = Config.Player.Movement.StopSpeed;

	[Header("Jump")]
	public float JumpForce = Config.Player.Movement.Jump.JumpForce;
	public float JumpCooldown = Config.Player.Movement.Jump.JumpCooldown;
	public bool CanJump = true;

	[Header("Dash")]
	public float DashForce = Config.Player.Movement.Dash.DashForce;
	public float DashCooldown = Config.Player.Movement.Dash.DashCooldown;
	public bool CanDash = true;

	[Header("Crouch")]
	public bool CanCrouch = true;

	[Header("Combat")]
	public bool CanAttack = true;
	public bool CanBlock = true;

	public Variable<float> Heat = new Variable<float>(1);
	public float MaxHeat = 4;

	protected HumanDescription _HumDesc;

	public Variable<Vector2> CharDirection = new Variable<Vector2>();
	public float Speed;

	public FighterState CurrentState = FighterState.Idling;

	protected float currentSpeed;

	private DesiredAction<Vector2> _CharFacingVector = new DesiredAction<Vector2>();
	protected Vector2 CharFacingVector {
		get => _CharFacingVector.Value;
		set {
			_CharFacingVector.Desired = value;
		}
	}


	private bool PlayMove = true;

	[Header("Management")]
	private Maid CounterMaid = new Maid();
	private Listener<float> JumpDebounce;
	private Listener<float> DashDebounce;
	private Listener<float> RecoveryListener;


	private void LockMovement() {
		CanJump = false;
		CanMove = false;
		CanCrouch = false;
		CanDash = false;
	}

	private void UnlockMovement() {
		CanJump = true;
		CanMove = true;
		CanCrouch = true;
		CanDash = true;
	}

	private void LockAttack() {
		CanAttack = false;
		CanBlock = false;
	}

	private void UnlockAttack() {
		CanAttack = true;
		CanBlock = true;
	}

	private void LockAll() {
		LockMovement();
		LockAttack();
	}

	private void UnlockAll() {
		UnlockMovement();
		UnlockAttack();
	}

	private FighterState ReturnNeutral() {
		UnlockAttack();
		if (CharDirection.Value.y > 0 && CanJump) {
			return FighterState.Jumping;
		}

		if (CharDirection.Value.y < 0) {
			return FighterState.Crouching;
		} else {
			return FighterState.Idling;
		}
	}

	public void ChangeState(FighterState state) {
		if (CurrentState != state) {
			CurrentState = state;
			switch (state) {
				case FighterState.Idling:
					Animator.Play("Idle");
					UnlockAll();
					break;
				case FighterState.Crouching:
					Animator.Play("Crouch");
					LockMovement();
					break;
				case FighterState.Walking:
					Animator.Play("Walk");
					break;
				case FighterState.Jumping:
					Animator.Play("Jump");
					LockMovement();
					break;
				case FighterState.Attacking:
					CanAttack = false;
					LockMovement();
					break;
				case FighterState.AttackingLow:
					CanAttack = false;
					LockMovement();
					break;
				case FighterState.AttackingHigh:
					CanAttack = false;
					LockMovement();
					break;
				case FighterState.Blocking:
					Animator.Play("Blocking");
					break;
				case FighterState.BlockingLow:
					Animator.Play("Blocking Low");
					break;
				case FighterState.Hitstunned:
					Animator.Play("Hurt");
					LockAll();
					break;
				case FighterState.Blockstunned:
					LockAll();
					break;
				case FighterState.Recovery:
					UnlockAll();
					break;
				case FighterState.Endlag:
					LockAll();
					break;
				case FighterState.Dashing:
					LockMovement();
					break;
				case FighterState.Backdashing:
					LockMovement();
					break;
			}
		}
	}

	protected Maid ControlMaid = new Maid();

	public void OnDestroy() {
		ControlMaid.Dispose();
		RecoveryListener?.Dispose();
		Maid.Dispose();
	}

	public virtual void BindPlayerControls(InputActionsClasses.Default controller) {
		ControlledInputAction = controller;

		ControlMaid.GiveTask(
			InputController.InputAction[controller.Player.Right].Connect(
				(InputAction.CallbackContext ctx) => {
					float val = ctx.ReadValue<float>();
					if (val == 1) {
						CharDirection.Value = new Vector2(1, CharDirection.Value.y);
					} else if (InputController.InputAction[controller.Player.Left]) {
						CharDirection.Value = new Vector2(-1, CharDirection.Value.y);
					} else {
						CharDirection.Value = new Vector2(0, CharDirection.Value.y);
					}
					return true;
				}
			)
		);

		ControlMaid.GiveTask(
			InputController.InputAction[controller.Player.Left].Connect(
				(InputAction.CallbackContext ctx) => {
					float val = ctx.ReadValue<float>();
					if (val == 1) {
						CharDirection.Value = new Vector2(-1, CharDirection.Value.y);
					} else if (InputController.InputAction[controller.Player.Right]) {
						CharDirection.Value = new Vector2(1, CharDirection.Value.y);
					} else {
						CharDirection.Value = new Vector2(0, CharDirection.Value.y);
					}
					return true;
				}
			)
		);

		ControlMaid.GiveTask(
			InputController.InputAction[controller.Player.Up].Connect(
				(InputAction.CallbackContext ctx) => {
					float val = ctx.ReadValue<float>();
					if (val == 1) {
						CharDirection.Value = new Vector2(CharDirection.Value.x, 1);
					} else if (InputController.InputAction[controller.Player.Down]) {
						CharDirection.Value = new Vector2(CharDirection.Value.x, -1);
					} else {
						CharDirection.Value = new Vector2(CharDirection.Value.x, 0);
					}
					return true;
				}
			)
		);

		ControlMaid.GiveTask(
			InputController.InputAction[controller.Player.Down].Connect(
				(InputAction.CallbackContext ctx) => {
					float val = ctx.ReadValue<float>();
					if (val == 1) {
						CharDirection.Value = new Vector2(CharDirection.Value.x, -1);
					} else if (InputController.InputAction[controller.Player.Up]) {
						CharDirection.Value = new Vector2(CharDirection.Value.x, 1);
					} else {
						CharDirection.Value = new Vector2(CharDirection.Value.x, 0);
					}
					return true;
				}
			)
		);

		ControlMaid.GiveTask(
			InputController.InputAction[controller.Player.Button1].Connect(
				(InputAction.CallbackContext ctx) => {
					float val = ctx.ReadValue<float>();
					if (val == 1) {
						Light();
					}
					return true;
				}
			)
		);

		ControlMaid.GiveTask(
			InputController.InputAction[controller.Player.Button2].Connect(
				(InputAction.CallbackContext ctx) => {
					float val = ctx.ReadValue<float>();
					if (val == 1) {
						Medium();
					}
					return true;
				}
			)
		);

		ControlMaid.GiveTask(
			InputController.InputAction[controller.Player.Button3].Connect(
				(InputAction.CallbackContext ctx) => {
					float val = ctx.ReadValue<float>();
					if (val == 1) {
						Heavy();
					}
					return true;
				}
			)
		);

		ControlMaid.GiveTask(
			InputController.InputAction[controller.Player.Button4].Connect(
				(InputAction.CallbackContext ctx) => {
					float val = ctx.ReadValue<float>();
					if (val == 1) {
						DP();
					}
					return true;
				}
			)
		);

		Debug.Log(ControlMaid._tasks.Count);
	}

	public virtual void UnbindControls() {
		ControlMaid.DoCleaning();
	}

	public override void Awake() {
		base.Awake();

		Animator = gameObject.GetComponent<Animator>();
		Rigidbody = gameObject.GetComponent<Rigidbody2D>();
	}

	public void Start() {
		Maid HealthMaid = new Maid();

		Maid.GiveTask(
			Heat.Connect(
				(float val) => {
					if (val > MaxHeat) {
						Heat.Value = MaxHeat;
						Heat.Call();
					}
					return true;
				}
			)
		);

		float prevHealth = Health.Value;
		Maid.GiveTask(Health.Connect((float val) => {
			if (val < prevHealth) {
				HealthMaid.DoCleaning();
				HealthMaid.GiveTask(Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, 5, (float dt) => {
					HealthMaid.GiveTask(Runservice.BindToFixedUpdate(Global.RunservicePriority.Heartbeat.Physics, (float dt) => {
						Health.Value += RegenRate * dt;
						if (Health.Value > MaxHealth) {
							Health.Value = MaxHealth;
							return false;
						}
						return true;
					}));
					return false;
				}));
			}

			if (val <= 0) {
				Dead = true;
				return false;
			}

			prevHealth = val;
			return true;
		}));
	}

	void FixedUpdate() {
		Animator.SetInteger("FighterState", ((int)CurrentState));

		if (Target) {
			if (Target.position.x < transform.position.x) {
				_CharFacingVector.Desired = new Vector2(-1, _CharFacingVector.Actual.y);
			} else if (Target.position.x > transform.position.x) {
				_CharFacingVector.Desired = new Vector2(1, _CharFacingVector.Actual.y);
			}
		}

		if (CharFacingVector.x < 0) {
			FlipX = true;
		} else if (CharFacingVector.x > 0) {
			FlipX = false;
		}
		if (FlipX) {
			Animator.SetBool("WalkBack", CharDirection.Value.x > 0);
		} else {
			Animator.SetBool("WalkBack", CharDirection.Value.x < 0);
		}

		if (Physics2D.OverlapCircle(GroundCheck.position, .1F, LayerMask.GetMask("Environment"))) {
			IsGrounded = true;
			if (CurrentState == FighterState.Jumping) {
				ChangeState(ReturnNeutral());
			}
		} else {
			IsGrounded = false;
		}

		if (CharDirection.Value.y > 0 && CanJump) {
			Jump();
		}

		if (CharDirection.Value.y < 0 && CanCrouch) {
			Crouch();
		} else if (CharDirection.Value.y >= 0) {
			UnCrouch();
		}

		if (CurrentState == FighterState.Idling) {
			if (CharDirection.Value.x != 0) {
				ChangeState(FighterState.Walking);
				if (FlipX) {
					if (CharDirection.Value.x > 0) {
						CanBlock = true;
					} else {
						CanBlock = false;
					}
				} else {
					if (CharDirection.Value.x < 0) {
						CanBlock = true;
					} else {
						CanBlock = false;
					}
				}
			}
		}

		if (CurrentState == FighterState.Crouching) {

			if (FlipX) {
				if (CharDirection.Value.x > 0) {
					CanBlock = true;
				} else {
					CanBlock = false;
				}
			} else {
				if (CharDirection.Value.x < 0) {
					CanBlock = true;
				} else {
					CanBlock = false;
				}
			}
		}


		if (CurrentState == FighterState.Walking) {
			if (CharDirection.Value.x == 0) {
				ChangeState(FighterState.Idling);
			}
		}

		if (CharDirection.Value.x == 0) {
			CanBlock = false;
		}

		if (CurrentState == FighterState.Walking) {
			Rigidbody.MovePosition(transform.position + new Vector3(CharDirection.Value.x * Speed, 0, 0) * Time.deltaTime);
		}

	}

	public void Stun(float hitstun) {
		RecoveryListener?.Destroy();
		CounterMaid.DoCleaning();

		RecoveryListener = Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, hitstun,
			(float dt) => {
				ChangeState(ReturnNeutral());
				return false;
			}
		);
	}

	public void TakeDamage(float damage) {
		Health.Value -= damage;
	}

	private void damageOther(Fighter other, AttackData data) {
		other.ChangeState(FighterState.Hitstunned);

		other.TakeDamage(data.RawDamage);
		other.Stun(data.Hitstun);
		other.Rigidbody.AddForce(new Vector2(data.HitForce.x * _CharFacingVector.Actual.x, data.HitForce.y));

		other.Heat.Value += data.HeatGain / 3;
		Heat.Value += data.HeatGain;
	}

	private void blockDamageOther(Fighter other, AttackData data) {
		other.TakeDamage(data.BlockDamage);
		other.Stun(data.Blockstun);
		other.Rigidbody.AddForce(new Vector2(data.BlockForce.x * _CharFacingVector.Actual.x, data.BlockForce.y));

		other.Heat.Value += data.HeatGain / 4;
		Heat.Value += data.HeatGain / 3;

		if (other.CurrentState == FighterState.Crouching) {
			other.ChangeState(FighterState.BlockingLow);
		} else {
			other.ChangeState(FighterState.Blocking);
		}
	}

	private void hitOther(Fighter other, AttackData data) {
		if (other.CanBlock) {
			blockDamageOther(other, data);
			SoundController.Singleton.PlayClipAtPoint("BHit", transform.position);
		} else {
			damageOther(other, data);
			SoundController.Singleton.PlayClipAtPoint("Hit", transform.position);
		}
	}

	public void Light_Hit(HitboxCollisionInfo other) {
		Fighter fighter = other.GameObject.GetComponent<Fighter>();

		if (fighter) {
			if (fighter.CanBlock) {
				if (fighter.CurrentState != FighterState.Crouching && CurrentState == FighterState.AttackingLow) {
					damageOther(fighter, AttackDataInfo.Fighter.Light);
					SoundController.Singleton.PlayClipAtPoint("Hit", transform.position);
				} else {
					blockDamageOther(fighter, AttackDataInfo.Fighter.Light);
					SoundController.Singleton.PlayClipAtPoint("Bhit", transform.position);
				}
			} else {
				damageOther(fighter, AttackDataInfo.Fighter.Light);
				SoundController.Singleton.PlayClipAtPoint("Hit", transform.position);
			}
		}
	}

	public void Medium_Hit(HitboxCollisionInfo other) {
		Fighter fighter = other.GameObject.GetComponent<Fighter>();
		if (fighter) {
			hitOther(fighter, AttackDataInfo.Fighter.Medium);
		}
	}

	public void Heavy_Hit(HitboxCollisionInfo other) {
		Fighter fighter = other.GameObject.GetComponent<Fighter>();
		if (fighter) {
			if (fighter.CanBlock) {
				if (fighter.CurrentState == FighterState.Crouching) {
					damageOther(fighter, AttackDataInfo.Fighter.Light);
					SoundController.Singleton.PlayClipAtPoint("Hit", transform.position);
				} else {
					blockDamageOther(fighter, AttackDataInfo.Fighter.Light);
					SoundController.Singleton.PlayClipAtPoint("Bhit", transform.position);
				}
			} else {
				damageOther(fighter, AttackDataInfo.Fighter.Light);
				SoundController.Singleton.PlayClipAtPoint("Hit", transform.position);
			}
		}
	}

	public void DP_Hit(HitboxCollisionInfo other) {
		Fighter fighter = other.GameObject.GetComponent<Fighter>();
		if (fighter) {
			hitOther(fighter, AttackDataInfo.Fighter.DP);
		}
	}

	public void Light() {
		if (CanAttack) {
			if (CurrentState == FighterState.Crouching) {
				Animator.Play("Attack Low");
				SoundController.Singleton.PlayClipAtPoint("Light", transform.position);
				ChangeState(FighterState.AttackingLow);
			} else {
				Animator.Play("Light Kick");
				SoundController.Singleton.PlayClipAtPoint("Light", transform.position);
				ChangeState(FighterState.Attacking);
			}

			RecoveryListener?.Destroy();

			RecoveryListener = Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, .35F,
				(float val) => {
					ChangeState(ReturnNeutral());
					return false;
				}
			);
		}
	}

	public void Medium() {
		if (CanAttack && CurrentState != FighterState.Jumping) {
			Animator.Play("Heavy Kick");
			SoundController.Singleton.PlayClipAtPoint("Mid", transform.position);

			ChangeState(FighterState.Attacking);

			CharDirection.Locked = true;

			RecoveryListener?.Destroy();

			Maid.GiveTask(Runservice.RunFor(Global.RunservicePriority.Heartbeat.Physics, .3F,
				(float val) => {

					Rigidbody.MovePosition(transform.position + (Vector2.right * _CharFacingVector.Actual.x * 5).AsVector3() * Time.deltaTime);
					return true;
				}
			));

			RecoveryListener = Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, .5F,
				(float val) => {
					CharDirection.Locked = false;
					ChangeState(ReturnNeutral());
					return false;
				}
			);
		}
	}

	public void Heavy() {
		if (CanAttack && CurrentState != FighterState.Jumping) {
			Animator.Play("Overhead");
			SoundController.Singleton.PlayClipAtPoint("Heavy", transform.position);

			ChangeState(FighterState.AttackingHigh);

			RecoveryListener?.Destroy();

			RecoveryListener = Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, .35F,
				(float val) => {
					ChangeState(ReturnNeutral());
					return false;
				}
			);
		}
	}

	public void DP() {
		if (CanAttack && CurrentState != FighterState.Jumping && Heat.Value >= 1) {
			Animator.Play("DP");
			SoundController.Singleton.PlayClipAtPoint("Placeholder", transform.position);

			ChangeState(FighterState.Attacking);

			Heat.Value -= 1;

			RecoveryListener?.Destroy();

			Maid.GiveTask(Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, .22F,
				(float dt) => {
					Runservice.RunFor(Global.RunservicePriority.Heartbeat.Physics, .1F,
						(float val) => {
							Rigidbody.MovePosition(transform.position + ((Vector2.up * 24).AsVector3() + (Vector2.right * _CharFacingVector.Actual.x * 10).AsVector3()) * Time.deltaTime);
							return true;
						}
					);
					return false;
				}
			));

			RecoveryListener = Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, .6F,
				(float val) => {
					ChangeState(ReturnNeutral());
					return false;
				}
			);
		}
	}

	public void Crouch() {
		if (CanCrouch) {
			ChangeState(FighterState.Crouching);
		}
	}

	public void UnCrouch() {
		if (CurrentState == FighterState.Crouching) {
			ChangeState(FighterState.Idling);
		}
	}

	public void Jump() {
		if (IsGrounded && CurrentState != FighterState.Jumping) {
			ChangeState(FighterState.Jumping);
			SoundController.Singleton.PlayClipAtPoint("Jump", transform.position);
			if (CharDirection.Value == Vector2.up) {
				Rigidbody.AddForce(CharDirection.Value * JumpForce);
			} else {
				Rigidbody.AddForce(new Vector2(CharDirection.Value.x / 2, 1) * JumpForce);
			}
		}
	}

	public void Dash() {
		if (IsGrounded) {

			DashDebounce?.Destroy();

			DashDebounce = Runservice.RunAfter(1, DashCooldown / 4,
				(float dt) => {
					ChangeState(ReturnNeutral());
					return false;
				}
			);

			if (CharDirection.Value.x > 0) {
				Rigidbody.AddForce(Vector2.right * DashForce);
			} else {
				Rigidbody.AddForce(Vector2.left * DashForce);
			}

			if (!FlipX) {
				ChangeState(FighterState.Dashing);
			} else {
				ChangeState(FighterState.Backdashing);
			}
		}
	}

	public void OnHitboxCollisionEnter(HitboxCollisionInfo other) {
		lastInfo = other;
	}

	void OnCollisionEnter2D(Collision2D col) {
		Debug.Log("Hi)");
	}

}
