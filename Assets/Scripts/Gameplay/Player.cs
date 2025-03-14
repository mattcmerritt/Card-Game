using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // player state information
    [SerializeField] private List<Card> hand;

    // generic behavior for playing a card
    public void PlayCard() {
        // TODO: reference the dec
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
        return hand.FindIndex((Card c) => c.cardType == cardType);
    }

    // debug only, used for testing
    // TODO: remove / deactivate
    void Update() {
        // test draw
        if(Input.GetKeyDown(KeyCode.Space)) {
            DrawCard();
        }

        // test discard
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            DiscardCard(0);
        }

        // test discard, part 2
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            DiscardCard(1);
        }
    }
}
