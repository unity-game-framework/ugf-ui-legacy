using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UGF.UI.Editor
{
    /// <summary>
    /// Provides utilities for managing UI Edit Mode.
    /// </summary>
    [InitializeOnLoad]
    public static class UIEditMode
    {
        /// <summary>
        /// Gets a value indicating whether the UI Edit Mode is active and shows only UI layer.
        /// </summary>
        public static bool IsActive { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the UI Edit Mode button is drawing in each SceneView.
        /// </summary>
        public static bool Drawing
        {
            get { return m_drawing; }
            set
            {
                if (m_drawing != value)
                {
                    m_drawing = value;

                    if (m_drawing)
                    {
                        EditorPrefs.DeleteKey(k_drawingPrefKey);
                    }
                    else
                    {
                        EditorPrefs.SetBool(k_drawingPrefKey, m_drawing);
                    }

                    SceneView.RepaintAll();
                }
            }
        }

        private static readonly Dictionary<SceneView, UIEditModeButton> m_buttons = new Dictionary<SceneView, UIEditModeButton>();
        private static readonly List<SceneView> m_remove = new List<SceneView>();
        private static bool m_drawing;

        private const string k_lastLayerMaskPrefKey = "UIEditMode.LastLayersMask";
        private const string k_drawingPrefKey = "UIEditMode.Drawing";

        static UIEditMode()
        {
            EditorApplication.update += OnEditorUpdate;
            SceneView.onSceneGUIDelegate += OnSceneGUI;

            IsActive = EditorPrefs.HasKey(k_lastLayerMaskPrefKey);
            Drawing = EditorPrefs.GetBool(k_drawingPrefKey, true);
        }

        /// <summary>
        /// Sets the layers to UI Edit Mode and 2D mode to last active scene view.
        /// </summary>
        /// <param name="state">If set to true, show only UI layer, otherwise show all layers except UI layer.</param>
        public static void SetActive(bool state)
        {
            SetActive(state, SceneView.lastActiveSceneView);
        }

        /// <summary>
        /// Sets the layers to UI Edit Mode and 2D mode to specified scene view.
        /// </summary>
        /// <param name="state">If set to true, show only UI layer, otherwise show all layers except UI layer.</param>
        /// <param name="sceneView">The specific scene view.</param>
        public static void SetActive(bool state, SceneView sceneView)
        {
            if (state)
            {
                EditorPrefs.SetInt(k_lastLayerMaskPrefKey, Tools.visibleLayers);

                if (EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode3D)
                {
                    sceneView.in2DMode = true;
                }

                Tools.visibleLayers = LayerMask.GetMask("UI");
            }
            else
            {
                if (EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode3D)
                {
                    sceneView.in2DMode = false;
                }

                if (EditorPrefs.HasKey(k_lastLayerMaskPrefKey))
                {
                    int mask = EditorPrefs.GetInt(k_lastLayerMaskPrefKey) & ~(1 << 5);

                    Tools.visibleLayers = mask == 0 ? ~LayerMask.GetMask("UI") : mask;

                    EditorPrefs.DeleteKey(k_lastLayerMaskPrefKey);
                }
                else
                {
                    Tools.visibleLayers = ~LayerMask.GetMask("UI");
                }
            }

            IsActive = state;
        }

        private static void OnEditorUpdate()
        {
            if (Drawing)
            {
                foreach (var pair in m_buttons)
                {
                    if (pair.Key == null)
                    {
                        m_remove.Add(pair.Key);
                    }
                }

                for (int i = 0; i < m_remove.Count; i++)
                {
                    m_buttons.Remove(m_remove[i]);
                }

                m_remove.Clear();
            }
            else if (m_buttons.Count > 0)
            {
                m_buttons.Clear();
            }
        }

        private static void OnSceneGUI(SceneView sceneview)
        {
            if (Drawing)
            {
                UIEditModeButton button;

                if (!m_buttons.TryGetValue(sceneview, out button))
                {
                    button = new UIEditModeButton();

                    m_buttons[sceneview] = button;
                }

                button.OnSceneGUI(sceneview);
            }
        }
    }
}