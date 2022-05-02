using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject cards;
    
    [SerializeField] Number[] numbers;

    [SerializeField] int totalTime;
    [SerializeField] int timeLeftPoints; 

    [HideInInspector]public Card previousCard;
    public bool GameEnd { get; private set; }
    public bool GameStarted { get; set; }
    public static int Score { get; private set; }
    public float Countdown { get; set; }
    public bool GameOver { get; private set; }

    private bool addExtraPoints = true; 

    void Awake()
    {
        AssignValuesToCards(ChooseValues());
        Countdown = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameEnd && GameStarted && !GameOver) { Countdown -= Time.deltaTime; }

        CheckGameOver();
        CheckGameEnd();

        if (GameEnd && !GameOver && addExtraPoints) { AddExtraPoints(); }
    }

    // Select randomly values from array "numbers"
    private List<Number> ChooseValues()
    {
        List<Number> selNumbers = new List<Number>();
        Number n;
        int pair_qty = cards.transform.childCount / 2; 
        for (int i = 0; i < pair_qty; i++)
        {
            n = numbers[Random.Range(0, numbers.Length)]; 
            
            while(selNumbers.Contains(n))
            {
                n = numbers[Random.Range(0, numbers.Length)];
            }
            selNumbers.Add(n); 
        }
        return selNumbers; 
    }

    // Assign values to cards 
    private void AssignValuesToCards(List<Number> selNumbers)
    {
        int cardsQty = cards.transform.childCount;

        // List of cards indexes 
        var cardsList = Enumerable.Range(0, cardsQty).ToList();

        // Shuffle list of cards 
        cardsList = cardsList.OrderBy(x => Random.value).ToList();

        int j,m,t = 0;
        for (int i = 0; i < cardsQty; i+=2)
        {   
            j = cardsList[i]; // card index
            m = cardsList[i + 1]; // next card index

            cards.transform.GetChild(j).GetComponent<Card>().Sp_value = selNumbers[t].numeral;
            cards.transform.GetChild(j).GetComponent<Card>().Card_value = selNumbers[t].id;

            cards.transform.GetChild(m).GetComponent<Card>().Jp_value = selNumbers[t].jp_num;
            cards.transform.GetChild(m).GetComponent<Card>().Card_value = selNumbers[t].id;

            t++; 
        }
    }

    private void CheckGameEnd()
    {
        GameEnd = true;
        for (int i = 0; i < cards.transform.childCount; i++)
        {
            if (cards.transform.GetChild(i).GetComponent<Card>().CanFlip)
            {
                GameEnd = false;
                break;
            }
        }
    }

    private void CheckGameOver()
    {
        if (Countdown <= 0)
        {
            Countdown = 0;
            GameOver = true;
            GameEnd = false;
            Score = 0;
        }
    }

    private void AddExtraPoints()
    {
        AddPoints(Mathf.RoundToInt(Countdown) * timeLeftPoints);
        addExtraPoints = false;
    }

    public void AddPoints(int points)
    {
        Score += points; 
    }
}
