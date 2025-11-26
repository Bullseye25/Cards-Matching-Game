using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    #region Fields

    [Header("Card Info")]
    [SerializeField] private int cardID;

    [Header("Components")]
    [SerializeField] private Image card;
    [SerializeField] private Animator anim;

    private const string ANIM_TRIGGER = "Flip";
    private bool isFound = false;

    #endregion

    #region Properties

    public bool IsFound
    {
        get => isFound;
        set => isFound = value;
    }

    public int CardID
    {
        get => cardID;
        set => cardID = value;
    }

    public Image Card
    {
        get => card;
        set => card = value;
    }

    #endregion

    #region Unity Methods

    private IEnumerator Start()
    {
        // Short delay to ensure all components initialized
        yield return new WaitForSeconds(0.2f);

        // Cache Animator component if not assigned
        if (anim == null)
            anim = GetComponent<Animator>();

        // Allow player to see cards before starting
        yield return new WaitForSeconds(1.5f);

        // Hide card and enable animation
        SetCardActive(1);
        anim.enabled = true;
        anim.Play(0);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Called when the player selects the card.
    /// Handles validation and flips card if valid.
    /// </summary>
    public void OnSelect()
    {
        // Ignore if card is already flipped
        if (!anim.GetBool(ANIM_TRIGGER))
            return;

        // Ignore if pair is already found
        if (isFound)
            return;

        // Ignore if game is currently occupied
        if (GameManager.Instance.GameState == GameState.OCCUPIED)
            return;

        Flip(1);
        GameManager.Instance.SetGameOccupied(this);
    }

    /// <summary>
    /// Flips the card on/off.
    /// </summary>
    /// <param name="value"></param>
    public void Flip(int value) 
    { 
        anim.SetBool(ANIM_TRIGGER, value == 1 ? false : true); 
    }

    /// <summary>
    /// Activates or deactivates the card GameObject.
    /// </summary>
    /// <param name="active">True = active, False = inactive</param>
    public void SetCardActive(int value)
    {
        if (card != null)
            card.gameObject.SetActive(value == 1 ? false : true);
    }

    #endregion
}
