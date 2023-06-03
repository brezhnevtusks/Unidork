namespace Unidork.Utility
{
	/// <summary>
	/// Defines the a <see cref="GameObjectGetter"/> acquires an object. 
	/// </summary>
	public enum GameObjectReferenceType
	{
		/// <summary>
		/// Object is directly assigned in the inspector.
		/// </summary>
		DirectReference,
		
		/// <summary>
		/// Object is located with GameObject.FindWithTag
		/// </summary>
		Tag,
		
		/// <summary>
		/// Object is located with GameObject.Find
		/// </summary>
		Name
	}
}