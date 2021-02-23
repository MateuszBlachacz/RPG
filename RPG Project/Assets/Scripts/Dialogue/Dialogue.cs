using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        void Awake()
        {
            Debug.Log("Awake");
            if(nodes.Count == 0)
            {
                DialogueNode rootNode = new DialogueNode();
                if("" == rootNode.uniquieId)
                rootNode.uniquieId = Guid.NewGuid().ToString();
                nodes.Add(rootNode);
            }
            OnValidate();
        }
#endif
        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.uniquieId] = node;
            }
        }
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childId in parentNode.children)
            {
                if (nodeLookup.ContainsKey(childId))
                {
                    yield return nodeLookup[childId];
                }
            }
        }

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newChildNode = new DialogueNode();
            newChildNode.rect.position = setPoition(parent);
            nodes.Add(newChildNode);
            parent.children.Add(newChildNode.uniquieId);
            OnValidate();
        }

        private Vector2 setPoition(DialogueNode parent)
        {
            return parent.rect.position + new Vector2(parent.rect.w + 10f, 0f);
        }
    }
}

