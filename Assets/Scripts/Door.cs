using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{

    private int state = 0;
    bool doorClosed = true;
    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (!doorClosed)
        {
            StartCoroutine(closeDoorAfterX());
        }
    }

    private IEnumerator closeDoorAfterX()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(PlayDoorAnimation());
    }

    // Update is called once per frame
    public IEnumerator PlayDoorAnimation()
    {
        switch (state)
        {
            case 0:
                if (!animation.isPlaying) animation.Play("DoorOpen");
                yield return new WaitForSeconds(animation.GetClip("DoorOpen").length);
                state = 1;
                doorClosed = false;
                break;
            case 1:
                if (!animation.isPlaying) animation.Play("DoorClose");
                yield return new WaitForSeconds(animation.GetClip("DoorClose").length);
                state = 0;
                doorClosed = true;
                break;
        }
    }
}
