﻿using AllReady.Areas.Admin.ViewModels.Invite;
using MediatR;

namespace AllReady.Areas.Admin.Features.Invite
{
    public class CreateCampaignManagerInviteCommand : IAsyncRequest
    {
        public InviteViewModel Invite { get; set; }
        public string UserId { get; set; }
    }
}
