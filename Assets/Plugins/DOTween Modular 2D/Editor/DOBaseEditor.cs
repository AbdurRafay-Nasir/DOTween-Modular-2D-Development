#if UNITY_EDITOR

namespace DOTweenModular2D.Editor
{

using DOTweenModular2D.Enums;
using DG.DOTweenEditor;
using DG.Tweening;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DOBase)), CanEditMultipleObjects]
public class DOBaseEditor : Editor 
{

#region Serialized Properties

    protected SerializedProperty beginProp;
    protected SerializedProperty tweenObjectProp;
    protected SerializedProperty killProp;
    protected SerializedProperty destroyComponentProp;
    protected SerializedProperty destroyGameObjectProp;
    protected SerializedProperty delayProp;
    protected SerializedProperty tweenTypeProp;
    protected SerializedProperty loopTypeProp;
    private SerializedProperty easeTypeProp;
    private SerializedProperty curveProp;
    protected SerializedProperty loopsProp;
    protected SerializedProperty durationProp;
    private SerializedProperty onTweenCreatedProp;
    private SerializedProperty onTweenCompletedProp;
    private SerializedProperty onTweenKilledProp;

#endregion

    private DOBase doBase;
    protected bool TweenPreviewing { get; private set; }

    protected Vector3 positionBeforePreview { get; private set; }
    private Quaternion rotationBeforePreview;
    private Vector3 scaleBeforePreview;

    protected const float buttonSize = 40;

#region Handle Properties

    protected int currentHandleIndex = 0;
    protected int currentHandleColorIndex = 0;
    protected int currentLineColorIndex = 0;
    protected float currentHandleRadius = 1f;
    protected float currentLineWidth = 0f;

    private string savedHandleIndex;
    private string savedHandleColorIndex;
    private string savedHandleRadius;
    private string savedLineColorIndex;
    private string savedLineWidth;

    protected Color[] color = new Color[] 
    {
        Color.black, 
        Color.blue,
        Color.clear,
        Color.cyan,
        Color.gray,
        Color.green,
        Color.magenta,
        Color.red,
        Color.white,
        Color.yellow,
    };

    private string[] colorDropdown = new string[]
    {
        "Black",
        "Blue",
        "Clear",
        "Cyan",
        "Gray",
        "Green",
        "Magenta",
        "Red",
        "White",
        "Yellow"
    };

    private string[] handleDropdown = new string[] { "Position", "Free" };

#endregion

#region Inspector Button Properties

    protected bool editPath;
    private string savedEditPath;

#endregion

#region Foldout Bools

    protected bool lifeTimeSettingsFoldout = true;
    protected bool typeSettingsFoldout = true;
    protected bool valuesFoldout = true;
    protected bool eventsFoldout = false;
    protected bool editorFoldout = false;

    protected string savedLifeTimeSettingsFoldout;
    protected string savedTypeSettingsFoldout;
    protected string savedValuesFoldout;
    protected string savedEventsFoldout;
    protected string savedEditorFoldout;

#endregion

#region Setup Functions

    /// <summary>
    /// Must call this method in OnEnable to initialize Common Serialized Properties
    /// </summary>
    protected virtual void SetupSerializedProperties()
    {
        beginProp = serializedObject.FindProperty("begin");
        tweenObjectProp = serializedObject.FindProperty("tweenObject");
        killProp = serializedObject.FindProperty("kill");
        destroyComponentProp = serializedObject.FindProperty("destroyComponent");
        destroyGameObjectProp = serializedObject.FindProperty("destroyGameObject");
        delayProp = serializedObject.FindProperty("delay");
        tweenTypeProp = serializedObject.FindProperty("tweenType");
        loopTypeProp = serializedObject.FindProperty("loopType");
        easeTypeProp = serializedObject.FindProperty("easeType");
        curveProp = serializedObject.FindProperty("curve");
        loopsProp = serializedObject.FindProperty("loops");
        durationProp = serializedObject.FindProperty("duration");
        onTweenCreatedProp = serializedObject.FindProperty("onTweenCreated");
        onTweenCompletedProp = serializedObject.FindProperty("onTweenCompleted");
        onTweenKilledProp = serializedObject.FindProperty("onTweenKilled");
    }

    /// <summary>
    /// Must call this method in OnEnable to load saved state of Foldout bools and edit path bool
    /// </summary>
    protected virtual void SetupSavedVariables(DOBase doBase)
    {
        this.doBase = doBase;

        SetupSavedVariablesPath(doBase);

        ApplySavedValuesToVariables();
    }

