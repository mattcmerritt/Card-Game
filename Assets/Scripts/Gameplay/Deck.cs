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
    public Dictionary<string, Func<Player, Player, int, string, bool>> cardActions;

    // supporting data structure that will contain the data used to construct cards
    private struct CardConfig {
        public string actionName;
        public int amount;
        public string extra;
    }

    // generate the card actions for the base game
    // TODO: implement a custom system for loading custom scripts
    public void GenerateDefaultCardActions() {
        // TODO: add an integer input for repetitions
        cardActions = new Dictionary<string, Func<Player, Player, int, string, bool>>();

        // TODO: for all actions, need to support performing multiple times (draw 2x, discard 2x, etc.)
        //  for multiples, partial action is still valid

        cardActions.Add("draw", (Player player, Player target, int amount, string extra) => {
            bool successful = false;
            for (int i = 0; i < amount; i++) {
                if (player.DrawCard()) successful = true;
            }
            return successful;
        });
        cardActions.Add("draw specific", (Player player, Player target, int amount, string extra) => {
            bool successful = false;
            for (int i = 0; i < amount; i++) {
                if (player.DrawCard(extra)) successful = true;
            }
            return successful;
        });
        cardActions.Add("discard random", (Player player, Player target, int amount, string extra) => {
            bool successful = false;
            // TODO: reimplement with player choice prompt instead of random
            for (int i = 0; i < amount; i++) {
                if (target.DiscardCard(UnityEngine.Random.Range(0, target.GetCardCount()))) successful = true;
            }
            return successful;
        });
        cardActions.Add("discard all", (Player player, Player target, int amount, string extra) => {
            bool successful = false;
            for (int i = 0; i < amount; i++) {
                // remove a card from each player
                foreach (Player other in GameManager.instance.GetCurrentPlayerList()) {
                    // exclude self
                    if (other == player) continue;

                    // attempt to discard a random card from the others
                    if (other.DiscardCard(UnityEngine.Random.Range(0, other.GetCardCount()))) successful = true;
                }
            }
            return successful;
        });
        cardActions.Add("reverse", (Player player, Player target, int amount, string extra) => {
            GameManager.instance.ReverseTurnOrder();
            return true;
        });
        cardActions.Add("view", (Player player, Player target, int amount, string extra) => {
            // TODO: this needs to be implemented with custom input interfaces
            return target.ShowHand();
        });
        cardActions.Add("steal", (Player player, Player target, int amount, string extra) => {
            // TODO: reimplement with player choice prompt instead of random
            //  random is not needed for this
            bool successful = false;
            for (int i = 0; i < amount; i++) {
                if (target.GetCardCount() == 0) return successful;
                if (player.AddCard(target.RemoveCard(UnityEngine.Random.Range(0, target.GetCardCount())))) successful = true;
            }
            return successful;
        });
        cardActions.Add("steal specific", (Player player, Player target, int amount, string extra) => {
            // TODO: reimplement with random choice instead of first
            bool successful = false;
            for (int i = 0; i < amount; i++) {
                if (target.GetCardCount() == 0) return successful;
                if (target.FindFirstCardOfType(extra) == -1) return successful;
                if (player.AddCard(target.RemoveCard(target.FindFirstCardOfType(extra)))) successful = true;
            }
            return successful;
        });
        cardActions.Add("skip", (Player player, Player target, int amount, string extra) => {
            GameManager.instance.SkipPlayer(target);
            return true;
        });
    }

    // prepares the card actions, the constructs a set of cards
    public void GenerateDefaultCards() {
        // build the action set
        GenerateDefaultCardActions();

        // determine what cards will populate the deck using a custom configuration
        // current types: "draw", "draw specific", "discard random", "discard all", "reverse", "view", "steal", "steal specific", "skip"
        Dictionary<CardConfig, int> deckSetupConfig = new Dictionary<CardConfig, int>();

        // default configuration
        deckSetupConfig.Add(new CardConfig { actionName = "draw", amount = 1, extra = "" }, 7);
        deckSetupConfig.Add(new CardConfig { actionName = "draw", amount = 2, extra = "" }, 5);
        deckSetupConfig.Add(new CardConfig { actionName = "draw specific", amount = 1, extra = "action" }, 3);
        deckSetupConfig.Add(new CardConfig { actionName = "discard random", amount = 1, extra = "" }, 6);
        deckSetupConfig.Add(new CardConfig { actionName = "discard random", amount = 2, extra = "" }, 4);
        deckSetupConfig.Add(new CardConfig { actionName = "discard all", amount = 1, extra = "" }, 4);
        deckSetupConfig.Add(new CardConfig { actionName = "reverse", amount = 1, extra = "" }, 8);
        deckSetupConfig.Add(new CardConfig { actionName = "view", amount = 1, extra = "" }, 6);
        deckSetupConfig.Add(new CardConfig { actionName = "steal", amount = 1, extra = "" }, 6);
        deckSetupConfig.Add(new CardConfig { actionName = "steal", amount = 2, extra = "" }, 3);
        deckSetupConfig.Add(new CardConfig { actionName = "steal specific", amount = 1, extra = "action" }, 2);
        deckSetupConfig.Add(new CardConfig { actionName = "skip", amount = 1, extra = "" }, 8);

        // build the deck itself
        cardSet = new List<Card>();
        foreach (KeyValuePair<CardConfig, int> cardConfig in deckSetupConfig) {
            for (int i = 0; i < cardConfig.Value; i++) {
                // generate card name
                // TODO: may need to implement a more complex system if user facing
                string name = $"{cardConfig.Key.actionName} x {cardConfig.Key.amount} ({cardConfig.Key.extra})";
                // TODO: implement system to support other types of cards
                Card cardCopy = new Card(name, cardConfig.Key.actionName, cardConfig.Key.amount, cardConfig.Key.extra, "action");
                cardActions.TryGetValue(cardConfig.Key.actionName, out Func<Player, Player, int, string, bool> actionFunction);
                cardCopy.SetCardAction(actionFunction);
                cardSet.Add(cardCopy);
            }
        }        
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
