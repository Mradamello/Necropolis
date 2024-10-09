using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] public Image characterIcon;
    [SerializeField] public TextMeshProUGUI characterName;
    [SerializeField] public TextMeshProUGUI dialogueArea;
    [SerializeField] public GameObject dialogueBox;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;
    [SerializeField] public float typingSpeed = 0.2f;
 
    // Start is called before the first frame update
    void Start()
    {
        dialogueArea.gameObject.SetActive(false);
        characterName.gameObject.SetActive(false);
        characterIcon.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);

        if (Instance == null)
        {
            Instance = this;
        }

        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        lines.Clear();

        foreach(DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        dialogueArea.gameObject.SetActive(true);
        characterName.gameObject.SetActive(true);
        characterIcon.gameObject.SetActive(true);
        dialogueBox.gameObject.SetActive(true);

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";

        foreach(char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
    }
}
