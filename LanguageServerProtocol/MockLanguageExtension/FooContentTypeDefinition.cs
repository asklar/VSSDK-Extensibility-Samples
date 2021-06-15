using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Utilities;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace MockLanguageExtension
{
    public class FooContentDefinition
    {
        [Export]
        [Name("MIDL")]
        [BaseDefinition(CodeRemoteContentDefinition.CodeRemoteContentTypeName)]
        internal static ContentTypeDefinition FooContentTypeDefinition;


        [Export]
        [FileExtension(".idl")]
        [ContentType("MIDL")]
        internal static FileExtensionToContentTypeDefinition FooFileExtensionDefinition;
    }


    public class MidlContentType : IContentType
    {
        public string TypeName => "MIDL";

        public string DisplayName => "Modern MIDL";

        public IEnumerable<IContentType> BaseTypes => new IContentType[] { };

        public bool IsOfType(string type)
        {
            System.Diagnostics.Debug.WriteLine($"IsOfType {type}");
            return type == "C/C++" || type == "any";
        }
    }
    [Export(typeof(IFilePathToContentTypeProvider))]
    [Name("MIDL")]
    [Order(Before = "C/C++")]
    [FileExtension(".idl")]
    public class MidlContentTypeProvider : IFilePathToContentTypeProvider
    {
        public bool TryGetContentTypeForFilePath(string filePath, out IContentType contentType)
        {
            contentType = null;
            if (filePath.EndsWith(".idl"))
            {
                contentType = new MidlContentType();
                return true;
            }
            return false;
        }
    }
}
