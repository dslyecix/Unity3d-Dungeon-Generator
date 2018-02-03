using UnityEngine;
using UnityEditor;

public abstract class EditorWithSubEditors<TEditor, TTarget> : Editor 
    where TEditor : Editor
    where TTarget : Object
{
	protected TEditor[] subEditors;

    protected void CheckAndCreateSubEditors (TTarget[] subEditorTargets)
    {
        if (subEditors != null && subEditors.Length == subEditorTargets.Length) return;

        CleanUpEditors();

        subEditors = new TEditor[subEditorTargets.Length];
        

    }

    protected void CleanUpEditors()
    {
        if (subEditors == null)
            return;
        
        for (int i = 0; i < subEditors.Length; i++)
        {
            DestroyImmediate(subEditors[i]);
        }

        subEditors = null;
    }

    
}
