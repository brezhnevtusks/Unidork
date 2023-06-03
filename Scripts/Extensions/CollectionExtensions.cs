using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unidork.Extensions
{
    public static class CollectionExtensions
    {
        #region General

        /// <summary>
        /// Checks whether a collection is null or empty.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to check.</param>
        /// <returns>
        /// True when collection is either null or empty, False otherwise.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// Checks whether an enumerable sequence is null or empty.
        /// </summary>
        /// <param name="collection">Sequence to check.</param>
        /// <returns>
        /// True if sequence is null or empty, False otherwise.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection == null || !collection.Any();

		/// <summary>
		/// Checks whether a collection is empty.
		/// </summary>
		/// <typeparam name="T">Element type of the collection.</typeparam>
		/// <param name="collection">Collection to check.</param>
		/// <returns>
		/// True when collection is empty, False otherwise.
		/// </returns>
		public static bool IsEmpty<T>(this ICollection<T> collection) => collection.Count == 0;

        /// <summary>
        /// Checks whether an enumerable sequence is  empty.
        /// </summary>
        /// <param name="collection">Sequence to check.</param>
        /// <returns>
        /// True if sequence is empty, False otherwise.
        /// </returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection) => !collection.Any();

        #endregion

        #region Random

        /// <summary>
        /// Gets a random item in a collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to get the random element from.</param>
        /// <param name="random">Random object to use.</param>
        /// <param name="removeElementFromCollection">Should the random element also be removed from collection?</param>
        /// <returns>
        /// A random element of the collection, or default value of <typeparamref name="T"/> if collection is null or empty.
        /// </returns>
        public static T GetRandomOrDefault<T>(this IList<T> collection, System.Random random = null, bool removeElementFromCollection = false)
        {
            if (collection == null)
            {
                return default;
            }

            int count = collection.Count;

            if (count == 0)
            {
                return default;
            }

            random ??= new System.Random();
            
            int randomIndex = random.Next(0, count);

			return removeElementFromCollection ? collection.PopAtIndex(randomIndex) : collection[randomIndex];
		}

        /// <summary>
        /// Gets a random item in a collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to get the random element from.</param>
        /// <param name="defaultValue">Default value to return if collection is null or empty.</param>
        /// <param name="random">Random object to use.</param>
        /// <param name="removeElementFromCollection">Should the random element also be removed from collection?</param>
        /// <returns>
        /// A random element of the collection, or the provided default value if collection is null or empty
        /// </returns>
        public static T GetRandomOrDefault<T>(this IList<T> collection, T defaultValue, System.Random random = null, bool removeElementFromCollection = false)
        {
            if (collection == null)
            {
                return defaultValue;
            }

            int count = collection.Count;

            if (count == 0)
            {
                return defaultValue;
            }

            random ??= new System.Random();
            int randomIndex = random.Next(0, count);

            return removeElementFromCollection ? collection.PopAtIndex(randomIndex) : collection[randomIndex];
        }

        /// <summary>
        /// Gets a random item in a set.
        /// </summary>
        /// <remarks>
        /// This method is required in addition to <see cref="GetRandomOrDefault{T}(IList{T})"/> since sets do not implement IList.
        /// </remarks>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="set">Set to get the random element from.</param>
        /// <param name="random">Random object to use.</param>
        /// <param name="removeElementFromSet">Should the random element also be removed from the set?</param>
        /// <returns>
        /// A random element of a set, or default value of <typeparamref name="T"/> if set is null or empty.
        /// </returns>
        public static T GetRandomOrDefault<T>(this ISet<T> set, System.Random random = null, bool removeElementFromSet = false)
        {
            if (set == null)
            {
                return default;
            }

            int count = set.Count;

            if (count == 0)
            {
                return default;
            }

            random ??= new System.Random();
            
            int randomIndex = random.Next(0, count);
            int currentIndex = 0;

            if (removeElementFromSet)
			{
                T randomElement = default;

                foreach (var element in set)
                {
                    if (currentIndex == randomIndex)
                    {
                        randomElement = element;
                        break;
                    }

                    currentIndex++;
                }

                set.Remove(randomElement);
                return randomElement;
            }
            
            foreach (var element in set)
            {
                if (currentIndex == randomIndex)
                {
                    return element;
                }

                currentIndex++;
            }

            return default;
        }

        /// <summary>
        /// Gets a random item in a set.
        /// </summary>
        /// <remarks>
        /// This method is required in addition to <see cref="GetRandomOrDefault{T}(IList{T}, T)"/> since sets do not implement IList.
        /// </remarks>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="set">Set to get the random element from.</param>
        /// <param name="defaultValue">Default value to return if set is null or empty.</param>
        /// <param name="random">Random object to use.</param>
        /// <param name="removeElementFromSet">Should the random element also be removed from the set?</param>
        /// <returns>
        /// A random element of the set, or the provided default value if collection is null or empty.
        /// </returns>
        public static T GetRandomOrDefault<T>(this ISet<T> set, T defaultValue, System.Random random = null, bool removeElementFromSet = false)
        {
            if (set == null)
            {
                return defaultValue;
            }

            int count = set.Count;

            if (count == 0)
            {
                return defaultValue;
            }

            random ??= new System.Random();
            
            int randomIndex = random.Next(0, count);
            int currentIndex = 0;

            if (removeElementFromSet)
            {
                T randomElement = default;

                foreach (var element in set)
                {
                    if (currentIndex == randomIndex)
                    {
                        randomElement = element;
                        break;
                    }

                    currentIndex++;
                }

                set.Remove(randomElement);
                return randomElement;
            }
            else
            {
                foreach (var element in set)
                {
                    if (currentIndex == randomIndex)
                    {
                        return element;
                    }

                    currentIndex++;
                }
            }

            return defaultValue;
        }

	    /// <summary>
	    /// Pops an item at specified index in a collection.
	    /// </summary>
	    /// <typeparam name="T">Element type of the collection.</typeparam>
	    /// <param name="collection">Collection to get the element from.</param>
	    /// <param name="index">Element index.</param>
	    /// <returns>
	    /// An element of the collection at the passed index, or default value of <typeparamref name="T"/> if collection is null or empty.
	    /// </returns>
	    public static T PopAtIndex<T>(this IList<T> collection, int index)
	    {
		    if (collection == null)
		    {
			    return default;
		    }

		    int count = collection.Count;

		    if (count == 0)
		    {
			    return default;
		    }

		    T elementToPop = collection[index];
		    
		    collection.RemoveAt(index);

		    return elementToPop;
	    }
	    
	    /// <summary>
	    /// Pops a random item in a collection.
	    /// </summary>
	    /// <typeparam name="T">Element type of the collection.</typeparam>
	    /// <param name="collection">Collection to get the random element from.</param>
	    /// <param name="random">Random object to use.</param>
	    /// <returns>
	    /// A random element of the collection, or default value of <typeparamref name="T"/> if collection is null or empty.
	    /// </returns>
	    public static T PopRandomOrDefault<T>(this IList<T> collection, System.Random random = null)
	    {
		    if (collection == null)
		    {
			    return default;
		    }

		    int count = collection.Count;

		    if (count == 0)
		    {
			    return default;
		    }

		    random ??= new System.Random();
		    int randomIndex = random.Next(0, count);

		    T randomElement = collection[randomIndex];

		    collection.RemoveAt(randomIndex);
		    
		    return randomElement;
	    }
	    
	    /// <summary>
	    /// Pops a random item in a collection.
	    /// </summary>
	    /// <typeparam name="T">Element type of the collection.</typeparam>
	    /// <param name="collection">Collection to get the random element from.</param>
	    /// <param name="defaultValue">Default value to return if collection is null or empty.</param>
	    /// <param name="random">Random object to use.</param>
	    /// <returns>
	    /// A random element of the collection, or the provided default value if collection is null or empty
	    /// </returns>
	    public static T PopRandomOrDefault<T>(this IList<T> collection, T defaultValue, System.Random random = null)
	    {
		    if (collection == null)
		    {
			    return defaultValue;
		    }

		    int count = collection.Count;

		    if (count == 0)
		    {
			    return defaultValue;
		    }

		    random ??= new System.Random();
		    int randomIndex = random.Next(0, count);

		    T randomElement = collection[randomIndex];

		    collection.RemoveAt(randomIndex);
		    
		    return randomElement;
	    }
	    
	    /// <summary>
        /// Pops a random item in a set.
        /// </summary>
        /// <remarks>
        /// This method is required in addition to <see cref="GetRandomOrDefault{T}(IList{T})"/> since sets do not implement IList.
        /// </remarks>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="set">Set to get the random element from.</param>
	    /// <param name="random">Random object to use.</param>
        /// <returns>
        /// A random element of a set, or default value of <typeparamref name="T"/> if set is null or empty.
        /// </returns>
        public static T PopRandomOrDefault<T>(this ISet<T> set, System.Random random = null)
        {
            if (set == null)
            {
                return default;
            }

            int count = set.Count;

            if (count == 0)
            {
                return default;
            }

            random ??= new System.Random();
            int randomIndex = random.Next(0, count);
            
            int currentIndex = 0;
	        T randomElement = default;
	        
            foreach (var element in set)
            {
                if (currentIndex == randomIndex)
                {
	                randomElement = element;
	                break;
                }

                currentIndex++;
            }

	        _ = set.Remove(randomElement);

            return randomElement;
        }

        /// <summary>
        /// Gets a random item in a set.
        /// </summary>
        /// <remarks>
        /// This method is required in addition to <see cref="GetRandomOrDefault{T}(IList{T}, T)"/> since sets do not implement IList.
        /// </remarks>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="set">Set to get the random element from.</param>
        /// <param name="defaultValue">Default value to return if set is null or empty.</param>
        /// <param name="random">Random object to use.</param>
        /// <returns>
        /// A random element of the set, or the provided default value if collection is null or empty.
        /// </returns>
        public static T PopRandomOrDefault<T>(this ISet<T> set, T defaultValue, System.Random random = null)
        {
            if (set == null)
            {
                return defaultValue;
            }

            int count = set.Count;

            if (count == 0)
            {
                return defaultValue;
            }

            random ??= new System.Random();
            int randomIndex = random.Next(0, count);
            
            int currentIndex = 0;
	        T randomElement = default;

            foreach (var element in set)
            {
                if (currentIndex == randomIndex)
                {
                    randomElement = element;
	                break;
                }

                currentIndex++;
            }

	        _ = set.Remove(randomElement);

	        return randomElement;
        }

        #endregion

        #region Index

        /// <summary>
        /// Gets the first element of a collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to get the first element of.</param>
        /// <returns>
        /// The first element of the collection.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when collection is empty.</exception>
        public static T First<T>(this IList<T> collection)
        {
            if (collection.IsEmpty())
            {
	            throw new IndexOutOfRangeException("First() called on an empty collection!");
            }

            return collection[0];
        }

	    /// <summary>
	    /// Gets the second element of a collection.
	    /// </summary>
	    /// <typeparam name="T">Element type of the collection.</typeparam>
	    /// <param name="collection">Collection to get the second element of.</param>
	    /// <returns>
	    /// The second element of the collection.
	    /// </returns>
	    /// <exception cref="IndexOutOfRangeException">Thrown when collection has less than two members.</exception>
	    public static T Second<T>(this IList<T> collection)
	    {
		    if (collection.Count < 2)
		    {
			    throw new IndexOutOfRangeException("Second() called on a collection with less than two members!");
		    }
		    
		    return collection[1];
	    }
	    
	    /// <summary>
	    /// Gets the third element of a collection.
	    /// </summary>
	    /// <typeparam name="T">Element type of the collection.</typeparam>
	    /// <param name="collection">Collection to get the third element of.</param>
	    /// <returns>
	    /// The third element of the collection.
	    /// </returns>
	    /// <exception cref="IndexOutOfRangeException">Thrown when collection has less than three members.</exception>
	    public static T Third<T>(this IList<T> collection)
	    {
		    if (collection.Count < 3)
		    {
			    throw new IndexOutOfRangeException("Third() called on a collection with less than three members!");
		    }
		    
		    return collection[2];
	    }
	    
	    /// <summary>
	    /// Gets the fourth element of a collection.
	    /// </summary>
	    /// <typeparam name="T">Element type of the collection.</typeparam>
	    /// <param name="collection">Collection to get the fourth element of.</param>
	    /// <returns>
	    /// The fourth element of the collection.
	    /// </returns>
	    /// <exception cref="IndexOutOfRangeException">Thrown when collection has less than four members.</exception>
	    public static T Fourth<T>(this IList<T> collection)
	    {
		    if (collection.Count < 4)
		    {
			    throw new IndexOutOfRangeException("Fourth() called on a collection with less than four members!");
		    }
		    
		    return collection[3];
	    }
	    
	    /// <summary>
	    /// Gets the fifth element of a collection.
	    /// </summary>
	    /// <typeparam name="T">Element type of the collection.</typeparam>
	    /// <param name="collection">Collection to get the fifth element of.</param>
	    /// <returns>
	    /// The fifth element of the collection.
	    /// </returns>
	    /// <exception cref="IndexOutOfRangeException">Thrown when collection has less than five members.</exception>
	    public static T Fifth<T>(this IList<T> collection)
	    {
		    if (collection.Count < 5)
		    {
			    throw new IndexOutOfRangeException("Fifth() called on a collection with less than five members!");
		    }
		    
		    return collection[4];
	    }

        /// <summary>
        /// Gets the first element of an enumerable sequence.
        /// </summary>
        /// <typeparam name="T">Element type of the sequence.</typeparam>
        /// <param name="collection">Enumerable sequence to get the first element from.</param>
        /// <returns>
        /// First element of the sequence or default value of <typeparamref name="T"/> when the sequence is null or empty.
        /// </returns>
        public static T FirstElement<T>(this IEnumerable<T> collection)
        {
	        IEnumerator<T> enumerator = collection.GetEnumerator();
	        T element = enumerator.MoveNext() ? enumerator.Current : default;
	        
	        enumerator.Dispose();

	        return element;
        }

        /// <summary>
        /// Gets the first element of a collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to get the first element of.</param>
        /// <param name="defaultValue">Default value to return if collection is null or empty.</param>
        /// <returns>
        /// The first element of the collection or the provided default value if collection is null or empty.
        /// </returns>
        public static T FirstOrDefault<T>(this IList<T> collection, T defaultValue)
        {
            if (collection.IsEmpty())
            {
                return defaultValue;
            }

            return collection[0];
        }

        /// <summary>
        /// Gets the first element of an enumerable sequence.
        /// </summary>
        /// <typeparam name="T">Element type of the enumerable sequence.</typeparam>
        /// <param name="collection">Enumerable sequence to get the first element of.</param>
        /// <param name="defaultValue">Default value to return if enumerable sequence is null or empty.</param>
        /// <returns>
        /// The first element of the enumerable sequence or the provided default value if collection is null or empty.
        /// </returns>
        public static T FirstOrDefault<T>(this IEnumerable<T> collection, T defaultValue)
        {
	        IEnumerator<T> enumerator = collection.GetEnumerator();
	        T element = enumerator.MoveNext() ? enumerator.Current : defaultValue;
	        
	        enumerator.Dispose();

	        return element;
        }

        /// <summary>
        /// Gets the last element of a collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to get the last element of.</param>
        /// <returns>
        /// The last element of the collection or default value of <typeparamref name="T"/> if collection is null or empty.
        /// </returns>
        public static T Last<T>(this IList<T> collection)
        {
            if (collection.IsEmpty())
            {
                return default;
            }

            return collection[collection.Count - 1];
        }

        /// <summary>
        /// Gets the last element of an enumerable sequence.
        /// </summary>
        /// <typeparam name="T">Element type of the sequence.</typeparam>
        /// <param name="collection">Enumerable sequence to get the last element from.</param>
        /// <returns>
        /// Last element of the sequence or default value of <typeparamref name="T"/> when the sequence is null or empty.
        /// </returns>
        public static T LastElement<T>(this IEnumerable<T> collection)
        {
	        IEnumerator<T> enumerator = collection.GetEnumerator();

	        T element = default;

	        while (enumerator.MoveNext())
	        {
		        element = enumerator.Current;
	        }
	        
	        enumerator.Dispose();

	        return element;
        }

        /// <summary>
        /// Gets the last element of a collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to get the last element of.</param>
        /// <param name="defaultValue">Default value to return if collection is null or empty.</param>
        /// <returns>
        /// The last element of the collection or the provided default value if collection is null or empty.
        /// </returns>
        public static T LastOrDefault<T>(this IList<T> collection, T defaultValue)
        {
	        IEnumerator<T> enumerator = collection.GetEnumerator();

	        T element = defaultValue;

	        while (enumerator.MoveNext())
	        {
		        element = enumerator.Current;
	        }
	        
	        enumerator.Dispose();

	        return element;
        }

        /// <summary>
        /// Gets the last element of an enumerable sequence.
        /// </summary>
        /// <typeparam name="T">Element type of the enumerable sequence.</typeparam>
        /// <param name="collection">Enumerable sequence to get the last element of.</param>
        /// <param name="defaultValue">Default value to return if enumerable sequence is null or empty.</param>
        /// <returns>
        /// The last element of the enumerable sequence or the provided default value if collection is null or empty.
        /// </returns>
        public static T LastOrDefault<T>(this IEnumerable<T> collection, T defaultValue)
        {
            if (collection.IsEmpty())
            {
                return defaultValue;
            }

            return collection.Last();
        }

        /// <summary>
        /// Checks whether a collection contains an index.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection.</param>
        /// <param name="index">Index.</param>
        /// <returns>
        /// True if the passed index is withing the bounds of the collection, False ortherwise.
        /// </returns>
        public static bool ContainsIndex<T>(this IList<T> collection, int index)
		{
            if (collection.IsEmpty())
			{
                return false;
			}

            return index >= 0 && index < collection.Count;
		}

	    /// <summary>
	    /// Gets the index of an element in a collection.
	    /// </summary>
	    /// <param name="collection">Collection.</param>
	    /// <param name="element">Element.</param>
	    /// <typeparam name="T">Element type of the collection.</typeparam>
	    /// <returns>Index of the element or -1 if collection is empty or doesn't contain the element.</returns>
	    public static int IndexOf<T>(this ICollection<T> collection, T element)
	    {
		    if (collection.IsEmpty())
		    {
			    return -1;
		    }
		    
		    IEnumerator<T> enumerator = collection.GetEnumerator();
		    var iteratorIndex = 0;

		    while (enumerator.MoveNext())
		    {
			    if (element.Equals(enumerator.Current))
			    {
				    return iteratorIndex;
			    }
			    
			    iteratorIndex++;
		    }
		    
		    enumerator.Dispose();

		    return -1;
	    }

        #endregion

        #region Add/Remove

        /// <summary>
        /// Adds an element to a collection only if it doesn't contain it already and the element is not null.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to add the element to.</param>
        /// <param name="elementToAdd">Element to add.</param>
        /// <returns>
        /// True when an element is successfully added to the collection, False otherwise.
        /// </returns>
        public static bool AddUnique<T>(this ICollection<T> collection, T elementToAdd)
        {
            if (elementToAdd == null || collection.Contains(elementToAdd))
            {
                return false;
            }

            collection.Add(elementToAdd);
            return true;
        }

        /// <summary>
        /// Adds an element to a collection only if the element is not null.
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to add the element to.</param>
        /// <param name="elementToAdd">Element to add.</param>
        /// <returns>
        /// True when an element is successfully added to the collection, False otherwise.
        /// </returns>
        public static bool AddIfNotNull<T>(this ICollection<T> collection, T elementToAdd)
        {
            if (elementToAdd == null)
            {
                return false;
            }

            collection.Add(elementToAdd);
            return true;
        }

        /// <summary>
        /// Adds an element to a collection at the specified index.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to add the element to.</param>
        /// <param name="elementToAdd">Element to add.</param>
        /// <param name="index">Index at which to add the element.</param>
        /// <returns>
        /// True when the element is successfully added, False otherwise.
        /// </returns>
        public static bool AddAt<T>(this IList<T> collection, T elementToAdd, int index)
        {
            if (collection == null)
            {
                Debug.LogError("Trying to add an element to a null collection. Make sure you initialize the collection first!");
                return false;
            }

            collection.Insert(index, elementToAdd);
            return true;
        }

		/// <summary>
		/// Adds elements from one collection to another.
		/// </summary>
		/// <typeparam name="T">Element type of the collection.</typeparam>
		/// <param name="collection">Collection to add missing elements to.</param>
		/// <param name="otherCollection">Other collection that contains potential elements to add.</param>
		/// <returns>
		/// Number of elements that were added.
		/// </returns>
		public static int AddRange<T>(this ICollection<T> collection, ICollection<T> otherCollection)
		{
			var numberOfAddedElements = 0;

			if (otherCollection == null)
			{
				Debug.LogError("AddRange was called to add elements from a null collection!");
				return numberOfAddedElements;
			}

			if (otherCollection.Count == 0)
			{
				return numberOfAddedElements;
			}

			foreach (var otherCollectionElement in otherCollection)
			{
				collection.Add(otherCollectionElement);				
				numberOfAddedElements++;				
			}

			return numberOfAddedElements;
		}

        /// <summary>
        /// Adds elements to a collection from another collection that the first collection doesn't have.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to add missing elements to.</param>
        /// <param name="otherCollection">Other collection that contains potential elements to add.</param>
		/// <returns>
		/// Number of elements that were added.
		/// </returns>
        public static int AddRangeUnique<T>(this ICollection<T> collection, ICollection<T> otherCollection)
        {
            var numberOfAddedElements = 0;

			if (otherCollection.IsNullOrEmpty())
			{
				return numberOfAddedElements;
			}

			foreach (var otherCollectionElement in otherCollection)
			{
				if (collection.AddUnique(otherCollectionElement))
				{
					numberOfAddedElements++;
				}
			}

			return numberOfAddedElements;
        }

        /// <summary>
        /// Appends an element to the end of the array.
        /// </summary>
        /// <typeparam name="T">Type of array element.</typeparam>
        /// <param name="array">Array.</param>
        /// <param name="element">Element to add.</param>
        /// <returns>
        /// An array of <typeparamref name="T"/>.
        /// </returns>        
        public static T[] Append<T>(this T[] array, T element)
        {
            if (array == null)
            {
                return new T[] { element };
            }

            T[] result = new T[array.Length + 1];
            array.CopyTo(result, 0);
            result[array.Length] = element;

            return result;
        }

        /// <summary>
        /// Removes the first element from a collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to remove the first element of.</param>
        /// <returns>
        /// True when an element is successfully removed, False otherwise.
        /// </returns>
        public static bool RemoveFirst<T>(this IList<T> collection)
        {
            if (collection.IsEmpty())
            {
                return false;
            }

            collection.RemoveAt(0);

            return true;
        }

        /// <summary>
        /// Removes the last element from a collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to remove the last element of.</param>
        /// <returns>
        /// True when an element is successfully removed, False otherwise.
        /// </returns>
        public static bool RemoveLast<T>(this IList<T> collection)
        {
            if (collection.IsEmpty())
            {
                return false;
            }

            collection.RemoveAt(collection.Count - 1);

            return true;
        }

        /// <summary>
        /// Removes elements of one collection from another collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to remove elements from.</param>
        /// <param name="collectionToRemove">Collection to remove.</param>
        public static void RemoveRange<T>(this ICollection<T> collection, ICollection<T> collectionToRemove)
		{
            if (collectionToRemove.IsNullOrEmpty())
			{
                return;
			}

            foreach (T elementToRemove in collectionToRemove)
			{
                _ = collection.Remove(elementToRemove);
			}
        }

        /// <summary>
        /// Removes all elements in a collection that match an element in another collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to remove matching elements from.</param>
        /// <param name="otherCollection">Other collection that contains potential matches.</param>
        public static void RemoveMatchingElements<T>(this ICollection<T> collection, ICollection<T> otherCollection)
        {
	        foreach (var otherCollectionElement in otherCollection)
            {
                if (!collection.Contains(otherCollectionElement))
                {
	                continue;
                }

	            _ = collection.Remove(otherCollectionElement);
            }
        }

        #endregion

	    #region Move

	    /// <summary>
	    /// Moves an element of a list collection from current index to a new one.
	    /// </summary>
	    /// <param name="list">List.</param>
	    /// <param name="oldIndex">Old index.</param>
	    /// <param name="newIndex">New index.</param>
	    /// <typeparam name="T">Type of list elements.</typeparam>
	    /// <returns>True if the move was successfully performed, False otherwise.</returns>
	    public static bool MoveElement<T>(this IList<T> list, int oldIndex, int newIndex)
	    {
		    if (oldIndex == newIndex)
		    {
			    return false;
		    }

		    int numberOfElements = list.Count;

		    if (oldIndex < 0 || oldIndex >= numberOfElements)
		    {
			    return false;
		    }

		    if (newIndex < 0 || newIndex >= numberOfElements)
		    {
			    return false;
		    }
		    
		    T elementToMove = list[oldIndex];
		    
		    if (oldIndex < newIndex)
		    {
			    for (var i = oldIndex; i < newIndex; i++)
			    {
				    list[i] = list[i + 1];
			    }
		    }
		    else
		    {
			    for (var i = oldIndex; i > newIndex; i--)
			    {
				    list[i] = list[i - 1];
			    }
		    }
		 
		    list[newIndex] = elementToMove;
		    
		    return true;
	    }

	    #endregion

        #region Contains

		/// <summary>
		/// Checks whether an array contains a specified element.
		/// </summary>
		/// <typeparam name="T">Type of array element.</typeparam>
		/// <param name="array">Array to check.</param>
		/// <param name="elementToCheckFor">Element to check for.</param>
		/// <returns>
		/// True if <paramref name="array"/> contains <paramref name="elementToCheckFor"/>, False if it doesn't or if the array is null or empty.
		/// </returns>
		public static bool Contains<T>(this T[] array, T elementToCheckFor)
		{
			if (array.IsNullOrEmpty())
			{
				return false;
			}

			for (int i = 0; i < array.Length; i++)
			{
                T currentElement = array[i];

                if(currentElement == null)
                {
                    continue;
                }

				if (currentElement.Equals(elementToCheckFor))
				{
					return true;
				}
			}

			return false;
		}

        /// <summary>
        /// Checks whether a collection contains any element of another collection.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Collection to check.</param>
        /// <param name="otherCollection">Other collection.</param>
        /// <returns>
        /// True when collections have at least one common element, False otherwise.
        /// </returns>
        public static bool ContainsAnyOfElements<T>(this ICollection<T> collection, ICollection<T> otherCollection)
        {
            foreach (var otherCollectionElement in otherCollection)
            {
                if (collection.Contains(otherCollectionElement))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Shuffle

        /// <summary>
        /// Shuffles a collection implementing the IList interfaces using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">Type of collection element.</typeparam>
        /// <param name="collection">Collection to shuffle.</param>
        /// <param name="random">Random object to use.</param>
        public static void FisherYatesShuffle<T>(this IList<T> collection, System.Random random = null)
        {
	        random ??= new System.Random();
	        
            for (int i = collection.Count - 1; i > 1; i--)
            {
	            int randomIndex = random.Next(0, i + 1);

                T tempValue = collection[randomIndex];
                collection[randomIndex] = collection[i];
                collection[i] = tempValue;
            }
        }

        #endregion

        #region To Collection

        /// <summary>
        /// Creates an array from an enumerable sequence.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Sequence to use for creating the array.</param>
        /// <param name="count">Array size used for memory efficiency.</param>
        /// <exception cref="System.ArgumentException">Thrown when collection we are trying to convert to array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the value of <paramref name="count"/> is negative.</exception>  
        /// <returns>
        /// An array containing the passed collection.
        /// </returns>
        public static T[] ToArray<T>(this IEnumerable<T> collection, int count)
	    {
		    if (collection == null)
		    {
			    throw new ArgumentException("Trying to create an array from a null collection!");
		    }

		    if (count < 0)
		    {
			    throw new ArgumentOutOfRangeException($"Received an invalid array size: {count}!");
		    }
		    
		    var array = new T[count];
		    
		    var i = 0;
		    
		    foreach (var element in collection)
		    {
			    array[i++] = element;
		    }
		    return array;
	    }
	    
        /// <summary>
        /// Creates a list from an enumerable sequence.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Sequence to use for creating the list.</param>
        /// <param name="count">List size used for memory efficiency.</param>
        /// <exception cref="System.ArgumentException">Thrown when collection we are trying to convert to list is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the value of <paramref name="count"/> is negative.</exception>  
        /// <returns>
        /// A list containing the passed collection.
        /// </returns>
        public static List<T> ToList<T>(this IEnumerable<T> collection, int count)
        {
	        if (collection == null)
	        {
		        throw new ArgumentException("Trying to create a list from a null collection!");
	        }

	        if (count < 0)
	        {
		        throw new ArgumentOutOfRangeException($"Received an invalid list size: {count}!");
	        }
	        
	        List<T> list = new List<T>(count);
	        list.AddRange(collection);
		    
		    return list;
	    }

	    /// <summary>
        /// Creates a hash set from an enumerable sequence.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Sequence to use for creating the hash set.</param>
        /// <param name="comparer">Equality comparer to be used by the hash set.</param>
        /// <exception cref="System.ArgumentException">Thrown when collection we are trying to convert to hash set is null.</exception>  
        /// <returns>
        /// A hash set containing the passed collection and using the passed comparer.
        /// </returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection, IEqualityComparer<T> comparer = null)
	    {
		    if (collection == null)
		    {
			    throw new ArgumentException($"Trying to create a hash set from a null collection!");
		    }
		    
		    return new HashSet<T>(collection, comparer);
	    }

	    /// <summary>
        /// Creates a queue from an enumerable sequence.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Sequence to use for creating the queue.</param>
        /// <exception cref="System.ArgumentException">Thrown when collection we are trying to convert to queue is null.</exception>  
        /// <returns>
        /// A queue containing the passed collection.
        /// </returns>
        public static Queue<T> ToQueue<T>(this IEnumerable<T> collection)
	    {
		    if (collection == null)
		    {
			    throw new ArgumentException($"Trying to create a queue from a null collection!");
		    }
		    
		    return new Queue<T>(collection);
	    }

	    /// <summary>
        /// Creates a stack from an enumerable sequence.
        /// </summary>
        /// <typeparam name="T">Element type of the collection.</typeparam>
        /// <param name="collection">Sequence to use for creating the stack.</param>
        /// <exception cref="System.ArgumentException">Thrown when collection we are trying to convert to stack is null.</exception> 
        /// <returns>
        /// A stack containing the passed collection.
        /// </returns>
        public static Stack<T> ToStack<T>(this IEnumerable<T> collection)
	    {
		    if (collection == null)
		    {
			    throw new ArgumentException($"Trying to create a stack from a null collection!");
		    }
		    
		    return new Stack<T>(collection);
	    }

	    /// <summary>
	    /// Creates a linked list from an enumerable sequence.
	    /// </summary>
	    /// <typeparam name="T">Element type of the collection.</typeparam>
	    /// <param name="collection">Sequence to use for creating the linked list.</param>
	    /// <exception cref="System.ArgumentException">Thrown when collection we are trying to convert to linked list is null.</exception> 
	    /// <returns>
	    /// A linked list containing the passed collection.
	    /// </returns>
	    public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> collection)
	    {
		    if (collection == null)
		    {
			    throw new ArgumentException($"Trying to create a linked list from a null collection!");
		    }
		    
		    return new LinkedList<T>(collection);
	    }

	    #endregion
    }
}