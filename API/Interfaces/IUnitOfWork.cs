using API.Entities;
using API.DTOs;
using API.Helpers;

namespace API.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IMessageRepository MessageRepository { get; }

    ILikesRepository LikesRepository { get; }
    Task<bool> Complete();
    bool HasChanges();

}
