
namespace ggolbik.csharp;

/// <summary>
/// Represents a line of /etc/gshadow
/// https://manpages.ubuntu.com/manpages/xenial/en/man5/gshadow.5.html
/// </summary>
public class EtcGshadowLine
{
    /// <summary>
    /// Group passwords are stored in the files /etc/group and /etc/gshadow.
    /// </summary>
    public const string EtcGshadowFile = "/etc/gshadow";

    /// <summary>
    /// It is the name of group. If you run ls -l command, you will see this name printed in the group field.
    /// </summary>
    private const int IndexGroupName = 0;

    /// <summary>
    /// If the password field contains some string that is not a valid result of crypt(3), for instance ! or *, users will not be able to use a unix password to access the group (but group members do not need the password).
    /// </summary>
    private const int IndexPassword = 1;

    /// <summary>
    /// It must be a comma-separated list of user names.
    /// Administrators can change the password or the members of the group.
    /// Administrators also have the same permissions as the members (see below).
    /// </summary>
    private const int IndexAdministrators = 2;
    
    /// <summary>
    /// It must be a comma-separated list of user names.
    /// </summary>
    private const int IndexMembers = 3;

    public string GroupName { get; set; }
    public string Password { get; set; }
    public IList<string> Administrators { get; set; } = new List<string>();
    public IList<string> Members { get; set; } = new List<string>();

    public EtcGshadowLine(string groupName, string password = "")
    {
        this.GroupName = groupName;
        this.Password = password;
    }

    public static bool TryCreate(string? line, out EtcGshadowLine? group)
    {
        group = null;
        if (String.IsNullOrWhiteSpace(line))
        {
            return false;
        }
        var elements = line.Split(":");
        if (elements.Length > IndexPassword)
        {
            var groupName = elements[IndexGroupName];
            var password = elements[IndexPassword];
            group = new EtcGshadowLine(groupName, password);
            if (elements.Length > IndexAdministrators)
            {
                var admins = elements[IndexAdministrators].Split(",");
                foreach(var admin in admins)
                {
                    if(!String.IsNullOrWhiteSpace(admin))
                    {
                        group.Administrators.Add(admin);
                    }
                }
            }
            if (elements.Length > IndexMembers)
            {
                var members = elements[IndexMembers].Split(",");
                foreach(var member in members)
                {
                    if(!String.IsNullOrWhiteSpace(member))
                    {
                        group.Members.Add(member);
                    }
                }
            }
            return true;
        }
        return false;
    }
}