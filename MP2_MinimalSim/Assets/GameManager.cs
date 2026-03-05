using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float sunlight;
    public float money;

    public float sunlightRate;
    public float moneyRate;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Euler integration
        sunlight += sunlightRate * Time.deltaTime;
        money += moneyRate * Time.deltaTime;
    }
}