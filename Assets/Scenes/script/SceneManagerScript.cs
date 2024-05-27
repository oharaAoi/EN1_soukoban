using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public string nextSceneName;
    public string nowSceneName;
	public FadeSceneLoader fadeSceneLoader;

    // sound
    private AudioSource audioSource;

	// Start is called before the first frame update
	void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (nowSceneName == "Select"){
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
			fadeSceneLoader.CallCoroutine();
		}

        if (fadeSceneLoader.isClear) {
			fadeSceneLoader.CallCoroutine();
		}
	}
}
