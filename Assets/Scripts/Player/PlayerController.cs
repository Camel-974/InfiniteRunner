using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum LifeState
    {
        Alive,
        Invincible,
        Dead
    }

    [Header("Lane Settings")]
    [SerializeField] private float _laneWidth = 2f;
    [SerializeField] private float _laneSwitchSpeed = 5f;
    [SerializeField] private int _laneCount = 5;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 5f;

    [Header("Charge Settings")]
    [SerializeField] private float _chargeDuration = 0.5f;

    [Header("Mega Charge Settings")]
    [SerializeField] private float _megaChargeDuration = 3f;
    [SerializeField] private float _megaChargeGaugeMax = 100f;
    [SerializeField] private float _megaChargeGaugeFillRate = 10f;

    // Public properties accessible by states
    public StateMachine StateMachine { get; private set; }
    public RunningState RunningState { get; private set; }
    public JumpingState JumpingState { get; private set; }
    public ChargingState ChargingState { get; private set; }
    public MegaChargeState MegaChargeState { get; private set; }
    public float ChargeDuration => _chargeDuration;
    public float MegaChargeDuration => _megaChargeDuration;

    // Gauge
    public float MegaChargeGauge { get; private set; }
    public float MegaChargeGaugeMax => _megaChargeGaugeMax;
    public bool IsMegaChargeReady => MegaChargeGauge >= _megaChargeGaugeMax;

    // Life state
    public LifeState lifeState = LifeState.Alive;

    // Lane management
    private int _currentLane;
    private float _targetXPosition;
    private bool _canSwitchLane = true;

    // Components
    private Rigidbody _rigidbody;

    // Ground detection
    private bool _isGrounded = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StateMachine = GetComponent<StateMachine>();

        RunningState = new RunningState(this);
        JumpingState = new JumpingState(this);
        ChargingState = new ChargingState(this);
        MegaChargeState = new MegaChargeState(this);
    }

    private void Start()
    {
        _currentLane = _laneCount / 2;
        _targetXPosition = GetLaneXPosition(_currentLane);
        StateMachine.ChangeState(RunningState);
    }

    private void Update()
    {
        // Smooth lane movement
        if (_canSwitchLane)
        {
            Vector3 targetPosition = new Vector3(
                _targetXPosition,
                transform.position.y,
                transform.position.z
            );

            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                _laneSwitchSpeed * Time.deltaTime
            );
        }

        // Fill mega charge gauge over time
        if (StateMachine.CurrentState != MegaChargeState)
        {
            MegaChargeGauge = Mathf.Min(
                MegaChargeGauge + _megaChargeGaugeFillRate * Time.deltaTime,
                _megaChargeGaugeMax
            );
        }
    }

    // ----- PUBLIC FUNCTIONS CALLED BY STATES -----

    public void Jump()
    {
        _rigidbody.linearVelocity = new Vector3(
            _rigidbody.linearVelocity.x,
            _jumpForce,
            _rigidbody.linearVelocity.z
        );
    }

    public void Charge()
    {
        Debug.Log("Bessy is charging !");
    }

    public void StartMegaCharge()
    {
        // Empty the gauge
        MegaChargeGauge = 0f;

        // Prevent lane switching
        _canSwitchLane = false;

        // Make invincible
        lifeState = LifeState.Invincible;

        // Grow Bessy to 3 lanes wide
        transform.localScale = new Vector3(3f, 1f, 1f);

        Debug.Log("Mega Charge started !");
    }

    public void EndMegaCharge()
    {
        // Allow lane switching again
        _canSwitchLane = true;

        // Back to normal size
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Back to alive
        lifeState = LifeState.Alive;

        Debug.Log("Mega Charge ended !");
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    // ----- TAKE DAMAGE -----

    public void TakeDamage()
    {
        if (lifeState != LifeState.Alive) return;

        Debug.Log("Player taking damage !");
        GameManager.Instance.LoseLife();

        lifeState = LifeState.Invincible;
        Invoke("BecomeVulnerable", 2f);
    }

    private void BecomeVulnerable()
    {
        lifeState = LifeState.Alive;
    }

    // ----- INPUT FUNCTIONS -----

    private void OnMove(InputValue value)
    {
        float direction = value.Get<Vector2>().x;

        if (direction > 0) SwitchLane(1);
        else if (direction < 0) SwitchLane(-1);
    }

    private void OnJump(InputValue value)
    {
        if (StateMachine.CurrentState == JumpingState) return;
        if (!_isGrounded) return;
        if (StateMachine != null)
            StateMachine.ChangeState(JumpingState);
    }

    private void OnCharge(InputValue value)
    {
        if (StateMachine.CurrentState == ChargingState) return;
        if (StateMachine != null)
            StateMachine.ChangeState(ChargingState);
    }

    private void OnMegaCharge(InputValue value)
    {
        // Can only use mega charge if gauge is full
        if (!IsMegaChargeReady) return;
        if (StateMachine.CurrentState == MegaChargeState) return;
        if (StateMachine != null)
            StateMachine.ChangeState(MegaChargeState);
    }

    // ----- COLLISION -----

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            _isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            _isGrounded = false;
    }

    // ----- PRIVATE HELPER FUNCTIONS -----

    private void SwitchLane(int direction)
    {
        if (!_canSwitchLane) return;

        int newLane = Mathf.Clamp(_currentLane + direction, 0, _laneCount - 1);
        _currentLane = newLane;
        _targetXPosition = GetLaneXPosition(_currentLane);
    }

    private float GetLaneXPosition(int lane)
    {
        float totalWidth = (_laneCount - 1) * _laneWidth;
        return (lane * _laneWidth) - (totalWidth / 2);
    }

    // ----- GIZMOS -----

    private void OnDrawGizmos()
    {
        float totalWidth = (_laneCount - 1) * _laneWidth;

        for (int i = 0; i < _laneCount; i++)
        {
            float xPos = (i * _laneWidth) - (totalWidth / 2);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(
                new Vector3(xPos, 0.1f, transform.position.z),
                new Vector3(_laneWidth, 0.1f, 30f)
            );
        }
    }
}

