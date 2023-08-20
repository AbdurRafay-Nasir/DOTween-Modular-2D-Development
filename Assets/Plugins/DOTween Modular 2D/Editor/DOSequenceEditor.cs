#if UNITY_EDITOR

namespace DOTweenModular2D.Editor
{

using DOTweenModular2D.Enums;
using DG.DOTweenEditor;
using DG.Tweening;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DOSequence)), CanEditMultipleObjects]
public class DOSequenceEditor : DOBaseEditor 
{   
    private SerializedProperty sequenceTweensProp;

    private DOSequence doSequence;
    private bool tweenPreviewing;

    private SavedTransforms[] savedTransforms;

#region Foldout Bool

    private bool sequenceSettingsFoldout = true;
    private string savedSequenceSettingsFoldout;

#endregion

#region Join Bool

    private bool join = false;
    private string savedJoin;

#endregion

#region Preview Buttons

    private GUIContent joinButton;
    private GUIContent stopButton;
    private GUIContent playButton;

#endregion

#region Unity Functions

    private void OnEnable() 
    {
        doSequence = (DOSequence) target;

        SetupSerializedProperties();
        SetupSavedVariables(doSequence);

        joinButton = EditorGUIUtility.IconContent("d_Linked");
        joinButton.tooltip = "Toggle join";

        stopButton = EditorGUIUtility.IconContent("d_winbtn_win_close_h@2x");
        stopButton.tooltip = "Stop Previewing tween";

        playButton = EditorGUIUtility.IconContent("PlayButton On@2x");
        playButton.tooltip = "Start Previewing tween";
    }

