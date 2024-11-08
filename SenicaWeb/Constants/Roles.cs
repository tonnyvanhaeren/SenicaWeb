namespace SenicaWeb.Constants;

public static class Roles
{
    public const string ADMIN = "Admin";
    public const string MANAGER = "Manager";
    public const string EDITOR = "Editor";
    public const string USER = "User";

    public const string ALL_ROLES = ADMIN + "," + EDITOR + "," + MANAGER + "," + USER ;
}
