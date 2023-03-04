namespace Melville.INPC;

/// <summary>
/// Enumeration for common accessibility combinations.
/// the values are specifically chosen so this can be casted directly to the
/// Microsoft.CodeAnalysis.Accessability class. 
/// </summary>
public enum Visibility
{
    /// <summary>
    /// No accessibility specified.
    /// </summary>
    NotApplicable = 0,
    /// <summary>
    /// The DelegateTo method will use the source symbol's visibility
    /// </summary>
    CopySourceVisibility = 0,

    // DO NOT CHANGE ORDER OF THESE ENUM VALUES
    /// <summary>
    /// Same a private in C#
    /// </summary>
    Private = 1,

    /// <summary>
    /// Only accessible where both protected and internal members are accessible
    /// (more restrictive than <see cref="Protected"/>, <see cref="Internal"/> and <see cref="ProtectedOrInternal"/>).
    /// </summary>
    ProtectedAndInternal = 2,

    /// <summary>
    /// Only accessible where both protected and friend members are accessible
    /// (more restrictive than <see cref="Protected"/>, <see cref="Friend"/> and <see cref="ProtectedOrFriend"/>).
    /// </summary>
    ProtectedAndFriend = ProtectedAndInternal,

    /// <summary>
    /// Same as protected in C#
    /// </summary>
    Protected = 3,

    /// <summary>
    /// Same as intenral in C#
    /// </summary>
    Internal = 4,

    /// <summary>
    /// Same as intenral in C#
    /// </summary>
    Friend = Internal,

    /// <summary>
    /// Accessible wherever either protected or internal members are accessible
    /// (less restrictive than <see cref="Protected"/>, <see cref="Internal"/> and <see cref="ProtectedAndInternal"/>).
    /// </summary>
    ProtectedOrInternal = 5,

    /// <summary>
    /// Accessible wherever either protected or friend members are accessible
    /// (less restrictive than <see cref="Protected"/>, <see cref="Friend"/> and <see cref="ProtectedAndFriend"/>).
    /// </summary>
    ProtectedOrFriend = ProtectedOrInternal,

    /// <summary>
    /// Same as public in c#
    /// </summary>
    Public = 6
}