using UnityEngine;

namespace RedBjorn.Utils
{
    public static class RectExtensions
    {
        public static Vector2 TopLeft(this Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMin);
        }

        public static Rect ScaleSizeBy(this Rect rect, float scale)
        {
            return rect.ScaleSizeBy(scale, rect.center);
        }

        public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
        {
            Rect result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale;
            result.xMax *= scale;
            result.yMin *= scale;
            result.yMax *= scale;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }

        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale)
        {
            return rect.ScaleSizeBy(scale, rect.center);
        }

        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale, Vector2 pivotPoint)
        {
            Rect result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale.x;
            result.xMax *= scale.x;
            result.yMin *= scale.y;
            result.yMax *= scale.y;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }

        public static bool ContainsCorner(this Rect rect, Rect anotherRect)
        {
            //Bottom left
            if (rect.xMin < anotherRect.xMin && anotherRect.xMin < rect.xMax
                && rect.yMin < anotherRect.yMin && anotherRect.yMin < rect.yMax)
            {
                return true;
            }

            //Top Left
            if (rect.xMin < anotherRect.xMin && anotherRect.xMin < rect.xMax
                && rect.yMin < anotherRect.yMax && anotherRect.yMax < rect.yMax)
            {
                return true;
            }

            //Top Right
            if (rect.xMin < anotherRect.xMax && anotherRect.xMax < rect.xMax
                && rect.yMin < anotherRect.yMax && anotherRect.yMax < rect.yMax)
            {
                return true;
            }

            //Bottom Right
            if (rect.xMin < anotherRect.xMax && anotherRect.xMax < rect.xMax
                && rect.yMin < anotherRect.yMin && anotherRect.yMin < rect.yMax)
            {
                return true;
            }
            return false;
        }

        public static bool ContainsCenter(this Rect rect, Rect anotherRect)
        {
            //Bottom left
            if (rect.xMin < anotherRect.center.x && anotherRect.center.x < rect.xMax
                && rect.yMin < anotherRect.center.y && anotherRect.center.y < rect.yMax)
            {
                return true;
            }
            return false;
        }

        public static bool ContainsPoint(this Rect rect, Vector2 point)
        {
            //Bottom left
            if (rect.xMin < point.x && point.x < rect.xMax
                && rect.yMin < point.y && point.y < rect.yMax)
            {
                return true;
            }
            return false;
        }
    }
}