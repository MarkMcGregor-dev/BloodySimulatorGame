using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerCollectDelegate();
    public static event PlayerCollectDelegate PlayerCollectedBloodCell;

    enum PlayerState {Idle, Moving, Collision, Dead, Respawn}
    PlayerState currentState = PlayerState.Idle;

    public string translateInputAxis = "Vertical";
    public string rotateInputAxis = "Horizontal";

    public float rotationRate = 360;    // allows 360 degrees of rotation
    public float moveForce = 1;         // speed at which player moves
    public float traverseSpeed;

    [Header("Movement Config")]
    public float moveSpeed;
    public float targetMoveSpeed;
    public float movementRadius;
    
    private float traverseSpeedScaler;
    private Vector3 currentMoveTarget;

    private void OnEnable()
    {
        // setup event listeners
        HostController.HeartRateChanged += OnHeartRateChanged;
    }

    private void OnDisable()
    {
        // cleanup event listeners
        HostController.HeartRateChanged -= OnHeartRateChanged;
    }

    /*--- START ---*/
    void Start()
    {
        // setup variables
        currentState = PlayerState.Idle;
        currentMoveTarget = Vector3.zero;
        traverseSpeedScaler = 0f;
    }

    /*--- UPDATE ---*/
    void Update()
    {
        /*--- PLAYER STATES---*/
        switch (currentState)
        {
            case PlayerState.Idle:
                break;

            case PlayerState.Moving:
                break;

            default:
                break;
        }

        float horizontalInput = Input.GetAxis(rotateInputAxis);
        float verticalInput = Input.GetAxis(translateInputAxis);
        
        // add the forwards movement to the player
        Vector3 forwardMovement = transform.forward * traverseSpeed * traverseSpeedScaler * Time.deltaTime;
        transform.Translate(forwardMovement, Space.World);

        // add the controlled movement to the player
        DoControlledMovement(horizontalInput, verticalInput);
    }

    private void DoControlledMovement(float xVal, float yVal)
    {
        // get the position of the player on the XY plane
        Vector2 currentXYPos = new Vector2(transform.position.x, transform.position.y);

        // calculate the 2d controlled movement
        Vector2 controlledMovement = Vector2.ClampMagnitude(new Vector2(xVal, yVal), 1);

        // clamp the movement to the radius of the artery (playable area)
        controlledMovement = Vector2.ClampMagnitude(currentXYPos + controlledMovement, movementRadius) - currentXYPos;

        // apply the movement
        transform.Translate(controlledMovement * moveSpeed * Time.deltaTime);

        Debug.DrawRay(transform.position, controlledMovement, Color.white);
    }

    /*--- COLLISIONS ---*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectible")
        {
            // call the collectedBloodCell event
            // "announces" that player has collected blood cell
            if (PlayerCollectedBloodCell != null) PlayerCollectedBloodCell();

            Destroy(other.gameObject);
        }
    }

    private void OnHeartRateChanged(float newHeartRate)
    {
        traverseSpeedScaler = newHeartRate / 10f;
        Debug.Log(traverseSpeedScaler);
    }
}
