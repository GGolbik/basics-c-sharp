namespace ggolbik.csharp;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Users:");
        var users = new UserProvider().GetUsers();
        foreach (var user in users)
        {
            Console.WriteLine("\t" + user);
        }

        Console.WriteLine("Groups:");
        var groups = new GroupProvider().GetGroups();
        foreach (var group in groups)
        {
            Console.WriteLine("\t" + group);
        }
    }
}