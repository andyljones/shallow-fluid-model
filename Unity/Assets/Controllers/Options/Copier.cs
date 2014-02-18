namespace Assets.Controllers.Options
{
    public static class Copier
    {
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
