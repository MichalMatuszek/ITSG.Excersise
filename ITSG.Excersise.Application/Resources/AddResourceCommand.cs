using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class AddResourceCommand : IRequest<Unit>
    {
        public string Name { get;}

        public AddResourceCommand(string name)
        {
            Name = name;
        }
    }
}
