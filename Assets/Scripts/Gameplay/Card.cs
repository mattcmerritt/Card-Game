using UnityEngine;

// TODO: consider whether these should be scriptable objects?
//  personally, I am not entirely sure this is the right way
//  since it would need to be generated dynamically when loading
//  a custom deck.
[System.Serializable]
public class Card
{
    // doesn't really do anything special, currently just has some data
    [SerializeField] public string name { get; private set; }
    [SerializeField] public string actionKey { get; private set; }
    [SerializeField] public string cardType { get; private set; }

    // simplest constructor for cards
    public Card(string name) {
        this.name = name;
    }

    // overloaded constructor for more complex cards
    public Card(string name, string actionKey, string cardType) : this(name) {
        this.actionKey = actionKey;
        this.cardType = cardType;

        // TODO: implement an abstract way to store the card actions
        //  currently, I think the best way would be a system leveraging
        //  partial function application.
    }

    // helper method for rendering with custom editor
    public override string ToString() {
        if (actionKey == null || cardType == null) 
            return name;
        else
            return $"{name}: (type: {cardType}, action: {actionKey})";
    }
}
