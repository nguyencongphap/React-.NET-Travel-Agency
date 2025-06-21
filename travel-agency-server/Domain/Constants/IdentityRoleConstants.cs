namespace Domain.Constants
{
    public static class IdentityRoleConstants
    {
        public static readonly Guid AdminRoleGuid = new("1f728fa9-408f-4a2b-9ad0-de2efdba3faf");
        public static readonly Guid UserRoleGuid = new("1f774166-86c6-405c-9565-4e4cc50eae67");

        // Names of roles
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
