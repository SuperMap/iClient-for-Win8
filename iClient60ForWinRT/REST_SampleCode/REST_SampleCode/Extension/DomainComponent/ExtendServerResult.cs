namespace serverExtend
{
    public class ExtendServerResult
    {
        public string Extendresult { get; private set; }

        public static ExtendServerResult FromJson(string json)
        {
            ExtendServerResult result = new ExtendServerResult();
            result.Extendresult = json;
            return result;
        }
    }
}
