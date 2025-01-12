using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [SerializeField] GameObject leverDown;
    [SerializeField] GameObject platform;
    [SerializeField] AudioClip acitvationSound;

    // Start is called before the first frame update
    void Start()
    {
        leverDown.SetActive(false);
        platform.GetComponent<WaypointFollower>().TogglePlatform();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        leverDown.SetActive(true);
        gameObject.SetActive(false);
        platform.GetComponent<WaypointFollower>().TogglePlatform();
        leverDown.GetComponent<AudioSource>().PlayOneShot(acitvationSound, AudioListener.volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
