using UnityEngine;

public class TrophyUnlocks : MonoBehaviour
{
    public ResourceBank bank;

    [Header("Trophy Objects")]
    public GameObject hotDogTrophy;
    public GameObject friesTrophy;
    public GameObject sandwichTrophy;
    public GameObject lasagnaTrophy;

    [Header("Unlock Requirements")]
    public float hotDogGoal = 5f;
    public float friesGoal = 5f;
    public float sandwichGoal = 5f;
    public float lasagnaGoal = 5f;

    void Start()
    {
    }

    void Update()
    {        
        if (bank == null)
            return;

        if (bank.hotDogs >= hotDogGoal)
            SetTrophyActive(hotDogTrophy);

        if (bank.fries >= friesGoal)
            SetTrophyActive(friesTrophy);

        if (bank.sandwiches >= sandwichGoal)
            SetTrophyActive(sandwichTrophy);

        if (bank.lasagne >= lasagnaGoal)
            SetTrophyActive(lasagnaTrophy);
    }

    void SetTrophyActive(GameObject trophy)
    {
        if (trophy == null)
            return;

        trophy.SetActive(true);
    }
}
