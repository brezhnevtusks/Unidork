using Doozy.Runtime.UIManager.Components;

namespace Unidork.Doozy
{
    public static class DoozyExtensions
    {
        /// <summary>
        /// Disables the game object to which a <see cref="UIButton"/> is attached.
        /// </summary>
        /// <param name="button">Button.</param>
        public static void DisableGo(this UIButton button) => button.gameObject.SetActive(false);

        /// <summary>
        /// Enables the game object to which a <see cref="UIButton"/> is attached.
        /// </summary>
        /// <param name="button">Button.</param>
        public static void EnableGo(this UIButton button) => button.gameObject.SetActive(true);
    }
}