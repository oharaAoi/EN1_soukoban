using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManagerScript : MonoBehaviour {

	// GemeObjectの追加
	public GameObject playerPrefab;
	public GameObject boxPrefab;
	public GameObject particlePrefab;
	public GameObject wallPrefab;

	// prefabではない
	public GameObject clearText;
	public GameObject goalsObj;
	public GameObject backGroundObj;
	public FadeSceneLoader fadeSceneLoader;
	//private StageMap stageMap;

	private MainSound mainSound;
	public AudioSource clearSound;
	public AudioSource playSound;

	// 配列の宣言
	int[,] map;
	GameObject[,] field;// ゲーム管理用の配列

	// fieldをスタックする変数
	Stack<GameObjectState[,]> fieldStack = new Stack<GameObjectState[,]>();

	// クリア後にフレームをカウントする変数
	int clearedFrameCount = 0;
	// クリアしたかのフラグ
	bool isClear = false;
	// 音が終わったかの判定
	bool isClearSoundFinish = false;

	// Start is called before the first frame update
	void Start() {
		// フルスクリーンにする
		Screen.SetResolution(1280, 720, true);

		mainSound = FindObjectOfType<MainSound>();
		// 音を鳴らす
		playSound.Play();

		//map = StageMap.Map;

		map = (new int[,] {
			{9,9,9,9,9,9,9},
			{9,1,0,0,0,0,9},
			{9,0,3,0,3,0,9},
			{9,0,0,2,0,0,9},
			{9,0,2,3,2,0,9},
			{9,0,0,0,0,0,9},
			{9,9,9,9,9,9,9},
		});

		//Debug.Assert(map == null, "mapがnullでした");

		field = new GameObject[
			map.GetLength(0),
			map.GetLength(1)
		];

		string debugText = "";

		for (int row = 0; row < map.GetLength(0); row++) {
			for (int col = 0; col < map.GetLength(1); col++) {
				debugText += map[row, col].ToString() + ",";

				// mapの1(Player)を確認
				if (map[row, col] == 1) {
					field[row, col] = Instantiate(
						playerPrefab,            // object
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0),     // pos(カメラの中心に来るような処理をしている)
																									//new Vector3(col, map.GetLength(0) -row , 0),     // pos
						Quaternion.LookRotation(new Vector3(0.0f, -1.0f, 0.0f), Vector3.back)      // rotate
					);
				} else if (map[row, col] == 2) {
					field[row, col] = Instantiate(
						boxPrefab,            // object
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0),     // pos
																									//new Vector3(col, map.GetLength(0) -row , 0),     // pos
						Quaternion.identity      // rotate
					);
				} else if (map[row, col] == 3) {
					GameObject instantiate = Instantiate(
						goalsObj,
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0.01f),
						Quaternion.identity
					);
				} else if (map[row, col] == 9) {
					field[row, col] = Instantiate(
						wallPrefab,
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0),
						Quaternion.identity
					);
				}
			}
			// 改行
			debugText += "\n";
		}

		// backGround
		GameObject backGroundInstance = Instantiate(
				backGroundObj,
				new Vector3(0.0f, 0.0f, 0.5f),
				Quaternion.identity
		);

		Debug.Log(debugText);
	}

	// Update is called once per frame
	void Update() {
		// ---------------------------------------------------------------
		// ↓　右
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			// playerの位置を取得
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x + 1, playerIndex.y);
			// スタックに移動前の情報を格納しておく
			GameObject[,] fieldCopy = DeepCopy(field);
			fieldStack.Push(GetFieldState(fieldCopy));

			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// クリア判定
			if (IsCleared()) {
				isClear = true;
				clearText.SetActive(true);
			}
		}

		// ---------------------------------------------------------------
		// ↓ 左
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			// playerの位置を取得
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x - 1, playerIndex.y);
			// スタックに移動前の情報を格納しておく
			GameObject[,] fieldCopy = DeepCopy(field);
			fieldStack.Push(GetFieldState(fieldCopy));
			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// クリア判定
			if (IsCleared()) {
				isClear = true;
				clearText.SetActive(true);
			}
		}

		// ---------------------------------------------------------------
		// ↓　上
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			// playerの位置を取得
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x, playerIndex.y - 1);
			// スタックに移動前の情報を格納しておく
			GameObject[,] fieldCopy = DeepCopy(field);
			fieldStack.Push(GetFieldState(fieldCopy));
			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			if (IsCleared()) {
				isClear = true;
				clearText.SetActive(true);
			}
		}

		// ---------------------------------------------------------------
		// ↓　下
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			// playerの位置を取得
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x, playerIndex.y + 1);
			// スタックに移動前の情報を格納しておく
			GameObject[,] fieldCopy = DeepCopy(field);
			fieldStack.Push(GetFieldState(fieldCopy));
			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// クリア判定
			if (IsCleared()) {
				isClear = true;
				clearText.SetActive(true);
			}
		}

		// ---------------------------------------------------------------
		// ↓　Undo
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.Z)) {
			if (fieldStack.Count <= 0) {
				return;
			}

			if (field != null) {
				// field の要素を順番に破棄する
				foreach (GameObject obj in field) {
					if (obj != null) {
						// GameObject を破棄する
						obj.SetActive(false);
					}
				}
			}

			// fieldStack から配列をポップする
			GameObjectState[,] tempField = fieldStack.Pop();

			// 新しい配列を作成して元の配列をコピーする
			field = new GameObject[tempField.GetLength(0), tempField.GetLength(1)];
			for (int row = 0; row < tempField.GetLength(0); row++) {
				for (int col = 0; col < tempField.GetLength(1); col++) {
					if (tempField[row, col] != null) {
						if (tempField[row, col].tag == "Player") {
							field[row, col] = Instantiate(
								playerPrefab,
								tempField[row, col].position,
								tempField[row, col].rotation
							);
						} else if (tempField[row, col].tag == "Box") {
							field[row, col] = Instantiate(
								boxPrefab,
								tempField[row, col].position,
								tempField[row, col].rotation
							);
						} else if (tempField[row, col].tag == "Goal") {
							Instantiate(
								goalsObj,
								tempField[row, col].position,
								tempField[row, col].rotation
							);
						} else if (tempField[row, col].tag == "Wall") {
							field[row, col] = Instantiate(
								wallPrefab,
								tempField[row, col].position,
								tempField[row, col].rotation
							);
						}
					}
				}
			}

		}

		// ---------------------------------------------------------------
		// ↓　クリア時にシーンを切り変える
		// ---------------------------------------------------------------
		if (isClear) {
			StartCoroutine(CheckClearSoundFinish());

			if (isClearSoundFinish) {
				fadeSceneLoader.isClear = true;
				fadeSceneLoader.CallCoroutine();

				mainSound.PlaySound();
			}
		}

		//=================================================================================================================
		//	↓　1が入っている要素を返す関数
		//=================================================================================================================
		Vector2Int GetPlayerIndex() {
			for (int row = 0; row < map.GetLength(0); row++) {
				for (int col = 0; col < map.GetLength(1); col++) {
					// まずはnullチェック
					if (field[row, col] == null) {
						continue;
					}
					// nullでないのでタグチェック
					if (field[row, col].tag == "Player") {
						return new Vector2Int(col, row);
					}
				}
			}
			// 見つからなかったので-1を返す
			return new Vector2Int(-1, -1);
		}

		//=================================================================================================================
		//	↓　限界値かどうかを返す関数
		//=================================================================================================================
		bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo) {
			// 移動先が範囲外なら移送不可
			if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) {
				return false; // 動けない
			}

			if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) {
				return false; // 動けない
			}

			// 動く先が壁だったら
			if (field[moveTo.y, moveTo.x] != null) {
				if (field[moveTo.y, moveTo.x].tag == "Wall") {
					return false;
				}
			}

			// 移動先に2(箱)があったら
			if (field[moveTo.y, moveTo.x] != null) {
				if (field[moveTo.y, moveTo.x].tag == "Box") {
					// 移動方向を計算(どこからどこまで)
					Vector2Int velocity = moveTo - moveFrom;
					// 箱の移動処理さらに先へ箱を進ませる
					// 移動先に箱があった時は箱も移動分進ませる
					bool success = MoveNumber(tag, moveTo, moveTo + velocity);
					// もし箱の移動が出来なかったら,プレイヤーも移動できない
					if (!success) {
						return false;
					}
				}
			}

			// moveFromにあるゲームオブジェクトに座標を操作する
			//field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x - map.GetLength(1) / 2, map.GetLength(0) - moveTo.y , 0);
			//field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x - map.GetLength(1) / 2, map.GetLength(0) / 2 - moveTo.y, 0);

			Vector3 moveFromPosition = new Vector3(moveFrom.x - map.GetLength(1) / 2, -moveFrom.y + map.GetLength(0) / 2, 0);
			Vector3 moveToPosition = new Vector3(moveTo.x - map.GetLength(1) / 2, -moveTo.y + map.GetLength(0) / 2, 0);
			field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
			// particleの生成.
			CreateParticle(moveToPosition);

			// 変わらない処理(移動して箱があったら再帰内で移動)
			field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
			field[moveTo.y, moveTo.x].transform.rotation = Quaternion.LookRotation(moveToPosition - moveFromPosition, Vector3.back);
			field[moveFrom.y, moveFrom.x] = null;

			return true;
		}

		//=================================================================================================================
		//	↓　ゴール判定
		//=================================================================================================================
		bool IsCleared() {
			//Debug.Assert(map == null, "mapがnullでした");

			List<Vector2Int> goals = new List<Vector2Int>();

			// mapから3(ゴールの位置)を抽出する
			for (int row = 0; row < map.GetLength(0); row++) {
				for (int col = 0; col < map.GetLength(1); col++) {
					if (map[row, col] == 3) {
						goals.Add(new Vector2Int(col, row));
					}
				}
			}

			// 抽出した位置をfield配列に代入し、そこに箱があるかを判定
			for (int index = 0; index < goals.Count; index++) {
				GameObject clear = field[goals[index].y, goals[index].x];
				if (clear == null || clear.tag != "Box") {
					// 1つでもなかったらアウト
					return false;
				}
			}

			playSound.Stop();
			// 
			return true;
		}

		//=================================================================================================================
		//	↓　particleの生成
		//=================================================================================================================
		void CreateParticle(Vector3 position) {
			for (int oi = 0; oi < 7; oi++) {
				GameObject instance = Instantiate(
					particlePrefab,            // object
					position,                  // pos
					Quaternion.identity        // rotate
				);
			}
		}

		//=================================================================================================================
		//	↓　Fieldをコピーする
		//=================================================================================================================
		GameObject[,] DeepCopy(GameObject[,] original) {
			int rows = original.GetLength(0);
			int cols = original.GetLength(1);
			GameObject[,] copy = new GameObject[rows, cols];

			for (int row = 0; row < rows; row++) {
				for (int col = 0; col < cols; col++) {
					copy[row, col] = original[row, col]; // 深いコピーの場合、Instantiateなどの方法が必要
				}
			}
			return copy;
		}

	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public IEnumerator CheckClearSoundFinish() {
		yield return new WaitWhile(() => clearSound.isPlaying);

		clearedFrameCount++;

		if (clearedFrameCount > 500) {
			isClearSoundFinish = true;
		}
	}

	GameObjectState[,] GetFieldState(GameObject[,] field) {
		int rows = field.GetLength(0);
		int cols = field.GetLength(1);
		GameObjectState[,] state = new GameObjectState[rows, cols];

		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				if (field[row, col] != null) {
					state[row, col] = new GameObjectState(
						field[row, col].tag,
						field[row, col].transform.position,
						field[row, col].transform.rotation
					);
				}
			}
		}
		return state;
	}

	[System.Serializable]
	public class GameObjectState {
		public string tag;
		public Vector3 position;
		public Quaternion rotation;

		public GameObjectState(string tag, Vector3 position, Quaternion rotation) {
			this.tag = tag;
			this.position = position;
			this.rotation = rotation;
		}
	}
}
