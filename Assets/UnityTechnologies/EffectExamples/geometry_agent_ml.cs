using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using System.Text;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.LowLevel;

public class GeometryBugFinder : Agent
{
    Rigidbody rb;

    //public Transform agentTarget; // donde esta el bug

    private HashSet<Vector3> visitedPositions = new HashSet<Vector3>();

    public grid_score_detection gridScore;

    public GameObject startPos;
    private int currentPos = 0;


    private float punishmentMultiplier = 1f;

    public float rayLength = 5f;
    public bool showDebugRays = true;

    public float[] distances = new float[8];

    private Dictionary<int, List<Vector3WithLabel>> recordedArrays = new Dictionary<int, List<Vector3WithLabel>>();
    private List<Vector3> dynamicData = new List<Vector3>();
    List<Vector3WithLabel> labeledPositions = new List<Vector3WithLabel>();

    private int episodeCount = 0;

    private bool firstPass = false;
    private bool bugFound = false;
    public bool record;

    public string fileName;

    //public ReplayBehavior replayScript;

    //private PlayerInput playerInput;

    //private Keyboard keyboard;

    public bool W_Pressed;
    public bool A_Pressed;
    public bool S_Pressed;
    public bool D_Pressed;
    public bool Space_Pressed;

    private bool[] keyStates = new bool[5];

    //public override void Initialize()
    //{
    //    base.Initialize();
    //    // Randomize starting behavior
    //    GetComponent<BehaviorParameters>().BehaviorType =
    //        Random.Range(0, 2) == 0 ? BehaviorType.Default : BehaviorType.HeuristicOnly;
    //}

    public float moveX;
    public float moveZ;

    public ReplayBehavior replayBHV;

    public PositionData posData;

    public struct Vector3WithLabel
    {
        public Vector3 Position;
        public string Label;

        public Vector3WithLabel(Vector3 position, string label)
        {
            Position = position;
            Label = label;
        }

        public override string ToString()
        {
            return $"{Position.x};{Position.y};{Position.z};{Label}";
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().collisionDetectionMode =
            CollisionDetectionMode.ContinuousDynamic;
        //Physics.simulationMode = true;
        Physics.gravity = new Vector3(0, -100f, 0);
        //keyboard = Keyboard.current;

        posData = replayBHV.LoadPositionData();

        try
        {
            string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/played_positions.json");
            List<Vector3> playedPositionsLoaded = JsonUtility.FromJson<List<Vector3>>(json);
        }

        catch
        {
            //pass   
        }
    }

    void Update()
    {
        Vector3 origin = transform.position;

        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, rayLength))
            {
                distances[i] = hit.distance / rayLength; // normalizamos
            }

            if (showDebugRays)
            {
                Color rayColor = hit.collider != null ? Color.red : Color.green;
                Debug.DrawRay(origin, direction * rayLength, rayColor);
            }
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("OutOfBoundsTrigger"))
    //    {
    //        Debug.Log("Agent touched the goal!");
    //        bugFound = true;
    //    }
    //}

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        //sensor.AddObservation(transform.position);
        foreach (float distance in distances)
        {
            sensor.AddObservation(distance);
        }
        //sensor.AddObservation(agentTarget.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        moveX = actions.ContinuousActions[0];
        moveZ = actions.ContinuousActions[1];

        //float axisBiasPenalty = Mathf.Abs(moveX - moveZ) * -0.01f;
        //AddReward(axisBiasPenalty);

        //Vector3 movement = new Vector3(moveX, 0, moveZ) * 5f;
        //rb.AddForce(movement, ForceMode.VelocityChange);

        //float distanceToTarget = Vector3.Distance(transform.position, agentTarget.position);
        //AddReward(-distanceToTarget * 0.01f);

        //if (visitedPositions.Add(transform.position))
        //{
        //    AddReward(0.001f);
        //}

        //W_Pressed = actions.DiscreteActions[0] == 1;
        //A_Pressed = actions.DiscreteActions[1] == 1;
        //S_Pressed = actions.DiscreteActions[2] == 1;
        //D_Pressed = actions.DiscreteActions[3] == 1;
        //Space_Pressed = actions.DiscreteActions[4] == 1;

        try
        {
                if (gridScore.CurrentCell(transform.position) == true)
                {
                    punishmentMultiplier += 0.001f;
                }

                else
                {
                    punishmentMultiplier = 1f;
                }

                if (gridScore.CheckWalked(transform.position) == false)
                {
                    AddReward(1f);
                }

                else
                {
                    AddReward(-0.001f * punishmentMultiplier);
                }

                gridScore.MarkWalked(transform.position);
            }

            catch
            {
                Debug.Log("critical error!");
            }

        //if (StepCount > 1000 && GetCumulativeReward() < -500)
        //{
        //    EndEpisode();
        //}
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    public override void OnEpisodeBegin()
    {
        if (firstPass)
        {
            StopAllCoroutines();
            recordedArrays.Add(episodeCount, labeledPositions);
            episodeCount++;
            labeledPositions = new List<Vector3WithLabel>();
        }

        transform.position = startPos.transform.position;

        gridScore.ResetGrid();

        StartCoroutine(LogPositionEverySecond());

        firstPass = true;

        if (currentPos < posData.positions.Count && posData != null)
        {
            startPos.transform.position = posData.positions[currentPos];

            Debug.Log($"Moved to position {currentPos}: {posData.positions[currentPos]}");

        }
    }

    public IEnumerator LogPositionEverySecond()
    {
        string bugStatus = "None";

        while (true)
        {
            
            if (bugFound) { bugStatus = "OutOfBounds"; bugFound = false; }
            else { bugStatus = "None"; }

            labeledPositions.Add(new Vector3WithLabel(transform.position, bugStatus));

            yield return new WaitForSeconds(1f);
        }
    }

    public void ExportToCSV(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        StringBuilder csvContent = new StringBuilder();

        foreach (var entry in recordedArrays)
        {
            int index = entry.Key;
            List<Vector3WithLabel> values = entry.Value;

            string valuesString = string.Join(",", values);

            csvContent.AppendLine($"{valuesString}");
        }

        File.WriteAllText(filePath, csvContent.ToString());
        Debug.Log($"CSV exported to: {filePath}");
    }

    private void OnDestroy()
    {
        if (record)
        {
            ExportToCSV(fileName);
        }
    }
}