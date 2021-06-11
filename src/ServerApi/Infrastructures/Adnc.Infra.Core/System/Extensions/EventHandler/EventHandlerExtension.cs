using JetBrains.Annotations;

namespace System
{
    public static class EventHandlerExtension
    {
        /// <summary>
        ///     An EventHandler extension method that raises the event event.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="sender">Source of the event.</param>
        public static void RaiseEvent([CanBeNull] this EventHandler @this, object sender)
        {
            @this?.Invoke(sender, null);
        }

        /// <summary>
        ///     An EventHandler extension method that raises.
        /// </summary>
        /// <param name="handler">The handler to act on.</param>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event information.</param>
        public static void RaiseEvent([CanBeNull] this EventHandler handler, object sender, EventArgs e)
        {
            handler?.Invoke(sender, e);
        }

        /// <summary>
        ///     An EventHandler&lt;TEventArgs&gt; extension method that raises the event event.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of the event arguments.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="sender">Source of the event.</param>
        public static void RaiseEvent<TEventArgs>([CanBeNull] this EventHandler<TEventArgs> @this, object sender) where TEventArgs : EventArgs
        {
            @this?.Invoke(sender, Activator.CreateInstance<TEventArgs>());
        }

        /// <summary>
        ///     An EventHandler&lt;TEventArgs&gt; extension method that raises the event event.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of the event arguments.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event information to send to registered event handlers.</param>
        public static void RaiseEvent<TEventArgs>([CanBeNull] this EventHandler<TEventArgs> @this, object sender, TEventArgs e) where TEventArgs : EventArgs
        {
            @this?.Invoke(sender, e);
        }
    }
}