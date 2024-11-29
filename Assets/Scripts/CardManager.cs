using System.Collections.Generic;
using GAD210_Enemy_Base;
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

    private FormStats selectedCard = null;
    private Image selectedCardImage = null; 
    private const float selectedScale = 1.5f; 
    private const float defaultScale = 1f; 
    [SerializeField] private PlayerManager playerManager; 
    [SerializeField] private EnemyBase enemyBase;

    void Start()
    {
        ShuffleDeck();

        // Assign click listeners to each card in the player's hand
        for (int i = 0; i < handUIImages.Count; i++)
        {
            int index = i; // Capture index for the closure
            handUIImages[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                HandleCardClick(index);
            });
        }

        // Add listener to draw card button
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

    // Needs refinement
    public void PlayCard(FormStats card, PlayerManager playerManager, EnemyBase enemyBase)
    {
        // Apply card effects: damage to the enemy and healing to the player
        playerManager.ApplyHealing(card.healingOutput);
        enemyBase.ApplyDamage(card.damageOutput);

        Debug.Log($"Played card {card.cardName}: Damage {card.damageOutput}, Heal {card.healingOutput}");
    }

    private void HandleCardClick(int cardIndex)
    {
        if (cardIndex < 0 || cardIndex >= playerHand.Count) return;

        FormStats clickedCard = playerHand[cardIndex];
        Image clickedCardImage = handUIImages[cardIndex];

        if (selectedCard == clickedCard)
        {
            // Play the card and remove it on the second click
            PlayCard(selectedCard, playerManager, enemyBase);
            RemoveCardFromHand(cardIndex);
            selectedCard = null;
            selectedCardImage = null;
        }
        else
        {
            // Select the card and scale it up
            ResetCardScales();
            selectedCard = clickedCard;
            selectedCardImage = clickedCardImage;
            ScaleCard(clickedCardImage, selectedScale);
        }
    }

    private void ScaleCard(Image cardImage, float scale)
    {
        cardImage.transform.localScale = new Vector3(scale, scale, 1f);
    }

    private void ResetCardScales()
    {
        foreach (var cardImage in handUIImages)
        {
            cardImage.transform.localScale = Vector3.one * defaultScale;
        }
    }

    private void RemoveCardFromHand(int cardIndex)
    {
        // Remove the card from the hand
        playerHand.RemoveAt(cardIndex);

        handUIImages[cardIndex].sprite = null;
        handUIImages[cardIndex].gameObject.SetActive(false);
        handUIImages[cardIndex].transform.localScale = Vector3.one * defaultScale; // Reset scale

        UpdateCardUI();
    }

    private void UpdateCardUI()
    {
        for (int i = 0; i < handUIImages.Count; i++)
        {
            if (i < playerHand.Count)
            {
                // Update the UI with the remaining cards
                handUIImages[i].sprite = playerHand[i].sprite;
                handUIImages[i].gameObject.SetActive(true);
            }
            else
            {
                // Disable unused card slots
                handUIImages[i].gameObject.SetActive(false);
            }
        }
    }


}


