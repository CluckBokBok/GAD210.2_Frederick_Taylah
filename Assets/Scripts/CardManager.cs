using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<FormStats> cardDeck = new List<FormStats>();
    private List<FormStats> deckCopy;

    [SerializeField] private List<FormStats> playerHand = new List<FormStats>();
    [SerializeField] private int maxHandSize = 4;

    [SerializeField] private Button drawCardButton;

    // Serialized field for the UI Image components where the cards will be displayed
    [SerializeField] private List<Image> handUIImages = new List<Image>();

    void Start()
    {
        ShuffleDeck();

        // Assign the button's click event
        if (drawCardButton != null)
            drawCardButton.onClick.AddListener(TryDrawCard);
    }

    // Shuffles the deck so players do not draw the same cards each time
    public void ShuffleDeck()
    {
        deckCopy = new List<FormStats>(cardDeck);

        for (int i = 0; i < deckCopy.Count; i++)
        {
            FormStats temp = deckCopy[i];
            int randomIndex = Random.Range(i, deckCopy.Count);
            deckCopy[i] = deckCopy[randomIndex];
            deckCopy[randomIndex] = temp;
        }
    }

    // Only allows players to draw a card if they have room in their hand
    public void TryDrawCard()
    {
        if (playerHand.Count < maxHandSize)
        {
            FormStats drawnCard = DrawCard();
            playerHand.Add(drawnCard);
            Debug.Log($"Drew card: {drawnCard.cardName}");

            // Update the UI with the drawn card
            UpdateCardUI();
        }
        else
        {
            Debug.Log("Hand is full! Cannot draw more cards.");
        }
    }

    // Draws a card from the deck and shuffles said deck if it becomes empty
    private FormStats DrawCard()
    {
        if (deckCopy.Count == 0)
        {
            Debug.Log("Deck is empty. Reshuffling...");
            ShuffleDeck();
        }

        FormStats drawnCard = deckCopy[0];
        deckCopy.RemoveAt(0);
        return drawnCard;
    }

    // Updates the UI with cards the player has drawn
    private void UpdateCardUI()
    {
        // Loop through the player's hand and the UI images
        for (int i = 0; i < playerHand.Count; i++)
        {
            if (i < handUIImages.Count)
            {
                // Update the UI image with the card sprite
                handUIImages[i].sprite = playerHand[i].sprite;

                // Optionally set the image active or visible here if you need more control over the UI
                handUIImages[i].gameObject.SetActive(true);
            }
        }
    }

    // Needs refinement, currently doesn't do anything 
    public void PlayCard(FormStats card)
    {
        // Logic to play the card, e.g., trigger an ability, remove the card from the hand, etc.
        Debug.Log($"Card {card.cardName} has been played.");
        playerHand.Remove(card);
        UpdateCardUI();
    }

}


