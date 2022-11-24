namespace Saitynai.Auth.Model
{
    public static class IsmRoles
    {
        public const string Admin = nameof(Admin);
        public const string IsmUser = nameof(IsmUser);

        public static readonly IReadOnlyCollection<string> All = new[] { Admin, IsmUser };
    }
}
