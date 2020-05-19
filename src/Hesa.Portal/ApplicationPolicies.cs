using Hesa.Logic;

namespace Hesa.Portal
{
    /// <summary>
    /// Defines the various application policies.
    /// </summary>
    public static class ApplicationPolicies
    {
        /// <summary>
        /// Indicates that a page requires admin rights. Associated with <see cref="ApplicationRoles.Administrator"/>.
        /// </summary>
        public const string RequiresAdminRights = "RequiresAdmin";
    }
}
