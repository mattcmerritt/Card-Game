using UnityEngine;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // player state information
    [SerializeField] private List<Card> hand;

    // generic behavior for playing a card
    public bool PlayCard(int index) {
        // ensure the card exists
        if (hand.Count < index)
            return false;

        // TODO: determine how to choose players to target, or if this can be handled later in the process
        //  this should be done using a coroutine with a wait until condition
        // for now, find all players and use the one that is not currently playing as target
        Player[] playersInGame = GameObject.FindObjectsByType<Player>(FindObjectsSortMode.None);
        Player other = Array.Find(playersInGame, (Player p) => p != this);

        // otherwise return result of playing the card
        return hand[index].AttemptPlayCard(this, other);
    }

    // method to draw card from draw pile
    //  may do something if there is no card to draw (see TODO comment)
    public bool DrawCard() {
        Card drawn = GameManager.instance.DrawCard();
        
        if(drawn != null) {
            hand.Add(drawn);
            return true;
        }
        else {
            // TODO: special behavior for if no card can be drawn?
            Debug.LogWarning("There are no cards to draw!");
            return false;
        }
    }

    // method to draw a specific card from the draw pile
    //  may do something if there is no card to draw (see TODO comment)
    public bool DrawCard(string type) {
        Card drawn = GameManager.instance.DrawCard();
        
        if(drawn != null) {
            hand.Add(drawn);
            return true;
        }
        else {
            // TODO: special behavior for if no card can be drawn?
            Debug.LogWarning("There are no cards to draw!");
            return false;
        }
    }

    // method to discard card from hand
    //  configured with indices so that the correct card is removed
    //  when duplicates of cards are allowed within the same hand
    //  (for example, if a player had two Jack of Diamonds 
    //  because they used two decks)
    public bool DiscardCard(int handIndex) {
        if(handIndex >= hand.Count || handIndex < 0) {
            Debug.LogError($"Failed to discard, index {handIndex} out of hand bounds");
            return false;
        }
        else {
            Card discardedCard = hand[handIndex];
            hand.RemoveAt(handIndex);
            GameManager.instance.DiscardCard(discardedCard);
            return true;
        }
    }

    // get the size of a player's hand, necessary for index checks
    public int GetCardCount() {
        return hand.Count;
    }

    // add card to hand
    public bool AddCard(Card card) {
        hand.Add(card);
        return true;
    }

    // remove card from hand
    public Card RemoveCard(int index) {
        Card removed = hand[index];
        hand.RemoveAt(index);
        return removed;
    }

    // TODO: implement in a way that actually works in game
    public bool ShowHand() {
        string output = "Opponents hand is:\n";
        foreach (Card c in hand) {
            output += $"\t{c.ToString()}\n";
        }
        Debug.Log(output);
        return true;
    }

    // TODO: implement this in a different way to return all
    public int FindFirstCardOfType(string cardType) {
        return hand.FindIndex((Card c) => c.CardType == cardType);
    }
}
