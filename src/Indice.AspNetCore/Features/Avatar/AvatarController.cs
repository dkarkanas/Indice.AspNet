﻿using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Indice.Extensions;
using Indice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Indice.AspNetCore.Features
{
    /// <summary>
    /// Creates an avatar based on a given name (first and last name) plus parameters.
    /// </summary>
    [Route("avatar")]
    [ApiExplorerSettings(IgnoreApi = true)]
    internal class AvatarController : ControllerBase
    {
        /// <summary>
        /// <see cref="AvatarController"/> constructor.
        /// </summary>
        public AvatarController(AvatarGenerator avatarGenerator) {
            AvatarGenerator = avatarGenerator ?? throw new ArgumentNullException(nameof(avatarGenerator));
        }

        public AvatarGenerator AvatarGenerator { get; }
        /// <summary>
        /// Creates an avatar using random background color based on fullname and optional size.
        /// </summary>
        /// <param name="fullname">The fullname to use when creating the avatar.</param>
        /// <param name="ext">The desired size of the avatar. Possible values are 16, 24, 32, 48, 64, 128, 192, 256 and 512.</param>
        /// <param name="foreground">The foreground color to use.</param>
        /// <param name="circular">Determines whether the tile will be circular or sqare. Defaults to false (sqare)</param>
        /// <returns></returns>
        [HttpGet("{fullname}.{ext?}"), ResponseCache(Duration = 345600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "foreground", "circular", "v" })]
        [AllowAnonymous]
        public IActionResult GetAvatar([FromRoute] string fullname, [FromRoute] string ext, [FromQuery] string foreground, [FromQuery] bool circular) => GetAvatar(fullname, size:null, background:null, ext, foreground: null, circular);

        /// <summary>
        /// Creates an avatar using random background color based on fullname and optional size.
        /// </summary>
        /// <param name="fullname">The fullname to use when creating the avatar.</param>
        /// <param name="size">The desired size of the avatar. Possible values are 16, 24, 32, 48, 64, 128, 192, 256 and 512.</param>
        /// <param name="foreground">The foreground color to use.</param>
        /// <param name="circular">Determines whether the tile will be circular or sqare. Defaults to false (sqare)</param>
        /// <returns></returns>
        [HttpGet("{fullname}/{size?}"), ResponseCache(Duration = 345600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "foreground", "circular", "v" })]
        [AllowAnonymous]
        public IActionResult GetAvatar([FromRoute] string fullname, [FromRoute] int? size, [FromQuery] string foreground, [FromQuery] bool circular) => GetAvatar(fullname, size, background:null, ext:null, foreground: null, circular);

        /// <summary>
        /// Creates an avatar using random background color based on fullname and optional size and extension.
        /// </summary>
        /// <param name="fullname">The fullname to use when creating the avatar.</param>
        /// <param name="size">The desired size of the avatar. Possible values are 16, 24, 32, 48, 64, 128, 192, 256 and 512.</param>
        /// <param name="ext">The file extension (.png or .jpg).</param>
        /// <param name="foreground">The foreground color to use.</param>
        /// <param name="circular">Determines whether the tile will be circular or sqare. Defaults to false (sqare)</param>
        /// <returns></returns>
        [HttpGet("{fullname}/{size}.{ext?}"), ResponseCache(Duration = 345600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "foreground", "circular", "v" })]
        [AllowAnonymous]
        public IActionResult GetAvatar([FromRoute] string fullname, [FromRoute] int? size, [FromRoute] string ext, [FromQuery] string foreground, [FromQuery] bool circular) => GetAvatar(fullname, size, background: null, ext, foreground: null, circular);

        /// <summary>
        /// Creates an avatar using fullname, size, background color and optional extension.
        /// </summary>
        /// <param name="fullname">The fullname to use when creating the avatar.</param>
        /// <param name="size">The desired size of the avatar. Possible values are 16, 24, 32, 48, 64, 128, 192, 256 and 512.</param>
        /// <param name="background">The background color to use.</param>
        /// <param name="ext">The file extension (.png or .jpg).</param>
        /// <param name="foreground">The foreground color to use.</param>
        /// <param name="circular">Determines whether the tile will be circular or sqare. Defaults to false (sqare)</param>
        /// <returns></returns>
        [HttpGet("{fullname}/{size}/{background}.{ext?}"), ResponseCache(Duration = 345600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "foreground", "circular", "v" })]
        [AllowAnonymous]
        public IActionResult GetAvatar([FromRoute] string fullname, [FromRoute] int? size, [FromRoute] string background, [FromRoute] string ext, [FromQuery] string foreground, [FromQuery] bool circular) {
            if (string.IsNullOrWhiteSpace(fullname)) {
                return BadRequest();
            }
            var parts = fullname.Trim().Split(' ');
            var firstName = parts[0];
            var lastName = fullname.Trim().Remove(0, firstName.Length).TrimStart();
            if (parts.Length == 1) {
                if (int.TryParse(firstName, out var number)) {
                    firstName = number.ToString();
                    lastName = null;
                } else { 
                    parts = Regex.Split(firstName, @"(?<=\p{L})(?=\p{Lu}\p{Ll})|(?<=[\p{Ll}0-9])(?=[0-9]?\p{Lu})");
                    firstName = parts[0];
                    lastName = fullname.Trim().Remove(0, firstName.Length).TrimStart();
                }
            }
            return GetAvatar(firstName, lastName, size, ext, background, foreground, circular, null);
        }

        /// <summary>
        /// Creates an avatar based on first and last name plus parameters
        /// </summary>
        /// <param name="firstName">First name to use.</param>
        /// <param name="lastName">Last name to use.</param>
        /// <param name="size">The desired size of the avatar. Possible values are 16, 24, 32, 48, 64, 128, 192, 256 and 512.</param>
        /// <param name="ext">The file extension (.png or .jpg).</param>
        /// <param name="background">The background color to use.</param>
        /// <param name="foreground">The foreground color to use.</param>
        /// <param name="circular">Determines whether the tile will be circular or sqare. Defaults to false (sqare)</param>
        /// <param name="v">Use for cache busting.</param>
        /// <returns></returns>
        [HttpGet, ResponseCache(Duration = 345600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "firstname", "lastname", "size", "ext", "background", "foreground", "circular", "v" })]
        [AllowAnonymous]
        public IActionResult GetAvatar([FromQuery] string firstName, [FromQuery] string lastName, [FromQuery] int? size, [FromQuery] string ext, [FromQuery] string background, [FromQuery] string foreground, [FromQuery] bool circular, [FromQuery] string v) {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName)) {
                ModelState.AddModelError(nameof(firstName), "Must provide with first or last name.");
                return BadRequest(ModelState);
            }
            if (size.HasValue && !AvatarGenerator.AllowedSizes.Contains((int)size)) {
                ModelState.AddModelError(nameof(size), $"Size is out of range. Valid sizes are {string.Join(", ", AvatarGenerator.AllowedSizes.OrderBy(x => x))}.");
                return BadRequest(ModelState);
            }
            if (!string.IsNullOrEmpty(ext) && !new[] { "png", "jpg" }.Contains(ext)) {
                ModelState.AddModelError(nameof(size), "Extension is out of range of valid values. Accepts only .png and .jpg.");
                return BadRequest(ModelState);
            }
            var data = new MemoryStream();
            AvatarGenerator.Generate(data, firstName, lastName, size ?? 192, ext == "jpg", background, foreground, circular);
            var hash = string.Empty;
            using (var md5 = MD5.Create()) {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes($"{firstName} {lastName}")).ToBase64UrlSafe();
            }
            //var filename = $"{firstName}_{lastName}_{size ?? 192}.{ext}";
            return File(data, ext == "jpg" ? "image/jpeg" : "image/png", DateTime.UtcNow, new EntityTagHeaderValue($"\"{hash}\""));
        }
    }
}
