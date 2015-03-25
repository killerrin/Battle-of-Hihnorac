using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public int weaponID;
	public string weaponType;

	public float speed;
	public float speedMultiplier;
	public float projectileMass;
	public float weaponDamage;

	public int clipSize;
	public int ammoInClip;

	public int rateOfFire;
	public int rateOfFireCtr;

	public bool isReloading = false;
	public float totalReloadTime;
	private float reloadingCtr;

	//public GameObject gameObject;

	public Weapon(int id, string _weaponType, float _speed, float _speedMultipier, float _projectileMass, float _weaponDamage, int _clipSize, int _rateoffire, int _totalReloadTime)
	{
		weaponID = id;
		weaponType = _weaponType;    
		speed = _speed;
		speedMultiplier = _speedMultipier;
		projectileMass =_projectileMass;
		weaponDamage = _weaponDamage;
		clipSize =_clipSize;
		ammoInClip = _clipSize;
		rateOfFire = _rateoffire;
		rateOfFireCtr = 0;
		totalReloadTime = _totalReloadTime;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	public void Update(){

	}

	public bool FireWeapon(Rigidbody projectile, Transform projectileSpawnPoint)
	{
		if (!isReloading)
		{	
			Rigidbody temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as Rigidbody;
			temp.GetComponent<Rigidbody>().mass = projectileMass;
			if (weaponID == 2) { temp.GetComponent<UnatroProjectile>().Damage = weaponDamage; }
			else { temp.GetComponent<Projectile>().Damage = weaponDamage; }

			//temp.AddForce(new Vector3(0, 0, speed), ForceMode.Impulse);
			temp.AddRelativeForce(new Vector3(0, 0, speed * speedMultiplier), ForceMode.Impulse);

			//Debug.Log(projectileSpawnPoint.root.name);
			Physics.IgnoreCollision(temp.collider, projectileSpawnPoint.root.collider);

			--ammoInClip;
			return true;
		}
		return false;
	}

	public void ReloadTimerUpdate()
	{
		if (isReloading)
		{
			if (reloadingCtr >= totalReloadTime) { isReloading = false; reloadingCtr = 0f; }
			else { reloadingCtr += 1 * Time.deltaTime; }
		}
	}

	public int Reload(int totalAmmunition )
	{
		if (isReloading || ammoInClip == clipSize) return -999;

		isReloading = true;

		if (totalAmmunition >= (clipSize - ammoInClip))
		{
			//Debug.Log(clipSize - ammoInClip);
			int ammoUsedToReload = (clipSize - ammoInClip);
			ammoInClip += ammoUsedToReload;

			Debug.Log(ammoUsedToReload);
			return ammoUsedToReload;
		}
		else
		{
			ammoInClip += totalAmmunition;
			return totalAmmunition;
		}
	}

	public float Reload(float totalAmmunition)
	{
		if (isReloading || ammoInClip == clipSize) return -999;

		isReloading = true;

		if (totalAmmunition >= (clipSize - ammoInClip))
		{
			//Debug.Log(clipSize - ammoInClip);
			int ammoUsedToReload = (clipSize - ammoInClip);
			ammoInClip += ammoUsedToReload;

			//Debug.Log(ammoUsedToReload);
			return ammoUsedToReload;
		}
		else
		{
			ammoInClip += (int)totalAmmunition;
			return totalAmmunition;
		}
	}

	public void ResetReloadingCtr() { reloadingCtr = 0f; }
}
