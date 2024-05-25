using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public string nextSceneName;
    public string nowSceneName;
	public FadeSceneLoader fadeSceneLoader;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
			fadeSceneLoader.CallCoroutine();
		}

        if (fadeSceneLoader.isClear) {
			fadeSceneLoader.CallCoroutine();
		}
	}
}
