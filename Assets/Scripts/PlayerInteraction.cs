using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour {
    
    [SerializeField] private float shootInterval = 1;
    [SerializeField] private Projectile bulletPrefab;
    [SerializeField] private Camera gameCam;
    
    private GameInputs _gameInputs;
    private bool _isShootPressed;
    private float _lastShot;
    
    private void Awake() {
        _gameInputs = new GameInputs();
    }

    void Update() {
        _isShootPressed = _gameInputs.Player1.Shoot.IsPressed();
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

        Vector3 mousePos = gameCam.ScreenToWorldPoint(Mouse.current.position.value);
        bullet.SetShotDirection(mousePos - transform.position);
    }
}
