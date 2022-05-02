using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] TextMesh front_text;
    [SerializeField] TextMesh back_text;

    [SerializeField] float delay_time = 0.5f;
    [SerializeField] int points = 10;
    public int Card_value { get; set; }
    private string jp_value;
    private string sp_value;
    public bool IsVisible { get; set; } 
    public bool CanFlip { get; set; }
    private GameManager gameManager;
    private Animator cardAnim;

    public string Jp_value
    {
        get { return jp_value; }

        set
        {
            jp_value = value;
            front_text.text = jp_value;
        }
    }

    public string Sp_value
    {
        get { return sp_value; }

        set
        {
            sp_value = value;
            front_text.text = sp_value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.CanFlip = true;
    }

    private void OnMouseDown()
    {
        if (!gameManager.GameStarted) { gameManager.GameStarted = true; }
        
        if (!gameManager.GameEnd && gameManager.GameStarted && !gameManager.GameOver)
        {
            ClickCard(); 
        }
    }

    private void FlipCard(bool showValue, Card c)
    {
        if (c != null)
        {
            AnimateCard(c, showValue);

            this.IsVisible = showValue;
            c.back_text.gameObject.SetActive(!showValue);
            c.front_text.gameObject.SetActive(showValue);
        }
    }

    private void CheckMatching()
    {
        if (gameManager.previousCard == null)
        {
            gameManager.previousCard = gameObject.transform.GetComponent<Card>();
        }
        else
        {
            if (gameManager.previousCard.Card_value == this.Card_value)
            {
                FindObjectOfType<AudioManager>().Play("pair_match");
                gameManager.previousCard.CanFlip = false;
                this.CanFlip = false; 
                gameManager.previousCard = null;
                gameManager.AddPoints(points); 
            }
            else
            {
                StartCoroutine(HideCard());
            }
        }
    }

    IEnumerator HideCard()
    { 
        yield return new WaitForSeconds(delay_time);

        FlipCard(false, this);
        FlipCard(false, gameManager.previousCard);
        gameManager.previousCard = null;
    }

    private void ClickCard()
    {
        if (gameManager.previousCard != this && this.CanFlip)
        {
            FindObjectOfType<AudioManager>().Play("click_card");

            FlipCard(true, this);
            CheckMatching();
        }
    }

    private void AnimateCard(Card c, bool b)
    {
        cardAnim = c.GetComponent<Animator>(); 
        if (cardAnim != null && cardAnim.isActiveAndEnabled)
        {
            cardAnim.SetBool("Flip_b", b);
            cardAnim.SetBool("Flip1_b", !b);
        }
    }

}
