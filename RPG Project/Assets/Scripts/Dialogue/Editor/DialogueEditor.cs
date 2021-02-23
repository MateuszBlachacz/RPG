using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor { 

    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [NonSerialized]
        GUIStyle nodeStyle;

        [NonSerialized]
        Vector2 draggingOffset;
        [NonSerialized]
        DialogueNode draggingNode = null;

        [NonSerialized]
        DialogueNode creatingNode = null;

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

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
           // nodeStyle.normal.textColor = Color.white;
            nodeStyle.padding = new RectOffset(20, 20 ,20 ,20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
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
                ProcessEvents();

                EditorGUILayout.LabelField(selectedDialogue.name);
                string newText = "";
                string newUniquieId = "";
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(out newText, out newUniquieId, node);
                }
                if (null != creatingNode) {
                    Undo.RecordObject(selectedDialogue, "Added Dialogue Node");
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
            }
            
        }


        void ProcessEvents()
        {
            
            if (Event.current.type == EventType.MouseDown && draggingNode == null) {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (null != draggingNode) draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Move Dialogue");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
               // draggingNode.rect.Contains();
               // MyDebug.info(this, ("Mouse Postion", Event.current.mousePosition));
                GUI.changed = true;// or Repaint
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode !=null)
            {
                draggingNode = null;
               // selectedDialogue.GetRootNode().rect.position = Event.current.mousePosition;
            } 
        }

        private void DrawNode(out string newText, out string newUniquieId, DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();
            //EditorGUILayout.LabelField("Node:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("uniqueId");
            newUniquieId = EditorGUILayout.TextField(node.uniquieId);
            EditorGUILayout.LabelField("Text");
            newText = EditorGUILayout.TextField(node.text);

            if (EditorGUI.EndChangeCheck())
            {
                //NOT necesery to set object as a dirty it was mark automaticliy
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
              //  node.uniquieId = newUniquieId;
                node.text = newText;

                //EditorUtility.SetDirty(selectedDialogue);
            }
            if (GUILayout.Button("+")) {
                MyDebug.info(this, ("", "Create new Node"));
                creatingNode = node; 
            }
            //foreach(DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            //{
            //    EditorGUILayout.LabelField(childNode.text);
            //}
            
            GUILayout.EndArea();
        }
        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.rect.xMax, node.rect.center.y);
            
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                //Vector3 startPosition = node.rect.center;
                //startPosition.x += node.rect.width / 2;
                //Vector3 endPosition = childNode.rect.center;
                //endPosition.x -= node.rect.width / 2;

                Vector3 endPosition = new Vector2(childNode.rect.xMin, childNode.rect.center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(
                    startPosition,
                    endPosition,
                    startPosition + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.white,
                    null,
                    4f
                    );

            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode nodeAtPoint = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.rect.Contains(point))nodeAtPoint = node;
            }
            return nodeAtPoint;
        }
    }

}
