using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnderdorkStudios.UnderTools.Extensions;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Events;
#endif
using UnityEngine;

namespace UnderdorkStudios.UnderTags
{
    [System.Serializable]
    public class UnderTagDatabase : ScriptableObject
    {
        #region Constants

        private const string AssetsFolderName = "Assets";
        private const string ResourcesFolderName = "Resources";
        private const string DatabaseAssetPath = "Assets/Resources/UnderTagDatabase.asset";
        private const string UnderTagDatabaseName = "UnderTagDatabase";

        #endregion
        
        #region Properties

        /// <summary>
        /// Root tags stored in the database.
        /// </summary>
        /// <value>
        /// A new list storing values in <see cref="rootTags"/>.
        /// </value>
        public List<UnderTag> RootTags => rootTags;

        /// <summary>
        /// Does the database have at least one <see cref="UnderTag"/>?
        /// </summary>
        public bool IsEmpty => rootTags.Count == 0;

#if UNITY_EDITOR
        public UnityEvent<UnderTag> OnTagAdded;
        public UnityEvent<int> OnRootTagRemoved;
        public UnityEvent<UnderTag> OnTagRemoved;
#endif
        
        #endregion
        
        #region Fields
        
        /// <summary>
        /// Root tags stored in the database.
        /// </summary>
        [SerializeField] private List<UnderTag> rootTags = new();

        /// <summary>
        /// Dictionary mapping tags to their parent tags. Used to avoid serialization depth limit issues that will
        /// occur if we decide to serialize direct references inside the tag struct itself.
        /// </summary>
        [SerializeField] private UnderTagParentDictionary tagParentDictionary = new();

        /// <summary>
        /// Dictionary mapping tags to their child tags. Used to avoid serialization depth limit issues that will
        /// occur if we decide to serialize direct references inside the tag struct itself.
        /// </summary>
        [SerializeField] private UnderTagChildDictionary tagChildDictionary = new();

        #endregion

        #region Database
        
        /// <summary>
        /// Gets the reference to the <see cref="UnderTagDatabase"/> scriptable object.
        /// Creates and saves it to disk if one doesn't exist.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="UnderTagDatabase"/>.
        /// </returns>
        public static UnderTagDatabase GetOrCreateUnderTagDatabase()
        {
#if !UNITY_EDITOR
            return Resources.Load<UnderTagDatabase>(UnderTagDatabaseName);
#else
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return Resources.Load<UnderTagDatabase>(UnderTagDatabaseName);
            }
            
            UnderTagDatabase database = null;
            
            // Try to find at default path in resources.
#if UNITY_2023_1_OR_NEWER
            if (AssetDatabase.AssetPathExists(DatabaseAssetPath))
            {
                database = AssetDatabase.LoadAssetAtPath<UnderTagDatabase>(DatabaseAssetPath);
            }
#else
            if (File.Exists(DatabaseAssetPath))
            {
                database = AssetDatabase.LoadAssetAtPath<UnderTagDatabase>(DatabaseAssetPath);
            }
#endif
            // If the asset is not present in the project, create it
            if (database == null)
            {
                if (!AssetDatabase.IsValidFolder($"{AssetsFolderName}/{ResourcesFolderName}"))
                {
                    AssetDatabase.CreateFolder(AssetsFolderName, ResourcesFolderName);
                }
                
                database = CreateInstance<UnderTagDatabase>();
                AssetDatabase.CreateAsset(database, DatabaseAssetPath);
                AssetDatabase.SaveAssets();
            }

            return database;
#endif
        }
        
#endregion
        
        #region Tags

        /// <summary>
        /// Checks if the tag has a valid value and is contained in the database.
        /// </summary>
        /// <param name="tag">Tag to check.</param>
        /// <returns>
        /// True if <paramref name="tag"/> has a valid <see cref="UnderTag.Value"/> and is contained in this database,
        /// False otherwise.
        /// </returns>
        public bool TagIsValid(UnderTag tag)
        {
            if (!tag.IsValid())
            {
                return false;
            }

            List<UnderTag> parentTags = GetParentTags(tag);
            return rootTags.Contains(parentTags.Count == 0 ? tag : parentTags[0]);
        }
        
