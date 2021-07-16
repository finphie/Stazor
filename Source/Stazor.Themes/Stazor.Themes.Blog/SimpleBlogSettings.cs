namespace Stazor.Themes
{
    public sealed record SimpleBlogSettings : ThemeBaseSettings, ISettingsKey
    {
        public static string Key => nameof(Blog);
    }
}