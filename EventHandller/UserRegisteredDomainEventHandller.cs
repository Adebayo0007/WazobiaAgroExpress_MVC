using MediatR;

namespace SeaBirdProject.EventHandller 
{
    public class UserRegisteredDomainEventHandller : INotificationHandler<UserRegisteredDomainEvent>
    {
        public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"user using name {notification.name} registered successfully");
            //implementing neccessary thing
            return Task.CompletedTask;
        }
    }
}
