namespace AutoVerdikt.Application.AI.Extraction;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ExtractionSchemaAttribute(string systemPrompt) : Attribute
{
    public string SystemPrompt { get; } = systemPrompt;
}
