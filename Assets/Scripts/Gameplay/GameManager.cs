using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // singleton instance
    public static GameManager instance;

    // game configuration
    [SerializeField] private List<Player> currentPlayers;
    [SerializeField] private Deck deck;
    [SerializeField] private Stack<Card> drawPile;
    [SerializeField] private Stack<Card> discardPile;

    // configuring singleton instance at earliest point
    private void Awake()
    {
        instance = this;
    }

    // any initial game setup
    private void Start() {
        // generate the deck of cards, and shuffle
        deck = new Deck();
        deck.GenerateStandardPlayingCards();
        List<Card> shuffledDeck = deck.GetShuffledDeck();

        // generate the stacks to play the game
        drawPile = new Stack<Card>(shuffledDeck);
        discardPile = new Stack<Card>();
    }
}
