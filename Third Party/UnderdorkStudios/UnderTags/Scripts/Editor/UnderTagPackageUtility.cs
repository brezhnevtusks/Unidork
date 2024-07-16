using UnderdorkStudios.UnderTools.EditorUtility;
using UnityEditor;

namespace UnderdorkStudios.UnderTags.Editor
{
    /// <summary>
    /// Manages adding the "UNDERTAGS" define symbol when the package is added to the project.
    /// </summary>
    public static class UnderTagPackageUtility
    {
        #region Constants

        private const string UnderTagDefineSymbol = "UNDERTAGS";

        #endregion
        
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling ||
                EditorApplication.isUpdating)
            {
                EditorApplication.update += AddDefineWhenPossible;
                return;
            }
            AddDefineSymbol();
        }

        /// <summary>
        /// Calls <see cref="AddDefineSymbol"/> when the editor is not playing, compiling or updating.
        /// </summary>
        private static void AddDefineWhenPossible()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling ||
                EditorApplication.isUpdating)
            {
                return;
            }

            EditorApplication.update -= AddDefineWhenPossible;
            AddDefineSymbol();
        }

        /// <summary>
        /// Tells the <see cref="DefineSymbolUtility"/> to add <see cref="UnderTagDefineSymbol"/> to all project build targets.
        /// </summary>
        private static void AddDefineSymbol()
        {
            DefineSymbolUtility.AddDefineSymbol(UnderTagDefineSymbol);
        }
    }
}