    public override void OnInspectorGUI() 
    {
        EditorGUILayout.Space();

        // Draw Life Time Settings
        lifeTimeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lifeTimeSettingsFoldout, "Life Time Settings");
        EditorPrefs.SetBool(savedLifeTimeSettingsFoldout, lifeTimeSettingsFoldout);
        if (lifeTimeSettingsFoldout)
        {
            EditorGUI.indentLevel++;

            DrawLifeTimeSettings();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        DrawTweenObjectHelpBox();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // Draw Type Settings
        typeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(typeSettingsFoldout, "Type Settings");
        EditorPrefs.SetBool(savedTypeSettingsFoldout, typeSettingsFoldout);
        if (typeSettingsFoldout)
        {
            EditorGUI.indentLevel++;

            DrawTypeSettings();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


        // Draw Sequence Settings
        sequenceSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(sequenceSettingsFoldout, "Sequence Settings");
        EditorPrefs.SetBool(savedSequenceSettingsFoldout, sequenceSettingsFoldout);
        if (sequenceSettingsFoldout)
        {
            EditorGUI.indentLevel++;

            DrawSequenceSettings();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


        EditorGUILayout.PropertyField(sequenceTweensProp);
        if (doSequence.sequenceTweens != null)
        {
            for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
            {
                DOBase currentTween = doSequence.sequenceTweens[i].tweenObject;

                if (currentTween != null)
                    currentTween.begin = Begin.Manual;
                else
                    EditorGUILayout.HelpBox("Element: " + i + " Tween Object is not assigned", MessageType.Error);
            }
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


        // Draw Events
        eventsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(eventsFoldout, "Events");
        EditorPrefs.SetBool(savedEventsFoldout, eventsFoldout);
        if (eventsFoldout)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.Space();
            DrawEvents();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (doSequence.sequenceTweens != null 
            && GUILayout.Button(joinButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
        {
            join = !join;
            EditorPrefs.SetBool(savedJoin, join);

            for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
            {
                doSequence.sequenceTweens[i].join = join;
            }
        }

        DrawPreviewButtons();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void OnSceneGUI()
    {
        if (doSequence.begin == Enums.Begin.After || 
            doSequence.begin == Enums.Begin.With)
        {
            Handles.color = Color.white;

            if (doSequence.tweenObject != null)
                DrawLineToTweenObject();            
        }
    }

#endregion

#region Preview Functions

    private new void DrawPreviewButtons()
    {
        // if tween is previewing enable stopButton
        GUI.enabled = tweenPreviewing;
        if (GUILayout.Button(stopButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
        {
            DOTweenEditorPreview.Stop(true);
            ClearTweenCallbacks();
            ApplySavedTransforms();
        }

        // if tween is not previewing enable playButton
        GUI.enabled = !tweenPreviewing;
        if (GUILayout.Button(playButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
        {
            SetupSavedTransforms();

            tweenPreviewing = true;

            doSequence.CreateTween();
            doSequence.Tween.onComplete += ClearTweenCallbacks;
            doSequence.Tween.onComplete += ApplySavedTransforms;
            DOTweenEditorPreview.PrepareTweenForPreview(doSequence.Tween, false, false);
            DOTweenEditorPreview.Start();
        }
    }

    private void ClearTweenCallbacks()
    {
        doSequence.Tween.OnComplete(null);
        doSequence.Tween.OnKill(null);
        doSequence.Tween.OnPause(null);
        doSequence.Tween.OnPlay(null);
        doSequence.Tween.OnRewind(null);
        doSequence.Tween.OnStart(null);
        doSequence.Tween.OnStepComplete(null);
        doSequence.Tween.OnUpdate(null);
        doSequence.Tween.OnWaypointChange(null);

        tweenPreviewing = false;
    }

    private void SetupSavedTransforms()
    {
        savedTransforms = new SavedTransforms[doSequence.sequenceTweens.Length];

        for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
        {
            savedTransforms[i].position = doSequence.sequenceTweens[i].tweenObject.transform.position;
            savedTransforms[i].rotation = doSequence.sequenceTweens[i].tweenObject.transform.rotation;
            savedTransforms[i].scale = doSequence.sequenceTweens[i].tweenObject.transform.localScale;
        }
    }

    private void ApplySavedTransforms()
    {
        for (int i = 0; i < savedTransforms.Length; i++)
        {
            doSequence.sequenceTweens[i].tweenObject.transform.position = savedTransforms[i].position;
            doSequence.sequenceTweens[i].tweenObject.transform.rotation = savedTransforms[i].rotation;
            doSequence.sequenceTweens[i].tweenObject.transform.localScale = savedTransforms[i].scale;
        }
    }

#endregion

#region Draw Functions

    protected override void DrawTypeSettings()
    {
        EditorGUILayout.PropertyField(beginProp);

        if ((Begin)beginProp.enumValueIndex == Begin.With ||
            (Begin)beginProp.enumValueIndex == Begin.After)
        {
            EditorGUILayout.PropertyField(tweenObjectProp);
            EditorGUILayout.PropertyField(delayProp);
        }
    }

    private void DrawSequenceSettings()
    {
        EditorGUILayout.PropertyField(tweenTypeProp);

        if ((Enums.TweenType)tweenTypeProp.enumValueIndex == Enums.TweenType.Looped)
        {
            EditorGUILayout.PropertyField(loopTypeProp);
            EditorGUILayout.PropertyField(loopsProp);
        }
    }

#endregion

#region Setup Functions

    protected override void SetupSerializedProperties()
    {
        base.SetupSerializedProperties();

        sequenceTweensProp = serializedObject.FindProperty("sequenceTweens");
    }

    protected override void SetupSavedVariables(DOBase doSequence)
    {
        base.SetupSavedVariables(doSequence);

        savedSequenceSettingsFoldout = "DOSequenceEditor_sequenceSettingsFoldout_" + doSequence.GetInstanceID();
        sequenceSettingsFoldout = EditorPrefs.GetBool(savedSequenceSettingsFoldout, true);

        savedJoin = "DOSequenceEditor_join_" + doSequence.GetInstanceID();
        join = EditorPrefs.GetBool(savedJoin, false);
    }

#endregion

}

struct SavedTransforms
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

}

#endif