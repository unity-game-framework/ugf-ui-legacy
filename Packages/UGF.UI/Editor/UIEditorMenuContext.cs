using UGF.UI.Runtime;
using UnityEditor;
using UnityEngine;

namespace UGF.UI.Editor
{
    internal static class UIEditorMenuContext
    {
        [MenuItem("CONTEXT/RectTransform/Anchors To Corners", false, 1000)]
        private static void RectTransformAnchorsToCorners(MenuCommand menuCommand)
        {
            var rectTransform = (RectTransform)menuCommand.context;

            Undo.RecordObject(rectTransform, "Anchors To Corners");
            UIUtility.SetAnchorsToCorners(rectTransform);
        }

        [MenuItem("CONTEXT/RectTransform/Corners To Anchors", false, 1000)]
        private static void RectTransformCornersToAnchors(MenuCommand menuCommand)
        {
            var rectTransform = (RectTransform)menuCommand.context;
            
            Undo.RecordObject(rectTransform, "Corners To Anchors");
            UIUtility.SetCornersToAnchors(rectTransform);
        }
    }
}