using UnityEngine;
using System.Collections;

public class UnatroProjectile : MonoBehaviour
{
    public float autoSelfDestroyTime = 5f;
    public Rigidbody explosion;
    
    private float damage;


    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, autoSelfDestroyTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision c)
    {
        Debug.Log("Bullet has collided with: " + c.gameObject.name + ", " + c.gameObject.tag);


        switch (c.gameObject.tag)
        {
            case "Player":
                c.gameObject.audio.Play();
                break;
            case "Enemy 1":
                // Insert Code to Decrease Enemy Health
                c.gameObject.GetComponent<EnemyConsts>().DecreaseHealth(damage);
                //c.gameObject.audio.Play();
                break;
            case "Enemy 2":
                // Insert Code to Decrease enemy Health
                //Destroy(c.gameObject);
                //c.gameObject.audio.Play();
                break;
            case "Collidable":
                break;
            case "Destructable":
            case "Test":
                Destroy(c.gameObject, 1f);
                break;
        }

        // Spawn in Explosion
        Rigidbody temp = Instantiate(explosion, rigidbody.position, rigidbody.rotation) as Rigidbody;
        temp.AddRelativeForce(new Vector3(0, 0, 20 * 20), ForceMode.Impulse);
        //temp.gameObject.GetComponent<AudioSource>().Play();

        // Destroy the Bullet
        Destroy(gameObject);

    }
}
