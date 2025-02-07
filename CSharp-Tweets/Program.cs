using CIS376_Assignment2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<TweetService>();

var app = builder.Build();

app.MapGet("/", () => "Twitter API Online");

app.MapGet("/tweets", async (TweetService service) =>
{
    var tweets = await service.GetTweetsAsync();
    return Results.Ok(tweets);
});

app.MapGet("/tweets/{id}", async (long id, TweetService service) =>
{
    var tweet = await service.GetTweetByIdAsync(id);
    return tweet is not null ? Results.Ok(tweet) : Results.NotFound();
});

app.MapGet("/users/{screen_name}", async (string screen_name, TweetService service) =>
{
    var user = await service.GetUserByScreenNameAsync(screen_name);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
