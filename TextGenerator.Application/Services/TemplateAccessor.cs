using TextGenerator.Application.Interfaces;
using TextGenerator.Application.Templates;

namespace TextGenerator.Application.Services
{
    public class TemplateAccessor : ITemplateAccessor
    {
        private readonly IFileReader _reader;

        public TemplateAccessor(IFileReader reader)
        {
            _reader = reader;
        }

        public string GetTemplateForDataModel<TDataModel>() where TDataModel : class
        {
            return GetTemplateForDataModel(typeof(TDataModel));
        }

        public string GetTemplateForDataModel(Type modelType)
        {
            string path = TemplatePaths.GetPathByModelName(modelType.Name);
            return _reader.ReadFile(path);
        }
    }
}
