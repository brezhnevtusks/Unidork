using System;
using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Serialization;

namespace UnderdorkStudios.UnderTags
{
    /// <summary>
    /// Serializable dictionary that will be used to store tag-parent tag relations.
    /// </summary>
    [Serializable]
    public class UnderTagParentDictionary : SerializableDictionary<UnderTag, UnderTag> { }
    
    /// <summary>
    /// Serializable dictionary that will be used to store tag-child tag relations.
    /// </summary>
    [Serializable]
    public class UnderTagChildDictionary : SerializableDictionary<UnderTag, UnderTagChildList> { }

    /// <summary>
    /// Required so we can serialize a list of tags in our <see cref="UnderTagChildDictionary"/>. Unity can't
    /// by default serialize a list of lists, so we use this wrapper as a workaround.
    /// </summary>
    [Serializable]
    public class UnderTagChildList
    {
        public List<UnderTag> List;
        
        public UnderTag this[int key]
        {
            get => List[key];
            set => List[key] = value;
        }
    }

    /// <summary>
    /// Serializable dictionary used to store indices of expressions and their sub expressions in an <see cref="UnderTagQuery"/>.
    /// We store them as integers to avoid object composition issues during serialization caused by <see cref="UnderTagQueryExpression"/>
    /// storing a reference to objects of the same type.
    /// </summary>
    [Serializable]
    public class UnderTagQueryExpressionDictionary : SerializableDictionary<int, List<int>> { }
}