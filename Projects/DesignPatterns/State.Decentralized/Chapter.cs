namespace State.Decentralized
{
    public class Chapter
    {
        private string title;
        private string content;

        public Chapter(string title)
        {
            this.title = title;
            this.Content = string.Empty;
        }

        public string Content
        {
            get => content;
            set => content = value;
        }
    }
}