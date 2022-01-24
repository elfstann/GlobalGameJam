using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour
{
    public UnityEvent onCutsceneStarted = new UnityEvent();
    public UnityEvent onCutsceneEnded= new UnityEvent();
    [SerializeField] PlayableDirector director;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        onCutsceneStarted.Invoke();
        GetComponent<BoxCollider2D>().enabled = false;
        director.stopped += OnDirectorStopped;
        director.Play();
        Debug.Log("Cutscene started");
    }
    private void OnDirectorStopped(PlayableDirector dir)
    {
        if (dir.duration >= dir.time) 
        {
            onCutsceneEnded.Invoke();
            Debug.Log("Cutscene Ended");
        }
    }
}
