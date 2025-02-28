using UnityEngine;

// TODO: consider whether these should be scriptable objects?
//  personally, I am not entirely sure this is the right way
//  since it would need to be generated dynamically when loading
//  a custom deck.
[System.Serializable]
public class Card
{
    // doesn't really do anything special, currently just has some data
    [SerializeField] private string name;

    // simplest constructor for cards
    public Card(string name) {
        this.name = name;
    }

    // overloaded constructor for more complex cards
    public Card(string name, string actionKey) : this(name) {
        // TODO: implement an abstract way to store the card actions
        //  currently, I think the best way would be a system leveraging
        //  partial function application.
    }
}
