#region Namespaces
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#endregion


/// <summary>
///		Custom editor object to show and hide the Game Controller UI Data Fields based on an enum value in a scriptable object 
/// </summary>
[CustomEditor(typeof(GameControllerUIData))]
[CanEditMultipleObjects]
public class GameControllerUIDataEditor : Editor
{

	/// <summary>
	///     The field conditions to set 
	/// </summary>
	private void SetFieldCondition()
    {
        // Shows the windows field if the game controller is an Xbox Controller 
        ShowOnEnum("ControllerType", "XboxController", "Windows");

        // Shows the Share & Touchpad options if the game controller is a PlayStation Controller 
        ShowOnEnum("ControllerType", "Playstation4Controller", "Share");
        ShowOnEnum("ControllerType", "Playstation4Controller", "Touchpad");

        ShowOnEnum("ControllerType", "NintendoSwitchController", "SwitchHome");
        ShowOnEnum("ControllerType", "NintendoSwitchController", "Minus");
        ShowOnEnum("ControllerType", "NintendoSwitchController", "Plus");
        ShowOnEnum("ControllerType", "NintendoSwitchController", "Square");
    }

  
	#region Unity References 

	/// <summary>
	///     Once enabled, creates a new field condition list 
	/// </summary>
	public void OnEnable()
    {
        fieldConditions = new List<p_FieldCondition>();
        SetFieldCondition();
    }

    /// <summary>
    ///     On Inspector GUI function 
    /// </summary>
    public override void OnInspectorGUI()
    {

        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();


        var obj = serializedObject.GetIterator();


        if (obj.NextVisible(true))
        {

            // Loops through all visiuble fields
            do
            {
                bool shouldBeVisible = true;
                // Tests if the field is a field that should be hidden/shown due to the enum value
                foreach (var fieldCondition in fieldConditions)
                {
                    //If the fieldcondition isn't valid, display an error msg.
                    if (!fieldCondition.p_Valid)
                    {
                        Debug.LogError(fieldCondition.p_ErrorMessage);
                    }
                    else if (fieldCondition.p_FieldName == obj.name)
                    {
                        FieldInfo enumField = target.GetType().GetField(fieldCondition.p_EnumFieldName);
                        var currentEnumValue = enumField.GetValue(target);
                        //If the enum value isn't equal to the wanted value the field will be set not to show
                        if (currentEnumValue.ToString() != fieldCondition.p_EnumFieldValue)
                        {
                            shouldBeVisible = false;
                            break;
                        }
                    }
                }

                if (shouldBeVisible)
                    EditorGUILayout.PropertyField(obj, true);


            } while (obj.NextVisible(false));
        }


        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();

    }

	#endregion

	#region Private Methods

	/// <summary>
	///    Used to show / hide enum values depending on the currently set enum field 
	/// </summary>
	/// <param name="enumFieldName">The enum reference field name</param>
	/// <param name="enumValue">The enum value to check</param>
	/// <param name="fieldName">The field name to show if the enum value is selected</param>
	private void ShowOnEnum(string enumFieldName, string enumValue, string fieldName)
    {
        p_FieldCondition newFieldCondition = new p_FieldCondition()
        {
            p_EnumFieldName = enumFieldName,
            p_EnumFieldValue = enumValue,
            p_FieldName = fieldName,
            p_Valid = true

        };


        //Valildating the "enumFieldName"
        newFieldCondition.p_ErrorMessage = "";
        FieldInfo enumField = target.GetType().GetField(newFieldCondition.p_EnumFieldName);
        if (enumField == null)
        {
            newFieldCondition.p_Valid = false;
            newFieldCondition.p_ErrorMessage = "Could not find a enum-field named: '" + enumFieldName + "' in '" + target + "'. Make sure you have spelled the field name for the enum correct in the script '" + this.ToString() + "'";
        }

        //Valildating the "enumValue"
        if (newFieldCondition.p_Valid)
        {

            var currentEnumValue = enumField.GetValue(target);
            var enumNames = currentEnumValue.GetType().GetFields();
            //var enumNames =currentEnumValue.GetType().GetEnumNames();
            bool found = false;
            foreach (FieldInfo enumName in enumNames)
            {
                if (enumName.Name == enumValue)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                newFieldCondition.p_Valid = false;
                newFieldCondition.p_ErrorMessage = "Could not find the enum value: '" + enumValue + "' in the enum '" + currentEnumValue.GetType().ToString() + "'. Make sure you have spelled the value name correct in the script '" + this.ToString() + "'";
            }
        }

        // Valildating the "fieldName"
        if (newFieldCondition.p_Valid)
        {
            FieldInfo fieldWithCondition = target.GetType().GetField(fieldName);
            if (fieldWithCondition == null)
            {
                newFieldCondition.p_Valid = false;
                newFieldCondition.p_ErrorMessage = "Could not find the field: '" + fieldName + "' in '" + target + "'. Make sure you have spelled the field name correct in the script '" + this.ToString() + "'";
            }
        }

        if (!newFieldCondition.p_Valid)
        {
            newFieldCondition.p_ErrorMessage += "\nYour error is within the Custom Editor Script to show/hide fields in the inspector depending on the an Enum." +
                    "\n\n" + this.ToString() + ": " + newFieldCondition.ToStringMessage() + "\n";
        }

        fieldConditions.Add(newFieldCondition);
    }

    /// <summary>
    ///     Private list of field conditions to test 
    /// </summary>
    private List<p_FieldCondition> fieldConditions;

    /// <summary>
    ///     Private Data Class for Handling Field Condition Values 
    /// </summary>
    private class p_FieldCondition
    {
        public string p_EnumFieldName { get; set; }
        public string p_EnumFieldValue { get; set; }
        public string p_FieldName { get; set; }
        public bool p_Valid { get; set; }
        public string p_ErrorMessage { get; set; }

        public string ToStringMessage()
        {
            return "'" + p_EnumFieldName + "', '" + p_EnumFieldValue + "', '" + p_FieldName + "'.";
        }
    }

	#endregion

}