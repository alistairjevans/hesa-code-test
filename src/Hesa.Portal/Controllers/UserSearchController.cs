using Hesa.Logic;
using Hesa.Portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hesa.Portal.Controllers
{
    /// <summary>
    /// Provides the API endpoint for performing a user search.
    /// </summary>
    [ApiController]
    [Route("api/{controller}")]
    [Authorize(Policy = ApplicationPolicies.RequiresAdminRights)]
    public class UserSearchController : ControllerBase
    {
        private const int MaxUserResults = 50;

        private readonly IUserSearch _userSearchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSearchController"/> class.
        /// </summary>
        /// <param name="userSearchService">The user search service.</param>
        public UserSearchController(IUserSearch userSearchService)
        {
            _userSearchService = userSearchService;
        }

        /// <summary>
        /// Searches users by last name, and returns a JSON array of results.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>A JSON set.</returns>
        [HttpGet("searchByLastName")]
        public IActionResult SearchByLastName(string lastName)
        {
            // No last name is a bad request.
            if (lastName is null)
            {
                return BadRequest();
            }

            // Invoke the service, and map directly to our output model.
            return Ok(_userSearchService.QueryUsersByLastName(lastName, MaxUserResults)
                                        .Map(user => new UserSearchResult
                                        {
                                            UserId = user.Id,
                                            UserName = user.UserName,
                                            EmailAddress = user.Email,
                                            FirstName = user.FirstName,
                                            LastName = user.LastName,
                                        }));
        }
    }
}