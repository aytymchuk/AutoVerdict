# Extraction Model Design Rules

Any C# class used as `T` with `IExtractionService.ExtractAsync<T>()` must follow these rules.

## Required attributes

- **`[ExtractionSchema("...")]`** on the class — carries the system prompt that scopes what the model should extract.
- **`[Description("...")]`** on every property — flows into the JSON Schema sent to the model. Without it the model receives no field-level guidance.

Optional: **`[JsonPropertyName("camelCaseName")]`** when the JSON property name must differ from the C# name.

## Type constraints

JSON Schema primitive compatibility only:

| Allowed | Not allowed — use `string?` instead |
|---------|-------------------------------------|
| `string?`, `string[]?` | `DateTime`, `DateOnly`, `DateTimeOffset` |
| `int?`, `long?` | `Uri`, `Guid` |
| `decimal?`, `double?` | `TimeSpan` |
| `bool?` | Any custom struct |
| Nested sealed classes with same rules | Any open generic type |
| `enum` (serializes as string with `[JsonConverter]`) | |

## Class shape

- **Sealed class** with a parameterless constructor (implicit when no positional constructor is declared).
- **All properties must be nullable** — missing data = `null`; not a failed extraction.
- **Use `{ get; init; }` properties** — no positional constructor.

## System prompt guidelines

The `[ExtractionSchema]` system prompt should:

1. State what domain the model is extracting from.
2. State the language(s) the input may appear in.
3. Instruct the model to return `null` for missing fields — not to guess or hallucinate.
4. State any normalisation rules (unit conversions, date formats, enum values).

## Usage pattern

```csharp
var result = await extractionService.ExtractAsync<MyFacts>(rawInput, ct);

if (!result.IsSuccess)
{
  // handle gracefully — do not throw
  return;
}

var facts = result.Value; // all fields nullable — check before using
// Do NOT log or persist result.RawJson in production
```
