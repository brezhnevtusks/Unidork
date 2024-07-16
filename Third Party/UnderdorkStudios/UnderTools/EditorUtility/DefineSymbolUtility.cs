using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace UnderdorkStudios.UnderTools.EditorUtility
{
	/// <summary>
	/// Editor utility methods for define symbols.
	/// </summary>
    public static class DefineSymbolUtility
    {
	    #region Constants

	    private const string ModuleManagerFullTypeName = "UnityEditor.Modules.ModuleManager, UnityEditor.dll";
	    private const string GetTargetStringFromBuildTargetGroupMethodName = "GetTargetStringFromBuildTargetGroup";
	    private const string GetPlatformNameMethodName = "GetPlatformName";

	    #endregion
	    
	    /// <summary>
	    /// Adds a define symbol to all build targets in the project(globally).
	    /// </summary>
	    /// <param name="defineSymbolToAdd"></param>
        public static void AddDefineSymbol(string defineSymbolToAdd)
        {
            var defineAdded = false;
            var timesAdded = 0;

#if UNITY_2023_1_OR_NEWER
	        foreach (BuildTargetGroup buildTargetGroup in (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup)))
	        {
		        if (!IsBuildTargetGroupValid(buildTargetGroup))
		        {
			        continue;
		        }
		        
		        NamedBuildTarget namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
		        string existingDefineSymbols = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);

		        if (DefineSymbolExists(existingDefineSymbols, defineSymbolToAdd))
		        {
			        continue;
		        }
                    
		        string stringToAppend = existingDefineSymbols.Length > 0 ? ";" + defineSymbolToAdd : defineSymbolToAdd;
		        string updatedDefineSymbols = $"{existingDefineSymbols}{stringToAppend}";
                    
		        PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, updatedDefineSymbols);
                    
		        defineAdded = true;
		        timesAdded++;
	        }
#else
			foreach (BuildTargetGroup buildTargetGroup in (BuildTargetGroup[]) Enum.GetValues(typeof(BuildTargetGroup)))
			{
				if (!IsBuildTargetGroupValid(buildTargetGroup))
				{
					continue;
				}
				
				string existingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

				if (DefineSymbolExists(existingDefineSymbols, defineSymbolToAdd))
				{
					continue;
				}
				
				string stringToAppend = existingDefineSymbols.Length > 0 ? ";" + defineSymbolToAdd : defineSymbolToAdd;
				string updatedDefineSymbols = $"{existingDefineSymbols}{stringToAppend}";
				
				PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, updatedDefineSymbols);
				
				defineAdded = true;
				timesAdded++;
			}
#endif
	        if (defineAdded)
	        {
		        Debug.Log($"UnderTools: Added define symbol {defineSymbolToAdd} to {timesAdded} BuildTargetGroups!");
	        }
        }

        /// <summary>
        /// Checks if the passed BuildTargetGroup is valid.
        /// </summary>
        /// <param name="buildTargetGroup">Build target group.</param>
        /// <returns>
        /// True if <paramref name="buildTargetGroup"/> is valid, False otherwise.
        /// </returns>
        private static bool IsBuildTargetGroupValid(BuildTargetGroup buildTargetGroup)
        {
	        if (buildTargetGroup is BuildTargetGroup.Unknown)
	        {
		        return false;
	        }

	        Type moduleManagerType = Type.GetType(ModuleManagerFullTypeName);

	        if (moduleManagerType == null)
	        {
		        return true;
	        }
	        
	        MethodInfo getTargetStringFromBuildTargetGroupMethodInfo = moduleManagerType.GetMethod(
	         GetTargetStringFromBuildTargetGroupMethodName, BindingFlags.Static | BindingFlags.NonPublic);
	        
	        MethodInfo getPlatformNameMethodInfo = typeof(PlayerSettings).GetMethod(
	         GetPlatformNameMethodName, BindingFlags.Static | BindingFlags.NonPublic);
	        
	        if (getTargetStringFromBuildTargetGroupMethodInfo == null || getPlatformNameMethodInfo == null)
	        {
		        return true;
	        }
	        
	        object[] parameters = { buildTargetGroup };
	        
	        string targetStringFromBuildTargetGroup = (string)getTargetStringFromBuildTargetGroupMethodInfo.Invoke(null, parameters);
	        string platformName = (string)getPlatformNameMethodInfo.Invoke(null, new object[] { buildTargetGroup });
	        
	        if (string.IsNullOrEmpty(targetStringFromBuildTargetGroup))
	        {
		        return !string.IsNullOrEmpty(platformName);
	        }
	        
	        return true;
        }
        
        /// <summary>
        /// Checks if a define symbol exists in a string of symbols.
        /// </summary>
        /// <param name="allDefineSymbols">String representing define symbols separated by semicolons.</param>
        /// <param name="defineSymbol">Define symbol to check.</param>
        /// <returns>
        /// True if <see cref="allDefineSymbols"/> contains <paramref name="defineSymbol"/>, False otherwise.
        /// </returns>
        private static bool DefineSymbolExists(string allDefineSymbols, string defineSymbol)
        {
	        return allDefineSymbols.Contains(defineSymbol);
        } 
    }
}