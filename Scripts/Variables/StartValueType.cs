namespace Unidork.Variables
{
    /// <summary>
    /// Start value type for a scriptable object that holds an integral or floating point value.
    /// <para>Default - start value will be set to the default value of the type that the variable represents.</para>
    /// <para>Min - start value will be set to the minimum value of the variable.</para>
    /// <para>Max - start value will be set to the maximum value of the variable.</para>
    /// <para>Custom - start value will be set to a custom value assigned in the inspector.</para>
    /// </summary>    
    public enum VariableStartValueType
    {
        Default,
        Min,
        Max,
        Custom
    }
}
