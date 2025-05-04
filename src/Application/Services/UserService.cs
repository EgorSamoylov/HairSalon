using Application.DTOs;
using Application.Exceptions;
using Application.Request.ClientRequest;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.ClientRepository;
using Microsoft.Extensions.Logging;


namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private IBCryptHasher hasher;
        private IAttachmentService attachmentService;


        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Add(CreateUserRequest request)
        {
            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Note = request.Note,
                Position = request.Position
            };

            int userId = await _userRepository.Create(user);
            _logger.LogInformation(
                @"User created with id {Id} with FirstName {FirstName}, 
                LastName {LastName},
                with PhoneNumber {PhoneNumber},
                with Email {Email},
                Note {Note},
                Position {Position}",
                userId,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Email,
                user.Note,
                user.Position);
            return userId;
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Attempting to delete user with id {Id}", id);

            var result = await _userRepository.Delete(id);
            if (!result)
            {
                _logger.LogWarning("User with id {Id} not found for deletion", id);
                throw new EntityDeleteException("User for deletion not found");
            }

            _logger.LogInformation("User with id {Id} successfully deleted", id);
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            _logger.LogInformation("Getting all users");

            var users = await _userRepository.ReadAll();
            var mappedUsers = users.Select(q => _mapper.Map<UserDTO>(q));

            _logger.LogInformation("Retrieved {Count} users", mappedUsers.Count());
            return mappedUsers;
        }

        public async Task<UserDTO> GetById(int id)
        {
            _logger.LogInformation("Getting user by id {Id}", id);

            var user = await _userRepository.ReadById(id);
            if (user is null)
            {
                _logger.LogWarning("User with id {Id} not found", id);
                throw new NotFoundApplicationException("User not found");
            }

            var mappedUser = _mapper.Map<UserDTO>(user);

            _logger.LogInformation(
                @"Retrieved client with id {Id}: 
                FirstName {FirstName},
                LastName {LastName},
                PhoneNumber {PhoneNumber},
                Email {Email},
                Note {Note},
                Position {Position}",
                id,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Email,
                user.Note,
                user.Position);

            return mappedUser;
        }

        public async Task Update(UpdateUserRequest request)
        {
            _logger.LogInformation(
                @"Attempting to update client with id {Id}: 
                FirstName {FirstName},
                LastName {LastName},
                PhoneNumber {PhoneNumber},
                Email {Email},
                Note {Note},
                Position {Position}",
                request.UserId,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Email,
                request.Note,
                request.Position);

            var user = new User()
            {
                Id = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Note = request.Note,
                Position = request.Position
            };

            var result = await _userRepository.Update(user);
            if (!result)
            {
                _logger.LogWarning("User with id {Id} not found for update", request.UserId);
                throw new EntityUpdateException("Client wasn't updated");
            }

            _logger.LogInformation("User with id {Id} successfully updated", request.UserId);
        }
    }
}
