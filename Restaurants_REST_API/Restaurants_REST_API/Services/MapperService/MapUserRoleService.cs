namespace Restaurants_REST_API.Services.MapperService
{
    public class MapUserRoleService
    {

        private readonly IConfiguration _config;
        private readonly string _ownerRole;
        private readonly string _supervisorRole;
        private readonly string _employeeRole;
        private readonly string _clientRole;

        public MapUserRoleService(IConfiguration config)
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
        /// This method selects min employee type id and maps to user role.
        /// </summary>
        /// <param name="typesId">List of employee types id</param>
        /// <returns>String - user role</returns>
        public string GetUserRoleBasesOnEmpployeeTypesId(IEnumerable<int>? typesId)
        {
            if (typesId == null)
            {
                return _clientRole;
            }

            int maxRole = typesId.Min();

            if (maxRole == 1)
            {
                return _ownerRole;
            }
            else if (maxRole == 2)
            {
                return _supervisorRole;
            }
            else
            {
                return _employeeRole;
            }
        }
    }
}
