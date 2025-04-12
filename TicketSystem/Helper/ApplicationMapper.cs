using AutoMapper;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.ViewModel;

namespace TicketSystem.Helper
{
    public class ApplicationMapper :Profile

    {
        public ApplicationMapper()
        {
            CreateMap<User, UserVM>();
            CreateMap<User, UserModel>();
            CreateMap<Department, DepartmentVM>().ReverseMap();
            CreateMap<Role, RoleVM>().ReverseMap();
            CreateMap<Category, CategoryVM>().ReverseMap();
            CreateMap<Ticket, TicketVM>().ReverseMap();
            CreateMap<TicketVM, Ticket>()
             .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<TicketFeedBack, TicketFeedBackVM>().ReverseMap();
            CreateMap<TicketFeedbackAssignee,TicketFeedBackAssigneeVM >().ReverseMap();
            CreateMap<TicketAssignment, TicketAssignmentVM>().ReverseMap();
            CreateMap<Notification, NotificationVM>().ReverseMap();

        }

    }
}
