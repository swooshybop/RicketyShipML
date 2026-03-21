using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ShipAgent : Agent
{
    [Header("References")]
    public RockSpawner rockSpawner;

    [Header("Flap Settings")]
    public float flapForce = 5f;

    [Header("Boundaries")]
    public float ceilingY = 5f;
    public float floorY = -5f;

    private Rigidbody2D rb;
    private Vector3 startPosition;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        // Reset bird position and velocity
        transform.localPosition = startPosition;
        rb.linearVelocity = Vector2.zero;

        // Clear all rocks and restart spawning
        rockSpawner.ResetPipes();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 1. Bird's Y position (normalized roughly to play area)
        sensor.AddObservation(transform.localPosition.y / ceilingY);

        // 2. Bird's Y velocity (normalized)
        sensor.AddObservation(rb.linearVelocity.y / 10f);

        // 3-4. Next rock pair info
        Transform nextRock = rockSpawner.GetNextPipe(transform.localPosition.x);
        if (nextRock != null)
        {
            float horizontalDist = nextRock.localPosition.x - transform.localPosition.x;
            sensor.AddObservation(horizontalDist / 15f);

            // Gap center in local space
            float gapCenterY = nextRock.localPosition.y;
            sensor.AddObservation((gapCenterY - transform.localPosition.y) / ceilingY);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
        // Total: 4 observations
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Discrete action: 0 = do nothing, 1 = flap
        int flap = actions.DiscreteActions[0];
        Debug.Log($"Action: {flap}, Bird Y: {transform.localPosition.y}, Vel: {rb.linearVelocity.y}");
    
        if (flap == 1)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flapForce);
        }

        // Small survival reward each step
        AddReward(0.001f);

        // Kill episode if bird goes out of bounds
        if (transform.localPosition.y > ceilingY || transform.localPosition.y < floorY)
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discrete = actionsOut.DiscreteActions;
        discrete[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ScoreZone"))
        {
            AddReward(2.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }
}