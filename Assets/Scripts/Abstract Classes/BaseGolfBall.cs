using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseGolfBall : MonoBehaviour
{
    public BallSettings ballSettings;

    [SerializeField]
    bool enablePhysics;

    NavMeshAgent agent;
    Rigidbody rb;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize()
    {
        SetYPosition();
        agent.enabled = true; // to make sure ball is in navmesh baked area, it is disabled at first and when enabled, it automatically moves object to nearest baked area
        Destroy(agent, 0.08f);
        Invoke(nameof(SetYPosition), 0.1f);

        if (enablePhysics)
            EnablePhysics();
    }

    private void EnablePhysics()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void SetYPosition()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~3))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.12f, transform.position.z);
        }
    }
}

[Serializable]
public record BallSettings
{
    public BallDifficulty ballDifficulty;
    public int points;
}

public enum BallDifficulty
{
    Easy,
    Medium,
    Hard
}
