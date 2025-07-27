using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialog", menuName = "NPC Dialog")]
public class NpcDialog : ScriptableObject
{
    public string npcName;
    public string[] dialogLines;
    public bool[] autoProgressLines;
    public float typingSpeed = 0.05f;
    public float autoProgressDelay = 1.5f;
}
