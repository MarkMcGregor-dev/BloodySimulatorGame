using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerCollectDelegate();
    public static event PlayerCollectDelegate PlayerCollectedBloodCell;

    enum PlayerState {Idle, Moving, Collision, Dead, Respawn}
    PlayerState currentState = PlayerState.Idle;
    Vector3 playerPos;

    public string translateInputAxis = "Vertical";
    public string rotateInputAxis = "Horizontal";

    public float rotationRate = 360;    // allows 360 degrees of rotation
    public float moveForce = 1;         // speed at which player moves
    public float traverseSpeed;

    /*--- START ---*/
    void Start()
    {
        currentState = PlayerState.Idle;
    }

    /*--- UPDATE ---*/
    void Update()
    {
        /*--- PLAYER STATES---*/
        switch (currentState)
        {
            case PlayerState.Idle:
                playerPos = GameObject.Find("Player").transform.position;
                break;

            case PlayerState.Moving:
                break;

            default:
                break;
        }

        float translateAxis = Input.GetAxis(translateInputAxis);
        float rotateAxis = Input.GetAxis(rotateInputAxis);

        playerInput(translateAxis, rotateAxis);

        // move the player forwards at a constant rate
        transform.position = transform.position + (transform.forward * traverseSpeed * Time.deltaTime);
    }

    /*--- CONTROLS ---*/
    private void playerInput(float translateInput, float rotateInput)
    {
        playerTranslate(translateInput);
        playerRotate(rotateInput);
    }
    private void playerTranslate(float input)
    {
        transform.Translate(Vector3.up * input * moveForce);
        currentState = PlayerState.Moving;
    }
    private void playerRotate(float input)
    {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
        currentState = PlayerState.Moving;
    }

    /*--- COLLISIONS ---*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectible")
        {
            // call the collectedBloodCell event
            if (PlayerCollectedBloodCell != null) PlayerCollectedBloodCell();

            Destroy(other.gameObject);
        }
    }

}
