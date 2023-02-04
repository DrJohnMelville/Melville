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
    CopySourceVisibility = 0,

    // DO NOT CHANGE ORDER OF THESE ENUM VALUES
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

    Protected = 3,

    Internal = 4,
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

    Public = 6
}