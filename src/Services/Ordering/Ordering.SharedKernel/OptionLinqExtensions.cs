namespace Ordering.SharedKernel;

public static class OptionLinqExtensions
{
    public static Option<TResult> Select<T, TResult>(
        this Option<T> option, Func<T, TResult> selector) => option.Map(selector);

    public static Option<TResult> SelectMany<T, TResult>(
        this Option<T> option, Func<T, Option<TResult>> binder) => option.Bind(binder);

    public static Option<TSelect> SelectMany<T, TIntermediate, TSelect>(
        this Option<T> option,
        Func<T, Option<TIntermediate>> binder,
        Func<T, TIntermediate, TSelect> projector)
    {
        return option.Bind(t =>
            binder(t).Map(u => projector(t, u)));
    }

    //private static void Test()
    //{
    //    var user = new UserDemo
    //    {
    //        Profile = new Profile
    //        {
    //            Address = new Address { City = "Cairo" }
    //        }
    //    };

    //    Option<UserDemo> maybeUser = Option<UserDemo>.Some(user);

    //    var cityOption =
    //        from u in maybeUser
    //        from p in Option<Profile>.Some(u.Profile!)
    //        from a in Option<Address>.Some(p.Address!)
    //        select a.City;

    //    string result = cityOption.Match(
    //        some => some,
    //        () => "Unknown");

    //    Console.WriteLine(result); // Output: Cairo

    //}
}
