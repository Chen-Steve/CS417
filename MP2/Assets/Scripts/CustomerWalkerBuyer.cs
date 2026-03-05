using System;
using System.Collections;
using UnityEngine;

public class CustomerWalkerBuyer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.2f;
    public float arriveDistance = 0.25f;

    [Header("Buying")]
    public float waitAtStationSeconds = 1.0f;
    public float dollarsPerPurchase = 5f;

    ResourceBank bank;
    StationGenerator station;
    Transform exitPoint;
    Action onDespawn;

    enum State { WalkingToStation, Waiting, Leaving }
    State state;

    public void Init(ResourceBank bank, StationGenerator station, Transform exitPoint, Action onDespawn)
    {
        this.bank = bank;
        this.station = station;
        this.exitPoint = exitPoint;
        this.onDespawn = onDespawn;

        state = State.WalkingToStation;
    }

    void Update()
    {
        if (bank == null || station == null || exitPoint == null)
            return;

        if (state == State.WalkingToStation)
        {
            MoveTowards(station.transform.position);

            if (IsArrived(station.transform.position))
            {
                state = State.Waiting;
                StartCoroutine(WaitThenBuy());
            }
        }
        else if (state == State.Leaving)
        {
            MoveTowards(exitPoint.position);

            if (IsArrived(exitPoint.position))
            {
                onDespawn?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator WaitThenBuy()
    {
        yield return new WaitForSeconds(waitAtStationSeconds);

        bool bought = TryBuyOne(bank, station.produces, dollarsPerPurchase);

        state = State.Leaving;
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 pos = transform.position;
        Vector3 next = Vector3.MoveTowards(pos, target, moveSpeed * Time.deltaTime);
        transform.position = next;

        Vector3 dir = (target - pos);
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir.normalized);
    }

    bool IsArrived(Vector3 target)
    {
        Vector3 a = transform.position;
        Vector3 b = target;
        a.y = 0f;
        b.y = 0f;
        return Vector3.Distance(a, b) <= arriveDistance;
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