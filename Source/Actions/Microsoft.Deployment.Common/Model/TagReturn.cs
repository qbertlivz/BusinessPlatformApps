namespace Microsoft.Deployment.Common.Model
{
    public class TagReturn
    {
        public string Tag { get; }
        public object Output { get; }

        public TagReturn(string tag, object output)
        {
            this.Tag = tag;
            this.Output = output;
        }
    }
}