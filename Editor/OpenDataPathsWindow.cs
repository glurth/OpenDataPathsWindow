using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;

public class OpenDataPathsWindow : EditorWindow
{
    // String constants for labels, messages, and paths
    private const string WINDOW_TITLE = "Open Data Path Window";
    private const string MENU_ITEM = "File/Open Data Paths";
    private const string EDITOR_PREFS_KEY = "AlternateExplorerPath";
    private const string LABEL_PERSISTENT_DATA_PATH = "Persistent Data Path:";
    private const string LABEL_DATA_PATH = "Data Path:";
    private const string LABEL_OPEN_PERSISTENT = "Open";
    private const string LABEL_OPEN_DATA = "Open";
    private const string LABEL_CANCEL = "Cancel";
    private const string TOOLTIP_ALTERNATE_EXPLORER = "Alternate Explorer";
    private const string LABEL_PATH_TO_EXPLORER = "Path to Explorer:";
    private const string LABEL_SET_ALTERNATE_EXPLORER = "Set Alternate Explorer";
    private const string LOG_SET_ALTERNATE_EXPLORER = "Alternate explorer path set to: ";
    private const string WARNING_FILE_DOES_NOT_EXIST = "The file does not exist.";
    private const string WARNING_UNSUPPORTED_PLATFORM = "Unsupported platform for opening file explorer.";

    // Integer constants for layout dimensions
    private const int LABEL_WIDTH = 140;
    private const int BUTTON_WIDTH = 50;
    private const int GEAR_ICON_SIZE = 30;
    private const int WARNING_ICON_SIZE = 20;
    private const int SINGLE_LINE_HEIGHT = 20;
    private const int WINDOW_BASE_HEIGHT = 120;
    private const int WINDOW_EXPANDED_HEIGHT = 200;

    // Variables
    private string alternateExplorerPath;
    private static Texture2D warningIcon;
    private static Texture2D gearIcon;
    private bool showAlternateExplorerOptions = false;

    [MenuItem(MENU_ITEM)]
    public static void ShowWindow()
    {
        OpenDataPathsWindow window = GetWindow<OpenDataPathsWindow>(WINDOW_TITLE);
        window.UpdateMinSize();
    }

    private void OnEnable()
    {
        // Load the built-in warning and gear icons
        warningIcon = EditorGUIUtility.FindTexture("console.warnicon");
        gearIcon = EditorGUIUtility.IconContent("d_SettingsIcon").image as Texture2D;

        // Initialize the alternate explorer path from EditorPrefs
        alternateExplorerPath = EditorPrefs.GetString(EDITOR_PREFS_KEY, string.Empty);
    }

    private void UpdateMinSize()
    {
        // Adjust the minimum size based on the visibility of alternate explorer options
        this.minSize = new Vector2(300, showAlternateExplorerOptions ? WINDOW_EXPANDED_HEIGHT : WINDOW_BASE_HEIGHT);
    }

    private void OnGUI()
    {
        // Persistent Data Path Row
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(LABEL_PERSISTENT_DATA_PATH, EditorStyles.boldLabel, GUILayout.Width(LABEL_WIDTH));
        EditorGUILayout.LabelField(Application.persistentDataPath, EditorStyles.textField, GUILayout.Height(SINGLE_LINE_HEIGHT));
        if (GUILayout.Button(LABEL_OPEN_PERSISTENT, GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(SINGLE_LINE_HEIGHT)))
        {
            OpenInFileBrowser(Application.persistentDataPath);
            Close();
        }
        EditorGUILayout.EndHorizontal();

        // Data Path Row
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(LABEL_DATA_PATH, EditorStyles.boldLabel, GUILayout.Width(LABEL_WIDTH));
        EditorGUILayout.LabelField(Application.dataPath, EditorStyles.textField, GUILayout.Height(SINGLE_LINE_HEIGHT));
        if (GUILayout.Button(LABEL_OPEN_DATA, GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(SINGLE_LINE_HEIGHT)))
        {
            OpenInFileBrowser(Application.dataPath);
            Close();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (GUILayout.Button(LABEL_CANCEL, GUILayout.ExpandWidth(true)))
        {
            Close();
        }

        GUILayout.Space(20);

        // Gear button to toggle the alternate explorer area
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent(gearIcon, TOOLTIP_ALTERNATE_EXPLORER), GUILayout.Width(GEAR_ICON_SIZE), GUILayout.Height(GEAR_ICON_SIZE)))
        {
            showAlternateExplorerOptions = !showAlternateExplorerOptions;
            UpdateMinSize(); // Update the minimum size when toggling the alternate explorer options
        }
        EditorGUILayout.EndHorizontal();

        if (showAlternateExplorerOptions)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField(LABEL_PATH_TO_EXPLORER, EditorStyles.boldLabel);

            // Draw the text field with optional warning icon
            EditorGUILayout.BeginHorizontal();
            alternateExplorerPath = EditorGUILayout.TextField(alternateExplorerPath);

            if (!string.IsNullOrEmpty(alternateExplorerPath) && !File.Exists(alternateExplorerPath))
            {
                GUILayout.Label(new GUIContent(warningIcon, WARNING_FILE_DOES_NOT_EXIST), GUILayout.Width(WARNING_ICON_SIZE));
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button(LABEL_SET_ALTERNATE_EXPLORER))
            {
                EditorPrefs.SetString(EDITOR_PREFS_KEY, alternateExplorerPath);
                Debug.Log(LOG_SET_ALTERNATE_EXPLORER + alternateExplorerPath);
            }

            EditorGUILayout.EndVertical();
        }
    }

    private void OpenInFileBrowser(string path)
    {
        if (!string.IsNullOrEmpty(alternateExplorerPath) && File.Exists(alternateExplorerPath))
        {
            Process.Start(alternateExplorerPath, "\"" + path + "\"");
        }
        else
        {
            // Windows
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                Process.Start("explorer.exe", path.Replace("/", "\\"));
            }
            // macOS
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                Process.Start("open", path);
            }
            // Linux
            else if (Application.platform == RuntimePlatform.LinuxEditor)
            {
                Process.Start("xdg-open", path);
            }
            else
            {
                Debug.LogWarning(WARNING_UNSUPPORTED_PLATFORM);
            }
        }
    }
}