        /// <summary>
        /// Gets all tags in the hierarchy of <paramref name="tag"/>. Tags go from root to last descendent.
        /// Can optionally exclude the tag itself.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="excludeTag">Should <paramref name="tag"/> be excluded?</param>
        /// <returns>
        /// A list of <see cref="UnderTag"/>.
        /// </returns>
        public List<UnderTag> GetAllTagsInHierarchy(UnderTag tag, bool excludeTag = false)
        {
            List<UnderTag> allTags = GetParentTags(tag);
            if (!excludeTag)
            {
                allTags.Add(tag);
            }
            GetDescendentTags(tag, allTags);
            return allTags;
        }

        /// <summary>
        /// Gets all parent tags in the passed tag's hierarchy.
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <returns>
        /// A list of <see cref="UnderTag"/>s.
        /// </returns>
        public List<UnderTag> GetParentTags(UnderTag tag)
        {
            List<UnderTag> parents = new();

            while (tagParentDictionary.TryGetValue(tag, out UnderTag parentTag))
            {
                parents.Insert(0, parentTag);
                tag = parentTag;
            }

            return parents;
        }
        
        /// <summary>
        /// Recursively gets all descendant tags: children, children of children, etc.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="childTags">List of child tags to fill.</param>
        /// <returns>
        /// A list of <see cref="UnderTag"/> containing all child tags, if any.
        /// </returns>
        public void GetDescendentTags(UnderTag tag, List<UnderTag> childTags)
        {
            if (TryGetChildTags(tag, out List<UnderTag> children))
            {
                foreach (UnderTag child in children)
                {
                    childTags.Add(child);
                    GetDescendentTags(child, childTags);
                }
            }
        }
#if UNITY_EDITOR
        /// <summary>
        /// Gets every bottom-most <see cref="UnderTag"/> in every tag hierarchy.
        /// </summary>
        /// <returns>
        /// A list of <see cref="UnderTag"/> that are at the bottom of their hierarchies (don't have children).
        /// </returns>
        public List<UnderTag> GetBottomTags()
        {
            if (rootTags.IsNullOrEmpty())
            {
                return new List<UnderTag>();
            }
            
            List<UnderTag> bottomTags = new();

            foreach (UnderTag rootTag in rootTags)
            {
                TryAddBottomTag(rootTag, bottomTags);
            }

            return bottomTags;
        }

        /// <summary>
        /// Adds a tag to the list of bottom-most tags if it had no children, otherwise calls the method recursively on its children.
        /// </summary>
        /// <param name="parentTag">Parent tag.</param>
        /// <param name="bottomTags">List of bottom tags to fill.</param>
        private void TryAddBottomTag(UnderTag parentTag, List<UnderTag> bottomTags)
        {
            if (tagChildDictionary.TryGetValue(parentTag, out UnderTagChildList childList))
            {
                foreach (UnderTag childTag in childList.List)
                {
                    TryAddBottomTag(childTag, bottomTags);
                }
            }
            else
            {
                bottomTags.Add(parentTag);
            }
        }
#endif
        /// <summary>
        /// Tries to create a tag from the passed string. Checks that the string is in correct format: only alphanumeric
        /// symbols and dots are used, there aren't multiple dots in a row and the tag doesn't begin or end with a dot. Also checks
        /// if the tag for the specified string has already been added.
        /// </summary>
        /// <param name="tagString">Tag string.</param>
        /// <returns>
        /// 
        /// </returns>
        public UnderTagDatabaseResult TryRegisterTag(string tagString)
        {
            if (tagString.StartsWith('.'))
            {
                return UnderTagDatabaseResult.TagStartsWithDot;
            }

            if (tagString.EndsWith('.'))
            {
                return UnderTagDatabaseResult.TagEndsWithDot;
            }
            
            var multipleDotRegex = new Regex("[.]{2}");

            if (multipleDotRegex.IsMatch(tagString))
            {
                return UnderTagDatabaseResult.TwoOrMoreDotsInARow;
            }   
            
            var allowedCharacterRegex = new Regex("^[a-zA-Z0-9.]+");

            Match match = allowedCharacterRegex.Match(tagString);
            
            if (!match.Success || match.Length != tagString.Length)
            {
                return UnderTagDatabaseResult.InvalidCharacters;
            }

            var newTag = new UnderTag(tagString);

            if (IsRootTag(newTag))
            {
                return UnderTagDatabaseResult.TagAlreadyExists;
            }
            
            AddTag(newTag);
            //AssetDatabase.SaveAssetIfDirty(this);
            return UnderTagDatabaseResult.Success;
        }

