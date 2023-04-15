
using UnityEngine;

public class BasicFollowDino: DinoAI {
	protected override void MovementStage1() {
		_moveInput = getTargetDist().normalized;
	}
}
