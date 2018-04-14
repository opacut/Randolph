using UnityEngine;
using UnityEditor;
using System;

public class EditorGUIExtension {

    /// <summary>
    /// Creates an array foldout like in inspectors for SerializedProperty of array type.
    /// Counterpart for standard EditorGUILayout.PropertyField which doesn't support SerializedProperty of array type.
    /// </summary>
    public static void ArrayField(SerializedProperty property) {
        // EditorGUIUtility.LookLikeInspector ();
        bool wasEnabled = GUI.enabled;
        int prevIdentLevel = EditorGUI.indentLevel;

        // Iterate over all child properties of array
        bool childrenAreExpanded = true;
        int propertyStartingDepth = property.depth;
        while (property.NextVisible(childrenAreExpanded) && propertyStartingDepth < property.depth) {
            childrenAreExpanded = EditorGUILayout.PropertyField(property);
        }

        EditorGUI.indentLevel = prevIdentLevel;
        GUI.enabled = wasEnabled;
    }

    /// <summary>
    /// Creates a filepath textfield with a browse button. Opens the open file panel.
    /// </summary>
    public static string FileLabel(string name, float labelWidth, string path, string extension) {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(name, GUILayout.MaxWidth(labelWidth));
        string filepath = EditorGUILayout.TextField(path);
        if (GUILayout.Button("Browse")) {
            filepath = EditorUtility.OpenFilePanel(name, path, extension);
        }
        EditorGUILayout.EndHorizontal();
        return filepath;
    }

    /// <summary>
    /// Creates a folder path textfield with a browse button. Opens the save folder panel.
    /// </summary>
    public static string FolderLabel(string name, float labelWidth, string path) {
        EditorGUILayout.BeginHorizontal();
        string filepath = EditorGUILayout.TextField(name, path);
        if (GUILayout.Button("Browse", GUILayout.MaxWidth(60))) {
            filepath = EditorUtility.SaveFolderPanel(name, path, "Folder");
        }
        EditorGUILayout.EndHorizontal();
        return filepath;
    }

    /// <summary>
    /// Creates an array foldout like in inspectors. Hand editable ftw!
    /// </summary>
    public static string[] ArrayFoldout(string label, string[] array, ref bool foldout) {
        EditorGUILayout.BeginVertical();
        // EditorGUIUtility.LookLikeInspector();
        foldout = EditorGUILayout.Foldout(foldout, label);
        string[] newArray = array;
        if (foldout) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            int arraySize = EditorGUILayout.IntField("Size", array.Length);
            if (arraySize != array.Length) newArray = new string[arraySize];
            for (int i = 0; i < arraySize; i++) {
                string entry = "";
                if (i < array.Length) entry = array[i];
                newArray[i] = EditorGUILayout.TextField("Element " + i, entry);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        return newArray;
    }

    /// <summary>
    /// Creates a toolbar that is filled in from an Enum. Useful for setting tool modes.
    /// </summary>
    public static Enum EnumToolbar(Enum selected) {
        string[] toolbar = Enum.GetNames(selected.GetType());
        Array values = Enum.GetValues(selected.GetType());

        for (int i = 0; i < toolbar.Length; i++) {
            string toolname = toolbar[i];
            toolname = toolname.Replace("_", " ");
            toolbar[i] = toolname;
        }

        int selectedIndex = 0;
        while (selectedIndex < values.Length) {
            if (selected.ToString() == values.GetValue(selectedIndex).ToString()) {
                break;
            }
            selectedIndex++;
        }
        selectedIndex = GUILayout.Toolbar(selectedIndex, toolbar);
        return (Enum) values.GetValue(selectedIndex);
    }

    /// <summary>
    /// Creates a button that can be toggled. Looks nice than GUI.toggle
    /// </summary>
    /// <returns>
    /// Toggle state
    /// </returns>
    /// <param name='state'>
    /// If set to <c>true</c> state.
    /// </param>
    /// <param name='label'>
    /// If set to <c>true</c> label.
    /// </param>
    public static bool ToggleButton(bool state, string label) {
        BuildStyle();

        bool outBool = false;

        outBool = (state)
                ? GUILayout.Button(label, toggledStyle)
                : GUILayout.Button(label);

        if (outBool) return !state;
        else return state;
    }
	
	// Show child property of parent serializedProperty
	void ShowRelativeProperty(SerializedProperty serializedProperty, string propertyName) {
		SerializedProperty property = serializedProperty.FindPropertyRelative(propertyName);
		if (property != null) {
			EditorGUI.indentLevel++;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(property, true);
			if (EditorGUI.EndChangeCheck()) serializedProperty.serializedObject.ApplyModifiedProperties();
			// EditorGUIUtility.LookLikeControls();
			EditorGUI.indentLevel--;
		}
	}

    public class ModalPopupWindow : EditorWindow {

        public event Action<bool> OnChosen;
        string popText = "";
        string trueText = "Yes";
        string falseText = "No";

        public void SetValue(string text, string accept, string no) {
            popText = text;
            trueText = accept;
            falseText = no;
        }

        void OnGUI() {
            GUILayout.BeginVertical();
            GUILayout.Label(popText);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(trueText)) {
                OnChosen?.Invoke(true);
                Close();
            }
            if (GUILayout.Button(falseText)) {
                OnChosen?.Invoke(false);
                Close();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

    }

//	public static bool ModalPopup(string text, string trueText, string falseText)
//	{
//		ModalPopupWindow popwindow = (ModalPopupWindow) EditorWindow.GetWindow(typeof(EditorGUIExtension.ModalPopupWindow));
//		popwindow.title = "Modal";
//		popwindow.SetValue(text, trueText, falseText);
//		popwindow.OnChosen += delegate(bool retValue) {
//			return retValue;
//		};
//	}

    static GUIStyle toggledStyle;

    public GUIStyle StyleButtonToggled {
        get {
            BuildStyle();
            return toggledStyle;
        }
    }

    static GUIStyle labelTextStyle;

    public static GUIStyle StyleLabelText {
        get {
            BuildStyle();
            return labelTextStyle;
        }
    }

    private static void BuildStyle() {
        if (toggledStyle == null) {
            toggledStyle = new GUIStyle(GUI.skin.button);
            toggledStyle.normal.background = toggledStyle.onActive.background;
            toggledStyle.normal.textColor = toggledStyle.onActive.textColor;
        }
        if (labelTextStyle == null) {
            labelTextStyle = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).textField) {
                    normal = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).button.onNormal
            };
        }
    }

}
