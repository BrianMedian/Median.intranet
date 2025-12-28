using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Median.Intranet.Controllers
{
    [Route("api/profiledata")]
    [ApiController]
    public class ProfileDataController : BaseController
    {
        private readonly IProfileDataRepository profileDataRepository;
        
        public ProfileDataController(IProfileDataRepository profileDataRepository)
        {
            this.profileDataRepository = profileDataRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfileData()
        {
            var profilesResult = await profileDataRepository.GetAllAsync();
            return FromResult(profilesResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfileDataById(Guid id)
        {
            var profileResult = await profileDataRepository.GetByIdAsync(id);
            return FromResult(profileResult);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetProfileDataByUserId(Guid id)
        {
            var profileResult = await profileDataRepository.GetByUserIdAsync(id);
            //find the user email
            var userEmail = User.FindFirst("email")?.Value ?? string.Empty;
            if (!profileResult.Success)
            {
                profileResult = await profileDataRepository.CreateAsync(new ProfileData
                {
                    UserId = id,
                    FullName = string.Empty,
                    Email = userEmail,
                    Position = string.Empty
                });
            }
            return FromResult(profileResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfileData([FromBody] CreateProfileDataRequest profileData)
        {
            ProfileData createProfileData = new ProfileData
            {
                FullName = profileData.FullName,
                Email = string.Empty,
                Position = string.Empty,
                UserId = profileData.UserId
            };
            var createResult = await profileDataRepository.CreateAsync(createProfileData);
            return FromResult(createResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfileData(Guid id, [FromBody] UpdateProfileDataRequest profileData)
        {
            var existingProfileResult = await profileDataRepository.GetByUserIdAsync(id);
            //var existingProfileResult = await profileDataRepository.GetByIdAsync(id);
            if (!existingProfileResult.Success)
            {
                return FromResult(existingProfileResult);
            }
            var existingProfile = existingProfileResult.Value;
            existingProfile.FullName = profileData.FullName;
            existingProfile.Position = profileData.Position;
            existingProfile.PhoneNumber = profileData.PhoneNumber;
            existingProfile.Email = profileData.Email;
            existingProfile.OnlineCardUrl = profileData.OnlineCardUrl;
            existingProfile.ProfilePictureUrl = profileData.ProfilePictureUrl;
            var updateResult = await profileDataRepository.UpdateAsync(existingProfile);
            return FromResult(updateResult);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProfileData(Guid id)
        {
            var deleteResult = await profileDataRepository.DeleteAsync(id);
            return FromResult(deleteResult);
        }
    }

    public class  CreateProfileDataRequest
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
    }

    public class UpdateProfileDataRequest
    {
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Position { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? OnlineCardUrl { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
