using LiteDB;

namespace Acorisoft.Morisa
{
    public class MorisaCompose : IMorisaCompose
    {
        private readonly LiteDatabase _database;
        private const int InitSize = 33554432;
        
        //
        // 这是个简单的密码
        private const string SPwd = "p62347.12";

        private MorisaCompose(LiteDatabase database)
        {
            _database = database;
        }
        
        public void BuildHierarchy()
        {
            
        }

        internal ILiteDatabase GetDatabase() => _database;

        public static MorisaCompose Open(string fileName)
        {
            return new MorisaCompose(new LiteDatabase(new ConnectionString
            {
                Filename = fileName,
                InitialSize = InitSize,
            }));
        }
    }
}