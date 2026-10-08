namespace SUIF.API
{
    public enum UILayer
    {
        Default,
        Background,
        Screens,
        Windows,
        Popups,
        Topmost
    }

    public static class UILayerExtensions
    {
        public static string ToContainerName(this UILayer layer)
        {
            return layer switch
            {
                UILayer.Default => "Default",
                UILayer.Background => "Layer-Background",
                UILayer.Screens => "Layer-Screens",
                UILayer.Windows => "Layer-Windows",
                UILayer.Popups => "Layer-Popups",
                UILayer.Topmost => "Layer-Topmost",
                _ => "Default"
            };
        }
    }
}
