using TextGenerator.Application.Models;

namespace TextGenerator.Application.Templates
{
    internal static class TemplatePaths
    {
        private const string DIRECTORY = "Templates";

        internal static string GetPathByModelName(string modelName) => modelName switch
        {
            nameof(InvitationEmail) => Path.Combine(DIRECTORY, "InvitationEmailTemplate.txt"),
            _ => throw new ArgumentException($"No template assigned to model '{modelName}'")
        };
    }
}
