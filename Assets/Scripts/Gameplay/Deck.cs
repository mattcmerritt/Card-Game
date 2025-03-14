using UnityEngine;
using System;
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

    // defines all the different behaviours that cards can cause
    //  for each of the functions, there are three inputs:
    //      Player - the person playing the card, the hand that will perform the action
    //      Player - the target of the card action, the hand that the action is done to
    //      string - any extra details that may be necessary
    //          for example, with draw specific, this could be the type of card to draw
    //          this will likely be unused in most cases
    //  each function also has one output, which is whether the action succeeded
    public Dictionary<string, Func<Player, Player, string, bool>> cardActions;

    // generate the card actions for the base game
    // TODO: implement a custom system for loading custom scripts
    public void GenerateDefaultCardActions() {
        string[] actions = { "draw", "discard", "all discard", "reverse", "draw specific", "steal", "view", "steal specific", "skip" };
        cardActions = new Dictionary<string, Func<Player, Player, string, bool>>();

        // TODO: for all actions, need to support performing multiple times (draw 2x, discard 2x, etc.)
        cardActions.Add("draw", (Player player, Player target, string extra) => {
            return player.DrawCard();
        });
        cardActions.Add("draw specific", (Player player, Player target, string extra) => {
            return player.DrawCard(extra);
        });
        cardActions.Add("discard", (Player player, Player target, string extra) => {
            // TODO: reimplement with player choice prompt instead of random
            return target.DiscardCard(UnityEngine.Random.Range(0, target.GetCardCount()));
        });
        cardActions.Add("discard all", (Player player, Player target, string extra) => {
            bool hasDiscarded = false;
            foreach (Player other in GameManager.instance.GetCurrentPlayerList()) {
                // exclude self
                if (other == player) continue;

                // attempt to discard a random card from the others
                if (other.DiscardCard(UnityEngine.Random.Range(0, other.GetCardCount()))) {
                    hasDiscarded = true;
                }
            }
            return hasDiscarded;
        });
        cardActions.Add("reverse", (Player player, Player target, string extra) => {
            GameManager.instance.ReverseTurnOrder();
            return true;
        });
        cardActions.Add("view", (Player player, Player target, string extra) => {
            // TODO: this needs to be implemented with custom input interfaces
            return target.ShowHand();
        });
        cardActions.Add("steal", (Player player, Player target, string extra) => {
            // TODO: reimplement with player choice prompt instead of random
            if (target.GetCardCount() == 0) return false;
            return player.AddCard(target.RemoveCard(UnityEngine.Random.Range(0, target.GetCardCount())));
        });
        cardActions.Add("steal specific", (Player player, Player target, string extra) => {
            // TODO: reimplement with player choice prompt instead of first
            if (target.GetCardCount() == 0) return false;
            if (target.FindFirstCardOfType(extra) == -1) return false;
            return player.AddCard(target.RemoveCard(target.FindFirstCardOfType(extra)));
        });
        cardActions.Add("skip", (Player player, Player target, string extra) => {
            GameManager.instance.SkipPlayer(target);
            return true;
        });
    }

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
            int swapIndex = UnityEngine.Random.Range(0, i + 1);
            Card temp = shuffledCopy[swapIndex];
            shuffledCopy[swapIndex] = shuffledCopy[i];
            shuffledCopy[i] = temp;
        }

        return shuffledCopy;
    }
}
