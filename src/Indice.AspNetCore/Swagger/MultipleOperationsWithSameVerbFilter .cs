﻿using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace Indice.AspNetCore.Swagger
{
    /// <summary>
    /// This filter ensures that operation ids generated by swagger take operation parameters into account.
    /// This way multiple overrides using the same http verb don't break the swagger UI
    /// </summary>
    /// <example>
    /// <code>
    ///     public class AccountController : Controller
    ///     {
    ///         [Route("change-password"), HttpPut]
    ///         public async Task ChangePassword([FromBody]ChangePasswordModel request) {
    ///     }
    ///     </code>
    /// Generates the following operation id: ApiAccountChange_passwordPutWithParamRequest
    /// </example>
    /// <see href="https://docs.microsoft.com/en-us/azure/app-service-api/app-service-api-dotnet-swashbuckle-customize"/>
    internal class MultipleOperationsWithSameVerbFilter : IOperationFilter
    {
        public const char SEPARATOR = '_';
        /// <summary>
        /// Changes the operation id to allow the use of multiple methods with the same verb.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context) {
            if (operation.Parameters != null) {
                //Check count of params to assign proper string because Grammar Nazi.
                var plural = operation.Parameters.Count() > 1 ? "s" : "";
                var parameters = string.Join(SEPARATOR + "", operation.Parameters.Select(CapitalizedName));
                operation.OperationId += $"{SEPARATOR}WithParam{plural}{SEPARATOR}{parameters}";
            }
        }

        private string CapitalizedName(IParameter parameter) {
            var paramName = parameter.Name[0].ToString().ToUpper() + new string(parameter.Name.Skip(1).ToArray());
            return paramName;
        }
    }
}
