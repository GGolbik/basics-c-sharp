
using System.Text;

namespace ggolbik.csharp;

public class GroupProvider
{
    public IEnumerable<EtcGroupLine> GetGroups()
    {
        var groups = new List<EtcGroupLine>();
        using (var groupFile = new FileStream(EtcGroupLine.EtcGroupFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var groupReader = new StreamReader(groupFile, Encoding.ASCII))
        {
            string? line;
            EtcGroupLine? groupLine = null;
            while ((line = groupReader.ReadLine()) != null)
            {
                if (!EtcGroupLine.TryCreate(line, out groupLine) || groupLine == null)
                {
                    // failed to parse line
                    continue;
                }
                // found group
                groups.Add(groupLine);
            }
        }
        return groups;
    }

    public EtcGroupLine? GetGroupById(string id)
    {
        return this.GetGroup(id, isGroupName: false);
    }

    public EtcGroupLine? GetGroupByName(string name)
    {
        return this.GetGroup(name, isGroupName: true);
    }

    private EtcGroupLine? GetGroup(string subject, bool isGroupName)
    {
        using (var groupFile = new FileStream(EtcGroupLine.EtcGroupFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var groupReader = new StreamReader(groupFile, Encoding.ASCII))
        {
            string? line;
            // read until the user line is found
            EtcGroupLine? groupLine = null;
            while ((line = groupReader.ReadLine()) != null)
            {
                if (!EtcGroupLine.TryCreate(line, out groupLine) || groupLine == null)
                {
                    // failed to parse line
                    continue;
                }
                if (subject != (isGroupName ? groupLine.GroupName : groupLine.GroupId))
                {
                    // group name does not match
                    continue;
                }
                // found group
                return groupLine;
            }
        }
        return null;
    }

}