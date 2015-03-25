using UnityEngine;
using System.Collections;

public class GUIScriptChange : MonoBehaviour {
	enum ScreenType { TITLE, OPTIONS, GAME, QUIT };
	

    private ScreenType screen;
    private int skin = 0;
    private static bool created = false;


    public float musicVolume = 1.0f;
    public GUISkin skin1;
    public GUISkin skin2;


	// Use this for initialization
    void Start()
    {
        if (!created)
        {
            //DontDestroyOnLoad(gameObject);
            created = true;
        }
        else
        {
            Destroy(gameObject);
        }

        screen = ScreenType.TITLE;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (skin == 0) { GUI.skin = skin2; }
        else if (skin == 1) { GUI.skin = skin1; }

        switch (screen)
        {
            case ScreenType.TITLE:
                GUI.BeginGroup(new Rect(Screen.width / 2-100, Screen.height/2, 200, 200));

                GUI.Box(new Rect(0, 0, 200, 200), "My Game");
                if (GUI.Button(new Rect(50, 40, 100, 40), "Start"))
                {
                    Application.LoadLevel(1);
                    screen = ScreenType.GAME;
                }
                if (GUI.Button(new Rect(50, 90, 100, 40), "Options"))
                {
                    screen = ScreenType.OPTIONS;
                }
                if (GUI.Button(new Rect(50, 140, 100, 40), "Exit Game"))
                {
                    Application.Quit();
                    screen = ScreenType.QUIT;
                }

                GUI.EndGroup();
                break;
            case ScreenType.OPTIONS:
                GUILayout.BeginArea(new Rect(0, 0, 800, 800));
                GUILayout.BeginVertical();

                GUILayout.Box("Music: ");
                musicVolume = GUILayout.HorizontalSlider(musicVolume, 0.0f, 1.0f);

                if (GUILayout.Button("Change Skin"))
                {
                    if (skin == 0) { skin = 1; }
                    else if (skin == 1) { skin = 0; }
                }

                if (GUILayout.Button("Go Back"))
                {
                    screen = ScreenType.TITLE;

                }


                GUILayout.EndArea();
                break;
            case ScreenType.GAME:
                break;
            case ScreenType.QUIT:
                break;
        }
    }
}
