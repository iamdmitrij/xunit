using System;
using System.Collections.Generic;
using Xunit.Internal;

namespace Xunit.Sdk;

/// <summary>
/// This message indicates that an error has occurred during test case cleanup.
/// </summary>
[JsonTypeID("test-case-cleanup-failure")]
public sealed class TestCaseCleanupFailure : TestCaseMessage, IErrorMetadata
{
	int[]? exceptionParentIndices;
	string?[]? exceptionTypes;
	string[]? messages;
	string?[]? stackTraces;

	/// <inheritdoc/>
	public required int[] ExceptionParentIndices
	{
		get => this.ValidateNullablePropertyValue(exceptionParentIndices, nameof(ExceptionParentIndices));
		set => exceptionParentIndices = Guard.ArgumentNotNullOrEmpty(value, nameof(ExceptionParentIndices));
	}

	/// <inheritdoc/>
	public required string?[] ExceptionTypes
	{
		get => this.ValidateNullablePropertyValue(exceptionTypes, nameof(ExceptionTypes));
		set => exceptionTypes = Guard.ArgumentNotNullOrEmpty(value, nameof(ExceptionTypes));
	}

	/// <inheritdoc/>
	public required string[] Messages
	{
		get => this.ValidateNullablePropertyValue(messages, nameof(Messages));
		set => messages = Guard.ArgumentNotNullOrEmpty(value, nameof(Messages));
	}

	/// <inheritdoc/>
	public required string?[] StackTraces
	{
		get => this.ValidateNullablePropertyValue(stackTraces, nameof(StackTraces));
		set => stackTraces = Guard.ArgumentNotNullOrEmpty(value, nameof(StackTraces));
	}

	/// <inheritdoc/>
	protected override void Deserialize(IReadOnlyDictionary<string, object?> root)
	{
		Guard.ArgumentNotNull(root);

		base.Deserialize(root);

		exceptionParentIndices = JsonDeserializer.TryGetArrayOfInt(root, nameof(ExceptionParentIndices));
		exceptionTypes = JsonDeserializer.TryGetArrayOfNullableString(root, nameof(ExceptionTypes));
		messages = JsonDeserializer.TryGetArrayOfString(root, nameof(Messages));
		stackTraces = JsonDeserializer.TryGetArrayOfNullableString(root, nameof(StackTraces));
	}

	/// <summary>
	/// Creates a new <see cref="TestCaseCleanupFailure"/> constructed from an <see cref="Exception"/> object.
	/// </summary>
	/// <param name="ex">The exception to use</param>
	/// <param name="assemblyUniqueID">The unique ID of the assembly</param>
	/// <param name="testCollectionUniqueID">The unique ID of the test collectioon</param>
	/// <param name="testClassUniqueID">The (optional) unique ID of the test class</param>
	/// <param name="testMethodUniqueID">The (optional) unique ID of the test method</param>
	/// <param name="testCaseUniqueID">The unique ID of the test case</param>
	public static TestCaseCleanupFailure FromException(
		Exception ex,
		string assemblyUniqueID,
		string testCollectionUniqueID,
		string? testClassUniqueID,
		string? testMethodUniqueID,
		string testCaseUniqueID)
	{
		Guard.ArgumentNotNull(ex);
		Guard.ArgumentNotNull(assemblyUniqueID);
		Guard.ArgumentNotNull(testCollectionUniqueID);
		Guard.ArgumentNotNull(testCaseUniqueID);

		var errorMetadata = ExceptionUtility.ExtractMetadata(ex);

		return new TestCaseCleanupFailure
		{
			AssemblyUniqueID = assemblyUniqueID,
			TestCollectionUniqueID = testCollectionUniqueID,
			TestClassUniqueID = testClassUniqueID,
			TestMethodUniqueID = testMethodUniqueID,
			TestCaseUniqueID = testCaseUniqueID,
			ExceptionTypes = errorMetadata.ExceptionTypes,
			Messages = errorMetadata.Messages,
			StackTraces = errorMetadata.StackTraces,
			ExceptionParentIndices = errorMetadata.ExceptionParentIndices,
		};
	}

	/// <inheritdoc/>
	protected override void Serialize(JsonObjectSerializer serializer)
	{
		Guard.ArgumentNotNull(serializer);

		base.Serialize(serializer);

		serializer.SerializeIntArray(nameof(ExceptionParentIndices), ExceptionParentIndices);
		serializer.SerializeStringArray(nameof(ExceptionTypes), ExceptionTypes);
		serializer.SerializeStringArray(nameof(Messages), Messages);
		serializer.SerializeStringArray(nameof(StackTraces), StackTraces);
	}

	/// <inheritdoc/>
	protected override void ValidateObjectState(HashSet<string> invalidProperties)
	{
		base.ValidateObjectState(invalidProperties);

		ValidatePropertyIsNotNull(exceptionParentIndices, nameof(ExceptionParentIndices), invalidProperties);
		ValidatePropertyIsNotNull(exceptionTypes, nameof(ExceptionTypes), invalidProperties);
		ValidatePropertyIsNotNull(messages, nameof(Messages), invalidProperties);
		ValidatePropertyIsNotNull(stackTraces, nameof(StackTraces), invalidProperties);
	}
}
