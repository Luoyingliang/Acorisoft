using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.Morisa.IO;
using Acorisoft.Morisa.Resources;

namespace Acorisoft.Morisa.Core
{
    public interface IDocumentFileManager
    {
        Stream OpenImage(ImageResource resource);

        Task<Guid> UploadImageAsync(string sourceFileName);
    }
}