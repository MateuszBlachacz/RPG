using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor { 

    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor),false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instaceId, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instaceId) as Dialogue;
            if (dialogue)
            {
                ShowEditorWindow();

                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged; 
        }

        private void OnSelectionChanged()
        {
            selectedDialogue = Selection.activeObject as Dialogue;
            
            if (selectedDialogue) {
                Repaint();
            }
            Debug.Log("On selection change");
            
        }
        private void OnGUI()
        {
            if (null == selectedDialogue)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
            } 
            else
            {
                EditorGUILayout.LabelField(selectedDialogue.name);
                string newText = "";
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                     newText = EditorGUILayout.TextField(node.text);
                    if (newText != node.text) {
                        node.text = newText;
                        EditorUtility.SetDirty(selectedDialogue);
                    }
                }
            }
        }
    }

}
