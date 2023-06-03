#if CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#endif

using System;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UniRx.InternalUtil;
#if !UniRxLibrary
using UnityEngine;
#endif
#if CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))
using System.Threading.Tasks;
#endif

namespace Unidork.UniRx
{
    #region Interfaces

    internal interface IObserverLinkedList<T>
    {
        void UnsubscribeNode(ObserverNode<T> node);
    }

    internal sealed class ObserverNode<T> : IObserver<T>, IDisposable
    {
        public ObserverNode<T> Previous { get; internal set; }
        public ObserverNode<T> Next { get; internal set; }
        
        readonly IObserver<T> observer;
        IObserverLinkedList<T> list;

        public ObserverNode(IObserverLinkedList<T> list, IObserver<T> observer)
        {
            this.list = list;
            this.observer = observer;
        }

        public void OnNext(T value) => observer.OnNext(value);
        public void OnError(Exception error) => observer.OnError(error);
        public void OnCompleted() => observer.OnCompleted();

        public void Dispose()
        {
            var sourceList = Interlocked.Exchange(ref list, null);
            if (sourceList != null)
            {
                sourceList.UnsubscribeNode(this);
                sourceList = null;
            }
        }
    }

    #endregion
    
    /// <summary>
    /// Read-only reactive property that stores the value before latest change.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadOnlyReactivePropertyWithOldValue<T> : IReadOnlyReactivePropertyWithOldValue<T>, IDisposable, IOptimizedObservable<T>, IObserverLinkedList<T>, IObserver<T>
    {
#if !UniRxLibrary
        static readonly IEqualityComparer<T> defaultEqualityComparer = UnityEqualityComparer.GetDefault<T>();
#else
        static readonly IEqualityComparer<T> defaultEqualityComparer = EqualityComparer<T>.Default;
#endif
        #region Properties

        public T Value => latestValue;
        public bool HasValue => canPublishValueOnSubscribe;
        protected virtual IEqualityComparer<T> EqualityComparer => defaultEqualityComparer;

        public T OldValue { get; set; }

        #endregion

        #region Fields

        readonly bool distinctUntilChanged = true;
        bool canPublishValueOnSubscribe;
        bool isDisposed;
        bool isSourceCompleted;

        T latestValue;
        Exception lastException;
        IDisposable sourceConnection;

        ObserverNode<T> root;
        ObserverNode<T> last;

        #endregion

        #region Constructors

        public ReadOnlyReactivePropertyWithOldValue(IObservable<T> source)
        {
            sourceConnection = source.Subscribe(this);
            OldValue = latestValue = default;
        }

        public ReadOnlyReactivePropertyWithOldValue(IObservable<T> source, bool distinctUntilChanged)
        {
            this.distinctUntilChanged = distinctUntilChanged;
            sourceConnection = source.Subscribe(this);
            OldValue = latestValue = default;
        }

        public ReadOnlyReactivePropertyWithOldValue(IObservable<T> source, T initialValue)
        {
            OldValue = latestValue = initialValue;
            canPublishValueOnSubscribe = true;
            sourceConnection = source.Subscribe(this);
        }

        public ReadOnlyReactivePropertyWithOldValue(IObservable<T> source, T initialValue, bool distinctUntilChanged)
        {
            this.distinctUntilChanged = distinctUntilChanged;
            OldValue = latestValue = initialValue;
            canPublishValueOnSubscribe = true;
            sourceConnection = source.Subscribe(this);
        }

        #endregion

        #region Subscriptions

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (lastException != null)
            {
                observer.OnError(lastException);
                return Disposable.Empty;
            }

            if (isSourceCompleted)
            {
                if (canPublishValueOnSubscribe)
                {
                    observer.OnNext(latestValue);
                    observer.OnCompleted();
                    return Disposable.Empty;
                }

                observer.OnCompleted();
                return Disposable.Empty;
            }

            if (isDisposed)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }

            if (canPublishValueOnSubscribe)
            {
                observer.OnNext(latestValue);
            }

            // subscribe node, node as subscription.
            var next = new ObserverNode<T>(this, observer);
            if (root == null)
            {
                root = last = next;
            }
            else
            {
                last.Next = next;
                next.Previous = last;
                last = next;
            }

