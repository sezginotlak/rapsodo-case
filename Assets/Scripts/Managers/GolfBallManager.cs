using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBallManager : MonoBehaviour
{
    public static GolfBallManager Instance;
    public int BallCount { private get; set; } = 10;

    [SerializeField]
    NPCDecision npcDecision;

    [SerializeField]
    Transform ballParentTransform;

    [SerializeField]
    List<BaseGolfBall> ballPrefabList;

    [SerializeField]
    SpawnBorders border;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SpawnBalls()
    {
        for(int i = 0; i < BallCount; i++)
        {
            BaseGolfBall ball = Instantiate(ballPrefabList[UnityEngine.Random.Range(0, ballPrefabList.Count)], GetRandomPoint(), Quaternion.identity, ballParentTransform);
            ball.Initialize();
            npcDecision.AddBall(ball);
        }
    }

    private Vector3 GetRandomPoint()
    {
        return new Vector3(UnityEngine.Random.Range(border.leftBottom.position.x, border.rightBottom.position.x), 
                           5, 
                           UnityEngine.Random.Range(border.leftBottom.position.z, border.rightTop.position.z));
    }
}

[Serializable]
public record SpawnBorders
{
    public Transform leftBottom;
    public Transform leftTop;
    public Transform rightBottom;
    public Transform rightTop;
}
