using FastMember;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using TextGenerator.Application.Interfaces;
using TextGenrator.Services;

namespace TextGenerator.Application.Services
{
    public class TemplateFiller : ITemplateFiller
    {
        internal const string PLACEHOLDER_OPENING = "#{{";
        internal const string PLACEHOLDER_CLOSING = "}}";
        private readonly Regex _regex;

        public TemplateFiller()
        {
            _regex = RegexService.GetRegex();
        }

        public string GenerateText<TDataModel>(string template, TDataModel model) where TDataModel : class
        {
            ValidateTemplate(template);
            ValidateDataModel(model);

            var placeholderNames = GetPlaceholderNames(template);

            var values = GetPlaceholderValues(placeholderNames, model);

            return GetText(template, values);
        }

        private void ValidateTemplate(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentNullException(nameof(template), "Template may not be null or an empty string.");

            if (_regex.Matches(template).Count == 0)
                throw new ArgumentException("Template does not include any valid placeholder. Accepted placeholder pattern is: '#{{placeholderName}}'.");
        }

        private void ValidateDataModel<TDataModel>(TDataModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
        }

        private IEnumerable<string> GetPlaceholderNames(string template)
        {
            int startIndex = PLACEHOLDER_OPENING.Length;
            int placeholderLength = PLACEHOLDER_OPENING.Length + PLACEHOLDER_CLOSING.Length;

            return _regex.Matches(template)
                .Select(placeholder => placeholder.Value.Substring(startIndex, placeholder.Value.Length - placeholderLength))
                .ToList();
        }

        private IDictionary<string, string> GetPlaceholderValues<TDataModel>(IEnumerable<string> placeholderNames, TDataModel model)
        {
            Dictionary<string, string> valuesDict = new();

            foreach (var name in placeholderNames)
            {
                valuesDict.Add(name, GetValue(name, model));
            }

            return valuesDict;
        }

        private string GetValue<TDataModel>(string placeholderName, TDataModel model)
        {
            var accessor = TypeAccessor.Create(model.GetType());

            var path = placeholderName.Split('.').Select(s => s.Trim());

            object? value = null;

            if (path.Count() > 0)
            {
                value = accessor[model, path.First()];
            }

            if (value is not null)
            {
                if (value is string s && string.IsNullOrWhiteSpace(s))
                    throw new ArgumentException($"Value of the placeholder '{path.First()}' cannot be empty.");

                if (value is string str)
                    return str;

                if(value is IEnumerable)
                    throw new ArgumentException($"Incorrect property: property '{path.First()}' has incorrect value.");

                if (value.GetType().IsClass)
                    return GetValue(placeholderName.Replace($"{path.First()}.", string.Empty), value);
            }

            throw new ArgumentException($"Incorrect property: property '{path.First()}' has incorrect value.");
        }

        private string GetText(string template, IDictionary<string, string> valuesDict)
        {
            var sb = new StringBuilder(template);

            foreach (var kayValue in valuesDict)
            {
                var placeholder = $"{PLACEHOLDER_OPENING}{kayValue.Key}{PLACEHOLDER_CLOSING}";
                sb.Replace(placeholder, kayValue.Value);
            }

            return sb.ToString();
        }
    }
}
