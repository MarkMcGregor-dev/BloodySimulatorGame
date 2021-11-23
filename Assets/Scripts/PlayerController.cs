using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerCollectDelegate();
    public static event PlayerCollectDelegate PlayerCollectedBloodCell;

    public delegate void PlayerDeathDelegate();
    public static event PlayerDeathDelegate PlayerDied;

    public AudioSource collect;
    public AudioSource destroy;
    public AudioSource powerUp;

    enum PlayerState {Idle, Moving, Collision, Dead, Respawn}
    PlayerState currentState = PlayerState.Idle;

    public string translateInputAxis = "Vertical";
    public string rotateInputAxis = "Horizontal";

    [Header("Movement Config")]
    public float traverseSpeed;
    public float moveSpeed;
    public float movementRadius;
    [Header("Stats config")]
    public int maxNumCells;
    public int cellsToBreakBlockage;
    
    private float traverseSpeedScaler;
    private HostController hostController;
    public int numCellsCollected;

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
        traverseSpeedScaler = 0f;
        hostController = GameObject.FindObjectOfType<HostController>();
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
        transform.Translate(controlledMovement * hostController.currentHeartRate * moveSpeed * Time.deltaTime);

        Debug.DrawRay(transform.position, controlledMovement, Color.white);
    }

    /*--- COLLISIONS ---*/
    private void OnTriggerEnter(Collider other)
    {
        // if collided with a blood cell and not holding max
        if (other.tag == "Collectible")
        {
            collect.Play();
            // call the collectedBloodCell event
            // "announces" that player has collected blood cell
            if (PlayerCollectedBloodCell != null) PlayerCollectedBloodCell();

            // increment the number of collected cells
            if (numCellsCollected <= maxNumCells)
            {
                numCellsCollected++;
            }
            
            // destroy the other collectable
            Destroy(other.gameObject);

        } else if (other.tag == "Blockage")
        {
            destroy.Play();
            // make sure the player has enough cells to break the blockage
            if (numCellsCollected >= cellsToBreakBlockage)
            {
                // break the blockage
                other.transform.parent.gameObject.GetComponent<BlockageBehaviour>().BreakBlockage();

                // decrement the cellCollected by the cost of breaking a blockage
                numCellsCollected -= cellsToBreakBlockage;

                Debug.Log("Blockage Broken!");

            } else
            {
                // kill the player
                // if (PlayerDied != null) PlayerDied();

                Debug.Log("Dead");
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnHeartRateChanged(float newHeartRate)
    {
        traverseSpeedScaler = newHeartRate / 10f;
    }
}
