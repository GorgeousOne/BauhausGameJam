using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField] private float lifetime = 5;
	[SerializeField] private float speed = 10;
	 
	private Rigidbody2D _rigid;
	
	private void Awake() {
		_rigid = GetComponent<Rigidbody2D>();
		Destroy(gameObject, lifetime);
	}
		
	public void SetShotDirection(Vector2 direction) {
		_rigid.velocity = speed * direction.normalized;
	}

	void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Dino")
        {
            Destroy(gameObject);
        }   
    }
}