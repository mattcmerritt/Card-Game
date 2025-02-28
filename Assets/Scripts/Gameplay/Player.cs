using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // player state information
    [SerializeField] private List<Card> hand;

    // method to draw card from draw pile
    //  may do something if there is no card to draw (see TODO comment)
    public void DrawCard() {
        // TODO: implement
        Card drawn = GameManager.instance.DrawCard();
        
        if(drawn != null) {
            hand.Add(drawn);
        }
        else {
            // TODO: special behavior for if no card can be drawn?
            Debug.LogWarning("There are no cards to draw!");
        }
    }

    // method to discard card from hand
    //  configured with indices so that the correct card is removed
    //  when duplicates of cards are allowed within the same hand
    //  (for example, if a player had two Jack of Diamonds 
    //  because they used two decks)
    public void DiscardCard(int handIndex) {
        if(handIndex >= hand.Count || handIndex < 0) {
            Debug.LogError($"Failed to discard, index {handIndex} out of hand bounds");
        }
        else {
            Card discardedCard = hand[handIndex];
            hand.RemoveAt(handIndex);
            GameManager.instance.DiscardCard(discardedCard);
        }
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
