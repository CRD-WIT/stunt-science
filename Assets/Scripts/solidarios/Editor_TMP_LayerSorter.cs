using UnityEditor;
using TMPro;
[CustomEditor(typeof(Script_TMP_LayerSorter))]
public class Editor_TMP_LayerSorter : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Script_TMP_LayerSorter script = (Script_TMP_LayerSorter)target;
        script.sortingLayer = EditorGUILayout.IntField("Sorting layer", script.sortingLayer);
        TextMeshProUGUI t = script.gameObject.GetComponent<TextMeshProUGUI>();
        if (t != null){
            t.canvas.sortingOrder = script.sortingLayer;
        }
    }
}
