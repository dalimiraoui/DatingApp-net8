
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

public class MessageHub(
    IMessageRepository messageRepository,
    IUserRepository userRepository,
    IMapper mapper
) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUser = httpContext?.Request.Query["user"];

        if (Context.User == null || string.IsNullOrEmpty(otherUser)) 
         throw new Exception("Can not join group");

        var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var messages = await messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser!);

        await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(CreateMessageDTO messageDTO)
    {
        var username = Context.User?.GetUsername() ?? throw new Exception("Could not get user");

        if (username.ToLower() == messageDTO.RecipientUsername.ToLower())
        throw new HubException("You can not message yourself");

        var sender =  await userRepository.GetUserByUsername(username);
        var recipient =  await userRepository.GetUserByUsername(messageDTO.RecipientUsername);

        if (sender== null || recipient== null || sender.UserName == null || recipient.UserName== null)
            throw new HubException("Can not send message at this time");

        var message = new Message
        {
            Sender =sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = messageDTO.Content

        };

        messageRepository.AddMessage(message);

        if (await messageRepository.SaveAllAsync()) 
        {
            var group = GetGroupName(sender.UserName, recipient.UserName);
            await Clients.Group(group).SendAsync("NewMessage", mapper.Map<MessageDTO>(message));
        }
    }

    private string GetGroupName(string caller, string? other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0 ;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";

    }

}
