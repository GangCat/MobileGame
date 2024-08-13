using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Note : EditorWindow
{
    public int i;

    private Vector2 scrollPos = Vector2.zero;
    [SerializeField] private bool fixation;

    private void OnGUI()
    {
        if (fixation)
            Focus();

        if (mouseOverWindow == this || focusedWindow == this)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Space(10f);
            if (GUILayout.Button("Add", EditorStyles.toolbarButton))
            {
                StickyNotes.notes.Add(string.Empty);
                StickyNotes.noteWindows.Add(null);
            }
            GUILayout.Space(10f);
            Undo.RecordObject(this, "Undo Fixation");
            fixation = GUILayout.Toggle(fixation, "Fixation", EditorStyles.toolbarButton);
            GUILayout.Space(position.width - 300f);
            if (GUILayout.Button("List", EditorStyles.toolbarButton))
            {
                StickyNotes.OpenWindow();
            }
            GUILayout.Space(10f);
            if (GUILayout.Button("Delete", EditorStyles.toolbarButton))
            {
                Close();
                StickyNotes.notes.RemoveAt(i);
                StickyNotes.noteWindows.RemoveAt(i);
            }
            GUILayout.Space(10f);
            if (GUILayout.Button("Close", EditorStyles.toolbarButton))
                Close();
            GUILayout.Space(10f);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        if (StickyNotes.notes.Count > i)
        {
            if (StickyNotes.notes[i].Length > 10)
                titleContent = new GUIContent(StickyNotes.notes[i].Substring(0, 10) + "...");
            else
                titleContent = new GUIContent(StickyNotes.notes[i]);
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Space(5f);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(5f);
        StickyNotes.notes[i] = EditorGUILayout.TextArea(StickyNotes.notes[i], EditorStyles.textArea, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.Space(5f);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5f);
        EditorGUILayout.EndVertical();
        GUILayout.EndScrollView();

        if (GUI.changed)
            Repaint();
    }

    private void OnEnable()
    {
        position = new Rect(Vector2.one * 50f, Vector2.one * 260f);
        minSize = new Vector2(260f, 60f);
    }
}