using UnityEngine;
using System.Collections;

public class EnemyConsts : MonoBehaviour {

	public bool isIdle = true;
	public bool isRunning = false;
	public bool isDead = false;

	public float health = 100f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void DecreaseHealth(float ammount)
    {
        if (health > 0.0f)
        {
            health -= (ammount);
        }
        if (health <= 0)
        {
            health = 0.0f;
            PlayDeadAnimation();
        }
    }

	public void PlayDeadAnimation()
	{
		isIdle = false;
		isRunning = false;
		isDead = true;
        Destroy(gameObject,5);
	}

}
