using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMap : MonoBehaviour
{
	int[,] map;
	private List<int[,]> maps = new List<int[,]>();

	int selectedStage = 0;

	public FadeSceneLoader fadeSceneLoader;

	// Start is called before the first frame update
	void Start()
    {
		// �����̃}�b�v��������
		maps.Add(new int[,] {
			{9,9,9,9,9,9,9},
			{9,1,0,0,0,0,9},
			{9,0,3,0,3,0,9},
			{9,0,0,2,0,0,9},
			{9,0,2,3,2,0,9},
			{9,0,0,0,0,0,9},
			{9,9,9,9,9,9,9},
		});

		maps.Add(new int[,] {
			{9,9,9,9,9,9,9},
			{9,0,1,0,0,0,9},
			{9,0,3,0,3,0,9},
			{9,0,0,2,0,0,9},
			{9,0,2,3,2,0,9},
			{9,0,0,0,0,0,9},
			{9,9,9,9,9,9,9},
		});

		maps.Add(new int[,] {
			{9,9,9,9,9,9,9},
			{9,0,0,1,0,0,9},
			{9,0,3,0,3,0,9},
			{9,0,0,2,0,0,9},
			{9,0,2,3,2,0,9},
			{9,0,0,0,0,0,9},
			{9,9,9,9,9,9,9},
		});

		maps.Add(new int[,] {
			{9,9,9,9,9,9,9},
			{9,0,0,0,4,0,9},
			{9,0,3,0,3,0,9},
			{9,0,0,2,0,0,9},
			{9,0,2,3,2,0,9},
			{9,0,0,0,0,0,9},
			{9,9,9,9,9,9,9},
		});

		 // �I�����ꂽ�X�e�[�W�i��: 0 �Ԗڂ̃X�e�[�W�j
		//SelectStage(selectedStage);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	// map �����J����v���p�e�B
	public int[,] Map {
		get { return map; }
	}

	public void SelectStage(int bossID) {
		if (bossID > 0 && bossID < maps.Count) {
			selectedStage = bossID - 1;
			map = maps[bossID];
			fadeSceneLoader.CallCoroutine();
		} else {
			Debug.LogError("�X�e�[�W�C���f�b�N�X���͈͊O�ł�");
		}
	}
}
