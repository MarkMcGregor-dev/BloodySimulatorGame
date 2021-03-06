using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerCollectDelegate();
    public static event PlayerCollectDelegate PlayerCollectedBloodCell;

    public delegate void PlayerDeathDelegate();
    public static event PlayerDeathDelegate PlayerDied;

    public AudioSource collect;
    public AudioSource destroy;
    public AudioSource powerUp;
    public AudioSource coffeeDrink;
    public AudioSource sandwichEat;

    public GameObject deathEffect;

    enum PlayerState {Idle, Moving, Collision, Dead, Respawn}
    //PlayerState currentState = PlayerState.Idle;

    public string translateInputAxis = "Vertical";
    public string rotateInputAxis = "Horizontal";

    [Header("Movement Config")]
    public float traverseSpeed;
    public float moveSpeed;
    public float movementRadius;
    [Header("Stats config")]
    public int maxNumCells;
    public int cellsToBreakBlockage;
    
    [Header("Do Not Touch")]
    public float distanceInLevel;
    private float traverseSpeedScaler;
    private HostController hostController;
    public int numCellsCollected;
    private LevelGeneratorV2 levelGenerator;
    private Spline levelSpline;
    private Vector2 localPosition;

    private GameObject spawner;
    private GameObject hostControllerInstance;

    private IEnumerator coroutineSandwich;

    private void OnEnable()
    {
        // setup event listeners
        HostController.HeartRateChanged += OnHeartRateChanged;
        LevelGeneratorV2.LevelSectionRemoved += OnLevelSectionRemoved;
    }

    private void OnDisable()
    {
        // cleanup event listeners
        HostController.HeartRateChanged -= OnHeartRateChanged;
        LevelGeneratorV2.LevelSectionRemoved += OnLevelSectionRemoved;
    }

    /*--- START ---*/
    void Start()
    {
        // setup variables
        //currentState = PlayerState.Idle;
        traverseSpeedScaler = 0f;
        hostController = GameObject.FindObjectOfType<HostController>();
        distanceInLevel = 30f;
        levelGenerator = FindObjectOfType<LevelGeneratorV2>();
        levelSpline = FindObjectOfType<Spline>();
        localPosition = Vector2.zero;
        spawner = GameObject.Find("Spawner");
        hostControllerInstance = GameObject.Find("HostController");
    }

    /*--- UPDATE ---*/
    void Update()
    {
        float horizontalInput = Input.GetAxis(rotateInputAxis);
        float verticalInput = Input.GetAxis(translateInputAxis);

        // calculate the new distance in the level
        distanceInLevel += traverseSpeed * ( 1 + traverseSpeedScaler) * Time.deltaTime;

        // get the sample on the curve where the player should be
        CurveSample curveSample = levelSpline.GetSampleAtDistance(distanceInLevel);

        // apply the forwards movement to the player
        transform.SetPositionAndRotation(curveSample.location, curveSample.Rotation);

        // add the forwards movement to the player
        //Vector3 forwardMovement = transform.forward * traverseSpeed * traverseSpeedScaler * Time.deltaTime;
        //transform.Translate(forwardMovement, Space.World);

        // add the controlled movement to the player
        DoControlledMovement(horizontalInput, verticalInput);
    }

    private void DoControlledMovement(float xVal, float yVal)
    {
        // get the position of the player on the XY plane
        //Vector2 currentXYPos = new Vector2(transform.position.x, transform.position.y);

        // calculate the 2d controlled movement
        Vector2 controlledMovement = Vector2.ClampMagnitude(new Vector2(xVal, yVal), 1);

        // clamp the movement to the radius of the artery (playable area)
        controlledMovement = Vector2.ClampMagnitude(localPosition + controlledMovement, movementRadius) - localPosition;

        // apply the movement
        localPosition += controlledMovement * moveSpeed * Time.deltaTime;

        Vector3 localTranslation =
            (transform.right * localPosition.x) +
            (transform.up * localPosition.y);

        transform.Translate(localTranslation);
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

        } 
        
        else if (other.tag == "Blockage")
        {   
            destroy.Play();
            // make sure the player has enough cells to break the blockage
            if (numCellsCollected >= cellsToBreakBlockage)
            {
                // break the blockage
                other.gameObject.GetComponent<BlockageBehaviour>().BreakBlockage();

                // decrement the cellCollected by the cost of breaking a blockage
                numCellsCollected -= cellsToBreakBlockage;

            } else
            {
                hostControllerInstance.GetComponent<HostController>().ExecuteOnPlayerDeath();
            }
        }

        else if (other.tag == "Sandwich")
        {
            sandwichEat.Play();

            float originalSpawnFrequency = spawner.GetComponent<SpawnerV2>().cellSpawnFrequency;
            coroutineSandwich = SandwichEffect(originalSpawnFrequency);
            StartCoroutine(coroutineSandwich);
           
            Destroy(other.gameObject);
        }

        else if (other.tag == "Coffee")
        {
            coffeeDrink.Play();

            hostControllerInstance.GetComponent<HostController>().OnCoffeeDrink();

            Destroy(other.gameObject);
        }
    }

    private IEnumerator SandwichEffect(float originalSpawnFrequency)
    {

        spawner.GetComponent<SpawnerV2>().cellSpawnFrequency = 5f;

        var obj = GameObject.Find("Energy Up Indicator");
        var anim = obj.GetComponent<Animation>();
        var animName = "energyUpArrows";

        obj.transform.GetChild(0).GetComponent<Animation>().Play();
        anim.Play(animName);

        yield return new WaitForSeconds(2.5f);

        spawner.GetComponent<SpawnerV2>().cellSpawnFrequency = originalSpawnFrequency;

        yield return new WaitForSeconds(3f);

        anim[animName].time = 0.0f;
        anim.Sample();
        anim[animName].enabled = false;

        StopCoroutine(coroutineSandwich);
    }

    private void OnHeartRateChanged(float newHeartRate)
    {
        traverseSpeedScaler = newHeartRate / 500f;
    }

    private void OnLevelSectionRemoved(float curveLength)
    {
        float previousDistance = distanceInLevel;

        // offset the distance travelled by the gap between points
        distanceInLevel -= curveLength;

        // Debug.Log("Previous distance: " + previousDistance + "\n\tNew Distance: " + distanceInLevel);
    }
}
