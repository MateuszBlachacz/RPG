using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor { 

    public class DialogueEditor : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor),false, "Dialogue Editor");
        }

        [OnOpenAssetAttribute(1)]
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
    }

}
