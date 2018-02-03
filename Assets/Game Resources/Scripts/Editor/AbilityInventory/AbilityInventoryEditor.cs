using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityInventory))]
public class AbilityInventoryEditor : Editor {

    private SerializedProperty abilityImagesProperty;
    private SerializedProperty abilitiesProperty;
    private bool[] expandAbilitySlots = new bool[AbilityInventory.numAbilitySlots];

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

        for (int i = 0; i < AbilityInventory.numAbilitySlots; i++)
        {
            AbilitySlotGUI(i);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AbilitySlotGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        expandAbilitySlots[index] = EditorGUILayout.Foldout(expandAbilitySlots[index], "Ability slot " + index);
        
        if (expandAbilitySlots[index])
        {
            EditorGUILayout.PropertyField(abilityImagesProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(abilitiesProperty.GetArrayElementAtIndex(index));

        }
        

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
