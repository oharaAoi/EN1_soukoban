using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCheck : MonoBehaviour
{
	public AudioSource audioSource;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckStopSound() {
        if (!audioSource.isPlaying) {
			return true;
		}

        return false;
	}
}
