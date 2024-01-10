namespace TextGenerator.Application.Interfaces
{
    public interface ITemplateFiller
    {
        string GenerateText<TDataModel>(string template, TDataModel model) where TDataModel : class;
    }
}
