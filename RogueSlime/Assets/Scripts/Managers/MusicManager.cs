using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
	static MusicManager instance;
	public AudioClip[] level_music;
	private AudioSource music;
	// Use this for initialization
	void Awake()
	{
        if(FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
        }
		DontDestroyOnLoad (gameObject);
	}
}
