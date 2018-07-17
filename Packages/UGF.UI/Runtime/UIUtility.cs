using UnityEngine;
using UnityEngine.Assertions;

namespace UGF.UI.Runtime
{
    /// <summary>
    /// Provides utilities for working with Unity UI.
    /// </summary>
    public static class UIUtility
    {
        /// <summary>
        /// Sets the anchors to corners.
        /// </summary>
        /// <param name="rectTransform">The target RectTransform.</param>
        public static void SetAnchorsToCorners(RectTransform rectTransform)
        {
            Assert.IsNotNull(rectTransform, "The target RectTransform cannot be null.");

            var parent = rectTransform.parent as RectTransform;

            if (parent != null)
            {
                var anchorsMin = Vector2.zero;
                var anchorsMax = Vector2.zero;

                anchorsMin.x = rectTransform.anchorMin.x + rectTransform.offsetMin.x / parent.rect.width;
                anchorsMin.y = rectTransform.anchorMin.y + rectTransform.offsetMin.y / parent.rect.height;
                anchorsMax.x = rectTransform.anchorMax.x + rectTransform.offsetMax.x / parent.rect.width;
                anchorsMax.y = rectTransform.anchorMax.y + rectTransform.offsetMax.y / parent.rect.height;

                rectTransform.anchorMin = anchorsMin;
                rectTransform.anchorMax = anchorsMax;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }
        }

        /// <summary>
        /// Sets the corners to anchors.
        /// </summary>
        /// <param name="rectTransform">The target RectTransform.</param>
        public static void SetCornersToAnchors(RectTransform rectTransform)
        {
            Assert.IsNotNull(rectTransform, "The target RectTransform cannot be null.");

            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}