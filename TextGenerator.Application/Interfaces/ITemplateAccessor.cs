using System.Runtime.InteropServices;

namespace TextGenerator.Application.Interfaces
{
    public interface ITemplateAccessor
    {
        string GetTemplateForDataModel<TDataModel>() where TDataModel : class;
        string GetTemplateForDataModel(Type modelType);
    }
}
