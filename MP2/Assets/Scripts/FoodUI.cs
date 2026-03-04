using TMPro;
using UnityEngine;

public class FoodUI : MonoBehaviour
{
    public ResourceBank bank;
    public TMP_Text foodText;

    string lastRendered;

    void Update()
    {
        if (bank == null || foodText == null)
            return;

        string nextText =
            $"Total Food Cooked: {bank.totalFood:0}\n" + "" +
            $"Hot Dogs: {bank.hotDogs:0}\n" +
            $"Fries: {bank.fries:0}\n" +
            $"Sandwiches: {bank.sandwiches:0}\n" +
            $"Lasagna: {bank.lasagne:0}";

        if (nextText == lastRendered)
            return;

        foodText.text = nextText;
        lastRendered = nextText;
    }
}