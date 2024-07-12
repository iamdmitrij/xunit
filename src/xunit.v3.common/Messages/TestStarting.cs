using System.Collections.Generic;
using System.Globalization;
using Xunit.Internal;

namespace Xunit.Sdk;

/// <summary>
/// This message indicates that a test is about to start executing.
/// </summary>
[JsonTypeID("test-starting")]
public sealed class TestStarting : TestMessage, ITestMetadata
{
	bool? @explicit;
	string? testDisplayName;
	int? timeout;
	IReadOnlyDictionary<string, IReadOnlyList<string>>? traits;

	/// <inheritdoc/>
	public required bool Explicit
	{
		get => this.ValidateNullablePropertyValue(@explicit, nameof(Explicit));
		set => @explicit = value;
	}

	/// <inheritdoc/>
	public required string TestDisplayName
	{
		get => this.ValidateNullablePropertyValue(testDisplayName, nameof(TestDisplayName));
		set => testDisplayName = Guard.ArgumentNotNullOrEmpty(value, nameof(TestDisplayName));
	}

	/// <inheritdoc/>
	public required int Timeout
	{
		get => this.ValidateNullablePropertyValue(timeout, nameof(Timeout));
		set => timeout = value;
	}

	/// <inheritdoc/>
	public required IReadOnlyDictionary<string, IReadOnlyList<string>> Traits
	{
		get => this.ValidateNullablePropertyValue(traits, nameof(Traits));
		set => traits = Guard.ArgumentNotNull(value, nameof(Traits));
	}

	string ITestMetadata.UniqueID =>
		TestUniqueID;

	/// <inheritdoc/>
	protected override void Deserialize(IReadOnlyDictionary<string, object?> root)
	{
		Guard.ArgumentNotNull(root);

		base.Deserialize(root);

		@explicit = JsonDeserializer.TryGetBoolean(root, nameof(Explicit));
		testDisplayName = JsonDeserializer.TryGetString(root, nameof(TestDisplayName));
		timeout = JsonDeserializer.TryGetInt(root, nameof(Timeout));
		traits = JsonDeserializer.TryGetTraits(root, nameof(Traits));
	}

	/// <inheritdoc/>
	protected override void Serialize(JsonObjectSerializer serializer)
	{
		Guard.ArgumentNotNull(serializer);

		base.Serialize(serializer);

		serializer.Serialize(nameof(Explicit), Explicit);
		serializer.Serialize(nameof(TestDisplayName), TestDisplayName);
		serializer.Serialize(nameof(Timeout), Timeout);
		serializer.SerializeTraits(nameof(Traits), Traits);
	}

	/// <inheritdoc/>
	public override string ToString() =>
		string.Format(CultureInfo.CurrentCulture, "{0} name={1}", base.ToString(), testDisplayName.Quoted());

	/// <inheritdoc/>
	protected override void ValidateObjectState(HashSet<string> invalidProperties)
	{
		base.ValidateObjectState(invalidProperties);

		ValidatePropertyIsNotNull(@explicit, nameof(Explicit), invalidProperties);
		ValidatePropertyIsNotNull(testDisplayName, nameof(TestDisplayName), invalidProperties);
		ValidatePropertyIsNotNull(timeout, nameof(Timeout), invalidProperties);
		ValidatePropertyIsNotNull(traits, nameof(Traits), invalidProperties);
	}
}
