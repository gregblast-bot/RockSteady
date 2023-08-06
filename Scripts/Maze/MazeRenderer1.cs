using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityRandom = UnityEngine.Random;

public class MazeRenderer1 : MonoBehaviour
{ 
    [SerializeField]
    [Range(1, 50)]
    private int width = 10; // Width of floor and walls

    [SerializeField]
    [Range(1, 50)]
    private int height = 10; // Height of floor and walls

    [SerializeField]
    private float size = 1f; // Defined size for scale and position

    [SerializeField]
    private Transform wallPrefab = null; // Wall object

    [SerializeField]
    private Transform doorPrefab = null; // Door object

    [SerializeField]
    private Transform floorPrefab = null; // Floor object

    [SerializeField]
    private Transform holePrefab = null; // Hole object

    [SerializeField]
    private Transform forceHolePrefab = null; // Force hole object

    [SerializeField]
    private Transform sylvesterSphere = null; // Character object

    [SerializeField]
    private Transform sylvesterStone = null; // Character statues

    [SerializeField]
    private Transform checkPointPrefab = null; // Checkpoint object

    [SerializeField]
    private float tiltSpeed = 15f; // Speed at which the platform tilts

    [SerializeField]
    private float tiltAngle = 14f; // Max tilt

    private float left, up, right, down; // Hold float
    private int randEntrancePosY; // Generate random entrance position
    private int randExitPosY; // Generate random entrance position
    private int count; // Keep track of time for timed linear sequence queue
    private int deathCount = 0; // Keep track of Sylvester's deaths
    private bool pressedLeft, pressedUp, pressedRight, pressedDown, pressedTracked; // Track key presses for queue
    private Quaternion targetRotation; // Rotation of maze
    private Transform sylvester; // Hold Sylvester data
    private Transform checkPoint; // Hold check point data
    private KeyState sharedKey; // State of the controls
    private LinearSequenceQueuey lsq; // Use the queue to manage tutorial instructions
    private LinearSequenceDeathQueue deathQueue; // Use the death queue to manage game end
    private LinearSequenceQueue2 winQueue; // Use the win queue to manage game end
    private OnTriggerWin onWin; // Use this to trigger a scene transition
    public UnityEvent triggerDeathEvent; // Use to update hearts

    // If using AddForce and AddTorque
    //private float forceMultiplier = 1f;
    //private float torqueMultiplier = 1f;
    //private Rigidbody rb;


    [Flags]
    public enum KeyState
    {
        /**********************************
         * Walls
         **********************************/
        // 0000 -> No Keys
        // 1111 -> Left, Top, Right, Bottom
        LeftKey = 8,   // 1000
        UpKey = 4,    // 0100
        RightKey = 2,  // 0010
        DownKey = 1, // 0001
    }

    // Start is called before the first frame update
    void Start()
    {
        // Randomly generated entrance and exit positions
        randEntrancePosY = UnityRandom.Range(0, height);
        randExitPosY = UnityRandom.Range(0, height);

        createSylvester();

        // Maze and floor
        var maze = MazeGenerator.Generate(width, height, randEntrancePosY, randExitPosY);
        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width, 1, height);

        lsq = this.GetComponent<LinearSequenceQueuey>();
        deathQueue = this.GetComponent<LinearSequenceDeathQueue>();
        winQueue = this.GetComponent<LinearSequenceQueue2>();

        // Get the rigid body for this MazeRenderer class if using force/torque
        //rb = this.GetComponent<Rigidbody>();

        Draw(maze);

