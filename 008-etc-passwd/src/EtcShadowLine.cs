
namespace ggolbik.csharp;

/// <summary>
/// Represents a line of /etc/shadow
/// </summary>
public class EtcShadowLine
{
    /// <summary>
    /// The /etc/shadow is a text-based password file. The shadow file stores the hashed passphrase (or "hash") format for Linux user account with additional properties related to the user password.
    /// </summary>
    public const string EtcShadowFile = "/etc/shadow";

    /// <summary>
    /// A valid account name, which exist on the system.
    /// </summary>
    private const int IndexUsername = 0;
    /// <summary>
    /// Your encrypted password is in hash format.
    /// The password should be minimum 15-20 characters long including special characters, digits, lower case alphabetic and more.
    /// Usually password format is set to $id$salt$hashed,
    /// </summary>
    private const int IndexPassword = 1;
    /// <summary>
    /// The date of the last password change, expressed as the number of days since Jan 1, 1970 (Unix time). The value 0 has a special meaning, which is that the user should change her password the next time she will log in the system. An empty field means that password aging features are disabled.
    /// </summary>
    private const int IndexLastChanged = 2;
    /// <summary>
    /// The minimum number of days required between password changes i.e. the number of days left before the user is allowed to change her password again. An empty field and value 0 mean that there are no minimum password age.
    /// </summary>
    private const int IndexMinimum = 3;
    /// <summary>
    /// The maximum number of days the password is valid, after that user is forced to change her password again.
    /// </summary>
    private const int IndexMaximum = 4;
    /// <summary>
    /// The number of days before password is to expire that user is warned that his/her password must be changed.
    /// </summary>
    private const int IndexWarn = 5;
    /// <summary>
    /// The number of days after password expires that account is disabled.
    /// </summary>
    private const int IndexInactive = 6;
    /// <summary>
    /// The date of expiration of the account, expressed as the number of days since Jan 1, 1970.
    /// </summary>
    private const int IndexExpire = 7;

    public string Username { get; set; }
    public string CryptPassword { get; set; }
    public string? Lastchanged { get; set; }
    public string? Minimum { get; set; }
    public string? Maximum { get; set; }
    public string? Warn { get; set; }
    public string? Inactive { get; set; }
    public string? Expire { get; set; }

    public EtcShadowLine(string username, string cryptPassword)
    {
        this.Username = username;
        this.CryptPassword = cryptPassword;
    }

    public static bool TryCreate(string? line, out EtcShadowLine? user)
    {
        user = null;
        if (String.IsNullOrWhiteSpace(line))
        {
            return false;
        }
        var elements = line.Split(":");
        if (elements.Length > IndexPassword)
        {
            var username = elements[IndexUsername];
            var password = elements[IndexPassword];
            user = new EtcShadowLine(username, password);
            if (elements.Length > IndexLastChanged)
            {
                user.Lastchanged = elements[IndexLastChanged];
            }
            if (elements.Length > IndexMinimum)
            {
                user.Minimum = elements[IndexMinimum];
            }
            if (elements.Length > IndexMaximum)
            {
                user.Maximum = elements[IndexMaximum];
            }
            if (elements.Length > IndexWarn)
            {
                user.Warn = elements[IndexWarn];
            }
            if (elements.Length > IndexInactive)
            {
                user.Inactive = elements[IndexInactive];
            }
            if (elements.Length > IndexExpire)
            {
                user.Expire = elements[IndexExpire];
            }
            return true;
        }
        return false;
    }
}