
using UnityEngine;

public class RunUpDino: DinoAI {

    [SerializeField] private float prepTime = 3;
    [SerializeField] private float moveTime = 3;
    [SerializeField] private float wiggleDist = .5f;
    
    private bool _isRunningUp = true;
    
    private float _prepStart;
    private Vector2 _prepPos;

    private float _moveStart;
    private Vector2 _moveDir;

    private bool _isWigglingForward;
    
    new void Start() {
        base.Start();
        _moveDir = getTargetDist().normalized;
        _prepPos = transform.position;
    }
    
    protected override void MovementStage1() {
        if (_isRunningUp) {
            
            if (Time.time > _moveStart + moveTime) {
                _isRunningUp = false;
                _prepStart = Time.time;
                _prepPos = transform.position;
                _moveDir = getTargetDist().normalized;
            }
        }
        else {
            transform.position = _prepPos + (_isWigglingForward ? 1 : -1) * wiggleDist * _moveDir;
            _isWigglingForward = !_isWigglingForward;
            
            if (Time.time > _prepStart + prepTime) {
                _isRunningUp = true;
                _moveStart = Time.time;
                _rigid.velocity = speed * _moveDir;
            }
        }
    }
}
