using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace UGF.UI.Editor
{
    internal sealed class UIEditModeButton
    {
        private readonly AnimFloat m_time = new AnimFloat(0F);        
        private static readonly GUIContent m_content = new GUIContent("UI", "Toggle UI Edit Mode.");

        public void OnSceneGUI(SceneView sceneView)
        {
            Handles.BeginGUI();

            if (sceneView.in2DMode && m_time.target == 0F)
            {
                m_time.value = sceneView.in2DMode ? 1F : 0F;
            }

            float positionLeft = sceneView.position.width - 95F;
            float positionRight = sceneView.position.width - 25F;
            float position = Mathf.Lerp(positionLeft, positionRight, m_time.value);
            
            var rectButton = new Rect(position, 8.5F, 20F, 20F);

            if (GUI.Button(rectButton, m_content, EditorStyles.miniBoldLabel))
            {
                UIEditMode.SetActive(!UIEditMode.IsActive, sceneView);
            }
            
            m_time.target = sceneView.in2DMode ? 1F : 0F;

            if (m_time.isAnimating)
            {
                sceneView.Repaint();
            }

            Handles.EndGUI();
        }
    }
}