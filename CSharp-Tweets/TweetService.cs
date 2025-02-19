using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CIS376_Assignment2
{
    public class TweetService
    {
        private readonly HttpClient _httpClient;
        private readonly string _jsonUrl = "https://foyzulhassan.github.io/files/favs.json";
        private List<Tweet> _tweets = new();

        public TweetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Tweet>> LoadTweetsAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(_jsonUrl);
                _tweets = JsonSerializer.Deserialize<List<Tweet>>(response) ?? new List<Tweet>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching JSON: {ex.Message}");
                _tweets = new List<Tweet>();
            }
            return _tweets;
        }

        public async Task<List<Tweet>> GetTweetsAsync()
        {
            return _tweets.Count > 0 ? _tweets : await LoadTweetsAsync();
        }

        public async Task<Tweet?> GetTweetByIdAsync(long id)
        {
            var tweets = await GetTweetsAsync();
            return tweets.FirstOrDefault(t => t.id == id);
        }

        public async Task<User?> GetUserByScreenNameAsync(string screenName)
        {
            var tweets = await GetTweetsAsync();
            var user = tweets.Select(t => t.user).FirstOrDefault(u => u.screen_name.Equals(screenName, StringComparison.OrdinalIgnoreCase));
            return user;
        }

        public async Task<List<GroupedExternalLinks>> GetExternalLinksFromTweets()
        {
            var tweets = await GetTweetsAsync();
            var externalLinks = new List<ExternalLinks>();
            string pattern = @"http:\/\/t\.co\/[A-Za-z0-9]+";
            foreach (var tweet in tweets)
            {
                var urls = Regex.Matches(tweet.text, pattern);

                foreach (Match url in urls)
                {
                    externalLinks.Add(new ExternalLinks
                    {
                        tweet_id = tweet.id,
                        link = url.Value
                    });
                }
            }
            var groupedLinks = externalLinks.GroupBy(link => link.tweet_id)
                    .Select(group => new GroupedExternalLinks
                    {
                        tweet_id = group.Key,
                        links = group.Select(l => l.link).ToList()
                    }).ToList();
            return groupedLinks;
        }
    }
}
