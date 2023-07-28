namespace SeaBirdProject.EventHandller
{
    public class UserRegisteredDomainEvent : IDomainEvent
    {
        public string name { get; }

        public UserRegisteredDomainEvent(string name)
        {
            this.name = name;
        }
    }
}
