using UnityEngine;

namespace RedBjorn.Utils
{
    public class Geometry
    {
        public const float Eps = 0.01f;

        public static bool IsOnStraightLine(float lineAx, float lineAy, float lineBx, float lineBy, float targetX, float targetY)
        {
            return Mathf.Abs((targetX - lineAx) * (lineBy - lineAy) - (targetY - lineAy) * (lineBx - lineAx)) < Eps;
        }

        public static bool IsOnRay(float originX, float originY, float rayX, float rayY, float targetX, float targetY)
        {
            var xDeltaRay = rayX - originX;
            var yDeltaRay = rayY - originY;
            var xDeltaTarget = targetX - originX;
            var yDeltaTarget = targetY - originY;
            if (Mathf.Abs(xDeltaTarget * yDeltaRay - yDeltaTarget * xDeltaRay) < Eps)
            {
                return xDeltaRay * xDeltaTarget >= 0f && yDeltaRay * yDeltaTarget >= 0f;
            }
            return false;
        }

        public static Vector3 BezierCurve(Vector3 start, Vector3 center, Vector3 finish, float t)
        {
            t = Mathf.Clamp01(t);
            return (1 - t) * (1 - t) * start + 2 * (1 - t) * t * center + t * t * finish;
        }
    }
}