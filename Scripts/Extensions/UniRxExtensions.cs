using System;
using TMPro;
using UniRx;

namespace Unidork.Extensions
{
    public static class ReactivePropertyExtensions
    {
        /// <summary>
        /// Inverts the value of a ReactiveProperty that stores a boolean.
        /// </summary>
        /// <param name="property">Property.</param>
        public static void Invert(this ReactiveProperty<bool> property)
        {
            property.Value = !property.Value;
        }
    }
    
	public static class UnityUIExtensions
    {
        
        /// <summary>
        /// Creates a subscription for a TextMeshProUGUI component.
        /// </summary>
        /// <param name="source">Observable source.</param>
        /// <param name="textMeshProUGUI">TextMeshProUGUI component</param>
        /// <param name="format">Format to use when formatting the value.</param>
        /// <returns>
        /// An <see cref="IDisposable"/>.
        /// </returns>
        public static IDisposable SubscribeToText(this IObservable<string> source, 
                                                  TextMeshProUGUI textMeshProUGUI,
                                                  string format)
        {
            return source.SubscribeWithState(textMeshProUGUI, 
                (x, t) => t.text = string.Format(format, x));
        }

        /// <summary>
        /// Creates a subscription for a TextMeshProUGUI component.
        /// </summary>
        /// <typeparam name="T">Type of observable source.</typeparam>
        /// <param name="source">Observable source</param>
        /// <param name="textMeshProUGUI"></param>
        /// <param name="format">Format to use when formatting the value.</param>
        /// <returns>
        /// An <see cref="IDisposable"/>.
        /// </returns>
        public static IDisposable SubscribeToText<T>(this IObservable<T> source, 
                                                     TextMeshProUGUI textMeshProUGUI,
                                                     string format)
        {
            return source.SubscribeWithState(textMeshProUGUI, (x, t) => t.text = string.Format(format, x));
        }
    }
}