using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// GemeObject�̒ǉ�
	public GameObject playerPrefab;
	public GameObject boxPrefab;


	// prefub�ł͂Ȃ�
	public GameObject clearText;
	public GameObject goalsObj;

	// �z��̐錾
	int[,] map;
	GameObject[,] field;// �Q�[���Ǘ��p�̔z��


	// Start is called before the first frame update
	void Start() {

		//GameObject instance = Instantiate(
		//	playerPrefab,			 // object
		//	new Vector3(0, 0, 0),	 // pos
		//	Quaternion.identity		 // rotate
		//);

		// �z��̎��̂ƍ쐬��������
		map = new int[,] {
			{0,0,0,0,0},
			{0,3,1,3,0},
			{0,0,2,0,0},
			{0,2,3,2,0},
			{0,0,0,0,0},
		};

		field = new GameObject[
			map.GetLength(0),
			map.GetLength(1)
		];

		string debugText = "";

		for (int row = 0; row < map.GetLength(0); row++) {
			for (int col = 0; col < map.GetLength(1); col++) {
				debugText += map[row, col].ToString() + ",";

				// map��1(Player)���m�F
				if (map[row, col] == 1) {
					field[row, col] = Instantiate(
						playerPrefab,            // object
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row , 0),     // pos(�J�����̒��S�ɗ���悤�ȏ��������Ă���)
						//new Vector3(col, map.GetLength(0) -row , 0),     // pos
						Quaternion.identity      // rotate
					);
				}else if(map[row, col] == 2) {
					field[row, col] = Instantiate(
						boxPrefab,            // object
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0),     // pos
						//new Vector3(col, map.GetLength(0) -row , 0),     // pos
						Quaternion.identity      // rotate
					);
				}else if(map[row, col] == 3) {
					GameObject instantiate = Instantiate(
						goalsObj,
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0.01f),
						Quaternion.identity
					);
				}
			}
			// ���s
			debugText += "\n";
		}

		Debug.Log(debugText);

	}

	// Update is called once per frame
	void Update() {
		// ---------------------------------------------------------------
		// ���@�E
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			// player�̈ʒu���擾
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus =new Vector2Int ( playerIndex.x + 1, playerIndex.y );
			// �ړ�����
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// �N���A����
			if (IsCleard()) {
				clearText.SetActive(true);
			}
		}

		// ---------------------------------------------------------------
		// �� ��
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			// player�̈ʒu���擾
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x - 1, playerIndex.y);
			// �ړ�����
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// �N���A����
			if (IsCleard()) {
				clearText.SetActive(true);
			}
		}

		// ---------------------------------------------------------------
		// ���@��
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			// player�̈ʒu���擾
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x, playerIndex.y - 1);
			// �ړ�����
			MoveNumber("Player", playerIndex, playerIndexPlus);

			if (IsCleard()) {
				clearText.SetActive(true);
			}
		}

		// ---------------------------------------------------------------
		// ���@��
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			// player�̈ʒu���擾
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x, playerIndex.y + 1);
			// �ړ�����
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// �N���A����
			if (IsCleard()) {
				clearText.SetActive(true);
			}
		}
	}

	//=================================================================================================================
	//	���@1�������Ă���v�f��Ԃ��֐�
	//=================================================================================================================
	Vector2Int GetPlayerIndex() {
		for (int row = 0; row < map.GetLength(0); row++) {
			for (int col = 0; col < map.GetLength(1); col++) {
				// �܂���null�`�F�b�N
				if (field[row, col] == null) {
					continue;
				}
				// null�łȂ��̂Ń^�O�`�F�b�N
				if (field[row, col].tag == "Player") {
					return new Vector2Int(col, row);
				}
			}
		}
		// ������Ȃ������̂�-1��Ԃ�
		return new Vector2Int(-1, -1);
	}

	//=================================================================================================================
	//	���@���E�l���ǂ�����Ԃ��֐�
	//=================================================================================================================
	bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo) {
		// �ړ��悪�͈͊O�Ȃ�ڑ��s��
		if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) {
			return false; // �����Ȃ�
		}

		if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) {
			return false; // �����Ȃ�
		}

		// �ړ����2(��)����������
		if (field[moveTo.y, moveTo.x] != null) {
			if (field[moveTo.y, moveTo.x].tag == "Box") {
				// �ړ��������v�Z(�ǂ�����ǂ��܂�)
				Vector2Int velocity = moveTo - moveFrom;
				// ���̈ړ���������ɐ�֔���i�܂���
				// �ړ���ɔ������������͔����ړ����i�܂���
				bool success = MoveNumber(tag, moveTo, moveTo + velocity);
				// �������̈ړ����o���Ȃ�������,�v���C���[���ړ��ł��Ȃ�
				if (!success) {
					return false; 
				}
			}
		}

		// moveFrom�ɂ���Q�[���I�u�W�F�N�g�ɍ��W�𑀍삷��
		field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x - map.GetLength(1) / 2, map.GetLength(0) - moveTo.y , 0);

		// �ς��Ȃ�����(�ړ����Ĕ�����������ċA���ňړ�)
		field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
		field[moveFrom.y, moveFrom.x] = null;

		return true;
	}

	//=================================================================================================================
	//	���@�S�[������
	//=================================================================================================================
	bool IsCleard() {
		List<Vector2Int> goals = new List<Vector2Int>();

		// map����3(�S�[���̈ʒu)�𒊏o����
		for(int row = 0; row < map.GetLength(0); row++) {
			for(int col = 0; col < map.GetLength(1); col++) {
				if (map[row, col] == 3) {
					goals.Add(new Vector2Int(col, row));
				}
			}
		}

		// ���o�����ʒu��field�z��ɑ�����A�����ɔ������邩�𔻒�
		for(int index = 0; index < goals.Count; index++) {
			GameObject clear = field[goals[index].y, goals[index].x];
			if(clear == null || clear.tag != "Box") {
				// 1�ł��Ȃ�������A�E�g
				return false;
			}
		}

		// 
		return true;
	}

}
