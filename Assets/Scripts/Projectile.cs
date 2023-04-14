using System;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField] private float speed = 10;
	
	private Rigidbody2D _rigid;
	
	private void Awake() {
		_rigid = GetComponent<Rigidbody2D>();
	}

	public void SetShotDirection(Vector2 direction) {
		_rigid.velocity = speed * direction.normalized;
	}
}