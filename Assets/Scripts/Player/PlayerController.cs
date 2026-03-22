using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Lane Settings")] 
    [SerializeField] private float _laneWidth = 2f;
    [SerializeField] private float _laneSwitchSpeed = 10f;
    [SerializeField] private int _laneCount = 5;
    
    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 8f;

    [Header("Charge Settings")] 
    [SerializeField] private float _chargeDuration = 0.5f;
    
    // accessible states
    public StateMachine StateMachine { get; private set; }
    public RunningState RunningState { get; private set; }
    public JumpingState JumpingState { get; private set; }
    public ChargingState ChargingState { get; private set; }
    public float ChargeDuration => _chargeDuration;
    
    // Lane managment
    private int _currentLane;
    private float _targetXposition;
    
    // Components
    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        
        StateMachine = GetComponent<StateMachine>();

        RunningState = new RunningState(this);
        JumpingState = new JumpingState(this);
        ChargingState = new ChargingState(this);
    }

    private void Start()
    {
        _currentLane = _laneCount / 2;
        _targetXposition = GetLaneXPosition(_currentLane);
        
        StateMachine.ChangeState(RunningState);
    }

    private void Update()
    {
        Vector3 targetposition =  new Vector3(_targetXposition, transform.position.y, transform.position.z);
        
        transform.position = Vector3.Lerp(transform.position, targetposition, _laneSwitchSpeed * Time.deltaTime);
    }
    
    // ----- FUNCTIONS CALLED BY STATES -----

    public void Jump()
    {
        _rigidBody.linearVelocity = new Vector3(_rigidBody.linearVelocity.x, _jumpForce, _rigidBody.linearVelocity.z);
    }

    public void Charge()
    {
        //todo : destroy obstacles
        Debug.Log("Charge");
    }
    
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.down, 1.1f);
    }
    
    // ----- INPUTS FUNCTIONS -----

    private void OnMove(InputValue value)
    {
        float direction = value.Get<Vector2>().x;

        if (direction > 0) SwitchLane(1);
        else if (direction < 0) SwitchLane(-1);
    }

    private void OnJump(InputValue value)
    {
        if (StateMachine != null)
            StateMachine.ChangeState(JumpingState);
    }

    private void OnCharge(InputValue value)
    {
        if (StateMachine != null)
            StateMachine.ChangeState(ChargingState);
    }
    
    // ----- SWITCH LANE -----

    private void SwitchLane(int direction)
    {
        int newLane = Mathf.Clamp(_currentLane + direction, 0, _laneCount - 1);
        _currentLane = newLane;
        _targetXposition = GetLaneXPosition(_currentLane);
    }

    private float GetLaneXPosition(int lane)
    {
        float totalWidth = (_laneCount - 1) * _laneWidth;
        return (lane * _laneWidth) - (totalWidth / 2);
    }


}
