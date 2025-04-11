using UnityEngine;
using System;
using System.Collections.Generic;

// TODO: consider whether these should be scriptable objects?
//  personally, I am not entirely sure this is the right way
//  since it would need to be generated dynamically when loading
//  a custom deck.
[System.Serializable]
public class Card
{
    // internal data
    [SerializeField] private string name;
    private string actionKey;
    private int actionAmount;
    private string actionExtra;
    private string cardType;

    // backing fields for internal data
    public string Name { get { return name; } private set { name = value; } }
    public string ActionKey { get; private set; }
    public int ActionAmount { get; private set; }
    public string ActionExtra { get; private set; }
    public string CardType { get; private set; }

    private Func<Player, Player, int, string, bool> cardAction;

    // simplest constructor for cards
    public Card(string name) {
        this.name = name;
    }

    // overloaded constructor for more complex cards
    public Card(string name, string actionKey, int actionAmount, string actionExtra, string cardType) : this(name) {
        this.actionKey = actionKey;
        this.actionAmount = actionAmount;
        this.actionExtra = actionExtra;
        this.cardType = cardType;
    }

    public void SetCardAction(Func<Player, Player, int, string, bool> cardAction) {
        this.cardAction = cardAction;
    }

    public bool AttemptPlayCard(Player player, Player target) {
        return cardAction(player, target, actionAmount, actionExtra);
    }

    // helper method for rendering with custom editor
    public override string ToString() {
        // if (actionKey == null || cardType == null) 
        //     return name;
        // else
        //     return $"{name}: (type: {cardType}, action: {actionKey})";
        
        return name;
    }
}
