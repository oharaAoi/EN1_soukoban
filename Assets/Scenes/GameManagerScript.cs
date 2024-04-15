using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// �z��̐錾
	int[,] map;

	// GemeObject�̒ǉ�
	public GameObject playerPrefab;


	// Start is called before the first frame update
	void Start() {

		//GameObject instance = Instantiate(
		//	playerPrefab,			 // object
		//	new Vector3(0, 0, 0),	 // pos
		//	Quaternion.identity		 // rotate
		//);

		// �z��̎��̂ƍ쐬��������
		map = new int[,] {
			{1,0,0,0,0},
			{0,0,0,0,0},
			{0,0,0,0,0}
		};

		string debugText = "";

		for(int row = 0; row < map.GetLength(0); row++) {
			for(int col = 0; col < map.GetLength(1); col++) {
				debugText += map[row, col].ToString() + ",";

				// map��1(Player)���m�F
				if (map[row, col] == 1) {
					GameObject instance = Instantiate(
						playerPrefab,			 // object
						new Vector3(col, row, 0),	 // pos
						Quaternion.identity		 // rotate
					);
				}
			}
			// ���s
			debugText += "\n";
		}

		Debug.Log(debugText);

		//PrintArray();

	}

	// Update is called once per frame
	//void Update() {
	//	// ---------------------------------------------------------------
	//	// ���@�E
	//	// ---------------------------------------------------------------
	//	if (Input.GetKeyDown(KeyCode.RightArrow)) {
	//		// player�̈ʒu���擾
	//		int playerIndex = GetPlayerIndex();
	//		// �ړ�����
	//		MoveNumber(1, playerIndex, playerIndex + 1);
	//		// �\�����鏈��
	//		PrintArray();
	//	}

	//	// ---------------------------------------------------------------
	//	// �� ��
	//	// ---------------------------------------------------------------
	//	if (Input.GetKeyDown(KeyCode.LeftArrow)) {
	//		// player�̈ʒu���擾
	//		int playerIndex = GetPlayerIndex();
	//		// �ړ�����
	//		MoveNumber(1, playerIndex, playerIndex - 1);
	//		// �\�����鏈��
	//		PrintArray();
	//	}
	//}

	////=================================================================================================================
	////	���@�z���\������֐�
	////=================================================================================================================
	//void PrintArray() {
	//	// ������̐錾�Ə�����
	//	string debugText = "";

	//	for (int oi = 0; oi < map.Length; oi++) {
	//		// �v�f���o��
	//		debugText += map[oi].ToString() + ",";
	//	}

	//	// ����������������o��
	//	Debug.Log(debugText);
	//}

	////=================================================================================================================
	////	���@1�������Ă���v�f��Ԃ��֐�
	////=================================================================================================================
	//int GetPlayerIndex() {
	//	for (int oi = 0; oi < map.Length; oi++) {
	//		if (map[oi] == 1) {
	//			return oi;
	//		}
	//	}

	//	return -1;
	//}

	////=================================================================================================================
	////	���@���E�l���ǂ�����Ԃ��֐�
	////=================================================================================================================
	//bool MoveNumber(int number, int moveFrom, int moveTo) {
	//	// �ړ��悪�͈͊O�Ȃ�ڑ��s��
	//	if (moveTo < 0 || moveTo >= map.Length) {
	//		return false; // �����Ȃ�
	//	}

	//	// �ړ����2(��)����������
	//	if (map[moveTo] == 2) {
	//		// �ړ��������v�Z(�ǂ�����ǂ��܂�)
	//		int velocity = moveTo - moveFrom;
	//		// ���̈ړ���������ɐ�֔���i�܂���
	//		// �ړ���ɔ������������͔����ړ����i�܂���
	//		bool success = MoveNumber(2, moveTo, moveTo + velocity);
	//		// �������̈ړ����o���Ȃ�������,�v���C���[���ړ��ł��Ȃ�
	//		if (!success) {
	//			return false;
	//		}
	//	}

	//	// �ς��Ȃ�����(�ړ����Ĕ�����������ċA���ňړ�)
	//	map[moveTo] = number;
	//	map[moveFrom] = 0;
	//	return true;
	//}
}