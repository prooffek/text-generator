using System.Text.RegularExpressions;
using TextGenerator.Application.Services;

namespace TextGenrator.Services
{
    internal partial class RegexService
    {
        private const string PATTERN = $"{TemplateFiller.PLACEHOLDER_OPENING}[ a-zA-Z0-9.]*{TemplateFiller.PLACEHOLDER_CLOSING}";

        [GeneratedRegex(PATTERN, RegexOptions.Compiled)]
        internal static partial Regex GetRegex();
    }
}
