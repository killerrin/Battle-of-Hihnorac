using UnityEngine;
using System.Collections;

public class Advanced_AI : MonoBehaviour {
	public Transform Target;
	public CharacterController charController;
	
	public float lookAtDistance = 25.0f;
	public float chaseRange = 15.0f;
	public float attackRange = 1.5f;
	public float moveSpeed = 5.0f;
	public float Damping = 6f;
	
	private float gravity = 20.0f;
	private Vector3 moveDirection = Vector3.zero;
	private float Distance;

    private float attackTimer = 0;
    public float MAX_ATTACK_TIMER = 1f;

    private EnemyConsts enemyConsts;
    private bool deathAniPlayed = false;

    private GameObject player;
    private PlayerConsts playerConsts;

	// Use this for initialization
	void Start () {
        enemyConsts = gameObject.GetComponent<EnemyConsts>();
        player = GameObject.Find("Player").gameObject;
        playerConsts = player.GetComponent<PlayerConsts>();
	}
	
	// Update is called once per frame
	void Update()
	{
        if (enemyConsts.isDead)
        {
            if (!deathAniPlayed)
            {
                animation.CrossFade("gethit");
                deathAniPlayed = true;
                audio.Play();
            }
        }
        else
        {
            Distance = Vector3.Distance(Target.position, transform.position);

            if (Distance < lookAtDistance)
            {
                if (!enemyConsts.isIdle)
                {
                    animation.CrossFade("StandingAimIdle");
                    enemyConsts.isIdle = true;
                    enemyConsts.isRunning = false;
                }

                lookAt();
            }

            if (Distance > lookAtDistance)
            {
                // Idle
                //renderer.material.color = Color.green;
                if (!enemyConsts.isIdle)
                {
                    animation.CrossFade("StandingAimIdle");
                    enemyConsts.isIdle = true;
                    enemyConsts.isRunning = false;
                }
            }

            if (Distance < chaseRange)
            {
                if (!enemyConsts.isRunning)
                {
                    animation.CrossFade("RunAim");
                    enemyConsts.isIdle = false;
                    enemyConsts.isRunning = true;
                }
                chase();
            }

            if (Distance < attackRange)
            {
                attack();
            }
        }
	}

	private void lookAt()
	{
		//renderer.material.color = Color.yellow;
		Quaternion rotation = Quaternion.LookRotation(Target.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);
	}

	private void chase()
	{
		//renderer.material.color = Color.red;

		charController.Move(moveDirection * Time.deltaTime);

		moveDirection = transform.forward;
		moveDirection *= moveSpeed;

		//- Apply Gravity to Player
		moveDirection.y -= gravity * Time.deltaTime;

		//- Finally Move the player
		charController.Move(moveDirection * Time.deltaTime);

	}
    private void attack()
    {
        Debug.Log("Attacking");

        if (attackTimer >= MAX_ATTACK_TIMER)
        {
            playerConsts.DecreaseHealth(5f);
            attackTimer = 0f;
        }
        else
        {
            attackTimer += 1 * Time.deltaTime;
        }
    }
}
