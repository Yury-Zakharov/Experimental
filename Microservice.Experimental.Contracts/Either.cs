using System.Diagnostics.Contracts;

namespace Microservice.Experimental.Contracts;

/// <summary>
/// A structure that can hold either a left or a right value, but not both. Aka, a Result pattern.
/// </summary>
/// <typeparam name="TL">Type parameter for the left value, usually indicating error situation.</typeparam>
/// <typeparam name="TR">Type parameter for the 'right' value. Pun intended.</typeparam>
public readonly struct Either<TL, TR>
{
    private readonly TL? left;
    private readonly TR? right;
    private readonly bool isLeft;

    /// <summary>
    /// Standard constructor for the left value.
    /// </summary>
    /// <param name="left"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public Either(TL left)
    {
        if(left is null)
            throw new ArgumentNullException(nameof(left));

        this.left = left;
        right = default;
        isLeft = true;
    }

    /// <summary>
    /// Standard constructor for the right value.
    /// </summary>
    /// <param name="right"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public Either(TR right)
    {
        if(right is null)
            throw new ArgumentNullException(nameof(right));

        this.right = right;
        left  = default;
        isLeft = false;
    }

    /// <summary>
    /// Match the left or right value with the provided functions.
    /// </summary>
    /// <typeparam name="T">Result type parameter.</typeparam>
    /// <param name="leftFunc">The function to be applied to the left value, when present.</param>
    /// <param name="rightFunc">The function to be applied to the right value, when present.</param>
    /// <returns></returns>
    [Pure]
    public T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc)
        => isLeft ? leftFunc(left!) : rightFunc(right!);

    /// <summary>
    /// Action-based match for the left or right value.
    /// </summary>
    /// <param name="onLeft">The action to be performed on the left value, when present.</param>
    /// <param name="onRight">The action to be performed on the right value, when present.</param>
    public void Match(Action<TL> onLeft, Action<TR> onRight)
    {
        if(isLeft)
            onLeft.Invoke(left!);
        else
            onRight.Invoke(right!);
    }

    /// <summary>
    /// Maps the right value to a new value.
    /// <para>Leaves the left value intact.</para>
    /// </summary>
    /// <typeparam name="T">Result type parameter.</typeparam>
    /// <param name="mapper">Mapping function to be applied to the right value, when present.</param>
    /// <returns></returns>
    [Pure]
    public Either<TL, T> Map<T>(Func<TR, T> mapper) =>
        isLeft
            ? new Either<TL, T>(left!)
            : new Either<TL, T>(mapper(right!));

    /// <summary>
    /// Implicitly converts <see cref="TL"/> to <see cref="Either{TL, TR}"/>.
    /// </summary>
    /// <param name="left"></param>
    public static implicit operator Either<TL, TR>(TL left) => new(left);

    /// <summary>
    /// Implicitly converts <see cref="TR"/> to <see cref="Either{TL, TR}"/>.
    /// </summary>
    /// <param name="right"></param>
    public static implicit operator Either<TL, TR>(TR right) => new(right);
}