    private void SetupSavedVariablesPath(DOBase doBase)
    {
        int instanceId = doBase.GetInstanceID();
        
        // Saved Foldout Bool Properties Path
        savedLifeTimeSettingsFoldout = "DOBaseEditor_lifeTimeSettings_" + instanceId;
        savedTypeSettingsFoldout = "DOBaseEditor_typeSettings_" + instanceId;
        savedValuesFoldout = "DOBaseEditor_values_" + instanceId;
        savedEventsFoldout = "DOBaseEditor_events_" + instanceId;
        savedEditorFoldout = "DOBaseEditor_editor_" + instanceId;

        // Saved handles properties Path
        savedHandleIndex = "DOBaseEditor_handleIndex_" + instanceId;
        savedHandleColorIndex = "DOBaseEditor_handleColorIndex_" + instanceId;
        savedLineColorIndex = "DOBaseEditor_lineColorIndex_" + instanceId;
        savedHandleRadius = "DOBaseEditor_handleRadius_" + instanceId;
        savedLineWidth = "DOBaseEditor_lineWidth_" + instanceId;

        // Saved Inspector button properties Path
        savedEditPath = "DOBaseEditor_editPath_" + instanceId;
    }

    private void ApplySavedValuesToVariables()
    {
        // Apply saved values to Foldout Bool Properties
        lifeTimeSettingsFoldout = EditorPrefs.GetBool(savedLifeTimeSettingsFoldout, true);
        typeSettingsFoldout = EditorPrefs.GetBool(savedTypeSettingsFoldout, true);
        valuesFoldout = EditorPrefs.GetBool(savedValuesFoldout, true);
        eventsFoldout = EditorPrefs.GetBool(savedEventsFoldout, false);
        editorFoldout = EditorPrefs.GetBool(savedEditorFoldout, false);

        // Apply saved values to Editor Properties
        currentHandleIndex = EditorPrefs.GetInt(savedHandleIndex, 0);
        currentHandleColorIndex = EditorPrefs.GetInt(savedHandleColorIndex, 5);
        currentLineColorIndex = EditorPrefs.GetInt(savedLineColorIndex, 5);
        currentHandleRadius = EditorPrefs.GetFloat(savedHandleRadius, 0.5f);
        currentLineWidth = EditorPrefs.GetFloat(savedLineWidth, 1f);

        // Apply saved values to Inspector Button Properties
        editPath = EditorPrefs.GetBool(savedEditPath, true);
    }

#endregion

#region Draw Properties Functions

    /// <summary>
    /// Draws begin, tweenObjectProp(if Begin = After or With), kill <br/>
    /// destroy component, destroy gameObject
    /// </summary>
    protected void DrawLifeTimeSettings()
    {
        EditorGUILayout.PropertyField(beginProp);

        if ((Begin)beginProp.enumValueIndex == Begin.With ||
            (Begin)beginProp.enumValueIndex == Begin.After)
        {
            EditorGUILayout.PropertyField(tweenObjectProp);
        }

        EditorGUILayout.PropertyField(killProp);
        EditorGUILayout.PropertyField(destroyComponentProp);
        EditorGUILayout.PropertyField(destroyGameObjectProp);
    }

    /// <summary>
    /// Draws tweenType loopType (if tweenType = Looped), <br/> 
    /// easeType, curve(if easeType = INTERNAL_Custom)
    /// </summary>
    protected virtual void DrawTypeSettings()
    {
        EditorGUILayout.PropertyField(tweenTypeProp);

        if ((Enums.TweenType)tweenTypeProp.enumValueIndex == Enums.TweenType.Looped)
        {
            EditorGUILayout.PropertyField(loopTypeProp);
        }

        EditorGUILayout.PropertyField(easeTypeProp);

        if ((DG.Tweening.Ease)easeTypeProp.enumValueIndex == DG.Tweening.Ease.INTERNAL_Custom)
        {
            EditorGUILayout.PropertyField(curveProp);
        }
    }

    /// <summary>
    /// Draws loops(if loopType = Looped), delay, duration Property
    /// </summary>    
    protected virtual void DrawValues()
    {
        if ((Enums.TweenType)tweenTypeProp.enumValueIndex == Enums.TweenType.Looped)
        {
            EditorGUILayout.PropertyField(loopsProp);
        }

        EditorGUILayout.PropertyField(delayProp);
        EditorGUILayout.PropertyField(durationProp);
    }

    /// <summary>
    /// Draws onTweenCreated, onTweenCompleted events
    /// </summary>
    protected void DrawEvents()
    {
        EditorGUILayout.PropertyField(onTweenCreatedProp);
        EditorGUILayout.PropertyField(onTweenCompletedProp);
        EditorGUILayout.PropertyField(onTweenKilledProp);
    }

    /// <summary>
    /// Draws Handle, Handle Color, Handle Radius, Line Color, Line Width Editor Inspector Properties
    /// </summary>
    protected void DrawEditorProperties()
    {
        currentHandleIndex = EditorGUILayout.Popup("Handle", currentHandleIndex, handleDropdown);
        EditorPrefs.SetInt(savedHandleIndex, currentHandleIndex);

        currentHandleColorIndex = EditorGUILayout.Popup("Handle Color", currentHandleColorIndex, colorDropdown);
        EditorPrefs.SetInt(savedHandleColorIndex, currentHandleColorIndex);

        EditorGUI.BeginChangeCheck();
        currentHandleRadius = EditorGUILayout.Slider("Handle Radius", currentHandleRadius, 0.5f, 3f);
        EditorPrefs.SetFloat(savedHandleRadius, currentHandleRadius);

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }

