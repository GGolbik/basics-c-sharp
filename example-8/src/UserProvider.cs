
using System.Text;

namespace ggolbik.csharp;

public class UserProvider
{
    /// <summary>
    /// An x character indicates that encrypted password is stored in /etc/shadow file.
    /// </summary>
    private const string PasswordInShadowFile = "x";

    public IEnumerable<EtcPasswdLine> GetUsers()
    {
        var users = new List<EtcPasswdLine>();
        using (var passwdFile = new FileStream(EtcPasswdLine.EtcPasswdFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var passwdReader = new StreamReader(passwdFile, Encoding.ASCII))
        {
            string? line;
            EtcPasswdLine? passwdLine = null;
            while ((line = passwdReader.ReadLine()) != null)
            {
                if (!EtcPasswdLine.TryCreate(line, out passwdLine) || passwdLine == null)
                {
                    // failed to parse line
                    continue;
                }
                // found user
                users.Add(passwdLine);
            }
        }
        return users;
    }

    public EtcPasswdLine? GetUserByUid(string uid)
    {
        return this.GetUser(uid, isUsername: false);
    }

    public EtcPasswdLine? GetUserByName(string name)
    {
        return this.GetUser(name, isUsername: true);
    }

    private EtcPasswdLine? GetUser(string subject, bool isUsername)
    {
        using (var passwdFile = new FileStream(EtcPasswdLine.EtcPasswdFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var passwdReader = new StreamReader(passwdFile, Encoding.ASCII))
        {
            string? line;
            // read until the user line is found
            EtcPasswdLine? passwdLine = null;
            while ((line = passwdReader.ReadLine()) != null)
            {
                if (!EtcPasswdLine.TryCreate(line, out passwdLine) || passwdLine == null)
                {
                    // failed to parse line
                    continue;
                }
                if (subject != (isUsername ? passwdLine.Username : passwdLine.Uid))
                {
                    // username does not match
                    continue;
                }
                // found user
                return passwdLine;
            }
        }
        return null;
    }

    /// <summary>
    /// Tries to sign in with the credentials.
    /// </summary>
    /// <param name="credentials">The credentials.</param>
    /// <returns>The user info.</returns>
    /// <exception cref="ArgumentNullException">If the user could not be found.</exception>
    /// <exception cref="ArgumentException">If the password is invalid.</exception>
    public EtcPasswdLine SignIn(Credentials credentials)
    {
        var passwdLine = this.GetUserByName(credentials.Username ?? "");
        return this.ValidatePassword(passwdLine, credentials.Password ?? "");
    }

    private EtcPasswdLine ValidatePassword(EtcPasswdLine? passwdLine, string password)
    {
        if (passwdLine == null)
        {
            throw new ArgumentNullException("Unknown user.");
        }
        if (string.IsNullOrEmpty(passwdLine.CryptPassword))
        {
            // An empty field at this point causes you to be able to log in under the user name without entering a password.
            // return to inidicate login success
            return passwdLine;
        }
        EtcShadowLine? shadowLine;
        if (passwdLine.CryptPassword == PasswordInShadowFile)
        {
            this.ReadShadow(passwdLine.Username, out shadowLine);
        }
        else
        {
            shadowLine = new EtcShadowLine(passwdLine.Username, passwdLine.CryptPassword);
        }
        if(ManagedUnixCrypt.Verify(shadowLine?.CryptPassword ?? "", password))
        {
            // valid password
            return passwdLine;
        }
        // unknown user
        throw new ArgumentException("Invalid credentials.");
    }

    private void ReadShadow(string username, out EtcShadowLine? shadowLine)
    {
        using (var shadowFile = new FileStream(EtcShadowLine.EtcShadowFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var shadow = new StreamReader(shadowFile, Encoding.ASCII))
        {
            string? line;
            while ((line = shadow.ReadLine()) != null)
            {
                if (!EtcShadowLine.TryCreate(line, out shadowLine) || shadowLine == null)
                {
                    // invalid line
                    continue;
                }
                if (shadowLine.Username != username)
                {
                    // another user
                    continue;
                }
                // found
                return;
            }
        }
        throw new NotImplementedException("Did not find password entry.");
    }
}