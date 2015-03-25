using UnityEngine;
using System.Collections;

public class GUITextureChange : MonoBehaviour {

	public Texture2D onOver;
	public Texture2D onExit;

	public AudioSource clickSound;

	public int guiID;

	void OnMouseEnter()
	{
		if (onOver)
			guiTexture.texture = onOver;
		else
			Debug.Log("onOver not set");
	}

	void OnMouseDown()
	{
		clickSound.Play();
		switch (guiID)
		{
			case 0:
				Application.LoadLevel(1);
				break;
			case 1:
				Application.Quit();
				Debug.Log("Quitting..");
				break;
            case 2:
                GameObject.Find("Player").GetComponent<PlayerConsts>().PauseGame();
                break;
            case 3:
                Time.timeScale = 1.0f;
                Destroy(GameObject.Find("GUI").gameObject);
                Destroy(GameObject.Find("Player").gameObject);
                Application.LoadLevel(0);
                break;
		}
	}

	void OnMouseExit()
	{
		if (onExit)
			guiTexture.texture = onExit;
		else
			Debug.Log("onExit not set");
	}
}
