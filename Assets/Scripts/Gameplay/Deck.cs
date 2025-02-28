using UnityEngine;
using System.Collections.Generic;

// NOTE: this class should represent a pre-build deck of cards
//  for example, with a standard deck of playing cards, this would
//  simply specify the 52 cards, each with suit and rank.
//  THIS CLASS DOES NOT UPDATE WITH GAMEPLAY.
[System.Serializable]
public class Deck
{
    // loaded data
    [SerializeField] private List<Card> cardSet;

    // TODO: add some way to serialize these to JSON and then read
    //  existing JSON back to save and load custom decks.

    // for debug purposes, generate a standard deck of playing cards
    public void GenerateStandardPlayingCards() {
        string[] suits = {"Spades", "Clubs", "Hearts", "Diamonds"};
        string[] ranks = {"Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King"};

        cardSet = new List<Card>();
        foreach (string suit in suits) {
            foreach (string rank in ranks) {
                Card newCard = new Card($"{rank} of {suit}");
                cardSet.Add(newCard);
            }
        }
    }

    // give a shuffled copy of the deck for use in the game manager
    public List<Card> GetShuffledDeck() {
        // NOTE: this will alias the cards, so do not modify them
        List<Card> shuffledCopy = new List<Card>(cardSet);

        // shuffling
        for (int i = shuffledCopy.Count - 1; i >= 0; i--) {
            int swapIndex = Random.Range(0, i + 1);
            Card temp = shuffledCopy[swapIndex];
            shuffledCopy[swapIndex] = shuffledCopy[i];
            shuffledCopy[i] = temp;
        }

        return shuffledCopy;
    }
}
