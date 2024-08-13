using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;

public class StickyNotes : EditorWindow
{
    public static StickyNotes window;

    [SerializeField] public static List<string> notes = new List<string>();
    [SerializeField] public static List<Note> noteWindows = new List<Note>();
    private Vector2 scrollPos;
    [SerializeField] private string searchText;

    [MenuItem("Tools/Sticky Notes")]
    public static void OpenWindow()
    {
        window = GetWindow<StickyNotes>();
        window.minSize = new Vector2(300f, 300f);
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Space(10f);
        if (GUILayout.Button("Add", EditorStyles.toolbarButton))
        {
            notes.Add(string.Empty);
            noteWindows.Add(null);
        }
        GUILayout.Space(10f);
        if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
        {
            notes.Clear();
            noteWindows.Clear();
        }
        GUILayout.Space(window.position.width - 190f);
        if (GUILayout.Button("Close", EditorStyles.toolbarButton))
            window.Close();
        GUILayout.Space(10f);
        if (GUILayout.Button("Help", EditorStyles.toolbarButton))
            Application.OpenURL("https://drive.google.com/file/d/1m_LCUWmRYjpKAsh_T1kAgQBE_TRT4aNA/view?usp=sharing");
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5f);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label(new GUIContent("StickyNotes", "Made by DHstudio"), new GUIStyle() { fontSize = 30, fontStyle = FontStyle.Bold, normal = new GUIStyleState() { textColor = new Color(1f, 1f, 1f, 0.3f) } });
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10f);
        searchText = EditorGUILayout.TextField(searchText, EditorStyles.toolbarSearchField);
        GUILayout.Space(5f);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10f);
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < notes.Count; i++)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Space(5f);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(5f);
                notes[i] = EditorGUILayout.TextField(notes[i], EditorStyles.textArea, GUILayout.MaxWidth(500f), GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                GUILayout.Space(5f);

                if (GUILayout.Button("∧", EditorStyles.miniButtonLeft, GUILayout.Width(30f)) && i > 0)
                {
                    notes.Insert(i - 1, notes[i]);
                    notes.RemoveAt(i + 1);
                    noteWindows.Insert(i - 1, noteWindows[i]);
                    noteWindows.RemoveAt(i + 1);
                    for (int n = 0; n < noteWindows.Count; n++)
                    {
                        if (noteWindows[n])
                        {
                            if (n != i)
                                noteWindows[n].i--;
                            else
                                noteWindows[n].i++;
                        }
                    }
                }
                else if (GUILayout.Button("∨", EditorStyles.miniButtonMid, GUILayout.Width(30f)) && i < notes.Count - 1)
                {
                    notes.Insert(i + 2, notes[i]);
                    notes.RemoveAt(i);
                    noteWindows.Insert(i + 2, noteWindows[i]);
                    noteWindows.RemoveAt(i);
                    for (int n = 0; n < noteWindows.Count; n++)
                    {
                        if (noteWindows[n])
                        {
                            if (n != i)
                                noteWindows[n].i++;
                            else
                                noteWindows[n].i--;
                        }
                    }
                }
                else if (GUILayout.Button("-", EditorStyles.miniButtonMid, GUILayout.Width(30f)))
                {
                    notes.RemoveAt(i);
                    if (noteWindows[i])
                        noteWindows[i].Close();
                    noteWindows.RemoveAt(i);
                }

                if (noteWindows.Count > 0 && i < noteWindows.Count)
                {
                    if (!noteWindows[i])
                    {
                        if (GUILayout.Button("Open", EditorStyles.miniButtonRight, GUILayout.Width(50f)))
                        {
                            noteWindows[i] = CreateInstance<Note>();
                            if (notes[i].Length > 10)
                                noteWindows[i].titleContent = new GUIContent(notes[i].Substring(0, 10) + "...");
                            else
                                noteWindows[i].titleContent = new GUIContent(notes[i]);
                            noteWindows[i].Show();
                            noteWindows[i].i = i;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Close", EditorStyles.miniButtonRight, GUILayout.Width(50f)))
                        {
                            noteWindows[i].Close();
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5f);
                EditorGUILayout.EndVertical();
                GUILayout.Space(5f);
            }
            else
            {
                if (notes[i].ToLower().Contains(searchText.ToLower()))
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(5f);
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(5f);
                    notes[i] = EditorGUILayout.TextField(notes[i], GUILayout.MaxWidth(500f), GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                    GUILayout.Space(5f);

                    if (GUILayout.Button("∧", EditorStyles.miniButtonLeft, GUILayout.Width(30f)) && i > 0)
                    {
                        notes.Insert(i - 1, notes[i]);
                        notes.RemoveAt(i + 1);
                    }
                    else if (GUILayout.Button("∨", EditorStyles.miniButtonMid, GUILayout.Width(30f)) && i < notes.Count - 1)
                    {
                        notes.Insert(i + 2, notes[i]);
                        notes.RemoveAt(i);
                    }
                    else if (GUILayout.Button("-", EditorStyles.miniButtonMid, GUILayout.Width(30f)))
                    {
                        notes.RemoveAt(i);
                        if (noteWindows[i])
                            noteWindows[i].Close();
                        noteWindows.RemoveAt(i);
                    }
                    if (!noteWindows[i])
                    {
                        if (GUILayout.Button("Open", EditorStyles.miniButtonRight, GUILayout.Width(50f)))
                        {
                            noteWindows[i] = CreateInstance<Note>();
                            if (notes[i].Length > 10)
                                noteWindows[i].titleContent = new GUIContent(notes[i].Substring(0, 10) + "...");
                            else
                                noteWindows[i].titleContent = new GUIContent(notes[i]);
                            noteWindows[i].Show();
                            noteWindows[i].i = i;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Close", EditorStyles.miniButtonRight, GUILayout.Width(50f)))
                        {
                            noteWindows[i].Close();
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                    EditorGUILayout.EndVertical();
                    GUILayout.Space(5f);
                }
            }
        }
        GUILayout.EndScrollView();

        if (GUI.changed)
            Repaint();
    }

    private void OnEnable()
    {
        notes.Clear();
        noteWindows.Clear();
        if (PlayerPrefs.GetString("Notes") != string.Empty)
            notes = PlayerPrefs.GetString("Notes").Split(Convert.ToChar(",")).ToList();
        if (PlayerPrefs.GetString("NoteWindows") != string.Empty)
        {
            string[] noteWins = PlayerPrefs.GetString("NoteWindows").Split(Convert.ToChar(","));
            for (int i = 0; i < noteWins.Length; i++)
            {
                int instanceID;
                int.TryParse(noteWins[i], out instanceID);

                if (instanceID != 0)
                    noteWindows.Add(EditorUtility.InstanceIDToObject(instanceID) as Note);
                else
                    noteWindows.Add(null);
            }
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.DeleteKey("Notes");
        string resultString;
        if (notes.Count > 0)
        {
            if (string.IsNullOrEmpty(notes[0]))
                resultString = " ";
            else
                resultString = notes[0];
            for (int i = 1; i < notes.Count; i++)
            {
                if (string.IsNullOrEmpty(notes[i]))
                    resultString += ", ";
                else
                    resultString += "," + notes[i];
            }
            PlayerPrefs.SetString("Notes", resultString);
        }
        else
            PlayerPrefs.SetString("Notes", string.Empty);
        if (noteWindows.Count > 0)
        {
            if (noteWindows[0])
                resultString = noteWindows[0].GetInstanceID().ToString();
            else
                resultString = "0";
            for (int i = 1; i < noteWindows.Count; i++)
            {
                if (noteWindows[i])
                    resultString += "," + noteWindows[i].GetInstanceID().ToString();
                else
                    resultString += ",0";
            }
            PlayerPrefs.SetString("NoteWindows", resultString);
        }
        else
            PlayerPrefs.SetString("NoteWindows", string.Empty);
    }
}