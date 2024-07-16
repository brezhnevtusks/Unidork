using Unity.Mathematics;

namespace Unidork.DOTS.Math
{
    public static class Curves
    {
        #region Cureves
        
        /// <summary>
        /// Calculates a quadratic Bezier curve point based on current progress.
        /// </summary>
        /// <param name="progress">Progress.</param>
        /// <param name="firstControlPoint">First control point.</param>
        /// <param name="secondControlPoint">Second control point.</param>
        /// <param name="thirdControlPoint">Third control point.</param>
        /// <returns>
        /// A float3 representing the calculated Bezier curve point.
        /// </returns>
        private static float3 CalculateQuadraticBezierPoint(float progress, float3 firstControlPoint, 
                                                            float3 secondControlPoint, float3 thirdControlPoint)
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
        /// A float3 representing the calculated Bezier curve point.
        /// </returns>
        private static float3 CalculateCubicBezierPoint(float progress, float3 firstControlPoint, float3 secondControlPoint, 
                                                        float3 thirdControlPoint, float3 fourthControlPoint)
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
