namespace FlowNetworkToolKit.Core.Utils.Loader
{
    public class AlgorithmInfo
    {
        public string Name { private set; get; }
        public string Description { private set; get; }
        public string Url { private set; get; }
        public string ClassName { private set; get; }
        public int Index;
        public dynamic Instance { private set; get; }

        public AlgorithmInfo(string name, string description, string url, string className, dynamic instance)
        {
            Name = name;
            Description = description;
            Url = url;
            ClassName = className;
            Index = -1;
            Instance = instance;
        }
    }
}
