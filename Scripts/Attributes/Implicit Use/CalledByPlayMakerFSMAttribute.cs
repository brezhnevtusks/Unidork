using System;
using JetBrains.Annotations;

namespace Unidork.Attributes
{
	/// <summary>
	/// Attribute to put on methods that are triggered from a PlayMaker FSM.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
	[MeansImplicitUse]
	public class CalledByPlayMakerFSMAttribute : Attribute
	{
		public string FSMName { get; }
		public string FSMNodeName { get; }

		public CalledByPlayMakerFSMAttribute(string fsmName, string fsmNodeName)
		{
			FSMName = fsmName ?? "";
			FSMNodeName = fsmNodeName ?? "";
		}
	}
}