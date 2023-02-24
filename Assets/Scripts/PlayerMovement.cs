using System.Collections;
using System.Collections.Generic;
using UnityEngine; //Connect to Unity Engine

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Player Variables")]
    [Tooltip("Set the health you want for the player")]
    public int hp = 3;
    [Tooltip("Set the amount of damage you want the player to do")]
    public float damage = 5f;
    [Tooltip("Set the cooldown timer for player to have to wait before attacking again")]
    public float cooldown = 0.5f;
    [Tooltip("Set the current time of the cooldown counter")]
    public float currentCooldown = 0f;
    //Private reference for distance to ground
    private float _distanceToGround;
    [Header("Movement Variables")]
    [Tooltip("Set the speed of the character")]
    public float speed = 5f;
    [Tooltip("Set the strength of the players jump ability")]
    public float jumpHeight = 6f;
    //A respawn position for the player to return to when they lose health
    [HideInInspector] public Vector3 respawnPosition = Vector3.zero;
    //A Vector2 to store input direction for movement
    [HideInInspector] public Vector3 moveDirection = Vector3.zero;
    [Header("References")]
    [Tooltip("Drag the GameManager in here")]
    public GameManager gameManager;
    [Tooltip("Attach the Animator from the character object here")]
    public Animator animator;
    [Tooltip("Attach the Rigidbody from the player here")]
    public Rigidbody2D rb;
    [Tooltip("Add the SpriteRenderer component from the player here")]
    public SpriteRenderer sprite;
    [Tooltip("Add the backgrounds to the array so we can offest the image as we move")]
    public Renderer[] backgrounds = new Renderer[3];
    //private bool check for whether we are in a doorway
    private bool inDoorway = false;
    //Array to store the sorting layers we have in our project
    private SortingLayer[] _sortingLayers;
    #endregion
    #region Setup & Update
    void Start()
    {
        //If we haven't attached our references get them from the player object
        if (gameManager == null) gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (animator == null) animator= GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
        if (backgrounds == null) backgrounds = Camera.main.transform.GetComponentsInChildren<Renderer>();
        //Set the respawn position to players starting position
        respawnPosition = transform.position;
        //Set distance to ground
        _distanceToGround = GetComponent<Collider2D>().bounds.extents.y + 0.2f; 
        //Fill sortingLayers array with the sorting layers from our project.
        _sortingLayers = SortingLayer.layers;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.state == GameManager.GameStates.InGame)
        {
            //If player has fallen below a certain level trigger respawn function
            if (transform.position.y < -10f) Respawn();
            //If we click the left button run the attack function
            if (Input.GetMouseButtonDown(0)) Attack();
            //If we use the horizontal axis run the movement function
            if (Input.GetButton("Horizontal")) Movement(Input.GetAxis("Horizontal"));
            //If we press the space button run the Jump function
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) Jump();
            //If we press up or down while inDoorway is true trigger the enter door function
            if (Input.GetButtonDown("Vertical") && inDoorway == true) EnterDoor(Input.GetAxis("Vertical"));
            //If we press escape button handle the pausing
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameManager.state = GameManager.GameStates.Paused;
                gameManager.ChangeState(gameManager.state);
            }
        }
    }
    #endregion
    #region Functions
    public bool IsGrounded()
    {
        //Raycast to see if there is ground beneath the player
        return Physics2D.Raycast(transform.position, Vector2.down, _distanceToGround);
    }
    public void Attack()
    {
        //Vector2 to store direction based on which way we are moving
        Vector2 _direction;
        //If we are moving right, set movement to right, else set it left
        if (!sprite.flipX) _direction = Vector2.right;
        else _direction = Vector2.left;
        //Get hitinfo from a raycast and if it hits anything kill the object as long as object is enemy
        RaycastHit2D _hitInfo = Physics2D.Raycast(transform.position, _direction, 1.18f);
        if (_hitInfo && _hitInfo.collider.CompareTag("Enemy")) Destroy(_hitInfo.collider.gameObject);
    }
    public void Respawn()
    {
        //If we have more than 1 health lower our health by 1 and move back to set respawn point
        if (hp > 1)
        {
            hp--;
            transform.position = respawnPosition;
            Camera.main.transform.position = new Vector3(transform.position.x, 0f, -10f);
        }
        //Else we have lost our last health, kill the object
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void Movement(float input)
    {
        //Move our character based on the input given and make camera match characters x position
        transform.position += new Vector3(input, 0f, 0f) * speed * Time.deltaTime;
        Camera.main.transform.position = new Vector3(transform.position.x, 0f, -10f);
        //For each of our backgrounds offset the background to make it seem like it is moving. Adjust speed to match our movement
        foreach (Renderer ren in backgrounds)
        {
            Vector4 offset = ren.material.GetVector("_Offset");
            offset.x += input * (speed / 20) * Time.deltaTime;
            ren.material.SetVector("_Offset", offset);
        }
        //Flip the sprite according to movement
        if (input < 0f) sprite.flipX = true;
        else sprite.flipX = false;
    }
    public void Jump()
    {
        //Add force to rigidbody to simulate jump
        rb.AddForce(Vector3.up * jumpHeight, ForceMode2D.Impulse);
    }
    public void EnterDoor(float direction)
    {
        //Set a layerID int to store the value from the array that matches our current sorting layer
        int layerID = 5;
        //Find the sortingLayer reference in the array that matches our current layer and set the layerID to that index
        for (int i = 0; i < _sortingLayers.Length; i++) if (sprite.sortingLayerID == _sortingLayers[i].id) layerID = i;
        //If we are using the W button and we are not at the lowest of our background layers change our layer to the one previous
        if (direction > 0f && layerID > 1)
        {
            sprite.sortingLayerID = _sortingLayers[layerID - 1].id;
        }
        //Else if we are pushing S and we are not at the highest layer change our layer to the one after our current
        else if (direction < 0f && layerID < _sortingLayers.Length - 1)
        {
            sprite.sortingLayerID = _sortingLayers[layerID + 1].id;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If we are colliding with an enemy run the Respawn function
        if (collision.transform.CompareTag("Enemy"))
        {
            Respawn();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If we enter the trigger zone of the respawn point set respawn point to that location.
        if (collision.transform.CompareTag("Respawn")) respawnPosition = collision.transform.position;
        //If we enter the trigger zone for a door set the bool to true
        if (collision.transform.CompareTag("Door")) inDoorway = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //If we exit the trigger zone for a door set the bool to false
        if (collision.transform.CompareTag("Door")) inDoorway = false;
    }
    #endregion
}
