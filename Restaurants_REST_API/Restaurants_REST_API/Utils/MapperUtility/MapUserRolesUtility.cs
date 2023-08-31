namespace Restaurants_REST_API.Utils.MapperService
{
    public class MapUserRolesUtility
    {

        private readonly IConfiguration _config;
        private readonly string _ownerRole;
        private readonly string _supervisorRole;
        private readonly string _employeeRole;
        private readonly string _clientRole;

        public MapUserRolesUtility(IConfiguration config)
        {
            _config = config;

            _ownerRole = _config["ApplicationSettings:UserRoles:Owner"];
            _supervisorRole = _config["ApplicationSettings:UserRoles:Supervisor"];
            _employeeRole = _config["ApplicationSettings:UserRoles:Employee"];
            _clientRole = _config["ApplicationSettings:UserRoles:Client"];

            try
            {
                if (string.IsNullOrEmpty(_ownerRole))
                {
                    throw new Exception("Owner role is empty");
                }

                if (string.IsNullOrEmpty(_supervisorRole))
                {
                    throw new Exception("Supervisor role is empty");
                }

                if (string.IsNullOrEmpty(_employeeRole))
                {
                    throw new Exception("Employee role is empty");
                }

                if (string.IsNullOrEmpty(_clientRole))
                {
                    throw new Exception("Client role is empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Method maps min employee type id to user role.
        /// </summary>
        /// <param name="typesId">List of employee types id.</param>
        /// <returns>String of user role.</returns>
        public string GetUserRoleBasedOnEmployeeTypesId(IEnumerable<int>? typesId)
        {
            if (typesId == null || typesId.Count() == 0)
            {
                return _employeeRole;
            }

            int minRole = typesId.Min();

            if (minRole == 1)
            {
                return _ownerRole;
            }
            else if (minRole == 2)
            {
                return _supervisorRole;
            }
            else
            {
                return _employeeRole;
            }
        }

        /// <summary>
        /// Returns name of client role.
        /// </summary>
        /// <returns>String of user role.</returns>
        public string GetClientUserRole()
        {
            return _clientRole;
        }
    }
}
