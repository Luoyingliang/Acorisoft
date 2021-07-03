using System.Threading.Tasks;

namespace Acorisoft.Morisa.Core
{
    public interface IDocumentPropertyManager
    {
        Task UpdatePropertyAsync(ComposeProperty property);
    }
}