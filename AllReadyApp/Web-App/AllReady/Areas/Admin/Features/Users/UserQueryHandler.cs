﻿using System.Linq;
using System.Threading.Tasks;
using AllReady.Areas.Admin.ViewModels.Site;
using AllReady.Models;
using AllReady.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllReady.Areas.Admin.Features.Users
{
    public class UserQueryHandler : IAsyncRequestHandler<UserQuery, EditUserViewModel>
    {
        private readonly AllReadyContext _context;

        public UserQueryHandler(AllReadyContext context)
        {
            _context = context;
        }

        public async Task<EditUserViewModel> Handle(UserQuery message)
        {
            var user = await _context.Users
                .Where(u => u.Id == message.UserId)
                .Include(u => u.AssociatedSkills).ThenInclude(us => us.Skill)
                .Include(u => u.Claims)
                .SingleOrDefaultAsync();

            EditUserViewModel viewModel = null;

            if (user != null)
            {
                var organizationId = user.GetOrganizationId();

                viewModel = new EditUserViewModel
                {
                    UserId = message.UserId,
                    UserName = user.UserName,
                    AssociatedSkills = user.AssociatedSkills,
                    IsOrganizationAdmin = user.IsUserType(UserType.OrgAdmin),
                    IsSiteAdmin = user.IsUserType(UserType.SiteAdmin),
                    Organization =
                        organizationId != null
                            ? await _context.Organizations.FirstAsync(t => t.Id == organizationId.Value)
                            : null
                };
            }

            return viewModel;
        }
    }
}
