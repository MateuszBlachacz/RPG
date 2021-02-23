using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string uniquieId = Guid.NewGuid().ToString();
        public string text;
        public Rect rect = new Rect(10,10, 200, 150);
        public List<string> children = new List<string>();

    }
}

