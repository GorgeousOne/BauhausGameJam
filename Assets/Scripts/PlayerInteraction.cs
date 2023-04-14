using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour {
    
    [SerializeField] private float shootInterval = 1;
    [SerializeField] private Projectile bulletPrefab;
    
    private GameInputs _gameInputs;
    private bool _isShootPressed;
    private float _lastShot;
    
    private void Awake() {
        _gameInputs = new GameInputs();
        _gameInputs.Enable();
    }

    void Update() {
        _isShootPressed = _gameInputs.Player1.Shoot.IsPressed();
        _shootLogic();
    }

    private void _shootLogic() {
        if (!_isShootPressed) {
            return;
        }
        if (Time.time - _lastShot < shootInterval) {
            return;
        }
        _shoot();
    }

    private void _shoot() {
        _lastShot = Time.time;
        Projectile bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);

        //Vector2 mouseScreenPos = _gameInputs.Player1.Aim.ReadValue<Vector2>();
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Debug.Log(mouseScreenPos);
        Debug.Log(Camera.main);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        bullet.SetShotDirection(mousePos - transform.position);
    }
}
