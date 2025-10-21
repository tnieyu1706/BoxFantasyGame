#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

public class JsonEditor : EditorWindow
{
    private GUIStyle inputStyle;
    private GUIStyle buttonStyle;
    private GUIStyle labelStyle;
    private GUIStyle pathStyle;
    private GUIStyle foldoutStyle;
    private GUIStyle fieldStyle;
    private GUILayoutOption[] fieldLayout;

    private Type type;
    private JObject jsonContext;
    private TextAsset textAsset;
    private Dictionary<string, (FieldInfo, object)> unityObjects;
    private Dictionary<string, bool> labels;
    private List<FieldInfo> fields;
    private string newFilename = "";
    private string selectedDatatype = "";
    private bool initialized = false;
    private string previousLabelType = "";

    private Vector2 scrollPosition;

    [MenuItem("Tools/Data/JsonEditor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(JsonEditor));
    }

    public void OnEnable()
    {
        Selection.selectionChanged += Initialize;
    }

    public void OnGUI()
    {
        inputStyle = new GUIStyle("TextField") { fontSize = 14, fixedHeight = 25, alignment = TextAnchor.MiddleLeft };
        buttonStyle = new GUIStyle("Button") { fontSize = 16 };
        pathStyle = new GUIStyle("Label") { fontSize = 12, fixedHeight = 30 };
        labelStyle = new GUIStyle("Label") { fontSize = 25, fixedHeight = 30 };
        foldoutStyle = new GUIStyle("Foldout") { fontSize = 16 };
        fieldStyle = new GUIStyle("Label") { fontSize = 14, fixedHeight = 25 };
        fieldLayout = new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.MinWidth(Screen.width / 2) };

        RenderWindow();
    }

    public void RenderWindow()
    {
        if (initialized && type != null && fields != null && textAsset != null)
        {
            GUILayout.Label(type.Name, EditorStyles.boldLabel);

            // Bắt đầu vùng cuộn
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width),
                GUILayout.Height(position.height - 100));

            GUILayout.BeginArea(new Rect(25, 100, position.width - 50, position.height - 150));
            GUILayout.Label(type.Name + " - " + textAsset.name, labelStyle);
            GUILayout.Space(10);
            GUILayout.Label("", GUI.skin.horizontalSlider);
            GUILayout.Space(25);
            ProcessFields();
            GUILayout.Space(50);
            PrintDatatypeSelector();
            GUILayout.Space(25);
            GUILayout.BeginHorizontal();
            labelStyle.fontSize = 20;
            if (GUILayout.Button("Save", buttonStyle, GUILayout.Height(30), GUILayout.Width(150))) SaveJson();
            if (GUILayout.Button("Create New", buttonStyle, GUILayout.Height(30), GUILayout.Width(150))) CreateJson();
            GUILayout.EndHorizontal();
            GUILayout.Space(25);
            GUILayout.EndArea();

