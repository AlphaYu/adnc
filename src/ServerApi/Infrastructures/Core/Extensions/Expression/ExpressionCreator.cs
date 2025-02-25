namespace System.Linq.Expressions
{
    /// <summary>
    /// Expression creator
    /// </summary>
    public static class ExpressionCreator
    {
        /// <summary>
        /// Generate a new expression
        /// </summary>
        /// <typeparam name="T">The type of the object being tested.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> New<T>(Expression<Func<T, bool>>? expr = null)
            where T : class
            => expr ?? (x => true);

        /// <summary>
        /// Generate a new expression
        /// </summary>
        /// <typeparam name="T1">The type of the first object being tested.</typeparam>
        /// <typeparam name="T2">The type of the second object being tested.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T1, T2, bool>> New<T1, T2>(Expression<Func<T1, T2, bool>>? expr = null)
            where T1 : class
            where T2 : class
            => expr ?? ((x, y) => true);
    }
}