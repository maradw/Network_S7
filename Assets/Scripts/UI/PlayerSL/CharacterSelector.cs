using UnityEngine;
using UnityEngine.UI;
using TMPro; 

[System.Serializable] 
public struct CharacterStats
{
    public string characterName; // Nombre opcional
    public Sprite characterSprite;
    public int life;
 
    public float speed;
    
}

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private Image characterDisplay;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI speedText;

    [SerializeField] private CharacterStats[] characters;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private int currentIndex = 0;

    private void Awake()
    {
        if (characters == null || characters.Length == 0)
        {
            Debug.LogError("CharacterSelector: No hay personajes asignados.");
            if (nextButton != null) nextButton.interactable = false;
            if (prevButton != null) prevButton.interactable = false;
            return;
        }

        if (nextButton != null) nextButton.onClick.AddListener(NextCharacter);
        if (prevButton != null) prevButton.onClick.AddListener(PreviousCharacter);

        UpdateCharacterDisplay();
    }

    private void NextCharacter()
    {
        if (characters.Length == 0) return;
        currentIndex = (currentIndex + 1) % characters.Length;
        UpdateCharacterDisplay();
    }

    private void PreviousCharacter()
    {
        if (characters.Length == 0) return;
        currentIndex = (currentIndex - 1 + characters.Length) % characters.Length;
        UpdateCharacterDisplay();
    }

    private void UpdateCharacterDisplay()
    {
        if (characters == null || characters.Length == 0 || currentIndex < 0 || currentIndex >= characters.Length)
        {
            Debug.LogWarning("CharacterSelector: No se puede actualizar la UI.");
            if (characterDisplay != null) characterDisplay.sprite = null;
            if (nameText != null) nameText.text = "Name: N/A";
            if (lifeText != null) lifeText.text = "Life: N/A";
            if (speedText != null) speedText.text = "Speed: N/A";
            return;
        }

        CharacterStats currentCharacter = characters[currentIndex];

        if (characterDisplay != null)
            characterDisplay.sprite = currentCharacter.characterSprite;

        if (nameText != null)
            nameText.text = currentCharacter.characterName;

        if (lifeText != null)
            lifeText.text = "❤️ Vida: " + currentCharacter.life;

        if (speedText != null)
            speedText.text = "🏃 Velocidad: " + currentCharacter.speed.ToString("F1");
    }

    public int GetSelectedCharacterIndex() => currentIndex;

    public CharacterStats GetSelectedCharacterData()
    {
        if (characters != null && characters.Length > 0 &&
            currentIndex >= 0 && currentIndex < characters.Length)
        {
            return characters[currentIndex];
        }

        Debug.LogError("CharacterSelector: No se pudo obtener el personaje.");
        return default;
    }
}