using UnityEngine;
using System.Collections;

public class EndOfLevelTrigger : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("The trigger has collided with: " + c.gameObject.name + ", " + c.gameObject.tag);
        if (c.gameObject.tag == "Player")
        {
            Time.timeScale = 1.0f;
            Destroy(GameObject.Find("GUI").gameObject);
            Destroy(GameObject.Find("Player").gameObject);
            Application.LoadLevel(0);
        }
    }
}
