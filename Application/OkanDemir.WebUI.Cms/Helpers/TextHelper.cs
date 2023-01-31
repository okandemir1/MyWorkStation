namespace OkanDemir.WebUI.Cms.Helpers
{
    public static class TextHelper
    {
        public static string FullnameSplinter(string name)
        {
            var returnName = "";
            var splitName = name.Split(' ');

            if (!string.IsNullOrEmpty(splitName[0]))
                returnName = splitName[0][0].ToString();
            if (!string.IsNullOrEmpty(splitName[1]))
                returnName += $"{splitName[1][0]}";

            return returnName;
        }
    }
}
