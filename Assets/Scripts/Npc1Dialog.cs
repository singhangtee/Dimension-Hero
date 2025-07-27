using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class Npc1Dialog : MonoBehaviour, IInteractable
{
    public NpcDialog dialogData;
    public GameObject dialogPanel;
    public TMP_Text dialogText;
    public TMP_Text nameText;

    private int _dialogIdx;
    private bool _isTyping;
    private bool _isDialogActive;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (!other.CompareTag("Player")) return;
        
        Interact();
    }

    public void Interact()
    {
        // If no dialog data or the game is paused and no dialog is active
        // if (!dialogData || !_isDialogActive) return;

        if (_isDialogActive) NextLine();
        else StartDialog();
    }

    public bool CanInteract()
    {
        return !_isDialogActive;
    }

    void StartDialog()
    {
        _isDialogActive = true;
        _dialogIdx = 0;
        
        nameText.SetText(dialogData.npcName);
        
        dialogPanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (_isTyping)
        {
            StopAllCoroutines();
            dialogText.SetText(dialogData.dialogLines[_dialogIdx]);
            _isTyping = false;
        }
        else if (++_dialogIdx < dialogData.dialogLines.Length)
        {
            // Type next line
            StartCoroutine(TypeLine());
        }
        else EndDialog();
        
    }

    IEnumerator TypeLine()
    {
        _isTyping = true;
        dialogText.SetText("");

        foreach (char letter in dialogData.dialogLines[_dialogIdx])
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(dialogData.typingSpeed);
        }
        
        _isTyping = false;

        if (dialogData.autoProgressLines.Length <= _dialogIdx || !dialogData.autoProgressLines[_dialogIdx]) yield break;
        yield return new WaitForSeconds(dialogData.autoProgressDelay);
        NextLine();
    }

    public void EndDialog()
    {
        StopAllCoroutines();
        _isDialogActive = false;
        dialogText.SetText("");
        dialogPanel.SetActive(false);
    }
}
