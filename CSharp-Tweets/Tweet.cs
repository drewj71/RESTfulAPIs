namespace CIS376_Assignment2
{
    public class Tweet
    {
        public string created_at { get; set; }
        public long id { get; set; }
        public string text { get; set; }
        public List<ExternalLinks> links { get; set; }
        public User user {  get; set; }
    }

    public class ExternalLinks
    {
        public int tweet_id { get; set; }
        public string link { get; set; }
    }
}
