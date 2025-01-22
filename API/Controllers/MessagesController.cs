using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class MessagesController(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : BaseApiController
{

    [HttpPost]
    public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO messageDTO)
    {
        var username = User.GetUsername();

        if (username.ToLower() == messageDTO.RecipientUsername.ToLower())
            return BadRequest("Can not  message  yourself");

        var sender = await unitOfWork.UserRepository.GetUserByUsername(username);
        var recipient = await unitOfWork.UserRepository.GetUserByUsername(messageDTO.RecipientUsername);

        if (sender == null || recipient == null || sender.UserName == null || recipient.UserName == null)
            return BadRequest("Can not send message at this time");

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = messageDTO.Content

        };

        unitOfWork.MessageRepository.AddMessage(message);

        if (await unitOfWork.Complete()) return Ok(mapper.Map<MessageDTO>(message));
        return BadRequest("Failed to save message");


    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUsername();

        var messages = await unitOfWork.MessageRepository.GetMessagesForUser(messageParams);

        Response.AddPaginationHeader(messages);

        return messages;
    }

    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesThread(string username)
    {
        var currentUsername = User.GetUsername();

        return Ok(await unitOfWork.MessageRepository.GetMessageThread(currentUsername, username));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var currentUsername = User.GetUsername();

        var message = await unitOfWork.MessageRepository.GetMessage(id);

        if (message == null) return BadRequest(" Can not delete message !");

        if (message.SenderUsername != currentUsername &&
            message.RecipientUsername != currentUsername
        ) return Forbid();

        if (message.SenderUsername == currentUsername) message.SenderDeleted = true;
        if (message.RecipientUsername == currentUsername) message.RecipientDeleted = true;

        if (message is { SenderDeleted: true, RecipientDeleted: true })
        {
            unitOfWork.MessageRepository.DeleteMessage(message);
        }

        if (await unitOfWork.Complete()) return Ok();
        return BadRequest("Problem Delelting the message");

    }


}
