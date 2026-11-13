namespace Restaurant.Models
{
    /// <summary>
    /// What the app keeps in memory about the signed-in user.
    /// It deliberately carries no password hash.
    /// </summary>
    public sealed record SessionUser(
        int Id,
        int CartId,
        string Username,
        string FirstName,
        string LastName,
        string Email);
}
