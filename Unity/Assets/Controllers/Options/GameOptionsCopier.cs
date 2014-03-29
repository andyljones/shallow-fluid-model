namespace Assets.Controllers.Options
{
    /// <summary>
    /// Creates a copy of a GameOptions object.
    /// </summary>
    public static class GameOptionsCopier
    {
        //TODO: Can't you just use Object.clone() for this?
        /// <summary>
        /// Returns a copy of the provided options.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static GameOptions Copy(this GameOptions original)
        {
            var properties = original.GetType().GetProperties();

            var copy = new GameOptions();

            foreach (var property in properties)
            {
                property.SetValue(copy, property.GetValue(original, null), null);
            }

            return copy;
        }
    }
}