            // Kết thúc vùng cuộn
            EditorGUILayout.EndScrollView();
        }
        else
        {
            GUILayout.Label("Json Editor", EditorStyles.boldLabel);
            GUILayout.BeginArea(new Rect(25, 100, position.width - 50, position.height - 150));
            GUILayout.Label("Json Editor", labelStyle);
            GUILayout.Space(25);
            GUILayout.BeginVertical();
            PrintDatatypeSelector();
            GUILayout.Space(25);
            if (GUILayout.Button("Create New", buttonStyle, GUILayout.Height(30), GUILayout.Width(110))) CreateJson();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }

    private void Update()
    {
        if (initialized)
        {
            Repaint();
        }
    }

    public void Initialize()
    {
        initialized = false;

        // Initialize after a TextAsset file has been selected
        if (Selection.activeObject && Selection.activeObject.GetType() == typeof(TextAsset))
        {
            // Find compatible DataType
            try
            {
                textAsset = (TextAsset)Selection.activeObject;
                jsonContext = JObject.Parse(textAsset.text);
                unityObjects = new Dictionary<string, (FieldInfo, object)>();
                if (labels == null)
                {
                    labels = new Dictionary<string, bool>();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed parsing the json file! \n " + e);
                jsonContext = null;
            }

            // Continue if parsing succeeded
            if (jsonContext != null && !string.IsNullOrEmpty(selectedDatatype))
            {
                // Setup Selected Datatype
                type = Type.GetType(selectedDatatype);

                if (type != null)
                {
                    fields = new List<FieldInfo>(type.GetFields());

                    fields = fields.OrderBy(g =>
                            Type.GetTypeCode(g.FieldType) == TypeCode.Int16 ||
                            Type.GetTypeCode(g.FieldType) == TypeCode.Int32 ||
                            Type.GetTypeCode(g.FieldType) == TypeCode.Int64)
                        .ThenBy(g =>
                            Type.GetTypeCode(g.FieldType) == TypeCode.UInt16 ||
                            Type.GetTypeCode(g.FieldType) == TypeCode.UInt32 ||
                            Type.GetTypeCode(g.FieldType) == TypeCode.UInt64)
                        .ThenBy(g =>
                            Type.GetTypeCode(g.FieldType) == TypeCode.Single ||
                            Type.GetTypeCode(g.FieldType) == TypeCode.Double ||
                            Type.GetTypeCode(g.FieldType) == TypeCode.Decimal)
                        .ThenBy(g =>
                            Type.GetTypeCode(g.FieldType) == TypeCode.String ||
                            Type.GetTypeCode(g.FieldType) == TypeCode.Char ||
                            Type.GetTypeCode(g.FieldType) == TypeCode.DateTime)
                        .ThenBy(g => Type.GetTypeCode(g.FieldType) == TypeCode.Boolean)
                        .ThenBy(g => Type.GetTypeCode(g.FieldType) == TypeCode.Object)
                        .ThenBy(g =>
                            Type.GetTypeCode(g.FieldType) == TypeCode.Byte ||
                            Type.GetTypeCode(g.FieldType) == TypeCode.SByte)
                        .ThenBy(g => g.FieldType.IsEnum)
                        .ToList();

                    initialized = true;
                    LoadReferences();
                }
                else
                {
                    // Datatype not found
                }
            }
        }
    }

    public void ProcessFields()
    {
        GUILayout.BeginVertical();

        previousLabelType = "";
        foreach (var field in fields)
        {
            try
            {
                if (jsonContext.TryGetValue(field.Name, out JToken token) && !field.Name.EndsWith("_path") &&
                    !field.FieldType.IsEnum)
                {
                    switch (Type.GetTypeCode(field.FieldType))
                    {
                        case TypeCode.Boolean:
                            PrintHeader("Boolean");

                            if (labels["Boolean"])
                            {
                                GUILayout.BeginHorizontal();
                                PrintFieldLabel(field.Name);
                                jsonContext[field.Name] = EditorGUILayout.Toggle(token.Value<bool>(), fieldLayout);
                                GUILayout.EndHorizontal();
                            }

                            previousLabelType = "Boolean";
                            break;
                        case TypeCode.Double:
                            PrintHeader("Double");

                            if (labels["Double"])
                            {
                                GUILayout.BeginHorizontal();
                                PrintFieldLabel(field.Name);
                                jsonContext[field.Name] =
                                    EditorGUILayout.DoubleField(token.Value<double>(), inputStyle, fieldLayout);
                                GUILayout.EndHorizontal();
                            }

                            previousLabelType = "Double";
                            break;
                        case TypeCode.Decimal:
                            PrintHeader("Decimal");

                            if (labels["Decimal"])
                            {
                                GUILayout.BeginHorizontal();
                                PrintFieldLabel(field.Name);
                                jsonContext[field.Name] =
                                    (decimal)EditorGUILayout.DoubleField(token.Value<double>(), inputStyle,
                                        fieldLayout);
                                GUILayout.EndHorizontal();
                            }

                            previousLabelType = "Decimal";
                            break;
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                            PrintHeader("Integer");

                            if (labels["Integer"])
                            {
                                GUILayout.BeginHorizontal();
                                PrintFieldLabel(field.Name);
                                jsonContext[field.Name] =
                                    EditorGUILayout.IntField(token.Value<int>(), inputStyle, fieldLayout);
                                GUILayout.EndHorizontal();
                            }

                            previousLabelType = "Integer";
                            break;
                        case TypeCode.Single:
                            PrintHeader("Float");

                            if (labels["Float"])
                            {
                                GUILayout.BeginHorizontal();
                                PrintFieldLabel(field.Name);
                                jsonContext[field.Name] =
                                    EditorGUILayout.FloatField(token.Value<float>(), inputStyle, fieldLayout);
                                GUILayout.EndHorizontal();
                            }

                            previousLabelType = "Float";
                            break;
                        case TypeCode.String:
                        case TypeCode.Char:
                        case TypeCode.Byte:
                        case TypeCode.SByte:
                        case TypeCode.DateTime:
                            PrintHeader("String");

                            if (labels["String"])
                            {
                                GUILayout.BeginHorizontal();
                                PrintFieldLabel(field.Name);
                                jsonContext[field.Name] =
                                    EditorGUILayout.TextField(token.Value<string>(), inputStyle, fieldLayout);
                                GUILayout.EndHorizontal();
                            }

                            previousLabelType = "String";
                            break;
                        default:
                            ProcessOtherDatatype(field, fieldLayout);
                            break;
                    }
                }
                else
                {
                    ProcessOtherDatatype(field, fieldLayout);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            catch (Exception e)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                GUILayout.EndHorizontal();
                EditorGUILayout.HelpBox("Parsing error.", MessageType.Error);
                Debug.LogWarning("Parsing Error! \n " + e.StackTrace);
            }
        }

        GUILayout.EndVertical();
    }

    private void ProcessOtherDatatype(FieldInfo field, GUILayoutOption[] fieldLayout)
    {
        // 1. Unity Object References (Prefab / GameObject / Texture / Sprite)
        if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
        {
            PrintHeader("References");

            if (labels["References"])
            {
                if (!unityObjects.ContainsKey(field.Name))
                    unityObjects.Add(field.Name, (field, null));

                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                PrintFieldLabel(field.Name);
                GUILayout.Space(20);

                UnityEngine.Object currentObj = null;
                if (unityObjects.ContainsKey(field.Name))
                {
                    currentObj = unityObjects[field.Name].Item2 as UnityEngine.Object;
                }
                else
                {
                    unityObjects.Add(field.Name, (field, null));
                }

                // Nếu chưa có object, thử load từ JSON instanceID
                if (currentObj == null && jsonContext[field.Name]?["instanceID"] != null)
                {
                    int id = jsonContext[field.Name]["instanceID"].Value<int>();
                    currentObj = EditorUtility.InstanceIDToObject(id);
                    unityObjects[field.Name] = (field, currentObj);
                }

                // ObjectField trực tiếp
                UnityEngine.Object newObj = EditorGUILayout.ObjectField(
                    currentObj,
                    field.FieldType,
                    false,
                    fieldLayout
                );

                // Nếu thay đổi, update unityObjects + JSON
                if (newObj != currentObj)
                {
                    unityObjects[field.Name] = (field, newObj);

                    if (newObj != null)
                    {
                        jsonContext[field.Name] = new JObject
                        {
                            { "instanceID", newObj.GetInstanceID() }
                        };

                        // Optional: lưu path
                        // jsonContext[field.Name + "_path"] = FetchPath(newObj).Replace("Assets/", "");
                    }
                    else
                    {
                        jsonContext[field.Name] = null;
                    }
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }

            previousLabelType = "References";
            return;
        }

        // 2. Enum
        if (field.FieldType.IsEnum)
        {
            PrintHeader("Enum");
            if (labels["Enum"])
            {
                GUILayout.BeginHorizontal();
                PrintFieldLabel(field.Name);
                GUILayout.Space(20);

                int[] enValues = Enum.GetValues(field.FieldType).Cast<int>().ToArray();
                jsonContext[field.Name] = EditorGUILayout.IntPopup(
                    jsonContext[field.Name].Value<int>(),
                    Enum.GetNames(field.FieldType),
                    enValues
                );

                GUILayout.EndHorizontal();
            }

            previousLabelType = "Enum";
            return;
        }

        // 3. Color / Color32
        if (unityObjects.ContainsKey(field.Name) &&
            (typeof(UnityEngine.Color).IsAssignableFrom(field.FieldType) ||
             typeof(UnityEngine.Color32).IsAssignableFrom(field.FieldType)))
        {
            PrintHeader("Color");
            if (labels["Color"])
            {
                GUILayout.BeginHorizontal();
                PrintFieldLabel(field.Name);

                JHelper col = new JHelper();
                if (unityObjects[field.Name].Item2 != null)
                    col = (JHelper)unityObjects[field.Name].Item2;

                Color colorValue = col.obj != null ? (Color)col.obj : Color.white;
                Color newColor = EditorGUILayout.ColorField(colorValue, fieldLayout);

                // Update
                unityObjects[field.Name] = (field, new JHelper(newColor, col.fields));

                // Update JSON
                JObject j = new JObject
                {
                    { col.fields[0], newColor.r },
                    { col.fields[1], newColor.g },
                    { col.fields[2], newColor.b }
                };
                if (col.fields[3] != null)
                    j.Add(col.fields[3], newColor.a);

                jsonContext[field.Name] = j;

                GUILayout.EndHorizontal();
            }

            previousLabelType = "Color";
            return;
        }

        // 4. Vector3 / Vector3Int
        if (unityObjects.ContainsKey(field.Name) &&
            (typeof(UnityEngine.Vector3).IsAssignableFrom(field.FieldType) ||
             typeof(UnityEngine.Vector3Int).IsAssignableFrom(field.FieldType)))
        {
            PrintHeader("Vector3");
            if (labels["Vector3"])
            {
                GUILayout.BeginHorizontal();
                PrintFieldLabel(field.Name);
                GUILayout.Space(20);

                var helper = (JHelper)unityObjects[field.Name].Item2 ??
                             new JHelper(Vector3.zero, new string[] { "x", "y", "z" });

                if (helper.obj.GetType() == typeof(Vector3))
                {
                    Vector3 vec = (Vector3)helper.obj;
                    Vector3 newVec = EditorGUILayout.Vector3Field("", vec);
                    unityObjects[field.Name] = (field, new JHelper(newVec, helper.fields));
                    jsonContext[field.Name] = new JObject
                    {
                        { helper.fields[0], newVec.x },
                        { helper.fields[1], newVec.y },
                        { helper.fields[2], newVec.z }
                    };
                }
                else if (helper.obj.GetType() == typeof(Vector3Int))
                {
                    Vector3Int vecInt = (Vector3Int)helper.obj;
                    Vector3 newVec = EditorGUILayout.Vector3Field("", vecInt);
                    Vector3Int newVecInt = new Vector3Int(Mathf.RoundToInt(newVec.x), Mathf.RoundToInt(newVec.y),
                        Mathf.RoundToInt(newVec.z));
                    unityObjects[field.Name] = (field, new JHelper(newVecInt, helper.fields));
                    jsonContext[field.Name] = new JObject
                    {
                        { helper.fields[0], newVecInt.x },
                        { helper.fields[1], newVecInt.y },
                        { helper.fields[2], newVecInt.z }
                    };
                }

                GUILayout.EndHorizontal();
            }

            previousLabelType = "Vector3";
            return;
        }

        // 5. Vector2 / Vector2Int
        if (unityObjects.ContainsKey(field.Name) &&
            (typeof(UnityEngine.Vector2).IsAssignableFrom(field.FieldType) ||
             typeof(UnityEngine.Vector2Int).IsAssignableFrom(field.FieldType)))
        {
            PrintHeader("Vector2");
            if (labels["Vector2"])
            {
                GUILayout.BeginHorizontal();
                PrintFieldLabel(field.Name);
                GUILayout.Space(20);

                var helper = (JHelper)unityObjects[field.Name].Item2 ??
                             new JHelper(Vector2.zero, new string[] { "x", "y" });

                if (helper.obj.GetType() == typeof(Vector2))
                {
                    Vector2 vec = (Vector2)helper.obj;
                    Vector2 newVec = EditorGUILayout.Vector2Field("", vec);
                    unityObjects[field.Name] = (field, new JHelper(newVec, helper.fields));
                    jsonContext[field.Name] = new JObject
                    {
                        { helper.fields[0], newVec.x },
                        { helper.fields[1], newVec.y }
                    };
                }
                else if (helper.obj.GetType() == typeof(Vector2Int))
                {
                    Vector2Int vecInt = (Vector2Int)helper.obj;
                    Vector2 newVec = EditorGUILayout.Vector2Field("", vecInt);
                    Vector2Int newVecInt = new Vector2Int(Mathf.RoundToInt(newVec.x), Mathf.RoundToInt(newVec.y));
                    unityObjects[field.Name] = (field, new JHelper(newVecInt, helper.fields));
                    jsonContext[field.Name] = new JObject
                    {
                        { helper.fields[0], newVecInt.x },
                        { helper.fields[1], newVecInt.y }
                    };
                }

                GUILayout.EndHorizontal();
            }

            previousLabelType = "Vector2";
            return;
        }
        
        // 6. Other
        if (!unityObjects.ContainsKey(field.Name))
            unityObjects.Add(field.Name, (field, new JHelper(jsonContext[field.Name])));

        PrintHeader("Other");
        if (labels["Other"])
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            PrintFieldLabel(field.Name);
            GUILayout.Space(20);

            var helper = (JHelper)unityObjects[field.Name].Item2;

            // Nếu helper.obj null thì tạo JObject rỗng
            if (helper.obj == null)
                helper.obj = new JObject();

            string rawJson = helper.obj.ToString();
            string newJson = EditorGUILayout.TextArea(rawJson, fieldLayout);

            // Nếu user chỉnh sửa, parse và update
            if (newJson != rawJson)
            {
                try
                {
                    JToken parsed = JToken.Parse(newJson);
                    unityObjects[field.Name] = (field, new JHelper(parsed));
                    jsonContext[field.Name] = parsed;
                }
                catch
                {
                    Debug.LogWarning("Invalid JSON input for field: " + field.Name);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        previousLabelType = "Other";

    }

    private void PrintHeader(string title)
    {
        if (title != previousLabelType)
        {
            if (!labels.ContainsKey(title))
            {
                GUILayout.Space(25);
                bool isFolded = labels.ContainsKey(title);

                labels.Add(title, EditorGUILayout.BeginFoldoutHeaderGroup(isFolded, title, foldoutStyle));
                GUILayout.Space(10);
            }
            else
            {
                labels[title] = EditorGUILayout.BeginFoldoutHeaderGroup(labels[title], title, foldoutStyle);
            }

            GUILayout.Space(10);
        }
    }

    private void PrintFieldLabel(string name)
    {
        GUILayout.Space(20);
        GUILayout.Label(name, fieldStyle, GUILayout.ExpandWidth(false), GUILayout.MinWidth(100));
    }

    private void PrintDatatypeSelector()
    {
        labelStyle.fontSize = 16;
        GUILayout.Label("", GUI.skin.horizontalSlider);
        GUILayout.Space(25);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Datatype", labelStyle);
        selectedDatatype = GUILayout.TextField(selectedDatatype, inputStyle, GUILayout.MinWidth(150));
        GUILayout.EndHorizontal();
        type = Type.GetType(selectedDatatype);
        if (type == null)
        {
            EditorGUILayout.HelpBox("No valid Datatype found.", MessageType.Warning, true);
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("Filename: ", labelStyle);
        newFilename = GUILayout.TextField(newFilename, inputStyle, GUILayout.MinWidth(150));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void ConvertReferenceToPath(string name, (FieldInfo, object) source)
    {
        if (source.Item2 != null)
        {
            var o = (UnityEngine.Object)source.Item2;
            unityObjects[name] = (unityObjects[name].Item1, o);
            jsonContext[name + "_path"] = FetchPath(o).Replace("Assets/", "");
        }
    }

    private T ConvertPathToReference<T>(string path) where T : UnityEngine.Object =>
        AssetDatabase.LoadAssetAtPath<T>("Assets/" + path);

    public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
    {
        int Place = Source.IndexOf(Find);
        string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
        return result;
    }

    public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
    {
        int place = Source.LastIndexOf(Find);

        if (place == -1)
            return Source;

        string result = Source.Remove(place, Find.Length).Insert(place, Replace);
        return result;
    }

    private string FetchPath(UnityEngine.Object o) => AssetDatabase.GetAssetPath(o);

    private void LoadReferences()
    {
        foreach (var field in type.GetFields())
        {
            if (jsonContext.TryGetValue(field.Name, out JToken token))
            {
                if (field.Name.EndsWith("_path"))
                {
                    // Load and Fetch Object References
                    var asset = ConvertPathToReference<UnityEngine.Object>(token.Value<string>());
                    string targetField = ReplaceLastOccurrence(field.Name, "_path", "");

                    if (!unityObjects.ContainsKey(targetField))
                        unityObjects.Add(targetField, (fields.Where(f => f.Name == targetField).First(), null));

                    if (asset != null && unityObjects.TryGetValue(targetField, out (FieldInfo, object) value))
                    {
                        if (value.Item1.FieldType == asset.GetType() || asset.GetType() == typeof(GameObject))
                        {
                            unityObjects[targetField] = (value.Item1, asset);
                        }
                        else if (asset.GetType() == typeof(UnityEngine.Texture2D))
                        {
                            Texture2D tex = (Texture2D)asset;
                            unityObjects[targetField] = (value.Item1, tex);
                        }
                        else if (asset != null)
                        {
                            Debug.LogError("Loaded asset " + asset.name + " doesn't match the given type!");
                        }
                    }
                }
                else if (typeof(UnityEngine.Color).IsAssignableFrom(field.FieldType) ||
                         typeof(UnityEngine.Color32).IsAssignableFrom(field.FieldType))
                {
                    // Load and Parse Color value
                    if (!unityObjects.ContainsKey(field.Name))
                        unityObjects.Add(field.Name, (field, null));

                    JHelper color = new JHelper();
                    try
                    {
                        if (token.HasValues)
                        {
                            float[] rgba = new float[4] { 0, 0, 0, 0 };
                            bool isColor32 = false;
                            float alpha = 1;
                            int i = 0;

                            // Loop object and find 3/4 values to parse to Color
                            foreach (var j in token.Values<JProperty>())
                            {
                                float num = 0;
                                if (int.TryParse(j.Value.ToString(), out int numInt))
                                {
                                    num = numInt;
                                }
                                else if (float.TryParse(j.Value.ToString(), out num))

                                    if (!isColor32 && num > 1)
                                    {
                                        isColor32 = true;
                                    }

                                switch (i)
                                {
                                    case 0:
                                        rgba[0] = num;
                                        break;
                                    case 1:
                                        rgba[1] = num;
                                        break;
                                    case 2:
                                        rgba[2] = num;
                                        break;
                                    case 3:
                                        rgba[3] = num;
                                        break;
                                }

                                color.fields[i] = j.Name;
                                i++;
                            }

                            if (i < 3)
                            {
                                if (isColor32)
                                    alpha = 255;
                                else
                                    alpha = 1;
                            }
                            else
                            {
                                alpha = rgba[3];
                            }

                            if (isColor32)
                            {
                                color.obj = new Color32((byte)rgba[0], (byte)rgba[1], (byte)rgba[2], (byte)alpha);
                            }
                            else
                            {
                                color.obj = new Color(rgba[0], rgba[1], rgba[2], alpha);
                            }

                            unityObjects[field.Name] = (field, color);
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Debug.LogError("Color index out of range!");
                    }
                    catch
                    {
                        Debug.LogError("Failed parsing Color!");
                    }
                }
                else if ((typeof(UnityEngine.Vector3).IsAssignableFrom(field.FieldType) ||
                          typeof(UnityEngine.Vector3Int).IsAssignableFrom(field.FieldType)))
                {
                    if (!unityObjects.ContainsKey(field.Name))
                        unityObjects.Add(field.Name, (field, Vector3Int.zero));

                    Vector3 vec = new Vector3();
                    Vector3Int vecInt = new Vector3Int();
                    bool isInt = false;
                    if ((typeof(UnityEngine.Vector3Int).IsAssignableFrom(field.FieldType)))
                    {
                        isInt = true;
                    }

                    try
                    {
                        if (token.HasValues)
                        {
                            var fields = new List<string>();
                            int i = 0;
                            foreach (var j in token.Values<JProperty>())
                            {
                                float num = 0;
                                if (isInt && float.TryParse(j.Value.ToString(), out float numInt))
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            vecInt.x = (int)numInt;
                                            break;
                                        case 1:
                                            vecInt.y = (int)numInt;
                                            break;
                                        case 2:
                                            vecInt.z = (int)numInt;
                                            break;
                                    }
                                }
                                else if (float.TryParse(j.Value.ToString(), out num))
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            vec.x = num;
                                            break;
                                        case 1:
                                            vec.y = num;
                                            break;
                                        case 2:
                                            vec.z = num;
                                            break;
                                    }
                                }

                                fields.Add(j.Name);
                                i++;
                            }

                            if (isInt)
                                unityObjects[field.Name] = (field, new JHelper(vecInt, fields.ToArray()));
                            else
                                unityObjects[field.Name] = (field, new JHelper(vec, fields.ToArray()));
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Failed parsing Vector3! \n " + e);
                    }
                }
                else if ((typeof(UnityEngine.Vector2).IsAssignableFrom(field.FieldType) ||
                          typeof(UnityEngine.Vector2Int).IsAssignableFrom(field.FieldType)))
                {
                    if (!unityObjects.ContainsKey(field.Name))
                        unityObjects.Add(field.Name, (field, Vector2Int.zero));

                    Vector2 vec = new Vector2();
                    Vector2Int vecInt = new Vector2Int();
                    bool isInt = false;
                    if ((typeof(UnityEngine.Vector2Int).IsAssignableFrom(field.FieldType)))
                    {
                        isInt = true;
                    }

                    try
                    {
                        if (token.HasValues)
                        {
                            var fields = new List<string>();
                            int i = 0;
                            foreach (var j in token.Values<JProperty>())
                            {
                                float num = 0;
                                if (isInt && float.TryParse(j.Value.ToString(), out float numInt))
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            vecInt.x = (int)numInt;
                                            break;
                                        case 1:
                                            vecInt.y = (int)numInt;
                                            break;
                                    }
                                }
                                else if (float.TryParse(j.Value.ToString(), out num))
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            vec.x = num;
                                            break;
                                        case 1:
                                            vec.y = num;
                                            break;
                                    }
                                }

                                fields.Add(j.Name);
                                i++;
                            }

                            if (isInt)
                                unityObjects[field.Name] = (field, new JHelper(vecInt, fields.ToArray()));
                            else
                                unityObjects[field.Name] = (field, new JHelper(vec, fields.ToArray()));
                        }
                    }
                    catch
                    {
                        Debug.LogError("Failed parsing Vector2!");
                    }
                }
            }
        }
    }

    public void SaveJson()
    {
        string path = (Application.dataPath + AssetDatabase.GetAssetPath(textAsset)).Replace("AssetsAssets", "Assets");

        try
        {
            File.WriteAllText(path, jsonContext.ToString().Trim());
            AssetDatabase.Refresh();
            Debug.Log(textAsset.name + " saved!");
        }
        catch
        {
            Debug.LogError("Failed writing .json to " + path);
        }
    }

    public void CreateJson()
    {
        try
        {
            // Lấy Type từ tên đã chọn
            Type type = Type.GetType(selectedDatatype);
            if (type == null)
            {
                Debug.LogError("Selected Datatype not found!");
                return;
            }

            // Tạo instance mới
            object obj = Activator.CreateInstance(type);

            // Serialize bằng JsonUtility
            string json = JsonUtility.ToJson(obj, true); // true = pretty print

            // Path đã truyền vào (ví dụ "Assets/_Project/.../json_data_2.json")
            string path = newFilename; // newFilename ở đây là full path từ Assets/...

            // Tạo thư mục nếu chưa tồn tại
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Viết file ra disk
            File.WriteAllText(path, json);
            AssetDatabase.Refresh();

            Debug.Log("Created JSON: " + path);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed creating new JSON file.\n" + e);
        }
    }


    private class JHelper
    {
        public object obj;
        public string[] fields;

        public JHelper()
        {
            obj = null;
            fields = new string[4];
        }

        public JHelper(object o)
        {
            obj = o;
            fields = new string[4];
        }

        public JHelper(object o, string[] _fields)
        {
            obj = o;
            fields = _fields;
        }
    }
}
#endif