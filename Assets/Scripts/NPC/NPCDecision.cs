using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPCDecision : MonoBehaviour
{
    List<BaseGolfBall> golfBallList = new List<BaseGolfBall>();
    List<BallPriority> priorityList = new List<BallPriority>();

    public void AddBall(BaseGolfBall ball)
    {
        golfBallList.Add(ball);
    }

    public void RemoveBall(BaseGolfBall ball)
    {
        golfBallList.Remove(ball);
    }

    public BaseGolfBall GetDestinationDecision(NavMeshAgent agent, float health)
    {
        priorityList.Clear();

        CalculatePriority(agent, health);

        return priorityList[0].ball;
    }

    public bool IsBallsFinished()
    {
        return golfBallList.Count < 1;
    }

    private void CalculatePriority(NavMeshAgent agent, float health)
    {
        foreach (BaseGolfBall golfBall in golfBallList)
        {
            NavMeshPath path = new NavMeshPath();
            BallPriority ballPriority = new BallPriority();
            float pathLength = 0f;
            float priority = 0f;

            if (agent.CalculatePath(golfBall.transform.position, path))
            {
                pathLength = GetPathLength(path);
                priority = (golfBall.ballSettings.points / pathLength) * health;
            }

            ballPriority.ball = golfBall;
            ballPriority.priority = priority;

            priorityList.Add(ballPriority);
        }

        priorityList = priorityList.OrderByDescending(x => x.priority).ToList();
    }

    private float GetPathLength(NavMeshPath path)
    {
        float length = 0.0f;

        if (path.corners.Length < 2)
            return length;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            length += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return length;
    }
}

[Serializable]
public record BallPriority
{
    public BaseGolfBall ball;
    public float priority;
}
