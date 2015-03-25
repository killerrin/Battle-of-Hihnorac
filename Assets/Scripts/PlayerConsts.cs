using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConsts : MonoBehaviour {

	public GameObject arms;


	public Weapon primaryWeapon;
	public Weapon secondaryWeapon;
	public Weapon unatro;



    public GUITexture[] pauseMenu;
	public GUITexture[] guiBars;
	public GUIText[] guiText;
	public AudioClip[] audioclips;

	//public AudioSource emptyClip;
	//public AudioSource shieldsLow;
	//public AudioSource shieldsHit;
	//public AudioSource noAmmoSound;

	public Rigidbody projectile;
	public Transform projectileSpawnPoint;

	public Rigidbody fireball;
	public Rigidbody explosion;
	public Transform UnatroSpawnPoint;


	public float health = 100.0f;
	public float energyShields = 100.0f;
	public float unatroEnergy = 100.0f;
	public int ammunitionAmount = 100;
    public int strength = 2000;

	private float directHealthDamageModifier = 1.5f;
	private int fireballCostMultiplyer = 10;

	public bool isMoving = false;
	public bool isStrafing = false;
	public bool fireballLaunched = false;
	private bool fireballfired = false;
	public bool meleeLaunched = false;
	public bool meleeFired = false;
	public bool isWeaponSwitching = false;


	private bool usedUnatroInRange = false;
	private bool lostHealthInRange = false;
	private float unatroRegenCtr = 0.0f;
	private float healthRegenCtr = 0.0f;
	private const float UNATRO_REGEN_CTR_RATE = 1f;
	private const float HEALTH_REGEN_CTR_RATE = 1f;
	private const float UNATRO_REGEN_CTR_MAX = 5f;
	private const float HEALTH_REGEN_CTR_MAX = 5f;

    private bool gamePaused;
	
	private bool dead;

	public Weapon GetPrimaryWeapon
	{
		get { return primaryWeapon; }
	}

	public Weapon GetSecondaryWeapon
	{
		get { return secondaryWeapon; }
	}


	// Use this for initialization
	void Start () {
		//secondaryWeapon.active = false;

		primaryWeapon = new Weapon(0,"railgun",30f,10f,2f,10f,240,12,4);
		secondaryWeapon = new Weapon(1,"shotgun",30f,10f,2f,20f,32,20,4);
		unatro = new Weapon(2, "fireball", 20f, 20f, 10f, 50.0f, 1, 0, 1);

		// set all animations to default loop
		// easier in one shot, than having to change everything individually
		arms.animation.wrapMode = WrapMode.Once;

		// Set certain animations to loop
		arms.animation["Idle"].wrapMode = WrapMode.Loop;
		arms.animation["Run"].wrapMode = WrapMode.Loop;
		arms.animation["Run2"].wrapMode = WrapMode.Loop;
		arms.animation["SMG_run"].wrapMode = WrapMode.Loop;
		arms.animation["SMG_run2"].wrapMode = WrapMode.Loop;

		// Speed up other animaions
		arms.animation["SMG_grenade2"].speed = 1.5f;
		arms.animation["SniperToSMG"].speed = 1.5f;
		arms.animation["SMGToSniper"].speed = 1.5f; 

			// Lastly, set up Idle.
		arms.animation.CrossFade("Idle");
	}

	public void Update()
	{
        if (!gamePaused) { Screen.lockCursor = (true); foreach (GUITexture tex in pauseMenu) { tex.enabled = false; } }


		primaryWeapon.ReloadTimerUpdate();
		unatro.ReloadTimerUpdate();

		if (fireballLaunched)
		{
			LaunchFireball();
		}
		if (meleeLaunched)
		{
			LaunchMelee();
		}

		if (isWeaponSwitching)
		{
			SwitchTheWeapon();
		}

		if (usedUnatroInRange)
		{
			unatroRegenCtr += (UNATRO_REGEN_CTR_RATE * Time.deltaTime);
			if (unatroRegenCtr >= UNATRO_REGEN_CTR_MAX) { usedUnatroInRange = false; }
		}
		else { IncreaseUnatroEnergy(0.5f); }

		if (lostHealthInRange)
		{
			healthRegenCtr += (HEALTH_REGEN_CTR_RATE * Time.deltaTime);
			if (healthRegenCtr >= HEALTH_REGEN_CTR_MAX) { lostHealthInRange = false; }
		}
		else { IncreaseHealth(0.5f); }

		UpdateGUI();        
	}

	private void UpdateGUI()
	{
		//--------------------------------------------------------------
		//----- Just for testing due to enemies not being in place
		//if (!dead)
		//{
		//    if (health >= 0) 
		//    { 
		//        DecreaseHealth(0.5f);
		//        if (health <= 0) { dead = true; } 
		//    }
		//}
		//else
		//{
		//    //IncreaseHealth(0.5f);
		//    if (energyShields >= 100.0f) { dead = false; }
		//}
		//--------------------------------------------------------------
		//--------------------------------------------------------------

		Rect gui = guiBars[0].pixelInset;
		guiBars[0].pixelInset = new Rect(gui.x, gui.y, (energyShields * 5), gui.height);
		if (energyShields > 0.0f)
		{
			if (energyShields >= 100.0f)
			{
				guiText[0].pixelOffset = new Vector2(-10.0f, guiText[0].pixelOffset.y);
			}
			else
			{
				guiText[0].pixelOffset = new Vector2(-5.0f, guiText[0].pixelOffset.y);
			}
			guiText[0].text = ((int)energyShields).ToString();
		}
		else guiText[0].text = "";

		
		gui = guiBars[1].pixelInset;
		guiBars[1].pixelInset = new Rect(gui.x, gui.y, (health * 5), gui.height);
		if (energyShields <= 0.0f)
		{
			if (health >= 100.0f)
			{
				guiText[1].pixelOffset = new Vector2(-10.0f, guiText[1].pixelOffset.y);
			}
			else
			{
				guiText[1].pixelOffset = new Vector2(-5.0f, guiText[1].pixelOffset.y);
			}
			guiText[1].text = ((int)health).ToString();

		}
		else guiText[1].text = "";



		gui = guiBars[2].pixelInset;
		guiBars[2].pixelInset = new Rect(gui.x, gui.y, (unatroEnergy * 2), gui.height);
		if (unatroEnergy >= 100.0f)
		{
			guiText[2].pixelOffset = new Vector2(-10.0f, guiText[2].pixelOffset.y);
		}
		else
		{
			guiText[2].pixelOffset = new Vector2(-5.0f, guiText[2].pixelOffset.y);
		}
		guiText[2].text = ((int)unatroEnergy).ToString();

        switch (primaryWeapon.weaponID)
        {
            case 0:
                guiBars[3].enabled = true;
                guiBars[4].enabled = false;
                break;
            case 1:
                guiBars[3].enabled = false;
                guiBars[4].enabled = true;
                break;
        }
        guiText[3].text = primaryWeapon.ammoInClip + "/" + primaryWeapon.clipSize;
        guiText[4].text = "Belt: "+ammunitionAmount;
	}

    public void PauseGame()
    {
        //Application.LoadLevel(0);
        switch(gamePaused)
        {
            case true:
                gamePaused = false;
                Screen.lockCursor = (true);
                Time.timeScale = 1;
                GameObject.Find("Main Camera").GetComponent<MouseLook>().enabled = true;
                GameObject.Find("scifi_model01_1p_all_animations_separated").GetComponent<MouseLook>().enabled = true;
                gameObject.GetComponent<MouseLook>().enabled = true;

                foreach (GUITexture tex in pauseMenu) { tex.enabled = false; }
                foreach (GUITexture tex in guiBars) { tex.enabled = true; }
                foreach (GUIText tex in guiText) { tex.enabled = true; }
                break;
            case false:
                Screen.lockCursor = (false);
                gamePaused = true;
                Time.timeScale = 0;
                GameObject.Find("Main Camera").GetComponent<MouseLook>().enabled = false;
                GameObject.Find("scifi_model01_1p_all_animations_separated").gameObject.GetComponent<MouseLook>().enabled = false;
                gameObject.GetComponent<MouseLook>().enabled = false;

                foreach (GUITexture tex in pauseMenu) { tex.enabled = true; }
                foreach (GUITexture tex in guiBars) { tex.enabled = false; }
                foreach (GUIText tex in guiText) { tex.enabled = false; }
                break;
        }


    }


	public void DecreaseHealth(float ammount)
	{
		if (energyShields > 0.0f)
		{
			//shieldsHit.Play();
			energyShields -= ammount;
			if (energyShields <= 0.0f) 
			{ 
				health -= (energyShields * directHealthDamageModifier);
				energyShields = 0.0f;
			}
		}
		else
		{
			if (health >= 0.0f) 
			{ 
				health -= (ammount * directHealthDamageModifier);
                if (health <= 0)
                {
                    health = 0.0f;

                    Destroy(GameObject.Find("Player").gameObject);
                    Destroy(GameObject.Find("GUI").gameObject);
                    Screen.lockCursor = (false);
                    Application.LoadLevel(0);
                }
			}
		}

		lostHealthInRange = true;
		healthRegenCtr = 0.0f;
	}

	public void IncreaseHealth(float ammount)
	{
		if (health < 100.0f)
		{
			health += ammount;
			if (health >= 100.0f)
			{ 
				float difference = health - 100.0f;
				energyShields += difference;
				health = 100.0f; 
			}
		}
		else
		{
			energyShields += ammount;
			if (energyShields >= 100.0f) { energyShields = 100.0f; }
		}
	}

	public void DecreaseUnatroEnergy(float ammount)
	{
		if (unatroEnergy > 0) { unatroEnergy -= ammount; }
		if (unatroEnergy <= 0) { unatroEnergy = 0; }

		usedUnatroInRange = true;
		unatroRegenCtr = 0.0f;
	}

	public void IncreaseUnatroEnergy(float ammount)
	{
		unatro.Reload(1.0f);

		if (unatroEnergy < 100) { unatroEnergy += ammount; }
		if (unatroEnergy >= 100) { unatroEnergy = 100; }        
	}

	public void SwitchWeapon()
	{
        if (gamePaused) { return; }
		//if (!primaryWeapon.isReloading)
		switch(primaryWeapon.weaponID)
		{
			case 0:
				arms.animation.CrossFade("SniperToSMG");
				break;
			case 1:
				arms.animation.CrossFade("SMGToSniper");
				break;
		}
		
		isWeaponSwitching = true;
	}

	private void SwitchTheWeapon()
	{
		switch(primaryWeapon.weaponID)
		{
			case 0:
				if (arms.animation["SniperToSMG"].time < 1 ||
					arms.animation["SniperToSMG"].time > 1.333)
					return;
				break;
			case 1:
				if (arms.animation["SMGToSniper"].time < 1 ||
					arms.animation["SMGToSniper"].time > 1.333)
					return;
				break;
		}

		Weapon temp = secondaryWeapon;

		secondaryWeapon = primaryWeapon;
		primaryWeapon = temp;

		//secondaryWeapon.gameObject.active = false;
		//primaryWeapon.gameObject.active = true;

		secondaryWeapon.ResetReloadingCtr();
		isWeaponSwitching = false;
	}

	public void PickupWeapon(Weapon weapon)
	{
		if ((primaryWeapon.weaponID != weapon.weaponID) && (secondaryWeapon.weaponID != weapon.weaponID)) { primaryWeapon = weapon; }
	}

	public void FireWeapon()
	{
        if (gamePaused) { return; }
		if (unatro.isReloading) { return; }
		if (isWeaponSwitching) { return; }

		if (primaryWeapon.ammoInClip > 0) 
		{
			if (primaryWeapon.FireWeapon(projectile, projectileSpawnPoint))
			{
				if (isMoving)
				{
					arms.animation["Fire1shot"].speed = 3;
					arms.animation["SMG_fire1shot"].speed = 3;
				}
				else
				{
					arms.animation["Fire1shot"].speed = 1;
					arms.animation["SMG_fire1shot"].speed = 1;
				}


				switch (primaryWeapon.weaponID)
				{
					case 0:
						arms.animation.CrossFade("Fire1shot");
						break;
					case 1:
						arms.animation.CrossFade("SMG_fire1shot");
						break;
				}
				//--primaryWeapon.ammoInClip;
			}
		}
		else 
		{
			Debug.Log("Reloading...");
			//noAmmoSound.Play();

			switch (primaryWeapon.weaponID)
			{
				case 0:
					arms.animation.CrossFade("Reload");
					break;
				case 1:
					arms.animation.CrossFade("SMG_reload");
					break;
			}

			ammunitionAmount -= primaryWeapon.Reload(ammunitionAmount);
		}
	}

	public void ShootFireball()
	{
        if (gamePaused) { return; }
		if (unatro.ammoInClip > 0)
		{
			if (!unatro.isReloading)
			{  
				switch (primaryWeapon.weaponID)
				{
					case 0:
						arms.animation.CrossFade("SMG_grenade2");
						break;
					case 1:
						arms.animation.CrossFade("SMG_grenade2");
						break;
				}

				fireballLaunched = true;
			}
		}
		
	}

	private void LaunchFireball()
	{
		//Debug.Log(unatroEnergy);

		if (!fireballfired)
		{
			if (arms.animation["SMG_grenade2"].time > 1.03 &&
			arms.animation["SMG_grenade2"].time < 1.133)
			{
				unatro.FireWeapon(fireball, UnatroSpawnPoint);
				DecreaseUnatroEnergy(unatro.Reload(unatroEnergy) * fireballCostMultiplyer);
				fireballfired = true;
			}
		}
		else
		{
			if (arms.animation["SMG_grenade2"].time > 1.433)
			{
				fireballLaunched = false;
				fireballfired = false;
				switch (primaryWeapon.weaponID)
				{
					case 0:
						arms.animation.CrossFade("Idle");
						break;
					case 1:
						arms.animation.CrossFade("SMG_idle");
						break;
				}
			}
		}
	}

	public void MeleeAttack()
	{
        if (gamePaused) { return; }
		Debug.Log("Attacking With Melee");
		switch (primaryWeapon.weaponID)
		{
			case 0:
				arms.animation.CrossFade("Melee");
				break;
			case 1:
				arms.animation.CrossFade("SMG_melee");
				break;
		}
		meleeLaunched = true;
	}

	private void LaunchMelee()
	{
		if (!meleeFired)
		{
			Debug.Log("Launching Melee");
			switch (primaryWeapon.weaponID)
			{
				case 0:
					if (arms.animation["Melee"].time > 0.5 &&
						arms.animation["Melee"].time < 0.66)
					{
						// Insert Melee Hit Code Here
						//DecreaseHealth(100f);
                        RaycastHit hit;
                        if (Physics.Linecast(gameObject.transform.forward, (gameObject.transform.forward * 3), out hit))
                        {
                            Debug.Log("Hit Detected: "+hit.collider.gameObject.tag+" | "+hit.collider.gameObject.name);
                            if (hit.collider.gameObject.tag == "Enemy 1" || hit.collider.gameObject.tag == "Enemy 2" || hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Collidable")
                            {
                                Debug.Log("Hit is Enemy");
                                hit.rigidbody.AddForceAtPosition((hit.point - gameObject.transform.forward).normalized * strength, hit.point);
                                hit.collider.gameObject.GetComponent<EnemyConsts>().DecreaseHealth(50);
                            }
                            else { Debug.Log("Hit Not Detected"); }
                        }
						meleeFired = true;
					}
					break;
				case 1:
					if (arms.animation["SMG_melee"].time > 0.5 &&
						arms.animation["SMG_melee"].time < 0.66)
					{
						// Insert Melee Hit Code Here
						//DecreaseHealth(100f);
						meleeFired = true;
					}
					break;
			}
		}
		else
		{
			switch (primaryWeapon.weaponID)
			{
				case 0:
					if (arms.animation["Melee"].time > 0.77)
					{
						meleeFired = false;
						meleeLaunched = false;

						arms.animation.CrossFade("Idle");
					}
					break;
				case 1:
					if (arms.animation["SMG_melee"].time > 0.77)
					{
						meleeFired = false;
						meleeLaunched = false;

						arms.animation.CrossFade("SMG_idle");
					}
					break;
			}
			Debug.Log("Melee Complete");
		}
	}

	public void ReloadWeapon()
	{
        if (gamePaused) { return; }
		switch(primaryWeapon.weaponID)
		{
			case 0:
				arms.animation.CrossFade("Reload");
				break;
			case 1:
				arms.animation.CrossFade("SMG_reload");
				break;
		}

		ammunitionAmount -= primaryWeapon.Reload(ammunitionAmount);
		Debug.Log("Total Ammunition Left: " + ammunitionAmount);
	}
}
