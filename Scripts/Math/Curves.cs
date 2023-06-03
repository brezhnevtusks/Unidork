using UnityEngine;

namespace Unidork.Math
{
    /// <summary>
    /// Contains various mathematical operations to perform with curves: arc movement, Bezier curves etc.
    /// </summary>
    public static class Curves
    {
        #region Arc movement

        /// <summary>
        /// Calculates the point on a symmetrical art going from passed start point to end point at current movement progress value.
        /// </summary>
        /// <param name="progress">Progress</param>
        /// <param name="startPosition">Arc's start position.</param>
        /// <param name="endPosition">Arc's end position.</param>
        /// <param name="arcHeight">Height of the arc.</param>
        /// <returns>
        /// Vector3 representing a point on the arc.
        /// </returns>
        public static Vector3 CalculateSymmetricalArcMovementPoint(float progress, Vector3 startPosition, Vector3 endPosition, float arcHeight)
        {
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, progress);
            newPosition.y += arcHeight * Mathf.Sin(progress * Mathf.PI);

            return newPosition;
        }

        #endregion

        #region Bezier curves

        /// <summary>
        /// Builds a quadratic Bezier curve.
        /// </summary>
        /// <param name="firstControlPoint">First control point.</param>
        /// <param name="secondControlPoint">Second control point.</param>
        /// <param name="thirdControlPoint">Third control point.</param>
        /// <param name="numberOfSegments">Number of segments to split the curve into.</param>
        /// <returns>
        /// An array of Vector3 values that constitute the curve.
        /// </returns>
        public static Vector3[] BuildQuadraticBezierCurve(Vector3 firstControlPoint, Vector3 secondControlPoint, Vector3 thirdControlPoint, int numberOfSegments)
        {
            var curve = new Vector3[numberOfSegments + 1];
            curve[0] = firstControlPoint;

            for (int i = 1; i <= numberOfSegments; i++)
            {
                curve[i] = CalculateQuadraticBezierPoint(i / (float)numberOfSegments, firstControlPoint, secondControlPoint, thirdControlPoint);
            }

            return curve;
        }

        /// <summary>
        /// Builds a cubic Bezier curve.
        /// </summary>
        /// <param name="firstControlPoint">First control point.</param>
        /// <param name="secondControlPoint">Second control point.</param>
        /// <param name="thirdControlPoint">Third control point.</param>
        /// <param name="fourthControlPoint">Fourth control point.</param>
        /// <param name="numberOfSegments">Number of segments to split the curve into.</param>
        /// <returns>
        /// An array of Vector3 values that constitute the curve.
        /// </returns>
        public static Vector3[] BuildCubicBezierCurve(Vector3 firstControlPoint, Vector3 secondControlPoint, Vector3 thirdControlPoint, Vector3 fourthControlPoint, 
                                                          int numberOfSegments)
        {
            var curve = new Vector3[numberOfSegments];
            curve[0] = firstControlPoint;

            for (int i = 1; i <= numberOfSegments; i++)
            {
                curve[i] = CalculateCubicBezierPoint(i / (float)numberOfSegments, firstControlPoint, secondControlPoint, thirdControlPoint, fourthControlPoint);
            }

            return curve;
        }

        /// <summary>
        /// Calculates a quadratic Bezier curve point based on current progress.
        /// </summary>
        /// <param name="progress">Progress.</param>
        /// <param name="firstControlPoint">First control point.</param>
        /// <param name="secondControlPoint">Second control point.</param>
        /// <param name="thirdControlPoint">Third control point.</param>
        /// <returns>
        /// A Vector3 representing the calculated Bezier curve point.
        /// </returns>
        private static Vector3 CalculateQuadraticBezierPoint(float progress, Vector3 firstControlPoint, Vector3 secondControlPoint, Vector3 thirdControlPoint)
        {         
            float progressRemaining = 1 - progress;

            firstControlPoint *= progressRemaining * progressRemaining;
            secondControlPoint *= 2 * progressRemaining * progress;
            thirdControlPoint *= progress * progress;

            return firstControlPoint + secondControlPoint + thirdControlPoint;
        }

        /// <summary>
        /// Calculates a cubic Bezier curve point based on current progress.
        /// </summary>
        /// <param name="progress">Progress.</param>
        /// <param name="firstControlPoint">First control point.</param>
        /// <param name="secondControlPoint">Second control point.</param>
        /// <param name="thirdControlPoint">Third control point.</param>
        /// <param name="fourthControlPoint">Fourth control point.</param>
        /// <returns>
        /// A Vector3 representing the calculated Bezier curve point.
        /// </returns>
        private static Vector3 CalculateCubicBezierPoint(float progress, Vector3 firstControlPoint, Vector3 secondControlPoint, Vector3 thirdControlPoint, Vector3 fourthControlPoint)
        {
            float progressRemaining = 1 - progress;

            firstControlPoint *= progressRemaining * progressRemaining * progressRemaining;
            secondControlPoint *= 3 * progressRemaining * progressRemaining * progress;
            thirdControlPoint *= 3 * progressRemaining * progress * progress;
            fourthControlPoint *= progress * progress * progress;

            return firstControlPoint + secondControlPoint + thirdControlPoint + fourthControlPoint;
        }

        #endregion
    }
}
