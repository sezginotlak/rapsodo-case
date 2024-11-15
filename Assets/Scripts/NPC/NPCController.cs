using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public BaseGolfBall CurrentBall { get; set; }
    public bool IsGameStarted { private get; set; }

    public State currentState;

    [SerializeField]
    Transform returnPoint;

    NavMeshAgent agent;
    Animator animator;
    NPCDecision npcDecision;
    NPCMovement npcMovement;
    NPCHealth npcHealth;
    NPCCanvas npcCanvas;

    bool isExecutedOnce;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        npcDecision = GetComponent<NPCDecision>();
        npcMovement = GetComponent<NPCMovement>();
        npcHealth = GetComponent<NPCHealth>();
        npcCanvas = GetComponent<NPCCanvas>();
    }

    private void Update()
    {
        if (!IsGameStarted) return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentState != State.Empty) return;

            OnStart();
        }

        if (IsFinished() && !isExecutedOnce)
        {
            if (currentState == State.Collecting || currentState == State.Arrived) return;

            isExecutedOnce = true;

            npcHealth.CancelHealthUpdate();

            StopAllCoroutines();

            ChangeState(State.Finished);
        }
    }

    private void ChangeState(State nextState)
    {
        if (currentState == nextState) return;

        currentState = nextState;
        SetAction();
    }

    private void SetAction()
    {
        switch (currentState)
        {
            case State.Idle:
                OnIdle();
                break;

            case State.Decided:
                OnDecisionMade();
                break;

            case State.Moving:
                OnMoving();
                break;

            case State.Arrived:
                OnArrivedBall();
                break;

            case State.Collecting:
                OnCollecting();
                break;

            case State.Collected:
                OnCollectedBall();
                break;

            case State.Finished:
                OnFinished();
                break;

            case State.Returning:
                OnReturning(); 
                break;

            case State.Returned:
                OnReturned();
                break;
        }
    }

    private void OnStart()
    {
        npcHealth.DecreaseHealth(npcCanvas);
        ChangeState(State.Idle);
    }

    private void OnIdle()
    {
        CurrentBall = npcDecision.GetDestinationDecision(agent, npcHealth.Health);
        ChangeState(State.Decided);
    }

    private void OnDecisionMade()
    {
        npcMovement.Move(agent, CurrentBall.transform.position);
        ChangeState(State.Moving);
    }

    private void OnMoving()
    {
        npcMovement.SetSpeed(animator, 3f);
        StartCoroutine(CheckArrival(State.Arrived));
    }

    private void OnArrivedBall()
    {
        ChangeState(State.Collecting);
    }

    private void OnCollecting()
    {
        npcMovement.SetTrigger(animator);
    }

    private void OnCollectedBall()
    {
        ChangeState(State.Idle);
    }

    private void OnFinished()
    {
        npcMovement.Move(agent, returnPoint.position);
        ChangeState(State.Returning);
    }

    private void OnReturning()
    {
        npcMovement.SetSpeed(animator, 3f);
        StartCoroutine(CheckArrival(State.Returned));
    }

    private void OnReturned()
    {
        MainCanvasManager.Instance.OpenGameOverPanel();
    }

    private bool IsFinished()
    {
        return npcDecision.IsBallsFinished() || npcHealth.IsHealthFinished();
    }

    private IEnumerator CheckArrival(State nextState)
    {
        while (agent.remainingDistance >= 0.1f || agent.pathPending)
        {
            yield return null;
        }

        npcMovement.SetSpeed(animator, 0);
        ChangeState(nextState);
    }

    //Called by Picking Up animation event
    public async Task PickUpObject()
    {
        ScoreManager.Instance.AddPoint(CurrentBall.ballSettings.points);
        CurrentBall.gameObject.SetActive(false);
        npcDecision.RemoveBall(CurrentBall);
        await Task.Delay(1500);
        ChangeState(State.Collected);
    }
}

public enum State
{
    Empty,
    Idle,
    Decided,
    Moving,
    Arrived,
    Collecting,
    Collected,
    Finished,
    Returning,
    Returned,
}
