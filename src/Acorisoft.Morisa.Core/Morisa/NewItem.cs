namespace Acorisoft.Morisa
{
    public class NewItem<TInfo> : INewItem<TInfo>
    {
        public NewItem(string name, TInfo info)
        {
            Name = name;
            Info = info;
        }

        protected NewItem()
        {
        }

        /// <summary>
        /// 获取名称。
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 获取或设置额外的信息
        /// </summary>
        public TInfo Info { get; protected set; }
    }


    public class NewDiskItem<TInfo> : NewItem<TInfo>, INewDiskItem<TInfo>
    {
        public NewDiskItem(string directory, string fileName, string name, TInfo info) : base(name, info)
        {
            Directory = directory;
            FileName = fileName;
        }

        protected NewDiskItem()
        {
        }

        /// <summary>
        /// 获取新的内容所在的目录。
        /// </summary>
        public string Directory { get; protected set; }


        /// <summary>
        /// 获取新的内容所在的文件路径。
        /// </summary>
        public string FileName { get; protected set; }
    }
}