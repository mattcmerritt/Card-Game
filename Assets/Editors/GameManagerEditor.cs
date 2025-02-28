using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : UnityEditor.Editor
{
    private bool showDrawPile = false;
    private bool showFullDrawPile = false;
    private bool showDiscardPile = false;
    private bool showFullDiscardPile = false;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GameManager attachedScript = (GameManager) target;

        Stack<Card> drawPile, discardPile;
        (drawPile, discardPile) = attachedScript.GetStackCopies();

        // draw pile foldout
        if (drawPile != null) {
            showDrawPile = EditorGUILayout.Foldout(showDrawPile, "Draw Pile");

            if (showDrawPile) {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField($"Size: {drawPile.Count}");
                EditorGUILayout.LabelField($"Next: {(drawPile.Count > 0 ? drawPile.Peek().ToString() : "None, stack empty")}");
                showFullDrawPile = EditorGUILayout.Foldout(showFullDrawPile, "Full Pile");
                if (showFullDrawPile) {
                    EditorGUI.indentLevel++;
                    if (drawPile.Count == 0) {
                        EditorGUILayout.LabelField("No elements to display.");
                    }
                    else {
                        foreach (Card c in drawPile) {
                            EditorGUILayout.LabelField($"{c.ToString()}");
                        }
                    }
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
        }
        else {
            EditorGUILayout.LabelField("Draw Pile is null.");
        }

        // discard pile foldout
        if (discardPile != null) {
            showDiscardPile = EditorGUILayout.Foldout(showDiscardPile, "Discard Pile");
            if (showDiscardPile) {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField($"Size: {discardPile.Count}");
                EditorGUILayout.LabelField($"Next: {(discardPile.Count > 0 ? discardPile.Peek().ToString() : "None, stack empty")}");
                showFullDiscardPile = EditorGUILayout.Foldout(showFullDiscardPile, "Full Pile");
                if (showFullDiscardPile) {
                    EditorGUI.indentLevel++;
                    if (discardPile.Count == 0) {
                        EditorGUILayout.LabelField("No elements to display.");
                    }
                    else {
                        foreach (Card c in discardPile) {
                            EditorGUILayout.LabelField($"{c.ToString()}");
                        }
                    }
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
        }
        else {
            EditorGUILayout.LabelField("Discard Pile is null.");
        }
    }
}
