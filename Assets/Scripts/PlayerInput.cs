using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	public float playerSpeed = 6.0f;
	public float jumpSpeed = 12.0f;
	public float gravity = 20.0f;

    public bool jumpOverride = true;

	public CharacterController characterController;
	private Vector3 movementDirection = Vector3.zero;

    private PlayerConsts playerConsts;
    private int boostIncr;

    public AudioClip running;
    public AudioClip jumping;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.gameObject);
        Screen.lockCursor = (true);

        playerConsts = gameObject.GetComponent<PlayerConsts>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Vertical"))
            playerConsts.isMoving = true;
        else
            playerConsts.isMoving = false;
        if (Input.GetButtonDown("Horizontal"))
            playerConsts.isStrafing = true;
        else
            playerConsts.isStrafing = false;


		MovePlayer();


        switch (playerConsts.primaryWeapon.weaponType)
		{
			case "shotgun":
				if (Input.GetButtonDown("Fire1"))
				{
                    playerConsts.FireWeapon();
				}
				break;
			case "railgun":
				if (Input.GetButton("Fire1"))
				{
                    playerConsts.FireWeapon();
				}
				break;
		}

        if (Input.GetButtonDown("Fire2"))
        {
            playerConsts.ShootFireball();
        }

        if (Input.GetButtonDown("Fire3"))
        {
            playerConsts.MeleeAttack();
        }

		if (Input.GetKeyDown("tab"))
		{
            playerConsts.SwitchWeapon();
		}

        /// FUCKING DOORS! WHY DO YOU HATE ME!
        /// //**ahem** doors removed due to problems in animation (teleporting doors)
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        //    {
        //        Door door = hit.transform.GetComponent<Door>();
        //        if (door) StartCoroutine(door.PlayDoorAnimation());
        //    }
        //}

        //if (Input.GetKeyDown("e"))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Vector3.forward);//Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        //    {
        //        Door door = hit.transform.GetComponent<Door>();
        //        if (door) StartCoroutine(door.PlayDoorAnimation());
        //    }
        //}

		if (Input.GetKeyDown("r"))
		{
            playerConsts.ReloadWeapon();
		}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerConsts.PauseGame();
        }

        playerConsts.Update();
	}

	private void MovePlayer()
	{
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            if (audio.clip == running)
                audio.Stop();
        }

        if (characterController.isGrounded)
        {
            movementDirection = new Vector3(horizontal, 0, vertical);
            movementDirection = transform.TransformDirection(movementDirection);
            movementDirection *= playerSpeed;
            //else movementDirection *= (playerSpeed / 3f);

            if (!audio.isPlaying)
            {
                audio.clip = running;
                audio.Play();
            }

            if (Input.GetButton("Jump"))
            {
                if (!jumpOverride)
                {
                    movementDirection.y = jumpSpeed;
                    audio.clip = jumping;
                    audio.Play();
                }
            }
        }
        else
        {
            /// Too Glitchy. Try again later.
            //if (horizontal != 0 && vertical != 0)
            //{
            //    movementDirection = new Vector3(horizontal, movementDirection.y, vertical);
            //    movementDirection = transform.TransformDirection(movementDirection);
            //    movementDirection.x *= (playerSpeed / 2);
            //    movementDirection.z *= (playerSpeed);
            //}
        }


		//- Apply Gravity to Player
		movementDirection.y -= gravity * Time.deltaTime;


		//- Finally Move the player
		characterController.Move(movementDirection * Time.deltaTime);

	}
}