        // Subscribe to win event for smooth scene transition
        onWin.triggerWinEvent.AddListener(() => subscribe2Change());
    }

    // Handle direction of tilt depending on key press
    void Update()
    {
        // Switch scene on death
        if (deathCount >= 3)
        {
            deathQueue.DequeueNextEvent();
            Thread.Sleep(5000);
            SceneManager.LoadScene("GameOver");
        }

        // Check if Sylvester has been destroyed or not
        if (sylvester == null)
        {
            createSylvester();
            deathCount += 1;
            triggerDeathEvent.Invoke();
        }

        //Reset tilt and update key state the first frame a game key is pressed
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (sharedKey != KeyState.LeftKey)
            {
                sharedKey = KeyState.LeftKey;
                pressedLeft = true;
                //left = Input.GetAxis("Horizontal");
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (sharedKey != KeyState.UpKey)
            {
                sharedKey = KeyState.UpKey;
                pressedUp = true;
                //up = Input.GetAxis("Vertical");
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (sharedKey != KeyState.RightKey)
            {
                sharedKey = KeyState.RightKey;
                pressedRight = true;
                //right = Input.GetAxis("Horizontal");
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (sharedKey != KeyState.DownKey)
            {
                sharedKey = KeyState.DownKey;
                pressedDown = true;
                //down = Input.GetAxis("Vertical");
            }
        }

        // After user uses controls go to next instruction in queue
        if (pressedLeft == true && pressedUp == true && pressedRight == true && pressedDown == true && pressedTracked == false)
        {
            lsq.DequeueNextEvent();
            pressedTracked = true;
        }
    }

    // Physics updates go here
    void FixedUpdate()
    {
        if (pressedTracked == true)
        {
            // Update queue approximately every 15 seconds
            count += 1;
            if (count == 750)
            {
                count = 0;
                lsq.DequeueNextEvent();
            }
        }

        // Increment tilt and update position depending on key state and if a key is pressed down
        if (sharedKey == KeyState.LeftKey && Input.anyKey)
        {
            //rb.AddTorque(transform.forward * torqueMultiplier, ForceMode.Acceleration);
            //incrementTilt(left);
            left = Input.GetAxis("Horizontal");
            startTilt(0f, -tiltAngle*left);
        }
        else if (sharedKey == KeyState.UpKey && Input.anyKey)
        {
            //rb.AddTorque(-transform.up * torqueMultiplier, ForceMode.Acceleration);
            //incrementTilt(up);
            up = Input.GetAxis("Vertical");
            startTilt(tiltAngle*up, 0f);
        }
        else if (sharedKey == KeyState.RightKey && Input.anyKey)
        {
            //rb.AddTorque(-transform.forward * torqueMultiplier, ForceMode.Acceleration);
            //incrementTilt(right);
            right = Input.GetAxis("Horizontal");
            startTilt(0f, -tiltAngle*right);
        }
        else if (sharedKey == KeyState.DownKey && Input.anyKey)
        {
            //rb.AddTorque(transform.up * torqueMultiplier, ForceMode.Acceleration);
            //incrementTilt(down);
            down = Input.GetAxis("Vertical");
            startTilt(tiltAngle*down, 0f);
        }

        float tiltStep = tiltSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, tiltStep);
    }

    private void Draw(WallState[,] maze)
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];

                // Position of cell offset so that the middle of the maze sits at the origin
                var position = new Vector3((-width / 2 + i) * size, 0, ((-height / 2 + j) * size) - 85);
                //var position = new Vector3(-width/2 + i, 0, (-height/2 + j) - 85);

                createCheckPoint();

                if (i != 0)
                {
                    holeGeneration(position);
                }

                if (i == width - 3 && j == randExitPosY)
                {
                    var door = Instantiate(doorPrefab, transform) as Transform;
                    door.position = position + new Vector3(-size / 2, 0, 0);
                    door.localScale = new Vector3(size, door.localScale.y, door.localScale.z);
                    door.eulerAngles = new Vector3(0, 90, 0);
                }

                if (cell.HasFlag(WallState.LeftWall))
                {
                    if (i != width - 3 || j != randExitPosY)
                    {
                        var leftWall = Instantiate(wallPrefab, transform) as Transform;
                        leftWall.position = position + new Vector3(-size / 2, 0, 0);
                        leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                        leftWall.eulerAngles = new Vector3(0, 90, 0);
                        if (i == 0)
                        {
                            var sylvesterStatue = Instantiate(sylvesterStone, transform) as Transform;
                            sylvesterStatue.position = position + new Vector3(-size / 2, 0, 0);
                            sylvesterStatue.eulerAngles = new Vector3(-45, 90, 0);
                        }
                    }
                }

                if (cell.HasFlag(WallState.TopWall))
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size / 2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                    if (j == height - 1)
                    {
                        var sylvesterStatue = Instantiate(sylvesterStone, transform) as Transform;
                        sylvesterStatue.position = position + new Vector3(0, 0, size / 2);
                        sylvesterStatue.eulerAngles = new Vector3(-45, 180, 0);
                    }
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RightWall))
                    {
                        var sylvesterStatue = Instantiate(sylvesterStone, transform) as Transform;
                        var rightWall = Instantiate(wallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                        sylvesterStatue.position = position + new Vector3(size / 2, 0, 0);
                        sylvesterStatue.eulerAngles = new Vector3(-45, -90, 0);
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.BottomWall))
                    {
                        var sylvesterStatue = Instantiate(sylvesterStone, transform) as Transform;
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                        sylvesterStatue.position = position + new Vector3(0, 0, -size / 2);
                        sylvesterStatue.eulerAngles = new Vector3(-45, 0, 0);
                    }
                }
            }
        }
    }


    private void startTilt(float xAngle, float zAngle)
    {
        targetRotation = Quaternion.Euler(xAngle, 0f, zAngle);
    }

    private void incrementTilt(float component)
    {
        component += 0.01f;
    }

    private void decrementTilt(float component)
    {
        component -= 0.01f;
    }

    private void createSylvester()
    {
        // Base position
        var position = new Vector3((-width / 2) * size, 20f, ((-height / 2 + randEntrancePosY) * size) - 85);

        // Sylvester's attributes
        sylvester = Instantiate(sylvesterSphere, transform) as Transform;
        sylvester.position = position + new Vector3(-size / 2, 0, 0);
        sylvester.eulerAngles = new Vector3(45, 90, 0);
    }

    private void destroySylvester()
    {
        Destroy(sylvester.gameObject);
    }

    private void createCheckPoint()
    {
        // Base position
        var position = new Vector3((width / 2) * size, 0f, ((-height / 2 + randExitPosY) * size) - 85);

        // Sylvester's attributes
        checkPoint = Instantiate(checkPointPrefab, transform) as Transform;
        checkPoint.position = position;
        onWin = checkPoint.GetComponent<OnTriggerWin>();
    }

    private void holeGeneration(Vector3 position)
    {
        // Give holes position and instantiate 
        int randHolePos = UnityRandom.Range(0, height);

        // TODO: Create a better algorithm for hole generation
        if (randHolePos % 4 == 0)
        {
            var hole = Instantiate(holePrefab, transform);
            hole.position = position + new Vector3(0, 0.26f, 0);
        }
        if (randHolePos % 5 == 0)
        {
            var forceHole = Instantiate(forceHolePrefab, transform);
            forceHole.position = position + new Vector3(0, 0.26f, 0);
        }
    }

    void subscribe2Change()
    {
        winQueue.DequeueNextEvent();
        Thread.Sleep(4200);
        SceneManager.LoadScene("Level2");
    }
}

