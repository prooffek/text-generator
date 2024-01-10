namespace TextGenerator.Application.Interfaces
{
    public interface ITextGeneratorManager
    {
        string Handle<TDataModel>(TDataModel model) where TDataModel : class;
    }
}
