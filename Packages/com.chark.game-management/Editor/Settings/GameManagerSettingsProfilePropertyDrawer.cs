using CHARK.GameManagement.Settings;
using UnityEditor;
using UnityEngine;

namespace CHARK.GameManagement.Editor.Settings
{
    [CustomPropertyDrawer(typeof(GameManagerSettingsProfile))]
    internal sealed class GameManagerSettingsProfilePropertyDrawer : PropertyDrawer
    {
        private const float ToggleWidth = 16f;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var profile = property.objectReferenceValue as GameManagerSettingsProfile;
            EditorGUI.BeginProperty(position, label, property);

            var initialColor = GUI.backgroundColor;
            if (profile && profile.IsActiveProfile)
            {
                GUI.backgroundColor = Color.green;
            }

            EditorGUI.BeginChangeCheck();

            var togglePosition = position;
            togglePosition.width = ToggleWidth;

            var isActiveProfileNew = profile
                ? EditorGUI.Toggle(togglePosition, profile.IsActiveProfile)
                : EditorGUI.Toggle(togglePosition, false);

            if (EditorGUI.EndChangeCheck() && profile)
            {
                profile.IsActiveProfile = isActiveProfileNew;
            }

            var propertyPosition = position;
            propertyPosition.width -= ToggleWidth;
            propertyPosition.x += ToggleWidth;

            EditorGUI.PropertyField(propertyPosition, property, new GUIContent("Profile"));
            GUI.backgroundColor = initialColor;

            EditorGUI.EndProperty();
        }
    }
}
