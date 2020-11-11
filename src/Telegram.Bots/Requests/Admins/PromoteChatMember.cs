// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

namespace Telegram.Bots.Requests.Admins
{
  public abstract record PromoteChatMember<TChatId> : IRequest<bool>, IChatMemberTargetable<TChatId>
  {
    public TChatId ChatId { get; }

    public int UserId { get; }

    public bool? IsAnonymous { get; init; }

    public bool? CanChangeInfo { get; init; }

    public bool? CanPostMessages { get; init; }

    public bool? CanEditMessages { get; init; }

    public bool? CanDeleteMessages { get; init; }

    public bool? CanInviteUsers { get; init; }

    public bool? CanRestrictMembers { get; init; }

    public bool? CanPinMessages { get; init; }

    public bool? CanPromoteMembers { get; init; }

    public string Method { get; } = "promoteChatMember";

    protected PromoteChatMember(TChatId chatId, int userId)
    {
      ChatId = chatId;
      UserId = userId;
    }
  }

  public sealed record PromoteChatMember : PromoteChatMember<long>
  {
    public PromoteChatMember(long chatId, int userId) : base(chatId, userId) { }
  }

  namespace Usernames
  {
    public sealed record PromoteChatMember : PromoteChatMember<string>
    {
      public PromoteChatMember(string username, int userId) : base(username, userId) { }
    }
  }
}
