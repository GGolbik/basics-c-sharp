
namespace ggolbik.csharp;

/// <summary>
/// Represents a line of /etc/group
/// </summary>
public class EtcGroupLine
{
    /// <summary>
    /// The /etc/group is a text file which defines the groups to which users belong under Linux and UNIX operating system.
    /// </summary>
    public const string EtcGroupFile = "/etc/group";

    /// <summary>
    /// It is the name of group. If you run ls -l command, you will see this name printed in the group field.
    /// </summary>
    private const int IndexGroupName = 0;

    /// <summary>
    /// Generally password is not used, hence it is empty/blank. It can store encrypted password. This is useful to implement privileged groups.
    /// </summary>
    private const int IndexPassword = 1;

    /// <summary>
    /// Each user must be assigned a group ID. You can see this number in your /etc/passwd file.
    /// </summary>
    private const int IndexGroupId = 2;
    
    /// <summary>
    /// It is a list of user names of users who are members of the group. The user names, must be separated by commas.
    /// </summary>
    private const int IndexGroupList = 3;

    public string GroupName { get; set; }
    public string Password { get; set; }
    public string GroupId { get; set; }
    public IList<string> Members { get; set; } = new List<string>();

    public EtcGroupLine(string groupName, string groupId, string password = "")
    {
        this.GroupName = groupName;
        this.GroupId = groupId;
        this.Password = password;
    }

    public static bool TryCreate(string? line, out EtcGroupLine? group)
    {
        group = null;
        if (String.IsNullOrWhiteSpace(line))
        {
            return false;
        }
        var elements = line.Split(":");
        if (elements.Length > IndexGroupId)
        {
            var groupName = elements[IndexGroupName];
            var password = elements[IndexPassword];
            var groupId = elements[IndexGroupId];
            group = new EtcGroupLine(groupName, groupId, password);
            if (elements.Length > IndexGroupList)
            {
                var members = elements[IndexGroupList].Split(",");
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

    public override string ToString()
    {
        var result = new List<string>();
        var properties = this.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(this);
            var valueStr = value?.ToString();
            var list = value as IEnumerable<string>;
            if(list != null && property.Name == nameof(EtcGroupLine.Members))
            {
                valueStr = "[";
                valueStr += string.Join(", ", list);
                valueStr += "]";
            }
            result.Add(property.Name + "=" + valueStr);
        }
        return string.Join(" ; ", result);
    }
}