        /// <summary>
        /// Checks if the passed tag has any children.
        /// </summary>
        /// <param name="tag">Tag to check.</param>
        /// <returns>
        /// True if the tag is contained in <see cref="tagChildDictionary"/>, False otherwise.
        /// </returns>
        public bool HasChildTags(UnderTag tag)
        {
            return tagChildDictionary.ContainsKey(tag);
        }

        /// <summary>
        /// Checks if a node has a child that matches the passed node.
        /// </summary>
        /// <param name="tag">Node to check.</param>
        /// <param name="childTag">Child tag to match.</param>
        /// <returns>
        /// True if <see cref="tag"/> has a node that matches <see cref="childTag"/>, False otherwise.
        /// </returns>
        public bool HasChildTag(UnderTag tag, UnderTag childTag)
        {
            return TryGetChildTags(tag, out List<UnderTag> childTags) && childTags.Contains(childTag);
        }
        
        /// <summary>
        /// Tries to get children of a tag.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="childTags">Resulting list of children.</param>
        /// <returns>
        /// True if the passed tag has any children, False otherwise.
        /// </returns>
        public bool TryGetChildTags(UnderTag tag, out List<UnderTag> childTags)
        {
            childTags = null;

            if (!tagChildDictionary.TryGetValue(tag, out UnderTagChildList childList))
            {
                return false;
            }

            childTags = childList.List;
            return true;
        }