            return next;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;
            sourceConnection.Dispose();

            var node = root;
            root = last = null;
            isDisposed = true;

            while (node != null)
            {
                node.OnCompleted();
                node = node.Next;
            }
        }

        void IObserverLinkedList<T>.UnsubscribeNode(ObserverNode<T> node)
        {
            if (node == root)
            {
                root = node.Next;
            }
            if (node == last)
            {
                last = node.Previous;
            }

            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }
        }

        void IObserver<T>.OnNext(T value)
        {
            if (isDisposed) return;

            if (canPublishValueOnSubscribe)
            {
                if (distinctUntilChanged && EqualityComparer.Equals(latestValue, value))
                {
                    return;
                }
            }

            canPublishValueOnSubscribe = true;

            // Store old value before setting new one
            OldValue = latestValue;
            latestValue = value;

            // call source.OnNext
            var node = root;
            while (node != null)
            {
                node.OnNext(value);
                node = node.Next;
            }
        }

        void IObserver<T>.OnError(Exception error)
        {
            lastException = error;

            // call source.OnError
            var node = root;
            while (node != null)
            {
                node.OnError(error);
                node = node.Next;
            }

            root = last = null;
        }

        void IObserver<T>.OnCompleted()
        {
            isSourceCompleted = true;
            root = last = null;
        }

        public override string ToString()
        {
            return (latestValue == null) ? "(null)" : latestValue.ToString();
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        #endregion
    }

    #region Extensions

    public static class ReactivePropertyExtensions
    {
        public static IReadOnlyReactivePropertyWithOldValue<T> ToReactivePropertyWithOldValue<T>(this IObservable<T> source)
        {
            return new ReadOnlyReactivePropertyWithOldValue<T>(source);
        }

        public static IReadOnlyReactivePropertyWithOldValue<T> ToReactivePropertyWithOldValue<T>(this IObservable<T> source, T initialValue)
        {
            return new ReadOnlyReactivePropertyWithOldValue<T>(source, initialValue);
        }

        public static ReadOnlyReactivePropertyWithOldValue<T> ToReadOnlyReactivePropertyWithOldValue<T>(this IObservable<T> source)
        {
            return new ReadOnlyReactivePropertyWithOldValue<T>(source);
        }
    }

    #endregion

    #region Utility

    internal static class UnityEqualityComparer
    {
        public static readonly IEqualityComparer<Vector2> Vector2 = new Vector2EqualityComparer();
        public static readonly IEqualityComparer<Vector3> Vector3 = new Vector3EqualityComparer();
        public static readonly IEqualityComparer<Vector4> Vector4 = new Vector4EqualityComparer();
        public static readonly IEqualityComparer<Color> Color = new ColorEqualityComparer();
        public static readonly IEqualityComparer<Color32> Color32 = new Color32EqualityComparer();
        public static readonly IEqualityComparer<Rect> Rect = new RectEqualityComparer();
        public static readonly IEqualityComparer<Bounds> Bounds = new BoundsEqualityComparer();
        public static readonly IEqualityComparer<Quaternion> Quaternion = new QuaternionEqualityComparer();

        static readonly RuntimeTypeHandle vector2Type = typeof(Vector2).TypeHandle;
        static readonly RuntimeTypeHandle vector3Type = typeof(Vector3).TypeHandle;
        static readonly RuntimeTypeHandle vector4Type = typeof(Vector4).TypeHandle;
        static readonly RuntimeTypeHandle colorType = typeof(Color).TypeHandle;
        static readonly RuntimeTypeHandle color32Type = typeof(Color32).TypeHandle;
        static readonly RuntimeTypeHandle rectType = typeof(Rect).TypeHandle;
        static readonly RuntimeTypeHandle boundsType = typeof(Bounds).TypeHandle;
        static readonly RuntimeTypeHandle quaternionType = typeof(Quaternion).TypeHandle;

#if UNITY_2017_2_OR_NEWER

        public static readonly IEqualityComparer<Vector2Int> Vector2Int = new Vector2IntEqualityComparer();
        public static readonly IEqualityComparer<Vector3Int> Vector3Int = new Vector3IntEqualityComparer();
        public static readonly IEqualityComparer<RangeInt> RangeInt = new RangeIntEqualityComparer();
        public static readonly IEqualityComparer<RectInt> RectInt = new RectIntEqualityComparer();
        public static readonly IEqualityComparer<BoundsInt> BoundsInt = new BoundsIntEqualityComparer();

        static readonly RuntimeTypeHandle vector2IntType = typeof(Vector2Int).TypeHandle;
        static readonly RuntimeTypeHandle vector3IntType = typeof(Vector3Int).TypeHandle;
        static readonly RuntimeTypeHandle rangeIntType = typeof(RangeInt).TypeHandle;
        static readonly RuntimeTypeHandle rectIntType = typeof(RectInt).TypeHandle;
        static readonly RuntimeTypeHandle boundsIntType = typeof(BoundsInt).TypeHandle;

#endif

        static class Cache<T>
        {
            public static readonly IEqualityComparer<T> Comparer;

            static Cache()
            {
                var comparer = GetDefaultHelper(typeof(T));
                if (comparer == null)
                {
                    Comparer = EqualityComparer<T>.Default;
                }
                else
                {
                    Comparer = (IEqualityComparer<T>)comparer;
                }
            }
        }

        public static IEqualityComparer<T> GetDefault<T>()
        {
            return Cache<T>.Comparer;
        }

        static object GetDefaultHelper(Type type)
        {
            var t = type.TypeHandle;

            if (t.Equals(vector2Type)) return Vector2;
            if (t.Equals(vector3Type)) return Vector3;
            if (t.Equals(vector4Type)) return Vector4;
            if (t.Equals(colorType)) return Color;
            if (t.Equals(color32Type)) return Color32;
            if (t.Equals(rectType)) return Rect;
            if (t.Equals(boundsType)) return Bounds;
            if (t.Equals(quaternionType)) return Quaternion;

#if UNITY_2017_2_OR_NEWER

            if (t.Equals(vector2IntType)) return Vector2Int;
            if (t.Equals(vector3IntType)) return Vector3Int;
            if (t.Equals(rangeIntType)) return RangeInt;
            if (t.Equals(rectIntType)) return RectInt;
            if (t.Equals(boundsIntType)) return BoundsInt;
#endif

            return null;
        }

        sealed class Vector2EqualityComparer : IEqualityComparer<Vector2>
        {
            public bool Equals(Vector2 self, Vector2 vector)
            {
                return self.x.Equals(vector.x) && self.y.Equals(vector.y);
            }

            public int GetHashCode(Vector2 obj)
            {
                return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2;
            }
        }

        sealed class Vector3EqualityComparer : IEqualityComparer<Vector3>
        {
            public bool Equals(Vector3 self, Vector3 vector)
            {
                return self.x.Equals(vector.x) && self.y.Equals(vector.y) && self.z.Equals(vector.z);
            }

            public int GetHashCode(Vector3 obj)
            {
                return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2 ^ obj.z.GetHashCode() >> 2;
            }
        }

        sealed class Vector4EqualityComparer : IEqualityComparer<Vector4>
        {
            public bool Equals(Vector4 self, Vector4 vector)
            {
                return self.x.Equals(vector.x) && self.y.Equals(vector.y) && self.z.Equals(vector.z) && self.w.Equals(vector.w);
            }

            public int GetHashCode(Vector4 obj)
            {
                return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2 ^ obj.z.GetHashCode() >> 2 ^ obj.w.GetHashCode() >> 1;
            }
        }

        sealed class ColorEqualityComparer : IEqualityComparer<Color>
        {
            public bool Equals(Color self, Color other)
            {
                return self.r.Equals(other.r) && self.g.Equals(other.g) && self.b.Equals(other.b) && self.a.Equals(other.a);
            }

            public int GetHashCode(Color obj)
            {
                return obj.r.GetHashCode() ^ obj.g.GetHashCode() << 2 ^ obj.b.GetHashCode() >> 2 ^ obj.a.GetHashCode() >> 1;
            }
        }

        sealed class RectEqualityComparer : IEqualityComparer<Rect>
        {
            public bool Equals(Rect self, Rect other)
            {
                return self.x.Equals(other.x) && self.width.Equals(other.width) && self.y.Equals(other.y) && self.height.Equals(other.height);
            }

            public int GetHashCode(Rect obj)
            {
                return obj.x.GetHashCode() ^ obj.width.GetHashCode() << 2 ^ obj.y.GetHashCode() >> 2 ^ obj.height.GetHashCode() >> 1;
            }
        }

        sealed class BoundsEqualityComparer : IEqualityComparer<Bounds>
        {
            public bool Equals(Bounds self, Bounds vector)
            {
                return self.center.Equals(vector.center) && self.extents.Equals(vector.extents);
            }

            public int GetHashCode(Bounds obj)
            {
                return obj.center.GetHashCode() ^ obj.extents.GetHashCode() << 2;
            }
        }

        sealed class QuaternionEqualityComparer : IEqualityComparer<Quaternion>
        {
            public bool Equals(Quaternion self, Quaternion vector)
            {
                return self.x.Equals(vector.x) && self.y.Equals(vector.y) && self.z.Equals(vector.z) && self.w.Equals(vector.w);
            }

            public int GetHashCode(Quaternion obj)
            {
                return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2 ^ obj.z.GetHashCode() >> 2 ^ obj.w.GetHashCode() >> 1;
            }
        }

        sealed class Color32EqualityComparer : IEqualityComparer<Color32>
        {
            public bool Equals(Color32 self, Color32 vector)
            {
                return self.a.Equals(vector.a) && self.r.Equals(vector.r) && self.g.Equals(vector.g) && self.b.Equals(vector.b);
            }

            public int GetHashCode(Color32 obj)
            {
                return obj.a.GetHashCode() ^ obj.r.GetHashCode() << 2 ^ obj.g.GetHashCode() >> 2 ^ obj.b.GetHashCode() >> 1;
            }
        }

#if UNITY_2017_2_OR_NEWER

        sealed class Vector2IntEqualityComparer : IEqualityComparer<Vector2Int>
        {
            public bool Equals(Vector2Int self, Vector2Int vector)
            {
                return self.x.Equals(vector.x) && self.y.Equals(vector.y);
            }

            public int GetHashCode(Vector2Int obj)
            {
                return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2;
            }
        }

        sealed class Vector3IntEqualityComparer : IEqualityComparer<Vector3Int>
        {
            public static readonly Vector3IntEqualityComparer Default = new Vector3IntEqualityComparer();

            public bool Equals(Vector3Int self, Vector3Int vector)
            {
                return self.x.Equals(vector.x) && self.y.Equals(vector.y) && self.z.Equals(vector.z);
            }

            public int GetHashCode(Vector3Int obj)
            {
                return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2 ^ obj.z.GetHashCode() >> 2;
            }
        }

        sealed class RangeIntEqualityComparer : IEqualityComparer<RangeInt>
        {
            public bool Equals(RangeInt self, RangeInt vector)
            {
                return self.start.Equals(vector.start) && self.length.Equals(vector.length);
            }

            public int GetHashCode(RangeInt obj)
            {
                return obj.start.GetHashCode() ^ obj.length.GetHashCode() << 2;
            }
        }

        sealed class RectIntEqualityComparer : IEqualityComparer<RectInt>
        {
            public bool Equals(RectInt self, RectInt other)
            {
                return self.x.Equals(other.x) && self.width.Equals(other.width) && self.y.Equals(other.y) && self.height.Equals(other.height);
            }

            public int GetHashCode(RectInt obj)
            {
                return obj.x.GetHashCode() ^ obj.width.GetHashCode() << 2 ^ obj.y.GetHashCode() >> 2 ^ obj.height.GetHashCode() >> 1;
            }
        }

        sealed class BoundsIntEqualityComparer : IEqualityComparer<BoundsInt>
        {
            public bool Equals(BoundsInt self, BoundsInt vector)
            {
                return Vector3IntEqualityComparer.Default.Equals(self.position, vector.position)
                    && Vector3IntEqualityComparer.Default.Equals(self.size, vector.size);
            }

            public int GetHashCode(BoundsInt obj)
            {
                return Vector3IntEqualityComparer.Default.GetHashCode(obj.position) ^ Vector3IntEqualityComparer.Default.GetHashCode(obj.size) << 2;
            }
        }

#endif
    }

    #endregion
}