        currentLineColorIndex = EditorGUILayout.Popup("Line Color", currentLineColorIndex, colorDropdown);
        EditorPrefs.SetInt(savedLineColorIndex, currentLineColorIndex);

        EditorGUI.BeginChangeCheck();
        currentLineWidth = EditorGUILayout.Slider("Line Width", currentLineWidth, 1f, 20f);
        EditorPrefs.SetFloat(savedLineWidth, currentLineWidth);

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    /// <summary>
    /// Draws Edit Button
    /// </summary>
    protected void DrawEditButton()
    {
        GUIContent editButton = EditorGUIUtility.IconContent("EditCollider");
        editButton.tooltip = "Toggle Path Editing";

        if (GUILayout.Button(editButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))            
        {
            editPath = !editPath;

            SceneView.RepaintAll();

            EditorPrefs.SetBool(savedEditPath, editPath);
        }
    }

    /// <summary>
    /// Draws Reset Editor Properties Button
    /// </summary>
    protected void DrawResetEditorPropertiesButton()
    {
        GUIContent resetButton = EditorGUIUtility.IconContent("Refresh@2x");
        resetButton.tooltip = "Reset Editor Properties";

        if (GUILayout.Button(resetButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
        {
            currentHandleIndex = 0;
            currentHandleColorIndex = 5;
            currentHandleRadius = 0.5f;
            currentLineColorIndex = 5;
            currentLineWidth = 1f;

            SceneView.RepaintAll();
        }
    }

#endregion

#region Preview Functions

    /// <summary>
    /// Draws a line to Tween Object, does not have null check you have to do it yourself
    /// </summary>
    protected void DrawLineToTweenObject()
    {
        DOBase tweenObject = (DOBase) tweenObjectProp.objectReferenceValue;

        Handles.DrawLine(doBase.transform.position, tweenObject.transform.position);
    }

    /// <summary>
    /// Draws Tween Preview buttons (Play and Stop)
    /// </summary>
    protected void DrawPreviewButtons()
    {
        GUIContent stopButton = EditorGUIUtility.IconContent("d_winbtn_win_close_h@2x");
        stopButton.tooltip = "Stop Previewing tween";

        GUIContent playButton = EditorGUIUtility.IconContent("PlayButton On@2x");
        playButton.tooltip = "Start Previewing tween";

        // if tween is previewing enable stopButton
        GUI.enabled = TweenPreviewing;
        if (GUILayout.Button(stopButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
        {
            DOTweenEditorPreview.Stop(true);
            ClearTweenCallbacks();
            ApplySavedTransform();
        }

        // if tween is not previewing enable playButton
        GUI.enabled = !TweenPreviewing;
        if (GUILayout.Button(playButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
        {
            SaveDefaultTransform();
            TweenPreviewing = true;

            doBase.CreateTween();
            doBase.Tween.onComplete += ClearTweenCallbacks;
            doBase.Tween.onComplete += ApplySavedTransform;
            DOTweenEditorPreview.PrepareTweenForPreview(doBase.Tween, false, false);
            DOTweenEditorPreview.Start();
        }
    }

    private void ClearTweenCallbacks()
    {
        doBase.Tween.OnComplete(null);
        doBase.Tween.OnKill(null);
        doBase.Tween.OnPause(null);
        doBase.Tween.OnPlay(null);
        doBase.Tween.OnRewind(null);
        doBase.Tween.OnStart(null);
        doBase.Tween.OnStepComplete(null);
        doBase.Tween.OnUpdate(null);
        doBase.Tween.OnWaypointChange(null);

        TweenPreviewing = false;
    }

    private void ApplySavedTransform()
    {
        doBase.transform.position = positionBeforePreview;
        doBase.transform.rotation = rotationBeforePreview;
        doBase.transform.localScale = scaleBeforePreview;
    }
    
    private void SaveDefaultTransform()
    {
        positionBeforePreview = doBase.transform.position;
        rotationBeforePreview = doBase.transform.rotation;
        scaleBeforePreview = doBase.transform.lossyScale;
    }

#endregion

    /// <summary>
    /// Draws Helpbox for Inspector messages regarding tweenObject and Begin property
    /// </summary>
    protected void DrawTweenObjectHelpBox()
    {
        if ((Begin)beginProp.enumValueIndex == Begin.After ||
            (Begin)beginProp.enumValueIndex == Begin.With)            
        {
            if (tweenObjectProp.objectReferenceValue == null)
                EditorGUILayout.HelpBox("Tween Object is not assigned", MessageType.Error);
        }
        else
        {
            if (tweenObjectProp.objectReferenceValue != null)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.HelpBox("Tween Object is assigned, it should be removed", MessageType.Warning);
                
                GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
                trashButton.tooltip = "Remove Tween Object";

                if (GUILayout.Button(trashButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize * 2f)))
                {
                    tweenObjectProp.objectReferenceValue = null;
                }

                EditorGUILayout.EndHorizontal(); 
            }            
        }
    }

}

}

#endif