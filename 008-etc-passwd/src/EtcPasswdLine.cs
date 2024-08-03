
using System.Text;

namespace ggolbik.csharp;

/// <summary>
/// Represents a line of /etc/passwd
/// </summary>
public class EtcPasswdLine
{
    /// <summary>
    /// /etc/passwd is only used for local users.
    /// </summary>
    public const string EtcPasswdFile = "/etc/passwd";

    /// <summary>
    /// It is used when user logs in. It should be between 1 and 32 characters in length
    /// </summary>
    private const int IndexUsername = 0;
    /// <summary>
    /// An x character indicates that encrypted password is stored in /etc/shadow file. Please note that you need to use the passwd command to computes the hash of a password typed at the CLI or to store/update the hash of the password in /etc/shadow file.
    /// </summary>
    private const int IndexPassword = 1;
    /// <summary>
    /// Each user must be assigned a user ID (UID). UID 0 (zero) is reserved for root and UIDs 1-99 are reserved for other predefined accounts. Further UID 100-999 are reserved by system for administrative and system accounts/groups.
    /// </summary>
    private const int IndexUid = 2;
    /// <summary>
    /// The primary group ID (stored in /etc/group file)
    /// </summary>
    private const int IndexGid = 3;
    /// <summary>
    ///  The comment field. It allow you to add extra information about the users such as user's full name, phone number etc. This field use by finger command.
    /// </summary>
    private const int IndexComment = 4;
    /// <summary>
    /// The absolute path to the directory the user will be in when they log in.
    /// If this directory does not exists then users directory becomes /
    /// </summary>
    private const int IndexHome = 5;
    /// <summary>
    /// The absolute path of a command or shell (/bin/bash). Typically, this is a shell. Please note that it does not have to be a shell. For example, sysadmin can use the nologin shell, which acts as a replacement shell for the user accounts. If shell set to /sbin/nologin and the user tries to log in to the Linux system directly, the /sbin/nologin shell closes the connection.
    /// </summary>
    private const int IndexCommand = 6;

    public string Username { get; set; } = "";
    public string CryptPassword { get; set; } = "";
    public string Uid { get; set; } = "";
    public string Gid { get; set; } = "";
    public string? Comment { get; set; }
    public string? Home { get; set; }
    public string? Command { get; set; }

    public static bool TryCreate(string? line, out EtcPasswdLine? user)
    {
        user = null;
        if (String.IsNullOrWhiteSpace(line))
        {
            return false;
        }
        var elements = line.Split(":");
        if (elements.Length > IndexCommand)
        {
            user = new EtcPasswdLine();
            user.Username = elements[IndexUsername];
            user.CryptPassword = elements[IndexPassword];
            user.Uid = elements[IndexUid];
            user.Gid = elements[IndexGid];
            if (elements.Length > IndexComment)
            {
                user.Comment = elements[IndexComment];
            }
            if (elements.Length > IndexHome)
            {
                user.Home = elements[IndexHome];
            }
            if (elements.Length > IndexCommand)
            {
                user.Command = elements[IndexCommand];
            }
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        var result = new List<string>();
        var properties = this.GetType().GetProperties();
        foreach (var property in properties)
        {
            result.Add(property.Name + "=" + property.GetValue(this));
        }
        return string.Join(" ; ", result);
    }
}

