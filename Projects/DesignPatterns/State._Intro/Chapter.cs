namespace State.Intro
{
    internal class Chapter
    {
        private readonly string title;
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