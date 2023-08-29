namespace Restaurants_REST_API.Services
{
    public static class UserRolesService
    {
        public const string Owner = "OWNER";
        public const string Supervisor = "SUPERVISOR";
        public const string OwnerAndSupervisor = $"{Owner},{Supervisor}";

        public const string Employee = "EMPLOYEE";
        public const string Client = "CLIENT";
        public const string EmployeeAndClient = $"{Employee},{Client}";

    }
}
