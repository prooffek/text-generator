using TextGenerator.Application.Interfaces;

namespace TextGenerator.Application.Services
{
    public class TextGeneratorManager : ITextGeneratorManager
    {
        private readonly ITemplateAccessor _templateAccessor;
        private readonly ITemplateFiller _templateFiller;

        public TextGeneratorManager(ITemplateAccessor templateAccessor, ITemplateFiller templateFiller)
        {
            _templateAccessor = templateAccessor;
            _templateFiller = templateFiller;
        }

        public string Handle<TDataModel>(TDataModel model) where TDataModel : class
        {
            string template = _templateAccessor.GetTemplateForDataModel<TDataModel>();
            return _templateFiller.GenerateText(template, model);

        }
    }
}
