using UnityEngine;

// helper class with animation event functions for card animation

// drawing:
//  splits card into four renderers:
//      main:   the full card
//      rim:    the outline of the card
//      sep:    the space between the fillable sections of the card
//      spc:    the background where the card information will be filled
//  each of the three subsections has its own colors and sprites based on the type of card

public class CardAnimationHelper : MonoBehaviour
{
    [SerializeField] private Animator ani;
    [SerializeField] private string cardType;

    private void Start()
    {
        // TODO: implement way to fetch this given card information
        cardType = /* something not yet implemented */ "secret";
    }

    public void TransitionToSpecificDrawAnimation()
    {
        if (cardType == "action")
        {
            ani.SetBool("isAction", true);
        }
        else if (cardType == "counter")
        {
            ani.SetBool("isCounter", true);
        }
        else if (cardType == "secret")
        {
            ani.SetBool("isSecret", true);
        }
        else
        {
            Debug.LogError($"Invalid card type (type \"{cardType}\") detected, unable to handle");
        }
    }
}
