using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CustomerWalkerBuyer : MonoBehaviour
{
    [Header("Movement")]
    public float arriveDistance = 0.35f;
    public float rotateSpeed = 720f;

    [Header("Buying")]
    public float waitAtStationSeconds = 1.0f;
    public float dollarsPerPurchase = 5f;

    ResourceBank bank;
    StationGenerator station;
    Vector3 exitPoint;
    Action onDespawn;

    NavMeshAgent agent;

    enum State
    {
        WalkingToStation,
        Waiting,
        Leaving
    }

    State state;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
            agent.avoidancePriority = UnityEngine.Random.Range(20, 80);
    }

    public void Init(ResourceBank bank, StationGenerator station, Vector3 exitPoint, Action onDespawn)
    {
        this.bank = bank;
        this.station = station;
        this.exitPoint = exitPoint;
        this.onDespawn = onDespawn;

        state = State.WalkingToStation;
        GoTo(GetStationStopPosition());
    }

    void Update()
    {
        if (agent == null || bank == null || station == null)
            return;

        switch (state)
        {
            case State.WalkingToStation:
                if (IsArrived())
                {
                    state = State.Waiting;
                    agent.isStopped = true;
                    Face(station.transform.position);
                    StartCoroutine(WaitThenBuy());
                }
                break;

            case State.Leaving:
                if (IsArrived())
                {
                    onDespawn?.Invoke();
                    Destroy(gameObject);
                }
                break;
        }
    }

    IEnumerator WaitThenBuy()
    {
        yield return new WaitForSeconds(waitAtStationSeconds);

        TryBuyOne(bank, station.produces, dollarsPerPurchase);

        state = State.Leaving;
        agent.isStopped = false;
        GoTo(exitPoint);
    }

    void GoTo(Vector3 target)
    {
        agent.stoppingDistance = arriveDistance;
        agent.SetDestination(target);
    }

    bool IsArrived()
    {
        if (agent.pathPending) return false;
        if (agent.remainingDistance > agent.stoppingDistance) return false;
        return !agent.hasPath || agent.velocity.sqrMagnitude < 0.01f;
    }

    Vector3 GetStationStopPosition()
    {
        if (station != null && station.customerStandPoint != null)
            return station.customerStandPoint.position;

        return station.transform.position - station.transform.forward * 0.6f;
    }

    void Face(Vector3 lookAt)
    {
        Vector3 dir = lookAt - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }

    static bool TryBuyOne(ResourceBank bank, StationGenerator.FoodType type, float dollars)
    {
        switch (type)
        {
            case StationGenerator.FoodType.HotDog:
                if (bank.hotDogs < 1f) return false;
                bank.hotDogs -= 1f;
                break;

            case StationGenerator.FoodType.Fries:
                if (bank.fries < 1f) return false;
                bank.fries -= 1f;
                break;

            case StationGenerator.FoodType.Sandwich:
                if (bank.sandwiches < 1f) return false;
                bank.sandwiches -= 1f;
                break;

            case StationGenerator.FoodType.Lasagna:
                if (bank.lasagne < 1f) return false;
                bank.lasagne -= 1f;
                break;
        }

        bank.AddMoney(dollars);
        return true;
    }
}