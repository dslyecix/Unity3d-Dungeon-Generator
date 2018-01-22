using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityInventory))]
public class AbilityInventoryEditor : Editor {

    private SerializedProperty abilityImagesProperty;
    private SerializedProperty abilitiesProperty;
    private bool[] showAbilitySlots = new bool[AbilityInventory.numbAbilitySlots];

    private const string abilityInventoryPropAbilityImagesName = "abilityImages";
    private const string abilityInventoryProprAbilitiesName = "abilities";

    private void OnEnable ()
    {
        abilityImagesProperty = serializedObject.FindProperty(abilityInventoryPropAbilityImagesName);
        abilitiesProperty = serializedObject.FindProperty(abilityInventoryProprAbilitiesName);
    }	

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < AbilityInventory.numbAbilitySlots; i++)
        {
            AbilitySlotGUI(i);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AbilitySlotGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        showAbilitySlots[index] = EditorGUILayout.Foldout(showAbilitySlots[index], "Ability slot " + index);
        
        if (showAbilitySlots[index])
        {
            EditorGUILayout.PropertyField(abilityImagesProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(abilitiesProperty.GetArrayElementAtIndex(index));

        }
        

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
