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
                string newUniquieId = "";
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.LabelField("uniqueId");
                    newUniquieId = EditorGUILayout.TextField(node.uniquieId);
                    EditorGUILayout.LabelField("Text");
                    newText = EditorGUILayout.TextField(node.text);

                    if (EditorGUI.EndChangeCheck()) {
                        //NOT necesery to set object as a dirty it was mark automaticliy
                        Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                        node.uniquieId = newUniquieId;
                        node.text = newText;

                        EditorUtility.SetDirty(selectedDialogue);
                    }
                }
            }
        }
    }

}
