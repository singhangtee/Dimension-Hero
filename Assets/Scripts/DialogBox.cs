using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class DialogBox : MonoBehaviour, IInteractable
{
    [Header("Essential Variables")]
    public NpcDialog dialogData;
    public GameObject dialogPanel;
    public TMP_Text dialogText;
    public TMP_Text nameText;

    [Header("Boolean Flags and Player")]
    private int _dialogIdx;
    private bool _isTyping;
    private bool _isDialogActive;
    private bool _firstEncounter = true;
    private Camera _camera;
    private Player _player;
    
    void Awake()
    {
        _camera = Camera.main;
        _player = FindObjectOfType<Player>();
    }
    
    void Update() {
        // Check if camera is focused on NPC, otherwise make the dialog box disappear
        Vector3 viewportPos = _camera.WorldToViewportPoint(transform.position);
        bool isVisible = viewportPos.x is >= 0 and <= 1 &&
                         viewportPos.y is >= 0 and <= 1 &&
                         viewportPos.z > 0;
        
        if (!isVisible) EndDialog();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // if (_firstEncounter) _player.CanMove = false;
        Interact();
    }

    public void Interact()
    {
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
        _player.CanMove = true;
        
        // Don't freeze the player again after first encounter
        _firstEncounter = false;
    }
}
