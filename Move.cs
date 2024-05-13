using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assembly_CSharp {
	internal class Move {
	}
}

public class Move : MonoBehaviour {

	public void MoveTo(Vector3 destination) {
		transform.position = destination; 
	}
}