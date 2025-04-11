using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // singleton instance
    public static GameManager instance;

    // game configuration
    [SerializeField] private int currentPlayerIndex;
    [SerializeField] private int turnDirection = 1;
    [SerializeField] private List<Player> playersToSkip;
    [SerializeField] private List<Player> currentPlayers;
    [SerializeField] private Deck deck;
    [SerializeField] private Stack<Card> drawPile;
    [SerializeField] private Stack<Card> discardPile;

    // configuring singleton instance at earliest point
    private void Awake() {
        instance = this;
    }

    // any initial game setup
    private void Start() {
        // generate the deck of cards, and shuffle
        deck = new Deck();
        deck.GenerateDefaultCards();
        List<Card> shuffledDeck = deck.GetShuffledDeck();

        // generate the stacks to play the game
        drawPile = new Stack<Card>(shuffledDeck);
        discardPile = new Stack<Card>();
        playersToSkip = new List<Player>();
    }

    // ------------------------------------------------------------
    // Primary game flow functionality
    //  Turns, order, etc.
    // ------------------------------------------------------------
    
    // This function should exclusively be used for gathering input
    private void Update() {
        // TODO: replace with mouse inputs
        // drawing a singular card
        if (Input.GetKeyDown(KeyCode.Space)) {
            currentPlayers[currentPlayerIndex].DrawCard();
            ProgressTurnOrder();
        }
        else {
            // TODO: replace with mouse inputs
            KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };
            for (int i = 0; i < keyCodes.Length; i++) {
                // TODO: implement a more robust turn system with more pauses for other types of cards
                if (Input.GetKeyDown(keyCodes[i])) {
                    if (currentPlayers[currentPlayerIndex].PlayCard(i)) {
                        // TODO: this may need to be implemented with references if indexes cause issues
                        currentPlayers[currentPlayerIndex].DiscardCard(i);
                        ProgressTurnOrder();
                    }
                    
                } 
            }
        }
    }

    private void ProgressTurnOrder() {
        currentPlayerIndex = ((currentPlayerIndex + turnDirection) + currentPlayers.Count) % currentPlayers.Count;
        
        // if the player should be skipped, move on again
        while (playersToSkip.Contains(currentPlayers[currentPlayerIndex])) {
            playersToSkip.Remove(currentPlayers[currentPlayerIndex]);
            currentPlayerIndex = ((currentPlayerIndex + turnDirection) + currentPlayers.Count) % currentPlayers.Count;
        }
    }

    // ------------------------------------------------------------
    // Functions designed for allowing other scripts to interact with game flow
    //  For example, Cards and Players needing to draw cards
    // ------------------------------------------------------------

    // retrieve the list of current players
    public List<Player> GetCurrentPlayerList() {
        return currentPlayers;
    }

    // reverse the turn order
    public void ReverseTurnOrder() {
        turnDirection *= -1;
    }

    // method for drawing a card during gameplay
    //  in this case, "drawing a card" removes it from the draw pile
    //  and gives a reference to the drawn card to use
    public Card DrawCard() {
        if(drawPile == null) return null;

        if(!drawPile.TryPop(out Card drawn)) {
            // TODO: decide on empty draw pile behavior
            return null; // TODO: change null if a new deck is generated or something
        }
        else return drawn;
    }

    // method for drawing a card during gameplay of a specific type
    public Card DrawCard(string targetType) {
        if (drawPile == null) return null;

        Stack<Card> skippedCards = new Stack<Card>();
        Card drawn = null;
        while ((drawn == null || drawn.CardType != targetType) && drawPile.Count > 0) {
            drawPile.TryPop(out drawn);

            if (drawn.CardType != targetType) {
                skippedCards.Push(drawn);
            }
        }

        // add the cards that were drawn before our target card back in order
        while (skippedCards.Count > 0) {
            drawPile.Push(skippedCards.Pop());
        }

        // only way to achieve this is if there were no cards on the draw pile of the right type
        if (drawn == null || drawn.CardType != targetType) {
            return null;
        }
        else {
            return drawn;
        }
    }

    // method for discarding a card
    //  ensures both card and pile exist, does nothing otherwise
    public void DiscardCard(Card discardCard) {
        if(discardPile != null || discardCard != null) {
            discardPile.Push(discardCard);
        }
    }

    // mark a player to be skipped
    public void SkipPlayer(Player player) {
        playersToSkip.Add(player);
    }

    // helper method to return copies of the stacks for usage in the editor
    public (Stack<Card>, Stack<Card>) GetStackCopies() {
        // NOTE: the copies of cards are shallow, but the stacks are separate
        return (
            drawPile == null ? null : new Stack<Card>(new Stack<Card>(drawPile)), 
            discardPile == null ? null : new Stack<Card>(new Stack<Card>(discardPile))
        );
    }
}
