using System;
using System.Text;
using UnityEngine;

namespace UnderdorkStudios.UnderTags
{
    [Serializable]
    public struct UnderTag : IEquatable<UnderTag>, IComparable<UnderTag>
    {
        #region Properties
        
        /// <summary>
        /// 
        /// </summary>
        public static StringBuilder TagStringBuilder { get; } = new();
        
        /// <summary>
        /// String representing the tag in TagLayer1.TagLayer2.TagLayer3... format.
        /// </summary>
        /// <value>
        /// Gets and sets the value of the string field value.
        /// </value>
        public string Value { get => value; set => this.value = value; }

        #endregion

        #region Fields
        
        /// <summary>
        /// String representing the tag in TagLayer1.TagLayer2.TagLayer3... format.
        /// </summary>
        [SerializeField]
        private string value;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="value">Tag value.</param>
        public UnderTag(string value)
        {
            this.value = value;
        }

        #endregion

        #region Instance

        /// <summary>
        /// Checks whether this tag is valid. Valid tags can't have an empty string as their values.
        /// As for validating the string itself, when tags are created via the database, they are always valid
        /// and when we create them via code, we also validate their format, so we don't need to perform
        /// any additional checks here.
        /// </summary>
        /// <returns>
        /// True if the field value is not null or empty, False otherwise.
        /// </returns>
        public bool IsValid() => !string.IsNullOrEmpty(value);

        /// <summary>
        /// Gets the individual name of this tag, removing all parent names.
        /// </summary>
        /// <returns>
        /// A string representing the bottom-most tag in this tag's hierarchy.
        /// </returns>
        public string GetIndividualName()
        {
            int lastDotIndex = Value.LastIndexOf('.');
            return lastDotIndex == -1 ? Value : Value.Substring(lastDotIndex + 1, Value.Length - 1 - lastDotIndex);
        }

        /// <summary>
        /// Gets the depth of the tag by counting the number of dots in its <see cref="UnderTag.Value"/>.
        /// </summary>
        /// <returns></returns>
        public int GetDepth()
        {
            return Value.Split('.').Length - 1;
        }
        
        /// <summary>
        /// Gets all tag parts in this tag's hierarchy starting from root. For example, if the tag is A.B.C, this will get A, B, and C.
        /// </summary>
        /// <returns>
        /// An array of <see cref="UnderTag"/>s that cover this tag's entire hierarchy.
        /// </returns>
        public UnderTag[] GetAllTagPartsInHierarchy()
        {
            string[] splitTag = value.Split('.');
            int tagCount = splitTag.Length;
            
            if (tagCount == 1)
            {
                return new [] { this };
            }

            var allTags = new UnderTag[splitTag.Length];
            
            for (var i = 0; i < tagCount; i++)
            {
                allTags[i] = new UnderTag(splitTag[i]);
            }

            return allTags;
        }

        public static implicit operator UnderTag(string tag) => new(tag);
        public static implicit operator string(UnderTag tag) => tag.ToString();
        
        public override string ToString()
        {
            return Value;
        }

        #endregion

        #region Match

        /// <summary>
        /// Checks if this tag matches another tag exactly.
        /// </summary>
        /// <param name="otherTag">Other tag.</param>
        /// <returns>
        /// True if other tag is valid and tags match exactly, False otherwise.
        /// </returns>
        public bool MatchesExactly(UnderTag otherTag)
        {
            return otherTag.IsValid() && string.Equals(value,otherTag.value);
        }

        #endregion

        #region Equality

        public static bool operator ==(UnderTag firstTag, UnderTag secondTag)
        {
            if (!firstTag.IsValid() || !secondTag.IsValid())
            {
                return false;
            }

            return string.Equals(firstTag.value, secondTag.value);
        }

        public static bool operator !=(UnderTag firstTag, UnderTag secondTag) => !(firstTag == secondTag);
        
        public bool Equals(UnderTag other)
        {
            return string.Equals(value, other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj.GetType() == GetType() && Equals((UnderTag)obj);
        }

        public override int GetHashCode()
        {
            return value != null ? value.GetHashCode() : 0;
        }

        #endregion

        #region Comparison
        
        public int CompareTo(UnderTag other)
        {
            return string.Compare(value, other.value, StringComparison.Ordinal);
        }

        #endregion
    }
}