        /// <summary>
        /// Removes an <see cref="UnderTag"/> from the database.
        /// </summary>
        /// <param name="tag">Tag to remove.</param>
        public void RemoveTag(UnderTag tag)
        {
            if (!tag.IsValid())
            {
                return;
            }

#if UNITY_EDITOR
            Undo.RecordObject(this, "Remove UnderTag from Database");
#endif
            if (IsRootTag(tag))
            {
#if UNITY_EDITOR
                int rootTagIndex = rootTags.IndexOf(tag);

                if (rootTags.Remove(tag))
                {
                    OnRootTagRemoved.Invoke(rootTagIndex);
                }
#endif
            }
            else
            {
                UnderTag parentTag = tagParentDictionary[tag];
                List<UnderTag> parentChildTags = tagChildDictionary[parentTag].List;
                parentChildTags.Remove(tag);

                if (parentChildTags.Count == 0)
                {
                    tagChildDictionary.Remove(parentTag);
                }
            }
            
            List<UnderTag> descendantTags = new();
            GetDescendentTags(tag, descendantTags);
            
            foreach (UnderTag descendantTag in descendantTags)
            {
                _ = tagParentDictionary.Remove(descendantTag);
                _ = tagChildDictionary.Remove(descendantTag);
            }
            
            _ = tagParentDictionary.Remove(tag);
            _ = tagChildDictionary.Remove(tag);
            
#if UNITY_EDITOR
            OnTagRemoved.Invoke(tag);
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Checks if the passed <see cref="UnderTag"/> is a root tag. Root tags are the tags at the top of each
        /// branch of the tag hierarchy.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <returns>
        /// True if <paramref name="tag"/> is valid and is a root tag, False otherwise.
        /// </returns>
        public bool IsRootTag(UnderTag tag)
        {
            return tag.IsValid() && rootTags.Contains(tag);
        }

        /// <summary>
        /// Returns the index of the passed <see cref="UnderTag"/> in root tag list.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>
        /// Index of <paramref name="tag"/> in <see cref="rootTags"/>.
        /// </returns>
        public int GetRootTagIndex(UnderTag tag)
        {
            return rootTags.IndexOf(tag);
        }
        
        /// <summary>
        /// Updates the dictionary storing tag-node data when a new tag is added to the database. Will create a node
        /// for each level of the tag hierarchy missing from the database.
        /// </summary>
        /// <param name="newTag">Tag added to the database.</param>
        private void AddTag(UnderTag newTag)
        {
            UnderTag[] allTagPartsInHierarchy = newTag.GetAllTagPartsInHierarchy();
            
            UnderTag currentRootTag = allTagPartsInHierarchy[0];
            int tagCount = allTagPartsInHierarchy.Length;
#if UNITY_EDITOR
            Undo.RecordObject(this, "Add UnderTag to Database");            
#endif
            if (!IsRootTag(currentRootTag))
            {
                rootTags.AddAndSort(currentRootTag);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            }

            if (tagCount <= 1)
            {
#if UNITY_EDITOR
                OnTagAdded.Invoke(currentRootTag);
#endif
                return;
            }

            UnderTag.TagStringBuilder.Clear();
            UnderTag.TagStringBuilder.Append(currentRootTag);

            for (var i = 1; i < tagCount; i++)
            {
                UnderTag.TagStringBuilder.Append('.');
                UnderTag.TagStringBuilder.Append(allTagPartsInHierarchy[i]);
                var childTag = new UnderTag(UnderTag.TagStringBuilder.ToString());

                if (!HasChildTag(currentRootTag, childTag))
                {
                    tagParentDictionary.Add(childTag, currentRootTag);

                    if (TryGetChildTags(currentRootTag, out List<UnderTag> childTags))
                    {
                        childTags.AddAndSort(childTag);
                    }
                    else
                    {
                        var newChildList = new UnderTagChildList
                        {
                            List = new List<UnderTag>
                            {
                                childTag
                            }
                        };
                        
                        tagChildDictionary.Add(currentRootTag, newChildList);
                    }
#if UNITY_EDITOR
                    EditorUtility.SetDirty(this);
#endif
                }
                currentRootTag = childTag;
            }
            
#if UNITY_EDITOR
            OnTagAdded.Invoke(newTag);
#endif
        }

        #endregion
        
#if UNITY_EDITOR
        [MenuItem("Tools/Underdork Studios/UnderTags/Generate UnderTagConstants")]
        public static void GenerateConstantsMenuItem()
        {
            GetOrCreateUnderTagDatabase().GenerateConstantsFile();
        }
        
        public void GenerateConstantsFile()
        {
            if (File.Exists("Assets/UnderdorkStudios/UnderTags/Scripts/UnderTagConstants.cs"))
            {
                AssetDatabase.DeleteAsset("Assets/UnderdorkStudios/UnderTags/Scripts/UnderTagConstants.cs");
            }

            var fileContent = new StringBuilder();

            fileContent.Append("namespace UnderdorkStudios.UnderTags\n");
            fileContent.Append("{\n");
            fileContent.Append("\tpublic static class UnderTagConstants\n");
            fileContent.Append("\t{\n");

            foreach (UnderTag rootTag in rootTags)
            {
                fileContent.AppendFormat("\t\tpublic const string {0} = \"{1}\";\n", rootTag.Value, rootTag.Value);

                List<UnderTag> descendentTags = new();
                GetDescendentTags(rootTag, descendentTags);

                foreach (UnderTag descendentTag in descendentTags)
                {
                    string tagName = descendentTag.Value;
                    tagName = tagName.Replace('.', '_');
                    fileContent.AppendFormat("\t\tpublic const string {0} = \"{1}\";\n", tagName, descendentTag.Value);
                }
            }
            
            fileContent.Append("\t}\n");
            fileContent.Append("}");

            string outfile = "Assets/UnderdorkStudios/UnderTags/Scripts/UnderTagConstants.cs";
            using(StreamWriter sw = new StreamWriter(outfile, false))
            {
                sw.Write(fileContent.ToString());
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}